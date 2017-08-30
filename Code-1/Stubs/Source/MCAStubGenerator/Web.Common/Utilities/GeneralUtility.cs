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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Web.Routing;


namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    /// <summary>
    /// Helper Class
    /// Purpose: Utility methods implementation for Presentation layer
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date Created 18/11/2009</para>
    /// </summary>
    public class GeneralUtility
    {
        #region HashTable to XML

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
        #endregion

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
        public static string GetColMonthName(DateTime colEndDate, bool onlyMonthFlg, CultureInfo culture)
        {
            string yearFormat = " yyyy";
            if (onlyMonthFlg)
                yearFormat = string.Empty;

            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM" + yearFormat, culture) :
                                      colEndDate.AddMonths(1).ToString("MMMM" + yearFormat, culture);

        }


        public static string GetMonthYear(DateTime colEndDate, int addDays)
        {

            //double VarAdddays = Convert.ToDouble(colAddDays);

            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM yyyy") :

                                          colEndDate.AddDays(addDays).ToString("MMMM yyyy");

        }


        ////.added by susmita
        #region GetGroup-MonthName
        /// <summary>
        /// Converts the English months to group months name
        /// </summary>
        /// <returns></returns>
        public static string GetGroupMonthName(string strMonths)
        {
            try
            {
                //string strMonths1 = "September 2013";
                string strYear = string.Empty;


                string[] words = strMonths.Split(' ');

                strMonths = words[0].ToString();
                if (words.Length > 1)
                {
                    strYear = words[1].ToString();
                }

                //  strMonths =  "'%'"+strMonths.ToUpper();
                string strGroupmonths = string.Empty;

                //Read the months file from XML
                string filePath = HttpContext.Current.Server.MapPath("~/XMLs/xmlGroupMonths.xml");
                DataTable dt = new DataTable("Months");
                //Add Columns in datatable
                //Column names must match XML File nodes
                dt.Columns.Add("Slno", typeof(System.String));
                dt.Columns.Add("MonthsEnGB", typeof(System.String));
                dt.Columns.Add("MonthsGroup", typeof(System.String));
                //Read XML File And Display Data in GridView
                dt.ReadXml(filePath);
                //Filter the rows here
                DataRow[] row = dt.Select("MonthsEnGB='" + strMonths + "'");
                if (row.Length > 0)
                {
                    strGroupmonths = row[0]["MonthsGroup"].ToString();
                }
                //DataView dv = new DataView();
                //dv = dt.DefaultView;
                //dv.RowFilter = "MonthsEnGB='" + strMonths + "'";
                //strGroupmonths = dv[0]["MonthsGroup"].ToString();
                if (words.Length > 1)
                {
                    return strGroupmonths + " " + strYear;
                }
                else
                {
                    return strGroupmonths;
                }
            }

            catch (Exception exp)
            {

                throw exp;
            }
        }

        #endregion

        /// <summary>
        /// formats the clubcard number supplied and return it back
        /// masks the clubcard number as per the PCIDSS requirements
        /// </summary>
        /// <param name="cardNumber">clubcard number to be masked</param>
        /// <param name="clubcardNumber"></param>
        /// <param name="isMaskReq">if clubcard number is required to be masked</param>
        /// <param name="maskChar">masking character</param>
        /// <returns>formatted clubcard number</returns>
        public static string MasknFormatClubcard(string clubcardNumber, bool isMaskReq, char maskChar)
        {
            string middleGroup = string.Empty;
            string maskedClubCard = string.Empty;

            try
            {
                if (clubcardNumber.Trim().Length > 15)
                {
                    middleGroup = clubcardNumber.Substring(6, 8);
                    StringBuilder MG = new StringBuilder(clubcardNumber);
                    MG.Replace(middleGroup, " XXXX XXXX ");

                    //return the formatted card number
                    maskedClubCard = MG.ToString();
                }
                //if clubcard number found lesser than 16 digits the unformatted clubcard number is returned
                else
                {
                    maskedClubCard = clubcardNumber;
                }

                return maskedClubCard;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
        //Added By Madhusmita his method for Currency value round of CR#2 check and slovag 
        public static string GetDecimalTrimmedCurrencyVal(string currencyVal)
        {
            //currencyVal = "100,00.10";
            string formattedVal = currencyVal;
            formattedVal = (currencyVal.Contains(',') ? currencyVal.TrimEnd('0').TrimEnd(',') : currencyVal.Contains('.') ? currencyVal.TrimEnd('0').TrimEnd('.') : formattedVal);
            formattedVal = formattedVal.Contains('.') ? currencyVal : formattedVal.Contains(',') ? currencyVal : formattedVal;

            return formattedVal;
        }

        /// <summary>
        /// Calculates the age for a date of birth provided
        /// </summary>
        /// <param name="dob">Date of birth for a person</param>
        /// <returns>age in years for Date of birth supplied</returns>
        public static int GetAge(DateTime dob)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dob.Year;
            if (now < dob.AddYears(age)) age--;
            return age;
        }
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = new XmlTextWriter(sw);
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(tw, o);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                sw.Close();
                tw.Close();
            }
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
        public static Int16 GetInt16Value(DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return 0;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return 0;

                if (dr[columnName].ToString().Length == 0)
                    return 0;

                return Convert.ToInt16(dr[columnName].ToString());
            }
        }

        public static decimal GetDecimalValue(DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return 0;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return 0;

                if (dr[columnName].ToString().Length == 0)
                    return 0;

                return Convert.ToDecimal(dr[columnName].ToString(), CultureInfo.InvariantCulture);
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
                    //NGCTrace.NGCTrace.TraceError("Error:GeneralUtility - GetDateTimeValue :" + "DateTime conversion error: Value " + dr[columnName].ToString()+ ex.ToString());
                    return (DateTime?)null;
                }

            }
        }

        /// <summary>
        /// FormatPostalCode
        /// </summary>
        public static string FormatPostalCode(string postalcode)
        {
            //Merging 3E branch to 3F branch :CCMCA:8 Post code Space story code
            string rText;
            string result;
            try
            {
                result = postalcode.Trim().Replace(" ", "");

                rText = result.Insert(result.Length - 3, " ");
            }
            catch
            {
                rText = postalcode;
            }
            return rText;
        }

        public static string FormateClubcardNumber(string strClubcard)
        {
            StringBuilder sbFormatedClubcard = new StringBuilder();
            int iChunkSize = 4;
            try
            {
                if (strClubcard.Length > 0)
                {
                    var strTarget = strClubcard;
                    while (strTarget.Length > 0)
                    {
                        if (strTarget.Length < iChunkSize)
                        {
                            sbFormatedClubcard.Append(strTarget);
                            break;
                        }
                        else
                        {
                            sbFormatedClubcard.Append(strTarget.Substring(0, iChunkSize));
                            sbFormatedClubcard.Append(" ");
                            strTarget = strTarget.Substring(iChunkSize);
                        }
                    }
                    return sbFormatedClubcard.ToString().Trim();
                }
                else
                {
                    return sbFormatedClubcard.ToString().Trim();
                }
            }
            catch
            {

            }
            return sbFormatedClubcard.ToString().Trim();
        }

        public static int GetCollectionPeriodMonth()
        {
            int numDays = -1; // string.Empty;

            try
            {
                var dbConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.ColMonthName];                            

                if (dbConfiguration[DbConfigurationItemNames.CollectionPeriodMonth].ConfigurationValue1 != null)
                {
                    numDays = Convert.ToInt32(dbConfiguration[DbConfigurationItemNames.CollectionPeriodMonth].ConfigurationValue1);
                }
                if (numDays == -1)
                {
                    numDays = 30;
                }
                return numDays;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
            }
        }

        public static string GetColYear(DateTime colEndDate)
        {
            return colEndDate.Year.ToString();
        }

        public static Dictionary<int, string> GetMonthsList()
        {
            Dictionary<int, string> months = new Dictionary<int, string>();
            string pCulture = System.Globalization.CultureInfo.CurrentCulture.Name;
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(pCulture);
            //CultureInfo culture = new CultureInfo(Locale.UK);

            for (int month = 1; month <= 12; month++)
            {
                months.Add(month,GetMonthName(month, culture));
            }
            return months;
        }

        private static string GetMonthName(int month, CultureInfo culture)
        {
            DateTime sourceDate = DateTime.Parse("01/01/2009");
            string monthName = sourceDate.AddMonths(month - 1).ToString("MMMM", culture).Trim();

            return monthName;
        }

        public static Dictionary<int, string> GetYearsList()
        {
            Dictionary<int, string> yearsList = new Dictionary<int, string>();

            DateTime currentYear = DateTime.Now;

            for (int year = currentYear.Year - 99; year <= currentYear.Year - 18; year++)
            {
                yearsList.Add(year, year.ToString());
            }
            return yearsList;
        }

        public static Dictionary<int, string> GetHouseholdYearsList()
        {
            Dictionary<int, string> yearsList = new Dictionary<int, string>();
            DateTime currentYear = DateTime.Now;
            for (int year = currentYear.Year - 99; year <= currentYear.Year - 1; year++)
            {
                yearsList.Add(year,year.ToString());
            }

            return yearsList;
        }

        public static Dictionary<int,string> GetDays()
        {
            Dictionary<int, string> daysList = new Dictionary<int, string>();
            for (int day = 1; day <= 31; day++)
            {
                daysList.Add(day, day.ToString());
            }
            return daysList;
        }

        public static Exception GetCustomException(string message, Exception ex, Dictionary<string, object> data)
        {
            Exception objException = new Exception(message, ex);

            foreach (string k in data.Keys)
            {
                if (!objException.Data.Contains(k))
                {
                    objException.Data.Add(k, data[k]);
                }
            }
            
            return objException;
        }

        public static bool IsExternalLink(string url)
        {
            return url.StartsWith("http");
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
}



