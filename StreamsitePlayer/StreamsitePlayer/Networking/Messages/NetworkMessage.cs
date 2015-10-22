using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking.Messages
{
    public class NetworkMessage
    {
        protected byte[] Data
        {
            private get;
            set;
        }

        protected byte Id
        {
            private get;
            set;
        }

        protected byte TypeVal
        {
            private get;
            set;
        }

        protected byte SpecificTypeVal
        {
            private get;
            set;
        }

        public byte[] GetFullMessageBytes()
        {
            byte[] message = new byte[3 + Data.Length];
            message[0] = TypeVal;
            message[1] = SpecificTypeVal;
            message[2] = Id;
            Array.Copy(Data, 0, message, 3, Data.Length);
            return message;
        }
    }
}
