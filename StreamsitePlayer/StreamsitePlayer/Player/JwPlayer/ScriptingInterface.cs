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
            void OnPrevious();
            void OnNext();
            bool InvokeRequired
            {
                get;
            }
            void Invoke(Delegate method);
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
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnPlaylocationChanged(timePlayed, timeLeft, timeTotal)));
                }
                else
                {
                    receiver.OnPlaylocationChanged(timePlayed, timeLeft, timeTotal);
                }
            }
        }

        public void OnFullscreenChanged(bool newState)
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnFullscreenChanged(newState)));
                }
                else
                {
                    receiver.OnFullscreenChanged(newState);
                }
            }
        }

        public void OnReady()
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnReady()));
                }
                else
                {
                    receiver.OnReady();
                }
            }
        }

        public void OnPlaybackComplete()
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnPlaybackComplete()));
                }
                else
                {
                    receiver.OnPlaybackComplete();
                }
            }
        }

        public void OnError(string errorMessage)
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnError(errorMessage)));
                }
                else
                {
                    receiver.OnError(errorMessage);
                }
            }
        }

        public void OnSetupError(string errorMessage)
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnSetupError(errorMessage)));
                }
                else
                {
                    receiver.OnStartupError(errorMessage);
                }
            }
        }

        public void OnVolumeChange(int newVolume)
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnVolumeChange(newVolume)));
                }
                else
                {
                    receiver.OnVolumeChange(newVolume);
                }
            }
        }

        public void OnMuteChange(bool muted)
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnMuteChange(muted)));
                }
                else
                {
                    receiver.OnMuteChange(muted);
                }
            }
        }

        public void OnPrevious()
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnPrevious()));
                }
                else
                {
                    receiver.OnPrevious();
                }
            }
        }

        public void OnNext()
        {
            if (receiver != null)
            {
                if (receiver.InvokeRequired)
                {
                    receiver.Invoke((System.Windows.Forms.MethodInvoker)(() => OnNext()));
                }
                else
                {
                    receiver.OnNext();
                }
            }
        }

    }
}
