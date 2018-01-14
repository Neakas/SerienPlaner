using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Watchlist.Json;
using Watchlist.TvDbwrapper;
using Watchlist.TvDbwrapper.JsonClasses;

namespace Watchlist.OMDBwrapper
{

    public class TvDbConnection
    {
        private string _apiKey = "517C3F261F6A0C96";
        public string _baseUrl = "https://api.thetvdb.com/";
        private string _apitoken;
        public static TvDbConnection CurrentInstance;

        public string ApiToken
        {
            get { return _apitoken; }
            set
            {
                _apitoken = value;
                MainWindow.CurrentInstance.SetConnectionState(true);
            }
        }

        public TvDbConnection()
        {
            CurrentInstance = this;
        }

        private async Task<T> Post<T>(RequestType requestType, string jsonstring)
        {
            string url = TvDbHelper.GetPostUrl(_baseUrl, requestType);
            HttpClient client = new HttpClient { BaseAddress = new Uri(url) };
            if (requestType != RequestType.Login)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_apitoken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent content = new StringContent(jsonstring);
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            var result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<Stream> Get<T>(RequestType requestType, string query)
        {
            string url = TvDbHelper.GetGetUrl(_baseUrl, requestType, query);
            HttpClient client = new HttpClient { BaseAddress = new Uri(url) };
            if (requestType != RequestType.Login)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apitoken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = Task.Run(() => client.GetAsync(url)).Result;
            return await response.Content.ReadAsStreamAsync();

        }

        private async Task<ApiTokenResponse> RequestApiToken()
        {
            string jsonstring = JsonConvert.SerializeObject(new Authenticate(_apiKey));
            return await Post<ApiTokenResponse>(RequestType.Login, jsonstring);
        }

       

        public async Task Connect()
        {
            ApiTokenResponse tokenresponse =await RequestApiToken();
            ApiToken = tokenresponse.Token;
        }

        public List<SeriesSearchData> SearchSeries(SeriesSearchRequest searchquery)
        {
            string query = searchquery.SeriesName;
            var result = Get<SeriesSearchData>(RequestType.Search, query).Result;
            using (var stream = new StreamReader(result))
            {
                string jsonresponse = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<SeriesSearchDataArray>(jsonresponse).Data;
            }
        }
    }



}
