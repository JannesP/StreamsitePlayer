using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Providers
{
    class BsToStreamProvider : StreamProvider
    {
        public const string NAME = "bs.to";
        private const string URL_PRE = "http://bs.to/serie/";
        private const string STREAMCLOUD_SEARCH = "<a class=\"icon Streamcloud\" title=\"Streamcloud\"   href=\"";

        private readonly string[] VALID_SITES = { StreamcloudStreamingSite.NAME };

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
            int seriesCount = ScanForSeriesCount(htmlEpisodeOverview, siteLinkExtension);
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ExtractEpisodesFromHtml(1, htmlEpisodeOverview, siteLinkExtension, threadAnchor));
            for (int i = 2; i <= seriesCount; i++)
            {
                htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkExtension + "/" + i);
                seasons.Add(ExtractEpisodesFromHtml(i, htmlEpisodeOverview, siteLinkExtension, threadAnchor));
            }
            string seriesName = Util.GetStringBetween(htmlEpisodeOverview, 0, "<h2>", "<");
            series = new Series(seasons, seriesName);
            Seriescache.CacheSeries(NAME, siteLinkExtension, series);
            FormMain.SeriesOpenCallback(null);
            return StreamProvider.RESULT_OK;
        }

        private static int ScanForSeriesCount(string html, string siteLinkNameExtension)
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
            List<Episode> episodes = new List<Episode>();
            int episodeNumber = 0;
            int episodeIndex = 0;
            while (episodeIndex != -1)
            {
                Episode e;
                string searchString = "<td>" + ++episodeNumber + "</td><td><a href=\"serie/" + siteLinkNameExtension + "/" + seasonNumber;
                episodeIndex = html.IndexOf(searchString, episodeIndex);
                int index = episodeIndex;
                if (episodeIndex != -1)
                {
                    string name = Util.GetStringBetween(html, index, "<strong>", "</strong>");
                    if (name != "")
                    {
                        index = html.IndexOf("<strong>", index);
                    }
                    name += " (" + Util.GetStringBetween(html, index, "\">", "</span>") + ")";
                    e = new Episode(seasonNumber, episodeNumber, name);

                    index = html.IndexOf(STREAMCLOUD_SEARCH, index);
                    if (index != -1)
                    {
                        string streamcloudSite = "http://bs.to/" + Util.GetStringBetween(html, index, STREAMCLOUD_SEARCH, "\"");
                        streamcloudSite = Util.RequestSimplifiedHtmlSite(streamcloudSite);
                        streamcloudSite = "http://streamcloud.eu/" + Util.GetStringBetween(streamcloudSite, 0, "<a href=\"http://streamcloud.eu/", "\"");
                        e.AddLink(StreamcloudStreamingSite.NAME, streamcloudSite);
                        threadAnchor.Invoke((MethodInvoker)(() => FormMain.SeriesOpenCallback(e)));
                        Application.DoEvents();
                    }
                    episodes.Add(e);
                }
            }
            return episodes;
        }

        public override string GetWebsiteLink()
        {
            return "http://bs.to/andere-serien";
        }
    }
}
