using System;
using System.Runtime.InteropServices;
using Alba.Interop.WinDef;

namespace Alba.Interop.CommonControls
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public RECT rcImage;
    }
}