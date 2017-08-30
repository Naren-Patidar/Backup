using System;
using System.Configuration;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationReader
    {
        public static string GetStringConfigKeyToUpper(string key)
        {
            try
            {
               return ConfigurationManager.AppSettings[key].ToString().ToUpperInvariant();
            }
            catch
            {
                return null;
            }
        }

        public static string GetStringConfigKeyToLower(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString().ToLowerInvariant();
            }
            catch
            {
                return null;
            }
        }

        public static string GetStringConfigKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static string GetStringConfigKeyTrimmed(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString(System.Globalization.CultureInfo.InvariantCulture).Trim();
            }
            catch
            {
                return null;
            }
        }

        public static Int32? GetInt32ConfigKey(string key)
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[key].ToString());
            }
            catch
            {
                return null;
            }            
        }

        public static Int64? GetInt64ConfigKey(string key)
        {
            try
            {
                return Convert.ToInt64(ConfigurationManager.AppSettings[key].ToString());
            }
            catch
            {
                return null;
            }            
        }

        public static bool? GetBoolConfigKey(string key)
        {
            try
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings[key].ToString());
            }
            catch
            {
                return null;
            }            
        }

        public static bool GetBooleanConfigKey(string key)
        {
            try
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings[key].ToString());
            }
            catch
            {
                return false;
            }
        }

    }
}