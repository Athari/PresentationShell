using System;
using System.Runtime.InteropServices;

namespace Alba.Interop.ShellObjects
{
    [StructLayout (LayoutKind.Sequential)]
    internal struct PIDLIST
    {
        private readonly IntPtr _handle;

        public IntPtr Handle
        {
            get { return _handle; }
        }

        public short MkidCb
        {
            get { return Marshal.ReadInt16(Handle); }
        }

        public PIDLIST (IntPtr handle) : this()
        {
            _handle = handle;
        }
    }
}