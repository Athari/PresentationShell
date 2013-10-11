using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using Alba.Diagnostics;
using Alba.Framework.Collections;
using Alba.Framework.Diagnostics;
using Alba.Framework.Sys;
using Alba.Framework.Text;
using Alba.Framework.Windows.Mvvm;
using Alba.Interop;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Alba.Windows.Shell
{
    public class ShellItem : ModelBase<ShellItem>, IDisposable
    {
        internal const int NoIconIndex = -1;
        internal const int AsyncIconIndex = -2;
        internal const int UndefinedIconIndex = -3;
        internal const int UnindexableIconIndex = -3;

        private static readonly ILog Log = GetLog(AlbaPresentaionShellTraceSources.Interop);
        internal static readonly ObservableCollectionEx<ShellItem> DummyChildren = new ObservableCollectionEx<ShellItem> { new ShellItem() };
        internal static readonly ObservableCollectionEx<ShellItem> NoChildren = new ObservableCollectionEx<ShellItem>();

        private ShellTree _tree;
        private ShellItem _parent;
        private PIDLIST _pidl;
        private NativeShellFolder _shellFolder;
        private ObservableCollectionEx<ShellItem> _children, _childrenFiles, _childrenFolders;
        private SFGAO _attrs, _attrsSet;
        private ShellItemState _state;
        private int _iconIndex = UndefinedIconIndex, _iconOverlayIndex = UndefinedIconIndex;

        private ShellItem ()
        {}

        internal ShellItem (ShellTree tree, ShellItem parent, PIDLIST pidl, NativeShellFolder shellFolder)
        {
            _tree = tree;
            _parent = parent;
            _pidl = pidl;
            _shellFolder = shellFolder;
            _children = _childrenFiles = _childrenFolders = DummyChildren;
            UpdateAttrs(SFGAO.HASSUBFOLDER | SFGAO.FILESYSTEM | SFGAO.FILESYSANCESTOR | SFGAO.FOLDER);
            if (!HasSubFolder)
                _childrenFolders = NoChildren;
        }

        public ShellTree Tree
        {
            get { return _tree; }
        }

        private NativeShellFolder ParentShellFolder
        {
            get { return (_parent ?? this)._shellFolder; }
        }

        private bool IsDesktop
        {
            get { return _pidl.IsEmpty; }
        }

        public bool HasSubFolder
        {
            get { return GetAttr(SFGAO.HASSUBFOLDER); }
        }

        public bool IsFileSystem
        {
            get { return GetAttr(SFGAO.FILESYSTEM); }
        }

        public bool IsFileSystemAncestor
        {
            get { return GetAttr(SFGAO.FILESYSANCESTOR); }
        }

        public bool IsFolder
        {
            get { return GetAttr(SFGAO.FOLDER); }
        }

        public bool IsSelected
        {
            get { return _state.Has(ShellItemState.IsSelected); }
            set { SetState(ShellItemState.IsSelected, value); }
        }

        public bool IsExpanded
        {
            get { return _state.Has(ShellItemState.IsExpanded); }
            set
            {
                if (SetState(ShellItemState.IsExpanded, value) && value)
                    ReplaceDummyChildren();
            }
        }

        public string DisplayName
        {
            get { return ParentShellFolder.GetDisplayNameOf(_pidl, SHGDN.NORMAL); }
        }

        public ImageSource IconSmall
        {
            get { return GetIcon(SHIL.SMALL, GILI.FORSHELL); }
        }

        public ImageSource IconLarge
        {
            get { return GetIcon(SHIL.LARGE, GILI.FORSHELL); }
        }

        public ImageSource IconExtraLarge
        {
            get { return GetIcon(SHIL.EXTRALARGE, GILI.FORSHELL); }
        }

        public ImageSource IconJumbo
        {
            get { return GetIcon(SHIL.JUMBO, GILI.FORSHELL); }
        }

        public ImageSource IconOverlaySmall
        {
            get { return GetIconOverlay(SHIL.SMALL); }
        }

        public ImageSource IconOverlayLarge
        {
            get { return GetIconOverlay(SHIL.LARGE); }
        }

        public ImageSource IconOverlayExtraLarge
        {
            get { return GetIconOverlay(SHIL.EXTRALARGE); }
        }

        public ImageSource IconOverlayJumbo
        {
            get { return GetIconOverlay(SHIL.JUMBO); }
        }

        public ObservableCollection<ShellItem> Children
        {
            get
            {
                ReplaceDummyChildren();
                return _children;
            }
        }

        public ObservableCollection<ShellItem> ChildrenUnexpanded
        {
            get { return _children; }
        }

        public ObservableCollection<ShellItem> ChildrenFiles
        {
            get
            {
                ReplaceDummyChildren();
                return _childrenFiles;
            }
        }

        public ObservableCollection<ShellItem> ChildrenFilesUnexpanded
        {
            get { return _childrenFiles; }
        }

        public ObservableCollection<ShellItem> ChildrenFolders
        {
            get
            {
                ReplaceDummyChildren();
                return _childrenFolders;
            }
        }

        public ObservableCollection<ShellItem> ChildrenFoldersUnexpanded
        {
            get { return _childrenFolders; }
        }

        private bool GetAttr (SFGAO attr)
        {
            if (!_attrsSet.Has(attr))
                UpdateAttrs(attr);
            return _attrs.Has(attr);
        }

        private bool SetState (ShellItemState state, bool value, [CallerMemberName] string propName = null)
        {
            return Set(ref _state, value ? (_state | state) : (_state & ~state), propName);
        }

        private void UpdateAttrs (SFGAO newAttrs)
        {
            newAttrs &= ~_attrsSet;
            _attrs |= ParentShellFolder.GetAttributesOf(_pidl, newAttrs);
            _attrsSet |= newAttrs;
        }

        private void ReplaceDummyChildren ()
        {
            if (_children == DummyChildren) {
                var children = new List<ShellItem>();
                try {
                    foreach (PIDLIST childPidl in _shellFolder.EnumObjects(_tree.WindowHandle, SHCONTF.FOLDERS | SHCONTF.NONFOLDERS)) {
                        NativeShellFolder childShellFolder = _shellFolder.BindToObject<IShellFolder>(childPidl).ToNative();
                        ShellItem childItem = new ShellItem(_tree, this, childPidl, childShellFolder);
                        children.Add(childItem);
                    }
                }
                catch (Exception e) {
                    if (e.IsAnyType<FileNotFoundException, Win32Exception>())
                        Log.Error("Failed to enumerate items of folder '{0}'.".Fmt(DisplayName), e);
                }
                finally {
                    children.Sort(new ShellItemComparer(ParentShellFolder));
                    _children = new ObservableCollectionEx<ShellItem>();
                    _childrenFiles = new ObservableCollectionEx<ShellItem>();
                    _childrenFolders = new ObservableCollectionEx<ShellItem>();
                    foreach (ShellItem childItem in children) {
                        _children.Add(childItem);
                        if (childItem.IsFolder)
                            _childrenFolders.Add(childItem);
                        else
                            _childrenFiles.Add(childItem);
                    }
                }
                OnPropertyChanged("Children", "ChildrenUnexpanded",
                    "ChildrenFiles", "ChildrenFilesUnexpanded",
                    "ChildrenFolders", "ChildrenFoldersUnexpanded");
            }
        }

        private ImageSource GetIconOverlay (SHIL iconSize)
        {
            return _tree.IconList.GetIconByIndex(iconSize, GetIconOverlayIndex());
        }

        private int GetIconOverlayIndex ()
        {
            if (_iconOverlayIndex != UndefinedIconIndex)
                return _iconOverlayIndex;
            if (IsDesktop)
                return _iconOverlayIndex = NoIconIndex;

            // TODO FIX GetIconOverlayIndex!!! (interface always null)
            Task.Run(() => {
                int iconOverlayIndex;
                using (NativeShellIconOverlay shellIconOverlay = ParentShellFolder.QueryInterface<IShellIconOverlay>().ToNative())
                    iconOverlayIndex = shellIconOverlay != null ? _tree.IconList.GetIconOverlayIndex(shellIconOverlay, _pidl) : NoIconIndex;
                Dispatcher.InvokeAsync(() =>
                    Set(ref _iconOverlayIndex, iconOverlayIndex, "IconOverlaySmall", "IconOverlayLarge", "IconOverlayExtraLarge", "IconOverlayJumbo"));
            });
            return NoIconIndex;
        }

        private ImageSource GetIcon (SHIL iconSize, GILI iconAttrs)
        {
            int iconIndex = GetIconIndex(iconAttrs);
            return iconIndex != UnindexableIconIndex
                ? _tree.IconList.GetIconByIndex(iconSize, iconIndex)
                : _tree.IconList.ExtractIcon(ParentShellFolder, _pidl, iconSize, iconAttrs);
        }

        private int GetIconIndex (GILI iconAttrs)
        {
            if (_iconIndex != UndefinedIconIndex)
                return _iconIndex;
            if (IsDesktop)
                using (PIDLIST desktopPidl = Native.SHGetKnownFolderIDList(FOLDERID.Desktop))
                    return _iconIndex = Native.SHGetFileInfo(desktopPidl, SHGFI.SYSICONINDEX).iIcon;

            Task.Run(() => {
                int iconIndex;
                using (NativeShellIcon shellIcon = ParentShellFolder.QueryInterface<IShellIcon>().ToNative())
                    iconIndex = shellIcon != null ? _tree.IconList.GetIconIndex(shellIcon, _pidl, iconAttrs) : UnindexableIconIndex;
                Dispatcher.InvokeAsync(() =>
                    Set(ref _iconIndex, iconIndex, "IconSmall", "IconLarge", "IconExtraLarge", "IconJumbo"));
            });
            return NoIconIndex;
        }

        public void InvokeDefault ()
        {
            Native.SHInvokeDefaultCommand(_tree.WindowHandle, ParentShellFolder.Com, _pidl);
        }

        public void Dispose ()
        {
            _pidl.Dispose();
        }

        private class ShellItemComparer : IComparer<ShellItem>
        {
            private readonly NativeShellFolder _shellFolder;

            public ShellItemComparer (NativeShellFolder shellFolder)
            {
                _shellFolder = shellFolder;
            }

            public int Compare (ShellItem x, ShellItem y)
            {
                int cmp = -CompareBool(x.IsFileSystemAncestor && !x.IsFileSystem, y.IsFileSystemAncestor && !y.IsFileSystem);
                if (cmp != 0)
                    return cmp;
                cmp = -CompareBool(x.IsFileSystem, y.IsFileSystem);
                if (cmp != 0)
                    return cmp;
                cmp = -CompareBool(x.IsFileSystemAncestor, y.IsFileSystemAncestor);
                if (cmp != 0)
                    return cmp;
                cmp = -CompareBool(x.IsFolder, y.IsFolder);
                if (cmp != 0)
                    return cmp;
                /*try {
                    cmp = _shellFolder.CompareIDs(x._pidl, y._pidl);
                    if (cmp != 0)
                        return cmp;
                }
                catch {}*/
                return string.Compare(x.DisplayName, y.DisplayName, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
            }

            private static int CompareBool (bool x, bool y)
            {
                if (x == y)
                    return 0;
                else if (x)
                    return 1;
                else
                    return -1;
            }
        }

        [Flags]
        private enum ShellItemState
        {
            IsSelected = 1 << 0,
            IsExpanded = 1 << 1,
        }
    }
}