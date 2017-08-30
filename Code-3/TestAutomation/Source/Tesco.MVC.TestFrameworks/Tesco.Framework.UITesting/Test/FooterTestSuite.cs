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
//using Tesco.Framework.UITesting.Test.TestMethod;
using Tesco.Framework.UITesting.Helpers;
using System.Diagnostics;

namespace Tesco.Framework.UITesting.Test
{

    [TestClass]
    public class FooterTestSuite : Base
    {
        public FooterTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
        }

        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        //static string LocalResourceFileName = "ClubcardOnline.Web-App_GlobalResources.en-GB.xml";

        // declare helpers
        Login objLogin = null;
        //Activation objActivation = null;
        Generic objGeneric = null;
        Message message = null;
        //TestData_AccountDetails testData = null;
        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;
        static ILogger CustomLogs;

        public static List<string> Cultures = new List<string>();
        public static List<Task> Tasks = new List<Task>();

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            /* string dataFile = string.Format(SanityConfiguration.TestDataFile, culture);
             AutomationHelper.GetTestData(dataFile);

             string msgFile = string.Format(SanityConfiguration.MessageDataFile, culture);
             AutomationHelper.GetMessages(msgFile); */

            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;
            //SanityConfiguration.LocalResourceFile = Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName);

            Utilities.InitializeLogger(ref CustomLogs, AppenderType.UNITTEST);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            //Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
            //objAutomationHelper = new AutomationHelper();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

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
                objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                //objCommonMethod = new CommonMethod(objAutomationHelper);
                driver = objAutomationHelper.WebDriver;
            }

            //initialize helper objects
            objLogin = new Login(objAutomationHelper, SanityConfiguration);
            objGeneric = new Generic(objAutomationHelper);

            //objActivation = new Activation(objAutomationHelper, SanityConfiguration);
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

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        #region P0

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifySiteMap()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objGeneric.linkNavigate(LabelKey.SITEMAP, ControlKeys.FOOTER_LINKS, "Footer Links");
            bool res = true;
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_SITEMAP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.SITEMAP, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyContactUs()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            objGeneric.linkNavigate(LabelKey.CONTACTUS, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_CONTACTUS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.CONTACTUS, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyPrivacyCookies()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            objGeneric.linkNavigate(LabelKey.PRIVACY_COOKIES, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_PRIVACY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.PRIVACY_COOKIES, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyEmailSignup()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            objGeneric.linkNavigate(LabelKey.EMAIL_SIGNUP, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_EMAILSIGNUP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.EMAIL_SIGNUP, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyLostClubcard()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            objGeneric.linkNavigate(LabelKey.LOST_CLUBCARD, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_LOSTCLUBCARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.LOST_CLUBCARD, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }


        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyTescoDotcom()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            objGeneric.linkNavigate(LabelKey.TESCO_DOTCOM, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_TESCODOTCOM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.TESCO_DOTCOM, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyHelpAndFAQs()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            Resource resource1 = AutomationHelper.GetResourceMessage(LabelKey.HELP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            Resource resource2 = AutomationHelper.GetResourceMessage(LabelKey.FAQs, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            var expectedLinkName = string.Concat(string.Concat(resource1.Value, " & "), resource2.Value);

            objGeneric.linkNavigateWithTwoRersource(expectedLinkName, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_HELP_FAQS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.HELP, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }


        [TestMethod]
        [Description(" ")]        
        [TestCategory("P0_Footer")]
        [TestCategory("Footer")]
        [Owner("Bhim")]
        public void Footer_VerifyTermsConditions()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            bool res = true;
            Resource resource1 = AutomationHelper.GetResourceMessage(LabelKey.TERMS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            Resource resource2 = AutomationHelper.GetResourceMessage(LabelKey.CONDITIONS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            var expectedLinkName = string.Concat(string.Concat(resource1.Value, " & "), resource2.Value);
            objGeneric.linkNavigateWithTwoRersource(expectedLinkName, ControlKeys.FOOTER_LINKS, "Footer Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.FOOTER_TERMS_CONDITIONS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.TESCO_DOTCOM, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        #endregion
       

        #region P2

        [TestMethod]
        [Description("To validate the footer html against the application footer")]
        [TestCategory("HTMLComparision")]
        [TestCategory("P2")]
        [TestCategory("P2_Header")]
        [TestCategory("Footer")]
        [TestCategory("P2_Regression")]
        public void Footer_VerifyFooterHTML()
        {
            string htmlFile = Path.Combine(SanityConfiguration.HtmlDataDirectory, SanityConfiguration.HTMLFiles.FOOTER_HTML);
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            string content = Encoding.ASCII.GetString(File.ReadAllBytes(htmlFile));
            objGeneric.ValidateHtml(htmlFile, ControlKeys.HTML_FOOTER);

        }
        [TestMethod]
        [Description("To validate the year footer html against the application footer")]
        [TestCategory("P2")]
        public void Footer_VerifyYearInFooterHTML()
        {
            string htmlFile = Path.Combine(SanityConfiguration.HtmlDataDirectory, SanityConfiguration.HTMLFiles.FOOTER_HTML);
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            string content = Encoding.ASCII.GetString(File.ReadAllBytes(htmlFile));
            objGeneric.ValidateYear(htmlFile, ControlKeys.HTML_FOOTER);
        } 


        #endregion

        

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
