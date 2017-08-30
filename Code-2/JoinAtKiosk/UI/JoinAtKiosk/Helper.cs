using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Web.Caching;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.CustomerService;
using System.ServiceModel;
using System.Diagnostics;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public class Helper
    {
        private static string OverWriteNGCConfig = string.Empty;

        #region Next Page
        /// <summary>
        /// It accepts the current page value, splits and returns the Next Page name
        /// </summary>
        /// <param name="currentPage">current page</param>
        /// <returns>Next Page Name</returns>
        public static string NextPage(string currentPage)
        {
            string[] strArrNextPage = new string[2];

            try
            {
                strArrNextPage = currentPage.Split('|');                                                
            }
            catch
            {
                return currentPage;
            }

            return strArrNextPage[1].ToString().Trim();
        }
        #endregion

        #region Previous Page
        /// <summary>
        /// It accepts the current page value, splits and returns the Previous Page name
        /// </summary>
        /// <param name="currentPage">current page</param>
        /// <returns>Previous Page Name</returns>
        public static string PreviousPage(string currentPage)
        {
            string[] strArrPreviousPage = new string[2];

            try
            {
                strArrPreviousPage = currentPage.Split('|');
            }
            catch
            {
                return currentPage;
            }

            return strArrPreviousPage[0].ToString().Trim();
        }
        #endregion

        #region HashTable to XML

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

            }
            finally
            {

            }
            return Convert.ToString(sb);
        }
        #endregion

        /// <summary>
        /// USed for validations, server side, validates with the help of regular expression.
        /// pass isEmptyAllowed as false if you want to check string.IsEmpty
        /// </summary>
        /// <param name="val"></param>
        /// <param name="regex"></param>
        /// <param name="isEmptyAllowed"></param>
        /// <returns></returns>
        public static bool IsRegexMatch(string val, string regex, bool isEmptyAllowed, bool IgnoreCase)
        {
            Regex objNaturalPattern = null;
            if (val == "" && isEmptyAllowed == true)
                return true;
            else if (val == "" && isEmptyAllowed == false)
                return false;
            if (IgnoreCase)
                objNaturalPattern = new Regex(regex, RegexOptions.IgnoreCase);
            else
                objNaturalPattern = new Regex(regex);
            return objNaturalPattern.IsMatch(val);
        }

        /// <summary>
        /// Encrypts the key and value for the cookie with TripleDES algorithm
        /// <para>and sets the cookie to the HttpContext</para>
        /// <para>cookie expiration is 3600 minutes (60 hours) by default</para>
        /// <para>(configurable with TimerCCOCookieExpiration appSettings)</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <param name="value">cookie value</param>
        public static void SetTripleDESEncryptedCookie(string key, string value)
        {
            //Convert parts 
            key = CryptoUtil.EncryptTripleDES(key);
            value = CryptoUtil.EncryptTripleDES(value);
            DateTime cookieExpiration = DateTime.Now.AddMinutes(
                        Convert.ToInt32(ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
            SetCookie(key, value, cookieExpiration);
        }

        /// <summary>
        /// SetTripleDESEncryptedCookie - overloaded method with expires parameter
        /// <para>Encrypts the key and value for the cookie with TripleDES algorithm</para>
        /// <para>and sets the cookie to the HttpContext</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <param name="value">cookie value</param>
        /// <param name="expires">expiration DateTime</param>
        public static void SetTripleDESEncryptedCookie(string key, string value, System.DateTime expires)
        {
            //Convert parts
            key = CryptoUtil.EncryptTripleDES(key);
            value = CryptoUtil.EncryptTripleDES(value);

            SetCookie(key, value, expires);
        }

        /// <summary>
        /// SetCookie - key & value only, without expiration
        /// <para>Direct interation with HttpContext</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <param name="value">cookie value</param>
        private static void SetCookie(string key, string value)
        {
            //Encode Part
            key = HttpContext.Current.Server.UrlEncode(key);
            value = HttpContext.Current.Server.UrlEncode(value);

            HttpCookie cookie = default(HttpCookie);
            cookie = new HttpCookie(key, value);
            SetCookie(cookie);
        }

        /// <summary>
        /// SetCookie - overloaded with expires parameter, without expiration
        /// <para>Direct interation with HttpContext</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <param name="value">cookie value</param>
        /// <param name="expires">expiration DateTime</param>
        private static void SetCookie(string key, string value, System.DateTime expires)
        {
            //Encode Parts
            key = HttpContext.Current.Server.UrlEncode(key);
            value = HttpContext.Current.Server.UrlEncode(value);

            HttpCookie cookie = default(HttpCookie);
            cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            SetCookie(cookie);
        }

        /// <summary>
        /// SetCookie - HttpCookie only
        /// <para>final step to set the cookie to the response clause</para>
        /// <para>Direct interation with HttpContext</para>
        /// </summary>
        /// <param name="cookie">HttpCookie</param>
        private static void SetCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// encrypts the string key and then fetches and decryptes
        /// <para>the value for the Cookie with TripleDES decryption algo</para>
        /// <para>returns the decrypted value</para>
        /// </summary>
        /// <param name="key">not encrypted, normal key for the cookie</param>
        /// <returns>returns the decrypted value</returns>
        public static string GetTripleDESEncryptedCookieValue(string key)
        {
            //encrypt key only - encoding done in GetCookieValue
            key = CryptoUtil.EncryptTripleDES(key);
            //get value 
            string value = null;
            value = GetCookieValue(key);
            //decrypt value
            value = CryptoUtil.DecryptTripleDES(value);
            return value;
        }
        // added by Robin Apoto.
        //Date: 09 Sept 2010
        //Reason: To reset cookie expiration time for active user.
        public static string CheckAndResetCookieExpiration(string key)
        {
            //encrypt key only - encoding done in GetCookieValue
            key = CryptoUtil.EncryptTripleDES(key);
            //get value 
            string value = null;
            value = GetCookieValue(key);
            //encrypt value
            value = CryptoUtil.EncryptTripleDES(value);
            if (!string.IsNullOrEmpty(value))
            {
                DateTime cookieExpiration = DateTime.Now.AddMinutes(
                        Convert.ToInt32(ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
                value = CryptoUtil.DecryptTripleDES(value);
                SetCookie(key, value, cookieExpiration);
            }
            //decrypt value
            value = CryptoUtil.DecryptTripleDES(value);
            return value;
        }

        /// <summary>
        /// Fetches the value from the HttpContext from the key supplied
        /// <para>Direct interation with HttpContext</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <returns>HttpCookie object for the cookie key supplied</returns>
        private static HttpCookie GetCookie(string key)
        {
            //encode key for retrieval
            key = HttpContext.Current.Server.UrlEncode(key);
            return HttpContext.Current.Request.Cookies.Get(key);
        }

        /// <summary>
        /// Fetches the value from the HttpContext from the key supplied
        /// <para>Direct interation with HttpContext</para>
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <returns>string value for the cookie key supplied</returns>
        private static string GetCookieValue(string key)
        {
            try
            {
                //don't encode key for retrieval here
                //done in the GetCookie function

                //get value 
                string value = null;
                value = GetCookie(key).Value;
                //decode stored value
                value = HttpContext.Current.Server.UrlDecode(value);
                return value;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Nullifies the Cookie and set the expiration of the cookie to - 1 day
        /// <para>So that on the next page request the cookie will get deleted from the client browser</para>
        /// </summary>
        /// <param name="key">key name for the cookie</param>
        public static void DeleteTripleDESEncryptedCookie(string key)
        {
            key = CryptoUtil.EncryptTripleDES(key);
            string value = CryptoUtil.EncryptTripleDES(string.Empty);
            SetCookie(key, value, DateTime.Now.AddDays(-1d));
        }

        public static void DeleteAllCookies()
        {
            #region Title Page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Tilte");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("FirstName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Initial");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("LastName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ClientID")))
            {
                Helper.DeleteTripleDESEncryptedCookie("ClientID");
            }
            #endregion

            #region Address page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PostCode");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MailingAddress1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MailingAddress2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MailingAddress3");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress4")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MailingAddress4");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress5")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MailingAddress5");
            }
            #endregion

            #region Contact page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAddress")))
            {
                Helper.DeleteTripleDESEncryptedCookie("EmailAddress");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EveningNumber")))
            {
                Helper.DeleteTripleDESEncryptedCookie("EveningNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MobileNumber")))
            {
                Helper.DeleteTripleDESEncryptedCookie("MobileNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DaytimeNumber");
            }
            #endregion

            #region FurtherPersonalDetails 1
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PreferredLanguage");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageKey")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PreferredLanguageKey");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageValue")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PreferredLanguageValue");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PassportNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")))
            {
                Helper.DeleteTripleDESEncryptedCookie("SSN");
            }
            #endregion
            
            #region FurtherPersonalDetails 2
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Race")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Race");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceID")))
            {
                Helper.DeleteTripleDESEncryptedCookie("RaceID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceSelect")))
            {
                Helper.DeleteTripleDESEncryptedCookie("RaceSelect");
            }
            #endregion

            #region Date of Birth & HH Ages page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Date");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Month");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Year");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DateOfBirth")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DateOfBirth");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Age1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Age2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Age3");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Age4");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Age5");
            }
            #endregion

            #region Dietry Page Cookies
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox1")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DietCheckBox1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox2")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DietCheckBox2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SelectedDietryPref")))
            {
                Helper.DeleteTripleDESEncryptedCookie("SelectedDietryPref");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesID")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DietryPreferencesID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DietryPreferencesName");
            }
            #endregion

            #region Contact Preferences
            //Existing Preferences. Check who is setting this cookie and from which page, since NGC is using these too.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoProduct");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoPartnerInfo");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")))
            {
                Helper.DeleteTripleDESEncryptedCookie("CustomerResearch");
            }

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoGroupMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoGroupEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoGroupPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TescoGroupSMS");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerMail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PartnerMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerEmail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PartnerEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerPhone")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PartnerPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerSMS")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PartnerSMS");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchMail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("ResearchMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchEmail")))
            {
                Helper.DeleteTripleDESEncryptedCookie("ResearchEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchPhone")))
            {
                Helper.DeleteTripleDESEncryptedCookie("ResearchPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchSMS")))
            {
                Helper.DeleteTripleDESEncryptedCookie("ResearchSMS");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PromotionCode");
            }
            #endregion
          
            #region configdetails
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")))
            {
                Helper.DeleteTripleDESEncryptedCookie("PostCodeAndAddress");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("TitleAndName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
            {
                Helper.DeleteTripleDESEncryptedCookie("DOBDietry");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")))
            {
                Helper.DeleteTripleDESEncryptedCookie("EmailAndPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
            {
                Helper.DeleteTripleDESEncryptedCookie("FurtherPersonalDetails");
            }
            #endregion
        }

        public static void CheckAndResetCookie()
        {
            #region Title Page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
            {
                Helper.CheckAndResetCookieExpiration("Tilte");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")))
            {
                Helper.CheckAndResetCookieExpiration("FirstName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")))
            {
                Helper.CheckAndResetCookieExpiration("Initial");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")))
            {
                Helper.CheckAndResetCookieExpiration("LastName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserFirstName")))
            {
                Helper.CheckAndResetCookieExpiration("UserFirstName");
            }
            #endregion
            #region Address page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")))
            {
                Helper.CheckAndResetCookieExpiration("PostCode");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")))
            {
                Helper.CheckAndResetCookieExpiration("MailingAddress1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")))
            {
                Helper.CheckAndResetCookieExpiration("MailingAddress2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")))
            {
                Helper.CheckAndResetCookieExpiration("MailingAddress3");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress4")))
            {
                Helper.CheckAndResetCookieExpiration("MailingAddress4");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress5")))
            {
                Helper.CheckAndResetCookieExpiration("MailingAddress5");
            }
            #endregion

            #region Contact page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAddress")))
            {
                Helper.CheckAndResetCookieExpiration("EmailAddress");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EveningNumber")))
            {
                Helper.CheckAndResetCookieExpiration("EveningNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MobileNumber")))
            {
                Helper.CheckAndResetCookieExpiration("MobileNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber")))
            {
                Helper.CheckAndResetCookieExpiration("DaytimeNumber");
            }
            #endregion

            #region Further Personal Details1 page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage")))
            {
                Helper.CheckAndResetCookieExpiration("PreferredLanguage");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageKey")))
            {
                Helper.CheckAndResetCookieExpiration("PreferredLanguageKey");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageValue")))
            {
                Helper.CheckAndResetCookieExpiration("PreferredLanguageValue");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")))
            {
                Helper.CheckAndResetCookieExpiration("PassportNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")))
            {
                Helper.CheckAndResetCookieExpiration("SSN");
            }
            #endregion

            #region Further Personal Details2 page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Race"))) //Eg: A or B or C to display on page load
            {
                Helper.CheckAndResetCookieExpiration("Race");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceID"))) //Eg: 1, 2, 3 to include in XML that is sent to NGC.
            {
                Helper.CheckAndResetCookieExpiration("RaceID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceSelect")))
            {
                Helper.CheckAndResetCookieExpiration("RaceSelect");
            }
            #endregion

            #region Date of Birth & HH Ages page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
            {
                Helper.CheckAndResetCookieExpiration("Date");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
            {
                Helper.CheckAndResetCookieExpiration("Month").ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
            {
                Helper.CheckAndResetCookieExpiration("Year").ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DateOfBirth")))
            {
                Helper.CheckAndResetCookieExpiration("DateOfBirth");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")))
            {
                Helper.CheckAndResetCookieExpiration("Age1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")))
            {
                Helper.CheckAndResetCookieExpiration("Age2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")))
            {
                Helper.CheckAndResetCookieExpiration("Age3");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")))
            {
                Helper.CheckAndResetCookieExpiration("Age4");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")))
            {
                Helper.CheckAndResetCookieExpiration("Age5");
            }
            #endregion

            #region Dietary Preferences page Cookies.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox1")))
            {
                Helper.CheckAndResetCookieExpiration("DietCheckBox1");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox2")))
            {
                Helper.CheckAndResetCookieExpiration("DietCheckBox2");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SelectedDietryPref")))
            {
                Helper.CheckAndResetCookieExpiration("SelectedDietryPref");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesID")))
            {
                Helper.CheckAndResetCookieExpiration("DietryPreferencesID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName")))
            {
                Helper.CheckAndResetCookieExpiration("DietryPreferencesName");
            }
            #endregion

            #region Contact Preferences
            //Existing Preferences. Check who is setting this cookie and from which page, since NGC is using these too.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")))
            {
                Helper.CheckAndResetCookieExpiration("TescoProduct");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")))
            {
                Helper.CheckAndResetCookieExpiration("TescoPartnerInfo");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")))
            {
                Helper.CheckAndResetCookieExpiration("CustomerResearch");
            }

            //Picking From Contact Preferences.
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail")))
            {
                Helper.CheckAndResetCookieExpiration("TescoGroupMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail")))
            {
                Helper.CheckAndResetCookieExpiration("TescoGroupEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone")))
            {
                Helper.CheckAndResetCookieExpiration("TescoGroupPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS")))
            {
                Helper.CheckAndResetCookieExpiration("TescoGroupSMS");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerMail")))
            {
                Helper.CheckAndResetCookieExpiration("PartnerMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerEmail")))
            {
                Helper.CheckAndResetCookieExpiration("PartnerEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerPhone")))
            {
                Helper.CheckAndResetCookieExpiration("PartnerPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerSMS")))
            {
                Helper.CheckAndResetCookieExpiration("PartnerSMS");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchMail")))
            {
                Helper.CheckAndResetCookieExpiration("ResearchMail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchEmail")))
            {
                Helper.CheckAndResetCookieExpiration("ResearchEmail");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchPhone")))
            {
                Helper.CheckAndResetCookieExpiration("ResearchPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchSMS")))
            {
                Helper.CheckAndResetCookieExpiration("ResearchSMS");
            }
            #endregion

            #region config details
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")))
            {
                Helper.CheckAndResetCookieExpiration("PostCodeAndAddress");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
            {
                Helper.CheckAndResetCookieExpiration("TitleAndName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
            {
                Helper.CheckAndResetCookieExpiration("DOBDietry");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")))
            {
                Helper.CheckAndResetCookieExpiration("EmailAndPhone");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
            {
                Helper.CheckAndResetCookieExpiration("FurtherPersonalDetails");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
            {
                Helper.CheckAndResetCookieExpiration("PromotionCode");
            }
           

            #endregion
        }

        public static void GetAndLoadConfigurationDetails()
        {
            Net35BasicAuthentication();
            CustomerServiceClient custClient = null;
            string conditionXML = ConfigurationReader.GetStringConfigKey("GetNCGConfigDetails");
            string errorXml = string.Empty;
            string resultxml = string.Empty;
            int rowCount = 0;
            string resultXml = string.Empty;
            XmlDocument resulDoc = null;
            DataSet dsConfigDetails = null;
            CommonClassForJoin.CurrentCulture = ConfigurationReader.GetStringConfigKey("CountryCode");

            try
            {
                object tmpCache = HttpContext.Current.Cache["InitCache"];
                double cacheTime = Convert.ToDouble(ConfigurationReader.GetStringConfigKey("CacheTime"));
                if (tmpCache == null)
                {
                    CommonClassForJoin com = new CommonClassForJoin();
                    dsConfigDetails = new DataSet();
                    custClient = new CustomerServiceClient();

                    custClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, CommonClassForJoin.CurrentCulture);
                    HttpContext.Current.Cache.Insert("InitCache", resultXml, null, DateTime.Now.AddMinutes(cacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                    UpdateConfigDetails(dsConfigDetails);
                }
               
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (custClient != null)
                {
                    if (custClient.State == CommunicationState.Faulted)
                    {
                        custClient.Abort();
                    }
                    else if (custClient.State != CommunicationState.Closed)
                    {
                        custClient.Close();
                    }
                }
            }
        }

        /// <summary>
        /// create cookies for all config details
        /// </summary>
        /// <param name="dsConfigDetails"></param>
        public static void UpdateConfigDetails(DataSet dsConfigDetails)
        {
            DataView dvConfigDetails = new DataView();
            dvConfigDetails.Table = dsConfigDetails.Tables["ActiveDateRangeConfig"];

            OverWriteNGCConfig = ConfigurationReader.GetStringConfigKeyToUpper("OverWriteNGCConfig");

            UpdateNameConfigDetails(dsConfigDetails);
            UpdateAddressConfigDetails(dsConfigDetails);
            UpdateContactDetails(dsConfigDetails);
            UpdateDOBandHHAgesConfigDetails(dsConfigDetails);
            UpdateDietryPref_ContactPrefConfigDetails(dsConfigDetails);
            UpdateConfirmConfigDetails(dsConfigDetails);
            UpdateFD1ConfigDetails(dsConfigDetails);
            UpdateFD2ConfigDetails(dsConfigDetails);
            UpdateProfinityConfigDetails(dsConfigDetails);
        }
        private static void UpdateDietryPref_ContactPrefConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "2") //Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "DIETRYPREFERENCES")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.DietryPrefReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.DietryPrefReq = false;
                        }
                    }
                }
            }
            if (OverWriteNGCConfig.Contains("DIETRYPREFREQ"))
            {
                CommonClassForJoin.DietryPrefReq = ConfigurationReader.GetBooleanConfigKey("DietryPrefReq");
            }
        }
        private static void UpdateDOBandHHAgesConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "2") //Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "DATEOFBIRTH")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.DOBReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.DOBReq = false;
                        }
                    }

                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "HHAGE1")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.HHAge1Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.HHAge1Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "HHAGE2")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.HHAge2Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.HHAge2Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant()== "HHAGE3")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.HHAge3Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.HHAge3Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "HHAGE4")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.HHAge4Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.HHAge4Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim().ToUpperInvariant() == "HHAGE5")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.HHAge5Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.HHAge5Req = false;
                        }
                    }
                }
            }
            if (OverWriteNGCConfig.Contains("DOBREQ"))
            {
                CommonClassForJoin.DOBReq = ConfigurationReader.GetBooleanConfigKey("DOBReq");
            }
            if (OverWriteNGCConfig.Contains("HHAGE1REQ"))
            {
                CommonClassForJoin.HHAge1Req = ConfigurationReader.GetBooleanConfigKey("HHAge1Req");
            }
            if (OverWriteNGCConfig.Contains("HHAGE2REQ"))
            {
                CommonClassForJoin.HHAge2Req = ConfigurationReader.GetBooleanConfigKey("HHAge2Req");
            }
            if (OverWriteNGCConfig.Contains("HHAGE3REQ"))
            {
                CommonClassForJoin.HHAge3Req = ConfigurationReader.GetBooleanConfigKey("HHAge3Req");
            }
            if (OverWriteNGCConfig.Contains("HHAGE4REQ"))
            {
                CommonClassForJoin.HHAge4Req = ConfigurationReader.GetBooleanConfigKey("HHAge4Req");
            }
            if (OverWriteNGCConfig.Contains("HHAGE5REQ"))
            {
                CommonClassForJoin.HHAge5Req = ConfigurationReader.GetBooleanConfigKey("HHAge5Req");
            }
        }
        private static void UpdateConfirmConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "5")// max length
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "PromotionalCode")
                    {
                        CommonClassForJoin.PromotionalCodeMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                        CommonClassForJoin.PromotionalCodeMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "2")//Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "PromotionalCode")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.PromotionalCodeReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.PromotionalCodeReq = false;
                        }
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "11") //Get Join Code.
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Join Route Code")
                    {
                        CommonClassForJoin.JoinRouteCodeForKiosk = dr["ConfigurationValue2"].ToString();
                    }
                }
            }
            if (OverWriteNGCConfig.Contains("PROMOTIONALCODEREQ"))
            {
                CommonClassForJoin.PromotionalCodeReq = ConfigurationReader.GetBooleanConfigKey("PromotionalCodeReq");
            }
        }
        private static void UpdateAddressConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "10")// regural exppression
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {
                        CommonClassForJoin.PostcodeRegExp1 = dr["ConfigurationValue1"].ToString();
                        CommonClassForJoin.PostcodeRegExp2 = dr["ConfigurationValue2"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                    {
                        CommonClassForJoin.MailingAddressRegExp = dr["ConfigurationValue1"].ToString();
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "2") //Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.PostcodeReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.PostcodeReq = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.AddressLine1Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.AddressLine1Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.AddressLine2Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.AddressLine2Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.AddressLine3Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.AddressLine3Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine4")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.AddressLine4Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.AddressLine4Req = true;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.AddressLine5Req = true;
                        }
                        else
                        {
                            CommonClassForJoin.AddressLine5Req = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Postcode")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.PostcodeReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.PostcodeReq = false;
                        }
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "5")// max length
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1")
                    {
                        CommonClassForJoin.AddressLine1MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.AddressLine1MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                    {
                        CommonClassForJoin.AddressLine2MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.AddressLine2MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                    {
                        CommonClassForJoin.AddressLine3MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.AddressLine3MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine4")
                    {
                        CommonClassForJoin.AddressLine4MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.AddressLine4MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                    {
                        CommonClassForJoin.AddressLine5MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.AddressLine5MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                    {
                        CommonClassForJoin.PostcodeMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.PostcodeMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                }
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.MailingAddressRegExp))
            {
                CommonClassForJoin.MailingAddressRegExp = ConfigurationReader.GetStringConfigKey("MailingAddressLine");
            }
            if (OverWriteNGCConfig.Contains("ADDRESSLINE1REQ"))
            {
                CommonClassForJoin.AddressLine1Req = ConfigurationReader.GetBooleanConfigKey("AddressLine1Req");
            }
            if (OverWriteNGCConfig.Contains("ADDRESSLINE2REQ"))
            {
                CommonClassForJoin.AddressLine2Req = ConfigurationReader.GetBooleanConfigKey("AddressLine2Req");
            }
            if (OverWriteNGCConfig.Contains("ADDRESSLINE3REQ"))
            {
                CommonClassForJoin.AddressLine3Req = ConfigurationReader.GetBooleanConfigKey("AddressLine3Req");
            }
            if (OverWriteNGCConfig.Contains("ADDRESSLINE4REQ"))
            {
                CommonClassForJoin.AddressLine4Req = ConfigurationReader.GetBooleanConfigKey("AddressLine4Req");
            }
            if (OverWriteNGCConfig.Contains("ADDRESSLINE5REQ"))
            {
                CommonClassForJoin.AddressLine5Req = ConfigurationReader.GetBooleanConfigKey("AddressLine5Req");
            }
            if (OverWriteNGCConfig.Contains("POSTCODEREQ"))
            {
                CommonClassForJoin.PostcodeReq = ConfigurationReader.GetBooleanConfigKey("PostCodeReq");
            }
        }
        private static void UpdateNameConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "5")// Length of the input fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Name1")
                    {
                        CommonClassForJoin.Name1MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                        CommonClassForJoin.Name1MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        CommonClassForJoin.Name2MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                        CommonClassForJoin.Name2MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        CommonClassForJoin.Name3MinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                        CommonClassForJoin.Name3MaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "2") //Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Name1")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.Name1Required = true;
                            //Helper.SetTripleDESEncryptedCookie("Name1Required", "true");
                        }
                        else
                        {
                            CommonClassForJoin.Name1Required = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.Name2Required = true;
                        }
                        else
                        {
                            CommonClassForJoin.Name2Required = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.Name3Required = true;
                        }
                        else
                        {
                            CommonClassForJoin.Name3Required = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "TitleEnglish")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.TitleRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.TitleRequired = false;
                        }
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "10")// regural exppression
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Name1")
                    {
                        CommonClassForJoin.Name1RegExp = dr["ConfigurationValue1"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name2")
                    {
                        CommonClassForJoin.Name2RegExp = dr["ConfigurationValue1"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Name3")
                    {
                        CommonClassForJoin.Name3RegExp = dr["ConfigurationValue1"].ToString();
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "12")// Is Profanity Check Needed
                {
                    if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                    {
                        CommonClassForJoin.IsProfinityCheckNeeded = true;
                    }
                    else
                    {
                        CommonClassForJoin.IsProfinityCheckNeeded = false;
                    }
                }
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.Name1RegExp))
            {
                CommonClassForJoin.Name1RegExp = ConfigurationReader.GetStringConfigKey("FirstNameRegExp");
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.Name3RegExp))
            {
                CommonClassForJoin.Name3RegExp = ConfigurationReader.GetStringConfigKey("SurnameRegExp");
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.Name2RegExp))
            {
                CommonClassForJoin.Name2RegExp = ConfigurationReader.GetStringConfigKey("MiddleNameRegExp");
            }
            if (OverWriteNGCConfig.Contains("FIRSTNAMEREQ"))
            {
                CommonClassForJoin.Name1Required = ConfigurationReader.GetBooleanConfigKey("FirstNameReq");
            }
            if (OverWriteNGCConfig.Contains("MIDDLENAMEREQ"))
            {
                CommonClassForJoin.Name2Required = ConfigurationReader.GetBooleanConfigKey("MiddleNameReq");
            }
            if (OverWriteNGCConfig.Contains("SURNAMEREQ"))
            {
                CommonClassForJoin.Name3Required = ConfigurationReader.GetBooleanConfigKey("SurnameReq");
            }
        }
        private static void UpdateContactDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "5")// Length of the input fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        CommonClassForJoin.DaytimeNumberMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.DaytimeNumberMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        CommonClassForJoin.MobileNumberMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.MobileNumberMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {
                        CommonClassForJoin.EveningNumberMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.EveningNumberMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {
                        CommonClassForJoin.EmailMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.EmailMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "2") //Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.EmailRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.EmailRequired = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.DaytimeNumberRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.DaytimeNumberRequired = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.MobileNumberRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.MobileNumberRequired = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.EveningNumberRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.EveningNumberRequired = false;
                        }
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "10")// regural exppression
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                    {
                        CommonClassForJoin.EmailRegExp = dr["ConfigurationValue1"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "Phone")
                    {
                        CommonClassForJoin.PhoneRegExp = dr["ConfigurationValue1"].ToString();
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "9")//Phone format
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                    {
                        CommonClassForJoin.DaytimePhoneFormat = dr["ConfigurationValue1"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                    {
                        CommonClassForJoin.MobilePhoneFormat = dr["ConfigurationValue1"].ToString();
                    }

                }
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.EmailRegExp))
            {
                CommonClassForJoin.EmailRegExp = ConfigurationReader.GetStringConfigKey("EmailIdRegExp");
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.PhoneRegExp))
            {
                CommonClassForJoin.PhoneRegExp = ConfigurationReader.GetStringConfigKey("PhoneNoRegExp");
            }
            if (OverWriteNGCConfig.Contains("EMAILIDREQ"))
            {
                CommonClassForJoin.EmailRequired = ConfigurationReader.GetBooleanConfigKey("EmailIdReq");
            }
            if (OverWriteNGCConfig.Contains("MOBILENOREQ"))
            {
                CommonClassForJoin.MobileNumberRequired= ConfigurationReader.GetBooleanConfigKey("MobileNoReq");
            }
            if (OverWriteNGCConfig.Contains("EVENINTNOREQ"))
            {
                CommonClassForJoin.EveningNumberRequired = ConfigurationReader.GetBooleanConfigKey("EveningNoReq");
            }
            if (OverWriteNGCConfig.Contains("DAYTIMENOREQ"))
            {
                CommonClassForJoin.DaytimeNumberRequired = ConfigurationReader.GetBooleanConfigKey("DayTimeNoReq");
            }
        }
        private static void UpdateFD1ConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "5")// max length
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "SecId")
                    {
                        CommonClassForJoin.PassportMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.PassportMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "PrimId")
                    {
                        CommonClassForJoin.SSNMaxLength = Convert.ToInt32(dr["ConfigurationValue2"].ToString());
                        CommonClassForJoin.SSNMinLength = Convert.ToInt32(dr["ConfigurationValue1"].ToString());
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "11") //Get Join Code.
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Join Route Code")
                    {
                        CommonClassForJoin.JoinRouteCodeForKiosk = dr["ConfigurationValue2"].ToString();
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "2")// Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Language")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.LanguageReq = true;
                        }
                        else
                        {
                            CommonClassForJoin.LanguageReq = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "SecondaryId")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.PassportRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.PassportRequired = false;
                        }
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "PrimaryId")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.SSNRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.SSNRequired = false;
                        }
                    }
                }
                if (dr["ConfigurationType"].ToString().Trim() == "10") //Reg exp
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "IdFormat")
                    {
                        CommonClassForJoin.PassportRegExp = dr["ConfigurationValue1"].ToString();
                    }
                    if (dr["ConfigurationName"].ToString().Trim() == "IdFormat")
                    {
                        CommonClassForJoin.SSNRegExp = dr["ConfigurationValue1"].ToString();
                    }
                }
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.PassportRegExp))
            {
                CommonClassForJoin.PassportRegExp = ConfigurationReader.GetStringConfigKey("PassportRegExp");
            }
            if (String.IsNullOrEmpty(CommonClassForJoin.SSNRegExp))
            {
                CommonClassForJoin.SSNRegExp = ConfigurationReader.GetStringConfigKey("SSNRegExp");
            }
            if (OverWriteNGCConfig.Contains("PASSPORTREQ"))
            {
                CommonClassForJoin.PassportRequired = ConfigurationReader.GetBooleanConfigKey("PassportReq");
            }
            if (OverWriteNGCConfig.Contains("SSNRREQ"))
            {
                CommonClassForJoin.SSNRequired = ConfigurationReader.GetBooleanConfigKey("SSNReq");
            }
        }
        private static void UpdateFD2ConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "2")// Mandatory fields
                {
                    if (dr["ConfigurationName"].ToString().Trim() == "Race")
                    {
                        if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                        {
                            CommonClassForJoin.RaceRequired = true;
                        }
                        else
                        {
                            CommonClassForJoin.RaceRequired = false;
                        }
                    }
                }
            }
        }
        private static void UpdateProfinityConfigDetails(DataSet dsConfigDetails)
        {
            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "8")// Profanity check fields
                {
                    if (dr["ConfigurationValue1"].ToString().Trim() == "1")
                    {
                        CommonClassForJoin.ProfinityCheckFields = CommonClassForJoin.ProfinityCheckFields + "," + dr["ConfigurationName"].ToString().Trim();
                    }
                }
            }
        }

        public static void Net35BasicAuthentication()
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);

        }
        private static bool customXertificateValidation(Object sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static bool AccountDuplicationAndPromotionalCodeCheck(out string updateXml, out string errorXml, string promCode)
        {
            Hashtable htCustomer = new Hashtable();
            string resultXml = string.Empty;
            XmlDocument resultDoc = null;
            Int64 dotcomID = 0; //This will be always zero for kiosk users. 
            int iCount = 0;

            string ctrlID = "Final";
            string resID = "DuplicateAccountErr";
            string imgID = "ImgCrumbPrint";
            string cResID = "NGCError";
            string culture = ConfigurationReader.GetStringConfigKey("CountryCode");
            Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService.JoinLoyaltyServiceClient loyaltyServiceClient = null;
            try
            {
                Helper.Net35BasicAuthentication();
                //Assign Name values to HashTable.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
                {
                    if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "1")
                    {
                        htCustomer["TitleEnglish"] = Resources.GlobalResources.FSTitle1.ToString();
                        htCustomer["Sex"] = Resources.GlobalResources.FSGenderMale.ToString();
                    }
                    else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "2")
                    {
                        htCustomer["TitleEnglish"] = Resources.GlobalResources.FSTitle2.ToString();
                        htCustomer["Sex"] = Resources.GlobalResources.FSGenderFemale.ToString();
                    }
                    else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "3")
                    {
                        htCustomer["TitleEnglish"] = Resources.GlobalResources.FSTitle3.ToString();
                        htCustomer["Sex"] = Resources.GlobalResources.FSGenderFemale.ToString();
                    }
                    else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "4")
                    {
                        htCustomer["TitleEnglish"] = Resources.GlobalResources.FSTitle4.ToString();
                        htCustomer["Sex"] = Resources.GlobalResources.FSGenderFemale.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")))
                {
                    htCustomer["Name1"] = Helper.GetTripleDESEncryptedCookieValue("FirstName").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")))
                {
                    htCustomer["Name2"] = Helper.GetTripleDESEncryptedCookieValue("Initial").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")))
                {
                    htCustomer["Name3"] = Helper.GetTripleDESEncryptedCookieValue("LastName").ToString().Trim();
                }

                //Assigning Address values to HashTable.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")))
                {
                    htCustomer["MailingAddressLine1"] = Helper.GetTripleDESEncryptedCookieValue("MailingAddress1").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")))
                {
                    htCustomer["MailingAddressLine2"] = Helper.GetTripleDESEncryptedCookieValue("MailingAddress2").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")))
                {
                    htCustomer["MailingAddressLine3"] = Helper.GetTripleDESEncryptedCookieValue("MailingAddress3").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress4")))
                {
                    htCustomer["MailingAddressLine4"] = Helper.GetTripleDESEncryptedCookieValue("MailingAddress4").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress5")))
                {
                    htCustomer["MailingAddressLine5"] = Helper.GetTripleDESEncryptedCookieValue("MailingAddress5").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")))
                {
                    htCustomer["MailingAddressPostCode"] = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString().Trim();
                }

                //Assigning values for EmailAddress and Phone Numbers.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAddress")))
                {
                    htCustomer["EmailAddress"] = Helper.GetTripleDESEncryptedCookieValue("EmailAddress").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EveningNumber")))
                {
                    htCustomer["EveningPhoneNumber"] = Helper.GetTripleDESEncryptedCookieValue("EveningNumber").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MobileNumber")))
                {
                    htCustomer["MobilePhoneNumber"] = Helper.GetTripleDESEncryptedCookieValue("MobileNumber").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber")))
                {
                    htCustomer["DaytimePhoneNumber"] = Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber").ToString().Trim();
                }

                //Assigning values for PreferredLanguage, PassportNumber, SSN, Race.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageKey")))
                {
                    htCustomer["Language"] = Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageKey").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")))
                {
                    htCustomer["PassportNumber"] = Helper.GetTripleDESEncryptedCookieValue("PassportNumber").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")))
                {
                    htCustomer["SSN"] = Helper.GetTripleDESEncryptedCookieValue("SSN").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceID")))
                {
                    htCustomer["Race"] = Helper.GetTripleDESEncryptedCookieValue("RaceID").ToString().Trim();
                }

                //Assigning values for DOB, HouseholdAges, DietryPreferencesID.
                string date = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                string month = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                string year = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                string dob = date + "/" + month + "/" + year;
                string strDob = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim() + Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim() + Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                if (!string.IsNullOrEmpty(strDob))
                {
                    htCustomer["DateOfBirth"] = DateTime.Parse(dob);
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")))
                {
                    htCustomer["family_member_1_dob"] = Helper.GetTripleDESEncryptedCookieValue("Age1").ToString().Trim();
                    iCount = iCount + 1;
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")))
                {
                    htCustomer["family_member_2_dob"] = Helper.GetTripleDESEncryptedCookieValue("Age2").ToString().Trim();
                    iCount = iCount + 1;
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")))
                {
                    htCustomer["family_member_3_dob"] = Helper.GetTripleDESEncryptedCookieValue("Age3").ToString().Trim();
                    iCount = iCount + 1;
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")))
                {
                    htCustomer["family_member_4_dob"] = Helper.GetTripleDESEncryptedCookieValue("Age4").ToString().Trim();
                    iCount = iCount + 1;
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")))
                {
                    htCustomer["family_member_5_dob"] = Helper.GetTripleDESEncryptedCookieValue("Age5").ToString().Trim();
                    iCount = iCount + 1;
                }
                htCustomer["number_of_household_members"] = iCount + 1;


                //Assigning values for Contact Preferences.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")))
                {
                    htCustomer["WantTescoInfo"] = Helper.CheckAndResetCookieExpiration("TescoProduct").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")))
                {
                    htCustomer["WantPartnerInfo"] = Helper.CheckAndResetCookieExpiration("TescoPartnerInfo").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")))
                {
                    htCustomer["IsResearchContactable"] = Helper.CheckAndResetCookieExpiration("CustomerResearch").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail")))
                {
                    htCustomer["TescoGroupMail"] = Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail")))
                {
                    htCustomer["TescoGroupEmail"] = Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone")))
                {
                    htCustomer["TescoGroupPhone"] = Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS")))
                {
                    htCustomer["TescoGroupSMS"] = Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerMail")))
                {
                    htCustomer["PartnerMail"] = Helper.GetTripleDESEncryptedCookieValue("PartnerMail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerEmail")))
                {
                    htCustomer["PartnerEmail"] = Helper.GetTripleDESEncryptedCookieValue("PartnerEmail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerPhone")))
                {
                    htCustomer["PartnerPhone"] = Helper.GetTripleDESEncryptedCookieValue("PartnerPhone").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerSMS")))
                {
                    htCustomer["PartnerSMS"] = Helper.GetTripleDESEncryptedCookieValue("PartnerSMS").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchMail")))
                {
                    htCustomer["ResearchMail"] = Helper.GetTripleDESEncryptedCookieValue("ResearchMail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchEmail")))
                {
                    htCustomer["ResearchEmail"] = Helper.GetTripleDESEncryptedCookieValue("ResearchEmail").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchPhone")))
                {
                    htCustomer["ResearchPhone"] = Helper.GetTripleDESEncryptedCookieValue("ResearchPhone").ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchSMS")))
                {
                    htCustomer["ResearchSMS"] = Helper.GetTripleDESEncryptedCookieValue("ResearchSMS").ToString().Trim();
                }

                //Dietry preferences for UK to save in NGC DB.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName"))) //Eg: IsDiabetic, IsKoshar, IsVegeterian, IsHalal
                {
                    string validPreferences = null;
                    validPreferences = Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName").ToString().ToUpperInvariant().Trim();
                    if (validPreferences.ToString().Contains("ISDIABETIC"))
                    {
                        htCustomer["IsDiabetic"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsDiabetic"] = "N";
                    }
                    if (validPreferences.ToString().Contains("ISKOSHER"))
                    {
                        htCustomer["IsKosher"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsKosher"] = "N";
                    }
                    if (validPreferences.ToString().Contains("ISHALAL"))
                    {
                        htCustomer["IsHalal"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsHalal"] = "N";
                    }
                    if (validPreferences.ToString().Contains("ISVEGETERIAN"))
                    {
                        htCustomer["IsVegetarian"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsVegetarian"] = "N";
                    }
                    if (validPreferences.ToString().Contains("ISTEETOTAL"))
                    {
                        htCustomer["IsTeeTotal"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsTeeTotal"] = "N";
                    }
                    if (validPreferences.ToString().Contains("ISCOELIAC"))
                    {
                        htCustomer["IsCoeliac"] = "Y";
                    }
                    else
                    {
                        htCustomer["IsCoeliac"] = "N";
                    }
                }
                else
                {
                    htCustomer["IsDiabetic"] = "N";
                    htCustomer["IsKosher"] = "N";
                    htCustomer["IsHalal"] = "N";
                    htCustomer["IsVegetarian"] = "N";
                    htCustomer["IsTeeTotal"] = "N";
                    htCustomer["IsCoeliac"] = "N";
                }

                //Dietry preferences for Group Countries, to save in NGC DB.
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesID")))
                {
                    string Dietryid = Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesID");
                  
                    htCustomer["DynamicPreferences"] = Dietryid;
                }


                //Promotional Code.
                if (!string.IsNullOrEmpty(promCode)) //Eg:1,4
                {
                    htCustomer["PromotionCode"] = promCode;
                }
                try
                {
                    string strjoinroutecode = string.Empty;
                    //Calling NGC Service to insert Customer Information into NGC Database.
                    updateXml = Helper.HashTableToXML(htCustomer, "customer");
                    loyaltyServiceClient = new Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService.JoinLoyaltyServiceClient();
                    if (CommonClassForJoin.JoinRouteCodeForKiosk != null && CommonClassForJoin.JoinRouteCodeForKiosk != string.Empty)
                    {
                        strjoinroutecode = CommonClassForJoin.JoinRouteCodeForKiosk;
                    }
                    else
                    {
                        strjoinroutecode = ConfigurationReader.GetStringConfigKey("JoinRouteCode");
                    }

                    //Check for Duplicate Account
                    if (loyaltyServiceClient.AccountDuplicateCheck(out resultXml, updateXml))
                    {
                        resultDoc = new XmlDocument();
                        resultDoc.LoadXml(resultXml);
                        DataSet dsJoin = new DataSet();
                        dsJoin.ReadXml(new XmlNodeReader(resultDoc));
                        if (dsJoin.Tables["Duplicate"].Rows[0].ItemArray[1].ToString().Trim() == "0" && promCode != string.Empty)
                        {
                            errorXml = "1";
                            updateXml = Helper.HashTableToXML(htCustomer, "customer");
                            return false;

                        }
                        else
                        {
                            if (dsJoin.Tables["Duplicate"].Rows[0].ItemArray[0].ToString().Trim() == "0" && dsJoin.Tables["Duplicate"].Rows[0].ItemArray[2].ToString().Trim() == "0")
                            {
                                errorXml = "";
                                updateXml = Helper.HashTableToXML(htCustomer, "customer");
                                return true;
                            }
                            else
                            {
                                errorXml = "2";
                                updateXml = Helper.HashTableToXML(htCustomer, "customer");
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (loyaltyServiceClient != null)
                {
                    if (loyaltyServiceClient.State == CommunicationState.Faulted)
                    {
                        loyaltyServiceClient.Abort();
                    }
                    else if (loyaltyServiceClient.State != CommunicationState.Closed)
                    {
                        loyaltyServiceClient.Close();
                    }
                }
            }
            errorXml = "";
            updateXml = Helper.HashTableToXML(htCustomer, "customer");
            return true;
        }

    }

    /// <summary>
    /// CryptoUtil has static functionality to TripleDES encryption and decryption
    /// <para>using 24 byte key</para>
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>26/06/2010</para>
    /// </summary>
    public class CryptoUtil
    {
        //24 byte or 192 bit key and IV for TripleDES
        private static byte[] KEY_192 = {
                                            42,16,93,156,78,4,218,32,15,167,44,80,26,250,155,112,2,94,11,204,119,35,184,197
                                        };
        private static byte[] IV_192 = {
		                                    55,103,246,79,36,99,167,3,42,5,62,83,184,7,209,13,145,23,200,58,173,10,121,222
	                                    };
        //TRIPLE DES encryption
        public static string EncryptTripleDES(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                try
                {
                    sw.Write(value);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    ms.Flush();
                    //convert back to a string
                    return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
                }
                finally
                {
                    sw.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            else return string.Empty;
        }


        //TRIPLE DES decryption
        public static string DecryptTripleDES(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();

                //convert from string to byte array
                byte[] buffer = Convert.FromBase64String(value);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                try
                {
                    return sr.ReadToEnd();
                }
                finally
                {
                    sr.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            else return string.Empty;
        }
    }
    
}



