using System;
using Alba.Interop.CommCtrl;

namespace Alba.Interop.CommonControls
{
    using HICON = IntPtr;

    internal class NativeImageList : NativeComInterface<IImageList>
    {
        public NativeImageList (IImageList com) : base(com)
        {}

        public int ImageCount
        {
            get
            {
                int res;
                Com.GetImageCount(out res);
                return res;
            }
        }

        public HICON GetIcon (int index)
        {
            HICON hicon;
            Com.GetIcon(index, ILD.IMAGE, out hicon);
            return hicon;
        }
    }
}