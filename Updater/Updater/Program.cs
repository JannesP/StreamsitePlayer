using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ParseCommandLine();
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void ParseCommandLine()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.Contains("-waitforpid="))
                {
                    string pidString = arg.Replace("-waitforpid=", "");
                    int pid;
                    bool res = int.TryParse(pidString, out pid);
                    if (res)
                    {
                        long startTime = DateTime.Now.Ticks;
                        Logger.Log("ARGS", "Found pid to wait for (\"" + arg + "\". Waiting for it to exit ...");
                        Process p = Process.GetProcessById(pid);
                        if (p != null)
                        {
                            p.WaitForExit();
                        }
                        Logger.Log("ARGS", "Waited " + (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond + "ms for the PID " + pid + "to exit.");
                    }
                    else
                    {
                        Logger.Log("ARGS", "Received \"" + arg + "\" which is not valid, exiting!");
                        return;
                    }
                }
                else
                {
                    Logger.Log("ARGS", "Received invalid arg: \"" + arg + "\", ignoring!");
                }
            }
        }
    }
}
