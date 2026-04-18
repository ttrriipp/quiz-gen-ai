using System;
using System.Windows.Forms;
using QuizGenAI.Enums;
using QuizGenAI.Forms.Student;
using QuizGenAI.Forms.Teacher;
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
                    MessageBox.Show(result.ErrorMessage, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hide();

                using (var nextForm = CreateLandingForm(result.Role, result.DisplayName))
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
                MessageBox.Show("Unable to complete login.\r\n\r\n" + errorDetails, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private Form CreateLandingForm(UserRole role, string displayName)
        {
            switch (role)
            {
                case UserRole.Admin:
                case UserRole.Teacher:
                    return new TeacherDashboardForm
                    {
                        Text = string.Format("Teacher Dashboard - {0}", displayName)
                    };

                case UserRole.Student:
                    return new StudentQuizzesForm
                    {
                        Text = string.Format("Student Quizzes - {0}", displayName)
                    };

                default:
                    throw new InvalidOperationException("Unsupported role.");
            }
        }
    }
}
