using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

    public class WrapView : ViewBase
    {
        public static readonly DependencyProperty ItemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(WrapView));
        public static readonly DependencyProperty ItemTemplateProperty = ItemsControl.ItemTemplateProperty.AddOwner(typeof(WrapView));
        public static readonly DependencyProperty ItemWidthProperty = WrapPanel.ItemWidthProperty.AddOwner(typeof(WrapView));
        public static readonly DependencyProperty ItemHeightProperty = WrapPanel.ItemHeightProperty.AddOwner(typeof(WrapView));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        protected override object DefaultStyleKey
        {
            get { return new ComponentResourceKey(GetType(), "styWrapView"); }
        }
    }
}