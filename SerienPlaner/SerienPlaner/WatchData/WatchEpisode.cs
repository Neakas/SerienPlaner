using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    public class WatchEpisode : INotifyPropertyChanged
    {
        [XmlAttribute(AttributeName = "Imdbid")]
        public string Imdbid { get; set; }
        [XmlAttribute(AttributeName = "Watched")]
        public bool Watched { get; set; }
        [XmlAttribute(AttributeName = "EpisodeName")]
        public string EpisodeName { get; set; }
        [XmlAttribute(AttributeName = "EpisodeId")]
        public int EpisodeId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
