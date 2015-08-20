﻿using StreamsitePlayer.Forms;
using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Providers;
using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private List<Button> seriesButtons;
        private List<Button> episodeButtons;
        private static Label labelCurrentlyLoadedS;
        private ISitePlayer player = null;

        StreamProvider currentProvider = null;

        public FormMain()
        {
            InitializeComponent();
            labelCurrentlyLoadedS = labelCurrentlyLoaded;
            InitStreamingProviders();
            seriesAnchorX = comboBoxStreamingProvider;
            seriesAnchorY = numericUpDownSkipEnd;
            panelEpisodeButtons.Focus();
            this.comboBoxStreamingProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxStreamingProvider_SelectedIndexChanged);
            LoadSettingValues();
        }

        private void LoadSettingValues()
        {
            checkBoxAutoplay.Checked = Settings.GetBool(Settings.AUTOPLAY);
            numericUpDownSkipEnd.Value = Settings.GetNumber(Settings.SKIP_END);
            numericUpDownSkipStart.Value = Settings.GetNumber(Settings.SKIP_BEGINNING);
            comboBoxStreamingProvider.SelectedIndex = Settings.GetNumber(Settings.LAST_PROVIDER);
            textBoxSeriesExtension.Text = Settings.GetString(Settings.LAST_SERIES);
        }

        private void InitStreamingProviders()
        {
            streamProviders = new List<string>();
            streamProviders.Add(BsToStreamProvider.NAME);
            streamProviders.Add(RyuanimeStreamProvider.NAME);
            streamProviders.Add(DubbedanimehdNetProvider.NAME);
#if DEBUG
            streamProviders.Add(TestProvider.NAME);
#endif
            comboBoxStreamingProvider.Items.Clear();
            comboBoxStreamingProvider.Items.AddRange(streamProviders.ToArray());
            if (comboBoxStreamingProvider.Items.Count != 0)
            {
                comboBoxStreamingProvider.SelectedIndex = 0;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formSettings = new FormSettings(this);
            formSettings.Show();
        }

        private StreamProvider CreateNewProvider(string name)
        {
            switch (name)
            {
                case BsToStreamProvider.NAME:
                    return new BsToStreamProvider();
                case TestProvider.NAME:
                    return new TestProvider();
                case RyuanimeStreamProvider.NAME:
                    return new RyuanimeStreamProvider();
                case DubbedanimehdNetProvider.NAME:
                    return new DubbedanimehdNetProvider();
                default:
                    return null;
            }
        }

        private void comboBoxStreamingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentProvider = CreateNewProvider(comboBoxStreamingProvider.SelectedItem.ToString());
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
                    BuildUIForCurrentProvider();
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
            if (seriesButtons != null) //remove old buttons from the window
            {
                foreach (Control c in seriesButtons)
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
            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 100;
            ClearEpisodePanel();
            seriesButtons = BuildButtonsForSeries(tooltip); //add new buttons to the window
            episodeButtons = BuildButtonsForEpisodes(selectedSeason, tooltip);
            panelEpisodeButtons.Controls.AddRange(seriesButtons.ToArray());
            panelEpisodeButtons.Controls.AddRange(episodeButtons.ToArray());   
            seriesButtons[selectedSeason - 1].Enabled = false;  //disable current series
            Button bottomButton = episodeButtons[episodeButtons.Count - 1];
            int bottomY = panelEpisodeButtons.Bounds.Y + panelEpisodeButtons.Bounds.Height;
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top; //calculate the titlebar height
            this.Height = bottomY + titleHeight;
            HighlightCurrentEpisode();
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
            if (seriesButtons.Count == 0) return buttons;
            int startX = seriesButtons[seriesButtons.Count - 1].Bounds.X;
            int startY = seriesButtons[seriesButtons.Count - 1].Bounds.Y + seriesButtons[seriesButtons.Count - 1].Bounds.Height + PADDING;
            
            List<Episode> episodes = currentProvider.GetEpisodeList(series);

            int episodeCount = episodes.Count;
            int fittingInOneRow = (this.Size.Width - PADDING) / (BUTTON_SIZE + PADDING);
            for (int i = 0; i < episodeCount; i++)
            {
                Button b = CreateNewButton((i + 1).ToString(), episodes[i].Name, tooltip);
                b.Location = new Point(startX + (PADDING + BUTTON_SIZE) * (i % fittingInOneRow), startY + (i / fittingInOneRow) * (PADDING + BUTTON_SIZE));
                b.Click += this.OnEpisodeButtonClicked;
                buttons.Add(b);
            }
            return buttons;
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

        private void HighlightCurrentEpisode()
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
                    selectedSeason = season == 0 ? 1 : season;
                    BuildUIForCurrentProvider();
                    return;
                }

                foreach (Button b in episodeButtons)
                {
                    int buttonEpisode = int.Parse(b.Text);
                    if (buttonEpisode == episode)
                    {
                        b.ForeColor = Color.FromArgb(255, 0, 255);
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

            HighlightCurrentEpisode();
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
            System.Diagnostics.Process.Start(currentProvider.GetWebsiteLink());
        }

        private void PanelFocus_Click(object sender, EventArgs e)
        {
            panelEpisodeButtons.Focus();
        }
    }
}
