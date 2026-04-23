using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;
using QuizGenAI.Services;
using ScottPlot.WinForms;
using ScottColor = ScottPlot.Color;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherDashboardForm : Form
    {
        private readonly Dictionary<string, Guna2Button> _navButtons = new Dictionary<string, Guna2Button>();
        private readonly ReportService _reportService = new ReportService();
        private readonly QuizService _quizService = new QuizService();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Panel _topBar;
        private Panel _contentHost;
        private Label _lblGreeting;
        private Control _hostedContentView;

        private static readonly Color MainWorkspaceGreen = Color.FromArgb(11, 48, 34);
        private static readonly Color DashboardCard = Color.FromArgb(18, 66, 50);
        private static readonly Color DashboardCardStrong = Color.FromArgb(28, 109, 77);
        private static readonly Color DashboardPlot = Color.FromArgb(12, 52, 39);
        private static readonly Color DashboardBorder = Color.FromArgb(72, 255, 255, 255);
        private static readonly Color DashboardText = Color.FromArgb(244, 248, 244);
        private static readonly Color DashboardMuted = Color.FromArgb(188, 208, 196);
        private static readonly Color DashboardAccent = Color.FromArgb(205, 224, 155);
        private static readonly Color DashboardMustard = Color.FromArgb(224, 190, 93);
        private static readonly Color DashboardDanger = Color.FromArgb(198, 59, 52);
        private string _displayName = "Teacher";
        public int CurrentUserId { get; set; }

        public TeacherDashboardForm()
        {
            InitializeComponent();
            if (DesignTimeHelper.IsInDesignMode(this))
            {
                return;
            }

            BuildShell();
            ShowSection("dashboard");
            AppTheme.ApplyCognitaTheme(this);
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = string.IsNullOrWhiteSpace(value) ? "Teacher" : value.Trim();
                if (_lblGreeting != null)
                {
                    _lblGreeting.Text = string.Format("Signed in as {0}", _displayName);
                }
            }
        }

        private void BuildShell()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = MainWorkspaceGreen;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ClientSize = new Size(1280, 800);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Teacher Dashboard";

            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 240,
                BackColor = Color.FromArgb(31, 41, 55)
            };

            var brandingPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(20, 24, 20, 16)
            };

            var lblBrand = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 38,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                Text = "QuizGen AI"
            };

            var lblRole = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = Color.FromArgb(156, 163, 175),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                Text = "Teacher / Admin Workspace"
            };

            brandingPanel.Controls.Add(lblRole);
            brandingPanel.Controls.Add(lblBrand);

            var navPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 230,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(16, 8, 16, 8)
            };

            navPanel.Controls.Add(CreateNavButton("dashboard", "Dashboard"));
            navPanel.Controls.Add(CreateNavButton("quizzes", "Quizzes"));
            navPanel.Controls.Add(CreateNavButton("reports", "Reports"));
            navPanel.Controls.Add(CreateNavButton("logs", "Logs"));

            var footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 132,
                Padding = new Padding(20, 12, 20, 16)
            };

            var btnLogout = new Guna2Button
            {
                Dock = DockStyle.Top,
                Height = 38,
                BorderRadius = 8,
                FillColor = Color.FromArgb(185, 28, 28),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Logout",
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 10)
            };
            btnLogout.Click += delegate
            {
                var result = MessageBox.Show(
                    "Do you want to logout and return to the login screen?",
                    "Confirm Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Close();
                }
            };

            _lblGreeting = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 48,
                ForeColor = Color.FromArgb(209, 213, 219),
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.BottomLeft,
                Text = string.Format("Signed in as {0}", _displayName)
            };

            footerPanel.Controls.Add(_lblGreeting);
            footerPanel.Controls.Add(btnLogout);

            sidebar.Controls.Add(footerPanel);
            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(brandingPanel);

            _topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = MainWorkspaceGreen,
                Padding = new Padding(28, 12, 28, 10)
            };

            _lblPageTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold)
            };

            _lblPageDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 10F)
            };

            _topBar.Controls.Add(_lblPageDescription);
            _topBar.Controls.Add(_lblPageTitle);

            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 16, 20, 20),
                BackColor = MainWorkspaceGreen
            };

            Controls.Add(_contentHost);
            Controls.Add(_topBar);
            Controls.Add(sidebar);
            ResumeLayout();
        }

        private Guna2Button CreateNavButton(string key, string text)
        {
            var button = new Guna2Button
            {
                Width = 208,
                Height = 46,
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.FromArgb(229, 231, 235),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = text,
                Tag = key,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 10)
            };
            button.BorderRadius = 10;
            button.FillColor = Color.FromArgb(31, 41, 55);
            button.HoverState.FillColor = Color.FromArgb(36, 52, 78);
            button.PressedColor = Color.FromArgb(24, 36, 56);
            button.TextAlign = HorizontalAlignment.Left;
            button.TextOffset = new Point(12, 0);
            button.Click += NavButton_Click;
            _navButtons[key] = button;
            return button;
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            var button = sender as Control;
            if (button == null)
            {
                return;
            }

            var sectionKey = Convert.ToString(button.Tag);
            ShowSection(sectionKey);
        }

        private void ShowSection(string sectionKey)
        {
            foreach (var pair in _navButtons)
            {
                var isActive = pair.Key == sectionKey;
                pair.Value.FillColor = isActive ? Color.FromArgb(14, 116, 144) : Color.FromArgb(31, 41, 55);
                pair.Value.ForeColor = isActive ? Color.White : Color.FromArgb(229, 231, 235);
            }

            switch (sectionKey)
            {
                case "dashboard":
                    _topBar.Visible = false;
                    _lblPageTitle.Text = "Dashboard";
                    _lblPageDescription.Text = "Teacher/admin overview with quick actions and project metrics.";
                    RenderDashboardView();
                    break;

                case "quizzes":
                    _topBar.Visible = true;
                    _lblPageTitle.Text = "Quizzes";
                    _lblPageDescription.Text = "Manage draft, published, and archived quizzes without leaving the teacher shell.";
                    RenderHostedForm(CreateHostedQuizManager());
                    break;

                case "reports":
                    _topBar.Visible = true;
                    _lblPageTitle.Text = "Reports";
                    _lblPageDescription.Text = "Review teacher analytics and reporting inside the same workspace.";
                    RenderHostedControl(CreateHostedReportsView());
                    break;

                case "logs":
                    _topBar.Visible = true;
                    _lblPageTitle.Text = "Logs";
                    _lblPageDescription.Text = "Structured Serilog output from the app runtime.";
                    RenderLogsView();
                    break;

                default:
                    _topBar.Visible = true;
                    _lblPageTitle.Text = "Logs";
                    _lblPageDescription.Text = "Structured Serilog output from the app runtime.";
                    RenderLogsView();
                    break;
            }
        }

        private void RenderDashboardView()
        {
            ClearHostedContentView();
            _contentHost.Controls.Clear();
            var dashboard = _reportService.GetTeacherDashboard();
            var reports = _reportService.GetTeacherReports();
            var quizzes = _quizService
                .GetQuizSummaries(null, null)
                .OrderByDescending(x => x.UpdatedAt)
                .Take(3)
                .ToList();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent
            };

            root.RowStyles.Clear();
            for (var i = 0; i < 5; i++)
            {
                root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            root.Controls.Add(CreateDashboardWelcomeHeader(), 0, 0);
            root.Controls.Add(CreateDashboardStatsRow(dashboard), 0, 1);
            root.Controls.Add(CreateDashboardAnalyticsRow(reports), 0, 2);
            root.Controls.Add(CreateDashboardPerformanceRow(dashboard), 0, 3);
            root.Controls.Add(CreateDashboardQuizzesSection(quizzes), 0, 4);
            _contentHost.Controls.Add(root);
        }

        private Control CreateDashboardWelcomeHeader()
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 2, 0, 12),
                BackColor = Color.Transparent
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188F));

            var textHost = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 88,
                BackColor = Color.Transparent
            };

            var lblSection = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold),
                ForeColor = DashboardMuted,
                Text = "TEACHER DASHBOARD"
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 46,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = string.Format("Welcome back, {0}", DisplayName)
            };

            var lblSubtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = DashboardMuted,
                Text = "Here is what is happening across your quizzes today."
            };

            textHost.Controls.Add(lblSection);
            textHost.Controls.Add(lblTitle);
            textHost.Controls.Add(lblSubtitle);

            var actionButton = new Guna2Button
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Width = 138,
                Height = 42,
                BorderRadius = 10,
                FillColor = Color.FromArgb(18, 95, 65),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Generate Quiz",
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 18, 0, 0)
            };
            actionButton.Click += delegate
            {
                ShowSection("quizzes");
            };

            row.Controls.Add(textHost, 0, 0);
            row.Controls.Add(actionButton, 1, 0);
            return row;
        }

        private Control CreateDashboardStatsRow(TeacherDashboardDto dashboard)
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 4,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Color.Transparent
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            row.Controls.Add(CreateDashboardStatCard("Total Quizzes", dashboard.TotalQuizzes.ToString(), "+" + dashboard.PublishedQuizzes + " published", false), 0, 0);
            row.Controls.Add(CreateDashboardStatCard("Published", dashboard.PublishedQuizzes.ToString(), dashboard.DraftQuizzes + " drafts waiting", false), 1, 0);
            row.Controls.Add(CreateDashboardStatCard("Active Students", dashboard.TotalStudents.ToString(), "+" + dashboard.RecentSubmissions.Count + " recent results", false), 2, 0);
            row.Controls.Add(CreateDashboardStatCard("Avg. Score", dashboard.AverageScore.ToString("0.#") + "%", dashboard.SubmittedAttempts + " submissions", true), 3, 0);
            return row;
        }

        private Control CreateDashboardAnalyticsRow(TeacherReportsDto reports)
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 16),
                BackColor = Color.Transparent
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            row.Controls.Add(CreateTrendChartCard(reports), 0, 0);
            row.Controls.Add(CreatePassFailCard(reports), 1, 0);
            return row;
        }

        private Control CreateDashboardPerformanceRow(TeacherDashboardDto dashboard)
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 18),
                BackColor = Color.Transparent
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            row.Controls.Add(CreateSubjectPerformanceCard(dashboard), 0, 0);
            row.Controls.Add(CreateRecentResultsCard(dashboard), 1, 0);
            return row;
        }

        private Control CreateDashboardQuizzesSection(List<QuizListItemDto> quizzes)
        {
            var section = new Panel
            {
                Dock = DockStyle.Top,
                Height = 214,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            var header = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 34,
                ColumnCount = 1,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = "Your quizzes",
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 10, 0, 0),
                BackColor = Color.Transparent
            };
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            for (var i = 0; i < 3; i++)
            {
                var quiz = i < quizzes.Count ? quizzes[i] : null;
                cards.Controls.Add(CreateQuizPreviewCard(quiz, i), i, 0);
            }

            section.Controls.Add(cards);
            section.Controls.Add(header);
            return section;
        }

        private Panel CreateDashboardStatCard(string title, string value, string note, bool highlighted)
        {
            var panel = CreateDashboardCardPanel();
            panel.Height = 92;
            panel.Margin = new Padding(0, 0, 16, 0);
            panel.Padding = new Padding(18, 14, 18, 14);
            panel.BackColor = highlighted ? DashboardCardStrong : DashboardCard;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = highlighted ? Color.FromArgb(232, 242, 232) : DashboardMuted,
                Text = title.ToUpperInvariant()
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = value
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = highlighted ? Color.FromArgb(228, 238, 228) : DashboardMuted,
                Text = note
            });

            return panel;
        }

        private Panel CreateTrendChartCard(TeacherReportsDto reports)
        {
            var panel = CreateDashboardCardPanel();
            panel.Height = 228;
            panel.Padding = new Padding(18, 14, 18, 16);
            panel.Margin = new Padding(0, 0, 16, 0);

            var chart = new FormsPlot
            {
                Dock = DockStyle.Fill,
                BackColor = DashboardPlot
            };

            ConfigureMiniPlot(chart.Plot);
            var xs = Enumerable.Range(0, reports.ScoreTrendByMonth.Count).Select(i => (double)i).ToArray();
            var ys = reports.ScoreTrendByMonth.Select(x => x.AverageScore ?? 0D).ToArray();
            var labels = reports.ScoreTrendByMonth.Select(x => x.MonthLabel).ToArray();
            if (xs.Length > 0)
            {
                var scatter = chart.Plot.Add.ScatterLine(xs, ys);
                scatter.Color = ScottColor.FromColor(DashboardAccent);
                scatter.LineWidth = 2.5F;
                chart.Plot.Add.ScatterPoints(xs, ys).Color = ScottColor.FromColor(DashboardAccent);
                chart.Plot.Axes.Bottom.SetTicks(xs, labels);
                chart.Plot.Axes.Left.Label.Text = "%";
                chart.Plot.Axes.Margins(0.06, 0.08, 0.1, 0.12);
            }

            panel.Controls.Add(chart);
            panel.Controls.Add(CreateDashboardCardHeader("Class performance trend"));
            return panel;
        }

        private Panel CreatePassFailCard(TeacherReportsDto reports)
        {
            var panel = CreateDashboardCardPanel();
            panel.Height = 228;
            panel.Padding = new Padding(14, 14, 14, 14);

            var chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = DashboardCard,
                Palette = ChartColorPalette.None
            };

            var area = new ChartArea("donut")
            {
                BackColor = DashboardCard
            };
            area.AxisX.Enabled = AxisEnabled.False;
            area.AxisY.Enabled = AxisEnabled.False;
            chart.ChartAreas.Add(area);

            var series = new Series("PassFail")
            {
                ChartType = SeriesChartType.Doughnut,
                ChartArea = "donut",
                IsValueShownAsLabel = false
            };
            series["DoughnutRadius"] = "72";
            series.Points.AddXY("Pass", Math.Max(0, reports.PassCount));
            series.Points.AddXY("Fail", Math.Max(0, reports.FailCount));
            series.Points[0].Color = Color.FromArgb(24, 105, 72);
            series.Points[1].Color = DashboardDanger;
            chart.Series.Add(series);
            chart.Legends.Clear();

            var legend = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            legend.Controls.Add(CreateLegendChip(Color.FromArgb(24, 105, 72), string.Format("Pass ({0})", reports.PassCount)));
            legend.Controls.Add(CreateLegendChip(DashboardDanger, string.Format("Fail ({0})", reports.FailCount)));

            panel.Controls.Add(legend);
            panel.Controls.Add(chart);
            panel.Controls.Add(CreateDashboardCardHeader("Pass vs fail"));
            return panel;
        }

        private Panel CreateSubjectPerformanceCard(TeacherDashboardDto dashboard)
        {
            var panel = CreateDashboardCardPanel();
            panel.Height = 228;
            panel.Padding = new Padding(18, 14, 18, 16);
            panel.Margin = new Padding(0, 0, 16, 0);

            var chart = new FormsPlot
            {
                Dock = DockStyle.Fill,
                BackColor = DashboardPlot
            };
            ConfigureMiniPlot(chart.Plot);
            var subjects = dashboard.SubjectPerformance.OrderByDescending(x => x.AverageScore).ToList();
            var xs = Enumerable.Range(0, subjects.Count).Select(i => (double)i).ToArray();
            var ys = subjects.Select(x => x.AverageScore).ToArray();
            if (subjects.Count > 0)
            {
                var bars = chart.Plot.Add.Bars(xs, ys);
                foreach (var bar in bars.Bars)
                {
                    bar.FillColor = ScottColor.FromColor(Color.FromArgb(24, 92, 67));
                    bar.LineColor = ScottColor.FromColor(DashboardAccent);
                    bar.LineWidth = 1;
                }

                chart.Plot.Axes.Bottom.SetTicks(xs, subjects.Select(x => x.SubjectName).ToArray());
                chart.Plot.Axes.Left.Label.Text = "%";
                chart.Plot.Axes.Margins(0.08, 0.08, 0, 0.12);
            }

            panel.Controls.Add(chart);
            panel.Controls.Add(CreateDashboardCardHeader("Average score by subject"));
            return panel;
        }

        private Panel CreateRecentResultsCard(TeacherDashboardDto dashboard)
        {
            var panel = CreateDashboardCardPanel();
            panel.Height = 228;
            panel.Padding = new Padding(16, 14, 16, 12);

            var list = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent
            };

            foreach (var item in dashboard.RecentSubmissions.Take(5))
            {
                list.Controls.Add(CreateRecentResultRow(item));
            }

            if (list.Controls.Count == 0)
            {
                list.Controls.Add(new Label
                {
                    AutoSize = false,
                    Width = 280,
                    Height = 28,
                    Font = new Font("Segoe UI", 9.5F),
                    ForeColor = DashboardMuted,
                    Text = "No recent results yet."
                });
            }

            panel.Controls.Add(list);
            panel.Controls.Add(CreateDashboardCardHeader("Recent results"));
            return panel;
        }

        private Control CreateRecentResultRow(RecentSubmissionDto item)
        {
            var row = new Panel
            {
                Width = 320,
                Height = 36,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.Transparent
            };

            row.Controls.Add(CreateScorePill(item.ScorePercentage));
            row.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashboardMuted,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = string.Format("{0}\n{1}", item.StudentName, item.QuizTitle)
            });

            return row;
        }

        private Control CreateScorePill(double score)
        {
            var pill = new Panel
            {
                Dock = DockStyle.Right,
                Width = 54,
                BackColor = score >= 75 ? Color.FromArgb(24, 105, 72) : (score >= 50 ? DashboardMustard : DashboardDanger),
                Margin = new Padding(10, 0, 0, 0)
            };
            pill.Paint += DashboardMiniPanel_Paint;
            pill.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = score.ToString("0.#") + "%"
            });
            return pill;
        }

        private Control CreateQuizPreviewCard(QuizListItemDto quiz, int index)
        {
            var card = CreateDashboardCardPanel();
            card.Height = 122;
            card.Margin = new Padding(index == 2 ? 0 : 0, 0, index == 2 ? 0 : 16, 0);
            card.Padding = new Padding(16, 12, 16, 12);

            if (quiz == null)
            {
                card.Controls.Add(new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = DashboardMuted,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Text = "No quiz available"
                });
                return card;
            }

            var badges = new Panel
            {
                Dock = DockStyle.Top,
                Height = 24,
                BackColor = Color.Transparent
            };
            badges.Controls.Add(CreateBadge(quiz.Status.ToString(), DockStyle.Right, GetStatusColor(quiz.Status)));
            badges.Controls.Add(CreateBadge(quiz.SubjectName, DockStyle.Left, Color.FromArgb(72, 132, 191, 88)));

            card.Controls.Add(new Label
            {
                Dock = DockStyle.Bottom,
                Height = 18,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = DashboardMuted,
                Text = string.Format("{0} questions                        {1}", quiz.QuestionCount, quiz.Difficulty)
            });
            card.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashboardMuted,
                Text = quiz.Topic
            });
            card.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = quiz.Title
            });
            card.Controls.Add(badges);

            return card;
        }

        private static Panel CreateBadge(string text, DockStyle dock, Color color)
        {
            var host = new Panel
            {
                Dock = dock,
                Width = Math.Max(72, text.Length * 7 + 18),
                BackColor = color,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 8, 0)
            };
            host.Paint += DashboardMiniPanel_Paint;
            host.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text
            });
            return host;
        }

        private static Color GetStatusColor(QuizStatus status)
        {
            switch (status)
            {
                case QuizStatus.Published:
                    return Color.FromArgb(24, 105, 72);
                case QuizStatus.Draft:
                    return Color.FromArgb(145, 108, 35);
                default:
                    return Color.FromArgb(92, 104, 119);
            }
        }

        private static Panel CreateDashboardCardHeader(string title)
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.Transparent
            };

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = title,
                TextAlign = ContentAlignment.MiddleLeft
            });

            return header;
        }

        private static Panel CreateDashboardCardPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = DashboardCard,
                Margin = new Padding(0)
            };
            panel.Paint += DashboardCardPanel_Paint;
            return panel;
        }

        private static void ConfigureMiniPlot(ScottPlot.Plot plot)
        {
            plot.Clear();
            plot.FigureBackground.Color = ScottColor.FromColor(DashboardCard);
            plot.DataBackground.Color = ScottColor.FromColor(DashboardPlot);
            plot.Axes.Color(ScottColor.FromColor(DashboardMuted));
            plot.Axes.FrameColor(ScottColor.FromColor(DashboardBorder));
            plot.Axes.DefaultGrid.MajorLineColor = ScottColor.FromColor(Color.FromArgb(42, 255, 255, 255));
            plot.Axes.DefaultGrid.MinorLineColor = ScottColor.FromColor(Color.FromArgb(20, 255, 255, 255));
            plot.HideLegend();
        }

        private static Control CreateLegendChip(Color color, string text)
        {
            var host = new FlowLayoutPanel
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 14, 0),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            host.Controls.Add(new Panel
            {
                Width = 10,
                Height = 10,
                BackColor = color,
                Margin = new Padding(0, 9, 6, 0)
            });

            host.Controls.Add(new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = DashboardMuted,
                Text = text,
                Margin = new Padding(0, 5, 0, 0)
            });

            return host;
        }

        private static void DashboardCardPanel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
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
                using (var pen = new Pen(DashboardBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static void DashboardMiniPanel_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (var pen = new Pen(Color.FromArgb(34, 255, 255, 255), 1))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private void RenderPlaceholderView(string title, string description, string[] bulletPoints)
        {
            ClearHostedContentView();
            _contentHost.Controls.Clear();
            _contentHost.Controls.Add(CreateInsightPanel(title, description, bulletPoints, DockStyle.Top, 420));
        }

        private void RenderLogsView(string selectedLogFile = null)
        {
            ClearHostedContentView();
            _contentHost.Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "QuizGenAI",
                "Logs");

            var logFiles = Directory.Exists(logDirectory)
                ? Directory.GetFiles(logDirectory, "*.log")
                    .OrderByDescending(File.GetLastWriteTimeUtc)
                    .ToList()
                : new List<string>();
            var activeLogFile = !string.IsNullOrWhiteSpace(selectedLogFile) && logFiles.Contains(selectedLogFile)
                ? selectedLogFile
                : logFiles.FirstOrDefault();

            var summaryPanel = CreateDashboardCardPanel();
            summaryPanel.Dock = DockStyle.Fill;
            summaryPanel.Padding = new Padding(24, 18, 24, 18);

            var summaryLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 148F));

            var btnRefresh = new Guna2Button
            {
                Anchor = AnchorStyles.Right,
                Width = 122,
                Height = 38,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Refresh",
                Cursor = Cursors.Hand
            };
            btnRefresh.BorderRadius = 10;
            btnRefresh.FillColor = Color.FromArgb(15, 118, 110);
            btnRefresh.Click += delegate { RenderLogsView(); };

            var summaryTextHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var lblSummaryTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = "Runtime Logs"
            };

            var lblSummary = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = DashboardMuted,
                TextAlign = ContentAlignment.TopLeft,
                Text = logFiles.Count == 0
                    ? "No log files found yet. Trigger a few actions, then refresh this view."
                    : string.Format(
                        "{0} log file(s) found in {1}. Select a file to inspect recent entries.",
                        logFiles.Count,
                        "LocalAppData\\QuizGenAI\\Logs")
            };
            summaryTextHost.Controls.Add(lblSummary);
            summaryTextHost.Controls.Add(lblSummaryTitle);

            var metricsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            metricsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            metricsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            metricsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            metricsPanel.Controls.Add(CreateLogMetricCard(
                "Log Files",
                logFiles.Count.ToString(),
                logFiles.Count == 0 ? "Waiting for the first entry" : "Files available to inspect"), 0, 0);
            metricsPanel.Controls.Add(CreateLogMetricCard(
                "Latest Update",
                activeLogFile == null ? "N/A" : File.GetLastWriteTime(activeLogFile).ToString("MMM dd"),
                activeLogFile == null ? "No active file selected" : File.GetLastWriteTime(activeLogFile).ToString("hh:mm tt")), 1, 0);
            metricsPanel.Controls.Add(CreateLogMetricCard(
                "Storage",
                FormatFileSize(logFiles.Aggregate(0L, (total, file) => total + GetFileSizeSafe(file))),
                activeLogFile == null ? "No active file selected" : Path.GetFileName(activeLogFile)), 2, 0);

            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            var filesPanel = CreateDashboardCardPanel();
            filesPanel.Dock = DockStyle.Fill;
            filesPanel.Padding = new Padding(16);

            var filesLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            filesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            filesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            filesLayout.Controls.Add(CreateLogSectionHeader(
                "Available Files",
                logFiles.Count == 0 ? "No log files discovered yet." : "Newest files appear first."), 0, 0);

            var filesList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };

            if (logFiles.Count == 0)
            {
                filesList.Controls.Add(CreateEmptyStateLabel("No logs yet. Run a few teacher or student actions, then refresh."));
            }
            else
            {
                foreach (var file in logFiles)
                {
                    filesList.Controls.Add(CreateLogFileCard(file, file == activeLogFile));
                }
            }

            filesLayout.Controls.Add(filesList, 0, 1);
            filesPanel.Controls.Add(filesLayout);

            var viewerPanel = CreateDashboardCardPanel();
            viewerPanel.Dock = DockStyle.Fill;
            viewerPanel.Padding = new Padding(0);

            var viewerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            viewerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            viewerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            viewerLayout.Controls.Add(CreateLogPreviewHeader(activeLogFile), 0, 0);

            var txtLogs = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(6, 24, 17),
                ForeColor = Color.FromArgb(231, 245, 236),
                Font = new Font("Consolas", 10F),
                Margin = new Padding(18, 0, 18, 18),
                Text = BuildLogPreview(activeLogFile)
            };
            viewerLayout.Controls.Add(txtLogs, 0, 1);
            viewerPanel.Controls.Add(viewerLayout);

            contentLayout.Controls.Add(filesPanel, 0, 0);
            contentLayout.Controls.Add(viewerPanel, 1, 0);

            summaryLayout.Controls.Add(summaryTextHost, 0, 0);
            summaryLayout.Controls.Add(btnRefresh, 1, 0);
            summaryPanel.Controls.Add(summaryLayout);

            root.Controls.Add(summaryPanel, 0, 0);
            root.Controls.Add(metricsPanel, 0, 1);
            root.Controls.Add(contentLayout, 0, 2);
            _contentHost.Controls.Add(root);
        }

        private void RenderHostedForm(Form form)
        {
            ClearHostedContentView();
            _contentHost.Controls.Clear();

            _hostedContentView = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.StartPosition = FormStartPosition.Manual;
            form.Padding = new Padding(0);

            _contentHost.Controls.Add(_hostedContentView);
            form.Show();
            _contentHost.PerformLayout();
            _hostedContentView.PerformLayout();
        }

        private void RenderHostedControl(Control control)
        {
            ClearHostedContentView();
            _contentHost.Controls.Clear();

            _hostedContentView = control;
            _hostedContentView.Dock = DockStyle.Fill;
            _hostedContentView.Margin = new Padding(0);
            _hostedContentView.Padding = new Padding(0);

            _contentHost.Controls.Add(_hostedContentView);
            _contentHost.PerformLayout();
            _hostedContentView.PerformLayout();
        }

        private void ClearHostedContentView()
        {
            if (_hostedContentView == null)
            {
                return;
            }

            if (_contentHost.Controls.Contains(_hostedContentView))
            {
                _contentHost.Controls.Remove(_hostedContentView);
            }

            _hostedContentView.Dispose();
            _hostedContentView = null;
        }

        private Panel CreateHeroPanel(string title, string description, string buttonText)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(26, 22, 26, 22);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));

            var actionButton = new Guna2Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Width = 144,
                Height = 42,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = buttonText,
                Cursor = Cursors.Hand,
                Margin = new Padding(8, 6, 0, 0)
            };
            actionButton.BorderRadius = 10;
            actionButton.FillColor = Color.FromArgb(15, 118, 110);

            var textHost = new Panel
            {
                Dock = DockStyle.Fill
            };

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI Semibold", 23F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = title
            };

            var lblDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 64,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = description
            };

            textHost.Controls.Add(lblDescription);
            textHost.Controls.Add(lblTitle);
            layout.Controls.Add(textHost, 0, 0);
            layout.Controls.Add(actionButton, 1, 0);
            panel.Controls.Add(layout);
            return panel;
        }

        private Panel CreateMetricCard(string title, string value, string note)
        {
            var panel = CreateSurfacePanel();
            panel.Margin = new Padding(0, 0, 18, 0);
            panel.Padding = new Padding(22, 20, 22, 20);

            var lblValue = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 52,
                Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = value
            };

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = title
            };

            var lblNote = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Text = note
            };

            panel.Controls.Add(lblNote);
            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateInsightPanel(string title, string description, string[] bulletPoints, DockStyle dock = DockStyle.Fill, int height = 0)
        {
            var panel = CreateSurfacePanel();
            panel.Dock = dock;
            if (height > 0)
            {
                panel.Height = height;
            }

            panel.Padding = new Padding(24, 22, 24, 22);

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = title
            };

            var lblDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = description
            };

            var bullets = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = BuildBulletList(bulletPoints)
            };

            panel.Controls.Add(bullets);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateSurfacePanel()
        {
            return new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static string BuildBulletList(IEnumerable<string> bulletPoints)
        {
            return "• " + string.Join(Environment.NewLine + "• ", bulletPoints);
        }

        private Control CreateLogFileCard(string filePath, bool isSelected)
        {
            var card = new Panel
            {
                Width = 270,
                Height = 82,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = isSelected ? DashboardCardStrong : Color.FromArgb(17, 76, 55),
                Cursor = Cursors.Hand,
                Padding = new Padding(18, 14, 18, 12)
            };
            card.Paint += DashboardCardPanel_Paint;

            var lblMeta = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = isSelected ? Color.FromArgb(226, 238, 231) : DashboardMuted,
                Text = string.Format(
                    "{0}  •  {1}",
                    File.GetLastWriteTime(filePath).ToString("MMM dd, yyyy hh:mm tt"),
                    FormatFileSize(GetFileSizeSafe(filePath)))
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = Path.GetFileName(filePath)
            };

            EventHandler openSelectedLog = delegate { RenderLogsView(filePath); };
            card.Click += openSelectedLog;
            lblTitle.Click += openSelectedLog;
            lblMeta.Click += openSelectedLog;

            card.Controls.Add(lblMeta);
            card.Controls.Add(lblTitle);
            return card;
        }

        private Panel CreateLogMetricCard(string title, string value, string note)
        {
            var panel = CreateDashboardCardPanel();
            panel.Margin = new Padding(0, 0, 16, 0);
            panel.Padding = new Padding(18, 16, 18, 16);

            var lblNote = new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = DashboardMuted,
                Text = note
            };

            var lblValue = new Label
            {
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = value
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = DashboardAccent,
                Text = title.ToUpperInvariant()
            };

            panel.Controls.Add(lblNote);
            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private static Panel CreateLogSectionHeader(string title, string description)
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var lblDescription = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashboardMuted,
                Text = description
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = title
            };

            host.Controls.Add(lblDescription);
            host.Controls.Add(lblTitle);
            return host;
        }

        private static Control CreateEmptyStateLabel(string text)
        {
            return new Label
            {
                AutoSize = false,
                Width = 270,
                Height = 72,
                Margin = new Padding(0, 0, 0, 12),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = DashboardCard,
                Padding = new Padding(14, 16, 14, 14),
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashboardMuted,
                Text = text
            };
        }

        private static Panel CreateLogPreviewHeader(string activeLogFile)
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(18, 16, 18, 10)
            };

            var lblMeta = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = DashboardMuted,
                Text = activeLogFile == null
                    ? "Select a log file once entries are available."
                    : string.Format(
                        "Last modified {0}  •  {1}",
                        File.GetLastWriteTime(activeLogFile).ToString("MMM dd, yyyy hh:mm tt"),
                        FormatFileSize(GetFileSizeSafe(activeLogFile)))
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = DashboardText,
                Text = activeLogFile == null ? "Log Preview" : Path.GetFileName(activeLogFile)
            };

            host.Controls.Add(lblMeta);
            host.Controls.Add(lblTitle);
            return host;
        }

        private static long GetFileSizeSafe(string filePath)
        {
            try
            {
                return string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)
                    ? 0L
                    : new FileInfo(filePath).Length;
            }
            catch
            {
                return 0L;
            }
        }

        private static string FormatFileSize(long bytes)
        {
            if (bytes <= 0)
            {
                return "0 B";
            }

            string[] suffixes = { "B", "KB", "MB", "GB" };
            var index = 0;
            double size = bytes;

            while (size >= 1024 && index < suffixes.Length - 1)
            {
                size /= 1024;
                index++;
            }

            return string.Format("{0:0.#} {1}", size, suffixes[index]);
        }

        private static string BuildLogPreview(string logFile)
        {
            if (string.IsNullOrWhiteSpace(logFile) || !File.Exists(logFile))
            {
                return "No logs are available yet.";
            }

            try
            {
                string[] lines;

                using (var stream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    lines = content
                        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                }

                var tail = lines.Skip(Math.Max(0, lines.Length - 320));
                return string.Join(Environment.NewLine, tail);
            }
            catch (Exception ex)
            {
                return "Unable to read the latest log file." + Environment.NewLine + Environment.NewLine + ex.Message;
            }
        }

        private Control CreateRecentSubmissionsPanel(TeacherDashboardDto dashboard)
        {
            var bullets = new List<string>();

            foreach (var item in dashboard.RecentSubmissions)
            {
                bullets.Add(string.Format("{0}: {1} scored {2:0.0}% on {3}", item.StudentName, item.QuizTitle, item.ScorePercentage, item.SubmittedAtDisplay));
            }

            if (bullets.Count == 0)
            {
                bullets.Add("No submitted attempts yet.");
            }

            if (dashboard.SubjectPerformance.Count > 0)
            {
                bullets.Add("Top subject averages:");
                foreach (var subject in dashboard.SubjectPerformance)
                {
                    bullets.Add(string.Format("{0}: {1:0.0}%", subject.SubjectName, subject.AverageScore));
                }
            }

            return CreateInsightPanel(
                "Recent Activity",
                "Latest student submissions and subject performance snapshots.",
                bullets.ToArray());
        }

        private Form CreateHostedQuizManager()
        {
            return new TeacherQuizzesForm
            {
                CurrentUserId = CurrentUserId,
                DisplayName = DisplayName,
                TopLevel = false
            };
        }

        private Control CreateHostedReportsView()
        {
            return new ReportsForm
            {
                CurrentUserId = CurrentUserId,
                DisplayName = DisplayName
            };
        }
    }
}
