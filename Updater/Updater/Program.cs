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
            Application.ThreadException += Application_ThreadException; //catch all exceptions to log them all.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ParseCommandLine();
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log("Exception!", e.ExceptionObject.GetType().ToString() + "\n" + ((Exception)e.ExceptionObject).StackTrace);
            MessageBox.Show("Ran into unexpected exception. Please report this!\nDo you want to close the program?\nNote: I can't guarentee, that everything works as expected after this.", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log("Exception!", e.Exception.GetType().ToString() + "\n" + e.Exception.StackTrace);
            MessageBox.Show("Ran into unexpected exception. Please report this!\nDo you want to close the program?\nNote: I can't guarentee, that everything works as expected after this.", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
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
                        Logger.Log("ARGS", "Found pid to wait for \"" + pid + "\". Waiting for it to exit ...");
                        if (ProcessExists(pid))
                        {
                            Process p = Process.GetProcessById(pid);
                            if (p != null)
                            {
                                p.WaitForExit();
                            }
                        }
                        Logger.Log("ARGS", "Waited " + (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond + "ms for the PID " + pid + "to exit.");
                    }
                    else
                    {
                        Logger.Log("ARGS", "Received \"" + arg + "\" which is not valid, exiting!");
                        return;
                    }
                }
                else if (arg.Contains("-ver="))
                {
                    FormMain.VERSION = arg.Replace("-ver=", "");
                }
                else
                {
                    Logger.Log("ARGS", "Received invalid arg: \"" + arg + "\", ignoring!");
                }
            }
            if (FormMain.VERSION == "")
            {
                Logger.Log("ARGS", "Got invalid version! Aborting.");
                return;
            }
            else
            {
                FormMain.REMOTE_FILE_PATH = FormMain.REMOTE_FILE_PATH.Replace("newest", FormMain.VERSION);
                Logger.Log("ARGS", "Updating to version " + FormMain.VERSION);
            }
        }

        private static bool ProcessExists(int id)
        {
            return Process.GetProcesses().Any(x => x.Id == id);
        }
    }
}
