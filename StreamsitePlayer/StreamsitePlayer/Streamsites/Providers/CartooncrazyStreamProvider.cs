using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class CartooncrazyStreamProvider : StreamProvider
    {
        public const string NAME = "toonme"; //got renamed to cartooncrazy
        private string[] VALID_SITES = new string[] { CartooncrazyStreamingSite.NAME };

        public override string GetLinkInstructions()
        {
            throw new NotImplementedException();
        }

        public override string GetReadableSiteName()
        {
            return "cartooncrazy.net";
        }

        public override Dictionary<string, string> GetSearchIndex()
        {
            var index = new Dictionary<string, string>();

            string site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink() + "cartoon-list/");
            index.AddAll(ParseSeriesOverview(site));
            int nextIndex = site.IndexOf("next page-numbers");
            while (nextIndex != -1)
            {
                Application.DoEvents();
                string nextPage = site.GetSubstringBetween(nextIndex, "href=\"", "\"");
                site = Util.RequestSimplifiedHtmlSite(nextPage);
                index.AddAll(ParseSeriesOverview(site));
                nextIndex = site.IndexOf("next page-numbers");
            }
            site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink() + "anime-dubbed/");
            index.AddAll(ParseSeriesOverview(site));
            nextIndex = site.IndexOf("next page-numbers");
            while (nextIndex != -1)
            {
                Application.DoEvents();
                string nextPage = site.GetSubstringBetween(nextIndex, "href=\"", "\"");
                site = Util.RequestSimplifiedHtmlSite(nextPage);
                index.AddAll(ParseSeriesOverview(site));
                nextIndex = site.IndexOf("next page-numbers");
            }

            return index;
        }

        private Dictionary<string, string> ParseSeriesOverview(string pagesource)
        {
            string seriesLinkStart = "<a href=\"";
            string seriesLinkEnd = "\" rel=\"tag\">";
            string seriesNameEnd = " <span class=";

            var index = new Dictionary<string, string>();

            int startIndex = pagesource.IndexOf("<div class=\"multi-column-taxonomy-list\"><ul class=\"multi-column-1\">");
            if (startIndex == -1) return index;
            int endIndex = pagesource.IndexOf("</li></ul></div>", startIndex);
            if (endIndex == -1) return index;

            int currentIndex = startIndex;
            while (currentIndex != -1 && currentIndex < endIndex)
            {
                string link = pagesource.GetSubstringBetween(currentIndex, seriesLinkStart, seriesLinkEnd, out currentIndex);
                if (currentIndex == -1) break;
                string name = pagesource.GetSubstringBetween(currentIndex, seriesLinkEnd, seriesNameEnd, out currentIndex);
                if (!index.ContainsKey(name))
                {
                    index.Add(name, link);
                }
            }

            return index;
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override string GetWebsiteLink()
        {
            return "http://www.cartooncrazy.net/";
        }

        public override bool IsSearchSupported()
        {
            return true;
        }

        public override int LoadSeries(string siteLinkExtension, Control threadAnchor)
        {
            siteLinkExtension = siteLinkExtension.Replace("&001", "/anime/").Replace("&002", "/cartoon/");
            base.siteLinkExtension = siteLinkExtension;
            base.series = Seriescache.ReadCachedSeries(NAME, siteLinkExtension.Replace("/anime/", "&001").Replace("/cartoon/", "&002").Replace("/", ""));
            if (base.series != null) return StreamProvider.RESULT_USE_CACHED;

            int result = StreamProvider.RESULT_OK;
            string seriesUrl = GetWebsiteLink().Remove(GetWebsiteLink().Length - 1) + siteLinkExtension;
            string page = Util.RequestSimplifiedHtmlSite(seriesUrl);
            if (page == "") return StreamProvider.RESULT_SERIES_MISSING;

            string seriesName = page.GetSubstringBetween(0, "<img src=\"http://www.cartooncrazy.me/img/star-icon.png\"></noscript>", "</h1>");
            if (seriesName == "")
            {
                seriesName = page.GetSubstringBetween(0, "<img src=\"http://www.cartooncrazy.me/img/star-icon.png\">", "</h1>");
            }
            seriesName = seriesName
                .Replace(" Anime", "").Replace(" Cartoons", "").Replace(" Info", "")
                .Replace(" English", "").Replace(" Dubbed", "").Replace(" at cartooncrazy.me", "")
                .Replace(" Episodes", "");

            List <List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ScanForEpisodes(page, seriesName));
            base.series = new Series(seasons, seriesName, NAME, siteLinkExtension.Replace("/anime/", "&001").Replace("/cartoon/", "&002").Replace("/", ""), seriesUrl);
            Seriescache.CacheSeries(base.series);
            FormMain.SeriesOpenCallback(null);

            return result;
        }

        private static List<Episode> ScanForEpisodes(string html, string seriesName)
        {
            List<Episode> episodes = new List<Episode>();

            string list = Util.RequestSimplifiedHtmlSite(html.GetSubstringBetween(0, "$(\"#load\").load('", "')"));

            int startIndex = list.IndexOf("<table id=\"episode-list-entry-tbl\">");
            if (startIndex == -1) return episodes;
            int endIndex = list.IndexOf("</table>");
            if (endIndex == -1) return episodes;

            string linkStart = "<a href=\"";
            string linkEnd = "\" title=\"";
            string nameStart = "<h2>";
            string nameEnd = "</h2>";

            int currentIndex = startIndex;
            while (currentIndex != -1 && currentIndex < endIndex)
            {
                string link = list.GetSubstringBetween(currentIndex, linkStart, linkEnd, out currentIndex);
                if (currentIndex == -1) continue; 
                string name = list.GetSubstringBetween(currentIndex, nameStart, nameEnd, out currentIndex)
                    .Replace(seriesName + " ", "")
                    .Replace(" English Dubbed", "");
                name = name.Trim();
                if (currentIndex == -1) continue;

                Episode e = new Episode();
                e.Season = 1;
                e.Name = name;
                e.AddLink(CartooncrazyStreamingSite.NAME, "http://www.cartooncrazy.net" + link);
                episodes.Add(e);
            }

            var orderedList = new List<Episode>(episodes.Count);
            for (int i = (episodes.Count - 1); i >= 0; i--)
            {
                episodes[i].Number = episodes.Count - i;
                orderedList.Add(episodes[i]);
            }

            return orderedList;
        }
    }
}
