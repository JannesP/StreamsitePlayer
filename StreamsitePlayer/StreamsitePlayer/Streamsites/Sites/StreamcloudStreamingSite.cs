using SeriesPlayer.Utility.ChromiumBrowsers;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SeriesPlayer.Streamsites.Sites
{
    class StreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "Streamcloud";
        public const int WAIT_TIME_UNKNOWN = 123456;
        public string trueLink;
        private OffscreenChromiumBrowser requestBrowser;
        private bool continued = false;

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
                if (e.Browser.MainFrame.Url == trueLink)
                {
                    if (continued)
                    {
                        finalSiteLoaded = true;
                        Logger.Log("STREAMCLOUD_NAVIGATION", "Navigated to the correct link, continued: " + continued);
                    }
                    else
                    {
                        startedWaiting = DateTime.Now.Ticks;
                    }
                }
            }
        }

        private long startedWaiting = 0; 
        public override int GetRemainingWaitTime()
        {
            if (!requestBrowser.IsPageLoaded || startedWaiting == 0)
            {
                return GetEstimateWaitTime();
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

        private async Task ContinueWhenReady(IProgress<int> progress, CancellationToken ct)
        {
            await Task.Run(async () =>
            {
                while (!continued)
                {
                    ct.ThrowIfCancellationRequested();
                    await Task.Delay(100);
                    progress.Report((int)((float)(GetEstimateWaitTime() - GetRemainingWaitTime()) / (float)GetEstimateWaitTime() * 100F));
                    if (!requestBrowser.IsPageLoaded) continue;
                    //check if button exists
                    bool btn_downloadExists = Convert.ToBoolean(await requestBrowser.EvaluateJavaScriptRawAsync("document.getElementById('btn_download') != null;"));
                    if (!btn_downloadExists) continue;
                    //check if button is 'blue' (active)
                    if (Convert.ToBoolean(await requestBrowser.EvaluateJavaScriptRawAsync("document.getElementById('btn_download').classList.contains('blue');")))
                    {
                        Logger.Log("SITE_REQUEST_STREAMCLOUD", "Clicking on watchButton");
                        requestBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('btn_download').click();");
                        continued = true;
                    }
                }
            });
        }

        public override int GetEstimateWaitTime()
        {
            return 11000;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }
        
        public async Task<string> GetFile(IProgress<int> progress, CancellationToken ct)
        {
            await ContinueWhenReady(progress, ct);

            return await Task.Run(async () =>
            {
                string file = "";
                while (!requestBrowser.IsPageLoaded || !finalSiteLoaded)
                {
                    ct.ThrowIfCancellationRequested();
                    await Task.Delay(500);
                }
                string htmlText = await requestBrowser.GetHtmlSourceAsync();
                if (htmlText == "")
                {
                    ct.ThrowIfCancellationRequested();
                    Logger.Log("SITE_REQUEST_STREAMCLOUD", "htmlText == \"\", failed to get file.");
                    Util.ShowUserInformation("Streamcloud didn't load properly, trying again . . .");
                    return await GetFile(progress, ct);
                }
                file = htmlText.GetSubstringBetween(0, "file: \"", "\"");
                ct.ThrowIfCancellationRequested();
                return file;
            });
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct);
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            try
            {
                return await GetFile(progress, ct);
            }
            catch (OperationCanceledException ex)
            {
                requestBrowser.LoadingStateChanged -= RequestBrowser_LoadingStateChanged;
                requestBrowser.Load("about:blank");
                throw ex;
            }
        }
    }
}
