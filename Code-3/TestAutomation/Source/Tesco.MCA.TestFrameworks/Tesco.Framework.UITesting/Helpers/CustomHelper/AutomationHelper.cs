using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.IO;
using Tesco.Framework.UITesting.Entities;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections.ObjectModel;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Constants;
using Enums = Tesco.Framework.UITesting.Enums;
using System.Xml.Linq;
using OpenQA.Selenium.Remote;
using System.Xml;
using System.Diagnostics;


namespace Tesco.Framework.UITesting.Helpers.CustomHelper
{
    public class AutomationHelper
    {
        private static List<CategoryControls> _categoryControls;
        private static List<Message> _messages;
        private static List<TestData> _testingData;
        private IWebDriver _webDriver = null;

        #region PROPERTIES

        public static List<Message> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
        public IWebDriver WebDriver
        {
            get { return _webDriver; }
            set { _webDriver = value; }
        }
        public static List<TestData> TestingData
        {
            get { return _testingData; }
            set { _testingData = value; }
        }
        
        #endregion

        #region PUBLIC METHODS

        public static T PopulateListOfXmlData<T>(string xmlPath)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(xmlPath);

                object xmlData = deserializer.Deserialize(reader);

                var deserializedData = (T)xmlData;
                reader.Close();

                return deserializedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetCategoryControls(string xmlPath)
        {
            var deserializedData = PopulateListOfXmlData<Controls>(xmlPath);
            if (deserializedData != null)
            {
                _categoryControls = deserializedData.lstCategoryControls;
            }
        }

        public Control GetControl(string controlFilter)
        {
            Tesco.Framework.UITesting.Entities.Control control = null;
            var keyArray = controlFilter.Split('_');
            if (_categoryControls != null && _categoryControls.Count > 0)
            {
                if (keyArray.Length > 1)
                {
                    CategoryControls selectedCategory = _categoryControls.FirstOrDefault(item => item.Name == keyArray[0]);
                    if (selectedCategory != null)
                    {
                        control = selectedCategory.lstControls.FirstOrDefault(ctrl => ctrl.Key.Equals(keyArray[1], StringComparison.OrdinalIgnoreCase));
                        if (control == null)
                        {
                            throw new Exception(string.Format("{0} : control not found in xml - Category : {1}, Control ID : {2}", Enums.ErrorType.InternalError, keyArray[0], keyArray[1]));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("{0} : Invalid control category - {1}", Enums.ErrorType.InternalError, keyArray[0]));
                    }
                }
                else
                {
                    throw new Exception(string.Format("{0} : Invalid control filter - {1}", Enums.ErrorType.InternalError, controlFilter));
                }
            }
            return control;
        }

