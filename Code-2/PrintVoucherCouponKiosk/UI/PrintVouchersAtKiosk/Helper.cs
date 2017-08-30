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
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public class Helper
    {
        public static string GetCulture()
        {
            string culture = String.Empty;
            string CountryCode = String.Empty;
            try
            {
                CountryCode = Convert.ToString(ConfigurationManager.AppSettings["CountryCode"]);
                if (!string.IsNullOrEmpty(CountryCode))
                {
                    culture = CountryCode;
                }
                else
                {
                    culture = "en-GB";
                }
            }
            catch (Exception ex)
            {
                // Logger.Write(ex, "General", 1, 3500, System.Diagnostics.TraceEventType.Error, "Coupon Setup UI layer - HelperClass GetCulture");
            }
            return culture;
        }
        public static string GetParameters(string clientIP)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    return clubcardRewardClient.PrintVoucherKioskEntryPage(clientIP);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        /// <summary>
        /// Creates the booking.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static BigExchange.BookingPrintVoucher CreateBooking(string status)
        {
            BigExchange.BookingPrintVoucher bookingPrintVoucher = new BigExchange.BookingPrintVoucher();

            //bookingPrintVoucher.StatusID = status;
            bookingPrintVoucher.UnusedVouchers = new BigExchange.UnusedVoucherCollection();
            bookingPrintVoucher.UnusedCoupons = new BigExchange.UnusedCouponCollection();
            bookingPrintVoucher.Clubcard = string.Empty;
            bookingPrintVoucher.TransactionID = 0;
            return bookingPrintVoucher;
        }


        public static BigExchange.UnusedVoucherCollection GetVoucherDetails(string clubcard)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    //todo 20120102 -- Remove below two lines and uncomment third line
                    // BigExchange.UnusedVoucherCollection vc = new BigExchange.UnusedVoucherCollection();
                    //return vc;
                    return clubcardRewardClient.GetUnusedVoucherDetails(clubcard);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BigExchange.UnusedCouponCollection GetCouponDetails(string clubcard, out int errorStatusCode)
        {
            try
            {
                BigExchange.UnusedCoupon objUnusedcoupon = null;
                errorStatusCode = 0;

                using (CouponEnquiryService.ClubcardCouponEnquiryServiceClient objCouponEnquiryClient = new CouponEnquiryService.ClubcardCouponEnquiryServiceClient())                
                {
                    BigExchange.BookingPrintVoucher bookingPrintVoucher = new BigExchange.BookingPrintVoucher();                    
                    List<CouponEnquiryService.AvailableCouponRequest> objlstAvailableCouponRequest = new List<CouponEnquiryService.AvailableCouponRequest>();
                    List<CouponEnquiryService.AvailableCoupons> objlstCouponResponse = new List<CouponEnquiryService.AvailableCoupons>();
                    CouponEnquiryService.AvailableCouponRequest objAvailableCouponRequest = new CouponEnquiryService.AvailableCouponRequest();
                    BigExchange.UnusedCouponCollection objUnusedCouponDetails = new BigExchange.UnusedCouponCollection();
                    objAvailableCouponRequest.ClubCardNumber = Convert.ToInt64(clubcard);
                    //objAvailableCouponRequest.ClubCardNumber =null;
                    //objAvailableCouponRequest.HouseHoldId = 643906;
                    objAvailableCouponRequest.HouseHoldId = null;
                    objAvailableCouponRequest.ImageRequired = false;
                    objlstAvailableCouponRequest.Add(objAvailableCouponRequest);
                    objlstCouponResponse = objCouponEnquiryClient.GetAvailableCoupons(objlstAvailableCouponRequest);

                    errorStatusCode = Convert.ToInt32(objlstCouponResponse[0].ErrorStatusCode);

                    foreach (CouponEnquiryService.AvailableCoupons objCoupon in objlstCouponResponse)
                    {
                        //objUnusedcoupon.ActiveCoupons = objCoupon.ActiveCoupon;
                        if (objCoupon.ActiveCoupon != null && objCoupon.ActiveCoupon!=0)
                        {
                            foreach (var objCouponInfo in objCoupon.CouponList)
                            {
                                objUnusedcoupon = new BigExchange.UnusedCoupon();
                                objUnusedcoupon.CouponDescription = objCouponInfo.CouponDescription;
                                objUnusedcoupon.SmartBarcode = objCouponInfo.SmartBarcode;
                                objUnusedcoupon.SmartAlphaCode = objCouponInfo.SmartAlphaNumeric;
                                objUnusedcoupon.CouponExpiryDate = objCouponInfo.RedemptionEndDate;
                                objUnusedCouponDetails.Add(objUnusedcoupon);
                            }
                        }

                    }
                    //for (int i = 1; i <= 2; i++)
                    //{
                    //    BigExchange.UnusedCoupon objUnusedcoupon1 = new BigExchange.UnusedCoupon();
                    //    objUnusedcoupon1.ActiveCoupons = i;
                    //    objUnusedcoupon1.CouponDescription = "This is Coupon description" + System.Convert.ToString( i);
                    //    objUnusedcoupon1.CouponExpiryDate = DateTime.Now;// objCouponInfo.RedemptionEndDate;       
                    //    objUnusedcoupon1.SmartBarcode = "4192939894454470300000";//objCouponInfo.SmartBarcode;
                    //    objUnusedcoupon1.SmartAlphaCode = "2588E3817ED4";// objCouponInfo.SmartAlphaNumeric; 
                    //    objUnusedCouponDetails.Add(objUnusedcoupon1);
                    //}
                    return objUnusedCouponDetails;
                  
                }                    
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        /// <summary>
        /// Gets the voucher total.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static decimal GetVoucherTotal(BigExchange.UnusedVoucherCollection unusedVoucherCollection)
        {
            decimal value = 0;
            try
            {
                foreach (BigExchange.UnusedVoucher voucher in unusedVoucherCollection)
                {

                    value += voucher.VoucherValue;
                }
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static void UpdateTranDetailsActiveVoucher(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsActiveVoucher);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added as part of International Kiosk Development
        //public static void UpdateTranDetailsActiveCoupon(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        //{
        //    try
        //    {
        //        using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
        //        {
        //            clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsActiveCoupon);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static void SaveVouchers(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.SaveUnusedVouchers);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveCoupons(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.SaveUnusedCoupons);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateTranDetailsStatus(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateTranDetailsPrintDate(BigExchange.BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                using (BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient())
                {
                    clubcardRewardClient.Logging(bookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsPrintDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool HasValid13DigitClubdcard(string clubcard)
        {
            String oldCardNumber = string.Empty;
            int CheckDigit = 0;
            int tailValue = 0;
            bool lbRetVal = false;
            List<String> oddNumberArray = new List<String>();
            List<String> evenNumberArray = new List<String>();
            try
            {
                oldCardNumber = clubcard.Remove(clubcard.Length - 1);
                CheckDigit = Convert.ToInt32(clubcard.Substring(clubcard.Length - 1));
                for (int splitIndex = 0; splitIndex < oldCardNumber.Length; splitIndex++)
                {
                    if (splitIndex % 2 == 0 || splitIndex == 0)
                    {
                        evenNumberArray.Add(oldCardNumber.Substring(splitIndex,
                                1));
                    }
                    else
                    {
                        oddNumberArray.Add(oldCardNumber.Substring(splitIndex,
                                1));
                    }
                }
                int totalNumber = calculateOddAndEvenValue(oddNumberArray,
                        evenNumberArray) % 10;
                if (totalNumber == 0)
                {
                    tailValue = 0;
                }
                else
                {
                    tailValue = 10 - totalNumber;
                }
                if (CheckDigit == tailValue)
                {
                    lbRetVal= true;
                }
                else
                {
                    lbRetVal= false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRetVal;
        }

        public static int calculateOddAndEvenValue(List<String> oddNumber,List<String> evenNumber)
        {
            int sumOddNumber = 0;
            int sumEvenNumber = 0;
            try
            {
                for (int oddIndex = 0; oddIndex < oddNumber.Count; oddIndex++)
                {
                    sumOddNumber += int.Parse((String)oddNumber[oddIndex]);
                }
                for (int evenIndex = 0; evenIndex < evenNumber.Count; evenIndex++)
                {
                    sumEvenNumber += int.Parse((String)evenNumber
                            [evenIndex]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sumOddNumber * 3 + sumEvenNumber;
        }

        public static bool HasValidClubcard(string clubcard)
        {
            return HasValidCheckDigit(clubcard);
        }





        /// <summary>
        /// Determines whether the Clubcard number passed in has a valid check digit
        /// </summary>
        /// <param name="clubcardNumber">The Clubcard number.</param>
        /// <returns>
        /// 	<c>true</c> if the Clubcard has a valid check digit; otherwise, <c>false</c>.
        /// </returns>
        private static Boolean HasValidCheckDigit(String clubcardNumber)
        {
            // Code lifted from VerifyValidCard.cls (Original Matthew code)
            // was based on variants so added object and added necessary conversions between strings and integers
            //
            // This function verifies the card check digit is correct
            int Loop_Count;
            int Times_2_Units;
            int Times_1_Units;
            double Times_2_Tens;
            int Running_Total;
            int Current_Check_Digit;
            int Calculated_Check_Digit;
            Boolean bFlag;
            Running_Total = 0;
            bFlag = true;
            int clubcard;

            if (!int.TryParse(clubcardNumber, out clubcard))
            {
                // Remove Spaces
                for (Loop_Count = clubcardNumber.Length; Loop_Count >= 1; Loop_Count--)
                {
                    if (Mid(clubcardNumber, Loop_Count, 1) == " ")
                    {
                        switch (Loop_Count)
                        {
                            case 1:
                                clubcardNumber = Right(clubcardNumber, clubcardNumber.Length - 1);
                                break;
                            case 2:
                                clubcardNumber = Left(clubcardNumber, Loop_Count - 1);
                                break;
                            default:
                                clubcardNumber = string.Format("{0}{1}", Left(clubcardNumber, Loop_Count - 1), Right(clubcardNumber, clubcardNumber.Length - Loop_Count));
                                break;
                        }
                    }
                }
                //If the string length is too short, go no further
                if (clubcardNumber.Length > 15)
                {
                    //Check For Numerics
                    for (Loop_Count = clubcardNumber.Length; Loop_Count >= 1; Loop_Count--)
                    {
                        if (!int.TryParse(Mid(clubcardNumber, Loop_Count, 1), out clubcard))
                        {
                            bFlag = false;

                        }
                    }
                }
            }


            //if (bFlag & (clubcardNumber.Length == 16 ))
            if (bFlag & (clubcardNumber.Length == 16 || clubcardNumber.Length == 18))
            {
                //Calculate Check Digit
                Current_Check_Digit = int.Parse(clubcardNumber.Substring(clubcardNumber.Length - 1, 1));
                for (Loop_Count = clubcardNumber.Length - 1; Loop_Count >= 1; Loop_Count--)
                {

                    Times_2_Units = int.Parse(Mid(clubcardNumber, Loop_Count, 1));
                    Times_2_Units = (int)(Times_2_Units) * 2;
                    if (Times_2_Units.ToString().Length > 1)
                    {
                        Times_2_Tens = (int)(Times_2_Units * Math.Pow(10, -1));
                        Times_2_Units = Times_2_Units - ((int)Times_2_Tens * 10);
                    }
                    else
                    {
                        Times_2_Tens = 0;
                    }
                    if (Loop_Count > 1)
                    {
                        Times_1_Units = int.Parse(Mid(clubcardNumber, Loop_Count - 1, 1));
                    }
                    else
                    {
                        Times_1_Units = 0;
                    }
                    Running_Total = Running_Total + Times_2_Units + Times_1_Units + (int)Times_2_Tens;

                    // VB.Net code uses step -2 to decrement this twice
                    Loop_Count--;
                }
                Calculated_Check_Digit = 10 - int.Parse(Right(Running_Total.ToString(), 1));
                if (Running_Total != 0)
                {
                    if (Calculated_Check_Digit > 9)
                    {
                        Calculated_Check_Digit = 0;
                    }
                    if (Current_Check_Digit == Calculated_Check_Digit)
                    {
                        return true;
                    }
                }
            }
            return false;
            //App.LogEvent ("bCheckDigit : " & Err.Number & ", " & Err.Description & " sClubCardNumber=>" & sClubCardNumber & "<"), 1
        }

        private static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }
        private static string Right(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }

        private static string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex - 1, length);
            return result;
        }

        private static string Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex - 1);
            return result;
        }

        /// <summary>
        /// Validate for numeric voucher number
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public static String ValidateClubcard(string clubcard)
        {
            try
            {
                long lngResult;
                int Loop_Count;
                clubcard = clubcard.Trim();
                if (clubcard.Length > 0)
                {
                    // Remove Spaces
                    for (Loop_Count = clubcard.Length; Loop_Count >= 1; Loop_Count--)
                    {
                        if (Mid(clubcard, Loop_Count, 1) == " ")
                        {
                            switch (Loop_Count)
                            {
                                case 1:
                                    clubcard = Right(clubcard, clubcard.Length - 1);
                                    break;
                                case 2:
                                    clubcard = Left(clubcard, Loop_Count - 1);
                                    break;
                                default:
                                    clubcard = string.Format("{0}{1}", Left(clubcard, Loop_Count - 1), Right(clubcard, clubcard.Length - Loop_Count));
                                    break;
                            }
                        }
                    }
                    if (Int64.TryParse(clubcard, out lngResult) && (clubcard.Length == 13 || clubcard.Length == 16 || clubcard.Length == 18))
                    {
                        return clubcard;
                    }
                    else
                    {
                        //Display Error -- Invalid Clubcard number, please enter again
                        return "";
                    }
                }
                else
                {
                    //Display Error -- Enter Clubcard number
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }


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
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("- Select Month -"));
            DateTime temp = DateTime.Parse("01/01/2009");
            for (int month = 0; month <= 11; month++)
            {
                ddl.Items.Add(new ListItem(temp.AddMonths(month).ToString("MMMM").Trim(), (month + 1).ToString()));
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

        /// <summary>
        /// Calculates the voucher value and returns as string, also returns the points residual after calculation
        /// </summary>
        /// <param name="totalPoints">points to convert to vouchers</param>
        /// <param name="residual">residual will have a value while converting points in vouchers</param>
        /// <returns>voucher value string</returns>
        //public static string VoucherDisplay(int totalPoints, out int residual)
        //{
        //    if (totalPoints < BusinessConstants.REWARDEE_LIMIT)
        //    {
        //        residual = totalPoints;
        //        return "0.0";
        //    }
        //    int remd = totalPoints % 50;
        //    residual = remd;
        //    int correctedPoints = totalPoints - remd;
        //    float dispVal = ((float)(correctedPoints)) / 100;
        //    string strDispVal = dispVal.ToString();
        //    if (strDispVal.Contains("."))
        //    {
        //        string temp = strDispVal.Substring(strDispVal.Length - 2, 1);
        //        if (temp != "0")
        //            strDispVal += "0";
        //    }
        //    else
        //    {
        //        strDispVal += ".00";
        //    }
        //    return strDispVal;
        //}

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
                        Convert.ToInt32(ConfigurationManager.AppSettings["TimerCCOCookieExpiration"]));//ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
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
                        Convert.ToInt32(ConfigurationManager.AppSettings["TimerCCOCookieExpiration"]));//ConfigurationSettings.AppSettings["TimerCCOCookieExpiration"]));
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
        public static string GetNumberFromStr(string str)
        {
            //string value = string.Empty;
            string[] numbers = Regex.Split(str, @"\D+");
            //value = numbers[0];
            //string value1 = Array.Find(numbers, element => element.GetTypeCode() == TypeCode.Int32 );
            string value1 = Array.Find(numbers, element => element.Length > 0);
            if (value1 == null) value1 = "";
            return value1;
            //return string.Join(null, System.Text.RegularExpressions.Regex.Split(str, "[^\\d]"));
        }
     

        public static Boolean IsNumeric(string str)
        {
            int result;
            return int.TryParse(str, out result);
        }

        /// <summary>
        /// Returns culture specific format of passed date sting
        /// </summary>
        /// <param name="date">string</param>
        public static string GetCultureSpecificDate(string date)
        {
            try
            {
                return DateTime.Parse(date, new CultureInfo(ApplicationConstants.CurrentCulture)).ToShortDateString();
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Helper GetCultureSpecificDate()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                throw ex;
            }
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



