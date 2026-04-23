namespace QuizGenAI.Forms.Auth
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.TableLayoutPanel layoutCard;
        private System.Windows.Forms.FlowLayoutPanel flowBrand;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.FlowLayoutPanel flowTagline;
        private System.Windows.Forms.Label lblTagline1;
        private System.Windows.Forms.Label lblTaglineGold;
        private System.Windows.Forms.Label lblTagline2;
        private System.Windows.Forms.Label lblBadge;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel pnlEmailWrap;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel pnlPasswordWrap;
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
            this.panelBackground = new System.Windows.Forms.Panel();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.layoutCard = new System.Windows.Forms.TableLayoutPanel();
            this.flowBrand = new System.Windows.Forms.FlowLayoutPanel();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblBadge = new System.Windows.Forms.Label();
            this.flowTagline = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTagline1 = new System.Windows.Forms.Label();
            this.lblTaglineGold = new System.Windows.Forms.Label();
            this.lblTagline2 = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.pnlEmailWrap = new System.Windows.Forms.Panel();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.pnlPasswordWrap = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.panelBackground.SuspendLayout();
            this.pnlCard.SuspendLayout();
            this.layoutCard.SuspendLayout();
            this.flowBrand.SuspendLayout();
            this.flowTagline.SuspendLayout();
            this.pnlEmailWrap.SuspendLayout();
            this.pnlPasswordWrap.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBackground
            // 
            this.panelBackground.Controls.Add(this.pnlCard);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(1920, 1200);
            this.panelBackground.TabIndex = 0;
            this.panelBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBackground_Paint);
            this.panelBackground.Resize += new System.EventHandler(this.panelBackground_Resize);
            // 
            // pnlCard
            // 
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(64)))), ((int)(((byte)(50)))));
            this.pnlCard.Controls.Add(this.layoutCard);
            this.pnlCard.Location = new System.Drawing.Point(520, 80);
            this.pnlCard.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Padding = new System.Windows.Forms.Padding(56, 52, 56, 44);
            this.pnlCard.Size = new System.Drawing.Size(880, 1040);
            this.pnlCard.TabIndex = 0;
            this.pnlCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCard_Paint);
            // 
            // layoutCard
            // 
            this.layoutCard.ColumnCount = 1;
            this.layoutCard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCard.Controls.Add(this.flowBrand, 0, 0);
            this.layoutCard.Controls.Add(this.lblBadge, 0, 1);
            this.layoutCard.Controls.Add(this.flowTagline, 0, 2);
            this.layoutCard.Controls.Add(this.lblSubtitle, 0, 3);
            this.layoutCard.Controls.Add(this.lblEmail, 0, 4);
            this.layoutCard.Controls.Add(this.pnlEmailWrap, 0, 5);
            this.layoutCard.Controls.Add(this.lblPassword, 0, 6);
            this.layoutCard.Controls.Add(this.pnlPasswordWrap, 0, 7);
            this.layoutCard.Controls.Add(this.btnLogin, 0, 8);
            this.layoutCard.Controls.Add(this.lblHint, 0, 9);
            this.layoutCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCard.Location = new System.Drawing.Point(56, 52);
            this.layoutCard.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.layoutCard.Name = "layoutCard";
            this.layoutCard.RowCount = 10;
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutCard.Size = new System.Drawing.Size(768, 944);
            this.layoutCard.TabIndex = 0;
            // 
            // flowBrand
            // 
            this.flowBrand.AutoSize = true;
            this.flowBrand.Controls.Add(this.lblIcon);
            this.flowBrand.Controls.Add(this.lblBrand);
            this.flowBrand.Location = new System.Drawing.Point(0, 0);
            this.flowBrand.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.flowBrand.Name = "flowBrand";
            this.flowBrand.Size = new System.Drawing.Size(428, 72);
            this.flowBrand.TabIndex = 0;
            this.flowBrand.WrapContents = false;
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIcon.ForeColor = System.Drawing.Color.White;
            this.lblIcon.Location = new System.Drawing.Point(0, 0);
            this.lblIcon.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(104, 72);
            this.lblIcon.TabIndex = 0;
            this.lblIcon.Text = "🎓";
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Location = new System.Drawing.Point(120, 0);
            this.lblBrand.Margin = new System.Windows.Forms.Padding(0);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(308, 72);
            this.lblBrand.TabIndex = 1;
            this.lblBrand.Text = "QuizGen AI";
            // 
            // lblBadge
            // 
            this.lblBadge.AutoSize = true;
            this.lblBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(90)))), ((int)(((byte)(72)))));
            this.lblBadge.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.lblBadge.Location = new System.Drawing.Point(0, 84);
            this.lblBadge.Margin = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.lblBadge.Name = "lblBadge";
            this.lblBadge.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);
            this.lblBadge.Size = new System.Drawing.Size(205, 46);
            this.lblBadge.TabIndex = 1;
            this.lblBadge.Text = "# AI-POWERED";
            // 
            // flowTagline
            // 
            this.flowTagline.AutoSize = true;
            this.flowTagline.Controls.Add(this.lblTagline1);
            this.flowTagline.Controls.Add(this.lblTaglineGold);
            this.flowTagline.Controls.Add(this.lblTagline2);
            this.flowTagline.Location = new System.Drawing.Point(0, 150);
            this.flowTagline.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.flowTagline.MaximumSize = new System.Drawing.Size(768, 0);
            this.flowTagline.Name = "flowTagline";
            this.flowTagline.Size = new System.Drawing.Size(708, 41);
            this.flowTagline.TabIndex = 2;
            // 
            // lblTagline1
            // 
            this.lblTagline1.AutoSize = true;
            this.lblTagline1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline1.ForeColor = System.Drawing.Color.White;
            this.lblTagline1.Location = new System.Drawing.Point(0, 0);
            this.lblTagline1.Margin = new System.Windows.Forms.Padding(0);
            this.lblTagline1.Name = "lblTagline1";
            this.lblTagline1.Size = new System.Drawing.Size(408, 41);
            this.lblTagline1.TabIndex = 0;
            this.lblTagline1.Text = "Generate brilliant quizzes in ";
            // 
            // lblTaglineGold
            // 
            this.lblTaglineGold.AutoSize = true;
            this.lblTaglineGold.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaglineGold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(201)))), ((int)(((byte)(76)))));
            this.lblTaglineGold.Location = new System.Drawing.Point(408, 0);
            this.lblTaglineGold.Margin = new System.Windows.Forms.Padding(0);
            this.lblTaglineGold.Name = "lblTaglineGold";
            this.lblTaglineGold.Size = new System.Drawing.Size(128, 41);
            this.lblTaglineGold.TabIndex = 1;
            this.lblTaglineGold.Text = "seconds";
            // 
            // lblTagline2
            // 
            this.lblTagline2.AutoSize = true;
            this.lblTagline2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline2.ForeColor = System.Drawing.Color.White;
            this.lblTagline2.Location = new System.Drawing.Point(536, 0);
            this.lblTagline2.Margin = new System.Windows.Forms.Padding(0);
            this.lblTagline2.Name = "lblTagline2";
            this.lblTagline2.Size = new System.Drawing.Size(172, 41);
            this.lblTagline2.TabIndex = 2;
            this.lblTagline2.Text = ", not hours.";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(210)))), ((int)(((byte)(205)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 207);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 36);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(418, 32);
            this.lblSubtitle.TabIndex = 3;
            this.lblSubtitle.Text = "Sign in with your email and password.";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(228)))), ((int)(((byte)(223)))));
            this.lblEmail.Location = new System.Drawing.Point(0, 275);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(71, 32);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email";
            // 
            // pnlEmailWrap
            // 
            this.pnlEmailWrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(107)))), ((int)(((byte)(82)))));
            this.pnlEmailWrap.Controls.Add(this.txtEmail);
            this.pnlEmailWrap.Location = new System.Drawing.Point(0, 315);
            this.pnlEmailWrap.Margin = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.pnlEmailWrap.Name = "pnlEmailWrap";
            this.pnlEmailWrap.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlEmailWrap.Size = new System.Drawing.Size(768, 72);
            this.pnlEmailWrap.TabIndex = 5;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(34)))), ((int)(((byte)(26)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.Color.White;
            this.txtEmail.Location = new System.Drawing.Point(2, 2);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(764, 36);
            this.txtEmail.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(228)))), ((int)(((byte)(223)))));
            this.lblPassword.Location = new System.Drawing.Point(0, 411);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(111, 32);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // pnlPasswordWrap
            // 
            this.pnlPasswordWrap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(107)))), ((int)(((byte)(82)))));
            this.pnlPasswordWrap.Controls.Add(this.txtPassword);
            this.pnlPasswordWrap.Location = new System.Drawing.Point(0, 451);
            this.pnlPasswordWrap.Margin = new System.Windows.Forms.Padding(0, 0, 0, 32);
            this.pnlPasswordWrap.Name = "pnlPasswordWrap";
            this.pnlPasswordWrap.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlPasswordWrap.Size = new System.Drawing.Size(768, 72);
            this.pnlPasswordWrap.TabIndex = 7;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(34)))), ((int)(((byte)(26)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(2, 2);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(764, 36);
            this.txtPassword.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(175)))), ((int)(((byte)(55)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(155)))), ((int)(((byte)(45)))));
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(195)))), ((int)(((byte)(75)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(24)))), ((int)(((byte)(22)))));
            this.btnLogin.Location = new System.Drawing.Point(0, 555);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(768, 80);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(160)))), ((int)(((byte)(152)))));
            this.lblHint.Location = new System.Drawing.Point(0, 659);
            this.lblHint.Margin = new System.Windows.Forms.Padding(0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(432, 90);
            this.lblHint.TabIndex = 9;
            this.lblHint.Text = "Demo: admin@quizgenai.local / Admin123!\r\nteacher@quizgenai.local / Teacher123!\r\ns" +
    "tudent@quizgenai.local / Student123!";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(43)))), ((int)(((byte)(27)))));
            this.ClientSize = new System.Drawing.Size(1920, 1200);
            this.Controls.Add(this.panelBackground);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuizGen AI — Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelBackground.ResumeLayout(false);
            this.pnlCard.ResumeLayout(false);
            this.layoutCard.ResumeLayout(false);
            this.layoutCard.PerformLayout();
            this.flowBrand.ResumeLayout(false);
            this.flowBrand.PerformLayout();
            this.flowTagline.ResumeLayout(false);
            this.flowTagline.PerformLayout();
            this.pnlEmailWrap.ResumeLayout(false);
            this.pnlEmailWrap.PerformLayout();
            this.pnlPasswordWrap.ResumeLayout(false);
            this.pnlPasswordWrap.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
