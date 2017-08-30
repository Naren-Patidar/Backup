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
using System.Diagnostics;
using System.Threading;
using Tesco.Framework.UITesting.Services;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class MyVoucherTestSuite
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        SmartVoucherAdapter objVoucherService = null;
        PreferenceServiceAdaptor objPrefService = null;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyVouchers objVoucher = null;

        private static string beginMessage = "********************* My Voucher Test Suite ****************************";
        private static string suiteName = "Voucher Test suite";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();
        
        
        static string culture;

        public MyVoucherTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.VOUCHERTESTSUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
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
            objVoucher = new MyVouchers(objAutomationHelper);
            objVoucherService = new SmartVoucherAdapter();
            objPrefService = new PreferenceServiceAdaptor();
        }

        #region Sanity

        [TestMethod]
        [Description("To Click on My Vouchers link And Verify the Title")]
        [Owner("Infosys")]
        [TestCategory("3435-TH")]
        [TestCategory("Sanity")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_Vouchers()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        #endregion

        #region BasicFunctionality

        [TestMethod]
        [Description("To check print voucher is working")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("MVC")]
        [Priority(0)]
        public void MyVoucher_PrintAll()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.UnusedVoucherAccount.Clubcard, testData.UnusedVoucherAccount.Password, testData.UnusedVoucherAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.UnusedVoucherAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.UnusedVoucherAccount.Clubcard);
                string type = objPrefService.GetPreference(testData.UnusedVoucherAccount.Clubcard, CountrySetting.culture);
                objVoucher.verifyPageName(type);
                objGeneric.ClickElement(ControlKeys.MYVOUCHER_SelectAll, FindBy.CSS_SELECTOR_ID);
                objGeneric.ClickElement(ControlKeys.MYVOUCHER_PrintVoucher, FindBy.CSS_SELECTOR_ID);
                customLogs.LogInformation(endMessage);
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        #endregion

        #region P0

        [TestMethod]
        [Description("To validate the stamp functionality for Voucher page")]
        public void StampHomepage_MyVoucher()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.VOUCHERS))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.VOUCHERS)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_MYVOUCHERS, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.VOUCHERS);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Voucher page", StampName.VOUCHERS);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.VOUCHER_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_27")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set7")]
        public void MyVoucher_PrintActiveVoucher()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.verifyValidationMessage(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objGeneric.ClickElementJavaElement(ControlKeys.MYVOUCHER_SelectAll, "vouchers");
                    objGeneric.ClickElementJavaElement(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                    customLogs.LogInformation(endMessage);
                }
                else
                    Assert.Fail("Clubcard does not have active vouchers");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_28")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set7")]
        public void MyVoucher_PrintAllctiveVouchers()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.verifyValidationMessage(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objGeneric.ClickElementJavaElement(ControlKeys.MYVOUCHER_SelectAll, "vouchers");
                    objGeneric.ClickElementJavaElement(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                    customLogs.LogInformation(endMessage);
                }
                else
                    Assert.Fail("Clubcard does not have active vouchers");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_39")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set1")]

        public void MyVoucher_VoucherOnSocialSite1()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string isFacebookRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isTwitterRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isFacebookRequired == "TRUE" && isTwitterRequired == "TRUE")
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE, ControlKeys.MYVOUCHER_SOCIALSITE, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.CheckImagePresent(Enums.VoucherSection.BothEnabled);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Test case not applicable for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_40")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set1")]

        public void MyVoucher_VoucherOnSocialSite2()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string isFacebookRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isTwitterRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isFacebookRequired == "FALSE" && isTwitterRequired == "TRUE")
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE, ControlKeys.MYVOUCHER_SOCIALSITE, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.CheckImagePresent(Enums.VoucherSection.TwitterEnabled);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Test case not applicable for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_41")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set1")]

        public void MyVoucher_VoucherOnSocialSite3()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string isFacebookRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isTwitterRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isFacebookRequired == "TRUE" && isTwitterRequired == "FALSE")
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE, ControlKeys.MYVOUCHER_SOCIALSITE, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.CheckImagePresent(Enums.VoucherSection.FacebookEnabled);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Test case not applicable for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_42")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set1")]

        public void MyVoucher_VoucherOnSocialSite4()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string isFacebookRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isTwitterRequired = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isFacebookRequired == "FALSE" && isTwitterRequired == "FALSE")
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objVoucher.CheckImagePresent(Enums.VoucherSection.BothDisabled);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Test case not applicable for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_14")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0Set7")]

        public void MyVoucher_Verify3Section()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.UnusedVoucherAccount.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.NoPreference.ToString()))
                {
                    if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                    {
                        objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                        objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                        objVoucher.VerifySection(Enums.VoucherSection.Displayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                        objVoucher.VerifySection(Enums.VoucherSection.Displayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                        objVoucher.VerifySection(Enums.VoucherSection.Displayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);
                        customLogs.LogInformation(endMessage);
                    }
                    else
                        Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
                }
                else
                {
                    Assert.Inconclusive("3section in voucher is not displayed for Preferences-"+ type);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_15")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set7")]

        public void MyVoucher_VerifyActiveVoucherSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.verifyValidationMessage(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objGeneric.VerifyTextExistOnPage(ControlKeys.MYVOUCHER_TABLEUNUSED);
                    customLogs.LogInformation(endMessage);
                }
                else
                    Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_16")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set7")]

        public void MyVoucher_VerifyTotalActiveVoucherCount()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);

                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);

                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");

                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objVoucher.VerifyTotalCount(Enums.VoucherSection.UnUsed, LabelKey.CURRENCYSYMBOL, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    customLogs.LogInformation(endMessage);
                }
                else
                    Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_17 & MCA_SCN_UK_005_TC_18")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0_Sequential")]

        public void MyVoucher_VerifyAviosSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.AviosAccount.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.Avios.ToString()))
                {
                    objLogin.Login_Verification(testData.AviosAccount.Clubcard, testData.AviosAccount.Password, testData.AviosAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.AviosAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.AviosAccount.Clubcard);
                   // objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.Avios);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Avios not present for country : {0}, Clubcard : {1}", CountrySetting.country, testData.AviosAccount));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_19 & MCA_SCN_UK_005_TC_20")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0_Sequential")]

        public void MyVoucher_VerifyBAAviosSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.OnlyStdAccount.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.BAAvios.ToString()))
                {
                    objLogin.Login_Verification(testData.OnlyStdAccount.Clubcard, testData.OnlyStdAccount.Password, testData.OnlyStdAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.OnlyStdAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.OnlyStdAccount.Clubcard);
                  //  objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.BAAvios);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);
                }
                else
                {
                    Assert.Inconclusive(string.Format("BAAvios not present for country : {0}, Clubcard : {1}", CountrySetting.country, testData.StandardAccount.Clubcard));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_21 & MCA_SCN_UK_005_TC_22")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0_Sequential")]
        public void MyVoucher_VerifyVirginAtlanticSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.VirginAccount.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.VirginAtlantic.ToString()))
                {
                    objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                   // objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.VirginAtlantic);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                    objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Virgin Atlantic not present for country : {0}, Clubcard : {1}", CountrySetting.country, testData.VirginAccount.Clubcard));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_23")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set7")]

        public void MyVoucher_PrintMyVoucherIsDisplayed()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUnUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_PrintVoucher).Id), driver))
                        customLogs.LogInformation("Print My Voucher Button Present");
                    else
                        Assert.Fail("Print My Voucher Button Not Present");
                }
                else
                    Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_24")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set8")]

        public void MyVoucher_VerifyUsedSectionForRedeemed()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.verifyValidationMessage(LabelKey.USEDVOUCHER, ControlKeys.MYVOUCHER_LBLUSEDVOUCHER, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    objGeneric.VerifyTextExistOnPage(ControlKeys.MYVOUCHER_TABLEREDEEMED);
                    customLogs.LogInformation(endMessage);
                }
                else
                    Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_25")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set8")]
        public void MyVoucher_TotalOfRedeemedVouchers()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objVoucher.VerifyTotalCount(Enums.VoucherSection.Used, LabelKey.CURRENCYSYMBOL, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                    customLogs.LogInformation(endMessage);
                }
                else
                {
                    Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_26")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("P0")]
        [TestCategory("MyVoucher")]
        [TestCategory("P0_MyVoucher")]
        [TestCategory("P0Set8")]
        public void MyVoucher_RedeemedVouchersPrintDisabled()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                if (objVoucherService.GetUsedVoucher(testData.StandardAccount.Clubcard))
                {
                    objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                    objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);                    
                    ReadOnlyCollection<IWebElement> coun = driver.FindElements(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_PrintVoucher).Id));
                    if (coun.Count <= 1)
                        customLogs.LogInformation("Print My voucher is not present under Redeemed Voucher Section");
                    else
                    {
                        customLogs.LogInformation("Print My voucher is present under Redeemed Voucher Section");
                        Assert.Fail("Print My voucher is present under Redeemed Voucher Section");
                    }
                }
                else
                    Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion       

        #region P1

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_29")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression_MVC")]
        [TestCategory("P1")]
        [TestCategory("MyVoucher")]
        [TestCategory("P1_MyVoucher")]
        public void MyVoucher_PrintTopUpVouchers()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.TopUpClubcard.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.XmasSaver.ToString()))
                {
                    if (objVoucherService.GetUnUsedVouchertype(testData.TopUpClubcard.Clubcard, Enums.VoucherType.TopUp.ToString()))
                    {
                        objLogin.Login_Verification(testData.TopUpClubcard.Clubcard, testData.TopUpClubcard.Password, testData.TopUpClubcard.EmailID);
                        objLogin.SecurityLayer_Verification(testData.TopUpClubcard.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                        objLogin.SecurityLayer_Verification(testData.TopUpClubcard.Clubcard);
                        objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                        objGeneric.ClickElementJavaElement(ControlKeys.MYVOUCHER_SELECTONE, "vouchers");
                        objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                    }
                    else
                        customLogs.LogInformation("Check the type of the card provided");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Topup voucher not present for country : {0}, clubcard : {1}", CountrySetting.country, testData.TopUpClubcard));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_30")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1")]
        [TestCategory("MyVoucher")]
        [TestCategory("P1_MyVoucher")]
        public void MyVoucher_PrintBonusVouchers()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                string type = objPrefService.GetPreference(testData.TopUpClubcard.Clubcard, CountrySetting.culture);
                if (type.Equals(Enums.Preferences.XmasSaver.ToString()))
                {
                    if (objVoucherService.GetUnUsedVouchertype(testData.TopUpClubcard.Clubcard, Enums.VoucherType.Bonus.ToString()))
                    {
                        objLogin.Login_Verification(testData.TopUpClubcard.Clubcard, testData.TopUpClubcard.Password, testData.TopUpClubcard.EmailID);
                        objLogin.SecurityLayer_Verification(testData.TopUpClubcard.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                        objLogin.SecurityLayer_Verification(testData.TopUpClubcard.Clubcard);
                        objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.MYVOUCHER_SELECTONE, "vouchers");
                        objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                    }
                    else
                        customLogs.LogInformation("Check the type of the card provided");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Bonus voucher not present for country : {0}, clubcard : {1}", CountrySetting.country, testData.TopUpClubcard));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region P2

        [TestMethod]
        [Description("To Validate the text on voucher page")]
        [TestCategory("P2_Regression")]
        [TestCategory("TextValidation")]
        [TestCategory("Perk_Elixir_S2")]
        [TestCategory("P2")]
        [TestCategory("MyVoucher")]
        [TestCategory("P2_MyVoucher")]
        public void MyVoucher_TextValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objVoucher.TextValidation("VoucherPage");
            }
            else
            {
                Assert.Inconclusive(string.Format("My Voucher page is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
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
