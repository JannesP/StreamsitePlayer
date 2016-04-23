﻿using SeriesPlayer.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer
{
    static class Util
    {
        private static List<IUserInformer> userInformers = new List<IUserInformer>();
        private static Queue<string> remainingMessages = new Queue<string>();
        private static WebBrowser requestBrowser = null;
        private static System.Threading.Timer userMessageHideTimer;

        public static void AddUserInformer(IUserInformer uinf)
        {
            lock (userInformers)
            {
                userInformers.Add(uinf);
            }
        }

        public static void RemoveUserInformer(IUserInformer uinf)
        {
            lock (userInformers)
            {
                userInformers.Remove(uinf);
            }
        }

        public static void ShowUserInformation(string message)
        {
            lock (userInformers)
            {
                remainingMessages.Enqueue(message);
                ShowNextUserInformation();
            }
        }

        private static void ShowNextUserInformation()
        {
            Logger.Log("USR_INFORM", "Showing new message to user.");
            lock (userInformers)
            {
                if (remainingMessages.Count != 0 && userInformers.Count != 0 && userMessageHideTimer == null)
                {
                    string currMsg = remainingMessages.Dequeue();
                    foreach (var informer in userInformers)
                    {
                        informer.ShowUserMessage(currMsg);
                    }
                    userMessageHideTimer = new System.Threading.Timer((state) => { HideUserInformation(); }, null, 5000, -1);
                }
            }
        }

        private static void HideUserInformation()
        {
            Logger.Log("USR_INFORM", "Hiding message from user.");
            lock (userInformers)
            {
                userMessageHideTimer.Dispose();
                userMessageHideTimer = null;

                if (userInformers.Count != 0)
                {
                    foreach (var informer in userInformers)
                    {
                        informer.HideUserMessage();
                    }
                }

                ShowNextUserInformation();
            }
        }

        private static string RequestHtmlSite(string url)
        {
            string responseFromServer = "";
            long start = DateTime.Now.Ticks;
            // Create a request for the URL. 
            try
            {
                WebRequest request = WebRequest.Create(url);
                Logger.Log("UA", ((HttpWebRequest)request).UserAgent);
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Clean up the streams and the response.
                reader.Close();
                response.Close();
                Console.WriteLine("HttpRequest of " + url + " took: " + ((DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond) + " ms");
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.ServiceUnavailable) 
            {
                if (((HttpWebResponse)ex.Response).Server.Contains("cloudflare"))
                {
                    responseFromServer = GetBrowserResponse(url);
                }
            }
            catch (WebException)
            {
                responseFromServer = "";
            }
            return responseFromServer;
        }

        private static string GetBrowserResponse(string url)
        {
            if (requestBrowser == null) requestBrowser = CreatePopuplessBrowser();
            requestBrowser.Navigate(url);
            string result = WaitForLoadingBrowser(requestBrowser);
            requestBrowser.Navigate("about:blank");
            return result;
        }

        private static string WaitForLoadingBrowser(WebBrowser browser)
        {
            if (browser.InvokeRequired)
            {
                return (string)browser.Invoke(new Func<string>(() => WaitForLoadingBrowser(browser)));
            }
            long timeout = 10 * TimeSpan.TicksPerSecond;
            long startTime = DateTime.Now.Ticks;
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(100);
                WebBrowserReadyState wrs = browser.ReadyState;
                if ((DateTime.Now.Ticks - startTime) > timeout)
                {
                    return "";
                } else if (wrs == WebBrowserReadyState.Complete && !browser.DocumentText.Contains("Checking your browser before accessing"))
                {
                    break;
                }
            }
            return browser.DocumentText;
        }

        public static string RequestSimplifiedHtmlSite(string url)
        {
            string requestedRaw = RequestHtmlSite(url);
            long start = DateTime.Now.Ticks;
            requestedRaw = requestedRaw.Replace("\r", "");
            requestedRaw = requestedRaw.Replace("\n", "");
            requestedRaw = requestedRaw.Replace("    ", "");
            Console.WriteLine("Cutting unnececcery things out took: " + ((DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond) + " ms");
            requestedRaw = WebUtility.HtmlDecode(requestedRaw);
            return requestedRaw;
        }

        public static bool CheckWriteAccess(string path)
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);
            FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, path);
            permissionSet.AddPermission(writePermission);
            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }

        public static void OpenLinkInDefaultBrowser(string link)
        {
            Logger.Log("OPEN_EXTERNAL_LINK", "Opening link: " + link);
            System.Diagnostics.Process.Start(link);
        }

        public static string GetAppFolder()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        public static string GetRalativePath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, path);
        }

        public static WebBrowser CreatePopuplessBrowser()
        {
            WebBrowser wb = null;
            if (FormMain.threadTrick.InvokeRequired)
            {
                wb = (WebBrowser)FormMain.threadTrick.Invoke(new Func<WebBrowser>(() => CreatePopuplessBrowser()));
            }
            else
            {
                wb = new Utility.ExtendedBrowser.PopuplessBrowser();
                wb.ScriptErrorsSuppressed = true;
                wb.WebBrowserShortcutsEnabled = false;
                wb.IsWebBrowserContextMenuEnabled = false;
                wb.AllowWebBrowserDrop = false;
            }
            return wb;
        }

        public static string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
