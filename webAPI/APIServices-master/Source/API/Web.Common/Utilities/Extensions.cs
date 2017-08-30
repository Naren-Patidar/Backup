using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Xml;
using System.Web;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using System.Configuration;

namespace Tesco.ClubcardProducts.MCA.API.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to XElement to get value in given type
        /// </summary>
        /// <typeparam name="T">given Type</typeparam>
        /// <param name="element">XElement </param>
        /// <returns>element value in the given type</returns>
        
        public static T GetValue<T>(this XElement element)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (element != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    value = (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, element.Value);

                }
                catch { }
            }
            else if (element == null && typeof(T).Name.Equals(typeof(string).Name))
            {
                value = (T)converter.ConvertFrom(string.Empty);
            }
            return value;
        }

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

        /// <summary>
        /// Extension method to object to convert it to given type
        /// </summary>
        /// <typeparam name="T">given Type </typeparam>
        /// <param name="objValue">object value</param>
        /// <returns>converted value in given type</returns>
        public static decimal TryParseDecimal(this object objValue)
        {
            Decimal value = default(decimal);
            if (!Convert.IsDBNull(objValue) && objValue != null)
            {
                try
                {
                    value = Convert.ToDecimal(objValue.ToString(), CultureInfo.InvariantCulture);
                }
                catch { }
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static bool TryParseDate(this string objValue, out DateTime dtResult)
        {
            dtResult = DateTime.Now;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    string[] dateformats = ConfigurationManager.AppSettings["DateFormats"].ToString()
                                            .Replace("\n", "")
                                            .Replace("\"", "")
                                            .Replace("\r", "")                                            
                                            .Split(',');
                    dateformats = dateformats.ToList().ConvertAll(s => s.Trim().TrimStart('\'').TrimEnd('\'')).ToArray();
                    return DateTime.TryParseExact(objValue, dateformats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtResult);
                }
                catch { }
            }
            return false;
        }

        public static bool CompareDateTimeOnly(this DateTime dtSource, DateTime dtTarget)
        {
            return (dtSource.Day == dtTarget.Day &&
                    dtSource.Month == dtTarget.Month &&
                    dtSource.Year == dtTarget.Year &&
                    dtSource.Hour == dtTarget.Hour &&
                    dtSource.Minute == dtTarget.Minute &&
                    dtSource.Second == dtTarget.Second);
        }
    
        /// <summary>
        /// Extension method to DataSet to get the xml  
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>xml string</returns>
        public static string ToXml(this DataSet ds)
        {
            string xml = string.Empty;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (TextWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(DataSet));
                        xmlSerializer.Serialize(streamWriter, ds);
                        xml = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
            catch { }
            return xml;
        }

        /// <summary>
        /// Extension method of DataRow to get the column value in the expected type
        /// </summary>
        /// <typeparam name="T">Type expected</typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="column">data column name</param>
        /// <returns>Type value</returns>
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
            return value;
        }

        /// <summary>
        /// extension method to object to convert it to xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXmlString(this object obj)
        {
            string xmlString = string.Empty;
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false);
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, obj);
                    }
                    xmlString = textWriter.ToString();
                }
            }
            catch { }
            return xmlString;
        }

        public static T ToObject<T>(this string xmlObj)
        {
            T value = default(T);
            try
            {
                var stringReader = new System.IO.StringReader(xmlObj);
                var serializer = new XmlSerializer(typeof(T));
                value = (T)serializer.Deserialize(stringReader);
            }
            catch { }
            return value;
        }

        public static string JsonText(this object obj)
        {
            return JsonConvert.SerializeObject(obj);            
        }

        public static T JsonToObject<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public static T FetchXElement<T>(this XDocument doc, string root, string element)
        {
            XElement elRoot = doc.Element(XName.Get(root));
            if (elRoot != null)
            {
                XElement node = elRoot.Element(element);
                if (node != null)
                {
                    return node.Value.TryParse<T>();
                }
            }

            return default(T);
        }
    }
}
