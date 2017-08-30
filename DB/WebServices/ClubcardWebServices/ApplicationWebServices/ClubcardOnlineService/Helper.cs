using System;
using System.Configuration;
using System.Xml;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Fujitsu.eCrm.Generic.SharedUtils;

namespace Tesco.com.ClubcardOnlineService
{
    /// <summary>
    /// Helper class containing common static methods used in CCO Services
    /// <para>Date: 25/05/2010</para>
    /// <para>Author: Padmanabh Ganorkar</para>
    /// </summary>
    internal class Helper
    {
        /// <summary>
        /// Returns the User ID which will be used in NGC database while updating the records
        /// These User IDs are configured in App.config
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public static short GetConsumerID(string consumer)
        {
            ApplicationUser appUserObject = null;
            short userID = 0;
            int maxRowCount = 100;
            int rowCount = 0;
            string resultXML = string.Empty;
            XmlDocument appUserInfo = null;
            string culture = string.Empty;
            Hashtable htblConsumer = null;


            #region Trace
            Trace trace = new Trace();
            StringBuilder sb = new StringBuilder("Helper.GetConsumerID");
            sb.Append(" Consumer: " + consumer);
            ITraceState trState = trace.StartProc(sb.ToString());
            #endregion

            try
            {
                //If userid is in cache
                if (HttpRuntime.Cache.Get("UserID") != null && HttpRuntime.Cache.Get("Consumer").ToString() == consumer)
                {
                    userID = Convert.ToInt16(HttpRuntime.Cache.Get("UserID"));
                }
                else
                {
                    appUserObject = new ApplicationUser();

                    htblConsumer = new Hashtable();
                    htblConsumer["UserName"] = consumer;

                    string appUser = HashTableToXML(htblConsumer, "ApplicationUser");

                    resultXML = appUserObject.Search(appUser, maxRowCount, out rowCount, culture);

                    if ((resultXML != null) && (resultXML != "<NewDataSet />") && (!string.IsNullOrEmpty(resultXML)))
                    {
                        appUserInfo = new XmlDocument();
                        appUserInfo.LoadXml(resultXML);

                        //Read the value of UserID element from XML string.
                        XmlNodeList eleName = appUserInfo.DocumentElement.GetElementsByTagName("UserID");
                        if (eleName != null)
                        {
                            userID = Convert.ToInt16(eleName[0].InnerText);
                        }

                        HttpRuntime.Cache.Insert("UserID", userID);
                        HttpRuntime.Cache.Insert("Consumer", consumer);
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (appUserObject != null)
                {
                    appUserObject = null;
                }

                resultXML = null;
                appUserInfo = null;
                trState.EndProc();
            }

            return userID;
        }

        /// <summary>Convert HashTable to XML</summary>
        /// <param name="ht"> HashTable to convert into XML </param>
        /// <param name="objName"> Name of the object </param>
        /// <returns> Returns XML </returns>
        /// <remarks>This method accepts a HashTable converts into XML and returning the XML in the form of string</remarks>
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
                        writer.WriteStartElement(item.Key.ToString());
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
                //trState.EndProc();
            }
            return Convert.ToString(sb);
        }
    }
}