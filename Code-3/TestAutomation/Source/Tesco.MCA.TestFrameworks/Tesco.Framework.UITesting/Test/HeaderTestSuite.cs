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

namespace Tesco.Framework.UITesting.Test
{

    [TestClass]
    public class HeaderTestSuite : Base
    {
        public HeaderTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
        }

        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        static string LocalResourceFileName1 = "ClubcardOnline.Web-App_GlobalResources.en-GB.xml";

        // declare helpers
        Login objLogin = null;
        //Activation objActivation = null;
        Generic objGeneric = null;
        Message message = null;
        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        //TestData testData = null;

        static string culture;

        public static List<string> Cultures = new List<string>();
        public static List<Task> Tasks = new List<Task>();

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            string dataFile = string.Format(SanityConfiguration.TestDataFile, culture);
            AutomationHelper.GetTestData(dataFile);
            string msgFile = string.Format(SanityConfiguration.MessageDataFile, culture);
            AutomationHelper.GetMessages(msgFile);
            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;
            SanityConfiguration.LocalResourceFile = Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName1);
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
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyAboutClubcard()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            
            bool res = true;

            objGeneric.linkNavigate(LabelKey.ABOUTCLUBCARD, ControlKeys.HEADER_CLUBCARDLINKS, "Header Clubcard Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.ABOUT_CLUBCARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.ABOUTCLUBCARD, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("To Click and Verify all the header links and buttons")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyClubcardBoost()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            Resource resource1 = AutomationHelper.GetResourceMessage(LabelKey.CLUBCARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            Resource resource2 = AutomationHelper.GetResourceMessage(LabelKey.BOOST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            var expectedLinkName = string.Concat(string.Concat(resource1.Value, " "), resource2.Value);

            objGeneric.linkNavigateWithTwoRersource(expectedLinkName, ControlKeys.HEADER_CLUBCARDLINKS, "Header Clubcard Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.CLUBCARD_BOOST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.BOOST, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("To Click and Verify all the header links and buttons")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyClubcardPerks()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            Resource resource1 = AutomationHelper.GetResourceMessage(LabelKey.CLUBCARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            Resource resource2 = AutomationHelper.GetResourceMessage(LabelKey.PERKS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
            var expectedLinkName = string.Concat(string.Concat(resource1.Value, " "), resource2.Value);

            objGeneric.linkNavigateWithTwoRersource(expectedLinkName, ControlKeys.HEADER_CLUBCARDLINKS, "Header Clubcard Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_CLUBCARDPERKS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.PERKS, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyEmailSignup()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigate(LabelKey.HEADER_EMAILSIGNUP, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_EMAILSIGNUP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.EMAIL_SIGNUP, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyWebsiteFeedback()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigate(LabelKey.WEBSITE_FEEDBACK, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");
            
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();
            System.Threading.Thread.Sleep(6000);
            //driver.Navigate().Back();
            //objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }


        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyTescoDotCom()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigate(LabelKey.HEADER_TESCOCOM, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");
            
            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_TESCO_DOTCOM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.HEADER_TESCOCOM, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }


        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyFacebook()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigateWithTwoRersource(LabelKey.FACEBOOK, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");

            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_FACEBOOK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.FACEBOOK, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyTwitter()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigateWithTwoRersource(LabelKey.TWITTER, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");

            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_TWITTER, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.TWITTER, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyYouTube()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigateWithTwoRersource(LabelKey.YOUTUBE, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");

            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_YOUTUBE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.YOUTUBE, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P0")]
        [Owner("Bhim")]
        public void VerifyBasket()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);

            bool res = true;

            objGeneric.linkNavigateWithTwoRersource(LabelKey.BASKET, ControlKeys.HEADER_EXTERNALINKS, "Header Clubcard Links");

            res = objGeneric.VerifyBrowserAddress((AutomationHelper.GetResourceMessage(ValidationKey.HEADER_BASKET, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE))).Value, driver, LabelKey.BASKET, res);
            driver.Navigate().Back();
            objLogin.LogOut_Verification();
            Assert.IsTrue(res);
        }
       /*[TestMethod]
        [Description("To Verify the Header text")]
        [TestCategory("P1")]
        public void Verify_WelcomeToTescoClubcard()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.VerifyTextonthePageByXpath(Enums.Messages.WelcomeToTescoClubcard,ControlKeys.HEADER_WELCOME_TESCO, "HeaderMessage", driver);
        }

        [TestMethod]
        [Description("To Verify the Header text")]
        [TestCategory("P1")]
        public void Verify_WebsiteFeedback()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.ClickElementByXpath(ControlKeys.HEADER_WEBSITE_FEEDBACK, "HeaderLinks");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));
            //IAlert alert = 
            //driver.SwitchTo().Alert().Dismiss();
            //Console.WriteLine(alert.Text);
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            //alert.Accept();
            //alert.Dismiss();
        }*/

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
        }
    }

