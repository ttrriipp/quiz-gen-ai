using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentResultsForm : Form
    {
        private static readonly Color HeroBg = Color.FromArgb(20, 99, 67);
        private static readonly Color HeroAccent = Color.FromArgb(229, 190, 77);
        private static readonly Color HeroBorder = Color.FromArgb(94, 255, 255, 255);
        private static readonly Color SurfaceBorder = Color.FromArgb(223, 228, 219);
        private static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
        private static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color TextSoftOnHero = Color.FromArgb(217, 228, 221);
        private static readonly Color RecommendationBg = Color.FromArgb(250, 251, 247);

        private readonly StudentAttemptSummaryDto _summary;
        private readonly ResultPdfExportService _pdfExportService = new ResultPdfExportService();

        public StudentResultsForm()
        {
            InitializeComponent();
        }

        public StudentResultsForm(StudentAttemptSummaryDto summary)
        {
            if (summary == null)
            {
                throw new ArgumentNullException("summary");
            }

            _summary = summary;
            InitializeComponent();
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
            Text = "Result Summary";

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 262F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));

            root.Controls.Add(BuildScoreHeroPanel(), 0, 0);
            root.Controls.Add(BuildRecommendationPanel(), 0, 1);
            root.Controls.Add(BuildActionPanel(), 0, 2);

            Controls.Add(root);
            ResumeLayout();
        }

        private Control BuildScoreHeroPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = HeroBg,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(28, 24, 28, 24),
                Margin = new Padding(0, 0, 0, 18)
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 78, 56),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = Color.Transparent
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var trophyHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var trophyPanel = new Panel
            {
                Size = new Size(62, 62),
                BackColor = HeroAccent
            };
            trophyPanel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Symbol", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 44, 35),
                Text = "★",
                TextAlign = ContentAlignment.MiddleCenter
            });
            trophyHost.Controls.Add(trophyPanel);
            trophyHost.Resize += delegate
            {
                trophyPanel.Location = new Point(
                    Math.Max(0, (trophyHost.ClientSize.Width - trophyPanel.Width) / 2),
                    Math.Max(0, (trophyHost.ClientSize.Height - trophyPanel.Height) / 2));
            };

            var effortLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = HeroAccent,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = GetEffortLabel()
            };

            var scoreLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 38F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = string.Format("{0:0.#}%", _summary.ScorePercentage)
            };

            var summaryLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14F),
                ForeColor = TextSoftOnHero,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = string.Format(
                    "{0} of {1} correct on {2}",
                    _summary.CorrectAnswers,
                    _summary.TotalQuestions,
                    _summary.QuizTitle)
            };

            var metaLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = TextSoftOnHero,
                TextAlign = ContentAlignment.TopCenter,
                Text = string.Format(
                    "{0} | {1}\r\nTime spent: {2} | Submitted: {3}",
                    _summary.SubjectName,
                    _summary.Topic,
                    _summary.TimeSpentDisplay,
                    _summary.SubmittedAtDisplay)
            };

            layout.Controls.Add(trophyHost, 0, 0);
            layout.Controls.Add(effortLabel, 0, 1);
            layout.Controls.Add(scoreLabel, 0, 2);
            layout.Controls.Add(summaryLabel, 0, 3);
            layout.Controls.Add(metaLabel, 0, 4);

            card.Controls.Add(layout);
            panel.Controls.Add(card);
            return panel;
        }

        private string GetEffortLabel()
        {
            if (_summary.ScorePercentage >= 85)
            {
                return "EXCELLENT WORK";
            }

            if (_summary.ScorePercentage >= 60)
            {
                return "GOOD EFFORT";
            }

            return "KEEP PRACTICING";
        }

        private Control BuildRecommendationPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(22, 20, 22, 20),
                Margin = new Padding(0, 0, 0, 18)
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.Transparent
            };

            var content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = RecommendationBg
            };

            if (_summary.Recommendations.Count == 0)
            {
                content.Controls.Add(new Label
                {
                    Width = 840,
                    Height = 52,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = TextMuted,
                    Text = "No study recommendations are available for this attempt yet."
                });
            }
            else
            {
                foreach (var item in _summary.Recommendations)
                {
                    content.Controls.Add(CreateRecommendationCard(item));
                }
            }

            headerPanel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = "AI Study Recommendations"
            });
            headerPanel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                Text = string.Format(
                    "Student: {0} | Answered: {1}/{2} | Source: {3}",
                    _summary.StudentName,
                    _summary.AnsweredQuestions,
                    _summary.TotalQuestions,
                    string.IsNullOrWhiteSpace(_summary.RecommendationSourceLabel) ? "Not available" : _summary.RecommendationSourceLabel)
            });

            panel.Controls.Add(content);
            panel.Controls.Add(headerPanel);
            return panel;
        }

        private Control BuildActionPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var btnAllResults = CreateActionButton("View All Results", Color.FromArgb(22, 88, 61), Color.White);
            btnAllResults.Click += delegate
            {
                DialogResult = DialogResult.Yes;
                Close();
            };

            var btnDownloadPdf = CreateActionButton("Download PDF", HeroAccent, Color.FromArgb(22, 44, 35));
            btnDownloadPdf.Click += DownloadPdf_Click;

            var btnBack = CreateActionButton("Back To Quizzes", Color.White, TextPrimary);
            btnBack.FlatAppearance.BorderColor = SurfaceBorder;
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            flow.Controls.Add(btnAllResults);
            flow.Controls.Add(btnDownloadPdf);
            flow.Controls.Add(btnBack);
            panel.Controls.Add(flow);
            return panel;
        }

        private void DownloadPdf_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF files (*.pdf)|*.pdf";
                dialog.DefaultExt = "pdf";
                dialog.AddExtension = true;
                dialog.FileName = ResultPdfExportService.BuildDefaultFileName(_summary);

                if (dialog.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    return;
                }

                try
                {
                    _pdfExportService.Export(_summary, dialog.FileName);
                    NotificationHelper.ShowSuccess(
                        this,
                        "PDF Exported",
                        string.Format("Saved result summary to {0}.", Path.GetFileName(dialog.FileName)));
                }
                catch (Exception ex)
                {
                    NotificationHelper.ShowError(this, "Export Failed", ex.Message);
                }
            }
        }

        private static Control CreateRecommendationCard(StudentRecommendationDto item)
        {
            var panel = new Panel
            {
                Width = 840,
                Height = 88,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(16, 12, 16, 12)
            };

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                Text = string.Format("Focus: {0}", item.Description)
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = TextPrimary,
                Text = item.Title
            });

            return panel;
        }

        private static Button CreateActionButton(string text, Color backColor, Color foreColor)
        {
            var button = new Button
            {
                Width = 154,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = text,
                Cursor = Cursors.Hand,
                Margin = new Padding(12, 0, 0, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }
    }
}
