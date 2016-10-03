using SeriesPlayer.Utility.ChromiumBrowsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using System.Threading;

namespace SeriesPlayer.Streamsites.Sites
{
    class VivoStreamingSite : StreamingSite
    {
        public const string NAME = "Vivo";

        private bool continued = false;
        private OffscreenChromiumBrowser requestBrowser;

        public VivoStreamingSite(string link) : base(link)
        {
            requestBrowser = new OffscreenChromiumBrowser();
            requestBrowser.WaitForInit();
        }

        private async Task ClickOnContinue()
        {
            var btnExists = await requestBrowser.EvaluateJavaScriptRawAsync("document.getElementById('access') != null;");
            Logger.Log("VivoLoading", "btnExists: " + btnExists);
            if (btnExists != null && Convert.ToBoolean(btnExists))
            {
                requestBrowser.ExecuteScriptAsync("document.getElementById('access').disabled = false;");
                requestBrowser.ExecuteScriptAsync("document.getElementById('access').click();");
            }
            else
            {
                await Task.Delay(100).ContinueWith(t => ClickOnContinue());
            }
        }

        private async Task<string> FindLink(CancellationToken ct)
        {
            if (!continued)
            {
                await ClickOnContinue();
                continued = true;
            }
            string fileUrl = Convert.ToString(await requestBrowser.EvaluateJavaScriptRawAsync(
                    @"(function() {
                        var elements = document.getElementsByClassName('stream-content');
                        if (elements.length > 0) {
                            return elements[0].getAttribute('data-url');
                        } else {
                            return 'failed';
                        }
                    })();"
                ));
            Logger.Log("VivoLoading", "fileUrl: " + fileUrl);
            if (fileUrl != "")
            {
                ct.ThrowIfCancellationRequested();
                return fileUrl;
            }
            else
            {
                ct.ThrowIfCancellationRequested();
                return (await Task.Delay(500).ContinueWith(t => FindLink(ct))).Result;
            }
        }

        public override int GetEstimateWaitTime()
        {
            return 1500;
        }

        public override int GetRemainingWaitTime()
        {
            return GetEstimateWaitTime();
        }

        public override string GetSiteName()
        {
            return "vivo";
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct) + "\",\ntype: \"mp4";
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            requestBrowser.Load(base.link);
            try
            {
                ct.ThrowIfCancellationRequested();
                return await FindLink(ct);
            }
            catch (OperationCanceledException ex)
            {
                requestBrowser.Load("about:blank");
                requestBrowser = null;
                throw ex;
            }
            
        }
    }
}
