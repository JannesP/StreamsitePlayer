using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Sites
{
    class StreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "Streamcloud";

        public StreamcloudStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link) { }

        public override string GetFileName()
        {
            throw new NotImplementedException();
        }

        public override int GetRemainingWaitTime()
        {
            throw new NotImplementedException();
        }

        public override string GetSiteName()
        {
            throw new NotImplementedException();
        }

        public override bool IsReadyToPlay()
        {
            throw new NotImplementedException();
        }

        public override bool Pause()
        {
            throw new NotImplementedException();
        }

        public override bool Play()
        {
            throw new NotImplementedException();
        }
    }
}
