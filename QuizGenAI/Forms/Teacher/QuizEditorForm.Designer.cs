namespace QuizGenAI.Forms.Teacher
{
    partial class QuizEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlQEHeader;
        private System.Windows.Forms.Label lblQETitleD;
        private System.Windows.Forms.Label lblQESubtitleD;
        private System.Windows.Forms.Panel pnlQEBody;
        private System.Windows.Forms.Panel pnlQEMeta;
        private System.Windows.Forms.TableLayoutPanel pnlQEMetaLayout;
        private System.Windows.Forms.Label lblQETitleField;
        private System.Windows.Forms.TextBox txtTitleD;
        private System.Windows.Forms.Label lblQESubjectField;
        private System.Windows.Forms.ComboBox cmbSubjectD;
        private System.Windows.Forms.Label lblQETopicField;
        private System.Windows.Forms.TextBox txtTopicD;
        private System.Windows.Forms.Label lblQEDiffField;
        private System.Windows.Forms.ComboBox cmbDifficultyD;
        private System.Windows.Forms.Label lblQEDurationField;
        private System.Windows.Forms.NumericUpDown nudDurationD;
        private System.Windows.Forms.TableLayoutPanel pnlQEEditorArea;
        private System.Windows.Forms.Panel pnlQEListPanel;
        private System.Windows.Forms.Label lblQEQuestionsTitle;
        private System.Windows.Forms.ListBox lstQuestionsD;
        private System.Windows.Forms.Panel pnlQEListButtons;
        private System.Windows.Forms.Button btnAddNewD;
        private System.Windows.Forms.Button btnRemoveD;
        private System.Windows.Forms.Panel pnlQEQuestionPanel;
        private System.Windows.Forms.Panel pnlQEQuestionScroll;
        private System.Windows.Forms.TableLayoutPanel pnlQEQuestionLayout;
        private System.Windows.Forms.TextBox txtQuestionTextD;
        private System.Windows.Forms.TextBox txtChoiceAD;
        private System.Windows.Forms.TextBox txtChoiceBD;
        private System.Windows.Forms.TextBox txtChoiceCD;
        private System.Windows.Forms.TextBox txtChoiceDD;
        private System.Windows.Forms.ComboBox cmbCorrectChoiceD;
        private System.Windows.Forms.TextBox txtExplanationD;
        private System.Windows.Forms.Label lblQEHintD;
        private System.Windows.Forms.Panel pnlQEQuestionButtons;
        private System.Windows.Forms.Button btnApplyD;
        private System.Windows.Forms.Panel pnlQEBottom;
        private System.Windows.Forms.Button btnSaveDraftD;
        private System.Windows.Forms.Button btnCancelD;

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
            this.pnlQEHeader = new System.Windows.Forms.Panel();
            this.lblQESubtitleD = new System.Windows.Forms.Label();
            this.lblQETitleD = new System.Windows.Forms.Label();
            this.pnlQEBody = new System.Windows.Forms.Panel();
            this.pnlQEEditorArea = new System.Windows.Forms.TableLayoutPanel();
            this.pnlQEListPanel = new System.Windows.Forms.Panel();
            this.lstQuestionsD = new System.Windows.Forms.ListBox();
            this.pnlQEListButtons = new System.Windows.Forms.Panel();
            this.btnAddNewD = new System.Windows.Forms.Button();
            this.btnRemoveD = new System.Windows.Forms.Button();
            this.lblQEQuestionsTitle = new System.Windows.Forms.Label();
            this.pnlQEQuestionPanel = new System.Windows.Forms.Panel();
            this.pnlQEQuestionScroll = new System.Windows.Forms.Panel();
            this.pnlQEQuestionLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtQuestionTextD = new System.Windows.Forms.TextBox();
            this.txtChoiceAD = new System.Windows.Forms.TextBox();
            this.txtChoiceBD = new System.Windows.Forms.TextBox();
            this.txtChoiceCD = new System.Windows.Forms.TextBox();
            this.txtChoiceDD = new System.Windows.Forms.TextBox();
            this.cmbCorrectChoiceD = new System.Windows.Forms.ComboBox();
            this.txtExplanationD = new System.Windows.Forms.TextBox();
            this.lblQEHintD = new System.Windows.Forms.Label();
            this.pnlQEQuestionButtons = new System.Windows.Forms.Panel();
            this.btnApplyD = new System.Windows.Forms.Button();
            this.pnlQEMeta = new System.Windows.Forms.Panel();
            this.pnlQEMetaLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblQETitleField = new System.Windows.Forms.Label();
            this.txtTitleD = new System.Windows.Forms.TextBox();
            this.lblQESubjectField = new System.Windows.Forms.Label();
            this.cmbSubjectD = new System.Windows.Forms.ComboBox();
            this.lblQETopicField = new System.Windows.Forms.Label();
            this.txtTopicD = new System.Windows.Forms.TextBox();
            this.lblQEDiffField = new System.Windows.Forms.Label();
            this.cmbDifficultyD = new System.Windows.Forms.ComboBox();
            this.lblQEDurationField = new System.Windows.Forms.Label();
            this.nudDurationD = new System.Windows.Forms.NumericUpDown();
            this.pnlQEBottom = new System.Windows.Forms.Panel();
            this.btnSaveDraftD = new System.Windows.Forms.Button();
            this.btnCancelD = new System.Windows.Forms.Button();
            this.pnlQEHeader.SuspendLayout();
            this.pnlQEBody.SuspendLayout();
            this.pnlQEEditorArea.SuspendLayout();
            this.pnlQEListPanel.SuspendLayout();
            this.pnlQEListButtons.SuspendLayout();
            this.pnlQEQuestionPanel.SuspendLayout();
            this.pnlQEQuestionScroll.SuspendLayout();
            this.pnlQEQuestionLayout.SuspendLayout();
            this.pnlQEQuestionButtons.SuspendLayout();
            this.pnlQEMeta.SuspendLayout();
            this.pnlQEMetaLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationD)).BeginInit();
            this.pnlQEBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlQEHeader
            // 
            this.pnlQEHeader.BackColor = System.Drawing.Color.White;
            this.pnlQEHeader.Controls.Add(this.lblQESubtitleD);
            this.pnlQEHeader.Controls.Add(this.lblQETitleD);
            this.pnlQEHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQEHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlQEHeader.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEHeader.Name = "pnlQEHeader";
            this.pnlQEHeader.Padding = new System.Windows.Forms.Padding(48, 36, 48, 32);
            this.pnlQEHeader.Size = new System.Drawing.Size(2880, 188);
            this.pnlQEHeader.TabIndex = 2;
            // 
            // lblQESubtitleD
            // 
            this.lblQESubtitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQESubtitleD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblQESubtitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblQESubtitleD.Location = new System.Drawing.Point(48, 104);
            this.lblQESubtitleD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQESubtitleD.Name = "lblQESubtitleD";
            this.lblQESubtitleD.Size = new System.Drawing.Size(2784, 44);
            this.lblQESubtitleD.TabIndex = 0;
            this.lblQESubtitleD.Text = "Build a draft quiz, add multiple-choice questions, and save for later review.";
            // 
            // lblQETitleD
            // 
            this.lblQETitleD.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQETitleD.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lblQETitleD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblQETitleD.Location = new System.Drawing.Point(48, 36);
            this.lblQETitleD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQETitleD.Name = "lblQETitleD";
            this.lblQETitleD.Size = new System.Drawing.Size(2784, 68);
            this.lblQETitleD.TabIndex = 1;
            this.lblQETitleD.Text = "Manual Quiz Editor";
            // 
            // pnlQEBody
            // 
            this.pnlQEBody.AutoScroll = true;
            this.pnlQEBody.Controls.Add(this.pnlQEEditorArea);
            this.pnlQEBody.Controls.Add(this.pnlQEMeta);
            this.pnlQEBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEBody.Location = new System.Drawing.Point(0, 188);
            this.pnlQEBody.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEBody.Name = "pnlQEBody";
            this.pnlQEBody.Padding = new System.Windows.Forms.Padding(48, 48, 48, 48);
            this.pnlQEBody.Size = new System.Drawing.Size(2880, 1283);
            this.pnlQEBody.TabIndex = 0;
            // 
            // pnlQEEditorArea
            // 
            this.pnlQEEditorArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlQEEditorArea.ColumnCount = 2;
            this.pnlQEEditorArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 840F));
            this.pnlQEEditorArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlQEEditorArea.Controls.Add(this.pnlQEListPanel, 0, 0);
            this.pnlQEEditorArea.Controls.Add(this.pnlQEQuestionPanel, 1, 0);
            this.pnlQEEditorArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEEditorArea.Location = new System.Drawing.Point(48, 386);
            this.pnlQEEditorArea.Margin = new System.Windows.Forms.Padding(0, 36, 0, 0);
            this.pnlQEEditorArea.Name = "pnlQEEditorArea";
            this.pnlQEEditorArea.RowCount = 1;
            this.pnlQEEditorArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlQEEditorArea.Size = new System.Drawing.Size(2784, 849);
            this.pnlQEEditorArea.TabIndex = 0;
            // 
            // pnlQEListPanel
            // 
            this.pnlQEListPanel.BackColor = System.Drawing.Color.White;
            this.pnlQEListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQEListPanel.Controls.Add(this.lstQuestionsD);
            this.pnlQEListPanel.Controls.Add(this.pnlQEListButtons);
            this.pnlQEListPanel.Controls.Add(this.lblQEQuestionsTitle);
            this.pnlQEListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEListPanel.Location = new System.Drawing.Point(6, 6);
            this.pnlQEListPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEListPanel.Name = "pnlQEListPanel";
            this.pnlQEListPanel.Padding = new System.Windows.Forms.Padding(36, 36, 36, 36);
            this.pnlQEListPanel.Size = new System.Drawing.Size(828, 837);
            this.pnlQEListPanel.TabIndex = 0;
            // 
            // lstQuestionsD
            // 
            this.lstQuestionsD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstQuestionsD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstQuestionsD.HorizontalScrollbar = true;
            this.lstQuestionsD.IntegralHeight = false;
            this.lstQuestionsD.ItemHeight = 37;
            this.lstQuestionsD.Location = new System.Drawing.Point(36, 92);
            this.lstQuestionsD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lstQuestionsD.Name = "lstQuestionsD";
            this.lstQuestionsD.Size = new System.Drawing.Size(754, 623);
            this.lstQuestionsD.TabIndex = 0;
            // 
            // pnlQEListButtons
            // 
            this.pnlQEListButtons.Controls.Add(this.btnAddNewD);
            this.pnlQEListButtons.Controls.Add(this.btnRemoveD);
            this.pnlQEListButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlQEListButtons.Location = new System.Drawing.Point(36, 715);
            this.pnlQEListButtons.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEListButtons.Name = "pnlQEListButtons";
            this.pnlQEListButtons.Size = new System.Drawing.Size(754, 84);
            this.pnlQEListButtons.TabIndex = 1;
            // 
            // btnAddNewD
            // 
            this.btnAddNewD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.btnAddNewD.FlatAppearance.BorderSize = 0;
            this.btnAddNewD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddNewD.ForeColor = System.Drawing.Color.White;
            this.btnAddNewD.Location = new System.Drawing.Point(0, 8);
            this.btnAddNewD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnAddNewD.Name = "btnAddNewD";
            this.btnAddNewD.Size = new System.Drawing.Size(256, 68);
            this.btnAddNewD.TabIndex = 0;
            this.btnAddNewD.Text = "New Question";
            this.btnAddNewD.UseVisualStyleBackColor = false;
            // 
            // btnRemoveD
            // 
            this.btnRemoveD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveD.Location = new System.Drawing.Point(276, 8);
            this.btnRemoveD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnRemoveD.Name = "btnRemoveD";
            this.btnRemoveD.Size = new System.Drawing.Size(184, 68);
            this.btnRemoveD.TabIndex = 1;
            this.btnRemoveD.Text = "Remove";
            // 
            // lblQEQuestionsTitle
            // 
            this.lblQEQuestionsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblQEQuestionsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblQEQuestionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(24)))), ((int)(((byte)(39)))));
            this.lblQEQuestionsTitle.Location = new System.Drawing.Point(36, 36);
            this.lblQEQuestionsTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQEQuestionsTitle.Name = "lblQEQuestionsTitle";
            this.lblQEQuestionsTitle.Size = new System.Drawing.Size(754, 56);
            this.lblQEQuestionsTitle.TabIndex = 2;
            this.lblQEQuestionsTitle.Text = "Questions";
            // 
            // pnlQEQuestionPanel
            // 
            this.pnlQEQuestionPanel.BackColor = System.Drawing.Color.White;
            this.pnlQEQuestionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQEQuestionPanel.Controls.Add(this.pnlQEQuestionScroll);
            this.pnlQEQuestionPanel.Controls.Add(this.pnlQEQuestionButtons);
            this.pnlQEQuestionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEQuestionPanel.Location = new System.Drawing.Point(846, 6);
            this.pnlQEQuestionPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEQuestionPanel.Name = "pnlQEQuestionPanel";
            this.pnlQEQuestionPanel.Padding = new System.Windows.Forms.Padding(36, 36, 36, 36);
            this.pnlQEQuestionPanel.Size = new System.Drawing.Size(1932, 837);
            this.pnlQEQuestionPanel.TabIndex = 1;
            // 
            // pnlQEQuestionScroll
            // 
            this.pnlQEQuestionScroll.AutoScroll = true;
            this.pnlQEQuestionScroll.Controls.Add(this.pnlQEQuestionLayout);
            this.pnlQEQuestionScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEQuestionScroll.Location = new System.Drawing.Point(36, 36);
            this.pnlQEQuestionScroll.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEQuestionScroll.Name = "pnlQEQuestionScroll";
            this.pnlQEQuestionScroll.Padding = new System.Windows.Forms.Padding(0, 0, 0, 28);
            this.pnlQEQuestionScroll.Size = new System.Drawing.Size(1858, 659);
            this.pnlQEQuestionScroll.TabIndex = 0;
            // 
            // pnlQEQuestionLayout
            // 
            this.pnlQEQuestionLayout.AutoSize = true;
            this.pnlQEQuestionLayout.ColumnCount = 2;
            this.pnlQEQuestionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.pnlQEQuestionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlQEQuestionLayout.Controls.Add(this.txtQuestionTextD, 1, 1);
            this.pnlQEQuestionLayout.Controls.Add(this.txtChoiceAD, 1, 2);
            this.pnlQEQuestionLayout.Controls.Add(this.txtChoiceBD, 1, 3);
            this.pnlQEQuestionLayout.Controls.Add(this.txtChoiceCD, 1, 4);
            this.pnlQEQuestionLayout.Controls.Add(this.txtChoiceDD, 1, 5);
            this.pnlQEQuestionLayout.Controls.Add(this.cmbCorrectChoiceD, 1, 6);
            this.pnlQEQuestionLayout.Controls.Add(this.txtExplanationD, 1, 7);
            this.pnlQEQuestionLayout.Controls.Add(this.lblQEHintD, 1, 8);
            this.pnlQEQuestionLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQEQuestionLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlQEQuestionLayout.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEQuestionLayout.Name = "pnlQEQuestionLayout";
            this.pnlQEQuestionLayout.RowCount = 9;
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.pnlQEQuestionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.pnlQEQuestionLayout.Size = new System.Drawing.Size(1824, 976);
            this.pnlQEQuestionLayout.TabIndex = 0;
            // 
            // txtQuestionTextD
            // 
            this.txtQuestionTextD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuestionTextD.Location = new System.Drawing.Point(246, 66);
            this.txtQuestionTextD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtQuestionTextD.Multiline = true;
            this.txtQuestionTextD.Name = "txtQuestionTextD";
            this.txtQuestionTextD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQuestionTextD.Size = new System.Drawing.Size(1572, 208);
            this.txtQuestionTextD.TabIndex = 1;
            // 
            // txtChoiceAD
            // 
            this.txtChoiceAD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChoiceAD.Location = new System.Drawing.Point(246, 286);
            this.txtChoiceAD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtChoiceAD.Name = "txtChoiceAD";
            this.txtChoiceAD.Size = new System.Drawing.Size(1572, 43);
            this.txtChoiceAD.TabIndex = 3;
            // 
            // txtChoiceBD
            // 
            this.txtChoiceBD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChoiceBD.Location = new System.Drawing.Point(246, 358);
            this.txtChoiceBD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtChoiceBD.Name = "txtChoiceBD";
            this.txtChoiceBD.Size = new System.Drawing.Size(1572, 43);
            this.txtChoiceBD.TabIndex = 5;
            // 
            // txtChoiceCD
            // 
            this.txtChoiceCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChoiceCD.Location = new System.Drawing.Point(246, 430);
            this.txtChoiceCD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtChoiceCD.Name = "txtChoiceCD";
            this.txtChoiceCD.Size = new System.Drawing.Size(1572, 43);
            this.txtChoiceCD.TabIndex = 7;
            // 
            // txtChoiceDD
            // 
            this.txtChoiceDD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChoiceDD.Location = new System.Drawing.Point(246, 502);
            this.txtChoiceDD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtChoiceDD.Name = "txtChoiceDD";
            this.txtChoiceDD.Size = new System.Drawing.Size(1572, 43);
            this.txtChoiceDD.TabIndex = 9;
            // 
            // cmbCorrectChoiceD
            // 
            this.cmbCorrectChoiceD.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCorrectChoiceD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCorrectChoiceD.Items.AddRange(new object[] {
            "Choice A",
            "Choice B",
            "Choice C",
            "Choice D"});
            this.cmbCorrectChoiceD.Location = new System.Drawing.Point(246, 574);
            this.cmbCorrectChoiceD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbCorrectChoiceD.Name = "cmbCorrectChoiceD";
            this.cmbCorrectChoiceD.Size = new System.Drawing.Size(0, 45);
            this.cmbCorrectChoiceD.TabIndex = 11;
            // 
            // txtExplanationD
            // 
            this.txtExplanationD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExplanationD.Location = new System.Drawing.Point(246, 658);
            this.txtExplanationD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtExplanationD.Multiline = true;
            this.txtExplanationD.Name = "txtExplanationD";
            this.txtExplanationD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExplanationD.Size = new System.Drawing.Size(1572, 208);
            this.txtExplanationD.TabIndex = 13;
            // 
            // lblQEHintD
            // 
            this.lblQEHintD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQEHintD.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblQEHintD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblQEHintD.Location = new System.Drawing.Point(246, 872);
            this.lblQEHintD.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQEHintD.Name = "lblQEHintD";
            this.lblQEHintD.Size = new System.Drawing.Size(1572, 104);
            this.lblQEHintD.TabIndex = 14;
            this.lblQEHintD.Text = "Select an existing question to edit it or start a new one.";
            // 
            // pnlQEQuestionButtons
            // 
            this.pnlQEQuestionButtons.Controls.Add(this.btnApplyD);
            this.pnlQEQuestionButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlQEQuestionButtons.Location = new System.Drawing.Point(36, 695);
            this.pnlQEQuestionButtons.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEQuestionButtons.Name = "pnlQEQuestionButtons";
            this.pnlQEQuestionButtons.Size = new System.Drawing.Size(1858, 104);
            this.pnlQEQuestionButtons.TabIndex = 1;
            // 
            // btnApplyD
            // 
            this.btnApplyD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.btnApplyD.FlatAppearance.BorderSize = 0;
            this.btnApplyD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnApplyD.ForeColor = System.Drawing.Color.White;
            this.btnApplyD.Location = new System.Drawing.Point(0, 8);
            this.btnApplyD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnApplyD.Name = "btnApplyD";
            this.btnApplyD.Size = new System.Drawing.Size(280, 68);
            this.btnApplyD.TabIndex = 0;
            this.btnApplyD.Text = "Apply Question";
            this.btnApplyD.UseVisualStyleBackColor = false;
            // 
            // pnlQEMeta
            // 
            this.pnlQEMeta.BackColor = System.Drawing.Color.White;
            this.pnlQEMeta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQEMeta.Controls.Add(this.pnlQEMetaLayout);
            this.pnlQEMeta.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQEMeta.Location = new System.Drawing.Point(48, 48);
            this.pnlQEMeta.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEMeta.Name = "pnlQEMeta";
            this.pnlQEMeta.Padding = new System.Windows.Forms.Padding(36, 36, 36, 36);
            this.pnlQEMeta.Size = new System.Drawing.Size(2784, 338);
            this.pnlQEMeta.TabIndex = 1;
            // 
            // pnlQEMetaLayout
            // 
            this.pnlQEMetaLayout.ColumnCount = 4;
            this.pnlQEMetaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.pnlQEMetaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlQEMetaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.pnlQEMetaLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlQEMetaLayout.Controls.Add(this.lblQETitleField, 0, 0);
            this.pnlQEMetaLayout.Controls.Add(this.txtTitleD, 1, 0);
            this.pnlQEMetaLayout.Controls.Add(this.lblQESubjectField, 2, 0);
            this.pnlQEMetaLayout.Controls.Add(this.cmbSubjectD, 3, 0);
            this.pnlQEMetaLayout.Controls.Add(this.lblQETopicField, 0, 1);
            this.pnlQEMetaLayout.Controls.Add(this.txtTopicD, 1, 1);
            this.pnlQEMetaLayout.Controls.Add(this.lblQEDiffField, 2, 1);
            this.pnlQEMetaLayout.Controls.Add(this.cmbDifficultyD, 3, 1);
            this.pnlQEMetaLayout.Controls.Add(this.lblQEDurationField, 0, 2);
            this.pnlQEMetaLayout.Controls.Add(this.nudDurationD, 1, 2);
            this.pnlQEMetaLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQEMetaLayout.Location = new System.Drawing.Point(36, 36);
            this.pnlQEMetaLayout.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEMetaLayout.Name = "pnlQEMetaLayout";
            this.pnlQEMetaLayout.RowCount = 4;
            this.pnlQEMetaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.pnlQEMetaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.pnlQEMetaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.pnlQEMetaLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.pnlQEMetaLayout.Size = new System.Drawing.Size(2710, 264);
            this.pnlQEMetaLayout.TabIndex = 0;
            // 
            // lblQETitleField
            // 
            this.lblQETitleField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQETitleField.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQETitleField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblQETitleField.Location = new System.Drawing.Point(6, 0);
            this.lblQETitleField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQETitleField.Name = "lblQETitleField";
            this.lblQETitleField.Size = new System.Drawing.Size(168, 68);
            this.lblQETitleField.TabIndex = 0;
            this.lblQETitleField.Text = "Title";
            this.lblQETitleField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTitleD
            // 
            this.txtTitleD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitleD.Location = new System.Drawing.Point(186, 6);
            this.txtTitleD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTitleD.Name = "txtTitleD";
            this.txtTitleD.Size = new System.Drawing.Size(1158, 43);
            this.txtTitleD.TabIndex = 1;
            // 
            // lblQESubjectField
            // 
            this.lblQESubjectField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQESubjectField.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQESubjectField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblQESubjectField.Location = new System.Drawing.Point(1356, 0);
            this.lblQESubjectField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQESubjectField.Name = "lblQESubjectField";
            this.lblQESubjectField.Size = new System.Drawing.Size(178, 68);
            this.lblQESubjectField.TabIndex = 2;
            this.lblQESubjectField.Text = "Subject";
            this.lblQESubjectField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSubjectD
            // 
            this.cmbSubjectD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSubjectD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubjectD.Location = new System.Drawing.Point(1546, 6);
            this.cmbSubjectD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbSubjectD.Name = "cmbSubjectD";
            this.cmbSubjectD.Size = new System.Drawing.Size(1158, 45);
            this.cmbSubjectD.TabIndex = 3;
            // 
            // lblQETopicField
            // 
            this.lblQETopicField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQETopicField.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQETopicField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblQETopicField.Location = new System.Drawing.Point(6, 68);
            this.lblQETopicField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQETopicField.Name = "lblQETopicField";
            this.lblQETopicField.Size = new System.Drawing.Size(168, 84);
            this.lblQETopicField.TabIndex = 4;
            this.lblQETopicField.Text = "Topic";
            this.lblQETopicField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTopicD
            // 
            this.txtTopicD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTopicD.Location = new System.Drawing.Point(186, 74);
            this.txtTopicD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtTopicD.Name = "txtTopicD";
            this.txtTopicD.Size = new System.Drawing.Size(1158, 43);
            this.txtTopicD.TabIndex = 5;
            // 
            // lblQEDiffField
            // 
            this.lblQEDiffField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQEDiffField.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQEDiffField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblQEDiffField.Location = new System.Drawing.Point(1356, 68);
            this.lblQEDiffField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQEDiffField.Name = "lblQEDiffField";
            this.lblQEDiffField.Size = new System.Drawing.Size(178, 84);
            this.lblQEDiffField.TabIndex = 6;
            this.lblQEDiffField.Text = "Difficulty";
            this.lblQEDiffField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDifficultyD
            // 
            this.cmbDifficultyD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDifficultyD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDifficultyD.Location = new System.Drawing.Point(1546, 74);
            this.cmbDifficultyD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbDifficultyD.Name = "cmbDifficultyD";
            this.cmbDifficultyD.Size = new System.Drawing.Size(1158, 45);
            this.cmbDifficultyD.TabIndex = 7;
            // 
            // lblQEDurationField
            // 
            this.lblQEDurationField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblQEDurationField.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblQEDurationField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblQEDurationField.Location = new System.Drawing.Point(6, 152);
            this.lblQEDurationField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblQEDurationField.Name = "lblQEDurationField";
            this.lblQEDurationField.Size = new System.Drawing.Size(168, 68);
            this.lblQEDurationField.TabIndex = 8;
            this.lblQEDurationField.Text = "Duration";
            this.lblQEDurationField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudDurationD
            // 
            this.nudDurationD.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudDurationD.Location = new System.Drawing.Point(186, 158);
            this.nudDurationD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.nudDurationD.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudDurationD.Name = "nudDurationD";
            this.nudDurationD.Size = new System.Drawing.Size(2, 43);
            this.nudDurationD.TabIndex = 9;
            this.nudDurationD.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // pnlQEBottom
            // 
            this.pnlQEBottom.BackColor = System.Drawing.Color.White;
            this.pnlQEBottom.Controls.Add(this.btnSaveDraftD);
            this.pnlQEBottom.Controls.Add(this.btnCancelD);
            this.pnlQEBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlQEBottom.Location = new System.Drawing.Point(0, 1471);
            this.pnlQEBottom.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlQEBottom.Name = "pnlQEBottom";
            this.pnlQEBottom.Padding = new System.Windows.Forms.Padding(48, 16, 48, 24);
            this.pnlQEBottom.Size = new System.Drawing.Size(2880, 108);
            this.pnlQEBottom.TabIndex = 1;
            // 
            // btnSaveDraftD
            // 
            this.btnSaveDraftD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.btnSaveDraftD.FlatAppearance.BorderSize = 0;
            this.btnSaveDraftD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDraftD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveDraftD.ForeColor = System.Drawing.Color.White;
            this.btnSaveDraftD.Location = new System.Drawing.Point(0, 12);
            this.btnSaveDraftD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSaveDraftD.Name = "btnSaveDraftD";
            this.btnSaveDraftD.Size = new System.Drawing.Size(252, 68);
            this.btnSaveDraftD.TabIndex = 0;
            this.btnSaveDraftD.Text = "Save Draft";
            this.btnSaveDraftD.UseVisualStyleBackColor = false;
            // 
            // btnCancelD
            // 
            this.btnCancelD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelD.Location = new System.Drawing.Point(272, 12);
            this.btnCancelD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCancelD.Name = "btnCancelD";
            this.btnCancelD.Size = new System.Drawing.Size(184, 68);
            this.btnCancelD.TabIndex = 1;
            this.btnCancelD.Text = "Cancel";
            // 
            // QuizEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(2880, 1579);
            this.Controls.Add(this.pnlQEBody);
            this.Controls.Add(this.pnlQEBottom);
            this.Controls.Add(this.pnlQEHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "QuizEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quiz Editor";
            this.pnlQEHeader.ResumeLayout(false);
            this.pnlQEBody.ResumeLayout(false);
            this.pnlQEEditorArea.ResumeLayout(false);
            this.pnlQEListPanel.ResumeLayout(false);
            this.pnlQEListButtons.ResumeLayout(false);
            this.pnlQEQuestionPanel.ResumeLayout(false);
            this.pnlQEQuestionScroll.ResumeLayout(false);
            this.pnlQEQuestionScroll.PerformLayout();
            this.pnlQEQuestionLayout.ResumeLayout(false);
            this.pnlQEQuestionLayout.PerformLayout();
            this.pnlQEQuestionButtons.ResumeLayout(false);
            this.pnlQEMeta.ResumeLayout(false);
            this.pnlQEMetaLayout.ResumeLayout(false);
            this.pnlQEMetaLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationD)).EndInit();
            this.pnlQEBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
