﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer
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
        private const int IEEDGE = 0x2AF9;

        public const string VERSION = "1.1.0b";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            SetWebBrowserVersion();
            DisableWebbrowserClick();

            Application.ThreadException += Application_ThreadException; //catch all exceptions to log them all.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log("Exception!", sender.GetType().ToString() + "\n\t" + ((Exception)e.ExceptionObject).StackTrace);
            DialogResult dr = MessageBox.Show("Ran into unexpected exception. Please report this!\nDo you want to close the program?\nNote: I can't guarentee, that everything works as expected after this.", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log("Exception!", sender.GetType().ToString() + "\n\t" + e.Exception.StackTrace);
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
            RegistryKey Regkey = null;
            try
            {
           
                Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);

                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    Logger.Log("PREINIT", "Error opening regkey for setting the webbrowser version! (" + @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION" + ")");
                    return;
                }
                Regkey.SetValue(Process.GetCurrentProcess().ProcessName + ".exe", unchecked(IE11), RegistryValueKind.DWord); //0x2AF9 = edge : 0x2AF8 = IE11 see https://msdn.microsoft.com/en-us/library/ee330730.aspx#browser_emulation
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
    }
}
