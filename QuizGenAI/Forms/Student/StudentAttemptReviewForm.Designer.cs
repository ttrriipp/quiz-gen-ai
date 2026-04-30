namespace QuizGenAI.Forms.Student
{
    partial class StudentAttemptReviewForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel pnlReviewRoot;
        private System.Windows.Forms.Panel pnlReviewHero;
        private System.Windows.Forms.Panel pnlReviewTitleStack;
        private System.Windows.Forms.Label lblReviewMetaD;
        private System.Windows.Forms.Label lblReviewTitleD;
        private System.Windows.Forms.Label lblReviewTopicD;
        private System.Windows.Forms.TableLayoutPanel pnlReviewMetrics;
        private System.Windows.Forms.Panel pnlScoreMetricD;
        private System.Windows.Forms.Label lblScoreValueD;
        private System.Windows.Forms.Label lblScoreTitleD;
        private System.Windows.Forms.Panel pnlCorrectMetricD;
        private System.Windows.Forms.Label lblCorrectValueD;
        private System.Windows.Forms.Label lblCorrectTitleD;
        private System.Windows.Forms.Panel pnlMissedMetricD;
        private System.Windows.Forms.Label lblMissedValueD;
        private System.Windows.Forms.Label lblMissedTitleD;
        private System.Windows.Forms.Panel pnlAnswerReview;
        private System.Windows.Forms.Label lblAnswerReviewTitleD;
        private System.Windows.Forms.FlowLayoutPanel flpQuestionPreviewD;
        private System.Windows.Forms.Panel pnlQuestionCardD;
        private System.Windows.Forms.Label lblQuestionStatusD;
        private System.Windows.Forms.Label lblQuestionTextD;
        private System.Windows.Forms.Label lblSelectedAnswerD;
        private System.Windows.Forms.Label lblCorrectAnswerD;
        private System.Windows.Forms.Label lblExplanationTitleD;
        private System.Windows.Forms.Label lblExplanationD;
        private System.Windows.Forms.Panel pnlReviewActions;
        private System.Windows.Forms.Button btnBackToQuizzesD;

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
            this.pnlReviewRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlReviewHero = new System.Windows.Forms.Panel();
            this.pnlReviewTitleStack = new System.Windows.Forms.Panel();
            this.lblReviewTopicD = new System.Windows.Forms.Label();
            this.lblReviewTitleD = new System.Windows.Forms.Label();
            this.lblReviewMetaD = new System.Windows.Forms.Label();
            this.pnlReviewMetrics = new System.Windows.Forms.TableLayoutPanel();
            this.pnlScoreMetricD = new System.Windows.Forms.Panel();
            this.lblScoreValueD = new System.Windows.Forms.Label();
            this.lblScoreTitleD = new System.Windows.Forms.Label();
            this.pnlCorrectMetricD = new System.Windows.Forms.Panel();
            this.lblCorrectValueD = new System.Windows.Forms.Label();
            this.lblCorrectTitleD = new System.Windows.Forms.Label();
            this.pnlMissedMetricD = new System.Windows.Forms.Panel();
            this.lblMissedValueD = new System.Windows.Forms.Label();
            this.lblMissedTitleD = new System.Windows.Forms.Label();
            this.pnlAnswerReview = new System.Windows.Forms.Panel();
            this.flpQuestionPreviewD = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlQuestionCardD = new System.Windows.Forms.Panel();
            this.lblExplanationD = new System.Windows.Forms.Label();
            this.lblExplanationTitleD = new System.Windows.Forms.Label();
            this.lblCorrectAnswerD = new System.Windows.Forms.Label();
            this.lblSelectedAnswerD = new System.Windows.Forms.Label();
            this.lblQuestionTextD = new System.Windows.Forms.Label();
            this.lblQuestionStatusD = new System.Windows.Forms.Label();
            this.lblAnswerReviewTitleD = new System.Windows.Forms.Label();
            this.pnlReviewActions = new System.Windows.Forms.Panel();
            this.btnBackToQuizzesD = new System.Windows.Forms.Button();
            this.pnlReviewRoot.SuspendLayout();
            this.pnlReviewHero.SuspendLayout();
            this.pnlReviewTitleStack.SuspendLayout();
            this.pnlReviewMetrics.SuspendLayout();
            this.pnlScoreMetricD.SuspendLayout();
            this.pnlCorrectMetricD.SuspendLayout();
            this.pnlMissedMetricD.SuspendLayout();
            this.pnlAnswerReview.SuspendLayout();
            this.flpQuestionPreviewD.SuspendLayout();
            this.pnlQuestionCardD.SuspendLayout();
            this.pnlReviewActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlReviewRoot
            // 
            this.pnlReviewRoot.ColumnCount = 1;
            this.pnlReviewRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlReviewRoot.Controls.Add(this.pnlReviewHero, 0, 0);
            this.pnlReviewRoot.Controls.Add(this.pnlAnswerReview, 0, 1);
            this.pnlReviewRoot.Controls.Add(this.pnlReviewActions, 0, 2);
            this.pnlReviewRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReviewRoot.Location = new System.Drawing.Point(0, 0);
            this.pnlReviewRoot.Name = "pnlReviewRoot";
            this.pnlReviewRoot.Padding = new System.Windows.Forms.Padding(20);
            this.pnlReviewRoot.RowCount = 3;
            this.pnlReviewRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.pnlReviewRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlReviewRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.pnlReviewRoot.Size = new System.Drawing.Size(1010, 620);
            this.pnlReviewRoot.TabIndex = 0;
            // 
            // pnlReviewHero
            // 
            this.pnlReviewHero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(99)))), ((int)(((byte)(67)))));
            this.pnlReviewHero.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReviewHero.Controls.Add(this.pnlReviewTitleStack);
            this.pnlReviewHero.Controls.Add(this.pnlReviewMetrics);
            this.pnlReviewHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReviewHero.Location = new System.Drawing.Point(20, 20);
            this.pnlReviewHero.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.pnlReviewHero.Name = "pnlReviewHero";
            this.pnlReviewHero.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);
            this.pnlReviewHero.Size = new System.Drawing.Size(970, 170);
            this.pnlReviewHero.TabIndex = 0;
            // 
            // pnlReviewTitleStack
            // 
            this.pnlReviewTitleStack.BackColor = System.Drawing.Color.Transparent;
            this.pnlReviewTitleStack.Controls.Add(this.lblReviewTopicD);
            this.pnlReviewTitleStack.Controls.Add(this.lblReviewTitleD);
            this.pnlReviewTitleStack.Controls.Add(this.lblReviewMetaD);
            this.pnlReviewTitleStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReviewTitleStack.Location = new System.Drawing.Point(24, 20);
            this.pnlReviewTitleStack.Name = "pnlReviewTitleStack";
            this.pnlReviewTitleStack.Size = new System.Drawing.Size(592, 128);
            this.pnlReviewTitleStack.TabIndex = 0;
            // 
            // lblReviewTopicD
            // 
            this.lblReviewTopicD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReviewTopicD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblReviewTopicD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(232)))), ((int)(((byte)(224)))));
            this.lblReviewTopicD.Location = new System.Drawing.Point(0, 92);
            this.lblReviewTopicD.Name = "lblReviewTopicD";
            this.lblReviewTopicD.Size = new System.Drawing.Size(592, 28);
            this.lblReviewTopicD.TabIndex = 2;
            this.lblReviewTopicD.Text = "Topic: Sample topic";
            // 
            // lblReviewTitleD
            // 
            this.lblReviewTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReviewTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            this.lblReviewTitleD.ForeColor = System.Drawing.Color.White;
            this.lblReviewTitleD.Location = new System.Drawing.Point(0, 30);
            this.lblReviewTitleD.Name = "lblReviewTitleD";
            this.lblReviewTitleD.Size = new System.Drawing.Size(592, 62);
            this.lblReviewTitleD.TabIndex = 1;
            this.lblReviewTitleD.Text = "Quiz Review";
            // 
            // lblReviewMetaD
            // 
            this.lblReviewMetaD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReviewMetaD.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblReviewMetaD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(232)))), ((int)(((byte)(224)))));
            this.lblReviewMetaD.Location = new System.Drawing.Point(0, 0);
            this.lblReviewMetaD.Name = "lblReviewMetaD";
            this.lblReviewMetaD.Size = new System.Drawing.Size(592, 30);
            this.lblReviewMetaD.TabIndex = 0;
            this.lblReviewMetaD.Text = "Subject | Submitted today";
            // 
            // pnlReviewMetrics
            // 
            this.pnlReviewMetrics.BackColor = System.Drawing.Color.Transparent;
            this.pnlReviewMetrics.ColumnCount = 3;
            this.pnlReviewMetrics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.pnlReviewMetrics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.pnlReviewMetrics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.pnlReviewMetrics.Controls.Add(this.pnlScoreMetricD, 0, 0);
            this.pnlReviewMetrics.Controls.Add(this.pnlCorrectMetricD, 1, 0);
            this.pnlReviewMetrics.Controls.Add(this.pnlMissedMetricD, 2, 0);
            this.pnlReviewMetrics.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlReviewMetrics.Location = new System.Drawing.Point(616, 20);
            this.pnlReviewMetrics.Name = "pnlReviewMetrics";
            this.pnlReviewMetrics.RowCount = 1;
            this.pnlReviewMetrics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlReviewMetrics.Size = new System.Drawing.Size(328, 128);
            this.pnlReviewMetrics.TabIndex = 1;
            // 
            // pnlScoreMetricD
            // 
            this.pnlScoreMetricD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(78)))), ((int)(((byte)(56)))));
            this.pnlScoreMetricD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlScoreMetricD.Controls.Add(this.lblScoreValueD);
            this.pnlScoreMetricD.Controls.Add(this.lblScoreTitleD);
            this.pnlScoreMetricD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScoreMetricD.Location = new System.Drawing.Point(8, 4);
            this.pnlScoreMetricD.Margin = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.pnlScoreMetricD.Name = "pnlScoreMetricD";
            this.pnlScoreMetricD.Padding = new System.Windows.Forms.Padding(10, 12, 10, 10);
            this.pnlScoreMetricD.Size = new System.Drawing.Size(101, 120);
            this.pnlScoreMetricD.TabIndex = 0;
            // 
            // lblScoreValueD
            // 
            this.lblScoreValueD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScoreValueD.Font = new System.Drawing.Font("Segoe UI Semibold", 19F, System.Drawing.FontStyle.Bold);
            this.lblScoreValueD.ForeColor = System.Drawing.Color.White;
            this.lblScoreValueD.Location = new System.Drawing.Point(10, 36);
            this.lblScoreValueD.Name = "lblScoreValueD";
            this.lblScoreValueD.Size = new System.Drawing.Size(79, 72);
            this.lblScoreValueD.TabIndex = 1;
            this.lblScoreValueD.Text = "86%";
            this.lblScoreValueD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScoreTitleD
            // 
            this.lblScoreTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblScoreTitleD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblScoreTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.lblScoreTitleD.Location = new System.Drawing.Point(10, 12);
            this.lblScoreTitleD.Name = "lblScoreTitleD";
            this.lblScoreTitleD.Size = new System.Drawing.Size(79, 24);
            this.lblScoreTitleD.TabIndex = 0;
            this.lblScoreTitleD.Text = "Score";
            this.lblScoreTitleD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCorrectMetricD
            // 
            this.pnlCorrectMetricD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(78)))), ((int)(((byte)(56)))));
            this.pnlCorrectMetricD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCorrectMetricD.Controls.Add(this.lblCorrectValueD);
            this.pnlCorrectMetricD.Controls.Add(this.lblCorrectTitleD);
            this.pnlCorrectMetricD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCorrectMetricD.Location = new System.Drawing.Point(117, 4);
            this.pnlCorrectMetricD.Margin = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.pnlCorrectMetricD.Name = "pnlCorrectMetricD";
            this.pnlCorrectMetricD.Padding = new System.Windows.Forms.Padding(10, 12, 10, 10);
            this.pnlCorrectMetricD.Size = new System.Drawing.Size(101, 120);
            this.pnlCorrectMetricD.TabIndex = 1;
            // 
            // lblCorrectValueD
            // 
            this.lblCorrectValueD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCorrectValueD.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblCorrectValueD.ForeColor = System.Drawing.Color.White;
            this.lblCorrectValueD.Location = new System.Drawing.Point(10, 36);
            this.lblCorrectValueD.Name = "lblCorrectValueD";
            this.lblCorrectValueD.Size = new System.Drawing.Size(79, 72);
            this.lblCorrectValueD.TabIndex = 1;
            this.lblCorrectValueD.Text = "3";
            this.lblCorrectValueD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCorrectTitleD
            // 
            this.lblCorrectTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCorrectTitleD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCorrectTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.lblCorrectTitleD.Location = new System.Drawing.Point(10, 12);
            this.lblCorrectTitleD.Name = "lblCorrectTitleD";
            this.lblCorrectTitleD.Size = new System.Drawing.Size(79, 24);
            this.lblCorrectTitleD.TabIndex = 0;
            this.lblCorrectTitleD.Text = "Correct";
            this.lblCorrectTitleD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMissedMetricD
            // 
            this.pnlMissedMetricD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(78)))), ((int)(((byte)(56)))));
            this.pnlMissedMetricD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMissedMetricD.Controls.Add(this.lblMissedValueD);
            this.pnlMissedMetricD.Controls.Add(this.lblMissedTitleD);
            this.pnlMissedMetricD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMissedMetricD.Location = new System.Drawing.Point(226, 4);
            this.pnlMissedMetricD.Margin = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.pnlMissedMetricD.Name = "pnlMissedMetricD";
            this.pnlMissedMetricD.Padding = new System.Windows.Forms.Padding(10, 12, 10, 10);
            this.pnlMissedMetricD.Size = new System.Drawing.Size(102, 120);
            this.pnlMissedMetricD.TabIndex = 2;
            // 
            // lblMissedValueD
            // 
            this.lblMissedValueD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMissedValueD.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblMissedValueD.ForeColor = System.Drawing.Color.White;
            this.lblMissedValueD.Location = new System.Drawing.Point(10, 36);
            this.lblMissedValueD.Name = "lblMissedValueD";
            this.lblMissedValueD.Size = new System.Drawing.Size(80, 72);
            this.lblMissedValueD.TabIndex = 1;
            this.lblMissedValueD.Text = "1";
            this.lblMissedValueD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMissedTitleD
            // 
            this.lblMissedTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMissedTitleD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMissedTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(190)))), ((int)(((byte)(77)))));
            this.lblMissedTitleD.Location = new System.Drawing.Point(10, 12);
            this.lblMissedTitleD.Name = "lblMissedTitleD";
            this.lblMissedTitleD.Size = new System.Drawing.Size(80, 24);
            this.lblMissedTitleD.TabIndex = 0;
            this.lblMissedTitleD.Text = "Missed";
            this.lblMissedTitleD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlAnswerReview
            // 
            this.pnlAnswerReview.BackColor = System.Drawing.Color.White;
            this.pnlAnswerReview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAnswerReview.Controls.Add(this.flpQuestionPreviewD);
            this.pnlAnswerReview.Controls.Add(this.lblAnswerReviewTitleD);
            this.pnlAnswerReview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAnswerReview.Location = new System.Drawing.Point(20, 208);
            this.pnlAnswerReview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.pnlAnswerReview.Name = "pnlAnswerReview";
            this.pnlAnswerReview.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.pnlAnswerReview.Size = new System.Drawing.Size(970, 316);
            this.pnlAnswerReview.TabIndex = 1;
            // 
            // flpQuestionPreviewD
            // 
            this.flpQuestionPreviewD.AutoScroll = true;
            this.flpQuestionPreviewD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.flpQuestionPreviewD.Controls.Add(this.pnlQuestionCardD);
            this.flpQuestionPreviewD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpQuestionPreviewD.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpQuestionPreviewD.Location = new System.Drawing.Point(18, 50);
            this.flpQuestionPreviewD.Name = "flpQuestionPreviewD";
            this.flpQuestionPreviewD.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flpQuestionPreviewD.Size = new System.Drawing.Size(932, 248);
            this.flpQuestionPreviewD.TabIndex = 1;
            this.flpQuestionPreviewD.WrapContents = false;
            // 
            // pnlQuestionCardD
            // 
            this.pnlQuestionCardD.BackColor = System.Drawing.Color.White;
            this.pnlQuestionCardD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQuestionCardD.Controls.Add(this.lblExplanationD);
            this.pnlQuestionCardD.Controls.Add(this.lblExplanationTitleD);
            this.pnlQuestionCardD.Controls.Add(this.lblCorrectAnswerD);
            this.pnlQuestionCardD.Controls.Add(this.lblSelectedAnswerD);
            this.pnlQuestionCardD.Controls.Add(this.lblQuestionTextD);
            this.pnlQuestionCardD.Controls.Add(this.lblQuestionStatusD);
            this.pnlQuestionCardD.Location = new System.Drawing.Point(0, 10);
            this.pnlQuestionCardD.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.pnlQuestionCardD.Name = "pnlQuestionCardD";
            this.pnlQuestionCardD.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlQuestionCardD.Size = new System.Drawing.Size(880, 190);
            this.pnlQuestionCardD.TabIndex = 0;
            // 
            // lblExplanationD
            // 
            this.lblExplanationD.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblExplanationD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblExplanationD.Location = new System.Drawing.Point(16, 126);
            this.lblExplanationD.Name = "lblExplanationD";
            this.lblExplanationD.Size = new System.Drawing.Size(820, 44);
            this.lblExplanationD.TabIndex = 5;
            this.lblExplanationD.Text = "This area previews the explanation that will be shown for the submitted attempt.";
            // 
            // lblExplanationTitleD
            // 
            this.lblExplanationTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblExplanationTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblExplanationTitleD.Location = new System.Drawing.Point(16, 102);
            this.lblExplanationTitleD.Name = "lblExplanationTitleD";
            this.lblExplanationTitleD.Size = new System.Drawing.Size(820, 22);
            this.lblExplanationTitleD.TabIndex = 4;
            this.lblExplanationTitleD.Text = "Explanation";
            // 
            // lblCorrectAnswerD
            // 
            this.lblCorrectAnswerD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCorrectAnswerD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCorrectAnswerD.Location = new System.Drawing.Point(440, 66);
            this.lblCorrectAnswerD.Name = "lblCorrectAnswerD";
            this.lblCorrectAnswerD.Size = new System.Drawing.Size(390, 30);
            this.lblCorrectAnswerD.TabIndex = 3;
            this.lblCorrectAnswerD.Text = "Correct answer: Sample answer";
            // 
            // lblSelectedAnswerD
            // 
            this.lblSelectedAnswerD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSelectedAnswerD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSelectedAnswerD.Location = new System.Drawing.Point(16, 66);
            this.lblSelectedAnswerD.Name = "lblSelectedAnswerD";
            this.lblSelectedAnswerD.Size = new System.Drawing.Size(390, 30);
            this.lblSelectedAnswerD.TabIndex = 2;
            this.lblSelectedAnswerD.Text = "Your answer: Sample answer";
            // 
            // lblQuestionTextD
            // 
            this.lblQuestionTextD.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblQuestionTextD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblQuestionTextD.Location = new System.Drawing.Point(16, 14);
            this.lblQuestionTextD.Name = "lblQuestionTextD";
            this.lblQuestionTextD.Size = new System.Drawing.Size(680, 46);
            this.lblQuestionTextD.TabIndex = 1;
            this.lblQuestionTextD.Text = "Q1. Sample question text appears here for designer preview.";
            // 
            // lblQuestionStatusD
            // 
            this.lblQuestionStatusD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.lblQuestionStatusD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblQuestionStatusD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(120)))), ((int)(((byte)(87)))));
            this.lblQuestionStatusD.Location = new System.Drawing.Point(730, 14);
            this.lblQuestionStatusD.Name = "lblQuestionStatusD";
            this.lblQuestionStatusD.Size = new System.Drawing.Size(112, 28);
            this.lblQuestionStatusD.TabIndex = 0;
            this.lblQuestionStatusD.Text = "Correct";
            this.lblQuestionStatusD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAnswerReviewTitleD
            // 
            this.lblAnswerReviewTitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAnswerReviewTitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblAnswerReviewTitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblAnswerReviewTitleD.Location = new System.Drawing.Point(18, 16);
            this.lblAnswerReviewTitleD.Name = "lblAnswerReviewTitleD";
            this.lblAnswerReviewTitleD.Size = new System.Drawing.Size(932, 34);
            this.lblAnswerReviewTitleD.TabIndex = 0;
            this.lblAnswerReviewTitleD.Text = "Answer Review";
            // 
            // pnlReviewActions
            // 
            this.pnlReviewActions.BackColor = System.Drawing.Color.Transparent;
            this.pnlReviewActions.Controls.Add(this.btnBackToQuizzesD);
            this.pnlReviewActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReviewActions.Location = new System.Drawing.Point(20, 542);
            this.pnlReviewActions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlReviewActions.Name = "pnlReviewActions";
            this.pnlReviewActions.Size = new System.Drawing.Size(970, 58);
            this.pnlReviewActions.TabIndex = 2;
            // 
            // btnBackToQuizzesD
            // 
            this.btnBackToQuizzesD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackToQuizzesD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(88)))), ((int)(((byte)(61)))));
            this.btnBackToQuizzesD.FlatAppearance.BorderSize = 0;
            this.btnBackToQuizzesD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackToQuizzesD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnBackToQuizzesD.ForeColor = System.Drawing.Color.White;
            this.btnBackToQuizzesD.Location = new System.Drawing.Point(806, 0);
            this.btnBackToQuizzesD.Name = "btnBackToQuizzesD";
            this.btnBackToQuizzesD.Size = new System.Drawing.Size(164, 42);
            this.btnBackToQuizzesD.TabIndex = 0;
            this.btnBackToQuizzesD.Text = "Back To Quizzes";
            this.btnBackToQuizzesD.UseVisualStyleBackColor = false;
            // 
            // StudentAttemptReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1010, 620);
            this.Controls.Add(this.pnlReviewRoot);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentAttemptReviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Review Answers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlReviewRoot.ResumeLayout(false);
            this.pnlReviewHero.ResumeLayout(false);
            this.pnlReviewTitleStack.ResumeLayout(false);
            this.pnlReviewMetrics.ResumeLayout(false);
            this.pnlScoreMetricD.ResumeLayout(false);
            this.pnlCorrectMetricD.ResumeLayout(false);
            this.pnlMissedMetricD.ResumeLayout(false);
            this.pnlAnswerReview.ResumeLayout(false);
            this.flpQuestionPreviewD.ResumeLayout(false);
            this.pnlQuestionCardD.ResumeLayout(false);
            this.pnlReviewActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
