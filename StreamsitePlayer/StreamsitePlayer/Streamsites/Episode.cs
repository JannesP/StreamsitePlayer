using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites
{
    public class Episode
    {
        Dictionary<string, string> links = new Dictionary<string, string>();

        public Episode() { }

        public Episode(int season, int number, string name)
        {
            Season = season;
            Number = number;
            Name = name;
            PlayLocation = 0L;
        }

        public Episode AddLink(string siteName, string link)
        {
            links.Add(siteName, link);
            return this;
        }

        public string GetLink(string siteName)
        {
            if (links.ContainsKey(siteName))
            {
                return links[siteName];
            }
            else
            {
                return "";
            }
        }

        public Dictionary<string, string> GetAllAvailableLinks()
        {
            return links;
        }
        
        public int Season
        {
            get;
            set;
        }
        
        public int Number
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }

        public long PlayLocation
        {
            get;
            set;
        }

        public override string ToString()
        {
            return "E" + this.Number + " " + this.Name;
        }

    }
}
