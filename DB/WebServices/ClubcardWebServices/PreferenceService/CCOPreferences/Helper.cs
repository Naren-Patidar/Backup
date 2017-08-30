using System;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Fujitsu.eCrm.Generic.SharedUtils;
using Tesco.NGC.Loyalty.EntityServiceLayer;

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
            StringBuilder sb = new StringBuilder("Helper.GetConsumerID");
            sb.Append(" Consumer: " + consumer);
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
            }
            return Convert.ToString(sb);
        }

        /// <summary>
        /// formats the clubcard number supplied and return it back
        /// masks the clubcard number as per the PCIDSS requirements
        /// </summary>
        /// <param name="cardNumber">clubcard number to be masked</param>
        /// <param name="isMaskReq">if clubcard number is required to be masked</param>
        /// <param name="maskChar">masking character</param>
        /// <returns>formatted clubcard number</returns>
        public static string MasknFormatClubcard(string clubcardNumber, bool isMaskReq, char maskChar)
        {
            #region Local variable declaration
            bool is18digit = false;
            string firstGroup = string.Empty, lastGroup = string.Empty;
            string middleGroup = string.Empty;
            StringBuilder maskedMiddleGroup = new StringBuilder();
            #endregion

            if (clubcardNumber.Length > 15)
            {
                if (clubcardNumber.Length == 18)//check if clubcard number is 18 digit number
                    is18digit = true;

                if (is18digit)
                    firstGroup = clubcardNumber.Substring(0, 6);//if yes then take first 6 digits as first group
                else
                    firstGroup = clubcardNumber.Substring(0, 4);//else take first 4 digits as first group
                lastGroup = clubcardNumber.Substring(clubcardNumber.Length - 4, 4);//always take last 4 digits as last group

                //take the middle digits left after first and last group seperated
                middleGroup = clubcardNumber.Substring(firstGroup.Length, clubcardNumber.Length - (firstGroup.Length + lastGroup.Length));
                //insert a space in between them (after 4th digit)
                middleGroup = middleGroup.Insert(4, " ");

                if (isMaskReq) // then replace the middle digits with masking character
                {
                    //foreach character in middle group
                    for (int index = 0; index < middleGroup.Length; index++)
                    {
                        //if first two charaters and clubcard number is not 18 digit then take the digits as it is without masking
                        if (index < 2 && !is18digit)
                            maskedMiddleGroup.Append(middleGroup[index]);
                        //else if the character is not the space, then mask the character with masking character
                        else if (middleGroup[index] != ' ')
                            maskedMiddleGroup.Append(maskChar);
                        //else add the space at the place of space
                        else
                            maskedMiddleGroup.Append(' ');
                    }
                }
                //return the formatted card number
                return firstGroup + " " + maskedMiddleGroup.ToString() + " " + lastGroup;
            }
            //if clubcard number found lesser than 16 digits the unformatted clubcard number is returned
            else return clubcardNumber;
        }
        /// <summary>
        /// formats the clubcard number supplied and return it back
        /// masks the clubcard number as per the PCIDSS requirements
        /// overloaded with isMaskReq flag set as true
        /// </summary>
        /// <param name="cardNumber">clubcard number to be masked</param>
        /// <param name="maskChar">masking character</param>
        /// <returns>formatted clubcard number</returns>
        public static string MasknFormatClubcard(string clubcardNumber, char maskChar)
        {
            return MasknFormatClubcard(clubcardNumber, true, maskChar);
        }
        /// <summary>
        /// formats the clubcard number supplied and return it back
        /// overloaded with isMaskReq flag set as true and masking character defaulted as 'X'
        /// masks the clubcard number as per the PCIDSS requirements
        /// </summary>
        /// <param name="cardNumber">clubcard number to be masked</param>
        /// <returns>formatted clubcard number</returns>
        public static string MasknFormatClubcard(string clubcardNumber)
        {
            return MasknFormatClubcard(clubcardNumber, true, 'X');
        }

    }
}