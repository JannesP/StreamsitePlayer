using SeriesPlayer.Networking;
using SeriesPlayer.Utility.Extensions;
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

        public override string ToString()
        {
            return "E" + this.Number + " " + this.Name;
        }


        public byte[] GetByteData()
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(Name);
            byte[] numberBytes = Number.ToByteArray();
            byte[] seasonBytes = Season.ToByteArray();

            int arraySize = nameBytes.Length + numberBytes.Length + seasonBytes.Length;
            if (arraySize > TcpServer.MSG_MAX_LENGTH)
            {
                byte[] buffer = new byte[TcpServer.MSG_MAX_LENGTH - numberBytes.Length - seasonBytes.Length];
                Array.Copy(nameBytes, buffer, buffer.Length);
                nameBytes = buffer;
                arraySize = TcpServer.MSG_MAX_LENGTH;
            }

            byte[] byteData = new byte[arraySize];
            Array.Copy(seasonBytes, byteData, seasonBytes.Length);
            Array.Copy(numberBytes, 0, byteData, 4, numberBytes.Length);
            Array.Copy(nameBytes, 0, byteData, 8, nameBytes.Length);
            return byteData;
        }
    }
}
