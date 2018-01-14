using System;
using System.Collections.Generic;
using System.Linq;
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
using MahApps.Metro.Controls;
using TVDBSharp.Models;
using Watchlist.TvDbwrapper.JsonClasses;

namespace Watchlist.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : MetroWindow
    {
        public List<Show> SearchSeries { get; set; }

        public SearchControl(List<Show> searchSeries)
        {
            InitializeComponent();
            SearchSeries = searchSeries; 
            dgDataList.AutoGenerateColumns = false;
            dgDataList.ItemsSource = SearchSeries;
            dgDataList.SelectionChanged += (sender, args) =>
            {
                try
                {
                    Show show = ((Show)args.AddedItems[0]);
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = show.Banner;
                    img.EndInit();
                    imgbanner.Source = img;
                    tbOverview.Text = show.Description;
                    BitmapImage imgposter = new BitmapImage();
                    imgposter.BeginInit();
                    imgposter.UriSource = show.Poster;
                    imgposter.EndInit();
                    imgPoster.Source = imgposter;
                }
                catch (Exception e)
                {
                    return; // HAHA :D 
                }
                

              
            };
            dgDataList.MouseDoubleClick += (sender, args) =>
            {
                if (((DataGrid)sender)?.CurrentCell == null) return;
                MainWindow.CurrentInstance.SeriesControl_OnWatchClicked((Show)(((DataGrid)sender).CurrentCell.Item));
                Close();
            };
        }
    }
}
