using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SerienPlaner.OMDBwrapper
{
    public enum ReturnType
    {
        Json,
        Xml
    }

    public enum PlotType
    {
        Short,
        Full
    }

    public enum TypeOfSearch
    {
        Movie,
        Series,
        Episode
    }

    public class OMDBConnection
    {
        public async Task<string> GetResult(OMDBSearchBuilder builder)
        {
            string Url = "http://www.omdbapi.com/?";
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString(Url + builder.RequestString);
            }
        }
    }
}
