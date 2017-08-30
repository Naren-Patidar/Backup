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
       // static string LocalResourceFileName = "Join-App_LocalResources.en-GB.xml";
        static string culture;

        private static string beginMessage = "********************* Join Test Suite ****************************";
        private static string suiteName = "Join Test Suite";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);
        static TestData_JoinDetails testData = null;
        static TestDataHelper<TestData_JoinDetails> TestDataHelper = new TestDataHelper<TestData_JoinDetails>();
        // declare helpers        
        Join objJoin = null;
        Generic objGeneric = null;

        public JoinTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.JOINTESTSUITE);
        }

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_JoinDetails).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
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
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + culture + " for domain" + SanityConfiguration.Domain);
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
            
            objJoin = new Join(objAutomationHelper, SanityConfiguration,testData);            
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

        [TestMethod]
        [Description("Browse Join page")]
        [TestCategory("Sanity")]        
        public void BrowseJoinPage()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            //For HideJoinfunctionality field "isdeleted" = N says page will be displayed
            if (isPresent == "N")
                objJoin.VerifyJoinPage(driver);
            else
                customLogs.LogInformation("Join Page is not enabled for country " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Fill Join mandatory details and confirm")]
        [TestCategory("BasicFunctionality")]
        [Priority(0)]
        public void ConfirmJoinDetails()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.VerifyJoinPage(driver);
                objJoin.FillMandatoryDetails(driver);
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                //objGeneric.verifyValidationMessage(LabelKey.JOINCONFIRMATIONIMAGE, ControlKeys.JOIN_imgalldone, "Join Page", LocalResourceFileName);
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_04")]
        [TestCategory("Join_Regression")]
        public void FillAllDetailsAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillAllFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
              // objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                // driver.Navigate().GoToUrl("https://172.21.179.41/Clubcard/MyAccount/Join/Confirmation.aspx?clubcardID=634004025000109941");
               // driver.Navigate().GoToUrl("https://s.tesco.pl/Clubcard/mojekonto/Join/Confirmation.aspx?clubcardID=634009528013100166"); For Poland
                objJoin.verifyConfirmMessage();
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                 customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_02")]
        [TestCategory("Join_Regression")]
         
        public void FillMandatoryDetailsAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_16")]
        [TestCategory("Join_Regression")]
        public void DownloadPDF()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_PDFDOWNLOAD, "Join Page");
                if (objGeneric.isAlertPresent())
                    objGeneric.alert.Accept();
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
          
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_11")]
        [TestCategory("Join_Regression")]
        public void PromotionCodeStartsTodayConfirmation()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                var isDeleted = (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, "HidePromotionalCodeInJoinPage", SanityConfiguration.DbConfigurationFile)).IsDeleted;
                if (isDeleted == "N")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                    objJoin.enterPromotionalCode(testData.PromotionalCodeStartingToday);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objJoin.verifyConfirmMessage();
                }
                else
                    Assert.AreEqual(isDeleted, "Y", "Configuration Value not matched with DBConfig");
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_12")]
        [TestCategory("Join_Regression")]
        public void PromotionCodeExpiresTodayConfirmation()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                var isDeleted = (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, "HidePromotionalCodeInJoinPage", SanityConfiguration.DbConfigurationFile)).IsDeleted;
                if (isDeleted == "N")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                    objJoin.enterPromotionalCode(testData.PromotionalCodeExpiringToday);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objJoin.verifyConfirmMessage();
                }
                else
                    Assert.AreEqual(isDeleted, "Y", "Configuration Value not matched with DBConfig");
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_01")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void LeaveAllFieldBlankAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.returnMandatoryFields(Enums.FieldType.Valid);
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyValidationCheck(Enums.FieldType.Mandatory);
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_03")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterInvalidDetailsAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillAllFields(Enums.FieldType.Invalid);
                objJoin.acceptLegalPolicy();
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyValidationCheck(Enums.FieldType.All);

            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_05")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterProfaneWordInFirstName()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                string isProfanityEnabled=AutomationHelper.GetWebConfiguration(WebConfigKeys.ISPROFANITYCHECKENABLED, SanityConfiguration.WebConfigurationFile).Value;
                string isCheckOnName1Enabled=AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME1, SanityConfiguration.DbConfigurationFile).IsDeleted;
                if (isProfanityEnabled.Equals("1")&&isCheckOnName1Enabled.Equals("N"))
                {
                     objJoin.fillAllFields(Enums.FieldType.ProfaneName1);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROFANE, ControlKeys.JOIN_ERRORPROFANE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_06")]
        [TestCategory("Join_Regression_N")]
         [TestCategory("P1_Regression")]
        public void EnterProfaneWordInSurName()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                string isProfanityEnabled = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISPROFANITYCHECKENABLED, SanityConfiguration.WebConfigurationFile).Value;
                string isCheckOnName3Enabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME3, SanityConfiguration.DbConfigurationFile).IsDeleted;
                if (isProfanityEnabled.Equals("1") && isCheckOnName3Enabled.Equals("N"))
                {
                    objJoin.fillAllFields(Enums.FieldType.ProfaneName3);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROFANE, ControlKeys.JOIN_ERRORPROFANE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
 
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_07")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterProfaneWordInMiddleName()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                string isProfanityEnabled = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISPROFANITYCHECKENABLED, SanityConfiguration.WebConfigurationFile).Value;
                string isCheckOnName2Enabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME2, SanityConfiguration.DbConfigurationFile).IsDeleted;
                if (isProfanityEnabled.Equals("1") && isCheckOnName2Enabled.Equals("N"))
                {
                    objJoin.fillAllFields(Enums.FieldType.ProfaneName2);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROFANE, ControlKeys.JOIN_ERRORPROFANE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_08")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterProfaneWordInHouseName()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                string isProfanityEnabled = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISPROFANITYCHECKENABLED, SanityConfiguration.WebConfigurationFile).Value;
                string isCheckOnHouseNumberEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.HOUSENAME, SanityConfiguration.DbConfigurationFile).IsDeleted;
                if (isProfanityEnabled.Equals("1") && isCheckOnHouseNumberEnabled.Equals("N"))
                {
                    objJoin.fillAllFields(Enums.FieldType.Valid);
                    driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.JOIN_HOUSENUMBER).Id)).SendKeys(testData.ProfaneHouseName);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROFANE, ControlKeys.JOIN_ERRORPROFANE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }    
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_09")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterInvalidPromotionalCodeAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                var isDeleted = (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, "HidePromotionalCodeInJoinPage", SanityConfiguration.DbConfigurationFile)).IsDeleted;
                if (isDeleted == "N")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                    objJoin.enterPromotionalCode(testData.InvalidPromotionalCode);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROMOCODE, ControlKeys.JOIN_ERRORPROMOCODE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
                else
                    Assert.AreEqual(isDeleted, "Y", "Configuration Value not matched with DBConfig");
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_13")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void EnterExpiredPromotionalCodeAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                var isDeleted = (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, "HidePromotionalCodeInJoinPage", SanityConfiguration.DbConfigurationFile)).IsDeleted;
                if (isDeleted == "N")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                    objJoin.enterPromotionalCode(testData.ExpiredPromotionalCode);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORPROMOCODE, ControlKeys.JOIN_ERRORPROMOCODE, "Join", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
                else
                    Assert.AreEqual(isDeleted, "Y", "Configuration Value not matched with DBConfig");
            }
            else
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_14")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void DuplicationCheckOnFirstName()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Duplicate_check_fields, "NameANDAddress", SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.DuplicateNameANDAddress);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORDUPLICACY, ControlKeys.JOIN_ERRORPROFANE, "Join Duplicacy", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
            }

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_15")]
        [TestCategory("Join_Regression_N")]
        [TestCategory("P1_Regression")]
        public void DuplicationCheckOnEmailID()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {

                if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Duplicate_check_fields, "EmailAddress", SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                {
                    objJoin.fillAllFields(Enums.FieldType.DuplicateEmail);
                    objJoin.acceptLegalPolicy();
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objGeneric.verifyValidationMessage(ValidationKey.ERRORDUPLICACY, ControlKeys.JOIN_ERRORPROFANE, "Join Email Duplicacy", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
            }
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_65")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P1_Regression")]
        public void VerifyHouseholdSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.JOIN_HOUSEHOLD, ControlKeys.JOIN_TXTHOUSEHOLD, "Message validated", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.JOIN_OPTIONALINFO, ControlKeys.JOIN_TXTOPTIONALINFO, "Optional Info title", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_66")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P1_Regression")]
        public void VerifyHouseholdDetailsList()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_AGE, ControlKeys.JOIN_LBLYOU, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_PERSON2, ControlKeys.JOIN_LBLPERSON2, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_PERSON3, ControlKeys.JOIN_LBLPERSON3, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_PERSON4, ControlKeys.JOIN_LBLPERSON4, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_PERSON5, ControlKeys.JOIN_LBLPERSON5, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.verifyValidationMessage(LabelKey.JOIN_PERSON6, ControlKeys.JOIN_LBLPERSON6, "", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_67")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P1_Regression")]
        public void VerifyYearinDDL()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                    objJoin.SelectYearInDOB();
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_68")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P1_Regression")]
        public void VerifyTotalYearsInDDL()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                    objJoin.VerifyYearInDOB();
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_69")]
        [TestCategory("Join_Regression_P")]
        public void SelectHouseHoldPersonAgeAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {
                    objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                    objJoin.acceptLegalPolicy();
                    driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.JOIN_DDLPERSON2AGE).Id)).SendKeys(Keys.Down);
                    objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    objJoin.verifyConfirmMessage();
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as Dietary section is disabled ");
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_47")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P1_Regression")]
        public void VerifyDataProtectionSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.verifyCheckBoxInDataProtection(ControlKeys.JOIN_CHKTESCOOFFER);
                objJoin.verifyCheckBoxInDataProtection(ControlKeys.JOIN_CHKPARTNEROFFER);
                objJoin.verifyCheckBoxInDataProtection(ControlKeys.JOIN_CHKRESEARCH);
            }          
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);

        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_48")]
        [TestCategory("Join_Regression_P")]
        [TestCategory("P0_Regression")]
        public void CheckGreatOffersAndConfirmMandatoryError()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.returnMandatoryFields(Enums.FieldType.Valid);
                objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Customer Research Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyValidationCheck(Enums.FieldType.Mandatory);
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("MCA_SCN_UK_012_TC_49&50")]
        [TestCategory("Join_Regression_P")]
        public void SelectGreatOffersAndConfirm()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Customer Research Checkbox in Join Page");
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_37, MCA_SCN_UK_002_TC_38, MCA_SCN_UK_002_TC_39, MCA_SCN_UK_002_TC_40, MCA_SCN_UK_002_TC_42, MCA_SCN_UK_002_TC_43")]
        [TestCategory("P0_Regression")]
        [TestCategory("Join_OptIn")]
        public void Join_SelectProductServiceCheckBox()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
                string isGridFormat = webConfig.Value;

                WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
                string isOptInBehaviour = webConfig1.Value;

                if (isGridFormat == "false" && isOptInBehaviour == "true")
                {
                    string country = CountrySetting.country;
                    if (country.Equals("UK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("PL"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRPPRODUCTS, ControlKeys.JOIN_LBLGRPTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("CZ"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("SK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("MY"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("HU"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("TH"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEONE, ControlKeys.JOIN_LBLTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                    }
                    //objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    //objJoin.verifyConfirmMessage();
                }
                else if (isGridFormat == "true" && isOptInBehaviour == "true")
                {
                    objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRIDPRODUCTS, ControlKeys.JOIN_LBLGRIDTESCOPRODUCTS, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGMAIL, "Group Tesco Products Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGEMAIL, "Group Tesco Products E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGPHONE, "Group Tesco Products Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGSMS, "Group Tesco Products SMS Checkbox in Join Page");
                }
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_37, MCA_SCN_UK_002_TC_38, MCA_SCN_UK_002_TC_39, MCA_SCN_UK_002_TC_40, MCA_SCN_UK_002_TC_42, MCA_SCN_UK_002_TC_44")]
        [TestCategory("P0_Regression")]
        [TestCategory("Join_OptIn")]
        public void Join_SelectPartnerServiceCheckBox()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
                string isGridFormat = webConfig.Value;

                WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
                string isOptInBehaviour = webConfig1.Value;

                if (isGridFormat == "false" && isOptInBehaviour == "true")
                {
                    string country = CountrySetting.country;
                    if (country.Equals("UK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("PL"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRPOFFER, ControlKeys.JOIN_LBLGRPTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("CZ"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("SK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("MY"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("HU"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("TH"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETWO, ControlKeys.JOIN_LBLTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                    }
                    //objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    //objJoin.verifyConfirmMessage();
                }
                else if (isGridFormat == "true" && isOptInBehaviour == "true")
                {
                    objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRIDPARTNER, ControlKeys.JOIN_LBLGRIDTESCOPARTNER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPMAIL, "Group Tesco Partners Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPEMAIL, "Group Tesco Partners E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPPHONE, "Group Tesco Partners Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPSMS, "Group Tesco Partners SMS Checkbox in Join Page");
                }
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_37, MCA_SCN_UK_002_TC_38, MCA_SCN_UK_002_TC_39, MCA_SCN_UK_002_TC_40, MCA_SCN_UK_002_TC_42, MCA_SCN_UK_002_TC_45")]
        [TestCategory("P0_Regression")]
        [TestCategory("Join_OptIn")]
        public void Join_SelectResearchServiceCheckBox()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
                string isGridFormat = webConfig.Value;

                WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
                string isOptInBehaviour = webConfig1.Value;

                if (isGridFormat == "false" && isOptInBehaviour == "true")
                {
                    string country = CountrySetting.country;
                    if (country.Equals("UK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("PL"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRPREASEARCH, ControlKeys.JOIN_LBLGRPTESCOOFFER, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("CZ"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("SK"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("MY"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("HU"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("TH"))
                    {
                        objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLETHREE, ControlKeys.JOIN_LBLTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    //objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    //objJoin.verifyConfirmMessage();
                }
                else if (isGridFormat == "true" && isOptInBehaviour == "true")
                {
                    objGeneric.verifyValidationMessage(LabelKey.JOINCONTACTPREFERENCE_LABLEGRIDRESEARCH, ControlKeys.JOIN_LBLGRIDTESCORESEARCH, "Validation message checked", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRMAIL, "Group Tesco Partners Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKREMAIL, "Group Tesco Partners E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRPHONE, "Group Tesco Partners Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRSMS, "Group Tesco Partners SMS Checkbox in Join Page");
                }
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_51")]
        [TestCategory("P0_Regression")]
        [TestCategory("Join_OptIn")]
        public void Join_SelectAllCheckBoxes()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEJOINPAGE);
            if (isPresent == "N")
            {
                objJoin.fillMandatoryFields(Enums.FieldType.Valid);
                objJoin.acceptLegalPolicy();
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
                string isGridFormat = webConfig.Value;

                WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
                string isOptInBehaviour = webConfig1.Value;

                if (isGridFormat == "false" && isOptInBehaviour == "true")
                {
                    string country = CountrySetting.country;
                    if (country.Equals("UK"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("PL"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKGROUPRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("CZ"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("SK"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("MY"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("HU"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    else if (country.Equals("TH"))
                    {
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKTESCOOFFER, "Tesco Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKPARTNEROFFER, "Partner Offer Checkbox in Join Page");
                        objGeneric.ClickElement(ControlKeys.JOIN_CHKRESEARCH, "Partner Offer Checkbox in Join Page");
                    }
                    //objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                    //objJoin.verifyConfirmMessage();
                }
                else if (isGridFormat == "true" && isOptInBehaviour == "true")
                {
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGMAIL, "Group Tesco Products Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGEMAIL, "Group Tesco Products E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGPHONE, "Group Tesco Products Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTGSMS, "Group Tesco Products SMS Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPMAIL, "Group Tesco Partners Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPEMAIL, "Group Tesco Partners E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPPHONE, "Group Tesco Partners Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKTPSMS, "Group Tesco Partners SMS Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRMAIL, "Group Tesco Partners Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKREMAIL, "Group Tesco Partners E-Mail Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRPHONE, "Group Tesco Partners Phone Checkbox in Join Page");
                    objGeneric.ClickElement(ControlKeys.JOIN_CHKRSMS, "Group Tesco Partners SMS Checkbox in Join Page");
                }
                objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, "Join Page");
                objJoin.verifyConfirmMessage();
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                customLogs.LogInformation("Test case not applicable as Join page not present ");
            }
            customLogs.LogInformation(endMessage);
        }


        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }
    }
}
