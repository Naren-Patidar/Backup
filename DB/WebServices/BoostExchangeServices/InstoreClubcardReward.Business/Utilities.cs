using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;

namespace InstoreClubcardReward.Business
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class Utilities
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        private const string IRISH_CULTURE = "en-IE";

        /// <summary>
        /// 
        /// </summary>
        private const string UK_CULTURE = "en-GB";

        #endregion

        #region Private Methods

        /// <summary>
        /// Changes the currency.
        /// </summary>
        /// <param name="country">The country.</param>
        /// <remarks></remarks>
        public static void ChangeCurrency(string country)
        {
            if (country == "ROI" && Thread.CurrentThread.CurrentCulture.Name != IRISH_CULTURE)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(IRISH_CULTURE);
            }
            else if (country == "UK" && Thread.CurrentThread.CurrentCulture.Name != UK_CULTURE)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(UK_CULTURE);
            }

        }

        /// <summary>
        /// Displays the currency.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DisplayCurrency(double value, int decimalPlaces)
        {
            ChangeCurrency(ConfigurationManager.AppSettings["Country"]);

            string currencyFormat = string.Format("c{0}", decimalPlaces);

            return value.ToString(currencyFormat);
        }

        /// <summary>
        /// Displays the currency.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DisplayCurrency(int value, int decimalPlaces)
        {
            ChangeCurrency(ConfigurationManager.AppSettings["Country"]);

            string currencyFormat = string.Format("c{0}", decimalPlaces);

            return (value / 100).ToString(currencyFormat);

        }

        #endregion

    }
}
