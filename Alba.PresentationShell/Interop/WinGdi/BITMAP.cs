using System;
using System.Runtime.InteropServices;

namespace Alba.Interop.WinGdi
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;
        public uint cbSize;
        public IntPtr pBuffer;
    }
}