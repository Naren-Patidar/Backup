using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public static class CookieUtility
    {
        public static string GetTripleDESEncryptedCookieValue(string key)
        {
            //encrypt key only - encoding done in GetCookieValue
            key = CryptoUtility.EncryptTripleDES(key);

            //get value 
            string value = null;
            value = GetCookieValue(key);
            //decrypt value

            value = CryptoUtility.DecryptTripleDES(value);
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
            key = CryptoUtility.EncryptTripleDES(key);
            string value = CryptoUtility.EncryptTripleDES(string.Empty);
            SetCookie(key, value, DateTime.Now.AddDays(-1d));
        }

        // added by Robin Apoto.
        //Date: 09 Sept 2010
        //Reason: To reset cookie expiration time for active user.
        public static string CheckAndResetCookieExpiration(string key)
        {
            //encrypt key only - encoding done in GetCookieValue
            key = CryptoUtility.EncryptTripleDES(key);
            //get value 
            string value = null;
            value = GetCookieValue(key);
            //encrypt value
            value = CryptoUtility.EncryptTripleDES(value);

            if (!string.IsNullOrEmpty(value))
            {
                //DateTime cookieExpiration = DateTime.Now.AddMinutes(
                //        Convert.ToInt32(ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
                value = CryptoUtility.DecryptTripleDES(value);
                //SetCookie(key, value, cookieExpiration);
                SetCookie(key, value);
            }

            //decrypt value
            value = CryptoUtility.DecryptTripleDES(value);
            return value;
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
            key = CryptoUtility.EncryptTripleDES(key);
            value = CryptoUtility.EncryptTripleDES(value);
            //DateTime cookieExpiration = DateTime.Now.AddMinutes(
            //            Convert.ToInt32(ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
            //SetCookie(key, value, cookieExpiration);
            SetCookie(key, value);
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
            key = CryptoUtility.EncryptTripleDES(key);
            value = CryptoUtility.EncryptTripleDES(value);

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
            //cookie.HttpOnly = true;
            //cookie.Secure = true;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}
