using System;

namespace Alba.Interop
{
    internal partial class Native
    {
        /// <summary>Prepares the index of an overlay mask so that the ImageList_Draw function can use it.</summary>
        /// <param name="iOverlay">An index of an overlay mask.</param>
        public static Int32 IndexToOverlayMask (Int32 iOverlay)
        {
            return iOverlay << 8;
        }
    }
}