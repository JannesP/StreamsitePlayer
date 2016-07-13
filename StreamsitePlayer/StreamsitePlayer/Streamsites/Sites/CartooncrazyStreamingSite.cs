using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites.Sites
{
    class CartooncrazyStreamingSite : StreamingSite
    {
        public const string NAME = "toonme";

        public CartooncrazyStreamingSite(string link) : base(link)
        { }

        public override int GetEstimateWaitTime()
        {
            return 500;
        }

        public override int GetRemainingWaitTime()
        {
            return 500;
        }

        public override string GetSiteName()
        {
            return "toonme";
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        private async Task<string> GetFileLink(CancellationToken ct)
        {
            string link = "";

            string page = await Util.RequestSimplifiedHtmlSiteAsync(base.link);
            string iFrame = page.GetSubstringBetween(0, "<iframe src=\"", "\" ");
            ct.ThrowIfCancellationRequested();
            link = await CheckNextIframeForLink(iFrame, ct);

            return link;
        }

        private async Task<string> CheckNextIframeForLink(string iFrameLink, CancellationToken ct)
        {
            if (iFrameLink == "")
            {
                return "";
            }
            string page = await Util.RequestSimplifiedHtmlSiteAsync(iFrameLink);
            page = page.Replace(" ", "");
            int index = 0;
            string link = page.GetSubstringBetween(0, ":[{file:\"", "\"", out index);
            if (link != "")
            {
                link = page.GetSubstringBetween(index, "file:\"", "\"");
            } else
            {
                link = page.GetSubstringBetween(0, "file:\"", "\"");
            }
            if (link == "")
            {
                link = page.GetSubstringBetween(0, "file:'", "'");
            }
            if (link == "")
            {
                ct.ThrowIfCancellationRequested();
                return await CheckNextIframeForLink(page.GetSubstringBetween(0, "<iframesrc=\"", "\""), ct);
            }
            else
            {
                return link;
            }
        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            string result = (await GetFileLink(ct)) + "\",\ntype: \"mp4";
            ct.ThrowIfCancellationRequested();
            return result;
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string result = (await GetFileLink(ct)) + "\",\ntype: \"mp4";
            ct.ThrowIfCancellationRequested();
            return result;
        }
    }
}
