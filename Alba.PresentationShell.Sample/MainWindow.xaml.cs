using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
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
            Desktop.IsExpanded = true;
            Desktop.ChildrenFolders[0].IsExpanded = true;
            Desktop.ChildrenFolders[0].IsSelected = true;

            Icon = Desktop.IconLarge;

            InitializeComponent();

            SourceInitialized += OnSourceInitialized;
        }

        private void OnSourceInitialized (object sender, EventArgs eventArgs)
        {
            Desktop.Tree.WindowHandle = new WindowInteropHelper(this).Handle;
        }

        private void TvwShell_OnSelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Cursor = Cursors.Wait;
            Dispatcher.InvokeAsync(() => { Cursor = Cursors.Arrow; }, DispatcherPriority.ContextIdle);
        }

        private void ShellItem_OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            var item = (ShellItem)((FrameworkElement)sender).DataContext;
            if (e.ClickCount == 2)
                item.InvokeDefault();
        }
    }
}