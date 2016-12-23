using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    [XmlRoot(ElementName = "Watch")]
    public class Watch
    {
        [XmlAttribute(AttributeName = "Imdbid")]
        public string Imdbid { get; set; }
        public List<WatchSeries> Series { get; set; }
    }
}
