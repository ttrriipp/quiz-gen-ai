namespace QuizGenAI.Forms.Student
{
    partial class StudentQuizzesForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlBranding;
        private System.Windows.Forms.Label lblBrandD;
        private System.Windows.Forms.Label lblRoleD;
        private System.Windows.Forms.Panel pnlNavD;
        private System.Windows.Forms.Panel pnlNavQuizzesD;
        private System.Windows.Forms.Label lblNavQuizzesD;
        private System.Windows.Forms.Panel pnlNavResultsD;
        private System.Windows.Forms.Label lblNavResultsD;
        private System.Windows.Forms.Panel pnlSideFooter;
        private System.Windows.Forms.Label lblGreetingD;
        private System.Windows.Forms.Panel pnlLogoutD;
        private System.Windows.Forms.Label lblLogoutD;
        private System.Windows.Forms.Panel pnlTopBar;
        private System.Windows.Forms.Label lblPageTitleD;
        private System.Windows.Forms.Label lblPageDescD;
        private System.Windows.Forms.Panel pnlContent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlBranding = new System.Windows.Forms.Panel();
            this.lblBrandD = new System.Windows.Forms.Label();
            this.lblRoleD = new System.Windows.Forms.Label();
            this.pnlNavD = new System.Windows.Forms.Panel();
            this.pnlNavQuizzesD = new System.Windows.Forms.Panel();
            this.lblNavQuizzesD = new System.Windows.Forms.Label();
            this.pnlNavResultsD = new System.Windows.Forms.Panel();
            this.lblNavResultsD = new System.Windows.Forms.Label();
            this.pnlSideFooter = new System.Windows.Forms.Panel();
            this.lblGreetingD = new System.Windows.Forms.Label();
            this.pnlLogoutD = new System.Windows.Forms.Panel();
            this.lblLogoutD = new System.Windows.Forms.Label();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.lblPageTitleD = new System.Windows.Forms.Label();
            this.lblPageDescD = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlBranding.SuspendLayout();
            this.pnlNavD.SuspendLayout();
            this.pnlNavQuizzesD.SuspendLayout();
            this.pnlNavResultsD.SuspendLayout();
            this.pnlSideFooter.SuspendLayout();
            this.pnlLogoutD.SuspendLayout();
            this.pnlTopBar.SuspendLayout();
            this.SuspendLayout();

            // pnlSidebar
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlSidebar.Controls.Add(this.pnlSideFooter);
            this.pnlSidebar.Controls.Add(this.pnlNavD);
            this.pnlSidebar.Controls.Add(this.pnlBranding);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 220;

            // pnlBranding
            this.pnlBranding.Controls.Add(this.lblRoleD);
            this.pnlBranding.Controls.Add(this.lblBrandD);
            this.pnlBranding.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBranding.Height = 112;
            this.pnlBranding.Padding = new System.Windows.Forms.Padding(20, 22, 20, 18);

            // lblBrandD
            this.lblBrandD.AutoSize = false;
            this.lblBrandD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBrandD.Font = new System.Drawing.Font("Segoe UI Semibold", 19F, System.Drawing.FontStyle.Bold);
            this.lblBrandD.ForeColor = System.Drawing.Color.White;
            this.lblBrandD.Height = 36;
            this.lblBrandD.Text = "QuizGen AI";

            // lblRoleD
            this.lblRoleD.AutoSize = false;
            this.lblRoleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRoleD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoleD.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblRoleD.Height = 22;
            this.lblRoleD.Text = "Student Workspace";

            // pnlNavD
            this.pnlNavD.Controls.Add(this.pnlNavResultsD);
            this.pnlNavD.Controls.Add(this.pnlNavQuizzesD);
            this.pnlNavD.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavD.Height = 142;
            this.pnlNavD.Padding = new System.Windows.Forms.Padding(16, 12, 16, 8);

            // pnlNavQuizzesD
            this.pnlNavQuizzesD.BackColor = System.Drawing.Color.FromArgb(3, 105, 161);
            this.pnlNavQuizzesD.Controls.Add(this.lblNavQuizzesD);
            this.pnlNavQuizzesD.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavQuizzesD.Height = 46;
            this.pnlNavQuizzesD.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);

            // lblNavQuizzesD
            this.lblNavQuizzesD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNavQuizzesD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblNavQuizzesD.ForeColor = System.Drawing.Color.White;
            this.lblNavQuizzesD.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblNavQuizzesD.Text = "Quizzes";
            this.lblNavQuizzesD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlNavResultsD
            this.pnlNavResultsD.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlNavResultsD.Controls.Add(this.lblNavResultsD);
            this.pnlNavResultsD.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavResultsD.Height = 46;

            // lblNavResultsD
            this.lblNavResultsD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNavResultsD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblNavResultsD.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblNavResultsD.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblNavResultsD.Text = "Results";
            this.lblNavResultsD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlSideFooter
            this.pnlSideFooter.Controls.Add(this.lblGreetingD);
            this.pnlSideFooter.Controls.Add(this.pnlLogoutD);
            this.pnlSideFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSideFooter.Height = 126;
            this.pnlSideFooter.Padding = new System.Windows.Forms.Padding(20, 12, 20, 16);

            // pnlLogoutD
            this.pnlLogoutD.BackColor = System.Drawing.Color.FromArgb(185, 28, 28);
            this.pnlLogoutD.Controls.Add(this.lblLogoutD);
            this.pnlLogoutD.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogoutD.Height = 38;

            // lblLogoutD
            this.lblLogoutD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogoutD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogoutD.ForeColor = System.Drawing.Color.White;
            this.lblLogoutD.Text = "Logout";
            this.lblLogoutD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblGreetingD
            this.lblGreetingD.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGreetingD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGreetingD.ForeColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.lblGreetingD.Height = 44;
            this.lblGreetingD.Text = "Signed in as Student";
            this.lblGreetingD.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            // pnlTopBar
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            this.pnlTopBar.Controls.Add(this.lblPageDescD);
            this.pnlTopBar.Controls.Add(this.lblPageTitleD);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Height = 102;
            this.pnlTopBar.Padding = new System.Windows.Forms.Padding(28, 16, 28, 14);

            // lblPageTitleD
            this.lblPageTitleD.AutoSize = false;
            this.lblPageTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPageTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblPageTitleD.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblPageTitleD.Height = 40;
            this.lblPageTitleD.Text = "Quizzes";

            // lblPageDescD
            this.lblPageDescD.AutoSize = false;
            this.lblPageDescD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPageDescD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPageDescD.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblPageDescD.Height = 28;
            this.lblPageDescD.Text = "Pick a quiz to start. Your progress is saved automatically.";

            // pnlContent
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Padding = new System.Windows.Forms.Padding(28, 24, 28, 28);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.ClientSize = new System.Drawing.Size(1220, 780);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Workspace";

            this.pnlLogoutD.ResumeLayout(false);
            this.pnlSideFooter.ResumeLayout(false);
            this.pnlNavResultsD.ResumeLayout(false);
            this.pnlNavQuizzesD.ResumeLayout(false);
            this.pnlNavD.ResumeLayout(false);
            this.pnlBranding.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTopBar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
