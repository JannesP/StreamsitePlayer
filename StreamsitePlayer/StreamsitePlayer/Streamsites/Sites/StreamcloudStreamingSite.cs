using StreamsitePlayer.Utility.Extensions;
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
        public Uri trueLink;

        private bool finalSiteLoaded;

        public StreamcloudStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            targetBrowser.DocumentCompleted += TargetBrowser_DocumentCompleted;
            trueLink = new Uri(link);
            targetBrowser.Navigate(trueLink);
        }

        private void TargetBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Logger.Log("STREAMCLOUD_NAVIGATION", e.Url.ToString());
            if (e.Url == trueLink && continued)
            {
                finalSiteLoaded = true;
                Logger.Log("STREAMCLOUD_NAVIGATION", "Navigated to the correct link, continued: " + continued);
            }
        }

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
            long ticks = DateTime.Now.Ticks - startedWaiting;
            long millis = ticks / TimeSpan.TicksPerMillisecond;
            int remainingMillis = GetEstimateWaitTime() - (int)millis;
            return remainingMillis < 0 ? 0 : remainingMillis;
        }

        public override string GetSiteName()
        {
            return NAME;
        }

        private bool ContinueWhenReady()
        {
            if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete) return false;
            //Logger.Log("SITE_REQUEST_STREAMCLOUD", "WebBrowser is ready.");
            HtmlElement watchButton = GetTargetBrowser().Document.GetElementById("btn_download");
            if (watchButton == null) return false;
            //Logger.Log("SITE_REQUEST_STREAMCLOUD", "watchButton != null -> " + watchButton.OuterHtml);
            if (watchButton.OuterHtml.Contains(" blue"))
            {
                Logger.Log("SITE_REQUEST_STREAMCLOUD", "Clicking on watchButton");
                watchButton.InvokeMember("click");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool continued = false;

        public override int GetEstimateWaitTime()
        {
            return 11000;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        System.Threading.Timer timerReference;
        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            if (!continued)
            {
                continued = ContinueWhenReady();
                receiver.JwLinkStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);
                //Logger.Log("SITE_REQUEST_STREAMCLOUD", "Continued: " + continued);
                if (GetTargetBrowser() != null && !GetTargetBrowser().IsDisposed)
                {
                    timerReference = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestJwData(receiver, requestId))); }, null, 500, -1);
                }
            }
            else
            {
                if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete || !finalSiteLoaded)
                {
                    Logger.Log("SITE_REQUEST_STREAMCLOUD", "Webbrowser is not fully loaded yet. Waiting ...");
                    receiver.JwLinkStatusUpdate(0, 10000, requestId);
                    if (GetTargetBrowser() != null && !GetTargetBrowser().IsDisposed)
                    {
                        timerReference = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestJwData(receiver, requestId))); }, null, 500, -1);
                    }
                }
                else
                {
                    string htmlText = GetTargetBrowser().DocumentText;
                    if (htmlText == "")
                    {
                        Logger.Log("SITE_REQUEST_STREAMCLOUD", "htmlText == \"\", failed to get file.");
                        receiver.ReceiveJwLinks("", requestId);
                        return;
                    }
                    string file = htmlText.GetSubstringBetween(0, "file: \"", "\"");
                    string image = htmlText.GetSubstringBetween(0, "image: \"", "\"");
                    if (file == "" || image == "")
                    {
                        receiver.ReceiveJwLinks("", requestId);
                        return;
                    }
                    string insertion = "file:\"" + file + "\",";   //file:"http://.../",
                    insertion += "\nimage:\"" + image + "\"";   //image:"http://.../"
                    receiver.ReceiveJwLinks(insertion, requestId);
                }
            }
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override void RequestFile(IFileCallbackReceiver receiver, int requestId)
        {
            if (!continued)
            {
                continued = ContinueWhenReady();
                receiver.FileRequestStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);
                Console.WriteLine("Continued: " + continued);
                if (GetTargetBrowser() != null && !GetTargetBrowser().IsDisposed)
                {
                    timerReference = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestFile(receiver, requestId))); }, null, 500, -1);
                }
            }
            else
            {
                if (GetTargetBrowser().ReadyState != WebBrowserReadyState.Complete)
                {
                    receiver.FileRequestStatusUpdate(0, 10000, requestId);
                    if (GetTargetBrowser() != null && !GetTargetBrowser().IsDisposed)
                    {
                        timerReference = new System.Threading.Timer((state) => { GetTargetBrowser().Invoke((MethodInvoker)(() => RequestFile(receiver, requestId))); }, null, 500, -1);
                    }
                }
                else
                {
                    string htmlText = GetTargetBrowser().DocumentText;
                    string file = htmlText.GetSubstringBetween(0, "file: \"", "\"");
                    receiver.ReceiveFileLink(file, requestId);
                }
            }
        }
    }
}
