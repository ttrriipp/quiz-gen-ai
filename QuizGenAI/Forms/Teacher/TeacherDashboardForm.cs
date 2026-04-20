using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherDashboardForm : Form
    {
        private readonly Dictionary<string, Guna2Button> _navButtons = new Dictionary<string, Guna2Button>();
        private readonly ReportService _reportService = new ReportService();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Panel _topBar;
        private Panel _contentHost;
        private Label _lblGreeting;
        private Form _hostedContentForm;

        private static readonly Color MainWorkspaceGreen = Color.FromArgb(11, 48, 34);
        private string _displayName = "Teacher";
        public int CurrentUserId { get; set; }

        public TeacherDashboardForm()
        {
            InitializeComponent();
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
                    _lblPageTitle.Text = "Dashboard";
                    _lblPageDescription.Text = "Teacher/admin overview with quick actions and project metrics.";
                    RenderDashboardView();
                    break;

                case "quizzes":
                    _lblPageTitle.Text = "Quizzes";
                    _lblPageDescription.Text = "Manage draft, published, and archived quizzes without leaving the teacher shell.";
                    RenderHostedForm(CreateHostedQuizManager());
                    break;

                case "reports":
                    _lblPageTitle.Text = "Reports";
                    _lblPageDescription.Text = "Review teacher analytics and reporting inside the same workspace.";
                    RenderHostedForm(CreateHostedReportsView());
                    break;

                case "logs":
                    _lblPageTitle.Text = "Logs";
                    _lblPageDescription.Text = "Structured Serilog output from the app runtime.";
                    RenderLogsView();
                    break;

                default:
                    _lblPageTitle.Text = "Logs";
                    _lblPageDescription.Text = "Structured Serilog output from the app runtime.";
                    RenderLogsView();
                    break;
            }
        }

        private void RenderDashboardView()
        {
            ClearHostedContentForm();
            _contentHost.Controls.Clear();
            var dashboard = _reportService.GetTeacherDashboard();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };

            root.RowStyles.Clear();
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 168F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 170F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var hero = CreateHeroPanel(
                "Welcome back",
                "Use this workspace to manage quizzes, review publishing status, and access reports from one place.",
                "Generate Quiz");

            var stats = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 18, 0, 18)
            };

            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            stats.Controls.Add(CreateMetricCard("Total Students", dashboard.TotalStudents.ToString(), "Students with the student role in the local database."), 0, 0);
            stats.Controls.Add(CreateMetricCard("Total Quizzes", dashboard.TotalQuizzes.ToString(), "All draft, published, and archived quizzes."), 1, 0);
            stats.Controls.Add(CreateMetricCard("Average Score", dashboard.AverageScore.ToString("0.0") + "%", "Average across submitted attempts."), 2, 0);

            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));

            bottom.Controls.Add(CreateInsightPanel(
                "Status Overview",
                string.Format("{0} submissions recorded so far.", dashboard.SubmittedAttempts),
                new[]
                {
                    string.Format("Draft quizzes: {0}", dashboard.DraftQuizzes),
                    string.Format("Published quizzes: {0}", dashboard.PublishedQuizzes),
                    string.Format("Archived quizzes: {0}", dashboard.ArchivedQuizzes)
                }), 0, 0);

            bottom.Controls.Add(CreateRecentSubmissionsPanel(dashboard), 1, 0);

            root.Controls.Add(hero, 0, 0);
            root.Controls.Add(stats, 0, 1);
            root.Controls.Add(bottom, 0, 2);
            _contentHost.Controls.Add(root);
        }

        private void RenderPlaceholderView(string title, string description, string[] bulletPoints)
        {
            ClearHostedContentForm();
            _contentHost.Controls.Clear();
            _contentHost.Controls.Add(CreateInsightPanel(title, description, bulletPoints, DockStyle.Top, 420));
        }

        private void RenderLogsView()
        {
            ClearHostedContentForm();
            _contentHost.Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
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

            var summaryPanel = CreateSurfacePanel();
            summaryPanel.Dock = DockStyle.Fill;
            summaryPanel.Padding = new Padding(22, 18, 22, 18);

            var summaryLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 126F));

            var btnRefresh = new Guna2Button
            {
                Anchor = AnchorStyles.Right,
                Width = 110,
                Height = 34,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Refresh",
                Cursor = Cursors.Hand
            };
            btnRefresh.BorderRadius = 9;
            btnRefresh.FillColor = Color.FromArgb(15, 118, 110);
            btnRefresh.Click += delegate { RenderLogsView(); };

            var lblSummary = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = logFiles.Count == 0
                    ? "No log files found yet. Trigger a few actions, then refresh this view."
                    : string.Format(
                        "{0} log file(s) found. Showing the latest entries from {1}.",
                        logFiles.Count,
                        Path.GetFileName(logFiles.First()))
            };

            summaryLayout.Controls.Add(lblSummary, 0, 0);
            summaryLayout.Controls.Add(btnRefresh, 1, 0);
            summaryPanel.Controls.Add(summaryLayout);

            var viewerPanel = CreateSurfacePanel();
            viewerPanel.Dock = DockStyle.Fill;
            viewerPanel.Padding = new Padding(0);

            var txtLogs = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Consolas", 9.5F),
                Text = BuildLogPreview(logFiles)
            };

            viewerPanel.Controls.Add(txtLogs);

            root.Controls.Add(summaryPanel, 0, 0);
            root.Controls.Add(viewerPanel, 0, 1);
            _contentHost.Controls.Add(root);
        }

        private void RenderHostedForm(Form form)
        {
            ClearHostedContentForm();
            _contentHost.Controls.Clear();

            _hostedContentForm = form;
            _hostedContentForm.TopLevel = false;
            _hostedContentForm.FormBorderStyle = FormBorderStyle.None;
            _hostedContentForm.Dock = DockStyle.Fill;
            _hostedContentForm.StartPosition = FormStartPosition.Manual;
            _hostedContentForm.Padding = new Padding(0);

            _contentHost.Controls.Add(_hostedContentForm);
            _hostedContentForm.Show();
            _contentHost.PerformLayout();
            _hostedContentForm.PerformLayout();
        }

        private void ClearHostedContentForm()
        {
            if (_hostedContentForm == null)
            {
                return;
            }

            if (_contentHost.Controls.Contains(_hostedContentForm))
            {
                _contentHost.Controls.Remove(_hostedContentForm);
            }

            _hostedContentForm.Dispose();
            _hostedContentForm = null;
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

        private static string BuildLogPreview(List<string> logFiles)
        {
            if (logFiles == null || logFiles.Count == 0)
            {
                return "No logs are available yet.";
            }

            try
            {
                var latestFile = logFiles.First();
                string[] lines;

                using (var stream = new FileStream(latestFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    lines = content
                        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                }

                var tail = lines.Skip(Math.Max(0, lines.Length - 250));
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

        private Form CreateHostedReportsView()
        {
            return new ReportsForm
            {
                CurrentUserId = CurrentUserId,
                DisplayName = DisplayName,
                TopLevel = false
            };
        }
    }
}
