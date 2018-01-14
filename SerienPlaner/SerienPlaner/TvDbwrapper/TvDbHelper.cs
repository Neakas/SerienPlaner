using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchlist.TvDbwrapper
{
    public enum RequestType
    {
        Login,
        Search
    }

   
    public static class TvDbHelper
    {
        public static Dictionary<RequestType, string> UrlParameters = new Dictionary<RequestType, string>
        {
            {RequestType.Login, "login"},
            {RequestType.Search, "search/series?name=" }
        };

        public static string GetPostUrl(string BaseUrl, RequestType requestType)
        {
            if (BaseUrl.Last() != '/')
            {
                BaseUrl += '/';
            }
            return BaseUrl + UrlParameters[requestType];
        }

        public static string GetGetUrl(string BaseUrl, RequestType requestType,string Query)
        {
            string baseget = BaseUrl + UrlParameters[requestType];
            return baseget += Query;
        }
    }
}
