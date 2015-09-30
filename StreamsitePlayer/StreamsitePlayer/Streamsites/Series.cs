using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Streamsites
{
    public class Series : IEquatable<Series>
    {
        private List<List<Episode>> seasons;
        private string name;
        private string provider;
        private string linkExtension;

        public Series(List<List<Episode>> seasons, string name, string provider, string linkExtension)
        {
            this.seasons = seasons;
            this.Name = name;
            this.Provider = provider;
            this.LinkExtension = linkExtension;
        }

        public int Count
        {
            get
            {
                return seasons.Count;
            }
        }

        public string Provider
        {
            get
            {
                return provider;
            }
            set
            {
                provider = value;
            }
        }

        public List<List<Episode>> Seasons
        {
            get
            {
                return seasons;
            }
            set
            {
                seasons = value;
            }
        }

        public List<Episode> this[int season]
        {
            get
            {
                return seasons[season];
            }
            set
            {
                seasons[season] = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string LinkExtension
        {
            get
            {
                return linkExtension;
            }

            set
            {
                linkExtension = value;
            }
        }

        public override string ToString()
        {
            return name;
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
