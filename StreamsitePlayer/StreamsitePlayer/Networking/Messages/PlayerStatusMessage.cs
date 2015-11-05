using SeriesPlayer.Networking.Events;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Networking.Messages
{
    class PlayerStatusMessage : NetworkMessage
    {
        public PlayerStatusMessage(byte messageId, bool isPlaying, int position, int duration, byte bufferPercent)
        {
            base.Id = messageId;
            base.TypeVal = (byte)NetworkEventType.Answer;
            base.SpecificTypeVal = (byte)NetworkRequestEvent.PlayerStatus;
            byte[] positionBytes = position.ToByteArray();
            byte[] durationBytes = duration.ToByteArray();
            byte[] data = new byte[10];
            data[0] = (byte)(isPlaying ? 1 : 0);
            Array.Copy(positionBytes, 0, data, 1, positionBytes.Length);
            Array.Copy(durationBytes, 0, data, 5, durationBytes.Length);
            data[9] = bufferPercent;
            base.Data = data;
        }
    }
}
