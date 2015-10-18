using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking.Events
{
    public enum NetworkEventType : byte
    {
        Request = 0,
        Control = 1
    }

    public abstract class NetworkEvent : EventArgs
    {
        public BufferedSocket Socket
        {
            get;
            private set;
        }
        public NetworkEvent(BufferedSocket socket)
        {
            Socket = socket;
        }
    }
    
}
