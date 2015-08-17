using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Sites
{
    class StreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "Streamcloud";
        public const int WAIT_TIME_UNKNOWN = 123456;

        public StreamcloudStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link) { targetBrowser.Navigate(link); }

        public override string GetFileName()
        {
            if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete)
            {
                return "";
            }
            HtmlElementCollection elements = GetTargetBrowser().Document.GetElementsByTagName("h1");
            foreach (HtmlElement element in elements)
            {
                if (element.InnerHtml.Contains("Watch video: "))
                {
                    return element.InnerHtml.Replace("Watch video: ", "");
                }
            }
            return "ERROR: TITLE NOT FOUND!";
        }

        private int GetSecondsFromString(string s)
        {
            int x = 0;
            string[] parts = s.Split(':');
            x += int.Parse(parts[0]) * 60;
            x += int.Parse(parts[1]);
            return x;
        }

        public override int GetRemainingPlayTime()
        {
            HtmlElement durationLabel = GetTargetBrowser().Document.GetElementById("mediaplayer_controlbar_duration");
            HtmlElement playedLabel = GetTargetBrowser().Document.GetElementById("mediaplayer_controlbar_elapsed");
            int duration = GetSecondsFromString(durationLabel.InnerHtml);
            int played = GetSecondsFromString(playedLabel.InnerHtml);
            return duration - played;
        }

        private long startedWaiting = 0; 
        public override int GetRemainingWaitTime()
        {
            if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete)
            {
                return GetEstimateWaitTime();
            } else
            {
                if (startedWaiting == 0)
                {
                    startedWaiting = DateTime.Now.Ticks;
                }
            }
            HtmlElement countdown = GetTargetBrowser().Document.GetElementById("countdown");
            if (countdown == null)
            {
                startedWaiting = 0;
                return 0;
            }
            //string cutNum = countdown.GetAttribute("value").Replace("Waiting time ", "");
            //cutNum = cutNum.Replace(" seconds", "");
            //cutNum = cutNum.Replace("Wartezeit: Noch ", "");
            //cutNum = cutNum.Replace(" Sekunden", "");
            long ticks = DateTime.Now.Ticks - startedWaiting;
            long millis = ticks / TimeSpan.TicksPerMillisecond;
            int remainingMillis = GetEstimateWaitTime() - (int)millis;
            return remainingMillis < 0 ? 0 : remainingMillis;
        }

        public override string GetSiteName()
        {
            return NAME;
        }

        public override bool IsReadyToPlay()
        {
            HtmlElement playButton = GetTargetBrowser().Document.GetElementById("mediaplayer_display_button");
            if (playButton == null)
            {
                return false;
            }
            if (playButton.Style.Contains("opacity: 1"))
            {
                return true;
            }
            return false;
        }

        public override bool Pause()
        {
            HtmlElement mediaController = GetTargetBrowser().Document.GetElementById("mediaplayer_controlbar");
            if (mediaController == null)
            {
                return false;
            }
            HtmlElementCollection mediaControls = mediaController.GetElementsByTagName("button");
            if (mediaControls.Count > 0)
            {
                mediaControls[0].InvokeMember("Click");
                return true;
            }
            return false;

        }

        public override bool Play()
        {
            HtmlElement playButton = GetTargetBrowser().Document.GetElementById("mediaplayer_display_button");
            if (playButton != null)
            {
                playButton.InvokeMember("Click");
                return true;
            }
            return false;
        }

        private bool ContinueWhenReady()
        {
            if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete) return false;
            HtmlElement watchButton = GetTargetBrowser().Document.GetElementById("btn_download");
            if (watchButton == null) return false;
            Console.WriteLine("OuterOfButton: " + watchButton.OuterHtml);
            if (watchButton.OuterHtml.Contains(" blue"))
            {
                Console.WriteLine("Clicking");
                watchButton.InvokeMember("Click");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool continued = false;
        public override void PlayWhenReady()
        {
            if (!continued)
            {
                continued = ContinueWhenReady();
                Console.WriteLine("Continued: " + continued);
                System.Threading.Timer t = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => PlayWhenReady())); }, null, 500, -1);
            }
            else
            {
                if (IsReadyToPlay())
                {
                    Play();
                }
                else
                {
                    System.Threading.Timer t = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => PlayWhenReady())); }, null, 500, -1);
                }
            }
            
        }

        public override void Maximize()
        {
            throw new NotImplementedException();
        }

        public override int GetEstimateWaitTime()
        {
            return 11000;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            if (!continued)
            {
                continued = ContinueWhenReady();
                receiver.JwLinkStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);
                Console.WriteLine("Continued: " + continued);
                System.Threading.Timer t = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestJwData(receiver, requestId))); }, null, 500, -1);
            }
            else
            {
                if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete)
                {
                    receiver.JwLinkStatusUpdate(0, 10000, requestId);
                    System.Threading.Timer t = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestJwData(receiver, requestId))); }, null, 500, -1);
                }
                else
                {
                    string htmlText = GetTargetBrowser().DocumentText;
                    string file = Util.GetStringBetween(htmlText, 0, "file: \"", "\"");
                    string image = Util.GetStringBetween(htmlText, 0, "image: \"", "\"");
                    string insertion = "file:\"" + file + "\",";   //file:"http://.../",
                    insertion += "\nimage:\"" + image + "\"";   //image:"http://.../"
                    receiver.ReceiveJwLinks(insertion, requestId);
                }
            }
        }
    }
}
