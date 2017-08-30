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
using OpenQA.Selenium.Interactions;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class HomeTestSuit
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
        MyContactPreference objMyContactPreference = null;
        Home objHome = null;
        HomeSecurity objHomeSecurity = null;

        private static string beginMessage = "********************* Home ****************************";
        private static string suiteName = "Home";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> homeTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public HomeTestSuit()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.HOMETESTSUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            homeTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = homeTestData.TestData;

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
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
            objHome = new Home(objAutomationHelper);
            objHomeSecurity = new HomeSecurity(objAutomationHelper);
            objMyContactPreference = new MyContactPreference(objAutomationHelper);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("To verify Print functionality of Temporary Clubcard on Home Page")]
        [TestCategory("P0_Regression")]
        [TestCategory("HomePage")]
        [TestCategory("OrderAReplacement")]
        public void Home_Verify_Print_TempClubcard()
        {
            string isPresent = objGeneric.VerifyAppSettings(DBConfigKeys.PRINT_TEMPCLUBCARD);
            if (isPresent == "N")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);

                //bool isPrintCLubcardClicked = objHome.Homepage_VerfiyPrintClubcard();
                //if (isPrintCLubcardClicked)
                //{
                //   customLogs.LogInformation("Print Temporary CLubcard button clicked");
                //objLogin.LogOut_Verification();
                //}
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Message on home page when customer mailinststus is 3")]
        [TestCategory("HomePage")]
        public void Home_MessageSection1()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            Thread.Sleep(5000);
            //objHome.VerifyMessagSectionCMS3();
        }

        [TestMethod]
        [Description("Verify Message on home page when current date is in between cutoff date and sign off date")]
        [TestCategory("HomePage")]
        public void Home_MessageSection2()
        {
            objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
            //objHome.VerifyMessagSectionCDlessthanSignoff();
        }

        [TestMethod]
        [Description("Verify Message on home page when current date is not in range of cutoff date and sign off date")]
        [TestCategory("HomePage")]
        public void Home_MessageSection3()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            //objHome.VerifyMessagSectionMailingAE();
        }

        [TestMethod]
        [Description("Verify Points summary home page")]
        [TestCategory("HomePage")]
        public void Home_PointsSummarySection()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            //objHome.VerifyPointsSection();
        }

        [TestMethod]
        [Description("Verify Vouchers summary on home page")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_VoucherSummarySection()
        {
            objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (secConfigs.ConfigurationValue1.Equals("1"))
            {
                objHome.verifySecondBoxValue(testData.StandardAccount.Clubcard, OptionPreference.None, false);
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");
            }
            objHome.verifySecondBoxValue(testData.StandardAccount.Clubcard, OptionPreference.None, true);
        }

        [TestMethod]
        [Description("Verify Total vouchers spend on home page for christmas saver type customer")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_ChristamasSaverSummarySection()
        {
            objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (secConfigs.ConfigurationValue1.Equals("1"))
            {
                objHome.verifySecondBoxValue(testData.ChristmasSaverAccount.Clubcard, OptionPreference.Xmas_Saver, false);
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");
            }
            objHome.verifySecondBoxValue(testData.ChristmasSaverAccount.Clubcard, OptionPreference.Xmas_Saver, true);
        }

        [TestMethod]
        [Description("Verify my Message box section on home page")]
        [TestCategory("P1")]
        [TestCategory("HomePage")]
        public void Home_MyMessageBoxSection()
        {
            objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (secConfigs.ConfigurationValue1.Equals("1"))
            {
                string IsMyMessageBoxVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMESSAGEBOXVISIBLE);
                bool ele = objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_LBL).Id));

                if (IsMyMessageBoxVisible == "1" && ele == true)
                {
                    customLogs.LogInformation("Configuration is enabled and element is also present hence test pass");
                }
                else if (IsMyMessageBoxVisible == "0" && ele == false)
                {
                    customLogs.LogInformation("Configuration is disable and element is also not present hence test pass");
                }
                else
                {
                    Assert.Fail();  
                }
            }
        }

        [TestMethod]
        [Description("Verify xmus saver customers lands on the xmus saver page if they click on voucher strip")]
        [TestCategory("P1")]
        [TestCategory("HomePage")]
        public void Home_ChristamasSaverCustomerLink()
        {
            objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (secConfigs.ConfigurationValue1.Equals("1"))
            {
                IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_VOUCHER_LINK).Id));             
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].click();", lnkView);               
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                Thread.Sleep(500);
                objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);                 
            }           
        }

        [TestMethod]
        [Description("Verify Voucher customers lands on the voucher page if they click on voucher strip")]
        [TestCategory("P1")]
        [TestCategory("HomePage")]
        public void Home_VoucherCustomerStripLink()
        {
            objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (secConfigs.ConfigurationValue1.Equals("1"))
            {
                IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_VOUCHER_LINK).Id));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].click();", lnkView);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                Thread.Sleep(500);               
                objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
            }
        }

        [TestMethod]
        [Description("Verify historical Voucher strip should not availiable for voucher customer")]
        [TestCategory("P2")]
        [TestCategory("HomePage")]
        public void Home_HistoricalVoucherIsAvailForVoucherCustmer()
        {
            string IsVoucherStripVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISVOUCHERSTRIPVISIBLE);
            if (IsVoucherStripVisible == "1")
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);

                if (secConfigs.ConfigurationValue1.Equals("1"))
                {
                    //-- you should be on home page
                    string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
                    Assert.AreEqual(Home, objAutomationHelper.WebDriver.Title);  
                   
                    //--Element should not be availiable for voucher customer
                    if ((objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id))))
                    {
                        Assert.Fail("HistoricalVoucher element is availiable for voucher customer");
                    }                   
                }
            }
        }

        [TestMethod]
        [Description("Verify historical Voucher strip is availiable if Config key IsVoucherStripVisible is enable and logged in customer is non voucher customer and having vouchers")]
        [TestCategory("P2")]
        [TestCategory("HomePage")]
        public void Home_HistoricalVoucherStripLinkIsAvailiable()
        {
            string IsVoucherStripVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISVOUCHERSTRIPVISIBLE);
            if (IsVoucherStripVisible == "1")
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);

                if (secConfigs.ConfigurationValue1.Equals("1"))
                {
                    // IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id));
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id))))
                    {
                        Assert.Fail("Element is not present");
                    }

                    string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
                    Assert.AreEqual(Home, objAutomationHelper.WebDriver.Title);
                }
            }
        }

        [TestMethod]
        [Description("Verify historical Voucher value is not visible if security is not cleared")]
        [TestCategory("P2")]
        [TestCategory("HomePage")]
        public void Home_HistoricalVoucherValueIsVisible()
        {
            string IsVoucherStripVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISVOUCHERSTRIPVISIBLE);
            if (IsVoucherStripVisible == "1")
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);

                if (secConfigs.ConfigurationValue1.Equals("1"))
                {
                    //--Verify the user is on the home page
                    string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
                    Assert.AreEqual(Home, objAutomationHelper.WebDriver.Title);

                    // IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id));
                    if ((objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id))))
                    {
                        IWebElement element = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERSLEFT).Id));
                        if (element.Text != "")
                        {
                            Assert.Fail();
                        }                        
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    
                }
            }
        }

        [TestMethod]
        [Description("Verify Customer click on the historical Voucher strip")]
        [TestCategory("P2")]
        [TestCategory("HomePage")]
        public void Home_HistoricalVoucherVerifyClick()
        {
            string IsVoucherStripVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISVOUCHERSTRIPVISIBLE);
            if (IsVoucherStripVisible == "1")
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);

                if (secConfigs.ConfigurationValue1.Equals("1"))
                {
                    //--Verify the user is on the home page
                    string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
                    Assert.AreEqual(Home, objAutomationHelper.WebDriver.Title);

                    //--Get the element and click on that
                    IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView);
                    objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                    Thread.Sleep(500);

                    //--Customer should navigate to voucher page
                    objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                }
            }
        }

        [TestMethod]
        [Description("Verify historical Voucher value on home page from voucher page availiable voucher value")]
        [TestCategory("P2")]
        [TestCategory("HomePage")]
        public void Home_HistoricalVoucherValueCompareWithVoucherPage()
        {
            string IsVoucherStripVisible = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISVOUCHERSTRIPVISIBLE);
            if (IsVoucherStripVisible == "1")
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);

                if (secConfigs.ConfigurationValue1.Equals("1"))
                {
                    //--Verify the user is on the home page
                    string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
                    Assert.AreEqual(Home, objAutomationHelper.WebDriver.Title);

                    //--Get the element and click on that
                    IWebElement lnkView = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERS).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView);
                    objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                    Thread.Sleep(500);

                    //--Customer should navigate to voucher page
                    objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);

                    //--Get the voucher total from the voucher page.
                    IWebElement voucherTotalOnVoucherPage = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_VOUCHERTOTAL).Id));
                    string strVoucherValueOnVoucherPage = voucherTotalOnVoucherPage.Text;

                    //--Navigate to home page and get the voucher total from there
                    objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");

                    Thread.Sleep(500);
                    IWebElement voucherTotalOnHomePage = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_HISTORICALVOUCHERSLEFT).Id));

                    if (!(strVoucherValueOnVoucherPage.Trim().ToUpper() == voucherTotalOnHomePage.Text.Trim().ToUpper()))
                    {
                        Assert.Fail("Not matching : voucherTotalOnHomePage=" + voucherTotalOnHomePage.Text + " voucherTotalOnVoucherPage=" + strVoucherValueOnVoucherPage);
                    }


                }
            }
        }            
        

        [TestMethod]
        [Description("VerifY home page for customer opted for Avois premimum")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_AviosSection()
        {
            objLogin.Login_Verification(testData.AviosAccount.Clubcard, testData.AviosAccount.Password, testData.AviosAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (!secConfigs.ConfigurationValue1.Equals("1"))
            {
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.AviosAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");
            }
            objHome.verifySecondBoxValue(testData.AviosAccount.Clubcard, OptionPreference.Airmiles_Standard);
        }


        [TestMethod]
        [Description("VerifY home page for customer opted for BA Avois premimum")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_BAAviosSection()
        {
            objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (!secConfigs.ConfigurationValue1.Equals("1"))
            {
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");
            }
            objHome.verifySecondBoxValue(testData.BAAviosPreAccount.Clubcard, OptionPreference.BA_Miles_Standard);
        }

        [TestMethod]
        [Description("VerifY home page for customer opted for  Virgin Atlantic ")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_VirginAtlanticSection()
        {
            objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
            DBConfiguration secConfigs = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "HOME", SanityConfiguration.DbConfigurationFile);
            if (!secConfigs.ConfigurationValue1.Equals("1"))
            {
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.HOME, "Home", "home");
            }
            objHome.verifySecondBoxValue(testData.VirginAccount.Clubcard, OptionPreference.Virgin_Atlantic);
        }

        [TestMethod]
        [Description("VerifY coupon banner on home page ")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_CouponSection()
        {
            DBConfiguration couponConf = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, "HideCouponBanner", SanityConfiguration.DbConfigurationFile);
            if (couponConf.ConfigurationValue1.Equals("1"))
            {
                objLogin.Login_Verification(testData.ActiveCouponAccount.Clubcard, testData.ActiveCouponAccount.Password, testData.ActiveCouponAccount.EmailID);
                objHomeSecurity.Verify2LAPage();
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                string error = objHome.VerifyCouponBanner();
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Banner is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (objAutomationHelper.WebDriver != null)
            {
                objAutomationHelper.WebDriver.Quit();
            }
        }

    }
}