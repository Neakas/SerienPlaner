using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using NeaUtils.Extensions.XmlExtensions;
using SerienPlaner.Json;

namespace SerienPlaner.WatchData
{
    public class WatchHandler
    {
        public FileInfo XmlFile;

        public Watch WatchXml { get; set; }

        public WatchHandler()
        {
            if (WatchXml != null) return;
            var appdir = NeaUtils.Application.ApplicationHelper.GetApplicationExecutionDirectory();
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
            WatchXml.Series.ForEach(x=> x.Update());
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void AddWatch( OmdbResult sender )
        {
            int totalSeasons;
            int newId = WatchXml.Series.OrderByDescending(x => x.Id).Select(x=> x.Id).FirstOrDefault();
            newId++;
            WatchXml.Series.Add(new WatchSeries(sender, !int.TryParse(sender.totalSeasons, out totalSeasons)) {Id = newId});
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void AddWatch(string Value)
        {
            WatchXml.Series.Add(new WatchSeries(Value));
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void AddSeasons(string Series,int Amount)
        {
            for (int i = 0; i < Amount; i++)
            {
                WatchXml.Series.First(x => x.Title == Series).Seasons.Add(new WatchSeason() { SeasonId = i});
            }
        }

        public void AddEpisodes(string Series, int SeasonId,int Amount)
        {
            for (int i = 0; i < Amount; i++)
            {
                WatchXml.Series.First(x => x.Title == Series).Seasons.First(x=> x.SeasonId == SeasonId).Episodes.Add(new WatchEpisode() {EpisodeId = i,EpisodeName = "Episode " + i,Imdbid = "Manuell"});
            }
        }
        public void Save()
        {
            WatchXml.SerializeToFile(XmlFile.FullName);
        }

        public void RemoveWatch(string imdbid)
        {
            var delseries = WatchXml.Series.First(x => x.Imdbid == imdbid);
            WatchXml.Series.Remove(delseries);
            WatchXml.SerializeToFile(XmlFile.FullName);
        }
    }
}
