namespace QuizGenAI.Forms.Teacher
{
    partial class TeacherDashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTDSidebar;
        private System.Windows.Forms.Panel pnlTDBranding;
        private System.Windows.Forms.Label lblTDBrand;
        private System.Windows.Forms.Label lblTDRole;
        private System.Windows.Forms.Panel pnlTDNav;
        private System.Windows.Forms.Panel pnlTDNavDashboard;
        private System.Windows.Forms.Label lblTDNavDashboard;
        private System.Windows.Forms.Panel pnlTDNavQuizzes;
        private System.Windows.Forms.Label lblTDNavQuizzes;
        private System.Windows.Forms.Panel pnlTDNavReports;
        private System.Windows.Forms.Label lblTDNavReports;
        private System.Windows.Forms.Panel pnlTDNavLogs;
        private System.Windows.Forms.Label lblTDNavLogs;
        private System.Windows.Forms.Panel pnlTDSideFooter;
        private System.Windows.Forms.Label lblTDGreeting;
        private System.Windows.Forms.Panel pnlTDLogout;
        private System.Windows.Forms.Label lblTDLogoutText;
        private System.Windows.Forms.Panel pnlTDTopBar;
        private System.Windows.Forms.Label lblTDPageTitle;
        private System.Windows.Forms.Label lblTDPageDesc;
        private System.Windows.Forms.Panel pnlTDContent;

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
            this.pnlTDSidebar = new System.Windows.Forms.Panel();
            this.pnlTDBranding = new System.Windows.Forms.Panel();
            this.lblTDBrand = new System.Windows.Forms.Label();
            this.lblTDRole = new System.Windows.Forms.Label();
            this.pnlTDNav = new System.Windows.Forms.Panel();
            this.pnlTDNavDashboard = new System.Windows.Forms.Panel();
            this.lblTDNavDashboard = new System.Windows.Forms.Label();
            this.pnlTDNavQuizzes = new System.Windows.Forms.Panel();
            this.lblTDNavQuizzes = new System.Windows.Forms.Label();
            this.pnlTDNavReports = new System.Windows.Forms.Panel();
            this.lblTDNavReports = new System.Windows.Forms.Label();
            this.pnlTDNavLogs = new System.Windows.Forms.Panel();
            this.lblTDNavLogs = new System.Windows.Forms.Label();
            this.pnlTDSideFooter = new System.Windows.Forms.Panel();
            this.lblTDGreeting = new System.Windows.Forms.Label();
            this.pnlTDLogout = new System.Windows.Forms.Panel();
            this.lblTDLogoutText = new System.Windows.Forms.Label();
            this.pnlTDTopBar = new System.Windows.Forms.Panel();
            this.lblTDPageTitle = new System.Windows.Forms.Label();
            this.lblTDPageDesc = new System.Windows.Forms.Label();
            this.pnlTDContent = new System.Windows.Forms.Panel();
            this.pnlTDSidebar.SuspendLayout();
            this.pnlTDBranding.SuspendLayout();
            this.pnlTDNav.SuspendLayout();
            this.pnlTDNavDashboard.SuspendLayout();
            this.pnlTDNavQuizzes.SuspendLayout();
            this.pnlTDNavReports.SuspendLayout();
            this.pnlTDNavLogs.SuspendLayout();
            this.pnlTDSideFooter.SuspendLayout();
            this.pnlTDLogout.SuspendLayout();
            this.pnlTDTopBar.SuspendLayout();
            this.SuspendLayout();

            // pnlTDSidebar
            this.pnlTDSidebar.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlTDSidebar.Controls.Add(this.pnlTDSideFooter);
            this.pnlTDSidebar.Controls.Add(this.pnlTDNav);
            this.pnlTDSidebar.Controls.Add(this.pnlTDBranding);
            this.pnlTDSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTDSidebar.Width = 240;

            // pnlTDBranding
            this.pnlTDBranding.Controls.Add(this.lblTDRole);
            this.pnlTDBranding.Controls.Add(this.lblTDBrand);
            this.pnlTDBranding.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDBranding.Height = 120;
            this.pnlTDBranding.Padding = new System.Windows.Forms.Padding(20, 24, 20, 16);

            // lblTDBrand
            this.lblTDBrand.AutoSize = false;
            this.lblTDBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTDBrand.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblTDBrand.ForeColor = System.Drawing.Color.White;
            this.lblTDBrand.Height = 38;
            this.lblTDBrand.Text = "QuizGen AI";

            // lblTDRole
            this.lblTDRole.AutoSize = false;
            this.lblTDRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTDRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTDRole.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.lblTDRole.Height = 22;
            this.lblTDRole.Text = "Teacher / Admin Workspace";

