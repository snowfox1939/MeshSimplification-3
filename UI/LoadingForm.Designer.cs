namespace Polynano.UI
{
    partial class LoadingForm
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
            this.statusBar = new System.Windows.Forms.ProgressBar();
            this.statusBarText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(6, 12);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(266, 20);
            this.statusBar.TabIndex = 0;
            // 
            // statusBarText
            // 
            this.statusBarText.AutoSize = true;
            this.statusBarText.Location = new System.Drawing.Point(91, 12);
            this.statusBarText.Name = "statusBarText";
            this.statusBarText.Size = new System.Drawing.Size(35, 13);
            this.statusBarText.TabIndex = 1;
            this.statusBarText.Text = "label1";
            this.statusBarText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statusBarText.Visible = false;
            // 
            // Polynano
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 46);
            this.Controls.Add(this.statusBarText);
            this.Controls.Add(this.statusBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Polynano";
            this.Text = "Reading from disk...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar statusBar;
        private System.Windows.Forms.Label statusBarText;
    }
}