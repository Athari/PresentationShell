using System.Runtime.InteropServices;

namespace Alba.Interop.WinDef
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct POINT
    {
        public int x;
        public int y;
    }
}