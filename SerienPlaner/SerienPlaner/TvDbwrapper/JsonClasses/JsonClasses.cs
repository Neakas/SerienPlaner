using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Watchlist.TvDbwrapper.JsonClasses
{

    public class Authenticate
    {
        public string Apikey { get; set; } = "517C3F261F6A0C96";

        public Authenticate(string apikey)
        {
            Apikey = apikey;
        }
    }

    public class ApiTokenResponse
    {
        public string Token { get; set; }
    }

    public class SeriesSearchRequest
    {
        public SeriesSearchRequest(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; set; }
        public string imdbId { get; set; }
        public string zap2itId { get; set; }
        [JsonProperty(PropertyName = "Accept-Language")]
        public string AcceptLanguage { get; set; }
}

    public class SeriesSearchData
    {
        
        public List<string> Aliases { get; set; }
        public string Banner { get; set; }
        public string FirstAired { get; set; }
        public int Id { get; set; }
        public string Network { get; set; }
        public string Overview { get; set; }
        public string SeriesName { get; set; }
        public string Status { get; set; }
    }

    public class SeriesSearchDataArray
    {
        public List<SeriesSearchData> Data { get; set; }
    }


}
