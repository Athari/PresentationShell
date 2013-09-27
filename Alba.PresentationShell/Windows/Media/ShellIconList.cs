using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
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
        private readonly List<NativeImageList> _imageLists;

        public ShellIconList ()
        {
            _imageLists = Enumerable.Range(0, (int)(SHIL.LAST + 1)).Select(i => (NativeImageList)null).ToList();
        }

        public NativeImageList GetImageList (SHIL iconSize)
        {
            return _imageLists[(int)iconSize] ?? (_imageLists[(int)iconSize] = NativeImageList.GetShellImageList(iconSize));
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
            return index != -1 ? GetImageList(iconSize).GetIconImageSource(index) : null;
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
    }
}