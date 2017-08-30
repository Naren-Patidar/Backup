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
        string htmlDataFile = string.Empty;
        string domain = string.Empty;
        bool dbConfigurationSwitch = false;
        string dbConfigurationFile = string.Empty;
        string webConfigurationFile = string.Empty;
        string messageDataDirectory = string.Empty;
        string htmlDataDirectory = string.Empty;
        string localResourceFile = string.Empty;
        string cultureFile = string.Empty;
        string mcaUrl = string.Empty;
        string activationUrl = string.Empty;
        string cscUrl = string.Empty;
        string joinUrl = string.Empty;
        string christmasSaverUrl = string.Empty;
        string couponsUrl = string.Empty;
        string pointsUrl = string.Empty;
        //string pointsSummaryUrl =string.Empty;
        string personalDetailsUrl =string.Empty;
        string myContactPreferencesUrl =string.Empty;
        string optionsBenefitsUrl =string.Empty;
        string viewMyCardsUrl =string.Empty;
        string orderAReplacementUrl =string.Empty;
        string boostUrl =string.Empty;
        //string pointsDetailsUrl =string.Empty;
        string vouchersrUrl =string.Empty;
        string myLatestStatementUrl =string.Empty;
        
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
        public string HtmlDataDirectory
        {
            get
            {
                return htmlDataDirectory;
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

        public HTMLFiles HTMLFiles { get; set; }

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

        public string ChristmasSaverUrl
        {
            get { return christmasSaverUrl; }
            set { christmasSaverUrl = value; }
        }

        public string CouponsUrl
        {
            get { return couponsUrl; }
            set { couponsUrl = value; }
        }

        public string PointsUrl
        {
            get { return pointsUrl; }
            set { pointsUrl= value; }
        }
        //public string PointsSummaryUrl
        //{
        //    get { return pointsSummaryUrl; }
        //    set { pointsSummaryUrl = value; }
        //}
        public string PersonalDetailsUrl
        {
            get { return personalDetailsUrl; }
            set { personalDetailsUrl= value; }
        }
        public string MyContactPreferencesUrl
        {
            get { return myContactPreferencesUrl; }
            set { myContactPreferencesUrl= value; }
        }
        public string OptionsBenefitsUrl
        {
            get { return optionsBenefitsUrl; }
            set { optionsBenefitsUrl = value; }
        }
        public string ViewMyCardsUrl
        {
            get { return viewMyCardsUrl; }
            set { viewMyCardsUrl = value; }
        }
        public string OrderAReplacementUrl
        {
            get { return orderAReplacementUrl; }
            set { orderAReplacementUrl = value; }
        }
        public string BoostUrl
        {
            get { return boostUrl; }
            set { boostUrl = value; }
        }
        //public string PointsDetailsUrl
        //{
        //    get { return pointsDetailsUrl; }
        //    set { pointsDetailsUrl= value; }
        //}
        public string VouchersUrl
        {
            get { return vouchersrUrl; }
            set { vouchersrUrl= value; }
        }
        public string MyLatestStatementUrl
        {
            get { return myLatestStatementUrl; }
            set { myLatestStatementUrl= value; }
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
            if (ConfigurationManager.AppSettings.AllKeys.Contains("HTML"))
            {
                htmlDataFile = string.Format(ConfigurationManager.AppSettings["HTML"], CountrySetting.country);
            }
            else
            {
                throw new Exception("HTML file path not specified.");
            }
            domain = ConfigurationManager.AppSettings.AllKeys.Contains("Domain") ?  ConfigurationManager.AppSettings["Domain"] : "DBT";
            dbConfigurationSwitch = ConfigurationManager.AppSettings.AllKeys.Contains("DBConfigurationSwitch") ? ConfigurationManager.AppSettings["DBConfigurationSwitch"].ToUpper().Equals("TRUE") : false;
            dbConfigurationFile = ConfigurationManager.AppSettings.AllKeys.Contains("DBConfigurationFile") ? ConfigurationManager.AppSettings["DBConfigurationFile"] : string.Empty;            
            dbConfigurationFile = string.Format(dbConfigurationFile, CountrySetting.country);
            webConfigurationFile = ConfigurationManager.AppSettings.AllKeys.Contains("WebConfigurationFile") ? ConfigurationManager.AppSettings["WebConfigurationFile"] : string.Empty;
            webConfigurationFile = string.Format(webConfigurationFile, CountrySetting.country);
            messageDataDirectory = Path.GetDirectoryName(messageDataFile);
            htmlDataDirectory = Path.GetDirectoryName(htmlDataFile);
            cultureFile = ConfigurationManager.AppSettings.AllKeys.Contains("CultureFile") ? ConfigurationManager.AppSettings["CultureFile"] : string.Empty;
            ResourceFiles = new ResourceFiles();
            HTMLFiles = new HTMLFiles();
            string landingMCAPageKey = string.Format("{0}-{1}-{2}", "MCAURL", CountrySetting.country, domain);
            mcaUrl = ConfigurationManager.AppSettings.AllKeys.Contains(landingMCAPageKey) ? ConfigurationManager.AppSettings[landingMCAPageKey].ToString() : string.Empty;
            activationUrl = GetApplicationURL(Enums.ApplicationPages.Activation);
            joinUrl = GetApplicationURL(Enums.ApplicationPages.Join);
            couponsUrl = GetApplicationURL(Enums.ApplicationPages.Coupons);
            pointsUrl = GetApplicationURL(Enums.ApplicationPages.Points);
           // pointsSummaryUrl = GetApplicationURL(Enums.ApplicationPages.PointsSummary);
            //pointsDetailsUrl= GetApplicationURL(Enums.ApplicationPages.PointsDetails);
            boostUrl= GetApplicationURL(Enums.ApplicationPages.Boost);
            christmasSaverUrl = GetApplicationURL(Enums.ApplicationPages.ChristmasSaver);
            myContactPreferencesUrl= GetApplicationURL(Enums.ApplicationPages.MyContactPreferences);
            myLatestStatementUrl = GetApplicationURL(Enums.ApplicationPages.MyLatestStatement);
            optionsBenefitsUrl= GetApplicationURL(Enums.ApplicationPages.OptionsBenefits);
            orderAReplacementUrl= GetApplicationURL(Enums.ApplicationPages.OrderAReplacement);
            personalDetailsUrl= GetApplicationURL(Enums.ApplicationPages.PersonalDetails);
            viewMyCardsUrl= GetApplicationURL(Enums.ApplicationPages.ViewMyCards);
            vouchersrUrl= GetApplicationURL(Enums.ApplicationPages.Vouchers);

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
                    pageName = "Activation/home";
                    break;
                case Enums.ApplicationPages.Join:
                    pageName = "Join/home";
                    break;
                case Enums.ApplicationPages.ChristmasSaver:
                    pageName = "ChristmasSavers/home";
                    break;
                case Enums.ApplicationPages.Coupons:
                    pageName = "Coupons/Home";
                    break;
                case Enums.ApplicationPages.Points:
                    pageName = "Points/Home";
                    break;
                //case Enums.ApplicationPages.PointsSummary:
                //    pageName = "Points/PointsSummaryView?offerid=1";
                //    break;
                case Enums.ApplicationPages.Boost:
                    pageName = "BoostsAtTesco/Home";
                    break;
                case Enums.ApplicationPages.MyContactPreferences:
                    pageName = "AccountManagement/ContactPreferences";
                    break;
                case Enums.ApplicationPages.MyLatestStatement:
                    pageName = "LatestStatement/Home";
                    break;
                case Enums.ApplicationPages.OptionsBenefits:
                    pageName = "AccountManagement/VoucherSchemes";
                    break;
                case Enums.ApplicationPages.OrderAReplacement:
                    pageName = "AccountManagement/OrderANewCard";
                    break;
                case Enums.ApplicationPages.PersonalDetails:
                    pageName = "AccountManagement/PersonalDetails";
                    break;
                //case Enums.ApplicationPages.PointsDetails:
                //    pageName = "Points/PointsDetail?offerid=211&period=CURRENT";
                //    break;
                case Enums.ApplicationPages.ViewMyCards:
                    pageName = "AccountManagement/ClubcardsOnAccount";
                    break;
                case Enums.ApplicationPages.Vouchers:
                    pageName = "Vouchers/Home";
                    break;
            }
            url = string.Format(MCAUrl.EndsWith("/") ? "{0}{1}" : "{0}/{1}", MCAUrl, pageName);            
            return url;
        }

        #endregion
    }
}
