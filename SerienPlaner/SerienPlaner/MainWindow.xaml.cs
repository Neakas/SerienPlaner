using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SerienPlaner.OMDBwrapper;
using dragonz.actb.core;
using NeaUtils.Extensions.XmlExtensions;
using SerienPlaner.Json;
using SerienPlaner.WatchData;

namespace SerienPlaner
{
    public partial class MainWindow
    {
        private AutoCompleteManager _acmOmdb;
        private WatchHandler watchHandler;
        public MainWindow()
        {
            InitializeComponent();
            watchHandler = new WatchHandler();
            InitControls();
        }

        private void InitControls()
        {
            _acmOmdb = new AutoCompleteManager(tbSearch)
            {
                DataProvider = new OmdbSuggestionProvider(),
                Asynchronous = true
            };
            tbSearch.KeyDown += tbSearch_KeyDown;
        }

        private void tbSearch_KeyDown( object sender, KeyEventArgs e )
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            var result = ( (OmdbResult) _acmOmdb.DataProvider.ResultObject ).Search.First(x => ( (TextBox) sender ).Text == x.Title);
            var con = new OmdbConnection();
            var root = con.GetResult(new OmdbRequestBuilder(result.Title, RequestBy.Title,PlotType.Full));
            var fullFilePath = root.Poster;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            seriesControl.OmdbResultObj = root;
            seriesControl.imgPoster.Source = bitmap;
        }

        private void SeriesControl_OnWatchClicked( object sender, RoutedEventArgs e )
        {
            watchHandler.AddWatch((OmdbResult)sender);
        }
    }
}
