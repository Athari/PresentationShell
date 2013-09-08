using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Alba.Interop
{
    using HICON = IntPtr;

    internal partial class Native
    {
        [DllImport (Dll.User, SetLastError = true, EntryPoint = "DestroyIcon")]
        private static extern bool DestroyIconInternal (HICON hIcon);

        public static void DestroyIcon (HICON hIcon)
        {
            if (!DestroyIconInternal(hIcon))
                throw new Win32Exception();
        }
    }
}