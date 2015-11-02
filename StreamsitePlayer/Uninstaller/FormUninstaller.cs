using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstaller
{
    public partial class FormUninstaller : Form
    {
        private bool removeEpisodes;
        private bool closing;
        private int step = 0;

        public FormUninstaller()
        {
            InitializeComponent();
            this.Controls.Remove(panelFinished);
            this.Controls.Remove(panelUninstalling);
        }

        private void checkBoxRemoveEpisodes_CheckedChanged(object sender, EventArgs e)
        {
            removeEpisodes = checkBoxRemoveEpisodes.Checked;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            switch (step)
            {
                case 0:
                    buttonCancel.Enabled = false;
                    buttonNext.Enabled = false;
                    this.Controls.Add(panelUninstalling);
                    panelUninstalling.Location = new Point(12, 12);
                    this.Controls.Remove(panelSettings);
                    step++;
                    backgroundWorkerUninstall.RunWorkerAsync();
                    break;
                case 1:
                    
                    break;
                case 2:
                    closing = true;
                    Application.Exit();
                    break;
            }
            
            
        }

        private void FormUninstaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closing || e.CloseReason != CloseReason.UserClosing)
            {
                if (backgroundWorkerUninstall.IsBusy) backgroundWorkerUninstall.CancelAsync();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorkerUninstall_DoWork(object sender, DoWorkEventArgs e)
        {
            List<FileInfo> filesToDelete = Util.GetUninstallFiles(removeEpisodes);
            backgroundWorkerUninstall.ReportProgress(0);

            for (int i = 0; i < filesToDelete.Count; i++)
            {
                if (backgroundWorkerUninstall.CancellationPending) return;

                FileInfo fi = filesToDelete[i];
                if (fi.Exists) fi.Delete();

                backgroundWorkerUninstall.ReportProgress((int)(((double)i / (double)filesToDelete.Count) * 100D));
            }

            List<DirectoryInfo> dirsToRemove = Util.GetEmptyUninstallDirectories(removeEpisodes);
            foreach (DirectoryInfo dir in dirsToRemove)
            {
                if (backgroundWorkerUninstall.CancellationPending) return;
                if (dir.Exists) dir.DeleteWithSubFolders();
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Program.path);
            if (dirInfo.IsDirectoryEmpty()) dirInfo.DeleteWithSubFolders();
        }

        private void backgroundWorkerUninstall_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarProgress.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerUninstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            step++;
            buttonNext.Enabled = true;
            buttonNext.Text = "Close";
            this.Controls.Add(panelFinished);
            panelFinished.Location = new Point(12, 12);
            this.Controls.Remove(panelUninstalling);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }
    }
}
