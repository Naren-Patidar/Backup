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
    public class MyLatestStatementTestSuite
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;

        private static string beginMessage = "********************* My Latest Statement Suite ****************************";
        private static string suiteName = "My Latest Statement";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public MyLatestStatementTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.MYLATESTSTATEMENTSUITE);
        }

        // Selects the country and load the control and message xml 
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
        }

        // Test initialization method
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
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + culture + " for domain" + SanityConfiguration.Domain);
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
        }


        [TestMethod]
        [Description("To Click on My Latest Statement Tab And Verify the Title")]
        [Owner("Infosys")]
        [TestCategory("Sanity")]
        public void MyLatestStatement_ClickAndVerifyTitle()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
              //  objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the stamp functionality for MLS page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void StampHomepage_MLS()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.LATESTSTATEMENT))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.LATESTSTATEMENT)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_LATESTSTATEMENT, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.LATESTSTATEMENT);

                    objGeneric.stampClick(ControlKeys.STAMP5, "MLS", StampName.LATESTSTATEMENT);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                    //objLogin.SecurityLayer_Verification(testData.Clubcard);
                   

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

      


        [TestMethod]
        [Description("To Click on My Latest Statement Tab And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyLatestStatement_ClickAndVerifyTitle_Stamp()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.LATESTSTATEMENT))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.LATESTSTATEMENT)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                 string isPresent = config.IsDeleted;
                 if (isPresent == "N")
                 {
                     objLogin.Login_Verification(testData.Clubcard, testData.Password,testData.EmailID);
                     objLogin.SecurityLayer_Verification(testData.Clubcard);
                     objGeneric.stampClick(ControlKeys.STAMP5, "My Latest Statement", StampName.LATESTSTATEMENT);
                    // objGeneric.ClickStamp(LabelKey.MYLATESTSTATEMENT, ControlKeys.STAMP4, "My Latest Statement");
                     objLogin.LogOut_Verification();
                 }
                 else
                     Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                 customLogs.LogInformation(endMessage);
             }
        }
       
        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }
}
