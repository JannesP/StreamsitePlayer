using SeriesPlayer.Forms;
using SeriesPlayer.Utility.ChromiumBrowsers;
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
        private static OffscreenChromiumBrowser requestBrowser = null;
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

        private async static Task<string> RequestHtmlSiteAsync(string url)
        {
            string responseFromServer = "";
            long start = DateTime.Now.Ticks;
            // Create a request for the URL. 
            try
            {
                WebRequest request = WebRequest.Create(url);
                Logger.Log("UA", ((HttpWebRequest)request).UserAgent);
                // Get the response.
                WebResponse response = await request.GetResponseAsync();
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
                    responseFromServer = await GetBrowserResponseAsync(url);
                }
            }
            catch (WebException)
            {
                responseFromServer = "";
            }
            return responseFromServer;
        }

        private async static Task<string> GetBrowserResponseAsync(string url)
        {
            if (requestBrowser == null) requestBrowser = new OffscreenChromiumBrowser();
            requestBrowser.WaitForInit();
            requestBrowser.Load(url);
            string result = await WaitForLoadingBrowser(requestBrowser);
            requestBrowser.Load("about:blank");
            return result;
        }

        private async static Task<string> WaitForLoadingBrowser(OffscreenChromiumBrowser browser)
        {
            ShowUserInformation("Waiting for Cloudflare protection ...");
            long timeout = 20 * TimeSpan.TicksPerSecond;
            long startTime = DateTime.Now.Ticks;
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(100);
                
                if ((DateTime.Now.Ticks - startTime) > timeout)
                {
                    return "";
                }
                else if (!browser.IsLoading && browser.GetBrowser().HasDocument && !(await browser.GetHtmlSourceAsync()).Contains("Checking your browser before accessing"))
                {
                    break;
                }
            }
            return await browser.GetHtmlSourceAsync();
        }

        public async static Task<string> RequestSimplifiedHtmlSiteAsync(string url)
        {
            string requestedRaw = await RequestHtmlSiteAsync(url);
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
            return Path.Combine(GetAppFolder(), path);
        }

        public static string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
