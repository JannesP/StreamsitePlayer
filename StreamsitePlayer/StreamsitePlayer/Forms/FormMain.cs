﻿using StreamsitePlayer.Forms;
using StreamsitePlayer.Networking;
using StreamsitePlayer.Networking.Events;
using StreamsitePlayer.Networking.Messages;
using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Providers;
using StreamsitePlayer.Utility;
using StreamsitePlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StreamsitePlayer
{
    public partial class FormMain : Form
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
        private static Label labelCurrentlyLoadedS;
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
        }

        private void LoadSettingValues()
        {
            checkBoxAutoplay.Checked = Settings.GetBool(Settings.AUTOPLAY);
            numericUpDownSkipEnd.Value = Settings.GetNumber(Settings.SKIP_END);
            numericUpDownSkipStart.Value = Settings.GetNumber(Settings.SKIP_BEGINNING);

            LoadCachedSeries();
            if (Settings.GetBool(Settings.REMOTE_CONTROL_ACTIVATED))
            {
                tcpServer = new TcpServer(Settings.GetNumber(Settings.REMOTE_CONTROL_PORT));
                tcpServer.NetworkControl += TcpServer_NetworkControl;
                tcpServer.NetworkRequest += TcpServer_NetworkRequest;
                tcpServer.Start();
            }

            //Add event listeners after the loaded settings got set to avoid saving of the same settings
            checkBoxAutoplay.CheckedChanged += checkBoxAutoplay_CheckedChanged;
            numericUpDownSkipEnd.ValueChanged += numericUpDownSkipEnd_ValueChanged;
            numericUpDownSkipStart.ValueChanged += numericUpDownSkipStart_ValueChanged;
            
            comboBoxChangeSeries.MouseWheel += ComboBoxChangeSeries_MouseWheel;
        }

        private void TcpServer_NetworkRequest(TcpServer source, NetworkRequestEventArgs e)
        {
            switch (e.EventId)
            {
                case NetworkRequestEvent.PlayerStatus:
                    PlayerStatusMessage psm;
                    if (player != null && player.IsLoaded)
                    {
                        //just cut off the position and length because they SHOULD never exceed the maximum int
                        psm = new PlayerStatusMessage(e.MessageId, player.IsPlaying, (int)player.Position, (int)player.Duration, player.BufferPercent);
                    }
                    else
                    {
                        psm = new PlayerStatusMessage(e.MessageId, false, 0, 0, 0);
                    }
                    source.SendToClient(e.Socket, psm);
                    break;
            }
        }

        private void TcpServer_NetworkControl(TcpServer source, NetworkControlEventArgs e)
        {
            switch (e.EventId)
            {
                case NetworkControlEvent.PlayPause:
                    if (player != null && player.IsLoaded)
                    {
                        if (player.IsPlaying)
                        {
                            player.Pause();
                        }
                        else
                        {
                            player.Play();
                        }
                    }
                    break;
                case NetworkControlEvent.Next:
                    if (player != null) player.Next();
                    break;
                case NetworkControlEvent.Previous:
                    if (player != null) player.Previous();
                    break;
                case NetworkControlEvent.PlayEpisode:
                    throw new NotImplementedException();
                    break;
                case NetworkControlEvent.ClosePlayer:
                    if (player != null) player.Close();
                    break;
                case NetworkControlEvent.SeekTo:
                    if (player != null)
                    {
                        int pos = e.Data.ReadInt(0);
                        player.Position = pos;
                    }
                    break;
            }
        }

        private void ComboBoxChangeSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            var seriesSelector = (ComboBox)sender;
            if (seriesSelector.SelectedIndex != -1)
            {
                if (seriesSelector.SelectedIndex == seriesSelector.Items.Count - 1) //add new selected
                {
                    new FormAddNewSeries(this).Show();
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

        public void LoadCachedSeries()
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
                Series s = Seriescache.ReadCachedSeries(provider, seriesExtension);
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
            comboBoxChangeSeries.Items.Clear();
            comboBoxChangeSeries.Items.AddRange(series.ToArray());
            comboBoxChangeSeries.Items.Add("-- add new --");
            comboBoxChangeSeries.SelectedIndexChanged += ComboBoxChangeSeries_SelectedIndexChanged;

            if (lastSeries != null)
            {
                Logger.Log("LOADING", "Selecting last played series: " + lastSeries.LinkExtension);
                comboBoxChangeSeries.SelectedItem = lastSeries;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings(this);
            formSettings.Show();
        }

        public void OpenSeries(Series series)
        {
            currentProvider = StreamProvider.Create(series.Provider);
            currentProvider.LoadSeries(series);
            if (currentProvider.GetSeriesCount() != 0) selectedSeason = 1;
            Settings.WriteValue(Settings.LAST_SERIES, currentProvider.GetLinkExtension());
            Settings.SaveFileSettings();
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

        private void OpenSeries(string linkExtension)
        {
            if (currentProvider != null)
            {
                this.player = null;
                FormLoadingIndicator.ShowDialog(this, "Loading series. This usually shouldn't take any longer then 30 seconds.");
                int res = currentProvider.LoadSeries(linkExtension, comboBoxChangeSeries);
                FormLoadingIndicator.CloseDialog();
                if (res == StreamProvider.RESULT_OK || res == StreamProvider.RESULT_USE_CACHED)
                {
                    if (currentProvider.GetSeriesCount() != 0) selectedSeason = 1;
                    comboBoxChangeSeries.Items.Insert(comboBoxChangeSeries.Items.Count - 1, currentProvider.GetSeries());
                    comboBoxChangeSeries.SelectedItem = currentProvider.GetSeries();
                    Settings.WriteValue(Settings.LAST_SERIES, currentProvider.GetLinkExtension());
                    Settings.SaveFileSettings();
                    BuildUIForCurrentProvider();
                    HighlightCurrentEpisode(true);
                }
            }
            panelEpisodeButtons.Focus();
        }

        private void RefreshSeries(Series series)
        {
            downloadToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Enabled = false;
            seriesToolStripMenuItem.Enabled = false;
            Seriescache.RemoveCachedSeries(series);
            comboBoxChangeSeries.Items.Remove(series);
            OpenSeries(series.LinkExtension);
        }

        private void RemoveSeries(Series series)
        {
            downloadToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Enabled = false;
            seriesToolStripMenuItem.Enabled = false;
            Seriescache.RemoveCachedSeries(series);
            comboBoxChangeSeries.Items.Remove(series);
            currentProvider = null;
            ClearEpisodePanel();
        }

        private static int currentlyLoaded = 0;
        public static void SeriesOpenCallback(Episode episode)
        {
            if (episode == null)
            {
                currentlyLoaded = 0;
                labelCurrentlyLoadedS.Visible = false;
                return;
            }
            if (currentlyLoaded == 0)
            {
                labelCurrentlyLoadedS.Visible = true;
            }
            labelCurrentlyLoadedS.Text = "Loaded " + currentlyLoaded++ + " episodes. Last loaded: S" + episode.Season + "E" + episode.Number + " " + episode.Name;
        }

        private void ClearEpisodePanel()
        {
            if (seasonButtons != null) //remove old buttons from the window
            {
                foreach (Control c in seasonButtons)
                {
                    panelEpisodeButtons.Controls.Remove(c);
                    c.Dispose();
                }
            }
            if (episodeButtons != null)
            {
                foreach (Control c in episodeButtons)
                {
                    panelEpisodeButtons.Controls.Remove(c);
                    c.Dispose();
                }
            }
        }

        private void BuildUIForCurrentProvider()
        {
            Size s = base.Size;
            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 100;
            ClearEpisodePanel();
            seasonButtons = BuildButtonsForSeries(panelEpisodeButtons, tooltip); //add new buttons to the window
            episodeButtons = BuildButtonsForEpisodes(panelEpisodeButtons, selectedSeason, tooltip);
            panelEpisodeButtons.Controls.AddRange(seasonButtons.ToArray());
            panelEpisodeButtons.Controls.AddRange(episodeButtons.ToArray());   
            seasonButtons[selectedSeason - 1].Enabled = false;  //disable current series

            HighlightCurrentEpisode(false);

            downloadToolStripMenuItem.Enabled = true;
            refreshToolStripMenuItem.Enabled = true;
            seriesToolStripMenuItem.Enabled = true;
        }

        private List<Button> BuildButtonsForSeries(Panel parent, ToolTip tooltip)
        {
            int startX = 0;
            int startY = 0;

            List<Button> buttons = new List<Button>();

            int seriesCount = currentProvider.GetSeriesCount();
            int fittingInOneRow = (this.Size.Width - PADDING) / (BUTTON_SIZE + PADDING);
            for (int i = 0; i < seriesCount; i++)
            {
                Button b = CreateNewButton(parent, "S" + (i + 1).ToString(), "Series " + (i + 1), tooltip);
                b.Location = new Point(startX + (PADDING + BUTTON_SIZE) * (i % fittingInOneRow), startY + (i / fittingInOneRow) * (PADDING + BUTTON_SIZE));
                b.Click += this.OnSeriesButtonClicked;
                buttons.Add(b);
            }
            return buttons;
        }

        private List<Button> BuildButtonsForEpisodes(Panel parent, int series, ToolTip tooltip)
        {
            List<Button> buttons = new List<Button>();
            if (seasonButtons.Count == 0) return buttons;
            int startX = seasonButtons[0].Bounds.X;
            int startY = seasonButtons[seasonButtons.Count - 1].Bounds.Y + seasonButtons[seasonButtons.Count - 1].Bounds.Height + PADDING;
            
            List<Episode> episodes = currentProvider.GetEpisodeList(series);

            int episodeCount = episodes.Count;
            int fittingInOneRow = (this.Size.Width - PADDING) / (BUTTON_SIZE + PADDING);
            for (int i = 0; i < episodeCount; i++)
            {
                Button b = CreateNewButton(parent, (i + 1).ToString(), episodes[i].Name, tooltip);
                b.Location = new Point(startX + (PADDING + BUTTON_SIZE) * (i % fittingInOneRow), startY + (i / fittingInOneRow) * (PADDING + BUTTON_SIZE));
                b.GotFocus += Button_GotFocus;
                b.Click += this.OnEpisodeButtonClicked;
                buttons.Add(b);
            }
            return buttons;
        }

        private void Button_GotFocus(object sender, EventArgs e)
        {
            panelEpisodeButtons.Focus();
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
                    panelEpisodeButtons.AutoScroll = true;
                    panelEpisodeButtons.ScrollToYPosition(b.Bounds.Bottom + BUTTON_SIZE + PADDING * 2);
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
            Seriescache.CacheSeries(currentProvider.GetSeries());

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
            panelEpisodeButtons.Focus();
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
                DialogResult dr = MessageBox.Show("Found new version. Do you want to restart and update now?\n\n" + e.Changelog, Program.VERSION + "->" + e.NewVersion, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(Path.Combine(Util.GetAppFolder(), "Updater.exe"), "-waitforpid=" + Process.GetCurrentProcess().Id + " -ver=" + e.NewVersion);
                    Application.Exit();
                    return;
                }
            }
            else if (!e.UpdateRequired && !autoUpdate)
            {
                MessageBox.Show("You got the newest version " + Program.VERSION + " installed :)", "No update found!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
            labelCurrentlyLoadedS = labelCurrentlyLoaded;
            seriesAnchorX = comboBoxChangeSeries;
            seriesAnchorY = numericUpDownSkipEnd;
            panelEpisodeButtons.Focus();
            LoadSettingValues();
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are currently using version " + Program.VERSION + "!", "Version " + Program.VERSION, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            f.Show();
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
    }
}
