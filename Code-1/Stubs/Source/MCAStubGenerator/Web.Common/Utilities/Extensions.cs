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

namespace Tesco.ClubcardProducts.MCA.Web.Common
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static string FormatDate(this DateTime dateValue, string format)
        {
            return dateValue.ToString(format);
        }

        /// <summary>
        /// Extension string method to format string date to string
        /// </summary>
        /// <param name="objValue"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatStringDate(this string objValue, string format)
        {
            string value = string.Empty;
            DateTime date;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    DateTime.TryParse(objValue, out date);
                    value = date.ToString(format, CultureInfo.InvariantCulture);
                }
                catch { }
            }
            return value;
        }

        public static bool CompareDateOnly(this DateTime dtSource, DateTime dtTarget)
        {
            return (dtSource.Day == dtTarget.Day &&
                    dtSource.Month == dtTarget.Month &&
                    dtSource.Year == dtTarget.Year);
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



        /// <summary>
        /// Extension method to HtmlHelper to get the resource values
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="key">resource key</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString Translate(this HtmlHelper htmlHelper, string key)
        {
            MvcHtmlString res = new MvcHtmlString(string.Empty);
            try
            {
                string ResourceVal = string.Empty;
                var viewPath = ((System.Web.Mvc.RazorView)htmlHelper.ViewContext.View).ViewPath;
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;

                var httpContext = htmlHelper.ViewContext.HttpContext;
                ResourceVal = (string)httpContext.GetLocalResourceObject(viewPath, key, culture);
                res = MvcHtmlString.Create(ResourceVal);
            }
            catch { }
            return res;
        }

        /// <summary>
        /// Extension method to HtmlHelper to get the resource values for partial view
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="view">partial view name</param>
        /// <param name="key">resource key</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString Translate(this HtmlHelper htmlHelper, string view, string key)
        {
            MvcHtmlString res = new MvcHtmlString(string.Empty);
            try
            {
                string ResourceVal = string.Empty;
                var viewPath = string.Format("~/Views/{0}.cshtml", view);
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;

                var httpContext = htmlHelper.ViewContext.HttpContext;
                ResourceVal = (string)httpContext.GetLocalResourceObject(viewPath, key, culture);
                res = MvcHtmlString.Create(ResourceVal);
            }
            catch { }
            return res;
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

        public static MvcHtmlString LoadHtmlContent(this HtmlHelper html, string file)
        {
            string htmlContent = string.Empty;
            if (File.Exists(html.ViewContext.HttpContext.Server.MapPath(file)))
            {
                htmlContent = System.IO.File.ReadAllText(html.ViewContext.HttpContext.Server.MapPath(file));
            }
            return MvcHtmlString.Create(htmlContent);
        }

        public static MvcHtmlString LoadHtmlContent(this HtmlHelper html, string file, object[] arguments)
        {
            string htmlContent = string.Empty;
            if (File.Exists(html.ViewContext.HttpContext.Server.MapPath(file)))
            {
                htmlContent = System.IO.File.ReadAllText(html.ViewContext.HttpContext.Server.MapPath(file));
                htmlContent = string.Format(htmlContent, arguments);
            }
            return MvcHtmlString.Create(htmlContent);
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

        public static string GetLocalResource(this HttpContextBase httpContext, string viewPath, string key)
        {
            string value = string.Empty;
            try
            {
                value = (string)httpContext.GetLocalResourceObject(viewPath, key, CultureInfo.CurrentCulture);
            }
            catch { }
            return value;
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

        private class ScriptBlock : IDisposable
        {
            private const string scriptsKey = "scripts";
            public static List<string> pageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[scriptsKey] == null)
                        HttpContext.Current.Items[scriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[scriptsKey];
                }
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                pageScripts.Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.pageScripts.Select(s => s.ToString())));
        }


    }
}
