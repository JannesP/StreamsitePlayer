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
using StreamsitePlayer.Player;

namespace StreamsitePlayer
{
    public partial class FormJwPlayer : Form, ISitePlayer, IJwCallbackReceiver, ScriptingInterface.IJwEventListener
    {
        public event OnEpisodeChangeHandler EpisodeChange;

        private const string JW_SITE_PATH = "jwplayer/player.html";
        StreamProvider streamProvider;
        private int currentEpisode;
        private int currentSeason;
        private int streamcloudWaitTime;
        private bool maximized = false;
        private JwPlayerControl jwPlayer;
        private bool nextRequested = false;
        private long lastPosition = 0;
        

        public FormJwPlayer()
        {
            InitializeComponent();
            oldClientSize = this.ClientSize;
            this.Resize += FormJwPlayer_Resize;
            jwPlayer = new JwPlayerControl();
            ScriptingInterface si = new ScriptingInterface();
            si.SetIJwEventReceiver(this);
            jwPlayer.ObjectForScripting = si;
            jwPlayer.ScriptErrorsSuppressed = true;
            jwPlayer.ScrollBarsEnabled = false;
            
            JSUtil.Init(ref jwPlayer);
            base.Controls.Add(jwPlayer);
            base.Controls.SetChildIndex(jwPlayer, 10);

            this.Size = new Size(964, 576);
        }

        protected virtual void OnEpisodeChange(EpisodeChangeEventArgs e)
        {
            if (EpisodeChange != null)
            {
                EpisodeChange(this, e);
            }
        }

        public bool Autoplay
        {
            get
            {
                return Settings.GetBool(Settings.AUTOPLAY);
            }

            set
            {
                Settings.WriteValue(Settings.AUTOPLAY, value);
                Settings.SaveFileSettings();
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
        private Rectangle oldBounds;
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
                    this.Resize -= FormJwPlayer_Resize;
                    this.maximized = true;
                    this.TopMost = true;

                    this.oldBounds = this.DesktopBounds;
                    this.normalFormBorderStyle = this.FormBorderStyle;

                    this.FormBorderStyle = FormBorderStyle.None;
                    Screen screen = Screen.FromControl(this);

                    this.Location = screen.Bounds.Location;
                    this.Size = screen.Bounds.Size;

                    this.Activate();
                    jwPlayer.Focus();
                    this.Resize += FormJwPlayer_Resize;
                }
                else
                {
                    this.Resize -= FormJwPlayer_Resize;
                    this.FormBorderStyle = this.normalFormBorderStyle;
                    this.DesktopBounds = oldBounds;

                    this.TopMost = false;
                    this.Activate();
                    jwPlayer.Focus();
                    this.maximized = false;
                    this.Resize += FormJwPlayer_Resize;
                }
            }
        }

        public int SkipEndSeconds
        {
            get
            {
                return Settings.GetNumber(Settings.SKIP_END);
            }

            set
            {
                Settings.WriteValue(Settings.SKIP_END, value);
                Settings.SaveFileSettings();
            }
        }

        public int SkipStartSeconds
        {
            get
            {
                return Settings.GetNumber(Settings.SKIP_BEGINNING);
            }

            set
            {
                Settings.WriteValue(Settings.SKIP_BEGINNING, value);
                Settings.SaveFileSettings();
            }
        }

