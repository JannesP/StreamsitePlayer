using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.JwPlayer
{
    class JwPlayerControl : WebBrowser
    {
        private const string JW_SITE_PATH = @"jwplayer\player.html";
        private const string JW_TEMP_SITE_PATH = @"jwplayer\tempPlayer.html";

        public JwPlayerControl()
        {
            base.Dock = DockStyle.Fill;
            base.Location = new System.Drawing.Point(0, 0);
            base.Name = "jwPlayer";
            base.NewWindow += JwPlayerControl_NewWindow;
            base.AllowWebBrowserDrop = false;
            base.IsWebBrowserContextMenuEnabled = false;
            base.ScriptErrorsSuppressed = true;
            base.ScrollBarsEnabled = false;
            base.WebBrowserShortcutsEnabled = false;
            base.GotFocus += JwPlayerControl_GotFocus;
        }

        private void JwPlayerControl_GotFocus(object sender, EventArgs e)
        {
            Focus();
        }

        public readonly float AspectRatio = 16F / 9F;

        private void JwPlayerControl_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;    //disable any popups
        }

        public bool IsPlaying
        {
            get
            {
                try
                {
                    return JSUtil.ExecuteFunctionForBool("IsPlaying");
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Maximized
        {
            get
            {
                try
                {
                    return JSUtil.ExecuteFunctionForBool("Maximized");
                }
                catch
                {
                    return false;
                }
            }

            set
            {
                if (this.Maximized != value) ClickOnFullscreen();
            }
        }

        public void Pause()
        {
            JSUtil.ExecuteFunction("Pause");
        }

        public void Play()
        {
            JSUtil.ExecuteFunction("Play");
        }

        public void ClickOnFullscreen()
        {
            JSUtil.ExecuteFunction("ClickOnFullscreen");
        }

        public long Position
        {
            get
            {
                try
                {
                    return (long)(JSUtil.ExecuteFunctionForDouble("GetPosition") * 1000d);
                }
                catch
                {
                    return 0L;
                }
            }
            set
            {
                try
                {
                    JSUtil.ExecuteFunction("SetPosition", (double)value / 1000d);
                }
                catch { };
            }
        }

        public long Duration
        {
            get
            {
                
                try
                {
                    return (long)(JSUtil.ExecuteFunctionForDouble("GetDuration") * 1000d);
                }
                catch
                {
                    return 0L;
                }
            }
        }

        public byte BufferPercent
        {
            get
            {
                try
                {
                    return (byte)(JSUtil.ExecuteFunctionForInt("GetBuffer"));
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int Volume
        {
            get
            {
                try
                {
                    return JSUtil.ExecuteFunctionForInt("GetVolume");
                }
                catch
                {
                    return Settings.GetNumber(Settings.VOLUME);
                }
            }
            set
            {
                JSUtil.ExecuteFunction("SetVolume", value);
            }
        }

        public bool Muted
        {
            get
            {
                return JSUtil.ExecuteFunctionForBool("GetMuted");
            }
            set
            {
                JSUtil.ExecuteFunction("SetMute", value);
            }
        }

        public new void Focus()
        {
            Logger.Log("JwPlayer", "Got focus.");
            base.Focus();
            try
            {
                JSUtil.ExecuteFunction("Focus");
            }
            catch { }
        }

        public void Play(string insertion)
        {
            string curDir = Directory.GetCurrentDirectory();
            string html = File.ReadAllText(JW_SITE_PATH);
            html = html.Replace("--insertion--", insertion);
            html = html.Replace("--key--", Settings.GetString(Settings.JW_KEY));
            DisplayHtml(html); //blocks until the site is laoded!
            Play();
        }

        private void DisplayHtml(string html)
        {
            if (File.Exists(JW_TEMP_SITE_PATH))
            {
                FileInfo fi1 = new FileInfo(JW_TEMP_SITE_PATH);
                fi1.Attributes &= ~FileAttributes.Hidden;
            }
            File.WriteAllText(JW_TEMP_SITE_PATH, html);
            FileInfo fi = new FileInfo(JW_TEMP_SITE_PATH);
            fi.Attributes |= FileAttributes.Hidden;
            string path = Path.Combine("file:///" + Util.GetAppFolder(), JW_TEMP_SITE_PATH);
            base.Navigate(new Uri(path));
            while (base.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }

    }
}
