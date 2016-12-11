using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    public class WatchSeason
    {
        [XmlAttribute(AttributeName = "SeasonId")]
        public int SeasonId { get; set; }
        public List<WatchEpisode> Episodes { get; set; }
    }
}
