using System.Windows;

namespace Watchlist.Controls
{
    /// <summary>
    /// Interaction logic for DoubleLabel.xaml
    /// </summary>
    public partial class DoubleLabel
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
