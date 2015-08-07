using System.Runtime.ExceptionServices;

namespace StreamsitePlayer
{
    partial class FormPlayerVlc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        [HandleProcessCorruptedStateExceptions]
        protected override void Dispose(bool disposing)
        {
            try     //Really dirty hack to catch an exception which is thrown by the VlcControl on dispose.
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonFullscreen = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.progressBarRequestingStatus = new System.Windows.Forms.ProgressBar();
            this.labelRequestingStatus = new System.Windows.Forms.Label();
            this.trackBarTime = new System.Windows.Forms.TrackBar();
            this.labelTimeMax = new System.Windows.Forms.Label();
            this.labelTimePlayed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlay.Location = new System.Drawing.Point(13, 442);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(575, 442);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonFullscreen
            // 
            this.buttonFullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFullscreen.Location = new System.Drawing.Point(666, 442);
            this.buttonFullscreen.Name = "buttonFullscreen";
            this.buttonFullscreen.Size = new System.Drawing.Size(68, 23);
            this.buttonFullscreen.TabIndex = 2;
            this.buttonFullscreen.Text = "Fullscreen";
            this.buttonFullscreen.UseVisualStyleBackColor = true;
            this.buttonFullscreen.Click += new System.EventHandler(this.buttonFullscreen_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrevious.Location = new System.Drawing.Point(494, 442);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(75, 23);
            this.buttonPrevious.TabIndex = 3;
            this.buttonPrevious.Text = "Previous";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // progressBarRequestingStatus
            // 
            this.progressBarRequestingStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarRequestingStatus.Location = new System.Drawing.Point(13, 222);
            this.progressBarRequestingStatus.Name = "progressBarRequestingStatus";
            this.progressBarRequestingStatus.Size = new System.Drawing.Size(722, 23);
            this.progressBarRequestingStatus.TabIndex = 4;
            // 
            // labelRequestingStatus
            // 
            this.labelRequestingStatus.AutoSize = true;
            this.labelRequestingStatus.Location = new System.Drawing.Point(13, 203);
            this.labelRequestingStatus.Name = "labelRequestingStatus";
            this.labelRequestingStatus.Size = new System.Drawing.Size(104, 13);
            this.labelRequestingStatus.TabIndex = 5;
            this.labelRequestingStatus.Text = "Requesting link . . . .";
            // 
            // trackBarTime
            // 
            this.trackBarTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTime.Location = new System.Drawing.Point(171, 442);
            this.trackBarTime.Maximum = 2147483647;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(240, 45);
            this.trackBarTime.TabIndex = 6;
            this.trackBarTime.ValueChanged += new System.EventHandler(this.trackBarTime_ValueChanged);
            // 
            // labelTimeMax
            // 
            this.labelTimeMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeMax.AutoSize = true;
            this.labelTimeMax.BackColor = System.Drawing.Color.Transparent;
            this.labelTimeMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeMax.Location = new System.Drawing.Point(417, 442);
            this.labelTimeMax.Name = "labelTimeMax";
            this.labelTimeMax.Size = new System.Drawing.Size(71, 25);
            this.labelTimeMax.TabIndex = 8;
            this.labelTimeMax.Text = "00:00";
            // 
            // labelTimePlayed
            // 
            this.labelTimePlayed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimePlayed.AutoSize = true;
            this.labelTimePlayed.BackColor = System.Drawing.Color.Transparent;
            this.labelTimePlayed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimePlayed.Location = new System.Drawing.Point(94, 443);
            this.labelTimePlayed.Name = "labelTimePlayed";
            this.labelTimePlayed.Size = new System.Drawing.Size(71, 25);
            this.labelTimePlayed.TabIndex = 9;
            this.labelTimePlayed.Text = "00:00";
            // 
            // FormPlayerVlc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 477);
            this.Controls.Add(this.labelTimePlayed);
            this.Controls.Add(this.labelTimeMax);
            this.Controls.Add(this.trackBarTime);
            this.Controls.Add(this.labelRequestingStatus);
            this.Controls.Add(this.progressBarRequestingStatus);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonFullscreen);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPlay);
            this.KeyPreview = true;
            this.Name = "FormPlayerVlc";
            this.Text = "FormVlcPlayer";
            this.Deactivate += new System.EventHandler(this.FormPlayerVlc_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlayerVlc_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVlcPlayer_KeyDown);
            this.MouseLeave += new System.EventHandler(this.FormPlayerVlc_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonFullscreen;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.ProgressBar progressBarRequestingStatus;
        private System.Windows.Forms.Label labelRequestingStatus;
        private System.Windows.Forms.TrackBar trackBarTime;
        private System.Windows.Forms.Label labelTimeMax;
        private System.Windows.Forms.Label labelTimePlayed;
    }
}