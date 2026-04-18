using System.Windows.Forms;

namespace QuizGenAI.Forms.Teacher
{
    public partial class TeacherDashboardForm
    {
        protected override void OnShown(System.EventArgs e)
        {
            base.OnShown(e);

            if (Controls.Count == 0)
            {
                Controls.Add(new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Text = "Teacher/Admin shell is ready.\r\nDashboard implementation comes next."
                });
            }
        }
    }
}
