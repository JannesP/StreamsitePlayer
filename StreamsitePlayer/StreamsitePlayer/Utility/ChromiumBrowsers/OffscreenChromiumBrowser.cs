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
