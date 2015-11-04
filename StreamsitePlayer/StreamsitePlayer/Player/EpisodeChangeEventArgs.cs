using SeriesPlayer.Streamsites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Player
{
    public delegate void OnEpisodeChangeHandler(object source, EpisodeChangeEventArgs e);
    public class EpisodeChangeEventArgs : EventArgs
    {
        private Episode newEpisode;
        public EpisodeChangeEventArgs(Episode newEpisode)
        {
            this.newEpisode = newEpisode;
        }
        public Episode NewEpisode { get { return this.newEpisode; } }
    }
}
