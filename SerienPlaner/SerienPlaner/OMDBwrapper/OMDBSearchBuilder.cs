using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerienPlaner.OMDBwrapper
{
    public class OMDBSearchBuilder
    {
        private int _imdbId;

        public int ImdbId
        {
            get { return _imdbId; }
            set
            {
                _imdbId = value;
                RequestString += "&i=" + value;
            }
        }
        private string _searchstring;

        public string Searchstring
        {
            get { return _searchstring; }
            set { _searchstring = value; }
        }
        private TypeOfSearch _typeOfSearch;

        public TypeOfSearch TypeOfSearch
        {
            get { return _typeOfSearch; }
            set
            {
                _typeOfSearch = value;
                RequestString += "&type=" + (value == TypeOfSearch.Episode ? "episode" : value == TypeOfSearch.Movie ? "movie" : "series");
            }
        }
        private int _yearofRelease;

        public int YearofRelease
        {
            get { return _yearofRelease; }
            set
            {
                _yearofRelease = value;
                RequestString += "&y=" + value;
            }
        }

        private PlotType plotType;

        public PlotType PlotType
        {
            get { return plotType; }
            set
            {
                plotType = value;
                RequestString += "&plot=" + (value == PlotType.Full ? "full" : "short");
            }
        }

        private ReturnType _returnFormat;

        public ReturnType ReturnFormat
        {
            get { return _returnFormat; }
            set
            {
                _returnFormat = value;
                RequestString += "&r=" + (value == ReturnType.Json ? "json" : "xml");
            }
        }

        private int _episode;

        public int Episode
        {
            get { return _episode; }
            set
            {
                _episode = value;
                RequestString += "&Episode=" + value;
            }
        }

        private int _season;

        public int Season
        {
            get { return _season; }
            set
            {
                _season = value;
                RequestString += "&Season=" + value;
            }
        }

        public string RequestString { get; set; }
        public OMDBSearchBuilder(string searchstring, TypeOfSearch typeOfSearch = TypeOfSearch.Series, ReturnType returnformat = ReturnType.Xml)
        {
            RequestString = "s=" + searchstring;
            TypeOfSearch = typeOfSearch;
            ReturnFormat = returnformat;
        }
    }
}
