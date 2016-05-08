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

        ~VivoStreamingSite()
        {
            requestBrowser.Dispose();
            requestBrowser = null;
            fileReceiver = null;
            jwReceiver = null;
        }

        private void RequestBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                if (!continued)
                {
                    if (Convert.ToBoolean(requestBrowser.EvaluateJavaScriptRaw("document.getElementById('access') == null;")))
                    {
                        requestBrowser.ExecuteScriptAsync("document.getElementById('access').disabled = false;");
                        requestBrowser.ExecuteScriptAsync("document.getElementById('access').click();");
                        continued = true;
                    }
                }
                else
                {
                    string fileUrl = Convert.ToString(requestBrowser.EvaluateJavaScriptRaw(
                        @"(function() {
                            var elements = document.getElementsByClassName('stream-content');
                            if (elements.length > 0) {
                                return elements[0].getAttribute('data-url');
                            } else {
                                return "";
                            }
                        })();"
                    ));
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
