using SeriesPlayer.Streamsites.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites
{
    public abstract class StreamProvider
    {
        public const int RESULT_USE_CACHED = 1;
        public const int RESULT_OK = 0;
        public const int RESULT_NET_FAILED = -1;
        public const int RESULT_SERIES_MISSING = -2;

        public static List<string> VALID_PROVIDERS;

        protected Series series;
        protected string siteLinkExtension = "";

        static StreamProvider()
        {
            Logger.Log("START", "Adding streaming providers.");
            VALID_PROVIDERS = new List<string>()
            {
                BsToStreamProvider.NAME,
                DubbedanimehdCoProvider.NAME,
                CartooncrazyStreamProvider.NAME,
                KissAnimeStreamProvider.NAME
            };
#if DEBUG
            VALID_PROVIDERS.Add(TestProvider.NAME);
#endif
        }

        /// <summary>
        /// Creates a new instance of the specified provider.
        /// </summary>
        /// <param name="name">The unique name of the provider [XxxxStreamProvider.NAME]</param>
        /// <returns>A StreamProvider instance.</returns>
        public static StreamProvider Create(string name)
        {
            switch (name)
            {
                case BsToStreamProvider.NAME:
                    return new BsToStreamProvider();
                case TestProvider.NAME:
                    return new TestProvider();
                case DubbedanimehdCoProvider.NAME:
                    return new DubbedanimehdCoProvider();
                case CartooncrazyStreamProvider.NAME:
                    return new CartooncrazyStreamProvider();
                case KissAnimeStreamProvider.NAME:
                    return new KissAnimeStreamProvider();
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Returns the linkExtension which was passed in the constructor.
        /// </summary>
        /// <returns>the link extension</returns>
        public string GetLinkExtension()
        {
            return series.LinkExtension;
        }
        /// <summary>
        /// Initilizes the class and requests all the series data.
        /// </summary>
        /// <param name="siteLinkExtension">Show <see cref="GetLinkInstructions"/> to the user. He should know what it needs!</param>
        public abstract Task<int> LoadSeriesAsync(string siteLinkExtension, Control threadAnchor);
        /// <summary>
        /// Initilizes the class and requests all the series data.
        /// </summary>
        /// <param name="siteLinkExtension">Show <see cref="GetLinkInstructions"/> to the user. He should know what it needs!</param>
        public abstract Task<int> ReloadSeriesAsync(string siteLinkExtension, Control threadAnchor);
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
        public Episode GetEpisode(int season, int episode)
        {
            return this.series[season - 1][episode - 1];
        }
        /// <summary>
        /// Requests the count of seasons of the series on the StreamProvider.
        /// </summary>
        /// <returns>the count of series</returns>
        public int GetSeasonCount()
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
        /// <summary>
        /// Returns the browsable link to the provider. Usually the search page or something.
        /// </summary>
        /// <returns>The website link.</returns>
        public abstract string GetWebsiteLink();

        public Series GetSeries()
        {
            return series;
        }

        public enum SearchMode
        {
            REMOTE, LOCAL
        }

        public abstract SearchMode SupportedSearchMode {
            get;
        }

        /// <summary>
        /// Returns the search result.
        /// </summary>
        /// <returns>Dictionary with names as keys and linkExtensions as values</returns>
        public abstract Task<Dictionary<string, string>> RequestRemoteSearchAsync(string keyword, CancellationToken ct);

        /// <summary>
        /// Returns the complete search index for the streamprovider regardless of the sites available.
        /// </summary>
        /// <returns>Dictionary with names as keys and linkExtensions as values</returns>
        public abstract Task<Dictionary<string, string>> RequestSearchIndexAsync(CancellationToken ct);
    }
}
