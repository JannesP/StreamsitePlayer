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
    class BsToVivoStreamingSite : StreamingSite
    {
        public const string NAME = "bsto_vivo_site";
        private VivoStreamingSite vivoStreamingSite;

        public BsToVivoStreamingSite(string link) : base(link)
        { }

        private async Task<string> GetVivoLinkAsync(string url)
        {
            string res = await Util.RequestSimplifiedHtmlSiteAsync(url);
            string vivoLink = "http://vivo.sx/" + res.GetSubstringBetween(0, "<a href=\"http://vivo.sx/", "\"");
            return vivoLink;
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
        }
        
        public override int GetRemainingWaitTime()
        {
            if (vivoStreamingSite != null)
            {
                return vivoStreamingSite.GetRemainingWaitTime();
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
            string vivoUrl = await GetVivoLinkAsync(base.link);
            vivoStreamingSite = new VivoStreamingSite(vivoUrl);
            return await vivoStreamingSite.RequestJwDataAsync(progress, ct);
        }

        public async override Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string vivoUrl = await GetVivoLinkAsync(base.link);
            vivoStreamingSite = new VivoStreamingSite(vivoUrl);
            return await vivoStreamingSite.RequestFileAsync(progress, ct);
        }
    }
}
