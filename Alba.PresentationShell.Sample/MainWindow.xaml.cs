using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Alba.Interop;
using Alba.Interop.ShellApi;
using Alba.Interop.ShellObjects;
using Alba.Windows.Media;

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
            var sb = new StringBuilder();
            var iconList = new ShellIconList();

            using (var folderDesktop = NativeShellFolder.GetDesktopFolder())
                //using (var folderPidl = folderDesktop.ParseDisplayName(/*"C:\\Windows\\System32"*/"C:\\"))
                //using (var folderPidl = folderDesktop.ParseDisplayName("::{26EE0668-A00A-44D7-9371-BEB064C98683}"))
            using (var folderPidl = folderDesktop.ParseDisplayName(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories"))
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
                    JumboIcons.Add(iconList.GetShellIconOverlay(folderIconOverlay, pidl, SHIL.JUMBO));
                    JumboIcons.Add(folderIcon != null
                        ? iconList.GetShellIcon(folderIcon, pidl, SHIL.JUMBO, GILI.FORSHELL)
                        : iconList.ExtractIcon(folder, pidl, 256, GILI.FORSHELL));
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