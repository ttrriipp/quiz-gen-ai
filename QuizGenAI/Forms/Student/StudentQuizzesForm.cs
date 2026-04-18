using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentQuizzesForm : Form
    {
        private readonly Dictionary<string, Button> _navButtons = new Dictionary<string, Button>();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Panel _contentHost;
        private Label _lblGreeting;
        private string _displayName = "Student";

        public StudentQuizzesForm()
        {
            InitializeComponent();
            BuildShell();
            ShowSection("quizzes");
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

        private void BuildShell()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = Color.FromArgb(244, 246, 248);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1120, 720);
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
                Height = 88,
                Padding = new Padding(20, 12, 20, 20)
            };

            _lblGreeting = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(203, 213, 225),
                Font = new Font("Segoe UI", 10F),
                Text = string.Format("Signed in as {0}", _displayName),
                TextAlign = ContentAlignment.BottomLeft
            };

            footer.Controls.Add(_lblGreeting);
            sidebar.Controls.Add(footer);
            sidebar.Controls.Add(navPanel);
            sidebar.Controls.Add(branding);

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
                ForeColor = Color.FromArgb(15, 23, 42)
            };

            _lblPageDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            topBar.Controls.Add(_lblPageDescription);
            topBar.Controls.Add(_lblPageTitle);

            _contentHost = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28, 24, 28, 28),
                BackColor = Color.FromArgb(244, 246, 248)
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
                Width = 188,
                Height = 46,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(226, 232, 240),
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

            ShowSection(Convert.ToString(button.Tag));
        }

        private void ShowSection(string sectionKey)
        {
            foreach (var pair in _navButtons)
            {
                var isActive = pair.Key == sectionKey;
                pair.Value.BackColor = isActive ? Color.FromArgb(3, 105, 161) : Color.FromArgb(15, 23, 42);
                pair.Value.ForeColor = isActive ? Color.White : Color.FromArgb(226, 232, 240);
            }

            if (sectionKey == "results")
            {
                _lblPageTitle.Text = "Results";
                _lblPageDescription.Text = "Progress, score history, and review feedback will appear here.";
                RenderPlaceholderView(
                    "Results workspace is ready",
                    "This shell keeps the student journey separated from quiz taking and exam review.",
                    new[] { "Average score summary", "Quizzes taken and best score cards", "Progress chart and results table" });
                return;
            }

            _lblPageTitle.Text = "Quizzes";
            _lblPageDescription.Text = "Students will browse available quizzes and launch timed exams from here.";
            RenderQuizLandingView();
        }

        private void RenderQuizLandingView()
        {
            _contentHost.Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 170F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(CreateHeroPanel(), 0, 0);

            var statRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 18, 0, 18)
            };

            statRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            statRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            statRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            statRow.Controls.Add(CreateMetricCard("Available Quizzes", "0", "Published quiz cards will appear here after quiz management is built."), 0, 0);
            statRow.Controls.Add(CreateMetricCard("Completed Attempts", "0", "This updates after exam submission and result calculation are added."), 1, 0);
            statRow.Controls.Add(CreateMetricCard("Best Score", "0%", "Pulled from result history once scoring exists."), 2, 0);

            root.Controls.Add(statRow, 0, 1);

            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            bottom.Controls.Add(CreateInsightPanel(
                "Student flow roadmap",
                "This shell now reflects the intended student journey from AGENTS.md.",
                new[]
                {
                    "Browse published quiz cards",
                    "Enter exam screen with timer and navigation",
                    "Review results and recommendations"
                }), 0, 0);

            bottom.Controls.Add(CreateInsightPanel(
                "Current shell coverage",
                "Navigation is separated before the exam flow is implemented.",
                new[]
                {
                    "Quizzes landing view is active",
                    "Results has a dedicated navigation slot",
                    "Header area is ready for filters or summary pills"
                }), 1, 0);

            root.Controls.Add(bottom, 0, 2);
            _contentHost.Controls.Add(root);
        }

        private Panel CreateHeroPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(24, 22, 24, 22);

            var lblTitle = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Ready for your next quiz"
            };

            var lblDescription = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "Published quizzes, timers, and answer review will plug into this student shell without changing the overall layout."
            };

            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
            return panel;
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

        private static string BuildBulletList(IEnumerable<string> bulletPoints)
        {
            return "• " + string.Join(Environment.NewLine + "• ", bulletPoints);
        }
    }
}
