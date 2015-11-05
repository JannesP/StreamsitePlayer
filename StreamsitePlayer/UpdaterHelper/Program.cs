using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo folderSrc = null;
            DirectoryInfo folderDest = null;
            FileInfo startAfterCopy = null;
            foreach (string arg in args)
            {
                if (arg.Contains("-waitforpid="))
                {
                    int pid;
                    bool success = int.TryParse(arg.Replace("-waitforpid=", ""), out pid);
                    if (success)
                    {
                        if (ProcessExists(pid))
                        {
                            Process p = Process.GetProcessById(pid);
                            if (p != null)
                            {
                                p.WaitForExit();
                            }
                        }
                    }
                }
                else if (arg.Contains("-src="))
                {
                    folderSrc = new DirectoryInfo(arg.Replace("-src=", "").Replace("\"", ""));
                    if (!folderSrc.Exists) folderSrc = null;
                    
                }
                else if (arg.Contains("-dst="))
                {
                    folderDest = new DirectoryInfo(arg.Replace("-dst=", "").Replace("\"", ""));
                    if (!folderDest.Exists) folderDest = null;
                }
                else if (arg.Contains("-start="))
                {
                    startAfterCopy = new FileInfo(arg.Replace("-start=", "").Replace("\"", ""));
                }
            }

            if (folderSrc != null && folderDest != null)
            {
                DirectoryCopy(folderSrc.FullName, folderDest.FullName);
                if (folderSrc.Parent.Name == "updatecache")
                {
                    folderSrc.Parent.Attributes &= ~FileAttributes.Hidden;
                    folderSrc.Parent.Delete(true);
                }

                if (startAfterCopy != null && startAfterCopy.Exists)
                {
                    ProcessStartInfo psi = new ProcessStartInfo(startAfterCopy.FullName);
                    psi.Arguments = "-nopatch";
                    Process.Start(psi);
                }

                NativeMethods.ScheduledDelete(Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                Console.WriteLine("Please don't manually use this program.");
                Console.In.ReadLine();
            }

        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }

        }

        private static bool ProcessExists(int id)
        {
            return Process.GetProcesses().Any(x => x.Id == id);
        }
    }
}
