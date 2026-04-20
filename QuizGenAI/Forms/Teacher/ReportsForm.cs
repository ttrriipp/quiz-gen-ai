using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using ScottPlot.WinForms;
using QuizGenAI.DTOs;
using QuizGenAI.Helpers;
using QuizGenAI.Services;
using ScottColor = ScottPlot.Color;

namespace QuizGenAI.Forms.Teacher
{
    public partial class ReportsForm : UserControl
    {
        private static readonly Color WorkspaceBg = Color.FromArgb(11, 48, 34);
        private static readonly Color CardBg = Color.FromArgb(16, 58, 44);
        private static readonly Color ChartPlotBg = Color.FromArgb(12, 46, 36);
        private static readonly Color SubtleBorder = Color.FromArgb(72, 255, 255, 255);
        private static readonly Color TextPrimary = Color.White;
        private static readonly Color TextMuted = Color.FromArgb(196, 210, 200);
        private static readonly Color AccentLine = Color.FromArgb(168, 230, 198);
        private static readonly Color Mustard = Color.FromArgb(212, 175, 80);
        private static readonly Color MustardBorder = Color.FromArgb(235, 205, 130);

        private readonly ReportService _reportService = new ReportService();
        private Panel _reportsScrollHost;
        private TableLayoutPanel _reportsRootLayout;
        private FormsPlot _averageScoreChart;
        private FormsPlot _subjectMasteryChart;

        public ReportsForm()
        {
            InitializeComponent();
            BuildLayout();
            Tag = AppTheme.SkipCognitaThemeTag;
            Load += ReportsForm_Load;
        }

