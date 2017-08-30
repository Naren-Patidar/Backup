using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;

namespace Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider
{
    public interface IConfigurationProvider
    {
        bool GetBoolAppSetting(AppConfigEnum key);
        string GetStringAppSetting(AppConfigEnum key);
        string GetStringConfigurations(DbConfigurationTypeEnum type, AppConfigEnum key);
        string GetStringConfigurations(DbConfigurationTypeEnum type, string key);
        DbConfigurationItem GetConfigurations(DbConfigurationTypeEnum type, AppConfigEnum key);
        DbConfigurationItem GetConfigurations(DbConfigurationTypeEnum type, string key);
        DBConfigurations GetConfigurations(DbConfigurationTypeEnum type);
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public bool GetBoolAppSetting(AppConfigEnum key)
        {
            return GetStringConfigurations(DbConfigurationTypeEnum.AppSettings, key.ToString()).TryParse<bool>(); 
        }

        public string GetStringAppSetting(AppConfigEnum key)
        {
            return GetStringConfigurations(DbConfigurationTypeEnum.AppSettings, key.ToString());
        }

        /// <summary>
        /// Method to retrieve the DB configuration item
        /// </summary>
        /// <param name="type">DbConfigurationTypeEnum</param>
        /// <param name="key">AppConfigEnum</param>
        /// <returns>DbConfigurationItem</returns>
        public string GetStringConfigurations(DbConfigurationTypeEnum type, AppConfigEnum key)
        {
            return GetStringConfigurations(type, key.ToString());
        }

        /// <summary>
        /// Method to retrieve the DB configuration item
        /// </summary>
        /// <param name="type">DbConfigurationTypeEnum</param>
        /// <param name="key">string</param>
        /// <returns>DbConfigurationItem</returns>
        public string GetStringConfigurations(DbConfigurationTypeEnum type, string key)
        {
            DbConfigurationItem item = DBConfigurationManager.Instance[type][key];
            return item.ConfigurationValue1;
        }

        /// <summary>
        /// Method to retrieve the DB configuration item
        /// </summary>
        /// <param name="type">DbConfigurationTypeEnum</param>
        /// <param name="key">AppConfigEnum</param>
        /// <returns>DbConfigurationItem</returns>
        public DbConfigurationItem GetConfigurations(DbConfigurationTypeEnum type, AppConfigEnum key)
        {
            return GetConfigurations(type, key.ToString());
        }

        /// <summary>
        /// Method to retrieve the DB configuration item
        /// </summary>
        /// <param name="type">DbConfigurationTypeEnum</param>
        /// <param name="key">string</param>
        /// <returns>DbConfigurationItem</returns>
        public DbConfigurationItem GetConfigurations(DbConfigurationTypeEnum type, string key)
        {
            DbConfigurationItem item = DBConfigurationManager.Instance[type][key];
            return item;
        }

        public DBConfigurations GetConfigurations(DbConfigurationTypeEnum type)
        {
            DBConfigurations item = DBConfigurationManager.Instance[type];
            return item;
        }
    }
}