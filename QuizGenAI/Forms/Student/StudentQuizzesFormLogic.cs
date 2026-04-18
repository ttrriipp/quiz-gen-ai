using System.Windows.Forms;

namespace QuizGenAI.Forms.Student
{
    public partial class StudentQuizzesForm
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
                    Text = "Student shell is ready.\r\nQuiz list implementation comes next."
                });
            }
        }
    }
}
