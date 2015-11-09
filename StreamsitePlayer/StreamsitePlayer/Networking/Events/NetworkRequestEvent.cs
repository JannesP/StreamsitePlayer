using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Networking.Events
{
    public enum NetworkRequestEvent
    {
        EpisodeList = 0,
        SeasonCount = 1,
        CurrentEpisode = 2,
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
