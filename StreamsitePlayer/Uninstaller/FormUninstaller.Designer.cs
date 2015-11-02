namespace Uninstaller
{
    partial class FormUninstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUninstaller));
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxRemoveEpisodes = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelUninstalling = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBarProgress = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerUninstall = new System.ComponentModel.BackgroundWorker();
            this.panelFinished = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panelSettings.SuspendLayout();
            this.panelUninstalling.SuspendLayout();
            this.panelFinished.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(354, 90);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "Uninstall";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(435, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxRemoveEpisodes
            // 
            this.checkBoxRemoveEpisodes.AutoSize = true;
            this.checkBoxRemoveEpisodes.Location = new System.Drawing.Point(6, 55);
            this.checkBoxRemoveEpisodes.Name = "checkBoxRemoveEpisodes";
            this.checkBoxRemoveEpisodes.Size = new System.Drawing.Size(178, 17);
            this.checkBoxRemoveEpisodes.TabIndex = 2;
            this.checkBoxRemoveEpisodes.Text = "Remove downloaded episodes?";
            this.checkBoxRemoveEpisodes.UseVisualStyleBackColor = true;
            this.checkBoxRemoveEpisodes.CheckedChanged += new System.EventHandler(this.checkBoxRemoveEpisodes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "You are about to remove the SeriesPlayer completely. ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(262, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Do you also want to remove all downloaded epsodes?";
            // 
            // panelSettings
            // 
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSettings.Controls.Add(this.label1);
            this.panelSettings.Controls.Add(this.label2);
            this.panelSettings.Controls.Add(this.checkBoxRemoveEpisodes);
            this.panelSettings.Location = new System.Drawing.Point(12, 12);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(498, 78);
            this.panelSettings.TabIndex = 5;
            // 
            // panelUninstalling
            // 
            this.panelUninstalling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUninstalling.Controls.Add(this.label3);
            this.panelUninstalling.Controls.Add(this.progressBarProgress);
            this.panelUninstalling.Location = new System.Drawing.Point(12, 96);
            this.panelUninstalling.Name = "panelUninstalling";
            this.panelUninstalling.Size = new System.Drawing.Size(498, 78);
            this.panelUninstalling.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Removing ...";
            // 
            // progressBarProgress
            // 
            this.progressBarProgress.Location = new System.Drawing.Point(3, 33);
            this.progressBarProgress.Name = "progressBarProgress";
            this.progressBarProgress.Size = new System.Drawing.Size(492, 25);
            this.progressBarProgress.TabIndex = 0;
            // 
            // backgroundWorkerUninstall
            // 
            this.backgroundWorkerUninstall.WorkerReportsProgress = true;
            this.backgroundWorkerUninstall.WorkerSupportsCancellation = true;
            this.backgroundWorkerUninstall.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerUninstall_DoWork);
            this.backgroundWorkerUninstall.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerUninstall_ProgressChanged);
            this.backgroundWorkerUninstall.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerUninstall_RunWorkerCompleted);
            // 
            // panelFinished
            // 
            this.panelFinished.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFinished.Controls.Add(this.label4);
            this.panelFinished.Location = new System.Drawing.Point(12, 180);
            this.panelFinished.Name = "panelFinished";
            this.panelFinished.Size = new System.Drawing.Size(498, 78);
            this.panelFinished.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Finished uninstall.";
            // 
            // FormUninstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 125);
            this.ControlBox = false;
            this.Controls.Add(this.panelFinished);
            this.Controls.Add(this.panelUninstalling);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUninstaller";
            this.Text = "SeriesPlayer Uninstaller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUninstaller_FormClosing);
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panelUninstalling.ResumeLayout(false);
            this.panelUninstalling.PerformLayout();
            this.panelFinished.ResumeLayout(false);
            this.panelFinished.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxRemoveEpisodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelUninstalling;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBarProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorkerUninstall;
        private System.Windows.Forms.Panel panelFinished;
        private System.Windows.Forms.Label label4;
    }
}

