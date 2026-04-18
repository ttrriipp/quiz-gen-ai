using System;
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
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
            AcceptButton = btnLogin;
            txtEmail.Text = "teacher@quizgenai.local";
            txtPassword.Text = "Teacher123!";
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
                        Text = string.Format("Teacher Dashboard - {0}", displayName)
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
