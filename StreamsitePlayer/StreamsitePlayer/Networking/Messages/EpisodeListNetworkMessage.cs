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
            if (episode == null)
            {
                base.Data = new byte[] { 1, 2, 3, 4 };  //end marker
            }
            else
            {
                base.Data = episode.GetByteData();
            }
        }
    }
}
