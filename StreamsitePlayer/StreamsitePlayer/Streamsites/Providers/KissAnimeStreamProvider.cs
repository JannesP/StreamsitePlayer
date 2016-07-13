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
        private OffscreenChromiumBrowser requestBrowser;
        private AjaxResponseHandler ajaxResponseHandler;

        public KissAnimeStreamProvider()
        {
            requestBrowser = new OffscreenChromiumBrowser("http://www.kissanime.to/");
            ajaxResponseHandler = new AjaxResponseHandler();
            requestBrowser.RegisterJsObject("ajaxResponseHandler", ajaxResponseHandler);
            requestBrowser.WaitForInit();
        }

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
            throw new NotImplementedException();
        }

        public override string GetWebsiteLink()
        {
            return "http://kissanime.to/";
        }

        public override Task<int> LoadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            throw new NotImplementedException();
        }

        public async override Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct)
        {
            if (requestBrowser == null)
            {
                requestBrowser.Load("http://www.kissanime.to/");
                await Task.Run(async () =>
                {
                    while (!requestBrowser.IsPageLoaded)
                    {
                        ct.ThrowIfCancellationRequested();
                        await Task.Delay(500);
                    }
                });
                ct.ThrowIfCancellationRequested();
            }
            if (keyword.Length > 1)
            {
                //await requestBrowser.ExecuteJavaScriptRawAsync(@"ajaxResponseHandler.onSearchResponse('message');");
                string script = @"$.ajax({
                    type: 'POST',
                    url: '/Search/SearchSuggest',
                    data: 'type=Anime' + '&keyword=" + keyword + @"',
                    success: function(message) {
                        ajaxResponseHandler.onSearchResponse('" + keyword + @"', message);
                    }
                });";
                await requestBrowser.ExecuteJavaScriptRawAsync(script);
                string response = await ajaxResponseHandler.WaitForSearchResponseAsync(keyword, ct);
                var result = new Dictionary<string, string>();

                const string SERIES_SEARCH = "<a href=\"http://kissanime.to/";
                const string END_LINK = "\">";
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
                            Logger.Log("KISSANIME", "A series with the name: " + name + " for the link: " + seriesExtension + "already exists!");
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
