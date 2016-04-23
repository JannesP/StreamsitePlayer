using SeriesPlayer.Utility.ExtendedBrowser.ActiveXWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SeriesPlayer.Utility.ExtendedBrowser
{
    class PopuplessBrowser : WebBrowser, DWebBrowserEvents2
    {

        /// <summary>
        /// This method supports the .NET Framework infrastructure and is not intended to be used directly from your code. 
        /// Called by the control when the underlying ActiveX control is created. 
        /// </summary>
        /// <param name="nativeActiveXObject"></param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void AttachInterfaces(object nativeActiveXObject)
        {
            base.AttachInterfaces(nativeActiveXObject);
        }

        /// <summary>
        /// This method supports the .NET Framework infrastructure and is not intended to be used directly from your code. 
        /// Called by the control when the underlying ActiveX control is discarded. 
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void DetachInterfaces()
        {
            base.DetachInterfaces();
        }

        System.Windows.Forms.AxHost.ConnectionPointCookie cookie;

        /// <summary>
        /// This method will be called to give you a chance to create your own event sink
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void CreateSink()
        {
            // Make sure to call the base class or the normal events won't fire
            base.CreateSink();
            cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, this, typeof(DWebBrowserEvents2));
        }

        /// <summary>
        /// Detaches the event sink
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void DetachSink()
        {
            if (null != cookie)
            {
                cookie.Disconnect();
                cookie = null;
            }
        }

        public void NewWindow2([In, MarshalAs(UnmanagedType.IDispatch), Out] ref object ppDisp, [In, Out] ref bool Cancel)
        {
            Cancel = true;
            ppDisp = null;
        }

        public void NewWindow3([In, MarshalAs(UnmanagedType.IDispatch), Out] ref object ppDisp, [In, Out] ref bool Cancel, [In] uint dwFlags, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrlContext, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl)
        {
            Cancel = true;
            ppDisp = null;
        }

        public void NewProcess([In] long CauseFlag, [In] object pWB2, [In, Out] bool Cancel)
        {
            Cancel = true;
        }

        #region Unused ActiveX Events
        public void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object URL, [In, MarshalAs(UnmanagedType.Struct)] ref object Flags, [In, MarshalAs(UnmanagedType.Struct)] ref object TargetFrameName, [In, MarshalAs(UnmanagedType.Struct)] ref object PostData, [In, MarshalAs(UnmanagedType.Struct)] ref object Headers, [In, Out] ref bool Cancel)
        {}
        
        public void ClientToHostWindow([In, Out] ref int CX, [In, Out] ref int CY)
        {}

        public void CommandStateChange([In] int Command, [In] bool Enable)
        {}

        public void DocumentComplete([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object URL)
        {}

        public void DownloadBegin()
        {}

        public void DownloadComplete()
        {}

        public void NavigateComplete2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object URL)
        {}

        public void NavigateError([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object URL, [In, MarshalAs(UnmanagedType.Struct)] ref object Frame, [In, MarshalAs(UnmanagedType.Struct)] ref object StatusCode, [In, Out] ref bool Cancel)
        {}

        public void OnFullScreen([In] bool FullScreen)
        {}

        public void OnMenuBar([In] bool MenuBar)
        {}

        public void OnQuit()
        {}

        public void OnStatusBar([In] bool StatusBar)
        {}

        public void OnTheaterMode([In] bool TheaterMode)
        {}

        public void OnToolBar([In] bool ToolBar)
        {}

        public void OnVisible([In] bool Visible)
        {}

        public void PrintTemplateInstantiation([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp)
        {}

        public void PrintTemplateTeardown([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp)
        {}

        public void PrivacyImpactedStateChange([In] bool bImpacted)
        {}

        public void ProgressChange([In] int Progress, [In] int ProgressMax)
        {}

        public void PropertyChange([In, MarshalAs(UnmanagedType.BStr)] string szProperty)
        {}

        public void SetSecureLockIcon([In] int SecureLockIcon)
        {}

        public void StatusTextChange([In, MarshalAs(UnmanagedType.BStr)] string Text)
        {}

        public void TitleChange([In, MarshalAs(UnmanagedType.BStr)] string Text)
        {}

        public void UpdatePageStatus([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object nPage, [In, MarshalAs(UnmanagedType.Struct)] ref object fDone)
        {}

        public void WindowClosing([In] bool IsChildWindow, [In, Out] ref bool Cancel)
        {}

        public void WindowSetHeight([In] int Height)
        {}

        public void WindowSetLeft([In] int Left)
        {}

        public void WindowSetResizable([In] bool Resizable)
        {}

        public void WindowSetTop([In] int Top)
        {}

        public void WindowSetWidth([In] int Width)
        {}

        void DWebBrowserEvents2.FileDownload(ref bool Cancel)
        {}
        #endregion
    }
}
