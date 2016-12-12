using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
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
        public XmlDataProvider XdataProvider;
        public static MainWindow CurrentInstance;
        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
            XdataProvider = (XmlDataProvider)this.FindResource("xmldata");
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

            OmdbLookup2View(new OmdbRequestBuilder(result.Title, RequestBy.Title, PlotType.Full));
            
        }

        private void SeriesControl_OnWatchClicked( object sender, RoutedEventArgs e )
        {
            watchHandler.AddWatch((OmdbResult)sender);
            XdataProvider.Refresh();
        }

        private void EpisodeCheckChanged(object sender, RoutedEventArgs e)
        {
            XdataProvider.Document.Save("UserData.xml");
        }

        private void OnSeriesDelete(object sender, RoutedEventArgs e)
        {
            string IMDBID = ((XmlElement) ((MenuItem) e.Source).DataContext).Attributes["IMDBID"].Value;
            watchHandler.RemoveWatch(IMDBID);
            XdataProvider.Refresh();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string IMDBID = ((XmlElement)e.NewValue).Attributes["IMDBID"]?.Value ?? "";
            if (IMDBID == "") return;
            OmdbLookup2View(new OmdbRequestBuilder(IMDBID, RequestBy.ImdbId, PlotType.Full));
        }

        private void SeasonCheckChanged(object sender, RoutedEventArgs e)
        {
            XmlElement seasonElement = (XmlElement)(((CheckBox)e.Source).DataContext);
            foreach (XmlElement childnode in seasonElement.FirstChild.ChildNodes)
            {
                
            } 
        }

        private void OmdbLookup2View(OmdbRequestBuilder builder)
        {
            var con = new OmdbConnection();
            var root = con.GetResult(builder);
            var fullFilePath = root.Poster;
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();
                seriesControl.OmdbResultObj = root;
                seriesControl.imgPoster.Source = bitmap;
            }
            catch (Exception)
            {
            }
        }
    }
}

