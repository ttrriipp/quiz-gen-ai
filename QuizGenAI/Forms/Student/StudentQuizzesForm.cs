using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentQuizzesForm : Form
    {
        private readonly Dictionary<string, Button> _navButtons = new Dictionary<string, Button>();
        private readonly QuizService _quizService = new QuizService();
        private Label _lblPageTitle;
        private Label _lblPageDescription;
        private Panel _contentHost;
        private Label _lblGreeting;
        private string _displayName = "Student";
        private int _currentUserId;
        private string _activeSection = "quizzes";

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
            _activeSection = string.IsNullOrWhiteSpace(sectionKey) ? "quizzes" : sectionKey;

            foreach (var pair in _navButtons)
            {
                var isActive = pair.Key == _activeSection;
                pair.Value.BackColor = isActive ? Color.FromArgb(3, 105, 161) : Color.FromArgb(15, 23, 42);
                pair.Value.ForeColor = isActive ? Color.White : Color.FromArgb(226, 232, 240);
            }

            if (_activeSection == "results")
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
            var quizzes = _quizService.GetStudentQuizList(_currentUserId);
            var completedAttempts = quizzes.Sum(x => x.AttemptCount);
            var bestScore = quizzes.Where(x => x.BestScore.HasValue).Select(x => x.BestScore.Value).DefaultIfEmpty().Max();
            var availableNow = quizzes.Count(x => x.CanStart);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 152F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            root.Controls.Add(CreateHeroPanel(quizzes.Count), 0, 0);

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

            statRow.Controls.Add(CreateMetricCard("Available Now", availableNow.ToString(), "Only quizzes inside their publish window can be started."), 0, 0);
            statRow.Controls.Add(CreateMetricCard("Attempt History", completedAttempts.ToString(), "This counts prior attempts already recorded for this student."), 1, 0);
            statRow.Controls.Add(CreateMetricCard("Best Score", bestScore > 0 ? string.Format("{0:0.#}%", bestScore) : "No result yet", "This updates as soon as scored attempts are recorded."), 2, 0);

            root.Controls.Add(statRow, 0, 1);

            root.Controls.Add(CreateQuizBrowserPanel(quizzes), 0, 2);
            _contentHost.Controls.Add(root);
        }

        private Panel CreateHeroPanel(int quizCount)
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
                Height = 52,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("{0} published quizzes are visible here. Start buttons are now gated by publish windows, while the full timed exam flow is the next step.", quizCount)
            };

            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Panel CreateQuizBrowserPanel(IList<StudentQuizListItemDto> quizzes)
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 20);

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Published quizzes"
            };

            var lblDescription = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = "Students can only see published quizzes. Cards show whether a quiz is open, upcoming, or already closed."
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            };

            if (quizzes.Count == 0)
            {
                flow.Controls.Add(CreateEmptyStateCard());
            }
            else
            {
                foreach (var quiz in quizzes)
                {
                    flow.Controls.Add(CreateQuizCard(quiz));
                }
            }

            panel.Controls.Add(flow);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Control CreateEmptyStateCard()
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
                Text = "No published quizzes are available yet. Once a teacher publishes a quiz, it will appear here automatically.",
                TextAlign = ContentAlignment.MiddleLeft
            });

            return panel;
        }

        private Control CreateQuizCard(StudentQuizListItemDto quiz)
        {
            var panel = new Panel
            {
                Width = 930,
                Height = 152,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 14),
                Padding = new Padding(18, 18, 18, 18)
            };

            var actionPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 190
            };

            var btnStart = new Button
            {
                Width = 150,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = quiz.CanStart ? Color.FromArgb(15, 118, 110) : Color.FromArgb(203, 213, 225),
                ForeColor = quiz.CanStart ? Color.White : Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = quiz.CanStart ? "Start Quiz" : "Unavailable",
                Cursor = Cursors.Hand,
                Enabled = true,
                Top = 12,
                Left = 18,
                Tag = quiz
            };

            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += StartQuiz_Click;

            var lblAttemptInfo = new Label
            {
                AutoSize = false,
                Width = 160,
                Height = 48,
                Left = 18,
                Top = 64,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = quiz.HasCompletedAttempt
                    ? string.Format("Attempts: {0}\r\nBest score: {1:0.#}%", quiz.AttemptCount, quiz.BestScore.GetValueOrDefault())
                    : string.Format("Attempts: {0}\r\nBest score: Not graded yet", quiz.AttemptCount)
            };

            actionPanel.Controls.Add(lblAttemptInfo);
            actionPanel.Controls.Add(btnStart);

            var body = new Panel
            {
                Dock = DockStyle.Fill
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = quiz.Title
            };

            var lblMeta = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(51, 65, 85),
                Text = string.Format("{0} | {1} | {2} mins | {3} questions", quiz.SubjectName, quiz.Difficulty, quiz.DurationMinutes, quiz.QuestionCount)
            };

            var lblTopic = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = string.Format("Topic: {0}", quiz.Topic)
            };

            var lblAvailability = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = quiz.CanStart ? Color.FromArgb(15, 118, 110) : Color.FromArgb(185, 28, 28),
                Text = quiz.AvailabilityLabel
            };

            body.Controls.Add(lblAvailability);
            body.Controls.Add(lblTopic);
            body.Controls.Add(lblMeta);
            body.Controls.Add(lblTitle);

            panel.Controls.Add(body);
            panel.Controls.Add(actionPanel);
            return panel;
        }

        private void StartQuiz_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var quiz = button != null ? button.Tag as StudentQuizListItemDto : null;
            if (quiz == null)
            {
                return;
            }

            if (!quiz.CanStart)
            {
                MessageBox.Show(quiz.AvailabilityLabel, "Quiz Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(
                "Quiz discovery and start gating are now working. The timed exam screen is the next implementation step.",
                "Exam Flow Pending",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
