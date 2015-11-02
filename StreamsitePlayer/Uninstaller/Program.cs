using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uninstaller
{
    static class Program
    {
        public static string path;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if !DEBUG
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                DialogResult dr = MessageBox.Show("Do you really want to uninstall the SeriesPlayer?", "Uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr != DialogResult.Yes)
                {
                    return;
                }
                string randomName = Guid.NewGuid().ToString() + ".exe";
                Console.WriteLine(randomName);
                string randomTempFile = Path.Combine(Path.GetTempPath(), randomName);
                if (!File.Exists(randomTempFile))
                {
                    File.Copy(System.Reflection.Assembly.GetEntryAssembly().Location, randomTempFile);
                    ProcessStartInfo psi = new ProcessStartInfo(randomTempFile, "-path=\"" + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\"");
                    Process.Start(psi);
                    return;
                }
                
            }
            else
            {
                foreach (string arg in args)
                {
                    string[] parts = arg.Split('=');
                    if (parts.Length == 2 && parts[0] == "-path")
                    {
                        path = parts[1].Replace("\"", "");
                        if (Directory.Exists(path))
                        {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new FormUninstaller());
                            Application.ApplicationExit += Application_ApplicationExit;
                            break;
                        }
                    }
                }
            }
#else
            path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "testInstall");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormUninstaller());
#endif
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            NativeMethods.ScheduledDelete(System.Reflection.Assembly.GetEntryAssembly().Location);
        }
    }
}
