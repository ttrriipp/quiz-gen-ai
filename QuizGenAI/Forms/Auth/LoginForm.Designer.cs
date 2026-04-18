namespace QuizGenAI.Forms.Auth
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layoutRoot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblHint;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.layoutRoot.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutRoot
            // 
            this.layoutRoot.ColumnCount = 1;
            this.layoutRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRoot.Controls.Add(this.lblTitle, 0, 0);
            this.layoutRoot.Controls.Add(this.lblSubtitle, 0, 1);
            this.layoutRoot.Controls.Add(this.lblEmail, 0, 2);
            this.layoutRoot.Controls.Add(this.txtEmail, 0, 3);
            this.layoutRoot.Controls.Add(this.lblPassword, 0, 4);
            this.layoutRoot.Controls.Add(this.txtPassword, 0, 5);
            this.layoutRoot.Controls.Add(this.btnLogin, 0, 6);
            this.layoutRoot.Controls.Add(this.lblHint, 0, 7);
            this.layoutRoot.Location = new System.Drawing.Point(210, 60);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.Padding = new System.Windows.Forms.Padding(24);
            this.layoutRoot.RowCount = 8;
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRoot.Size = new System.Drawing.Size(380, 290);
            this.layoutRoot.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(27, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QuizGen AI Login";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblSubtitle.Location = new System.Drawing.Point(27, 56);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(224, 13);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Use one of the seeded demo accounts to log in.";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(27, 87);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(27, 103);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(302, 20);
            this.txtEmail.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(27, 138);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(27, 154);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(302, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(27, 193);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(302, 32);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(27, 240);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(242, 39);
            this.lblHint.TabIndex = 7;
            this.lblHint.Text = "admin@quizgenai.local / Admin123!\r\nteacher@quizgenai.local / Teacher123!\r\nstudent" +
                "@quizgenai.local / Student123!";
            // 
            // LoginForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.layoutRoot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuizGen AI Login";
            this.layoutRoot.ResumeLayout(false);
            this.layoutRoot.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
