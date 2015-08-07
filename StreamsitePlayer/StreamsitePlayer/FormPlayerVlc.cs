using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StreamsitePlayer.Streamsites;
using Vlc.DotNet.Forms;
using StreamsitePlayer.Streamsites.Sites;
using System.Threading;
using Vlc.DotNet.Core;

namespace StreamsitePlayer
{
    public partial class FormPlayerVlc : Form, ISitePlayer, IVlcCallbackReceiver
    {
        private const string LIBVLC_PATH = "vlclib/";
        private static VlcControl vlc;
        StreamProvider streamProvider;
        private int currentEpisode;
        private int currentSeason;
        private bool maximized = false;

        private FormBorderStyle normalFormBorderStyle;
        private Point oldLocation;
        private Size oldSize;

        public FormPlayerVlc()
        {
            InitializeComponent();
            this.Size = new Size(964, 576);
        }

        private void InitVlc()
        {
            if (vlc != null)
            {
                this.Controls.Remove(vlc);
            }
            Panel vlcMousePanel = new Panel();
            vlc = new VlcControl();
            vlc.VlcLibDirectory = new System.IO.DirectoryInfo(LIBVLC_PATH);
            vlc.Dock = DockStyle.Fill;
            vlc.Playing += Vlc_Playing;
            vlc.Paused += Vlc_Paused;
            vlc.PositionChanged += Vlc_PositionChanged;
            vlc.LengthChanged += Vlc_LengthChanged;
            vlc.EndReached += Vlc_EndReached;
            
            vlcMousePanel.Dock = DockStyle.Fill;
            vlcMousePanel.BackColor = Color.Transparent;
            vlcMousePanel.MouseMove += new MouseEventHandler(FormPlayerVlc_MouseMove);
            vlcMousePanel.MouseDoubleClick += VlcMousePanel_MouseDoubleClick;
            vlc.Controls.Add(vlcMousePanel);

            this.Controls.Add(vlc);
            this.Controls.SetChildIndex(vlc, 10);
        }

        private void Vlc_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            if (buttonPlay.InvokeRequired)
            {
                buttonPlay.Invoke((MethodInvoker)(() => Vlc_EndReached(sender, e)));
                return;
            }
            else
            {
                buttonPlay.Text = "Play";
                if (Program.settings.GetBool(Settings.AUTOPLAY))
                {
                    //Next();
                    Console.WriteLine("Next from Vlc_EndReched");
                }
            }
        }

