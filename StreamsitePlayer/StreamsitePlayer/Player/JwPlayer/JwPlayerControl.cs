using SeriesPlayer.Utility.ChromiumBrowsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.JwPlayer
{
    class JwPlayerControl : OnscreenChromiumBrowser
    {
        private const string JW_SITE_PATH = @"jwplayer\player.html";
        private const string JW_TEMP_SITE_PATH = @"jwplayer\tempPlayer.html";

        public JwPlayerControl(object jsExposedObject) : base(jsExposedObject, "comInterface")
        {
            base.Dock = DockStyle.Fill;
            base.Location = new System.Drawing.Point(0, 0);
            base.Name = "jwPlayer";
            base.GotFocus += JwPlayerControl_GotFocus;
        }

        private void JwPlayerControl_GotFocus(object sender, EventArgs e)
        {
            Focus();
        }

        public readonly float AspectRatio = 16F / 9F;

        public bool IsPlaying
        {
            get
            {
                try
                {
                    return EvaluateJavaScriptForBool("IsPlaying");
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
                    return EvaluateJavaScriptForBool("Maximized");
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
            ExecuteJavaScriptAsync("Pause");
        }

        public void Play()
        {
            ExecuteJavaScriptAsync("Play");
        }

        public void ClickOnFullscreen()
        {
            ExecuteJavaScriptAsync("ClickOnFullscreen");
        }

        public long Position
        {
            get
            {
                try
                {
                    return (long)(EvaluateJavaScriptForDouble("GetPosition") * 1000d);
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
                    ExecuteJavaScriptAsync("SetPosition", Convert.ToString((double)value / 1000d));
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
                    return (long)(EvaluateJavaScriptForDouble("GetDuration") * 1000d);
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
                    return (byte)(EvaluateJavaScriptForInt("GetBuffer"));
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
                    return EvaluateJavaScriptForInt("GetVolume");
                }
                catch
                {
                    return Settings.GetNumber(Settings.VOLUME);
                }
            }
            set
            {
                ExecuteJavaScriptAsync("SetVolume", Convert.ToString(value));
            }
        }

        public bool Muted
        {
            get
            {
                return EvaluateJavaScriptForBool("GetMuted");
            }
            set
            {
                ExecuteJavaScriptAsync("SetMute", Convert.ToString(value));
            }
        }

        public new void Focus()
        {
            if (base.InvokeRequired)
            {
                base.Invoke((MethodInvoker)(() => Focus()));
                return;
            }
            Logger.Log("JwPlayer", "Got focus.");
            
            base.Focus();
            try
            {
                ExecuteJavaScriptAsync("Focus");
            }
            catch { }
        }

        public void Play(string file, string title)
        {
            string html = File.ReadAllText(Util.GetRalativePath(JW_SITE_PATH));
            html = html.Replace("--file--", file)
                .Replace("--title--", title)
                .Replace("--key--", Settings.GetString(Settings.JW_KEY));
            DisplayHtml(html);
            Play();
        }

        public bool IsLoaded
        {
            get
            {
                return GetBrowser().HasDocument && !GetBrowser().IsLoading;
            }
        }

        private void DisplayHtml(string html)
        {
            string tempFile = Util.GetRalativePath(JW_TEMP_SITE_PATH);

            if (File.Exists(tempFile))
            {
                FileInfo fi1 = new FileInfo(tempFile);
                fi1.Attributes &= ~FileAttributes.Hidden;
            }
            File.WriteAllText(tempFile, html);
            FileInfo fi = new FileInfo(tempFile);
            fi.Attributes |= FileAttributes.Hidden;
            string addr = "";
            string[] urlParts = tempFile.Split('\\');
            addr += urlParts[0];
            for (int i = 1; i < urlParts.Length; i++)
            {
                addr += "/" + System.Net.WebUtility.UrlEncode(urlParts[i]);
            }
            base.GetBrowser().MainFrame.LoadUrl(addr);
        }

    }
}
