using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Streamsites.Sites
{
    class VivoStreamingSite : StreamingSite
    {
        public const string NAME = "Vivo";

        private bool continued = false;
        private IFileCallbackReceiver fileReceiver = null;
        private IJwCallbackReceiver jwReceiver = null;
        private int fileRequestId = int.MaxValue;
        private int jwRequestId = int.MaxValue;

        public VivoStreamingSite(WebBrowser targetBrowser, string link) : base(targetBrowser, link)
        {
            targetBrowser.DocumentCompleted += TargetBrowser_DocumentCompleted;
        }

        private void TargetBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.targetBrowser.Document != null)
            {
                if (!continued)
                {
                    HtmlElement continueButton = base.targetBrowser.Document.GetElementById("access");
                    if (continueButton != null)
                    {
                        continueButton.SetAttribute("disabled", "");
                        continueButton.InvokeMember("click");
                        continued = true;
                    }
                }
                else
                {
                    HtmlElementCollection divs = this.targetBrowser.Document.GetElementsByTagName("div");
                    string fileUrl = "";
                    foreach (HtmlElement element in divs)
                    {
                        if (element.GetAttribute("classname").ToString() == "stream-content")
                        {
                            targetBrowser.DocumentCompleted -= TargetBrowser_DocumentCompleted;
                            targetBrowser.Stop();
                            fileUrl = element.GetAttribute("data-url");
                            break;
                        }
                    }
                    if (fileUrl == null) fileUrl = "";
                    if (fileUrl != "")
                    {
                        if (fileReceiver != null)
                        {
                            fileReceiver.ReceiveFileLink(fileUrl, fileRequestId);
                            fileReceiver = null;
                        }
                        if (jwReceiver != null)
                        {
                            fileUrl += "\",\ntype: \"mp4\",\nprovider: \"http";
                            jwReceiver.ReceiveJwLinks(fileUrl, jwRequestId);
                            jwReceiver = null;
                        }

                    }
                }
            }
        }

        public override int GetEstimateWaitTime()
        {
            return 1500;
        }

        public override string GetFileName()
        {
            throw new NotImplementedException();
        }

        public override int GetRemainingWaitTime()
        {
            return GetEstimateWaitTime();
        }

        public override string GetSiteName()
        {
            return "vivo";
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
            this.fileRequestId = requestId;
            fileReceiver = receiver;
            targetBrowser.Navigate(base.link);
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            this.jwRequestId = requestId;
            jwReceiver = receiver;
            targetBrowser.Navigate(base.link);
        }
    }
}
