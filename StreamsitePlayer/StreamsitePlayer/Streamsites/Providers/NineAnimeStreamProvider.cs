using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;

namespace SeriesPlayer.Streamsites.Providers
{
    class NineAnimeStreamProvider : StreamProvider
    {
        public const string NAME = "nineanime";
        public override async Task<int> LoadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            base.siteLinkExtension = siteLinkExtension;
            series = await Seriescache.ReadCachedSeriesAsync(NAME, siteLinkExtension);
            if (series != null) return StreamProvider.RESULT_USE_CACHED;
            return await ReloadSeriesAsync(siteLinkExtension, threadAnchor);
        }

        public override async Task<int> ReloadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            string html = await Util.RequestSimplifiedHtmlSiteAsync(GetWebsiteLink() + "watch/" + siteLinkExtension);

            string name = html.GetSubstringBetween(0, "<h1 class=\"title\">", "</h1>");

            List<Episode> episodes = new List<Episode>();
            int number = 1;
            int currentIndex = html.IndexOf("Server F1", StringComparison.Ordinal);
            int endIndex = html.IndexOf("</ul>", currentIndex, StringComparison.Ordinal);
            while (currentIndex < endIndex && currentIndex > -1)
            {
                string dataid = html.GetSubstringBetween(currentIndex, "data-id=\"", "\"", out currentIndex);
                string epName = html.GetSubstringBetween(currentIndex, ">", "</a>", out currentIndex);

                Episode e = new Episode();
                e.Name = epName;
                e.Number = number++;
                e.Season = 1;
                e.AddLink(NineAnimeStreamingSite.NAME, $"{GetWebsiteLink()}watch/{base.siteLinkExtension}/{dataid}");
                episodes.Add(e);
                currentIndex = html.IndexOf("<a data", currentIndex, StringComparison.Ordinal);
            }

            var seasons = new List<List<Episode>>() { episodes };
            var s = new Series(seasons, name, NAME, siteLinkExtension, GetWebsiteLink() + "watch/" + siteLinkExtension);
            base.series = s;
            await Seriescache.CacheSeriesAsync(s);

            return StreamProvider.RESULT_OK;
        }

        public override string GetReadableSiteName()
        {
            return "9anime";
        }

        public override string[] GetValidStreamingSites()
        {
            return new[] {NineAnimeStreamingSite.NAME};
        }

        public override string GetWebsiteLink()
        {
            return "https://9anime.to/";
        }

        public override SearchMode SupportedSearchMode { get; } = SearchMode.REMOTE;
        public override async Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct)
        {
            string response = await Util.RequestSimplifiedHtmlSiteAsync("https://9anime.to/ajax/film/search?sort=year%3Adesc&keyword=" + keyword);
            var result = new Dictionary<string, string>();
            dynamic jsonObj = JsonConvert.DeserializeObject(response);
            var html = "";
            try
            {
                html = jsonObj.html;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { }
            if (html != "" && html != "\n")
            {
                int currIndex = 0;
                while (currIndex != -1)
                {
                    string link = html.GetSubstringBetween(currIndex, "://9anime.to/watch/", "\">", out currIndex);
                    string name = html.GetSubstringBetween(currIndex, ">", "</a>", out currIndex);
                    currIndex = html.IndexOf("class=\"name\"", currIndex, StringComparison.Ordinal);
                    result.Add(name, link);
                }

            }

            return result;
        }

        public override Task<Dictionary<string, string>> RequestSearchIndexAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
