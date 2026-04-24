using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class AiQuizGeneratorForm : Form
    {
        private const long MaxSourceDocumentBytes = 10L * 1024L * 1024L;
        private readonly AiQuizService _aiQuizService;
        private readonly QuizService _quizService;
        private readonly DocumentTextExtractionService _documentTextExtractionService;
        private ComboBox _cmbSubject;
        private ComboBox _cmbDifficulty;
        private NumericUpDown _nudQuestionCount;
        private NumericUpDown _nudDuration;
        private TextBox _txtTopic;
        private Label _lblDocumentStatus;
        private Label _lblNote;
        private Button _btnGenerate;
        private ProgressBar _prgGenerateLoading;
        private Button _btnAttachDocument;
        private Button _btnClearDocument;
        private ProgressBar _prgDocumentLoading;
        private ToolTip _documentStatusToolTip;
        private string _selectedDocumentPath;
        private string _selectedDocumentText;

        public AiQuizGeneratorForm()
        {
            InitializeComponent();
            if (DesignTimeHelper.IsInDesignMode(this)) return;
            _aiQuizService = new AiQuizService();
            _quizService = new QuizService();
            _documentTextExtractionService = new DocumentTextExtractionService();
            _documentStatusToolTip = new ToolTip();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
        }

        public AiQuizGenerationResultDto GeneratedResult { get; private set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadLookups();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F);
            AutoScaleMode = AutoScaleMode.Dpi;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New AI Quiz";

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = Color.White,
                Padding = new Padding(20, 18, 20, 14)
            };

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "Generate MCQ drafts, review them in the editor, and save only after teacher review."
            });

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "AI Quiz Generator"
            });

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 16, 18, 18)
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(22)
            };

            var cardLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 7,
                Margin = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (var i = 0; i < 6; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            }
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _cmbSubject = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                IntegralHeight = false,
                DropDownHeight = 240,
                Margin = new Padding(0, 6, 0, 6)
            };
            _txtTopic = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 6, 0, 6)
            };
            _cmbDifficulty = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 6, 0, 6)
            };
            _nudQuestionCount = CreateNumericInput(1, 20, 5);
            _nudDuration = CreateNumericInput(1, 180, 15);

            foreach (QuizDifficulty difficulty in Enum.GetValues(typeof(QuizDifficulty)))
            {
                _cmbDifficulty.Items.Add(difficulty);
            }
            _cmbDifficulty.SelectedIndex = 0;

            _lblNote = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(430, 0),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Margin = new Padding(0, 8, 0, 0),
                Text = string.Format("Attach PDF, DOCX, or DOC files (max {0}) to generate questions from source material. If no AI endpoint is configured, the app uses a demo fallback generator.", FormatFileSize(MaxSourceDocumentBytes))
            };

            _lblDocumentStatus = new Label
            {
                AutoSize = false,
                Width = 440,
                Height = 34,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Margin = new Padding(0, 0, 0, 0),
                Text = "No document attached.",
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            layout.Controls.Add(CreateFieldLabel("Subject"), 0, 0);
            layout.Controls.Add(_cmbSubject, 1, 0);
            layout.Controls.Add(CreateFieldLabel("Topic"), 0, 1);
            layout.Controls.Add(_txtTopic, 1, 1);
            layout.Controls.Add(CreateFieldLabel("Difficulty"), 0, 2);
            layout.Controls.Add(_cmbDifficulty, 1, 2);
            _nudQuestionCount.Dock = DockStyle.Left;
            _nudDuration.Dock = DockStyle.Left;

            layout.Controls.Add(CreateFieldLabel("Question Count"), 0, 3);
            layout.Controls.Add(_nudQuestionCount, 1, 3);
            layout.Controls.Add(CreateFieldLabel("Duration (minutes)"), 0, 4);
            layout.Controls.Add(_nudDuration, 1, 4);
            layout.Controls.Add(CreateFieldLabel("Source Document"), 0, 5);
            layout.Controls.Add(CreateDocumentPicker(), 1, 5);
            layout.Controls.Add(CreateFieldLabel("Notes"), 0, 6);
            layout.Controls.Add(_lblNote, 1, 6);

            var buttonPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Padding(0, 18, 0, 0),
                WrapContents = false
            };

            _btnGenerate = CreatePrimaryButton("Generate");
            _btnGenerate.Width = 120;
            _btnGenerate.Margin = new Padding(0, 0, 8, 0);
            _btnGenerate.Click += async delegate { await GenerateAsync(); };

            _prgGenerateLoading = new ProgressBar
            {
                Width = 110,
                Height = 16,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 24,
                Visible = false,
                Margin = new Padding(0, 9, 8, 0)
            };

            var btnCancel = CreateSecondaryButton("Cancel");
            btnCancel.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };

            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(_btnGenerate);
            buttonPanel.Controls.Add(_prgGenerateLoading);

            cardLayout.Controls.Add(layout, 0, 0);
            cardLayout.Controls.Add(buttonPanel, 0, 1);
            card.Controls.Add(cardLayout);
            body.Controls.Add(card);
            Controls.Add(body);
            Controls.Add(header);
            AcceptButton = _btnGenerate;
            CancelButton = btnCancel;
            ResumeLayout();
        }

        private void LoadLookups()
        {
            if (_cmbSubject.Items.Count > 0)
            {
                return;
            }

            foreach (var subject in _quizService.GetSubjects())
            {
                _cmbSubject.Items.Add(subject);
            }

            if (_cmbSubject.Items.Count > 0)
            {
                _cmbSubject.SelectedIndex = 0;
            }
            else
            {
                NotificationHelper.ShowError(this, "No Subjects Found", "Please create at least one subject before generating an AI quiz.");
            }
        }

        private async Task GenerateAsync()
        {
            SetGenerateLoading(true);

            try
            {
                var subject = _cmbSubject.SelectedItem as SubjectOptionDto;
                if (subject == null)
                {
                    throw new InvalidOperationException("Select a subject.");
                }

                var request = new AiQuizRequestDto
                {
                    SubjectId = subject.Id,
                    SubjectName = subject.Name,
                    Topic = _txtTopic.Text,
                    SourceDocumentFileName = string.IsNullOrWhiteSpace(_selectedDocumentPath) ? null : Path.GetFileName(_selectedDocumentPath),
                    SourceDocumentText = _selectedDocumentText,
                    Difficulty = _cmbDifficulty.SelectedItem is QuizDifficulty ? (QuizDifficulty)_cmbDifficulty.SelectedItem : QuizDifficulty.Easy,
                    QuestionCount = Convert.ToInt32(_nudQuestionCount.Value),
                    DurationMinutes = Convert.ToInt32(_nudDuration.Value)
                };

                GeneratedResult = await _aiQuizService.GenerateQuizAsync(request);
                NotificationHelper.ShowSuccess(
                    this,
                    GeneratedResult.UsedFallback ? "AI Draft Ready (Fallback)" : "AI Draft Ready",
                    GeneratedResult.UsedFallback
                        ? "The demo fallback generated a draft for review."
                        : "The AI generated a draft quiz for review.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "AI quiz generation failed in the UI flow.");
                NotificationHelper.ShowError(this, "AI Generation Failed", ex.Message);
            }
            finally
            {
                SetGenerateLoading(false);
            }
        }

        private static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Margin = new Padding(0, 6, 12, 6),
                Text = text
            };
        }

        private Control CreateDocumentPicker()
        {
            var holder = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 6, 0, 6)
            };

            _btnAttachDocument = CreateSecondaryButton("Attach File");
            _btnAttachDocument.Width = 110;
            _btnAttachDocument.Margin = new Padding(0, 0, 8, 0);
            _btnAttachDocument.Click += async delegate { await AttachSourceDocumentAsync(); };

            _btnClearDocument = CreateSecondaryButton("Clear");
            _btnClearDocument.Width = 80;
            _btnClearDocument.Click += delegate
            {
                _selectedDocumentPath = null;
                _selectedDocumentText = null;
                _lblDocumentStatus.Text = "No document attached.";
                _documentStatusToolTip.SetToolTip(_lblDocumentStatus, _lblDocumentStatus.Text);
            };

            _prgDocumentLoading = new ProgressBar
            {
                Width = 96,
                Height = 16,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 24,
                Visible = false,
                Margin = new Padding(0, 8, 10, 0)
            };

            holder.Controls.Add(_btnAttachDocument);
            holder.Controls.Add(_btnClearDocument);
            holder.Controls.Add(_prgDocumentLoading);
            holder.Controls.Add(_lblDocumentStatus);
            return holder;
        }

        private async Task AttachSourceDocumentAsync()
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                dialog.Filter = "Supported Documents (*.pdf;*.docx;*.doc)|*.pdf;*.docx;*.doc";
                    dialog.Multiselect = false;
                    dialog.Title = "Select source document";

                    if (dialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    SetDocumentLoading(true);
                    var selectedFile = dialog.FileName;
                    var fileInfo = new FileInfo(selectedFile);
                    if (fileInfo.Length > MaxSourceDocumentBytes)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "The selected file is {0}. Maximum allowed size is {1}.",
                                FormatFileSize(fileInfo.Length),
                                FormatFileSize(MaxSourceDocumentBytes)));
                    }

                    var extracted = await Task.Run(() => _documentTextExtractionService.ExtractText(selectedFile));
                    _selectedDocumentPath = dialog.FileName;
                    _selectedDocumentText = extracted;
                    _lblDocumentStatus.Text = string.Format("Attached: {0} ({1} chars)", Path.GetFileName(dialog.FileName), extracted.Length);
                    _documentStatusToolTip.SetToolTip(_lblDocumentStatus, _lblDocumentStatus.Text);
                }
            }
            catch (Exception ex)
            {
                _selectedDocumentPath = null;
                _selectedDocumentText = null;
                _lblDocumentStatus.Text = "No document attached.";
                _documentStatusToolTip.SetToolTip(_lblDocumentStatus, _lblDocumentStatus.Text);
                LoggingService.Error(ex, "Failed to attach source document.");
                MessageBox.Show(
                    this,
                    "Unable to attach and read this file. Try a smaller or text-based PDF/DOCX/DOC file.\n\n" + ex.Message,
                    "Attach Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                SetDocumentLoading(false);
            }
        }

        private void SetDocumentLoading(bool isLoading)
        {
            if (_prgDocumentLoading != null)
            {
                _prgDocumentLoading.Visible = isLoading;
            }

            if (_btnAttachDocument != null)
            {
                _btnAttachDocument.Enabled = !isLoading;
            }

            if (_btnClearDocument != null)
            {
                _btnClearDocument.Enabled = !isLoading;
            }
        }

        private void SetGenerateLoading(bool isLoading)
        {
            if (_btnGenerate != null)
            {
                _btnGenerate.Enabled = !isLoading;
            }

            if (_prgGenerateLoading != null)
            {
                _prgGenerateLoading.Visible = isLoading;
            }
        }

        private static NumericUpDown CreateNumericInput(decimal minimum, decimal maximum, decimal value)
        {
            return new NumericUpDown
            {
                Minimum = minimum,
                Maximum = maximum,
                Value = value,
                Width = 110,
                Margin = new Padding(0)
            };
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes < 1024)
            {
                return string.Format("{0} B", bytes);
            }

            if (bytes < 1024 * 1024)
            {
                return string.Format("{0:0.#} KB", bytes / 1024D);
            }

            return string.Format("{0:0.#} MB", bytes / (1024D * 1024D));
        }

        private static Button CreatePrimaryButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Height = 34,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static Button CreateSecondaryButton(string text)
        {
            return new Button
            {
                Text = text,
                Width = 92,
                Height = 34,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
        }
    }
}
