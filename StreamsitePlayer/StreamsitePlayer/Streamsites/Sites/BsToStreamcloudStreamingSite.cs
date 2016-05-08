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
    class BsToStreamcloudStreamingSite : StreamingSite
    {
        public const string NAME = "bsto_streamcloud_site";
        private StreamcloudStreamingSite streamcloudStreamingSite;

        public BsToStreamcloudStreamingSite(string link) : base(link)
        { }

        private void RequestStreamcloudJw(string url, IJwCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string streamcloudLink = "http://streamcloud.eu/" + res.GetSubstringBetween(0, "<a href=\"http://streamcloud.eu/", "\"");
            if (FormMain.threadTrick != null && !FormMain.threadTrick.IsDisposed)
            {
                FormMain.threadTrick.Invoke((MethodInvoker)(() => StartStreamcloudJwRequest(streamcloudLink, receiver, requestId)));
            }
        }

        private void RequestStreamcloudFile(string url, IFileCallbackReceiver receiver, int requestId)
        {
            string res = Util.RequestSimplifiedHtmlSite(url);
            string streamcloudLink = "http://streamcloud.eu/" + res.GetSubstringBetween(0, "<a href=\"http://streamcloud.eu/", "\"");
            if (FormMain.threadTrick != null && !FormMain.threadTrick.IsDisposed)
            {
                FormMain.threadTrick.Invoke((MethodInvoker)(() => StartStreamcloudFileRequest(streamcloudLink, receiver, requestId)));
            }
        }

        private void StartStreamcloudJwRequest(string streamcloudlink, IJwCallbackReceiver receiver, int requestId)
        {

            streamcloudStreamingSite = new StreamcloudStreamingSite(streamcloudlink);
            streamcloudStreamingSite.RequestJwData(receiver, requestId);
        }

        private void StartStreamcloudFileRequest(string streamcloudlink, IFileCallbackReceiver receiver, int requestId)
        {
            streamcloudStreamingSite = new StreamcloudStreamingSite(streamcloudlink);
            streamcloudStreamingSite.RequestFile(receiver, requestId);
        }

        public override int GetEstimateWaitTime()
        {
            return 12000;
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
            Thread t = new Thread(() => RequestStreamcloudFile(link, receiver, requestId));
            t.Start();
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            Thread t = new Thread(() => RequestStreamcloudJw(link, receiver, requestId));
            t.Start();
        }
    }
}
