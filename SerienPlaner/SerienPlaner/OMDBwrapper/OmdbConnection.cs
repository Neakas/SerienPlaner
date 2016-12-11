using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using SerienPlaner.Json;

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

    public enum RequestBy
    {
        ImdbId,
        Title,
        Search
    }

    public class OmdbConnection
    {
        public OmdbResult GetResult(OmdbRequestBuilder builder)
        {
            const string url = "http://www.omdbapi.com/?";
            var uri = new Uri(url + builder.RequestString);

            WebRequest request = null;
            WebResponse response = null;
            try
            {
                request = WebRequest.Create(uri);
                try
                {
                    response = request.GetResponse();
                }
                catch
                {
                    return null;
                }
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    var json = reader.ReadToEnd();
                    json = json.Replace(@"\", "");
                    return jsonSerializer.Deserialize<Json.OmdbResult>(json);
                }
            }
            finally
            {
                response?.Close();
                request?.Abort();
            }
        }
    }
}
