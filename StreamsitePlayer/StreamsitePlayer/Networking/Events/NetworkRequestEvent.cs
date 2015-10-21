using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking.Events
{
    public enum NetworkRequestEvent
    {
        Series = 0,
        Season = 1,
        Episode = 2,
        PlayerStatus = 3
    }

    public delegate void OnNetworkRequestEventHandler(TcpServer source, NetworkRequestEventArgs e);
    public class NetworkRequestEventArgs : NetworkEvent
    {
        public NetworkRequestEvent EventId
        {
            get;
            private set;
        }
        public byte[] Data
        {
            get;
            private set;
        }
        public NetworkRequestEventArgs(BufferedSocket bufSoc, NetworkRequestEvent eventId, byte messageId, byte[] data) : base(bufSoc, messageId)
        {
            EventId = eventId;
            Data = data;
        }
    }
}
