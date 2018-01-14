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
    public partial class ShowControl : INotifyPropertyChanged
    {
        private Show _show;

        public Show Show
        {
            get => _show;
            set
            {
                _show = value;
                OnPropertyChanged();
                OnPropertyChanged("Genres");
            }
        }

        public string Genres
        {
            get { return Show?.Genres?.Aggregate((current, next) => current + ", " + next) ?? ""; }
        }


        public ShowControl(Show show)
        {
            InitializeComponent();
            Show = show;
            imgPoster.LoadImage(Show.Poster);
            imgBanner.LoadImage(Show.Banner);
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
