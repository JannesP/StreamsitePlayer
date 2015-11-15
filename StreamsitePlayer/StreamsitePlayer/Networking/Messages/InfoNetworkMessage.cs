using SeriesPlayer.Networking.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Networking.Messages
{
    public class InfoNetworkMessage : NetworkMessage
    {
        public enum InfoMessage : byte
        {
            SeriesChanged = 0
        }

        public InfoNetworkMessage(byte messageId, InfoMessage msg)
        {
            base.Id = messageId;
            base.TypeVal = (byte)NetworkEventType.Info;
            base.SpecificTypeVal = (byte)msg;
            base.Data = null;
        }

    }
}
