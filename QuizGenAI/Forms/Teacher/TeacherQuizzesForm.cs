using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherQuizzesForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EmSetCueBanner = 0x1501;

        private static readonly Color CardBg = Color.FromArgb(16, 58, 44);
        private static readonly Color SubtleBorder = Color.FromArgb(72, 255, 255, 255);

        private readonly QuizService _quizService;
        private TextBox _txtSearch;
        private ComboBox _cmbStatus;
        private FlowLayoutPanel _cardsHost;

        public TeacherQuizzesForm()
        {
            InitializeComponent();
            if (DesignTimeHelper.IsInDesignMode(this))
            {
                return;
            }

            _quizService = new QuizService();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
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

            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(1180, 760);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Teacher Quizzes";

            // Compact toolbar: search | filter | Generate Quiz
            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = Color.Transparent
            };

            _txtSearch = new TextBox
            {
                Width = 260,
                Location = new Point(0, 12)
            };
            _txtSearch.TextChanged += delegate { LoadQuizCards(); };
            _txtSearch.HandleCreated += delegate
            {
                SendMessage(_txtSearch.Handle, EmSetCueBanner, IntPtr.Zero, "Search by title, topic, subject...");
            };

            _cmbStatus = new ComboBox
            {
                Width = 148,
                Location = new Point(268, 14),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbStatus.Items.Add(new StatusFilterOption("All statuses", null));
            _cmbStatus.Items.Add(new StatusFilterOption("Draft", QuizStatus.Draft));
            _cmbStatus.Items.Add(new StatusFilterOption("Posted", QuizStatus.Published));
            _cmbStatus.Items.Add(new StatusFilterOption("Archived", QuizStatus.Archived));
            _cmbStatus.SelectedIndex = 0;
            _cmbStatus.SelectedIndexChanged += delegate { LoadQuizCards(); };

            var btnNewAiQuiz = CreateActionButton("Generate Quiz");
            btnNewAiQuiz.Width = 148;
            btnNewAiQuiz.Height = 34;
            btnNewAiQuiz.Location = new Point(428, 10);
            btnNewAiQuiz.Click += async delegate { await OpenAiGeneratorAsync(); };

            toolbar.Controls.Add(_txtSearch);
            toolbar.Controls.Add(_cmbStatus);
            toolbar.Controls.Add(btnNewAiQuiz);

            _cardsHost = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(0, 14, 0, 0),
                BackColor = Color.Transparent
            };

            Controls.Add(_cardsHost);
            Controls.Add(toolbar);
            ResumeLayout();
        }

        private void LoadQuizCards()
        {
            var selectedStatus = _cmbStatus.SelectedItem as StatusFilterOption;
            var quizzes = _quizService.GetQuizSummaries(_txtSearch.Text, selectedStatus != null ? selectedStatus.Value : null);

            _cardsHost.SuspendLayout();
            _cardsHost.Controls.Clear();

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
            const int cardW = 290;
            const int px = 14;
            const int contentW = cardW - px * 2;

            var card = new Panel
            {
                Width = cardW,
                Height = 242,
                Margin = new Padding(0, 0, 12, 12),
                BackColor = CardBg,
                BorderStyle = BorderStyle.None
            };
            card.Paint += CardPanel_PaintRoundedBorder;

            // Subject badge – pill on the left
            var subjectBadge = CreateBadgePill(quiz.SubjectName, Color.FromArgb(22, 74, 56), new Point(px, 12));

            // Status badge – pill on the right, dark themed colors
            var statusBadge = CreateBadgePill(GetStatusDisplayText(quiz.Status), GetStatusBgColor(quiz.Status), Point.Empty);
            statusBadge.Location = new Point(cardW - statusBadge.Width - px, 12);

            var lblTitle = new Label
            {
                AutoSize = false,
                Width = contentW,
                Height = 44,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(235, 243, 239),
                Text = quiz.Title,
                Location = new Point(px, 44)
            };

            var lblTopic = new Label
            {
                AutoSize = false,
                Width = contentW,
                Height = 20,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(184, 201, 193),
                Text = quiz.Topic,
                Location = new Point(px, 92)
            };

            var lblUpdated = new Label
            {
                AutoSize = false,
                Width = contentW,
                Height = 16,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(140, 165, 155),
                Text = string.Format("Updated {0:g}", quiz.UpdatedAt.ToLocalTime()),
                Location = new Point(px, 114)
            };

            // Meta: "X questions  ·  Difficulty · Xm"
            var lblMeta = new Label
            {
                AutoSize = false,
                Width = contentW,
                Height = 20,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(184, 201, 193),
                Text = string.Format("{0} question{1}  ·  {2} · {3}m",
                    quiz.QuestionCount,
                    quiz.QuestionCount == 1 ? string.Empty : "s",
                    quiz.Difficulty,
                    quiz.DurationMinutes),
                Location = new Point(px, 136)
            };

            // "Review & edit" – full-width primary button
            var btnEdit = new Button
            {
                Text = quiz.IsLockedForEditing ? "View Quiz" : "Review && edit",
                Width = contentW,
                Height = 32,
                Location = new Point(px, 164),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(26, 90, 68),
                ForeColor = Color.FromArgb(235, 243, 239),
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(44, 105, 82);
            btnEdit.FlatAppearance.BorderSize = 1;
            btnEdit.Click += delegate { OpenEditor(quiz.Id, quiz.IsLockedForEditing); };

            // Secondary action buttons
            var btnStatus = new Button
            {
                Text = GetPrimaryStatusActionLabel(quiz),
                Width = 92,
                Height = 26,
                Location = new Point(px, 202),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnStatus.Click += delegate { HandlePrimaryStatusAction(quiz); };

            var btnArchive = new Button
            {
                Text = "Archive",
                Width = 70,
                Height = 26,
                Location = new Point(px + 98, 202),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnArchive.Click += delegate { ArchiveQuiz(quiz); };
            btnArchive.Enabled = quiz.Status != QuizStatus.Archived;

            var btnDelete = new Button
            {
                Text = "Delete",
                Width = 70,
                Height = 26,
                Location = new Point(px + 174, 202),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(240, 100, 90),
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            btnDelete.Click += delegate { DeleteQuiz(quiz); };

            card.Controls.Add(btnDelete);
            card.Controls.Add(btnArchive);
            card.Controls.Add(btnStatus);
            card.Controls.Add(btnEdit);
            card.Controls.Add(lblMeta);
            card.Controls.Add(lblUpdated);
            card.Controls.Add(lblTopic);
            card.Controls.Add(lblTitle);
            card.Controls.Add(statusBadge);
            card.Controls.Add(subjectBadge);

            return card;
        }

        private static Panel CreateBadgePill(string text, Color bgColor, Point location)
        {
            const int badgeH = 24;
            var font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            var textW = TextRenderer.MeasureText(text, font).Width;
            var w = Math.Max(56, Math.Min(120, textW + 18));

            var pill = new Panel
            {
                Location = location,
                Size = new Size(w, badgeH),
                BackColor = bgColor
            };
            pill.Paint += BadgePill_PaintBorder;
            pill.Controls.Add(new Label
            {
                Bounds = new Rectangle(0, 0, w, badgeH),
                Font = font,
                ForeColor = Color.FromArgb(235, 243, 239),
                BackColor = Color.Transparent,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            });

            return pill;
        }

        private Control CreateEmptyStatePanel()
        {
            var panel = new Panel
            {
                Width = 420,
                Height = 120,
                BackColor = CardBg,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20)
            };
            panel.Paint += CardPanel_PaintRoundedBorder;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(184, 201, 193),
                Text = "No quizzes match the current filters.\r\nCreate a new quiz to get started."
            });

            return panel;
        }

        private void OpenEditor(int? quizId, bool readOnly)
        {
            using (var form = new QuizEditorForm())
            {
                form.CurrentUserId = CurrentUserId;
                form.QuizId = quizId;
                form.IsReadOnly = readOnly;

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
                        NotificationHelper.ShowSuccess(this, "Quiz Saved", "The AI-generated quiz draft was reviewed and saved.");
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
                NotificationHelper.ShowSuccess(
                    this,
                    "Quiz removed",
                    string.Format(
                        "\"{0}\" was removed from the app. The row is kept in the database for history, but the quiz can no longer be opened or used here.",
                        quiz.Title));
                LoadQuizCards();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Quiz delete failed. QuizId={QuizId}", quiz.Id);
                NotificationHelper.ShowError(this, "Delete Failed", ex.Message);
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

                NotificationHelper.ShowSuccess(
                    this,
                    "Quiz Status Updated",
                    quiz.Status == QuizStatus.Published
                        ? string.Format("\"{0}\" moved back to draft.", quiz.Title)
                        : string.Format("\"{0}\" is now posted.", quiz.Title));
                LoadQuizCards();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Quiz status update failed. QuizId={QuizId}", quiz.Id);
                NotificationHelper.ShowError(this, "Status Update Failed", ex.Message);
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
                NotificationHelper.ShowSuccess(this, "Quiz Archived", string.Format("\"{0}\" was archived.", quiz.Title));
                LoadQuizCards();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Quiz archive failed. QuizId={QuizId}", quiz.Id);
                NotificationHelper.ShowError(this, "Archive Failed", ex.Message);
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
            return quiz.Status == QuizStatus.Published ? "Draft" : "Post";
        }

        private static string GetStatusDisplayText(QuizStatus status)
        {
            return status == QuizStatus.Published ? "Posted" : status.ToString();
        }

        private static Color GetStatusBgColor(QuizStatus status)
        {
            switch (status)
            {
                case QuizStatus.Published: return Color.FromArgb(24, 105, 72);
                case QuizStatus.Archived: return Color.FromArgb(60, 70, 80);
                default: return Color.FromArgb(130, 92, 18);
            }
        }

        private static void CardPanel_PaintRoundedBorder(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            if (panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            const int radius = 14;
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(SubtleBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static void BadgePill_PaintBorder(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            if (panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            const int r = 10;
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(SubtleBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
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
