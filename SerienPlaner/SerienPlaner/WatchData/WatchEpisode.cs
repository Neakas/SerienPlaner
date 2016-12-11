using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    public class WatchEpisode
    {
        [XmlAttribute(AttributeName = "IMDBID")]
        public string IMDBID { get; set; }
        [XmlAttribute(AttributeName = "Watched")]
        public bool Watched { get; set; }
        [XmlAttribute(AttributeName = "EpisodeName")]
        public string EpisodeName { get; set; }
        [XmlAttribute(AttributeName = "EpisodeId")]
        public int EpisodeId { get; set; }
    }
}
