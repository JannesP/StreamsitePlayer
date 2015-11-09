using SeriesPlayer.Networking.Events;
using SeriesPlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Networking.Messages
{
    class EpisodeListNetworkMessage : NetworkMessage
    {
        public EpisodeListNetworkMessage(byte messageId, Episode episode)
        {
            base.Id = messageId;
            base.TypeVal = (byte)NetworkEventType.Answer;
            base.SpecificTypeVal = (byte)NetworkRequestEvent.EpisodeList;
            base.Data = episode.GetByteData();
        }
    }
}
