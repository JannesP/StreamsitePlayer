using StreamsitePlayer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Sites
{
    class DubbedanimehdNetStreamingSite : StreamingSite
    {
        public DubbedanimehdNetStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            targetBrowser.DocumentCompleted += TargetBrowser_DocumentCompleted;
            this.link = link;
        }

        public const string NAME = "dubbedanimehd";

        public override int GetEstimateWaitTime()
        {
            return 1;
        }

        public override string GetFileName()
        {
            throw new NotImplementedException();
        }

        public override int GetRemainingPlayTime()
        {
            throw new NotImplementedException();
        }

        public override int GetRemainingWaitTime()
        {
            return 0;
        }

        public override string GetSiteName()
        {
            return NAME;
        }

        public override bool IsJwLinkSupported()
        {
            return true;
        }

        public override bool IsReadyToPlay()
        {
            throw new NotImplementedException();
        }

        public override void Maximize()
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

        public override void PlayWhenReady()
        {
            throw new NotImplementedException();
        }

        private bool iFrameNavigated = false;
        private string iFrameUrl = "";
        IJwCallbackReceiver jwReceiver;
        IFileCallbackReceiver fileReceiver;
        int requestId;
        private void TargetBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;

            if (!iFrameNavigated)
            {
                Logger.Log("SITE_REQUEST_DAHD", "Loaded Source. Searching for IFrame.");
                string htmlText = GetTargetBrowser().DocumentText;

                string iFrameSearch = "<iframe id='video' src='";
                string iframeUrl = htmlText.GetSubstringBetween(0, iFrameSearch, "'");
                Logger.Log("SITE_REQUEST_DAHD", "Found IFrame for: " + iframeUrl);
                GetTargetBrowser().Navigate(iframeUrl);
                iFrameNavigated = true;
                iFrameUrl = iframeUrl;
            }
            else if (iFrameNavigated && browser.Url.AbsolutePath.Equals(new Uri(iFrameUrl).AbsolutePath))
            {
                Logger.Log("SITE_REQUEST_DAHD", "Loaded IFrame. Searching for file.");
                string html = GetTargetBrowser().DocumentText;
                string file = html.GetSubstringBetween(0, "file: '", "'");
                if (fileReceiver != null)
                {
                    Logger.Log("SITE_REQUEST_DAHD", "fileReceiver.ReceiveFileLink()");
                    fileReceiver.ReceiveFileLink(file, requestId);
                    GetTargetBrowser().Dispose();
                    return;
                }
                string insertion = "file:\"" + file + "\"";   //file:"http://.../"
                Logger.Log("SITE_REQUEST_DAHD", "Found file at: " + file);
                if (jwReceiver != null)
                {
                    jwReceiver.ReceiveJwLinks(insertion, requestId);
                }
                GetTargetBrowser().Dispose();
            }

        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            this.jwReceiver = receiver;
            this.requestId = requestId;
            Logger.Log("SITE_REQUEST_DAHD", "Navigating borwser to: " + this.link);
            GetTargetBrowser().Navigate(this.link);
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override void RequestFile(IFileCallbackReceiver receiver, int requestId)
        {
            this.fileReceiver = receiver;
            this.requestId = requestId;
            Logger.Log("SITE_REQUEST_DAHD", "Navigating borwser to: " + this.link);
            GetTargetBrowser().Navigate(this.link);
        }
    }
}
