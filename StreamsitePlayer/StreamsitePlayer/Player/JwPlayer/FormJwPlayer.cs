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
using StreamsitePlayer.Utility;
using StreamsitePlayer.Forms;

namespace StreamsitePlayer
{
    public partial class FormJwPlayer : Form, ISitePlayer, IJwCallbackReceiver, ScriptingInterface.IJwEventListener, IUserInformer
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

            this.GotFocus += FormJwPlayer_GotFocus;

            this.Size = new Size(964, 576);
            WinAPIHelper.PreventIdle();
        }

        private void FormJwPlayer_GotFocus(object sender, EventArgs e)
        {
            jwPlayer.Focus();
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
                if (value)  //is it should maximized
                {
                    Logger.Log("JW_MAXIMIZING", "Values BEFORE fullscreen:");
                    LogScreenData();

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

                    Logger.Log("JW_MAXIMIZING", "Values AFTER fullscreen:");
                    LogScreenData();
                }
                else    //if it should restore
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

        private void LogScreenData()
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Logger.Log("JW_SCREEN_INFO", "Screen: " + screen.DeviceName + "\tWidth: " + screen.Bounds.Width + "\tHeight: " + screen.Bounds.Height + "\tX: " + screen.Bounds.X + "\tY: " + screen.Bounds.Y + "\tPrimary: " + screen.Primary);
            }
            Logger.Log("JW_SCREEN_INFO", "Window: \tWidth: " + base.Width + "\tHeight: " + base.Height + "\tX: " + base.Location.X + "\tY: " + base.Location.Y);
            Screen s = Screen.FromHandle(base.Handle);
            Logger.Log("JW_SCREEN_INFO", "Active screen: " + s.DeviceName + "\tWidth: " + s.Bounds.Width + "\tHeight: " + s.Bounds.Height + "\tX: " + s.Bounds.X + "\tY: " + s.Bounds.Y + "\tPrimary: " + s.Primary);
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

        public StreamProvider StreamProvider
        {
            get
            {
                return streamProvider;
            }

            set
            {
                streamProvider = value;
                currentEpisode = -1;
                currentSeason = -1;
            }
        }

        public void Next()
        {
            nextRequested = true;
            if (currentEpisode != -1)
            {
                if (++currentEpisode > streamProvider.GetEpisodeCount(currentSeason))
                {
                    if (++currentSeason > streamProvider.GetSeriesCount())
                    {
                        return;
                    }
                    currentEpisode = 1;
                }
            }
            else
            {
                currentEpisode = 1;
                currentSeason = 1;
            }
            Play(currentSeason, currentEpisode);
        }

        public void Previous()
        {
            if (currentEpisode != -1)
            {
                if (currentEpisode == 1)
                {
                    if (currentSeason == 1)
                    {
                        currentSeason = streamProvider.GetSeriesCount();
                    }
                    currentEpisode = streamProvider.GetEpisodeCount(currentSeason);
                }
                
            }
            else
            {
                currentEpisode = 1;
                currentSeason = 1;
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
        private WebBrowser requestBrowser;
        public void Play(int season, int episode)
        {
            currentSeason = season;
            currentEpisode = episode;
            string episodeLink = "";
            int usedProvider = 0;
            for (int i = 0; i < streamProvider.GetValidStreamingSites().Length; i++)
            {
                episodeLink = streamProvider.GetEpisodeLink(season, episode, streamProvider.GetValidStreamingSites()[i]);
                if (episodeLink != "")
                {
                    usedProvider = i;
                    break;
                }
            }

            if (episodeLink != "")
            {
                if (requestBrowser != null)
                {
                    base.Controls.Remove(requestBrowser);
                    requestBrowser.Dispose();
                    requestBrowser = null;
                }
                requestBrowser = Util.CreatePopuplessBrowser();
#if DEBUG
                requestBrowser.Visible = true;
                requestBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
                requestBrowser.Size = base.ClientSize;
                requestBrowser.Location = new Point(0, 0);
                base.Controls.Add(requestBrowser);
#endif
                StreamingSite site = StreamingSite.CreateStreamingSite(streamProvider.GetValidStreamingSites()[usedProvider], requestBrowser, episodeLink);
                streamcloudWaitTime = site.GetEstimateWaitTime();
                site.RequestJwData(this, ++validRequestId);
                playNextId = validRequestId;
                progressBarLoadingNext.Style = ProgressBarStyle.Marquee;
                progressBarRequestingStatus.Style = ProgressBarStyle.Marquee;
                Util.ShowUserInformation("Playing next: " + streamProvider.GetEpisode(season, episode).Number + " - " + streamProvider.GetEpisode(season, episode).Name);
                OnEpisodeChange(new EpisodeChangeEventArgs(streamProvider.GetEpisode(season, episode)));
            }
            else
            {
                Util.ShowUserInformation("Didn't find any links for the episode, jumping to the next one.");
                Next();
            }
            
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
                if (remainingTime == max || remainingTime == 0)
                {
                    progressBarLoadingNext.Style = ProgressBarStyle.Marquee;
                    progressBarRequestingStatus.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    progressBarRequestingStatus.Style = ProgressBarStyle.Continuous;
                    progressBarLoadingNext.Style = ProgressBarStyle.Continuous;
                }
                
                progressBarRequestingStatus.Maximum = max;
                progressBarRequestingStatus.Value = max - remainingTime;
                progressBarLoadingNext.Maximum = max;
                progressBarLoadingNext.Value = max - remainingTime;
                string baseMsg = "Processing " + streamProvider.GetValidStreamingSites()[0] + " page";
                string baseTitle = "Currently loading Season " + currentSeason + " Episode " + currentEpisode;
                string pointsString = "";
                for (int i = 0; i < pointsOnMessage; i++)
                {
                    pointsString += " .";
                }
                pointsOnMessage = ++pointsOnMessage % 5;
                labelRequestingStatus.Text = baseMsg + pointsString;
                base.Text = baseTitle + pointsString;
            }
        }

        public void ReceiveJwLinks(string insertion, int requestId)
        {
            if (requestId == playNextId)
            {
                if (insertion == "")
                {
                    Logger.Log("JwLink", "Got no file link");
                    Util.ShowUserInformation("Couldn't load episode because the hoster didn't respond.");
                    progressBarRequestingStatus.Value = 0;
                }
                else
                {
                    Logger.Log("JwLink", "Received link for playNextId " + playNextId + "with the insertion " + insertion);
#if DEBUG
                    base.Controls.Remove(requestBrowser);
#endif
                    if (!requestBrowser.IsDisposed) requestBrowser.Dispose();
                    nextFullscreen = jwPlayer.Maximized;
                    jwPlayer.Play(insertion);
                    jwPlayer.Visible = true;
                    jwPlayer.Focus();

                    progressBarRequestingStatus.Visible = false;
                    labelRequestingStatus.Visible = false;
                    progressBarLoadingNext.Visible = false;
                    this.Text = streamProvider.GetEpisodeName(currentSeason, currentEpisode) + " - " + streamProvider.GetSeriesName();
                    nextRequested = false;

                    UpdateLabelEpisode();
                }
            }
        }

        System.Threading.Timer timerLabelEpisodeHiding;
        private const long LABEL_EPISODE_DISPLAY_TIME = 7000;
        private void UpdateLabelEpisode()
        {
            if (labelEpisode.InvokeRequired)
            {
                labelEpisode.Invoke((MethodInvoker)(() =>
                {
                    labelEpisode.Visible = true;
                    labelEpisode.Text = "Episode " + currentEpisode;
                }));
            }
            else
            {
                labelEpisode.Visible = true;
                labelEpisode.Text = "Episode " + currentEpisode;
            }

            Logger.Log("LABEL_EPISODE_HIDING", "Timer set for HideLabelEpisode() to " + LABEL_EPISODE_DISPLAY_TIME + "ms");
            timerLabelEpisodeHiding = new System.Threading.Timer((state) => HideLabelEpisode(), null, LABEL_EPISODE_DISPLAY_TIME, -1);
        }

        private void HideLabelEpisode()
        {
            Logger.Log("LABEL_EPISODE_HIDING", "HideLabelEpisode() called.");
            if (labelEpisode.InvokeRequired)
            {
                Logger.Log("LABEL_EPISODE_HIDING", "labelEpisode.InvokeRequired true");
                labelEpisode.Invoke((MethodInvoker)(() =>
                {
                    Logger.Log("LABEL_EPISODE_HIDING", "invoke successful");
                    if (!labelEpisode.IsDisposed)
                    {
                        Logger.Log("LABEL_EPISODE_HIDING", "labelEpisode.IsDisposed false, hiding label");
                        labelEpisode.Visible = false;
                        labelEpisode.Text = "Episode X";
                    }
                }));
            }
            else
            {
                Logger.Log("LABEL_EPISODE_HIDING", "labelEpisode.InvokeRequired false");
                if (!labelEpisode.IsDisposed)
                {
                    Logger.Log("LABEL_EPISODE_HIDING", "labelEpisode.IsDisposed false, hiding label");
                    labelEpisode.Visible = false;
                    labelEpisode.Text = "Episode X";
                }
            }
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
            jwPlayer.Focus();
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
            if (!this.maximized)
            {
                int widthChange = Math.Abs(this.ClientSize.Width - oldClientSize.Width);
                int heightChange = Math.Abs(this.ClientSize.Height - oldClientSize.Height);

                this.ClientSize = new Size((int)((float)this.ClientSize.Height * jwPlayer.AspectRatio), this.ClientSize.Height);

                oldClientSize = this.ClientSize;
            }
        }

        private void FormJwPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            WinAPIHelper.AllowIdle();
            Util.RemoveUserInformer(this);
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
                if (IsPlaying)
                {
                    labelUserInformer.BackColor = Color.Black;
                    labelUserInformer.ForeColor = Color.White;
                }
                else
                {
                    labelUserInformer.BackColor = Color.FromKnownColor(KnownColor.Control);
                    labelUserInformer.ForeColor = Color.Black;
                }

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

        private void FormJwPlayer_Shown(object sender, EventArgs e)
        {
            Util.AddUserInformer(this);
        }
    }
}
