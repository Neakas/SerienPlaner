using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using TVDBSharp.Models;
using Watchlist.Json;
using Watchlist.OMDBwrapper;

namespace Watchlist.WatchData
{
    public class WatchSeries : INotifyPropertyChanged
    {
        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "Imdbid")]
        public string Imdbid { get; set; }
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "Watched")]
        public bool Watched { get; set; }

        [XmlAttribute(AttributeName = "isManual")]
        public bool IsManual { get; set; }


        public Show Show { get; set; }

        public WatchSeries(string value)
        {
            Title = value;
            Imdbid = "Manual";
            IsManual = true;
        }

        public WatchSeries()
        {
        }

        public WatchSeries(Show tvDbResult)
        {
            Show = tvDbResult;
            Imdbid = tvDbResult.ImdbId;
            Title = tvDbResult.Name;
            //Seasons = FindSeasons(tvDbResult,seasons);
            //else MessageBox.Show("The Series doesnt have any Seasons listed on OMDB. Can not Watch!");
        }

        private static ObservableCollection<WatchEpisode> FindEpisodes(string imdbId, int season, ObservableCollection<WatchEpisode> episodeList)
        {
            var con = new TvDbConnection();
            //TvDbResult episoderesult = con.GetResult(new TvDbRequestBuilder(imdbId, season, PlotType.Full));
            if (episodeList == null)
            {
                episodeList = new ObservableCollection<WatchEpisode>();
            }
            //episoderesult.Episodes?.ForEach(x =>
            //{
            //    if (episodeList.Any(y => y.Imdbid == x.ImdbId))
            //        return;
            //    episodeList.Add(new WatchEpisode
            //    {
            //        Imdbid = x.ImdbId,
            //        EpisodeName = x.Title,
            //        EpisodeId = int.Parse(x.Episode)
            //    });
            //});
            return new ObservableCollection<WatchEpisode>(episodeList.OrderBy(x => x.EpisodeId).ToList());
        }

        private ObservableCollection<WatchSeason> FindSeasons(Show show, int seasons)
        {
            //var seasonlist = new ObservableCollection<WatchSeason>();
            //if (Seasons != null)
            //{
            //    seasonlist = Seasons;
            //}


            //for (var i = 0; i < seasons; i++)
            //{
            //    if (seasonlist.Any(x => x.SeasonId == i + 1))
            //    {
            //        WatchSeason foundseason = seasonlist.First(x => x.SeasonId == i + 1);
            //        //foundseason.Episodes = FindEpisodes(imdbId, i + 1, foundseason.Episodes);
            //    }
            //    else
            //    {
            //        seasonlist.Add(new WatchSeason
            //        {
            //            SeasonId = i + 1,
            //            //Episodes = FindEpisodes(imdbId, i + 1, null)
            //        });

            //    }
            //}
            //return new ObservableCollection<WatchSeason>(seasonlist.OrderBy(x => x.SeasonId).ToList());
            return null;
        }

        public void Update()
        {
            //if (IsManual) return;
            //var con = new TvDbConnection();
            //TvDbResult tvDbResult = con.GetResult(new TvDbRequestBuilder(Title, RequestBy.Title, PlotType.Full));
            //if(tvDbResult != null)  Seasons = FindSeasons(tvDbResult.ImdbId, int.Parse(tvDbResult.TotalSeasons));
        }

        public void Refresh()
        {
            //foreach (WatchSeason season in Seasons)
            //{
            //    season.Episodes = new ObservableCollection<WatchEpisode>(season.Episodes.OrderBy(x => x.EpisodeId));
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
