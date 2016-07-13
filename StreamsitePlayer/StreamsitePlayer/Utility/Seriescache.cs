using SeriesPlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer
{
    static class Seriescache
    {
        static Seriescache() 
        {
            if (!Directory.Exists(Settings.NOSETTING_CACHE_PATH))
            {
                Directory.CreateDirectory(Settings.NOSETTING_CACHE_PATH);
            }
        }

        public async static void CacheSeriesAsync(Series series)
        {
            string filepath = Path.Combine(Util.GetRalativePath(Settings.NOSETTING_CACHE_PATH), series.Provider + "." + series.LinkExtension + ".series");
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            using (StreamWriter sw = new StreamWriter(File.Create(filepath)))
            {
                await sw.WriteLineAsync("seriesname." + series.Name);
                await sw.WriteLineAsync("provider." + series.Provider);
                await sw.WriteLineAsync("linkExtension." + series.LinkExtension);
                await sw.WriteLineAsync("lastPlayedSeason." + series.LastPlayedSeason);
                await sw.WriteLineAsync("lastPlayedEpisode." + series.LastPlayedEpisode);
                await sw.WriteLineAsync("seriesLink." + series.EpisodeOverviewLink);

                for (int s = 0; s < series.Count; s++)
                {
                    List<Episode> season = series[s];
                    sw.WriteLine("[Season " + (s + 1) + "]"); //[Season s]
                    foreach (Episode episode in season)
                    {
                        await sw.WriteLineAsync("[Episode " + episode.Number + "]"); //[Episode e] 
                        await sw.WriteLineAsync("name." + episode.Name);   //name.whatever
                        await sw.WriteLineAsync("season." + episode.Season);   //season.whatever
                        await sw.WriteLineAsync("episode." + episode.Number);
                        await sw.WriteLineAsync("playLocation." + episode.PlayLocation);
                        Dictionary<string, string> links = episode.GetAllAvailableLinks();
                        for (int i = 0; i < links.Count; i++)
                        {
                            KeyValuePair<string, string> link = links.ElementAt(i);
                            await sw.WriteLineAsync("link." + link.Key + " " + link.Value);
                        }
                    }
                }
            }
        }

        public async static Task<Series> ReadCachedSeriesAsync(string providerName, string fileName)
        {
            string filepath = Path.Combine(Util.GetRalativePath(Settings.NOSETTING_CACHE_PATH), providerName + "." + fileName + ".series");
            if (!File.Exists(filepath)) return null;
            List<List<Episode>> seasons = new List<List<Episode>>();
            string seriesName = fileName;
            string seriesProvider = providerName;
            string linkExtension = fileName;
            string seriesLink = "";
            int lastPlayedEpisode = 1, lastPlayedSeason = 1; 
            using (StreamReader sr = new StreamReader(File.OpenRead(filepath)))
            {
                Episode currEpisode = new Episode();
                while (!sr.EndOfStream)
                {
                    string line = await sr.ReadLineAsync();
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
                    int res;
                    switch (parts[0])
                    {
                        case "name":
                            currEpisode.Name = parts[1];
                            break;
                        case "season":
                            if (int.TryParse(parts[1], out res)) currEpisode.Season = res;
                            break;
                        case "episode":
                            if (int.TryParse(parts[1], out res)) currEpisode.Number = res;
                            break;
                        case "link":
                            string[] hostAndLink = parts[1].Split(new char[] { ' ' }, 2);
                            currEpisode.AddLink(hostAndLink[0], hostAndLink[1]);
                            break;
                        case "seriesLink":
                            seriesLink = parts[1];
                            break;
                        case "seriesname":
                            seriesName = parts[1];
                            break;
                        case "provider":
                            seriesProvider = parts[1];
                            break;
                        case "linkExtension":
                            linkExtension = parts[1];
                            break;
                        case "lastPlayedSeason":
                            if (int.TryParse(parts[1], out res)) lastPlayedSeason = res;
                            break;
                        case "lastPlayedEpisode":
                            if (int.TryParse(parts[1], out res)) lastPlayedEpisode = res;
                            break;
                        case "playLocation":
                            long resL;
                            if (long.TryParse(parts[1], out resL)) currEpisode.PlayLocation = resL;
                            break;
                    }
                }
            }
            Series series = new Series(seasons, seriesName, providerName, linkExtension, seriesLink);
            series.LastPlayedEpisode = lastPlayedEpisode;
            series.LastPlayedSeason = lastPlayedSeason;
            return series;
        }

        public static void RemoveCachedSeries(Series s)
        {
            string filepath = Path.Combine(Util.GetRalativePath(Settings.NOSETTING_CACHE_PATH), s.Provider + "." + s.LinkExtension + ".series");
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        public static List<string> FindCachedSeries()
        {
            if (Directory.Exists(Util.GetRalativePath(Settings.NOSETTING_CACHE_PATH)))
            {
                var files = new List<string>(Directory.GetFiles(Util.GetRalativePath(Settings.NOSETTING_CACHE_PATH)));
                for (int i = 0; i < files.Count; i++) files[i] = Path.GetFileName(files[i]);  //reduce to the file names
                files.RemoveAll(x => !IsValidCacheFileName(x));
                return files;
            }
            return new List<string>(0);
        }

        private static bool IsValidCacheFileName(string name)
        {
            int endingIndex = name.LastIndexOf(".series");
            if (endingIndex != 0
                && (name.Length - ".series".Length == endingIndex))  //.series must be the end of the path
            {
                foreach (string provider in StreamProvider.VALID_PROVIDERS)    //starts with a valid provider
                {
                    if (name.IndexOf(provider) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
