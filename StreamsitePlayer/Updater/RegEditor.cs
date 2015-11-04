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
        private static Guid UninstallGuid = Guid.Parse("{57f74a7a-02b6-4674-a5fb-ff22820fd36b}");

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
                        string guidText = UninstallGuid.ToString("B");
                        key = parent.OpenSubKey(guidText, true) ??
                              parent.CreateSubKey(guidText);

                        if (key == null)
                        {
                            throw new Exception(String.Format("Unable to create uninstaller '{0}\\{1}'", UNINSTALL_REG_KEY_PATH, guidText));
                        }

                        AssemblyName asm = AssemblyName.GetAssemblyName(Path.Combine(instFolder, UNINSTALL_FILE_NAME));
                        Version v = asm.Version;

                        key.SetValue("DisplayName", Application.ProductName);
                        key.SetValue("ApplicationVersion", v.ToString());
                        key.SetValue("Publisher", Application.CompanyName);
                        key.SetValue("DisplayIcon", Path.Combine(instFolder, "SeriesPlayer.exe"));
                        key.SetValue("DisplayVersion", v.ToString());
                        key.SetValue("URLInfoAbout", "--none--");
                        key.SetValue("Contact", "--none--");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", Path.Combine(instFolder, UNINSTALL_FILE_NAME));
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch { }
            }
        }

    }
}
