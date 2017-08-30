using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public static class RegexUtility
    {
        /// <summary>
        /// USed for validations, server side, validates with the help of regular expression.
        /// pass isEmptyAllowed as false if you want to check string.IsEmpty
        /// </summary>
        /// <param name="val"></param>
        /// <param name="regex"></param>
        /// <param name="isEmptyAllowed"></param>
        /// <param name="IgnoreCase"></param>
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
    }
}
