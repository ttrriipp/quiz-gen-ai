using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class QuizEditorForm : Form
    {
        private readonly QuizService _quizService;
        private ComboBox _cmbSubject;
        private ComboBox _cmbDifficulty;
        private NumericUpDown _nudDuration;
        private DateTimePicker _dtpAvailableFrom;
        private DateTimePicker _dtpAvailableUntil;
        private DateTimePicker _dtpAvailableFromTime;
        private DateTimePicker _dtpAvailableUntilTime;
        private TextBox _txtTitle;
        private TextBox _txtTopic;
        private ListBox _lstQuestions;
        private TextBox _txtQuestionText;
        private TextBox _txtChoiceA;
        private TextBox _txtChoiceB;
        private TextBox _txtChoiceC;
        private TextBox _txtChoiceD;
        private ComboBox _cmbCorrectChoice;
        private TextBox _txtExplanation;
        private Label _lblQuestionHint;
        private Button _btnNewQuestion;
        private Button _btnRemoveQuestion;
        private Button _btnApplyQuestion;
        private Button _btnSaveDraft;
        private Button _btnCancel;
        private Label _lblHeaderTitle;
        private Label _lblHeaderSubtitle;
        private bool _isReadOnly;
        private List<QuizQuestionEditorDto> _questions;

        public QuizEditorForm()
        {
            InitializeComponent();
            if (DesignTimeHelper.IsInDesignMode(this))
            {
                return;
            }

            _quizService = new QuizService();
            _questions = new List<QuizQuestionEditorDto>();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
        }

        public int CurrentUserId { get; set; }
        public int? QuizId { get; set; }
        public QuizEditorDto InitialQuizData { get; set; }
        public bool IsReadOnly { get; set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadLookupData();
            LoadQuizIfNeeded();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quiz Editor";

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 94,
                BackColor = Color.White,
                Padding = new Padding(24, 18, 24, 16)
            };

            _lblHeaderTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "Manual Quiz Editor"
            };

            _lblHeaderSubtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "Build a draft quiz, add multiple-choice questions, and save for later review."
            };

            header.Controls.Add(_lblHeaderSubtitle);
            header.Controls.Add(_lblHeaderTitle);

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(24)
            };

            var metaPanel = CreateSurfacePanel();
            metaPanel.Dock = DockStyle.Top;
            metaPanel.Height = 256;
            metaPanel.Padding = new Padding(18);

            var metaLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5
            };
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

            _txtTitle = CreateEditorTextBox();
            _txtTopic = CreateEditorTextBox();
            _cmbSubject = CreateEditorComboBox(true, DockStyle.Fill);
            _cmbDifficulty = CreateEditorComboBox(true, DockStyle.Fill);
            _nudDuration = new NumericUpDown
            {
                Dock = DockStyle.Left,
                Width = 120,
                Minimum = 0,
                Maximum = 300,
                Value = 20,
                Font = new Font("Segoe UI", 11F)
            };
            _dtpAvailableFrom = CreateScheduleDatePicker();
            _dtpAvailableUntil = CreateScheduleDatePicker();
            _dtpAvailableFromTime = CreateScheduleTimePicker();
            _dtpAvailableUntilTime = CreateScheduleTimePicker();
            _dtpAvailableFrom.ValueChanged += delegate { _dtpAvailableFromTime.Enabled = _dtpAvailableFrom.Checked && !_isReadOnly; };
            _dtpAvailableUntil.ValueChanged += delegate { _dtpAvailableUntilTime.Enabled = _dtpAvailableUntil.Checked && !_isReadOnly; };

            metaLayout.Controls.Add(CreateFieldLabel("Title"), 0, 0);
            metaLayout.Controls.Add(_txtTitle, 1, 0);
            metaLayout.Controls.Add(CreateFieldLabel("Subject"), 2, 0);
            metaLayout.Controls.Add(_cmbSubject, 3, 0);
            metaLayout.Controls.Add(CreateFieldLabel("Topic"), 0, 1);
            metaLayout.Controls.Add(_txtTopic, 1, 1);
            metaLayout.Controls.Add(CreateFieldLabel("Difficulty"), 2, 1);
            metaLayout.Controls.Add(_cmbDifficulty, 3, 1);
            metaLayout.Controls.Add(CreateFieldLabel("Duration"), 0, 2);
            metaLayout.Controls.Add(_nudDuration, 1, 2);
            var lblDurationUnit = CreateHintLabel("minutes");
            lblDurationUnit.Dock = DockStyle.Left;
            lblDurationUnit.TextAlign = ContentAlignment.MiddleLeft;
            lblDurationUnit.Margin = new Padding(8, 0, 0, 0);
            metaLayout.Controls.Add(lblDurationUnit, 2, 2);
            metaLayout.Controls.Add(CreateScheduleInputPanel("Available from", _dtpAvailableFrom, _dtpAvailableFromTime), 1, 3);
            metaLayout.Controls.Add(CreateScheduleInputPanel("Available until", _dtpAvailableUntil, _dtpAvailableUntilTime), 3, 3);

            metaPanel.Controls.Add(metaLayout);

            var editorArea = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 18, 0, 0)
            };
            editorArea.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 800F));
            editorArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            editorArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var listPanel = CreateSurfacePanel();
            listPanel.Dock = DockStyle.Fill;
            listPanel.Padding = new Padding(18);

            var lblQuestions = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "Questions"
            };

            _lstQuestions = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11F),
                IntegralHeight = false,
                HorizontalScrollbar = false,
                DrawMode = DrawMode.OwnerDrawVariable
            };
            _lstQuestions.MeasureItem += LstQuestions_MeasureItem;
            _lstQuestions.DrawItem += LstQuestions_DrawItem;
            _lstQuestions.SelectedIndexChanged += LstQuestions_SelectedIndexChanged;

            var listButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 42
            };

            _btnNewQuestion = CreatePrimaryButton("New Question");
            _btnNewQuestion.Width = 128;
            _btnNewQuestion.Location = new Point(0, 4);
            _btnNewQuestion.Click += delegate { BeginNewQuestion(); };

            _btnRemoveQuestion = new Button
            {
                Text = "Remove",
                Width = 104,
                Height = 38,
                Location = new Point(138, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10.5F)
            };
            _btnRemoveQuestion.Click += delegate { RemoveSelectedQuestion(); };

            listButtons.Controls.Add(_btnNewQuestion);
            listButtons.Controls.Add(_btnRemoveQuestion);
            listPanel.Controls.Add(_lstQuestions);
            listPanel.Controls.Add(listButtons);
            listPanel.Controls.Add(lblQuestions);

            var questionPanel = CreateSurfacePanel();
            questionPanel.Dock = DockStyle.Fill;
            questionPanel.Padding = new Padding(18);

            var questionScrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0, 0, 0, 14)
            };

            var questionLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 9
            };
            questionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            questionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            _txtQuestionText = CreateEditorTextBox(true);
            _txtChoiceA = CreateEditorTextBox();
            _txtChoiceB = CreateEditorTextBox();
            _txtChoiceC = CreateEditorTextBox();
            _txtChoiceD = CreateEditorTextBox();
            _cmbCorrectChoice = CreateEditorComboBox(true, DockStyle.Left);
            _cmbCorrectChoice.Width = 190;
            _cmbCorrectChoice.Items.AddRange(new object[] { "Choice A", "Choice B", "Choice C", "Choice D" });
            _cmbCorrectChoice.SelectedIndex = 0;
            _txtExplanation = CreateEditorTextBox(true);
            _lblQuestionHint = CreateHintLabel("Select an existing question to edit it or start a new one.");

            questionLayout.Controls.Add(CreateFieldLabel("Question"), 0, 0);
            questionLayout.Controls.Add(CreateFieldLabel("Question Text"), 0, 1);
            questionLayout.Controls.Add(_txtQuestionText, 1, 1);
            questionLayout.Controls.Add(CreateFieldLabel("Choice A"), 0, 2);
            questionLayout.Controls.Add(_txtChoiceA, 1, 2);
            questionLayout.Controls.Add(CreateFieldLabel("Choice B"), 0, 3);
            questionLayout.Controls.Add(_txtChoiceB, 1, 3);
            questionLayout.Controls.Add(CreateFieldLabel("Choice C"), 0, 4);
            questionLayout.Controls.Add(_txtChoiceC, 1, 4);
            questionLayout.Controls.Add(CreateFieldLabel("Choice D"), 0, 5);
            questionLayout.Controls.Add(_txtChoiceD, 1, 5);
            questionLayout.Controls.Add(CreateFieldLabel("Correct"), 0, 6);
            questionLayout.Controls.Add(_cmbCorrectChoice, 1, 6);
            questionLayout.Controls.Add(CreateFieldLabel("Explanation"), 0, 7);
            questionLayout.Controls.Add(_txtExplanation, 1, 7);
            questionLayout.Controls.Add(_lblQuestionHint, 1, 8);

            var questionButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52
            };

            _btnApplyQuestion = CreatePrimaryButton("Apply Question");
            _btnApplyQuestion.Width = 140;
            _btnApplyQuestion.Location = new Point(0, 4);
            _btnApplyQuestion.Click += delegate { TryApplyQuestionChanges(); };

            questionButtons.Controls.Add(_btnApplyQuestion);
            questionScrollPanel.Controls.Add(questionLayout);
            questionPanel.Controls.Add(questionScrollPanel);
            questionPanel.Controls.Add(questionButtons);

            editorArea.Controls.Add(listPanel, 0, 0);
            editorArea.Controls.Add(questionPanel, 1, 0);

            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                Padding = new Padding(24, 8, 24, 12),
                BackColor = Color.White
            };

            _btnSaveDraft = CreatePrimaryButton("Save Draft");
            _btnSaveDraft.Width = 126;
            _btnSaveDraft.Location = new Point(0, 6);
            _btnSaveDraft.Click += delegate { SaveDraft(); };

            _btnCancel = new Button
            {
                Text = "Cancel",
                Width = 104,
                Height = 38,
                Location = new Point(136, 6),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10.5F)
            };
            _btnCancel.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };

            bottomPanel.Controls.Add(_btnSaveDraft);
            bottomPanel.Controls.Add(_btnCancel);

            body.Controls.Add(editorArea);
            body.Controls.Add(metaPanel);

            Controls.Add(body);
            Controls.Add(bottomPanel);
            Controls.Add(header);
            ResumeLayout();
        }

        private void LoadLookupData()
        {
            if (_cmbSubject.Items.Count == 0)
            {
                var subjects = _quizService.GetSubjects();
                foreach (var subject in subjects)
                {
                    _cmbSubject.Items.Add(subject);
                }
            }

            if (_cmbDifficulty.Items.Count == 0)
            {
                foreach (QuizDifficulty difficulty in Enum.GetValues(typeof(QuizDifficulty)))
                {
                    _cmbDifficulty.Items.Add(difficulty);
                }
            }

            if (_cmbDifficulty.SelectedIndex < 0 && _cmbDifficulty.Items.Count > 0)
            {
                _cmbDifficulty.SelectedIndex = 0;
            }

            if (_cmbSubject.SelectedIndex < 0 && _cmbSubject.Items.Count > 0)
            {
                _cmbSubject.SelectedIndex = 0;
            }
        }

        private void LoadQuizIfNeeded()
        {
            _isReadOnly = IsReadOnly;

            if (InitialQuizData != null)
            {
                LoadQuizData(InitialQuizData);
                InitializeQuestionEditorState();
                ApplyReadOnlyMode();
                return;
            }

            if (!QuizId.HasValue)
            {
                BeginNewQuestion();
                return;
            }

            _isReadOnly = _isReadOnly || _quizService.IsQuizLockedForEditing(QuizId.Value);
            var quiz = _quizService.GetQuizEditor(QuizId.Value);
            LoadQuizData(quiz);
            InitializeQuestionEditorState();
            ApplyReadOnlyMode();
        }

        private void InitializeQuestionEditorState()
        {
            if (_isReadOnly && _questions.Count > 0)
            {
                _lstQuestions.SelectedIndex = 0;
                return;
            }

            BeginNewQuestion();
        }

        private void LoadQuizData(QuizEditorDto quiz)
        {
            _txtTitle.Text = quiz.Title;
            _txtTopic.Text = quiz.Topic;
            _nudDuration.Value = quiz.DurationMinutes > _nudDuration.Maximum ? _nudDuration.Maximum : quiz.DurationMinutes;
            _cmbDifficulty.SelectedItem = quiz.Difficulty;
            ApplyScheduleValue(_dtpAvailableFrom, _dtpAvailableFromTime, quiz.AvailableFrom);
            ApplyScheduleValue(_dtpAvailableUntil, _dtpAvailableUntilTime, quiz.AvailableUntil);

            for (var i = 0; i < _cmbSubject.Items.Count; i++)
            {
                var subject = _cmbSubject.Items[i] as SubjectOptionDto;
                if (subject != null && subject.Id == quiz.SubjectId)
                {
                    _cmbSubject.SelectedIndex = i;
                    break;
                }
            }

            _questions = quiz.Questions ?? new List<QuizQuestionEditorDto>();
            RefreshQuestionList();
        }

        private void RefreshQuestionList()
        {
            _lstQuestions.Items.Clear();

            for (var i = 0; i < _questions.Count; i++)
            {
                var question = _questions[i];
                var label = BuildQuestionPreviewText(question, i + 1);
                _lstQuestions.Items.Add(label);
            }
            _lstQuestions.Invalidate();
        }

        private static string BuildQuestionPreviewText(QuizQuestionEditorDto question, int number)
        {
            var lines = new List<string>
            {
                string.Format("Q{0}. {1}", number, question.Text ?? string.Empty)
            };

            var choices = question.Choices ?? new List<QuizChoiceEditorDto>();
            var correctChoiceIndex = -1;
            for (var i = 0; i < choices.Count; i++)
            {
                if (correctChoiceIndex < 0 && choices[i] != null && choices[i].IsCorrect)
                {
                    correctChoiceIndex = i;
                }
            }

            if (correctChoiceIndex >= 0 && correctChoiceIndex < choices.Count)
            {
                var answerPrefix = correctChoiceIndex < 26
                    ? ((char)('A' + correctChoiceIndex)).ToString()
                    : (correctChoiceIndex + 1).ToString();
                var answerText = choices[correctChoiceIndex] != null ? choices[correctChoiceIndex].Text : string.Empty;
                lines.Add(string.Format("    Answer: {0}) {1}", answerPrefix, answerText));
            }

            return string.Join(Environment.NewLine, lines);
        }

        private void LstQuestions_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _lstQuestions.Items.Count)
            {
                e.ItemHeight = 44;
                return;
            }

            var text = Convert.ToString(_lstQuestions.Items[e.Index]) ?? string.Empty;
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var questionLine = lines.Length > 0 ? lines[0] : string.Empty;
            var answerLine = lines.Length > 1 ? lines[1] : string.Empty;
            var width = Math.Max(160, _lstQuestions.ClientSize.Width - 16);

            using (var questionFont = new Font(_lstQuestions.Font, FontStyle.Bold))
            using (var answerFont = new Font(_lstQuestions.Font.FontFamily, Math.Max(9F, _lstQuestions.Font.Size - 1.5F), FontStyle.Regular))
            {
                var questionSize = TextRenderer.MeasureText(
                    questionLine,
                    questionFont,
                    new Size(width, int.MaxValue),
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                var answerSize = TextRenderer.MeasureText(
                    answerLine,
                    answerFont,
                    new Size(width - 18, int.MaxValue),
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                e.ItemHeight = Math.Max(50, questionSize.Height + answerSize.Height + 14);
            }
        }

        private void LstQuestions_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _lstQuestions.Items.Count)
            {
                return;
            }

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var background = selected ? Color.FromArgb(17, 93, 86) : _lstQuestions.BackColor;
            var foreground = selected ? Color.White : _lstQuestions.ForeColor;

            var text = Convert.ToString(_lstQuestions.Items[e.Index]) ?? string.Empty;
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var questionLine = lines.Length > 0 ? lines[0] : string.Empty;
            var answerLine = lines.Length > 1 ? lines[1] : string.Empty;

            using (var bg = new SolidBrush(background))
            using (var fg = new SolidBrush(foreground))
            using (var questionFont = new Font(_lstQuestions.Font, FontStyle.Bold))
            using (var answerFont = new Font(_lstQuestions.Font.FontFamily, Math.Max(9F, _lstQuestions.Font.Size - 1.5F), FontStyle.Regular))
            {
                e.Graphics.FillRectangle(bg, e.Bounds);
                var questionBounds = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 4, e.Bounds.Width - 12, e.Bounds.Height - 8);
                e.Graphics.DrawString(questionLine, questionFont, fg, questionBounds);

                if (!string.IsNullOrWhiteSpace(answerLine))
                {
                    var questionHeight = TextRenderer.MeasureText(
                        questionLine,
                        questionFont,
                        new Size(questionBounds.Width, int.MaxValue),
                        TextFormatFlags.WordBreak | TextFormatFlags.NoPadding).Height;
                    var answerBounds = new Rectangle(questionBounds.X + 16, questionBounds.Y + questionHeight + 2, questionBounds.Width - 16, questionBounds.Height - questionHeight - 2);
                    e.Graphics.DrawString(answerLine, answerFont, fg, answerBounds);
                }
            }

            e.DrawFocusRectangle();
        }

        private void LstQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lstQuestions.SelectedIndex < 0 || _lstQuestions.SelectedIndex >= _questions.Count)
            {
                return;
            }

            var question = _questions[_lstQuestions.SelectedIndex];
            _txtQuestionText.Text = question.Text;
            _txtExplanation.Text = question.Explanation;

            var choices = question.Choices.ToList();
            _txtChoiceA.Text = choices.Count > 0 ? choices[0].Text : string.Empty;
            _txtChoiceB.Text = choices.Count > 1 ? choices[1].Text : string.Empty;
            _txtChoiceC.Text = choices.Count > 2 ? choices[2].Text : string.Empty;
            _txtChoiceD.Text = choices.Count > 3 ? choices[3].Text : string.Empty;
            _cmbCorrectChoice.SelectedIndex = Math.Max(0, choices.FindIndex(x => x.IsCorrect));
            _lblQuestionHint.Text = string.Format("Editing question {0}. Click Apply Question to keep your changes.", _lstQuestions.SelectedIndex + 1);
        }

        private void BeginNewQuestion()
        {
            _lstQuestions.ClearSelected();
            _txtQuestionText.Clear();
            _txtChoiceA.Clear();
            _txtChoiceB.Clear();
            _txtChoiceC.Clear();
            _txtChoiceD.Clear();
            _txtExplanation.Clear();
            _cmbCorrectChoice.SelectedIndex = 0;
            _lblQuestionHint.Text = "Creating a new question. Add text, choices, and the correct answer, then apply it.";
            _txtQuestionText.Focus();
        }

        private void ApplyQuestionChanges()
        {
            var question = BuildQuestionFromInputs();

            if (_lstQuestions.SelectedIndex >= 0 && _lstQuestions.SelectedIndex < _questions.Count)
            {
                _questions[_lstQuestions.SelectedIndex] = question;
            }
            else
            {
                _questions.Add(question);
            }

            RefreshQuestionList();
            _lstQuestions.SelectedIndex = _questions.Count - 1;
        }

        private void TryApplyQuestionChanges()
        {
            try
            {
                ApplyQuestionChanges();
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowWarning(this, "Question Validation", ex.Message);
            }
        }

        private void RemoveSelectedQuestion()
        {
            if (_lstQuestions.SelectedIndex < 0 || _lstQuestions.SelectedIndex >= _questions.Count)
            {
                NotificationHelper.ShowInfo(this, "No Selection", "Select a question to remove.");
                return;
            }

            _questions.RemoveAt(_lstQuestions.SelectedIndex);
            RefreshQuestionList();
            BeginNewQuestion();
        }

        private void SaveDraft()
        {
            try
            {
                if (_isReadOnly)
                {
                    throw new InvalidOperationException("This posted quiz already has student submissions and is now view-only.");
                }

                TryCapturePendingQuestion();

                var subject = _cmbSubject.SelectedItem as SubjectOptionDto;
                if (subject == null)
                {
                    throw new InvalidOperationException("Select a subject.");
                }

                var difficulty = _cmbDifficulty.SelectedItem is QuizDifficulty
                    ? (QuizDifficulty)_cmbDifficulty.SelectedItem
                    : QuizDifficulty.Easy;

                var dto = new QuizEditorDto
                {
                    Id = QuizId.GetValueOrDefault(),
                    Title = _txtTitle.Text,
                    SubjectId = subject.Id,
                    Topic = _txtTopic.Text,
                    Difficulty = difficulty,
                    DurationMinutes = Convert.ToInt32(_nudDuration.Value),
                    Status = QuizStatus.Draft,
                    AvailableFrom = BuildScheduleValue(_dtpAvailableFrom, _dtpAvailableFromTime),
                    AvailableUntil = BuildScheduleValue(_dtpAvailableUntil, _dtpAvailableUntilTime),
                    Questions = _questions.ToList()
                };

                var savedQuizId = _quizService.SaveDraft(dto, CurrentUserId);
                QuizId = savedQuizId;
                NotificationHelper.ShowSuccess(this, "Draft Saved", "Quiz draft saved successfully.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Quiz draft save failed. QuizId={QuizId}", QuizId.GetValueOrDefault());
                NotificationHelper.ShowError(this, "Save Draft Failed", ex.Message);
            }
        }

        private void TryCapturePendingQuestion()
        {
            var hasPendingValues =
                !string.IsNullOrWhiteSpace(_txtQuestionText.Text) ||
                !string.IsNullOrWhiteSpace(_txtChoiceA.Text) ||
                !string.IsNullOrWhiteSpace(_txtChoiceB.Text) ||
                !string.IsNullOrWhiteSpace(_txtChoiceC.Text) ||
                !string.IsNullOrWhiteSpace(_txtChoiceD.Text);

            if (!hasPendingValues)
            {
                return;
            }

            ApplyQuestionChanges();
        }

        private QuizQuestionEditorDto BuildQuestionFromInputs()
        {
            if (string.IsNullOrWhiteSpace(_txtQuestionText.Text))
            {
                throw new InvalidOperationException("Question text is required.");
            }

            var choices = new[]
            {
                _txtChoiceA.Text,
                _txtChoiceB.Text,
                _txtChoiceC.Text,
                _txtChoiceD.Text
            };

            if (choices.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException("All four choices are required for this manual editor.");
            }

            var correctIndex = _cmbCorrectChoice.SelectedIndex < 0 ? 0 : _cmbCorrectChoice.SelectedIndex;
            var question = new QuizQuestionEditorDto
            {
                Text = _txtQuestionText.Text.Trim(),
                Explanation = string.IsNullOrWhiteSpace(_txtExplanation.Text) ? null : _txtExplanation.Text.Trim()
            };

            for (var i = 0; i < choices.Length; i++)
            {
                question.Choices.Add(new QuizChoiceEditorDto
                {
                    Text = choices[i].Trim(),
                    IsCorrect = i == correctIndex
                });
            }

            return question;
        }

        private static Control CreateFieldLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = text
            };
        }

        private static Label CreateHintLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                TextAlign = ContentAlignment.TopLeft,
                Text = text
            };
        }

        private static Panel CreateSurfacePanel()
        {
            return new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static Button CreatePrimaryButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Height = 38,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static TextBox CreateEditorTextBox(bool multiline = false)
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = multiline,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None,
                Font = new Font("Segoe UI", 12F)
            };
        }

        private static ComboBox CreateEditorComboBox(bool dropDownList, DockStyle dockStyle)
        {
            return new ComboBox
            {
                Dock = dockStyle,
                DropDownStyle = dropDownList ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown,
                Font = new Font("Segoe UI", 12F)
            };
        }

        private static DateTimePicker CreateScheduleDatePicker()
        {
            return new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMM d, yyyy",
                Font = new Font("Segoe UI", 10F),
                ShowCheckBox = true,
                Checked = false,
                Value = DateTime.Now.Date
            };
        }

        private static DateTimePicker CreateScheduleTimePicker()
        {
            return new DateTimePicker
            {
                Dock = DockStyle.Right,
                Width = 92,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "h:mm tt",
                Font = new Font("Segoe UI", 10F),
                ShowUpDown = true,
                Enabled = false,
                Value = DateTime.Today.AddHours(8)
            };
        }

        private static Control CreateScheduleInputPanel(string labelText, DateTimePicker datePicker, DateTimePicker timePicker)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 12, 0)
            };

            var label = new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = labelText,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var inputRow = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 4, 0, 0)
            };
            inputRow.Controls.Add(datePicker);
            inputRow.Controls.Add(timePicker);

            panel.Controls.Add(inputRow);
            panel.Controls.Add(label);
            return panel;
        }

        private static void ApplyScheduleValue(DateTimePicker datePicker, DateTimePicker timePicker, DateTime? value)
        {
            var localValue = value.HasValue ? value.Value : DateTime.Now.AddHours(1);

            datePicker.Value = localValue.Date;
            datePicker.Checked = value.HasValue;
            timePicker.Value = DateTime.Today.Add(localValue.TimeOfDay);
            timePicker.Enabled = value.HasValue;
        }

        private static DateTime? BuildScheduleValue(DateTimePicker datePicker, DateTimePicker timePicker)
        {
            if (!datePicker.Checked)
            {
                return null;
            }

            return datePicker.Value.Date.Add(timePicker.Value.TimeOfDay);
        }

        private void ApplyReadOnlyMode()
        {
            if (!_isReadOnly)
            {
                return;
            }

            _lblHeaderTitle.Text = "Quiz Viewer";
            _lblHeaderSubtitle.Text = "This posted quiz has student submissions and is now view-only.";
            _lblQuestionHint.Text = "View-only mode: editing is disabled because students already submitted answers.";

            _txtTitle.ReadOnly = true;
            _txtTopic.ReadOnly = true;
            _txtQuestionText.ReadOnly = true;
            _txtChoiceA.ReadOnly = true;
            _txtChoiceB.ReadOnly = true;
            _txtChoiceC.ReadOnly = true;
            _txtChoiceD.ReadOnly = true;
            _txtExplanation.ReadOnly = true;
            _txtTitle.TabStop = false;
            _txtTopic.TabStop = false;
            _txtQuestionText.TabStop = false;
            _txtChoiceA.TabStop = false;
            _txtChoiceB.TabStop = false;
            _txtChoiceC.TabStop = false;
            _txtChoiceD.TabStop = false;
            _txtExplanation.TabStop = false;

            _cmbSubject.Enabled = false;
            _cmbDifficulty.Enabled = false;
            _cmbCorrectChoice.Enabled = false;
            _nudDuration.Enabled = false;
            _dtpAvailableFrom.Enabled = false;
            _dtpAvailableUntil.Enabled = false;
            _dtpAvailableFromTime.Enabled = false;
            _dtpAvailableUntilTime.Enabled = false;

            _btnNewQuestion.Enabled = false;
            _btnRemoveQuestion.Enabled = false;
            _btnApplyQuestion.Enabled = false;
            _btnSaveDraft.Visible = false;
            _btnCancel.Text = "Close";
            _btnCancel.Location = new Point(0, 6);
        }
    }
}
