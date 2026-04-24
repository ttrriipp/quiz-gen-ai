using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;

namespace QuizGenAI.Forms.Student
{
    public class StudentAttemptReviewForm : Form
    {
        private readonly StudentAttemptReviewDto _review;

        public StudentAttemptReviewForm(StudentAttemptReviewDto review)
        {
            if (review == null)
            {
                throw new ArgumentNullException("review");
            }

            _review = review;
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
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

            root.Controls.Add(BuildHeaderPanel(), 0, 0);
            root.Controls.Add(BuildQuestionsPanel(), 0, 1);
            root.Controls.Add(BuildFooterPanel(), 0, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildHeaderPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 16, 20, 16);
            panel.Margin = new Padding(0, 0, 0, 12);

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = _review.QuizTitle
            };

            var lblMeta = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0} | Score: {1:0.#}% | Submitted: {2}", _review.SubjectName, _review.ScorePercentage, _review.SubmittedAtDisplay)
            };

            var lblTopic = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = string.IsNullOrWhiteSpace(_review.Topic) ? "Topic: N/A" : string.Format("Topic: {0}", _review.Topic)
            };

            panel.Controls.Add(lblTopic);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Control BuildQuestionsPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(16, 12, 16, 12);
            panel.Margin = new Padding(0, 0, 0, 12);

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            if (_review.Questions.Count == 0)
            {
                content.Controls.Add(new Label
                {
                    Width = 840,
                    Height = 48,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(100, 116, 139),
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
            return panel;
        }

        private Control BuildFooterPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var btnClose = new Button
            {
                Width = 140,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Close",
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Location = new Point(Math.Max(0, panel.Width - btnClose.Width), 8);
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.OK;
                Close();
            };
            panel.Resize += delegate
            {
                btnClose.Location = new Point(Math.Max(0, panel.ClientSize.Width - btnClose.Width), 8);
            };

            panel.Controls.Add(btnClose);
            return panel;
        }

        private static Panel CreateQuestionCard(StudentAttemptReviewQuestionDto question)
        {
            var explanationText = string.IsNullOrWhiteSpace(question.Explanation)
                ? "No explanation provided for this question."
                : question.Explanation.Trim();
            var answerText = string.IsNullOrWhiteSpace(question.CorrectAnswerText)
                ? "No correct answer configured"
                : question.CorrectAnswerText.Trim();

            var card = new Panel
            {
                Width = 960,
                Height = 236,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(14, 12, 14, 12)
            };

            var lblQuestion = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = string.Format("Q{0}. {1}", question.OrderIndex, question.QuestionText)
            };

            var lblAnswer = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                TextAlign = ContentAlignment.MiddleRight,
                Text = string.Format("Answer: {0}", answerText)
            };

            var explanationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 6, 0, 0)
            };

            var lblExplanationTitle = new Label
            {
                AutoSize = false,
                Width = 900,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Explanation"
            };
            explanationPanel.Controls.Add(lblExplanationTitle);

            var lblExplanation = new Label
            {
                AutoSize = false,
                Width = 900,
                Height = 130,
                Font = new Font("Segoe UI", 9.8F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = explanationText
            };
            explanationPanel.Controls.Add(lblExplanation);

            card.Controls.Add(explanationPanel);
            card.Controls.Add(lblAnswer);
            card.Controls.Add(lblQuestion);
            return card;
        }

        private static Panel CreateSurfacePanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0)
            };
        }
    }
}
