using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Streamsites.Providers
{
    class BsToStreamProvider : StreamProvider
    {
        public const string NAME = "bs.to";

        public static string StreamcliudStreamingSite { get; private set; }

        private string[] VALID_SITES = { StreamcloudStreamingSite.NAME };

        public override int GetEpisodeCount(int series)
        {
            throw new NotImplementedException();
        }

        public override string GetEpisodeLink(int series, int episode, string siteName)
        {
            throw new NotImplementedException();
        }

        public override List<Episode> GetEpisodeList(int series)
        {
            throw new NotImplementedException();
        }

        public override string GetEpisodeName(int series, int episode)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override string[] GetValidStreamingSites()
        {
            return VALID_SITES;
        }

        public override int LoadSeries(string siteLinkNameExtension)
        {
            throw new NotImplementedException();
        }
    }
}
