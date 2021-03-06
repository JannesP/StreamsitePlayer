﻿using SeriesPlayer.Streamsites.Sites;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Providers
{
    class DubbedanimehdCoProvider : StreamProvider
    {
        public const string NAME = "dubbedanimehd";
        private const string URL_PRE = "http://www.dubbedanimehd.co/watch/";

        private string[] VALID_SITES = { DubbedanimehdTvStreamingSite.NAME };

        public override SearchMode SupportedSearchMode
        {
            get
            {
                return SearchMode.LOCAL;
            }
        }

        public override string GetReadableSiteName()
        {
            return "dubbedanimehd.co";
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public async override Task<int> LoadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            base.series = await Seriescache.ReadCachedSeriesAsync(NAME, siteLinkExtension);
            base.siteLinkExtension = siteLinkExtension;
            if (base.series != null) return StreamProvider.RESULT_USE_CACHED;
            return await ReloadSeriesAsync(siteLinkExtension, threadAnchor);
            
        }

        public async override Task<int> ReloadSeriesAsync(string siteLinkExtension, Control threadAnchor)
        {
            string htmlEpisodeOverview = await Util.RequestSimplifiedHtmlSiteAsync(URL_PRE + siteLinkExtension);
            List<List<Episode>> seasons = new List<List<Episode>>();
            seasons.Add(ScanForEpisodes(htmlEpisodeOverview, siteLinkExtension));
            string seriesName = htmlEpisodeOverview.GetSubstringBetween(0, "wp-post-image\" alt=\"", "\" />");
            base.series = new Series(seasons, seriesName, NAME, siteLinkExtension, URL_PRE + siteLinkExtension);
            await Seriescache.CacheSeriesAsync(base.series);
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
            return "http://www.dubbedanimehd.co/dubbed-anime";
        }

        public override Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async override Task<Dictionary<string, string>> RequestSearchIndexAsync(CancellationToken ct)
        {
            var index = new Dictionary<string, string>();
            string site = await Util.RequestSimplifiedHtmlSiteAsync(GetWebsiteLink());
            ct.ThrowIfCancellationRequested();

            const string SERIES_SEARCH = "<li><a href='http://www.dubbedanimehd.co/watch/";
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
            ct.ThrowIfCancellationRequested();
            return index;
        }
    }
}
