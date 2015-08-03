using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Providers
{
    class TestProvider : StreamProvider
    {
        public const string NAME = "test-provider.bz";
        private string[] VALID_SITES = { StreamcloudStreamingSite.NAME };

        private List<List<Episode>> series;

        public TestProvider()
        {
            series = new List<List<Episode>>();
            series.Add(new List<Episode>());
            series[0].Add(new Episode(1, 1, "Hello World!"));
            series[0].Add(new Episode(1, 2, "Hello Bacon, I'm here!"));
            series[0].Add(new Episode(1, 3, "Hello World!213132"));
            series[0].Add(new Episode(1, 4, "Hello World!513451"));
            series[0].Add(new Episode(1, 5, "Hello World616163!"));
            series[0].Add(new Episode(1, 6, "Hello World665487465!"));
            series[0].Add(new Episode(1, 7, "Hello World867986!"));
            series[0].Add(new Episode(1, 8, "Hello World!996769"));
            series[0].Add(new Episode(1, 9, "Hello World09870!"));
            series.Add(new List<Episode>());
            series[1].Add(new Episode(2, 1, "12213Hello World!"));
            series[1].Add(new Episode(2, 2, "143214Hello Bacon, I'm here!"));
            series[1].Add(new Episode(2, 3, "2352345Hello World!213132"));
            series[1].Add(new Episode(2, 4, "346346Hello World!513451"));
            series[1].Add(new Episode(2, 5, "643634Hello World616163!"));
            series[1].Add(new Episode(2, 6, "43634Hello World665487465!"));
        }

        public override int GetEpisodeCount(int series)
        {
            return this.series[series - 1].Count;
        }

        public override string GetEpisodeLink(int series, int episode, string siteName)
        {
            foreach (Episode e in this.series[series - 1])
            {
                if (e.Number == episode) return e.GetLink(siteName);
            }
            return "";
        }

        public override List<Episode> GetEpisodeList(int series)
        {
            return this.series[series - 1];
        }

        public override string GetEpisodeName(int series, int episode)
        {
            foreach (Episode e in this.series[series - 1])
            {
                if (e.Number == episode) return e.Name;
            }
            return "ERROR: EPISODE NOT FOUND!";
        }

        public override string GetLinkInstructions()
        {
            return "This is a test class ... just enter nothing ^^";
        }

        public override string GetReadableSiteName()
        {
            return "test-provider.bz";
        }

        public override int GetSeriesCount()
        {
            return series.Count;
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkNameExtension, Control threadAnchor)
        {
            Util.RequestSimplifiedHtmlSite("http://bs.to/serie/Sword-Art-Online/1");
            return StreamProvider.RESULT_OK;
        }
    }
}
