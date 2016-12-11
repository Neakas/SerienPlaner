using System;
using System.Collections.Generic;
using System.Linq;
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

        public WatchSeries()
        {
            
        }

        public WatchSeries(OmdbResult omdbResult)
        {
            IMDBID = omdbResult.imdbID;
            Title = omdbResult.Title;
            Seasons = FindSeasons(omdbResult.imdbID, Int32.Parse(omdbResult.totalSeasons));
        }

        private List<WatchEpisode> FindEpisodes(string imdbId, int season,List<WatchEpisode> EpisodeList)
        {
            OmdbConnection con = new OmdbConnection();
            OmdbResult episoderesult = new OmdbResult();
            episoderesult = con.GetResult(new OmdbRequestBuilder(imdbId, season, PlotType.Full));
            if (EpisodeList == null)
            {
                EpisodeList = new List<WatchEpisode>();
            }
            episoderesult.Episodes.ForEach(x =>
            {
                if (EpisodeList.Any(y => y.IMDBID == x.imdbID))
                    return;
                EpisodeList.Add(new WatchEpisode()
                {
                    IMDBID = x.imdbID,
                    EpisodeName = x.Title,
                    EpisodeId = Int32.Parse(x.Episode)
                });
            });
            return EpisodeList.OrderBy(x=> x.EpisodeId).ToList();
        }

        private List<WatchSeason> FindSeasons(string imdbId, int seasons)
        {
            List<WatchSeason> seasonlist = new List<WatchSeason>();
            if (Seasons != null)
            {
                seasonlist = Seasons;
            }
            
            for (int i = 0; i < seasons; i++)
            {
                if (seasonlist.Any(x => x.SeasonId == i + 1))
                {
                    WatchSeason foundseason = seasonlist.First(x => x.SeasonId == i + 1);
                    foundseason.Episodes = FindEpisodes(imdbId, i + 1, foundseason.Episodes);
                }
                else
                {
                    seasonlist.Add(new WatchSeason()
                    {
                        SeasonId = i + 1,
                        Episodes = FindEpisodes(imdbId, i + 1,null)
                    });
                }
            }
            return seasonlist.OrderBy(x=> x.SeasonId).ToList();
        }

        public void Update()
        {
            var con = new OmdbConnection();
            OmdbResult omdbResult = con.GetResult(new OmdbRequestBuilder(Title, RequestBy.Title, PlotType.Full));
            Seasons = FindSeasons(omdbResult.imdbID, Int32.Parse(omdbResult.totalSeasons));
        }
    }

}
