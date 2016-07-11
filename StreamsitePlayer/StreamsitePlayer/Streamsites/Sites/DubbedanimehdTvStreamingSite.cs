using SeriesPlayer.Utility.ChromiumBrowsers;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SeriesPlayer.Streamsites.Sites
{
    class DubbedanimehdTvStreamingSite : StreamingSite
    {
        private OffscreenChromiumBrowser requestBrowser;
        public DubbedanimehdTvStreamingSite(string link) : base(link)
        {
            base.link = link.Replace("dubbedanimehd.org", "dubbedanimehd.co")
                .Replace("dubbedanimehd.net", "dubbedanimehd.co")
                .Replace("dubbedanimehd.tv", "dubbedanimehd.co");
        }

        public const string NAME = "dubbedanimehd";

        public override int GetEstimateWaitTime()
        {
            return 1;
        }

        public override int GetRemainingWaitTime()
        {
            return 0;
        }

        public override string GetSiteName()
        {
            return NAME;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        private bool iFrameNavigated = false;
        private string iFrameUrl = "";

        
        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        private async Task<string> HandleRequest(CancellationToken ct)
        {
            string htmlText = await Util.RequestSimplifiedHtmlSiteAsync(base.link);
            ct.ThrowIfCancellationRequested();
            string iFrameSearch = "id='video' src='";
            string iframeUrl = htmlText.GetSubstringBetween(0, iFrameSearch, "'");
            Logger.Log("SITE_REQUEST_DAHD", "Found IFrame for: " + iframeUrl);

            htmlText = await Util.RequestSimplifiedHtmlSiteAsync(iframeUrl);
            string file = htmlText.GetSubstringBetween(0, "var x04c = unescape('", "');");
            file = WebUtility.UrlDecode(file);
            Logger.Log("SITE_REQUEST_DAHD", "Found file at: " + file);
            return file;
        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            string link = await HandleRequest(ct);
            ct.ThrowIfCancellationRequested();
            return link;
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string link = await HandleRequest(ct);
            ct.ThrowIfCancellationRequested();
            return link;
        }
    }
}
