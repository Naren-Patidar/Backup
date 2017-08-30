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
using System.Reflection;


namespace Tesco.Framework.UITesting.Test
{

    [TestClass]
    public class JoinTestSuite
    {

        public IWebDriver driver;
        private ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        

        private static string beginMessage = "********************* Join Test Suite ****************************";
        private static string suiteName = "Join Test Suite";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);
        static TestData_JoinDetails testData = null;
        static TestDataHelper<TestData_JoinDetails> TestDataHelper = new TestDataHelper<TestData_JoinDetails>();
        static TestData_PersonalDetails testData_Personal = null;
        static TestDataHelper<TestData_PersonalDetails> ADTestData_Personal = new TestDataHelper<TestData_PersonalDetails>();
        // declare helpers        
        Join objJoin = null;
        Generic objGeneric = null;
        PersonalDetails objPersonalDetails = null;

        public JoinTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.JOINTESTSUITE);
        }

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {   
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_JoinDetails).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
            ADTestData_Personal.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_PersonalDetails).Name, SanityConfiguration.Domain);
            testData_Personal = ADTestData_Personal.TestData;
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            if (SanityConfiguration.RunAllBrowsers)
            {
                List<string> browsers = Enum.GetNames(typeof(Browser)).ToList();
                foreach (string browser in browsers)
                {
                    objAutomationHelper = new AutomationHelper();
                    objAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.JoinUrl);
                    lstAutomationHelper.Add(objAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + CountrySetting.culture + " for domain" + SanityConfiguration.Domain);
                objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.JoinUrl);
                driver = objAutomationHelper.WebDriver;
                objGeneric = new Generic(objAutomationHelper);
                switch (SanityConfiguration.DefaultBrowser)
                {
                    case Browser.IE:
                        if (objGeneric.isAlertPresent())
                            objGeneric.alert.Accept();
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
            objPersonalDetails = new PersonalDetails(objAutomationHelper, SanityConfiguration, testData_Personal); 
            objJoin = new Join(objAutomationHelper, SanityConfiguration, testData);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Sanity 

        [TestMethod]
        [Description("Browse Join page")]
        [TestCategory("Sanity")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        [TestCategory("Join")]
        public void LeftNavigation_ValidatePageTitle_Join()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region BasicFunctionality

        [TestMethod]
        [Description("Fill Join mandatory details and confirm - MCA_SCN_UK_012_TC_02")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("MVC")]
        [TestCategory("Join")]
        [Priority(0)]
        public void Join_ConfirmJoinDetails()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {   
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.Mandatory);
                errorMsg += objJoin.Confirm();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Assert.Fail(errorMsg);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion        

        #region P0

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_04")]
        [TestCategory("P0")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        public void Join_FillAllDetailsAndConfirm()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                errorMsg += objJoin.Confirm();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Assert.Fail(errorMsg);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }      

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_16")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]        
        public void Join_DownloadClubcard()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                errorMsg += objJoin.Confirm();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Assert.Fail(errorMsg);
                }
                objGeneric.ClickElement(ControlKeys.JOIN_PDFDOWNLOAD, FindBy.CSS_SELECTOR_ID);
                objAutomationHelper.WebDriver.AcceptAlert();
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }            
            customLogs.LogInformation(endMessage);
        }

        //Join Promotional Offers has been removed in reskin
        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_11")]
        //[TestCategory("P0")]
        //[TestCategory("P0Set1")]
        //[TestCategory("P0_Join")]
        //[TestCategory("Join")]
        //[TestCategory("Join_Regression")]        
        //public void Join_PromotionCodeStartsTodayConfirmation()
        //{
        //    string errorMsg = string.Empty;
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    bool promotionalCodePresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPROMOTIONALCODE);
        //    if (isPresent)
        //    {
        //        if (promotionalCodePresent)
        //        {
        //            objJoin.VerifyJoinPage();
        //            errorMsg = objJoin.EnterData(FieldType.All);
        //            objGeneric.EnterDataInField(ControlKeys.JOIN_TXTPROMOCODE, testData.PromotionalCodeStartingToday);
        //            errorMsg += objJoin.Confirm();
        //            if (!string.IsNullOrEmpty(errorMsg))
        //            {
        //                Assert.Fail(errorMsg);
        //            }
        //        }
        //        else
        //            Assert.Inconclusive(string.Format("Join promotional code is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }           
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //    customLogs.LogInformation(endMessage);              
        //}

        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_12")]
        //[TestCategory("P0")]
        //[TestCategory("P0Set1")]
        //[TestCategory("P0_Join")]
        //[TestCategory("Join")]
        //[TestCategory("Join_Regression")] 
        //public void Join_PromotionCodeExpiresTodayConfirmation()
        //{
        //    string errorMsg = string.Empty;
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    if (isPresent)
        //    {
        //        bool promotionalCodePresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPROMOTIONALCODE);          
        //        if (promotionalCodePresent)
        //        {
        //            objJoin.VerifyJoinPage();
        //            errorMsg = objJoin.EnterData(FieldType.All);
        //            objGeneric.EnterDataInField(ControlKeys.JOIN_TXTPROMOCODE, testData.PromotionalCodeStartingToday);
        //            errorMsg += objJoin.Confirm();
        //            if (!string.IsNullOrEmpty(errorMsg))
        //            {
        //                Assert.Fail(errorMsg);
        //            }
        //        }
        //        else
        //            Assert.Inconclusive(string.Format("Join promotional code is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }        
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //    customLogs.LogInformation(endMessage);            
        //}

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_13")]
        [TestCategory("P0")]
        [TestCategory("P0Set5")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        public void Join_DuplicationCheckOnNameAddressAndEmail()
        {
            string errorMsg = string.Empty;
            string expectedError = string.Empty;
            string isDuplicacyEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DUPLICATECHECKREQUIRED);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            bool isFieldVisibleName1 = objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME1);
            bool isFieldVisibleEmail = objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL);
            if (isPresent && isFieldVisibleName1 && isFieldVisibleEmail && isDuplicacyEnabled.Equals("TRUE"))
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                objGeneric.EnterDataInField(ControlKeys.JOIN_FIRSTNAME, testData.DuplicateName1);
                objGeneric.EnterDataInField(ControlKeys.JOIN_MIDDLENAME, testData.DuplicateName2);
                objGeneric.EnterDataInField(ControlKeys.JOIN_SURNAME, testData.DuplicateName3);
                objGeneric.EnterDataInField(ControlKeys.JOIN_EMAIL, testData.DuplicateEmailAddress);
                errorMsg += objJoin.Confirm();
                expectedError = objJoin.GetDuplicateError();
                if (!errorMsg.Trim().Equals(expectedError.Trim()))
                {
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        Assert.Fail(errorMsg);
                    }
                    else
                    {
                        Assert.Fail("No validation error for duplicate first name.");
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", isPresent ? "First Name" : "Join Page", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_14")]
        [TestCategory("P0")]
        [TestCategory("P0Set5")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        public void Join_DuplicationCheckOnNameAndAddress()
        {
            string errorMsg = string.Empty;
            string expectedError = string.Empty;
            string isDuplicacyEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DUPLICATECHECKREQUIRED);
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            bool isFieldVisibleName1 = objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME1);
            
            bool isFieldVisibleName3 = objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME3);
            if (isPresent && isFieldVisibleName1 && isFieldVisibleName3 && isDuplicacyEnabled.Equals("TRUE"))
            {                
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                objGeneric.EnterDataInField(ControlKeys.JOIN_FIRSTNAME, testData.DuplicateName1);
                objGeneric.EnterDataInField(ControlKeys.JOIN_MIDDLENAME, testData.DuplicateName2);
                objGeneric.EnterDataInField(ControlKeys.JOIN_SURNAME, testData.DuplicateName3);
                errorMsg += objJoin.Confirm();
                expectedError = objJoin.GetDuplicateError();
                if (!errorMsg.Trim().Equals(expectedError.Trim()))
                {
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        Assert.Fail(errorMsg);
                    }
                    else
                    {
                        Assert.Fail("No validation error for duplicate first name.");
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", isPresent ? "First Name" : "Join Page" , CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);             
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_15")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")] 
        public void Join_DuplicationCheckOnEmailID()
        {
            string errorMsg = string.Empty;
            string expectedError = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            bool isFieldVisible = objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL);
            if (isPresent && isFieldVisible)
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                objGeneric.EnterDataInField(ControlKeys.JOIN_EMAIL, testData.DuplicateEmailAddress);
                errorMsg += objJoin.Confirm();
                expectedError = objJoin.GetDuplicateError();
                if (!errorMsg.Trim().Equals(expectedError.Trim()))
                {
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        Assert.Fail(errorMsg);
                    }
                    else
                    {
                        Assert.Fail("No validation error for duplicate Email.");
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", isPresent ? "Email" : "Join Page", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);            
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_49&50")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("CCMCA-4990")]
        [TestCategory("Join_Regression")] 
        public void Join_SelectGreatOffersAndConfirm()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            bool isFieldVisible = objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL);
            if (isPresent && isFieldVisible)
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.All);
                errorMsg += objJoin.SelectMarketingPreferences(SecurityPreference.Tesco_Products);
                errorMsg += objJoin.SelectMarketingPreferences(SecurityPreference.Tesco_Partners);
                errorMsg += objJoin.SelectMarketingPreferences(SecurityPreference.Customer_Research);
                errorMsg += objJoin.Confirm();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Assert.Fail(errorMsg);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", isPresent ? "Email" : "Join Page", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_68")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        public void Join_VerifyTotalYearsInDDL()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    objJoin.HouseholdAgeDropdownValues();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "TRUE", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_67")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        public void Join_VerifyYearinDDL()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    objJoin.SelectYearInDOB();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "TRUE", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_69")]
        [TestCategory("Join_Regression")]
        [TestCategory("Join")]
        public void Join_SelectHouseHoldPersonAgeAndConfirm()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            bool isFieldVisible = objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL);
            if (isPresent && isFieldVisible)
            {
                objJoin.VerifyJoinPage();
                errorMsg = objJoin.EnterData(FieldType.Mandatory);
                objJoin.selectddl();
                errorMsg += objJoin.Confirm();
                objJoin.verifyConfirmMessage();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Assert.Fail(errorMsg);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", isPresent ? "Email" : "Join Page", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);            
        } 

        #endregion
        
        #region P1

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_05")]
        [TestCategory("P1")]
        [TestCategory("P1_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        
        public void Join_EnterProfaneWordInFirstName()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isProfanityEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROFANITYREQUIRED, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isCheckOnFieldEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME1, SanityConfiguration.DbConfigurationFile);
            if (isPresent)
            {
                if (!isProfanityEnabled.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if(!isCheckOnFieldEnabled.ConfigurationValue1.Equals("1"))                
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled on First Name : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!fieldConfig.ConfigurationValue1.Equals("0"))
                {
                    Assert.Inconclusive(string.Format("First Name field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    objJoin.VerifyJoinPage();
                    errorMsg = objJoin.EnterData(FieldType.All);
                    errorMsg += objJoin.EnterDataInField(ControlKeys.PERSONALDETAILS_FIRSTNAME, testData_Personal.ProfaneName1);
                    errorMsg += objJoin.Confirm();
                    if (!errorMsg.Trim().ToUpper().Equals(objJoin.GetProfaneError().Trim().ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            Assert.Fail(errorMsg);
                        }
                        else
                        {
                            Assert.Fail("No validation error for Profane word in First Name Field");
                        }
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_06")]
        [TestCategory("P1")]
        [TestCategory("P1_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
       
        public void Join_EnterProfaneWordInSurName()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isProfanityEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROFANITYREQUIRED, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isCheckOnFieldEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME3, SanityConfiguration.DbConfigurationFile);
            if (isPresent)
            {
                if (!isProfanityEnabled.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!isCheckOnFieldEnabled.ConfigurationValue1.Equals("1"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled on Sur Name : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!fieldConfig.ConfigurationValue1.Equals("0"))
                {
                    Assert.Inconclusive(string.Format("SurName field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    objJoin.VerifyJoinPage();
                    errorMsg = objJoin.EnterData(FieldType.All);
                    errorMsg += objJoin.EnterDataInField(ControlKeys.PERSONALDETAILS_SURNAME, testData_Personal.ProfaneName1);
                    errorMsg += objJoin.Confirm();
                    if (!errorMsg.Trim().ToUpper().Equals(objJoin.GetProfaneError().Trim().ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            Assert.Fail(errorMsg);
                        }
                        else
                        {
                            Assert.Fail("No validation error for Profane word in SurName Field");
                        }
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_07")]
        [TestCategory("P1")]
        [TestCategory("P1_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
       
        public void Join_EnterProfaneWordInMiddleName()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isProfanityEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROFANITYREQUIRED, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isCheckOnFieldEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME2, SanityConfiguration.DbConfigurationFile);
            if (isPresent)
            {
                if (!isProfanityEnabled.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!isCheckOnFieldEnabled.ConfigurationValue1.Equals("1"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled on Middle Name : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!fieldConfig.ConfigurationValue1.Equals("0"))
                {
                    Assert.Inconclusive(string.Format("Middle Name field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    objJoin.VerifyJoinPage();
                    errorMsg = objJoin.EnterData(FieldType.All);
                    errorMsg += objJoin.EnterDataInField(ControlKeys.PERSONALDETAILS_MIDDLENAME, testData_Personal.ProfaneName1);
                    errorMsg += objJoin.Confirm();
                    if (!errorMsg.Trim().ToUpper().Equals(objJoin.GetProfaneError().Trim().ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            Assert.Fail(errorMsg);
                        }
                        else
                        {
                            Assert.Fail("No validation error for Profane word in Middle Name Field");
                        }
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);            
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_08")]
        [TestCategory("P1")]
        [TestCategory("P1_Join")]
        [TestCategory("Join")]
        [TestCategory("Join_Regression")]
        
        public void Join_EnterProfaneWordInHouseName()
        {
            string errorMsg = string.Empty;
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isProfanityEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROFANITYREQUIRED, SanityConfiguration.DbConfigurationFile);
            DBConfiguration isCheckOnFieldEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.HOUSENAME, SanityConfiguration.DbConfigurationFile);
            if (isPresent)
            {
                if (!isProfanityEnabled.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!isCheckOnFieldEnabled.ConfigurationValue1.Equals("1"))
                {
                    Assert.Inconclusive(string.Format("Profane check is not enabled on Sur Name : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else if (!fieldConfig.ConfigurationValue1.Equals("0"))
                {
                    Assert.Inconclusive(string.Format("Middle Name field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    objJoin.VerifyJoinPage();
                    errorMsg = objJoin.EnterData(FieldType.All);
                    errorMsg += objJoin.EnterDataInField(ControlKeys.JOIN_HOUSENUMBER, testData_Personal.ProfaneName1);
                    errorMsg += objJoin.Confirm();
                    if (!errorMsg.Trim().ToUpper().Equals(objJoin.GetProfaneError().Trim().ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            Assert.Fail(errorMsg);
                        }
                        else
                        {
                            Assert.Fail("No validation error for Profane word in House Name Field");
                        }
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);            
        }

        //Join Promotional Offers has been removed in reskin
        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_09")]
        //[TestCategory("P1")]
        //[TestCategory("P1_Join")]
        //[TestCategory("Join")]
        //[TestCategory("Join_Regression")]        
        //public void Join_EnterInvalidPromotionalCodeAndConfirm()
        //{
        //    string errorMsg = string.Empty;
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPROMOTIONALCODE, SanityConfiguration.DbConfigurationFile);
            
        //    if (isPresent)
        //    {
        //        if (!fieldConfig.ConfigurationValue1.Equals("0"))
        //        {
        //            Assert.Inconclusive(string.Format("Promotional Code field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //        }
        //        else
        //        {
        //            objJoin.VerifyJoinPage();
        //            errorMsg = objJoin.EnterData(FieldType.All);
        //            errorMsg += objJoin.EnterDataInField(ControlKeys.JOIN_TXTPROMOCODE, testData.InvalidPromotionalCode);
        //            errorMsg += objJoin.Confirm();
        //            Resource res = AutomationHelper.GetResourceMessage(ValidationKey.ERRORPROMOCODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
        //            if (res != null)
        //            {
        //                if (!errorMsg.Trim().ToUpper().Equals(res.Value.Trim().ToUpper()))
        //                {
        //                    if (!string.IsNullOrEmpty(errorMsg))
        //                    {
        //                        Assert.Fail(errorMsg);
        //                    }
        //                    else
        //                    {
        //                        Assert.Fail("No validation error for Invalid Promotional Code");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Assert.Fail(string.Format("Value for resource key '{0}' not found.", ValidationKey.ERRORPROMOCODE));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //    customLogs.LogInformation(endMessage);
        //}


        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_13")]
        //[TestCategory("P1")]
        //[TestCategory("P1_Join")]
        //[TestCategory("Join")]
        //[TestCategory("Join_Regression")]        
        //public void Join_EnterExpiredPromotionalCodeAndConfirm()
        //{
        //    string errorMsg = string.Empty;
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    DBConfiguration fieldConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPROMOTIONALCODE, SanityConfiguration.DbConfigurationFile);

        //    if (isPresent)
        //    {
        //        if (!fieldConfig.ConfigurationValue1.Equals("0"))
        //        {
        //            Assert.Inconclusive(string.Format("Promotional Code field is not enabled : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //        }
        //        else
        //        {
        //            objJoin.VerifyJoinPage();
        //            errorMsg = objJoin.EnterData(FieldType.All);
        //            errorMsg += objJoin.EnterDataInField(ControlKeys.JOIN_TXTPROMOCODE, testData.ExpiredPromotionalCode);
        //            errorMsg += objJoin.Confirm();
        //            Resource res = AutomationHelper.GetResourceMessage(ValidationKey.ERRORPROMOCODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
        //            if (res != null)
        //            {
        //                if (!errorMsg.Trim().ToUpper().Equals(res.Value.Trim().ToUpper()))
        //                {
        //                    if (!string.IsNullOrEmpty(errorMsg))
        //                    {
        //                        Assert.Fail(errorMsg);
        //                    }
        //                    else
        //                    {
        //                        Assert.Fail("No validation error for Expired Promotional Code");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Assert.Fail(string.Format("Value for resource key '{0}' not found.", ValidationKey.ERRORPROMOCODE));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Join page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //    }
        //    customLogs.LogInformation(endMessage);            
        //}


        #endregion

        #region  P2

        //Join Page banner has been removed in reskin
        //[TestMethod]
        //[Description("Verify Text in Header Image in Join page")]
        //[TestCategory("P2")]
        //[TestCategory("P2_Join")]
        //[TestCategory("Join")]
        //[TestCategory("MVC")]
        //[TestCategory("P2_Join_Christmas")]
        //public void Join_VerifyHeaderImageTextOnJoinPage()
        //{
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    if (isPresent)
        //    {
        //        objJoin.VerifyJoinPage();
        //        objJoin.VerifyHeaderImageTextOnJoinPage(driver);
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //        customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
        //    }
        //    customLogs.LogInformation(endMessage);
        //}

        [TestMethod]
        [Description("Verify Text in Header in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyHeaderTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                objJoin.VerifyHeaderTextOnJoinPage(driver);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }
        
        //Join doesnt have this scenario. Its present in Personal Details page only.
        //[TestMethod]
        //[Description("Verify Text in Header Para One in Join page")]
        //[TestCategory("P2")]
        //[TestCategory("P2_Join")]
        //[TestCategory("Join")]
        //[TestCategory("MVC")]
        //[TestCategory("P2_Join_ChristmasOne")]
        //public void Join_VerifyHeaderParaOneTextOnJoinPage()
        //{
        //    bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
        //    if (isPresent)
        //    {
        //        objJoin.VerifyJoinPage();
        //        objJoin.VerifyHeaderParaOneTextOnJoinPage(driver);
        //    }
        //    else
        //    {
        //        Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
        //        customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
        //    }
        //    customLogs.LogInformation(endMessage);
        //}

        [TestMethod]
        [Description("Verify Text in Header Text in Contact Details in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyHeaderTextInContactDetailsOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                objJoin.VerifyHeaderInContactDetailsOnJoinPage(driver);
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Text in Header Text in Terms and Conditions in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyHeaderTextInTermsAndConditionsOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDELEGALPOLICY, SanityConfiguration.DbConfigurationFile);
                if (config.IsDeleted != "N")
                {
                    objJoin.VerifyHeaderInTermsAndConditionsOnJoinPage(driver);
                }
                else
                {
                    Assert.Inconclusive("Legal Policy is not enabled for country " + CountrySetting.country);
                    customLogs.LogInformation("Legal Policy is not enabled for country " + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Agree Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyAgreeTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDELEGALPOLICY, SanityConfiguration.DbConfigurationFile);
                if (config.IsDeleted != "N")
                {
                    objJoin.VerifyAgreeTextOnJoinPage(driver);
                }
                else
                {
                    Assert.Inconclusive("Legal Policy is not enabled for country " + CountrySetting.country);
                    customLogs.LogInformation("Legal Policy is not enabled for country " + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Confirm Click Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyConfirmClickTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDELEGALPOLICY, SanityConfiguration.DbConfigurationFile);
                if (config.IsDeleted != "N")
                {
                    objJoin.VerifyConfirmClickTextOnJoinPage(driver);
                }
                else
                {
                    Assert.Inconclusive("Legal Policy is not enabled for country " + CountrySetting.country);
                    customLogs.LogInformation("Legal Policy is not enabled for country " + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Captcha Header Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyCaptchaHeaderTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISCAPTCHAENABLED, SanityConfiguration.DbConfigurationFile);
                if (config.ConfigurationValue1 == "TRUE")
                {
                    objJoin.VerifyCaptchaTextOnJoinPage(driver);
                }
                else
                {
                    Assert.Inconclusive("Legal Policy is not enabled for country " + CountrySetting.country);
                    customLogs.LogInformation("Legal Policy is not enabled for country " + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Verify Opt Ins/Outs Header Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("CCMCA-4990")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyMarketingPrefHeaderTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
                if (config.ConfigurationValue1.ToUpper().Equals("FALSE"))
                {
                    objJoin.VerifyOptInsHeaderTextOnJoinPage(driver);
                }
                else
                {
                    objJoin.VerifyOptOutsHeaderTextOnJoinPage(driver);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }
        
        [TestMethod]
        [Description("Verify Opt Ins/Outs Header Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("CCMCA-4990")]
        public void Join_VerifyMarketingPreFooterTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
                if (config.ConfigurationValue1.ToUpper().Equals("FALSE"))
                {
                    objJoin.VerifyOptInsFooterTextOnJoinPage(driver, LabelKey.JOIN_FOOTERNOTEOPTIN, ControlKeys.JOIN_PREFERENCEFOOTERNOTE);
                    //objJoin.VerifyOptInsFooterLinkOnJoin(driver);
                }
                else
                {
                    objJoin.VerifyOptInsFooterTextOnJoinPage(driver, LabelKey.JOIN_FOOTERNOTEOPTOUT, ControlKeys.JOIN_PREFERENCEFOOTERNOTEOPTOUT);
                }
                
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Opt Footer Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("CCMCA-4990")]
        public void Join_VerifyMarketingPreFooterLinksOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            DBConfiguration isPrivacyFooterLinkEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEFOOTERLINK, SanityConfiguration.DbConfigurationFile);
            if (isPresent)
            {
                if (isPrivacyFooterLinkEnabled.ConfigurationValue1.Equals("1"))
                {
                    objJoin.VerifyJoinPage();
                    objJoin.VerifyOptFooterLinkOnJoinPage(driver);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Privacy Footer Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    customLogs.LogInformation("Privacy Footer Link is not present for country " + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Verify Call Rate Text in Join page")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyCallRateTextOnJoinPage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                objJoin.VerifyJoinPage();
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDELEGALPOLICY, SanityConfiguration.DbConfigurationFile);
                if (config.IsDeleted != "N")
                {
                    objJoin.VerifyCallRateTextOnJoinPage(driver);
                }
                else
                {
                    customLogs.LogInformation("Legal Policy is not enabled for country " + CountrySetting.country);
                }
            }
            else
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_65")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyHouseholdSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                string isDisabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isDisabled.ToUpper().Equals("FALSE"))
                {
                    string error = objJoin.ValidateHouseHoldTest(driver);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Assert.Fail(error);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("House hold section is disabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_47")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("New")]
        [TestCategory("P2_Join_Christmas")]
        public void Join_VerifyDataProtectionSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            string error = string.Empty;
            if (isPresent)
            {
                error += objJoin.SelectMarketingPreferences(SecurityPreference.Tesco_Products);
                error += objJoin.SelectMarketingPreferences(SecurityPreference.Tesco_Partners);
                error += objJoin.SelectMarketingPreferences(SecurityPreference.Customer_Research);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            }
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_66")]
        [TestCategory("P2")]
        [TestCategory("P2_Join")]
        [TestCategory("Join")]
        [TestCategory("MVC")]        
        public void Join_VerifyHouseholdDetailsList()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    objJoin.YourHouseholdAge_Titles();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "TRUE", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
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