        private void VlcMousePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Maximized = !this.Maximized;
        }

        private void Vlc_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            if (!labelTimeMax.IsDisposed && labelTimeMax.InvokeRequired)
            {
                labelTimeMax.Invoke((MethodInvoker)(() => Vlc_LengthChanged(sender, e)));
                return;
            }
            else
            {
                VlcControl senderVlc = (VlcControl)sender;
                TimeSpan length = TimeSpan.FromMilliseconds(senderVlc.Length);
                labelTimeMax.Text = String.Format("{0:D2}:{1:D2}", length.Minutes + length.Hours * 60, length.Seconds);
            }
        }

        private void Vlc_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            if (!trackBarTime.IsDisposed && trackBarTime.InvokeRequired)
            {
                trackBarTime.Invoke((MethodInvoker)(() => Vlc_PositionChanged(sender, e)));
            }
            else
            {
                VlcControl senderVlc = (VlcControl)sender;
                float position = senderVlc.Position;
                int trackBarPosition = (int)((double)position * (double)trackBarTime.Maximum);
                trackBarTime.Value = trackBarPosition;

                long positionMillis = (long)((double)senderVlc.Length * (double)position);
                TimeSpan length = TimeSpan.FromMilliseconds(positionMillis);
                labelTimePlayed.Text = String.Format("{0:D2}:{1:D2}", length.Minutes + length.Hours * 60, length.Seconds);

                CheckForAutoplay(senderVlc);

                long ticksWithoutMouseMove = DateTime.Now.Ticks - lastMouseMove;
                this.ControlsVisible = (ticksWithoutMouseMove < (TimeSpan.TicksPerSecond * 5L)); //when not moved for 5 seconds hide controls
            }
        }

        private void CheckForAutoplay(VlcControl vlcControl)
        {
            this.Autoplay = Program.settings.GetBool(Settings.AUTOPLAY);
            long remainingMillis = vlcControl.Length - (long)((double)vlcControl.Length * (double)vlcControl.Position);
            if (Autoplay && vlcControl.IsPlaying)
            {
                if (remainingMillis < 8000)
                {
                    Console.WriteLine("Next from CheckForAutoplay remainingMillis < 1000");
                    Next();
                }
                else if ((this.SkipEndSeconds != 0) && (remainingMillis < (SkipEndSeconds * 1000)))
                {
                    Console.WriteLine("Next from CheckForAutoplay remainingMillis < (SkipEndSeconds * 1000) " + Thread.CurrentThread.ManagedThreadId);
                    Next();
                }
            }
        }

        private void Vlc_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            if (buttonPlay.InvokeRequired)
            {
                buttonPlay.Invoke((MethodInvoker)(() => Vlc_Paused(sender, e)));
                return;
            }
            else
            {
                buttonPlay.Text = "Play";
                this.ControlsVisible = true;
            }
        }

        private void Vlc_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            if (buttonPlay.InvokeRequired)
            {
                buttonPlay.Invoke((MethodInvoker)(() => Vlc_Playing(sender, e)));
                return;
            }
            else
            {
                buttonPlay.Text = "Pause";
            }
        }

        #region properties
        public bool ControlsVisible
        {
            get
            {
                return buttonPlay.Visible;
            }

            set
            {
                buttonPlay.Visible = value;
                buttonPrevious.Visible = value;
                buttonNext.Visible = value;
                buttonFullscreen.Visible = value;
                labelTimePlayed.Visible = value;
                labelTimeMax.Visible = value;
                trackBarTime.Visible = value;
                CursorShown = value;
            }
        }

        private bool cursorShown = true;
        public bool CursorShown
        {
            get
            {
                return cursorShown;
            }
            set
            {
                if (value == cursorShown)
                {
                    return;
                }

                if (value)
                {
                    Cursor.Show();
                }
                else
                {
                    Cursor.Hide();
                }

                cursorShown = value;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return vlc.IsPlaying;
            }
        }

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

        private bool autoplay = false;
        public bool Autoplay
        {
            get
            {
                return autoplay;
            }

            set
            {
                autoplay = value;
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
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        private void FormVlcPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Keycode " + e.KeyCode + " pressed!");
            switch (e.KeyCode)
            {
                case Keys.Space:
                    if (vlc != null)
                    {
                        if (vlc.IsPlaying)
                        {
                            vlc.Pause();
                        }
                        else
                        {
                            vlc.Play();
                        }
                    }
                    break;
                case Keys.Escape:
                    this.Maximized = false;
                    break;
            }

            if (e.KeyCode == Keys.Space)
            {
                
            }
        }

        public void Open(StreamProvider streamProvider)
        {
            this.streamProvider = streamProvider;
            //InitVlc();
            this.Show();
        }

        public void PlayPause()
        {
            if (vlc != null)
            {
                if (vlc.IsPlaying)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }

        public void Play()
        {
            if (vlc != null && !vlc.IsPlaying)
            {
                vlc.Play();
            }
        }

        private int validRequestId = int.MinValue;
        public void Play(int season, int episode)
        {
            Console.WriteLine("Playrequest for S" + season + "E" + episode);
            string streamcloudLink = streamProvider.GetEpisodeLink(season, episode, StreamcloudStreamingSite.NAME);
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            StreamcloudStreamingSite streamcloud = new StreamcloudStreamingSite(browser, streamcloudLink);
            streamcloud.RequestVlcLink(this, ++validRequestId);
            currentEpisode = episode;
            currentSeason = season;
            if (vlc != null)
            {
                try
                {
                    vlc.Position = 1f;
                }
                catch { }
            }
            InitVlc();
        }

        public void Pause()
        {
            if (vlc != null)
            {
                vlc.Pause();
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

        public void VlcLinkStatusUpdate(int remainingTime, int max, int requestId)
        {
            if (requestId == validRequestId)
            {
                progressBarRequestingStatus.Visible = true;
                progressBarRequestingStatus.Maximum = max;
                progressBarRequestingStatus.Value = max - remainingTime;
                labelRequestingStatus.Visible = true;
                string baseMsg = "Processing " + StreamcloudStreamingSite.NAME + " page";
                int points = ((baseMsg.Length + 1) % 4) + 1;
                string pointsString = "";
                for (int i = 0; i < points; i++)
                {
                    pointsString += " .";
                }
                labelRequestingStatus.Text = baseMsg + pointsString;

                Console.WriteLine(requestId + ": " + remainingTime + "/" + max);
            }
        }

        public void ReceiveVlcLink(string link, int requestId)
        {
            if (requestId == validRequestId)
            {
                vlc.Play(new Uri(link), "-tcp-caching=1000", "-udp-caching=1000", "-realrtsp-caching=1000");
                progressBarRequestingStatus.Visible = false;
                labelRequestingStatus.Visible = false;
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (vlc.IsPlaying)
            {
                ((Button)sender).Text = "Play";
            }
            else
            {
                ((Button)sender).Text = "Pause";
            }
            PlayPause();
        }

        private void FormPlayerVlc_FormClosing(object sender, FormClosingEventArgs e)
        {
            vlc.Playing -= Vlc_Playing;
            vlc.Paused -= Vlc_Paused;
            vlc.PositionChanged -= Vlc_PositionChanged;
            vlc.LengthChanged -= Vlc_LengthChanged;

            vlc.Position = 1f;  //use isntead of Stop() because it doesn't throw an exception.
            this.Controls.Remove(vlc);  //disable disposing of our only vlc instance.
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            Previous();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void buttonFullscreen_Click(object sender, EventArgs e)
        {
            this.Maximized = !this.Maximized;
        }

        private void FormPlayerVlc_Deactivate(object sender, EventArgs e)
        {
            this.Maximized = false;
        }

        private long lastMouseMove = 0;
        private Point lastMousePos = new Point(0, 0);
        private void FormPlayerVlc_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lastMousePos.Equals(e.Location))
            {
                lastMouseMove = DateTime.Now.Ticks;
                this.ControlsVisible = true;
                lastMousePos = e.Location;
            }
        }

        private void trackBarTime_ValueChanged(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                vlc.Position = (float)((double)trackBarTime.Value / (double)int.MaxValue);
            }
        }

        private void FormPlayerVlc_MouseLeave(object sender, EventArgs e)
        {
            this.CursorShown = true;
        }
    }
}
