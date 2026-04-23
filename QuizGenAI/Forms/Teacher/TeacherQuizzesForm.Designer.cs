namespace QuizGenAI.Forms.Teacher
{
    partial class TeacherQuizzesForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTQToolbar;
        private System.Windows.Forms.TextBox txtSearchD;
        private System.Windows.Forms.ComboBox cmbStatusD;
        private System.Windows.Forms.Button btnNewQuizD;
        private System.Windows.Forms.Button btnNewAiQuizD;
        private System.Windows.Forms.FlowLayoutPanel pnlCardsHostD;

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
            this.pnlTQToolbar = new System.Windows.Forms.Panel();
            this.txtSearchD = new System.Windows.Forms.TextBox();
            this.cmbStatusD = new System.Windows.Forms.ComboBox();
            this.btnNewQuizD = new System.Windows.Forms.Button();
            this.btnNewAiQuizD = new System.Windows.Forms.Button();
            this.pnlCardsHostD = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlTQToolbar.SuspendLayout();
            this.SuspendLayout();

            // pnlTQToolbar
            this.pnlTQToolbar.BackColor = System.Drawing.Color.Transparent;
            this.pnlTQToolbar.Controls.Add(this.txtSearchD);
            this.pnlTQToolbar.Controls.Add(this.cmbStatusD);
            this.pnlTQToolbar.Controls.Add(this.btnNewQuizD);
            this.pnlTQToolbar.Controls.Add(this.btnNewAiQuizD);
            this.pnlTQToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTQToolbar.Height = 54;

            // txtSearchD
            this.txtSearchD.Location = new System.Drawing.Point(0, 12);
            this.txtSearchD.Width = 260;

            // cmbStatusD
            this.cmbStatusD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusD.Items.AddRange(new object[] { "All statuses", "Draft", "Posted", "Archived" });
            this.cmbStatusD.Location = new System.Drawing.Point(268, 14);
            this.cmbStatusD.SelectedIndex = 0;
            this.cmbStatusD.Width = 148;

            // btnNewQuizD
            this.btnNewQuizD.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnNewQuizD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewQuizD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewQuizD.ForeColor = System.Drawing.Color.White;
            this.btnNewQuizD.Height = 34;
            this.btnNewQuizD.Location = new System.Drawing.Point(428, 10);
            this.btnNewQuizD.Text = "New Manual Quiz";
            this.btnNewQuizD.Width = 148;
            this.btnNewQuizD.FlatAppearance.BorderSize = 0;

            // btnNewAiQuizD
            this.btnNewAiQuizD.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnNewAiQuizD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewAiQuizD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewAiQuizD.ForeColor = System.Drawing.Color.White;
            this.btnNewAiQuizD.Height = 34;
            this.btnNewAiQuizD.Location = new System.Drawing.Point(584, 10);
            this.btnNewAiQuizD.Text = "New AI Quiz";
            this.btnNewAiQuizD.Width = 136;
            this.btnNewAiQuizD.FlatAppearance.BorderSize = 0;

            // pnlCardsHostD
            this.pnlCardsHostD.AutoScroll = true;
            this.pnlCardsHostD.BackColor = System.Drawing.Color.Transparent;
            this.pnlCardsHostD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardsHostD.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlCardsHostD.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.pnlCardsHostD.WrapContents = true;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(11, 48, 34);
            this.ClientSize = new System.Drawing.Size(1180, 760);
            this.Controls.Add(this.pnlCardsHostD);
            this.Controls.Add(this.pnlTQToolbar);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Teacher Quizzes";

            this.pnlTQToolbar.ResumeLayout(false);
            this.pnlTQToolbar.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
