using SeriesPlayer.Forms;
using SeriesPlayer.Networking;
using SeriesPlayer.Networking.Events;
using SeriesPlayer.Networking.Messages;
using SeriesPlayer.Streamsites;
using SeriesPlayer.Streamsites.Providers;
using SeriesPlayer.Utility;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer
{
    public partial class FormMain : Form, IUserInformer
    {
        public static List<string> streamProviders;
        public static Control threadTrick;  //very bad workaround for accessing the right thread, but i'm too lazy now and this works for now 

        private Control seriesAnchorX;
        private Control seriesAnchorY;
        private const int PADDING = 5;
        private const int BUTTON_SIZE = 60;
        private int selectedSeason = 1;
        private List<Button> seasonButtons;
        private List<Button> episodeButtons;
        private ISitePlayer player = null;
        private bool autoUpdate = false;
        private TcpServer tcpServer;

        StreamProvider currentProvider = null;

        public FormMain()
        {
            Logger.Log("START", "Creating new FormMain instance.");
            InitializeComponent();
            threadTrick = comboBoxChangeSeries;
            VersionChecker.VersionChecked += VersionChecker_VersionChecked;
            Util.AddUserInformer(this);
            InitCefSharp();
        }

        private void LoadSettingValues()
        {
            checkBoxAutoplay.Checked = Settings.GetBool(Settings.AUTOPLAY);
            numericUpDownSkipEnd.Value = Settings.GetNumber(Settings.SKIP_END);
            numericUpDownSkipStart.Value = Settings.GetNumber(Settings.SKIP_BEGINNING);

            LoadCachedSeriesAsync();
            if (Settings.GetBool(Settings.REMOTE_CONTROL_ACTIVATED))
            {
                CreateTcp();
            }

            //Add event listeners after the loaded settings got set to avoid saving of the same settings
            checkBoxAutoplay.CheckedChanged += checkBoxAutoplay_CheckedChanged;
            numericUpDownSkipEnd.ValueChanged += numericUpDownSkipEnd_ValueChanged;
            numericUpDownSkipStart.ValueChanged += numericUpDownSkipStart_ValueChanged;
            
            comboBoxChangeSeries.MouseWheel += ComboBoxChangeSeries_MouseWheel;
        }

        private void CreateTcp()
        {
            tcpServer = new TcpServer(Settings.GetNumber(Settings.REMOTE_CONTROL_PORT));
            tcpServer.NetworkControl += TcpServer_NetworkControl;
            tcpServer.NetworkRequest += TcpServer_NetworkRequest;
            tcpServer.Start();
        }

        private void TcpServer_NetworkRequest(TcpServer source, NetworkRequestEventArgs e)
        {
            switch (e.EventId)
            {
                case NetworkRequestEvent.PlayerStatus:
                    PlayerStatusMessage psm;
                    if (player != null && !player.IsDisposed && player.IsLoaded)
                    {
                        //just cut off the position and length because they SHOULD never exceed the maximum int
                        psm = new PlayerStatusMessage(e.MessageId, player.IsPlaying, (int)player.Position, (int)player.Duration, player.BufferPercent, (byte)player.Volume);
                    }
                    else
                    {
                        byte volume = (byte)Settings.GetNumber(Settings.VOLUME);
                        psm = new PlayerStatusMessage(e.MessageId, false, 0, 0, 0, volume);
                    }
                    source.SendToClient(e.Socket, psm);
                    break;
                case NetworkRequestEvent.EpisodeList:
                    if (currentProvider != null)
                    {
                        Series s = currentProvider.GetSeries();
                        foreach (List<Episode> season in s.Seasons)
                        {
                            foreach(Episode episode in season)
                            {
                                source.SendToClient(e.Socket, new EpisodeListNetworkMessage(e.MessageId, episode));
                            }
                        }
                        source.SendToClient(e.Socket, new EpisodeListNetworkMessage(e.MessageId, null)); //send last episode marker
                    }
                    break;
            }
        }

        private void TcpServer_NetworkControl(TcpServer source, NetworkControlEventArgs e)
        {
            switch (e.EventId)  //happens always
            {
                case NetworkControlEvent.SkipStart:
                    int newSkipStartValue = e.Data.ReadInt(0);
                    numericUpDownSkipStart.Value = newSkipStartValue;
                    break;
                case NetworkControlEvent.SkipEnd:
                    int newSkipEndValue = e.Data.ReadInt(0);
                    numericUpDownSkipEnd.Value = newSkipEndValue;
                    break;
            }

            if (player != null && !player.IsDisposed && player.IsLoaded)    //player available
            {
                switch (e.EventId)
                {
                    case NetworkControlEvent.PlayPause:
                        if (player.IsPlaying)
                        {
                            player.Pause();
                        }
                        else
                        {
                            player.Play();
                        }
                        break;
                    case NetworkControlEvent.Next:
                        player.Next();
                        break;
                    case NetworkControlEvent.Previous:
                        player.Previous();
                        break;
                    case NetworkControlEvent.PlayEpisode:
                        player.Play(selectedSeason, e.Data.ReadInt(0));
                        break;
                    case NetworkControlEvent.ClosePlayer:
                        player.Close();
                        break;
                    case NetworkControlEvent.SeekTo:
                        int pos = e.Data.ReadInt(0);
                        player.Position = pos;
                        break;
                    case NetworkControlEvent.ToggleFullscreen:
                        player.Maximized = !player.Maximized;
                        break;
                    case NetworkControlEvent.Volume:
                        player.Volume = e.Data[0];
                        break;
                }
            }
            else    //player not available
            {
                switch (e.EventId)
                {
                    case NetworkControlEvent.Volume:
                        Settings.WriteValue(Settings.VOLUME, e.Data[0]);
                        break;
                    case NetworkControlEvent.PlayEpisode:
                        player = new FormJwPlayer();
                        player.Open(currentProvider);
                        player.EpisodeChange += Player_EpisodeChange;
                        player.Play(selectedSeason, e.Data.ReadInt(0));
                        break;
                }
            }
            
        }

        private void ComboBoxChangeSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            var seriesSelector = (ComboBox)sender;
            if (seriesSelector.SelectedIndex != -1)
            {
                if (seriesSelector.SelectedIndex == seriesSelector.Items.Count - 1) //add new selected
                {
                    new FormAddNewSeries(this).ShowParentCentered(this);
                }
                else
                {
                    OpenSeries((Series)seriesSelector.SelectedItem);
                }
            }
            else
            {
                ClearEpisodePanel();
            }
        }

        public async Task LoadCachedSeriesAsync()
        {
            List<string> cachedSeriesFiles = Seriescache.FindCachedSeries();
            string lastPlayedSeries = Settings.GetString(Settings.LAST_SERIES);
            var series = new List<Series>(cachedSeriesFiles.Count);
            Series lastSeries = null;
            for (int i = 0; i < cachedSeriesFiles.Count; i++)
            {
                cachedSeriesFiles[i] = cachedSeriesFiles[i].Replace(".series", "");
                string seriesExtension = cachedSeriesFiles[i].Substring(cachedSeriesFiles[i].LastIndexOf('.') + 1);
                string provider = cachedSeriesFiles[i].Substring(0, cachedSeriesFiles[i].LastIndexOf('.'));
                Series s = await Seriescache.ReadCachedSeriesAsync(provider, seriesExtension);
                if (s != null)
                {
                    if (s.LinkExtension == lastPlayedSeries)
                    {
                        lastSeries = s;
                        Logger.Log("LOADING", "Found last played series: " + s.LinkExtension);
                    }
                    series.Add(s);
                }
            }
            comboBoxChangeSeries.Invoke((MethodInvoker)(() => {
                comboBoxChangeSeries.Items.Clear();
                comboBoxChangeSeries.Items.AddRange(series.ToArray());
                comboBoxChangeSeries.Items.Add("-- add new --");
                comboBoxChangeSeries.SelectedIndexChanged -= ComboBoxChangeSeries_SelectedIndexChanged;
                comboBoxChangeSeries.SelectedIndexChanged += ComboBoxChangeSeries_SelectedIndexChanged;

                if (lastSeries != null)
                {
                    Logger.Log("LOADING", "Selecting last played series: " + lastSeries.LinkExtension);
                    comboBoxChangeSeries.SelectedItem = lastSeries;
                }
            }));
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings(this);
            formSettings.FormClosed += FormSettings_FormClosed;
            formSettings.ShowParentCentered(this);
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool shouldTcpRun = Settings.GetBool(Settings.REMOTE_CONTROL_ACTIVATED);
            int port = Settings.GetNumber(Settings.REMOTE_CONTROL_PORT);
            if (tcpServer != null)
            {
                if (shouldTcpRun)
                {
                    if (!tcpServer.IsRunning || tcpServer.Port != port)
                    {
                        tcpServer.Stop();
                        CreateTcp();
                    }
                }
                else
                {
                    tcpServer.Stop();
                    tcpServer = null;
                }
            }
            else
            {
                if (shouldTcpRun)
                {
                    CreateTcp();
                }
            }
        }

        public void OpenSeries(Series series)
        {
            currentProvider = StreamProvider.Create(series.Provider);
            currentProvider.LoadSeries(series);
            if (currentProvider.GetSeasonCount() != 0) selectedSeason = 1;
            if (player != null)
            {
                player.StreamProvider = currentProvider;
            }
            Settings.WriteValue(Settings.LAST_SERIES, currentProvider.GetLinkExtension());
            Settings.SaveFileSettings();
            if (tcpServer != null)
            {
                tcpServer.BroadcastInfo(InfoNetworkMessage.InfoMessage.SeriesChanged);
            }
            BuildUIForCurrentProvider();
            HighlightCurrentEpisode(true);
        }

        public void SelectSeries(Series series)
        {
            if (series == null)
            {
                comboBoxChangeSeries.SelectedIndex = -1;
                return;
            }
            Series found = null;
            foreach (object o in comboBoxChangeSeries.Items)
            {
                if (series.Equals((Series)o))
                {
                    found = (Series)o;
                    break;
                }
            }
            if (series != null && found != null)
            {
                comboBoxChangeSeries.SelectedItem = found;
            }
        }

        private async void CreateAndSelectSeries(string linkExtension)
        {
            if (currentProvider != null)
            {
                this.player = null;
                FormLoadingIndicator.ShowDialog(this, "Loading series. This usually shouldn't take any longer then 30 seconds.");
                int res = await currentProvider.LoadSeriesAsync(linkExtension, comboBoxChangeSeries);
                FormLoadingIndicator.CloseDialog();
                if (res == StreamProvider.RESULT_OK || res == StreamProvider.RESULT_USE_CACHED)
                {
                    comboBoxChangeSeries.Invoke((MethodInvoker)(() =>
                    {
                        if (currentProvider.GetSeasonCount() != 0) selectedSeason = 1;
                        comboBoxChangeSeries.Items.Insert(comboBoxChangeSeries.Items.Count - 1, currentProvider.GetSeries());
                        comboBoxChangeSeries.SelectedItem = currentProvider.GetSeries();
                        Settings.WriteValue(Settings.LAST_SERIES, currentProvider.GetLinkExtension());
                        Settings.SaveFileSettings();
                        BuildUIForCurrentProvider();
                        HighlightCurrentEpisode(true);
                    }));
                }
            }
            flowPanelEpisodeButtons.Invoke((MethodInvoker)(() =>
            {
                flowPanelEpisodeButtons.Focus();
            }));
        }

        private void RefreshSeries(Series series)
        {
            downloadToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Enabled = false;
            seriesToolStripMenuItem.Enabled = false;
            Seriescache.RemoveCachedSeries(series);
            comboBoxChangeSeries.Items.Remove(series);
            CreateAndSelectSeries(series.LinkExtension);
        }

        private void RemoveSeries(Series series)
        {
            downloadToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Enabled = false;
            seriesToolStripMenuItem.Enabled = false;
            Seriescache.RemoveCachedSeries(series);
            comboBoxChangeSeries.Items.Remove(series);
            if (player != null && !player.IsDisposed)
            {
                player.Close();
                player = null;
            }
            currentProvider = null;
            ClearEpisodePanel();
        }
        
        public static void SeriesOpenCallback(Episode episode)
        {
            if (episode == null)
            {
                return;
            }
        }

        private void ClearEpisodePanel()
        {
            if (seasonButtons != null) //remove old buttons from the window
            {
                foreach (Control c in seasonButtons)
                {
                    flowPanelSeriesButtons.Controls.Remove(c);
                    c.Dispose();
                }
            }
            if (episodeButtons != null)
            {
                foreach (Control c in episodeButtons)
                {
                    flowPanelEpisodeButtons.Controls.Remove(c);
                    c.Dispose();
                }
            }
            buttonOpenHoster.Enabled = false;
        }

        private void BuildUIForCurrentProvider()
        {
            Size s = base.Size;
            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 100;

            WinAPIHelper.SuspendDrawing(this.Handle);
            flowPanelSeriesButtons.SuspendLayout();
            flowPanelEpisodeButtons.SuspendLayout();

            ClearEpisodePanel();

            seasonButtons = BuildButtonsForSeries(flowPanelSeriesButtons, tooltip);
            episodeButtons = BuildButtonsForEpisodes(flowPanelEpisodeButtons, selectedSeason, tooltip);
            
            flowPanelSeriesButtons.Controls.AddRange(seasonButtons.ToArray());
            flowPanelEpisodeButtons.Controls.AddRange(episodeButtons.ToArray());

            seasonButtons[selectedSeason - 1].Enabled = false;  //disable current series

            flowPanelEpisodeButtons.ResumeLayout();
            flowPanelSeriesButtons.ResumeLayout();
            buttonOpenHoster.Enabled = true;
            WinAPIHelper.ResumeDrawing(this.Handle);
            Refresh();  //force redraw
            
            HighlightCurrentEpisode(false);

            downloadToolStripMenuItem.Enabled = true;
            refreshToolStripMenuItem.Enabled = true;
            seriesToolStripMenuItem.Enabled = true;
        }

        private List<Button> BuildButtonsForSeries(Panel parent, ToolTip tooltip)
        {
            List<Button> buttons = new List<Button>();

            int seriesCount = currentProvider.GetSeasonCount();
            for (int i = 0; i < seriesCount; i++)
            {
                Button b = CreateNewButton(parent, "S" + (i + 1).ToString(), "Season " + (i + 1), tooltip);
                b.Height /= 2;
                b.Click += this.OnSeriesButtonClicked;
                buttons.Add(b);
            }
            return buttons;
        }

        private List<Button> BuildButtonsForEpisodes(Panel parent, int series, ToolTip tooltip)
        {
            List<Button> buttons = new List<Button>();
            if (seasonButtons.Count == 0) return buttons;
            
            List<Episode> episodes = currentProvider.GetEpisodeList(series);

            int episodeCount = episodes.Count;
            for (int i = 0; i < episodeCount; i++)
            {
                Button b = CreateNewButton(parent, (i + 1).ToString(), episodes[i].Name, tooltip);
                b.GotFocus += Button_GotFocus;
                b.Click += this.OnEpisodeButtonClicked;
                buttons.Add(b);
            }
            return buttons;
        }

        private void Button_GotFocus(object sender, EventArgs e)
        {
            flowPanelEpisodeButtons.Focus();
        }

        private Button CreateNewButton(Panel parent, string text, string tooltipText, ToolTip tip)
        {
            Button b = new Button();
            b.Parent = parent;
            tip.SetToolTip(b, tooltipText);
            b.Text = text;
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.Visible = true;
            b.Width = BUTTON_SIZE;
            b.Height = BUTTON_SIZE;
            b.Padding = Padding.Empty;
            return b;
        }

        private void OnEpisodeButtonClicked(object sender, EventArgs e)
        {
            int episode = int.Parse(((Button)sender).Text);
            if (player == null || player.IsDisposed)
            {
                player = new FormJwPlayer();
                player.Open(currentProvider);
                player.EpisodeChange += Player_EpisodeChange;
            }
            
            player.Play(selectedSeason, episode);
        }

        private void HighlightCurrentEpisode(bool changeSeason)
        {
            int episode = currentProvider.GetSeries().LastPlayedEpisode;
            int season = currentProvider.GetSeries().LastPlayedSeason;

            season = season == 0 ? 1 : season;  //if season == 0 -> season = 1
            if (season != selectedSeason)
            {
                if (changeSeason)
                {
                    selectedSeason = season == 0 ? 1 : season;
                    BuildUIForCurrentProvider();
                }
                return;
            }

            foreach (Button b in episodeButtons)
            {
                int buttonEpisode = int.Parse(b.Text);
                if (buttonEpisode == episode)
                {
                    flowPanelEpisodeButtons.AutoScroll = true;
                    flowPanelEpisodeButtons.ScrollToYPosition(b.Bounds.Bottom + BUTTON_SIZE + PADDING * 2);
                    b.ForeColor = Color.FromArgb(255, 0, 255);
                    if (player == null)
                    {
                        b.Focus();
                    }
                }
                else
                {
                    b.ForeColor = Color.Black;
                }
            }
            
        }

        private void Player_EpisodeChange(object source, Player.EpisodeChangeEventArgs e)
        {
            currentProvider.GetSeries().LastPlayedEpisode = e.NewEpisode.Number;
            currentProvider.GetSeries().LastPlayedSeason = e.NewEpisode.Season;
            Settings.WriteValue(Settings.LAST_PLAYED_SERIES, currentProvider.GetLinkExtension());
            Settings.SaveFileSettings();
            Seriescache.CacheSeriesAsync(currentProvider.GetSeries());

            HighlightCurrentEpisode(true);
        }

        private void OnSeriesButtonClicked(object sender, EventArgs e)
        {
            selectedSeason = int.Parse(((Button)sender).Text.Replace("S", ""));
            BuildUIForCurrentProvider();
        }

        private void checkBoxAutoplay_CheckedChanged(object sender, EventArgs e)
        {
            Settings.WriteValue(Settings.AUTOPLAY, ((CheckBox)sender).Checked);
            Settings.SaveFileSettings();
        }

        private void numericUpDownSkipEnd_ValueChanged(object sender, EventArgs e)
        {
            Settings.WriteValue(Settings.SKIP_END, (int)((NumericUpDown)sender).Value);
            Settings.SaveFileSettings();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tcpServer != null)
            {
                tcpServer.Stop();
            }
            CefSharp.Cef.Shutdown();
            Settings.SaveFileSettings();
        }

        private void numericUpDownSkipStart_ValueChanged(object sender, EventArgs e)
        {
            Settings.WriteValue(Settings.SKIP_BEGINNING, (int)((NumericUpDown)sender).Value);
            Settings.SaveFileSettings();
        }

        private void buttonOpenProviderSite_Click(object sender, EventArgs e)
        {
            Util.OpenLinkInDefaultBrowser(currentProvider.GetWebsiteLink());
        }

        private void PanelFocus_Click(object sender, EventArgs e)
        {
            flowPanelEpisodeButtons.Focus();
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.OpenLinkInDefaultBrowser("https://github.com/JannesP/StreamsitePlayer");
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkForUpdateToolStripMenuItem.Enabled = false;
            VersionChecker.CheckForUpdateAsync();
        }

        private void VersionChecker_VersionChecked(VersionChecker.VersionCheckedEventArgs e)
        {
            if (e.ErrorOccured)
            {
                MessageBox.Show("Failed to check for version. Details are in the newest log file.", "Failed update check!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.UpdateRequired)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Util.GetAppFolder());
                if (dirInfo.HasWritePermission())
                {
                    DialogResult dr = MessageBox.Show("Found new version. Do you want to restart and update now?\n\n" + e.Changelog, Util.GetCurrentVersion() + "->" + e.NewVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        Process.Start(Path.Combine(Util.GetAppFolder(), "Updater.exe"), "-waitforpid=" + Process.GetCurrentProcess().Id + " -ver=" + e.NewVersion);
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("A new version was found but write access to the program folder is needed, please restart as admin.", Util.GetCurrentVersion() + "->" + e.NewVersion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (!e.UpdateRequired && !autoUpdate)
            {
                MessageBox.Show("You got the newest version " + Util.GetCurrentVersion() + " installed :)", "No update found!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

            autoUpdate = false;
            checkForUpdateToolStripMenuItem.Enabled = true;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
#if !DEBUG
            if (Settings.GetBool(Settings.AUTOCHECK_FOR_UPDATES))
            {
                autoUpdate = true;
                checkForUpdateToolStripMenuItem.Enabled = false;
                VersionChecker.CheckForUpdateAsync();
            }
#endif
            seriesAnchorX = comboBoxChangeSeries;
            seriesAnchorY = numericUpDownSkipEnd;
            flowPanelEpisodeButtons.Focus();
            LoadSettingValues();
        }

        private static void InitCefSharp()
        {
            CefSharp.CefSettings cefSettings = new CefSharp.CefSettings()
            {
                BrowserSubprocessPath = Util.GetRalativePath(@"cef\CefSharp.BrowserSubprocess.exe"),
                CachePath = Util.GetRalativePath(@"cef\cache"),
                ResourcesDirPath = Util.GetRalativePath(@"cef"),
                UserDataPath = Util.GetRalativePath(@"cef\user_data"),
                LocalesDirPath = Util.GetRalativePath(@"cef\locales"),
                LogFile = Util.GetRalativePath(@"cef\debug.log")
            };
            cefSettings.CefCommandLineArgs.Add("disable-gpu", "1");
            cefSettings.CefCommandLineArgs.Add("disable-extensions", "1");
            cefSettings.CefCommandLineArgs.Add("disable-plugins-discovery", "1");
            Version osVersion = Environment.OSVersion.Version;
            if (osVersion.Major == 6)
            {
                switch (osVersion.Minor)
                {
                    case 0: //Vista
                        if (Environment.Is64BitOperatingSystem)
                        {
                            Logger.Log("CefInit", "Running on Vista x64, turning off sandbox.");
                            MessageBox.Show("You are running Windows Vista x64. The player will not run sandboxed.", "SeriesPlayer Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            cefSettings.CefCommandLineArgs.Add("no-sandbox", "1");
                        }
                        break;
                    case 1: //Win 7
                        CefSharp.Cef.EnableHighDPISupport();
                        break;
                    case 2: //Win 8
                    case 3: //Win 8.1
                        //Should work fine
                        break;
                }
            }
            else if (osVersion.Major >= 10) //Win 10+
            {
                //Should work fine
            }
            else if (osVersion.Major <= 5) //Win XP and lower
            {
                MessageBox.Show("You are running Windows XP or lower which is not supported. Please consider upgrading your OS. This application supports Vista and higher.", "SeriesPlayer - Unsupported OS found!", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            if (!CefSharp.Cef.Initialize(cefSettings, true, false))
            {
                throw new Exception("Unable to Initialize Cef!");
            }
            
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are currently using version " + Util.GetCurrentVersion() + "!", "Version " + Util.GetCurrentVersion(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No help available at the moment :P ask me on whatsapp ^^");
        }

        private void ComboBoxChangeSeries_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void ShowHelp()
        {
            MessageBox.Show("Please fill in whatever stands in the link instead of the question marks. It's USUALLY the series name with hyphens.\nExample:\"http://bs.to/serie/Prison-Break/1\" results in \"Prison-Break\".", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonRefreshSeries_Click(object sender, EventArgs e)
        {
            
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new FormDownload(currentProvider);
            f.ShowParentCentered(this);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxChangeSeries.SelectedIndex != -1 && (comboBoxChangeSeries.SelectedIndex != comboBoxChangeSeries.Items.Count - 1))
            {
                DialogResult dr = MessageBox.Show("This will reset your currently played episode. Also if the refresh fails the episode is gone and you have to look for it again!\n Do you still want to refresh?", "Know the risks?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    var currSeries = (Series)comboBoxChangeSeries.SelectedItem;
                    RefreshSeries(currSeries);
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Seriescache.RemoveCachedSeries(currentProvider.GetSeries());
            if (comboBoxChangeSeries.SelectedIndex != -1 && (comboBoxChangeSeries.SelectedIndex != comboBoxChangeSeries.Items.Count - 1))
            {
                DialogResult dr = MessageBox.Show("This will reset your currently played episode and remove the series.\nYour downloaded episodes are not affected!\nStill want to remove it from the list?", "Know the risks?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    var currSeries = (Series)comboBoxChangeSeries.SelectedItem;
                    RemoveSeries(currSeries);
                }
            }
        }

        public void ShowUserMessage(string message)
        {
            if (labelUserInformer.InvokeRequired)
            {
                if (!labelUserInformer.IsDisposed && labelUserInformer.IsHandleCreated)
                {
                    labelUserInformer.Invoke((MethodInvoker)(() => ShowUserMessage(message)));
                }
            }
            else
            {
                labelUserInformer.Text = message;
                labelUserInformer.Visible = true;
            }
        }

        public void HideUserMessage()
        {
            if (labelUserInformer.InvokeRequired)
            {
                if (!labelUserInformer.IsDisposed && labelUserInformer.IsHandleCreated)
                {
                    labelUserInformer.Invoke((MethodInvoker)(() => HideUserMessage()));
                }
            }
            else
            {
                labelUserInformer.Visible = false;
            }
        }

        private void FormMain_Layout(object sender, LayoutEventArgs e)
        {
            ResizeFlowPanelEpisodeButtons();
        }

        private void flowPanelSeriesButtons_SizeChanged(object sender, EventArgs e)
        {
            ResizeFlowPanelEpisodeButtons();
        }
        
        private void ResizeFlowPanelEpisodeButtons()
        {
            flowPanelEpisodeButtons.Location = new Point(flowPanelSeriesButtons.Location.X, flowPanelSeriesButtons.Location.Y + flowPanelSeriesButtons.Size.Height + PADDING);
            flowPanelEpisodeButtons.Size = new Size(flowPanelSeriesButtons.Size.Width, this.ClientSize.Height - flowPanelEpisodeButtons.Location.Y - PADDING * 2);
        }

        private void buttonOpenHoster_Click(object sender, EventArgs e)
        {
            if (currentProvider != null)
            {
                string link = currentProvider.GetSeries().EpisodeOverviewLink;
                if (link != "")
                {
                    Util.OpenLinkInDefaultBrowser(currentProvider.GetSeries().EpisodeOverviewLink);
                    Util.ShowUserInformation("Opened " + currentProvider.GetSeries().EpisodeOverviewLink + " in your default browser.");
                }
                else
                {
                    Util.ShowUserInformation("The link for this series is missing. Please recache to solve the problem.");
                }
                
            }
        }
    }
}
