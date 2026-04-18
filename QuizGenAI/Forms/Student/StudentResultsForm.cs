using System;
using System.Drawing;
using System.Windows.Forms;
using QuizGenAI.DTOs;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentResultsForm : Form
    {
        private readonly StudentAttemptSummaryDto _summary;

        public StudentResultsForm(StudentAttemptSummaryDto summary)
        {
            if (summary == null)
            {
                throw new ArgumentNullException("summary");
            }

            _summary = summary;
            InitializeComponent();
            BuildLayout();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(243, 244, 246);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(900, 640);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Result Summary";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(BuildHeaderPanel(), 0, 0);
            root.Controls.Add(BuildMetricRow(), 0, 1);
            root.Controls.Add(BuildDetailsPanel(), 0, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildHeaderPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(22, 18, 22, 18);

            var btnClose = new Button
            {
                Dock = DockStyle.Right,
                Width = 138,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(3, 105, 161),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "View All Results",
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += delegate
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            var body = new Panel
            {
                Dock = DockStyle.Fill
            };

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0} | {1}", _summary.SubjectName, _summary.Topic)
            });

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI Semibold", 23F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = _summary.QuizTitle
            });

            panel.Controls.Add(btnClose);
            panel.Controls.Add(body);
            return panel;
        }

        private Control BuildMetricRow()
        {
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 16, 0, 16)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            grid.Controls.Add(CreateMetricCard("Score", string.Format("{0:0.#}%", _summary.ScorePercentage), "Computed immediately after submission."), 0, 0);
            grid.Controls.Add(CreateMetricCard("Correct Answers", _summary.CorrectAnswers.ToString(), string.Format("Wrong: {0} | Unanswered: {1}", _summary.WrongAnswers, _summary.UnansweredQuestions)), 1, 0);
            grid.Controls.Add(CreateMetricCard("Time Spent", _summary.TimeSpentDisplay, _summary.SubmittedAtDisplay), 2, 0);

            return grid;
        }

        private Control BuildDetailsPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(22, 20, 22, 20);

            var lblBody = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = string.Format(
                    "Student: {0}\r\nQuestions answered: {1} of {2}\r\nCorrect: {3}\r\nWrong: {4}\r\nUnanswered: {5}\r\nSubmission time: {6}",
                    _summary.StudentName,
                    _summary.AnsweredQuestions,
                    _summary.TotalQuestions,
                    _summary.CorrectAnswers,
                    _summary.WrongAnswers,
                    _summary.UnansweredQuestions,
                    _summary.SubmittedAtDisplay)
            };

            panel.Controls.Add(lblBody);
            return panel;
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

        private static Control CreateMetricCard(string title, string value, string note)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 18);
            panel.Margin = new Padding(0, 0, 18, 0);

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = note
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = value
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = title
            });

            return panel;
        }
    }
}
