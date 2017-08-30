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
        static TestData_Voucher testData_Voucher = null;
        static TestDataHelper<TestData_Voucher> TestDataHelper_Voucher = new TestDataHelper<TestData_Voucher>();
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
            TestDataHelper_Voucher.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_Voucher).Name, SanityConfiguration.Domain);
            testData_Voucher = TestDataHelper_Voucher.TestData;
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

        [TestMethod]
        [Description("To Click on My Vouchers Tab And Verify the Title")]
        [Owner("Infosys")]
        [TestCategory("Sanity")]
        public void MyVoucher_ClickAndVerifyTitle()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYVOUCHER, "vouchers", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
               // objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the stamp functionality for Voucher page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
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
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.VOUCHERS);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Voucher page", StampName.VOUCHERS);

                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                  //  objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYVOUCHER, "My Voucher", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }
      
        [TestMethod]
        [Description("To Click on My Vouchers stamp And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyVoucher_ClickAndVerifyTitle_Stamp()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();            
            if (expectedStampName.ContainsValue(StampName.VOUCHERS))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.VOUCHERS)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                   // objGeneric.ClickElement(ControlKeys.STAMP3, "My Voucher");
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Voucher", StampName.VOUCHERS);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYVOUCHER, "My Voucher", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                   //objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To check print voucher is working")]
        [TestCategory("BasicFunctionality")]
        [Priority(0)]
        public void MyVoucher_PrintAll()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.verifyPageName(LabelKey.MYVOUCHER, "vouchers", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            //objLogin.LogOut_Verification();
            objGeneric.ClickElement(ControlKeys.MYVOUCHER_SelectAll, "vouchers");
            objGeneric.ClickElement(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();
            customLogs.LogInformation(endMessage);          
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_27")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression")]
        public void MyVoucher_PrintActiveVoucher()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objGeneric.ClickElement(ControlKeys.MYVOUCHER_SELECTONE, "vouchers");
                objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have active vouchers");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_28")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression")]
        public void MyVoucher_PrintAllctiveVouchers()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objGeneric.ClickElement(ControlKeys.MYVOUCHER_SelectAll, "vouchers");
                objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have active vouchers");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_29")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression")]
        public void MyVoucher_PrintTopUpVouchers()
        {
           string type = objPrefService.GetPreference(testData_Voucher.TopUpClubcard, CountrySetting.culture);
           if (type.Equals(Enums.Preferences.XmasSaver))
           {
               if (objVoucherService.GetUnUsedVouchertype(testData_Voucher.TopUpClubcard, Enums.VoucherType.TopUp.ToString()))
               {
                   objLogin.Login_Verification(testData_Voucher.TopUpClubcard, testData_Voucher.Password, "");
                   objLogin.SecurityLayer_Verification(testData_Voucher.TopUpClubcard);
                   objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                   objLogin.SecurityLayer_Verification(testData_Voucher.TopUpClubcard);
                   objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                   objGeneric.ClickElement(ControlKeys.MYVOUCHER_SELECTONE, "vouchers");
                   objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
               }
               else
                   customLogs.LogInformation("Check the type of the card provided");
           }
           else
               customLogs.LogInformation("Provided clubcard has not opted in for christmas saver");
               customLogs.LogInformation(endMessage);           
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_30")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P1_Regression")]
        public void MyVoucher_PrintBonusVouchers()
        {
            string type = objPrefService.GetPreference(testData_Voucher.BonusClubCard, CountrySetting.culture);
            if (type.Equals(Enums.Preferences.XmasSaver))
            {
               if (objVoucherService.GetUnUsedVouchertype(testData_Voucher.BonusClubCard, Enums.VoucherType.Bonus.ToString()))
               {
                   objLogin.Login_Verification(testData_Voucher.BonusClubCard, testData_Voucher.Password, "");
                   objLogin.SecurityLayer_Verification(testData_Voucher.BonusClubCard);
                   objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                   objLogin.SecurityLayer_Verification(testData_Voucher.BonusClubCard);
                   objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                   objGeneric.ClickElement(ControlKeys.MYVOUCHER_SELECTONE, "vouchers");
                   objVoucher.ClickElement_Print(ControlKeys.MYVOUCHER_PrintVoucher, "vouchers");
               }
               else
                   customLogs.LogInformation("Check the type of the card provided");
           }else
               customLogs.LogInformation("Provided clubcard has not opted in for christmas saver");
               customLogs.LogInformation(endMessage);           
           }
        
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_39")]
        [TestCategory("Regression_voucher_p1")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VoucherOnSocialSite1()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isFacebookRequired = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isTwitterRequired = webConfig1.Value;

            if (isFacebookRequired == "Y" && isTwitterRequired == "Y")
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE , ControlKeys.MYVOUCHER_SOCIALSITE ,"voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.CheckImagePresent(Enums.VoucherSection.BothEnabled);
            }
            else 
            {
               Assert.AreEqual(isFacebookRequired, "N", "Configuration Value not matched with WebConfig");
               Assert.AreEqual(isTwitterRequired, "N", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_40")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VoucherOnSocialSite2()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isFacebookRequired = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isTwitterRequired = webConfig1.Value;

            if (isFacebookRequired == "N" && isTwitterRequired == "Y")
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE, ControlKeys.MYVOUCHER_SOCIALSITE, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.CheckImagePresent(Enums.VoucherSection.TwitterEnabled);
            }
            else
            {
                Assert.AreEqual(isFacebookRequired, "Y", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isTwitterRequired, "N", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_41")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VoucherOnSocialSite3()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isFacebookRequired = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isTwitterRequired = webConfig1.Value;

            if (isFacebookRequired == "Y" && isTwitterRequired == "N")
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.SOCIALSITE, ControlKeys.MYVOUCHER_SOCIALSITE, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.CheckImagePresent(Enums.VoucherSection.FacebookEnabled);
            }
            else
            {
                Assert.AreEqual(isFacebookRequired, "N", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isTwitterRequired, "Y", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_42")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VoucherOnSocialSite4()
        {
           // WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISFACEBOOKREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isFacebookRequired = objGeneric.verifyPageEnabledByWebConfig(WebConfigKeys.ISFACEBOOKREQUIRED);

            //WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISTWITTERREQUIRED, SanityConfiguration.WebConfigurationFile);
            string isTwitterRequired = objGeneric.verifyPageEnabledByWebConfig(WebConfigKeys.ISTWITTERREQUIRED);

            if (isFacebookRequired == "N" && isTwitterRequired == "N")
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objVoucher.CheckImagePresent(Enums.VoucherSection.BothDisabled);
            }
            else
            {
                Assert.AreEqual(isFacebookRequired, "Y", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isTwitterRequired, "Y", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_14")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_Verify3Section()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objVoucher.VerifySection(Enums.VoucherSection.Displayed,ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.Displayed,ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.Displayed,ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_15")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyActiveVoucherSection()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.UNUSEDVOUCHER, ControlKeys.MYVOUCHER_LBLUNSEDVOUCHER, "voucher", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objGeneric.VerifyTextExistOnPage(ControlKeys.MYVOUCHER_TABLEUNUSED);
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_16")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyTotalActiveVoucherCount()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objVoucher.VerifyTotalCount(Enums.VoucherSection.UnUsed , LabelKey.CURRENCYSYMBOL , SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_17 & MCA_SCN_UK_005_TC_18")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyAviosSection()
        {
            string type = objPrefService.GetPreference(testData_Voucher.AviosClubcard, CountrySetting.culture);
            if (type.Equals(Enums.Preferences.Avios.ToString()))
            {
                objLogin.Login_Verification(testData_Voucher.AviosClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.AviosClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.AviosClubcard);                  
                objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.Avios);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);                                 
            }
            else
                customLogs.LogInformation("Not applicable for the country " + CountrySetting.country);
                customLogs.LogInformation(endMessage);  
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_19 & MCA_SCN_UK_005_TC_20")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyBAAviosSection()
        {
            string type = objPrefService.GetPreference(testData_Voucher.BAAviosClubcard, CountrySetting.culture);
            if (type.Equals(Enums.Preferences.BAAvios.ToString()))
            {
                objLogin.Login_Verification(testData_Voucher.BAAviosClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.BAAviosClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.BAAviosClubcard);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.BAAvios);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);               
            }
            else
                customLogs.LogInformation("Not applicable for the country " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_21 & MCA_SCN_UK_005_TC_22")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyVirginAtlanticSection()
        {
            string type = objPrefService.GetPreference(testData_Voucher.VirginAtlantic, CountrySetting.culture);
            if (type.Equals(Enums.Preferences.VirginAtlantic.ToString()))
            {
                objLogin.Login_Verification(testData_Voucher.VirginAtlantic, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.VirginAtlantic);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.VirginAtlantic);
                objGeneric.VerifyTextonthePageByXpath(LabelKey.WHICHHAVEBEENCONVERTED, ControlKeys.MYVOUCHER_TXTCONVERTEDTO, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                objVoucher.VerifyOptedPreferenceSection(Enums.Preferences.VirginAtlantic);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX1, LabelKey.POINTBOX1TEXT, ControlKeys.MYVOUCHER_POINTBOX1TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX2, LabelKey.POINTBOX2TEXT, ControlKeys.MYVOUCHER_POINTBOX2TEXT);
                objVoucher.VerifySection(Enums.VoucherSection.NotDisplayed, ControlKeys.MYVOUCHER_POINTBOX3, LabelKey.POINTBOX3TEXT, ControlKeys.MYVOUCHER_POINTBOX3TEXT);               
            }
            else
                customLogs.LogInformation("Not applicable for the country " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_23")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_PrintMyVoucherIsDisplayed()
        {
            if (objVoucherService.GetUnUsedVoucher(testData_Voucher.ActiveClubcard))
            {
                objLogin.Login_Verification(testData_Voucher.ActiveClubcard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.ActiveClubcard);
                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_PrintVoucher).Id), driver))
                    customLogs.LogInformation("Print My Voucher Button Present");
                else
                    Assert.Fail("Print My Voucher Button Not Present");               
            }
            else
                Assert.Fail("Clubcard does not have active vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_24")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_VerifyUsedSectionForRedeemed()
        {
            if (objVoucherService.GetUsedVoucher(testData_Voucher.RedeemedClubCard))
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.VerifyTextonthePageByXpath( LabelKey.USEDVOUCHER , ControlKeys.MYVOUCHER_LBLUSEDVOUCHER , "vouchers" , SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);              
                objGeneric.VerifyTextExistOnPage(ControlKeys.MYVOUCHER_TABLEREDEEMED);
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_25")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_TotalOfRedeemedVouchers()
        {
            if (objVoucherService.GetUsedVoucher(testData_Voucher.RedeemedClubCard))
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objVoucher.VerifyTotalCount(Enums.VoucherSection.Used, LabelKey.CURRENCYSYMBOL, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                customLogs.LogInformation(endMessage);
            }
            else
                Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_26")]
        [TestCategory("Regression_voucher_p0")]
        [TestCategory("P0_Regression")]
        public void MyVoucher_RedeemedVouchersPrintDisabled()
        {
            if (objVoucherService.GetUsedVoucher(testData_Voucher.RedeemedClubCard))
            {
                objLogin.Login_Verification(testData_Voucher.RedeemedClubCard, testData_Voucher.Password, "");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData_Voucher.RedeemedClubCard);
                if (!objGeneric.VerifyText_Contains(LabelKey.PRINTMYVOUCHERTEXT, ControlKeys.MYVOUCHER_PrintVoucher, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE))
                    customLogs.LogInformation("Print My voucher is not present under Reddemed Voucher Section");
                else
                {
                    customLogs.LogInformation("Print My voucher is present under Reddemed Voucher Section");
                    Assert.Fail("Print My voucher is present under Reddemed Voucher Section");
                }                
            }
            else
                Assert.Fail("Clubcard does not have redeemed vouchers, Kindly login with another card");
            customLogs.LogInformation(endMessage);
        }
        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }
}
