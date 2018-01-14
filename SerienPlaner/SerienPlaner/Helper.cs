using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TVDBSharp.Models;

namespace Watchlist
{
    public static class Helper
    {
        public static T ToType<T>(this XmlNode thisnode)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(thisnode.OuterXml));   
        }
    }
}
