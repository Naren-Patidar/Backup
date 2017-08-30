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
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using Tesco.Framework.UITesting.Services;



namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class HomeSecurityTestSuite
    {
        public IWebDriver driver;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        protected List<string> lst = new List<string>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        ILogger customLogs = null;
        static private string isSecurityPageEnabled = String.Empty;
        string clubcard = string.Empty;
        SmartVoucherAdapter objVoucherService = null;
        // declare helpers
        Home objHome = null;
        Login objLogin = null;
        HomeSecurity objHomeSecurity = null;
        Generic objGeneric = null;
        Stamp objStamp = null;
        MyContactPreference objMyContactPreference = null;
        static string culture;

        public static List<string> Cultures = new List<string>();

        static TestData_AccountDetails testData = null;
        static TestData_HomeSecurity testData_Home = null;

        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static TestDataHelper<TestData_HomeSecurity> ADTestData_home = new TestDataHelper<TestData_HomeSecurity>();

        private static string beginMessage = "********************* Home Security ****************************";
        private static string suiteName = "Home Security";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        public HomeSecurityTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
        }

        /// Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;

            ADTestData_home.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_HomeSecurity).Name, SanityConfiguration.Domain);
            testData_Home = ADTestData_home.TestData;

            //isSecurityPageEnabled = (AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile)).Value;
        }

        // Test initialization method
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
                customLogs.LogInformation(suiteName + "Suite is currently running for country " + culture);
                objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                driver = objAutomationHelper.WebDriver;
            }

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
            //initialize helper objects
            objLogin = new Login(objAutomationHelper, SanityConfiguration);
            objHomeSecurity = new HomeSecurity(objAutomationHelper, SanityConfiguration, testData_Home);
            objHome = new Home(objAutomationHelper);
            objGeneric = new Generic(objAutomationHelper);
            objStamp = new Stamp(objAutomationHelper, SanityConfiguration, testData);
            objMyContactPreference = new MyContactPreference(objAutomationHelper);
            objVoucherService = new SmartVoucherAdapter();
            //populating the list with clubcard numbers
            lst.Add(testData.MainAccount.Clubcard);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }


        #region Sanity

        [TestMethod]
        [Description("Click and verify stamp 1")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp1()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY1);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(0);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 1 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 2")]
        [TestCategory("Stamp")]
        [TestCategory("HomeSecurity")]
        [TestCategory("Sanity")]
        public void HomeSecurity_Stamp_ValidateStamp2()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY2);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 2 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 3")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp3()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY3);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 3 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 4")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp4()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY4);
            if (isPresent)
            {
                Control stampC = new Control();
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(3);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 4 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 5")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp5()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY5);
            if (isPresent)
            {
                Control stampC = new Control();
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(4);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 5 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 6")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp6()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY6);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(5);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 6 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 7")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp7()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY7);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(6);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 7 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();

        }

        [TestMethod]
        [Description("Click and verify stamp 8")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        [TestCategory("HomeSecurity")]
        public void HomeSecurity_Stamp_ValidateStamp8()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY8);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(7);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 8 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Click and verify stamp 9")]
        [TestCategory("Stamp")]
        [TestCategory("Sanity")]
        public void HomeSecurity_Stamp_ValidateStamp9()
        {
            bool isPresent = objGeneric.IsStampEnabled(ControlKeys.STAMPKEY9);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string error = objStamp.ValidateStamp(8);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Stamp 9 is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        #endregion

        #region P0

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_06")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_VerifySuccess()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }


        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_01")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]

        public void HomeSecurity_VerifySecurityForUnusedVouchers()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_06, MCA_SCN_UK_005_TC_07")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_VerifySecurityForUsedorNoVouchers()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_15")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("HomeSecurity")]
        [Owner("Tesco")]
        public void HomeSecurity_VoucherToPersonalDetailsAndVerifySecurity()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_13")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void HomeSecurity_SubmitSecurityOnVoucherAndNavigateToPersonalDetails()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify if 2LA is enabled for Voucher Page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_Validate2LAForVoucherPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                DBConfiguration homeSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
                DBConfiguration voucherSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, DBConfigKeys.VOUCHERS, SanityConfiguration.DbConfigurationFile);
                if (Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1))
                {
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                }
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");

                if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                    voucherSecurityConfig.ConfigurationValue1.Equals("0") && voucherSecurityConfig.ConfigurationValue2.Equals("0")
                    && objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                {
                    customLogs.LogInformation(endMessage);
                }
                else if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                   voucherSecurityConfig.ConfigurationValue1.Equals("1"))
                {
                    objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    customLogs.LogInformation(endMessage);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Voucher Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify 2LA configuration for Voucher Page based on VoucherValue")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_Validate2LAforVoucherPageAsperValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                DBConfiguration homeSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
                DBConfiguration voucherSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, DBConfigKeys.VOUCHERS, SanityConfiguration.DbConfigurationFile);
                if (Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1))
                {
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                }
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");


                if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                    voucherSecurityConfig.ConfigurationValue1.Equals("0") && !voucherSecurityConfig.ConfigurationValue2.Equals("0"))
                {
                    if (objVoucherService.GetUnUsedVoucher(testData.MainAccount.Clubcard))
                    {
                        objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id));
                    }
                    else
                    {
                        objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    }
                    customLogs.LogInformation(endMessage);

                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Voucher Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify if 2LA is enabled for Boosts Page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_Validate2LAForBoostsPage()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                DBConfiguration homeSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
                DBConfiguration boostSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, DBConfigKeys.BOOSTSATTESCO, SanityConfiguration.DbConfigurationFile);
                if (Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1))
                {
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                }
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");

                if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                    boostSecurityConfig.ConfigurationValue1.Equals("0") && boostSecurityConfig.ConfigurationValue2.Equals("0")
                    && objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                {
                    customLogs.LogInformation(endMessage);
                }
                 else if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                   boostSecurityConfig.ConfigurationValue1.Equals("1"))
                {               
                    objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                    customLogs.LogInformation(endMessage);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("To verify 2LA configuration for Boosts Page based on BoostsValue")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_HomeSecurity")]
        public void HomeSecurity_Validate2LAforBoostsPageAsperValue()
        {
              bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
              if (isPresent)
              {
                  objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                  DBConfiguration homeSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
                  DBConfiguration boostSecurityConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, DBConfigKeys.BOOSTSATTESCO, SanityConfiguration.DbConfigurationFile);
                  if (Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1))
                  {
                      objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                  }
                  objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                  objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");


                  if (!Convert.ToBoolean(homeSecurityConfig.ConfigurationValue1) &&
                      boostSecurityConfig.ConfigurationValue1.Equals("0") && !boostSecurityConfig.ConfigurationValue2.Equals("0"))
                  {
                      if (objGeneric.IsUnSpendBoostExists())
                      {
                          objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id));
                      }
                      else
                      {
                          objGeneric.verifyPageName(LabelKey.TESCOBOOST, "BoostatTesco", SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                      }
                      customLogs.LogInformation(endMessage);
                  }
              }
              else
              {
                  Assert.Inconclusive(string.Format("Boost Page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
              }
              customLogs.LogInformation(endMessage);


        }

        /*
         * User should not be taken to previous page if:
         ** user has signed out from security page
         ** user has signed out from vouchers page
         ** user has signed out from home page before 2LA check
         ** user has signed out from home page after 2LA check
         * User should be taken to previous page if:
         ** home page -> security page (voucher flow) -> ##Back button## -> home page
         ** home page -> security page (voucher flow) -> voucher page --> home page -> ##Back button## -> voucher page 
         ** home page -> security page (voucher flow) -> orer replacement page -> ##Back button## -> either security page or home page
         */

        [TestMethod]
        [Description("User should not be taken to previous page if user has signed out from security page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn1()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.LogOut_Verification();

                objGeneric.NavigateBrowserBack();
                Assert.IsFalse(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)));

                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("User should not be taken to previous page if user has signed out from vouchers page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn2()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objLogin.LogOut_Verification();

                objGeneric.NavigateBrowserBack();
                Assert.IsFalse(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_CLICK).Id)));

                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("User should not be taken to previous page if user has signed out from home page before 2LA check")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn3()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);                
                objLogin.LogOut_Verification();

                objGeneric.NavigateBrowserBack();
                Assert.IsFalse(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)));

                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("User should not be taken to previous page if user has signed out from home page after 2LA check")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn4()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                driver.Navigate().GoToUrl(SanityConfiguration.MCAUrl);

                objLogin.LogOut_Verification();

                objGeneric.NavigateBrowserBack();
                Assert.IsFalse(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)));

                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("User should be allowed to go to previous page:: home page -> security page (voucher flow) -> ##Back button## -> home page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn5()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objGeneric.NavigateBrowserBack();
                Assert.IsTrue(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_WELCOME_MSGNAME).Id)));
                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("User should be allowed to go to previous page:: home page -> security page (voucher flow) -> voucher page --> home page -> ##Back button## -> voucher page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn6()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                driver.Navigate().GoToUrl(SanityConfiguration.MCAUrl);
                objGeneric.NavigateBrowserBack();

                objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("User should be allowed to go to previous page:: home page -> security page (voucher flow) -> order replacement page -> ##Back button## -> either security page or home page")]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0_Set1")]
        [TestCategory("P0_BackButton")]
        [TestCategory("P0_HomeSecurity")]
        [Owner("Tesco")]
        public void Security_BackBtn7()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objGeneric.ClickAnchorTagInsideAContainer(ControlKeys.SECURITY_LNKORDERREPLACEMENT, "Security");
                objGeneric.NavigateBrowserBack();
                Assert.IsTrue(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)));
                driver.Quit();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);

        }


        #endregion

        #region P1

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_14")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P1_HomeSecurity")]
        [TestCategory("P1Set5")]
        [Owner("Tesco")]
        public void HomeSecurity_SubmitWrongSecurityOnVoucherAndNavigateToPersonalDetails()
        {
            try
            {
                objHomeSecurity.CheckValueFromConfigurationType();
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        //The below test is not valid anymore as the security digists field allows only numeric values
        //[TestMethod]
        //[Description("MCA_SCN_UK_013_TC_02")]
        //[TestCategory("P1")]
        //[TestCategory("HomeSecurity")]
        //[TestCategory("P1_HomeSecurity")]
        //[Owner("Infy")]
        //public void HomeSecurity_VerifyErrorMessage_Space()
        //{
        //    try
        //    {
        //        objLogin.Login_Verification(testData.Clubcard.Clubcard, testData.Clubcard.Password, testData.Clubcard.EmailID);
        //        objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
        //        //for (int i = 0; i <= lst.Count; i++)
        //        //{
        //        // objLogin.Login_Verification(lst[i], testData.Clubcard.Password, testData.Clubcard.EmailID);
        //        //    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
        //        //    //    break;
        //        //    //else
        //        //    //    i++;
        //        //    break;
        //        //}
        //        objHomeSecurity.CaptureSecurityDigitsPosition();
        //        objHomeSecurity.InsertSecurityDigitsPosition_Space();
        //        objHomeSecurity.CaptureInvalidSpaceErrorMessage();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScreenShotDetails.TakeScreenShot(driver, ex);
        //        driver.Quit();
        //        customLogs.LogError(ex);
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //    }
        //    customLogs.LogInformation(endMessage);
        //}

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_07")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P1")]
        [TestCategory("P1_HomeSecurity")]
        [Owner("Infy")]
        [TestCategory("P1Set5")]
        public void HomeSecurity_VerifyErrorMsg_ClubcardNoNotInRange()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objHomeSecurity.CheckValueFromConfigurationType();
                objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
                objHomeSecurity.CaptureSecurityDigitsPosition();
                objHomeSecurity.InsertWrongSecurityDigitsPosition(testData.MainAccount.Clubcard);
                objHomeSecurity.CaptureInvalidNoErrorMessage();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_11")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P1_HomeSecurity")]
        [Owner("Infy")]
        [TestCategory("P1Set5")]
        public void HomeSecurity_VerifySecurityPageSwitchOff()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
                objHome.Homepage_Verification();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description(" To check that account gets locked after 5 incorrect attempts, MCA_SCN_UK_013_TC_09")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P1_HomeSecurity")]
        [TestCategory("P1_Sequential")]
        [Owner("Infy")]
        public void HomeSecurity_ExceedingMaxAttempts()
        {
            Debug.WriteLine(string.Format("{0}|{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            try
            {
                objLogin.Login_Verification(testData.BlockedAccount.Clubcard, testData.BlockedAccount.Password, testData.BlockedAccount.EmailID);
                objHomeSecurity.Verify2LAPage();
                objHomeSecurity.Verify_SecurityMaxAttempts();
                objLogin.Security_LogOut();
                objLogin.Login_Verification(testData.BlockedAccount.Clubcard, testData.BlockedAccount.Password, testData.BlockedAccount.EmailID);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
                string isSecurityEnabled = config.ConfigurationValue1;
                if (isSecurityEnabled == "FALSE")
                {
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objHomeSecurity.Verify_MaxAttemptsBlockedClubcard();
                    objLogin.Security_LogOut();
                }
                else
                    objHomeSecurity.Verify_MaxAttemptsBlockedClubcard();
                    objLogin.Security_LogOut();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_04")]
        [TestCategory("P1")]
        [TestCategory("P1_HomeSecurity")]
        [TestCategory("HomeSecurity")]
        [Owner("Infy")]
        [TestCategory("P1Set5")]
        public void HomeSecurity_VerifyErrorMessage_RandomNo()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
                objHomeSecurity.CaptureSecurityDigitsPosition();
                objHomeSecurity.InsertSecurityDigitsPosition_random();
                objHomeSecurity.CaptureInvalidNoErrorMessage();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate User does not lands to home/security page after logout.")]
        [TestCategory("Security")]
        [TestCategory("P1")]
        public void LogOutSuccess()
        {
            try
            {
                customLogs.LogInformation("LogOutSuccess started");
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                string UrlAfterLogin = driver.Url;
                objLogin.LogOut_Verification();
                objHomeSecurity.VerifyUrlAfterLogOut(UrlAfterLogin);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region P2

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_01")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P2")]
        [TestCategory("P2_HomeSecurity")]
        public void HomeSecurity_PageText()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
              //  objHomeSecurity.VerifyIsSecurityPageEnabled(DBConfigKeys.ENABLEHOMEPAGE);
                objHomeSecurity.Verify2LAPage();
               
                objHomeSecurity.VerifyDefaultMsg(ControlKeys.SECURITY_DEFAULTMESSAGE);
                objHomeSecurity.VerifySecurityDigitPosition();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify My Message Section")]
        [TestCategory("P2")]
        [TestCategory("P2_HomeSecurity")]
        public void HomeSecurity_VerifyMyMessageSection()
        {
            try
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objHome.VerifyMyMessageSection(ControlKeys.HOME_PREFMSG, LabelKey.MYMESSAGE_PREFERENCEMESSAGE);
              //  objHome.VerifyMyMessageSection(ControlKeys.HOME_ADDRESSMSG, LabelKey.MYMESSAGE_ADDRESSMESSAGE);
              //  objHome.VerifyMyMessageSection(ControlKeys.HOME_POINTSMSG, LabelKey.MYMESSAGE_POINTSMESSAGE);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }
        #endregion

        # region Home Page Banner

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_IsBanner")]

        public void HomePage_IsBannerEnabled()
        {

            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objHomeSecurity.Verify_BannerEnabled(testData.MainAccount.DotcomId);
                customLogs.LogInformation(endMessage);
                driver.Quit();
            }

            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));

        }

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Banner_PostcodeFirst3DigitMasked")]

        public void HomePage_Banner_PostcodeFirst3DigitMasked()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    objHomeSecurity.Verify_BannerPostcode();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        }

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Banner_Surname")]

        public void HomePage_Banner_Surname()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    objHomeSecurity.Verify_BannerSurname();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));

        }
        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Banner_Yes")]
        public void HomePage_Banner_Yes()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    objHomeSecurity.Verify_BannerYes();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        }

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Tesco.Com_Banner_Yes")]
        public void HomePage_TescoDotComBanner_Yes()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    objHomeSecurity.Verify_TescoDotCom_BannerYes();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        }

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Tesco.Com_Banner_Yes")]
        public void HomePage_TescoDotComBanner_DetailsNotCorrect()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);

                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    objHomeSecurity.Verify_TescoDotCom_BannerNo();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        }


        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Banner_No")]
        public void HomePage_Banner_No()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    //driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_NO).Id)).Click();

                    ReadOnlyCollection<IWebElement> lnkView = driver.FindElements(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_NO).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView[0]);
                   
                    
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                   // objHomeSecurity.Verify_Banner_No();
                    driver.Quit();
                }
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));

        }

        [TestMethod]
        [TestCategory("P0")]
        [TestCategory("HomeSecurity")]
        [TestCategory("P0Set1")]
        [TestCategory("HomePage_Banner_Details")]
        [Description("")]
        public void HomePage_Banner_Details()
        {
            string isBannerEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISBANNERENABLED);
            if (isBannerEnabled == "1")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                if (!objHomeSecurity.IsOnlineDetailsMatched(testData.MainAccount.DotcomId))
                {
                    var actualBannerPostcode = (driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_POSTCODE).Id)).Text).ToString();
                    var actualBannerSurname = (driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_SURNAME).Id)).Text).ToString();
                   //  string str = (driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_NO).Id))).Text;
                    ReadOnlyCollection<IWebElement> lnkView = driver.FindElements(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_BANNER_NO).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView[0]);

                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objHomeSecurity.Verify_Banner_Details(actualBannerSurname, actualBannerPostcode);

                }
                driver.Quit();
            }
            else
                Assert.Inconclusive(string.Format("Banner is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));

        }



        # endregion

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }
    }
}
