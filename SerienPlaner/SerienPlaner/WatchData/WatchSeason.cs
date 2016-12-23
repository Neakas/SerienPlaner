using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SerienPlaner.WatchData
{
    public class WatchSeason : INotifyPropertyChanged
    {
        [XmlAttribute(AttributeName = "SeasonId")]
        public int SeasonId { get; set; }
        public ObservableCollection<WatchEpisode> Episodes { get; set; }

        [XmlAttribute(AttributeName = "Watched")]
        public bool Watched { get; set; }

        public WatchSeason()
        {
            if(Episodes == null) Episodes = new ObservableCollection<WatchEpisode>();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