        public int CurrentUserId { get; set; }
        public string DisplayName { get; set; }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            RenderReports();
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(InvalidateChartsAfterLayout));
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible || !IsHandleCreated)
            {
                return;
            }

            RenderReports();
            BeginInvoke(new Action(InvalidateChartsAfterLayout));
        }

        private void InvalidateChartsAfterLayout()
        {
            if (_averageScoreChart != null)
            {
                _averageScoreChart.PerformLayout();
                _averageScoreChart.Refresh();
            }

            if (_subjectMasteryChart != null)
            {
                _subjectMasteryChart.PerformLayout();
                _subjectMasteryChart.Refresh();
            }
        }

        private void BuildLayout()
        {
            SuspendLayout();
            Controls.Clear();

            BackColor = WorkspaceBg;
            ForeColor = TextPrimary;
            Font = new Font("Segoe UI", 10F);
            Size = new Size(1280, 900);

            _reportsScrollHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = WorkspaceBg,
                Padding = new Padding(20)
            };
            _reportsScrollHost.Resize += ReportsScrollHost_Resize;

            _reportsRootLayout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };
            _reportsRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            _reportsRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 204F));
            _reportsRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 1240F));

            _reportsRootLayout.Controls.Add(BuildHeaderPanel(), 0, 0);
            _reportsRootLayout.Controls.Add(BuildMetricRow(), 0, 1);
            _reportsRootLayout.Controls.Add(BuildContentRow(), 0, 2);

            _reportsScrollHost.Controls.Add(_reportsRootLayout);
            Controls.Add(_reportsScrollHost);
            ResumeLayout();
            SyncReportsRootWidth();
        }

        private void ReportsScrollHost_Resize(object sender, EventArgs e)
        {
            SyncReportsRootWidth();
        }

        private void SyncReportsRootWidth()
        {
            if (_reportsScrollHost == null || _reportsRootLayout == null)
            {
                return;
            }

            var inner = _reportsScrollHost.ClientSize.Width - _reportsScrollHost.Padding.Horizontal;
            _reportsRootLayout.Width = Math.Max(720, inner);
        }

        private Control BuildHeaderPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(22, 14, 22, 18)
            };

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            body.Controls.Add(new Label
            {
                Name = "lblReportsMeta",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = "Loading teacher report summary..."
            });

            body.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
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
                Margin = new Padding(0, 18, 0, 18),
                BackColor = Color.Transparent
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
                RowCount = 2,
                BackColor = Color.Transparent
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 920F));

            grid.Controls.Add(BuildAverageScoreTrendPanel(), 0, 0);
            grid.Controls.Add(BuildSubjectMasteryChartPanel(), 1, 0);
            grid.Controls.Add(BuildHardestQuestionsPanel(), 0, 1);
            grid.Controls.Add(BuildRecentSubmissionsPanel(), 1, 1);
            return grid;
        }

        private Control BuildAverageScoreTrendPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);

            _averageScoreChart = CreateTrendChart();

            var host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ChartPlotBg
            };

            var chartHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ChartPlotBg,
                Padding = new Padding(8, 8, 8, 8)
            };

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = "Average score over time",
                TextAlign = ContentAlignment.MiddleLeft
            };

            chartHost.Controls.Add(_averageScoreChart);
            host.Controls.Add(chartHost);
            host.Controls.Add(title);

            panel.Controls.Add(host);
            return panel;
        }

        private Control BuildSubjectMasteryChartPanel()
        {
            var panel = CreateSurfacePanel();
            panel.Padding = new Padding(18, 16, 18, 16);
            panel.Margin = new Padding(18, 0, 0, 0);

            _subjectMasteryChart = CreateSubjectChart();

            var host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ChartPlotBg
            };

            var chartHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ChartPlotBg,
                Padding = new Padding(8, 8, 8, 8)
            };

            var title = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = "Subject mastery",
                TextAlign = ContentAlignment.MiddleLeft
            };

            chartHost.Controls.Add(_subjectMasteryChart);
            host.Controls.Add(chartHost);
            host.Controls.Add(title);

            panel.Controls.Add(host);
            return panel;
        }

        private static FormsPlot CreateTrendChart()
        {
            var chart = new FormsPlot
            {
                BackColor = ChartPlotBg,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty
            };
            ApplyPlotTheme(chart.Plot);
            return chart;
        }

        private static FormsPlot CreateSubjectChart()
        {
            var chart = new FormsPlot
            {
                BackColor = ChartPlotBg,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty
            };
            ApplyPlotTheme(chart.Plot);
            return chart;
        }

        private static void ApplyPlotTheme(ScottPlot.Plot plot)
        {
            var figureColor = ScottColor.FromColor(WorkspaceBg);
            var dataColor = ScottColor.FromColor(ChartPlotBg);
            var axisColor = ScottColor.FromColor(TextMuted);
            var gridColor = ScottColor.FromColor(Color.FromArgb(48, 255, 255, 255));

            plot.FigureBackground.Color = figureColor;
            plot.DataBackground.Color = dataColor;
            plot.Axes.Color(axisColor);
            plot.Axes.FrameColor(ScottColor.FromColor(SubtleBorder));
            plot.Axes.DefaultGrid.MajorLineColor = gridColor;
            plot.Axes.DefaultGrid.MinorLineColor = gridColor;
            plot.HideLegend();
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
                Padding = new Padding(0, 8, 0, 0),
                BackColor = ChartPlotBg
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = "Hardest questions"
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
                Padding = new Padding(0, 8, 0, 0),
                BackColor = ChartPlotBg
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = "Latest submissions"
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

            UpdateAverageScoreChart(reports);
            UpdateSubjectMasteryChart(reports);
            if (_averageScoreChart != null)
            {
                _averageScoreChart.Refresh();
            }

            if (_subjectMasteryChart != null)
            {
                _subjectMasteryChart.Refresh();
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

                FitFlowCardWidths(hardestFlow, 340);
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
                    recentFlow.Controls.Add(CreateSubmissionTableHeader());
                    foreach (var item in reports.RecentSubmissions)
                    {
                        recentFlow.Controls.Add(CreateSubmissionTableRow(item));
                    }
                }

                FitFlowCardWidths(recentFlow, 320);
            }

            SyncReportsRootWidth();
        }

        private void UpdateAverageScoreChart(TeacherReportsDto reports)
        {
            var labels = reports.ScoreTrendByMonth.Select(m => m.MonthLabel).ToList();
            var values = reports.ScoreTrendByMonth.Select(m => m.AverageScore).ToList();
            var xs = Enumerable.Range(0, labels.Count).Select(i => (double)i).ToArray();
            var ys = values.Select(v => v ?? 0D).ToArray();
            var plot = _averageScoreChart.Plot;
            plot.Clear();
            ApplyPlotTheme(plot);

            if (labels.Count == 0)
            {
                _averageScoreChart.Refresh();
                return;
            }

            var scatter = plot.Add.ScatterLine(xs, ys);
            scatter.Color = ScottColor.FromColor(AccentLine);
            plot.Axes.Bottom.SetTicks(xs, labels.ToArray());
            plot.Axes.Margins(0.08, 0.12, 0.12, 0.18);
            plot.Axes.Left.Label.Text = "Average %";
            _averageScoreChart.Refresh();
        }

        private void UpdateSubjectMasteryChart(TeacherReportsDto reports)
        {
            var ordered = reports.SubjectPerformance.OrderBy(s => s.AverageScore).ToList();
            var values = ordered.Select(s => s.AverageScore).ToArray();
            var positions = Enumerable.Range(0, ordered.Count).Select(i => (double)i).ToArray();
            var labels = ordered.Select(s => s.SubjectName).ToArray();
            var plot = _subjectMasteryChart.Plot;
            plot.Clear();
            ApplyPlotTheme(plot);

            if (ordered.Count == 0)
            {
                _subjectMasteryChart.Refresh();
                return;
            }

            var bars = plot.Add.Bars(positions, values);
            foreach (var bar in bars.Bars)
            {
                bar.FillColor = ScottColor.FromColor(Mustard);
                bar.LineColor = ScottColor.FromColor(MustardBorder);
                bar.LineWidth = 1;
            }
            plot.Axes.Bottom.SetTicks(positions, labels);
            plot.Axes.Bottom.TickLabelStyle.Rotation = -30;
            plot.Axes.Margins(0.08, 0.12, 0, 0.18);
            plot.Axes.Left.Label.Text = "Average %";
            _subjectMasteryChart.Refresh();
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
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = note
            });

            panel.Controls.Add(new Label
            {
                Name = "metric_value_" + title.Replace(" ", "_"),
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = value
            });

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = title
            });

            return panel;
        }

        private static Control CreateHardQuestionCard(HardestQuestionDto item)
        {
            const int padH = 14;
            const int padV = 12;
            const int gapQuestionPill = 10;
            const int gapMeta = 6;

            var row = new Panel
            {
                Width = 520,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = CardBg,
                Padding = new Padding(padH, padV, padH, padV),
                AutoSize = false,
                MinimumSize = new Size(280, 52)
            };
            row.Paint += RoundedInsetRow_Paint;

            var qLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = item.QuestionText ?? string.Empty,
                UseCompatibleTextRendering = true
            };

            var metaLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = string.Format("{0} · {1} · {2} attempt(s)", item.QuizTitle ?? string.Empty, item.SubjectName ?? string.Empty, item.Attempts),
                UseCompatibleTextRendering = true
            };

            var pill = CreateRatePillPanel(string.Format("{0:0}% correct", item.CorrectRate), CorrectRatePillBack(item.CorrectRate), CorrectRatePillFore(item.CorrectRate));
            row.Controls.Add(qLabel);
            row.Controls.Add(metaLabel);
            row.Controls.Add(pill);

            EventHandler layoutHardest = delegate
            {
                var innerLeft = row.Padding.Left;
                var innerTop = row.Padding.Top;
                var innerW = row.ClientSize.Width - row.Padding.Horizontal;
                if (innerW < 120)
                {
                    return;
                }

                var pillW = pill.Width;
                var pillH = pill.Height;
                var qMaxW = Math.Max(80, innerW - pillW - gapQuestionPill);
                qLabel.MaximumSize = new Size(qMaxW, 0);
                qLabel.Location = new Point(innerLeft, innerTop);
                pill.Location = new Point(innerLeft + innerW - pillW, innerTop);
                var qTextH = qLabel.GetPreferredSize(new Size(qMaxW, 0)).Height;
                var qBlockH = Math.Max(qTextH, pillH);
                metaLabel.MaximumSize = new Size(innerW, 0);
                metaLabel.Location = new Point(innerLeft, innerTop + qBlockH + gapMeta);
                var bottom = Math.Max(metaLabel.Bottom, pill.Bottom);
                var nextH = bottom + row.Padding.Bottom;
                if (row.Height != nextH)
                {
                    row.Height = nextH;
                }
            };

            row.SizeChanged += layoutHardest;
            row.HandleCreated += layoutHardest;
            layoutHardest(row, EventArgs.Empty);

            return row;
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
                AutoSize = false,
                AutoEllipsis = false
            });

            return pill;
        }

        private static Color CorrectRatePillBack(double rate)
        {
            if (rate < 50)
            {
                return WorkspaceBg;
            }

            if (rate < 70)
            {
                return Mustard;
            }

            return CardBg;
        }

        private static Color CorrectRatePillFore(double rate)
        {
            if (rate < 50)
            {
                return TextPrimary;
            }

            if (rate < 70)
            {
                return WorkspaceBg;
            }

            return TextPrimary;
        }

        private static Color ScorePillBack(double score)
        {
            if (score < 60)
            {
                return WorkspaceBg;
            }

            if (score < 85)
            {
                return Mustard;
            }

            return CardBg;
        }

        private static Color ScorePillFore(double score)
        {
            if (score < 60)
            {
                return TextPrimary;
            }

            if (score < 85)
            {
                return WorkspaceBg;
            }

            return TextPrimary;
        }

        private static Control CreateSubmissionTableHeader()
        {
            const int padX = 12;
            const int padY = 8;
            var bar = new Panel
            {
                Width = 520,
                Height = 36,
                Margin = new Padding(0, 0, 0, 6),
                BackColor = CardBg
            };
            bar.Paint += RoundedInsetRow_Paint;

            var h1 = CreateHeaderCell("STUDENT");
            var h2 = CreateHeaderCell("QUIZ");
            var h3 = CreateHeaderCell("SCORE");
            bar.Controls.Add(h1);
            bar.Controls.Add(h2);
            bar.Controls.Add(h3);

            EventHandler arrange = delegate
            {
                var innerW = bar.ClientSize.Width - padX * 2;
                var innerH = bar.ClientSize.Height - padY * 2;
                if (innerW < 60 || innerH < 8)
                {
                    return;
                }

                var c0 = (int)Math.Floor(innerW * 0.30);
                var c1 = (int)Math.Floor(innerW * 0.46);
                var c2 = innerW - c0 - c1;
                h1.SetBounds(padX, padY, c0, innerH);
                h2.SetBounds(padX + c0, padY, c1, innerH);
                h3.SetBounds(padX + c0 + c1, padY, c2, innerH);
            };

            bar.HandleCreated += arrange;
            bar.Resize += arrange;
            arrange(bar, EventArgs.Empty);
            return bar;
        }

        private static Label CreateHeaderCell(string text)
        {
            return new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = text,
                UseCompatibleTextRendering = true
            };
        }

        private static Control CreateSubmissionTableRow(RecentSubmissionDto item)
        {
            const int padX = 12;
            const int padY = 6;
            var row = new Panel
            {
                Width = 520,
                Height = 46,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = ChartPlotBg
            };
            row.Paint += SubmissionRowDivider_Paint;

            var lblStudent = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = TextPrimary,
                BackColor = Color.Transparent,
                Text = item.StudentName ?? string.Empty,
                UseCompatibleTextRendering = true,
                AutoEllipsis = true
            };

            var lblQuiz = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = item.QuizTitle ?? string.Empty,
                UseCompatibleTextRendering = true,
                AutoEllipsis = true
            };

            var pill = CreateRatePillPanel(string.Format("{0:0}%", item.ScorePercentage), ScorePillBack(item.ScorePercentage), ScorePillFore(item.ScorePercentage));
            row.Controls.Add(lblStudent);
            row.Controls.Add(lblQuiz);
            row.Controls.Add(pill);

            EventHandler arrange = delegate
            {
                var innerW = row.ClientSize.Width - padX * 2;
                var innerH = row.ClientSize.Height - padY * 2;
                if (innerW < 80 || innerH < 10)
                {
                    return;
                }

                var c0 = (int)Math.Floor(innerW * 0.30);
                var c1 = (int)Math.Floor(innerW * 0.46);
                var c2 = innerW - c0 - c1;
                lblStudent.SetBounds(padX, padY, c0, innerH);
                lblQuiz.SetBounds(padX + c0, padY, c1, innerH);
                var pillY = padY + Math.Max(0, (innerH - pill.Height) / 2);
                pill.SetBounds(padX + c0 + c1 + Math.Max(0, c2 - pill.Width), pillY, pill.Width, pill.Height);
            };

            row.HandleCreated += arrange;
            row.Resize += arrange;
            arrange(row, EventArgs.Empty);
            return row;
        }

        private static void PillBorder_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            if (pnl.Width <= 1 || pnl.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
            const int r = 10;
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(SubtleBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static void RoundedInsetRow_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            if (pnl.Width <= 1 || pnl.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
            const int r = 10;
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(SubtleBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static void SubmissionRowDivider_Paint(object sender, PaintEventArgs e)
        {
            var pnl = (Panel)sender;
            using (var pen = new Pen(SubtleBorder, 1))
            {
                e.Graphics.DrawLine(pen, 12, pnl.Height - 1, pnl.Width - 12, pnl.Height - 1);
            }
        }

        private static Control CreateEmptyInfoCard(string message)
        {
            var panel = new Panel
            {
                Width = 470,
                Height = 84,
                BackColor = ChartPlotBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(14, 12, 14, 12)
            };
            panel.Paint += InsetCard_Paint;

            panel.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = TextMuted,
                BackColor = Color.Transparent,
                Text = message
            });

            return panel;
        }

        private static Panel CreateSurfacePanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0)
            };
            panel.Paint += SurfacePanel_PaintRoundedBorder;
            return panel;
        }

        private static void SurfacePanel_PaintRoundedBorder(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            if (panel.Width <= 1 || panel.Height <= 1)
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
                using (var pen = new Pen(SubtleBorder, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private static void InsetCard_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            if (panel.Width <= 1 || panel.Height <= 1)
            {
                return;
            }

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var pen = new Pen(SubtleBorder, 1))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }
    }
}
