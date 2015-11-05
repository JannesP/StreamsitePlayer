using Installer.FSShellLink;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Installer
{
    static class Util
    {
        public const string REMOTE_VERSION_PATH = "http://62.75.142.161/streamplayer/newest.ver";
        public const string REMOTE_UPDATER_PATH = "http://62.75.142.161/streamplayer/updater.exe";

        public static bool DownloadUpdaterTo(string existingPath)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(new Uri(REMOTE_UPDATER_PATH), existingPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string DownloadNewestVersion()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    return webClient.DownloadString(new Uri(REMOTE_VERSION_PATH)).Replace("\n", "");
                }
                catch
                {
                    return "";
                }
            }
        }

        public static void CreateShortcut(string filePath, string linkPath)
        {
            IShellLink link = (IShellLink)new ShellLink();

            link.SetDescription("Starts the SeriesPlayer.");
            link.SetPath(filePath);

            IPersistFile file = (IPersistFile)link;
            file.Save(linkPath + ".lnk", false);
        }

        public static string GetDefaultPath()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), Application.ProductName);
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), Application.ProductName);
            }
        }

        public static void DeleteIgnoreContent(this DirectoryInfo dir)
        {
            DirectoryInfo[] subdirs = dir.GetDirectories();
            foreach (DirectoryInfo subdir in subdirs)
            {
                subdir.DeleteIgnoreContent();
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            dir.Delete();
        }

        public static bool IsValidRootedFolderPath(this string pathToCheck)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            foreach (char c in invalidChars)
            {
                if (pathToCheck.Contains(c))
                {
                    return false;
                }
            }

            DirectoryInfo fi = null;
            try
            {
                fi = new DirectoryInfo(pathToCheck);
            }
            catch { }
            if (fi == null)
            {
                return false;
            }

            if (!Path.IsPathRooted(pathToCheck))
            {
                return false;
            }

            return true;
        }

        public static void CreateWithParents(this DirectoryInfo dir)
        {
            if (dir.Parent.Exists)
            {
                dir.Create();
            }
            else
            {
                dir.CreateWithParents();
            }
        }

    }
}
