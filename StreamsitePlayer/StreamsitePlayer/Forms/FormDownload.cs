using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Sites;
using StreamsitePlayer.Utility;
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

namespace StreamsitePlayer.Forms
{
    public partial class FormDownload : Form, IFileCallbackReceiver
    {
        private StreamProvider currentProvider;
        private Dictionary<string, string> downloadList = new Dictionary<string, string>();
        private WebClient webClient;
        private string currentFile;
        private string currentLocalFile;
        private long startTime;
        private long lastBytesReceived;
        private long lastProgressChangedTime;
        private const string DOWNLOADS = @"downloads\";
        private List<Episode> requestedEpisodes = new List<Episode>();
        private bool requesting = false;

        public FormDownload(StreamProvider provider)
        {
            this.currentProvider = provider;
            webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            InitializeComponent();
        }

        private void StartNext()
        {
            if (!webClient.IsBusy)
            {
                if (downloadList.Count != 0)
                {
                    string[] keys = downloadList.Keys.ToArray();
                    string link = keys[0];
                    string fileName = downloadList[keys[0]];
                    string dir = DOWNLOADS + currentProvider.GetSeriesName();
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    this.currentFile = link;
                    labelCurrentEpisode.Text = fileName;

                    fileName = Path.Combine(Environment.CurrentDirectory, dir, fileName);
                    currentLocalFile = fileName + ".part";
                    labelEpisodesLeft.Text = downloadList.Count.ToString();
                    if (File.Exists(fileName))
                    {
                        downloadList.Remove(currentFile);
                        StartNext();
                        return;
                    }
                    if (File.Exists(currentLocalFile))
                    {
                        File.Delete(currentLocalFile);
                    }
                    if (startTime == 0) startTime = DateTime.Now.Ticks;
                    webClient.DownloadFileAsync(new Uri(link), currentLocalFile);
                }
                else
                {
                    startTime = 0;
                }
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            else if (e.Error != null)
            {
                Logger.Log("DOWNLOADER", "Error downloading file: \"" + currentFile + "\"!\n\t" + e.Error.GetType() + ": " + e.Error.Message + "\n" + e.Error.StackTrace);
                AutoClosingMessageBox.Show("Error downloading the current episode. Further detailes are in the log.", "Error downloading episode.", MessageBoxButtons.OK, MessageBoxIcon.Error, 10000);
                StartNext();
                return;
            }

            while (downloadList.Keys.Contains(currentFile))
            {
                downloadList.Remove(currentFile);
            }

            labelEpisodesLeft.Text = downloadList.Count.ToString();
            lastBytesReceived = 0;

            stateProgressBarCurrentFile.Value = 100;
            stateProgressBarCurrentFile.CurrentState = StateProgressBar.State.WARNING;

            try
            {
                File.Move(currentLocalFile, currentLocalFile.Replace(".part", ""));
            }
            catch (Exception ex)
            {
                Logger.Log("DOWNLOAD", "Downloaded file coultn't be renamed.\n\t" + ex.GetType().ToString() + "\n\t" + ex.Message + "\n" + ex.StackTrace);
            }
            StartNext();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            stateProgressBarCurrentFile.Value = e.ProgressPercentage;
            stateProgressBarCurrentFile.CurrentState = StateProgressBar.State.NORMAL;
            long sizeAll = e.TotalBytesToReceive * downloadList.Count;
            double secondsPassed = (double)(DateTime.Now.Ticks - lastProgressChangedTime) / (double)TimeSpan.TicksPerSecond;
            long bytesSinceLastUpdate = e.BytesReceived - lastBytesReceived;
            if (bytesSinceLastUpdate == 0) return;
            lastBytesReceived = e.BytesReceived;
            long bytesPerSecond = (long)(bytesSinceLastUpdate / secondsPassed);
            if (bytesPerSecond > 0)
            {
                labelSpeed.Text = (bytesPerSecond / 1000L).ToString() + "KB/s";
                long timeLeft = sizeAll / bytesPerSecond;
                TimeSpan ts_timeLeft = TimeSpan.FromSeconds(timeLeft);
                labelTimeLeft.Text = CreateTimeString(ts_timeLeft);
            }
            labelTimeRunning.Text = CreateTimeString(TimeSpan.FromTicks(DateTime.Now.Ticks - startTime));

            lastProgressChangedTime = DateTime.Now.Ticks;
        }

        private static string CreateTimeString(TimeSpan ts)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void FormDownload_Load(object sender, EventArgs e)
        {
            FillSeasons();
            
        }

        private void FillSeasons()
        {
            int seasons = currentProvider.GetSeriesCount();
            for (int i = 0; i < seasons; i++)
            {
                listBoxSeason.Items.Add("Season " + (i + 1));
            }
            if (listBoxSeason.Items.Count != 0)
            {
                listBoxSeason.SelectedIndex = 0;
            }
        }

        private void FillEpisodes(int season)
        {
            if (currentProvider.GetEpisodeCount(season) != 0)
            {
                listBoxEpisodes.Items.Clear();
                foreach (Episode e in currentProvider.GetEpisodeList(season))
                {
                    listBoxEpisodes.Items.Add(e);
                }
                if (listBoxEpisodes.Items.Count != 0) listBoxEpisodes.SelectedIndex = 0;
            }
        }

        private void listBoxSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ((ListBox)sender).SelectedIndex;
            FillEpisodes(index + 1);
        }

