using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SerienPlaner.Windows
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow
    {

        public string InputValue { get; set; }

        public AddItemWindow(string Header)
        {
            InitializeComponent();
            DataContext = Header;
        }

        private void btAddSeries_Click(object sender, RoutedEventArgs e)
        {
            InputValue = tbInput.Text;
            this.Close();
        }
    }
}
