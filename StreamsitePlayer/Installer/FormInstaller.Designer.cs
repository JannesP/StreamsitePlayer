namespace Installer
{
    partial class FormInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInstaller));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInstall = new System.Windows.Forms.Button();
            this.checkBoxAdvanced = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxStartMenuShortcut = new System.Windows.Forms.CheckBox();
            this.checkBoxDesktopShortcut = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelAdvanced = new System.Windows.Forms.Panel();
            this.buttonBrowseForInstallationPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxInstallationDir = new System.Windows.Forms.TextBox();
            this.checkBoxStartAfterInstallation = new System.Windows.Forms.CheckBox();
            this.backgroundWorkerInstall = new System.ComponentModel.BackgroundWorker();
            this.folderBrowserDialogInstallationFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxInstallAllUsers = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.panelAdvanced.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(489, 156);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInstall
            // 
            this.buttonInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInstall.Location = new System.Drawing.Point(365, 155);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(118, 23);
            this.buttonInstall.TabIndex = 1;
            this.buttonInstall.Text = "Download and Install";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // checkBoxAdvanced
            // 
            this.checkBoxAdvanced.AutoSize = true;
            this.checkBoxAdvanced.Location = new System.Drawing.Point(16, 100);
            this.checkBoxAdvanced.Name = "checkBoxAdvanced";
            this.checkBoxAdvanced.Size = new System.Drawing.Size(75, 17);
            this.checkBoxAdvanced.TabIndex = 2;
            this.checkBoxAdvanced.Text = "Advanced";
            this.checkBoxAdvanced.UseVisualStyleBackColor = true;
            this.checkBoxAdvanced.CheckedChanged += new System.EventHandler(this.checkBoxAdvanced_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Do you want to install the player?";
            // 
            // checkBoxStartMenuShortcut
            // 
            this.checkBoxStartMenuShortcut.AutoSize = true;
            this.checkBoxStartMenuShortcut.Checked = true;
            this.checkBoxStartMenuShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartMenuShortcut.Location = new System.Drawing.Point(16, 38);
            this.checkBoxStartMenuShortcut.Name = "checkBoxStartMenuShortcut";
            this.checkBoxStartMenuShortcut.Size = new System.Drawing.Size(118, 17);
            this.checkBoxStartMenuShortcut.TabIndex = 4;
            this.checkBoxStartMenuShortcut.Text = "Start menu shortcut";
            this.checkBoxStartMenuShortcut.UseVisualStyleBackColor = true;
            // 
            // checkBoxDesktopShortcut
            // 
            this.checkBoxDesktopShortcut.AutoSize = true;
            this.checkBoxDesktopShortcut.Checked = true;
            this.checkBoxDesktopShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDesktopShortcut.Location = new System.Drawing.Point(16, 61);
            this.checkBoxDesktopShortcut.Name = "checkBoxDesktopShortcut";
            this.checkBoxDesktopShortcut.Size = new System.Drawing.Size(107, 17);
            this.checkBoxDesktopShortcut.TabIndex = 5;
            this.checkBoxDesktopShortcut.Text = "Desktop shortcut";
            this.checkBoxDesktopShortcut.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Location = new System.Drawing.Point(97, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 1);
            this.panel1.TabIndex = 6;
            // 
            // panelAdvanced
            // 
            this.panelAdvanced.Controls.Add(this.buttonBrowseForInstallationPath);
            this.panelAdvanced.Controls.Add(this.label2);
            this.panelAdvanced.Controls.Add(this.textBoxInstallationDir);
            this.panelAdvanced.Location = new System.Drawing.Point(16, 123);
            this.panelAdvanced.Name = "panelAdvanced";
            this.panelAdvanced.Size = new System.Drawing.Size(548, 28);
            this.panelAdvanced.TabIndex = 7;
            // 
            // buttonBrowseForInstallationPath
            // 
            this.buttonBrowseForInstallationPath.Location = new System.Drawing.Point(492, 2);
            this.buttonBrowseForInstallationPath.Name = "buttonBrowseForInstallationPath";
            this.buttonBrowseForInstallationPath.Size = new System.Drawing.Size(53, 23);
            this.buttonBrowseForInstallationPath.TabIndex = 2;
            this.buttonBrowseForInstallationPath.Text = "Browse";
            this.buttonBrowseForInstallationPath.UseVisualStyleBackColor = true;
            this.buttonBrowseForInstallationPath.Click += new System.EventHandler(this.buttonBrowseForInstallationPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Installation directory:";
            // 
            // textBoxInstallationDir
            // 
            this.textBoxInstallationDir.Location = new System.Drawing.Point(112, 4);
            this.textBoxInstallationDir.Name = "textBoxInstallationDir";
            this.textBoxInstallationDir.Size = new System.Drawing.Size(374, 20);
            this.textBoxInstallationDir.TabIndex = 0;
            this.textBoxInstallationDir.TextChanged += new System.EventHandler(this.textBoxInstallationDir_TextChanged);
            // 
            // checkBoxStartAfterInstallation
            // 
            this.checkBoxStartAfterInstallation.AutoSize = true;
            this.checkBoxStartAfterInstallation.Checked = true;
            this.checkBoxStartAfterInstallation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartAfterInstallation.Location = new System.Drawing.Point(440, 61);
            this.checkBoxStartAfterInstallation.Name = "checkBoxStartAfterInstallation";
            this.checkBoxStartAfterInstallation.Size = new System.Drawing.Size(124, 17);
            this.checkBoxStartAfterInstallation.TabIndex = 8;
            this.checkBoxStartAfterInstallation.Text = "Start after installation";
            this.checkBoxStartAfterInstallation.UseVisualStyleBackColor = true;
            // 
            // backgroundWorkerInstall
            // 
            this.backgroundWorkerInstall.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerInstall_DoWork);
            this.backgroundWorkerInstall.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerInstall_RunWorkerCompleted);
            // 
            // folderBrowserDialogInstallationFolder
            // 
            this.folderBrowserDialogInstallationFolder.Description = "Select the installation folder.";
            this.folderBrowserDialogInstallationFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // checkBoxInstallAllUsers
            // 
            this.checkBoxInstallAllUsers.AutoSize = true;
            this.checkBoxInstallAllUsers.Checked = true;
            this.checkBoxInstallAllUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInstallAllUsers.Location = new System.Drawing.Point(455, 38);
            this.checkBoxInstallAllUsers.Name = "checkBoxInstallAllUsers";
            this.checkBoxInstallAllUsers.Size = new System.Drawing.Size(109, 17);
            this.checkBoxInstallAllUsers.TabIndex = 1;
            this.checkBoxInstallAllUsers.Text = "Install for all users";
            this.checkBoxInstallAllUsers.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 68);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(544, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // panelProgress
            // 
            this.panelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Location = new System.Drawing.Point(13, 9);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(551, 148);
            this.panelProgress.TabIndex = 9;
            // 
            // FormInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 191);
            this.Controls.Add(this.checkBoxStartAfterInstallation);
            this.Controls.Add(this.panelAdvanced);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBoxDesktopShortcut);
            this.Controls.Add(this.checkBoxStartMenuShortcut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxAdvanced);
            this.Controls.Add(this.buttonInstall);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxInstallAllUsers);
            this.Controls.Add(this.panelProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormInstaller";
            this.Text = "SeriesPlayer Installer";
            this.Load += new System.EventHandler(this.FormInstaller_Load);
            this.panelAdvanced.ResumeLayout(false);
            this.panelAdvanced.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.CheckBox checkBoxAdvanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxStartMenuShortcut;
        private System.Windows.Forms.CheckBox checkBoxDesktopShortcut;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelAdvanced;
        private System.Windows.Forms.Button buttonBrowseForInstallationPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxInstallationDir;
        private System.Windows.Forms.CheckBox checkBoxStartAfterInstallation;
        private System.ComponentModel.BackgroundWorker backgroundWorkerInstall;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogInstallationFolder;
        private System.Windows.Forms.CheckBox checkBoxInstallAllUsers;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelProgress;
    }
}

