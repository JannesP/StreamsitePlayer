using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeriesPlayer
{
    public static class Settings
    {
        public const string NOSETTING_CACHE_PATH = "cache";

        public const string AUTOPLAY = "autoplay";
        public const string SKIP_END = "skipEnd";
        public const string SKIP_BEGINNING = "skipBeginning";
        public const string LAST_SERIES = "lastSeries";
        public const string LAST_PROVIDER = "lastProvider";
        public const string LAST_PLAYED_SERIES = "lastPlayedSeries";
        public const string LAST_PLAYED_PROVIDER = "lastPlayedProvider";
        public const string LAST_PLAYED_EPISODE = "lastPlayedEpisode";
        public const string LAST_PLAYED_SEASON = "lastPlayedSeason";
        public const string JW_KEY = "jwKey";
        public const string VOLUME = "volume";
        public const string MUTED = "muted";
        public const string AUTOCHECK_FOR_UPDATES = "updateCheckAtStartup";
        public const string REMOTE_CONTROL_PORT = "remoteControlPort";
        public const string REMOTE_CONTROL_ACTIVATED = "remoteControlActive";
        public const string REMEMBER_PLAY_LOCATION = "rememberPlayLocation";

        private static class Defaults
        {
            private static Dictionary<string, string> defaults = new Dictionary<string, string>();

            static Defaults()
            {
                defaults.Add(AUTOPLAY, "true");
                defaults.Add(SKIP_END, "0");
                defaults.Add(SKIP_BEGINNING, "0");
                defaults.Add(LAST_SERIES, "");
                defaults.Add(LAST_PROVIDER, "0");
                defaults.Add(JW_KEY, "brtm4UPVPkavg6wYzjRl3RufI+x0oquEsWZ8RA==");
                defaults.Add(LAST_PLAYED_EPISODE, "-1");
                defaults.Add(LAST_PLAYED_SEASON, "-1");
                defaults.Add(LAST_PLAYED_SERIES, "");
                defaults.Add(LAST_PLAYED_PROVIDER, "0");
                defaults.Add(VOLUME, "100");
                defaults.Add(MUTED, "false");
                defaults.Add(AUTOCHECK_FOR_UPDATES, "true");
                defaults.Add(REMOTE_CONTROL_PORT, "8003");
                defaults.Add(REMOTE_CONTROL_ACTIVATED, "true");
                defaults.Add(REMEMBER_PLAY_LOCATION, "true");
            }

            public static string GetValue(string key)
            {
                string returnValue = "";
                bool foundVal = defaults.TryGetValue(key, out returnValue);
                if (foundVal)
                {
                    return returnValue;
                }
                else
                {
                    throw new MissingFieldException("The requested key: " + key + " is missing in the default config!");
                }
            }

        }

        private const string FILE_NAME = "settings.ini";


        static Settings()
        {
            Logger.Log("SETTINGS", "Checking if settings are present.");
            if (!File.Exists(FILE_NAME))
            {
                Logger.Log("SETTINGS", "The settings weren't present. Creating a new one!");
                File.Create(FILE_NAME).Close();
            }
            LoadFileSettings();
        }

        private static void LoadFileSettings()
        {
            string[] fileLines = File.ReadAllLines(FILE_NAME, Encoding.UTF8);
            Logger.Log("SETTINGS", "Found " + fileLines.Length + " lines in the settings file, parsing ...");
            foreach (string line in fileLines)
            {
                string[] splitResult = line.Split(new char[] { '=' }, 2);
                if (splitResult.Length != 2)
                {
                    Logger.Log("SETTINGS", "Found invalid line: " + line);
                    continue;
                }
                else
                {
                    WriteValue(splitResult[0], splitResult[1]);
                    Logger.Log("SETTINGS", "Loaded key-value-pair from file: KEY-" + splitResult[0] + " | VALUE-" + splitResult[1]);
                }
            }
        }

        public static void SaveFileSettings()
        {
            Logger.Log("FILEIO", "Generating savefile lines ...");
            long startTime = DateTime.Now.Ticks;
            string[] lines = GetSaveFileLines();
            Logger.Log("FILEIO", "Creating IO thread ...");
            Thread savingThread = new Thread(new ThreadStart(() =>
            {
                File.WriteAllLines(FILE_NAME, lines, Encoding.UTF8);
                double msTaken = (double)(DateTime.Now.Ticks - startTime) / 10000d;
                Logger.Log("FILEIO", "Done saving settings async after " + msTaken + "ms!");
            }));
            savingThread.Start();
        }


        private static Dictionary<string, string> settings = new Dictionary<string, string>();

        public static void WriteValue(string key, int value)
        {
            WriteValue(key, value.ToString());
        }

        public static void WriteValue(string key, bool value)
        {
            WriteValue(key, value.ToString());
        }

        public static void WriteValue(string key, string value)
        {
            Logger.Log("SETTINGS", "Writing the value: " + value + " for the key: " + key);
            if (settings.ContainsKey(key))
            {
                if (!settings[key].Equals(value))
                {
                    settings[key] = value;
                }
            }
            else
            {
                settings.Add(key, value);
            }
        }

        public static bool GetBool(string key)
        {
            bool res = false;
            bool succeeded = bool.TryParse(GetString(key), out res);
            if (succeeded)
            {
                return res;
            }
            else
            {
                RestoreDefault(key);
                return GetBool(key);
            }
        }

        public static string GetString(string key)
        {
            string result = "";
            bool foundVal = settings.TryGetValue(key, out result);
            if (foundVal)
            {
                return result;
            }
            else
            {
                settings.Add(key, Defaults.GetValue(key));
                return GetString(key);
            }
        }

        public static int GetNumber(string key)
        {
            int res = -1;
            bool succeeded = int.TryParse(GetString(key), out res);
            if (succeeded)
            {
                return res;
            }
            else
            {
                RestoreDefault(key);
                return GetNumber(key);
            }
        }

        public static void RestoreDefault(string key)
        {
            WriteValue(key, Defaults.GetValue(key));
        }

        public static string[] GetSaveFileLines()
        {
            string[] result = new string[settings.Count];
            for (int i = 0; i < settings.Count; i++)
            {
                KeyValuePair<string, string> kvp = settings.ElementAt(i);
                result[i] = kvp.Key + "=" + kvp.Value;
                Logger.Log("SETTINGS", "Created savefile line: " + result[i]);
            }

            return result;
        }
    }
}
