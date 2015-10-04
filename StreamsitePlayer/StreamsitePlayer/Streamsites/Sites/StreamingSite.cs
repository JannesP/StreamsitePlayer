using StreamsitePlayer.Streamsites.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites
{
    abstract class StreamingSite
    {
        protected WebBrowser targetBrowser;
        protected string link;

        public static StreamingSite CreateStreamingSite(string name, WebBrowser targetBrowser, string link)
        {
            switch (name)
            {
                case StreamcloudStreamingSite.NAME:
                    return new StreamcloudStreamingSite(targetBrowser, link);
                case RyuanimeStreamingSite.NAME:
                    return new RyuanimeStreamingSite(targetBrowser, link);
                case DubbedanimehdNetStreamingSite.NAME:
                    return new DubbedanimehdNetStreamingSite(targetBrowser, link);
                case BsToStreamingSite.NAME:
                    return new BsToStreamingSite(targetBrowser, link);
                default:
                    Logger.Log("ERROR!", "Failed creating StreamingSite for: " + name);
                    return null;
            }
        }

        /// <summary>
        /// Initializes a new StreamingSite with the given link.
        /// It will automatically process until it can play the video.
        /// </summary>
        /// <param name="targetBrowser">the WebBrowser to play in</param>
        /// <param name="link">the complete link to the video including the site url</param>
        public StreamingSite(WebBrowser targetBrowser, string link)
        {
            this.targetBrowser = targetBrowser;
            this.targetBrowser.NewWindow += TargetBrowser_NewWindow;
            this.link = link;
        }

        private void TargetBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        public WebBrowser GetTargetBrowser()
        {
            return targetBrowser;
        }

        /// <summary>
        /// Pauses video playback.
        /// </summary>
        /// <returns></returns>
        public abstract bool Pause();
        /// <summary>
        /// Resumes video playback.
        /// </summary>
        /// <returns></returns>
        public abstract bool Play();
        /// <summary>
        /// Check if the site is ready to instantly play the video.
        /// </summary>
        /// <returns>if the video can be played now</returns>
        public abstract bool IsReadyToPlay();
        /// <summary>
        /// Returns the readable name for the streaming site. eg Steamcloud
        /// </summary>
        /// <returns>readable name for the streaming site</returns>
        public abstract string GetSiteName();
        /// <summary>
        /// Returns the file name of the played video.
        /// </summary>
        /// <returns>current file name</returns>
        public abstract string GetFileName();
        /// <summary>
        /// Checks for remaining wait time.
        /// Note: Do NOT use this to check if the video is playable. Use <see cref="IsReadyToPlay"/>
        /// </summary>
        /// <returns>the number in seconds</returns>
        public abstract int GetRemainingWaitTime();
        /// <summary>
        /// Checks for the remaining time which has to be played.
        /// </summary>
        /// <returns>remaining play time in seconds</returns>
        public abstract int GetRemainingPlayTime();
        /// <summary>
        /// Plays the video when it's ready.
        /// </summary>
        public abstract void PlayWhenReady();
        /// <summary>
        /// Maximizes the player.
        /// </summary>
        public abstract void Maximize();
        /// <summary>
        /// Returns the estimated wait time for the purpose of preloading the next episode.
        /// </summary>
        /// <returns>the estimated time in ms</returns>
        public abstract int GetEstimateWaitTime();
        /// <summary>
        /// Returns if the extraction of the jw data is supported.
        /// </summary>
        public abstract bool IsJwLinkSupported();
        /// <summary>
        /// Returns if the extraction of the file is supported.
        /// </summary>
        public abstract bool IsFileDownloadSupported();
        /// <summary>
        /// Requests the jw link. All results and updates are sent to the callback receiver !in another thread!.
        /// </summary>
        public abstract void RequestJwData(IJwCallbackReceiver receiver, int requestId);
        /// <summary>
        /// Requests the jw link. All results and updates are sent to the callback receiver !in another thread!.
        /// </summary>
        public abstract void RequestFile(IFileCallbackReceiver receiver, int requestId);
    }
}
