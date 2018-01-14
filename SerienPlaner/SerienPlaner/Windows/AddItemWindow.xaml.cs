using System.Windows;

namespace Watchlist.Windows
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow
    {

        public string InputValue { get; set; }

        public AddItemWindow(string header)
        {
            InitializeComponent();
            DataContext = header;
        }

        private void btAddSeries_Click(object sender, RoutedEventArgs e)
        {
            InputValue = tbInput.Text;
            Close();
        }
    }
}
