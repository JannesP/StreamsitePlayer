using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Streamsites
{
    class Episode
    {
        Dictionary<string, string> links = new Dictionary<string, string>();

        public Episode(int series, int number, string name)
        {
            Series = series;
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
            return links[siteName];
        }

        private int series;
        public int Series
        {
            get
            {
                return series;
            }

            private set
            {
                series = value;
            }
        }

        private int number;
        public int Number
        {
            get
            {
                return number;
            }

            private set
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

            private set
            {
                name = value;
            }
        }

    }
}
