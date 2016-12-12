using System.Collections.Generic;

namespace dragonz.actb.provider
{
    public interface IAutoCompleteDataProvider
    {
        object ResultObject { get; set; }
        IEnumerable<string> GetItems(string textPattern);
    }
}
