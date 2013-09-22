using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Alba.Framework.Collections;
using Alba.Interop;
using Alba.Interop.CommonControls;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;
using Microsoft.Win32;

namespace Alba.PresentationShell.Sample
{
    public partial class MainWindow
    {
        public ObservableCollection<ImageSource> JumboIcons { get; set; }

        public MainWindow ()
        {
            JumboIcons = new ObservableCollection<ImageSource>();

            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded (object sender, RoutedEventArgs routedEventArgs)
        {
            IntPtr ptr0 = Marshal.AllocCoTaskMem(sizeof(Int64));
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
                ));

            new OpenFileDialog().ShowDialog();

            var _jumboShellImageList = NativeImageList.GetShellImageList(SHIL.JUMBO);
            JumboIcons.AddRange(Enumerable.Range(0, _jumboShellImageList.ImageCount).Select(_jumboShellImageList.GetIconImageSource));
        }
    }
}