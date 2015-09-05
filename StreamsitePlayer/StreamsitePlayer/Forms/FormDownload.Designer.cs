namespace StreamsitePlayer.Forms
{
    partial class FormDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDownload));
            this.listBoxEpisodes = new System.Windows.Forms.ListBox();
            this.buttonDownloadSelected = new System.Windows.Forms.Button();
            this.buttonDownloadSeries = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listBoxSeason = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDownloadSeason = new System.Windows.Forms.Button();
            this.labelAllProgress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCurrentEpisode = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelTimeRunning = new System.Windows.Forms.Label();
            this.labelTimeLeft = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelEpisodesLeft = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.listBoxDownloadQueue = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelLinkRequest = new System.Windows.Forms.Label();
            this.buttonRemoveSelected = new System.Windows.Forms.Button();
            this.stateProgressBarLinkRequest = new StreamsitePlayer.Forms.StateProgressBar();
            this.stateProgressBarCurrentFile = new StreamsitePlayer.Forms.StateProgressBar();
            this.SuspendLayout();
            // 
            // listBoxEpisodes
            // 
            this.listBoxEpisodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxEpisodes.FormattingEnabled = true;
            this.listBoxEpisodes.Location = new System.Drawing.Point(120, 25);
            this.listBoxEpisodes.Name = "listBoxEpisodes";
            this.listBoxEpisodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxEpisodes.Size = new System.Drawing.Size(252, 277);
            this.listBoxEpisodes.TabIndex = 0;
            this.listBoxEpisodes.DoubleClick += new System.EventHandler(this.listBoxEpisodes_DoubleClick);
            // 
            // buttonDownloadSelected
            // 
            this.buttonDownloadSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSelected.Location = new System.Drawing.Point(612, 25);
            this.buttonDownloadSelected.Name = "buttonDownloadSelected";
            this.buttonDownloadSelected.Size = new System.Drawing.Size(115, 23);
            this.buttonDownloadSelected.TabIndex = 1;
            this.buttonDownloadSelected.Text = "Download Selected";
            this.buttonDownloadSelected.UseVisualStyleBackColor = true;
            this.buttonDownloadSelected.Click += new System.EventHandler(this.buttonDownloadSelected_Click);
            // 
            // buttonDownloadSeries
            // 
            this.buttonDownloadSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSeries.Location = new System.Drawing.Point(612, 83);
            this.buttonDownloadSeries.Name = "buttonDownloadSeries";
            this.buttonDownloadSeries.Size = new System.Drawing.Size(115, 23);
            this.buttonDownloadSeries.TabIndex = 2;
            this.buttonDownloadSeries.Text = "Download Series";
            this.buttonDownloadSeries.UseVisualStyleBackColor = true;
            this.buttonDownloadSeries.Click += new System.EventHandler(this.buttonDownloadSeries_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(612, 344);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(115, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listBoxSeason
            // 
            this.listBoxSeason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxSeason.FormattingEnabled = true;
            this.listBoxSeason.Location = new System.Drawing.Point(13, 25);
            this.listBoxSeason.Name = "listBoxSeason";
            this.listBoxSeason.Size = new System.Drawing.Size(101, 277);
            this.listBoxSeason.TabIndex = 4;
            this.listBoxSeason.SelectedIndexChanged += new System.EventHandler(this.listBoxSeason_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Seasons:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Episodes:";
            // 
            // buttonDownloadSeason
            // 
            this.buttonDownloadSeason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSeason.Location = new System.Drawing.Point(612, 54);
            this.buttonDownloadSeason.Name = "buttonDownloadSeason";
            this.buttonDownloadSeason.Size = new System.Drawing.Size(115, 23);
            this.buttonDownloadSeason.TabIndex = 7;
            this.buttonDownloadSeason.Text = "Download Season";
            this.buttonDownloadSeason.UseVisualStyleBackColor = true;
            this.buttonDownloadSeason.Click += new System.EventHandler(this.buttonDownloadSeason_Click);
            // 
            // labelAllProgress
            // 
            this.labelAllProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAllProgress.AutoSize = true;
            this.labelAllProgress.Location = new System.Drawing.Point(138, 357);
            this.labelAllProgress.Name = "labelAllProgress";
            this.labelAllProgress.Size = new System.Drawing.Size(74, 13);
            this.labelAllProgress.TabIndex = 11;
            this.labelAllProgress.Text = "Episodes Left:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 333);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "File Progress:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Current File:";
            // 
            // labelCurrentEpisode
            // 
            this.labelCurrentEpisode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentEpisode.AutoSize = true;
            this.labelCurrentEpisode.Location = new System.Drawing.Point(83, 307);
            this.labelCurrentEpisode.Name = "labelCurrentEpisode";
            this.labelCurrentEpisode.Size = new System.Drawing.Size(70, 13);
            this.labelCurrentEpisode.TabIndex = 15;
            this.labelCurrentEpisode.Text = "S1 Episode 1";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 357);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Running:";
            // 
            // labelTimeRunning
            // 
            this.labelTimeRunning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimeRunning.AutoSize = true;
            this.labelTimeRunning.Location = new System.Drawing.Point(83, 357);
            this.labelTimeRunning.Name = "labelTimeRunning";
            this.labelTimeRunning.Size = new System.Drawing.Size(49, 13);
            this.labelTimeRunning.TabIndex = 20;
            this.labelTimeRunning.Text = "00:00:00";
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.Location = new System.Drawing.Point(556, 357);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(49, 13);
            this.labelTimeLeft.TabIndex = 22;
            this.labelTimeLeft.Text = "00:00:00";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(490, 357);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Remaining:";
            // 
            // labelEpisodesLeft
            // 
            this.labelEpisodesLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEpisodesLeft.AutoSize = true;
            this.labelEpisodesLeft.Location = new System.Drawing.Point(209, 357);
            this.labelEpisodesLeft.Name = "labelEpisodesLeft";
            this.labelEpisodesLeft.Size = new System.Drawing.Size(19, 13);
            this.labelEpisodesLeft.TabIndex = 23;
            this.labelEpisodesLeft.Text = "00";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(235, 357);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Speed:";
            // 
            // labelSpeed
            // 
            this.labelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(282, 357);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(40, 13);
            this.labelSpeed.TabIndex = 25;
            this.labelSpeed.Text = "0 KB/s";
            // 
            // listBoxDownloadQueue
            // 
            this.listBoxDownloadQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDownloadQueue.FormattingEnabled = true;
            this.listBoxDownloadQueue.Location = new System.Drawing.Point(378, 25);
            this.listBoxDownloadQueue.Name = "listBoxDownloadQueue";
            this.listBoxDownloadQueue.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxDownloadQueue.Size = new System.Drawing.Size(227, 277);
            this.listBoxDownloadQueue.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Downloadqueue:";
            // 
            // labelLinkRequest
            // 
            this.labelLinkRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLinkRequest.AutoSize = true;
            this.labelLinkRequest.Location = new System.Drawing.Point(611, 304);
            this.labelLinkRequest.Name = "labelLinkRequest";
            this.labelLinkRequest.Size = new System.Drawing.Size(70, 13);
            this.labelLinkRequest.TabIndex = 29;
            this.labelLinkRequest.Text = "LinkRequest:";
            this.labelLinkRequest.Visible = false;
            // 
            // buttonRemoveSelected
            // 
            this.buttonRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveSelected.Location = new System.Drawing.Point(612, 190);
            this.buttonRemoveSelected.Name = "buttonRemoveSelected";
            this.buttonRemoveSelected.Size = new System.Drawing.Size(115, 23);
            this.buttonRemoveSelected.TabIndex = 30;
            this.buttonRemoveSelected.Text = "Remove Selected";
            this.buttonRemoveSelected.UseVisualStyleBackColor = true;
            this.buttonRemoveSelected.Click += new System.EventHandler(this.buttonRemoveSelected_Click);
            // 
            // stateProgressBarLinkRequest
            // 
            this.stateProgressBarLinkRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stateProgressBarLinkRequest.Location = new System.Drawing.Point(611, 323);
            this.stateProgressBarLinkRequest.Name = "stateProgressBarLinkRequest";
            this.stateProgressBarLinkRequest.Size = new System.Drawing.Size(116, 13);
            this.stateProgressBarLinkRequest.TabIndex = 28;
            this.stateProgressBarLinkRequest.Visible = false;
            // 
            // stateProgressBarCurrentFile
            // 
            this.stateProgressBarCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateProgressBarCurrentFile.Location = new System.Drawing.Point(86, 323);
            this.stateProgressBarCurrentFile.Name = "stateProgressBarCurrentFile";
            this.stateProgressBarCurrentFile.Size = new System.Drawing.Size(519, 23);
            this.stateProgressBarCurrentFile.TabIndex = 16;
            // 
            // FormDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 379);
            this.Controls.Add(this.buttonRemoveSelected);
            this.Controls.Add(this.labelLinkRequest);
            this.Controls.Add(this.stateProgressBarLinkRequest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxDownloadQueue);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelEpisodesLeft);
            this.Controls.Add(this.labelTimeLeft);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelTimeRunning);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.stateProgressBarCurrentFile);
            this.Controls.Add(this.labelCurrentEpisode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelAllProgress);
            this.Controls.Add(this.buttonDownloadSeason);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSeason);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDownloadSeries);
            this.Controls.Add(this.buttonDownloadSelected);
            this.Controls.Add(this.listBoxEpisodes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDownload";
            this.Text = "StreamDownload";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDownload_FormClosing);
            this.Load += new System.EventHandler(this.FormDownload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxEpisodes;
        private System.Windows.Forms.Button buttonDownloadSelected;
        private System.Windows.Forms.Button buttonDownloadSeries;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListBox listBoxSeason;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDownloadSeason;
        private System.Windows.Forms.Label labelAllProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCurrentEpisode;
        private StateProgressBar stateProgressBarCurrentFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelTimeRunning;
        private System.Windows.Forms.Label labelTimeLeft;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelEpisodesLeft;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.ListBox listBoxDownloadQueue;
        private System.Windows.Forms.Label label3;
        private StateProgressBar stateProgressBarLinkRequest;
        private System.Windows.Forms.Label labelLinkRequest;
        private System.Windows.Forms.Button buttonRemoveSelected;
    }
}