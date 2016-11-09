using SeriesPlayer.Utility.ChromiumBrowsers;
using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites.Sites
{
    class BsToOpenLoadHDSite : BsToOpenLoadSite
    {
        public new const string NAME = "OpenLoadHD";

        public BsToOpenLoadHDSite(string link) : base(link) { }
    }
}
