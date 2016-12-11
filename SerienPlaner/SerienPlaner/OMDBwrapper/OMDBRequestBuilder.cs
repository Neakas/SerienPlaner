using System;

namespace SerienPlaner.OMDBwrapper
{
    public class OmdbRequestBuilder
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

        private string Searchstring { get; set; }

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

        private PlotType _plotType;

        public PlotType PlotType
        {
            get { return _plotType; }
            set
            {
                _plotType = value;
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
        public OmdbRequestBuilder(string search,RequestBy requestBy,PlotType plotType = PlotType.Short, TypeOfSearch typeOfSearch = TypeOfSearch.Series,ReturnType returnformat = ReturnType.Json)
        {
            switch (requestBy)
            {
                case RequestBy.ImdbId:
                    RequestString = "i=" + search;
                    break;
                case RequestBy.Title:
                    RequestString = "t=" + search;
                    break;
                case RequestBy.Search:
                    RequestString = "s=" + search;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestBy), requestBy, null);
            }
            
            PlotType = plotType;
            TypeOfSearch = typeOfSearch;
            ReturnFormat = returnformat;
        }

        public OmdbRequestBuilder(string search, int season, PlotType plotType = PlotType.Short, TypeOfSearch typeOfSearch = TypeOfSearch.Series, ReturnType returnformat = ReturnType.Json)
        {
            RequestString = "i=" + search;
            Season = season;
            PlotType = plotType;
            TypeOfSearch = typeOfSearch;
            ReturnFormat = returnformat;
        }

        public OmdbRequestBuilder(string search, int season,int episode, PlotType plotType = PlotType.Short, TypeOfSearch typeOfSearch = TypeOfSearch.Series, ReturnType returnformat = ReturnType.Json)
        {
            RequestString = "i=" + search;
            Season = season;
            Episode = episode;
            PlotType = plotType;
            TypeOfSearch = typeOfSearch;
            ReturnFormat = returnformat;
        }
    }
}
