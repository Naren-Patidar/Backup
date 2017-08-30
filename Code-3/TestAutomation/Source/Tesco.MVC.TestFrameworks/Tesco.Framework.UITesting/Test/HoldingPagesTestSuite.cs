using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.Common.Utilities;
using System.Diagnostics;
using System.Configuration;
using Tesco.Framework.UITesting.Constants;

using System.IO;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting;
using System.Data;
using Tesco.Framework.UITesting.Helpers;

namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class HoldingPagesTestSuite : Base
    {        
        ILogger customLogs = null;        
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyPoints objPoint = null;

        private static string beginMessage = "********************* Holding pages ****************************";
        private static string suiteName = "Holding pages";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_HoldingPages testData = null;


        static TestDataHelper<TestData_HoldingPages> HoldingPageTestDataHelper = new TestDataHelper<TestData_HoldingPages>();


        static string culture;

        // CustomerServiceAdaptor customerServiceAdptr = null;
        private Dictionary<string, string> expectedStampName;

        public HoldingPagesTestSuite()
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

            HoldingPageTestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_HoldingPages).Name, SanityConfiguration.Domain);
            testData = HoldingPageTestDataHelper.TestData;


            //XmasTestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_XmusSaver).Name, SanityConfiguration.Domain);
            //xmasTestData = XmasTestDataHelper.TestData;

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
            objPoint = new MyPoints(ObjAutomationHelper, SanityConfiguration);

            //customerServiceAdptr = new CustomerServiceAdaptor();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("To Verify Boost at Tesco Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("BoostHolding")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_Boost()
        {            
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isEnabled)
            {
                objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "BoostatTesco");
                objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                ValidateHoldingPage(SanityConfiguration.HTMLFiles.BOOSTHOLDING_HTML, ControlKeys.HTML_BOOST_HOLDINGPAGE);                                
            }
            else
            {
                Assert.Inconclusive(string.Format("Boost page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify XmasSaver Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("XmasSaverHolding")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_XmasSaver()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmasSaverAccount.Clubcard, testData.XmasSaverAccount.Password, testData.XmasSaverAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmasSaverAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    objLogin.SecurityLayer_Verification(testData.XmasSaverAccount.Clubcard);
                    ValidateHoldingPage(SanityConfiguration.HTMLFiles.XMASHOLDING_HTML, ControlKeys.HTML_XMASSAVER_HOLDINGPAGE);
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Voucher Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("VoucherHoldingPage")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_MyVoucher()
        {            
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE);
            if (isEnabled)
            {
                objLogin.Login_Verification(testData.VoucherAccount.Clubcard, testData.VoucherAccount.Password, testData.VoucherAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VoucherAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.VoucherAccount.Clubcard);
                ValidateHoldingPage(SanityConfiguration.HTMLFiles.VOUCHERHOLDING_HTML, ControlKeys.HTML_VOUCHER_HOLDINGPAGE);                
            }
            else
            {
                Assert.Inconclusive(string.Format("Voucher page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify Coupons Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("CouponsHoldingPage")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_MyCoupon()
        {
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            DateTime showMyCouponValue = DateTime.Parse(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ShowMyCouponPage, DBConfigKeys.SHOWMYCOUPON, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
            if (isEnabled)
            {
                if (showMyCouponValue.Date > DateTime.Now.Date)
                {
                    string errorMessage = string.Empty;
                    objLogin.Login_Verification(testData.CouponsAccount.Clubcard, testData.CouponsAccount.Password, testData.CouponsAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                    objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                    ValidateHoldingPage(SanityConfiguration.HTMLFiles.COUPONHOLDING_HTML, ControlKeys.HTML_COUPONS_HOLDINGPAGE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Coupon Holding page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify My Latest statment Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("MLSHoldingPage")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_MLS()
        {
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isEnabled)
            {
                string errorMessage = string.Empty;
                objLogin.Login_Verification(testData.CouponsAccount.Clubcard, testData.CouponsAccount.Password, testData.CouponsAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                ValidateHoldingPage(SanityConfiguration.HTMLFiles.MLSHOLDING_HTML, ControlKeys.HTML_MLS_HOLDINGPAGE);
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Verify My Points Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("MyPointsHoldingPage")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_MyPoints()
        {
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            string isPointSummaryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            if (isEnabled)
            {
                if (isPointSummaryEnabled.Equals("0"))
                {
                    string errorMessage = string.Empty;
                    objLogin.Login_Verification(testData.CouponsAccount.Clubcard, testData.CouponsAccount.Password, testData.CouponsAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYPOINTS_LINK, ControlKeys.MYPOINTS_CLICK, "points");
                    objLogin.SecurityLayer_Verification(testData.CouponsAccount.Clubcard);
                    objPoint.PointSummary_CurrentMinusOne_click();
                    ValidateHoldingPage(SanityConfiguration.HTMLFiles.POINTSHOLDING_HTML, ControlKeys.HTML_POINTS_HOLDINGPAGE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Points holding page is not applicable for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To Verify Order replacement Holding page")]
        [TestCategory("P2_Regression")]
        [TestCategory("MLSHoldingPage")]
        [TestCategory("Reese_Elixir_S1")]
        [TestCategory("P2")]
        [TestCategory("HoldingPage")]
        public void HoldingPage_OrderReplacement()
        {
            bool isEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isEnabled)
            {
                string errorMessage = string.Empty;
                objLogin.Login_Verification(testData.OrderReplacementAccount.Clubcard, testData.OrderReplacementAccount.Password, testData.OrderReplacementAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.OrderReplacementAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                objLogin.SecurityLayer_Verification(testData.OrderReplacementAccount.Clubcard);
                ValidateHoldingPage(SanityConfiguration.HTMLFiles.ORDERREPLACEMNTHOLDING_HTML, ControlKeys.ORDERREPLACEMENT_ORDERINPROCESSMSG);                
            }
            else
            {
                Assert.Inconclusive(string.Format("Order Replacement page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        public void ValidateHoldingPage(string holdingPageHTML, string controlKey)
        {
            string htmlFile = Path.Combine(SanityConfiguration.HtmlDataDirectory, holdingPageHTML);
            string content = Encoding.ASCII.GetString(File.ReadAllBytes(htmlFile));
            objGeneric.ValidateHtml(htmlFile, controlKey);

        }

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
