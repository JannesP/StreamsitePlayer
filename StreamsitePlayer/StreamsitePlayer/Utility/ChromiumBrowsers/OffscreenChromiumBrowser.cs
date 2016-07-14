using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeriesPlayer.Utility.ChromiumBrowsers
{
    class OffscreenChromiumBrowser : ChromiumWebBrowser, IJsDialogHandler, ILifeSpanHandler
    {
        private const string TAG = "OFFSCREEN_CEF";
        private static BrowserSettings settings = new BrowserSettings()
        {
            DefaultEncoding = "utf-8",
            Javascript = CefState.Enabled,
            JavascriptOpenWindows = CefState.Disabled,
            Plugins = CefState.Disabled,
            UniversalAccessFromFileUrls = CefState.Enabled
        };

        public OffscreenChromiumBrowser() : this("about:blank") { }
        public OffscreenChromiumBrowser(string address) : this(address, settings)
        {
            base.JsDialogHandler = this;
            base.LifeSpanHandler = this;
            base.FrameLoadEnd += OffscreenChromiumBrowser_FrameLoadEnd;
        }

        private void OffscreenChromiumBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Logger.Log(TAG, "Main Frame loaded!");
                e.Frame.ExecuteJavaScriptAsync(
                    @"(function() {
	                var muteAllVideos = function() {
		                var videoTags = document.getElementsByTagName('video');
                        for (var i = 0; i < videoTags.length; i++)
                            {
                                videoTags[i].volume = 0;
                            }
                            setTimeout(muteAllVideos, 100);
                        }
                        muteAllVideos();
                    })();"
                );
            }
        }

        public async Task<string> GetHtmlSourceAsync()
        {
            return await GetBrowser().MainFrame.GetSourceAsync();
        }

        public bool IsPageLoaded
        {
            get
            {
                return (!IsLoading && GetBrowser().HasDocument);
            }
        }

        public void WaitForInit()
        {
            long startTime = DateTime.Now.Ticks;
            while (!IsBrowserInitialized) { System.Windows.Forms.Application.DoEvents(); }
            Logger.Log(TAG, "Waited " + ((DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond) + " ms for initialization of the instance.");
        }

        public async Task ExecuteJavaScriptRawAsync(string script)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(10000);
            await Task.Run(async () =>
            {
                while (!IsPageLoaded)
                {
                    await Task.Delay(500);
                    cts.Token.ThrowIfCancellationRequested();
                }
            }, cts.Token);
            if (IsPageLoaded)
            {
                base.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
            }
        }

        public async Task<object> EvaluateJavaScriptRawAsync(string script)
        {
            object result = null;
            if (IsPageLoaded)
            {
                try
                {
                    var task = GetBrowser().MainFrame.EvaluateScriptAsync(script);
                    await task.ContinueWith(res => {
                        if (!res.IsFaulted && !res.IsCanceled && res.IsCompleted)
                        {
                            var response = res.Result;
                            result = response.Success ? (response.Result ?? null) : response.Result;
                        }
                    });
                }
                catch (Exception e)
                {
                    Logger.Log(e.InnerException);
                }
            }
            return result;
        }

        private OffscreenChromiumBrowser(string address, BrowserSettings settings) : base(address, settings) { }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        { return true; }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        { }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        { }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        { newBrowser = null; return true; }

        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        { }

        public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        { return true; }

        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, string acceptLang, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        { suppressMessage = true; return false; }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        { }
    }
}
