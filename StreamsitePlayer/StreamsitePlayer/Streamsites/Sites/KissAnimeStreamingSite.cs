using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites.Sites
{
    class KissAnimeStreamingSite : StreamingSite
    {
        public const string NAME = "kissanime";

        public KissAnimeStreamingSite(string link) : base(link)
        {

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
            return "kissanime";
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
            string jwplaylisturl = page.GetSubstringBetween(0, "load_player('//", "');");
            string jwPlaylist = (await Util.RequestSimplifiedHtmlSiteAsync("http://" + jwplaylisturl)).Replace("\\/", "/");
            string file = jwPlaylist.GetSubstringBetween(0, "file\":\"", "\"");
            return file;
        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct) + "\",\ntype: \"mp4";
        }
    }
}
