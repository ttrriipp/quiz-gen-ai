using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherDashboardForm : Form
    {
        private readonly Dictionary<string, Button> _navButtons = new Dictionary<string, Button>();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Panel _contentHost;
        private Label _lblGreeting;
        private string _displayName = "Teacher";
        public int CurrentUserId { get; set; }

        public TeacherDashboardForm()
        {
            InitializeComponent();
            BuildShell();
            ShowSection("dashboard");
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

            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1180, 760);
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
                Height = 92,
                Padding = new Padding(20, 12, 20, 20)
            };

            _lblGreeting = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(209, 213, 219),
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.BottomLeft,
                Text = string.Format("Signed in as {0}", _displayName)
            };

            footerPanel.Controls.Add(_lblGreeting);

            sidebar.Controls.Add(footerPanel);
            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(brandingPanel);

            var topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = Color.White,
                Padding = new Padding(28, 20, 28, 16)
            };

            _lblPageTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39)
            };

            _lblPageDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128)
            };

            topBar.Controls.Add(_lblPageDescription);
            topBar.Controls.Add(_lblPageTitle);

            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28, 24, 28, 28),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            Controls.Add(_contentHost);
            Controls.Add(topBar);
            Controls.Add(sidebar);
            ResumeLayout();
        }

        private Button CreateNavButton(string key, string text)
        {
            var button = new Button
            {
                Width = 208,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(31, 41, 55),
                ForeColor = Color.FromArgb(229, 231, 235),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Tag = key,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 10)
            };

            button.FlatAppearance.BorderSize = 0;
            button.Click += NavButton_Click;
            _navButtons[key] = button;
            return button;
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var sectionKey = Convert.ToString(button.Tag);
            if (sectionKey == "quizzes")
            {
                OpenQuizManager();
                ShowSection("dashboard");
                return;
            }

            ShowSection(sectionKey);
        }

        private void ShowSection(string sectionKey)
        {
            foreach (var pair in _navButtons)
            {
                var isActive = pair.Key == sectionKey;
                pair.Value.BackColor = isActive ? Color.FromArgb(14, 116, 144) : Color.FromArgb(31, 41, 55);
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
                    _lblPageDescription.Text = "Manual and AI-generated quizzes will be managed here next.";
                    RenderPlaceholderView(
                        "Quiz workspace is ready",
                        "Next step: build the teacher quiz list, search, filters, and quiz editor flow.",
                        new[] { "Search and filter bar", "Quiz cards with status badges", "New AI Quiz and manual editor entry points" });
                    break;

                case "reports":
                    _lblPageTitle.Text = "Reports";
                    _lblPageDescription.Text = "Charts and analytics will sit on this screen after quiz flow is complete.";
                    RenderPlaceholderView(
                        "Reports area is reserved",
                        "This shell already separates reporting from quiz management so chart work can be added cleanly.",
                        new[] { "Average score and pass/fail cards", "Subject mastery chart host", "Recent submissions table area" });
                    break;

                default:
                    _lblPageTitle.Text = "Logs";
                    _lblPageDescription.Text = "Operational history and audit events can plug into this slot later.";
                    RenderPlaceholderView(
                        "Logs section placeholder",
                        "Serilog output is part of the required stack; this navigation slot keeps the layout ready for it.",
                        new[] { "Authentication events", "Quiz publish/update history", "AI request and error logs" });
                    break;
            }
        }

        private void RenderDashboardView()
        {
            _contentHost.Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
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

            stats.Controls.Add(CreateMetricCard("Total Students", "0", "Will be populated from student records once reports are wired."), 0, 0);
            stats.Controls.Add(CreateMetricCard("Total Quizzes", "0", "Quiz CRUD comes next, so this card is the future home for that count."), 1, 0);
            stats.Controls.Add(CreateMetricCard("Average Score", "0%", "Appears after exam attempts and scoring are implemented."), 2, 0);

            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));

            bottom.Controls.Add(CreateInsightPanel(
                "Dashboard roadmap",
                "This shell is ready for the next sequence in AGENTS.md.",
                new[]
                {
                    "Teacher quiz list and manual quiz editor",
                    "AI quiz generation review-first workflow",
                    "Dashboard metrics and LiveCharts-based reports"
                }), 0, 0);

            bottom.Controls.Add(CreateInsightPanel(
                "Navigation status",
                "The layout now separates key teacher/admin areas.",
                new[]
                {
                    "Dashboard is the landing view",
                    "Quizzes has a dedicated slot",
                    "Reports and Logs are reserved for later steps"
                }), 1, 0);

            root.Controls.Add(hero, 0, 0);
            root.Controls.Add(stats, 0, 1);
            root.Controls.Add(bottom, 0, 2);
            _contentHost.Controls.Add(root);
        }

        private void RenderPlaceholderView(string title, string description, string[] bulletPoints)
        {
            _contentHost.Controls.Clear();
            _contentHost.Controls.Add(CreateInsightPanel(title, description, bulletPoints, DockStyle.Top, 420));
        }

        private Panel CreateHeroPanel(string title, string description, string buttonText)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(26, 22, 26, 22);

            var actionButton = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Width = 144,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = buttonText,
                Location = new Point(panel.Width - 180, 24),
                Cursor = Cursors.Hand
            };
            actionButton.FlatAppearance.BorderSize = 0;

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 38,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = title
            };

            var lblDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 54,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = description
            };

            panel.Resize += delegate
            {
                actionButton.Left = panel.ClientSize.Width - actionButton.Width - 26;
            };

            panel.Controls.Add(actionButton);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
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

        private void OpenQuizManager()
        {
            using (var form = new TeacherQuizzesForm())
            {
                form.CurrentUserId = CurrentUserId;
                form.DisplayName = DisplayName;
                form.ShowDialog(this);
            }
        }
    }
}
