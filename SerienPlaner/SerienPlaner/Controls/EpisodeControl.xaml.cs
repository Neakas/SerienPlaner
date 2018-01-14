using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TVDBSharp.Models;
using Watchlist.Json;

namespace Watchlist.Controls
{
    /// <summary>
    /// Interaction logic for Series.xaml
    /// </summary>
    public partial class EpisodeControl : INotifyPropertyChanged
    {
        private Episode _episode;

        public Episode Episode
        {
            get => _episode;
            set
            {
                _episode = value;
                OnPropertyChanged();
            }
        }

        public EpisodeControl(Episode episode)
        {
            InitializeComponent();
            Episode = episode;
            imgPoster.LoadImage(Episode.EpisodeImage);
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
