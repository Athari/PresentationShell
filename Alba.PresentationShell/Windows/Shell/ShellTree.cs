using System;
using Alba.Interop.ShellObjects;
using Alba.Windows.Media;

namespace Alba.Windows.Shell
{
    using HWND = IntPtr;

    public class ShellTree
    {
        private ShellItem _desktop;

        internal ShellIconList IconList { get; private set; }
        public HWND WindowHandle { get; set; }

        public ShellTree (HWND windowHandle)
        {
            IconList = new ShellIconList();
            WindowHandle = windowHandle;
        }

        public ShellTree () : this(IntPtr.Zero)
        {}

        public ShellItem Desktop
        {
            get { return _desktop ?? (_desktop = new ShellItem(this, null, PIDLIST.Empty, NativeShellFolder.GetDesktopFolder())); }
        }
    }
}