        /// <summary>
        /// Get Data from Message XML
        /// </summary>
        /// <param name="xmlPath"></param>
        public static void GetMessages(string xmlPath)
        {
            try
            {
                var deserializedData = PopulateListOfXmlData<Messages>(xmlPath);

                if (deserializedData != null)
                {
                    Messages = deserializedData.lstMessages;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Message GetMessageByID(Enums.Messages messageID)
        {
            Message message = null;

            try
            {
                // this.CustomLogs.LogMessage(Constants.Consts.GETMESSAGEBYID, System.Diagnostics.TraceEventType.Start);

                if (Messages != null && Messages.Count > 0)
                {
                    message = Messages.FirstOrDefault(item => item.Id.Equals(messageID.ToString(), StringComparison.OrdinalIgnoreCase));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.CustomLogs.LogMessage(Constants.Consts.GETMESSAGEBYID, System.Diagnostics.TraceEventType.Stop);
            }

            return message;
        }

        /// <summary>
        /// Get Data from Data XML
        /// </summary>
        /// <param name="xmlPath"></param>
        public static void GetTestData(string xmlPath)
        {
            try
            {
                var deserializedData = PopulateListOfXmlData<TestingData>(xmlPath);

                if (deserializedData != null)
                {
                    TestingData = deserializedData.lstTestingData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public TestData GetTestDataByID(Enums.Messages messageID)
        {
            TestData testdata = null;

            try
            {
                // this.CustomLogs.LogMessage(Constants.Consts.GETMESSAGEBYID, System.Diagnostics.TraceEventType.Start);

                if (TestingData != null && TestingData.Count > 0)
                {
                    testdata = TestingData.FirstOrDefault(item => item.Id.Equals(messageID.ToString(), StringComparison.OrdinalIgnoreCase));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.CustomLogs.LogMessage(Constants.Consts.GETMESSAGEBYID, System.Diagnostics.TraceEventType.Stop);
            }

            return testdata;
        }

        /// <summary>
        /// Takes the browser code and initializes the webdriver for the particular browser
        /// </summary>
        /// <param name="BrowserCode"></param>

        public void InitializeWebDriver(string BrowserCode, string url)
        {
            IWebDriver driver = null;

            switch (BrowserCode.ToUpper())
            {
                case Consts.INTERNET_EXPLORER:
                    {
                        InternetExplorerDriverService iedriverService = InternetExplorerDriverService.CreateDefaultService(Utilities.EnvironmentSettings["DriverPath"]);
                        iedriverService.HideCommandPromptWindow = true;
                        InternetExplorerOptions options = new InternetExplorerOptions();
                        options.EnsureCleanSession = true;
                        options.IgnoreZoomLevel = true;
                        options.IntroduceInstabilityByIgnoringProtectedModeSettings = false;
                        options.AddAdditionalCapability("disable-popup-blocking", true);
                        options.EnsureCleanSession = true;
                        options.UnexpectedAlertBehavior = OpenQA.Selenium.IE.InternetExplorerUnexpectedAlertBehavior.Ignore;
                        driver = new InternetExplorerDriver(iedriverService, options);
                        driver.Manage().Window.Maximize();
                       
                        break;
                    }
                case Consts.MOZILLA_FIREFOX:
                    {
                        FirefoxProfile profile = new FirefoxProfile();   
                       // profile.SSetAcceptUntrustedCertificates(true);   
                        driver = new FirefoxDriver(profile);
                        //DesiredCapabilities caps = DesiredCapabilities.Firefox();
                        //caps.SetCapability(CapabilityType.AcceptSslCertificates, true);
                        //caps.SetCapability(CapabilityType.UnexpectedAlertBehavior, false);
                        //driver = new FirefoxDriver(caps);
                        //FirefoxProfile profile = new FirefoxProfile();
                        //profile.SetPreference("intl.accept_languages", "jp"); 
                        //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));  
                        //driver.Manage().Window.Maximize();
                        break;
                    }
                case Consts.GOOGLE_CHROME:
                    {
                       // ChromeDriverService gcdriverService = ChromeDriverService.CreateDefaultService(Utilities.EnvironmentSettings["DriverPath"]);
                       // gcdriverService.HideCommandPromptWindow = true;
                       // DesiredCapabilities capability = DesiredCapabilities.Chrome();
                       //// capability.SetCapability(CapabilityType.AcceptSslCertificates, true); 
                       // ChromeOptions gcoptions = new ChromeOptions();
                       //// gcoptions.AddArgument("--ignore-certificate-errors");
                       // driver = new ChromeDriver(gcdriverService);
                       // break;
                        ChromeDriverService gcdriverService = ChromeDriverService.CreateDefaultService(Utilities.EnvironmentSettings["DriverPath"]);
                        gcdriverService.HideCommandPromptWindow = true;
                        ChromeOptions gcoptions = new ChromeOptions();
                        DesiredCapabilities capability = DesiredCapabilities.Chrome();
                        capability.SetCapability(CapabilityType.AcceptSslCertificates, true); 
                        driver = new ChromeDriver(gcdriverService, new ChromeOptions());
                        break;

                    }
            }
            driver.Navigate().GoToUrl(url);
            this.WebDriver = driver;
        }
        
       
        /// <summary>
        /// Method to read the DB Configuration
        /// </summary>
        /// <returns></returns>
        public static DBConfiguration GetDBConfiguration(Enums.ConfugurationTypeEnum configType, string configKey, string xmlFile)
        {
            DBConfiguration configuration = new DBConfiguration();
            List<XElement> lstXmlConfig = new List<XElement>();
            XElement xmlConfig;
            XDocument xConfig = XDocument.Load(xmlFile);

            lstXmlConfig = (from t in xConfig.Descendants("Configuration")
                            where t.Element("ConfigurationType").Value.Equals(((Int32)configType).ToString())
                                      && (t.Element("ConfigurationName").Value.Equals(configKey) || t.Element("ConfigurationValue1").Value.Equals(configKey))
                                      select t).ToList();

            xmlConfig = lstXmlConfig.FirstOrDefault();
            if (xmlConfig != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DBConfiguration));
                StringReader reader = new StringReader(xmlConfig.ToString());
                configuration = (DBConfiguration)serializer.Deserialize(reader);
            }
            return configuration;
        }
       
        public static WebConfiguration GetWebConfiguration(string configKey, string xmlFile)
        {
            WebConfiguration configuration = new WebConfiguration();
            try
            {
                List<XElement> lstXmlConfig = new List<XElement>();
                XElement xmlConfig;
                XDocument xConfig = XDocument.Load(xmlFile);

                lstXmlConfig = (from t in xConfig.Descendants("appSettings").First().Descendants("add")
                                where t.Attribute("key").Value.Equals(configKey, StringComparison.CurrentCultureIgnoreCase)
                                select t).ToList();

                xmlConfig = lstXmlConfig.FirstOrDefault();
                if (xmlConfig != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(WebConfiguration));
                    StringReader reader = new StringReader(xmlConfig.ToString());
                    configuration = (WebConfiguration)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return configuration;
        }

        public static Resource GetResourceMessageContains(string configKey, string xmlFile)
        {
            Resource configuration = new Resource();
            try
            {
                List<XElement> lstXmlConfig = new List<XElement>();
                XElement xmlConfig;
                XDocument xConfig = XDocument.Load(xmlFile);

                lstXmlConfig = (from t in xConfig.Descendants("Data")
                                where t.Attribute("key").Value.Contains(configKey)
                                select t).ToList();
                xmlConfig = lstXmlConfig.FirstOrDefault();
              
                if (xmlConfig != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Resource));
                    StringReader reader = new StringReader(xmlConfig.ToString());
                    configuration = (Resource)serializer.Deserialize(reader);
                    // code to remove extra spaces
                    configuration.Value = string.Join(" ", configuration.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return configuration;
        }


        public static Resource GetResourceMessage(string configKey, string xmlFile)
        {
            Resource configuration = new Resource();
            try
            {
                List<XElement> lstXmlConfig = new List<XElement>();
                XElement xmlConfig;
                XDocument xConfig = XDocument.Load(xmlFile);

                lstXmlConfig = (from t in xConfig.Descendants("Data")
                                where t.Attribute("key").Value.Equals(configKey, StringComparison.CurrentCultureIgnoreCase)
                                select t).ToList();
                xmlConfig = lstXmlConfig.FirstOrDefault();

                if (xmlConfig != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Resource));
                    StringReader reader = new StringReader(xmlConfig.ToString());
                    configuration = (Resource)serializer.Deserialize(reader);
                    // code to remove extra spaces
                    configuration.Value = string.Join(" ", configuration.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return configuration;
        }

        public static Resource GetResourceMessageNew(string configKey, string node, string xmlFile)
        {
            Resource configuration = new Resource();
            try
            {
                List<XElement> lstXmlConfig = new List<XElement>();
                XElement xmlConfig;
               XDocument xConfig = XDocument.Load(xmlFile);
               XElement xElement = xConfig.Element(node);

               lstXmlConfig = (from t in xConfig.Descendants(node).Descendants("Data")
                               where t.Attribute("key").Value.Equals(configKey, StringComparison.CurrentCultureIgnoreCase)
                               select t).ToList();
               xmlConfig = lstXmlConfig.FirstOrDefault();

               if (xmlConfig != null)
               {
                   XmlSerializer serializer = new XmlSerializer(typeof(Resource));
                   StringReader reader = new StringReader(xmlConfig.ToString());
                   configuration = (Resource)serializer.Deserialize(reader);
                   // code to remove extra spaces
                   configuration.Value = string.Join(" ", configuration.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return configuration;
        }

        public static string GetMaskedClubcard(string clubcard)
        {            
            var aStringBuilder = new StringBuilder(clubcard);
            if (!clubcard.Contains(" XXXX XXXX "))
            {
                aStringBuilder.Remove(6, 8);
                aStringBuilder.Insert(6, " XXXX XXXX ");
            }
            return aStringBuilder.ToString();
        }

        #endregion
    }
}
