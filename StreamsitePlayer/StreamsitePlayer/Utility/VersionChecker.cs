using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Utility
{
    static class VersionChecker
    {
        public const string REMOTE_VERSION_PATH = "http://127.0.0.1:6112/streamsiteplayer/newest.ver";
        public const string REMOTE_CHANGELOG_PATH = "http://127.0.0.1:6112/streamsiteplayer/newest.changelog";

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
            if (!Program.VERSION.Equals(e.Result.Replace("\n", "")))
            {
                using (WebClient webClientChangelog = new WebClient())
                {
                    webClientChangelog.DownloadStringCompleted += WebClientChangelog_DownloadStringCompleted; ;
                    webClientChangelog.DownloadStringAsync(new Uri(REMOTE_CHANGELOG_PATH));
                }
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
