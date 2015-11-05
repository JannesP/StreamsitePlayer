using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater;

namespace Installer
{
    public partial class FormInstaller : Form
    {
        private static Size standardSize = new Size(592, 194);
        private static Size advancedSize = new Size(592, 229);
        private string installPath;
        private volatile InstallationInfo installInfo;

        public FormInstaller()
        {
            InitializeComponent();
        }

        private void FormInstaller_Load(object sender, EventArgs e)
        {
            this.Advanced = false;
            this.Controls.SetChildIndex(panelProgress, 1);
            textBoxInstallationDir.Text = Util.GetDefaultPath();
            IsRunning = false;
            installPath = Util.GetDefaultPath();
        }

        private void checkBoxAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            this.Advanced = checkBoxAdvanced.Checked;
        }

        private bool Advanced
        {
            get
            {
                return checkBoxAdvanced.Checked;
            }
            set
            {
                this.Size = value ? advancedSize : standardSize;
                panelAdvanced.Visible = value;
            }
        }

        private bool IsRunning
        {
            set
            {
                panelProgress.Visible = value;
                buttonBrowseForInstallationPath.Enabled = !value;
                buttonCancel.Enabled = !value;
                buttonInstall.Enabled = !value;
                checkBoxAdvanced.Enabled = !value;
                checkBoxStartAfterInstallation.Enabled = !value;
                checkBoxStartMenuShortcut.Enabled = !value;
                checkBoxInstallAllUsers.Enabled = !value;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            string installationPath = Util.GetDefaultPath();
            if (Advanced)
            {
                if (textBoxInstallationDir.Text.IsValidRootedFolderPath())
                {
                    installationPath = textBoxInstallationDir.Text;
                }
                else
                {
                    MessageBox.Show("The typed path was not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            Install(checkBoxStartMenuShortcut.Checked, checkBoxDesktopShortcut.Checked, checkBoxStartAfterInstallation.Checked, checkBoxInstallAllUsers.Checked, installationPath);
        }

        private void Install(bool startMenuEntry, bool desktopShortcut, bool startAfterInstallation, bool installAllUsers, string installationFolder)
        {
            installInfo = new InstallationInfo(startMenuEntry, desktopShortcut, startAfterInstallation, installAllUsers, installationFolder);
            IsRunning = true;
            backgroundWorkerInstall.RunWorkerAsync();
        }

        private void backgroundWorkerInstall_DoWork(object sender, DoWorkEventArgs e)
        {
            string installPath = installInfo.InstallationFolder;
            DirectoryInfo installDir = new DirectoryInfo(installPath);
            if (installDir.Exists)
            {
                if (installDir.GetFiles().Length != 0 || installDir.GetDirectories().Length != 0)   //not empty
                {
                    e.Result = "The installation folder is not empty!";
                    return;
                }
            }
            else
            {
                try
                {
                    installDir.CreateWithParents();
                }
                catch
                {
                    e.Result = "Don't have the right to create to the installation folder!";
                    return;
                }
            }

            string downloadCacheFile = Guid.NewGuid().ToString();
            downloadCacheFile = Path.Combine(Path.GetTempPath(), downloadCacheFile);
            if (!Util.DownloadUpdaterTo(downloadCacheFile))
            {
                e.Result = "Can't connect to the download server or can't write to temp directory!";
                return;
            }
            string version = Util.DownloadNewestVersion();
            if (version == "")
            {
                e.Result = "Can't connect to the download server!";
                return;
            }

            File.Move(downloadCacheFile, Path.Combine(installDir.FullName, "updater.exe"));

            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(installDir.FullName, "updater.exe"));
            psi.Arguments = " -ver=" + version;
            
            if (!installInfo.StartAfterInstallation)
            {
                psi.Arguments += " -nostart";
            }
            psi.WorkingDirectory = installDir.FullName;
            Process updater = Process.Start(psi);
            updater.WaitForExit();
            if (updater.ExitCode != 0)
            {
                installDir.DeleteIgnoreContent();
                e.Result = "It seems the updater crashed ... i'm really sorry about that, please contact me!";
                return;
            }

            string linkName = "SeriesPlayer";
            string filePath = Path.Combine(installDir.FullName, "SeriesPlayer.exe");
            if (installInfo.StartMenuEntry) {
                string startMenuFolder;
                if (installInfo.InstallAllUsers)
                {
                    startMenuFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                }
                else
                {
                    startMenuFolder = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                }

                string linkPath = Path.Combine(startMenuFolder, linkName);
                Util.CreateShortcut(filePath, linkPath);
            }
            if (installInfo.DesktopShortcut)
            {
                string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string linkPath = Path.Combine(desktopFolder, linkName);
                Util.CreateShortcut(filePath, linkPath);
            }
                
            e.Result = "";
        }

        private void backgroundWorkerInstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((string)e.Result != "")
            {
                MessageBox.Show((string)e.Result, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.Exit();
        }

        private void textBoxInstallationDir_TextChanged(object sender, EventArgs e)
        {
            if (textBoxInstallationDir.Text.IsValidRootedFolderPath())
            {
                textBoxInstallationDir.BackColor = Color.White;
            }
            else
            {
                textBoxInstallationDir.BackColor = Color.FromArgb(255, 30, 30);
            }
        }

        private void buttonBrowseForInstallationPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialogInstallationFolder.SelectedPath = new DirectoryInfo(Util.GetDefaultPath()).Parent.FullName;
            DialogResult dr = folderBrowserDialogInstallationFolder.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialogInstallationFolder.SelectedPath))
                {
                    if (folderBrowserDialogInstallationFolder.SelectedPath.EndsWith("\\SeriesPlayer") || folderBrowserDialogInstallationFolder.SelectedPath.EndsWith("\\SeriesPlayer\\"))
                    {
                        textBoxInstallationDir.Text = folderBrowserDialogInstallationFolder.SelectedPath;
                    }
                    else
                    {
                        textBoxInstallationDir.Text = Path.Combine(folderBrowserDialogInstallationFolder.SelectedPath, "SeriesPlayer");
                    }
                    
                }
            }
        }
    }
}
