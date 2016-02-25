using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class BsToStreamProvider : StreamProvider
    {
        public const string NAME = "bs.to";
        private const string URL_PRE = "http://bs.to/serie/";
        private const string STREAMCLOUD_SEARCH = "<a class=\"icon Streamcloud\" title=\"Streamcloud\"   href=\"";
        private const string VIVO_SEARCH = "<a class=\"icon Vivo\" title=\"Vivo\"   href=\"";

        private readonly string[] VALID_SITES = { BsToStreamcloudStreamingSite.NAME, BsToVivoStreamingSite.NAME, StreamcloudStreamingSite.NAME };

        public override string GetLinkInstructions()
        {
            return "http://bs.to/serie/???/...";
        }

        public override string GetReadableSiteName()
        {
            return "bs.to";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkExtension, Control threadAnchor)
        {
            series = Seriescache.ReadCachedSeries(NAME, siteLinkExtension);
            base.siteLinkExtension = siteLinkExtension;
            if (series != null) return StreamProvider.RESULT_USE_CACHED;

            string htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkExtension);
            int seriesCount = ScanForSeasonCount(htmlEpisodeOverview, siteLinkExtension);
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ExtractEpisodesFromHtml(1, htmlEpisodeOverview, siteLinkExtension, threadAnchor));
            for (int i = 2; i <= seriesCount; i++)
            {
                htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkExtension + "/" + i);
                seasons.Add(ExtractEpisodesFromHtml(i, htmlEpisodeOverview, siteLinkExtension, threadAnchor));
            }
            string seriesName = htmlEpisodeOverview.GetSubstringBetween(0, "<h2>", "<");
            series = new Series(seasons, seriesName, NAME, siteLinkExtension, URL_PRE + siteLinkExtension);
            Seriescache.CacheSeries(series);
            FormMain.SeriesOpenCallback(null);
            return StreamProvider.RESULT_OK;
        }

        private static int ScanForSeasonCount(string html, string siteLinkNameExtension)
        {
            int seriesCount = 0;
            int index = 0;
            while (index != -1)
            {
                string searchString = "serie/" + siteLinkNameExtension + "/" + ++seriesCount + "\">" + seriesCount + "</a>";
                index = html.IndexOf(searchString, index);
            }
            return seriesCount - 1;
        }

        private static List<Episode> ExtractEpisodesFromHtml(int seasonNumber, string html, string siteLinkNameExtension, Control threadAnchor)
        {
            var episodes = new List<Episode>();
            int episodeNumber = 0;
            int episodeIndex = 0;
            var episodeIndices = new List<int>();
            Application.DoEvents();
            //Scan for all episode indices.
            do
            {
                string searchString = "<td>" + ++episodeNumber + "</td><td><a href=\"serie/" + siteLinkNameExtension + "/" + seasonNumber;
                episodeIndex = html.IndexOf(searchString, episodeIndex);
                if (episodeIndex != -1)
                {
                    episodeIndices.Add(episodeIndex);
                }
            } while (episodeIndex != -1);

            //Scan for episodes
            for (int i = 0; i < episodeIndices.Count; i++)
            {
                episodeIndex = episodeIndices[i];
                Episode e = null;
                int index = episodeIndex;
                string name = html.GetSubstringBetween(index, "<strong>", "</strong>", out index);
                if (name == "" || i == episodeIndices.Count - 1 || index > episodeIndices[i + 1])
                {
                    index = episodeIndex;
                    name = "";
                }
                string nameExt = html.GetSubstringBetween(index, "<span lang=en\">", "</span>", out index);
                if (nameExt == "" || i == episodeIndices.Count - 1 || index > episodeIndices[i + 1])
                {
                    index = episodeIndex;
                    nameExt = "";
                }
                else
                {
                    name += " (" + nameExt + ")";
                }
                e = new Episode(seasonNumber, i + 1, name);

                int indexVivo = html.IndexOf(VIVO_SEARCH, index);
                if (indexVivo != -1)    //check if a vivo link is found
                {
                    if (!(i + 1 < episodeIndices.Count) || ((i + 1 < episodeIndices.Count) && (indexVivo < episodeIndices[i + 1])))  //check if the streamcloud link is before the next episode.
                    {
                        string vivoSite = "http://bs.to/" + html.GetSubstringBetween(indexVivo, VIVO_SEARCH, "\"");
                        e.AddLink(BsToVivoStreamingSite.NAME, vivoSite);
                    }
                }

                int indexStreamcloud = html.IndexOf(STREAMCLOUD_SEARCH, index);
                if (indexStreamcloud != -1)    //check if a streamcloud link is found
                {
                    if (!(i + 1 < episodeIndices.Count) || ((i + 1 < episodeIndices.Count) && (indexStreamcloud < episodeIndices[i + 1])))  //check if the streamcloud link is before the next episode.
                    {
                        string streamcloudSite = "http://bs.to/" + html.GetSubstringBetween(indexStreamcloud, STREAMCLOUD_SEARCH, "\"");
                        e.AddLink(BsToStreamcloudStreamingSite.NAME, streamcloudSite);
                    }
                }
                
                threadAnchor.Invoke((MethodInvoker)(() => FormMain.SeriesOpenCallback(e)));
                episodes.Add(e);
            }
            return episodes;
        }

        public override string GetWebsiteLink()
        {
            return "http://bs.to/andere-serien";
        }

        public override bool IsSearchSupported()
        {
            return true;
        }

        public override Dictionary<string, string> GetSearchIndex()
        {
            var index = new Dictionary<string, string>();
            string site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink());

            const string SERIES_SEARCH = "<li><a href=\"serie/";
            const string END_LINK = "\">";
            const string END_NAME = "</a></li>";

            int searchIndex = site.IndexOf("<div id=\"seriesContainer\">");
            while (searchIndex != -1)
            {
                string seriesExtension = site.GetSubstringBetween(searchIndex, SERIES_SEARCH, END_LINK, out searchIndex);
                if (searchIndex == -1) continue;
                string name = site.GetSubstringBetween(searchIndex, END_LINK, END_NAME, out searchIndex);
                if (searchIndex != -1)
                {
                    index.Add(name, seriesExtension);
                }
            }

            return index;
        }
    }
}
