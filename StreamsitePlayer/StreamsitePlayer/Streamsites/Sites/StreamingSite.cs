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
        private WebBrowser targetBrowser;
        private string link;

        /// <summary>
        /// Initializes a new StreamingSite with the given link.
        /// It will automatically process until it can play the video.
        /// </summary>
        /// <param name="targetBrowser">the WebBrowser to play in</param>
        /// <param name="link">the complete link to the video including the site url</param>
        public StreamingSite(WebBrowser targetBrowser, string link)
        {
            this.targetBrowser = targetBrowser;
            this.link = link;
            targetBrowser.Navigate(link);
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
        /// <returns>the estimated time in seconds</returns>
        public abstract int GetEstimatedVlcWaitTime();
        /// <summary>
        /// Returns if the extraction of a direct link to the file is supported.
        /// </summary>
        public abstract bool IsVlcLinkSupported();
        /// <summary>
        /// Requests the vlc link. All results and updates are sent to the callback receiver !in another thread!.
        /// </summary>
        public abstract void RequestVlcLink(IVlcCallbackReceiver receiver, int requestId);
    }
}
