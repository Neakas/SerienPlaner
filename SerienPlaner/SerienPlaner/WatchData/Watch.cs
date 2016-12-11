using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    [XmlRoot(ElementName = "Watch")]
    public class Watch
    {
        [XmlAttribute(AttributeName = "IMDBID")]
        public string IMDBID { get; set; }
        public List<WatchSeries> Series { get; set; }
    }
}
