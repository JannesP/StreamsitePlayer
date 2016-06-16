using SeriesPlayer.Utility.ChromiumBrowsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;

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
        private OffscreenChromiumBrowser requestBrowser;

        public VivoStreamingSite(string link) : base(link)
        {
            requestBrowser = new OffscreenChromiumBrowser();
            requestBrowser.WaitForInit();
            requestBrowser.LoadingStateChanged += RequestBrowser_LoadingStateChanged;
        }

        private void RequestBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                if (!continued)
                {
                    Task.Delay(500).ContinueWith(t => ClickOnContinue());
                }
                else
                {
                    Task.Delay(500).ContinueWith(t => FindAndProcessLink());
                }
            }
        }

        private void ClickOnContinue()
        {
            if (!continued)
            {
                var btnExists = requestBrowser.EvaluateJavaScriptRaw("document.getElementById('access') != null;").GetAwaiter().GetResult();
                Logger.Log("VivoLoading", "btnExists: " + btnExists);
                if (btnExists != null && Convert.ToBoolean(btnExists))
                {
                    requestBrowser.ExecuteScriptAsync("document.getElementById('access').disabled = false;");
                    requestBrowser.ExecuteScriptAsync("document.getElementById('access').click();");
                    continued = true;
                }
                else
                {
                    Task.Delay(100).ContinueWith(t => ClickOnContinue());
                }
            }
        }

        private void FindAndProcessLink()
        {
            string fileUrl = Convert.ToString(requestBrowser.EvaluateJavaScriptRaw(
                        @"(function() {
                            var elements = document.getElementsByClassName('stream-content');
                            if (elements.length > 0) {
                                return elements[0].getAttribute('data-url');
                            } else {
                                return 'failed';
                            }
                        })();"
                    ).GetAwaiter().GetResult());
            Logger.Log("VivoLoading", "fileUrl: " + fileUrl);
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
            else
            {
                Task.Delay(500).ContinueWith(t => FindAndProcessLink());
            }
        }

        public override int GetEstimateWaitTime()
        {
            return 1500;
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
            requestBrowser.Load(base.link);
        }

        public override void RequestJwData(IJwCallbackReceiver receiver, int requestId)
        {
            this.jwRequestId = requestId;
            jwReceiver = receiver;
            requestBrowser.Load(base.link);
        }
    }
}
