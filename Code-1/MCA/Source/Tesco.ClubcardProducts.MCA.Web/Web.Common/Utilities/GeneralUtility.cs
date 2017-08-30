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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;



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
                IConfigurationProvider _Config = new ConfigurationProvider.ConfigurationProvider();
                DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.IsPostCodeFormatRequired);
                if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1) && item.ConfigurationValue1.Equals("1"))
                {
                    result = postalcode.Trim().Replace(" ", "");
                    rText = result.Insert(result.Length - 3, " ");
                }
                else
                {
                    rText = postalcode;
                }
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

            for (int month = 1; month <= 12; month++)
            {
                months.Add(month, GetMonthName(month, culture));
            }
            return months;
        }

        private static string GetMonthName(int month, CultureInfo culture)
        {
            IConfigurationProvider _Config = new ConfigurationProvider.ConfigurationProvider();
            DbConfigurationItem item = _Config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.IsMonthsinNumeric);
            DateTime sourceDate = DateTime.Parse("01/01/2009");
            string monthName = string.Empty;
            if (item != null && !item.IsDeleted && !String.IsNullOrWhiteSpace(item.ConfigurationValue1) && item.ConfigurationValue1.Equals("1"))
            {
                 monthName = sourceDate.AddMonths(month - 1).ToString("MM", culture).Trim();
            }
            else
            {
                monthName = sourceDate.AddMonths(month - 1).ToString("MMMM", culture).Trim();
            }

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
                yearsList.Add(year, year.ToString());
            }

            return yearsList;
        }

        public static Dictionary<int, string> GetDays()
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

        public string GetCustomerDisplayName(CustomerDisplayName CustomerName, string pageName)
        {
            string displayName = string.Empty;
            List<bool> isNameDisplay = new List<bool>();
            List<bool> isNameAbbreviated = new List<bool>();
            List<int> isNameCased = new List<int>();
            try
            {
                ConfigurationProvider.IConfigurationProvider _config = new ConfigurationProvider.ConfigurationProvider();
                DBConfigurations nameDisplayList = _config.GetConfigurations(DbConfigurationTypeEnum.DisplayFunctionality);
                DBConfigurations nameAbbreviationList = _config.GetConfigurations(DbConfigurationTypeEnum.FieldAbbreviation);

                List<string> name1CaseList = _config.GetStringConfigurations(DbConfigurationTypeEnum.TextCasing, DbConfigurationItemNames.Name1Casing).Split(',').ToList();
                List<string> name2CaseList = _config.GetStringConfigurations(DbConfigurationTypeEnum.TextCasing, DbConfigurationItemNames.Name2Casing).Split(',').ToList();
                List<string> name3CaseList = _config.GetStringConfigurations(DbConfigurationTypeEnum.TextCasing, DbConfigurationItemNames.Name3Casing).Split(',').ToList();

                bool isTitleDisplay = _config.GetStringConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, DbConfigurationItemNames.ChinaHiddenFunctionalityTitle).Equals("0");

                isNameDisplay = nameDisplayList.Instance.Select(id => ((id.Value.ConfigurationValue2.ToUpper().Contains(pageName) && id.Value.ConfigurationValue1.Equals("1")) || (!id.Value.ConfigurationValue2.ToUpper().Contains(pageName) && id.Value.ConfigurationValue1.Equals("0")))).ToList();
                isNameAbbreviated = nameAbbreviationList.Instance.Select(id => ((id.Value.ConfigurationValue2.ToUpper().Contains(pageName) && id.Value.ConfigurationValue1.Equals("1")) || (!id.Value.ConfigurationValue2.ToUpper().Contains(pageName) && id.Value.ConfigurationValue1.Equals("0")))).ToList();
                string Name1Case = name1CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));
                string Name2Case = name2CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));
                string Name3Case = name3CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));


                string name1 = GetFormattedValue(CustomerName.Name1, isNameDisplay.Count > 0 ? isNameDisplay[0] : true, isNameAbbreviated.Count > 0 ? isNameAbbreviated[0] : false, Name1Case != null ? Name1Case.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last() : string.Empty);
                string name2 = GetFormattedValue(CustomerName.Name2, isNameDisplay.Count > 1 ? isNameDisplay[1] : true, isNameAbbreviated.Count > 1 ? isNameAbbreviated[1] : false, Name2Case != null ? Name2Case.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last() : string.Empty);
                string name3 = GetFormattedValue(CustomerName.Name3, isNameDisplay.Count > 2 ? isNameDisplay[2] : true, isNameAbbreviated.Count > 2 ? isNameAbbreviated[2] : false, Name3Case != null ? Name3Case.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last() : string.Empty);

                string title = isTitleDisplay ? CustomerName.TitleEnglish.Trim() : string.Empty;
                string[] data = new string[] { title, name1, name2, name3 };
                displayName = string.Join(" ", data);
                data = displayName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                displayName = string.Join(" ", data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return displayName;
        }

        public string GetFormattedValue(string nameInput, bool isVisible, bool isInitial, string TextCasing)
        {
            string output = string.Empty;
            NameCasingEnum result;

            output = !string.IsNullOrEmpty(nameInput) ? isVisible ? isInitial ? nameInput.FirstOrDefault().ToString() : nameInput : string.Empty : string.Empty;
            if (!string.IsNullOrEmpty(output))
            {
                if (TextCasing.Length.Equals(1))
                {
                    if (Enum.IsDefined(typeof(NameCasingEnum), (int)Convert.ToChar(TextCasing)))
                    {
                        result = (NameCasingEnum)Convert.ToChar(TextCasing);
                        switch (result)
                        {
                            case NameCasingEnum.Camel:
                                output = output.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture);
                                break;
                            case NameCasingEnum.Lower:
                                output = output.ToLower();
                                break;
                            case NameCasingEnum.Upper:
                                output = output.ToUpper();
                                break;
                        }
                    }
                }
            }
            return output;
        }


        #region PUBLIC METHODS

        /// This Method will Validate the virgin Memenbership
        /// </summary>
        /// <param name="inputText">input Text</param>
        /// <returns>boolean</returns>
        public static bool ValidateVirginMembershipNumber(string inputText, string validationType, string regex)
        {
            bool isValid = false;
            try
            {
                switch (validationType.ToUpper())
                {
                    case "11":
                        {
                            isValid = ValidateElevenDigitViginMembershipNumber(inputText, regex);
                            break;
                        }
                    case "10":
                        {
                            isValid = ValidateTenDigitViginMembershipNumber(inputText, regex);
                            break;
                        }
                    default:
                        isValid = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isValid;
        }

        /// <summary>
        /// This method will validate BA Membership Number 
        /// </summary>
        /// <param name="inputText">input Text</param>
        /// <returns>boolean value</returns>
        public static bool ValidateBAAviosMembership(string inputText, string regMobile)
        {
            try
            {
                ConfigurationProvider.IConfigurationProvider _config = new ConfigurationProvider.ConfigurationProvider();
                int membershipIDLen = Convert.ToInt32(_config.GetStringConfigurations(DbConfigurationTypeEnum.Length_of_the_input_fields, DbConfigurationItemNames.BAAviosMembershipID));

                if (inputText.Length > membershipIDLen)
                {
                    return false;
                }
                if (!RegexUtility.IsRegexMatch(inputText, regMobile, false, false))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool ValidateAviosMembership(string inputText, string regMobile)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);


            try
            {
                ConfigurationProvider.IConfigurationProvider _config = new ConfigurationProvider.ConfigurationProvider();
                int membershipIDLen = Convert.ToInt32(_config.GetStringConfigurations(DbConfigurationTypeEnum.Length_of_the_input_fields, DbConfigurationItemNames.AviosMembershipID));
                string membershipIdPrefix = _config.GetConfigurations(DbConfigurationTypeEnum.AppSettings, AppConfigEnum.AviosMembershipIdPrefix).ConfigurationValue1.ToString();


                if (inputText.Length > membershipIDLen)
                {
                    return false;
                }
                if (!RegexUtility.IsRegexMatch(inputText, regMobile, false, false))
                {
                    return false;
                }

                //The first fice digits are always the prefix 308147
                if (inputText.Substring(0, membershipIdPrefix.Length) != membershipIdPrefix)
                {
                    return false;
                }
                //START: Algorithm for Check digit
                char[] detailsChars = inputText.Substring(0, 15).ToCharArray();


                //Then take the odd digits first, third, fifth, seventh ,ninth till length - 1  digits and add them together without any multiplication
                //Then add the the result of multiplication value as ex (3*6 =18=> 1+8 )
                //Then add the result of all the odd digits sum 
                index = 0;
                while (index <= detailsChars.Length - 1)
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region PRIVATE METHODS

        private static bool ValidateElevenDigitViginMembershipNumber(string inputText, string regMobile)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);

            //11 digits long
            if (inputText.Length != 11)
            {
                return false;
            }

            //completely numeric
            if (!RegexUtility.IsRegexMatch(inputText, regMobile, false, false))
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
            //Then add the result of each of these even number digits multiplication�s together
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

        private static bool ValidateTenDigitViginMembershipNumber(string inputText, string regMobile)
        {
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);

            //Total length should be 10
            if (inputText.Length != 10)
            {
                return false;
            }

            //It should be number only
            if (!RegexUtility.IsRegexMatch(inputText, regMobile, false, false))
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
                    //add the result of each of these odd digit place number multiplication�s together
                    result = result + sumOfOddResult;
                }
                //add the result of each of these odd digit place number multiplication�s together
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

        public void CreateSecurityClearedCookie(bool isDotcomEnvironmentEnabled, string dotcomCustomerId, long customerId)
        {
            if (isDotcomEnvironmentEnabled)
            {
                string cookievalue = "Y_" + dotcomCustomerId + "_" + customerId;
                MCACookie.Cookie.Add(MCACookieEnum.IsSecurityCheckDone, cookievalue);
            }
            else
            {
                if (customerId != 0)
                {
                    string cookievalue = "Y_" + customerId;
                    MCACookie.Cookie.Add(MCACookieEnum.IsSecurityCheckDone, cookievalue);
                }
            }
        }

        public static string getFormattedPostcode(string strPostCode)
        {
            string maskedPostCode = string.Empty;
            if (!String.IsNullOrWhiteSpace(strPostCode) && strPostCode.Length >= 3)
            {
                StringBuilder PC = new StringBuilder(strPostCode);
                string strM = strPostCode.Substring(0, 3);
                PC.Replace(strM, "XXX");
                maskedPostCode = PC.ToString();
            }
            return maskedPostCode;
        }

        public static string removeEmptySpaceFromPostcode(string strPostCode)
        {
            string strResult = string.Empty;

            if (!String.IsNullOrWhiteSpace(strPostCode))
            {
                strResult = strPostCode.Replace(" ", "");
            }
            return strResult;
        }

        /// <summary>
        /// Method to check that provided encripted date is not older than 5 min
        /// </summary>
        /// <returns></returns>
        public static bool CheckEncriptedDate(string value)
        { 
            bool check = false;
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime date = CryptoUtility.DecryptTripleDES(value).TryParse<DateTime>();
                    check = (DateTime.Now - date).TotalSeconds <= 5*60;
                }
            }
            catch { }
            return check;
        }

        /// <summary>
        /// Method to get the Holding dates from cache
        /// and add them in cache in case not available 
        /// </summary>
        /// <param name="getDbConfigurations">Function to get the Holding dates from service</param>
        /// <param name="seconds">duration in seconds for cache expiration</param>
        /// <returns></returns>
        public static List<DbConfigurationItem> GetCachedHoldingDateConfigs(Func<DbConfiguration> getDbConfigurations, double seconds)
        {
            string appSettingKey = DbConfigurationTypeEnum.Holding_dates.ToString();
            List<DbConfigurationItem> appSettingObject = GlobalCachingProvider.Instance.GetItem(appSettingKey) as List<DbConfigurationItem>;

            if (appSettingObject == null)
            {
                DbConfiguration dbConfigs = getDbConfigurations();
                appSettingObject = dbConfigs.ConfigurationItems;
                GlobalCachingProvider.Instance.AddItem(appSettingKey, appSettingObject, DateTimeOffset.Now.AddSeconds(seconds));
            }
            return appSettingObject;
        }
    }


}


