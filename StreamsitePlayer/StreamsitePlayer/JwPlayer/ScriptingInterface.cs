using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.JwPlayer
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ScriptingInterface
    {
        public interface IJwEventListener
        {
            void OnPlaylocationChanged(long timePlayed, long timeLeft, long timeTotal);
            void OnPlaybackComplete();
            void OnFullscreenChanged(bool newState);
        }
        
        public IJwEventListener receiver = null;
        public void SetIJwEventReceiver(IJwEventListener eventReceiver)
        {
            receiver = eventReceiver;
        }

        public void Log(string message)
        {
            Console.WriteLine("Javascript: " + message);
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

        public void OnPlaybackComplete()
        {
            if (receiver != null)
            {
                receiver.OnPlaybackComplete();
            }
        }

    }
}
