using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Alba.Interop.CommCtrl;
using Alba.Interop.ShellApi;

namespace Alba.Interop.CommonControls
{
    public class NativeImageList : IDisposable
    {
        private IImageList _imageList;
        private List<ImageSource> _iconImageSources;

        private NativeImageList (IImageList imageList)
        {
            _imageList = imageList;
            _iconImageSources = new List<ImageSource>(ImageCount);
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

        public ImageSource GetIconImageSource (int index)
        {
            return GetCachedIconImageSource(index) ?? GetIconImageSourceInternal(index);
        }

        private ImageSource GetIconImageSourceInternal (int index)
        {
            IntPtr hicon;
            _imageList.GetIcon(index, ILD.IMAGE, out hicon);
            BitmapSource img = Imaging.CreateBitmapSourceFromHIcon(hicon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            Native.DestroyIcon(hicon);
            SetCachedIconImageSource(index, img);
            return img;
        }

        private ImageSource GetCachedIconImageSource (int index)
        {
            return index < _iconImageSources.Count ? _iconImageSources[index] : null;
        }

        private void SetCachedIconImageSource (int index, ImageSource img)
        {
            if (index >= _iconImageSources.Capacity * 2)
                _iconImageSources.Capacity = index;
            while (_iconImageSources.Count <= index)
                _iconImageSources.Add(null);
            _iconImageSources[index] = img;
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

        private void Dispose (bool disposing)
        {
            if (_imageList == null)
                return;
            Marshal.ReleaseComObject(_imageList);
            _imageList = null;
            if (disposing) {
                _iconImageSources = null;
            }
        }
    }
}