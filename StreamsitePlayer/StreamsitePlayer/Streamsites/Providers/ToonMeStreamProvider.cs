﻿using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class ToonMeStreamProvider : StreamProvider
    {
        public const string NAME = "toonme";
        private string[] VALID_SITES = new string[] { ToonMeStreamingSite.NAME }; 

        public override string GetLinkInstructions()
        {
            throw new NotImplementedException();
        }

        public override string GetReadableSiteName()
        {
            return "Toonme (Watchcartoon)";
        }

        public override Dictionary<string, string> GetSearchIndex()
        {
            var index = new Dictionary<string, string>();

            string site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink() + "cartoon-list/");
            index.AddAll(ParseSeriesOverview(site));
            site = Util.RequestSimplifiedHtmlSite(GetWebsiteLink() + "anime-dubbed/");
            index.AddAll(ParseSeriesOverview(site));

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
            return "http://www.toonme.tv/";
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

            string seriesName = page.GetSubstringBetween(0, "<h1 itemprop=\"name\"><img src=\"http://www.toonme.tv/img/star-icon.png\">", "</h1>")
                .Replace(" Anime", "").Replace(" Cartoons", "").Replace(" Info", "").Replace(" English", "").Replace(" Dubbed", "");

            List<List<Episode>> seasons = new List<List<Episode>>();
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

            int startIndex = list.IndexOf("<table id=\"episode-list-entry-tbl\"><tr>");
            if (startIndex == -1) return episodes;
            int endIndex = list.IndexOf("</tr></table>");
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
                if (currentIndex == -1) continue;

                Episode e = new Episode();
                e.Season = 1;
                e.Name = name;
                e.AddLink(ToonMeStreamingSite.NAME, "http://www.toonme.tv" + link);
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
