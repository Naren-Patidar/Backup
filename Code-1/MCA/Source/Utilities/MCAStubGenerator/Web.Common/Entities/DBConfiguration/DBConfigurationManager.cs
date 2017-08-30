using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Xml.Linq;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Microsoft.Practices.ServiceLocation;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration
{
    public class DBConfigurationManager
    {
        static object _monitor = new object();
        ObjectCache _cache = MemoryCache.Default;        
        string _configurationFile = HttpContext.Current.Server.MapPath(AppConfiguration.Settings[AppConfigEnum.DBConfigurationXmlFile]);
        Dictionary<string , Dictionary<string, DbConfigurationItem>> lstConfigs = new Dictionary<string, Dictionary<string, DbConfigurationItem>>();
        LoggingService _logger;

        //private constructor
        private DBConfigurationManager()
        {
            _logger = new LoggingService();
        }

        public DBConfigurations this[DbConfigurationTypeEnum type]
        {
            get 
            {
                var configuration = _cache.Get(type.ToString());
                if (configuration == null)
                {
                    this.LoadConfigurations();
                    configuration = _cache.Get(type.ToString());                  
                }
                return (DBConfigurations)configuration;
            }
        }

        private static DBConfigurationManager _instance = new DBConfigurationManager();
        public static DBConfigurationManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public void LoadConfigurations()
        {
            lock (_monitor)
            {
                LogData _logData = new LogData();
                List<XElement> configurationItems = new List<XElement>();
                Dictionary<DbConfigurationTypeEnum, DBConfigurations> configurations = new Dictionary<DbConfigurationTypeEnum, DBConfigurations>();
                string currentKey = string.Empty;
                try
                {
                    XDocument xConfig = XDocument.Load(_configurationFile);

                    List<string> keys = (from t in xConfig.Descendants(ParameterNames.CONFIGURATION_TABLE_NAME)
                                         select t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).Value).Distinct().ToList();

                    foreach (string key in keys)
                    {
                        currentKey = key;                        
                        Dictionary<string, DbConfigurationItem> configs = (from t in xConfig.Descendants(ParameterNames.CONFIGURATION_TABLE_NAME)
                                                                           where t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).Value == key.ToString()
                                                                           select new
                                                                           {
                                                                               Name = t.Element(DbConfigurationItemEnum.ConfigurationName.ToString()).Value,
                                                                               Value = new DbConfigurationItem
                                                                               {
                                                                                   ConfigurationName = t.Element(DbConfigurationItemEnum.ConfigurationName.ToString()).Value,
                                                                                   ConfigurationValue1 = t.Element(DbConfigurationItemEnum.ConfigurationValue1.ToString()).Value,
                                                                                   ConfigurationValue2 = t.Element(DbConfigurationItemEnum.ConfigurationValue2.ToString()).Value,
                                                                                   ConfigurationType = (DbConfigurationTypeEnum)t.Element(DbConfigurationItemEnum.ConfigurationType.ToString()).Value.TryParse<Int32>(),
                                                                                   IsDeleted = t.Element(DbConfigurationItemEnum.IsDeleted.ToString()).Value.Equals("Y")
                                                                               }
                                                                           }
                                          ).ToDictionary(o => o.Name, o => o.Value);
                        if (!lstConfigs.ContainsKey(((DbConfigurationTypeEnum)Convert.ToInt32(key)).ToString()))
                        {
                            lstConfigs.Add(((DbConfigurationTypeEnum)Convert.ToInt32(key)).ToString(), configs);
                        }
                        CacheItemPolicy _policy = new CacheItemPolicy();
                        _policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { _configurationFile }));
                        _cache.Add(((DbConfigurationTypeEnum)Convert.ToInt32(key)).ToString(), new DBConfigurations(configs), _policy);
                        _logData.RecordStep(string.Format("DBConfig Data loaded for key : {0}", key));
                        
                    }
                }
                catch (Exception ex)
                {
                    throw GeneralUtility.GetCustomException(string.Format(string.Format("DBConfig error while reading key : {0}", currentKey)), ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
                }
            }
        }

        /// <summary>
        /// Method to load database settings
        /// </summary>
        /// <param name="dbConfigurations"></param>
        public void LoadDBConfigurations(DbConfiguration dbConfigurations)
        {
            try
            {
                List<string> keys = (from t in dbConfigurations.ConfigurationItems
                                     select t.ConfigurationTypeName).Distinct().ToList();
                foreach (string key in keys)
                {
                    Dictionary<string, DbConfigurationItem> configs = (from t in dbConfigurations.ConfigurationItems
                                                                       where t.ConfigurationTypeName == key.ToString()
                                                                       select new
                                                                       {
                                                                           Name = t.ConfigurationName,
                                                                           Value = new DbConfigurationItem
                                                                           {
                                                                               ConfigurationName = t.ConfigurationName,
                                                                               ConfigurationValue1 = t.ConfigurationValue1,
                                                                               ConfigurationValue2 = t.ConfigurationValue2,
                                                                               IsDeleted = t.IsDeleted
                                                                           }
                                                                       }
                                      ).ToDictionary(o => o.Name, o => o.Value);

                    if (_cache.Contains(key))
                    {
                        _cache[key] = new DBConfigurations(configs);
                    }
                    else
                    {
                        CacheItemPolicy _policy = new CacheItemPolicy();
                        _policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { _configurationFile }));
                        _cache.Add(key, new DBConfigurations(configs), _policy);
                    }
                }
                LoadSettings<ISOLanguage>(dbConfigurations.Languages);
                LoadSettings<Race>(dbConfigurations.Races);
                LoadSettings<Province>(dbConfigurations.Province);
                ISOLanguages.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public void LoadSettings<T>(List<T> settings)
        {
            try
            {
                CacheItemPolicy _policy = new CacheItemPolicy();
                _policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { _configurationFile }));
                _cache.Add(typeof(T).Name, settings, _policy);
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        public List<DbConfigurationItem> FindConfigs(List<string> names)
        {
            List<DbConfigurationItem> configs = new List<DbConfigurationItem>();
            lstConfigs.ToList().ForEach(c => c.Value.ToList().ForEach(ic => configs.Add(ic.Value)));
            configs.RemoveAll(c => !names.Contains(c.ConfigurationName));
            return configs;
        }

        public List<DbConfigurationItem> FindConfigs(string name)
        {
            List<DbConfigurationItem> configs = new List<DbConfigurationItem>();
            lstConfigs.ToList().ForEach(c => c.Value.ToList().ForEach(ic => configs.Add(ic.Value)));
            configs.RemoveAll(c => !name.Equals(c.ConfigurationName));
            return configs;
        }
    }

    public class DBConfigurations
    {
        public DBConfigurations(Dictionary<string, DbConfigurationItem> instance)
        {
            this._instance = instance;
        }

        public DBConfigurations()
        {

        }

        private Dictionary<string, DbConfigurationItem> _instance = new Dictionary<string, DbConfigurationItem>();
        public Dictionary<string, DbConfigurationItem> Instance
        {
            get
            {
                return _instance;
            }
        }

        public DbConfigurationItem this[string key]
        {
            get 
            {
                DbConfigurationItem item = new DbConfigurationItem();
                if (_instance.Keys.Contains(key))
                {
                    item = _instance[key];
                }
                return item;
            }
            set { _instance[key] = value; }
        }

        public DbConfigurationItem GetConfigurationItem(string key)
        {
            DbConfigurationItem config = new DbConfigurationItem();
            try
            {
                if (this.Instance.Keys.Contains(key))
                {
                    config = this.Instance[key];
                }
            }
            catch { }
            return config;
        }

        public DbConfigurationItem GetConfigurationByValue1(string value1)
        {
            DbConfigurationItem config = new DbConfigurationItem();
            try
            {
                if (this.Instance.ToList().FindAll(c => c.Value.ConfigurationValue1.ToUpper() == value1.ToUpper()).Count > 0)
                {
                    config = this.Instance.ToList().Find(c => c.Value.ConfigurationValue1.ToUpper() == value1.ToUpper()).Value;
                }
            }
            catch { }
            return config;
        }
    }
}
