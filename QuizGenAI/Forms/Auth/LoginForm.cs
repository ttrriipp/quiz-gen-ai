using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuizGenAI.Enums;
using QuizGenAI.Forms.Student;
using QuizGenAI.Forms.Teacher;
using QuizGenAI.Helpers;
using QuizGenAI.Services;

namespace QuizGenAI.Forms.Auth
{
    public partial class LoginForm : Form
    {
        private static readonly Color FieldBorder = Color.FromArgb(42, 107, 82);
        private static readonly Color FieldBorderFocus = Color.FromArgb(212, 175, 55);

        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            ApplyLoginFonts();
            if (DesignTimeHelper.IsInDesignMode(this))
            {
                return;
            }

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            _authService = new AuthService();
            AcceptButton = btnLogin;
            txtEmail.Text = "teacher@quizgenai.local";
            txtPassword.Text = "Teacher123!";

            WireFieldFocus(txtEmail, pnlEmailWrap);
            WireFieldFocus(txtPassword, pnlPasswordWrap);
        }

        private void ApplyLoginFonts()
        {
            Font = AppTheme.GetBodyFont(10F);
            lblBrand.Font = AppTheme.GetTitleFont(20F);
            lblTagline1.Font = AppTheme.GetTitleFont(11.25F);
            lblTaglineGold.Font = AppTheme.GetTitleFont(11.25F);
            lblTagline2.Font = AppTheme.GetTitleFont(11.25F);
            lblBadge.Font = AppTheme.GetBodyFont(8.25F, FontStyle.Bold);
            lblSubtitle.Font = AppTheme.GetBodyFont(9F);
            lblEmail.Font = AppTheme.GetBodyFont(9F);
            lblPassword.Font = AppTheme.GetBodyFont(9F);
            txtEmail.Font = AppTheme.GetBodyFont(10F);
            txtPassword.Font = AppTheme.GetBodyFont(10F);
            btnLogin.Font = AppTheme.GetBodyFont(10.5F, FontStyle.Bold);
            lblHint.Font = AppTheme.GetBodyFont(8.25F);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            CenterCard();
        }

        private void panelBackground_Resize(object sender, EventArgs e)
        {
            CenterCard();
        }

        private void CenterCard()
        {
            if (pnlCard == null || panelBackground == null)
            {
                return;
            }

            pnlCard.Left = Math.Max(0, (panelBackground.ClientSize.Width - pnlCard.Width) / 2);
            pnlCard.Top = Math.Max(0, (panelBackground.ClientSize.Height - pnlCard.Height) / 2);
        }

        private void panelBackground_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = panelBackground.ClientRectangle;
            var baseColor = Color.FromArgb(0, 43, 27);
            using (var b = new SolidBrush(baseColor))
            {
                g.FillRectangle(b, rect);
            }

            int glowW = (int)(rect.Width * 0.72f);
            int glowH = (int)(rect.Height * 0.58f);
            var glowRect = new Rectangle(rect.Width - glowW + 40, -80, glowW, glowH + 120);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glowRect);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.FromArgb(90, 35, 110, 85);
                    pgb.SurroundColors = new[] { Color.FromArgb(0, baseColor.R, baseColor.G, baseColor.B) };
                    g.FillPath(pgb, path);
                }
            }
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var bounds = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);
            const int radius = 14;
            using (var path = RoundedRect(bounds, radius))
            {
                using (var pen = new Pen(Color.FromArgb(100, 212, 175, 55), 1f))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
            var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void WireFieldFocus(TextBox box, Panel wrap)
        {
            box.Enter += (s, e) => { wrap.BackColor = FieldBorderFocus; };
            box.Leave += (s, e) => { wrap.BackColor = FieldBorder; };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            try
            {
                var result = _authService.Login(txtEmail.Text, txtPassword.Text);
                if (!result.IsSuccess)
                {
                    NotificationHelper.ShowWarning(this, "Login Failed", result.ErrorMessage);
                    return;
                }

                NotificationHelper.ShowSuccess(this, "Login Success", string.Format("Welcome back, {0}.", result.DisplayName));
                Hide();

                using (var nextForm = CreateLandingForm(result.UserId, result.Role, result.DisplayName))
                {
                    nextForm.ShowDialog(this);
                }

                Show();
                txtPassword.Clear();
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                var errorDetails = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                LoggingService.Error(ex, "Login flow failed unexpectedly.");
                NotificationHelper.ShowError(this, "Application Error", "Unable to complete login.\r\n\r\n" + errorDetails);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private Form CreateLandingForm(int userId, UserRole role, string displayName)
        {
            switch (role)
            {
                case UserRole.Admin:
                case UserRole.Teacher:
                    return new TeacherDashboardForm
                    {
                        CurrentUserId = userId,
                        DisplayName = displayName,
                        Text = string.Format("Teacher Panel - {0}", displayName)
                    };

                case UserRole.Student:
                    return new StudentQuizzesForm
                    {
                        CurrentUserId = userId,
                        DisplayName = displayName,
                        Text = string.Format("Student Quizzes - {0}", displayName)
                    };

                default:
                    throw new InvalidOperationException("Unsupported role.");
            }
        }
    }
}
