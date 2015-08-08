using System;
using System.IO;

namespace StreamsitePlayer
{
    partial class FormJwPlayer
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
            this.labelRequestingStatus = new System.Windows.Forms.Label();
            this.progressBarRequestingStatus = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelRequestingStatus
            // 
            this.labelRequestingStatus.AutoSize = true;
            this.labelRequestingStatus.Location = new System.Drawing.Point(12, 178);
            this.labelRequestingStatus.Name = "labelRequestingStatus";
            this.labelRequestingStatus.Size = new System.Drawing.Size(104, 13);
            this.labelRequestingStatus.TabIndex = 7;
            this.labelRequestingStatus.Text = "Requesting link . . . .";
            // 
            // progressBarRequestingStatus
            // 
            this.progressBarRequestingStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarRequestingStatus.Location = new System.Drawing.Point(12, 197);
            this.progressBarRequestingStatus.Name = "progressBarRequestingStatus";
            this.progressBarRequestingStatus.Size = new System.Drawing.Size(921, 23);
            this.progressBarRequestingStatus.TabIndex = 6;
            // 
            // FormJwPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 478);
            this.Controls.Add(this.labelRequestingStatus);
            this.Controls.Add(this.progressBarRequestingStatus);
            this.KeyPreview = true;
            this.Name = "FormJwPlayer";
            this.Text = "FormJwPlayer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormJwPlayer_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRequestingStatus;
        private System.Windows.Forms.ProgressBar progressBarRequestingStatus;
    }
}