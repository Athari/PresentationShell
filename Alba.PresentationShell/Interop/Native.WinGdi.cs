using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Alba.Interop
{
    using HGDIOBJ = IntPtr;

    internal partial class Native
    {
        [DllImport (Dll.Gdi, SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "DeleteObject")]
        private static extern bool DeleteObjectInternal (HGDIOBJ hObject);

        public static void DeleteObject (HGDIOBJ hObject)
        {
            if (!DeleteObjectInternal(hObject))
                throw new Win32Exception();
        }
    }
}