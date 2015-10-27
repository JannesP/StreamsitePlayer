using StreamsitePlayer.Player;
using StreamsitePlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer
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
    }
}
