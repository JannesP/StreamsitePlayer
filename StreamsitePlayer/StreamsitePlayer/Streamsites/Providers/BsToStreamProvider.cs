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
        private List<List<Episode>> seasons;

        private string[] VALID_SITES = { StreamcloudStreamingSite.NAME };

        public override int GetEpisodeCount(int season)
        {
            return this.seasons[season - 1].Count;
        }

        public override string GetEpisodeLink(int season, int episode, string siteName)
        {
            return this.seasons[season - 1][episode - 1].GetLink(siteName);
        }

        public override List<Episode> GetEpisodeList(int season)
        {
            return this.seasons[season - 1];
        }

        public override string GetEpisodeName(int season, int episode)
        {
            return this.seasons[season - 1][episode - 1].Name;
        }

        public override string GetLinkInstructions()
        {
            return "http://bs.to/serie/???/...";
        }

        public override string GetReadableSiteName()
        {
            return "bs.to";
        }

        public override int GetSeriesCount()
        {
            return this.seasons.Count;
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkNameExtension, Control threadAnchor)
        {
            seasons = Seriescache.ReadCachedSeries(siteLinkNameExtension);
            if (seasons != null) return StreamProvider.RESULT_USE_CACHED;

            string htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkNameExtension);
            int seriesCount = ScanForSeriesCount(htmlEpisodeOverview, siteLinkNameExtension);
            seasons = new List<List<Episode>>();
            seasons.Add(ExtractEpisodesFromHtml(1, htmlEpisodeOverview, siteLinkNameExtension, threadAnchor));
            for (int i = 2; i <= seriesCount; i++)
            {
                htmlEpisodeOverview = Util.RequestSimplifiedHtmlSite(URL_PRE + siteLinkNameExtension + "/" + i);
                seasons.Add(ExtractEpisodesFromHtml(i, htmlEpisodeOverview, siteLinkNameExtension, threadAnchor));
            }
            Seriescache.CacheSeries(siteLinkNameExtension, seasons);
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
            int index = 0;
            while (index != -1)
            {
                Episode e;
                string searchString = "<td>" + ++episodeNumber + "</td><td><a href=\"serie/" + siteLinkNameExtension + "/" + seasonNumber;
                index = html.IndexOf(searchString, index);
                if (index != -1)
                {
                    e = new Episode(seasonNumber, episodeNumber, Util.GetStringBetween(html, index, "<strong>", "</strong>"));
                }
                else
                {
                    break;
                }
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
            return episodes;
        }

        public override string GetSeriesName()
        {
            throw new NotImplementedException();
        }
    }
}
