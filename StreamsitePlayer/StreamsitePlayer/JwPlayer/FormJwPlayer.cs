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
using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Sites;
using StreamsitePlayer.JwPlayer;

namespace StreamsitePlayer
{
    public partial class FormJwPlayer : Form, ISitePlayer, IJwCallbackReceiver, ScriptingInterface.IJwEventListener
    {
        private const string JW_SITE_PATH = "jwplayer/player.html";
        StreamProvider streamProvider;
        private int currentEpisode;
        private int currentSeason;
        private int streamcloudWaitTime;
        private bool maximized = false;
        private JwPlayerControl jwPlayer;

        public FormJwPlayer()
        {
            InitializeComponent();

            jwPlayer = new JwPlayerControl();
            ScriptingInterface si = new ScriptingInterface();
            si.SetIJwEventReceiver(this);
            jwPlayer.ObjectForScripting = si;
            jwPlayer.ScriptErrorsSuppressed = true;
            jwPlayer.ScrollBarsEnabled = false;
            
            JSUtil.init(ref jwPlayer);
            base.Controls.Add(jwPlayer);

            this.Size = new Size(964, 576);
        }

        public bool Autoplay
        {
            get
            {
                return Program.settings.GetBool(Settings.AUTOPLAY);
            }

            set
            {
                Program.settings.WriteValue(Settings.AUTOPLAY, value);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return jwPlayer.IsPlaying;
            }
        }


        private FormBorderStyle normalFormBorderStyle;
        private Point oldLocation;
        private Size oldSize;
        public bool Maximized
        {
            get
            {
                return maximized;
            }

            set
            {
                if (value == this.maximized) return;
                if (value)
                {
                    this.maximized = true;
                    this.TopMost = true;

                    this.normalFormBorderStyle = this.FormBorderStyle;
                    this.FormBorderStyle = FormBorderStyle.None;

                    Screen screen = Screen.FromControl(this);
                    this.oldLocation = this.Location;
                    this.Location = screen.Bounds.Location;

                    this.oldSize = this.Size;
                    this.Size = screen.Bounds.Size;
                }
                else
                {
                    this.FormBorderStyle = this.normalFormBorderStyle;
                    this.Location = this.oldLocation;
                    this.Size = this.oldSize;
                    this.TopMost = false;
                    this.Activate();

                    this.maximized = false;
                }
            }
        }

        public int SkipEndSeconds
        {
            get
            {
                return Program.settings.GetNumber(Settings.SKIP_END);
            }

            set
            {
                Program.settings.WriteValue(Settings.SKIP_END, value);
            }
        }

        public int SkipStartSeconds
        {
            get
            {
                return Program.settings.GetNumber(Settings.SKIP_BEGINNING);
            }

            set
            {
                Program.settings.WriteValue(Settings.SKIP_BEGINNING, value);
            }
        }

        public void Next()
        {
            currentEpisode = ++currentEpisode % streamProvider.GetEpisodeCount(currentSeason);
            if (currentEpisode == 1)
            {
                currentSeason = ++currentSeason % streamProvider.GetSeriesCount();
            }
            Play(currentSeason, currentEpisode);
        }

        public void Previous()
        {
            if (currentEpisode == 1)
            {
                if (currentSeason == 1)
                {
                    currentSeason = streamProvider.GetSeriesCount();
                }
                currentEpisode = streamProvider.GetEpisodeCount(currentSeason);
            }
            Play(currentSeason, currentEpisode);
        }

        public void Open(StreamProvider streamProvider)
        {
            this.streamProvider = streamProvider;
            this.Show();
        }

        public void Pause()
        {
            jwPlayer.Pause();
        }

        public void Play()
        {
            jwPlayer.Play();
        }

        private int playNextId = 0;
        private int validRequestId = int.MinValue;
        public void Play(int season, int episode)
        {
            string streamcloudLink = streamProvider.GetEpisodeLink(season, episode, StreamcloudStreamingSite.NAME);
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            StreamcloudStreamingSite streamcloud = new StreamcloudStreamingSite(browser, streamcloudLink);
            streamcloudWaitTime = streamcloud.GetEstimateWaitTime();
            streamcloud.RequestJwData(this, ++validRequestId);
            playNextId = validRequestId;
        }

        private int pointsOnMessage = 1;
        public void JwLinkStatusUpdate(int remainingTime, int max, int rqeuestId)
        {
            bool isPlaying;
            try
            {
                isPlaying = IsPlaying;
            } catch { isPlaying = false; }
            if (!isPlaying)
            {
                jwPlayer.Visible = false;
                progressBarRequestingStatus.Visible = true;
                labelRequestingStatus.Visible = true;
            } else
            {
                jwPlayer.Visible = true;
                progressBarRequestingStatus.Visible = false;
                labelRequestingStatus.Visible = false;
            }
            progressBarRequestingStatus.Maximum = max;
            progressBarRequestingStatus.Value = max - remainingTime;
            labelRequestingStatus.Visible = true;
            string baseMsg = "Processing " + StreamcloudStreamingSite.NAME + " page";
            string pointsString = "";
            for (int i = 0; i < pointsOnMessage; i++)
            {
                pointsString += " .";
            }
            pointsOnMessage = ++pointsOnMessage % 5;
            labelRequestingStatus.Text = baseMsg + pointsString;

        }

        public void ReceiveJwLinks(string filePath, string imagePath, int requestId)
        {
            if (requestId == playNextId)
            {
                jwPlayer.Play(filePath, imagePath);
                jwPlayer.ClickOnFullscreen();
                jwPlayer.Visible = true;
                progressBarRequestingStatus.Visible = false;
                labelRequestingStatus.Visible = false;

            }
        }

        public void OnPlaylocationChanged(long timePlayed, long timeLeft, long timeTotal)
        {
            CheckForAutoplay(timeLeft);
        }

        public void OnPlaybackComplete()
        {
            CheckForAutoplay(0);
        }

        private void CheckForAutoplay(long timeLeft)
        {
            if (Autoplay)
            {
                if (timeLeft <= 0)
                {
                    Console.WriteLine("Next from CheckForAutoplay remainingMillis < 1000");
                    Next();
                }
                else if ((this.SkipEndSeconds != 0) && (timeLeft < ((SkipEndSeconds * 1000) + streamcloudWaitTime)))
                {
                    Console.WriteLine("Next from CheckForAutoplay remainingMillis < (SkipEndSeconds * 1000)");
                    Next();
                }
            }
        }

        public void OnFullscreenChanged(bool newState)
        {
            this.Maximized = newState;
        }

        private void FormJwPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Maximized = false;
                    break;
            }
        }
    }
}
