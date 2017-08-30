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
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;
using System.Threading.Tasks;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.UITesting.Enums;
using System.Threading;

namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class OrderAReplacementTestSuite
    {
        public IWebDriver driver;
        private ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();


        // declare helpers
        Login objLogin = null;
        OrderAReplacement objOrderAReplacement = null;
        Generic objGeneric = null;

        private static string beginMessage = "********************* OrderAReplacement Test Suite ****************************";
        private static string suiteName = "OrderAReplacement";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public OrderAReplacementTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.ORDERAREPLACEMENTSUITE);
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
            objOrderAReplacement = new OrderAReplacement(objAutomationHelper, SanityConfiguration);
        }

        #region Sanity

        [TestMethod]
        [Description("To Click on Order A replacement link and verify the title")]
        [TestCategory("P0_Regression")]
        [TestCategory("Sanity")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("LeftNavigation")]
        [TestCategory("P0Set1")]
        public void LeftNavigation_ValidatePageTitle_OR()
        {

            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                string error = objOrderAReplacement.VerifyPageTitle();
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region P0

        [TestMethod]
        [Description("To verify the parsed digits of the Tesco bank Clubcard in Order A Replacement page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set1")]
        public void OrderReplacement_ValidateStandardClubcardMaskedDigits()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.MainAccount.Clubcard, CountrySetting.culture);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                objOrderAReplacement.GetClubcardMaskedDigits(ControlKeys.ORDERREPLACEMENT_CLUBCARDNUMBER);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the parsed digits of the Tesco bank Clubcard in Order A Replacement page using VirginClubcard")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set6")]
        public void OrderReplacement_ValidateTescoBankClubcardMaskedDigits()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.VirginAccount.Clubcard, CountrySetting.culture);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                objOrderAReplacement.GetClubcardMaskedDigits(ControlKeys.ORDERREPLACEMENT_CLUBCARDNUMBER);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify 'Print Temporary CLubcard' functionality is not present on 'OrderReplacement' Page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set1")]
        public void OrderReplacement_VerifyPrintTempCLubcardNotPresent()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if (!objGeneric.IsElementPresentOnPage(By.Id(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)))
                {
                    customLogs.LogInformation("Verification passed.'Print Temp.Clubcard' functionality is not present on the Order Replacement page");
                }
                else
                {
                    Assert.Fail("Test case Failed. 'Print Temp.Clubcard' functionality is present on the Order Replacement page");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("Verify 'OrderReplacement' functionality for Standard and Non-Standard Clubcards")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacementStandardNonStandard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set1")]

        public void OrderReplacement_VerifyStandardandOnlineStandardClubcardAccess()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.OnlyStdAccount.Clubcard, CountrySetting.culture);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.OnlyStdAccount.Clubcard, testData.OnlyStdAccount.Password, testData.OnlyStdAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.OnlyStdAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.OnlyStdAccount.Clubcard);
                objGeneric.ClickElement(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN, FindBy.CSS_SELECTOR_ID);
                bool bSuccess = objOrderAReplacement.VerifySuccessfulConfirm();
                if (bSuccess)
                {
                    customLogs.LogInformation("Verification passed.Order Replacement request is confirmed for STANDARD customer.");
                }
                else
                {
                    Assert.Fail("Test case Failed.Order Replacement process failed");
                }
                customLogs.LogInformation("Verification passed.Order Replacement Page is accessible for Standard Customer");
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify 'OrderReplacement' functionality for Standard and Non-Standard Clubcards")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacementStandardNonStandard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0_Sequential")]


        public void OrderReplacement_VerifyNonStandardBankClubcardAccess()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.ClubcardNonStdTypeB.Clubcard, CountrySetting.culture);
            string style = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                //Scenario for Customer -ClubcardType=B
                objLogin.Login_Verification(testData.ClubcardNonStdTypeB.Clubcard, testData.ClubcardNonStdTypeB.Password, testData.ClubcardNonStdTypeB.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB.Clubcard);
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN).Id)).Click();
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                bool bSuccess = objOrderAReplacement.VerifySuccessfulConfirm();
                if (bSuccess)
                {
                    customLogs.LogInformation("Verification passed.Order Replacement request is confirmed for customer type B but with error message.");
                }
                else
                {
                    Assert.Fail("Test case Failed.Order Replacement process failed");
                }
                customLogs.LogInformation("Verification passed.Order Replacement page is accessible for Customer -ClubcardType 'B' but with sorry message");
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify 'OrderReplacement' functionality for Standard and Non-Standard Clubcards")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacementStandardNonStandard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0_Sequential")]
        public void OrderReplacement_VerifyNonStandardClubcardAccess()
        {
            string style = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                //Scenario for Customer -ClubcardType=N
                objLogin.Login_Verification(testData.ClubcardNonStdTypeN.Clubcard, testData.ClubcardNonStdTypeN.Password, testData.ClubcardNonStdTypeN.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeN.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeN.Clubcard);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if ((objGeneric.IsElementPresentOnPage(By.XPath(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN).XPath))))
                {
                    customLogs.LogInformation("Verification passed.Order Replacement page is not accessible for Customer -ClubcardType 'N'");
                }
                else
                {
                    Assert.Fail("Test case Failed Customer -ClubcardType 'N'");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate Order repalcement for fourth time is not available")]
        [TestCategory("P0_Regression")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P1_Sequential")]
        public void OrderReplacement_ValidateMaxReplacementOrdersReached()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MaxOrdersReached.Clubcard, testData.MaxOrdersReached.Password, testData.MaxOrdersReached.EmailID);
                objLogin.SecurityLayer_Verification(testData.MaxOrdersReached.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                Thread.Sleep(5000);
                objOrderAReplacement.OrderReplacementMaxCountReached(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }


        [TestMethod]
        [Description("Verify Unsuccessful Confirm of OrderReplacement Request")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set2")]
        public void OrderReplacement_VerifyUnSuccessfulConfirm()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.ResetClubcard1.Clubcard, CountrySetting.culture);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ResetClubcard1.Clubcard, testData.ResetClubcard1.Password, testData.ResetClubcard1.EmailID);
                objLogin.SecurityLayer_Verification(testData.ResetClubcard1.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.ResetClubcard1.Clubcard);
                bool bSuccess = objOrderAReplacement.VerifySuccessfulConfirm();
                if (bSuccess)
                {
                    customLogs.LogInformation("Verification passed.Order Replacement request is not confirmed.NO reason selected by the user.");
                }
                else
                {
                    Assert.Fail("Test case Failed.Error message not displayed to the user when no reason is selected while confirming Order Replacment Request");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("Verify Successful Confirm of OrderReplacement Request")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacementSuccess")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0")]
        [TestCategory("OrderReplacement")]
        [TestCategory("P0_OrderReplacement")]
        [TestCategory("P0Set3")]
        public void OrderReplacement_VerifySuccessfulConfirm()
        {
            objOrderAReplacement.ResetOrderReplacementStubData(testData.ResetClubcard2.Clubcard, CountrySetting.culture);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ResetClubcard2.Clubcard, testData.ResetClubcard2.Password, testData.ResetClubcard2.EmailID);
                objLogin.SecurityLayer_Verification(testData.ResetClubcard2.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.ResetClubcard2.Clubcard);
                objGeneric.ClickElement(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN, FindBy.CSS_SELECTOR_ID);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                bool bSuccess = objOrderAReplacement.VerifySuccessfulConfirm();
                if (bSuccess)
                {
                    customLogs.LogInformation("Verification passed.Order Replacement request is confirmed.Reason selected by the user.");
                }
                else
                {
                    Assert.Fail("Test case Failed.Success message not displayed to the user when reason is selected while confirming Order Replacment Request");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }
        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
