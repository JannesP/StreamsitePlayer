using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UpdaterHelper
{
    class NativeMethods
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll")]
        public static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);

        [Flags]
        public enum MoveFileExFlags
        {
            MOVEFILE_REPLACE_EXISTING = 0x1,
            MOVEFILE_COPY_ALLOWED = 0x2,
            MOVEFILE_DELAY_UNTIL_REBOOT = 0x4,
            MOVEFILE_WRITE_THROUGH = 0x8
        }

        public static bool ScheduledDelete(string fileFullName)
        {
            if (!System.IO.File.Exists(fileFullName))
                throw new InvalidOperationException("File does not exist.");

            return MoveFileEx(fileFullName, null, (int)MoveFileExFlags.MOVEFILE_DELAY_UNTIL_REBOOT); //MOVEFILE_DELAY_UNTIL_REBOOT = 0x04
        }

    }
}
