namespace QuizGenAI.Forms.Teacher
{
    partial class AiQuizGeneratorForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlAIHeader;
        private System.Windows.Forms.Label lblAITitle;
        private System.Windows.Forms.Label lblAISubtitle;
        private System.Windows.Forms.Panel pnlAIBody;
        private System.Windows.Forms.Panel pnlAICard;
        private System.Windows.Forms.TableLayoutPanel tlpAICardLayout;
        private System.Windows.Forms.TableLayoutPanel tlpAIFields;
        private System.Windows.Forms.Label lblAISubjectLbl;
        private System.Windows.Forms.ComboBox cmbAISubject;
        private System.Windows.Forms.Label lblAITopicLbl;
        private System.Windows.Forms.TextBox txtAITopic;
        private System.Windows.Forms.Label lblAIDifficultyLbl;
        private System.Windows.Forms.ComboBox cmbAIDifficulty;
        private System.Windows.Forms.Label lblAIQCountLbl;
        private System.Windows.Forms.NumericUpDown nudAIQuestionCount;
        private System.Windows.Forms.Label lblAIDurationLbl;
        private System.Windows.Forms.NumericUpDown nudAIDuration;
        private System.Windows.Forms.Label lblAINotesLbl;
        private System.Windows.Forms.Label lblAINote;
        private System.Windows.Forms.FlowLayoutPanel flpAIButtons;
        private System.Windows.Forms.Button btnAIGenerate;
        private System.Windows.Forms.Button btnAICancel;

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
            this.pnlAIHeader = new System.Windows.Forms.Panel();
            this.lblAITitle = new System.Windows.Forms.Label();
            this.lblAISubtitle = new System.Windows.Forms.Label();
            this.pnlAIBody = new System.Windows.Forms.Panel();
            this.pnlAICard = new System.Windows.Forms.Panel();
            this.tlpAICardLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tlpAIFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblAISubjectLbl = new System.Windows.Forms.Label();
            this.cmbAISubject = new System.Windows.Forms.ComboBox();
            this.lblAITopicLbl = new System.Windows.Forms.Label();
            this.txtAITopic = new System.Windows.Forms.TextBox();
            this.lblAIDifficultyLbl = new System.Windows.Forms.Label();
            this.cmbAIDifficulty = new System.Windows.Forms.ComboBox();
            this.lblAIQCountLbl = new System.Windows.Forms.Label();
            this.nudAIQuestionCount = new System.Windows.Forms.NumericUpDown();
            this.lblAIDurationLbl = new System.Windows.Forms.Label();
            this.nudAIDuration = new System.Windows.Forms.NumericUpDown();
            this.lblAINotesLbl = new System.Windows.Forms.Label();
            this.lblAINote = new System.Windows.Forms.Label();
            this.flpAIButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAIGenerate = new System.Windows.Forms.Button();
            this.btnAICancel = new System.Windows.Forms.Button();
            this.pnlAIHeader.SuspendLayout();
            this.pnlAIBody.SuspendLayout();
            this.pnlAICard.SuspendLayout();
            this.tlpAICardLayout.SuspendLayout();
            this.tlpAIFields.SuspendLayout();
            this.flpAIButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAIQuestionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAIDuration)).BeginInit();
            this.SuspendLayout();

            // pnlAIHeader
            this.pnlAIHeader.BackColor = System.Drawing.Color.White;
            this.pnlAIHeader.Controls.Add(this.lblAISubtitle);
            this.pnlAIHeader.Controls.Add(this.lblAITitle);
            this.pnlAIHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAIHeader.Height = 88;
            this.pnlAIHeader.Padding = new System.Windows.Forms.Padding(20, 18, 20, 14);

            // lblAITitle
            this.lblAITitle.AutoSize = false;
            this.lblAITitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAITitle.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblAITitle.ForeColor = System.Drawing.Color.FromArgb(17, 24, 39);
            this.lblAITitle.Height = 34;
            this.lblAITitle.Text = "AI Quiz Generator";

            // lblAISubtitle
            this.lblAISubtitle.AutoSize = false;
            this.lblAISubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAISubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAISubtitle.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.lblAISubtitle.Height = 24;
            this.lblAISubtitle.Text = "Generate MCQ drafts, review them in the editor, and save only after teacher review.";

            // pnlAIBody
            this.pnlAIBody.Controls.Add(this.pnlAICard);
            this.pnlAIBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAIBody.Padding = new System.Windows.Forms.Padding(18, 16, 18, 18);

            // pnlAICard
            this.pnlAICard.BackColor = System.Drawing.Color.White;
            this.pnlAICard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAICard.Controls.Add(this.tlpAICardLayout);
            this.pnlAICard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAICard.Padding = new System.Windows.Forms.Padding(22);

            // tlpAICardLayout
            this.tlpAICardLayout.ColumnCount = 1;
            this.tlpAICardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAICardLayout.Controls.Add(this.tlpAIFields, 0, 0);
            this.tlpAICardLayout.Controls.Add(this.flpAIButtons, 0, 1);
            this.tlpAICardLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAICardLayout.RowCount = 2;
            this.tlpAICardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAICardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // tlpAIFields
            this.tlpAIFields.AutoSize = true;
            this.tlpAIFields.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpAIFields.ColumnCount = 2;
            this.tlpAIFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpAIFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAIFields.Controls.Add(this.lblAISubjectLbl, 0, 0);
            this.tlpAIFields.Controls.Add(this.cmbAISubject, 1, 0);
            this.tlpAIFields.Controls.Add(this.lblAITopicLbl, 0, 1);
            this.tlpAIFields.Controls.Add(this.txtAITopic, 1, 1);
            this.tlpAIFields.Controls.Add(this.lblAIDifficultyLbl, 0, 2);
            this.tlpAIFields.Controls.Add(this.cmbAIDifficulty, 1, 2);
            this.tlpAIFields.Controls.Add(this.lblAIQCountLbl, 0, 3);
            this.tlpAIFields.Controls.Add(this.nudAIQuestionCount, 1, 3);
            this.tlpAIFields.Controls.Add(this.lblAIDurationLbl, 0, 4);
            this.tlpAIFields.Controls.Add(this.nudAIDuration, 1, 4);
            this.tlpAIFields.Controls.Add(this.lblAINotesLbl, 0, 5);
            this.tlpAIFields.Controls.Add(this.lblAINote, 1, 5);
            this.tlpAIFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpAIFields.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAIFields.RowCount = 6;
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpAIFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));

            // lblAISubjectLbl
            this.lblAISubjectLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAISubjectLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAISubjectLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAISubjectLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAISubjectLbl.Text = "Subject";
            this.lblAISubjectLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // cmbAISubject
            this.cmbAISubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAISubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAISubject.IntegralHeight = false;
            this.cmbAISubject.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);

            // lblAITopicLbl
            this.lblAITopicLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAITopicLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAITopicLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAITopicLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAITopicLbl.Text = "Topic";
            this.lblAITopicLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // txtAITopic
            this.txtAITopic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAITopic.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);

            // lblAIDifficultyLbl
            this.lblAIDifficultyLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAIDifficultyLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAIDifficultyLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAIDifficultyLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAIDifficultyLbl.Text = "Difficulty";
            this.lblAIDifficultyLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // cmbAIDifficulty
            this.cmbAIDifficulty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAIDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAIDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            this.cmbAIDifficulty.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.cmbAIDifficulty.SelectedIndex = 0;

            // lblAIQCountLbl
            this.lblAIQCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAIQCountLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAIQCountLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAIQCountLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAIQCountLbl.Text = "Question Count";
            this.lblAIQCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // nudAIQuestionCount
            this.nudAIQuestionCount.Margin = new System.Windows.Forms.Padding(0);
            this.nudAIQuestionCount.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudAIQuestionCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudAIQuestionCount.Value = new decimal(new int[] { 5, 0, 0, 0 });
            this.nudAIQuestionCount.Width = 110;

            // lblAIDurationLbl
            this.lblAIDurationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAIDurationLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAIDurationLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAIDurationLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAIDurationLbl.Text = "Duration (minutes)";
            this.lblAIDurationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // nudAIDuration
            this.nudAIDuration.Margin = new System.Windows.Forms.Padding(0);
            this.nudAIDuration.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            this.nudAIDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudAIDuration.Value = new decimal(new int[] { 15, 0, 0, 0 });
            this.nudAIDuration.Width = 110;

            // lblAINotesLbl
            this.lblAINotesLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAINotesLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAINotesLbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblAINotesLbl.Margin = new System.Windows.Forms.Padding(0, 6, 12, 6);
            this.lblAINotesLbl.Text = "Notes";
            this.lblAINotesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblAINote
            this.lblAINote.AutoSize = true;
            this.lblAINote.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAINote.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.lblAINote.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblAINote.MaximumSize = new System.Drawing.Size(430, 0);
            this.lblAINote.Text = "If no AI endpoint is configured, the app uses a demo fallback generator so the review-first workflow remains testable.";

            // flpAIButtons
            this.flpAIButtons.AutoSize = true;
            this.flpAIButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpAIButtons.Controls.Add(this.btnAIGenerate);
            this.flpAIButtons.Controls.Add(this.btnAICancel);
            this.flpAIButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAIButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpAIButtons.Margin = new System.Windows.Forms.Padding(0, 18, 0, 0);
            this.flpAIButtons.WrapContents = false;

            // btnAIGenerate
            this.btnAIGenerate.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnAIGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAIGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAIGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAIGenerate.ForeColor = System.Drawing.Color.White;
            this.btnAIGenerate.Height = 34;
            this.btnAIGenerate.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnAIGenerate.Text = "Generate";
            this.btnAIGenerate.Width = 120;
            this.btnAIGenerate.FlatAppearance.BorderSize = 0;

            // btnAICancel
            this.btnAICancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAICancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAICancel.Height = 34;
            this.btnAICancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnAICancel.Text = "Cancel";
            this.btnAICancel.Width = 92;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(700, 470);
            this.Controls.Add(this.pnlAIBody);
            this.Controls.Add(this.pnlAIHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New AI Quiz";

            ((System.ComponentModel.ISupportInitialize)(this.nudAIQuestionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAIDuration)).EndInit();
            this.flpAIButtons.ResumeLayout(false);
            this.flpAIButtons.PerformLayout();
            this.tlpAIFields.ResumeLayout(false);
            this.tlpAIFields.PerformLayout();
            this.tlpAICardLayout.ResumeLayout(false);
            this.tlpAICardLayout.PerformLayout();
            this.pnlAICard.ResumeLayout(false);
            this.pnlAIBody.ResumeLayout(false);
            this.pnlAIHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
