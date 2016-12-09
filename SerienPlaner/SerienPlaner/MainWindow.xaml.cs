using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml.Serialization;
using SerienPlaner.OMDBwrapper;
using SerienPlaner.XML;
using UIControls;

namespace SerienPlaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> sections = new List<string> {"Author",
                               "Title", "Comment"};
            tbSearch.SectionsList = sections;
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var con = new OMDBConnection();
                OMDBRequestBuilder request = new OMDBRequestBuilder(tbSearch.Text,PlotType.Full);
                OMDBSearchBuilder searchrequest = new OMDBSearchBuilder(tbSearch.Text);
                request.Season = 1;
                //request.Episode = 1;

                var result = con.GetResult(searchrequest).Result.Replace("\\","");

                Root root = ReadXml(result);
                //tbResult.Text = "Title: " + root.Title + " Season: " + root.Season + Environment.NewLine;
                //root.Result.ForEach(x=> tbResult.Text += "Episode: " + x.Episode + " Title: " + x.Title + Environment.NewLine);
            }
        }

        private Root ReadXml(string xmlstring)
        {
            var xmlSerializer = new XmlSerializer(typeof(Root));
            Root result;

            using (TextReader reader = new StringReader(xmlstring))
            {
                result = (Root)xmlSerializer.Deserialize(reader);
            }
            return result;
        }

        private void TbSearch_OnOnSearch(object sender, RoutedEventArgs e)
        {
            SearchEventArgs searchArgs = e as SearchEventArgs;
        }

    }
}
