using System;
using System.Runtime.InteropServices;
using Alba.Interop.CommCtrl;
using Alba.Interop.ShellApi;

namespace Alba.Interop.CommonControls
{
    using HICON = IntPtr;

    internal class NativeImageList : IDisposable
    {
        private IImageList _imageList;

        public NativeImageList (IImageList imageList)
        {
            _imageList = imageList;
        }

        public static NativeImageList GetShellImageList (SHIL shil)
        {
            return new NativeImageList(Native.SHGetImageList(shil));
        }

        public int ImageCount
        {
            get
            {
                int res;
                _imageList.GetImageCount(out res);
                return res;
            }
        }

        public HICON GetIcon (int index)
        {
            HICON hicon;
            _imageList.GetIcon(index, ILD.IMAGE, out hicon);
            return hicon;
        }

        ~NativeImageList ()
        {
            Dispose(false);
        }

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose (bool disposing)
        {
            if (_imageList == null)
                return;
            Marshal.ReleaseComObject(_imageList);
            _imageList = null;
        }
    }
}