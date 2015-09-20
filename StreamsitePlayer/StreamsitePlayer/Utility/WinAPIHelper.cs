using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Utility
{
    static class WinAPIHelper
    {

        //Prevent computer from going to idle state (automatic sleep/display off)
        [Flags]
        enum ThreadExecutionState : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002
        }

        [DllImport("kernel32.dll")]
        private static extern uint SetThreadExecutionState(ThreadExecutionState esFlags);

        public static void PreventIdle()
        {
            uint prevState =  SetThreadExecutionState(ThreadExecutionState.ES_SYSTEM_REQUIRED | ThreadExecutionState.ES_DISPLAY_REQUIRED | ThreadExecutionState.ES_CONTINUOUS);
            if (prevState == 0)
            {
                Logger.Log("WinAPI", "Preventing idle failed.");
            }
            else
            {
                Logger.Log("WinAPI", "Preventing idle.");
            }
        }

        public static void AllowIdle()
        {
            uint prevState = SetThreadExecutionState(ThreadExecutionState.ES_CONTINUOUS);
            if (prevState == 0)
            {
                Logger.Log("WinAPI", "Allowing idle failed.");
            }
            else
            {
                Logger.Log("WinAPI", "Allowing idle.");
            }
        }
        


    }
}
