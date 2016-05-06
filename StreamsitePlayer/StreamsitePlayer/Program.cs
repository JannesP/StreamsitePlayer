using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer
{
    static class Program
    {
        [DllImport("urlmon.dll")]
        [return: MarshalAs(UnmanagedType.Error)]
        private static extern int CoInternetSetFeatureEnabled(int FeatureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);   //doc: https://msdn.microsoft.com/en-us/library/ms537168%28v=vs.85%29.aspx
        private const int FEATURE_DISABLE_NAVIGATION_SOUNDS = 21;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;

        private const int IE7 = 0x1B58;
        private const int IE8 = 0x1F40;
        private const int IE9 = 0x2328;
        private const int IE10 = 0x2710;
        private const int IE11 = 0x2AF8;
        private const int IE_EDGE = 0x2AF9;

        private const string IE_VERSION_REG_PATH = @"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";

        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Environment.CurrentDirectory = Util.GetAppFolder();

            Logger.Log("PREINIT", "Started version " + Util.GetCurrentVersion());
            
            SetWebBrowserVersion();
            DisableWebbrowserClick();

            Logger.Log("PREINIT", "Adding global exception handlers.");
            Application.ThreadException += Application_ThreadException; //catch all exceptions to log them all.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Logger.Log("PREINIT", "Creating main form.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log((Exception)e.ExceptionObject);
            DialogResult dr = MessageBox.Show("Ran into unexpected exception. Please report this!\nDo you want to close the program?\nNote: I can't guarentee, that everything works as expected after this.", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log(e.Exception);
            DialogResult dr = MessageBox.Show("Ran into unexpected exception. Please report this!\nDo you want to close the program?\nNote: I can't guarentee, that everything works as expected after this.", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) Application.Exit();
        }

        private static void DisableWebbrowserClick()
        {
            int res = CoInternetSetFeatureEnabled(FEATURE_DISABLE_NAVIGATION_SOUNDS, SET_FEATURE_ON_PROCESS, true);
            if (res == 0)
            {
                Logger.Log("PREINIT", "Turning off browser click off succeeded.");
            }
            else
            {
                Logger.Log("PREINIT_ERROR", "Turning off browser click off FAILED!");
            }
        }

        private static void SetWebBrowserVersion()
        {
            int targetIEVersion = (Environment.OSVersion.Version.Major >= 10) ? IE_EDGE : IE11;
            RegistryKey Regkey = null;
            try
            {
           
                Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(IE_VERSION_REG_PATH, true);

                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    Logger.Log("PREINIT", "Error opening regkey for setting the webbrowser version! (" + IE_VERSION_REG_PATH + ")");
                    return;
                }

                int currentValue = (int)Regkey.GetValue(Process.GetCurrentProcess().ProcessName + ".exe", 0);
                if (currentValue != targetIEVersion)
                {
                    Logger.Log("PREINIT", "Changing IE version to " + targetIEVersion);
                    Regkey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", unchecked(targetIEVersion), RegistryValueKind.DWord); //0x2AF9 = edge : 0x2AF8 = IE11 see https://msdn.microsoft.com/en-us/library/ee330730.aspx#browser_emulation
                    Logger.Log("PREINIT", "Successfully set the IE version.");
                    Logger.Log("PREINIT", "Restarting program to apply changes ...");
                    Process.Start(Environment.GetCommandLineArgs()[0]);
                    Application.Exit();
                }
                else
                {
                    Logger.Log("PREINIT", "The right IE version is already set.");
                }

                Regkey.Close();

            }
            catch (Exception ex)
            {
                Logger.Log("PREINIT", "Failed setting reg value for webbrowser version.\n" + ex.GetType().ToString() + "\n\t" + ex.StackTrace);
            }
            finally
            {
                //Close the Registry 
                if (Regkey != null)
                    Regkey.Close();
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = Util.GetRalativePath(@"CefBinaries\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
                    break;
                }

            }
            //Load the assembly from the specified path. 					
            MyAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return MyAssembly;
        }
    }
}
