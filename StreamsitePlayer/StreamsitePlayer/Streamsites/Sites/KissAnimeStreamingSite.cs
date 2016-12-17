using Newtonsoft.Json;
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
            string episodeId = page.GetSubstringBetween(0, "episode_id = '", "';");
            string response = await Util.PostRequestAsync("http://kissanime.io/ajax/anime/load_episodes/", new Dictionary<string, string>
            {
                { "episode_id", episodeId } 
            });
            dynamic jsonObj = JsonConvert.DeserializeObject(response);
            var embed = false;
            var value = "";
            try
            {
                embed = jsonObj.embed;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { }
            try
            {
                value = jsonObj.value;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                throw new Exception("Error loading file url.");
            }
            if (embed)
            {
                string url = "http://" + value.GetSubstringBetween(0, "src=\"//", "\"");
                OpenloadSite site = new OpenloadSite(url);
                return await site.RequestFileAsync(progress, ct);
            }
            else
            {
                string jwPlaylist = (await Util.RequestSimplifiedHtmlSiteAsync("http:" + value)).Replace("\\/", "/");
                string file = jwPlaylist.GetSubstringBetween(0, "file\":\"", "\"");
                return file;
            }

        }

        public async override Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct) + "\",\ntype: \"mp4";
        }
    }
}
