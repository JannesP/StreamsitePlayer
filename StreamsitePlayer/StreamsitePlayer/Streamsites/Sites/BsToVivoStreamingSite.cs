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

        public BsToVivoStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            
        }

        private void RequestStreamcloudJw(Control anchor, string url, IJwCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string vivoLink = "http://vivo.sx/" + res.GetSubstringBetween(0, "<a href=\"http://vivo.sx/", "\"");
            if (anchor != null && !anchor.IsDisposed)
            {
                anchor.Invoke((MethodInvoker)(() => StartVivoJwRequest(vivoLink, receiver, requestId)));
            }
        }

        private void RequestStreamcloudFile(Control anchor, string url, IFileCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string vivoLink = "http://vivo.sx/" + res.GetSubstringBetween(0, "<a href=\"http://vivo.sx/", "\"");
            if (anchor != null && !anchor.IsDisposed)
            {
                anchor.Invoke((MethodInvoker)(() => StartVivoFileRequest(vivoLink, receiver, requestId)));
            }
        }

        private void StartVivoJwRequest(string streamcloudlink, IJwCallbackReceiver receiver, int requestId)
        {

            vivoStreamingSite = new VivoStreamingSite(targetBrowser, streamcloudlink);
            vivoStreamingSite.RequestJwData(receiver, requestId);
        }

        private void StartVivoFileRequest(string streamcloudlink, IFileCallbackReceiver receiver, int requestId)
        {
            vivoStreamingSite = new VivoStreamingSite(targetBrowser, streamcloudlink);
            vivoStreamingSite.RequestFile(receiver, requestId);
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
        }

        public override string GetFileName()
        {
            return vivoStreamingSite.GetFileName();
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
            Thread t = new Thread(() => RequestStreamcloudFile(targetBrowser, link, receiver, requestId));
            t.Start();
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            Thread t = new Thread(() => RequestStreamcloudJw(targetBrowser, link, receiver, requestId));
            t.Start();
        }
    }
}
