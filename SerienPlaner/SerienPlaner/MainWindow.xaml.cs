using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using dragonz.actb.core;
using Watchlist.Json;
using Watchlist.OMDBwrapper;
using Watchlist.WatchData;
using Watchlist.Windows;
using Newtonsoft.Json;
using TVDBSharp;
using TVDBSharp.Models;
using TVDBSharp.Models.Enums;
using Watchlist.Controls;
using Watchlist.TvDbwrapper;
using Watchlist.TvDbwrapper.JsonClasses;
using System.Threading.Tasks;

namespace Watchlist
{
    public partial class MainWindow
    {
        private AutoCompleteManager _acmOmdb;
        private readonly WatchHandler _watchHandler;
        public XmlDataProvider XdataProvider;
        public static MainWindow CurrentInstance;
        public TVDB tvdbhandler;
        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
            tvdbhandler = new TVDB("517C3F261F6A0C96");
            
            XdataProvider = (XmlDataProvider)FindResource("Xmldata");
            _watchHandler = new WatchHandler();

            InitControls();
        }

        private void InitControls()
        {
           
            TbSearch.KeyDown += tbSearch_KeyDown;
        }

        private void tbSearch_KeyDown( object sender, KeyEventArgs e )
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            try
            {
                var results = tvdbhandler.Search(TbSearch.Text, 10);
                SearchControl ctrl = new SearchControl(results);
                ctrl.ShowDialog();

            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public void SeriesControl_OnWatchClicked( Show show )
        {
            _watchHandler.AddWatch(show);
            _watchHandler.Save();
            XdataProvider.Refresh();
        }



        private void OnSeriesDelete(object sender, RoutedEventArgs e)
        {
            string imdbid = ((XmlElement) ((MenuItem) e.Source).DataContext).Attributes["Imdbid"].Value;
            MessageBoxResult result =
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

        private  void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            XmlElement node = ((XmlElement)e.NewValue);
            if (node == null ) return;

            SeriesControlGrid.Children.Clear();

            if (node.Name == "WatchSeries")
            {
                
                var shownode = node.SelectSingleNode("Show");
                Show selectedShow = shownode.ToType<Show>();

                ShowControl seriesctrl = new ShowControl(selectedShow);
                SeriesControlGrid.Children.Add(seriesctrl);
            }
            if (node.Name == "Episode")
            {
                Episode selectedEpisode = node.ToType<Episode>();
                EpisodeControl episodectrl = new EpisodeControl(selectedEpisode);
                SeriesControlGrid.Children.Add(episodectrl);
            }
            //seriesctrl.imgPoster.LoadImage();
            //string imdbid = ((XmlElement) e.NewValue).Attributes["Imdbid"]?.Value ?? "";
            //if (imdbid == "") return;
            //OmdbLookup2View(new TvDbRequestBuilder(imdbid, RequestBy.ImdbId, PlotType.Full));
        }

        private void EpisodeCheckChanged(object sender, RoutedEventArgs e)
        {
             //TODO: Problem ist, das die Werte direkt im XMl angepasst werden, anstatt in den an das Xml gebunden WatchEpisode XML. Dabei wird die XMl Aktualisiert, die Objekte im Watchhandler aber noch nicht.
            var episodeElement = (XmlElement) ((CheckBox) e.Source).DataContext;
            var seasonElement = (XmlElement) episodeElement?.ParentNode?.ParentNode;

            if (((CheckBox) sender).IsChecked == false)
            {
                seasonElement?.SetAttribute("Watched", "false");
            }
            else
            {
                if (seasonElement?.LastChild != null)
                {
                    var episodeelementlist = new List<XmlNode>(seasonElement.LastChild.ChildNodes.Cast<XmlNode>());
                    if (episodeelementlist.All(x => x.Attributes?["Watched"].Value == "true")) seasonElement.SetAttribute("Watched", "true");
                }
            }
            XdataProvider.Document.Save("UserData.xml");
        }

        private void SeasonCheckChanged(object sender, RoutedEventArgs e)
        {
            //TODO: Problem ist, das die Werte direkt im XMl angepasst werden, anstatt in den an das Xml gebunden WatchEpisode XML. Dabei wird die XMl Aktualisiert, die Objekte im Watchhandler aber noch nicht.
            var seasonElement = (XmlElement) ((CheckBox) e.Source).DataContext;
            if (((CheckBox) sender).IsFocused)
            {
                if (((CheckBox) sender).IsChecked == true)
                {
                    seasonElement.SetAttribute("Watched", "true");
                    foreach (XmlElement childnode in seasonElement.SelectSingleNode("Episodes").SelectNodes("Episode"))
                    {
                        childnode.SetAttribute("Watched", "true");
                    }
                }
                else
                {
                    seasonElement.SetAttribute("Watched", "false");
                    foreach (XmlElement childnode in seasonElement.SelectSingleNode("Episodes").SelectNodes("Episode"))
                    {
                        childnode.SetAttribute("Watched", "false");
                    }
                }
            }

            var seriesElement = (XmlElement) seasonElement?.ParentNode?.ParentNode;
            if (seriesElement?.FirstChild != null)
            {
                var seasonelementlist = new List<XmlNode>(seriesElement.SelectSingleNode("Seasons").ChildNodes.Cast<XmlNode>());
                seriesElement.SetAttribute("Watched", seasonelementlist.All(x => x.Attributes?["Watched"].Value == "true")
                                                           ? "true"
                                                           : "false");
            }
            //XdataProvider.Refresh();
            XdataProvider.Document.Save("UserData.xml");
        }

        //private void OmdbLookup2View(TvDbRequestBuilder builder)
        //{
        //    var con = new TvDbConnection();
        //    try
        //    {
        //        TvDbResult root = con.GetResult(builder);
        //        string fullFilePath = root.Poster;
        //        var bitmap = new BitmapImage();
        //        try
        //        {
        //            bitmap.BeginInit();
        //            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
        //            bitmap.EndInit();
        //        }
        //        catch (Exception)
        //        {
        //            //Kein Bild gefunden
        //        }
        //        SeriesControl.TvDbResultObj = root;
        //        SeriesControl.ImgPoster.Source = bitmap;
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //}

        private void OnSeriesAdd(object sender, RoutedEventArgs e)
        {
            var aiw = new AddItemWindow("Add Series");
            aiw.ShowDialog();
            _watchHandler.AddWatch(aiw.InputValue);
            _watchHandler.Save();
            XdataProvider.Refresh();
        }

        private void OnSeriesEdit(object sender, RoutedEventArgs e)
        {
            //var seriesElement = (XmlElement) ((MenuItem) e.Source).DataContext;
            //int id = int.Parse(seriesElement.Attributes["Id"].Value);
            //var es = new EditSeries(_watchHandler.WatchXml.Series.First(x => x.Id == id));
            //es.ShowDialog();
            //_watchHandler.Save();
            //XdataProvider.Refresh();
        }

        public void UpdateDataSet()
        {
            //_watchHandler.RefreshWatch();
            _watchHandler.Save();
            XdataProvider.Refresh();
        }

        public void SetConnectionState(bool IsConnected)
        {
            if (IsConnected)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    lblConState.Foreground = System.Windows.Media.Brushes.Green;
                    lblConState.Content = "Connected";
                });
            }
        }
    }
}

