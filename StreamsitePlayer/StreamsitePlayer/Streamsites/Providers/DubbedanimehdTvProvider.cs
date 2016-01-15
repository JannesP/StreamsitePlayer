using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class DubbedanimehdTvProvider : StreamProvider
    {
        public const string NAME = "dubbedanimehd";
        private const string URL_PRE = "http://www.dubbedanimehd.tv/watch/";

        private string[] VALID_SITES = { DubbedanimehdTvStreamingSite.NAME };

        public override string GetLinkInstructions()
        {
            return "http://www.dubbedanimehd.tv/watch/???/";
        }

        public override string GetReadableSiteName()
        {
            return "dubbedanimehd.tv";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkExtension, Control threadAnchor)
        {
            base.series = Seriescache.ReadCachedSeries(NAME, siteLinkExtension);
            base.siteLinkExtension = siteLinkExtension;
            if (base.series != null) return StreamProvider.RESULT_USE_CACHED;

            string htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkExtension);
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ScanForEpisodes(htmlEpisodeOverview, siteLinkExtension));
            string seriesName = htmlEpisodeOverview.GetSubstringBetween(0, "wp-post-image\" alt=\"", "\" />");
            base.series = new Series(seasons, seriesName, NAME, siteLinkExtension, URL_PRE + siteLinkExtension);
            Seriescache.CacheSeries(base.series);
            FormMain.SeriesOpenCallback(null);
            return StreamProvider.RESULT_OK;
        }

        private static List<Episode> ScanForEpisodes(string html, string siteLinkNameExtension)
        {
            List<Episode> episodes = new List<Episode>();
            html = html.GetSubstringBetween(0, "<div style=\"font-size:14px;\">", "</div>");
                
            List<string> linkList = new List<string>(html.Split('"'));
            for (int i = 0; i < linkList.Count; i++)
            {
                if (!linkList[i].Contains("http")) linkList.RemoveAt(i--);
            }
            for (int i = (linkList.Count - 1); i >= 0; i--)
            {
                Episode e = new Episode();
                string link = linkList[i];

                e.AddLink(DubbedanimehdTvStreamingSite.NAME, link);
                e.Number = episodes.Count + 1;
                e.Name = "Episode " + e.Number.ToString();
                e.Season = 0;

                episodes.Add(e);
                FormMain.SeriesOpenCallback(e);
            }
            
            return episodes;
        }

        public override string GetWebsiteLink()
        {
            return "http://www.dubbedanimehd.tv/dubbed-anime";
        }

        public override bool IsSearchSupported()
        {
            return true;
        }

        public override Dictionary<string, string> GetSearchIndex()
        {
            var index = new Dictionary<string, string>();
            string site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink());

            const string SERIES_SEARCH = "<li><a href='http://www.dubbedanimehd.tv/watch/";
            const string END_LINK = "' title='";
            const string BEGINNING_NAME = "'>";
            const string END_NAME = "</a></li>";

            int searchIndex = site.IndexOf("<div id=\"tab1\" class=\"tab\">");
            while (searchIndex != -1)
            {
                string seriesExtension = site.GetSubstringBetween(searchIndex, SERIES_SEARCH, END_LINK, out searchIndex);
                if (searchIndex == -1) continue;
                string nameOnSite = site.GetSubstringBetween(searchIndex, BEGINNING_NAME, END_NAME, out searchIndex);
                if (searchIndex != -1)
                {
                    string name = nameOnSite;
                    int rescueIndex = 1;
                    while (index.ContainsKey(name))
                    {
                        name = nameOnSite + "(" + rescueIndex++ + ")";
                    }
                    index.Add(name, seriesExtension);
                }
            }

            return index;
        }
    }
}
