using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SerienPlaner.Json;

namespace SerienPlaner
{
    /// <summary>
    /// Interaction logic for Series.xaml
    /// </summary>
    public partial class Series : UserControl,INotifyPropertyChanged
    {
        private OmdbResult _omdbResultObj;
        public event RoutedEventHandler WatchClicked;

        public OmdbResult OmdbResultObj
        {
            get { return _omdbResultObj; }
            set
            {
                _omdbResultObj = value;
                OnPropertyChanged("OmdbResultObj");
            }
        }
        public Series()
        {
            InitializeComponent();
            this.DataContext = this;
            //WatchClicked = new RoutedEventHandler(OnWatchClicked);
            //btWatch.Click += WatchClicked;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void btWatch_Click(object sender, RoutedEventArgs e)
        {
            WatchClicked?.Invoke(OmdbResultObj, e);
        }
    }
}
