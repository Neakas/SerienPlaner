using System.Linq;
using System.Windows;
using System.Windows.Input;
using Watchlist.WatchData;

namespace Watchlist.Windows
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
            LbSeasons.SelectionChanged += delegate
            {
                BtRemoveSeason.IsEnabled = btAddEpisodes.IsEnabled = LbSeasons.SelectedItem != null;
                btAutoFill.IsEnabled = LbSeasons.SelectedItem != null;
            };
            LbEpisodes.SelectionChanged += delegate
            {
                btRemoveEpisodes.IsEnabled = (LbEpisodes.SelectedItem != null) && (((WatchEpisode)LbEpisodes.SelectedItem)?.Imdbid == null);
                btUp.IsEnabled = (LbEpisodes.SelectedItem != null) && (((WatchEpisode)LbEpisodes.SelectedItem).Imdbid == null);
                btDown.IsEnabled = (LbEpisodes.SelectedItem != null) && (((WatchEpisode)LbEpisodes.SelectedItem).Imdbid == null);
            };
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) => ((UIElement) sender).IsManipulationEnabled = true;

        private void BtAddSeason_OnClick(object sender, RoutedEventArgs e)
        {
            //int id = Series.Seasons.LastOrDefault()?.SeasonId + 1 ?? 1;
            //Series.Seasons.Add(new WatchSeason
            //{SeasonId = id});
        }

        private void BtAddEpisodes_OnClick(object sender, RoutedEventArgs e)
        {
            //int id = ((WatchSeason)LbSeasons.SelectedItem).Episodes.LastOrDefault()?.EpisodeId + 1 ?? 1;
            //Series.Seasons[Series.Seasons.IndexOf((WatchSeason)LbSeasons.SelectedItem)].Episodes.Add(new WatchEpisode
            //{ EpisodeId = id, EpisodeName = "Episode " + id });
            //LbEpisodes.Items.Refresh();
        }
        //private void btRemoveEpisodes_Click(object sender, RoutedEventArgs e) => Series.Seasons[Series.Seasons.IndexOf((WatchSeason) LbSeasons.SelectedItem)].Episodes.Remove(
        //    (WatchEpisode) LbEpisodes.SelectedItem);

        private void BtUp_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedEpisode = (WatchEpisode)LbEpisodes.SelectedItem;
            var selectedSeason = (WatchSeason) LbSeasons.SelectedItem;
            Move(selectedEpisode, selectedSeason,true);
            LbEpisodes.Items.Refresh();
            LbEpisodes.SelectedItem = selectedEpisode;
        }

        private static void Move(WatchEpisode selectedEpisode,WatchSeason selectedSeason, bool moveUp)
        {
            int startIndex = selectedSeason.Episodes.IndexOf(selectedEpisode);
            int newIndex = startIndex;
            if(moveUp) if (startIndex == 0) return;
            if(!moveUp) if (startIndex == selectedSeason.Episodes.Count - 1) return;
            int id = selectedEpisode.EpisodeId;
            var validId = false;

            while (!validId)
            {
                if (moveUp)
                {
                    id--;
                    if (selectedSeason.Episodes.All(x => x.EpisodeId != id))
                    {
                        validId = true;
                        continue;
                    }
                    newIndex--;
                }
                else
                {
                    id++;
                    if (selectedSeason.Episodes.All(x => x.EpisodeId != id))
                    {
                        validId = true;
                        continue;
                    }
                    newIndex++;
                }
            }
            selectedSeason.Episodes.Move(startIndex, newIndex);
            selectedEpisode.EpisodeId = id;
        }

        private void btDown_Click(object sender, RoutedEventArgs e)
        {
            var selectedEpisode = (WatchEpisode)LbEpisodes.SelectedItem;
            var selectedSeason = (WatchSeason)LbSeasons.SelectedItem;
            Move(selectedEpisode,selectedSeason,false);
            LbEpisodes.Items.Refresh();
            LbEpisodes.SelectedItem = selectedEpisode;
        }

        //private void AddEpisode(int id) => Series.Seasons[Series.Seasons.IndexOf((WatchSeason)LbSeasons.SelectedItem)].Episodes.Add(new WatchEpisode
        //{ EpisodeId = id, EpisodeName = "Episode " + id });


        private void BtAutoFill_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedSeason = (WatchSeason) LbSeasons.SelectedItem;
            const int startid = 1;
            var count = 0;
            int maxid = ((WatchSeason) LbSeasons.SelectedItem).Episodes.Last().EpisodeId;
            for (int i = startid; i < maxid; i++)
            {
                if (selectedSeason.Episodes.Any(x => x.EpisodeId == i)) continue;
                //AddEpisode(i);
                count++;
            }
            MessageBox.Show(count + " Episodes have been Auto-Created.");
            MainWindow.CurrentInstance.UpdateDataSet();
            Series.Refresh();
            LbSeasons.Items.Refresh();
            LbEpisodes.Items.Refresh();
            var season = (WatchSeason) LbSeasons.SelectedItem;
            LbSeasons.SelectedItem = null;
            LbSeasons.SelectedItem = season;
        }

        private void btRemoveEpisodes_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
