using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Alba.Framework.Collections;
using Alba.Interop.CommonControls;
using Alba.Interop.ShellApi;
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
            new OpenFileDialog().ShowDialog();

            var _jumboShellImageList = NativeImageList.GetShellImageList(SHIL.JUMBO);
            JumboIcons.AddRange(Enumerable.Range(0, _jumboShellImageList.ImageCount).Select(_jumboShellImageList.GetIconImageSource));
        }
    }
}