using System;
using System.Runtime.InteropServices;
using Alba.Interop.CommCtrl;

namespace Alba.Interop.CommonControls
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct IMAGELISTDRAWPARAMS
    {
        public uint cbSize;
        [MarshalAs (UnmanagedType.IUnknown)]
        public object himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;
        public int yBitmap;
        public uint rgbBk;
        public uint rgbFg;
        public ILD fStyle;
        public uint dwRop;
        public ILS fState;
        public uint Frame;
        public uint crEffect;
    }
}