﻿using SeriesPlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesPlayer.Streamsites.Sites
{
    class CartooncrazyStreamingSite : StreamingSite
    {
        public const string NAME = "toonme";

        public CartooncrazyStreamingSite(string link) : base(link)
        { }

        public override int GetEstimateWaitTime()
        {
            return 500;
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
            Thread thread = new Thread(() => ReturnFileLink(receiver, requestId));
            thread.Start();
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            Thread thread = new Thread(() => ReturnJwLink(receiver, requestId));
            thread.Start();
        }

        private void ReturnFileLink(IFileCallbackReceiver receiver, int requestId)
        {
            receiver.ReceiveFileLink(GetFileLink(), requestId);
        }

        private void ReturnJwLink(IJwCallbackReceiver receiver, int requestId)
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
            link = CheckNextIframeForLink(iFrame);

            return link;
        }

        private string CheckNextIframeForLink(string iFrameLink)
        {
            if (iFrameLink == "")
            {
                return "";
            }
            string page = Util.RequestSimplifiedHtmlSite(iFrameLink);
            page = page.Replace(" ", "");
            int index = 0;
            string link = page.GetSubstringBetween(0, ":[{file:\"", "\"", out index);
            if (link != "")
            {
                link = page.GetSubstringBetween(index, "file:\"", "\"");
            } else
            {
                link = page.GetSubstringBetween(0, "file:\"", "\"");
            }
            if (link == "")
            {
                link = page.GetSubstringBetween(0, "file:'", "'");
            }
            if (link == "")
            {
                return CheckNextIframeForLink(page.GetSubstringBetween(0, "<iframesrc=\"", "\""));
            }
            else
            {
                return link;
            }
        }
    }
}
