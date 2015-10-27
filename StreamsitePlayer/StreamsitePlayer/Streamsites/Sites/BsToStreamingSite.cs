using StreamsitePlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Sites
{
    class BsToStreamingSite : StreamingSite
    {
        public const string NAME = "bsto_site";
        private StreamcloudStreamingSite streamcloudStreamingSite;

        public BsToStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            
        }

        private void RequestStreamcloudJw(Control anchor, string url, IJwCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string streamcloudLink = "http://streamcloud.eu/" + res.GetSubstringBetween(0, "<a href=\"http://streamcloud.eu/", "\"");
            anchor.Invoke((MethodInvoker)(() => StartStreamcloudJwRequest(streamcloudLink, receiver, requestId)));
        }

        private void RequestStreamcloudFile(Control anchor, string url, IFileCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string streamcloudLink = "http://streamcloud.eu/" + res.GetSubstringBetween(0, "<a href=\"http://streamcloud.eu/", "\"");
            anchor.Invoke((MethodInvoker)(() => StartStreamcloudFileRequest(streamcloudLink, receiver, requestId)));
        }

        private void StartStreamcloudJwRequest(string streamcloudlink, IJwCallbackReceiver receiver, int requestId)
        {

            streamcloudStreamingSite = new StreamcloudStreamingSite(targetBrowser, streamcloudlink);
            streamcloudStreamingSite.RequestJwData(receiver, requestId);
        }

        private void StartStreamcloudFileRequest(string streamcloudlink, IFileCallbackReceiver receiver, int requestId)
        {
            streamcloudStreamingSite = new StreamcloudStreamingSite(targetBrowser, streamcloudlink);
            streamcloudStreamingSite.RequestFile(receiver, requestId);
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
        }

        public override string GetFileName()
        {
            return streamcloudStreamingSite.GetFileName();
        }

        public override int GetRemainingWaitTime()
        {
            if (streamcloudStreamingSite != null)
            {
                return streamcloudStreamingSite.GetRemainingWaitTime();
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
