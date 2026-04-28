namespace QuizGenAI.Forms.Student
{
    partial class ExamForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel pnlExamRoot;
        private System.Windows.Forms.Panel pnlExamHeader;
        private System.Windows.Forms.Panel pnlTimerCard;
        private System.Windows.Forms.Label lblTimerTitleD;
        private System.Windows.Forms.Label lblTimerValueD;
        private System.Windows.Forms.Panel pnlHeaderBody;
        private System.Windows.Forms.Label lblExamTitleD;
        private System.Windows.Forms.Label lblExamMetaD;
        private System.Windows.Forms.Panel pnlQuestionArea;
        private System.Windows.Forms.Label lblQuestionMetaD;
        private System.Windows.Forms.Label lblQuestionTextD;
        private System.Windows.Forms.FlowLayoutPanel pnlChoicesD;
        private System.Windows.Forms.Panel pnlTrackerArea;
        private System.Windows.Forms.Label lblTrackerTitleD;
        private System.Windows.Forms.Label lblProgressSummaryD;
        private System.Windows.Forms.FlowLayoutPanel pnlTrackerFlowD;
        private System.Windows.Forms.Panel pnlFooterArea;
        private System.Windows.Forms.FlowLayoutPanel pnlActionPanelD;
        private System.Windows.Forms.Button btnPrimaryD;
        private System.Windows.Forms.Button btnPreviousD;
        private System.Windows.Forms.Label lblFooterNoteD;

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
            this.pnlExamRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlExamHeader = new System.Windows.Forms.Panel();
            this.pnlTimerCard = new System.Windows.Forms.Panel();
            this.lblTimerTitleD = new System.Windows.Forms.Label();
            this.lblTimerValueD = new System.Windows.Forms.Label();
            this.pnlHeaderBody = new System.Windows.Forms.Panel();
            this.lblExamTitleD = new System.Windows.Forms.Label();
            this.lblExamMetaD = new System.Windows.Forms.Label();
            this.pnlQuestionArea = new System.Windows.Forms.Panel();
            this.lblQuestionMetaD = new System.Windows.Forms.Label();
            this.lblQuestionTextD = new System.Windows.Forms.Label();
            this.pnlChoicesD = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlTrackerArea = new System.Windows.Forms.Panel();
            this.lblTrackerTitleD = new System.Windows.Forms.Label();
            this.lblProgressSummaryD = new System.Windows.Forms.Label();
            this.pnlTrackerFlowD = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFooterArea = new System.Windows.Forms.Panel();
            this.pnlActionPanelD = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrimaryD = new System.Windows.Forms.Button();
            this.btnPreviousD = new System.Windows.Forms.Button();
            this.lblFooterNoteD = new System.Windows.Forms.Label();
            this.pnlExamRoot.SuspendLayout();
            this.pnlExamHeader.SuspendLayout();
            this.pnlTimerCard.SuspendLayout();
            this.pnlHeaderBody.SuspendLayout();
            this.pnlQuestionArea.SuspendLayout();
            this.pnlTrackerArea.SuspendLayout();
            this.pnlFooterArea.SuspendLayout();
            this.pnlActionPanelD.SuspendLayout();
            this.SuspendLayout();

            // pnlExamRoot
            this.pnlExamRoot.ColumnCount = 2;
            this.pnlExamRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74F));
            this.pnlExamRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.pnlExamRoot.Controls.Add(this.pnlExamHeader, 0, 0);
            this.pnlExamRoot.Controls.Add(this.pnlQuestionArea, 0, 1);
            this.pnlExamRoot.Controls.Add(this.pnlTrackerArea, 1, 1);
            this.pnlExamRoot.Controls.Add(this.pnlFooterArea, 0, 2);
            this.pnlExamRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExamRoot.Padding = new System.Windows.Forms.Padding(20);
            this.pnlExamRoot.RowCount = 3;
            this.pnlExamRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.pnlExamRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlExamRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.pnlExamRoot.SetColumnSpan(this.pnlExamHeader, 2);
            this.pnlExamRoot.SetColumnSpan(this.pnlFooterArea, 2);

            // pnlExamHeader
            this.pnlExamHeader.BackColor = System.Drawing.Color.White;
            this.pnlExamHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlExamHeader.Controls.Add(this.pnlHeaderBody);
            this.pnlExamHeader.Controls.Add(this.pnlTimerCard);
            this.pnlExamHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExamHeader.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);

            // pnlTimerCard
            this.pnlTimerCard.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlTimerCard.Controls.Add(this.lblTimerValueD);
            this.pnlTimerCard.Controls.Add(this.lblTimerTitleD);
            this.pnlTimerCard.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTimerCard.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.pnlTimerCard.Width = 210;

            // lblTimerTitleD
            this.lblTimerTitleD.AutoSize = false;
            this.lblTimerTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTimerTitleD.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblTimerTitleD.Height = 24;
            this.lblTimerTitleD.Left = 18;
            this.lblTimerTitleD.Text = "Time Remaining";
            this.lblTimerTitleD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTimerTitleD.Top = 10;
            this.lblTimerTitleD.Width = 170;

            // lblTimerValueD
            this.lblTimerValueD.AutoSize = false;
            this.lblTimerValueD.Font = new System.Drawing.Font("Consolas", 22F, System.Drawing.FontStyle.Bold);
            this.lblTimerValueD.ForeColor = System.Drawing.Color.White;
            this.lblTimerValueD.Height = 40;
            this.lblTimerValueD.Left = 18;
            this.lblTimerValueD.Text = "00:00";
            this.lblTimerValueD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTimerValueD.Top = 40;
            this.lblTimerValueD.Width = 170;

            // pnlHeaderBody
            this.pnlHeaderBody.Controls.Add(this.lblExamMetaD);
            this.pnlHeaderBody.Controls.Add(this.lblExamTitleD);
            this.pnlHeaderBody.Dock = System.Windows.Forms.DockStyle.Fill;

            // lblExamTitleD
            this.lblExamTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExamTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 21F, System.Drawing.FontStyle.Bold);
            this.lblExamTitleD.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblExamTitleD.Height = 34;
            this.lblExamTitleD.Text = "Exam";

            // lblExamMetaD
            this.lblExamMetaD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExamMetaD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblExamMetaD.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblExamMetaD.Height = 24;
            this.lblExamMetaD.Text = "Subject | 30 minutes | 10 questions | Topic: General";

            // pnlQuestionArea
            this.pnlQuestionArea.BackColor = System.Drawing.Color.White;
            this.pnlQuestionArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQuestionArea.Controls.Add(this.pnlChoicesD);
            this.pnlQuestionArea.Controls.Add(this.lblQuestionTextD);
            this.pnlQuestionArea.Controls.Add(this.lblQuestionMetaD);
            this.pnlQuestionArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQuestionArea.Padding = new System.Windows.Forms.Padding(22, 20, 22, 20);

            // lblQuestionMetaD
            this.lblQuestionMetaD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQuestionMetaD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuestionMetaD.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.lblQuestionMetaD.Height = 26;
            this.lblQuestionMetaD.Text = "Question 1 of 10";

            // lblQuestionTextD
            this.lblQuestionTextD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQuestionTextD.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblQuestionTextD.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblQuestionTextD.Height = 110;
            this.lblQuestionTextD.Padding = new System.Windows.Forms.Padding(0, 8, 0, 12);
            this.lblQuestionTextD.Text = "What is the correct answer to this sample question?";

            // pnlChoicesD
            this.pnlChoicesD.AutoScroll = true;
            this.pnlChoicesD.BackColor = System.Drawing.Color.Transparent;
            this.pnlChoicesD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChoicesD.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlChoicesD.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlChoicesD.WrapContents = false;

            // pnlTrackerArea
            this.pnlTrackerArea.BackColor = System.Drawing.Color.White;
            this.pnlTrackerArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrackerArea.Controls.Add(this.pnlTrackerFlowD);
            this.pnlTrackerArea.Controls.Add(this.lblProgressSummaryD);
            this.pnlTrackerArea.Controls.Add(this.lblTrackerTitleD);
            this.pnlTrackerArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrackerArea.Padding = new System.Windows.Forms.Padding(18, 18, 18, 18);

            // lblTrackerTitleD
            this.lblTrackerTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrackerTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTrackerTitleD.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTrackerTitleD.Height = 28;
            this.lblTrackerTitleD.Text = "Question Tracker";

            // lblProgressSummaryD
            this.lblProgressSummaryD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProgressSummaryD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblProgressSummaryD.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblProgressSummaryD.Height = 42;
            this.lblProgressSummaryD.Text = "Answered: 0\r\nUnanswered: 10";

            // pnlTrackerFlowD
            this.pnlTrackerFlowD.AutoScroll = true;
            this.pnlTrackerFlowD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrackerFlowD.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlTrackerFlowD.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlTrackerFlowD.WrapContents = true;

            // pnlFooterArea
            this.pnlFooterArea.BackColor = System.Drawing.Color.White;
            this.pnlFooterArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooterArea.Controls.Add(this.pnlActionPanelD);
            this.pnlFooterArea.Controls.Add(this.lblFooterNoteD);
            this.pnlFooterArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooterArea.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);

            // pnlActionPanelD
            this.pnlActionPanelD.AutoSize = true;
            this.pnlActionPanelD.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlActionPanelD.Controls.Add(this.btnPreviousD);
            this.pnlActionPanelD.Controls.Add(this.btnPrimaryD);
            this.pnlActionPanelD.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActionPanelD.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlActionPanelD.Margin = new System.Windows.Forms.Padding(0);
            this.pnlActionPanelD.WrapContents = false;

            // btnPrimaryD
            this.btnPrimaryD.BackColor = System.Drawing.Color.FromArgb(3, 105, 161);
            this.btnPrimaryD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrimaryD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrimaryD.ForeColor = System.Drawing.Color.White;
            this.btnPrimaryD.Height = 44;
            this.btnPrimaryD.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnPrimaryD.Text = "Next";
            this.btnPrimaryD.Width = 150;
            this.btnPrimaryD.FlatAppearance.BorderSize = 0;

            // btnPreviousD
            this.btnPreviousD.BackColor = System.Drawing.Color.White;
            this.btnPreviousD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPreviousD.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnPreviousD.Height = 44;
            this.btnPreviousD.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnPreviousD.Text = "Previous";
            this.btnPreviousD.Width = 116;

            // lblFooterNoteD
            this.lblFooterNoteD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooterNoteD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFooterNoteD.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblFooterNoteD.Text = "Selections are saved as you move between questions.";

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.ClientSize = new System.Drawing.Size(1280, 820);
            this.Controls.Add(this.pnlExamRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exam";

            this.pnlActionPanelD.ResumeLayout(false);
            this.pnlActionPanelD.PerformLayout();
            this.pnlFooterArea.ResumeLayout(false);
            this.pnlTrackerArea.ResumeLayout(false);
            this.pnlQuestionArea.ResumeLayout(false);
            this.pnlHeaderBody.ResumeLayout(false);
            this.pnlTimerCard.ResumeLayout(false);
            this.pnlExamHeader.ResumeLayout(false);
            this.pnlExamRoot.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
