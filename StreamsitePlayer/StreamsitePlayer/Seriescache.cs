using StreamsitePlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer
{
    static class Seriescache
    {
        private const string CACHE_PATH = "cache/";

        static Seriescache() 
        {
            if (!Directory.Exists(@CACHE_PATH))
            {
                Directory.CreateDirectory(@CACHE_PATH);
            }
        }

        public static void CacheSeries(string name, List<List<Episode>> seasons)
        {
            string filepath = CACHE_PATH + name + ".series";
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (StreamWriter sw = new StreamWriter(File.Create(filepath)))
            {
                sw.WriteLine("name." + name);

                for (int s = 0; s < seasons.Count; s++)
                {
                    List<Episode> season = seasons[s];
                    sw.WriteLine("[Season " + (s + 1) + "]"); //[Season s]
                    foreach (Episode episode in season)
                    {
                        sw.WriteLine("[Episode " + episode.Number + "]"); //[Episode e] 
                        sw.WriteLine("name." + episode.Name);   //name.whatever
                        sw.WriteLine("season." + episode.Season);   //season.whatever
                        sw.WriteLine("episode." + episode.Number);
                        Dictionary<string, string> links = episode.GetAllAvailableLinks();
                        for (int i = 0; i < links.Count; i++)
                        {
                            KeyValuePair<string, string> link = links.ElementAt(i);
                            sw.WriteLine("link." + link.Key + " " + link.Value);
                        }
                    }
                }
            }
        }

        public static List<List<Episode>> ReadCachedSeries(string name)
        {
            string filepath = CACHE_PATH + name + ".series";
            if (!File.Exists(filepath)) return null;
            List<List<Episode>> seasons = new List<List<Episode>>();

            using (StreamReader sr = new StreamReader(File.OpenRead(filepath)))
            {
                Episode currEpisode = new Episode();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains("[Season "))
                    {
                        seasons.Add(new List<Episode>());
                        continue;
                    }
                    else if (line.Contains("[Episode "))
                    {
                        seasons[seasons.Count - 1].Add(new Episode());
                        currEpisode = seasons[seasons.Count - 1][seasons[seasons.Count - 1].Count - 1]; //seasons[last][last]
                        continue;
                    }
                    string[] parts = line.Split(new char[]{ '.' }, 2);
                    if (parts.Length != 2) return null;
                    switch (parts[0])
                    {
                        case "name":
                            currEpisode.Name = parts[1];
                            break;
                        case "season":
                            currEpisode.Season = int.Parse(parts[1]);
                            break;
                        case "episode":
                            currEpisode.Number = int.Parse(parts[1]);
                            break;
                        case "link":
                            string[] hostAndLink = parts[1].Split(new char[] { ' ' }, 2);
                            currEpisode.AddLink(hostAndLink[0], hostAndLink[1]);
                            break;
                    }
                }
            }

            



            return seasons;
        }

    }
}
