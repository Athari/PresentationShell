using System.Runtime.InteropServices;

namespace Alba.Interop.WinDef
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}