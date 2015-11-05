using SeriesPlayer.Utility.TaskbarProgressBarStatus;
using System;
using System.Runtime.InteropServices;

namespace SeriesPlayer.Utility.TaskbarStatus
{
    [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ITaskbarList4
    {
        [PreserveSig]
        void HrInit();

        [PreserveSig]
        void AddTab(IntPtr hwnd);

        [PreserveSig]
        void DeleteTab(IntPtr hwnd);

        [PreserveSig]
        void ActivateTab(IntPtr hwnd);

        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        [PreserveSig]
        void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

        [PreserveSig]
        void SetProgressState(IntPtr hwnd, TaskbarProgressBarState tbpFlags);

        [PreserveSig]
        void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

        [PreserveSig]
        void UnregisterTab(IntPtr hwndTab);

        [PreserveSig]
        void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

        [PreserveSig]
        void SetTabActive(IntPtr hwndTab, IntPtr hwndInsertBefore, uint dwReserved);

        [PreserveSig]
        int ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] object[] pButtons);

        [PreserveSig]
        int ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] object[] pButtons);

        [PreserveSig]
        void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);

        [PreserveSig]
        void SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

        [PreserveSig]
        void SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

        [PreserveSig]
        void SetThumbnailClip(IntPtr hwnd, IntPtr prcClip);

        void SetTabProperties(IntPtr hwndTab, int stpFlags);
    }
}
