using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeaUtils.Extensions.XmlExtensions;
using SerienPlaner.Json;

namespace SerienPlaner.WatchData
{
    public class WatchHandler
    {
        private Watch _watchxml;
        private FileInfo xmlFile;

        public Watch WatchXml
        {
            get
            {
                return _watchxml;
            }
            set
            {
                _watchxml = value;
            }
        }

        public WatchHandler()
        {
            if (WatchXml != null) return;
            
            var appdir = NeaUtils.Application.ApplicationHelper.GetApplicationExecutionDirectory();
            xmlFile = appdir.EnumerateFiles().FirstOrDefault(x => x.Name == "UserData.xml");

            if (xmlFile != null) //Load the Xml Data
            {
                WatchXml = new Watch().DeserializeFromFile(xmlFile.FullName);
            }
            else
            {
                WatchXml = new Watch
                {
                    Series = new List<WatchSeries>()
                };
                xmlFile = new FileInfo(Path.Combine(appdir.FullName, "UserData.xml"));
            }
            UpdateWatch();
        }

        private void UpdateWatch()
        {
            WatchXml.Series.ForEach(x=> x.Update());
            WatchXml.SerializeToFile(xmlFile.FullName);
        }

        public void AddWatch( OmdbResult sender )
        {
            WatchXml.Series.Add(new WatchSeries(sender));
            WatchXml.SerializeToFile(xmlFile.FullName);
        }
    }
}
