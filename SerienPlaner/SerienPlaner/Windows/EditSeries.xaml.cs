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
                btAutoFill.IsEnabled = lbSeasons.SelectedItem != null;
            };
            lbEpisodes.SelectionChanged += delegate
            {
                btRemoveEpisodes.IsEnabled = lbEpisodes.SelectedItem != null && ((WatchEpisode)lbEpisodes.SelectedItem)?.Imdbid == null;
                btUp.IsEnabled = lbEpisodes.SelectedItem != null && ((WatchEpisode)lbEpisodes.SelectedItem).Imdbid == null;
                btDown.IsEnabled = lbEpisodes.SelectedItem != null && ((WatchEpisode)lbEpisodes.SelectedItem).Imdbid == null;
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
            //AddEpisode()
            int id = ((WatchSeason)lbSeasons.SelectedItem).Episodes.LastOrDefault()?.EpisodeId + 1 ?? 1;
            Series.Seasons[Series.Seasons.IndexOf((WatchSeason)lbSeasons.SelectedItem)].Episodes.Add(new WatchEpisode() { EpisodeId = id, EpisodeName = "Episode " + id });
            lbEpisodes.Items.Refresh();
        }
        private void btRemoveEpisodes_Click(object sender, RoutedEventArgs e)
        {
            Series.Seasons[Series.Seasons.IndexOf((WatchSeason) lbSeasons.SelectedItem)].Episodes.Remove(
                (WatchEpisode) lbEpisodes.SelectedItem);
        }

        private void BtUp_OnClick(object sender, RoutedEventArgs e)
        {
            WatchEpisode selectedEpisode = ((WatchEpisode)lbEpisodes.SelectedItem);
            WatchSeason selectedSeason = ((WatchSeason) lbSeasons.SelectedItem);
            Move(selectedEpisode, selectedSeason,true);
            lbEpisodes.Items.Refresh();
            lbEpisodes.SelectedItem = selectedEpisode;
        }

        private void Move(WatchEpisode selectedEpisode,WatchSeason selectedSeason, bool MoveUp)
        {
            int StartIndex = selectedSeason.Episodes.IndexOf(selectedEpisode);
            int NewIndex = StartIndex;
            if(MoveUp) if (StartIndex == 0) return;
            if(!MoveUp) if (StartIndex == selectedSeason.Episodes.Count - 1) return;
            int Id = selectedEpisode.EpisodeId;
            bool validId = false;

            while (!validId)
            {
                if (MoveUp)
                {
                    Id--;
                    if (!selectedSeason.Episodes.Any(x => x.EpisodeId == Id))
                    {
                        validId = true;
                        continue;
                    }
                    NewIndex--;
                }
                else
                {
                    Id++;
                    //selectedSeason.Episodes.Move(Index, Index + 1);
                    if (!selectedSeason.Episodes.Any(x => x.EpisodeId == Id))
                    {
                        validId = true;
                        continue;
                    }
                    NewIndex++;
                }
            }
            selectedSeason.Episodes.Move(StartIndex, NewIndex);
            selectedEpisode.EpisodeId = Id;
        }

        private void btDown_Click(object sender, RoutedEventArgs e)
        {
            WatchEpisode selectedEpisode = ((WatchEpisode)lbEpisodes.SelectedItem);
            WatchSeason selectedSeason = ((WatchSeason)lbSeasons.SelectedItem);
            Move(selectedEpisode,selectedSeason,false);
            lbEpisodes.Items.Refresh();
            lbEpisodes.SelectedItem = selectedEpisode;
        }

        private void AddEpisode(int id)
        {
            Series.Seasons[Series.Seasons.IndexOf((WatchSeason)lbSeasons.SelectedItem)].Episodes.Add(new WatchEpisode() { EpisodeId = id, EpisodeName = "Episode " + id });
        }


        private void BtAutoFill_OnClick(object sender, RoutedEventArgs e)
        {
            WatchSeason selectedSeason = ((WatchSeason) lbSeasons.SelectedItem);
            int startid = 1;
            int Count = 0;
            int maxid = ((WatchSeason) lbSeasons.SelectedItem).Episodes.Last().EpisodeId;
            for (int i = startid; i < maxid; i++)
            {
                if (!selectedSeason.Episodes.Any(x => x.EpisodeId == i))
                {
                    AddEpisode(i);
                    Count++;
                }
            }
            MessageBox.Show(Count + " Episodes have been Auto-Created.");
            MainWindow.CurrentInstance.UpdateDataSet();
            Series.Refresh();
            lbSeasons.Items.Refresh();
            lbEpisodes.Items.Refresh();
            var season = (WatchSeason) lbSeasons.SelectedItem;
            lbSeasons.SelectedItem = null;
            lbSeasons.SelectedItem = season;
        }
    }
}
