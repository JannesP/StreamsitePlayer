using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstaller
{
    static class Util
    {
        private static string[] DIRECTORIES = new string[] { "downloads", "cache", "logs", "jwPlayer", "updatecache" };
        private static string[] FILES = new string[] { "SeriesPlayer.exe", "updater.exe", "settings.ini", "uninst.exe" };
        
        public static string GetFullPath(string fileOrDir)
        {
            return Path.Combine(Program.path, fileOrDir);
        }

        public static List<FileInfo> GetUninstallFiles(bool removeDownloads)
        {
            List<FileInfo> files = new List<FileInfo>();

            foreach (string file in FILES)
            {
                string fullFilePath = GetFullPath(file);
                if (File.Exists(fullFilePath))
                {
                    files.Add(new FileInfo(fullFilePath));
                }
            }

            foreach (string removeDir in DIRECTORIES)
            {
                if (removeDir == "downloads" && !removeDownloads)
                {
                    continue;
                }
                string fullRemoveDirPath = GetFullPath(removeDir);
                if (Directory.Exists(fullRemoveDirPath))
                {
                    files.AddRange(new DirectoryInfo(fullRemoveDirPath).GetAllFiles());
                }
            }

            return files;
        }

        public static void DeleteWithSubFolders(this DirectoryInfo dirInfo)
        {
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                subDir.DeleteWithSubFolders();
            }
            try
            {
                dirInfo.Delete();
            }
            catch
            {
                return;
            }
        }

        private static List<FileInfo> GetAllFiles(this DirectoryInfo dir)
        {
            List<FileInfo> files = new List<FileInfo>();
            files.AddRange(dir.GetFiles());

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                files.AddRange(subDir.GetAllFiles());
            }

            return files;
        }

        public static List<DirectoryInfo> GetEmptyUninstallDirectories(bool removeDownloads)
        {
            List<DirectoryInfo> dirs = new List<DirectoryInfo>();

            foreach (string dirPath in DIRECTORIES)
            {
                if (dirPath == "downloads" && removeDownloads)
                {
                    continue;
                }

                string fullPath = GetFullPath(dirPath);
                if (Directory.Exists(fullPath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                    if (dirInfo.IsDirectoryEmpty()) dirs.Add(dirInfo);
                }
            }
            return dirs;
        }

        public static bool IsDirectoryEmpty(this DirectoryInfo dirInfo)
        {
            if (dirInfo.GetFiles().Length == 0)
            {
                DirectoryInfo[] subdirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo di in subdirs)
                {
                    if (!di.IsDirectoryEmpty())
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}
