using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Watchlist.Controls
{
    /// <summary>
    /// Interaction logic for LoadableImage.xaml
    /// </summary>
    public partial class LoadableImage : UserControl,INotifyPropertyChanged
    {
        private bool _noImageLoaded = true;

        public LoadableImage(Uri source) : this()
        {
            LoadImage(source);
        }

        private BitmapImage bitmapimage;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool NeedsDownload = true;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void LoadImage(Uri imageSource)
        {
            bitmapimage.BeginInit();
            bitmapimage.UriSource = imageSource;
            bitmapimage.EndInit();
            if (!bitmapimage.IsDownloading)
            {
                NoImageLoaded = false;
            }
            imgCtrl.Source = bitmapimage;
        }

        public LoadableImage()
        {
            InitializeComponent();
            bitmapimage = new BitmapImage();
            bitmapimage.DownloadCompleted += (sender, args) =>
            {
                NoImageLoaded = false;
                NeedsDownload = false;
            };
        }


        public bool NoImageLoaded
        {
            get { return _noImageLoaded; }
            set
            {
                _noImageLoaded = value;
                OnPropertyChanged();
            }
        }
    }
}
