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

        private int season;
        public int Season
        {
            get
            {
                return season;
            }

            set
            {
                season = value;
            }
        }

        private int number;
        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }

        private string name;
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

        public override string ToString()
        {
            return "E" + this.Number + " " + this.Name;
        }

    }
}
