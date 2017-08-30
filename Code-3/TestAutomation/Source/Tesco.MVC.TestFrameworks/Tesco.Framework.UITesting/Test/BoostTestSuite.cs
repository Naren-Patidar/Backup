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
using System.Threading;
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
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using System.Globalization;


namespace Tesco.Framework.UITesting.Test.Common
{
    [TestClass]
    public class BoostAtTescoTestSuite : Base
    {
        ILogger customLogs = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        OptionsAndBenefits objOpt = null;
        BoostAtTesco objBoostAtTesco = null;
        private static string beginMessage = "********************* Option And Benefit ****************************";
        private static string suiteName = "Option And Benefit";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public BoostAtTescoTestSuite()
        {
            ObjAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.ACCOUNTDETAILSSUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;
            string msgFile = string.Format(SanityConfiguration.MessageDataFile, culture);
            AutomationHelper.GetMessages(msgFile);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
            if (SanityConfiguration.RunAllBrowsers)
            {
                List<string> browsers = Enum.GetNames(typeof(Browser)).ToList();
                foreach (string browser in browsers)
                {
                    ObjAutomationHelper = new AutomationHelper();
                    ObjAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl);
                    lstAutomationHelper.Add(ObjAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + culture + " for domain" + SanityConfiguration.Domain);
                ObjAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                switch (SanityConfiguration.DefaultBrowser)
                {
                    case Browser.IE:
                        if (ObjAutomationHelper.WebDriver.Title == "Certificate Error: Navigation Blocked")
                            ObjAutomationHelper.WebDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                        break;
                    case Browser.GC:
                        break;
                    case Browser.MF:
                        break;
                }
            }

            //initialize helper objects
            objLogin = new Login(ObjAutomationHelper, SanityConfiguration);
            objGeneric = new Generic(ObjAutomationHelper);
            objOpt = new OptionsAndBenefits(ObjAutomationHelper, SanityConfiguration);
            objBoostAtTesco = new BoostAtTesco(ObjAutomationHelper);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        #region Boost P0_Regression Test Cases

        [TestMethod]
        [Description("To Verify Boost token description in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostToken")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenDescription()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostTokenDescriptionOnline(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify Boost  Order status in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostToken")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]

        public void Boost_TokenOrderStatus()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostOrderStatus(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify Boost Token Value in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostToken")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]

        public void Boost_TokenValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostTokenValue(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify BoostTokenEcode in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostToken")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenEcode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostTokenEcode(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify BoostTokenEcode in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenExpiryDate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostExpiryDate(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify Boost booking date in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenBookingDate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateBoostBookingDate(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("P0Set9")]
        public void Boost_TokenInstoreDescription()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateTokenDescription(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("P0Set9")]
        public void Boost_TokenInstoreStatus()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateInstorestatus(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("P0Set9")]
        public void Boost_TokenInstoreValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateInstoreValue(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenInstoreDateOrdered()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateInstoreDateOrdered(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenInstoreValidUntil()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                ValidateInstoreValidUntil(testData.BoostAccount.Clubcard);
                ObjAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P0")]
        [TestCategory("P0_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P0_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set9")]
        public void Boost_TokenPrintAll()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                objGeneric.ClickElementJavaElement(ControlKeys.BOOSTSELECTALL, "vouchers");
                objGeneric.ClickElementJavaElement(ControlKeys.BOOSTPRINTALL, "vouchers");
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region Boost P1 Regression Test Cases

        [TestMethod]
        [Description("To Verify Boost token descripton in online section")]
        [TestCategory("P1")]
        [TestCategory("P1_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P1_Regression")]
        [TestCategory("BoostExpiryDate")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P1Set1")]
        public void Boost_TokenPrintWithoutSelecting()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                objGeneric.ClickElementJavaElement(ControlKeys.BOOSTPRINTALL, "vouchers");
                objGeneric.VerifyTextonthePageByXpath(LabelKey.PRINTALLWITHOUTSELECTION, ControlKeys.BOOSTMESSAGE, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region Boost P2 Regression Test Cases

        [TestMethod]
        [Description("To Verify Boost text against application text")]
        [TestCategory("P2")]
        [TestCategory("P2_Boost")]
        [TestCategory("Boost")]
        [TestCategory("P2_Regression")]
        [TestCategory("TextValidation")]
        [TestCategory("Perk_Elixir_S2")]
        public void Boost_TextValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                objBoostAtTesco.TextValidation("BoostPage");
                ObjAutomationHelper.WebDriver.Close();
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region Helper Methods

        public void ValidateBoostExpiryDate(string clubcard)
        {
            try
            {


                List<Reward> BoostTokenEcode = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTEXPIRYDATE).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                BoostTokenEcode = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);



                int iCheck = 0;
                for (int i = 0; i < BoostTokenEcode.Count; i++)
                {

                    String MyDateString;
                    DateTime MyDateTime;

                    MyDateString = BoostTokenEcode[i].ValidUntil.ToString().Trim();
                    MyDateTime = Convert.ToDateTime(MyDateString);
                    MyDateString = MyDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    // MyDateTime = DateTime.ParseExact(MyDateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(MyDateString))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, BoostTokenEcode.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }



        }

        public void ValidateBoostBookingDate(string clubcard)
        {
            try
            {


                List<Reward> BoostBookingDate = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTDATEORDERED).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                BoostBookingDate = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);



                int iCheck = 0;
                for (int i = 0; i < BoostBookingDate.Count; i++)
                {

                    String MyDateString;
                    DateTime MyDateTime;

                    MyDateString = BoostBookingDate[i].BookingDate.ToString().Trim();
                    MyDateTime = Convert.ToDateTime(MyDateString);
                    MyDateString = MyDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    // MyDateTime = DateTime.ParseExact(MyDateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(MyDateString))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, BoostBookingDate.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ValidateTokenDescription(string clubcard)
        {
            try
            {

                List<Reward> TokenDescription = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TOKENINSTOREDESCRIPTION).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetTokenDetails(Login.CustomerID, CountrySetting.culture);


                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    TokenDescription[i].TokenValue.ToString().Trim();

                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(TokenDescription[i].TokenDescription.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, TokenDescription.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ValidateBoostTokenDescriptionOnline(string clubcard)
        {
            try
            {
                List<Reward> TokenDescription = new List<Reward>();
                ReadOnlyCollection<IWebElement> BoostTokenDescription = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTTOKEN).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);
                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    if (BoostTokenDescription[i].Text.ToString().Trim().Equals(TokenDescription[i].TokenDescription.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }
                }
                Assert.AreEqual(iCheck, TokenDescription.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ValidateInstoreValidUntil(string clubcard)
        {
            try
            {
                List<Reward> TokenDescription = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TOKENINSTOREVALIDUNTIL).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetTokenDetails(Login.CustomerID, CountrySetting.culture);
                String MyDateString;
                DateTime MyDateTime;
                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    MyDateString = TokenDescription[i].ValidUntil.ToString().Trim();
                    MyDateTime = Convert.ToDateTime(MyDateString);
                    MyDateString = MyDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //  x = "£" + x + ".00";
                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(MyDateString))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }
                }
                Assert.AreEqual(iCheck, TokenDescription.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ValidateBoostOrderStatus(string clubcard)
        {
            try
            {

                List<Reward> BoostOrderStatus = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTORDERSTATUS).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                BoostOrderStatus = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);

                int iCheck = 0;
                for (int i = 0; i < BoostOrderStatus.Count; i++)
                {


                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(BoostOrderStatus[i].ProductStatus.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, BoostOrderStatus.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ValidateInstorestatus(string clubcard)
        {
            try
            {

                List<Reward> TokenDescription = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TOKENINSTOREPRODUCTSTATUS).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetTokenDetails(Login.CustomerID, CountrySetting.culture);


                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    TokenDescription[i].TokenValue.ToString().Trim();

                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(TokenDescription[i].ProductStatus.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, TokenDescription.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ValidateInstoreDateOrdered(string clubcard)
        {
            try
            {

                List<Reward> TokenDescription = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TOKENINSTOREDATE).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetTokenDetails(Login.CustomerID, CountrySetting.culture);

                String MyDateString;
                DateTime MyDateTime;
                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    MyDateString = TokenDescription[i].BookingDate.ToString().Trim();
                    MyDateTime = Convert.ToDateTime(MyDateString);
                    MyDateString = MyDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //  x = "£" + x + ".00";


                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(MyDateString))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, TokenDescription.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ValidateInstoreValue(string clubcard)
        {
            try
            {

                List<Reward> TokenDescription = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TOKENINSTORETOKENVALUE).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                TokenDescription = service.GetTokenDetails(Login.CustomerID, CountrySetting.culture);


                int iCheck = 0;
                for (int i = 0; i < TokenDescription.Count; i++)
                {
                    string x = TokenDescription[i].ProductTokenValue.ToString().Trim();
                    //  x = "£" + x + ".00";
                    TokenDescription[i].TokenValue.ToString().Trim();

                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Contains(TokenDescription[i].ProductTokenValue.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, TokenDescription.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }


        public void ValidateBoostTokenEcode(string clubcard)
        {
            try
            {

                List<Reward> BoostTokenEcode = new List<Reward>();
                ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTTOKENECODE).XPath));
                RewardServiceAdaptor service = new RewardServiceAdaptor();
                string description = string.Empty;
                BoostTokenEcode = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);


                int iCheck = 0;
                for (int i = 0; i < BoostTokenEcode.Count; i++)
                {
                    BoostTokenEcode[i].TokenValue.ToString().Trim();

                    if (BoostOrderStatusapplication[i].Text.ToString().Trim().Equals(BoostTokenEcode[i].SupplierTokenCode.ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, BoostTokenEcode.Count);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }


        public void ValidateBoostTokenValue(string clubcard)
        {
            try
            {
                //bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.DISABLE_CURRENCY_DECIMAL);
                //if (isPresent == false)
                {
                    List<Reward> BoostTokenValue = new List<Reward>();
                    ObjAutomationHelper.WebDriver = ObjAutomationHelper.WebDriver;
                    ReadOnlyCollection<IWebElement> BoostOrderStatusapplication = ObjAutomationHelper.WebDriver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.BOOSTTokenValue).XPath));
                    RewardServiceAdaptor service = new RewardServiceAdaptor();
                    string description = string.Empty;
                    BoostTokenValue = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);


                    int iCheck = 0;
                    for (int i = 0; i < BoostTokenValue.Count; i++)
                    {
                        BoostTokenValue[i].TokenValue.ToString().Trim();

                        if (BoostOrderStatusapplication[i].Text.ToString().Trim().Contains(BoostTokenValue[i].TokenValue.ToString().Trim()))
                        {
                            iCheck++;
                        }
                        else
                        {
                            CustomLogs.LogMessage("Clubcard doesnot have boost token description", TraceEventType.Start);
                        }

                    }

                    Assert.AreEqual(iCheck, BoostTokenValue.Count);
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(ObjAutomationHelper.WebDriver, ex);
                CustomLogs.LogException(ex);
                ObjAutomationHelper.WebDriver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }


        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            if (ObjAutomationHelper != null && ObjAutomationHelper.WebDriver != null)
            {
                ObjAutomationHelper.WebDriver.Quit();
            }
        }

    }
}
