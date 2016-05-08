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
            Logger.Log("PROGRAM", "Experienced fatal crash!");
            Logger.Log((Exception)e.ExceptionObject);
            MessageBox.Show("Ran into unexpected exception. Please report this!", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log("PROGRAM", "Experienced fatal crash!");
            Logger.Log(e.Exception);
            MessageBox.Show("Ran into unexpected exception. Please report this!", "Unexpected exception in StreamsitePlayer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
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
                    strTempAssmbPath = Util.GetRalativePath(@"cef\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
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
