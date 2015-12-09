using System;
using System.IO;

namespace SeriesPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJwPlayer));
            this.labelRequestingStatus = new System.Windows.Forms.Label();
            this.progressBarRequestingStatus = new System.Windows.Forms.ProgressBar();
            this.progressBarLoadingNext = new System.Windows.Forms.ProgressBar();
            this.labelUserInformer = new System.Windows.Forms.Label();
            this.buttonFullscreen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRequestingStatus
            // 
            this.labelRequestingStatus.AutoSize = true;
            this.labelRequestingStatus.Location = new System.Drawing.Point(18, 274);
            this.labelRequestingStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRequestingStatus.Name = "labelRequestingStatus";
            this.labelRequestingStatus.Size = new System.Drawing.Size(150, 20);
            this.labelRequestingStatus.TabIndex = 7;
            this.labelRequestingStatus.Text = "Requesting link . . . .";
            // 
            // progressBarRequestingStatus
            // 
            this.progressBarRequestingStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarRequestingStatus.Location = new System.Drawing.Point(18, 303);
            this.progressBarRequestingStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBarRequestingStatus.Name = "progressBarRequestingStatus";
            this.progressBarRequestingStatus.Size = new System.Drawing.Size(1382, 35);
            this.progressBarRequestingStatus.TabIndex = 6;
            // 
            // progressBarLoadingNext
            // 
            this.progressBarLoadingNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBarLoadingNext.Location = new System.Drawing.Point(18, 702);
            this.progressBarLoadingNext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBarLoadingNext.Name = "progressBarLoadingNext";
            this.progressBarLoadingNext.Size = new System.Drawing.Size(156, 15);
            this.progressBarLoadingNext.TabIndex = 8;
            this.progressBarLoadingNext.Visible = false;
            // 
            // labelUserInformer
            // 
            this.labelUserInformer.AutoSize = true;
            this.labelUserInformer.BackColor = System.Drawing.Color.Black;
            this.labelUserInformer.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserInformer.ForeColor = System.Drawing.Color.White;
            this.labelUserInformer.Location = new System.Drawing.Point(18, 180);
            this.labelUserInformer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserInformer.Name = "labelUserInformer";
            this.labelUserInformer.Size = new System.Drawing.Size(406, 55);
            this.labelUserInformer.TabIndex = 10;
            this.labelUserInformer.Text = "labelUserInformer";
            this.labelUserInformer.Visible = false;
            // 
            // buttonFullscreen
            // 
            this.buttonFullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFullscreen.Location = new System.Drawing.Point(1248, 682);
            this.buttonFullscreen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFullscreen.Name = "buttonFullscreen";
            this.buttonFullscreen.Size = new System.Drawing.Size(150, 35);
            this.buttonFullscreen.TabIndex = 11;
            this.buttonFullscreen.Text = "Toggle Fullscreen";
            this.buttonFullscreen.UseVisualStyleBackColor = true;
            this.buttonFullscreen.Click += new System.EventHandler(this.buttonFullscreen_Click);
            // 
            // FormJwPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1418, 735);
            this.Controls.Add(this.buttonFullscreen);
            this.Controls.Add(this.labelUserInformer);
            this.Controls.Add(this.progressBarLoadingNext);
            this.Controls.Add(this.labelRequestingStatus);
            this.Controls.Add(this.progressBarRequestingStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormJwPlayer";
            this.Text = "StreamsitePlayer loading ...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJwPlayer_FormClosing);
            this.Load += new System.EventHandler(this.FormJwPlayer_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormJwPlayer_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRequestingStatus;
        private System.Windows.Forms.ProgressBar progressBarRequestingStatus;
        private System.Windows.Forms.ProgressBar progressBarLoadingNext;
        private System.Windows.Forms.Label labelUserInformer;
        private System.Windows.Forms.Button buttonFullscreen;
    }
}