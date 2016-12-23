using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using SerienPlaner.Json;
using SerienPlaner.OMDBwrapper;

namespace SerienPlaner.WatchData
{
    public class WatchSeries : INotifyPropertyChanged
    {
        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; set; }
        [XmlAttribute(AttributeName = "Imdbid")]
        public string Imdbid { get; set; }
        public ObservableCollection<WatchSeason> Seasons { get; set; }
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "Watched")]
        public bool Watched { get; set; }

        [XmlAttribute(AttributeName = "IsManual")]
        public bool IsManual { get; set; }



        public WatchSeries(string Value)
        {
            Title = Value;
            Imdbid = "Manual";
            IsManual = true;
        }

        public WatchSeries()
        {
        }

        public WatchSeries(OmdbResult omdbResult, bool IsManual)
        {
            Imdbid = omdbResult.imdbID;
            Title = omdbResult.Title;
            if (!IsManual) Seasons = FindSeasons(omdbResult.imdbID, int.Parse(omdbResult.totalSeasons));
            else MessageBox.Show("The Series doesnt have any Seasons listed on OMDB. Can not Watch!");
        }

        private static ObservableCollection<WatchEpisode> FindEpisodes(string imdbId, int season,ObservableCollection<WatchEpisode> episodeList)
        {
            var con = new OmdbConnection();
            var episoderesult = con.GetResult(new OmdbRequestBuilder(imdbId, season, PlotType.Full));
            if (episodeList == null)
            {
                episodeList = new ObservableCollection<WatchEpisode>();
            }
            episoderesult.Episodes.ForEach(x =>
            {
                if (episodeList.Any(y => y.Imdbid == x.imdbID))
                    return;
                episodeList.Add(new WatchEpisode
                {
                    Imdbid = x.imdbID,
                    EpisodeName = x.Title,
                    EpisodeId = int.Parse(x.Episode)
                });
            });
            return new ObservableCollection<WatchEpisode>(episodeList.OrderBy(x => x.EpisodeId).ToList());
        }

        private ObservableCollection<WatchSeason> FindSeasons(string imdbId, int seasons)
        {
            var seasonlist = new ObservableCollection<WatchSeason>();
            if (Seasons != null)
            {
                seasonlist = Seasons;
            }

            for (var i = 0; i < seasons; i++)
            {
                if (seasonlist.Any(x => x.SeasonId == i + 1))
                {
                    var foundseason = seasonlist.First(x => x.SeasonId == i + 1);
                    foundseason.Episodes = FindEpisodes(imdbId, i + 1, foundseason.Episodes);
                }
                else
                {
                    seasonlist.Add(new WatchSeason
                    {
                        SeasonId = i + 1,
                        Episodes = FindEpisodes(imdbId, i + 1,null)
                    });
                }
            }
            return new ObservableCollection<WatchSeason>(seasonlist.OrderBy(x => x.SeasonId).ToList());
        }

        public void Update()
        {
            if (IsManual) return;
            var con = new OmdbConnection();
            var omdbResult = con.GetResult(new OmdbRequestBuilder(Title, RequestBy.Title, PlotType.Full));
            if(omdbResult != null)  Seasons = FindSeasons(omdbResult.imdbID, int.Parse(omdbResult.totalSeasons));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
