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
            base.IsBrowserInitializedChanged += JwPlayerControl_IsBrowserInitializedChanged;
        }

        public readonly float AspectRatio = 16F / 9F;

        public async Task<bool> GetIsPlayingAsync()
        {
            try
            {
                return await EvaluateJavaScriptForBool("IsPlaying");
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GetIsMaximizedAsync()
        {
            try
            {
                return await EvaluateJavaScriptForBool("Maximized");
            }
            catch
            {
                return false;
            }
        }

        public async void SetMaximizedAsync(bool newState)
        {
            if (await GetIsMaximizedAsync() != newState) ClickOnFullscreen();
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

        public async Task<long> GetPositionAsync()
        {
            try
            {
                return (long)(await EvaluateJavaScriptForDouble("GetPosition") * 1000d);
            }
            catch
            {
                return 0L;
            }
        }

        public void SetPosition(long position)
        {
            ExecuteJavaScriptAsync("SetPosition", Convert.ToString((double)position / 1000d));
        }

        public async Task<long> GetDurationAsync()
        {
            try
            {
                return (long)(await EvaluateJavaScriptForDouble("GetDuration") * 1000d);
            }
            catch
            {
                return 0L;
            }
        }

        public async Task<byte> GetBufferPercentAsync()
        {
            try
            {
                return (byte)(await EvaluateJavaScriptForInt("GetBuffer"));
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> GetVolumeAsync()
        {
            try
            {
                return await EvaluateJavaScriptForInt("GetVolume");
            }
            catch
            {
                return Settings.GetNumber(Settings.VOLUME);
            }
        }

        public void SetVolume(int volume)
        {
            ExecuteJavaScriptAsync("SetVolume", Convert.ToString(volume));
        }

        public async Task<bool> GetMutedAsync()
        {
            return await EvaluateJavaScriptForBool("GetMuted");
        }

        public void SetMute(bool mute)
        {
            ExecuteJavaScriptAsync("SetMute", Convert.ToString(mute).ToLower());
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

        string fileToPlay = null, titleToPlay = null;
        public void Play(string file, string title)
        {
            if (!base.IsBrowserInitialized)
            {
                fileToPlay = file;
                titleToPlay = title;
                return;
            }
            string html = File.ReadAllText(Util.GetRalativePath(JW_SITE_PATH));
            html = html.Replace("--file--", file)
                .Replace("--title--", title)
                .Replace("--key--", Settings.GetString(Settings.JW_KEY));
            DisplayHtml(html);
            Play();
        }

        private void JwPlayerControl_IsBrowserInitializedChanged(object sender, CefSharp.IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized)
            {
                if (fileToPlay != null && titleToPlay != null)
                {
                    //TODO Find better solution to wait for the about:blank page to 'load' since it prevents my Load in DisplayHtml.
                    Task.Delay(500).ContinueWith(t =>
                    {
                        Play(fileToPlay, titleToPlay);
                        titleToPlay = fileToPlay = null;
                    });
                }
            }
        }

        public bool IsLoaded
        {
            get
            {
                return IsPageLoaded;
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
            addr += "file:///" + urlParts[0];
            for (int i = 1; i < urlParts.Length; i++)
            {
                addr += "/" + System.Net.WebUtility.UrlEncode(urlParts[i]);
            }
            base.Load(addr);
        }

    }
}
