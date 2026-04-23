namespace QuizGenAI.Forms.Student
{
    partial class StudentResultsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel pnlResultsRoot;
        private System.Windows.Forms.Panel pnlScoreHero;
        private System.Windows.Forms.Panel pnlHeroInner;
        private System.Windows.Forms.Label lblTrophyD;
        private System.Windows.Forms.Label lblEffortD;
        private System.Windows.Forms.Label lblScoreD;
        private System.Windows.Forms.Label lblSummaryD;
        private System.Windows.Forms.Label lblMetaD;
        private System.Windows.Forms.Panel pnlRecommendation;
        private System.Windows.Forms.Panel pnlRecommendHeader;
        private System.Windows.Forms.Label lblRecommendTitleD;
        private System.Windows.Forms.Label lblRecommendSubD;
        private System.Windows.Forms.Panel pnlRecommendContent;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnAllResultsD;
        private System.Windows.Forms.Button btnDownloadD;
        private System.Windows.Forms.Button btnBackD;

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
            this.pnlResultsRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlScoreHero = new System.Windows.Forms.Panel();
            this.pnlHeroInner = new System.Windows.Forms.Panel();
            this.lblMetaD = new System.Windows.Forms.Label();
            this.lblSummaryD = new System.Windows.Forms.Label();
            this.lblScoreD = new System.Windows.Forms.Label();
            this.lblEffortD = new System.Windows.Forms.Label();
            this.lblTrophyD = new System.Windows.Forms.Label();
            this.pnlRecommendation = new System.Windows.Forms.Panel();
            this.pnlRecommendContent = new System.Windows.Forms.Panel();
            this.pnlRecommendHeader = new System.Windows.Forms.Panel();
            this.lblRecommendSubD = new System.Windows.Forms.Label();
            this.lblRecommendTitleD = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnAllResultsD = new System.Windows.Forms.Button();
            this.btnDownloadD = new System.Windows.Forms.Button();
            this.btnBackD = new System.Windows.Forms.Button();
            this.pnlResultsRoot.SuspendLayout();
            this.pnlScoreHero.SuspendLayout();
            this.pnlHeroInner.SuspendLayout();
            this.pnlRecommendation.SuspendLayout();
            this.pnlRecommendHeader.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlResultsRoot
            // 
            this.pnlResultsRoot.ColumnCount = 1;
            this.pnlResultsRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlResultsRoot.Controls.Add(this.pnlScoreHero, 0, 0);
            this.pnlResultsRoot.Controls.Add(this.pnlRecommendation, 0, 1);
            this.pnlResultsRoot.Controls.Add(this.pnlActions, 0, 2);
            this.pnlResultsRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultsRoot.Location = new System.Drawing.Point(0, 0);
            this.pnlResultsRoot.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlResultsRoot.Name = "pnlResultsRoot";
            this.pnlResultsRoot.Padding = new System.Windows.Forms.Padding(40, 40, 40, 40);
            this.pnlResultsRoot.RowCount = 3;
            this.pnlResultsRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 524F));
            this.pnlResultsRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlResultsRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.pnlResultsRoot.Size = new System.Drawing.Size(1960, 1400);
            this.pnlResultsRoot.TabIndex = 0;
            // 
            // pnlScoreHero
            // 
            this.pnlScoreHero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(99)))), ((int)(((byte)(67)))));
            this.pnlScoreHero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlScoreHero.Controls.Add(this.pnlHeroInner);
            this.pnlScoreHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScoreHero.Location = new System.Drawing.Point(40, 40);
            this.pnlScoreHero.Margin = new System.Windows.Forms.Padding(0, 0, 0, 36);
            this.pnlScoreHero.Name = "pnlScoreHero";
            this.pnlScoreHero.Padding = new System.Windows.Forms.Padding(56, 48, 56, 48);
            this.pnlScoreHero.Size = new System.Drawing.Size(1880, 488);
            this.pnlScoreHero.TabIndex = 0;
            // 
            // pnlHeroInner
            // 
            this.pnlHeroInner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(78)))), ((int)(((byte)(56)))));
            this.pnlHeroInner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeroInner.Controls.Add(this.lblMetaD);
            this.pnlHeroInner.Controls.Add(this.lblSummaryD);
            this.pnlHeroInner.Controls.Add(this.lblScoreD);
            this.pnlHeroInner.Controls.Add(this.lblEffortD);
            this.pnlHeroInner.Controls.Add(this.lblTrophyD);
            this.pnlHeroInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeroInner.Location = new System.Drawing.Point(56, 48);
            this.pnlHeroInner.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlHeroInner.Name = "pnlHeroInner";
            this.pnlHeroInner.Padding = new System.Windows.Forms.Padding(40, 40, 40, 40);
            this.pnlHeroInner.Size = new System.Drawing.Size(1766, 390);
            this.pnlHeroInner.TabIndex = 0;
            // 
            // lblMetaD
            // 
            this.lblMetaD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMetaD.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblMetaD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(228)))), ((int)(((byte)(221)))));
            this.lblMetaD.Location = new System.Drawing.Point(40, 444);
            this.lblMetaD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblMetaD.Name = "lblMetaD";
            this.lblMetaD.Size = new System.Drawing.Size(1684, 0);
            this.lblMetaD.TabIndex = 0;
            this.lblMetaD.Text = "Subject | Topic\r\nTime spent: 00:00 | Submitted: now";
            this.lblMetaD.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSummaryD
            // 
            this.lblSummaryD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSummaryD.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblSummaryD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(228)))), ((int)(((byte)(221)))));
            this.lblSummaryD.Location = new System.Drawing.Point(40, 372);
            this.lblSummaryD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSummaryD.Name = "lblSummaryD";
            this.lblSummaryD.Size = new System.Drawing.Size(1684, 72);
            this.lblSummaryD.TabIndex = 1;
            this.lblSummaryD.Text = "9 of 10 correct on Sample Quiz";
            this.lblSummaryD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScoreD
            // 
            this.lblScoreD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblScoreD.Font = new System.Drawing.Font("Segoe UI Semibold", 38F, System.Drawing.FontStyle.Bold);
            this.lblScoreD.ForeColor = System.Drawing.Color.White;
            this.lblScoreD.Location = new System.Drawing.Point(40, 220);
            this.lblScoreD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblScoreD.Name = "lblScoreD";
            this.lblScoreD.Size = new System.Drawing.Size(1684, 152);
            this.lblScoreD.TabIndex = 2;
            this.lblScoreD.Text = "95.0%";
            this.lblScoreD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEffortD
            // 
            this.lblEffortD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEffortD.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblEffortD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.lblEffortD.Location = new System.Drawing.Point(40, 164);
            this.lblEffortD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEffortD.Name = "lblEffortD";
            this.lblEffortD.Size = new System.Drawing.Size(1684, 56);
            this.lblEffortD.TabIndex = 3;
            this.lblEffortD.Text = "EXCELLENT WORK";
            this.lblEffortD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTrophyD
            // 
            this.lblTrophyD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.lblTrophyD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrophyD.Font = new System.Drawing.Font("Segoe UI Symbol", 22F, System.Drawing.FontStyle.Bold);
            this.lblTrophyD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(44)))), ((int)(((byte)(35)))));
            this.lblTrophyD.Location = new System.Drawing.Point(40, 40);
            this.lblTrophyD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTrophyD.Name = "lblTrophyD";
            this.lblTrophyD.Size = new System.Drawing.Size(1684, 124);
            this.lblTrophyD.TabIndex = 4;
            this.lblTrophyD.Text = "★";
            this.lblTrophyD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlRecommendation
            // 
            this.pnlRecommendation.BackColor = System.Drawing.Color.White;
            this.pnlRecommendation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecommendation.Controls.Add(this.pnlRecommendContent);
            this.pnlRecommendation.Controls.Add(this.pnlRecommendHeader);
            this.pnlRecommendation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecommendation.Location = new System.Drawing.Point(40, 564);
            this.pnlRecommendation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 36);
            this.pnlRecommendation.Name = "pnlRecommendation";
            this.pnlRecommendation.Padding = new System.Windows.Forms.Padding(44, 40, 44, 40);
            this.pnlRecommendation.Size = new System.Drawing.Size(1880, 652);
            this.pnlRecommendation.TabIndex = 1;
            // 
            // pnlRecommendContent
            // 
            this.pnlRecommendContent.AutoScroll = true;
            this.pnlRecommendContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.pnlRecommendContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecommendContent.Location = new System.Drawing.Point(44, 168);
            this.pnlRecommendContent.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlRecommendContent.Name = "pnlRecommendContent";
            this.pnlRecommendContent.Padding = new System.Windows.Forms.Padding(0, 16, 0, 0);
            this.pnlRecommendContent.Size = new System.Drawing.Size(1790, 442);
            this.pnlRecommendContent.TabIndex = 0;
            // 
            // pnlRecommendHeader
            // 
            this.pnlRecommendHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlRecommendHeader.Controls.Add(this.lblRecommendSubD);
            this.pnlRecommendHeader.Controls.Add(this.lblRecommendTitleD);
            this.pnlRecommendHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRecommendHeader.Location = new System.Drawing.Point(44, 40);
            this.pnlRecommendHeader.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlRecommendHeader.Name = "pnlRecommendHeader";
            this.pnlRecommendHeader.Size = new System.Drawing.Size(1790, 128);
            this.pnlRecommendHeader.TabIndex = 1;
            // 
            // lblRecommendSubD
            // 
            this.lblRecommendSubD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecommendSubD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblRecommendSubD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblRecommendSubD.Location = new System.Drawing.Point(0, 56);
            this.lblRecommendSubD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblRecommendSubD.Name = "lblRecommendSubD";
            this.lblRecommendSubD.Size = new System.Drawing.Size(1790, 52);
            this.lblRecommendSubD.TabIndex = 0;
            this.lblRecommendSubD.Text = "Student: Sample | Answered: 10/10 | Source: Claude API";
            // 
            // lblRecommendTitleD
            // 
            this.lblRecommendTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecommendTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblRecommendTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblRecommendTitleD.Location = new System.Drawing.Point(0, 0);
            this.lblRecommendTitleD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblRecommendTitleD.Name = "lblRecommendTitleD";
            this.lblRecommendTitleD.Size = new System.Drawing.Size(1790, 56);
            this.lblRecommendTitleD.TabIndex = 1;
            this.lblRecommendTitleD.Text = "AI Study Recommendations";
            // 
            // pnlActions
            // 
            this.pnlActions.BackColor = System.Drawing.Color.Transparent;
            this.pnlActions.Controls.Add(this.btnAllResultsD);
            this.pnlActions.Controls.Add(this.btnDownloadD);
            this.pnlActions.Controls.Add(this.btnBackD);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.Location = new System.Drawing.Point(46, 1258);
            this.pnlActions.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(1868, 96);
            this.pnlActions.TabIndex = 2;
            // 
            // btnAllResultsD
            // 
            this.btnAllResultsD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllResultsD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(88)))), ((int)(((byte)(61)))));
            this.btnAllResultsD.FlatAppearance.BorderSize = 0;
            this.btnAllResultsD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllResultsD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAllResultsD.ForeColor = System.Drawing.Color.White;
            this.btnAllResultsD.Location = new System.Drawing.Point(2392, 12);
            this.btnAllResultsD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnAllResultsD.Name = "btnAllResultsD";
            this.btnAllResultsD.Size = new System.Drawing.Size(308, 84);
            this.btnAllResultsD.TabIndex = 0;
            this.btnAllResultsD.Text = "View All Results";
            this.btnAllResultsD.UseVisualStyleBackColor = false;
            // 
            // btnDownloadD
            // 
            this.btnDownloadD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.btnDownloadD.FlatAppearance.BorderSize = 0;
            this.btnDownloadD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDownloadD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(44)))), ((int)(((byte)(35)))));
            this.btnDownloadD.Location = new System.Drawing.Point(2060, 12);
            this.btnDownloadD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDownloadD.Name = "btnDownloadD";
            this.btnDownloadD.Size = new System.Drawing.Size(308, 84);
            this.btnDownloadD.TabIndex = 1;
            this.btnDownloadD.Text = "Download PDF";
            this.btnDownloadD.UseVisualStyleBackColor = false;
            // 
            // btnBackD
            // 
            this.btnBackD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackD.BackColor = System.Drawing.Color.White;
            this.btnBackD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnBackD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnBackD.Location = new System.Drawing.Point(1728, 12);
            this.btnBackD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnBackD.Name = "btnBackD";
            this.btnBackD.Size = new System.Drawing.Size(308, 84);
            this.btnBackD.TabIndex = 2;
            this.btnBackD.Text = "Back To Quizzes";
            this.btnBackD.UseVisualStyleBackColor = false;
            // 
            // StudentResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1960, 1400);
            this.Controls.Add(this.pnlResultsRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "StudentResultsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Result Summary";
            this.pnlResultsRoot.ResumeLayout(false);
            this.pnlScoreHero.ResumeLayout(false);
            this.pnlHeroInner.ResumeLayout(false);
            this.pnlRecommendation.ResumeLayout(false);
            this.pnlRecommendHeader.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
