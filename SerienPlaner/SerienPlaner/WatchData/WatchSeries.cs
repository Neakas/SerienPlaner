using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SerienPlaner.Json;
using SerienPlaner.OMDBwrapper;

namespace SerienPlaner.WatchData
{
    public class WatchSeries
    {
        [XmlAttribute(AttributeName = "IMDBID")]
        public string IMDBID { get; set; }
        public List<WatchSeason> Seasons { get; set; }
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }

        public static WatchSeries Create(OmdbResult omdbResult)
        {
            return new WatchSeries()
            {
                IMDBID = omdbResult.imdbID,
                Title = omdbResult.Title,
                Seasons = FindSeasons(omdbResult.imdbID, Int32.Parse(omdbResult.totalSeasons))
            };
        }

        private static List<WatchEpisode> FindEpisodes(string imdbId, int season)
        {
            OmdbConnection con = new OmdbConnection();
            OmdbResult episoderesult = new OmdbResult();
            episoderesult = con.GetResult(new OmdbRequestBuilder(imdbId, season, PlotType.Full));
            List<WatchEpisode> EpisodeList = new List<WatchEpisode>();
            episoderesult.Episodes.ForEach(x => EpisodeList.Add(new WatchEpisode() { IMDBID = x.imdbID, EpisodeName = x.Title, EpisodeId = Int32.Parse(x.Episode) }));
            return EpisodeList;
        }

        private static List<WatchSeason> FindSeasons(string imdbId, int seasons)
        {
            List<WatchSeason> seasonlist = new List<WatchSeason>();
            for (int i = 0; i < seasons; i++)
            {
                seasonlist.Add(new WatchSeason()
                {
                    SeasonId = i + 1,
                    Episodes = FindEpisodes(imdbId, i + 1)
                });
            }
            return seasonlist;
        }
    }

}
