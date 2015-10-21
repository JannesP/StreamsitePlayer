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
        Control = 1,
        Answer = 2,
        Message = 3
    }

    public abstract class NetworkEvent : EventArgs
    {
        public BufferedSocket Socket
        {
            get;
            private set;
        }
        public byte MessageId
        {
            get;
            private set;
        }
        public NetworkEvent(BufferedSocket socket, byte messageId)
        {
            MessageId = messageId;
            Socket = socket;
        }
    }
    
}
