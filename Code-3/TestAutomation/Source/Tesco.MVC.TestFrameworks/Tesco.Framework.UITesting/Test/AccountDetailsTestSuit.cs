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
    public class AccountDetailsTestSuit
    {

        public IWebDriver driver;
        public IWebDriver jse;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyContactPreference objMyContactPreference = null;
        MyAccountDetails objAccountDetails = null;

        private static string beginMessage = "********************* Account Details ****************************";
        private static string suiteName = "Account Details";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public AccountDetailsTestSuit()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.ACCOUNTDETAILSSUITE);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
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

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
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
            objAccountDetails = new MyAccountDetails(objAutomationHelper);
            objMyContactPreference = new MyContactPreference(objAutomationHelper);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        #region Sanity

        [TestMethod]
        [Description("To Click on Personal Details and verify the title")]
        [Owner("Infosys")]
        [TestCategory("3435-TH")]
        [TestCategory("Sanity")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_PD()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILSNEW, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        //[TestMethod]
        //[Description("To Click on Tesco Boost and verify the title")]
        //[Owner("Infosys")]
        //[TestCategory("Sanity")]
        //[TestCategory("MVC")]
        //[TestCategory("LeftNavigation")]
        //public void LeftNavigation_ValidatePageTitle_Boost()
        //{
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
        //    if (isPresent)
        //    {
        //        objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
        //        objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
        //        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
        //        objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "Boosts at Tesco");
        //        objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
        //        objGeneric.verifyPageName(LabelKey.TESCOBOOST, "Boosts at Tesco", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //    customLogs.LogInformation(endMessage);
        //    driver.Quit();
        //}

        [TestMethod]
        [Description("To Click on View My Card and verify the title")]
        [Owner("infosys")]
        [TestCategory("Sanity")]
        [TestCategory("3435-TH")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_ViewCard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        #endregion

        #region Stamps


        [TestMethod]
        [Description("To validate the stamp functionality in Personal Details page")]
        public void StampHomepage_PersonalDetails()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.PERSONALDETAILS))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.PERSONALDETAILS)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_MYPERSONALDETAILS, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampNumber, StampName.PERSONALDETAILS);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Personal Details", StampName.PERSONALDETAILS);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objAutomationHelper.WebDriver.Quit();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            driver.Quit();
        }

        [TestMethod]
        [Description("To validate the stamp functionality in ORderReplacement page")]
        public void StampHomepage_OrderReplacement()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.ORDERAREPLACEMENT))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.ORDERAREPLACEMENT)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_ORDERREPLACEMENT, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.ORDERAREPLACEMENT);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Order Replacement", StampName.ORDERAREPLACEMENT);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENT, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAutomationHelper.WebDriver.Quit();

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

            driver.Quit();
        }


        [TestMethod]
        [Description("To validate the stamp functionality for Boost page")]
        public void StampHomepage_Boost()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.TESCOBOOST))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.TESCOBOOST)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_TESCOBOOST, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.BoostAccount.Clubcard, testData.BoostAccount.Password, testData.BoostAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.TESCOBOOST);

                    objGeneric.stampClick(ControlKeys.STAMP5, "TESCOBOOST", StampName.TESCOBOOST);
                    objLogin.SecurityLayer_Verification(testData.BoostAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.TESCOBOOST, "Boost At Tesco", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    //objLogin.LogOut_Verification();
                    objAutomationHelper.WebDriver.Quit();

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            driver.Quit();
        }


        [TestMethod]
        [Description("To validate the stamp functionality for Flying scheme")]
        public void StampHomepage_flyingscheme()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.OPTIONANDBENEFIT))
            {

                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.OPTIONANDBENEFIT)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_OPTIONBENEFIT, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.OPTIONANDBENEFIT);

                    objGeneric.stampClick(ControlKeys.STAMP5, "OPTIONANDBENEFIT", StampName.OPTIONANDBENEFIT);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "Option And Benefit", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                    objAutomationHelper.WebDriver.Quit();

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            driver.Quit();
        }


        [TestMethod]
        [Description("To Click on Contact Pref stamp and verify the title")]
        [TestCategory("3435-TH")]
        public void StampHomepage_Preference()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.PREFERENCE))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.PREFERENCE)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampNumber, StampName.PREFERENCE);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Option And Benefit", StampName.PREFERENCE);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.PREFERENCE, "Preference", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                    //objLogin.LogOut_Verification();
                    objAutomationHelper.WebDriver.Close();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
        }

        [TestMethod]
        [Description("To Click on View My Card stamp and verify the title")]
        [TestCategory("3435-TH")]
        public void StampHomepage_ViewMyCard()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.VIEWMYCARD))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.VIEWMYCARD)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampNumber, StampName.VIEWMYCARD);
                    objGeneric.stampClick(ControlKeys.STAMP5, "View My Card", StampName.VIEWMYCARD);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "View My Card", SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                    // objLogin.LogOut_Verification();
                    objAutomationHelper.WebDriver.Close();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
        }

        #endregion

        #region ContactPreferences

        [TestMethod]
        [Description("To verify Selected preference as Email")]
        [TestCategory("1609")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_Email()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ContactPreferences_Email();

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Selected preference as Email")]
        [TestCategory("1609")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_EmailText()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);

                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");

                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.MYCONTACTPREFERENCES);
                objMyContactPreference.ContactPreferences_EmailText(testData.MainAccount.Clubcard);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify mobile phone number")]
        [TestCategory("1609")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_phonenumber()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                if (CountrySetting.culture == "UK")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.MYCONTACTPREFERENCES);
                    objMyContactPreference.ContactPreferences_Phonenumber(testData.MainAccount.Clubcard);
                }
                else
                {
                    customLogs.LogMessage("Not aplicable for countries other then UK", TraceEventType.Start);
                }


            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify confirm mobile phone number should be blank at page load")]
        [TestCategory("1609")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_confirmPhoneNumber()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
               
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.MYCONTACTPREFERENCES);
                    objMyContactPreference.ContactPreferences_ConfirmPhonenumber(testData.MainAccount.Clubcard);              

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify confirm email should be blank at page load")]
        [TestCategory("1609")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_confirmEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {

                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.MYCONTACTPREFERENCES);
                objMyContactPreference.ContactPreferences_ConfirmEmail(testData.MainAccount.Clubcard);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }






        [TestMethod]
        [Description("To verify labels on my conatct preference page")]
        [TestCategory("3435-TH")]
        public void AccountDetails_ContactPreferences_Labels()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ContactPreferenceLabels(ValidationKey.CONTACTPREFERENCE_LABLEONE, ValidationKey.CONTACTPREFERENCE_LABLETWO, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preferences Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region ViewMyCards

        //As a part of re-skin there won't be any common salutation.
        [Description("To verify the common salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set1")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        public void ViewMyCard_Common_Salutation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Common_Salutation_verify(ValidationKey.VALIDATAIONNAMEOFACCOUNT.ToString(), ValidationKey.VALIDATIONANDSEPRATOR.ToString(), testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the main customer's salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        public void ViewMyCard_Main_Salutation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ClubcardNonStdTypeB.Clubcard, testData.ClubcardNonStdTypeB.Password, testData.ClubcardNonStdTypeB.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Main_Salutation_verify(ValidationKey.VALIDATIONMESSAGEFORMAIN.ToString(), ValidationKey.VALIDATIONMESSAGEFORASSO.ToString(), ValidationKey.VALIDATIONMESSAGEFORMAINCARDHOLDER.ToString(), testData.ClubcardNonStdTypeB.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the associate customer's salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("P0Set1")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        public void ViewMyCard_Asso_Salutation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Asso_Salutation_verify(ValidationKey.VALIDATIONMESSAGEFORASSO.ToString(), ValidationKey.VALIDATIONMESSAGEFORMAINCARDHOLDER.ToString(), testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the Associate customer's clubcards on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        public void ViewMyCard_Asso_Cards()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Asso_Cards_verify(testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }


        [TestMethod]
        [Description("To verify the Main customer's clubcards on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_Main_Cards()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {

                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Main_Cards_verify(testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To validate Show Type of card column in View My Cards for Main customer")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_ShowTypeofCard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
                string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
                if (showTypeofCard == "TRUE")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_ShowCardType_Verify(ValidationKey.VALIDATIONTYPEOFCARD.ToString(), testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(showTypeofCard, "FALSE", "Configuration Value not matched with WebConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To validate Show Type of card column in View My Cards for Associate customer")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_Asso_ShowTypeofCard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
                string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
                if (showTypeofCard == "TRUE")
                {

                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_Asso_ShowCardType_Verify(ValidationKey.VALIDATIONTYPEOFCARD.ToString(), testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(showTypeofCard, "FALSE", "Configuration Value not matched with WebConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the Main customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_Main_WhereUsed()
        {
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Main_WhereUsed_verify(showTypeofCard, ValidationKey.VALIDATIONWHEREUSED.ToString(), dateFormat, testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the Associate customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_Asso_WhereUsed()
        {
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Asso_WhereUsed_verify(showTypeofCard, ValidationKey.VALIDATIONWHEREUSED.ToString(), dateFormat, testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the Main customer's last used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_LastUsed_Main()
        {
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_LastUsed_Main_verify(showTypeofCard, dateFormat, testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify the Associate customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_ViewMyCard")]
        [TestCategory("ViewMyCard")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("P0Set1")]
        public void ViewMyCard_LastUsed_Asso()
        {
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_LastUsed_Asso_verify(showTypeofCard, dateFormat, testData.MainAccount.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }


        [TestMethod]
        [Description("To validate if lock is present on home page for vouchers value")]
        [TestCategory("Regression_home")]

        public void AccountDetails_Home_ViewBalanceOnVouchersbox()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
            string isSecurityEnable = config.ConfigurationValue1;
            if (isSecurityEnable == "FALSE")
            {
                // objLogin.Login_Verification(testData.BlockedClubcard, testData.Clubcard.Password, "");

                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_ViewVouchersHome).ClassName), driver))
                {
                    objGeneric.ClickElement(ControlKeys.HOME_ViewVouchersHome, "Home Page");
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
                    {
                        Assert.Fail("Home security page is not displayed");
                    }
                    else
                    {
                    }
                }
                else
                {
                    Assert.Fail("Lock is not present on home page");
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
        }

        [TestMethod]
        [Description("To check if the voucher value is present after providing the security digits")]
        [TestCategory("Regression_home")]

        public void AccountDetails_Home_VouchersValue()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
            string isSecurityEnable = config.ConfigurationValue1;
            if (isSecurityEnable == "FALSE")
            {


                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_ViewVouchersHome).Id), driver))
                {
                    objGeneric.ClickElement(ControlKeys.HOME_ViewVouchersHome, "Home Page");
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
                    {
                        Assert.Fail("Home security page is not displayed");
                    }
                    else
                    {
                        objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.HOME);
                        Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_VALUE).Id), driver);
                    }
                }
                else
                {
                    Assert.Fail("Lock is not present on home page");
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
        }


        [TestMethod]
        [Description("To Click on Home Page and verify the title")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidateWelcomeMessage_Home()
        {

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.ClubcardNonStdTypeB.Clubcard, testData.ClubcardNonStdTypeB.Password, testData.ClubcardNonStdTypeB.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardNonStdTypeB.Clubcard);
            objAccountDetails.VerifyWelcomeMessageName(SanityConfiguration.DbConfigurationFile, testData.ClubcardNonStdTypeB.Clubcard);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region Login Redirect with Query Stream

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_HomePage()
        {
            string requestedPage = string.Empty;
            string responsePage = string.Empty;
            driver.Navigate().GoToUrl(SanityConfiguration.MCAUrl);
            requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
            if (requestedPage != responsePage)
            {
                Assert.Fail("User provided Login Url and Landing page do not match");
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_PersonalDetails()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.PersonalDetailsUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_ContactPreferences()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.MyContactPreferencesUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preferences page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_OptionsandBenefits()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.OptionsBenefitsUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Options and Benefits page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_ViewMyCards()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.ViewMyCardsUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Manage Cards page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_OrderAReplacement()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.OrderAReplacementUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Order A Replacement page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        //[TestMethod]
        //[Description("To verify if the Login url is maintained till we land on the user provided link")]
        //[TestCategory("P1")]
        //[TestCategory("P1_LoginRedirect")]
        //[TestCategory("Login Redirect with query stream")]
        //[TestCategory("CCMCA-4383")]
        //[TestCategory("CCMCA-4688")]
        //public void Login_Redirect_Boosts()
        //{
        //    if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE))
        //    {
        //        string requestedPage = string.Empty;
        //        string responsePage = string.Empty;
        //        driver.Navigate().GoToUrl(SanityConfiguration.BoostUrl);
        //        requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
        //        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
        //        responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        if (requestedPage != responsePage)
        //        {
        //            Assert.Fail("User provided Login Url and Landing page do not match");
        //        }
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Boosts page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //}

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_Points()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.PointsUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        //[TestMethod]
        //[Description("To verify if the Login url is maintained till we land on the user provided link")]
        //[TestCategory("P1")]
        //[TestCategory("Login Redirect with query stream")]
        //[TestCategory("CCMCA-4383,4688")]
        //public void Login_Redirect_PointsSummary()
        //{
        //    if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE))
        //    {
        //        string requestedPage = string.Empty;
        //        string responsePage = string.Empty;
        //        driver.Navigate().GoToUrl(SanityConfiguration.PointsSummaryUrl);
        //        requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
        //        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
        //        responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        if (requestedPage != responsePage)
        //        {
        //            Assert.Fail("User provided Login Url and Landing page do not match");
        //        }
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Points Summary page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //}

        //[TestMethod]
        //[Description("To verify if the Login url is maintained till we land on the user provided link")]
        //[TestCategory("P1")]
        //[TestCategory("Login Redirect with query stream")]
        //[TestCategory("CCMCA-4383,4688")]
        //public void Login_Redirect_PointsDetails()
        //{
        //    if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE))
        //    {
        //        string requestedPage = string.Empty;
        //        string responsePage = string.Empty;
        //        driver.Navigate().GoToUrl(SanityConfiguration.PointsDetailsUrl);
        //        requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
        //        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
        //        responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
        //        if (requestedPage != responsePage)
        //        {
        //            Assert.Fail("User provided Login Url and Landing page do not match");
        //        }
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Points Details page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //}

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_Vouchers()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEVOUCHERSPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.VouchersUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Vouchers page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_Coupons()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDEECOUPONPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.CouponsUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Coupons page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_MyLatestStatement()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.MyLatestStatementUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("My Latest Statement page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify if the Login url is maintained till we land on the user provided link")]
        [TestCategory("P1")]
        [TestCategory("P1_LoginRedirect")]
        [TestCategory("Login Redirect with query stream")]
        [TestCategory("CCMCA-4383")]
        [TestCategory("CCMCA-4688")]
        public void Login_Redirect_ChristmasSaver()
        {
            if (objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE))
            {
                string requestedPage = string.Empty;
                string responsePage = string.Empty;
                driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);
                requestedPage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                responsePage = objGeneric.GetRedirectedUrl(objGeneric.DecodeUrlString(driver.Url));
                if (requestedPage != responsePage)
                {
                    Assert.Fail("User provided Login Url and Landing page do not match");
                }
            }

            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver page is disabled/not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        #endregion

        #region Hiding Temp CC - 2LA changes
        [TestMethod]
        [Description("To check if the Temp Clubcard banner is not present before providing the security digits")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set2")]
        public void AccountDetails_Home_VerifyPrintTempCLubcard_Bfr2LAAuth()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");

                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if (!objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)))
                {
                    customLogs.LogInformation("Verification passed.'Print Temp.Clubcard' functionality is not present on the Home page before 2LA Auth");
                }
                else
                {
                    Assert.Fail("Test case Failed. 'Print Temp.Clubcard' functionality is present on the Home page before 2LA Auth");
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
        [Description("To check if the Temp clubcard banner is present after providing the security digits")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set2")]
        public void AccountDetails_Home_VerifyPrintTempCLubcard_After2LAAuth()
        {

            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.Validate_Title();
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }



            driver.Navigate().GoToUrl(SanityConfiguration.MCAUrl);
            var keyArray = ControlKeys.HOME_PRINT_CLUBCARD.Split('_');
            if (objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)))
            {
                objGeneric.ClickElement(keyArray[1], FindBy.ID);

                //IWebElement element = driver.FindElement(By.Id("btnPrintClubcard"));
                //IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                //executor.ExecuteScript("arguments[0].click();", element);
                // objGeneric.ClickElement(ControlKeys.HOME_PRINT_CLUBCARD, "Home Page");
                customLogs.LogInformation("Verification passed.'Print Temp.Clubcard' functionality is present on the Home page after 2LAAut");
            }
            else
            {
                Assert.Fail("Test case Failed. 'Print Temp.Clubcard' functionality is not present on the Home page after 2LAAuth");
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
