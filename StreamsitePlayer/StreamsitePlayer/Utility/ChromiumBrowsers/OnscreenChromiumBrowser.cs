﻿using CefSharp.WinForms;
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
                Plugins = CefState.Disabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };
            base.JsDialogHandler = this;
            if (jsExposedObject != null && objectName != null && objectName != "")
            {
                base.RegisterJsObject(objectName, jsExposedObject);
            }
        }

        public void WaitForInit()
        {
            long startTime = DateTime.Now.Ticks;
            while (!IsBrowserInitialized) { System.Windows.Forms.Application.DoEvents(); }
            Logger.Log(TAG, "Waited " + ((DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond) + " ms for initialization of the instance.");
        }

        protected bool IsPageLoaded
        {
            get
            {
                return (IsBrowserInitialized && !IsDisposed && !IsLoading && GetBrowser().HasDocument);
            }
        }

        public void ExecuteJavaScriptAsync(string function, params string[] args)
        {
            if (IsPageLoaded)
            {
                string script = BuildJsFunctionCall(function, args);
                base.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
            }
        }

        public async Task<object> EvaluateJavaScript(string function, params string[] args)
        {
            object result = null;
            if (IsPageLoaded)
            {
                string script = BuildJsFunctionCall(function, args);

                try
                {
                    var task = GetBrowser().MainFrame.EvaluateScriptAsync(script, new TimeSpan(TimeSpan.TicksPerMillisecond * 10));
                    await task.ContinueWith(res => {
                        if (!res.IsFaulted && !res.IsCanceled && res.IsCompleted)
                        {
                            var response = res.Result;
                            result = response.Success ? (response.Result ?? null) : response.Result;
                        }
                    }).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            else
            {
                Logger.Log(TAG, "Got js call while page not loaded! " + this.GetBrowser().MainFrame.Url);
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
            object jsResult = EvaluateJavaScript(function, args).GetAwaiter().GetResult();
            bool result = Convert.ToBoolean(jsResult);
            return result;
        }

        public string EvaluateJavaScriptForString(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args).GetAwaiter().GetResult();
            string result = Convert.ToString(jsResult);
            return result;
        }

        public long EvaluateJavaScriptForLong(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args).GetAwaiter().GetResult();
            long result = Convert.ToInt64(jsResult);
            return result;
        }

        public int EvaluateJavaScriptForInt(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args).GetAwaiter().GetResult();
            int result = Convert.ToInt32(jsResult);
            return result;
        }

        public double EvaluateJavaScriptForDouble(string function, params string[] args)
        {
            object jsResult = EvaluateJavaScript(function, args).GetAwaiter().GetResult();
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
