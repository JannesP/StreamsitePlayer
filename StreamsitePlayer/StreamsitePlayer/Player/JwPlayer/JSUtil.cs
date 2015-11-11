﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.JwPlayer
{
    class JSUtil
    {
        private static bool isInitialized = false;
        private static WebBrowser browser = null;

        public static void Init(ref JwPlayerControl browser)
        {
            if (browser == null) throw new ArgumentNullException("ref browser", "Browser reference shouldn't be null!");
            JSUtil.browser = browser;
            JSUtil.isInitialized = true;
        }

        private static bool IsBrowserReady()
        {
            if (!JSUtil.isInitialized)
                throw new Exception("JSUtil not initialized. Call JSUtil.init(...) before executing!");
            if (JSUtil.browser == null)
                throw new ArgumentNullException("browser", "Browser reference is null!");
            if (!JSUtil.browser.IsDisposed && JSUtil.browser.Document == null)
                throw new ArgumentException("browser", "Browser is not ready. Browser.Document is null or browser got disposed!");
            return (JSUtil.isInitialized && JSUtil.browser != null && JSUtil.browser.Document != null);
        }

        public static object ExecuteFunction(string functionName)
        {
            IsBrowserReady();
            return JSUtil.browser.Document.InvokeScript(functionName);
        }

        public static object ExecuteFunction(string functionName, params object[] args)
        {
            IsBrowserReady();
            return JSUtil.browser.Document.InvokeScript(functionName, args);
        }

        public static long ExecuteFunctionForLong(string functionName)
        {
            IsBrowserReady();
            return ResolveLong(JSUtil.browser.Document.InvokeScript(functionName));
        }

        public static long ExecuteFunctionForLong(string functionName, params object[] args)
        {
            IsBrowserReady();
            return ResolveLong(JSUtil.browser.Document.InvokeScript(functionName, args));
        }

        public static int ExecuteFunctionForInt(string functionName, params object[] args)
        {
            IsBrowserReady();
            return ResolveInt(JSUtil.browser.Document.InvokeScript(functionName, args));
        }

        public static int ExecuteFunctionForInt(string functionName)
        {
            IsBrowserReady();
            return ResolveInt(JSUtil.browser.Document.InvokeScript(functionName));
        }

        internal static double ExecuteFunctionForDouble(string functionName)
        {
            IsBrowserReady();
            return ResolveDouble(JSUtil.browser.Document.InvokeScript(functionName));
        }

        public static bool ExecuteFunctionForBool(string functionName)
        {
            IsBrowserReady();
            return ResolveBool(JSUtil.browser.Document.InvokeScript(functionName));
        }

        public static bool ExecuteFunctionForBool(string functionName, params object[] args)
        {
            IsBrowserReady();
            return ResolveBool(JSUtil.browser.Document.InvokeScript(functionName, args));
        }

        private static string ResolveString(object sysComObject)
        {
            return Convert.ToString(sysComObject);
        }

        private static int ResolveInt(object sysComObject)
        {
            return Convert.ToInt32(sysComObject);
        }

        private static long ResolveLong(object sysComObject)
        {
            return Convert.ToInt64(sysComObject);
        }

        private static double ResolveDouble(object sysComObject)
        {
            return Convert.ToDouble(sysComObject);
        }

        private static bool ResolveBool(object sysComObject)
        {
            return Convert.ToBoolean(sysComObject);
        }
    }
}
