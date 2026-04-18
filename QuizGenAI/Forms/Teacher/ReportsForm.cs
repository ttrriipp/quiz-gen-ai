using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class ReportsForm : Form
    {
        private readonly ReportService _reportService = new ReportService();

        public ReportsForm()
        {
            InitializeComponent();
            BuildLayout();
        }

        public int CurrentUserId { get; set; }
        public string DisplayName { get; set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RenderReports();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(243, 244, 246);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1220, 780);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reports";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 168F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(BuildHeaderPanel(), 0, 0);
            root.Controls.Add(BuildMetricRow(), 0, 1);
            root.Controls.Add(BuildContentRow(), 0, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildHeaderPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(22, 18, 22, 18);

            var body = new Panel
            {
                Dock = DockStyle.Fill
            };

            body.Controls.Add(new Label
            {
                Name = "lblReportsMeta",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "Loading teacher report summary..."
            });

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Performance Reports"
            });

            panel.Controls.Add(body);
            return panel;
        }

        private Control BuildMetricRow()
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 18, 0, 18)
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            row.Controls.Add(CreateMetricCard("Average Score", "0.0%", "Across all submitted attempts."), 0, 0);
            row.Controls.Add(CreateMetricCard("Pass Count", "0", "Scores at or above 75%."), 1, 0);
            row.Controls.Add(CreateMetricCard("Fail Count", "0", "Scores below 75%."), 2, 0);
            return row;
        }

        private Control BuildContentRow()
        {
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));

            grid.Controls.Add(BuildPassFailChartPanel(), 0, 0);
            grid.Controls.Add(BuildSubjectChartPanel(), 1, 0);
            grid.Controls.Add(BuildHardestQuestionsPanel(), 0, 1);
            grid.Controls.Add(BuildRecentSubmissionsPanel(), 1, 1);
            return grid;
        }

        private Control BuildPassFailChartPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);

            panel.Controls.Add(new FlowLayoutPanel
            {
                Name = "flowPassFailOverview",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Pass / Fail Overview"
            });

            return panel;
        }

        private Control BuildSubjectChartPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);
            panel.Margin = new Padding(18, 0, 0, 0);

            panel.Controls.Add(new FlowLayoutPanel
            {
                Name = "flowSubjectPerformance",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Subject Mastery"
            });

            return panel;
        }

        private Control BuildHardestQuestionsPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);
            panel.Margin = new Padding(0, 18, 0, 0);

            panel.Controls.Add(new FlowLayoutPanel
            {
                Name = "flowHardestQuestions",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Hardest Questions"
            });

            return panel;
        }

        private Control BuildRecentSubmissionsPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);
            panel.Margin = new Padding(18, 18, 0, 0);

            panel.Controls.Add(new FlowLayoutPanel
            {
                Name = "flowRecentSubmissions",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Recent Submissions"
            });

            return panel;
        }

        private void RenderReports()
        {
            var reports = _reportService.GetTeacherReports();

            var lblMeta = Controls.Find("lblReportsMeta", true).FirstOrDefault() as Label;
            if (lblMeta != null)
            {
                var teacherName = string.IsNullOrWhiteSpace(DisplayName) ? "Teacher" : DisplayName;
                lblMeta.Text = string.Format("Signed in as {0}. Reports update from submitted attempts in the local SQLite database.", teacherName);
            }

            UpdateMetricCard("Average Score", string.Format("{0:0.#}%", reports.AverageScore), "Across all submitted attempts.");
            UpdateMetricCard("Pass Count", reports.PassCount.ToString(), "Scores at or above 75%.");
            UpdateMetricCard("Fail Count", reports.FailCount.ToString(), "Scores below 75%.");

            var passFailFlow = Controls.Find("flowPassFailOverview", true).FirstOrDefault() as FlowLayoutPanel;
            if (passFailFlow != null)
            {
                passFailFlow.Controls.Clear();
                var totalAttempts = reports.PassCount + reports.FailCount;
                if (totalAttempts == 0)
                {
                    passFailFlow.Controls.Add(CreateEmptyInfoCard("No submitted attempts yet. Pass/fail analytics will appear after quizzes are completed."));
                }
                else
                {
                    passFailFlow.Controls.Add(CreateProgressStatCard("Pass", reports.PassCount, totalAttempts, Color.FromArgb(15, 118, 110)));
                    passFailFlow.Controls.Add(CreateProgressStatCard("Fail", reports.FailCount, totalAttempts, Color.FromArgb(220, 38, 38)));
                }
            }

            var subjectFlow = Controls.Find("flowSubjectPerformance", true).FirstOrDefault() as FlowLayoutPanel;
            if (subjectFlow != null)
            {
                subjectFlow.Controls.Clear();
                if (reports.SubjectPerformance.Count == 0)
                {
                    subjectFlow.Controls.Add(CreateEmptyInfoCard("No graded subject data yet. Subject mastery appears after students submit attempts."));
                }
                else
                {
                    foreach (var item in reports.SubjectPerformance)
                    {
                        subjectFlow.Controls.Add(CreateSubjectPerformanceCard(item));
                    }
                }
            }

            var hardestFlow = Controls.Find("flowHardestQuestions", true).FirstOrDefault() as FlowLayoutPanel;
            if (hardestFlow != null)
            {
                hardestFlow.Controls.Clear();
                if (reports.HardestQuestions.Count == 0)
                {
                    hardestFlow.Controls.Add(CreateEmptyInfoCard("No graded questions yet. Hardest-question analytics will appear after students submit attempts."));
                }
                else
                {
                    foreach (var item in reports.HardestQuestions)
                    {
                        hardestFlow.Controls.Add(CreateHardQuestionCard(item));
                    }
                }
            }

            var recentFlow = Controls.Find("flowRecentSubmissions", true).FirstOrDefault() as FlowLayoutPanel;
            if (recentFlow != null)
            {
                recentFlow.Controls.Clear();
                if (reports.RecentSubmissions.Count == 0)
                {
                    recentFlow.Controls.Add(CreateEmptyInfoCard("No recent submissions yet. Submit a few student attempts to populate this report."));
                }
                else
                {
                    foreach (var item in reports.RecentSubmissions)
                    {
                        recentFlow.Controls.Add(CreateRecentSubmissionCard(item));
                    }
                }
            }
        }

        private void UpdateMetricCard(string title, string value, string note)
        {
            var valueLabel = Controls.Find("metric_value_" + title.Replace(" ", "_"), true).FirstOrDefault() as Label;
            var noteLabel = Controls.Find("metric_note_" + title.Replace(" ", "_"), true).FirstOrDefault() as Label;
            if (valueLabel != null)
            {
                valueLabel.Text = value;
            }

            if (noteLabel != null)
            {
                noteLabel.Text = note;
            }
        }

        private static Control CreateMetricCard(string title, string value, string note)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 18);
            panel.Margin = new Padding(0, 0, 18, 0);

            panel.Controls.Add(new Label
            {
                Name = "metric_note_" + title.Replace(" ", "_"),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = note
            });

            panel.Controls.Add(new Label
            {
                Name = "metric_value_" + title.Replace(" ", "_"),
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

        private static Control CreateHardQuestionCard(HardestQuestionDto item)
        {
            var panel = new Panel
            {
                Width = 520,
                Height = 110,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0}\r\nCorrect rate: {1:0.#}% | Attempts: {2}\r\n{3}", item.SubjectName, item.CorrectRate, item.Attempts, item.QuestionText)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = item.QuizTitle
            });

            return panel;
        }

        private static Control CreateRecentSubmissionCard(RecentSubmissionDto item)
        {
            var panel = new Panel
            {
                Width = 470,
                Height = 88,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0}\r\n{1} | {2:0.#}%", item.StudentName, item.SubmittedAtDisplay, item.ScorePercentage)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = item.QuizTitle
            });

            return panel;
        }

        private static Control CreateProgressStatCard(string title, int value, int total, Color accentColor)
        {
            var percent = total == 0 ? 0D : (value / (double)total) * 100D;

            var panel = new Panel
            {
                Width = 560,
                Height = 98,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };

            var barHost = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 16,
                BackColor = Color.FromArgb(226, 232, 240),
                Padding = new Padding(0),
                Margin = new Padding(0, 10, 0, 0)
            };

            var barWidth = (int)Math.Round(Math.Max(0D, Math.Min(1D, percent / 100D)) * 500D);
            barHost.Controls.Add(new Panel
            {
                Dock = DockStyle.Left,
                Width = Math.Max(8, barWidth),
                BackColor = accentColor
            });

            panel.Controls.Add(barHost);
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0} of {1} submitted attempts ({2:0.#}%).", value, total, percent)
            });
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = title
            });

            return panel;
        }

        private static Control CreateSubjectPerformanceCard(SubjectPerformanceDto item)
        {
            var panel = new Panel
            {
                Width = 500,
                Height = 86,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };

            var barHost = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 16,
                BackColor = Color.FromArgb(226, 232, 240),
                Margin = new Padding(0, 10, 0, 0)
            };

            var barWidth = (int)Math.Round(Math.Max(0D, Math.Min(1D, item.AverageScore / 100D)) * 440D);
            barHost.Controls.Add(new Panel
            {
                Dock = DockStyle.Left,
                Width = Math.Max(8, barWidth),
                BackColor = Color.FromArgb(37, 99, 235)
            });

            panel.Controls.Add(barHost);
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("Average score: {0:0.#}% across {1} attempt(s).", item.AverageScore, item.AttemptCount)
            });
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = item.SubjectName
            });

            return panel;
        }

        private static Control CreateEmptyInfoCard(string message)
        {
            var panel = new Panel
            {
                Width = 470,
                Height = 84,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = message
            });

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
    }
}
