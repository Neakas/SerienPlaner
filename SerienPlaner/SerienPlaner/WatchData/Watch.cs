using System.Collections.Generic;
using System.Xml.Serialization;
using TVDBSharp.Models;

namespace Watchlist.WatchData
{
    [XmlRoot(ElementName = "Watch")]
    public class Watch
    {
        [XmlAttribute(AttributeName = "Imdbid")]
        public string Imdbid { get; set; }
        public List<WatchSeries> Series { get; set; }
    }
}
