using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ScottPlot.WinForms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;
using QuizGenAI.Services;
using ScottColor = ScottPlot.Color;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentQuizzesForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private const int EmSetCueBanner = 0x1501;
        private static readonly Color ReportsWorkspaceBg = Color.FromArgb(11, 48, 34);
        private static readonly Color ReportsCardBg = Color.FromArgb(16, 58, 44);
        private static readonly Color ReportsInnerBg = Color.FromArgb(12, 46, 36);
        private static readonly Color ReportsBorder = Color.FromArgb(72, 255, 255, 255);
        private static readonly Color ReportsTextPrimary = Color.White;
        private static readonly Color ReportsTextMuted = Color.FromArgb(196, 210, 200);
        private static readonly Color ReportsAccent = Color.FromArgb(168, 230, 198);
        private static readonly Color ReportsMustard = Color.FromArgb(212, 175, 80);
        private static readonly Color ReportsMustardBorder = Color.FromArgb(235, 205, 130);

        private readonly Dictionary<string, Guna2Button> _navButtons = new Dictionary<string, Guna2Button>();
        private readonly ExamService _examService = new ExamService();
        private readonly QuizService _quizService = new QuizService();
        private readonly RecommendationService _recommendationService = new RecommendationService();
        private readonly ReportService _reportService = new ReportService();
        private List<StudentQuizListItemDto> _studentQuizzes = new List<StudentQuizListItemDto>();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Label _lblQuizSearchSummary;
        private Panel _contentHost;
        private FlowLayoutPanel _quizCardsHost;
        private TextBox _txtQuizSearch;
        private Panel _topBar;
        private Label _lblGreeting;
        private string _displayName = "Student";
        private int _currentUserId;
        private string _activeSection = "quizzes";
        private string _quizSearchTerm = string.Empty;

        public StudentQuizzesForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            if (DesignTimeHelper.IsInDesignMode(this))
            {
                return;
            }

            BuildShell();
            ShowSection("quizzes");
            AppTheme.ApplyCognitaTheme(this);
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = string.IsNullOrWhiteSpace(value) ? "Student" : value.Trim();
                if (_lblGreeting != null)
                {
                    _lblGreeting.Text = string.Format("Signed in as {0}", _displayName);
                }
            }
        }

        public int CurrentUserId
        {
            get { return _currentUserId; }
            set
            {
                _currentUserId = value;
                if (_contentHost != null && _activeSection == "quizzes")
                {
                    RenderQuizLandingView();
                }
            }
        }

        private void BuildShell()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(244, 246, 248);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Student Workspace";

            var sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(15, 23, 42)
            };

            var branding = new Panel
            {
                Dock = DockStyle.Top,
                Height = 112,
                Padding = new Padding(20, 22, 20, 18)
            };

            var lblBrand = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 19F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "QuizGen AI"
            };

            var lblRole = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Text = "Student Workspace"
            };

            branding.Controls.Add(lblRole);
            branding.Controls.Add(lblBrand);

            var navPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 142,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(16, 12, 16, 8)
            };

            navPanel.Controls.Add(CreateNavButton("quizzes", "Quizzes"));
            navPanel.Controls.Add(CreateNavButton("results", "Results"));

            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 126,
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
                Dock = DockStyle.Bottom,
                Height = 44,
                ForeColor = Color.FromArgb(203, 213, 225),
                Font = new Font("Segoe UI", 10F),
                Text = string.Format("Signed in as {0}", _displayName),
                TextAlign = ContentAlignment.BottomLeft
            };

            footer.Controls.Add(_lblGreeting);
            footer.Controls.Add(btnLogout);
            sidebar.Controls.Add(footer);
            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(branding);

            _topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 102,
                BackColor = Color.White,
                Padding = new Padding(28, 16, 28, 14)
            };

            _lblPageTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };

            _lblPageDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            _topBar.Controls.Add(_lblPageDescription);
            _topBar.Controls.Add(_lblPageTitle);

            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(28, 24, 28, 28),
                BackColor = Color.FromArgb(244, 246, 248)
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
                Width = 188,
                Height = 46,
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = text,
                Tag = key,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 10)
            };
            button.BorderRadius = 10;
            button.FillColor = Color.FromArgb(15, 23, 42);
            button.HoverState.FillColor = Color.FromArgb(26, 40, 68);
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

            ShowSection(Convert.ToString(button.Tag));
        }

        private void ShowSection(string sectionKey)
        {
            _activeSection = string.IsNullOrWhiteSpace(sectionKey) ? "quizzes" : sectionKey;

            foreach (var pair in _navButtons)
            {
                var isActive = pair.Key == _activeSection;
                pair.Value.FillColor = isActive ? Color.FromArgb(3, 105, 161) : Color.FromArgb(15, 23, 42);
                pair.Value.ForeColor = isActive ? Color.White : Color.FromArgb(226, 232, 240);
            }

            if (_activeSection == "results")
            {
                _topBar.Visible = false;
                _lblPageTitle.Text = string.Empty;
                _lblPageDescription.Text = string.Empty;
                RenderResultsView();
                return;
            }

            _topBar.Visible = false;
            _lblPageTitle.Text = string.Empty;
            _lblPageDescription.Text = string.Empty;
            RenderQuizLandingView();
        }

        private void RenderQuizLandingView()
        {
            _contentHost.Controls.Clear();
            _studentQuizzes = _quizService.GetStudentQuizList(_currentUserId);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 158F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Controls.Add(CreateQuizLandingHeroPanel(), 0, 0);
            root.Controls.Add(CreateQuizBrowserPanel(), 0, 1);
            _contentHost.Controls.Add(root);
        }

        private void RenderResultsView()
        {
            _contentHost.Controls.Clear();
            var results = _reportService.GetStudentResults(_currentUserId);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 122F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 236F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(CreateResultsOverviewPanel(), 0, 0);
            root.Controls.Add(CreateResultsMetricsRow(results), 0, 1);
            root.Controls.Add(CreateResultsTrendPanel(results), 0, 2);
            root.Controls.Add(CreateResultsTablePanel(results), 0, 3);

            _contentHost.Controls.Add(root);
        }

        private Control CreateResultsOverviewPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 54,
                Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                Text = "Your progress so far"
            });

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = ReportsAccent,
                Text = "MY RESULTS"
            });

            return panel;
        }

        private Control CreateResultsMetricsRow(StudentResultsDto results)
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 10, 0, 18),
                BackColor = Color.Transparent
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            var bestAttempt = results.History.OrderByDescending(x => x.ScorePercentage).ThenByDescending(x => x.SubmittedAtLocal).FirstOrDefault();
            row.Controls.Add(CreateResultsMetricCard("Average Score", results.QuizzesTaken > 0 ? string.Format("{0:0.#}%", results.AverageScore) : "No result yet", string.Format("{0} scored attempts", results.QuizzesTaken), false), 0, 0);
            row.Controls.Add(CreateResultsMetricCard("Quizzes Taken", results.QuizzesTaken.ToString(), "Completed quizzes in your history", false), 1, 0);
            row.Controls.Add(CreateResultsMetricCard("Best", results.QuizzesTaken > 0 ? string.Format("{0:0.#}%", results.BestScore) : "No result yet", bestAttempt != null ? bestAttempt.QuizTitle : "No attempts yet", true), 2, 0);
            return row;
        }

        private Control CreateResultsMetricCard(string title, string value, string note, bool highlight)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = highlight ? Color.FromArgb(24, 105, 72) : ReportsCardBg,
                BorderStyle = BorderStyle.None,
                Margin = highlight ? new Padding(18, 0, 0, 0) : new Padding(0, 0, 18, 0),
                Padding = new Padding(18, 16, 18, 16)
            };
            panel.Paint += SurfacePanel_PaintRoundedBorder;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = highlight ? Color.FromArgb(226, 232, 240) : ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = note
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = value
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = highlight ? ReportsAccent : ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = title.ToUpperInvariant()
            });

            return panel;
        }

        private Control CreateResultsTrendPanel(StudentResultsDto results)
        {
            var panel = CreateReportStyleSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 18);

            var chartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ReportsInnerBg,
                Padding = new Padding(14, 14, 14, 14)
            };
            chartPanel.Controls.Add(CreateStudentProgressPlot(results));

            panel.Controls.Add(chartPanel);
            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = "Progress over time"
            });

            return panel;
        }

        private Control CreateStudentProgressPlot(StudentResultsDto results)
        {
            var history = results.History
                .Where(x => x.SubmittedAtLocal.HasValue)
                .OrderBy(x => x.SubmittedAtLocal.Value)
                .Take(8)
                .ToList();

            var chart = new FormsPlot
            {
                Dock = DockStyle.Fill,
                BackColor = ReportsInnerBg,
                Margin = Padding.Empty
            };

            var plot = chart.Plot;
            ApplyStudentResultsPlotTheme(plot);

            if (history.Count == 0)
            {
                chart.Refresh();
                return chart;
            }

            var xs = Enumerable.Range(0, history.Count).Select(i => (double)i).ToArray();
            var ys = history.Select(x => x.ScorePercentage).ToArray();
            var labels = history
                .Select(x => x.SubmittedAtLocal.HasValue ? x.SubmittedAtLocal.Value.ToString("MMM d") : x.SubmittedAtDisplay)
                .ToArray();

            var scatter = plot.Add.ScatterLine(xs, ys);
            scatter.Color = ScottColor.FromColor(ReportsAccent);
            scatter.LineWidth = 3;
            scatter.MarkerSize = 8;
            scatter.MarkerFillColor = ScottColor.FromColor(ReportsMustard);
            scatter.MarkerLineColor = ScottColor.FromColor(ReportsBorder);
            scatter.MarkerLineWidth = 1;

            plot.Axes.Bottom.SetTicks(xs, labels);
            plot.Axes.Left.Min = 50;
            plot.Axes.Left.Max = 100;
            plot.Axes.Left.SetTicks(
                new double[] { 50, 65, 80, 100 },
                new[] { "50", "65", "80", "100" });
            plot.Axes.Margins(0.05, 0.08, 0.08, 0.18);
            chart.Refresh();
            return chart;
        }

        private static void ApplyStudentResultsPlotTheme(ScottPlot.Plot plot)
        {
            var figureColor = ScottColor.FromColor(ReportsInnerBg);
            var dataColor = ScottColor.FromColor(ReportsInnerBg);
            var axisColor = ScottColor.FromColor(ReportsTextMuted);
            var gridColor = ScottColor.FromColor(Color.FromArgb(48, 255, 255, 255));

            plot.Clear();
            plot.FigureBackground.Color = figureColor;
            plot.DataBackground.Color = dataColor;
            plot.Axes.Color(axisColor);
            plot.Axes.FrameColor(ScottColor.FromColor(ReportsBorder));
            plot.Axes.DefaultGrid.MajorLineColor = gridColor;
            plot.Axes.DefaultGrid.MinorLineColor = gridColor;
            plot.HideLegend();
        }

        private Control CreateResultsTablePanel(StudentResultsDto results)
        {
            var panel = CreateReportStyleSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0),
                BackColor = ReportsInnerBg
            };

            content.Controls.Add(CreateStudentResultsTableHeader());
            if (results.History.Count == 0)
            {
                content.Controls.Add(CreateEmptyInfoCard("No submitted results yet. Complete a quiz and your history will appear here."));
            }
            else
            {
                foreach (var item in results.History)
                {
                    content.Controls.Add(CreateStudentResultsTableRow(item));
                }
            }

            FitFlowCardWidths(content, 320);
            panel.Controls.Add(content);
            return panel;
        }

        private static Control CreateStudentResultsTableHeader()
        {
            const int padX = 12;
            const int padY = 8;
            var bar = new Panel
            {
                Width = 520,
                Height = 40,
                Margin = new Padding(0, 0, 0, 6),
                BackColor = ReportsCardBg
            };
            bar.Paint += RoundedInsetRow_Paint;

            var h1 = CreateHeaderCell("QUIZ");
            var h2 = CreateHeaderCell("SUBJECT");
            var h3 = CreateHeaderCell("DATE");
            var h4 = CreateHeaderCell("SCORE");
            bar.Controls.Add(h1);
            bar.Controls.Add(h2);
            bar.Controls.Add(h3);
            bar.Controls.Add(h4);

            EventHandler arrange = delegate
            {
                var innerW = bar.ClientSize.Width - padX * 2;
                var innerH = bar.ClientSize.Height - padY * 2;
                var c0 = (int)Math.Floor(innerW * 0.42);
                var c1 = (int)Math.Floor(innerW * 0.22);
                var c2 = (int)Math.Floor(innerW * 0.20);
                var c3 = innerW - c0 - c1 - c2;
                h1.SetBounds(padX, padY, c0, innerH);
                h2.SetBounds(padX + c0, padY, c1, innerH);
                h3.SetBounds(padX + c0 + c1, padY, c2, innerH);
                h4.SetBounds(padX + c0 + c1 + c2, padY, c3, innerH);
            };

            bar.HandleCreated += arrange;
            bar.Resize += arrange;
            arrange(bar, EventArgs.Empty);
            return bar;
        }

        private static Control CreateStudentResultsTableRow(StudentResultHistoryItemDto item)
        {
            const int padX = 12;
            const int padY = 8;
            var row = new Panel
            {
                Width = 520,
                Height = 60,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = ReportsInnerBg
            };
            row.Paint += SubmissionRowDivider_Paint;

            var lblQuiz = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = item.QuizTitle,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            var lblSubject = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = item.SubjectName,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            var lblDate = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = item.SubmittedAtLocal.HasValue ? item.SubmittedAtLocal.Value.ToString("MMM d") : item.SubmittedAtDisplay,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            var pill = CreateRatePillPanel(string.Format("{0:0.#}%", item.ScorePercentage), ScorePillBack(item.ScorePercentage), ScorePillFore(item.ScorePercentage));
            row.Controls.Add(lblQuiz);
            row.Controls.Add(lblSubject);
            row.Controls.Add(lblDate);
            row.Controls.Add(pill);

            EventHandler arrange = delegate
            {
                var innerW = row.ClientSize.Width - padX * 2;
                var innerH = row.ClientSize.Height - padY * 2;
                var c0 = (int)Math.Floor(innerW * 0.42);
                var c1 = (int)Math.Floor(innerW * 0.22);
                var c2 = (int)Math.Floor(innerW * 0.20);
                var c3 = innerW - c0 - c1 - c2;
                lblQuiz.SetBounds(padX, padY, c0, innerH);
                lblSubject.SetBounds(padX + c0, padY, c1, innerH);
                lblDate.SetBounds(padX + c0 + c1, padY, c2, innerH);
                var pillY = padY + Math.Max(0, (innerH - pill.Height) / 2);
                pill.SetBounds(padX + c0 + c1 + c2 + Math.Max(0, c3 - pill.Width), pillY, pill.Width, pill.Height);
            };

            row.HandleCreated += arrange;
            row.Resize += arrange;
            arrange(row, EventArgs.Empty);
            return row;
        }

        private Panel CreateResultsHeroPanel(string studentName)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(24, 22, 24, 22);

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "This view tracks scored submissions. After each exam submission, the latest result also opens in a dedicated summary dialog."
            });

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = string.Format("{0}'s results", string.IsNullOrWhiteSpace(studentName) ? "Student" : studentName)
            });

            return panel;
        }

        private Panel CreateResultsHistoryPanel(StudentResultsDto results)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 20);

            var scrollHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var topSection = new Panel
            {
                Width = 760,
                Height = 62,
                Margin = new Padding(0, 0, 0, 8)
            };

            topSection.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = "Most recent submitted attempts"
            });

            topSection.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Results history"
            });

            var recommendationPanel = default(Control);
            if (results.LatestRecommendations.Count > 0 || !string.IsNullOrWhiteSpace(results.LatestWeakAreaSummary))
            {
                recommendationPanel = CreateLatestRecommendationPanel(results);
                recommendationPanel.Margin = new Padding(0, 0, 0, 12);
            }

            var resultsFlow = new FlowLayoutPanel
            {
                Width = 760,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0)
            };

            if (results.History.Count == 0)
            {
                resultsFlow.Controls.Add(CreateEmptyStateCard("No submitted results yet. Finish an exam and the result history will appear here."));
            }
            else
            {
                foreach (var item in results.History)
                {
                    resultsFlow.Controls.Add(CreateResultHistoryCard(item));
                }
            }

            content.Controls.Add(topSection);
            if (recommendationPanel != null)
            {
                content.Controls.Add(recommendationPanel);
            }
            content.Controls.Add(resultsFlow);
            scrollHost.Controls.Add(content);
            panel.Controls.Add(scrollHost);
            FitResultCardWidths(content, topSection, recommendationPanel, resultsFlow, 760);
            return panel;
        }

        private Control CreateLatestRecommendationPanel(StudentResultsDto results)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 188,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(16, 14, 16, 14)
            };

            var bulletText = results.LatestRecommendations.Count == 0
                ? "No recommendation items available."
                : string.Join(
                    Environment.NewLine,
                    results.LatestRecommendations.Select(x => string.Format("• {0}: {1}", x.Title, x.Description)));

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format(
                    "Source: {0}\r\n\r\n{1}\r\n\r\n{2}",
                    string.IsNullOrWhiteSpace(results.RecommendationSourceLabel) ? "Unavailable" : results.RecommendationSourceLabel,
                    string.IsNullOrWhiteSpace(results.LatestWeakAreaSummary) ? "No weak-area summary available." : results.LatestWeakAreaSummary,
                    bulletText)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Latest study recommendations"
            });

            return panel;
        }

        private Control CreateResultHistoryCard(StudentResultHistoryItemDto item)
        {
            var panel = new Panel
            {
                Width = 760,
                Height = 138,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(18, 18, 18, 18)
            };

            var scorePanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 170
            };

            scorePanel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = string.Format("{0:0.#}%", item.ScorePercentage),
                TextAlign = ContentAlignment.MiddleRight
            });

            scorePanel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = item.SubmittedAtDisplay,
                TextAlign = ContentAlignment.MiddleRight
            });

            var body = new Panel
            {
                Dock = DockStyle.Fill
            };

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("Correct: {0} | Wrong: {1} | Unanswered: {2} | Time: {3}", item.CorrectAnswers, item.WrongAnswers, item.UnansweredQuestions, item.TimeSpentDisplay)
            });

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = item.SubjectName
            });

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = item.QuizTitle
            });

            panel.Controls.Add(body);
            panel.Controls.Add(scorePanel);
            return panel;
        }

        private static void FitResultCardWidths(FlowLayoutPanel content, Panel topSection, Control recommendationPanel, FlowLayoutPanel resultsFlow, int minWidth)
        {
            if (content == null || resultsFlow == null || topSection == null)
            {
                return;
            }

            Action resizeCards = delegate
            {
                var targetWidth = Math.Max(minWidth, content.Parent != null ? content.Parent.ClientSize.Width - 22 : minWidth);
                topSection.Width = targetWidth;
                if (recommendationPanel != null)
                {
                    recommendationPanel.Width = targetWidth;
                }
                resultsFlow.Width = targetWidth;

                foreach (Control control in resultsFlow.Controls)
                {
                    control.Width = targetWidth;
                }
            };

            resizeCards();
            if (content.Parent != null)
            {
                content.Parent.Resize += delegate { resizeCards(); };
            }
        }

        private Panel CreateQuizBrowserPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var controlsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 108,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 16)
            };

            var searchCard = new Panel
            {
                Size = new Size(520, 56),
                Location = new Point(0, 0),
                BackColor = ReportsCardBg,
                Margin = Padding.Empty,
                Padding = new Padding(18, 0, 18, 0),
                Tag = AppTheme.SkipCognitaThemeTag
            };
            searchCard.Paint += RoundedInsetRow_Paint;

            var lblSearchPrefix = new Label
            {
                AutoSize = false,
                Location = new Point(18, 0),
                Width = 76,
                Height = searchCard.Height,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = ReportsAccent,
                BackColor = Color.Transparent,
                Text = "Search",
                TextAlign = ContentAlignment.MiddleLeft
            };

            _txtQuizSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = ReportsCardBg,
                ForeColor = ReportsTextPrimary,
                Font = new Font("Segoe UI", 10.5F),
                Location = new Point(94, 19),
                Width = searchCard.Width - 112,
                Text = _quizSearchTerm
            };
            _txtQuizSearch.HandleCreated += delegate
            {
                SendMessage(_txtQuizSearch.Handle, EmSetCueBanner, IntPtr.Zero, "Search by title, topic, or subject...");
            };
            _txtQuizSearch.TextChanged += QuizSearchTextChanged;

            searchCard.Resize += delegate
            {
                lblSearchPrefix.Height = searchCard.Height;
                _txtQuizSearch.Width = Math.Max(180, searchCard.ClientSize.Width - 112);
                _txtQuizSearch.Top = Math.Max(16, (searchCard.ClientSize.Height - _txtQuizSearch.PreferredHeight) / 2);
            };
            searchCard.Controls.Add(_txtQuizSearch);
            searchCard.Controls.Add(lblSearchPrefix);

            _lblQuizSearchSummary = new Label
            {
                AutoSize = false,
                Location = new Point(0, 68),
                Width = 560,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent
            };

            controlsPanel.Controls.Add(_lblQuizSearchSummary);
            controlsPanel.Controls.Add(searchCard);

            _quizCardsHost = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 4, 0, 0),
                Margin = new Padding(0),
                BackColor = Color.Transparent,
                AutoSize = false
            };

            panel.Controls.Add(_quizCardsHost);
            panel.Controls.Add(controlsPanel);

            RefreshQuizCards();
            return panel;
        }

        private void QuizSearchTextChanged(object sender, EventArgs e)
        {
            _quizSearchTerm = _txtQuizSearch == null ? string.Empty : _txtQuizSearch.Text;
            RefreshQuizCards();
        }

        private void RefreshQuizCards()
        {
            if (_quizCardsHost == null)
            {
                return;
            }

            var filteredQuizzes = GetFilteredStudentQuizzes();

            _quizCardsHost.SuspendLayout();
            _quizCardsHost.Controls.Clear();

            if (_studentQuizzes.Count == 0)
            {
                var empty = CreateQuizEmptyStateCard();
                empty.Margin = new Padding(0, 0, 18, 18);
                _quizCardsHost.Controls.Add(empty);
            }
            else
            {
                if (filteredQuizzes.Count == 0)
                {
                    var empty = CreateQuizEmptyStateCard("No quizzes match your search. Try a different title, topic, or subject.");
                    empty.Margin = new Padding(0, 0, 18, 18);
                    _quizCardsHost.Controls.Add(empty);
                }
                else
                {
                    foreach (var quiz in filteredQuizzes)
                    {
                        _quizCardsHost.Controls.Add(CreateQuizCard(quiz));
                    }
                }
            }

            _quizCardsHost.ResumeLayout();
            UpdateQuizSearchSummary(filteredQuizzes.Count);
        }

        private List<StudentQuizListItemDto> GetFilteredStudentQuizzes()
        {
            if (string.IsNullOrWhiteSpace(_quizSearchTerm))
            {
                return _studentQuizzes.ToList();
            }

            var search = _quizSearchTerm.Trim();

            return _studentQuizzes
                .Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Title) && x.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(x.Topic) && x.Topic.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(x.SubjectName) && x.SubjectName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
        }

        private void UpdateQuizSearchSummary(int filteredCount)
        {
            if (_lblQuizSearchSummary == null)
            {
                return;
            }

            if (_studentQuizzes.Count == 0)
            {
                _lblQuizSearchSummary.Text = "Posted quizzes will appear here as soon as they are available.";
                return;
            }

            if (string.IsNullOrWhiteSpace(_quizSearchTerm))
            {
                _lblQuizSearchSummary.Text = string.Format("{0} posted quiz{1} available.", filteredCount, filteredCount == 1 ? string.Empty : "zes");
                return;
            }

            _lblQuizSearchSummary.Text = string.Format(
                "{0} of {1} quiz{2} shown for \"{3}\".",
                filteredCount,
                _studentQuizzes.Count,
                _studentQuizzes.Count == 1 ? string.Empty : "zes",
                _quizSearchTerm.Trim());
        }

        private Control CreateQuizEmptyStateCard()
        {
            return CreateQuizEmptyStateCard("No posted quizzes are available yet. Once a teacher posts a quiz, it will appear here automatically.");
        }

        private Control CreateEmptyStateCard(string message)
        {
            var panel = new Panel
            {
                Width = 760,
                Height = 128,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(20, 18, 20, 18)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = message,
                TextAlign = ContentAlignment.MiddleLeft
            });

            return panel;
        }

        private Control CreateQuizEmptyStateCard(string message)
        {
            var panel = new Panel
            {
                Width = 432,
                Height = 120,
                BackColor = ReportsCardBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(24, 22, 24, 22)
            };
            panel.Paint += RoundedInsetRow_Paint;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = message,
                TextAlign = ContentAlignment.MiddleLeft
            });

            return panel;
        }

        private Control CreateQuizCard(StudentQuizListItemDto quiz)
        {
            const int cardWidth = 432;
            const int paddingX = 22;
            const int contentWidth = cardWidth - (paddingX * 2);

            var panel = new Panel
            {
                Width = cardWidth,
                Height = 228,
                BackColor = ReportsCardBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 18, 18),
                Padding = new Padding(22, 22, 22, 20),
                Tag = AppTheme.SkipCognitaThemeTag
            };
            panel.Paint += RoundedInsetRow_Paint;

            var subjectPill = CreateQuizMetaPill(quiz.SubjectName, false);
            subjectPill.Location = new Point(paddingX, 16);

            var difficultyPill = CreateQuizMetaPill(quiz.Difficulty.ToString(), true);
            difficultyPill.Location = new Point(cardWidth - paddingX - difficultyPill.Width, 16);

            var lblTitle = new Label
            {
                AutoSize = false,
                Location = new Point(paddingX, 62),
                Width = contentWidth,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = quiz.Title,
                AutoEllipsis = true
            };

            var lblTopic = new Label
            {
                AutoSize = false,
                Location = new Point(paddingX, 102),
                Width = contentWidth,
                Height = 24,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = string.IsNullOrWhiteSpace(quiz.Topic) ? "No topic provided" : quiz.Topic,
                AutoEllipsis = true
            };

            var lblMeta = new Label
            {
                AutoSize = false,
                Location = new Point(paddingX, 140),
                Width = contentWidth,
                Height = 22,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = string.Format("{0} question{1}  |  {2} min", quiz.QuestionCount, quiz.QuestionCount == 1 ? string.Empty : "s", quiz.DurationMinutes)
            };

            var lblAvailability = new Label
            {
                AutoSize = false,
                Location = new Point(paddingX, 164),
                Width = contentWidth,
                Height = 18,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = quiz.CanStart ? ReportsAccent : ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = quiz.AvailabilityLabel,
                AutoEllipsis = true
            };

            var btnStart = new Guna2Button
            {
                Location = new Point(paddingX, 186),
                Width = quiz.HasCompletedAttempt ? ((contentWidth - 10) / 2) : contentWidth,
                Height = 34,
                FillColor = quiz.CanStart ? ReportsMustard : ReportsInnerBg,
                ForeColor = quiz.CanStart ? Color.Black : ReportsTextMuted,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = quiz.CanStart ? "Start Quiz" : (quiz.HasCompletedAttempt ? "Completed" : "Unavailable"),
                Cursor = Cursors.Hand,
                Enabled = true,
                Tag = quiz
            };
            btnStart.BorderRadius = 10;
            btnStart.BorderThickness = 1;
            btnStart.BorderColor = quiz.CanStart ? ReportsMustardBorder : ReportsBorder;
            btnStart.DisabledState.FillColor = btnStart.FillColor;
            btnStart.DisabledState.ForeColor = btnStart.ForeColor;
            btnStart.DisabledState.BorderColor = btnStart.BorderColor;
            btnStart.Click += StartQuiz_Click;

            panel.Controls.Add(btnStart);
            if (quiz.HasCompletedAttempt)
            {
                var btnReview = new Guna2Button
                {
                    Location = new Point(paddingX + btnStart.Width + 10, 186),
                    Width = contentWidth - btnStart.Width - 10,
                    Height = 34,
                    FillColor = Color.FromArgb(22, 88, 61),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    Text = "Review Answers",
                    Cursor = Cursors.Hand,
                    Enabled = true,
                    Tag = quiz
                };
                btnReview.BorderRadius = 10;
                btnReview.BorderThickness = 1;
                btnReview.BorderColor = ReportsBorder;
                btnReview.Click += ReviewAnswers_Click;
                panel.Controls.Add(btnReview);
            }
            panel.Controls.Add(lblAvailability);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblTopic);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(difficultyPill);
            panel.Controls.Add(subjectPill);
            return panel;
        }

        private void StartQuiz_Click(object sender, EventArgs e)
        {
            var button = sender as Control;
            var quiz = button != null ? button.Tag as StudentQuizListItemDto : null;
            if (quiz == null)
            {
                return;
            }

            if (!quiz.CanStart)
            {
                NotificationHelper.ShowInfo(this, "Quiz Unavailable", quiz.AvailabilityLabel);
                return;
            }

            try
            {
                var attemptId = _examService.StartAttempt(_currentUserId, quiz.Id);
                NotificationHelper.ShowInfo(this, "Exam Started", string.Format("Starting \"{0}\" now.", quiz.Title));
                using (var examForm = new ExamForm(_currentUserId, attemptId))
                {
                    examForm.Text = quiz.Title;
                    var examResult = examForm.ShowDialog(this);
                    if (examResult == DialogResult.Yes)
                    {
                        ShowSection("results");
                        return;
                    }
                }

                RenderQuizLandingView();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Unable to start quiz. QuizId={QuizId} StudentId={StudentId}", quiz.Id, _currentUserId);
                NotificationHelper.ShowError(this, "Unable To Start Quiz", ex.Message);
            }
        }

        private void ReviewAnswers_Click(object sender, EventArgs e)
        {
            var button = sender as Control;
            var quiz = button != null ? button.Tag as StudentQuizListItemDto : null;
            if (quiz == null)
            {
                return;
            }

            try
            {
                var review = _examService.GetAttemptReview(_currentUserId, quiz.Id);
                using (var reviewForm = new StudentAttemptReviewForm(review))
                {
                    reviewForm.Text = string.Format("Review Answers - {0}", review.QuizTitle);
                    reviewForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowError(this, "Unable To Load Review", ex.Message);
            }
        }

        private void RenderPlaceholderView(string title, string description, string[] bulletPoints)
        {
            _contentHost.Controls.Clear();
            _contentHost.Controls.Add(CreateInsightPanel(title, description, bulletPoints));
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
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = title
            };

            var lblNote = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = note
            };

            panel.Controls.Add(lblNote);
            panel.Controls.Add(lblValue);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateQuizLandingHeroPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0),
                Margin = new Padding(0)
            };

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI", 12.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = "Pick a quiz to start. Your progress is saved automatically."
            });

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 64,
                Font = new Font("Segoe UI Semibold", 30F, FontStyle.Bold),
                ForeColor = ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = "Ready when you are."
            });

            panel.Controls.Add(new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = ReportsAccent,
                BackColor = Color.Transparent,
                Text = "AVAILABLE QUIZZES"
            });

            return panel;
        }

        private Panel CreateInsightPanel(string title, string description, string[] bulletPoints)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(24, 22, 24, 22);

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = title
            };

            var lblDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = description
            };

            var lblBullets = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = BuildBulletList(bulletPoints)
            };

            panel.Controls.Add(lblBullets);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateSurfacePanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0)
            };
        }

        private static Panel CreateReportStyleSurfacePanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ReportsCardBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0)
            };
            panel.Paint += SurfacePanel_PaintRoundedBorder;
            return panel;
        }

        private static Control CreateAvailabilityPillPanel(string text, bool canStart)
        {
            const int pillHeight = 28;
            var font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            var width = Math.Max(
                120,
                Math.Min(
                    360,
                    TextRenderer.MeasureText(text ?? string.Empty, font, new Size(short.MaxValue, pillHeight), TextFormatFlags.SingleLine | TextFormatFlags.NoPadding).Width + 24));

            var backColor = canStart ? ReportsMustard : ReportsInnerBg;
            var foreColor = canStart ? ReportsWorkspaceBg : ReportsTextPrimary;
            var panel = new Panel
            {
                Size = new Size(width, pillHeight),
                BackColor = backColor,
                Margin = new Padding(0)
            };
            panel.Paint += PillBorder_Paint;

            panel.Controls.Add(new Label
            {
                Bounds = new Rectangle(0, 0, width, pillHeight),
                Font = font,
                ForeColor = foreColor,
                BackColor = Color.Transparent,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                UseCompatibleTextRendering = true
            });

            return panel;
        }

        private static Panel CreateQuizMetaPill(string text, bool rightAligned)
        {
            var label = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = rightAligned ? Color.Black : ReportsTextPrimary,
                BackColor = Color.Transparent,
                Text = string.IsNullOrWhiteSpace(text) ? "General" : text,
                TextAlign = ContentAlignment.MiddleCenter,
                UseCompatibleTextRendering = true
            };

            var size = TextRenderer.MeasureText(label.Text, label.Font, new Size(short.MaxValue, 30), TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            var panel = new Panel
            {
                Width = Math.Max(92, Math.Min(180, size.Width + 28)),
                Height = 32,
                BackColor = rightAligned ? ReportsMustard : ReportsInnerBg,
                Margin = new Padding(0),
                Tag = AppTheme.SkipCognitaThemeTag
            };
            panel.Paint += PillBorder_Paint;

            label.Dock = DockStyle.Fill;
            panel.Controls.Add(label);
            return panel;
        }

        private static void FitFlowCardWidths(FlowLayoutPanel flow, int minWidth)
        {
            if (flow == null)
            {
                return;
            }

            Action apply = delegate
            {
                var targetWidth = Math.Max(minWidth, flow.ClientSize.Width - 8);
                foreach (Control control in flow.Controls)
                {
                    control.Width = targetWidth;
                }
            };

            apply();
            flow.Resize -= Flow_ResizeFitCards;
            flow.Resize += Flow_ResizeFitCards;
        }

        private static void Flow_ResizeFitCards(object sender, EventArgs e)
        {
            var flow = sender as FlowLayoutPanel;
            if (flow == null)
            {
                return;
            }

            var targetWidth = Math.Max(280, flow.ClientSize.Width - 8);
            foreach (Control control in flow.Controls)
            {
                control.Width = targetWidth;
            }
        }

        private static Control CreateEmptyInfoCard(string message)
        {
            var panel = new Panel
            {
                Width = 470,
                Height = 84,
                BackColor = ReportsInnerBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };
            panel.Paint += RoundedInsetRow_Paint;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = message
            });

            return panel;
        }

        private static Label CreateHeaderCell(string text)
        {
            return new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = ReportsTextMuted,
                BackColor = Color.Transparent,
                Text = text,
                UseCompatibleTextRendering = true
            };
        }

        private static Control CreateRatePillPanel(string text, Color back, Color fore)
        {
            const int pillH = 28;
            var font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            var textW = TextRenderer.MeasureText(text, font, new Size(short.MaxValue, pillH), TextFormatFlags.SingleLine | TextFormatFlags.NoPadding).Width;
            var w = Math.Max(88, Math.Min(220, textW + 24));
            var pill = new Panel
            {
                Size = new Size(w, pillH),
                BackColor = back,
                Margin = new Padding(0)
            };
            pill.Paint += PillBorder_Paint;

            pill.Controls.Add(new Label
            {
                Bounds = new Rectangle(0, 0, w, pillH),
                Font = font,
                ForeColor = fore,
                BackColor = Color.Transparent,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                UseCompatibleTextRendering = true,
                AutoSize = false
            });

            return pill;
        }

        private static Color ScorePillBack(double score)
        {
            if (score < 60)
            {
                return ReportsWorkspaceBg;
            }

            if (score < 85)
            {
                return ReportsMustard;
            }

            return ReportsCardBg;
        }

        private static Color ScorePillFore(double score)
        {
            if (score < 60)
            {
                return ReportsTextPrimary;
            }

            if (score < 85)
            {
                return ReportsWorkspaceBg;
            }

            return ReportsTextPrimary;
        }

        private static void SubmissionRowDivider_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null)
            {
                return;
            }

            using (var pen = new Pen(ReportsBorder, 1))
            {
                e.Graphics.DrawLine(pen, 12, panel.Height - 1, panel.Width - 12, panel.Height - 1);
            }
        }

        private static void PillBorder_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            const int radius = 10;
            using (var path = BuildRoundedPath(rect, radius))
            using (var pen = new Pen(ReportsBorder, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static void RoundedInsetRow_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            const int radius = 10;
            using (var path = BuildRoundedPath(rect, radius))
            using (var pen = new Pen(ReportsBorder, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static void SurfacePanel_PaintRoundedBorder(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            const int radius = 14;
            using (var path = BuildRoundedPath(rect, radius))
            using (var pen = new Pen(ReportsBorder, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static GraphicsPath BuildRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static string BuildBulletList(IEnumerable<string> bulletPoints)
        {
            return "• " + string.Join(Environment.NewLine + "• ", bulletPoints);
        }
    }
}
