using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.ComponentModel;
using System.Data;

namespace CompareConfigurations.Classes
{
    public static class Extensions
    {
        public static T ToObject<T>(this string xmlString)
        {
            var stringReader = new System.IO.StringReader(xmlString);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }

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

        public static T GetValue<T>(this DataRow row, string column)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            if (row != null && row.Table.Columns.Contains(column) && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    if (!Convert.IsDBNull(row[column]))
                    {
                        string tmp = row[column].ToString();
                        value = (T)converter.ConvertFrom(tmp);
                    }
                }
                catch { }
            }
            if (value == null && typeof(T).Name.Equals(typeof(string).Name))
            {
                value = (T)converter.ConvertFrom(string.Empty);
            }
            return value;
        }

        public static bool EqualSpecial(this string val, string target)
        {
            bool chk = true;
            try
            {
                if (val == null && target == null)
                {
                    return true;
                }
                if (val == null && target != null)
                {
                    return false;
                }
                if (val != null && target == null)
                {
                    return false;
                }
                if (val.Length != target.Length)
                {
                    return false;
                }
                for (int i = 0; i < ((val.Length > target.Length) ? val.Length : target.Length); i++)
                {
                    if (val[i] != target[i])
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
 
            }
            return chk;
        }
    }
}
