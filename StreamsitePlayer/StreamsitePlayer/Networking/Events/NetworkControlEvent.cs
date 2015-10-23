using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking.Events
{
    public enum NetworkControlEvent
    {
        PlayPause = 0,
        ClosePlayer = 1,
        Next = 2,
        Previous = 3,
        PlayEpisode = 4,
        SeekTo = 5,
        SkipStart = 6,
        SkipEnd = 7
    }

    public delegate void OnNetworkControlEventHandler(TcpServer source, NetworkControlEventArgs e);
    public class NetworkControlEventArgs : NetworkEvent
    {
        public NetworkControlEvent EventId
        {
            get;
            private set;
        }
        public byte[] Data
        {
            get;
            private set;
        }
        public NetworkControlEventArgs(BufferedSocket bufSoc, NetworkControlEvent eventId, byte messageId, byte[] data) : base(bufSoc, messageId)
        {
            EventId = eventId;
            Data = data;
        }
    }
    
}
