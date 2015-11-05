using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class InstallationInfo
    {
        public InstallationInfo(bool startMenuEntry, bool desktopShortcut, bool startAfterInstallation, bool installAllUsers, string installationFolder)
        {
            StartMenuEntry = startMenuEntry;
            DesktopShortcut = desktopShortcut;
            StartAfterInstallation = startAfterInstallation;
            InstallationFolder = installationFolder;
            InstallAllUsers = installAllUsers;
        }

        public bool InstallAllUsers
        {
            get;
            private set;
        }

        public bool StartMenuEntry
        {
            get;
            private set;
        }

        public bool DesktopShortcut
        {
            get;
            private set;
        }

        public bool StartAfterInstallation
        {
            get;
            private set;
        }

        public string InstallationFolder
        {
            get;
            private set;
        }
    }
}
