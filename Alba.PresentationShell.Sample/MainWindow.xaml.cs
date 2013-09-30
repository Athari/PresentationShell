﻿using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Alba.Interop;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;
using Alba.Windows.Media;
using Alba.Windows.Shell;

namespace Alba.PresentationShell.Sample
{
    public partial class MainWindow
    {
        public ObservableCollection<ImageSource> ShellIcons { get; set; }
        public ShellItem Desktop { get; set; }

        public MainWindow ()
        {
            ShellIcons = new ObservableCollection<ImageSource>();
            Desktop = new ShellTree().Desktop;
            Desktop.IsSelected = true;

            InitializeComponent();

            Loaded += OnLoaded;
            SourceInitialized += OnSourceInitialized;
        }

        private void OnSourceInitialized (object sender, EventArgs eventArgs)
        {
            Desktop.Tree.WindowHandle = new WindowInteropHelper(this).Handle;
        }

        private void OnLoaded (object sender, RoutedEventArgs routedEventArgs)
        {
            //Tmp();
            UpdateShellIconsList();
        }

        private void Tmp ()
        {
            /*IntPtr ptr0 = Marshal.AllocCoTaskMem(sizeof(Int64));
            Marshal.WriteInt64(ptr0, 0);
            PIDLIST pidl0 = new PIDLIST(ptr0);
            PIDLIST pidlW = Native.ILCreateFromPath("C:\\Windows");
            PIDLIST pidlN = Native.ILCreateFromPath("C:\\Windows\\Notepad.exe");
            Native.ILAppendID(pidl0, pidl0, true);
            Native.ILClone(pidl0);
            Native.ILCloneFirst(pidl0);
            Native.ILCombine(pidl0, pidl0);
            MessageBox.Show(this, string.Format(
                "Notepad.exe: Size={0}\n" +
                "N is W child: {1}" +
                " pidl0: {2}\n pidlW: {3}\n pidlN: {4}\n",
                Native.ILGetSize(pidlN),
                Native.ILFindChild(pidlW, pidlN),
                pidl0, pidlW, pidlN
                ));*/

            /*IntPtr ptr0 = Marshal.AllocCoTaskMem(2);
            Marshal.WriteInt16(ptr0, 0);
            PIDLIST pidl0 = new PIDLIST(ptr0);
            MessageBox.Show(this, NativeShellFolder.GetDesktopFolder().GetDisplayNameOf(pidl0, SHGDN.NORMAL));*/
        }

        private void UpdateShellIconsList ()
        {
            ShellIcons.Clear();

            const SHIL iconSize = SHIL.SMALL;
            var sb = new StringBuilder();
            var iconList = new ShellIconList();

            using (var folderDesktop = NativeShellFolder.GetDesktopFolder())
            using (var folderPidl = folderDesktop.ParseDisplayName( /*"C:\\Windows\\System32"*/"C:\\"))
                //using (var folderPidl = folderDesktop.ParseDisplayName("::{26EE0668-A00A-44D7-9371-BEB064C98683}"))
                //using (var folderPidl = folderDesktop.ParseDisplayName(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories"))
            using (var folder = folderDesktop.BindToObject<IShellFolder>(folderPidl).ToNative())
            using (var folderIcon = folder.QueryInterface<IShellIcon>().ToNative())
            using (var folderIconOverlay = folder.QueryInterface<IShellIconOverlay>().ToNative()) {
                /*using (var folder = NativeShellFolder.GetDesktopFolder())
            using (var folderIcon = folder.QueryInterface<IShellIcon>().ToNative())
            using (var folderIconOverlay = folder.QueryInterface<IShellIconOverlay>().ToNative()) {*/
                foreach (PIDLIST pidl in folder.EnumObjects(IntPtr.Zero, SHCONTF.FOLDERS | SHCONTF.NONFOLDERS)) {
                    sb.AppendFormat("{0} - {1}\n",
                        folder.GetDisplayNameOf(pidl, SHGDN.NORMAL),
                        folder.GetDisplayNameOf(pidl, SHGDN.FORPARSING));
                    ShellIcons.Add(folderIcon != null
                        ? iconList.GetShellIcon(folderIcon, pidl, iconSize, GILI.FORSHELL)
                        : iconList.ExtractIcon(folder, pidl, iconSize, GILI.FORSHELL));
                    ShellIcons.Add(iconList.GetShellIconOverlay(folderIconOverlay, pidl, iconSize));
                    pidl.Dispose();
                }
            }
            //MessageBox.Show(this, sb.ToString());

            //new OpenFileDialog().ShowDialog();

            //var jumboImageList = NativeImageList.GetShellImageList(SHIL.JUMBO);
            //JumboIcons.AddRange(Enumerable.Range(0, jumboImageList.ImageCount).Select(jumboImageList.GetIconImageSource));
        }
    }
}