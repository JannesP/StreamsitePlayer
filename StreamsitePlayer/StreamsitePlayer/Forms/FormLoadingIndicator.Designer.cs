namespace SeriesPlayer.Forms
{
    partial class FormLoadingIndicator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoadingIndicator));
            this.progressBarLoadingIndicator = new System.Windows.Forms.ProgressBar();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarLoadingIndicator
            // 
            this.progressBarLoadingIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLoadingIndicator.Location = new System.Drawing.Point(18, 18);
            this.progressBarLoadingIndicator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBarLoadingIndicator.Name = "progressBarLoadingIndicator";
            this.progressBarLoadingIndicator.Size = new System.Drawing.Size(498, 35);
            this.progressBarLoadingIndicator.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoadingIndicator.TabIndex = 0;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(18, 65);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(74, 20);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "Message";
            // 
            // FormLoadingIndicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(534, 100);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.progressBarLoadingIndicator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoadingIndicator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Loading ...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLoadingIndicator_FormClosing);
            this.Shown += new System.EventHandler(this.FormLoadingIndicator_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoadingIndicator;
        private System.Windows.Forms.Label labelMessage;
    }
}