using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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

namespace Alba.Windows.Shell
{
    public class ShellItem : ModelBase<ShellItem>, IDisposable
    {
        private static readonly ILog Log = GetLog(AlbaPresentaionShellTraceSources.Interop);
        internal static readonly ObservableCollectionEx<ShellItem> DummyChildren = new ObservableCollectionEx<ShellItem> { new ShellItem() };
        internal static readonly ObservableCollectionEx<ShellItem> NoChildren = new ObservableCollectionEx<ShellItem>();

        private ShellTree _tree;
        private ShellItem _parent;
        private PIDLIST _pidl;
        private NativeShellFolder _shellFolder;
        private ObservableCollectionEx<ShellItem> _children;
        private SFGAO _attrs, _attrsSet;
        private ShellItemState _state;

        private ShellItem ()
        {}

        internal ShellItem (ShellTree tree, ShellItem parent, PIDLIST pidl, NativeShellFolder shellFolder)
        {
            _tree = tree;
            _parent = parent;
            _pidl = pidl;
            _shellFolder = shellFolder;
            _children = DummyChildren;
            UpdateAttrs(SFGAO.HASSUBFOLDER);
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

        public bool HasSubFolders
        {
            get { return GetAttr(SFGAO.HASSUBFOLDER); }
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

        public ImageSource IconSmallOpen
        {
            get { return GetIcon(SHIL.SMALL, GILI.FORSHELL | GILI.OPENICON); }
        }

        public ImageSource IconLargeOpen
        {
            get { return GetIcon(SHIL.LARGE, GILI.FORSHELL | GILI.OPENICON); }
        }

        public ImageSource IconExtraLargeOpen
        {
            get { return GetIcon(SHIL.EXTRALARGE, GILI.FORSHELL | GILI.OPENICON); }
        }

        public ImageSource IconJumboOpen
        {
            get { return GetIcon(SHIL.JUMBO, GILI.FORSHELL | GILI.OPENICON); }
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
            get { return _children; }
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
                _children = new ObservableCollectionEx<ShellItem>();
                try {
                    foreach (PIDLIST childPidl in _shellFolder.EnumObjects(_tree.WindowHandle, SHCONTF.FOLDERS | SHCONTF.NONFOLDERS)) {
                        NativeShellFolder childShellFolder = _shellFolder.BindToObject<IShellFolder>(childPidl).ToNative();
                        ShellItem childItem = new ShellItem(_tree, this, childPidl, childShellFolder);
                        if (!childItem.HasSubFolders)
                            childItem._children = NoChildren;
                        _children.Add(childItem);
                    }
                }
                catch (Exception e) {
                    if (e.IsAnyType<FileNotFoundException, Win32Exception>())
                        Log.Error("Failed to enumerate items of folder '{0}'.".Fmt(DisplayName), e);
                }
                OnPropertyChanged("Children");
            }
        }

        private ImageSource GetIconOverlay (SHIL iconSize)
        {
            if (IsDesktop)
                return null;
            NativeShellIconOverlay shellIconOverlay = ParentShellFolder.QueryInterface<IShellIconOverlay>().ToNative(false);
            return shellIconOverlay != null ? _tree.IconList.GetIconOverlay(shellIconOverlay, _pidl, iconSize) : null;
        }

        private ImageSource GetIcon (SHIL iconSize, GILI iconAttrs)
        {
            if (IsDesktop) {
                using (PIDLIST desktoPidl = Native.SHGetKnownFolderIDList(FOLDERID.Desktop))
                    return _tree.IconList.GetIconByIndex(iconSize, Native.SHGetFileInfo(desktoPidl, SHGFI.SYSICONINDEX).iIcon);
            }
            else {
                NativeShellIcon shellIcon = ParentShellFolder.QueryInterface<IShellIcon>().ToNative(false);
                return shellIcon != null
                    ? _tree.IconList.GetIcon(shellIcon, _pidl, iconSize, iconAttrs)
                    : _tree.IconList.ExtractIcon(ParentShellFolder, _pidl, iconSize, iconAttrs);
            }
        }

        public void Dispose ()
        {
            _pidl.Dispose();
        }

        [Flags]
        private enum ShellItemState
        {
            IsSelected = 1 << 0,
            IsExpanded = 1 << 1,
        }
    }
}