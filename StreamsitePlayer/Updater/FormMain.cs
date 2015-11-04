using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Diagnostics;

namespace Updater
{
    public partial class FormMain : Form
    {
        public static string REMOTE_FILE_PATH = "http://62.75.142.161/streamplayer/newest.zip"; //has to be changed
        public const string CACHE_DIR = "updatecache";
        public const string CACHE_FILE = CACHE_DIR + @"\cache.zip";
        public const string DECOMPRESS_DIR = CACHE_DIR + @"\decompressed";
        public const string EXECUTABLE = "SeriesPlayer.exe";
        public static string VERSION = "";

        private string cacheDir;
        private string cacheFile;
        private string decompressDir;

        private long lastUpdate = 0L;
        private long bytesAtLastUpdate = 0L;

        private static bool canceled = false;

        public FormMain()
        {
            cacheDir = Path.Combine(Environment.CurrentDirectory, CACHE_DIR);
            cacheFile = Path.Combine(Environment.CurrentDirectory, CACHE_FILE);
            decompressDir = Path.Combine(Environment.CurrentDirectory, DECOMPRESS_DIR);
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CleanCacheDir();
            if (canceled) Cancel();
            DownloadNewestZip();
        }

        private void Cancel()
        {
            buttonCancel.Enabled = false;
            Logger.Log("CANCEL", "Patch cancelled. Removing temporary files!");
            CleanCacheDir();
            MessageBox.Show("Failed to update. For detailed information open the latest log file.\nIf you think this is a bug, please report on github or personally.", "Error, aborted update!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        private void CleanCacheDir()
        {
            if (Directory.Exists(cacheDir))    //remove cache dir ... should never be nececcary but whatever
            {
                Logger.Log("CLEANUP", "Found old cache directory. Removing with all necessary subdirectories and files!");
                File.Delete(cacheFile);
                if (Directory.Exists(decompressDir)) Directory.Delete(decompressDir, true);
            }
            else
            {
                Logger.Log("CLEANUP", "Creating cache dir.");
                Directory.CreateDirectory(cacheDir);
                DirectoryInfo di = new DirectoryInfo(cacheDir);
                di.Attributes = di.Attributes | FileAttributes.Hidden;
            }
        }

        private void DownloadNewestZip()
        {
            Logger.Log("WEBCLIENT", "Creating WebClient for file download ...");
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                Logger.Log("WEBCLIENT", "Starting asynchronous download of the file.");
                lastUpdate = DateTime.Now.Ticks;
                bytesAtLastUpdate = 0L;
                webClient.DownloadFileAsync(new Uri(REMOTE_FILE_PATH), cacheFile);
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient)sender).DownloadFileCompleted -= WebClient_DownloadFileCompleted;
            ((WebClient)sender).DownloadProgressChanged -= WebClient_DownloadProgressChanged;

            if (e.Error != null)
            {
                Logger.Log("WEBCLIENT", "An error occured while downloading the file.\n\t" + e.Error.Message + "\n\t" + e.Error.GetType() + "\n" + e.Error.StackTrace);
                Cancel();
                return;
            }

            Logger.Log("WEBCLIENT", "Finished file download.");
            DecompressZip();
            CopyNewFiles();
            if (!canceled)
            {
                Logger.Log("SUCCESS", "The program was patched without problems.");
                CleanCacheDir();
                RegEditor.CreateUninstaller(Environment.CurrentDirectory);
                if (Program.startAfterUpdate)
                {
                    Logger.Log("STARTING", "Starting the new version of the main program!");
                    Process.Start(Path.Combine(Environment.CurrentDirectory, EXECUTABLE), "-nopatch");
                }
                Application.Exit();
            }
        }

        private void CopyNewFiles()
        {
            buttonCancel.Enabled = false;
            try
            {
                DirectoryCopy(decompressDir, Environment.CurrentDirectory);
            }
            catch (IOException ex)
            {
                Logger.Log("DIRCOPY", ex.GetType() + "\n\t" + ex.StackTrace);
                Cancel();
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                Logger.Log("DIRCOPY", "Copying file \"" + Path.Combine(sourceDirName, file.Name) + "\" to \"" + temppath + "\" ...");
                file.CopyTo(temppath, true);
                Logger.Log("DIRCOPY", "\"" + Path.Combine(sourceDirName, file.Name) + "\" successfully copied to \"" + temppath + "\" ...");
            }
            
            foreach (DirectoryInfo subdir in dirs)
            {
                Logger.Log("DIRCOPY", "Found subdir: " + subdir.Name + " copying ...");
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
                Logger.Log("DIRCOPY", "Subdir \"" + subdir.Name + "\" finished!");
            }
            
        }

        private void DecompressZip()
        {
            if (!Directory.Exists(decompressDir))
            {
                Logger.Log("ZIP", "Creating decompression directory.");
                Directory.CreateDirectory(decompressDir);
            }
            long startTime = DateTime.Now.Ticks;
            Logger.Log("ZIP", "Decompressing ...");
            ZipFile.ExtractToDirectory(cacheFile, decompressDir);
            Logger.Log("ZIP", "Finished decompression after " + (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond + "ms");
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (canceled)
            {
                ((WebClient)sender).CancelAsync();
                Cancel();
            }
            labelDownloaded.Text = (e.BytesReceived / 1000L) + "/" + (e.TotalBytesToReceive / 1000L) + " KB";
            progressBarDownload.Maximum = 100;
            progressBarDownload.Value = e.ProgressPercentage;

            double secondsPassed = (double)(DateTime.Now.Ticks - lastUpdate) / (double)TimeSpan.TicksPerSecond;
            double bytesSinceLastUpdate = (e.BytesReceived - bytesAtLastUpdate) / 1000D;
            labelSpeed.Text = ((long)(bytesSinceLastUpdate / secondsPassed)).ToString() + "KB/s";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            canceled = true;
            buttonCancel.Enabled = false;
        }
    }
}
