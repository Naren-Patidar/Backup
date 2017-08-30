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
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyAccountDetails objAccountDetails = null;
        MyContactPreference objMyContactPreference = null;


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


        [Description("To Expand Account Details Tab")]
        public void AccountDetails_expand()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            //objLogin.LogOut_Verification();  
        }

        [TestMethod]
        [Description("To Click on Personal Details and verify the title")]
        [Owner("Infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_PersonalDetails()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILSNEW, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //objLogin.LogOut_Verification();
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on Personal Details and verify the title")]
        [TestCategory("Sanity")]
        public void AccountDetails_StampClickAndVerifyTitle_PersonalDetails()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.PERSONALDETAILS))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.PERSONALDETAILS)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;

                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Personal Details", StampName.PERSONALDETAILS);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }

        }
        [TestMethod]
        [Description("To Click on View My Card Stamp and verify the title")]
        [TestCategory("Sanity")]
        public void AccountDetails_StampClickAndVerifyTitle_ViewMyCard()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.VIEWMYCARD))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.VIEWMYCARD)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.stampClick(ControlKeys.STAMP5, "View My Card", StampName.VIEWMYCARD);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                 

                    //objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }



        [TestMethod]
        [Description("To validate the stamp functionality in Personal Details page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
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
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampNumber, StampName.PERSONALDETAILS);
                    objGeneric.stampClick(ControlKeys.STAMP5, "My Personal Details", StampName.PERSONALDETAILS);

                    //  objGeneric.3(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //objLogin.LogOut_Verification();
                    // objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("To validate the stamp functionality in ORderReplacement page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
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
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.ORDERAREPLACEMENT);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Order Replacement", StampName.ORDERAREPLACEMENT);


                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                   // objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENT, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    //objLogin.LogOut_Verification();
                    // objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("To validate the stamp functionality for Boost page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
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
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.TESCOBOOST);

                    objGeneric.stampClick(ControlKeys.STAMP5, "TESCOBOOST", StampName.TESCOBOOST);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                  //  objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.TESCOBOOST, "Boost At Tesco", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    //objLogin.LogOut_Verification();
                    // objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("To validate the stamp functionality for Flying scheme")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
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
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.OPTIONANDBENEFIT);

                    objGeneric.stampClick(ControlKeys.STAMP5, "OPTIONANDBENEFIT", StampName.OPTIONANDBENEFIT);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                  //  objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "Option And Benefit", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("To save changes in personal details and validate the message")]
        [TestCategory("BasicFunctionality")]
        [Priority(0)]

        public void AccountDetails_PersonalDetails_Savechanges()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "true")
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //  objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
        }

        [TestMethod]
        [Description("To Click on Contact Preferences and verify the title")]
        [Owner("infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_ContactPreferences()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.verifyPageName(LabelKey.CONTACTPREFERENCETITLE, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }



        [TestMethod]
        [Description("")]        
        [TestCategory("")]
        public void test()
        {






            //driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys("634004022757820812");

            //driver.FindElement(By.Id("ctl00_PageContainer_txtPassword")).SendKeys("cco1234");
            //driver.FindElement(By.Id("ctl00_PageContainer_btnLogin")).Click();

            //string clubcard = "634004022757820812";
            //int number1=0;
            //int number2 = 0;
            //int number3 = 0;



           // number1 = Convert.ToInt32(driver.FindElement(By.Id("ctl00_PageContainer_spnFirstDigit")).Text) - 1;
          //  driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys("634004022757820812");
           // driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys("634004022757820812");
            //driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys("634004022757820812");
          //  driver.FindElement(By.Id("ctl00_PageContainer_txtSecurityAnswer1")).SendKeys("" + clubcard[number1]);

               // objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                //objLogin.SecurityLayer_Verification(testData.Clubcard);
              
               // objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                //objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                //objGeneric.verifyPageName(LabelKey.CONTACTPREFERENCETITLE, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
               // driver.FindElement(By.Id("ctl00_ctl00_PageContainer_MyAccountContainer_btnPrintCC")).Click();
               // objGeneric.ClickElement(ControlKeys.RADIOBUTTON_EMAIL, "contact preferences");
                //objLogin.LogOut_Verification();
           
        }

       
        [TestMethod]
        [Description("To Save Contact Preferences and verify save message")]
        [TestCategory("BasicFunctionality")]
        [Priority(0)]
        public void AccountDetails_ContactPreferences_SaveChanges()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.ClickElement(ControlKeys.BUTTON_CLICK, "contact preferences");
                objGeneric.verifyValidationMessage(LabelKey.CONTACTPREFERENCESAVE, ControlKeys.VALIDATION_MESSAGE, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                // objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on Option And Benefit and verify the title")]
        [Owner("infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_OptionAndBenefit()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                //  objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Regression of Option and Benifits clear and confirm button test case")]
        
        [TestCategory("P0_Regression")]
        public void OptionandBenifits()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                //  objLogin.LogOut_Verification();

                objGeneric.ClickElement(ControlKeys.Button_Clear, "options and benefits");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
                objGeneric.VerifyTextonthePageByXpath_Contains(LabelKey.OPTIONANDBENIFITSCRISTMASSAVER, ControlKeys.Lable_Christmasmessage, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);







            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);



        }
        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message ChistmasSavechanges")]
       
        [TestCategory("P0_Regression")]
        [Priority(0)]
        public void OptionAndBenefit_ChistmasSavechanges()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                //objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElementByXpath(ControlKeys.OPTIONSBENEFIT_LINKCHISTMAS, "options and benefits");

                driver.Navigate().Back();

                objGeneric.ClickElement(ControlKeys.RADIOBUTTON_CHISTMAS, "options and benefits");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.OPTIONSBENEFIT_Message, "options and benefits", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.RADIOBUTTON_CHISTMAS, "options and benefits");

                

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message AviosSavechanges")]
        [TestCategory("P0_Regression")]
        [Priority(0)]
        public void OptionAndBenefit_AviosSavechanges()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                //objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN, "options and benefits");

                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
             
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.OPTIONSBENEFIT_Message, "options and benefits", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //objGeneric.VerifyTextExistOnPage(ControlKeys.OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE);
                objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN, "options and benefits");


                objGeneric.ClickElementByXpath(ControlKeys.OPTIONSBENEFIT_LINKAVIOS, "options and benefits");

                driver.Navigate().Back();

                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN, "options and benefits");
               
                
              

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
              
        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message VirginAtlanticFlyingClubValidateMessage")]
        [TestCategory("P0_Regression")]
        [Priority(0)]
        public void OptionAndBenefit_VirginAtlanticFlyingClubValidateMessage()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                //objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElement(ControlKeys.Button_Clear, "options and benefits");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
                objGeneric.VerifyTextonthePageByXpath_Contains(LabelKey.OPTIONANDBENIFITSCRISTMASSAVER, ControlKeys.Lable_Christmasmessage, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElement(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");

                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");

                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);



                objGeneric.ClickElement(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");
                //driver.FindElement(By.Id(ControlKeys.TEXTBOX_VIRGINMEMBERSHIPID)).SendKeys("12121212");

                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.TEXTBOX_VIRGINMEMBERSHIPID).Id)).SendKeys("12333333333");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");

                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.OPTIONSBENEFIT_Message, "options and benefits", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);




                objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");

                objGeneric.ClickElementByXpath(ControlKeys.OPTIONSBENEFIT_LINKVIRGINATLANTICINFORMATION, "options and benefits");

                driver.Navigate().Back();

                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");
               
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
      

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message BA ValidateMessage")]
        [TestCategory("P0_Regression ")]
        [Priority(0)]
        public void OptionAndBenefit_BritishAirwaysExecutiveClub_AviosValidateMessage()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                //objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElement(ControlKeys.Button_Clear, "options and benefits");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
                objGeneric.VerifyTextonthePageByXpath_Contains(LabelKey.OPTIONANDBENIFITSCRISTMASSAVER, ControlKeys.Lable_Christmasmessage, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);


                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN, "options and benefits");
                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");


                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_BAVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);

                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN, "options and benefits");





                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.TEXTBOX_BAVIOSID).Id)).SendKeys("12333333");


                objGeneric.ClickElement(ControlKeys.OPTIONSBENEFIT_ConfirmButton, "options and benefits");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.OPTIONSBENEFIT_Message, "options and benefits", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);




                objGeneric.ClickElement(ControlKeys.HOMEPAGE_LEFTNAVIGATION, "Home Page");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");

                objGeneric.ElementEnabled(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");

                objGeneric.ClickElementByXpath(ControlKeys.OPTIONSBENEFIT_LINKBAAVIOS, "options and benefits");

                driver.Navigate().Back();

                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objGeneric.ElementEnabled(ControlKeys.RADIOBUTTON_VIRGINATLANTICFLYINGCLUB, "options and benefits");
               
               
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on View My Card and verify the title")]
        [Owner("infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                string x = driver.FindElement(By.Id("ctl00_ctl00_PageContainer_MyAccountContainer_pnlCards")).Text;
               // driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys("634004022757820812");
               
                // objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on Order A replacement and verify the title")]
        [Owner("infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_OrderReplacement()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.ORDERREPLACEMENT, ControlKeys.LINK_CLICK, "replacement");
                //objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENT, "replacement", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENTRS, "replacement", SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE);
                // objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on Order A replacement and verify the title")]
        [TestCategory("Sanity")]
        public void Stamp_ClickAndVerifyTitle_OrderReplacement()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.ORDERAREPLACEMENT))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.ORDERAREPLACEMENT)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Order A Replacement", StampName.ORDERAREPLACEMENT);
                    //objGeneric.ClickStamp(LabelKey.ORDERREPLACEMENT, ControlKeys.STAMP8, "Order A Replacement");
                    objGeneric.verifyPageName(LabelKey.ORDERREPLACEMENT, "Order A Replacement", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    // objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
        }

        [TestMethod]
        [Description("To Click on Tesco Boost and verify the title")]
        [Owner("Infosys")]
        [TestCategory("Sanity")]
        public void AccountDetails_linkClickAndVerifyTitle_TescoBoost()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEEXCHANGESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.TESCOBOOST, ControlKeys.LINK_CLICK, "Boosts at Tesco");
                objGeneric.verifyPageName(LabelKey.TESCOBOOST, "Boosts at Tesco", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                //  objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Click on Tesco Boost and verify the title")]
        [TestCategory("Sanity")]
        public void AccountDetails_StampClickAndVerifyTitle_TescoBoost()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();
            if (expectedStampName.ContainsValue(StampName.TESCOBOOST))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.TESCOBOOST)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Boost At Tesco", StampName.TESCOBOOST);
                    //objGeneric.ClickStamp(LabelKey.TESCOBOOST, ControlKeys.STAMP6, "Boost At Tesco");
                    objGeneric.verifyPageName(LabelKey.TESCOBOOST, "Boost At Tesco", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    // objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("To verify Selected preference as Email")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_Email()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objMyContactPreference.ContactPreferences_Email();

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Selected preference as Email")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_EmailText()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);

                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");

                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");

                objMyContactPreference.ContactPreferences_EmailText(testData.Clubcard);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify mobile phone number")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_phonenumber()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                if (CountrySetting.culture == "UK")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objMyContactPreference.ContactPreferences_Phonenumber(testData.Clubcard);
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
        [Description("To verify labels on my conatct preference page")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_labels()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                //objMyContactPreference.ContactPreferenceLabels(ValidationKey.CONTACTPREFERENCE_LABLEONE, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreferenceLabels(ValidationKey.CONTACTPREFERENCE_LABLEONE, ValidationKey.CONTACTPREFERENCE_LABLETWO, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify BA Avios premium on my conatct preference page")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_BAAviosPremium()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("BA Aviois not applicable for this counrty", TraceEventType.Start);
            }
            else
            {
                string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
                if (isPresent == "Y")
                {
                    objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                    objMyContactPreference.ContactPreferences_BAAviosPremium();
                }
                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To verify BA Avios standerd on my conatct preference page")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_BAAviosStanderd()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("Country is not UK", TraceEventType.Start);
                customLogs.LogMessage("BA Avios not applicable for this counrty", TraceEventType.Stop);
            }
            else
            {

                string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
                if (isPresent == "Y")
                {
                    objLogin.Login_Verification(testData.ClubcardBAAviosStd, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosStd);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                    objMyContactPreference.ContactPreferences_BAAviosStanderd();
                }
                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To verify Avios on my conatct preference page")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_AviosPremium()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("BA Aviois not applicable for this counrty", TraceEventType.Start);
            }
            else
            {

                string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
                if (isPresent == "Y")
                {
                    objLogin.Login_Verification(testData.ClubcardAviosPre, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardAviosPre);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                    objMyContactPreference.ContactPreferences_AviosPremium();
                }
                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To verify Avios standerd on my conatct preference page")]
        [TestCategory("1609")]
        public void AccountDetails_ContactPreferences_AviosStanderd()
        {
            string culture = CountrySetting.culture;
            if (culture != "UK")
            {
                customLogs.LogMessage("BA Aviois not applicable for this counrty", TraceEventType.Start);
            }
            else
            {
                string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
                if (isPresent == "Y")
                {
                    objLogin.Login_Verification(testData.ClubcardAviosStd, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardAviosStd);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                    objMyContactPreference.ContactPreferences_AviosStanderd();
                }
                else
                    Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
            }
        }

        #region ViewMyCards

        [TestMethod]
        [Description("To verify the common salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Common_Salutation_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Common_Salutation_verify(ValidationKey.VALIDATAIONNAMEOFACCOUNT.ToString(), ValidationKey.VALIDATIONANDSEPRATOR.ToString(),testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the main customer's salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Main_Salutation_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Main_Salutation_verify(ValidationKey.VALIDATIONMESSAGEFORMAIN.ToString(), ValidationKey.VALIDATIONMESSAGEFORASSO.ToString(), ValidationKey.VALIDATIONMESSAGEFORMAINCARDHOLDER.ToString(), testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the associate customer's salutaion on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Asso_Salutation_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Asso_Salutation_verify(ValidationKey.VALIDATIONMESSAGEFORASSO.ToString(), ValidationKey.VALIDATIONMESSAGEFORMAINCARDHOLDER.ToString(), testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the Associate customer's clubcards on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Asso_Cards_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_Asso_Cards_verify(testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
               
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify the Main customer's clubcards on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Main_Cards_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Main_Cards_verify(testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate Show Type of card column in View My Cards for Main customer")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_ShowTypeofCard_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
                string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
                if (showTypeofCard == "TRUE")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_ShowCardType_Verify(ValidationKey.VALIDATIONTYPEOFCARD.ToString(), testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(showTypeofCard, "FALSE", "Configuration Value not matched with WebConfig");
                }

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig or Type of card is not enabled");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate Show Type of card column in View My Cards for Associate customer")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Asso_ShowTypeofCard_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            if (isPresent == "Y")
            {
                DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
                string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
                if (showTypeofCard == "TRUE")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_Asso_ShowCardType_Verify(ValidationKey.VALIDATIONTYPEOFCARD.ToString(), testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(showTypeofCard, "FALSE", "Configuration Value not matched with WebConfig");
                }

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig or Type of card is not enabled");
            customLogs.LogInformation(endMessage);
        }
        
        [TestMethod]
        [Description("To verify the Main customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Main_WhereUsed_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE); 
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            if (isPresent == "Y")
            {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                    objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    objAccountDetails.ViewMyCard_Main_WhereUsed_verify(ValidationKey.VALIDATIONWHEREUSED.ToString(), dateFormat, testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
                    
                    
               
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the Associate customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_Asso_WhereUsed_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_Asso_WhereUsed_verify(ValidationKey.VALIDATIONWHEREUSED.ToString(), dateFormat, testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);



            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the Main customer's last used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_LastUsed_Main_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_LastUsed_Main_verify(showTypeofCard,dateFormat, testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the Associate customer's where used column on view my cards")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Regression_viewmycard")]
        public void AccountDetails_LastUsed_Asso_ViewMyCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEMANAGECARDSPAGE);
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            string dateFormat = dateFormatConfig.ConfigurationValue1.ToString();
            DBConfiguration showTypeofCardConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.SHOW_TYPE_OF_CARD, SanityConfiguration.DbConfigurationFile);
            string showTypeofCard = showTypeofCardConfig.ConfigurationValue1.ToString();
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.VIEWMYCARDS, ControlKeys.LINK_CLICK, "cards");
                objGeneric.verifyPageName(LabelKey.VIEWMYCARDS, "cards", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                objAccountDetails.ViewMyCard_LastUsed_Asso_verify(showTypeofCard, dateFormat, testData.Clubcard, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE);
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
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
