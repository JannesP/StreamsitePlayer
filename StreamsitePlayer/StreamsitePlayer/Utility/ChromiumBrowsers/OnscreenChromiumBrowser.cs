using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace SeriesPlayer.Utility.ChromiumBrowsers
{
    class OnscreenChromiumBrowser : ChromiumWebBrowser, IJsDialogHandler, ILifeSpanHandler
    {
        private const string TAG = "ONSCREEN_BROWSER";
        public OnscreenChromiumBrowser() : this("about:blank", null, "") { }

        public OnscreenChromiumBrowser(object jsExposedObject, string objectName) : this("about:blank", jsExposedObject, objectName) { }

        public OnscreenChromiumBrowser(string address, object jsExposedObject, string objectName) : base(address)
        {
            base.BrowserSettings = new BrowserSettings()
            {
                DefaultEncoding = "utf-8",
                Javascript = CefState.Enabled,
                JavascriptOpenWindows = CefState.Disabled,
                LocalStorage = CefState.Disabled,
                Plugins = CefState.Disabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };
            base.JsDialogHandler = this;
            if (jsExposedObject != null && objectName != null && objectName != "")
            {
                base.RegisterJsObject(objectName, jsExposedObject);
            }
        }

        public void ExecuteJavaScriptAsync(string function, params string[] args)
        {
            string script = BuildJsFunctionCall(function, args);
            base.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
        }

        public object EvaluateJavaScript(string function, params string[] args)
        {
            string script = BuildJsFunctionCall(function, args);
            object result = null;
            Task<JavascriptResponse> task = base.GetBrowser().MainFrame.EvaluateScriptAsync(script, new TimeSpan(TimeSpan.TicksPerMillisecond * 100));
            JavascriptResponse response = task.Result;
            if (response.Success)
            {
                result = response.Result;
            }
            else
            {
                Logger.Log(TAG, "Got invalid or timed out JS evaluation call:\n" + response.Message);
            }
            return result;
        }

        private static string BuildJsFunctionCall(string function, params string[] args)
        {
            string call = function + "(";
            for (int i = 0; i < args.Length; i++)
            {
                call += args[i];
                if (i < (args.Length - 1))
                {
                    call += ",";
                }
            }
            call += ");";
            return call;
        }

        public bool EvaluateJavaScriptForBool(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args);
            bool result = Convert.ToBoolean(jsResult);
            return result;
        }

        public string EvaluateJavaScriptForString(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args);
            string result = Convert.ToString(jsResult);
            return result;
        }

        public long EvaluateJavaScriptForLong(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args);
            long result = Convert.ToInt64(jsResult);
            return result;
        }

        public int EvaluateJavaScriptForInt(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args);
            int result = Convert.ToInt32(jsResult);
            return result;
        }

        public double EvaluateJavaScriptForDouble(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args);
            double result = Convert.ToDouble(jsResult);
            return result;
        }

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
