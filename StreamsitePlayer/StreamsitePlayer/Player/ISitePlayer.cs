using SeriesPlayer.Networking;
using SeriesPlayer.Player;
using SeriesPlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer
{
    interface ISitePlayer
    {
        event OnEpisodeChangeHandler EpisodeChange;

        void Open(StreamProvider streamProvider);
        void Play();
        void Play(int season, int episode);
        void Pause();
        void Close();
        void Next();
        void Previous();
        
        StreamProvider StreamProvider
        {
            get;
            set;
        }

        bool IsDisposed
        {
            get;
        }

        bool IsPlaying
        {
            get;
        }

        bool Maximized
        {
            get;
            set;
        }
        
        bool Autoplay
        {
            get;
            set;
        }
        
        int SkipEndSeconds
        {
            get;
            set;
        }
        
        int SkipStartSeconds
        {
            get;
            set;
        }

        byte BufferPercent
        {
            get;
        }

        long Position
        {
            get;
            set;
        }

        long Duration
        {
            get;
        }

        bool IsLoaded
        {
            get;
        }

        int Volume
        {
            set;
            get;
        }
    }
}
