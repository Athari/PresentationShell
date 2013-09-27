using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Alba.Interop
{
    using HICON = IntPtr;

    internal partial class Native
    {
        public static BitmapSource CreateBitmapSourceFromHIcon (HICON hicon, bool destroyIcon = true)
        {
            BitmapSource imageSource = Imaging.CreateBitmapSourceFromHIcon(hicon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            imageSource.Freeze();
            if (destroyIcon)
                DestroyIcon(hicon);
            return imageSource;
        }
    }
}