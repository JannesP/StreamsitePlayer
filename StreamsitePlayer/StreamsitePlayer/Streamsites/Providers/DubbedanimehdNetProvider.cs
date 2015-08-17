using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Providers
{
    class DubbedanimehdNetProvider : StreamProvider
    {
        public const string NAME = "dubbedanimehd";
        private const string URL_PRE = "http://www.dubbedanimehd.net/watch/";

        private string[] VALID_SITES = { DubbedanimehdNetStreamingSite.NAME };

        public override string GetLinkInstructions()
        {
            return "http://www.dubbedanimehd.net/watch/???/";
        }

        public override string GetReadableSiteName()
        {
            return "dubbedanimehd.net";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkNameExtension, Control threadAnchor)
        {
            base.series = Seriescache.ReadCachedSeries(NAME, siteLinkNameExtension);
            if (base.series != null) return StreamProvider.RESULT_USE_CACHED;

            string htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkNameExtension);
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ScanForEpisodes(htmlEpisodeOverview, siteLinkNameExtension));
            string seriesName = Util.GetStringBetween(htmlEpisodeOverview, 0, "wp-post-image\" alt=\"", "\" />");
            base.series = new Series(seasons, seriesName);
            Seriescache.CacheSeries(NAME, siteLinkNameExtension, base.series);
            FormMain.SeriesOpenCallback(null);
            return StreamProvider.RESULT_OK;
        }

        private static List<Episode> ScanForEpisodes(string html, string siteLinkNameExtension)
        {
            List<Episode> episodes = new List<Episode>();
            html = Util.GetStringBetween(html, 0, "<div style=\"font-size:14px;\">", "</div>");
                
            List<string> linkList = new List<string>(html.Split('"'));
            for (int i = 0; i < linkList.Count; i++)
            {
                if (!linkList[i].Contains("http")) linkList.RemoveAt(i--);
            }
            for (int i = (linkList.Count - 1); i >= 0; i--)
            {
                Episode e = new Episode();
                string link = linkList[i];

                e.AddLink(DubbedanimehdNetStreamingSite.NAME, link);
                e.Number = episodes.Count + 1;
                e.Name = "Episode " + e.Number.ToString();
                e.Season = 0;

                episodes.Add(e);
            }
            
            return episodes;
        }
    }
}
