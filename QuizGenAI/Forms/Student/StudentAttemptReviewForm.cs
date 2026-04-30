using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentAttemptReviewForm : Form
    {
        private static readonly Color HeroBg = Color.FromArgb(20, 99, 67);
        private static readonly Color HeroAccent = Color.FromArgb(229, 190, 77);
        private static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
        private static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color CorrectBg = Color.FromArgb(236, 253, 245);
        private static readonly Color CorrectText = Color.FromArgb(4, 120, 87);
        private static readonly Color WrongBg = Color.FromArgb(254, 242, 242);
        private static readonly Color WrongText = Color.FromArgb(185, 28, 28);

        private readonly StudentAttemptReviewDto _review;

        public StudentAttemptReviewForm()
        {
            _review = new StudentAttemptReviewDto();
            InitializeComponent();
        }

        public StudentAttemptReviewForm(StudentAttemptReviewDto review)
        {
            if (review == null)
            {
                throw new ArgumentNullException("review");
            }

            _review = review;
            InitializeComponent();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(243, 244, 246);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Review Answers";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));

            root.Controls.Add(BuildHeaderPanel(), 0, 0);
            root.Controls.Add(BuildQuestionsPanel(), 0, 1);
            root.Controls.Add(BuildFooterPanel(), 0, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildHeaderPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = HeroBg,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(24, 20, 24, 20),
                Margin = new Padding(0, 0, 0, 18)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330F));

            var titleStack = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            titleStack.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(218, 232, 224),
                Text = string.Format("{0} | Submitted {1}", NullIfWhite(_review.SubjectName, "Unknown Subject"), NullIfWhite(_review.SubmittedAtDisplay, "N/A"))
            });
            titleStack.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 62,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = NullIfWhite(_review.QuizTitle, "Quiz Review")
            });
            titleStack.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(218, 232, 224),
                Text = string.Format("Topic: {0}", NullIfWhite(_review.Topic, "N/A"))
            });

            var metricPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            metricPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            metricPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            metricPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            var total = _review.Questions == null ? 0 : _review.Questions.Count;
            var correct = _review.Questions == null ? 0 : _review.Questions.Count(x => x.IsCorrect);
            var missed = Math.Max(0, total - correct);
            metricPanel.Controls.Add(CreateMetricBlock("Score", string.Format("{0:0.#}%", _review.ScorePercentage)), 0, 0);
            metricPanel.Controls.Add(CreateMetricBlock("Correct", correct.ToString()), 1, 0);
            metricPanel.Controls.Add(CreateMetricBlock("Missed", missed.ToString()), 2, 0);

            layout.Controls.Add(titleStack, 0, 0);
            layout.Controls.Add(metricPanel, 1, 0);
            panel.Controls.Add(layout);
            return panel;
        }

        private Control BuildQuestionsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18, 16, 18, 16),
                Margin = new Padding(0, 0, 0, 18)
            };

            var header = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = "Answer Review"
            };

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(250, 251, 247),
                Padding = new Padding(0, 10, 0, 0)
            };

            if (_review.Questions == null || _review.Questions.Count == 0)
            {
                content.Controls.Add(new Label
                {
                    Width = 900,
                    Height = 64,
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = TextMuted,
                    Text = "No question data was found for this attempt."
                });
            }
            else
            {
                foreach (var question in _review.Questions.OrderBy(x => x.OrderIndex))
                {
                    content.Controls.Add(CreateQuestionCard(question));
                }
            }

            panel.Controls.Add(content);
            panel.Controls.Add(header);
            return panel;
        }

        private Control BuildFooterPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var btnClose = CreateActionButton("Back To Quizzes", Color.FromArgb(22, 88, 61), Color.White);
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            flow.Controls.Add(btnClose);
            panel.Controls.Add(flow);
            return panel;
        }

        private static Control CreateMetricBlock(string title, string value)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 78, 56),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(8, 4, 0, 4),
                Padding = new Padding(10, 12, 10, 10)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = value
            });
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = HeroAccent,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = title
            });

            return panel;
        }

        private static Control CreateQuestionCard(StudentAttemptReviewQuestionDto question)
        {
            var selectedText = string.IsNullOrWhiteSpace(question.SelectedAnswerText)
                ? "No answer selected"
                : question.SelectedAnswerText.Trim();
            var correctText = string.IsNullOrWhiteSpace(question.CorrectAnswerText)
                ? "No correct answer configured"
                : question.CorrectAnswerText.Trim();
            var explanationText = string.IsNullOrWhiteSpace(question.Explanation)
                ? "No explanation was provided for this question."
                : question.Explanation.Trim();

            var card = new Panel
            {
                Width = 980,
                Height = 190,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16, 14, 16, 14)
            };

            var status = question.IsCorrect ? "Correct" : (question.SelectedChoiceId.HasValue ? "Incorrect" : "Unanswered");
            var statusBg = question.IsCorrect ? CorrectBg : WrongBg;
            var statusText = question.IsCorrect ? CorrectText : WrongText;

            var badge = new Label
            {
                Width = 112,
                Height = 28,
                Location = new Point(card.Width - 144, 14),
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                BackColor = statusBg,
                ForeColor = statusText,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = status,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var questionLabel = new Label
            {
                Location = new Point(16, 14),
                Size = new Size(790, 46),
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = string.Format("Q{0}. {1}", question.OrderIndex, question.QuestionText)
            };

            var selectedLabel = new Label
            {
                Location = new Point(16, 66),
                Size = new Size(460, 30),
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextMuted,
                Text = string.Format("Your answer: {0}", selectedText)
            };

            var correctLabel = new Label
            {
                Location = new Point(500, 66),
                Size = new Size(430, 30),
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextMuted,
                Text = string.Format("Correct answer: {0}", correctText)
            };

            var explanationTitle = new Label
            {
                Location = new Point(16, 102),
                Size = new Size(920, 22),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = "Explanation"
            };

            var explanationLabel = new Label
            {
                Location = new Point(16, 126),
                Size = new Size(920, 44),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                Text = explanationText
            };

            card.Controls.Add(badge);
            card.Controls.Add(questionLabel);
            card.Controls.Add(selectedLabel);
            card.Controls.Add(correctLabel);
            card.Controls.Add(explanationTitle);
            card.Controls.Add(explanationLabel);
            return card;
        }

        private static Button CreateActionButton(string text, Color backColor, Color foreColor)
        {
            var button = new Button
            {
                Width = 164,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = text,
                Cursor = Cursors.Hand,
                Margin = new Padding(12, 0, 0, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static string NullIfWhite(string value, string fallback)
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }
    }
}
