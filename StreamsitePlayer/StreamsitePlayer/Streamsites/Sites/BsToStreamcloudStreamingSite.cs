using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Sites
{
    class BsToStreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "bsto_streamcloud_site";
        private StreamcloudStreamingSite streamcloudStreamingSite;

        public BsToStreamcloudStreamingSite(string link) : base(link)
        { }

        private async Task<string> GetStreamcloudLinkAsync(string url)
        {
            string res = await Util.RequestSimplifiedHtmlSiteAsync(url);
            string streamcloudLink = "http://streamcloud.eu/" + res.GetSubstringBetween(0, "<a href=\"http://streamcloud.eu/", "\"");
            return streamcloudLink;
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
        }

        public override int GetRemainingWaitTime()
        {
            if (streamcloudStreamingSite != null)
            {
                return streamcloudStreamingSite.GetRemainingWaitTime();
            }
            return GetEstimateWaitTime();
        }

        public override string GetSiteName()
        {
            return NAME;
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
            string streamcloudUrl = await GetStreamcloudLinkAsync(base.link);
            ct.ThrowIfCancellationRequested();
            streamcloudStreamingSite = new StreamcloudStreamingSite(streamcloudUrl);
            return await streamcloudStreamingSite.RequestJwDataAsync(progress, ct);
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string streamcloudUrl = await GetStreamcloudLinkAsync(base.link);
            ct.ThrowIfCancellationRequested();
            streamcloudStreamingSite = new StreamcloudStreamingSite(streamcloudUrl);
            return await streamcloudStreamingSite.RequestFileAsync(progress, ct);
        }
    }
}
