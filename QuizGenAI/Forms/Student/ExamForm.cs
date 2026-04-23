using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Student
{
    public partial class ExamForm : Form
    {
        private static readonly Color ReportsCardBg = Color.FromArgb(16, 58, 44);
        private static readonly Color ReportsInnerBg = Color.FromArgb(12, 46, 36);
        private static readonly Color ReportsBorder = Color.FromArgb(72, 255, 255, 255);
        private static readonly Color ReportsTextPrimary = Color.White;
        private static readonly Color ReportsTextMuted = Color.FromArgb(196, 210, 200);
        private static readonly Color ReportsAccent = Color.FromArgb(168, 230, 198);
        private static readonly Color ReportsSuccess = Color.FromArgb(34, 197, 94);
        private static readonly Color ReportsMustard = Color.FromArgb(212, 175, 80);
        private static readonly Color ReportsMustardBorder = Color.FromArgb(235, 205, 130);

        private readonly ExamService _examService = new ExamService();
        private readonly RecommendationService _recommendationService = new RecommendationService();
        private readonly int _studentId;
        private readonly int _attemptId;
        private readonly Timer _examTimer;
        private readonly List<Button> _trackerButtons = new List<Button>();
        private ExamSessionDto _session;
        private Label _lblTimerTitle;
        private Label _lblTimerValue;
        private Label _lblQuestionMeta;
        private Label _lblQuestionText;
        private FlowLayoutPanel _choicesPanel;
        private Label _lblProgressSummary;
        private Button _btnPrevious;
        private Button _btnPrimaryAction;
        private int _currentQuestionIndex;
        private int _remainingSeconds;
        private bool _isSubmitting;
        private bool _timeWarningShown;

        public ExamForm()
        {
            InitializeComponent();
        }

        public ExamForm(int studentId, int attemptId)
        {
            _studentId = studentId;
            _attemptId = attemptId;
            _examTimer = new Timer { Interval = 1000 };
            _examTimer.Tick += ExamTimer_Tick;

            InitializeComponent();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                LoadSession();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "Unable to load exam session. AttemptId={AttemptId} StudentId={StudentId}", _attemptId, _studentId);
                NotificationHelper.ShowError(this, "Unable To Start Exam", ex.Message);
                Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_isSubmitting && _session != null && DialogResult != DialogResult.OK)
            {
                var result = MessageBox.Show(
                    "This exam is still in progress. You can reopen it later and continue from the same attempt.\r\n\r\nClose the exam window now?",
                    "Leave Exam",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _examTimer.Stop();
            base.OnFormClosing(e);
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
            Text = "Exam";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(20)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));

            var header = BuildHeaderPanel();
            root.Controls.Add(header, 0, 0);
            root.SetColumnSpan(header, 2);

            var questionPanel = BuildQuestionPanel();
            root.Controls.Add(questionPanel, 0, 1);

            var trackerPanel = BuildTrackerPanel();
            root.Controls.Add(trackerPanel, 1, 1);

            var footer = BuildFooterPanel();
            root.Controls.Add(footer, 0, 2);
            root.SetColumnSpan(footer, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildHeaderPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 18, 20, 18);

            var timerCard = new Panel
            {
                Dock = DockStyle.Right,
                Width = 210,
                BackColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(18, 12, 18, 12)
            };

            _lblTimerTitle = new Label
            {
                AutoSize = false,
                Left = 18,
                Top = 10,
                Width = 170,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                Text = "Time Remaining",
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblTimerValue = new Label
            {
                AutoSize = false,
                Left = 18,
                Top = 40,
                Width = 170,
                Height = 40,
                Font = new Font("Consolas", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "00:00",
                TextAlign = ContentAlignment.MiddleLeft
            };

            timerCard.Controls.Add(_lblTimerValue);
            timerCard.Controls.Add(_lblTimerTitle);

            var body = new Panel
            {
                Dock = DockStyle.Fill
            };

            var lblTitle = new Label
            {
                Name = "lblExamTitle",
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 21F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Exam"
            };

            var lblMeta = new Label
            {
                Name = "lblExamMeta",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "Loading exam details..."
            };

            body.Controls.Add(lblMeta);
            body.Controls.Add(lblTitle);

            panel.Controls.Add(body);
            panel.Controls.Add(timerCard);
            return panel;
        }

        private Control BuildQuestionPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(22, 20, 22, 20);

            _lblQuestionMeta = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 118, 110),
                Text = "Question"
            };

            _lblQuestionText = new Label
            {
                Dock = DockStyle.Top,
                Height = 110,
                Font = new Font("Segoe UI", 13F),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Loading question...",
                Padding = new Padding(0, 8, 0, 12)
            };

            _choicesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 4, 0, 0),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(_choicesPanel);
            panel.Controls.Add(_lblQuestionText);
            panel.Controls.Add(_lblQuestionMeta);
            return panel;
        }

        private Control BuildTrackerPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 18, 18, 18);

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Text = "Question Tracker"
            };

            _lblProgressSummary = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "Loading progress..."
            };

            var trackerFlow = new FlowLayoutPanel
            {
                Name = "trackerFlow",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0)
            };

            panel.Controls.Add(trackerFlow);
            panel.Controls.Add(_lblProgressSummary);
            panel.Controls.Add(lblTitle);
            return panel;
        }

        private Control BuildFooterPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(20, 14, 20, 14);

            var actionPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0)
            };

            _btnPrimaryAction = new Button
            {
                Width = 150,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(3, 105, 161),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Next",
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 12, 0)
            };
            _btnPrimaryAction.FlatAppearance.BorderSize = 0;
            _btnPrimaryAction.Click += BtnPrimaryAction_Click;

            _btnPrevious = new Button
            {
                Width = 116,
                Height = 44,
                Margin = new Padding(0, 0, 12, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Previous",
                Cursor = Cursors.Hand
            };
            _btnPrevious.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnPrevious.Click += BtnPrevious_Click;

            var leftNote = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = "Selections are saved as you move between questions."
            };

            actionPanel.Controls.Add(_btnPrimaryAction);
            actionPanel.Controls.Add(_btnPrevious);

            panel.Controls.Add(actionPanel);
            panel.Controls.Add(leftNote);
            return panel;
        }

        private void LoadSession()
        {
            _session = _examService.GetExamSession(_studentId, _attemptId);
            var lblTitle = Controls.Find("lblExamTitle", true).FirstOrDefault() as Label;
            var lblMeta = Controls.Find("lblExamMeta", true).FirstOrDefault() as Label;
            var trackerFlow = Controls.Find("trackerFlow", true).FirstOrDefault() as FlowLayoutPanel;

            if (lblTitle != null)
            {
                lblTitle.Text = _session.QuizTitle;
            }

            if (lblMeta != null)
            {
                lblMeta.Text = string.Format("{0} | {1} minutes | {2} questions | Topic: {3}", _session.SubjectName, _session.DurationMinutes, _session.Questions.Count, _session.Topic);
            }

            if (trackerFlow == null)
            {
                throw new InvalidOperationException("Exam tracker could not be initialized.");
            }

            trackerFlow.Controls.Clear();
            _trackerButtons.Clear();

            for (var i = 0; i < _session.Questions.Count; i++)
            {
                var trackerButton = new Button
                {
                    Width = 52,
                    Height = 42,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(0, 0, 10, 10),
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    Text = (i + 1).ToString(),
                    Tag = i,
                    Cursor = Cursors.Hand
                };

                trackerButton.Click += TrackerButton_Click;
                trackerFlow.Controls.Add(trackerButton);
                _trackerButtons.Add(trackerButton);
            }

            var elapsed = Math.Max(0, (int)Math.Floor((DateTime.Now - _session.StartedAtLocal).TotalSeconds));
            _remainingSeconds = Math.Max(0, (_session.DurationMinutes * 60) - elapsed);

            if (_remainingSeconds == 0)
            {
                AutoSubmitDueToTime();
                return;
            }

            UpdateTimerDisplay();
            _examTimer.Start();
            _currentQuestionIndex = 0;
            RenderQuestion();
            UpdateTrackerState();
        }

        private void RenderQuestion()
        {
            if (_session == null || !_session.Questions.Any())
            {
                return;
            }

            var question = _session.Questions[_currentQuestionIndex];
            _lblQuestionMeta.Text = string.Format("Question {0} of {1}", _currentQuestionIndex + 1, _session.Questions.Count);
            _lblQuestionText.Text = question.Text;

            _choicesPanel.SuspendLayout();
            _choicesPanel.Controls.Clear();

            foreach (var choice in question.Choices)
            {
                var isSelected = question.SelectedChoiceId == choice.Id;
                var optionPanel = new Panel
                {
                    Width = Math.Max(640, _choicesPanel.ClientSize.Width - 30),
                    Height = 72,
                    BackColor = isSelected ? ReportsCardBg : ReportsInnerBg,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0, 0, 0, 12),
                    Padding = new Padding(16, 12, 16, 12),
                    Tag = choice.Id,
                    Cursor = Cursors.Hand
                };
                if (isSelected)
                {
                    optionPanel.Paint += ChoiceCardSelected_Paint;
                }
                else
                {
                    optionPanel.Paint += ChoiceCard_Paint;
                }
                optionPanel.Click += ChoiceCard_Click;

                var radioButton = new RadioButton
                {
                    AutoSize = false,
                    Dock = DockStyle.Left,
                    Width = 28,
                    Font = new Font("Segoe UI", 10.5F),
                    Checked = question.SelectedChoiceId == choice.Id,
                    Tag = choice.Id,
                    BackColor = Color.Transparent,
                    ForeColor = ReportsTextPrimary,
                    Cursor = Cursors.Hand
                };
                radioButton.CheckedChanged += ChoiceRadio_CheckedChanged;
                radioButton.Click += ChoiceCard_Click;

                var textLabel = new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = ReportsTextPrimary,
                    BackColor = Color.Transparent,
                    Text = choice.Text,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true,
                    Cursor = Cursors.Hand,
                    Tag = choice.Id
                };
                textLabel.Click += ChoiceCard_Click;

                if (isSelected)
                {
                    var selectedBadge = CreateSelectedChoiceBadge();
                    selectedBadge.Dock = DockStyle.Right;
                    optionPanel.Controls.Add(selectedBadge);
                }

                optionPanel.Controls.Add(textLabel);
                optionPanel.Controls.Add(radioButton);
                _choicesPanel.Controls.Add(optionPanel);
            }

            _choicesPanel.ResumeLayout();

            _btnPrevious.Enabled = _currentQuestionIndex > 0;
            _btnPrimaryAction.Text = _currentQuestionIndex >= _session.Questions.Count - 1 ? "Submit" : "Next";
            UpdateTrackerState();
        }

        private void UpdateTrackerState()
        {
            if (_session == null)
            {
                return;
            }

            for (var i = 0; i < _trackerButtons.Count; i++)
            {
                var trackerButton = _trackerButtons[i];
                var question = _session.Questions[i];
                var isCurrent = i == _currentQuestionIndex;
                var isAnswered = question.SelectedChoiceId.HasValue;

                trackerButton.BackColor = isCurrent
                    ? Color.FromArgb(3, 105, 161)
                    : isAnswered
                        ? Color.FromArgb(15, 118, 110)
                        : Color.White;

                trackerButton.ForeColor = isCurrent || isAnswered
                    ? Color.White
                    : Color.FromArgb(51, 65, 85);

                trackerButton.FlatAppearance.BorderColor = isCurrent
                    ? Color.FromArgb(3, 105, 161)
                    : Color.FromArgb(203, 213, 225);
            }

            var answered = _session.Questions.Count(x => x.SelectedChoiceId.HasValue);
            var unanswered = _session.Questions.Count - answered;
            _lblProgressSummary.Text = string.Format("Answered: {0}\r\nUnanswered: {1}", answered, unanswered);
        }

        private void ChoiceRadio_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked || _session == null)
            {
                return;
            }

            var choiceId = Convert.ToInt32(radioButton.Tag);
            SaveCurrentSelection(choiceId);
        }

        private void ChoiceCard_Click(object sender, EventArgs e)
        {
            if (_session == null)
            {
                return;
            }

            var source = sender as Control;
            if (source == null || source.Tag == null)
            {
                return;
            }

            SaveCurrentSelection(Convert.ToInt32(source.Tag));
            RenderQuestion();
        }

        private void SaveCurrentSelection(int? selectedChoiceId)
        {
            if (_session == null)
            {
                return;
            }

            var question = _session.Questions[_currentQuestionIndex];
            if (question.SelectedChoiceId == selectedChoiceId)
            {
                return;
            }

            _examService.SaveAnswer(_studentId, _session.AttemptId, question.Id, selectedChoiceId);
            question.SelectedChoiceId = selectedChoiceId;
            UpdateTrackerState();
        }

        private void TrackerButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            _currentQuestionIndex = Convert.ToInt32(button.Tag);
            RenderQuestion();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentQuestionIndex <= 0)
            {
                return;
            }

            _currentQuestionIndex--;
            RenderQuestion();
        }

        private void BtnPrimaryAction_Click(object sender, EventArgs e)
        {
            if (_session == null)
            {
                return;
            }

            if (_currentQuestionIndex >= _session.Questions.Count - 1)
            {
                ReviewBeforeSubmit();
                return;
            }

            _currentQuestionIndex++;
            RenderQuestion();
        }

        private void ReviewBeforeSubmit()
        {
            if (_session == null || _isSubmitting)
            {
                return;
            }

            var answered = _session.Questions.Count(x => x.SelectedChoiceId.HasValue);
            var unanswered = _session.Questions.Count - answered;
            var message = string.Format(
                "Answered questions: {0}\r\nUnanswered questions: {1}\r\n\r\nSubmit this exam now?",
                answered,
                unanswered);

            var result = MessageBox.Show(message, "Review Answers", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            SubmitExam(false);
        }

        private void ExamTimer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds = Math.Max(0, _remainingSeconds - 1);
            UpdateTimerDisplay();

            if (!_timeWarningShown && _remainingSeconds > 0 && _remainingSeconds <= 60)
            {
                _timeWarningShown = true;
                NotificationHelper.ShowWarning(this, "Time Warning", "One minute remaining. Review your unanswered questions now.");
            }

            if (_remainingSeconds == 0)
            {
                AutoSubmitDueToTime();
            }
        }

        private void AutoSubmitDueToTime()
        {
            if (_isSubmitting)
            {
                return;
            }

            NotificationHelper.ShowInfo(this, "Exam Time Ended", "Time is up. Your exam will be submitted automatically.");
            SubmitExam(true);
        }

        private void SubmitExam(bool autoSubmitted)
        {
            if (_session == null || _isSubmitting)
            {
                return;
            }

            _isSubmitting = true;
            _examTimer.Stop();

            try
            {
                var result = _examService.SubmitAttempt(_studentId, _session.AttemptId, autoSubmitted);
                var summary = _examService.GetAttemptSummary(_studentId, result.AttemptId);
                var recommendations = _recommendationService.GetRecommendationsForAttempt(_studentId, result.AttemptId);
                summary.Recommendations = recommendations.Recommendations;
                summary.UsedFallbackRecommendations = recommendations.UsedFallback;
                summary.RecommendationSourceLabel = recommendations.SourceLabel;
                summary.WeakAreaSummary = recommendations.WeakAreaSummary;
                NotificationHelper.ShowSuccess(this, "Exam Submitted", "Your answers were submitted and scored successfully.");

                var openAllResults = false;
                using (var resultsForm = new StudentResultsForm(summary))
                {
                    resultsForm.Text = string.Format("Results - {0}", summary.QuizTitle);
                    openAllResults = resultsForm.ShowDialog(this) == DialogResult.Yes;
                }

                DialogResult = openAllResults ? DialogResult.Yes : DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _isSubmitting = false;
                _examTimer.Start();
                LoggingService.Error(ex, "Unable to submit exam. AttemptId={AttemptId} StudentId={StudentId}", _session != null ? _session.AttemptId : 0, _studentId);
                NotificationHelper.ShowError(this, "Unable To Submit Exam", ex.Message);
            }
        }

        private void UpdateTimerDisplay()
        {
            var safeSeconds = Math.Max(0, _remainingSeconds);
            var timeSpan = TimeSpan.FromSeconds(safeSeconds);
            _lblTimerValue.Text = string.Format("{0:D2}:{1:D2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
            _lblTimerValue.ForeColor = safeSeconds <= 60 ? Color.FromArgb(254, 202, 202) : Color.White;
            _lblTimerTitle.ForeColor = safeSeconds <= 60 ? Color.FromArgb(254, 202, 202) : Color.FromArgb(148, 163, 184);
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

        private static void ChoiceCard_Paint(object sender, PaintEventArgs e)
        {
            PaintRoundedBorder(sender as Panel, e, ReportsBorder, 10);
        }

        private static void ChoiceCardSelected_Paint(object sender, PaintEventArgs e)
        {
            PaintRoundedBorder(sender as Panel, e, ReportsMustardBorder, 10);
        }

        private static void PaintRoundedBorder(Panel panel, PaintEventArgs e, Color borderColor, int radius)
        {
            if (panel == null || panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(borderColor, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static Control CreateSelectedChoiceBadge()
        {
            var panel = new Panel
            {
                Width = 118,
                Dock = DockStyle.Right,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };

            var chip = new Panel
            {
                Width = 94,
                Height = 28,
                BackColor = ReportsSuccess,
                Margin = Padding.Empty,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            chip.Paint += SelectedBadge_Paint;

            var icon = new Label
            {
                Dock = DockStyle.Left,
                Width = 24,
                Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Text = "\u2713",
                TextAlign = ContentAlignment.MiddleCenter
            };

            var text = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Text = "SELECTED",
                TextAlign = ContentAlignment.MiddleLeft
            };

            chip.Controls.Add(text);
            chip.Controls.Add(icon);
            panel.Controls.Add(chip);
            panel.Resize += delegate
            {
                chip.Location = new Point(Math.Max(0, panel.ClientSize.Width - chip.Width), Math.Max(0, (panel.ClientSize.Height - chip.Height) / 2));
            };

            return panel;
        }

        private static void SelectedBadge_Paint(object sender, PaintEventArgs e)
        {
            PaintRoundedBorder(sender as Panel, e, Color.FromArgb(190, 255, 255, 255), 10);
        }
    }
}
