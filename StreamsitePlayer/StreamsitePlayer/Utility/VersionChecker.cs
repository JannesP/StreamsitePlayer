using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Utility
{
    static class VersionChecker
    {
        public const string REMOTE_VERSION_PATH = "http://85.214.249.105/streamplayer/newest.ver";
        public const string REMOTE_CHANGELOG_PATH = "http://85.214.249.105/streamplayer/newest.changelog";

        public delegate void OnVersionCheckedEventHandler(VersionCheckedEventArgs e);
        public static event OnVersionCheckedEventHandler VersionChecked;
        public class VersionCheckedEventArgs : EventArgs
        {
            private string changelog, newVersion;
            private bool updateRequired, errorOccured;
            public VersionCheckedEventArgs(string changelog, string newVersion, bool updateRequired, bool errorOccured)
            {
                this.changelog = changelog;
                this.newVersion = newVersion;
                this.updateRequired = updateRequired;
                this.errorOccured = errorOccured;
            }

            public bool ErrorOccured { get { return errorOccured; } }
            public string Changelog { get { return changelog; } }
            public string NewVersion { get { return newVersion; } }
            public bool UpdateRequired { get { return updateRequired; } }
        }

        private static string newVersion = "";

        public static void CheckForUpdateAsync()
        {
            using (WebClient webClientNewVersion = new WebClient())
            {
                webClientNewVersion.DownloadStringCompleted += WebClientNewVersion_DownloadStringCompleted;
                webClientNewVersion.DownloadStringAsync(new Uri(REMOTE_VERSION_PATH));
            }
        }

        private static void WebClientNewVersion_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Log("UPDATE", "Couldn't check for new version.\n\t" + e.Error.Message + "\n\t" + e.Error.GetType() + "\n" + e.Error.StackTrace);
                OnVersionChecked(new VersionCheckedEventArgs("", "", false, true));
                return;
            }
            newVersion = e.Result.Replace("\n", "");
            if (!Util.GetCurrentVersion().Equals(e.Result.Replace("\n", "")))
            {
                Logger.Log("UPDATE", "Successfully checked for update. Update needed, downloading changelog ...");
                using (WebClient webClientChangelog = new WebClient())
                {
                    webClientChangelog.DownloadStringCompleted += WebClientChangelog_DownloadStringCompleted; ;
                    webClientChangelog.DownloadStringAsync(new Uri(REMOTE_CHANGELOG_PATH));
                }
            }
            else
            {
                Logger.Log("UPDATE", "Successfully checked for update. No update required.");
                OnVersionChecked(new VersionCheckedEventArgs("", "", false, false));
            }
        }

        private static void WebClientChangelog_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Log("UPDATE", "Couldn't download changelog.\n\t" + e.Error.Message + "\n\t" + e.Error.GetType() + "\n" + e.Error.StackTrace);
                OnVersionChecked(new VersionCheckedEventArgs("", "", false, true));
                return;
            }
            Logger.Log("UPDATE", "Successfully downloaded changelog.");
            OnVersionChecked(new VersionCheckedEventArgs(e.Result, newVersion, true, false));
        }

        private static void OnVersionChecked(VersionCheckedEventArgs e)
        {
            if (VersionChecked != null)
            {
                VersionChecked(e);
            }
        }
    }
}
