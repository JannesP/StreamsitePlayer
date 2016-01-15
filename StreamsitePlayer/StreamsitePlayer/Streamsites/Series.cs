using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites
{
    public class Series : IEquatable<Series>
    {
        public Series(List<List<Episode>> seasons, string name, string provider, string linkExtension, string episodeOverviewLink)
        {
            this.Seasons = seasons;
            this.Name = name;
            this.Provider = provider;
            this.LinkExtension = linkExtension;
            this.EpisodeOverviewLink = episodeOverviewLink;
        }

        public int Count
        {
            get
            {
                return Seasons.Count;
            }
        }

        public string EpisodeOverviewLink
        {
            get;
            set;
        }

        public string Provider
        {
            get;
            set;
        }

        public List<List<Episode>> Seasons
        {
            get;
            set;
        }

        public List<Episode> this[int season]
        {
            get
            {
                return Seasons[season];
            }
            set
            {
                Seasons[season] = value;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string LinkExtension
        {
            get;
            set;
        }

        public int LastPlayedSeason
        {
            get;
            set;
        }

        public int LastPlayedEpisode
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Series other)
        {
            if (other == null) return false;
            if (other.GetType() != this.GetType()) return false;
            if (this.Count != other.Count) return false;
            if (this.LinkExtension != other.LinkExtension) return false;
            if (this.Name != other.Name) return false;
            if (this.Provider != other.Provider) return false;
            return true;
        }
    }
}
