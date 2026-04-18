using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherQuizzesForm : Form
    {
        private readonly QuizService _quizService;
        private TextBox _txtSearch;
        private ComboBox _cmbStatus;
        private FlowLayoutPanel _cardsHost;
        private Label _lblSummary;

        public TeacherQuizzesForm()
        {
            InitializeComponent();
            _quizService = new QuizService();
            BuildLayout();
        }

        public int CurrentUserId { get; set; }
        public string DisplayName { get; set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadQuizCards();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(244, 246, 248);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Teacher Quizzes";

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 126,
                BackColor = Color.White,
                Padding = new Padding(24, 20, 24, 18)
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "Quiz Management"
            };

            _lblSummary = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "Search, review, and maintain draft quizzes."
            };

            var filtersPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 42
            };

            _txtSearch = new TextBox
            {
                Width = 320,
                Location = new Point(0, 8)
            };
            _txtSearch.TextChanged += delegate { LoadQuizCards(); };

            _cmbStatus = new ComboBox
            {
                Width = 170,
                Location = new Point(336, 8),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbStatus.Items.Add(new StatusFilterOption("All statuses", null));
            _cmbStatus.Items.Add(new StatusFilterOption("Draft", QuizStatus.Draft));
            _cmbStatus.Items.Add(new StatusFilterOption("Published", QuizStatus.Published));
            _cmbStatus.Items.Add(new StatusFilterOption("Archived", QuizStatus.Archived));
            _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += delegate { LoadQuizCards(); };

            var btnNewQuiz = CreateActionButton("New Manual Quiz");
            btnNewQuiz.Location = new Point(530, 4);
            btnNewQuiz.Click += delegate { OpenEditor(null); };

            var btnNewAiQuiz = CreateActionButton("New AI Quiz");
            btnNewAiQuiz.Width = 116;
            btnNewAiQuiz.Location = new Point(688, 4);
            btnNewAiQuiz.Click += async delegate { await OpenAiGeneratorAsync(); };

            var btnClose = new Button
            {
                Text = "Close",
                Width = 96,
                Height = 34,
                Location = new Point(814, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.Click += delegate { Close(); };

            filtersPanel.Controls.Add(_txtSearch);
            filtersPanel.Controls.Add(_cmbStatus);
            filtersPanel.Controls.Add(btnNewQuiz);
            filtersPanel.Controls.Add(btnNewAiQuiz);
            filtersPanel.Controls.Add(btnClose);

            topPanel.Controls.Add(filtersPanel);
            topPanel.Controls.Add(_lblSummary);
            topPanel.Controls.Add(lblTitle);

            _cardsHost = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(24),
                BackColor = Color.FromArgb(244, 246, 248)
            };

            Controls.Add(_cardsHost);
            Controls.Add(topPanel);
            ResumeLayout();
        }

        private void LoadQuizCards()
        {
            var selectedStatus = (_cmbStatus.SelectedItem as StatusFilterOption);
            var quizzes = _quizService.GetQuizSummaries(_txtSearch.Text, selectedStatus != null ? selectedStatus.Value : null);

            _cardsHost.SuspendLayout();
            _cardsHost.Controls.Clear();

            _lblSummary.Text = string.Format(
                "{0} quiz{1} found{2}. Signed in as {3}.",
                quizzes.Count,
                quizzes.Count == 1 ? string.Empty : "zes",
                quizzes.Count == 1 ? string.Empty : "",
                string.IsNullOrWhiteSpace(DisplayName) ? "teacher" : DisplayName);

            if (quizzes.Count == 0)
            {
                _cardsHost.Controls.Add(CreateEmptyStatePanel());
                _cardsHost.ResumeLayout();
                return;
            }

            foreach (var quiz in quizzes)
            {
                _cardsHost.Controls.Add(CreateQuizCard(quiz));
            }

            _cardsHost.ResumeLayout();
        }

        private Panel CreateQuizCard(QuizListItemDto quiz)
        {
            var card = new Panel
            {
                Width = 330,
                Height = 240,
                Margin = new Padding(0, 0, 18, 18),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18, 16, 18, 16)
            };

            var badge = new Label
            {
                AutoSize = false,
                Width = 94,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Text = quiz.Status.ToString(),
                BackColor = GetStatusBackColor(quiz.Status),
                ForeColor = GetStatusForeColor(quiz.Status),
                Location = new Point(card.Width - 118, 16)
            };
            badge.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            var lblSubject = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(14, 116, 144),
                Text = quiz.SubjectName
            };

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 54,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = quiz.Title
            };

            var lblTopic = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = quiz.Topic
            };

            var lblMeta = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 44,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = string.Format(
                    "{0} question{1}  |  {2}  |  {3} mins",
                    quiz.QuestionCount,
                    quiz.QuestionCount == 1 ? string.Empty : "s",
                    quiz.Difficulty,
                    quiz.DurationMinutes)
            };

            var lblUpdated = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Text = string.Format("Updated {0:g}", quiz.UpdatedAt.ToLocalTime())
            };

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 84
            };

            var btnEdit = CreateActionButton("Edit");
            btnEdit.Width = 88;
            btnEdit.Location = new Point(0, 4);
            btnEdit.Click += delegate { OpenEditor(quiz.Id); };

            var btnStatus = CreateActionButton(GetPrimaryStatusActionLabel(quiz));
            btnStatus.Width = 120;
            btnStatus.Location = new Point(98, 4);
            btnStatus.Click += delegate { HandlePrimaryStatusAction(quiz); };

            var btnArchive = new Button
            {
                Text = "Archive",
                Width = 88,
                Height = 34,
                Location = new Point(228, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnArchive.Click += delegate { ArchiveQuiz(quiz); };
            btnArchive.Enabled = quiz.Status != QuizStatus.Archived;

            var btnDelete = new Button
            {
                Text = "Delete",
                Width = 88,
                Height = 34,
                Location = new Point(0, 44),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(185, 28, 28),
                Cursor = Cursors.Hand
            };
            btnDelete.Click += delegate { DeleteQuiz(quiz); };

            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnStatus);
            buttonPanel.Controls.Add(btnArchive);
            buttonPanel.Controls.Add(btnDelete);

            card.Controls.Add(buttonPanel);
            card.Controls.Add(lblUpdated);
            card.Controls.Add(lblMeta);
            card.Controls.Add(lblTopic);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSubject);
            card.Controls.Add(badge);

            return card;
        }

        private Control CreateEmptyStatePanel()
        {
            var panel = new Panel
            {
                Width = 520,
                Height = 220,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(24)
            };

            var label = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "No quizzes match the current filters.\r\nCreate a manual quiz to start building the teacher workflow."
            };

            panel.Controls.Add(label);
            return panel;
        }

        private void OpenEditor(int? quizId)
        {
            using (var form = new QuizEditorForm())
            {
                form.CurrentUserId = CurrentUserId;
                form.QuizId = quizId;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadQuizCards();
                }
            }
        }

        private async Task OpenAiGeneratorAsync()
        {
            using (var generatorForm = new AiQuizGeneratorForm())
            {
                if (generatorForm.ShowDialog(this) != DialogResult.OK || generatorForm.GeneratedResult == null)
                {
                    return;
                }

                using (var editorForm = new QuizEditorForm())
                {
                    editorForm.CurrentUserId = CurrentUserId;
                    editorForm.InitialQuizData = generatorForm.GeneratedResult.Quiz;

                    if (editorForm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadQuizCards();
                    }
                }
            }

            await Task.CompletedTask;
        }

        private void DeleteQuiz(QuizListItemDto quiz)
        {
            var confirmation = MessageBox.Show(
                string.Format("Delete the quiz \"{0}\"?", quiz.Title),
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _quizService.DeleteQuiz(quiz.Id, CurrentUserId);
                LoadQuizCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandlePrimaryStatusAction(QuizListItemDto quiz)
        {
            try
            {
                if (quiz.Status == QuizStatus.Published)
                {
                    _quizService.UnpublishQuiz(quiz.Id, CurrentUserId);
                }
                else
                {
                    _quizService.PublishQuiz(quiz.Id, CurrentUserId);
                }

                LoadQuizCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Status Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchiveQuiz(QuizListItemDto quiz)
        {
            if (quiz.Status == QuizStatus.Archived)
            {
                return;
            }

            var confirmation = MessageBox.Show(
                string.Format("Archive the quiz \"{0}\"?", quiz.Title),
                "Confirm Archive",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _quizService.ArchiveQuiz(quiz.Id, CurrentUserId);
                LoadQuizCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Button CreateActionButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Width = 148,
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

        private static string GetPrimaryStatusActionLabel(QuizListItemDto quiz)
        {
            return quiz.Status == QuizStatus.Published ? "Move To Draft" : "Publish";
        }

        private static Color GetStatusBackColor(QuizStatus status)
        {
            switch (status)
            {
                case QuizStatus.Published:
                    return Color.FromArgb(220, 252, 231);
                case QuizStatus.Archived:
                    return Color.FromArgb(229, 231, 235);
                default:
                    return Color.FromArgb(254, 249, 195);
            }
        }

        private static Color GetStatusForeColor(QuizStatus status)
        {
            switch (status)
            {
                case QuizStatus.Published:
                    return Color.FromArgb(22, 101, 52);
                case QuizStatus.Archived:
                    return Color.FromArgb(55, 65, 81);
                default:
                    return Color.FromArgb(161, 98, 7);
            }
        }

        private class StatusFilterOption
        {
            public StatusFilterOption(string text, QuizStatus? value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; private set; }
            public QuizStatus? Value { get; private set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
