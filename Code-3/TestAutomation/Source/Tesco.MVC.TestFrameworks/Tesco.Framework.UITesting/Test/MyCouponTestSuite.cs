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


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class MyCouponTestSuite : Base
    {
        #region Fields

        ILogger customLogs = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        private Dictionary<string, string> expectedStampName;
        static string culture;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyCoupon objcoupon = null;
        static TestData_AccountDetails testData = null;        
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();        

        private static string beginMessage = "********************* My Coupon Test Suite ****************************";
        private static string suiteName = "My Coupon ";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        #endregion        

        public MyCouponTestSuite()
        {
            ObjAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.COUPONTESTSUITE);
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
            objcoupon = new MyCoupon(ObjAutomationHelper);

        }

        #region Sanity

        [TestMethod]
        [Description("To Click on My Point Tab And Verify the Title")]
        [TestCategory("Sanity")]
        [TestCategory("MVC")]
        [TestCategory("3435-TH")]
        [TestCategory("LeftNavigation")]
        [TestCategory("MyCoupon")]
        public void LeftNavigation_ValidatePageTitle_Coupon()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region BasicFunctionality

        [TestMethod]
        [Description("To select one coupon and click on Print coupon button")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("MVC")]
        [TestCategory("3435-TH")]
        [TestCategory("MyCoupon")]
        [Priority(0)]
        public void MyCoupon_PrintCoupon()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            DateTime showMyCouponValue = DateTime.Parse(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ShowMyCouponPage, DBConfigKeys.SHOWMYCOUPON, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
            if (isPresent)
            {
                if (showMyCouponValue.Date < DateTime.Now.Date)
                {
                    objLogin.Login_Verification(testData.ActiveCouponAccount.Clubcard, testData.ActiveCouponAccount.Password, testData.ActiveCouponAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ActiveCouponAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                    objLogin.SecurityLayer_Verification(testData.ActiveCouponAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                    int availableCoupons = objcoupon.GetAvailableCouponCount();
                    if (availableCoupons > 0)
                    {
                        objGeneric.ClickElement(ControlKeys.MYCOUPON_SelectCoupon, FindBy.CSS_SELECTOR_ID);
                        objGeneric.ClickElement(ControlKeys.MYCOUPON_PrintButton, FindBy.CSS_SELECTOR_ID);
                    }
                    else
                    {
                        Assert.Fail(string.Format("Customer (customer ID : '{0}', clubcard : '{1}') does not have available coupons", Login.CustomerID, testData.MainAccount));
                    }
                }
                else
                {
                    Assert.Fail(string.Format("Available coupon not available, Coupon holding page is displayed for customer ID : '{0}', clubcard : '{1}'", Login.CustomerID, testData.MainAccount));
                }
                customLogs.LogInformation(endMessage);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        #endregion

        #region P0

        [TestMethod]
        [Description("To validate the stamp functionality for Coupon page")]
        public void MyCoupon_StampHomepage()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.MYCOUPON))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.MYCOUPON)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_COUPON, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.MYCOUPON);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Coupon", StampName.MYCOUPON);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);               
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
        }

        [TestMethod]
        [Description("To validate Available Coupon in Coupon Summary page")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_Summary_AvailableToUse()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ActiveCouponAccount.Clubcard, testData.ActiveCouponAccount.Password, testData.ActiveCouponAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ActiveCouponAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.ActiveCouponAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.AvailableCoupon(testData.ActiveCouponAccount.Clubcard);
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.AVAILABLECOUPON_SUMMARY1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
                var expectedMessage = res.Value;
                objcoupon.ValidateTextCoupon(expectedMessage, ControlKeys.MYCOUPON_AVAILABLETEXT, "CouponPage");
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To validate Used Coupon grid in Coupon Summary page")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_Summary_CouponUsed()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                //var x= driver.FindElement(By.XPath("//*[@id='page-body']/div/div/div[2]/div[1]/div[2]/dl/dd/span"));
                objcoupon.UsedCoupon(testData.MainAccount.Clubcard);
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.USEDCOUPON4WEEKS_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
                var expectedMessage = res.Value.Trim();
                objcoupon.ValidateTextCoupon(expectedMessage, ControlKeys.MYCOUPON_USEDSUMMARYTEXT, "CouponPage");
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To validate Sent Coupon grid in Coupon Summary page")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_Summary_CouponSent()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.TotalCoupon(testData.MainAccount.Clubcard);
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.COUPONSENT_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
                var expectedMessage = res.Value;
                objcoupon.ValidateTextCoupon(expectedMessage, ControlKeys.MYCOUPON_SENTSUMMARYTEXT, "CouponPage");
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }



        [TestMethod]
        [Description("To validate Coupon redeemed date")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_DateRedeemed()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.DateRedeemed(testData.MainAccount.Clubcard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To validate Coupon redeemed Store")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_StoreName()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.StoreName(testData.MainAccount.Clubcard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }


        [TestMethod]
        [Description("To validate Active coupons")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        public void MyCoupon_NoActiveCoupons()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.NoActiveCouponAccount.Clubcard, testData.NoActiveCouponAccount.Password, testData.NoActiveCouponAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.NoActiveCouponAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.NoActiveCouponAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.NoActiveCouponPresent(ValidationKey.NOACTIVECOUPON_PRESENT, "My Coupon", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To validate Coupon redeemed date")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        public void MyCoupon_NoRedeemedCouponsIn4Weeks()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.NoActiveCouponAccount.Clubcard, testData.NoActiveCouponAccount.Password, testData.NoActiveCouponAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.NoActiveCouponAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.NoActiveCouponAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.NoRedeemedCouponsIn4Weeks(ValidationKey.NOREDEEMEDCOUPON_4WEEKS, "My Coupon", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }


        

        [TestMethod]
        [Description("To validate Coupon redeemed date Format")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_DateFormat()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.DateFormat(testData.MainAccount.Clubcard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Twithout selecting coupon and click on Print coupon button")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        [Priority(0)]
        public void MyCoupon_PrintCouponWithoutSelectingCoupon()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                Int32 couponC = 0;
                int availableCount = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out couponC);
                if (availableCount == 0)
                {
                    Assert.Fail(string.Format("No coupons avaliable for clubcard : '{0}', Email : '{1}'", testData.MainAccount.Clubcard, testData.MainAccount.EmailID));
                }
                else
                {
                    objGeneric.ClickElementJavaElement(ControlKeys.MYCOUPON_PrintButton, "mycoupons");
                    objcoupon.SelectCouponMessage(ValidationKey.SELECTCOUPONMESSAGE, "My Coupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To select one coupon and click on Print coupon button")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        [Priority(0)]
        public void MyCoupon_PrintCouponSelectingAll()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objGeneric.ClickElementJavaElement(ControlKeys.MYCOUPON_SelectAllCoupon, "mycoupons");
                objGeneric.ClickElementJavaElement(ControlKeys.MYCOUPON_PrintButton, "mycoupons");
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To Print coupon button")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression_MVC")]
        [TestCategory("MyCoupon")]
        [TestCategory("MVC")]
        [TestCategory("P0Set1")]
        public void MyCoupon_PrintCouponIcon()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objGeneric.ClickElement(ControlKeys.MYCOUPON_PrintIcon, FindBy.CSS_SELECTOR_ID);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }


        [TestMethod]
        [Description("To validate Coupon redeemed Store")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]

        public void MyCoupon_UsedUnusedCount()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.couponCount(testData.MainAccount.Clubcard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        #endregion

        #region P1

        [TestMethod]
        [Description("To Click on My Point Tab And Verify the Title, when household ID is greater than 2,147,483,647")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_MyCoupon")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCoupon")]
        [TestCategory("P0Set1")]
        public void MyCoupon_VerifyPageFunctionalityforHouseholdID()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("HouseHold ID is not implemented for countries other then UK", TraceEventType.Start);
            }
            else
            {
                bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
                if (isPresent)
                {
                    objLogin.Login_Verification(testData.HouseholdAccount.Clubcard, testData.HouseholdAccount.Password, testData.HouseholdAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.HouseholdAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                    objLogin.SecurityLayer_Verification(testData.HouseholdAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                customLogs.LogInformation(endMessage);
            }
        }

        #endregion

        #region P2

        [TestMethod]
        [Description("To validate Coupon page text")]
        [TestCategory("P2")]
        [TestCategory("3435-TH")]
        [TestCategory("P2_MyCoupon")]
        [TestCategory("P2_Regression")]
        [TestCategory("TextValidation")]
        [TestCategory("Perk_Elixir_S2")]
        [TestCategory("MyCoupon")]
        public void MyCoupon_TextValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                string errorMessage = string.Empty;
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.TextValidation("CouponPage");
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }


        [TestMethod]
        [Description("To validate text and click functionality above available Coupon")]
        [TestCategory("P2_MyCoupon")]
        [TestCategory("P2_Regression")]
        [TestCategory("MyCoupon")]
        public void MyCoupon_ValidateCouponHeaderLabel()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ActiveCouponAccount.Clubcard, testData.ActiveCouponAccount.Password, testData.ActiveCouponAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ActiveCouponAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYCOUPONS_TITLE, "mycoupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                objcoupon.ValidateCouponHeaderLabel();
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupon Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
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
