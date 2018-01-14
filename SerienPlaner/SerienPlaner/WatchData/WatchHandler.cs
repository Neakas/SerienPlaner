using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeaUtils.Extensions.XmlExtensions;
using TVDBSharp.Models;
using Watchlist.Json;

namespace Watchlist.WatchData
{
    public class WatchHandler
    {
        public FileInfo XmlFile;

        public Watch WatchXml { get; set; }

        public WatchHandler()
        {
            if (WatchXml != null) return;
            DirectoryInfo appdir = NeaUtils.Application.ApplicationHelper.GetApplicationExecutionDirectory();
            XmlFile = appdir.EnumerateFiles().FirstOrDefault(x => x.Name == "UserData.xml");

            if (XmlFile != null) //Load the Xml Data
            {
                WatchXml = new Watch().DeserializeFromFile(XmlFile.FullName);
            }
            else
            {
                WatchXml = new Watch
                {
                    Series = new List<WatchSeries>()
                };
                XmlFile = new FileInfo(Path.Combine(appdir.FullName, "UserData.xml"));
            }


            UpdateWatch();
        }

        private void UpdateWatch()
        {
            //WatchXml.Series.ForEach(x=> x.Update());
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void RefreshWatch() => WatchXml.Series.ForEach(x=> x.Refresh());

        public void AddWatch( Show show )
        {
            int TotalSeasons = show.Seasons.Max(x => x.SeasonNumber);
            int newId = WatchXml.Series.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
            newId++;
            WatchSeries showS = new WatchSeries(show);
            WatchXml.Series.Add(showS);
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void AddWatch(string value)
        {
            //WatchXml.Series.Add(new WatchSeries(value));
            //WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void AddSeasons(string series,int amount)
        {
            //for (var i = 0; i < amount; i++)
            //{
            //    WatchXml.Series.First(x => x.Name == series).Seasons.Add(new WatchSeason
            //    { SeasonId = i});
            //}
        }

        public void AddEpisodes(string series, int seasonId,int amount)
        {
            //for (var i = 0; i < amount; i++)
            //{
            //    WatchXml.Series.First(x => x.Title == series).Seasons.First(x=> x.SeasonId == seasonId).Episodes.Add(new WatchEpisode
            //    {EpisodeId = i,EpisodeName = "Episode " + i,Imdbid = "Manuell"});
            //}
        }
        public void Save() => WatchXml.SerializeToFile(XmlFile.FullName);

        public void RemoveWatch(string imdbid)
        {
            WatchSeries delseries = WatchXml.Series.First(x => x.Imdbid == imdbid);
            WatchXml.Series.Remove(delseries);
            WatchXml.SerializeToFile(XmlFile.FullName);
        }
    }
}
