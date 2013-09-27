using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Alba.Framework.Collections;
using Alba.Framework.Sys;
using Alba.Interop;
using Alba.Interop.CommonControls;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;
using Alba.Interop.WinUser;

namespace Alba.Windows.Media
{
    using HICON = IntPtr;

    internal class ShellIconList
    {
        private readonly Dictionary<IconLocation, IconData> _icons = new Dictionary<IconLocation, IconData>();
        private readonly ShellImageListDictionary _imageLists = new ShellImageListDictionary();

        private ShellImageList GetImageList (SHIL iconSize)
        {
            return _imageLists.GetOrAdd(iconSize, () => new ShellImageList(iconSize));
        }

        public ImageSource ExtractIcon (NativeShellFolder shellFolder, PIDLIST pidl, int iconSize, GILI iconFlags)
        {
            using (NativeExtractIcon extractIcon = shellFolder.GetUIObjectOf<IExtractIcon>(pidl).ToNative()) {
                string iconFile;
                int iconIndex;
                GILR iconResultFlags;
                if (!extractIcon.GetIconLocation(iconFlags, out iconFile, out iconIndex, out iconResultFlags).GetValueOrDefault())
                    return null; // TODO return default icon, schedule async icon extraction

                HICON hicon;
                // TODO check cache
                /*if (!iconResultFlags.Has(GILR.NOTFILENAME))
                    Native.PrivateExtractIcons(iconFile, iconIndex, iconSize, out hicon, LR.LOADFROMFILE);*/
                if (!extractIcon.Extract(iconFile, iconIndex, (ushort)iconSize, out hicon) && !iconResultFlags.Has(GILR.NOTFILENAME))
                    Native.PrivateExtractIcons(iconFile, iconIndex, iconSize, out hicon, LR.LOADFROMFILE);
                if (hicon == IntPtr.Zero)
                    return null;

                return Native.CreateBitmapSourceFromHIcon(hicon);
            }
        }

        public ImageSource GetShellIcon (NativeShellIcon shellIcon, PIDLIST pidl, SHIL iconSize, GILI iconFlags)
        {
            int index = shellIcon.GetIconOf(pidl, iconFlags);
            return index >= 0 ? GetImageList(iconSize).GetIconImageSource(index) : null;
        }

        public ImageSource GetShellIconOverlay (NativeShellIconOverlay shellIconOverlay, PIDLIST pidl, SHIL iconSize)
        {
            try {
                int index = shellIconOverlay.GetOverlayIconIndex(pidl);
                return index >= 0 ? GetImageList(iconSize).GetIconImageSource(index) : null;
            }
            catch (COMException) { // TODO Log error
                return null;
            }
            catch (ArgumentException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }
        }

        private struct IconLocation
        {
            public string FileName;
            public int Index;
            public GILI IconFlags;
            public SHIL Size;

            public IconLocation (string fileName, int index, GILI iconFlags, SHIL size)
            {
                FileName = string.Intern(Path.GetFullPath(fileName));
                Index = index;
                IconFlags = iconFlags;
                Size = size;
            }
        }

        private struct IconData
        {}

        private class ShellImageList
        {
            private readonly NativeImageList _imageList;
            private readonly IndexDictionary<ImageSource> _imageSourceCache;

            public ShellImageList (SHIL iconSize)
            {
                _imageList = NativeImageList.GetShellImageList(iconSize);
                _imageSourceCache = new IndexDictionary<ImageSource>();
            }

            public ImageSource GetIconImageSource (int index)
            {
                return _imageSourceCache.GetOrAdd(index, () => Native.CreateBitmapSourceFromHIcon(_imageList.GetIcon(index)));
                //return Native.CreateBitmapSourceFromHIcon(_imageList.GetIcon(index));
            }
        }

        private class ShellImageListDictionary : IndexDictionaryBase<SHIL, ShellImageList>
        {
            public ShellImageListDictionary () : base((int)(SHIL.LAST + 1))
            {}

            protected override int KeyToIndex (SHIL key)
            {
                return (int)key;
            }

            protected override SHIL IndexToKey (int index)
            {
                return (SHIL)index;
            }
        }
    }
}