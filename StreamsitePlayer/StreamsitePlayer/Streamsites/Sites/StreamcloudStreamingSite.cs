using SeriesPlayer.Utility.ChromiumBrowsers;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Sites
{
    class StreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "Streamcloud";
        public const int WAIT_TIME_UNKNOWN = 123456;
        public string trueLink;
        private OffscreenChromiumBrowser requestBrowser;

        private bool finalSiteLoaded;

        public StreamcloudStreamingSite(string link) : base(link)
        {
            requestBrowser = new OffscreenChromiumBrowser();
            requestBrowser.WaitForInit();
            requestBrowser.LoadingStateChanged += RequestBrowser_LoadingStateChanged;
            trueLink = link;
            requestBrowser.Load(link);
        }

        private void RequestBrowser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Logger.Log("STREAMCLOUD_NAVIGATION", e.Browser.MainFrame.Url);
                if (e.Browser.MainFrame.Url == trueLink && continued)
                {
                    finalSiteLoaded = true;
                    Logger.Log("STREAMCLOUD_NAVIGATION", "Navigated to the correct link, continued: " + continued);
                }
            }
        }

        ~StreamcloudStreamingSite()
        {
            requestBrowser.Dispose();
            requestBrowser = null;
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
            if (!requestBrowser.IsPageLoaded)
            {
                return GetEstimateWaitTime();
            }
            else
            {
                if (startedWaiting == 0)
                {
                    startedWaiting = DateTime.Now.Ticks;
                }
            }
            if (Convert.ToBoolean(requestBrowser.EvaluateJavaScriptRaw("document.getElementById('countdown') == null;")))
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
            if (!requestBrowser.IsPageLoaded) return false;
            //check if button exists
            bool btn_downloadExists = Convert.ToBoolean(requestBrowser.EvaluateJavaScriptRaw("document.getElementById('btn_download') != null;"));
            if (!btn_downloadExists) return false;
            //check if button is 'blue' (active)
            if (Convert.ToBoolean(requestBrowser.EvaluateJavaScriptRaw("document.getElementById('btn_download').classList.contains('blue');")))
            {
                Logger.Log("SITE_REQUEST_STREAMCLOUD", "Clicking on watchButton");
                requestBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('btn_download').click();");
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

                RequestJwDataLoop(receiver, requestId);
            }
            else
            {
                if (!requestBrowser.IsPageLoaded || !finalSiteLoaded)
                {
                    Logger.Log("SITE_REQUEST_STREAMCLOUD", "Webbrowser is not fully loaded yet. Waiting ...");
                    receiver.JwLinkStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);

                    RequestJwDataLoop(receiver, requestId);
                }
                else
                {
                    string htmlText = requestBrowser.HtmlSource;
                    if (htmlText == "")
                    {
                        Logger.Log("SITE_REQUEST_STREAMCLOUD", "htmlText == \"\", failed to get file.");
                        Util.ShowUserInformation("Streamcloud didn't load properly, please try again.");
                        receiver.ReceiveJwLinks("", requestId);
                        return;
                    }
                    string file = htmlText.GetSubstringBetween(0, "file: \"", "\"");
                    string image = htmlText.GetSubstringBetween(0, "image: \"", "\"");
                    if (file == "" || image == "")
                    {
                        Util.ShowUserInformation("Streamcloud didn't load properly, please try again.");
                        receiver.ReceiveJwLinks("", requestId);
                        return;
                    }
                    receiver.ReceiveJwLinks(file, requestId);
                }
            }
        }

        private void RequestJwDataLoop(IJwCallbackReceiver receiver, int requestId)
        {
            timerReference = new System.Threading.Timer((state) =>
            {
                if (receiver == null || (receiver is Control && ((Control)receiver).IsDisposed)) return;    //if receiver is disposed or null

                if (receiver is Control && ((Control)receiver).InvokeRequired)
                {
                    ((Control)receiver).Invoke((MethodInvoker)(() => RequestJwData(receiver, requestId)));
                }
                else
                {
                    RequestJwData(receiver, requestId);
                }
            }, null, 500, -1);
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        private void RequestFileLoop(IFileCallbackReceiver receiver, int requestId)
        {
            timerReference = new System.Threading.Timer((state) =>
            {
                if (receiver == null || (receiver is Control && ((Control)receiver).IsDisposed)) return;    //if receiver is disposed or null

                if (receiver is Control && ((Control)receiver).InvokeRequired)
                {
                    ((Control)receiver).Invoke((MethodInvoker)(() => RequestFile(receiver, requestId)));
                }
                else
                {
                    RequestFile(receiver, requestId);
                }

            }, null, 500, -1);
        }

        public override void RequestFile(IFileCallbackReceiver receiver, int requestId)
        {
            if (!continued)
            {
                continued = ContinueWhenReady();
                receiver.FileRequestStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);
                Console.WriteLine("Continued: " + continued);
                RequestFileLoop(receiver, requestId);
            }
            else
            {
                if (!requestBrowser.IsPageLoaded || !finalSiteLoaded)
                {
                    receiver.FileRequestStatusUpdate(GetRemainingWaitTime(), GetEstimateWaitTime(), requestId);
                    RequestFileLoop(receiver, requestId);
                }
                else
                {
                    string htmlText = requestBrowser.HtmlSource;
                    string file = htmlText.GetSubstringBetween(0, "file: \"", "\"");
                    if (file == "")
                    {
                        Util.ShowUserInformation("Streamcloud didn't load properly, please try again.");
                    }
                    receiver.ReceiveFileLink(file, requestId);
                }
            }
        }
    }
}
