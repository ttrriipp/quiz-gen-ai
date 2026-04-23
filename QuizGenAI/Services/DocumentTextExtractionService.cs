using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace QuizGenAI.Services
{
    public class DocumentTextExtractionService
    {
        private const int MaxExtractedCharacters = 6000;
        private const int MaxFileSizeBytes = 10 * 1024 * 1024;
        private const int MaxPdfProcessingCharacters = 2 * 1024 * 1024;
        private const int MaxDistinctLines = 140;

        public string ExtractText(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new InvalidOperationException("The selected document could not be found.");
            }

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length > MaxFileSizeBytes)
            {
                throw new InvalidOperationException("The selected file is too large. Please attach a PDF/DOCX file smaller than 10 MB.");
            }

            var extension = (Path.GetExtension(filePath) ?? string.Empty).ToLowerInvariant();
            var header = ReadFileHeader(filePath, 8);

            if (extension.Equals(".docx", StringComparison.OrdinalIgnoreCase))
            {
                if (!LooksLikeZip(header))
                {
                    throw new InvalidOperationException("This file is not a valid DOCX document. Please upload a real .docx file (not .doc).");
                }

                return PrepareForPrompt(ExtractDocxText(filePath));
            }

            if (extension.Equals(".doc", StringComparison.OrdinalIgnoreCase))
            {
                return PrepareForPrompt(ExtractDocText(filePath));
            }

            if (extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                if (!LooksLikePdf(header))
                {
                    throw new InvalidOperationException("This file does not appear to be a valid PDF.");
                }

                return PrepareForPrompt(ExtractPdfText(filePath));
            }

            throw new InvalidOperationException("Only PDF, DOCX, and DOC files are supported.");
        }

        private static string ExtractDocxText(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    var documentEntry = archive.GetEntry("word/document.xml");
                    if (documentEntry == null)
                    {
                        throw new InvalidOperationException("The DOCX file does not contain readable document content.");
                    }

                    using (var entryStream = documentEntry.Open())
                    using (var reader = new StreamReader(entryStream))
                    {
                        var xml = XDocument.Parse(reader.ReadToEnd());
                        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

                        var lines = xml
                            .Descendants(w + "p")
                            .Select(p =>
                                string.Join(string.Empty, p.Descendants(w + "t").Select(t => (string)t)))
                            .Where(text => !string.IsNullOrWhiteSpace(text))
                            .ToList();

                        var result = string.Join(Environment.NewLine, lines);
                        if (string.IsNullOrWhiteSpace(result))
                        {
                            throw new InvalidOperationException("No readable text was found in the DOCX file.");
                        }

                        return result;
                    }
                }
            }
            catch (InvalidDataException)
            {
                throw new InvalidOperationException("This file is not a valid DOCX package. Please re-save it as .docx and try again.");
            }
        }

        private static string ExtractPdfText(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var content = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
            if (content.Length > MaxPdfProcessingCharacters)
            {
                content = content.Substring(0, MaxPdfProcessingCharacters);
            }

            var textChunks = new List<string>();
            var searchIndex = 0;
            while (searchIndex < content.Length)
            {
                var streamIndex = content.IndexOf("stream", searchIndex, StringComparison.Ordinal);
                if (streamIndex < 0)
                {
                    break;
                }

                var dataStart = streamIndex + "stream".Length;
                if (dataStart < content.Length && (content[dataStart] == '\r' || content[dataStart] == '\n'))
                {
                    if (content[dataStart] == '\r') dataStart++;
                    if (dataStart < content.Length && content[dataStart] == '\n') dataStart++;
                }

                var endIndex = content.IndexOf("endstream", dataStart, StringComparison.Ordinal);
                if (endIndex < 0)
                {
                    break;
                }

                var streamData = content.Substring(dataStart, endIndex - dataStart);
                var decoded = TryDecodeFlateStream(streamData);
                ExtractPdfTextOperators(decoded, textChunks);
                searchIndex = endIndex + "endstream".Length;
            }

            if (textChunks.Count == 0)
            {
                // Fallback for simple, non-compressed PDFs.
                ExtractPdfTextOperators(content, textChunks);
            }

            if (textChunks.Count == 0)
            {
                // Last-resort fallback for tricky PDFs without expensive regex.
                var printable = ExtractPrintableSegments(content);
                if (!string.IsNullOrWhiteSpace(printable))
                {
                    textChunks.Add(printable);
                }
            }

            var result = string.Join(" ", textChunks)
                .Replace("\\r", " ")
                .Replace("\\n", " ");

            result = CollapseWhitespace(result);
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException("No readable text was found in the PDF file.");
            }

            return result;
        }

        private static string ExtractDocText(string filePath)
        {
            object wordApplication = null;
            object documents = null;
            object document = null;

            try
            {
                var wordType = Type.GetTypeFromProgID("Word.Application");
                if (wordType == null)
                {
                    throw new InvalidOperationException("Reading .doc files requires Microsoft Word to be installed on this machine.");
                }

                wordApplication = Activator.CreateInstance(wordType);
                SetProperty(wordApplication, "Visible", false);
                SetProperty(wordApplication, "DisplayAlerts", 0); // wdAlertsNone

                documents = wordType.InvokeMember("Documents", BindingFlags.GetProperty, null, wordApplication, null);

                var openArgs = new object[]
                {
                    filePath, // FileName
                    false,    // ConfirmConversions
                    true,     // ReadOnly
                    false,    // AddToRecentFiles
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, // Revert
                    false,    // WritePasswordDocument
                    false,    // WritePasswordTemplate
                    false,    // Format
                    false,    // Encoding
                    false     // Visible
                };

                document = documents.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, documents, openArgs);
                var content = document.GetType().InvokeMember("Content", BindingFlags.GetProperty, null, document, null);
                var text = content.GetType().InvokeMember("Text", BindingFlags.GetProperty, null, content, null) as string;

                ReleaseCom(content);

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException("No readable text was found in the DOC file.");
                }

                return text;
            }
            catch (COMException)
            {
                throw new InvalidOperationException("Unable to read .doc file. Please install Microsoft Word or convert the file to .docx.");
            }
            finally
            {
                CloseDocument(document);
                QuitWord(wordApplication);
                ReleaseCom(document);
                ReleaseCom(documents);
                ReleaseCom(wordApplication);
            }
        }

        private static string TryDecodeFlateStream(string encodedStream)
        {
            try
            {
                var rawBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(encodedStream);
                using (var source = new MemoryStream(rawBytes))
                using (var output = new MemoryStream())
                using (var deflate = new DeflateStream(source, CompressionMode.Decompress))
                {
                    deflate.CopyTo(output);
                    return Encoding.GetEncoding("ISO-8859-1").GetString(output.ToArray());
                }
            }
            catch
            {
                return encodedStream;
            }
        }

        private static void ExtractPdfTextOperators(string streamContent, List<string> chunks)
        {
            if (string.IsNullOrWhiteSpace(streamContent))
            {
                return;
            }

            var index = 0;
            while (index < streamContent.Length)
            {
                if (streamContent[index] != '(')
                {
                    index++;
                    continue;
                }

                var end = FindLiteralEnd(streamContent, index + 1);
                if (end < 0)
                {
                    break;
                }

                var literal = streamContent.Substring(index + 1, end - index - 1);
                var after = end + 1;
                while (after < streamContent.Length && char.IsWhiteSpace(streamContent[after]))
                {
                    after++;
                }

                var isTextOperator =
                    (after + 1 < streamContent.Length && streamContent[after] == 'T' && streamContent[after + 1] == 'j') ||
                    (after < streamContent.Length && streamContent[after] == ']');

                if (isTextOperator)
                {
                    var value = DecodePdfTextLiteral(literal);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        chunks.Add(value);
                    }
                }

                index = end + 1;
            }
        }

        private static string DecodePdfTextLiteral(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value
                .Replace("\\(", "(")
                .Replace("\\)", ")")
                .Replace("\\\\", "\\")
                .Replace("\\n", " ")
                .Replace("\\r", " ")
                .Replace("\\t", " ");
        }

        private static string PrepareForPrompt(string text)
        {
            var normalized = CollapseWhitespace((text ?? string.Empty).Trim());
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return string.Empty;
            }

            // Reduce repeated lines/slides to keep prompt compact and faster for AI.
            var uniqueLines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var selectedLines = new List<string>();
            var lines = normalized
                .Split(new[] { '.', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CollapseWhitespace(x))
                .Where(x => x.Length >= 20)
                .ToList();

            foreach (var line in lines)
            {
                if (uniqueLines.Add(line))
                {
                    selectedLines.Add(line);
                }

                if (selectedLines.Count >= MaxDistinctLines)
                {
                    break;
                }
            }

            var compact = string.Join(". ", selectedLines).Trim();
            if (compact.Length <= MaxExtractedCharacters)
            {
                return compact;
            }

            return compact.Substring(0, MaxExtractedCharacters);
        }

        private static string ExtractPrintableSegments(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(input.Length);
            var segmentLength = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                var isPrintable = (ch >= 32 && ch <= 126) || ch == '\r' || ch == '\n' || ch == '\t';
                if (isPrintable)
                {
                    builder.Append(ch);
                    segmentLength++;
                }
                else
                {
                    if (segmentLength >= 24)
                    {
                        builder.Append(' ');
                    }

                    segmentLength = 0;
                }
            }

            return builder.ToString();
        }

        private static string CollapseWhitespace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(input.Length);
            var lastWasSpace = false;

            for (var i = 0; i < input.Length; i++)
            {
                var ch = input[i];
                if (char.IsWhiteSpace(ch))
                {
                    if (!lastWasSpace)
                    {
                        builder.Append(' ');
                        lastWasSpace = true;
                    }
                }
                else
                {
                    builder.Append(ch);
                    lastWasSpace = false;
                }
            }

            return builder.ToString().Trim();
        }

        private static int FindLiteralEnd(string content, int startIndex)
        {
            var escaped = false;
            for (var i = startIndex; i < content.Length; i++)
            {
                var ch = content[i];
                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (ch == '\\')
                {
                    escaped = true;
                    continue;
                }

                if (ch == ')')
                {
                    return i;
                }
            }

            return -1;
        }

        private static byte[] ReadFileHeader(string filePath, int byteCount)
        {
            var buffer = new byte[byteCount];
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var read = stream.Read(buffer, 0, buffer.Length);
                if (read < buffer.Length)
                {
                    Array.Resize(ref buffer, read);
                }
            }

            return buffer;
        }

        private static bool LooksLikePdf(byte[] header)
        {
            return header != null &&
                   header.Length >= 4 &&
                   header[0] == 0x25 &&
                   header[1] == 0x50 &&
                   header[2] == 0x44 &&
                   header[3] == 0x46;
        }

        private static bool LooksLikeZip(byte[] header)
        {
            return header != null &&
                   header.Length >= 4 &&
                   header[0] == 0x50 &&
                   header[1] == 0x4B &&
                   (header[2] == 0x03 || header[2] == 0x05 || header[2] == 0x07) &&
                   (header[3] == 0x04 || header[3] == 0x06 || header[3] == 0x08);
        }

        private static void SetProperty(object target, string propertyName, object value)
        {
            if (target == null)
            {
                return;
            }

            target.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, target, new[] { value });
        }

        private static void CloseDocument(object document)
        {
            if (document == null)
            {
                return;
            }

            try
            {
                document.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, document, new object[] { false });
            }
            catch
            {
                // Ignore close failures in cleanup path.
            }
        }

        private static void QuitWord(object wordApplication)
        {
            if (wordApplication == null)
            {
                return;
            }

            try
            {
                wordApplication.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, wordApplication, null);
            }
            catch
            {
                // Ignore quit failures in cleanup path.
            }
        }

        private static void ReleaseCom(object comObject)
        {
            if (comObject == null)
            {
                return;
            }

            try
            {
                if (Marshal.IsComObject(comObject))
                {
                    Marshal.FinalReleaseComObject(comObject);
                }
            }
            catch
            {
                // Ignore release failures in cleanup path.
            }
        }
    }
}
