using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using System.Collections.Generic;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Newtonsoft.Json.Converters;

namespace Tesco.ClubcardProducts.MCA.API.Common.Utilities
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public class GeneralUtility
    {
        /// <summary>Convert HashTable to XML</summary>
        /// <param name="ht"> HashTable to convert into XML </param>
        /// <param name="objName"> Name of the object </param>
        /// <returns> Returns XML </returns>
        /// <remarks>This method accepts a HashTable converts into XML and returning the XML in the form of string</remarks>
        /// 
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
                //NGCTrace.NGCTrace.TraceCritical("Critical:Helper.cs - Error Message :" + ex.ToString());
                //NGCTrace.NGCTrace.TraceError("Error:Helper.cs- Error Message :" + ex.ToString());
                //NGCTrace.NGCTrace.TraceWarning("Warning:Helper.cs");
                //NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return Convert.ToString(sb);
        }

        public static int GetInt32Value(DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return 0;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return 0;

                if (dr[columnName].ToString().Length == 0)
                    return 0;

                return Convert.ToInt32(dr[columnName].ToString());
            }
        }

        public static Int64 GetInt64Value(DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return 0;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return 0;

                if (dr[columnName].ToString().Length == 0)
                    return 0;

                return Convert.ToInt64(dr[columnName].ToString());
            }
        }

        public static DateTime? GetDateTimeValue(DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return (DateTime?)null;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return (DateTime?)null;

                if (dr[columnName].ToString().Length == 0)
                    return (DateTime?)null;
                try
                {
                    return Convert.ToDateTime(dr[columnName].ToString());
                }
                catch (Exception ex)
                {
                    return (DateTime?)null;
                }

            }
        }

        public static Exception GetCustomException(string message, Exception ex, Dictionary<string, object> data)
        {
            Exception objException = new Exception(message, ex);

            if (data != null)
            {
                foreach (string k in data.Keys)
                {
                    if (!objException.Data.Contains(k))
                    {
                        objException.Data.Add(k, data[k]);
                    }
                }
            }

            return objException;
        }

        public static bool StringComparison(string s1, string s2)
        {
            if (s1.Length != s2.Length) return false;

            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] != s2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}