using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizGenAI.DTOs;
using QuizGenAI.Enums;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Teacher
{
    public class AiQuizGeneratorForm : Form
    {
        private readonly AiQuizService _aiQuizService;
        private readonly QuizService _quizService;
        private ComboBox _cmbSubject;
        private ComboBox _cmbDifficulty;
        private NumericUpDown _nudQuestionCount;
        private NumericUpDown _nudDuration;
        private TextBox _txtTopic;
        private Label _lblNote;
        private Button _btnGenerate;

        public AiQuizGeneratorForm()
        {
            _aiQuizService = new AiQuizService();
            _quizService = new QuizService();
            BuildLayout();
            AppTheme.ApplyCognitaTheme(this);
        }

        public AiQuizGenerationResultDto GeneratedResult { get; private set; }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadLookups();
        }

        private void BuildLayout()
        {
            SuspendLayout();
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(640, 420);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New AI Quiz";

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 88,
                BackColor = Color.White,
                Padding = new Padding(20, 18, 20, 14)
            };

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Text = "Generate MCQ drafts, review them in the editor, and save only after teacher review."
            });

            header.Controls.Add(new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Text = "AI Quiz Generator"
            });

            var body = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (var i = 0; i < 6; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _cmbSubject = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _txtTopic = new TextBox { Dock = DockStyle.Fill };
            _cmbDifficulty = new ComboBox { Dock = DockStyle.Left, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            _nudQuestionCount = new NumericUpDown { Dock = DockStyle.Left, Width = 100, Minimum = 1, Maximum = 20, Value = 5 };
            _nudDuration = new NumericUpDown { Dock = DockStyle.Left, Width = 100, Minimum = 0, Maximum = 180, Value = 15 };

            foreach (QuizDifficulty difficulty in Enum.GetValues(typeof(QuizDifficulty)))
            {
                _cmbDifficulty.Items.Add(difficulty);
            }
            _cmbDifficulty.SelectedIndex = 0;

            _lblNote = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Text = "If no AI endpoint is configured, the app uses a demo fallback generator so the review-first workflow remains testable."
            };

            layout.Controls.Add(CreateFieldLabel("Subject"), 0, 0);
            layout.Controls.Add(_cmbSubject, 1, 0);
            layout.Controls.Add(CreateFieldLabel("Topic"), 0, 1);
            layout.Controls.Add(_txtTopic, 1, 1);
            layout.Controls.Add(CreateFieldLabel("Difficulty"), 0, 2);
            layout.Controls.Add(_cmbDifficulty, 1, 2);
            layout.Controls.Add(CreateFieldLabel("Question Count"), 0, 3);
            layout.Controls.Add(_nudQuestionCount, 1, 3);
            layout.Controls.Add(CreateFieldLabel("Duration"), 0, 4);
            layout.Controls.Add(_nudDuration, 1, 4);
            layout.Controls.Add(CreateFieldLabel("Notes"), 0, 5);
            layout.Controls.Add(_lblNote, 1, 5);

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48
            };

            _btnGenerate = CreatePrimaryButton("Generate");
            _btnGenerate.Width = 120;
            _btnGenerate.Location = new Point(0, 6);
            _btnGenerate.Click += async delegate { await GenerateAsync(); };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Width = 92,
                Height = 34,
                Location = new Point(132, 6),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };

            buttonPanel.Controls.Add(_btnGenerate);
            buttonPanel.Controls.Add(btnCancel);

            card.Controls.Add(buttonPanel);
            card.Controls.Add(layout);
            body.Controls.Add(card);
            Controls.Add(body);
            Controls.Add(header);
            ResumeLayout();
        }

        private void LoadLookups()
        {
            if (_cmbSubject.Items.Count > 0)
            {
                return;
            }

            foreach (var subject in _quizService.GetSubjects())
            {
                _cmbSubject.Items.Add(subject);
            }

            if (_cmbSubject.Items.Count > 0)
            {
                _cmbSubject.SelectedIndex = 0;
            }
        }

        private async Task GenerateAsync()
        {
            _btnGenerate.Enabled = false;

            try
            {
                var subject = _cmbSubject.SelectedItem as SubjectOptionDto;
                if (subject == null)
                {
                    throw new InvalidOperationException("Select a subject.");
                }

                var request = new AiQuizRequestDto
                {
                    SubjectId = subject.Id,
                    SubjectName = subject.Name,
                    Topic = _txtTopic.Text,
                    Difficulty = _cmbDifficulty.SelectedItem is QuizDifficulty ? (QuizDifficulty)_cmbDifficulty.SelectedItem : QuizDifficulty.Easy,
                    QuestionCount = Convert.ToInt32(_nudQuestionCount.Value),
                    DurationMinutes = Convert.ToInt32(_nudDuration.Value)
                };

                GeneratedResult = await _aiQuizService.GenerateQuizAsync(request);
                NotificationHelper.ShowSuccess(
                    this,
                    GeneratedResult.UsedFallback ? "AI Draft Ready (Fallback)" : "AI Draft Ready",
                    GeneratedResult.UsedFallback
                        ? "The demo fallback generated a draft for review."
                        : "The AI generated a draft quiz for review.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex, "AI quiz generation failed in the UI flow.");
                NotificationHelper.ShowError(this, "AI Generation Failed", ex.Message);
            }
            finally
            {
                _btnGenerate.Enabled = true;
            }
        }

        private static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                Text = text
            };
        }

        private static Button CreatePrimaryButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Height = 34,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }
    }
}
