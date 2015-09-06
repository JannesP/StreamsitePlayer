using StreamsitePlayer.Utility.TaskbarStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Utility
{
    public class TaskbarManager
    {
        private static ITaskbarList4 taskBarList;
        private static object _syncLock = new object();

        public static ITaskbarList4 Instance
        {
            get
            {
                if (taskBarList == null)
                {
                    lock (_syncLock)
                    {
                        if (taskBarList == null)
                        {
                            taskBarList = (ITaskbarList4)new CTaskbarList();
                            taskBarList.HrInit();
                        }
                    }
                }
                return taskBarList;
            }
        }

        public static bool IsPlatformSupported {
            get {
                return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0; //Running Windows 7 (6.1)
            }
        }
    }
}
