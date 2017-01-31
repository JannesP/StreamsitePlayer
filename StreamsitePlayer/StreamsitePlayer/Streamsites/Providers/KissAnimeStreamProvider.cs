using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.ChromiumBrowsers;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class KissAnimeStreamProvider : StreamProvider
    {
        public const string NAME = "kissanime";
        private OffscreenChromiumBrowser requestBrowser = null;
        private AjaxResponseHandler ajaxResponseHandler;

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        private class AjaxResponseHandler
        {
            public Dictionary<string, string> SearchResponses
            {
                private set;
                get;
            } = new Dictionary<string, string>();
            public void OnSearchResponse(string search, string response)
            {
                SearchResponses.Add(search, response);
            }
            
            public async Task<string> WaitForSearchResponseAsync(string key, CancellationToken ct)
            {
                while (!SearchResponses.Keys.Contains(key))
                {
                    ct.ThrowIfCancellationRequested();
                    await Task.Delay(100, ct);
                }
                string response = SearchResponses[key];
                SearchResponses.Remove(key);
                return response;
            }
        }

        private void LoadBrowser()
        {
            requestBrowser = new OffscreenChromiumBrowser("http://www.kissanime.io/");
            ajaxResponseHandler = new AjaxResponseHandler();
            requestBrowser.RegisterJsObject("ajaxResponseHandler", ajaxResponseHandler);
            requestBrowser.WaitForInit();
        }

        public override SearchMode SupportedSearchMode
        {
            get
            {
                return SearchMode.REMOTE;
            }
        }

        public override string GetReadableSiteName()
        {
            return "KissAnime.to";
        }

        public override string[] GetValidStreamingSites()
        {
            return new string[] { KissAnimeStreamingSite.NAME };
        }

        public override string GetWebsiteLink()
        {
            return "http://kissanime.io/";
        }

        public async override Task<int> LoadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            base.siteLinkExtension = siteLinkExtension;
            series = await Seriescache.ReadCachedSeriesAsync(NAME, siteLinkExtension);
            if (series != null) return StreamProvider.RESULT_USE_CACHED;
            return await ReloadSeriesAsync(siteLinkExtension, threadAnchor);
        }

        public async override Task<int> ReloadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            string seriesPage = await Util.RequestSimplifiedHtmlSiteAsync(GetWebsiteLink() + "Anime/" + siteLinkExtension);

            if (seriesPage.Contains("The service is unavailable."))
            {
                return RESULT_NET_FAILED;
            }

            int titleStartIndex = seriesPage.IndexOf("bigChar\" href=");
            string name = "";
            if (titleStartIndex != -1)
            {
                name = seriesPage.GetSubstringBetween(titleStartIndex, "\">", "</a>");
            }

            List<Episode> episodes = new List<Episode>();

            int startIndex = seriesPage.IndexOf("<div class=\"listing");
            if (startIndex == -1) return StreamProvider.RESULT_NET_FAILED;
            int endIndex = seriesPage.IndexOf("</div></div></div>", startIndex);

            int currentIndex = startIndex;
            while (currentIndex < endIndex && currentIndex > -1)
            {
                Episode e = new Episode();
                string link = "http:" + seriesPage.GetSubstringBetween(currentIndex, "href=\"", "\"", out currentIndex);
                e.AddLink(KissAnimeStreamingSite.NAME, link);
                string epName = seriesPage.GetSubstringBetween(currentIndex, ">", "</a>", out currentIndex).Replace(name + " ", "");
                e.Name = epName;
                e.Season = 1;
                if (currentIndex < endIndex)
                {
                    episodes.Insert(0, e);
                }
            }

            for (int i = 0; i < episodes.Count; i++)
            {
                episodes[i].Number = i + 1;
            }

            List<List<Episode>> seasons = new List<List<Episode>>() { episodes };
            Series s = new Series(seasons, name, NAME, siteLinkExtension, GetWebsiteLink() + "Anime/" + siteLinkExtension);
            base.series = s;
            await Seriescache.CacheSeriesAsync(s);

            return StreamProvider.RESULT_OK;
        }

        public async override Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct)
        {
            if (keyword.Contains("\"") || keyword.Contains("'"))
            {
                Util.ShowUserInformation("Don't use \" or ' in your search!");
                return new Dictionary<string, string>();
            }
            if (this.requestBrowser == null)
            {
                LoadBrowser();
            }
            if (!requestBrowser.IsPageLoaded)
            {
                await Task.Run(async () =>
                {
                    while (!requestBrowser.IsPageLoaded)
                    {
                        ct.ThrowIfCancellationRequested();
                        await Task.Delay(200);
                    }
                });
            }
            ct.ThrowIfCancellationRequested();
            if (keyword.Length > 1)
            {
                string script = @"$.ajax({
                    type: 'GET',
                    url: '/Search/SearchSuggest/?type=Anime&keyword=" + keyword + @"',
                    data: 'type=Anime&keyword=" + keyword + @"',
                    success: function(message) {
                        ajaxResponseHandler.onSearchResponse('" + keyword + @"', message);
                    }
                });";
                await requestBrowser.ExecuteJavaScriptRawAsync(script);
                string response = await ajaxResponseHandler.WaitForSearchResponseAsync(keyword, ct);
                var result = new Dictionary<string, string>();

                const string SERIES_SEARCH = "/Anime/";
                const string END_LINK = "/\">";
                const string END_NAME = "</a>";

                int searchIndex = response != "" ? 0 : -1;
                while (searchIndex != -1)
                {
                    string seriesExtension = response.GetSubstringBetween(searchIndex, SERIES_SEARCH, END_LINK, out searchIndex);
                    if (searchIndex == -1) continue;
                    string name = response.GetSubstringBetween(searchIndex, END_LINK, END_NAME, out searchIndex);
                    if (searchIndex != -1)
                    {
                        if (seriesExtension.ToLower().Contains("dub") && !name.ToLower().Contains("dub"))
                        {
                            name += " (dub)";
                        }
                        if (!result.Keys.Contains(name))
                        {
                            result.Add(name, seriesExtension);
                        }
                        else
                        {
                            Logger.Log("KISSANIME", "A series with the name: " + name + " for the link: " + seriesExtension + " already exists and was ignored!");
                        }
                    }
                }
                return result;
            }
            else
            {
                return new Dictionary<string, string>();
            }
            
        }

        public override Task<Dictionary<string, string>> RequestSearchIndexAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
