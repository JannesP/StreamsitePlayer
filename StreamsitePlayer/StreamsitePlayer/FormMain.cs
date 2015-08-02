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

        private Control seriesAnchor;
        private const int PADDING = 5;
        private const int BUTTON_SIZE = 60;
        private int selectedSeries = 1;
        List<Button> seriesButtons;
        List<Button> episodeButtons;

        StreamProvider currentProvider = null;

        public FormMain()
        {
            InitializeComponent();
            InitStreamProvider();
            seriesAnchor = comboBoxStreamingProvider;
        }

        private void InitStreamProvider()
        {
            streamProviders = new List<string>();
            streamProviders.Add(BsToStreamProvider.NAME);
            streamProviders.Add(TestProvider.NAME);
            comboBoxStreamingProvider.Items.Clear();
            comboBoxStreamingProvider.Items.AddRange(streamProviders.ToArray());
            if (comboBoxStreamingProvider.Items.Count != 0)
            {
                comboBoxStreamingProvider.SelectedIndex = 0;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("coming soon!");
        }

        private StreamProvider CreateNewProvider(string name)
        {
            switch (name)
            {
                case BsToStreamProvider.NAME:
                    return new BsToStreamProvider();
                case TestProvider.NAME:
                    return new TestProvider();
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
                int res = currentProvider.LoadSeries(textBoxSeriesExtension.Text);
                if (res == StreamProvider.RESULT_OK)
                {
                    BuildUIForCurrentProvider();
                }
            }
        }

        private void BuildUIForCurrentProvider()
        {
            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 100;
            if (seriesButtons != null) //remove old buttons from the window
            {
                foreach (Control c in seriesButtons)
                {
                    this.Controls.Remove(c);
                }
            }
            if (episodeButtons != null)
            {
                foreach (Control c in episodeButtons)
                {
                    this.Controls.Remove(c);
                }
            }
            seriesButtons = BuildButtonsForSeries(tooltip); //add new buttons to the window
            episodeButtons = BuildButtonsForEpisodes(selectedSeries, tooltip);
            this.Controls.AddRange(seriesButtons.ToArray());
            this.Controls.AddRange(episodeButtons.ToArray());   
            seriesButtons[selectedSeries - 1].Enabled = false;  //disable current series
            Button bottomButton = episodeButtons[episodeButtons.Count - 1];
            Console.WriteLine(bottomButton.Text);
            int bottomY = bottomButton.Location.Y + bottomButton.Height + PADDING * 3;
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top; //calculate the titlebar height
            this.Height = bottomY + titleHeight;
        }

        private List<Button> BuildButtonsForSeries(ToolTip tooltip)
        {
            int startX = seriesAnchor.Bounds.X;
            int startY = seriesAnchor.Bounds.Y + seriesAnchor.Bounds.Height + PADDING;

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
            int startX = seriesAnchor.Bounds.X;
            int startY = seriesAnchor.Bounds.Y + seriesAnchor.Bounds.Height + PADDING * 2 + BUTTON_SIZE;

            List<Button> buttons = new List<Button>();
            List<Episode> episodes = currentProvider.GetEpisodeList(series);

            int episodeCount = episodes.Count;
            int fittingInOneRow = (this.Size.Width - PADDING) / (BUTTON_SIZE + PADDING);
            Console.WriteLine("fittingInOneRow: " + fittingInOneRow);
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
            Console.WriteLine("EButton: " + ((Button)sender).Text);
        }

        private void OnSeriesButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("SButton: " + ((Button)sender).Text);
            selectedSeries = int.Parse(((Button)sender).Text.Replace("S", ""));
            BuildUIForCurrentProvider();
        }
    }
}
