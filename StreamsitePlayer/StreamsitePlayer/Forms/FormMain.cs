using StreamsitePlayer.Forms;
using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Providers;
using StreamsitePlayer.Utility;
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
        List<string> streamProviders;

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

        StreamProvider currentProvider = null;

        public FormMain()
        {
            Logger.Log("START", "Creating new FormMain instance.");
            InitializeComponent();
            labelCurrentlyLoadedS = labelCurrentlyLoaded;
            InitStreamingProviders();
            seriesAnchorX = comboBoxStreamingProvider;
            seriesAnchorY = numericUpDownSkipEnd;
            panelEpisodeButtons.Focus();
            VersionChecker.VersionChecked += VersionChecker_VersionChecked;
            LoadSettingValues();
        }

        private void LoadSettingValues()
        {
            comboBoxStreamingProvider.SelectedIndexChanged += comboBoxStreamingProvider_SelectedIndexChanged;

            checkBoxAutoplay.Checked = Settings.GetBool(Settings.AUTOPLAY);
            numericUpDownSkipEnd.Value = Settings.GetNumber(Settings.SKIP_END);
            numericUpDownSkipStart.Value = Settings.GetNumber(Settings.SKIP_BEGINNING);
            int selectedProvider = Settings.GetNumber(Settings.LAST_PROVIDER);
            comboBoxStreamingProvider.SelectedIndex = (selectedProvider >= 0 && selectedProvider < comboBoxStreamingProvider.Items.Count) ? selectedProvider : 0;
            textBoxSeriesExtension.Text = Settings.GetString(Settings.LAST_SERIES);

            //Add event listeners after the loaded settings got set to avoid saving of the same settings
            checkBoxAutoplay.CheckedChanged += checkBoxAutoplay_CheckedChanged;
            numericUpDownSkipEnd.ValueChanged += numericUpDownSkipEnd_ValueChanged;
            numericUpDownSkipStart.ValueChanged += numericUpDownSkipStart_ValueChanged;
            textBoxSeriesExtension.TextChanged += textBoxSeriesExtension_TextChanged;
        }

        private void InitStreamingProviders()
        {
            Logger.Log("START", "Adding streaming providers.");
            streamProviders = new List<string>();
            streamProviders.Add(BsToStreamProvider.NAME);
            Logger.Log("START", "Added " + BsToStreamProvider.NAME);
            streamProviders.Add(RyuanimeStreamProvider.NAME);
            Logger.Log("START", "Added " + RyuanimeStreamProvider.NAME);
            streamProviders.Add(DubbedanimehdNetProvider.NAME);
            Logger.Log("START", "Added " + DubbedanimehdNetProvider.NAME);
#if DEBUG
            streamProviders.Add(TestProvider.NAME);
            Logger.Log("START", "Added " + TestProvider.NAME);
#endif
            comboBoxStreamingProvider.Items.Clear();
            Logger.Log("START", "Filling combobox with providers.");
            comboBoxStreamingProvider.Items.AddRange(streamProviders.ToArray());
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings(this);
            formSettings.Show();
        }

        

        private void comboBoxStreamingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentProvider = StreamProvider.Create(comboBoxStreamingProvider.SelectedItem.ToString());
            ReloadStreamingProviderInfo();
        }

        private void ReloadStreamingProviderInfo()
        {
            comboBoxStreamingSites.Items.Clear();
            comboBoxStreamingSites.Items.AddRange(currentProvider.GetValidStreamingSites());
            if (comboBoxStreamingSites.Items.Count != 0) comboBoxStreamingSites.SelectedIndex = 0;

            labelSeriesExtensionHelp.Text = currentProvider.GetLinkInstructions();
        }

        private void buttonOpenSeries_Click(object sender, EventArgs e)
        {
            if (currentProvider != null)
            {
                this.player = null;
                this.Enabled = false;
                string oldName = this.Text;
                this.Text = "Working, please be patient ...";
                int res = currentProvider.LoadSeries(textBoxSeriesExtension.Text, comboBoxStreamingProvider);
                this.Text = oldName;
                this.Enabled = true;
                if (res == StreamProvider.RESULT_OK || res == StreamProvider.RESULT_USE_CACHED)
                {
                    if (currentProvider.GetSeriesCount() != 0) selectedSeason = 1;
                    Settings.WriteValue(Settings.LAST_PROVIDER, comboBoxStreamingProvider.SelectedIndex);
                    Settings.WriteValue(Settings.LAST_SERIES, currentProvider.GetLinkExtension());
                    Settings.SaveFileSettings();
                    BuildUIForCurrentProvider();
                    HighlightCurrentEpisode(true);
                }
            }
            panelEpisodeButtons.Focus();
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
            seasonButtons = BuildButtonsForSeries(tooltip); //add new buttons to the window
            episodeButtons = BuildButtonsForEpisodes(selectedSeason, tooltip);
            panelEpisodeButtons.Controls.AddRange(seasonButtons.ToArray());
            panelEpisodeButtons.Controls.AddRange(episodeButtons.ToArray());   
            seasonButtons[selectedSeason - 1].Enabled = false;  //disable current series

            HighlightCurrentEpisode(false);

            buttonDownload.Enabled = true;
            buttonDownload.Text = "Download";
        }

        private List<Button> BuildButtonsForSeries(ToolTip tooltip)
        {
            int startX = 0;
            int startY = 0;

            List<Button> buttons = new List<Button>();

            int seriesCount = currentProvider.GetSeriesCount();
            int fittingInOneRow = (this.Size.Width - PADDING) / (BUTTON_SIZE + PADDING);
            for (int i = 0; i < seriesCount; i++)
            {
                Button b = CreateNewButton("S" + (i + 1).ToString(), "Series " + (i + 1), tooltip);
                b.Location = new Point(startX + (PADDING + BUTTON_SIZE) * (i % fittingInOneRow), startY + (i / fittingInOneRow) * (PADDING + BUTTON_SIZE));
                b.Click += this.OnSeriesButtonClicked;
                buttons.Add(b);
            }
            return buttons;
        }

        private List<Button> BuildButtonsForEpisodes(int series, ToolTip tooltip)
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
                Button b = CreateNewButton((i + 1).ToString(), episodes[i].Name, tooltip);
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

        private Button CreateNewButton(string text, string tooltipText, ToolTip tip)
        {
            Button b = new Button();
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
            int episode = Settings.GetNumber(Settings.LAST_PLAYED_EPISODE);
            int season = Settings.GetNumber(Settings.LAST_PLAYED_SEASON);
            string series = Settings.GetString(Settings.LAST_PLAYED_SERIES);
            string provider = Settings.GetString(Settings.LAST_PLAYED_PROVIDER);

            if (episode == -1 || season == -1 || series.Equals("") || provider.Equals("")) return;

            if (series == currentProvider.GetLinkExtension() && provider == currentProvider.GetReadableSiteName())
            {
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
                        panelEpisodeButtons.SetAutoScrollMargin(0, BUTTON_SIZE * 2 + PADDING);
                        b.ForeColor = Color.FromArgb(255, 0, 255);
                        if (player == null)
                        {
                            b.Focus();
                        }
                        panelEpisodeButtons.ScrollControlIntoView(b);
                    }
                    else
                    {
                        b.ForeColor = Color.Black;
                    }
                }
            }
            
        }

        private void Player_EpisodeChange(object source, Player.EpisodeChangeEventArgs e)
        {
            Settings.WriteValue(Settings.LAST_PLAYED_EPISODE, e.NewEpisode.Number);
            Settings.WriteValue(Settings.LAST_PLAYED_SEASON, e.NewEpisode.Season);
            Settings.WriteValue(Settings.LAST_PLAYED_PROVIDER, currentProvider.GetReadableSiteName());
            Settings.WriteValue(Settings.LAST_PLAYED_SERIES, currentProvider.GetLinkExtension());
            Settings.SaveFileSettings();

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
            Settings.SaveFileSettings();
        }

        private void numericUpDownSkipStart_ValueChanged(object sender, EventArgs e)
        {
            Settings.WriteValue(Settings.SKIP_BEGINNING, (int)((NumericUpDown)sender).Value);
            Settings.SaveFileSettings();
        }

        private void textBoxSeriesExtension_TextChanged(object sender, EventArgs e)
        {
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
                    Process.Start(Path.Combine(Environment.CurrentDirectory, "Updater.exe"), "-waitforpid=" + Process.GetCurrentProcess().Id + " -ver=" + e.NewVersion);
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
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are currently using version " + Program.VERSION + "!", "Version " + Program.VERSION, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            Form f = new FormDownload(currentProvider);
            f.Show();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            MessageBox.Show("Please fill in whatever stands in the link instead of the question marks. It's USUALLY the series name with hyphens.\nExample:\"http://bs.to/serie/Prison-Break/1\" results in \"Prison-Break\".", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
