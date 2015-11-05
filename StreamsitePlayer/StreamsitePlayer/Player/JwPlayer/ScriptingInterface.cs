using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.JwPlayer
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ScriptingInterface
    {
        public interface IJwEventListener
        {
            void OnPlaylocationChanged(long timePlayed, long timeLeft, long timeTotal);
            void OnPlaybackComplete();
            void OnFullscreenChanged(bool newState);
            void OnError(string message);
            void OnStartupError(string message);
            void OnReady();
            void OnVolumeChange(int newVolume);
            void OnMuteChange(bool muted);
        }
        
        public IJwEventListener receiver = null;
        public void SetIJwEventReceiver(IJwEventListener eventReceiver)
        {
            receiver = eventReceiver;
        }

        public void Log(string message)
        {
            Logger.Log("JwPlayerJS", message);
        }

        public void OnPlaylocationChanged(long timePlayed, long timeLeft, long timeTotal)
        {
            if (receiver != null)
            {
                receiver.OnPlaylocationChanged(timePlayed, timeLeft, timeTotal);
            }
        }

        public void OnFullscreenChanged(bool newState)
        {
            if (receiver != null)
            {
                receiver.OnFullscreenChanged(newState);
            }
        }

        public void OnReady()
        {
            if (receiver != null)
            {
                receiver.OnReady();
            }
        }

        public void OnPlaybackComplete()
        {
            if (receiver != null)
            {
                receiver.OnPlaybackComplete();
            }
        }

        public void OnError(string errorMessage)
        {
            if (receiver != null)
            {
                receiver.OnError(errorMessage);
            }
        }

        public void OnSetupError(string errorMessage)
        {
            if (receiver != null)
            {
                receiver.OnStartupError(errorMessage);
            }
        }

        public void OnVolumeChange(int newVolume)
        {
            if (receiver != null)
            {
                receiver.OnVolumeChange(newVolume);
            }
        }

        public void OnMuteChange(bool muted)
        {
            if (receiver != null)
            {
                receiver.OnMuteChange(muted);
            }
        }

    }
}
