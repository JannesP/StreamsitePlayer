using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Providers
{
    class RyuanimeStreamProvider : StreamProvider
    {
        public const string NAME = "Ryuanime";
        private const string URL_PRE = "http://www.ryuanime.com/watch-anime/";
        private const string LINK_SEARCH = "<a class=\"icon Streamcloud\" title=\"Streamcloud\"   href=\"";
        private List<List<Episode>> seasons;

        private string[] VALID_SITES = { RyuanimeStreamingSite.NAME };

        public override string GetLinkInstructions()
        {
            return "http://www.ryuanime.com/watch-anime/???/";
        }

        public override string GetReadableSiteName()
        {
            return "Ryuanime.com";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkNameExtension, Control threadAnchor)
        {
            base.series = Seriescache.ReadCachedSeries(NAME, siteLinkNameExtension);
            if (seasons != null) return StreamProvider.RESULT_USE_CACHED;

            string htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkNameExtension);
            seasons = new List<List<Episode>>();
            seasons.Add(ScanForEpisodes(htmlEpisodeOverview, siteLinkNameExtension));
            string seriesName = Util.GetStringBetween(htmlEpisodeOverview, 0, "<h1> ", "</h1>");
            base.series = new Series(seasons, seriesName);
            Seriescache.CacheSeries(NAME, siteLinkNameExtension, series);
            FormMain.SeriesOpenCallback(null);
            return StreamProvider.RESULT_OK;
        }

        private static List<Episode> ScanForEpisodes(string html, string siteLinkNameExtension)
        {
            List<Episode> episodes = new List<Episode>();
            int index = 0;
            while (true)
            {
                string searchString = "/watch/dubbed/episode/" + siteLinkNameExtension + "-episode-" + (episodes.Count + 1);
                index = html.IndexOf(searchString, index);
                if (index != -1)
                {
                    Episode e = new Episode();
                    string link = "http://ryuanime.com/" + searchString;

                    e.AddLink(RyuanimeStreamingSite.NAME, link);
                    e.Name = (episodes.Count + 1).ToString();
                    e.Number = episodes.Count + 1;
                    e.Season = 0;

                    episodes.Add(e);
                    Application.DoEvents();
                }
                else break;
            }
            return episodes;
        }
    }
}
