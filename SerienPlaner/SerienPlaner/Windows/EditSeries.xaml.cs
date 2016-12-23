using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using SerienPlaner.WatchData;

namespace SerienPlaner.Windows
{
    /// <summary>
    /// Interaction logic for EditSeries.xaml
    /// </summary>
    public partial class EditSeries
    {
        public WatchSeries Series { get; set; }
        public EditSeries(WatchSeries series)
        {
            Series = series;
            InitializeComponent();
            lbSeasons.SelectionChanged += delegate
            {
                btRemoveSeason.IsEnabled = btAddEpisodes.IsEnabled = lbSeasons.SelectedItem != null;
            };
            lbEpisodes.SelectionChanged += delegate
            {
                btRemoveEpisodes.IsEnabled = lbEpisodes.SelectedItem != null;
            };
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((UIElement) sender).IsManipulationEnabled = true;
        }

        private void BtAddSeason_OnClick(object sender, RoutedEventArgs e)
        {
            int id = Series.Seasons.LastOrDefault()?.SeasonId + 1 ?? 1;
            Series.Seasons.Add(new WatchSeason() {SeasonId = id});
        }

        private void BtAddEpisodes_OnClick(object sender, RoutedEventArgs e)
        {
            int id = ((WatchSeason)lbSeasons.SelectedItem).Episodes.LastOrDefault()?.EpisodeId + 1 ?? 1;
            Series.Seasons[Series.Seasons.IndexOf((WatchSeason)lbSeasons.SelectedItem)].Episodes.Add(new WatchEpisode() { EpisodeId = id, EpisodeName = "Episode " + id });
            lbEpisodes.Items.Refresh();
        }
    }
}