        private Dictionary<int, Episode> requested = new Dictionary<int, Episode>();

        int requestId = int.MinValue;
        private void buttonDownloadSelected_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection toDownload = listBoxEpisodes.SelectedItems;
            foreach (object o in toDownload)
            {
                RequestEpisode((Episode)o);

            }
        }

        public void FileRequestStatusUpdate(int remainingTime, int max, int rqeuestId)
        {
            return;
        }

        public void ReceiveFileLink(string file, int requestId)
        {
            requesting = false;
            CheckForRequest();
            Logger.Log("DOWNLOADER", "Received filelink: " + file + " for requestId " + requestId);
            Episode e = requested[requestId];
            string season = e.Season == 0 ? "" : "S" + e.Season.ToString();
            string fileName = season + " " + e.Name + GetFileExtension(file);
            requested.Remove(requestId);
            downloadList.Add(file, fileName);
            labelEpisodesLeft.Text = downloadList.Count.ToString();
            StartNext();
        }

        private static string GetFileExtension(string file)
        {
            string[] parts = file.Split('.');
            if (parts.Length != 0)
            {
                string ext = "." + parts[parts.Length - 1];
                ext = ext.Split('?')[0];
                return ext;
            }
            else
            {
                return "";
            }
        }

        private void buttonDownloadSeason_Click(object sender, EventArgs e)
        {
            List<Episode> episodes = currentProvider.GetEpisodeList(listBoxSeason.SelectedIndex + 1);
            foreach (Episode episode in episodes)
            {
                RequestEpisode(episode);
            }

        }

        private void buttonDownloadSeries_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < currentProvider.GetSeriesCount(); i++)
            {
                List<Episode> episodes = currentProvider.GetEpisodeList(i + 1);
                foreach (Episode episode in episodes)
                {
                    RequestEpisode(episode);
                }
            }
        }

        private void RequestEpisode(Episode e)
        {
            requestedEpisodes.Add(e);
            CheckForRequest();
            
        }

        private void CheckForRequest()
        {
            if (!requesting)
            {
                StreamingSite site;
                requesting = true;
                if (requestedEpisodes.Count != 0)
                {
                    Dictionary<string, string> links = requestedEpisodes[0].GetAllAvailableLinks();
                    if (links.Keys.Count != 0)
                    {
                        string siteName = links.Keys.ToArray()[0];
                        site = StreamingSite.CreateStreamingSite(siteName, new WebBrowser(), requestedEpisodes[0].GetLink(siteName));
                    
                        requested.Add(requestId, requestedEpisodes[0]);
                        requestedEpisodes.RemoveAt(0);
                        site.RequestFile(this, requestId++);
                    }

                }
            }
        }

        private void FormDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webClient.IsBusy)
            {
                DialogResult dr = MessageBox.Show("Closing the window will stop all running downloads, are you sure?", "Calcel?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    CancelDownloads();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void ResetUi()
        {
            labelAllProgress.Text = 0.ToString();
            labelCurrentEpisode.Text = "";
            labelEpisodesLeft.Text = "0";
            labelSpeed.Text = "- KB/s";
            labelTimeLeft.Text = "--.--.--";
            labelTimeRunning.Text = "--.--.--";
            stateProgressBarCurrentFile.Value = 100;
        }

        private void CancelDownloads()
        {
            webClient.DownloadFileCompleted -= WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged -= WebClient_DownloadProgressChanged;
            webClient.CancelAsync();
            requested.Clear();
            requestedEpisodes.Clear();
            ResetUi();
            stateProgressBarCurrentFile.CurrentState = StateProgressBar.State.ERROR;
            while (File.Exists(currentLocalFile))
            {
                try
                {
                    File.Delete(currentLocalFile);
                    System.Threading.Thread.Sleep(5);
                }
                catch { }
            }
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("This will stop all running downloads, are you sure?", "Calcel?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                CancelDownloads();
            }
        }
    }
}
