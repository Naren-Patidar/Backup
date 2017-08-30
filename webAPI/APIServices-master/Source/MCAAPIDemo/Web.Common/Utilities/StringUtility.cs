using System;
using System.Globalization;
using System.Configuration;
using Microsoft.Security.Application;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public static class StringUtility
    {
        /// <summary>
        ///     Converts the passed string to Title Case, considers UK culture info
        /// </summary>
        /// <param name="mText">Text to convert to lower case</param>
        /// <returns>Converted text</returns>
        public static string ToTitleCase(string mText)
        {
            var rText = "";
            try
            {
                //Removed hardcoding of culture as fix for defect MKTG00010267
                var TextInfo =
                    new CultureInfo(ConfigurationManager.AppSettings["CultureDefault"].ToString(), false).TextInfo;
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
        ///     Converts the passed string to Title Case, considers UK culture info
        /// </summary>
        /// <param name="mText">Text to convert to lower case</param>
        /// <returns>Converted text</returns>
        public static string ToTitleCase(this string text, string locale)
        {
            var rText = "";
            try
            {
                var TextInfo = new CultureInfo(locale, false).TextInfo;
                text = TextInfo.ToLower(text);
                rText = TextInfo.ToTitleCase(text);
            }
            catch
            {
                rText = text;
            }
            return rText;
        }

        public static string ToTitleCase(this string strName, CultureInfo culture)
        {
            var strTitleCase = strName;
            if (!string.IsNullOrEmpty(strName))
            {
                strTitleCase = culture.TextInfo.ToTitleCase(strName.ToLower());
            }
            else
            {
                strTitleCase = string.Empty;
            }
            return strTitleCase;
        }

        public static DateTime? ToNullableDateTime(this string dateTimeText)
        {
            if (string.IsNullOrEmpty(dateTimeText))
                return null;

            return DateTime.Parse(dateTimeText);
        }

        public static string HTMLEncode(string text)
        {
            return Sanitizer.GetSafeHtmlFragment(text);
        }
    }
}