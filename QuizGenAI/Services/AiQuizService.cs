using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;

namespace QuizGenAI.Services
{
    public class AiQuizService
    {
        public async Task<AiQuizGenerationResultDto> GenerateQuizAsync(AiQuizRequestDto request)
        {
            ValidateRequest(request);

            var prompt = BuildPrompt(request);
            var baseUrl = AiApiConfiguration.GetBaseUrl();
            var apiKey = AiApiConfiguration.GetApiKey();

            LoggingService.Information(
                "AI quiz generation started. Subject={Subject} Topic={Topic} Difficulty={Difficulty} QuestionCount={QuestionCount} SourceDocument={SourceDocument}",
                request.SubjectName,
                request.Topic,
                request.Difficulty,
                request.QuestionCount,
                request.SourceDocumentFileName ?? "none");

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                LoggingService.Warning("AI quiz generation is using the fallback generator because no AI API base URL is configured.");
                return BuildFallbackResult(request, prompt);
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                LoggingService.Warning("AI quiz generation is using the fallback generator because no AI API key is configured.");
                return BuildFallbackResult(request, prompt);
            }

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    responseMimeType = "application/json"
                }
            };

            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey.Trim());
                }

                var json = JsonConvert.SerializeObject(payload);
                const int maxAttempts = 3;
                for (var attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                    using (var response = await client.PostAsync(baseUrl.Trim(), content).ConfigureAwait(false))
                    {
                        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent))
                        {
                            var normalizedQuestions = ParseQuestions(responseContent);
                            LoggingService.Information("AI quiz generation completed successfully. Provider={Provider} QuestionCount={QuestionCount}", "Configured AI API", normalizedQuestions.Count);
                            return BuildResultFromQuestions(request, prompt, responseContent, normalizedQuestions, "Configured AI API", "external");
                        }

                        var isTransient = response.StatusCode == HttpStatusCode.ServiceUnavailable ||
                                          (int)response.StatusCode == 429 ||
                                          response.StatusCode == HttpStatusCode.BadGateway ||
                                          response.StatusCode == HttpStatusCode.GatewayTimeout;

                        if (isTransient && attempt < maxAttempts)
                        {
                            LoggingService.Warning(
                                "AI quiz generation transient failure (Attempt {Attempt}/{MaxAttempts}) StatusCode={StatusCode} Details={Details}. Retrying.",
                                attempt,
                                maxAttempts,
                                response.StatusCode,
                                AiApiConfiguration.GetSafeResponseDetail(responseContent));
                            await Task.Delay(attempt * 1200).ConfigureAwait(false);
                            continue;
                        }

                        var details = AiApiConfiguration.GetSafeResponseDetail(responseContent);
                        var statusCode = (int)response.StatusCode;
                        var hasLeakedKeySignal = !string.IsNullOrWhiteSpace(responseContent) &&
                            responseContent.IndexOf("leaked", StringComparison.OrdinalIgnoreCase) >= 0;

                        if (isTransient)
                        {
                            LoggingService.Warning(
                                "AI service remained unavailable after retries. Falling back to local generator. StatusCode={StatusCode} Details={Details}",
                                response.StatusCode,
                                details);
                            return BuildFallbackResult(request, prompt);
                        }

                        if (statusCode == 401 || statusCode == 403 || hasLeakedKeySignal)
                        {
                            LoggingService.Warning(
                                "AI request rejected (StatusCode={StatusCode}). Using fallback generator. Details={Details}",
                                response.StatusCode,
                                details);
                            return BuildFallbackResult(request, prompt);
                        }

                        LoggingService.Warning("AI quiz generation failed with status code {StatusCode}.", response.StatusCode);
                        throw new InvalidOperationException(
                            string.Format("AI generation failed with status code {0}.{1}{1}{2}",
                                response.StatusCode,
                                Environment.NewLine,
                                details));
                    }
                }
            }

            LoggingService.Warning("AI quiz generation exhausted retries unexpectedly; using fallback generator.");
            return BuildFallbackResult(request, prompt);
        }

        public string BuildPrompt(AiQuizRequestDto request)
        {
            var prompt = string.Format(
                "Generate {0} multiple-choice quiz questions for subject '{1}' about topic '{2}'. " +
                "Difficulty: {3}. Return strict JSON only as an array. " +
                "Each item must contain questionText, choices, correctAnswer, and explanation. " +
                "Each question must have exactly 4 choices and exactly 1 correct answer.",
                request.QuestionCount,
                request.SubjectName,
                string.IsNullOrWhiteSpace(request.Topic) ? "General Coverage" : request.Topic.Trim(),
                request.Difficulty);

            if (!string.IsNullOrWhiteSpace(request.SourceDocumentText))
            {
                prompt += string.Format(
                    " Use the attached source document '{0}' as the primary reference. " +
                    "Prioritize factual details from this content when building questions.{1}{1}Source content:{1}{2}",
                    request.SourceDocumentFileName ?? "uploaded-file",
                    Environment.NewLine,
                    request.SourceDocumentText.Trim());
            }

            return prompt;
        }

        private static void ValidateRequest(AiQuizRequestDto request)
        {
            if (request == null)
            {
                throw new InvalidOperationException("AI request data is required.");
            }

            if (request.SubjectId <= 0 || string.IsNullOrWhiteSpace(request.SubjectName))
            {
                throw new InvalidOperationException("Subject is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Topic) && string.IsNullOrWhiteSpace(request.SourceDocumentText))
            {
                throw new InvalidOperationException("Topic is required unless a document is attached.");
            }

            if (!Enum.IsDefined(typeof(QuizDifficulty), request.Difficulty))
            {
                throw new InvalidOperationException("Difficulty is required.");
            }

            if (request.QuestionCount <= 0)
            {
                throw new InvalidOperationException("Question count must be greater than zero.");
            }
        }

        private AiQuizGenerationResultDto BuildFallbackResult(AiQuizRequestDto request, string prompt)
        {
            var topic = string.IsNullOrWhiteSpace(request.Topic) ? "the attached document topic" : request.Topic.Trim();
            var fallbackQuestions = new List<AiQuizQuestionDto>();
            for (var i = 1; i <= request.QuestionCount; i++)
            {
                var correct = string.Format("{0} concept {1}", topic, i);
                fallbackQuestions.Add(new AiQuizQuestionDto
                {
                    QuestionText = string.Format("Which option best describes {0} for {1}?", topic, request.SubjectName),
                    CorrectAnswer = correct,
                    Explanation = string.Format("This fallback explanation highlights a core point about {0} at {1} difficulty.", topic, request.Difficulty),
                    Choices = new List<string>
                    {
                        correct,
                        string.Format("Common misconception about {0}", topic),
                        string.Format("Unrelated idea from {0}", request.SubjectName),
                        string.Format("Incorrect detail for item {0}", i)
                    }
                });
            }

            var rawJson = JsonConvert.SerializeObject(fallbackQuestions, Formatting.Indented);
            var result = BuildResultFromQuestions(request, prompt, rawJson, fallbackQuestions, "Demo Fallback", "local");
            result.UsedFallback = true;
            return result;
        }

        private static AiQuizGenerationResultDto BuildResultFromQuestions(
            AiQuizRequestDto request,
            string prompt,
            string rawResponse,
            List<AiQuizQuestionDto> normalizedQuestions,
            string provider,
            string modelName)
        {
            if (normalizedQuestions == null || normalizedQuestions.Count == 0)
            {
                throw new InvalidOperationException("The AI response did not contain any questions.");
            }

            var quiz = new QuizEditorDto
            {
                Title = string.Format("{0} - {1}", request.SubjectName, request.Topic.Trim()),
                SubjectId = request.SubjectId,
                Topic = request.Topic.Trim(),
                Difficulty = request.Difficulty,
                DurationMinutes = request.DurationMinutes <= 0 ? Math.Max(10, request.QuestionCount * 2) : request.DurationMinutes,
                Status = QuizStatus.Draft,
                IsAiGenerated = true,
                AiGenerationPrompt = prompt,
                AiGenerationRawResponseJson = rawResponse,
                AiGenerationProvider = provider,
                AiGenerationModelName = modelName
            };

            foreach (var question in normalizedQuestions)
            {
                var normalizedChoices = question.Choices
                    .Select(StripChoicePrefix)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Take(4)
                    .ToList();

                if (normalizedChoices.Count < 4)
                {
                    throw new InvalidOperationException("Each AI-generated question must contain four choices.");
                }

                var correctChoice = ResolveCorrectChoice(normalizedChoices, StripChoicePrefix(question.CorrectAnswer));

                if (string.IsNullOrWhiteSpace(correctChoice))
                {
                    throw new InvalidOperationException("The AI response contains a correct answer that does not match its choices.");
                }

                var quizQuestion = new QuizQuestionEditorDto
                {
                    Text = question.QuestionText.Trim(),
                    Explanation = string.IsNullOrWhiteSpace(question.Explanation) ? null : question.Explanation.Trim()
                };

                foreach (var choice in normalizedChoices)
                {
                    quizQuestion.Choices.Add(new QuizChoiceEditorDto
                    {
                        Text = choice.Trim(),
                        IsCorrect = string.Equals(choice.Trim(), correctChoice.Trim(), StringComparison.OrdinalIgnoreCase)
                    });
                }

                quiz.Questions.Add(quizQuestion);
            }

            return new AiQuizGenerationResultDto
            {
                Quiz = quiz,
                Prompt = prompt,
                RawResponseJson = rawResponse,
                Provider = provider,
                ModelName = modelName
            };
        }

        private static string ResolveCorrectChoice(List<string> normalizedChoices, string rawCorrectAnswer)
        {
            var answer = (rawCorrectAnswer ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(answer))
            {
                return null;
            }

            var directMatch = normalizedChoices.FirstOrDefault(x =>
                string.Equals(x.Trim(), answer, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(directMatch))
            {
                return directMatch;
            }

            // Support AI answers like "B", "2", "Option C", "(d)", etc.
            var token = RemoveWordIgnoreCase(RemoveWordIgnoreCase(RemoveWordIgnoreCase(answer, "option"), "choice"), "answer")
                .Replace(":", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Trim();

            if (token.Length == 1)
            {
                var letter = char.ToUpperInvariant(token[0]);
                if (letter >= 'A' && letter <= 'D')
                {
                    var index = letter - 'A';
                    if (index >= 0 && index < normalizedChoices.Count)
                    {
                        return normalizedChoices[index];
                    }
                }
            }

            int numericIndex;
            if (int.TryParse(token, out numericIndex))
            {
                if (numericIndex >= 1 && numericIndex <= normalizedChoices.Count)
                {
                    return normalizedChoices[numericIndex - 1];
                }
            }

            // Partial text fallback if AI returns decorated answer text.
            var partialMatch = normalizedChoices.FirstOrDefault(x =>
                answer.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(partialMatch))
            {
                return partialMatch;
            }

            return null;
        }

        private static string StripChoicePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return Regex.Replace(value.Trim(), @"^(?:choice|option|answer)?\s*[\(\[]?(?:[A-Da-d]|[1-4])[\)\].:-]\s*", string.Empty).Trim();
        }

        private static string RemoveWordIgnoreCase(string input, string value)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(value))
            {
                return input ?? string.Empty;
            }

            var result = input;
            var index = result.IndexOf(value, StringComparison.OrdinalIgnoreCase);
            while (index >= 0)
            {
                result = result.Remove(index, value.Length);
                index = result.IndexOf(value, StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }

        private static List<AiQuizQuestionDto> ParseQuestions(string content)
        {
            var parsedToken = JToken.Parse(content);
            var questionArray = ResolveQuestionArray(parsedToken);
            if (questionArray == null)
            {
                throw new InvalidOperationException("The AI response could not be parsed into quiz questions.");
            }

            var questions = questionArray.ToObject<List<AiQuizQuestionDto>>();
            if (questions == null || questions.Count == 0)
            {
                throw new InvalidOperationException("The AI response returned an empty question list.");
            }

            return questions;
        }

        private static JArray ResolveQuestionArray(JToken token)
        {
            if (token == null)
            {
                return null;
            }

            if (token.Type == JTokenType.Array)
            {
                return (JArray)token;
            }

            var objectToken = token as JObject;
            if (objectToken == null)
            {
                return null;
            }

            var directQuestions = objectToken["questions"] as JArray;
            if (directQuestions != null)
            {
                return directQuestions;
            }

            var contentToken = objectToken.SelectToken("choices[0].message.content");
            if (contentToken != null && !string.IsNullOrWhiteSpace(contentToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(contentToken.ToString()));
            }

            var geminiTextToken = objectToken.SelectToken("candidates[0].content.parts[0].text");
            if (geminiTextToken != null && !string.IsNullOrWhiteSpace(geminiTextToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(geminiTextToken.ToString()));
            }

            var textOutputToken = objectToken.SelectToken("output[0].content[0].text");
            if (textOutputToken != null && !string.IsNullOrWhiteSpace(textOutputToken.ToString()))
            {
                return ResolveQuestionArray(JToken.Parse(textOutputToken.ToString()));
            }

            return null;
        }
    }
}
