using System.Runtime.InteropServices;

namespace Alba.Interop.CommonControls
{
    [StructLayout (LayoutKind.Sequential, Pack = 4)]
    internal struct IMAGELISTSTATS
    {
        public uint cbSize;
        public int cAlloc;
        public int cUsed;
        public int cStandby;
    }
}