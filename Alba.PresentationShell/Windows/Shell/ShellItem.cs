using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Alba.Framework.Collections;
using Alba.Framework.Sys;
using Alba.Framework.Windows.Mvvm;
using Alba.Interop;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;

namespace Alba.Windows.Shell
{
    public class ShellItem : ModelBase<ShellItem>, IDisposable
    {
        internal static readonly ObservableCollectionEx<ShellItem> DummyChildren = new ObservableCollectionEx<ShellItem> {
            new ShellItem()
            //new ShellItem(null, null, PIDLIST.Empty, NativeShellFolder.GetDesktopFolder())
        };
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
                foreach (PIDLIST childPidl in _shellFolder.EnumObjects(IntPtr.Zero, SHCONTF.FOLDERS | SHCONTF.NONFOLDERS)) {
                    NativeShellFolder childShellFolder = _shellFolder.BindToObject<IShellFolder>(childPidl).ToNative();
                    ShellItem childItem = new ShellItem(_tree, this, childPidl, childShellFolder);
                    if (!childItem.HasSubFolders)
                        childItem._children = NoChildren;
                    _children.Add(childItem);
                }
                OnPropertyChanged("Children");
            }
        }

        private ImageSource GetIconOverlay (SHIL iconSize)
        {
            NativeShellIconOverlay shellIconOverlay = ParentShellFolder.QueryInterface<IShellIconOverlay>().ToNative(false);
            return shellIconOverlay != null ? _tree.IconList.GetShellIconOverlay(shellIconOverlay, _pidl, iconSize) : null;
        }

        private ImageSource GetIcon (SHIL iconSize, GILI iconAttrs)
        {
            NativeShellIcon shellIcon = ParentShellFolder.QueryInterface<IShellIcon>().ToNative(false);
            return shellIcon != null
                ? _tree.IconList.GetShellIcon(shellIcon, _pidl, iconSize, iconAttrs)
                : _tree.IconList.ExtractIcon(ParentShellFolder, _pidl, iconSize, iconAttrs);
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