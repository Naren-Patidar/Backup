using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using System.IO;

using System.Xml.Serialization;
using System.Configuration;
using System.Collections.ObjectModel;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.CustomException;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;
using System.Threading.Tasks;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.UITesting.Helpers;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class ActivationTestSuite
    {
        public IWebDriver driver;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        ILogger customLogs = null;

        // declare helpers
        Login objLogin = null;
        Activation objActivation = null;
        Generic objGeneric = null;
       
        static TestDataHelper<TestData_Activation> ActTestData = new TestDataHelper<TestData_Activation>();
        static TestData_Activation testData;

        static string culture;

        public static List<string> Cultures = new List<string>();

        private static string beginMessage = "********************* Activation ****************************";
        private static string suiteName = "Activation";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);


        public ActivationTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.ACTIVATIONSUITE);
         }

        /// <summary>
        /// Selects the country and load the control and message xml
        /// </summary>
        /// <param name="testContext"></param>
        /// 
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            
            ActTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_Activation).Name, SanityConfiguration.Domain);
            testData = ActTestData.TestData;
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
         if (SanityConfiguration.RunAllBrowsers)
            {
                List<string> browsers = Enum.GetNames(typeof(Browser)).ToList();
                foreach (string browser in browsers)
                {
                    objAutomationHelper = new AutomationHelper();
                    objAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl);
                    lstAutomationHelper.Add(objAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + "Suite is currently running for country " + culture  + " for domain" + SanityConfiguration.Domain);
                if (SanityConfiguration.Domain.Equals("DBT"))
                    objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.ActivationUrl);
                else if (SanityConfiguration.Domain.Equals("PPE"))
                    objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                driver = objAutomationHelper.WebDriver;
                switch (SanityConfiguration.DefaultBrowser)
                {
                    case Browser.IE:
                        if (driver.Title == "Certificate Error: Navigation Blocked")
                            objAutomationHelper.WebDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                        break;
                    case Browser.GC:
                        break;
                    case Browser.MF:
                        break;
                }
            }

            //initialize helper objects
            objLogin = new Login(objAutomationHelper, SanityConfiguration);
            objGeneric = new Generic(objAutomationHelper);
            objActivation = new Activation(objAutomationHelper, SanityConfiguration, testData);          
        }

        [TestMethod]
        [Description("Browse Activation page")]
        [TestCategory("Sanity")]
        public void BrowseActivationPage()
        {            
            if (SanityConfiguration.Domain.Equals("PPE"))
            {
                objActivation.RegisterNewId();
                objActivation.CheckConfigurationForActivation();
                objActivation.VerifyActivationPage(driver);
                driver.Manage().Cookies.DeleteAllCookies();
            }
            else if (SanityConfiguration.Domain.Equals("DBT"))
            {
                objActivation.CheckConfigurationForActivation();
                objActivation.VerifyActivationPage(driver); 
            }
        }


        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_25")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_SuccessfulActivationConfirmationMsgVerification()
        {
           
            objActivation.CheckConfigurationForActivation();
            driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
            objActivation.CSC_Login();
            objActivation.CSC_SearchCustomerAndDelinkAccount();
           // objActivation.CSC_Logout();
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            objActivation.Activation_CheckSuccessMessage("Activation");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("")]
        [Owner("Infosys Team")]
        [TestCategory("P0")]
        public void Activation_ActivatedConfirmatioMsgVerification()
        {
            objActivation.CheckConfigurationForActivation();
            driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
            objActivation.CSC_Login();
            if (objActivation.CSC_SearchCustomerAndCheckAccount())
            {
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                objActivation.Activation_CheckConnectedMessage("Activation");
            }
            else
            {
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                objActivation.Activation_CheckSuccessMessage("Activation");
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                objActivation.Activation_CheckConnectedMessage("Activation");
            }

            customLogs.LogInformation(endMessage);
                       
        }


        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_26")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_ActivationConfirmationErrorMsgVerification()
        {
            objActivation.CheckConfigurationForActivation();
            driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
            objActivation.CSC_Login();
            if (objActivation.CSC_SearchCustomerAndDelinkAccount())
            {
               // objActivation.CSC_Logout();
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                objActivation.Activation_CheckActivationErrorMessage("Activation");
            }
            else 
            {
                //objActivation.CSC_Logout();
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                objActivation.Activation_CheckActivationErrorMessage("Activation");
            }
            customLogs.LogInformation(endMessage);

        }
       
        [TestMethod]
        [Description("")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_ErrorMsgVerification_Clubcard()
        {
            objActivation.CheckConfigurationForActivation();
            objActivation.Activation_CheckErrorMsgForInvalidClubCard();
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_ErrorMsgVerification_FirstName()
        {
            objActivation.CheckConfigurationForActivation();
            objActivation.Activation_CheckErrorMsgForInvalidFirstName();
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_ErrorMsgVerification_PostCode()
        {
            objActivation.CheckConfigurationForActivation();
            objActivation.Activation_CheckErrorMsgForInvalidPostCode();
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("")]
        [Owner("Infosys Team")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        public void Activation_ErrorMsgVerification_Surname()
        {
            objActivation.CheckConfigurationForActivation();
            objActivation.Activation_CheckErrorMsgForInvalidSurName();
            customLogs.LogInformation(endMessage);
        }


        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
            customLogs.LogInformation(endMessage);

        }

    }
}
