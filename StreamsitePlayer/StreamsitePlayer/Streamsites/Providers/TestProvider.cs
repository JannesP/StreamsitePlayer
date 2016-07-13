using SeriesPlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class TestProvider : StreamProvider
    {
        public const string NAME = "test-provider.bz";
        private string[] VALID_SITES = { StreamcloudStreamingSite.NAME };

        public TestProvider()
        {
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(new List<Episode>());
            seasons[0].Add(new Episode(1, 1, "Hello World!"));
            seasons[0].Add(new Episode(1, 2, "Hello Bacon, I'm here!"));
            seasons[0].Add(new Episode(1, 3, "Hello World!213132"));
            seasons[0].Add(new Episode(1, 4, "Hello World!513451"));
            seasons[0].Add(new Episode(1, 5, "Hello World616163!"));
            seasons[0].Add(new Episode(1, 6, "Hello World665487465!"));
            seasons[0].Add(new Episode(1, 7, "Hello World867986!"));
            seasons[0].Add(new Episode(1, 8, "Hello World!996769"));
            seasons[0].Add(new Episode(1, 9, "Hello World09870!"));
            seasons.Add(new List<Episode>());
            seasons[1].Add(new Episode(2, 1, "12213Hello World!"));
            seasons[1].Add(new Episode(2, 2, "143214Hello Bacon, I'm here!"));
            seasons[1].Add(new Episode(2, 3, "2352345Hello World!213132"));
            seasons[1].Add(new Episode(2, 4, "346346Hello World!513451"));
            seasons[1].Add(new Episode(2, 5, "643634Hello World616163!"));
            seasons[1].Add(new Episode(2, 6, "43634Hello World665487465!"));
            base.series = new Series(seasons, "TestName", NAME, "test-series", "http://www.google.de/");
        }

        public override SearchMode SupportedSearchMode
        {
            get
            {
                return SearchMode.LOCAL;
            }
        }

        public override string GetReadableSiteName()
        {
            return "test-provider.bz";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override string GetWebsiteLink()
        {
            return "https://www.google.com/";
        }

        public async override Task<int> LoadSeriesAsync(string siteLinkNameExtension, Control threadAnchor)
        {
            return await Task.Run(() => { return StreamProvider.RESULT_OK; });
        }

        public override Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public override Task<Dictionary<string, string>> RequestSearchIndexAsync(CancellationToken ct)
        {
            return Task.Run(() => { return new Dictionary<string, string>(); } );
        }
    }
}
