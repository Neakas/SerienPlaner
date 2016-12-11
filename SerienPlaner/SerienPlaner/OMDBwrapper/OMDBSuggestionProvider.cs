using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using dragonz.actb.provider;

namespace SerienPlaner.OMDBwrapper
{
    internal class OmdbSuggestionProvider : IAutoCompleteDataProvider
    {
        public object ResultObject { get; set; }

        public IEnumerable<string> GetItems( string textPattern )
        {
            if (textPattern.Length < 2)
            {
                return null;
            }
            var con = new OmdbConnection();
            var jsonResult = con.GetResult(new OmdbRequestBuilder(textPattern, RequestBy.Search));
            if (jsonResult == null || jsonResult.Response == "False")
                return null;

            var result = jsonResult.Search.Select(x => x.Title);
            ResultObject = jsonResult;
            return result;
        }
    }
}
