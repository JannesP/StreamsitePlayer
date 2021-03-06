﻿namespace SeriesPlayer.Forms
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
            this.buttonBrowseDownloaded = new System.Windows.Forms.Button();
            this.stateProgressBarLinkRequest = new SeriesPlayer.Forms.StateProgressBar();
            this.stateProgressBarCurrentFile = new SeriesPlayer.Forms.StateProgressBar();
            this.SuspendLayout();
            // 
            // listBoxEpisodes
            // 
            this.listBoxEpisodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxEpisodes.FormattingEnabled = true;
            this.listBoxEpisodes.ItemHeight = 20;
            this.listBoxEpisodes.Location = new System.Drawing.Point(180, 38);
            this.listBoxEpisodes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxEpisodes.Name = "listBoxEpisodes";
            this.listBoxEpisodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxEpisodes.Size = new System.Drawing.Size(376, 424);
            this.listBoxEpisodes.TabIndex = 0;
            this.listBoxEpisodes.DoubleClick += new System.EventHandler(this.listBoxEpisodes_DoubleClick);
            // 
            // buttonDownloadSelected
            // 
            this.buttonDownloadSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSelected.Location = new System.Drawing.Point(918, 38);
            this.buttonDownloadSelected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDownloadSelected.Name = "buttonDownloadSelected";
            this.buttonDownloadSelected.Size = new System.Drawing.Size(172, 35);
            this.buttonDownloadSelected.TabIndex = 1;
            this.buttonDownloadSelected.Text = "Download Selected";
            this.buttonDownloadSelected.UseVisualStyleBackColor = true;
            this.buttonDownloadSelected.Click += new System.EventHandler(this.buttonDownloadSelected_Click);
            // 
            // buttonDownloadSeries
            // 
            this.buttonDownloadSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSeries.Location = new System.Drawing.Point(918, 128);
            this.buttonDownloadSeries.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDownloadSeries.Name = "buttonDownloadSeries";
            this.buttonDownloadSeries.Size = new System.Drawing.Size(172, 35);
            this.buttonDownloadSeries.TabIndex = 2;
            this.buttonDownloadSeries.Text = "Download Series";
            this.buttonDownloadSeries.UseVisualStyleBackColor = true;
            this.buttonDownloadSeries.Click += new System.EventHandler(this.buttonDownloadSeries_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(918, 529);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(172, 35);
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
            this.listBoxSeason.ItemHeight = 20;
            this.listBoxSeason.Location = new System.Drawing.Point(20, 38);
            this.listBoxSeason.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxSeason.Name = "listBoxSeason";
            this.listBoxSeason.Size = new System.Drawing.Size(150, 424);
            this.listBoxSeason.TabIndex = 4;
            this.listBoxSeason.SelectedIndexChanged += new System.EventHandler(this.listBoxSeason_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Seasons:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Episodes:";
            // 
            // buttonDownloadSeason
            // 
            this.buttonDownloadSeason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownloadSeason.Location = new System.Drawing.Point(918, 83);
            this.buttonDownloadSeason.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDownloadSeason.Name = "buttonDownloadSeason";
            this.buttonDownloadSeason.Size = new System.Drawing.Size(172, 35);
            this.buttonDownloadSeason.TabIndex = 7;
            this.buttonDownloadSeason.Text = "Download Season";
            this.buttonDownloadSeason.UseVisualStyleBackColor = true;
            this.buttonDownloadSeason.Click += new System.EventHandler(this.buttonDownloadSeason_Click);
            // 
            // labelAllProgress
            // 
            this.labelAllProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAllProgress.AutoSize = true;
            this.labelAllProgress.Location = new System.Drawing.Point(207, 549);
            this.labelAllProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAllProgress.Name = "labelAllProgress";
            this.labelAllProgress.Size = new System.Drawing.Size(111, 20);
            this.labelAllProgress.TabIndex = 11;
            this.labelAllProgress.Text = "Episodes Left:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 512);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "File Progress:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 472);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Current File:";
            // 
            // labelCurrentEpisode
            // 
            this.labelCurrentEpisode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentEpisode.AutoSize = true;
            this.labelCurrentEpisode.Location = new System.Drawing.Point(124, 472);
            this.labelCurrentEpisode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentEpisode.Name = "labelCurrentEpisode";
            this.labelCurrentEpisode.Size = new System.Drawing.Size(104, 20);
            this.labelCurrentEpisode.TabIndex = 15;
            this.labelCurrentEpisode.Text = "S1 Episode 1";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 549);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 20);
            this.label12.TabIndex = 19;
            this.label12.Text = "Running:";
            // 
            // labelTimeRunning
            // 
            this.labelTimeRunning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimeRunning.AutoSize = true;
            this.labelTimeRunning.Location = new System.Drawing.Point(124, 549);
            this.labelTimeRunning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeRunning.Name = "labelTimeRunning";
            this.labelTimeRunning.Size = new System.Drawing.Size(71, 20);
            this.labelTimeRunning.TabIndex = 20;
            this.labelTimeRunning.Text = "00:00:00";
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.Location = new System.Drawing.Point(834, 549);
            this.labelTimeLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(71, 20);
            this.labelTimeLeft.TabIndex = 22;
            this.labelTimeLeft.Text = "00:00:00";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(735, 549);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "Remaining:";
            // 
            // labelEpisodesLeft
            // 
            this.labelEpisodesLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEpisodesLeft.AutoSize = true;
            this.labelEpisodesLeft.Location = new System.Drawing.Point(314, 549);
            this.labelEpisodesLeft.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEpisodesLeft.Name = "labelEpisodesLeft";
            this.labelEpisodesLeft.Size = new System.Drawing.Size(27, 20);
            this.labelEpisodesLeft.TabIndex = 23;
            this.labelEpisodesLeft.Text = "00";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(352, 549);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 20);
            this.label7.TabIndex = 24;
            this.label7.Text = "Speed:";
            // 
            // labelSpeed
            // 
            this.labelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(423, 549);
            this.labelSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(55, 20);
            this.labelSpeed.TabIndex = 25;
            this.labelSpeed.Text = "0 KB/s";
            // 
            // listBoxDownloadQueue
            // 
            this.listBoxDownloadQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDownloadQueue.FormattingEnabled = true;
            this.listBoxDownloadQueue.ItemHeight = 20;
            this.listBoxDownloadQueue.Location = new System.Drawing.Point(567, 38);
            this.listBoxDownloadQueue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxDownloadQueue.Name = "listBoxDownloadQueue";
            this.listBoxDownloadQueue.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxDownloadQueue.Size = new System.Drawing.Size(338, 424);
            this.listBoxDownloadQueue.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(562, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 27;
            this.label3.Text = "Downloadqueue:";
            // 
            // labelLinkRequest
            // 
            this.labelLinkRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLinkRequest.AutoSize = true;
            this.labelLinkRequest.Location = new System.Drawing.Point(916, 468);
            this.labelLinkRequest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLinkRequest.Name = "labelLinkRequest";
            this.labelLinkRequest.Size = new System.Drawing.Size(103, 20);
            this.labelLinkRequest.TabIndex = 29;
            this.labelLinkRequest.Text = "LinkRequest:";
            this.labelLinkRequest.Visible = false;
            // 
            // buttonRemoveSelected
            // 
            this.buttonRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveSelected.Location = new System.Drawing.Point(918, 292);
            this.buttonRemoveSelected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRemoveSelected.Name = "buttonRemoveSelected";
            this.buttonRemoveSelected.Size = new System.Drawing.Size(172, 35);
            this.buttonRemoveSelected.TabIndex = 30;
            this.buttonRemoveSelected.Text = "Remove Selected";
            this.buttonRemoveSelected.UseVisualStyleBackColor = true;
            this.buttonRemoveSelected.Click += new System.EventHandler(this.buttonRemoveSelected_Click);
            // 
            // buttonBrowseDownloaded
            // 
            this.buttonBrowseDownloaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseDownloaded.Location = new System.Drawing.Point(918, 200);
            this.buttonBrowseDownloaded.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonBrowseDownloaded.Name = "buttonBrowseDownloaded";
            this.buttonBrowseDownloaded.Size = new System.Drawing.Size(172, 54);
            this.buttonBrowseDownloaded.TabIndex = 31;
            this.buttonBrowseDownloaded.Text = "Open Downloads Folder";
            this.buttonBrowseDownloaded.UseVisualStyleBackColor = true;
            this.buttonBrowseDownloaded.Click += new System.EventHandler(this.buttonBrowseDownloaded_Click);
            // 
            // stateProgressBarLinkRequest
            // 
            this.stateProgressBarLinkRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stateProgressBarLinkRequest.Location = new System.Drawing.Point(916, 497);
            this.stateProgressBarLinkRequest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stateProgressBarLinkRequest.Name = "stateProgressBarLinkRequest";
            this.stateProgressBarLinkRequest.Size = new System.Drawing.Size(174, 20);
            this.stateProgressBarLinkRequest.TabIndex = 28;
            this.stateProgressBarLinkRequest.Visible = false;
            // 
            // stateProgressBarCurrentFile
            // 
            this.stateProgressBarCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateProgressBarCurrentFile.Location = new System.Drawing.Point(129, 497);
            this.stateProgressBarCurrentFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stateProgressBarCurrentFile.Name = "stateProgressBarCurrentFile";
            this.stateProgressBarCurrentFile.Size = new System.Drawing.Size(778, 35);
            this.stateProgressBarCurrentFile.TabIndex = 16;
            // 
            // FormDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1108, 583);
            this.Controls.Add(this.buttonBrowseDownloaded);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
        private System.Windows.Forms.Button buttonBrowseDownloaded;
    }
}