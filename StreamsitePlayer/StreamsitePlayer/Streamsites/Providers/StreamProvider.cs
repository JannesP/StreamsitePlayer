﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites
{
    public abstract class StreamProvider
    {
        public const int RESULT_USE_CACHED = 1;
        public const int RESULT_OK = 0;
        public const int RESULT_NET_FAILED = -1;
        public const int RESULT_SERIES_MISSING = -2;

        protected Series series;
        /// <summary>
        /// Initilizes the class and requests all the series data.
        /// </summary>
        /// <param name="siteLinkNameExtension">Show <see cref="GetLinkInstructions"/> to the user. He should know what it needs!</param>
        public abstract int LoadSeries(string siteLinkNameExtension, Control threadAnchor);
        /// <summary>
        /// Requests the cound of episodes in the given series.
        /// </summary>
        /// <param name="season">the number of the series !1-based!</param>
        /// <returns>the number of episodes</returns>
        public int GetEpisodeCount(int season)
        {
            return this.series[season - 1].Count;
        }
        /// <summary>
        /// Requests an a list of all episodes contained in the series.
        /// </summary>
        /// <param name="season">the number of the series !1-based!</param>
        /// <returns>a list of all episodes included in the series</returns>
        public List<Episode> GetEpisodeList(int season)
        {
            return this.series[season - 1];
        }
        /// <summary>
        /// Requests the count of series of the series on the StreamProvider.
        /// </summary>
        /// <returns>the count of series</returns>
        public int GetSeriesCount()
        {
            return this.series.Count;
        }
        /// <summary>
        /// Request the episode name from the StreamProvider.
        /// Note: for many request just use <see cref="GetEpisodeList(int)"/>
        /// </summary>
        /// <param name="season">1-based number of the series</param>
        /// <param name="episode">1-based number of the episode</param>
        /// <returns>the episode name</returns>
        public string GetEpisodeName(int season, int episode)
        {
            return this.series[season - 1][episode - 1].Name;
        }
        /// <summary>
        /// Request the episode name from the StreamProvider.
        /// Note: for many request just use <see cref="GetEpisodeList(int)"/>
        /// </summary>
        /// <param name="season">1-based number of the series</param>
        /// <param name="episode">1-based number of the episode</param>
        /// <param name="siteName">the provider name const StreamingSite.Site.???</param>
        /// <returns>the episode link</returns>
        public string GetEpisodeLink(int season, int episode, string siteName)
        {
            return this.series[season - 1][episode - 1].GetLink(siteName);
        }
        /// <summary>
        /// Request the site name which is displayed to the user.
        /// </summary>
        /// <returns>readable site name</returns>
        public abstract string GetReadableSiteName();
        /// <summary>
        /// Returns a string which is an explanation for the user which part of the series link he has to provide.
        /// </summary>
        /// <returns>an instruction for the user</returns>
        public abstract string GetLinkInstructions();
        /// <summary>
        /// Returns all valid streaming sites which are supported on this site.
        /// </summary>
        /// <returns>a list of supported sites</returns>
        public abstract string[] GetValidStreamingSites();
        /// <summary>
        /// Returns the series name which can be displayed.
        /// </summary>
        /// <returns>the name of the series</returns>
        public string GetSeriesName()
        {
            return series.Name;
        }

    }
}
