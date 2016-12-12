using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dragonz.actb.provider
{
    public class DataProviderContains : IAutoCompleteDataProvider
    {
        private IEnumerable<string> source;

        public DataProviderContains(IEnumerable<string> _source)
        {
            source = _source;
        }

        public object ResultObject { get; set; }

        public IEnumerable<string> GetItems(string textPattern)
        {
            foreach (string current in source)
            {
                string currentLow = current.ToLower();
                if (currentLow.Contains(textPattern.ToLower()))
                {
                    yield return current;
                }
            }
            yield break;
        }
    }
}
