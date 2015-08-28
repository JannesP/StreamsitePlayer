using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Streamsites.Sites
{
    class RyuanimeStreamingSite : StreamingSite
    {
        private string link;

        public RyuanimeStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            targetBrowser.DocumentCompleted += TargetBrowser_DocumentCompleted;
            this.link = link;
        }

        

        public const string NAME = "Ryuanime";

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
                string htmlText = GetTargetBrowser().DocumentText;

                string iFrameSearch = "<iframe src=\"";
                string iframeUrl = Util.GetStringBetween(htmlText, 0, iFrameSearch, "\"");
                int indexToCut = iframeUrl.IndexOf('&');
                if (indexToCut != -1)
                {
                    iframeUrl = iframeUrl.Substring(0, indexToCut);
                }
                GetTargetBrowser().Navigate(iframeUrl);
                iFrameNavigated = true;
                iFrameUrl = iframeUrl;
            }
            else if (iFrameNavigated && browser.Url.AbsolutePath.Equals(new Uri(iFrameUrl).AbsolutePath))
            {
                HtmlElement element = GetTargetBrowser().Document.GetElementById("flowplayer_api");
                string stringToSearch = element.InnerHtml;
                int testSearch = stringToSearch.IndexOf("url");
                string file = Util.GetStringBetween(stringToSearch, 0, "\"clip\":{\"url\":\"", "\"");
                if (fileReceiver != null)
                {
                    fileReceiver.ReceiveFileLink(file, requestId);
                    GetTargetBrowser().Dispose();
                    return;
                }
                string insertion = "file:\"" + file + "\"";   //file:"http://.../"
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
            GetTargetBrowser().Navigate(this.link);
        }
    }
}
