namespace Polynano.UI
{
    partial class GuiApplicationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.modelStatsTable = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.statsSizeLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StatsVertexCountLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StatsFaceCountLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statsComplexityLabel = new System.Windows.Forms.Label();
            this.simplifyGroupBox = new System.Windows.Forms.GroupBox();
            this.simplifyButton = new System.Windows.Forms.Button();
            this.complexityTrackbar = new System.Windows.Forms.TrackBar();
            this.viewGroupBox = new System.Windows.Forms.GroupBox();
            this.viewVerticesCheckbox = new System.Windows.Forms.CheckBox();
            this.viewEdgesCheckbox = new System.Windows.Forms.CheckBox();
            this.viewFacesCheckbox = new System.Windows.Forms.CheckBox();
            this.modelGroupBox = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.authorLabel = new System.Windows.Forms.Label();
            this.simplificationTimer = new System.Windows.Forms.Timer(this.components);
            this.leftPanel.SuspendLayout();
            this.modelStatsTable.SuspendLayout();
            this.simplifyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.complexityTrackbar)).BeginInit();
            this.viewGroupBox.SuspendLayout();
            this.modelGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.modelStatsTable);
            this.leftPanel.Controls.Add(this.simplifyGroupBox);
            this.leftPanel.Controls.Add(this.viewGroupBox);
            this.leftPanel.Controls.Add(this.modelGroupBox);
            this.leftPanel.Location = new System.Drawing.Point(0, 1);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(160, 431);
            this.leftPanel.TabIndex = 0;
            // 
            // modelStatsTable
            // 
            this.modelStatsTable.ColumnCount = 2;
            this.modelStatsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.modelStatsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.modelStatsTable.Controls.Add(this.label6, 0, 3);
            this.modelStatsTable.Controls.Add(this.statsSizeLabel, 1, 3);
            this.modelStatsTable.Controls.Add(this.label1, 0, 2);
            this.modelStatsTable.Controls.Add(this.StatsVertexCountLabel, 1, 2);
            this.modelStatsTable.Controls.Add(this.label4, 0, 1);
            this.modelStatsTable.Controls.Add(this.StatsFaceCountLabel, 1, 1);
            this.modelStatsTable.Controls.Add(this.label2, 0, 0);
            this.modelStatsTable.Controls.Add(this.statsComplexityLabel, 1, 0);
            this.modelStatsTable.Location = new System.Drawing.Point(8, 304);
            this.modelStatsTable.Name = "modelStatsTable";
            this.modelStatsTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.modelStatsTable.RowCount = 4;
            this.modelStatsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.modelStatsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.modelStatsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.modelStatsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.modelStatsTable.Size = new System.Drawing.Size(142, 118);
            this.modelStatsTable.TabIndex = 2;
            this.modelStatsTable.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Größe:";
            // 
            // statsSizeLabel
            // 
            this.statsSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.statsSizeLabel.AutoSize = true;
            this.statsSizeLabel.Location = new System.Drawing.Point(78, 95);
            this.statsSizeLabel.Name = "statsSizeLabel";
            this.statsSizeLabel.Size = new System.Drawing.Size(61, 13);
            this.statsSizeLabel.TabIndex = 14;
            this.statsSizeLabel.Text = "-";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Ecken:";
            // 
            // StatsVertexCountLabel
            // 
            this.StatsVertexCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.StatsVertexCountLabel.AutoSize = true;
            this.StatsVertexCountLabel.Location = new System.Drawing.Point(78, 65);
            this.StatsVertexCountLabel.Name = "StatsVertexCountLabel";
            this.StatsVertexCountLabel.Size = new System.Drawing.Size(61, 13);
            this.StatsVertexCountLabel.TabIndex = 13;
            this.StatsVertexCountLabel.Text = "-";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Flächen:";
            // 
            // StatsFaceCountLabel
            // 
            this.StatsFaceCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.StatsFaceCountLabel.AutoSize = true;
            this.StatsFaceCountLabel.Location = new System.Drawing.Point(78, 36);
            this.StatsFaceCountLabel.Name = "StatsFaceCountLabel";
            this.StatsFaceCountLabel.Size = new System.Drawing.Size(61, 13);
            this.StatsFaceCountLabel.TabIndex = 12;
            this.StatsFaceCountLabel.Text = "-";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Komplexität:";
            // 
            // statsComplexityLabel
            // 
            this.statsComplexityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.statsComplexityLabel.AutoSize = true;
            this.statsComplexityLabel.Location = new System.Drawing.Point(78, 8);
            this.statsComplexityLabel.Name = "statsComplexityLabel";
            this.statsComplexityLabel.Size = new System.Drawing.Size(61, 13);
            this.statsComplexityLabel.TabIndex = 16;
            this.statsComplexityLabel.Text = "-";
            // 
            // simplifyGroupBox
            // 
            this.simplifyGroupBox.Controls.Add(this.simplifyButton);
            this.simplifyGroupBox.Controls.Add(this.complexityTrackbar);
            this.simplifyGroupBox.Location = new System.Drawing.Point(3, 211);
            this.simplifyGroupBox.Name = "simplifyGroupBox";
            this.simplifyGroupBox.Size = new System.Drawing.Size(154, 87);
            this.simplifyGroupBox.TabIndex = 4;
            this.simplifyGroupBox.TabStop = false;
            this.simplifyGroupBox.Text = "Vereinfachen";
            // 
            // simplifyButton
            // 
            this.simplifyButton.Enabled = false;
            this.simplifyButton.Location = new System.Drawing.Point(7, 45);
            this.simplifyButton.Name = "simplifyButton";
            this.simplifyButton.Size = new System.Drawing.Size(142, 31);
            this.simplifyButton.TabIndex = 2;
            this.simplifyButton.Text = "Vereinfachen";
            this.simplifyButton.UseVisualStyleBackColor = true;
            this.simplifyButton.Click += new System.EventHandler(this.OnSimplifyButtonClicked);
            // 
            // complexityTrackbar
            // 
            this.complexityTrackbar.Enabled = false;
            this.complexityTrackbar.LargeChange = 1;
            this.complexityTrackbar.Location = new System.Drawing.Point(7, 19);
            this.complexityTrackbar.Maximum = 500;
            this.complexityTrackbar.Name = "complexityTrackbar";
            this.complexityTrackbar.Size = new System.Drawing.Size(142, 45);
            this.complexityTrackbar.TabIndex = 0;
            this.complexityTrackbar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.complexityTrackbar.Value = 100;
            this.complexityTrackbar.ValueChanged += new System.EventHandler(this.OnComplexityTrackbar_ValueChanged);
            // 
            // viewGroupBox
            // 
            this.viewGroupBox.Controls.Add(this.viewVerticesCheckbox);
            this.viewGroupBox.Controls.Add(this.viewEdgesCheckbox);
            this.viewGroupBox.Controls.Add(this.viewFacesCheckbox);
            this.viewGroupBox.Location = new System.Drawing.Point(3, 119);
            this.viewGroupBox.Name = "viewGroupBox";
            this.viewGroupBox.Size = new System.Drawing.Size(154, 86);
            this.viewGroupBox.TabIndex = 3;
            this.viewGroupBox.TabStop = false;
            this.viewGroupBox.Text = "Ansicht";
            // 
            // viewVerticesCheckbox
            // 
            this.viewVerticesCheckbox.AutoSize = true;
            this.viewVerticesCheckbox.Location = new System.Drawing.Point(5, 65);
            this.viewVerticesCheckbox.Name = "viewVerticesCheckbox";
            this.viewVerticesCheckbox.Size = new System.Drawing.Size(57, 17);
            this.viewVerticesCheckbox.TabIndex = 2;
            this.viewVerticesCheckbox.Text = "Ecken";
            this.viewVerticesCheckbox.UseVisualStyleBackColor = true;
            this.viewVerticesCheckbox.CheckedChanged += new System.EventHandler(this.OnViewVerticesCheckbox_CheckedChanged);
            // 
            // viewEdgesCheckbox
            // 
            this.viewEdgesCheckbox.AutoSize = true;
            this.viewEdgesCheckbox.Location = new System.Drawing.Point(5, 42);
            this.viewEdgesCheckbox.Name = "viewEdgesCheckbox";
            this.viewEdgesCheckbox.Size = new System.Drawing.Size(60, 17);
            this.viewEdgesCheckbox.TabIndex = 1;
            this.viewEdgesCheckbox.Text = "Kanten";
            this.viewEdgesCheckbox.UseVisualStyleBackColor = true;
            this.viewEdgesCheckbox.CheckedChanged += new System.EventHandler(this.OnViewEdgesCheckbox_CheckedChanged);
            // 
            // viewFacesCheckbox
            // 
            this.viewFacesCheckbox.AutoSize = true;
            this.viewFacesCheckbox.Checked = true;
            this.viewFacesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewFacesCheckbox.Location = new System.Drawing.Point(5, 19);
            this.viewFacesCheckbox.Name = "viewFacesCheckbox";
            this.viewFacesCheckbox.Size = new System.Drawing.Size(64, 17);
            this.viewFacesCheckbox.TabIndex = 0;
            this.viewFacesCheckbox.Text = "Flächen";
            this.viewFacesCheckbox.UseVisualStyleBackColor = true;
            this.viewFacesCheckbox.CheckedChanged += new System.EventHandler(this.OnViewFacesCheckbox_CheckedChanged);
            // 
            // modelGroupBox
            // 
            this.modelGroupBox.Controls.Add(this.saveButton);
            this.modelGroupBox.Controls.Add(this.loadButton);
            this.modelGroupBox.Location = new System.Drawing.Point(3, 3);
            this.modelGroupBox.Name = "modelGroupBox";
            this.modelGroupBox.Size = new System.Drawing.Size(154, 110);
            this.modelGroupBox.TabIndex = 2;
            this.modelGroupBox.TabStop = false;
            this.modelGroupBox.Text = "Modell";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(5, 61);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(143, 38);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClicked);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(6, 19);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(142, 36);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Laden";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.OnLoadButtonClicked);
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Location = new System.Drawing.Point(25, 512);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(114, 13);
            this.authorLabel.TabIndex = 1;
            this.authorLabel.Text = "Abschlussprojekt 2018";
            // 
            // simplificationTimer
            // 
            this.simplificationTimer.Tick += new System.EventHandler(this.OnSimplificationTimerTick);
            // 
            // GuiApplicationForm
            // 
            this.ClientSize = new System.Drawing.Size(1317, 816);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.leftPanel);
            this.Name = "GuiApplicationForm";
            this.Text = "Abs 2018";
            this.leftPanel.ResumeLayout(false);
            this.modelStatsTable.ResumeLayout(false);
            this.modelStatsTable.PerformLayout();
            this.simplifyGroupBox.ResumeLayout(false);
            this.simplifyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.complexityTrackbar)).EndInit();
            this.viewGroupBox.ResumeLayout(false);
            this.viewGroupBox.PerformLayout();
            this.modelGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.GroupBox simplifyGroupBox;
        private System.Windows.Forms.GroupBox viewGroupBox;
        private System.Windows.Forms.CheckBox viewVerticesCheckbox;
        private System.Windows.Forms.CheckBox viewEdgesCheckbox;
        private System.Windows.Forms.CheckBox viewFacesCheckbox;
        private System.Windows.Forms.GroupBox modelGroupBox;
        private System.Windows.Forms.Button simplifyButton;
        private System.Windows.Forms.TrackBar complexityTrackbar;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Timer simplificationTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel modelStatsTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label StatsFaceCountLabel;
        private System.Windows.Forms.Label StatsVertexCountLabel;
        private System.Windows.Forms.Label statsSizeLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label statsComplexityLabel;
    }
}