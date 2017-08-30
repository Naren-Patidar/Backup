using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Xml;

namespace APITestClient.Helper
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to object to convert it to given type
        /// </summary>
        /// <typeparam name="T">given Type </typeparam>
        /// <param name="objValue">object value</param>
        /// <returns>converted value in given type</returns>
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

        public static Uri ParseUri(this Uri uri)
        {
            Uri parsedUri = uri;
            string protocol = ConfigurationManager.AppSettings["Protocol"];
            if (!string.IsNullOrEmpty(protocol))
            {
                if (!uri.AbsolutePath.StartsWith(protocol))
                {
                    parsedUri = new Uri(uri.AbsoluteUri.ToLower().Replace(uri.Scheme.ToLower(), protocol));
                }
            }
            return parsedUri;
        }

        public static string HashTableToXML(Hashtable ht, string objName)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    writer.WriteStartElement(objName);
                    foreach (DictionaryEntry item in ht)
                    {
                        writer.WriteStartElement(item.Key.ToString().Trim());
                        writer.WriteValue(item.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Convert.ToString(sb);
        }
    }
}