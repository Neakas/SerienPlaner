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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerienPlaner.Controls
{
    /// <summary>
    /// Interaction logic for DoubleLabel.xaml
    /// </summary>
    public partial class DoubleLabel : UserControl
    { 

        public DoubleLabel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("LabelContent", typeof(string), typeof(DoubleLabel), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("TextContent", typeof(string), typeof(DoubleLabel), new FrameworkPropertyMetadata(string.Empty));



        public string LabelContent
        {
            get
            {
                return GetValue(LabelProperty).ToString();
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }

        public string TextContent
        {
            get
            {
                return GetValue(TextProperty).ToString();
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}
