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

        [TestMethod]
        [Description("To Click on Order A replacement and verify the title")]
        [TestCategory("P0_Regression")]
        [TestCategory("Sanity")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_ClickAndVerifyTitle()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                objOrderAReplacement.GetClubcardMaskedDigits(ControlKeys.ORDERREPLACEMENT_CLUBCARDNUMBER);
            }
            else
            {
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the parsed digits of the Tesco bank Clubcard in Order A Replacement page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_ValidateStandardClubcardMaskedDigits()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                objOrderAReplacement.GetClubcardMaskedDigits(ControlKeys.ORDERREPLACEMENT_CLUBCARDNUMBER);
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("To verify the parsed digits of the Tesco bank Clubcard in Order A Replacement page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_ValidateTescoBankClubcardMaskedDigits()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            string culture = CountrySetting.country;
            if (culture == "UK")
            {
                if (isPresent == "Y")
                {

                    objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                    objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                    objOrderAReplacement.GetClubcardMaskedDigits(ControlKeys.ORDERREPLACEMENT_CLUBCARDNUMBER);

                }

                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify 'Print Temporary CLubcard' functionality is not present on 'OrderReplacement' Page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_VerifyPrintTempCLubcardNotPresent()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                if (!objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)))
                {
                    customLogs.LogInformation("Verification passed.'Print Temp.Clubcard' functionality is not present on the Order Replacement page");
                    //objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.Fail("Test case Failed. 'Print Temp.Clubcard' functionality is present on the Order Replacement page");
                }

            }
            else
            {
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }


        [TestMethod]
        [Description("Verify 'OrderReplacement' functionality for Standard and Non-Standard Clubcards")]
        [TestCategory("OrderAReplacementStandardNonStandard")]
        public void OrderReplacement_VerifyStandardandOnlineStandardClubcardAccess()
        {
            string style = string.Empty;
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");

                if ((objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEB).Id))) &&
                    (!objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN).Id))))
                {
                     style = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEB).Id)).GetAttribute("style");
                    if (style == "DISPLAY: none")
                    {
                        driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN).Id)).Click();
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
                }
                else
                {
                    Assert.Fail("Test case Failed. Verfication failed for Standard Customer-ClubcardType 'S'");
                }
                objLogin.LogOut_Verification();

                //Scenario for Customer -ClubcardType=B
                objLogin.Login_Verification(testData.ClubcardNonStdTypeB, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                if ((objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEB).Id))) &&
                    (!objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN).Id))))
                {
                   
                     style = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEB).Id)).GetAttribute("style");
                    if (style == "DISPLAY: block")
                    {
                        driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN).Id)).Click();
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
                }
                else
                {
                    Assert.Fail("Test case Failed for Customer -ClubcardType 'B'");
                }
                objLogin.LogOut_Verification();

                //Scenario for Customer -ClubcardType=N
                objLogin.Login_Verification(testData.ClubcardNonStdTypeN, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeN);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                if ((!objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEB).Id))) &&
                      (objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN).Id))))
                {
                   
                    string country = CountrySetting.country;
                    if (country.Equals("UK"))
                    {
                     style = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN).Id)).GetAttribute("style");
                    }
                    else if(country.Equals("SK"))
                    {
                         style = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CLUBCARDTYPEN_SK).Id)).GetAttribute("style");
                    }
                    if (style == "DISPLAY: block")
                    {
                        customLogs.LogInformation("Verification passed.Order Replacement page is not accessible for Customer -ClubcardType 'N'");
                    }
                }
                else
                {
                    Assert.Fail("Test case Failed Customer -ClubcardType 'N'");
                }
                //objLogin.LogOut_Verification();
            }
            else
            {
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate Order repalcement for fourth time is not available")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_ValidateMaxReplacementOrdersReached()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardMaxOrdersReached, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardMaxOrdersReached);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objOrderAReplacement.OrderReplacementMaxCountReached(Login.CustomerID, CountrySetting.culture);
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }


        [TestMethod]
        [Description("Verify Unsuccessful Confirm of OrderReplacement Request")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        public void OrderReplacement_VerifyUnSuccessfulConfirm()
        {
           
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM).Id)).Click();
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
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("Verify Successful Confirm of OrderReplacement Request")]
        [TestCategory("OrderAReplacementSuccess")]
        public void OrderReplacement_VerifySuccessfulConfirm()
        {

            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN).Id)).Click();
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
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }


        [TestMethod]
        [Description("Reset OrderReplacement Request data for a specific Clubcard")]
        [TestCategory("ResetOrderAReplacementData")]
        public void OrderReplacement_ResetStubData()
        {
            bool bSuccess = false;
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                #region ResetClubcard1

                bSuccess = objOrderAReplacement.ResetOrderReplacementStubData(testData.ResetClubcard1, CountrySetting.culture);
                if (bSuccess)
                {
                    customLogs.LogInformation("Test case passed.Order Replacement Data is reset for process again");
                }
                else
                {
                    Assert.Fail("Test case Failed.Issue occured while resetting Order Replacement data");
                }
                #endregion ResetClubcard1

                #region ResetClubcard2
                bSuccess = objOrderAReplacement.ResetOrderReplacementStubData(testData.ResetClubcard2, CountrySetting.culture);
                if (bSuccess)
                {
                    customLogs.LogInformation("Test case passed.Order Replacement Data is reset for process again.");
                }
                else
                {
                    Assert.Fail("Test case Failed.Issue occured while resetting Order Replacement data");
                }
                #endregion ResetClubcard2
                
            }
            else
            {
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);

        }


        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
