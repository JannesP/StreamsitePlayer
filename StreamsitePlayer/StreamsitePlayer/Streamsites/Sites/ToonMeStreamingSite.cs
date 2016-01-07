using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites.Sites
{
    class ToonMeStreamingSite : StreamingSite
    {
        public const string NAME = "toonme";

        public ToonMeStreamingSite(System.Windows.Forms.WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {

        }


        public override int GetEstimateWaitTime()
        {
            return 500;
        }

        public override string GetFileName()
        {
            throw new NotImplementedException();
        }

        public override int GetRemainingWaitTime()
        {
            return 500;
        }

        public override string GetSiteName()
        {
            return "toonme";
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
            receiver.ReceiveFileLink(GetFileLink(), requestId);
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            string jwString = GetFileLink();
            jwString += "\",\ntype: \"mp4";

            receiver.ReceiveJwLinks(jwString, requestId);
        }

        private string GetFileLink()
        {
            string link = "";

            string page = Util.RequestSimplifiedHtmlSite(base.link);
            string iFrame = page.GetSubstringBetween(0, "<iframe src=\"", "\" ");
            iFrame = Util.RequestSimplifiedHtmlSite(iFrame);
            link = iFrame.GetSubstringBetween(0, "},{file: \"", "\",");
            if (link == "")
            {
                link = iFrame.GetSubstringBetween(0, "file: \"", "\",");
            }

            return link;
        }
    }
}
