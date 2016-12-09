using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerienPlaner.XML
{
    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "Released")]
        public string Released { get; set; }
        [XmlAttribute(AttributeName = "Episode")]
        public string Episode { get; set; }
        [XmlAttribute(AttributeName = "imdbRating")]
        public string ImdbRating { get; set; }
        [XmlAttribute(AttributeName = "imdbID")]
        public string ImdbID { get; set; }
    }

    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [XmlElement(ElementName = "result")]
        public List<Result> Result { get; set; }
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "Season")]
        public string Season { get; set; }
        [XmlAttribute(AttributeName = "totalSeasons")]
        public string TotalSeasons { get; set; }
        [XmlAttribute(AttributeName = "Response")]
        public string Response { get; set; }
        [XmlElement(ElementName = "movie")]
        public Movie Movie { get; set; }
    }

    [XmlRoot(ElementName = "movie")]
    public class Movie
    {
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "year")]
        public string Year { get; set; }
        [XmlAttribute(AttributeName = "rated")]
        public string Rated { get; set; }
        [XmlAttribute(AttributeName = "released")]
        public string Released { get; set; }
        [XmlAttribute(AttributeName = "season")]
        public string Season { get; set; }
        [XmlAttribute(AttributeName = "episode")]
        public string Episode { get; set; }
        [XmlAttribute(AttributeName = "runtime")]
        public string Runtime { get; set; }
        [XmlAttribute(AttributeName = "genre")]
        public string Genre { get; set; }
        [XmlAttribute(AttributeName = "director")]
        public string Director { get; set; }
        [XmlAttribute(AttributeName = "writer")]
        public string Writer { get; set; }
        [XmlAttribute(AttributeName = "actors")]
        public string Actors { get; set; }
        [XmlAttribute(AttributeName = "plot")]
        public string Plot { get; set; }
        [XmlAttribute(AttributeName = "language")]
        public string Language { get; set; }
        [XmlAttribute(AttributeName = "country")]
        public string Country { get; set; }
        [XmlAttribute(AttributeName = "awards")]
        public string Awards { get; set; }
        [XmlAttribute(AttributeName = "poster")]
        public string Poster { get; set; }
        [XmlAttribute(AttributeName = "metascore")]
        public string Metascore { get; set; }
        [XmlAttribute(AttributeName = "imdbRating")]
        public string ImdbRating { get; set; }
        [XmlAttribute(AttributeName = "imdbVotes")]
        public string ImdbVotes { get; set; }
        [XmlAttribute(AttributeName = "imdbID")]
        public string ImdbID { get; set; }
        [XmlAttribute(AttributeName = "seriesID")]
        public string SeriesID { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

  


}
