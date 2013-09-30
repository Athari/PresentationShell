using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Alba.Interop.ShellObjects
{
    [StructLayout (LayoutKind.Sequential)]
    internal struct PIDLIST : IDisposable
    {
        private static readonly PIDLIST _empty;

        private readonly IntPtr _handle;

        static PIDLIST ()
        {
            IntPtr ptrEmpty = Marshal.AllocCoTaskMem(4);
            Marshal.WriteInt32(ptrEmpty, 0);
            _empty = new PIDLIST(ptrEmpty);
        }

        public PIDLIST (IntPtr handle)
        {
            _handle = handle;
        }

        public static PIDLIST Empty
        {
            get { return _empty; }
        }

        public IntPtr Handle
        {
            get { return _handle; }
        }

        public ushort Cb
        {
            get { return (ushort)Marshal.ReadInt16(Handle); }
        }

        public bool IsEmpty
        {
            get { return Native.ILIsEmpty(this); }
        }

        public void Dispose ()
        {
            Marshal.FreeCoTaskMem(Handle);
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