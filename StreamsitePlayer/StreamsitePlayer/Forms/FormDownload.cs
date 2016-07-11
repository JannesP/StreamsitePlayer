using SeriesPlayer.Streamsites;
using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility;
using SeriesPlayer.Utility.TaskbarProgressBarStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Forms
{
    public partial class FormDownload : Form, IProgress<int>
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

        private CancellationTokenSource _currentCancellationTokenSource = new CancellationTokenSource();
        private CancellationTokenSource CurrentCancellationTokenSource
        {
            get
            {
                return _currentCancellationTokenSource;
            }
            set
            {
                if (_currentCancellationTokenSource != null && !_currentCancellationTokenSource.IsCancellationRequested)
                {
                    _currentCancellationTokenSource.Cancel();
                }
                _currentCancellationTokenSource = value;
            }
        }

        public FormDownload(StreamProvider provider)
        {
            this.currentProvider = provider;
            Logger.Log("DOWNLOADER", "Creating new WebClient.");
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
                    dir = ValidateDirectoryName(ref dir);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    this.currentFile = link;
                    ValidateFileName(ref fileName);
                    labelCurrentEpisode.Text = fileName;

                    fileName = Path.Combine(Util.GetAppFolder(), dir, fileName);
                    currentLocalFile = fileName + ".part";
                    labelEpisodesLeft.Text = downloadList.Count.ToString();
                    if (File.Exists(fileName))
                    {
                        downloadList.Remove(currentFile);
                        ListBoxDownloadingRefresh();
                        StartNext();
                        return;
                    }
                    if (File.Exists(currentLocalFile))
                    {
                        File.Delete(currentLocalFile);
                    }
                    if (startTime == 0) startTime = DateTime.Now.Ticks;
                    Logger.Log("DOWNLOADER", "Downloading the remote file: " + link + " to " + currentLocalFile + ".");
                    webClient.DownloadFileAsync(new Uri(link), currentLocalFile);
                }
                else
                {
                    startTime = 0;
                }
            }
        }

        private void ListBoxDownloadingRefresh()
        {
            if (listBoxDownloadQueue.InvokeRequired)
            {
                listBoxDownloadQueue.Invoke((MethodInvoker)(() => { ListBoxDownloadingRefresh(); }));
                return;
            }
            listBoxDownloadQueue.Items.Clear();

            string[] entries = downloadList.Values.ToArray();
            foreach (string entry in entries)
            {
                listBoxDownloadQueue.Items.Add(entry);
            }
        }

        private string ValidateDirectoryName(ref string nameToCheck)
        {
            char[] invalid = Path.GetInvalidPathChars();
            foreach (char c in invalid)
            {
                nameToCheck = nameToCheck.Replace(c.ToString(), "");
            }
            //TODO: Fix it to work correctly for complete paths with wrong ':'
            if (!nameToCheck.Contains(":\\"))
            {
                nameToCheck = nameToCheck.Replace(":", "_");
            }
            return nameToCheck;
        }

        private string ValidateFileName(ref string nameToCheck)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            foreach (char c in invalid)
            {
                nameToCheck = nameToCheck.Replace(c.ToString(), "");
            }
            return nameToCheck;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            else if (e.Error != null)
            {
                Logger.Log("DOWNLOADER", "Error downloading file: \"" + currentFile + " as " + currentLocalFile + "\"!\n\t" + e.Error.GetType() + ": " + e.Error.Message + "\n" + e.Error.StackTrace);
                AutoClosingMessageBox.Show("Error downloading the current episode. Further detailes are in the log.", "Error downloading episode.", MessageBoxButtons.OK, MessageBoxIcon.Error, 10000);
                StartNext();
                return;
            }

            while (downloadList.Keys.Contains(currentFile))
            {
                downloadList.Remove(currentFile);
            }
            ListBoxDownloadingRefresh();

            labelEpisodesLeft.Text = downloadList.Count.ToString();
            lastBytesReceived = 0;

            stateProgressBarCurrentFile.Value = 100;
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressValue(this.Handle, 0, 100);
                TaskbarManager.Instance.SetProgressState(this.Handle, TaskbarProgressBarState.NoProgress);
            }
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
            stateProgressBarCurrentFile.CurrentState = StateProgressBar.State.NORMAL;
            stateProgressBarCurrentFile.Value = e.ProgressPercentage;
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressState(this.Handle, TaskbarProgressBarState.Normal);
                TaskbarManager.Instance.SetProgressValue(this.Handle, (ulong)Convert.ToUInt32(e.ProgressPercentage), 100UL);
            }
            
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
            int seasons = currentProvider.GetSeasonCount();
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

        public void ReceiveFileLink(string file, Episode requestedEpisode)
        {
            if (labelEpisodesLeft.InvokeRequired)
            {
                labelEpisodesLeft.Invoke((MethodInvoker)(() => ReceiveFileLink(file, requestedEpisode)));
                return;
            }
            requesting = false;
            CheckForRequest();
            if (!requesting)
            {
                stateProgressBarLinkRequest.Visible = false;
                labelLinkRequest.Visible = false;
            }
            if (file == "") return;
            
            Episode e = requestedEpisode;
            string season = e.Season == 0 ? "" : "S" + e.Season.ToString();
            string fileName = season + " E" + e.Number + " - " + e.Name + GetFileExtension(file);
            requested.Remove(requestId);
            downloadList.Add(file, ValidateFileName(ref fileName));
            ListBoxDownloadingRefresh();
            Logger.Log("DOWNLOADER", "Received filelink: " + file + " for requestId " + requestId + " downloading as " + fileName + ".");
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
            for (int i = 0; i < currentProvider.GetSeasonCount(); i++)
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
            Logger.Log("DOWNLOADER", "CheckForRequest called. " + "requesting = " + requesting);

            if (!requesting)
            {
                Logger.Log("DOWNLOADER", "requestedEpisodes.Count = " + requestedEpisodes.Count);
                if (requestedEpisodes.Count != 0)
                {
                    StreamingSite site;
                    requesting = true;
                    Dictionary<string, string> links = requestedEpisodes[0].GetAllAvailableLinks();
                    Logger.Log("DOWNLOADER", "requestedEpisodes[0].GetAllAvailableLinks().Count = " + links.Keys.Count);
                    if (links.Keys.Count != 0)
                    {
                        string siteName = links.Keys.ToArray()[0];
                        if (siteName.Contains("vivo"))
                        {
                            siteName = links.Keys.ToArray()[1];
                        }
                        site = StreamingSite.CreateStreamingSite(siteName, requestedEpisodes[0].GetLink(siteName));
                        Episode currentRequest = requestedEpisodes[0];
                        requestedEpisodes.RemoveAt(0);
                        site.RequestFileAsync(this, CurrentCancellationTokenSource.Token).ContinueWith((fileTask) => {
                            if (!fileTask.IsCanceled && !fileTask.IsFaulted)
                            {
                                ReceiveFileLink(fileTask.Result, currentRequest);
                            }
                        });
                    }

                }
            }
        }

        private void FormDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webClient.IsBusy)
            {
                DialogResult dr = MessageBox.Show("Closing the window will stop all running downloads, are you sure?", "Cancle?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    CancelDownloads();
                    CurrentCancellationTokenSource.Cancel();
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
            TaskbarManager.Instance.SetProgressState(this.Handle, TaskbarProgressBarState.NoProgress);
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
            TaskbarManager.Instance.SetProgressState(this.Handle, TaskbarProgressBarState.NoProgress);
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
            if (webClient.IsBusy || downloadList.Count != 0)
            {
                DialogResult dr = MessageBox.Show("This will stop all running downloads, are you sure?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    CancelDownloads();
                    CurrentCancellationTokenSource.Cancel();
                    CurrentCancellationTokenSource = new CancellationTokenSource();
                }
            }
        }

        private void buttonRemoveSelected_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection selectedIndices = listBoxDownloadQueue.SelectedIndices;
            foreach (int index in selectedIndices)
            {
                RemoveFromDictionary(downloadList, (string)(listBoxDownloadQueue.Items[index]));
            }
            ListBoxDownloadingRefresh();
        }

        private void RemoveFromDictionary(Dictionary<string, string> dictionary, string value)
        {
            foreach (KeyValuePair<string, string> item in dictionary.Where(kvp => kvp.Value == value).ToList())
            {
                dictionary.Remove(item.Key);
            }
        }

        private void listBoxEpisodes_DoubleClick(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection toDownload = listBoxEpisodes.SelectedItems;
            foreach (object o in toDownload)
            {
                RequestEpisode((Episode)o);

            }
        }

        private void buttonBrowseDownloaded_Click(object sender, EventArgs e)
        {
            string downloadDir = Path.Combine(Util.GetAppFolder(), DOWNLOADS);
            if (!Directory.Exists(downloadDir))
            {
                Directory.CreateDirectory(downloadDir);
            }
            Process.Start(downloadDir);
        }

        public void Report(int value)
        {
            stateProgressBarLinkRequest.CurrentState = StateProgressBar.State.WARNING;
            stateProgressBarLinkRequest.Value = value;
            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressState(this.Handle, TaskbarProgressBarState.Paused);
                TaskbarManager.Instance.SetProgressValue(this.Handle, (ulong)Convert.ToUInt32(value), (ulong)Convert.ToUInt32(stateProgressBarLinkRequest.Maximum));
            }

            if (value == 0)
            {
                stateProgressBarLinkRequest.CurrentState = StateProgressBar.State.NORMAL;
            }

            stateProgressBarLinkRequest.Visible = true;
            labelLinkRequest.Visible = true;
        }
    }
}
