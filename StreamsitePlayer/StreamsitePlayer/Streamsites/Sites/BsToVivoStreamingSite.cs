using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Sites
{
    class BsToVivoStreamingSite : StreamingSite
    {
        public const string NAME = "bsto_vivo_site";
        private VivoStreamingSite vivoStreamingSite;

        public BsToVivoStreamingSite(string link) : base(link)
        { }

        private void RequestVivoJw(string url, IJwCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string vivoLink = "http://vivo.sx/" + res.GetSubstringBetween(0, "<a href=\"http://vivo.sx/", "\"");
            if (FormMain.threadTrick != null && !FormMain.threadTrick.IsDisposed)
            {
                FormMain.threadTrick.Invoke((MethodInvoker)(() => StartVivoJwRequest(vivoLink, receiver, requestId)));
            }
        }

        private void RequestVivoFile(string url, IFileCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string vivoLink = "http://vivo.sx/" + res.GetSubstringBetween(0, "<a href=\"http://vivo.sx/", "\"");
            if (FormMain.threadTrick != null && !FormMain.threadTrick.IsDisposed)
            {
                FormMain.threadTrick.Invoke((MethodInvoker)(() => StartVivoFileRequest(vivoLink, receiver, requestId)));
            }
        }

        private void StartVivoJwRequest(string streamcloudlink, IJwCallbackReceiver receiver, int requestId)
        {

            vivoStreamingSite = new VivoStreamingSite(streamcloudlink);
            vivoStreamingSite.RequestJwData(receiver, requestId);
        }

        private void StartVivoFileRequest(string streamcloudlink, IFileCallbackReceiver receiver, int requestId)
        {
            vivoStreamingSite = new VivoStreamingSite(streamcloudlink);
            vivoStreamingSite.RequestFile(receiver, requestId);
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
        }
        
        public override int GetRemainingWaitTime()
        {
            if (vivoStreamingSite != null)
            {
                return vivoStreamingSite.GetRemainingWaitTime();
            }
            return GetEstimateWaitTime();
        }

        public override string GetSiteName()
        {
            return NAME;
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        public override void RequestFile(IFileCallbackReceiver receiver, int requestId)
        {
            Thread t = new Thread(() => RequestVivoFile(link, receiver, requestId));
            t.Start();
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            Thread t = new Thread(() => RequestVivoJw(link, receiver, requestId));
            t.Start();
        }
    }
}
