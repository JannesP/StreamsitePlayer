using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            LocalStorage = CefState.Disabled,
            Plugins = CefState.Disabled,
            UniversalAccessFromFileUrls = CefState.Enabled
        };

        public OffscreenChromiumBrowser() : this("about:blank") { }
        public OffscreenChromiumBrowser(string address) : this(address, settings)
        {
            base.JsDialogHandler = this;
            base.LifeSpanHandler = this;
        }

        public string HtmlSource
        {
            get
            {
                Task<string> sourceTask = GetBrowser().MainFrame.GetSourceAsync();
                sourceTask.Wait();
                return sourceTask.Result;
            }
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

        public object EvaluateJavaScriptRaw(string script)
        {
            object result = null;
            if (IsPageLoaded)
            {
                try
                {
                    Task<JavascriptResponse> task = base.GetBrowser().MainFrame.EvaluateScriptAsync(script, new TimeSpan(TimeSpan.TicksPerMillisecond * 100));
                    task.Wait();
                    JavascriptResponse response = task.Result;
                    if (response.Success)
                    {
                        result = response.Result;
                    }
                    else
                    {
                        Logger.Log(TAG, "Got invalid or timed out JS evaluation call:\n" + response.Message);
                    }
                }
                catch (AggregateException ex)
                {
                    Logger.Log(TAG, "Aggregate exception while evaluating JS: ");
                    Logger.Log(ex);
                }
            }
            else
            {
                Logger.Log(TAG, "Got js call while page not loaded! " + this.GetBrowser().MainFrame.Url);
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
