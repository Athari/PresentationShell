using System;
using System.Runtime.InteropServices;

namespace Alba.Interop.ShellTypes
{
    [StructLayout (LayoutKind.Explicit)]
    internal struct STRRET
    {
        [FieldOffset (0)]
        public uint uType;
        [FieldOffset (4)]
        public IntPtr pOleStr;
        [FieldOffset (4)]
        public IntPtr pStr;
        [FieldOffset (4)]
        public uint uOffset;
        [FieldOffset (4)]
        public IntPtr cStr;
    }
}