using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer
{
    static class Program
    {
        private const int IE7 = 0x1B58; //7000
        private const int IE8 = 0x1F40; //7000
        private const int IE9 = 0x2328; //7000
        private const int IE10 = 0x2710; //0x2710
        private const int IE11 = 0x2AF8; //7000
        private const int IEEDGE = 0x2AF9; //7000

        public static Settings settings;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            SetWebBrowserVersion();
            settings = new Settings();

            Application.ThreadException += Application_ThreadException; //catch all exceptions because of a bug in the VlcControl code.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is ArgumentException) Console.WriteLine("Caught exception!");
            else Logger.Log("Exception!", ((Exception)e.ExceptionObject).Message + "\n" + ((Exception)e.ExceptionObject).StackTrace);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception is ArgumentException) Console.WriteLine("Caught exception!");
            else Logger.Log("Exception!", e.Exception.Message + "\n" + e.Exception.StackTrace);
        }

        private static void SetWebBrowserVersion()
        {
            RegistryKey Regkey = null;
            try
            {
           
                Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);

                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    MessageBox.Show("Application Settings Failed - Address Not found");
                    return;
                }
                Regkey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", unchecked(IE11), RegistryValueKind.DWord); //0x2AF9 = edge : 0x2AF8 = IE11 see https://msdn.microsoft.com/en-us/library/ee330730.aspx#browser_emulation
                Regkey.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Application Settings Failed");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the Registry 
                if (Regkey != null)
                    Regkey.Close();
            }
        }
    }
}
