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
using CCODundeeApplication.NGCReportingService;
using System.ServiceModel;

namespace CCODundeeApplication
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public class Helper
    {
        protected static NGCReportingServiceClient objReportingService = null;

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
        /// Get month dropdown box populated with 12 records
        /// </summary>
        /// <param name="ddl">Dropdown list reference to be populated for Month</param>
        public static void GetMonthDdl(DropDownList ddl)
        {
            System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["Culture"].ToString());
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("- Select Month -"));
            DateTime temp = DateTime.Parse("01/01/2009");
            for (int month = 0; month <= 11; month++)
            {
                ddl.Items.Add(new ListItem(temp.AddMonths(month).ToString("MMMM", enGBCulture).Trim(), (month + 1).ToString()));
            }
        }

        /// <summary>
        /// Get year dropdown box populated with records
        /// </summary>
        /// <param name="ddl">Dropdown list reference to be populated for Month</param>
        public static void GetYearDdl(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Year"));
            DateTime temp = DateTime.Now;
            for (int year = temp.Year - 99; year <= temp.Year - 18; year++)
            {
                ddl.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }
        //CCMCA-441
        public static void GetHouseholdYearsList(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Year"));
            DateTime temp = DateTime.Now;
            for (int year = temp.Year - 99; year <= temp.Year - 1; year++)
            {
                ddl.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }
        /// <summary>
        /// Get year dropdown box populated with records
        /// </summary>
        /// <param name="ddl">Dropdown list reference to be populated for Month</param>
        public static void GetYearDdlForBT(DropDownList ddl, short ConfigValue)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Year"));
            DateTime temp = DateTime.Now;
            for (int year = temp.Year - ConfigValue; year <= temp.Year + 1; year++)
            {
                ddl.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

        /// <summary>
        /// Get year dropdown box populated with records
        /// </summary>
        /// <param name="ddl">Dropdown list reference to be populated for Month</param>
        public static void GetYearDdlReport(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Year"));
            DateTime temp = DateTime.Now;
            for (int year = temp.Year - 2; year <= temp.Year; year++)
            {
                ddl.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }

   
        /// <summary>
        /// Converts the passed string to Title Case, considers UK culture info
        /// </summary>
        /// <param name="mText">Text to convert to lower case</param>
        /// <returns>Converted text</returns>
        public static string ToTitleCase(string mText)
        {
            string rText = "";
            try
            {
                System.Globalization.TextInfo TextInfo = new System.Globalization.CultureInfo("en-GB", false).TextInfo;
                mText = TextInfo.ToLower(mText);
                rText = TextInfo.ToTitleCase(mText);
            }
            catch
            {
                rText = mText;
            }
            return rText;
        }

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

        public static string GetColMonthName(DateTime colEndDate, string colAddDays)
        {

            double VarAdddays = Convert.ToDouble(colAddDays);
            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM yyyy") :
                                          colEndDate.AddDays(VarAdddays).ToString("MMMM yyyy");
        }

        /// <summary>
        /// Calculates the voucher value and returns as string, also returns the points residual after calculation
        /// </summary>
        /// <param name="totalPoints">points to convert to vouchers</param>
        /// <param name="residual">residual will have a value while converting points in vouchers</param>
        /// <returns>voucher value string</returns>
        public static string VoucherDisplay(int totalPoints, out int residual)
        {
            if (totalPoints < BusinessConstants.REWARDEE_LIMIT)
            {
                residual = totalPoints;
                return "0.0";
            }
            int remd = totalPoints % 50;
            residual = remd;
            int correctedPoints = totalPoints - remd;
            float dispVal = ((float)(correctedPoints)) / 100;
            string strDispVal = dispVal.ToString();
            if (strDispVal.Contains("."))
            {
                string temp = strDispVal.Substring(strDispVal.Length - 2, 1);
                if (temp != "0")
                    strDispVal += "0";
            }
            else
            {
                strDispVal += ".00";
            }
            return strDispVal;
        }

        /// <summary>
        /// Returns the Collection period month name in the format : Month yyyy
        /// <para>Logic involved here is: if EndDate is less than 15 then current month name</para>
        /// <para>If EndDate is greater than 15 then next month name</para>
        ///<example>If EndDate is 14/03/2010 then this function will return March 2010</example>
        ///<example>If EndDate is 28/03/2010 then this function will return April 2010</example>
        /// </summary>
        /// <param name="colEndDate">Collection period end date</param>
        /// <returns>Statement name in Month Year format ex. August 2010</returns>
        public static string GetColMonthName(DateTime colEndDate)
        {
            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM yyyy") :
                                          colEndDate.AddMonths(1).ToString("MMMM yyyy");
        }

        /// <summary>
        /// This method returns year from the date.
        /// </summary>
        /// <param name="colEndDate"></param>
        /// <returns></returns>
        public static string GetColYear(DateTime colEndDate)
        {
            return colEndDate.Year.ToString();
        }

        /// <summary>
        /// Returns the Collection period month name in the format : Month yyyy
        /// <para>Logic involved here is: if EndDate is less than 15 then current month name</para>
        /// <para>If EndDate is greater than 15 then next month name</para>
        /// <para>Overloaded with only onlyMonthFlg</para>
        ///<example>If EndDate is 14/03/2010 then this function will return March 2010</example>
        ///<example>If EndDate is 28/03/2010 then this function will return April 2010</example>
        /// </summary>
        /// <param name="colEndDate">Collection period end date</param>
        /// <param name="onlyMonthFlg">if true only Month is returned else Month Year format</param>
        /// <returns>Statement name in Month format ex. August</returns>
        public static string GetColMonthName(DateTime colEndDate, bool onlyMonthFlg)
        {
            string yearFormat = " yyyy";
            if (onlyMonthFlg)
                yearFormat = string.Empty;
            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM" + yearFormat) :
                                      colEndDate.AddMonths(1).ToString("MMMM" + yearFormat);
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

        public static DataTable PopulateWeekData()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                objReportingService = new NGCReportingServiceClient();
                ds = objReportingService.PopulateWeekData();
                dt = ds.Tables[0];
            }
            catch
            {
            }
            finally
            {
                if (objReportingService != null)
                {
                    if (objReportingService.State == CommunicationState.Faulted)
                    {
                        objReportingService.Abort();
                    }
                    else if (objReportingService.State != CommunicationState.Closed)
                    {
                        objReportingService.Close();
                    }
                }
            }
            return dt;
        }

        public static DataTable PopulatePeriodData()
        {

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                objReportingService = new NGCReportingServiceClient();
                ds = objReportingService.PopulatePeriodData();
                dt = ds.Tables[0];
            }
            catch { }
            finally
            {
                if (objReportingService != null)
                {
                    if (objReportingService.State == CommunicationState.Faulted)
                    {
                        objReportingService.Abort();
                    }
                    else if (objReportingService.State != CommunicationState.Closed)
                    {
                        objReportingService.Close();
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Mask the String expect LeftunMasked length and RightUnMasked Length range.
        /// E.g if Ip=Testing, Left=1,Right=2,mask=*; Op->T****ng
        /// </summary>
        /// <param name="inputString">Input string need to be mask</param>
        /// <param name="leftUnMaskLength">Length which required to unmask from left side of ip string</param>
        /// <param name="rightUnMaskLength">Length which required to unmask from right side of ip string</param>
        /// <param name="mask">Mask character</param>
        /// <returns></returns>
        public static string MaskString(string inputString, int leftUnMaskLength, int rightUnMaskLength, char mask)
        {
            if ((leftUnMaskLength + rightUnMaskLength) > inputString.Length)
                return inputString;

            return inputString.Substring(0, leftUnMaskLength) +
                new string(mask, inputString.Length - (leftUnMaskLength + rightUnMaskLength)) +
                inputString.Substring(inputString.Length - rightUnMaskLength);
        }

        #region PUBLIC METHODS

        /// This Method will Validate the virgin Memenbership
        /// </summary>
        /// <param name="inputText">input Text</param>
        /// <returns>boolean</returns>
        public static bool ValidateVirginMembershipNumber(string inputText, string validationType, string regex, int[] VALength)
        {
            bool isValid = false;
            switch (validationType.ToUpper())
            {
                case "11":
                    {
                        isValid = ValidateElevenDigitViginMembershipNumber(inputText, regex, VALength);
                        break;
                    }
                case "10":
                    {
                        isValid = ValidateTenDigitViginMembershipNumber(inputText, regex, VALength);
                        break;
                    }
                default:
                    isValid = false;
                    break;
            }

            return isValid;
        }

        /// <summary>
        /// This method will validate BA Membership Number 
        /// </summary>
        /// <param name="inputText">input Text</param>
        /// <returns>boolean value</returns>
        public static bool ValidateBAAviosMembership(string inputText, string regMobile, int[] BALength)
        {
            if (inputText.Length < BALength[0] || inputText.Length > BALength[1])
            {
                return false;
            }
            if (!IsRegexMatch(inputText, regMobile, false, false))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateAviosMembership(string inputText, string regMobile, int[] AviosLength)
        {
           // if (inputText.Length > 16)
            if (inputText.Length < AviosLength[0] || inputText.Length > AviosLength[0])
            {
                return false;
            }
            if (!IsRegexMatch(inputText, regMobile, false, false))
            {
                return false;
            }


            return true;
        }
        /// <summary>
        /// Validate the Avios Member ship number : Luhn Algorithm 
        /// For Example: Membership Number - 3081 4701 1746 017K
        /// 7*2+1+0*2+6+4*2+7+1*2+1+0*2+7+4*2+1+8*2+0+3*2
        ///(14)1(0)6(8)7(2)1(0)7(8)1(16)0(6)
        ///(1 + 4)1(0)6(8)7(2)1(0)7(8)1(1 + 6)0(6)
        ///(5)1(0)6(8)7(2)1(0)7(8)1(7)0(6) = 59
        ///60 - 59 = 1 = check digit
        ///Membership Number: 3081 4701 1746 0171
        /// </summary>
        /// <param name="inputText">Avios Membership Number</param>
        /// <param name="regMobile">Regular expression as numeric</param>
        /// <param name="AviosLength">To check min and max length of provided input</param>
        /// <returns></returns>
        public static bool ValidateAviosMembershipNumber(string inputText, string regMobile, int[] AviosLength)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);

            //11 digits long
            if (inputText.Length < AviosLength[0] || inputText.Length > AviosLength[1])
            {
                return false;
            }
            //completely numeric
            if (!IsRegexMatch(inputText, regMobile, false, false))
            {
                return false;
            }
            //The first fice digits are always the prefix 308147
            if (inputText.Substring(0, 6) != "308147")
            {
                return false;
            }
            //START: Algorithm for Check digit
            char[] detailsChars = inputText.Substring(0, 15).ToCharArray();


            //Then take the odd digits first, third, fifth, seventh ,ninth till length - 1  digits and add them together without any multiplication
            //Then add the the result of multiplication value as ex (3*6 =18=> 1+8 )
            //Then add the result of all the odd digits sum 
            index = 0;
            while (index <= detailsChars.Length -1)
            {
                result = result + (int.Parse(detailsChars[index].ToString()) * 2).ToString().ToCharArray().Sum(c => c - '0');
                index += 2;
            }

            //Take the even digits second, fourth, sixth, eighth and tenth till length -1 digits of the account number and multiply each number by two
            //Then add the result of each of these even number digits multiplication’s together
            index = 1;
            while (index <= detailsChars.Length - 1)
            {
                result = result + int.Parse(detailsChars[index].ToString());
                index += 2;
            }

            //result = two totals from the even digits and the odd digits together.
            roundedTotal = Math.Ceiling(result / 10) * 10;

            //Round this final total up to the nearest ten, and then subtract the calculated total from the rounded total to give you the correct check digit
            double checksum = roundedTotal - result;

            if (double.Parse(inputText[15].ToString()) != checksum)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region PRIVATE METHODS

       

        private static bool ValidateElevenDigitViginMembershipNumber(string inputText, string regMobile, int[] VALength)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);

            //11 digits long
            if (inputText.Length != VALength[1])
            {
                return false;
            }

            //completely numeric
            if (!IsRegexMatch(inputText, regMobile, false, false))
            {
                return false;
            }


            //The first two digits are always the prefix 00
            if (inputText.Substring(0, 2) != "00")
            {
                return false;
            }

            //All 0 is not valid number
            if (inputText == "00000000000")
            {
                return false;
            }

            //START: Algorithm for Check digit
            char[] detailsChars = inputText.Substring(0, 10).ToCharArray();

            //Take the second, fourth, sixth, eighth and tenth digits of the account number and multiply each number by two
            //Then add the result of each of these even number digits multiplication’s together
            index = 1;
            while (index <= detailsChars.Length - 1)
            {
                result = result + (double.Parse(detailsChars[index].ToString()) * 2);
                index += 2;
            }

            //Then take the remaining first, third, fifth, seventh and ninth digits and add them together without any multiplication
            index = 0;
            while (index <= detailsChars.Length - 1)
            {
                result = result + double.Parse(detailsChars[index].ToString());
                index += 2;
            }

            //result = two totals from the even digits and the odd digits together.
            roundedTotal = Math.Ceiling(result / 10) * 10;

            //Round this final total up to the nearest ten, and then subtract the calculated total from the rounded total to give you the correct check digit
            double checksum = roundedTotal - result;

            if (double.Parse(inputText[10].ToString()) != checksum)
            {
                return false;
            }

            return true;
        }

        private static bool ValidateTenDigitViginMembershipNumber(string inputText, string regMobile, int[] VALength)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);

            //Total length should be 10
            if (inputText.Length != VALength[0])
            {
                return false;
            }

            //It should be number only
            if (!IsRegexMatch(inputText, regMobile, false, false))
            {
                return false;
            }

            //All 0 is not valid number
            if (inputText == "0000000000")
            {
                return false;
            }

            //START: Algorithm for check digit. The last index digit            
            char[] detailsChars = inputText.Substring(0, 9).ToCharArray();
            //Take the first, third, fifth, seventh and ninth digits of the account number and multiply each number by two.
            index = 0;
            while (index <= detailsChars.Length - 1)
            {
                double oddResult = (double.Parse(detailsChars[index].ToString()) * 2);
                //If the product of the multiplication is a 2 digit number (i.e. greater than 9) add the 2 digits together to get one digit.
                if (oddResult > 9)
                {
                    char[] oddResultStr = oddResult.ToString().ToCharArray();
                    double sumOfOddResult = default(double);
                    for (int i = 0; i < oddResultStr.Length; i++)
                    {
                        sumOfOddResult = sumOfOddResult + double.Parse(oddResultStr[i].ToString());
                    }
                    //add the result of each of these odd digit place number multiplication’s together
                    result = result + sumOfOddResult;
                }
                //add the result of each of these odd digit place number multiplication’s together
                else
                {
                    result = result + oddResult;
                }
                index += 2;
            }

            //Then take the remaining second, forth, sixth, and eight digits and add them together without any multiplication. 
            //Add all four even digit place numbers together for one sum
            index = 1;
            while (index <= detailsChars.Length - 1)
            {
                result = result + double.Parse(detailsChars[index].ToString());
                index += 2;
            }

            //Now result=sum of all even and odd numbers
            //Round this final result to the nearest ten
            roundedTotal = Math.Ceiling(result / 10) * 10;
            //Subtract the calculated total from the rounded total
            double checksum = roundedTotal - result;

            //Output after subtraction should be same as check digit i.e. 10th index
            if (double.Parse(inputText[9].ToString()) != checksum)
            {
                return false;
            }
            //END: Algorithm for check digit. The last index digit

            return true;
        }

        #endregion

         

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
