using System;
using System.Collections;
using System.Collections.Generic;
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
using SerienPlaner.Windows;

namespace SerienPlaner
{
    public partial class MainWindow
    {
        private AutoCompleteManager _acmOmdb;
        private readonly WatchHandler _watchHandler;
        public XmlDataProvider XdataProvider;
        public static MainWindow CurrentInstance;
        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
            XdataProvider = (XmlDataProvider)FindResource("Xmldata");
            _watchHandler = new WatchHandler();
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
            try
            {
                var result = ((OmdbResult)_acmOmdb.DataProvider.ResultObject).Search.First(x => ((TextBox)sender).Text == x.Title);

                OmdbLookup2View(new OmdbRequestBuilder(result.Title, RequestBy.Title, PlotType.Full));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SeriesControl_OnWatchClicked( object sender, RoutedEventArgs e )
        {
            _watchHandler.AddWatch((OmdbResult)sender);
            _watchHandler.Save();
            XdataProvider.Refresh();
        }



        private void OnSeriesDelete(object sender, RoutedEventArgs e)
        {
            var imdbid = ((XmlElement) ((MenuItem) e.Source).DataContext).Attributes["Imdbid"].Value;
            var result =
                MessageBox.Show(
                    "Are you Sure you want to Delete your Watch for '" +
                    ((XmlElement) ((MenuItem) e.Source).DataContext).Attributes["Title"].Value + "'", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    _watchHandler.RemoveWatch(imdbid);
                    XdataProvider.Refresh();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var imdbid = ((XmlElement) e.NewValue).Attributes["Imdbid"]?.Value ?? "";
            if (imdbid == "") return;
            OmdbLookup2View(new OmdbRequestBuilder(imdbid, RequestBy.ImdbId, PlotType.Full));
        }

        private void EpisodeCheckChanged(object sender, RoutedEventArgs e)
        {
            var episodeElement = (XmlElement) ((CheckBox) e.Source).DataContext;
            var seasonElement = (XmlElement) episodeElement?.ParentNode?.ParentNode;
            if (((CheckBox) sender).IsChecked == false)
            {
                seasonElement?.SetAttribute("Watched", "false");
            }
            else
            {
                var episodeelementlist = new List<XmlNode>(seasonElement?.FirstChild?.ChildNodes.Cast<XmlNode>());
                if (episodeelementlist.All(x => x.Attributes["Watched"].Value == "true")) seasonElement.SetAttribute("Watched", "true");
            }
            XdataProvider.Document.Save("UserData.xml");
        }

        private void SeasonCheckChanged(object sender, RoutedEventArgs e)
        {
            var seasonElement = (XmlElement) ((CheckBox) e.Source).DataContext;
            if (((CheckBox) sender).IsFocused)
            {
                if (((CheckBox) sender).IsChecked == true)
                {
                    seasonElement.SetAttribute("Watched", "true");
                    foreach (XmlElement childnode in seasonElement.FirstChild.ChildNodes)
                    {
                        childnode.SetAttribute("Watched", "true");
                    }
                }
                else
                {
                    seasonElement.SetAttribute("Watched", "false");
                    foreach (XmlElement childnode in seasonElement.FirstChild.ChildNodes)
                    {
                        childnode.SetAttribute("Watched", "false");
                    }
                }
            }

            var seriesElement = ((XmlElement) seasonElement?.ParentNode?.ParentNode);
            var seasonelementlist = new List<XmlNode>(seriesElement?.FirstChild?.ChildNodes.Cast<XmlNode>());
            if (seasonelementlist.All(x => x.Attributes["Watched"].Value == "true")) seriesElement.SetAttribute("Watched", "true");
            else seriesElement.SetAttribute("Watched", "false");
            //XdataProvider.Refresh();
            XdataProvider.Document.Save("UserData.xml");
        }

        private void OmdbLookup2View(OmdbRequestBuilder builder)
        {
            var con = new OmdbConnection();
            try
            {
                var root = con.GetResult(builder);
                var fullFilePath = root.Poster;
                var bitmap = new BitmapImage();
                try
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                    bitmap.EndInit();
                }
                catch (Exception)
                {
                    //Kein Bild gefunden
                }
                seriesControl.OmdbResultObj = root;
                seriesControl.imgPoster.Source = bitmap;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void OnSeriesAdd(object sender, RoutedEventArgs e)
        {
            AddItemWindow aiw = new AddItemWindow("Add Series");
            aiw.ShowDialog();
            _watchHandler.AddWatch(aiw.InputValue);
            _watchHandler.Save();
            XdataProvider.Refresh();
        }

        private void OnSeriesEdit(object sender, RoutedEventArgs e)
        {
            var seriesElement = (XmlElement) ((MenuItem) e.Source).DataContext;
            var Id = int.Parse(seriesElement.Attributes["Id"].Value);
            EditSeries es = new EditSeries(_watchHandler.WatchXml.Series.First(x => x.Id == Id));
            es.ShowDialog();
            _watchHandler.Save();
            XdataProvider.Refresh();
        }

        public void UpdateDataSet()
        {
            _watchHandler.RefreshWatch();
            _watchHandler.Save();
            XdataProvider.Refresh();
        }
    }
}

