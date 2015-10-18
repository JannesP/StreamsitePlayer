﻿using System;
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
        Previous = 4,
        PlayEpisode = 8
    }

    public delegate void OnNetworkControlEventHandler(object source, NetworkControlEventArgs e);
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
        public NetworkControlEventArgs(BufferedSocket bufSoc, NetworkControlEvent eventId, byte[] data) : base(bufSoc)
        {
            EventId = eventId;
            Data = data;
        }
    }
    
}
