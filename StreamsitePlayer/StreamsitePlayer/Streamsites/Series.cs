using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Streamsites
{
    public class Series
    {
        private List<List<Episode>> seasons;
        private string name;

        public Series(List<List<Episode>> seasons, string name)
        {
            this.seasons = seasons;
            this.Name = name;
        }

        public int Count
        {
            get
            {
                return seasons.Count;
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
    }
}
