using SeriesPlayer.Utility.ChromiumBrowsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SeriesPlayer.Utility.Extensions;

namespace SeriesPlayer.Streamsites.Sites
{
    class BsToOpenLoadSite : StreamingSite
    {
        public const string NAME = "OpenLoad";

        private OffscreenChromiumBrowser requestBrowser;

        public BsToOpenLoadSite(string link) : base(link)
        {
            requestBrowser = new OffscreenChromiumBrowser();
        }

        public override int GetEstimateWaitTime()
        {
            return 1000;
        }

        public override int GetRemainingWaitTime()
        {
            return 500;
        }

        public override string GetSiteName()
        {
            return "OpenLoad HD";
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string page = await Util.RequestSimplifiedHtmlSiteAsync(base.link);
            string frameSearch = "height='390' allowfullscreen='ture' webkitallowfullscreen='true' mozallowfullscreen='true' src='";
            string frameSearchEnd = "'";
            int startIndex = page.IndexOf(frameSearch) + frameSearch.Length;
            string iFrameUrl = page.GetSubstringBetween(0, frameSearch, frameSearchEnd);

            requestBrowser.WaitForInit();
            requestBrowser.Load(iFrameUrl);

            while (!requestBrowser.IsPageLoaded)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(100);
            }
            string filePartSearch = "id=\"streamurl\">";
            string filePartSearchEnd = "<";
            string iFrame = await requestBrowser.GetHtmlSourceAsync();
            string filePart = iFrame.GetSubstringBetween(0, filePartSearch, filePartSearchEnd);
            string fileUrl = "https://openload.co/stream/" + filePart + "?mime=true";
            return fileUrl;

        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct) + "\",\ntype: \"mp4";
        }
    }
}
