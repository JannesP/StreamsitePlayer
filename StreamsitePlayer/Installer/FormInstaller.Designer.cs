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
            this.buttonCancel.Location = new System.Drawing.Point(734, 240);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 35);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInstall
            // 
            this.buttonInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInstall.Location = new System.Drawing.Point(548, 238);
            this.buttonInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(177, 35);
            this.buttonInstall.TabIndex = 1;
            this.buttonInstall.Text = "Download and Install";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // checkBoxAdvanced
            // 
            this.checkBoxAdvanced.AutoSize = true;
            this.checkBoxAdvanced.Location = new System.Drawing.Point(24, 154);
            this.checkBoxAdvanced.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxAdvanced.Name = "checkBoxAdvanced";
            this.checkBoxAdvanced.Size = new System.Drawing.Size(106, 24);
            this.checkBoxAdvanced.TabIndex = 2;
            this.checkBoxAdvanced.Text = "Advanced";
            this.checkBoxAdvanced.UseVisualStyleBackColor = true;
            this.checkBoxAdvanced.CheckedChanged += new System.EventHandler(this.checkBoxAdvanced_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Do you want to install the player?";
            // 
            // checkBoxStartMenuShortcut
            // 
            this.checkBoxStartMenuShortcut.AutoSize = true;
            this.checkBoxStartMenuShortcut.Checked = true;
            this.checkBoxStartMenuShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartMenuShortcut.Location = new System.Drawing.Point(24, 58);
            this.checkBoxStartMenuShortcut.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxStartMenuShortcut.Name = "checkBoxStartMenuShortcut";
            this.checkBoxStartMenuShortcut.Size = new System.Drawing.Size(176, 24);
            this.checkBoxStartMenuShortcut.TabIndex = 4;
            this.checkBoxStartMenuShortcut.Text = "Start menu shortcut";
            this.checkBoxStartMenuShortcut.UseVisualStyleBackColor = true;
            // 
            // checkBoxDesktopShortcut
            // 
            this.checkBoxDesktopShortcut.AutoSize = true;
            this.checkBoxDesktopShortcut.Checked = true;
            this.checkBoxDesktopShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDesktopShortcut.Location = new System.Drawing.Point(24, 94);
            this.checkBoxDesktopShortcut.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxDesktopShortcut.Name = "checkBoxDesktopShortcut";
            this.checkBoxDesktopShortcut.Size = new System.Drawing.Size(157, 24);
            this.checkBoxDesktopShortcut.TabIndex = 5;
            this.checkBoxDesktopShortcut.Text = "Desktop shortcut";
            this.checkBoxDesktopShortcut.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Location = new System.Drawing.Point(146, 166);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 2);
            this.panel1.TabIndex = 6;
            // 
            // panelAdvanced
            // 
            this.panelAdvanced.Controls.Add(this.buttonBrowseForInstallationPath);
            this.panelAdvanced.Controls.Add(this.label2);
            this.panelAdvanced.Controls.Add(this.textBoxInstallationDir);
            this.panelAdvanced.Location = new System.Drawing.Point(24, 189);
            this.panelAdvanced.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelAdvanced.Name = "panelAdvanced";
            this.panelAdvanced.Size = new System.Drawing.Size(822, 43);
            this.panelAdvanced.TabIndex = 7;
            // 
            // buttonBrowseForInstallationPath
            // 
            this.buttonBrowseForInstallationPath.Location = new System.Drawing.Point(738, 3);
            this.buttonBrowseForInstallationPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonBrowseForInstallationPath.Name = "buttonBrowseForInstallationPath";
            this.buttonBrowseForInstallationPath.Size = new System.Drawing.Size(80, 35);
            this.buttonBrowseForInstallationPath.TabIndex = 2;
            this.buttonBrowseForInstallationPath.Text = "Browse";
            this.buttonBrowseForInstallationPath.UseVisualStyleBackColor = true;
            this.buttonBrowseForInstallationPath.Click += new System.EventHandler(this.buttonBrowseForInstallationPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Installation directory:";
            // 
            // textBoxInstallationDir
            // 
            this.textBoxInstallationDir.Location = new System.Drawing.Point(168, 6);
            this.textBoxInstallationDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxInstallationDir.Name = "textBoxInstallationDir";
            this.textBoxInstallationDir.Size = new System.Drawing.Size(559, 26);
            this.textBoxInstallationDir.TabIndex = 0;
            this.textBoxInstallationDir.TextChanged += new System.EventHandler(this.textBoxInstallationDir_TextChanged);
            // 
            // checkBoxStartAfterInstallation
            // 
            this.checkBoxStartAfterInstallation.AutoSize = true;
            this.checkBoxStartAfterInstallation.Checked = true;
            this.checkBoxStartAfterInstallation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStartAfterInstallation.Location = new System.Drawing.Point(660, 94);
            this.checkBoxStartAfterInstallation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxStartAfterInstallation.Name = "checkBoxStartAfterInstallation";
            this.checkBoxStartAfterInstallation.Size = new System.Drawing.Size(186, 24);
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
            this.checkBoxInstallAllUsers.Location = new System.Drawing.Point(682, 58);
            this.checkBoxInstallAllUsers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxInstallAllUsers.Name = "checkBoxInstallAllUsers";
            this.checkBoxInstallAllUsers.Size = new System.Drawing.Size(162, 24);
            this.checkBoxInstallAllUsers.TabIndex = 1;
            this.checkBoxInstallAllUsers.Text = "Install for all users";
            this.checkBoxInstallAllUsers.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(4, 105);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(816, 35);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // panelProgress
            // 
            this.panelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Location = new System.Drawing.Point(20, 14);
            this.panelProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(826, 228);
            this.panelProgress.TabIndex = 9;
            // 
            // FormInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(864, 294);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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