            // pnlTDNav
            this.pnlTDNav.Controls.Add(this.pnlTDNavLogs);
            this.pnlTDNav.Controls.Add(this.pnlTDNavReports);
            this.pnlTDNav.Controls.Add(this.pnlTDNavQuizzes);
            this.pnlTDNav.Controls.Add(this.pnlTDNavDashboard);
            this.pnlTDNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDNav.Height = 230;
            this.pnlTDNav.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);

            // pnlTDNavDashboard
            this.pnlTDNavDashboard.BackColor = System.Drawing.Color.FromArgb(14, 116, 144);
            this.pnlTDNavDashboard.Controls.Add(this.lblTDNavDashboard);
            this.pnlTDNavDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDNavDashboard.Height = 46;

            // lblTDNavDashboard
            this.lblTDNavDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDNavDashboard.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTDNavDashboard.ForeColor = System.Drawing.Color.White;
            this.lblTDNavDashboard.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTDNavDashboard.Text = "Dashboard";
            this.lblTDNavDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlTDNavQuizzes
            this.pnlTDNavQuizzes.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlTDNavQuizzes.Controls.Add(this.lblTDNavQuizzes);
            this.pnlTDNavQuizzes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDNavQuizzes.Height = 46;

            // lblTDNavQuizzes
            this.lblTDNavQuizzes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDNavQuizzes.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTDNavQuizzes.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.lblTDNavQuizzes.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTDNavQuizzes.Text = "Quizzes";
            this.lblTDNavQuizzes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlTDNavReports
            this.pnlTDNavReports.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlTDNavReports.Controls.Add(this.lblTDNavReports);
            this.pnlTDNavReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDNavReports.Height = 46;

            // lblTDNavReports
            this.lblTDNavReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDNavReports.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTDNavReports.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.lblTDNavReports.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTDNavReports.Text = "Reports";
            this.lblTDNavReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlTDNavLogs
            this.pnlTDNavLogs.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlTDNavLogs.Controls.Add(this.lblTDNavLogs);
            this.pnlTDNavLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDNavLogs.Height = 46;

            // lblTDNavLogs
            this.lblTDNavLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDNavLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTDNavLogs.ForeColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.lblTDNavLogs.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTDNavLogs.Text = "Logs";
            this.lblTDNavLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pnlTDSideFooter
            this.pnlTDSideFooter.Controls.Add(this.lblTDGreeting);
            this.pnlTDSideFooter.Controls.Add(this.pnlTDLogout);
            this.pnlTDSideFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTDSideFooter.Height = 132;
            this.pnlTDSideFooter.Padding = new System.Windows.Forms.Padding(20, 12, 20, 16);

            // pnlTDLogout
            this.pnlTDLogout.BackColor = System.Drawing.Color.FromArgb(185, 28, 28);
            this.pnlTDLogout.Controls.Add(this.lblTDLogoutText);
            this.pnlTDLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDLogout.Height = 38;

            // lblTDLogoutText
            this.lblTDLogoutText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTDLogoutText.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTDLogoutText.ForeColor = System.Drawing.Color.White;
            this.lblTDLogoutText.Text = "Logout";
            this.lblTDLogoutText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblTDGreeting
            this.lblTDGreeting.AutoSize = false;
            this.lblTDGreeting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTDGreeting.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTDGreeting.ForeColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.lblTDGreeting.Height = 48;
            this.lblTDGreeting.Text = "Signed in as Teacher";
            this.lblTDGreeting.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            // pnlTDTopBar
            this.pnlTDTopBar.BackColor = System.Drawing.Color.FromArgb(11, 48, 34);
            this.pnlTDTopBar.Controls.Add(this.lblTDPageDesc);
            this.pnlTDTopBar.Controls.Add(this.lblTDPageTitle);
            this.pnlTDTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTDTopBar.Height = 76;
            this.pnlTDTopBar.Padding = new System.Windows.Forms.Padding(28, 12, 28, 10);

            // lblTDPageTitle
            this.lblTDPageTitle.AutoSize = false;
            this.lblTDPageTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTDPageTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblTDPageTitle.ForeColor = System.Drawing.Color.White;
            this.lblTDPageTitle.Height = 36;
            this.lblTDPageTitle.Text = "Dashboard";

            // lblTDPageDesc
            this.lblTDPageDesc.AutoSize = false;
            this.lblTDPageDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTDPageDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTDPageDesc.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.lblTDPageDesc.Height = 20;
            this.lblTDPageDesc.Text = "Teacher/admin overview with quick actions and project metrics.";

            // pnlTDContent
            this.pnlTDContent.AutoScroll = true;
            this.pnlTDContent.BackColor = System.Drawing.Color.FromArgb(11, 48, 34);
            this.pnlTDContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTDContent.Padding = new System.Windows.Forms.Padding(20, 16, 20, 20);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(11, 48, 34);
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.pnlTDContent);
            this.Controls.Add(this.pnlTDTopBar);
            this.Controls.Add(this.pnlTDSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teacher Dashboard";

            this.pnlTDLogout.ResumeLayout(false);
            this.pnlTDSideFooter.ResumeLayout(false);
            this.pnlTDNavLogs.ResumeLayout(false);
            this.pnlTDNavReports.ResumeLayout(false);
            this.pnlTDNavQuizzes.ResumeLayout(false);
            this.pnlTDNavDashboard.ResumeLayout(false);
            this.pnlTDNav.ResumeLayout(false);
            this.pnlTDBranding.ResumeLayout(false);
            this.pnlTDSidebar.ResumeLayout(false);
            this.pnlTDTopBar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
