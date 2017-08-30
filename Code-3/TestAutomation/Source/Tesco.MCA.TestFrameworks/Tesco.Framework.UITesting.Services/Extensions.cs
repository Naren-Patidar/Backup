using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Linq;
using System.Globalization;

namespace Tesco.Framework.UITesting.Services
{
    public static class Extensions
    {
        public static T GetValue<T>(this XElement element)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (element != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    value = (T)converter.ConvertFrom(element.Value);
                }
                catch { }
            }
            else if (element == null && typeof(T).Name.Equals(typeof(string).Name))
            {
                value = (T)converter.ConvertFrom(string.Empty);
            }
            return value;
        }

        public static bool TryParseDate(this string objValue, out DateTime dtResult)
        {
            bool bReturn = false;
            dtResult = DateTime.Now;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    if (DateTime.TryParseExact(objValue, Extensions.DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtResult))
                    {
                        bReturn = true;
                    }
                }
                catch { }
            }
            return bReturn;
        }

        public static string[] DateFormats = 
        {
            "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
            "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
            "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
            "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
            "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
            "yyyy/MM/dd hh:mm", "yyyy/MM/dd hh:mm:ss",
            "yyyy-MM-ddThh:mm:ss:fff", "yyyy-MM-ddThh:mm:ss",
            "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd hh:mm",
            "M-dd-yyyy h:mm:ss", "M-dd-yyyy h:mm",
            "M-dd-yyyy hh:mm:ss", "M-dd-yyyy hh:mm",
            "yyyy/M/dd hh:mm", "yyyy/M/dd hh:mm:ss",
            "M/d/yyyy H:mm:ss tt", "M/d/yyyy H:mm tt", 
            "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss", 
            "M/d/yyyy HH:mm tt", "M/d/yyyy HH tt", 
            "M/d/yyyy H:mm", "M/d/yyyy H:mm", 
            "MM/dd/yyyy HH:mm", "M/dd/yyyy HH:mm",
            "yyyy/MM/dd HH:mm", "yyyy/MM/dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss:fff", "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm",
            "M-dd-yyyy H:mm:ss", "M-dd-yyyy H:mm",
            "M-dd-yyyy HH:mm:ss", "M-dd-yyyy HH:mm",
            "yyyy/M/dd HH:mm", "yyyy/M/dd HH:mm:ss",
            "dd/MM/yy", "MMM dd yyyy hh:mmtt", "MMM d yyyy hh:mmtt",
            "yyyy-MM-ddTHH:mm:sszzz", "yyyy-MM-ddTHH:mm:ssTZD",
            "yyyy-MM-ddTHH:mm:ssTZ", "dd MMM yyyy",
            "dd/MM/yyyy HH:mm:ss"
        };

    }

}
