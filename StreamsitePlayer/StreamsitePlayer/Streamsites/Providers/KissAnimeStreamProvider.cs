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
            if (keyword.Length > 1)
            {
                string responseString = "";
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "type", "Anime" },
                        { "keyword", keyword }
                    };

                    var content = new FormUrlEncodedContent(values);
                    try
                    {
                        var response = await client.PostAsync("http://kissanime.to/Search/SearchSuggest", content, ct);
                        responseString = await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        Logger.Log(ex);
                    }
                }
                ct.ThrowIfCancellationRequested();
                return new Dictionary<string, string>();
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