        public void Next()
        {
            nextRequested = true;
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
        private long nextPlayTime = 0;
        public void Play(int season, int episode)
        {
            currentSeason = season;
            currentEpisode = episode;
            string episodeLink = streamProvider.GetEpisodeLink(season, episode, streamProvider.GetValidStreamingSites()[0]);
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            StreamingSite site = StreamingSite.CreateStreamingSite(streamProvider.GetValidStreamingSites()[0], browser, episodeLink);
            streamcloudWaitTime = site.GetEstimateWaitTime();
            site.RequestJwData(this, ++validRequestId);
            playNextId = validRequestId;
            OnEpisodeChange(new EpisodeChangeEventArgs(streamProvider.GetEpisode(season, episode)));
        }

        public void RestartStream()
        {
            nextPlayTime = lastPosition;
            Play(currentSeason, currentEpisode);
        }

        private int pointsOnMessage = 1;
        public void JwLinkStatusUpdate(int remainingTime, int max, int requestId)
        {
            if (requestId == playNextId)
            {
                bool isPlaying;
                try
                {
                    isPlaying = IsPlaying;
                }
                catch { isPlaying = false; }
                if (!isPlaying)
                {
                    jwPlayer.Visible = false;
                    progressBarRequestingStatus.Visible = true;
                    labelRequestingStatus.Visible = true;
                    progressBarLoadingNext.Visible = false;
                }
                else
                {
                    jwPlayer.Visible = true;
                    progressBarRequestingStatus.Visible = false;
                    labelRequestingStatus.Visible = false;
                    progressBarLoadingNext.Visible = true;
                }
                progressBarRequestingStatus.Maximum = max;
                progressBarRequestingStatus.Value = max - remainingTime;
                progressBarLoadingNext.Maximum = max;
                progressBarLoadingNext.Value = max - remainingTime;
                string baseMsg = "Processing " + streamProvider.GetValidStreamingSites()[0] + " page";
                string pointsString = "";
                for (int i = 0; i < pointsOnMessage; i++)
                {
                    pointsString += " .";
                }
                pointsOnMessage = ++pointsOnMessage % 5;
                labelRequestingStatus.Text = baseMsg + pointsString;
            }
        }

        public void ReceiveJwLinks(string insertion, int requestId)
        {
            if (requestId == playNextId)
            {
                Logger.Log("JwLink", "Received link for playNextId " + playNextId + "with the insertion " + insertion);
                nextFullscreen = jwPlayer.Maximized;
                jwPlayer.Play(insertion);
                jwPlayer.Visible = true;
                
                progressBarRequestingStatus.Visible = false;
                labelRequestingStatus.Visible = false;
                progressBarLoadingNext.Visible = false;
                this.Text = streamProvider.GetEpisodeName(currentSeason, currentEpisode) + " - " + streamProvider.GetSeriesName();
                nextRequested = false;

                UpdateLabelEpisode();
            }
        }

        private const long LABEL_EPISODE_DISPLAY_TIME = 7;
        private const long LABEL_EPISODE_FADING_TIME = 3;
        private long displayTimeLabelEpisode = 0;
        private void UpdateLabelEpisode()
        {
            if (displayTimeLabelEpisode == 0L) displayTimeLabelEpisode = DateTime.Now.Ticks;

            if ((DateTime.Now.Ticks - displayTimeLabelEpisode) > TimeSpan.TicksPerSecond * LABEL_EPISODE_DISPLAY_TIME)
            {
                displayTimeLabelEpisode = 0L;
                if (labelEpisode.InvokeRequired)
                {
                    labelEpisode.Invoke((MethodInvoker)(() => labelEpisode.Visible = false));
                }
                if (labelEpisode.InvokeRequired)
                {
                    labelEpisode.Invoke((MethodInvoker)(() => labelEpisode.Text = "Episode X"));
                }
                return;
            }

            Color c = labelEpisode.ForeColor;
            long timeDisplayed = (DateTime.Now.Ticks - displayTimeLabelEpisode);
            long timeFullDisplay = TimeSpan.TicksPerSecond * (LABEL_EPISODE_DISPLAY_TIME - LABEL_EPISODE_FADING_TIME);
            long timeFading = timeDisplayed - timeFullDisplay;
            long timeTotalFading = TimeSpan.TicksPerSecond * LABEL_EPISODE_FADING_TIME;

            /*if (timeFading > 0L)
            {
                int newAlpha = (int)(((double)(timeTotalFading - timeFading) / (double)timeTotalFading) * 255d);
                if (newAlpha > 255) newAlpha = 255;
                c = Color.FromArgb(newAlpha, c);
                Console.WriteLine("A: " + newAlpha);
                Console.WriteLine("Time left: " + (timeTotalFading - timeFading));
            }
            else
            {
                c = Color.FromArgb(255, c);
            }*/ //TODO implement a transparent label. This is currently too much effort for a little effect.

            if (labelEpisode.InvokeRequired)
            {
                labelEpisode.Invoke((MethodInvoker)(() => labelEpisode.ForeColor = c));
            }
            if (labelEpisode.InvokeRequired)
            {
                labelEpisode.Invoke((MethodInvoker)(() => labelEpisode.Visible = true));
            }
            if (labelEpisode.InvokeRequired)
            {
                labelEpisode.Invoke((MethodInvoker)(() => labelEpisode.Text = "Episode " + currentEpisode));
            }

            System.Threading.Timer t = new System.Threading.Timer((state) => UpdateLabelEpisode(), null, 490, -1);
        }

        public void OnPlaylocationChanged(long timePlayed, long timeLeft, long timeTotal)
        {
            lastPosition = timePlayed;
            CheckForAutoplay(timeLeft);
        }

        bool nextFullscreen = false;
        public void OnPlaybackComplete()
        {
            nextFullscreen = Maximized;
            CheckForAutoplay(0);
        }

        private void CheckForAutoplay(long timeLeft)
        {
            if (Autoplay)
            {
                if (timeLeft <= 1000) jwPlayer.Pause();
                if (!nextRequested)
                {
                    if (timeLeft <= (1000 + streamcloudWaitTime))
                    {
                        Next();
                    }
                    else if ((this.SkipEndSeconds != 0) && (timeLeft < ((SkipEndSeconds * 1000) + streamcloudWaitTime)))
                    {
                        Next();
                    }
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

        public void OnError(string message)
        {
            Logger.Log("JwPlayerJS", "OnError: " + message);
        }

        public void OnStartupError(string message)
        {
            Logger.Log("JwPlayerJS", "OnStartupError: " + message);
        }

        public void OnReady()
        {
            Logger.Log("JwPlayerOnReady", "Event fired at:\n\tPosition: " + jwPlayer.Position + "\n\tLength: " + jwPlayer.Length);
            CheckForLateStart();
            jwPlayer.Volume = Settings.GetNumber(Settings.VOLUME);
            jwPlayer.Muted = Settings.GetBool(Settings.MUTED);
            jwPlayer.Maximized = nextFullscreen;
        }

        private void CheckForLateStart()
        {
            int skipSeconds = Settings.GetNumber(Settings.SKIP_BEGINNING);
            if (skipSeconds != 0)
            {
                if (jwPlayer.Length > (skipSeconds * 1000))
                {
                    jwPlayer.Position = skipSeconds * 1000;
                }
            }
        }

        public void OnVolumeChange(int newVolume)
        {
            Settings.WriteValue(Settings.VOLUME, newVolume);
        }

        public void OnMuteChange(bool muted)
        {
            Settings.WriteValue(Settings.MUTED, muted);
        }

        private Size oldClientSize;
        private void FormJwPlayer_Resize(object sender, EventArgs e)
        {
            int widthChange = Math.Abs(this.ClientSize.Width - oldClientSize.Width);
            int heightChange = Math.Abs(this.ClientSize.Height - oldClientSize.Height);
            
            this.ClientSize = new Size((int)((float)this.ClientSize.Height * jwPlayer.AspectRatio), this.ClientSize.Height);

            oldClientSize = this.ClientSize;
        }
    }
}
