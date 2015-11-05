using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    class RegEditor
    {
        private const string UNINSTALL_REG_KEY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string UNINSTALL_FILE_NAME = "uninst.exe";
        private static Guid UninstallGuid = Guid.Parse("{57F74A7A-02B6-4674-A5FB-FF22820FD36B}");

        public static void CreateUninstaller(string instFolder)
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(UNINSTALL_REG_KEY_PATH, true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = UninstallGuid.ToString("B").ToUpper();
                        key = parent.CreateSubKey(guidText, RegistryKeyPermissionCheck.ReadWriteSubTree);

                        if (key == null)
                        {
                            Logger.Log("REGISTRY", "key == null");
                            throw new Exception(String.Format("Unable to create uninstaller '{0}\\{1}'", UNINSTALL_REG_KEY_PATH, guidText));
                        }

                        AssemblyName asm = Assembly.GetExecutingAssembly().GetName();
                        Logger.Log("REGISTRY", asm.FullName);
                        Version v = asm.Version;
                        Logger.Log("REGISTRY", v.ToString());

                        Logger.Log("REGISTRY", "Writing sub keys.");
                        key.SetValue("DisplayName", Application.ProductName);
                        key.SetValue("ApplicationVersion", v.ToString());
                        key.SetValue("Publisher", "");
                        key.SetValue("DisplayIcon", Path.Combine(instFolder, "SeriesPlayer.exe"));
                        key.SetValue("DisplayVersion", v.ToString());
                        key.SetValue("URLInfoAbout", "");
                        key.SetValue("Contact", "");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", Path.Combine(instFolder, UNINSTALL_FILE_NAME));
                        key.SetValue("NoModify", 1, RegistryValueKind.DWord);
                        Logger.Log("REGISTRY", "Finished writing sub keys.");
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("REGISTRY", e.GetType().ToString() + ": " + e.Message + "\n\t" + e.StackTrace);
                }
            }
        }

        public static void RemoveUninstaller()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(UNINSTALL_REG_KEY_PATH, true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    parent.DeleteSubKey(UninstallGuid.ToString("B"), false);
                }
                catch { }
            }
        }

    }
}
