using System.Collections.Generic;
using System.Linq;
using dragonz.actb.provider;
using Watchlist.Json;
using Watchlist.TvDbwrapper.JsonClasses;

namespace Watchlist.OMDBwrapper
{
    internal class TvDbSuggestionProvider //: IAutoCompleteDataProvider
    {
        public object ResultObject { get; set; }

        //public IEnumerable<string> GetItems(string textPattern)
        //{
        //    if (textPattern.Length < 2)
        //    {
        //        return null;
        //    }
        //    if (textPattern != "House")
        //    {
        //        return null;
        //    }

        //}
    }
}
