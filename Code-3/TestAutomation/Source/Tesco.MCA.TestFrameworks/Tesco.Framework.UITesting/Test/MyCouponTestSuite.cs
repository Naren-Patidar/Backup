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
using Tesco.Framework.UITesting.Services;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class MyCouponTestSuite : Base
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        static string culture;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyCoupon objcoupon = null;
        static TestData_AccountDetails testData = null;
        //static TestData_MyCoupon testData_coupon = null;
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();
        //static TestDataHelper<TestData_MyCoupon> TestDataHelper_coupon = new TestDataHelper<TestData_MyCoupon>();

        private static string beginMessage = "********************* My Coupon Test Suite ****************************";
        private static string suiteName = "My Coupon ";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        public MyCouponTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
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

            // SanityConfiguration.LocalResourceFile = Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName);
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
            objcoupon = new MyCoupon(objAutomationHelper);

        }

        [TestMethod]
        [Description("To Click on My Point Tab And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyCoupon_ClickAndVerifyTitle()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To select one coupon and click on Print coupon button")]
        [TestCategory("BasicFunctionality")]
        [Priority(0)]
        public void MyCoupon_PrintCoupon()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objGeneric.ClickElement(ControlKeys.MYCOUPON_SelectCoupon, "mycoupons");
            objGeneric.ClickElement(ControlKeys.MYCOUPON_PrintButton, "mycoupons");
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();
            customLogs.LogInformation(endMessage);

        }



        [TestMethod]
        [Description("To validate the stamp functionality for Coupon page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void StampHomepage_MyCoupon()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.MYCOUPON))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.MYCOUPON)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_COUPON, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.MYCOUPON);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Coupon", StampName.MYCOUPON);

                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                   // objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS, "My Coupon", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("To Click on Coupon stamp And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyCoupon_ClickAndVerifyTitle_Stamp()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.MYCOUPON))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.MYCOUPON)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Coupon", StampName.MYCOUPON);
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS, "My Coupon", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    //  objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }






        [TestMethod]
        [Description("To validate Available Coupon in Coupon Summary page")]
        [TestCategory("P0_Regression")]
        public void MyCoupon_Summary_AvailableToUse()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            //objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.AvailableCoupon(testData.ClubcardforDate);

            Resource res = AutomationHelper.GetResourceMessage(ValidationKey.AVAILABLECOUPON_SUMMARY1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.AVAILABLECOUPON_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            var expectedMessage = res.Value;
            var expectedMessage1 = res1.Value;

            objcoupon.ValidateTextCoupon((expectedMessage1 + "\r\n" + expectedMessage), ControlKeys.MYCOUPON_AVAILABLETEXT, "CouponPage");


        }




        [TestMethod]
        [Description("To validate Used Coupon grid in Coupon Summary page")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        public void MyCoupon_Summary_CouponUsed()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            //objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.UsedCoupon(testData.ClubcardforDate);

            Resource res = AutomationHelper.GetResourceMessage(ValidationKey.USEDCOUPON4WEEKS_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.USEDCOUPON4WEEKS_SUMMARY1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            var expectedMessage = res.Value.Trim();
            var expectedMessage1 = res1.Value.Trim();

            objcoupon.ValidateTextCoupon((expectedMessage + "\r\n" + expectedMessage1), ControlKeys.MYCOUPON_USEDSUMMARYTEXT, "CouponPage");

        }

        [TestMethod]
        [Description("To validate Sent Coupon grid in Coupon Summary page")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        public void MyCoupon_Summary_CouponSent()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            //objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.TotalCoupon(testData.ClubcardforDate);

            Resource res = AutomationHelper.GetResourceMessage(ValidationKey.COUPONSENT_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.COUPONSENT_SUMMARY1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE));
            var expectedMessage = res.Value;
            var expectedMessage1 = res1.Value;

            objcoupon.ValidateTextCoupon((expectedMessage + "\r\n" + expectedMessage1), ControlKeys.MYCOUPON_SENTSUMMARYTEXT, "CouponPage");

        }



        [TestMethod]
        [Description("To validate Coupon redeemed date")]
        [TestCategory("1607")]
        public void MyCoupon_DateRedeemed()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.DateRedeemed(testData.ClubcardforDate);
            //objLogin.LogOut_Verification();
        }

        [TestMethod]
        [Description("To validate Coupon redeemed Store")]
        [TestCategory("1607")]
        public void MyCoupon_StoreName()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.StoreName(testData.ClubcardforDate);
            //objLogin.LogOut_Verification();
        }

        //[TestMethod]
        //[Description("To validate Coupon redeemed date")]
        //[TestCategory("P0")]
        //[TestCategory("P0_Regression_DateRedeemed")]
        //public void MyCoupon_CouponsRedeemedOnline()
        //{
        //    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
        //    objLogin.SecurityLayer_Verification(testData.Clubcard);
        //    objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
        //    objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
        //    objcoupon.CouponsRedeemedOnline();
        //}

        [TestMethod]
        [Description("To validate Active coupons")]
        [TestCategory("1607")]
        public void MyCoupon_NoActiveCoupons()
        {
            objLogin.Login_Verification(testData.NoActiveCouponClubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.NoActiveCouponClubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.NoActiveCouponPresent(ValidationKey.NOACTIVECOUPON_PRESENT, "My Coupon", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            //objLogin.LogOut_Verification();
        }

        [TestMethod]
        [Description("To validate Coupon redeemed date")]
        [TestCategory("1607")]
        public void MyCoupons_NoRedeemedCouponsIn4Weeks()
        {
            objLogin.Login_Verification(testData.NoActiveCouponClubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.NoActiveCouponClubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.NoRedeemedCouponsIn4Weeks(ValidationKey.NOREDEEMEDCOUPON_4WEEKS, "My Coupon", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            //objLogin.LogOut_Verification();
        }


        [TestMethod]
        [Description("To Click on My Point Tab And Verify the Title, when household ID is greater than 2,147,483,647")]
        [TestCategory("1607")]
        public void MyCoupon_VerifyPageFunctionalityforHouseholdID()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("HouseHold ID is not implemented for countries other then UK", TraceEventType.Start);
            }
            else
            {

                string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEECOUPONPAGE);
                if (isPresent == "Y")
                {
                    objLogin.Login_Verification(testData.ClubcardforHouseholdID, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardforHouseholdID);
                    objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    //objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To validate Coupon redeemed date Format")]
        [TestCategory("personal")]
        public void MyCoupon_DateFormat()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.DateFormat(testData.ClubcardforDate);
            //objLogin.LogOut_Verification();
        }

        [TestMethod]
        [Description("Twithout selecting coupon and click on Print coupon button")]
        [TestCategory("personal")]
        [Priority(0)]
        public void MyCoupon_PrintCouponWithoutSelectingCoupon()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
            Int32 couponC = 0;
            int availableCount = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out couponC);
            if (availableCount == 0)
            {
                CustomLogs.LogMessage("no available coupon", TraceEventType.Start);
            }
            else
            {
                objGeneric.ClickElement(ControlKeys.MYCOUPON_PrintButton, "mycoupons");
                objcoupon.SelectCouponMessage(ValidationKey.SELECTCOUPONMESSAGE, "My Coupons", SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
            }
        }

        [TestMethod]
        [Description("To select one coupon and click on Print coupon button")]
        [TestCategory("personal")]
        [Priority(0)]
        public void MyCoupon_PrintCouponSelectingAll()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objGeneric.ClickElement(ControlKeys.MYCOUPON_SelectAllCoupon, "mycoupons");
            objGeneric.ClickElement(ControlKeys.MYCOUPON_PrintButton, "mycoupons");
            //IAlert alert = driver.SwitchTo().Alert();
            //alert.Dismiss();
            //customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("To Print coupon button")]
        [TestCategory("personal")]

        public void MyCoupon_PrintCouponIcon()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            // objGeneric.ClickElement(ControlKeys.MYCOUPON_SelectCoupon, "mycoupons");
            Generic.IsElementPresent(By.XPath(objAutomationHelper.GetControl(ControlKeys.MYCOUPON_PrintIcon).XPath), driver);
        }


        [TestMethod]
        [Description("To validate Coupon redeemed Store")]
        [TestCategory("personal")]
        public void MyCoupon_UsedUnusedCount()
        {
            objLogin.Login_Verification(testData.ClubcardforDate, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardforDate);
            objGeneric.linkNavigate(LabelKey.MYCOUPONS, ControlKeys.MYVOUCHER_CLICK, "mycoupons");
            objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            objcoupon.couponCount(testData.ClubcardforDate);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }

    }
}
