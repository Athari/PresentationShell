﻿using System;
using System.Runtime.InteropServices;
using System.Text;

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

        public ushort Cb
        {
            get { return (ushort)Marshal.ReadInt16(Handle); }
        }

        public PIDLIST (IntPtr handle) : this()
        {
            _handle = handle;
        }

        public override string ToString ()
        {
            var sb = new StringBuilder();
            int i = 0;
            for (PIDLIST pidl = this; !Native.ILIsEmpty(pidl); pidl = Native.ILNext(pidl)) {
                byte[] data = new byte[pidl.Cb];
                Marshal.Copy(Handle, data, 2, pidl.Cb - 2);
                sb.AppendFormat("{0} ({1} bytes): {2} ({3})\n", i++, pidl.Cb,
                    BitConverter.ToString(data).Replace("-", ""),
                    Encoding.ASCII.GetString(data).Replace('\0', ' '));
            }
            return sb.ToString();
        }
    }
}