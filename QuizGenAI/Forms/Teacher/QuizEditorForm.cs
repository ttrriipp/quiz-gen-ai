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
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(1440, 900);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quiz Editor";

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 94,
                BackColor = Color.White,
                Padding = new Padding(24, 18, 24, 16)
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "Manual Quiz Editor"
            };

            var lblSubtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "Build a draft quiz, add multiple-choice questions, and save for later review."
            };

            header.Controls.Add(lblSubtitle);
            header.Controls.Add(lblTitle);

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(24)
            };

            var metaPanel = CreateSurfacePanel();
            metaPanel.Dock = DockStyle.Top;
            metaPanel.Height = 170;
            metaPanel.Padding = new Padding(18);

            var metaLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 4
            };
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            metaLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            metaLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            _txtTitle = new TextBox { Dock = DockStyle.Fill };
            _txtTopic = new TextBox { Dock = DockStyle.Fill };
            _cmbSubject = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbDifficulty = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _nudDuration = new NumericUpDown
            {
                Dock = DockStyle.Left,
                Width = 120,
                Minimum = 0,
                Maximum = 300,
                Value = 20
            };

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
            metaLayout.Controls.Add(CreateHintLabel("minutes"), 1, 3);

            metaPanel.Controls.Add(metaLayout);

            var editorArea = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 18, 0, 0)
            };
            editorArea.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420F));
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
                Font = new Font("Segoe UI", 10F),
                IntegralHeight = false,
                HorizontalScrollbar = true
            };
            _lstQuestions.SelectedIndexChanged += LstQuestions_SelectedIndexChanged;

            var listButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 42
            };

            var btnAddNew = CreatePrimaryButton("New Question");
            btnAddNew.Width = 128;
            btnAddNew.Location = new Point(0, 4);
            btnAddNew.Click += delegate { BeginNewQuestion(); };

            var btnRemove = new Button
            {
                Text = "Remove",
                Width = 92,
                Height = 34,
                Location = new Point(138, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRemove.Click += delegate { RemoveSelectedQuestion(); };

            listButtons.Controls.Add(btnAddNew);
            listButtons.Controls.Add(btnRemove);
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
            questionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            questionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            questionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            _txtQuestionText = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _txtChoiceA = new TextBox { Dock = DockStyle.Fill };
            _txtChoiceB = new TextBox { Dock = DockStyle.Fill };
            _txtChoiceC = new TextBox { Dock = DockStyle.Fill };
            _txtChoiceD = new TextBox { Dock = DockStyle.Fill };
            _cmbCorrectChoice = new ComboBox { Dock = DockStyle.Left, Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbCorrectChoice.Items.AddRange(new object[] { "Choice A", "Choice B", "Choice C", "Choice D" });
            _cmbCorrectChoice.SelectedIndex = 0;
            _txtExplanation = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
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

            var btnApply = CreatePrimaryButton("Apply Question");
            btnApply.Width = 140;
            btnApply.Location = new Point(0, 4);
            btnApply.Click += delegate { TryApplyQuestionChanges(); };

            questionButtons.Controls.Add(btnApply);
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

            var btnSaveDraft = CreatePrimaryButton("Save Draft");
            btnSaveDraft.Width = 126;
            btnSaveDraft.Location = new Point(0, 6);
            btnSaveDraft.Click += delegate { SaveDraft(); };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Width = 92,
                Height = 34,
                Location = new Point(136, 6),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };

            bottomPanel.Controls.Add(btnSaveDraft);
            bottomPanel.Controls.Add(btnCancel);

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
            if (InitialQuizData != null)
            {
                LoadQuizData(InitialQuizData);
                BeginNewQuestion();
                return;
            }

            if (!QuizId.HasValue)
            {
                BeginNewQuestion();
                return;
            }

            var quiz = _quizService.GetQuizEditor(QuizId.Value);
            LoadQuizData(quiz);
            BeginNewQuestion();
        }

        private void LoadQuizData(QuizEditorDto quiz)
        {
            _txtTitle.Text = quiz.Title;
            _txtTopic.Text = quiz.Topic;
            _nudDuration.Value = quiz.DurationMinutes > _nudDuration.Maximum ? _nudDuration.Maximum : quiz.DurationMinutes;
            _cmbDifficulty.SelectedItem = quiz.Difficulty;

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
            var horizontalExtent = 0;

            for (var i = 0; i < _questions.Count; i++)
            {
                var question = _questions[i];
                var label = string.Format("Q{0}. {1}", i + 1, question.Text);
                _lstQuestions.Items.Add(label);
                var measured = TextRenderer.MeasureText(label, _lstQuestions.Font).Width + 24;
                if (measured > horizontalExtent)
                {
                    horizontalExtent = measured;
                }
            }

            _lstQuestions.HorizontalExtent = horizontalExtent;
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
                Font = new Font("Segoe UI", 9F),
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
    }
}
