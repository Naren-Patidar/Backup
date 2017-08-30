using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;

using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Xml;
using System.Web;
using Newtonsoft.Json;


namespace ServiceUtility
{
    public static class Extensions
    {
       
        public static string JsonText(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

       
        public static T TryParse<T>(this object objValue)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (!Convert.IsDBNull(objValue) && objValue != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    value = (T)converter.ConvertFrom(objValue.ToString());
                }
                catch { }
            }
            return value;
        }

        public static Exception GetCustomException(string message, Exception ex, Dictionary<string, object> data)
        {
            Exception objException = new Exception(message, ex);

            foreach (string k in data.Keys)
            {
                if (!objException.Data.Contains(k))
                {
                    objException.Data.Add(k, data[k]);
                }
            }

            return objException;
        }

    }
}
