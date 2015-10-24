using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer
{
    class Util
    {
        private static string RequestHtmlSite(string url)
        {
            long start = DateTime.Now.Ticks;
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(url);
            Logger.Log("UA", ((HttpWebRequest)request).UserAgent);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            Console.WriteLine("HttpRequest of " + url + " took: " + ((DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond) + " ms");
            return responseFromServer;
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
            WebBrowser wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            wb.NewWindow += Wb_NewWindow;
            return wb;
        }

        private static void Wb_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
