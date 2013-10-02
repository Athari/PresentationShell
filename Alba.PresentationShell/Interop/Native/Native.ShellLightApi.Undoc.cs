using System;
using System.Runtime.InteropServices;
using Alba.Interop.ShellObjects;

namespace Alba.Interop
{
    using HWND = IntPtr;

    internal partial class Native
    {
        /// <summary>Get and show a context menu from a shell folder.</summary>
        /// <param name="hWnd">Window displaying the shell folder.</param>
        /// <param name="lpFolder">IShellFolder interface.</param>
        /// <param name="lpApidl">Id for the particular folder desired.</param>
        /// <remarks>See http://source.winehq.org/WineAPI/SHInvokeDefaultCommand.html </remarks>
        [DllImport(Dll.ShellLight, EntryPoint = "#279", PreserveSig = false)]
        public static extern void SHInvokeDefaultCommand ([In] HWND hWnd, [In] IShellFolder lpFolder, [In] PIDLIST lpApidl);
    }
}