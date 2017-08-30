using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

namespace Tesco.Framework.UITesting.Entities
{
    public class AppConfiguration
    {
        #region Private Fields
        
        bool runAllBrowsers = false;
        Browser defaultBrowser = Browser.IE;
        string testDataFile = string.Empty;
        string messageDataFile = string.Empty;
        string domain = string.Empty;
        bool dbConfigurationSwitch = false;
        string dbConfigurationFile = string.Empty;
        string webConfigurationFile = string.Empty;
        string messageDataDirectory = string.Empty;
        string localResourceFile = string.Empty;
        string cultureFile = string.Empty;
        string mcaUrl = string.Empty;
        string activationUrl = string.Empty;
        string cscUrl = string.Empty;
        string joinUrl = string.Empty;
                          
        #endregion


        #region Public Properties

        /// <summary>
        /// Property to get or set the flag for running test cases in all browsers parallaly.
        /// </summary>
        public bool RunAllBrowsers
        {
            get { return runAllBrowsers; }
            set { runAllBrowsers = value; }
        }

        /// <summary>
        /// Property to get or set the default browser.
        /// </summary>
        public Browser DefaultBrowser
        {
            get { return defaultBrowser; }
            set { defaultBrowser = value; }
        }

        /// <summary>
        /// Property to get or set the value of test data file name.
        /// </summary>
        public string TestDataFile
        {
            get { return testDataFile; }
            set { testDataFile = value; }
        }

        public string MessageDataFile
        {
            get { return messageDataFile; }
            set { messageDataFile = value; }
        }

        public string MessageDataDirectory
        {
            get 
            {
                return messageDataDirectory; 
            }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public bool DbConfigurationSwitch
        {
            get { return dbConfigurationSwitch; }
            set { dbConfigurationSwitch = value; }
        }

        public string DbConfigurationFile
        {
            get { return dbConfigurationFile; }
            set { dbConfigurationFile = value; }
        }

        public string WebConfigurationFile
        {
            get { return webConfigurationFile; }
            set { webConfigurationFile = value; }
        }

        public string LocalResourceFile
        {
            get { return localResourceFile; }
            set { localResourceFile = value; }
        }

        public string CultureFile
        {
            get { return cultureFile; }
            set { cultureFile = value; }
        }

        public ResourceFiles ResourceFiles { get; set; }

        /// <summary>
        /// Property to get or set the application landing page
        /// </summary>
        public string MCAUrl
        {
            get { return mcaUrl; }
            set { mcaUrl = value; }
        }

        public string ActivationUrl
        {
            get { return activationUrl; }
            set { activationUrl = value; }
        }

        public string CscUrl
        {
            get { return cscUrl; }
            set { cscUrl = value; }
        }

        public string JoinUrl
        {
            get { return joinUrl; }
            set { joinUrl = value; }
        }
          
        #endregion

        #region Constructor

        public AppConfiguration()
        {
            runAllBrowsers = ConfigurationManager.AppSettings.AllKeys.Contains("RunAllBrowsers") ? bool.Parse(ConfigurationManager.AppSettings["RunAllBrowsers"]) : false;
            defaultBrowser = ConfigurationManager.AppSettings.AllKeys.Contains("DefaultBrowser") ? (Browser)Enum.Parse(typeof(Browser), ConfigurationManager.AppSettings["DefaultBrowser"]) : Browser.IE;
            if (ConfigurationManager.AppSettings.AllKeys.Contains("DataXML"))
            {
                testDataFile = string.Format(ConfigurationManager.AppSettings["DataXML"], CountrySetting.country);
            }
            else
            {
                throw new Exception("Test data file not specified.");
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("MessageXML"))
            {
                messageDataFile = string.Format(ConfigurationManager.AppSettings["MessageXML"], CountrySetting.country);
            }
            else
            {
                throw new Exception("Message XML file not specified.");
            }
            domain = ConfigurationManager.AppSettings.AllKeys.Contains("Domain") ?  ConfigurationManager.AppSettings["Domain"] : "DBT";
            dbConfigurationSwitch = ConfigurationManager.AppSettings.AllKeys.Contains("DBConfigurationSwitch") ? ConfigurationManager.AppSettings["DBConfigurationSwitch"].ToUpper().Equals("TRUE") : false;
            dbConfigurationFile = ConfigurationManager.AppSettings.AllKeys.Contains("DBConfigurationFile") ? ConfigurationManager.AppSettings["DBConfigurationFile"] : string.Empty;            
            dbConfigurationFile = string.Format(dbConfigurationFile, CountrySetting.country);
            webConfigurationFile = ConfigurationManager.AppSettings.AllKeys.Contains("WebConfigurationFile") ? ConfigurationManager.AppSettings["WebConfigurationFile"] : string.Empty;
            webConfigurationFile = string.Format(webConfigurationFile, CountrySetting.country);
            messageDataDirectory = Path.GetDirectoryName(messageDataFile);
            cultureFile = ConfigurationManager.AppSettings.AllKeys.Contains("CultureFile") ? ConfigurationManager.AppSettings["CultureFile"] : string.Empty;
            ResourceFiles = new ResourceFiles();
            string landingMCAPageKey = string.Format("{0}-{1}-{2}", "MCAURL", CountrySetting.country, domain);
            mcaUrl = ConfigurationManager.AppSettings.AllKeys.Contains(landingMCAPageKey) ? ConfigurationManager.AppSettings[landingMCAPageKey].ToString() : string.Empty;
            activationUrl = GetApplicationURL(Enums.ApplicationPages.Activation);
            joinUrl = GetApplicationURL(Enums.ApplicationPages.Join);
            string landingCSCPageKey = string.Format("{0}-{1}", "CSCURL", CountrySetting.country);
            cscUrl = ConfigurationManager.AppSettings.AllKeys.Contains(landingCSCPageKey) ? ConfigurationManager.AppSettings[landingCSCPageKey].ToString() : string.Empty;
        }


        private string GetApplicationURL(Enums.ApplicationPages page)
        {
            string url = string.Empty;
            string pageName = string.Empty;
            switch (page)
            {
                case Enums.ApplicationPages.Activation:
                    pageName = "Activation/home.aspx";
                    break;
                case Enums.ApplicationPages.Join:
                    pageName = "Join/home.aspx";
                    break;
            }
            url = string.Format("{0}{1}", MCAUrl, pageName);
            return url;
        }

        #endregion
    }
}
