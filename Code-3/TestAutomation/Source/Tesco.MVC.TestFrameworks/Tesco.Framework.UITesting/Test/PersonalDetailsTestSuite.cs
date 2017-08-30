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
using Tesco.NGC.RestClient;

namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class PersonalDetailsTestSuite
    {
        private bool _AddressAPIEnabled = false;

        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        LocatorServiceAdaptor objLocator = null;
        AddressAPIAdapter _objAddressService = null;
        PreferenceServiceAdaptor objPrefService = null;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        Join objJoin = null;
        PersonalDetails objPersonalDetails = null;
        private static string beginMessage = "********************* My Personal Details ****************************";
        private static string suiteName = "Personal Details";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();

        static TestData_PersonalDetails testData_Personal = null;
        static TestDataHelper<TestData_PersonalDetails> ADTestData_Personal = new TestDataHelper<TestData_PersonalDetails>();

        static TestData_JoinDetails testData_Join = null;
        static TestDataHelper<TestData_JoinDetails> TestDataHelper_Join = new TestDataHelper<TestData_JoinDetails>();
        static string culture;

        public PersonalDetailsTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.PERSONALDETAILSSUITE);
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

            ADTestData_Personal.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_PersonalDetails).Name, SanityConfiguration.Domain);
            testData_Personal = ADTestData_Personal.TestData;

            TestDataHelper_Join.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_JoinDetails).Name, SanityConfiguration.Domain);
            testData_Join = TestDataHelper_Join.TestData;
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
                    objAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl); ;
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
            objJoin = new Join(objAutomationHelper, SanityConfiguration, testData_Join);
            objGeneric = new Generic(objAutomationHelper);
            //objPersonalDetails = new PersonalDetails(objAutomationHelper);
            objPersonalDetails = new PersonalDetails(objAutomationHelper, SanityConfiguration, testData_Personal);
            objLocator = new LocatorServiceAdaptor();
            bool bAddressAPIEnabled = false;

            var addressAPICfg = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.IS_ADDRESSAPI_ENABLED, SanityConfiguration.DbConfigurationFile);

            this._AddressAPIEnabled = bool.TryParse(addressAPICfg.ConfigurationValue1, out bAddressAPIEnabled) && bAddressAPIEnabled && !String.IsNullOrWhiteSpace(addressAPICfg.ConfigurationValue2);

            if (this._AddressAPIEnabled)
            {
                _objAddressService = new AddressAPIAdapter(addressAPICfg.ConfigurationValue2);
            }

            objPrefService = new PreferenceServiceAdaptor();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("To save changes in personal details and validate the message")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("3435-TH")]
        [TestCategory("MVC")]
        [TestCategory("PersonalDetails")]
        [TestCategory("PersonalDetails_AddressAPI")]
        [Priority(0)]
        public void PersonalDetails_Savechanges()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "true")
                {
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, FindBy.CSS_SELECTOR_ID);
                }
                else
                {
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, FindBy.CSS_SELECTOR_ID);

                }
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_60")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_EnterInvalidPostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    string isRegexApplicable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORPOSTCODE, SanityConfiguration.DbConfigurationFile).IsDeleted;
                    if (isRegexApplicable == "N")
                    {
                        bool bIsValidPostCode = false;

                        if (this._AddressAPIEnabled)
                        {
                            bIsValidPostCode = this._objAddressService.IsValidPostCode(testData_Personal.InvalidPostcode);
                        }
                        else
                        {
                            bIsValidPostCode = objLocator.ValidatePostcode(testData_Personal.InvalidPostcode);
                        }

                        if (!bIsValidPostCode)
                        {
                            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                            objPersonalDetails.EnterPostcode(testData_Personal.InvalidPostcode);
                            objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                            objPersonalDetails.ValidateErrorMessage();
                        }
                        else
                            Assert.Fail("Postcode Valid");
                    }
                    else
                        Assert.Inconclusive(string.Format("Regex check not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_61")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_EnterValidPostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {

                    string isRegexApplicable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORPOSTCODE, SanityConfiguration.DbConfigurationFile).IsDeleted;
                    if (isRegexApplicable == "N")
                    {
                        bool bIsValidPostCode = false;

                        if (this._AddressAPIEnabled)
                        {
                            bIsValidPostCode = this._objAddressService.IsValidPostCode(testData_Personal.ValidPostode);
                        }
                        else
                        {
                            bIsValidPostCode = objLocator.ValidatePostcode(testData_Personal.ValidPostode);
                        }

                        if (bIsValidPostCode)
                        {
                            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                            objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                            objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");

                            if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                            {
                                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                            }
                            else
                            {
                                Assert.IsNotNull("Drop Down for House Address not Present");
                            }
                        }
                        else
                        {
                            Assert.Fail("Postcode Not valid");
                        }
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Regex check not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_62")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_EnterOutOfRangePostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    string isLengthCheckEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.REGEXFORPOSTCODE, SanityConfiguration.DbConfigurationFile).IsDeleted;
                    if (isLengthCheckEnabled == "N")
                    {
                        string OutOfRangePostcode = objPersonalDetails.SelectPostCodeOutOfRange(testData_Personal.ValidPostode);
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objPersonalDetails.EnterPostcode(OutOfRangePostcode);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                        objPersonalDetails.ValidateErrorMessage();
                    }
                    else
                        Assert.Inconclusive(string.Format("Length check not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_63")]
        [Owner("Infosys")]
        [TestCategory("Personal")]        
        public void PersonalDetails_ValidatePostCodeSection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.validatePostCodeSection();
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_64")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSpaceInPostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                    objPersonalDetails.verifySpaceInPostcode();
                    if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                        driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                    else
                        Assert.IsNotNull("Drop Down for House Address not Present");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_65")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSpaceComesAutomaticallyInPostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.EnterPostcode(testData_Personal.SpacePostcode);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                    objPersonalDetails.verifySpaceInPostcode();
                    if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                        driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                    else
                        Assert.IsNotNull("Drop Down for House Address not Present");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_66")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSucessfullPostCode()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    bool bIsValidPostCode = false;
                    if (this._AddressAPIEnabled)
                    {
                        bIsValidPostCode = this._objAddressService.IsValidPostCode(testData_Personal.ValidPostode);
                    }
                    else
                    {
                        bIsValidPostCode = objLocator.ValidatePostcode(testData_Personal.ValidPostode);
                    }

                    if (bIsValidPostCode)
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");

                        if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                            driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                        else
                            Assert.IsNotNull("Drop Down for House Address not Present");
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNSAVECHANGES, "Save Changes");
                        objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPersonalDetails.VerifyPostcodeInDB(testData.MainAccount.Clubcard);
                    }
                    else
                        Assert.Fail("Postcode InValid");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("to verify Check/amend message")]
        [Owner("Bhim")]
        [TestCategory("")]
        public void PersonalDetails_VerifyCheckamendyourpersonaldetails()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objPersonalDetails.Checkamend();
        }

        [TestMethod]
        [Description("to verify Required Warining message")]
        [TestCategory("")]
        public void PersonalDetails_VerifyRequiredwarningtxt()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.REQUIREDWARNINGText();
        }

        [TestMethod]
        [Description("to verify Your Contact detail Text")]
        [TestCategory("")]
        public void PersonalDetails_Verifyyourcontactdetailstxt()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.YCDetailsTXT();
        }

        //[TestMethod]
        //[Description("to verify page Descrption message")]
        //[TestCategory("")]
        //public void PersonalDetails_VerifyPageDescriptiontxt()
        //{
        //    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
        //    objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
        //    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
        //    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
        //    objPersonalDetails.PageDecriptionText();
        //}

        [TestMethod]
        [Description("to verify customer Details")]
        [TestCategory("")]
        public void PersonalDetails_VerifyCustomerDetails()
        {
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.ReturnAllFieldValues();
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_90")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]
        public void PersonalDetails_VerifyProvinceLocaleDropDown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isProvincedenabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROVINCEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (groupCountryEnabled.Equals("1") && isProvincedenabled.Equals("1"))
                {
                    string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (isLocaleEnabled.Equals("TRUE"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        if (objPersonalDetails.CheckProvice(testData.MainAccount.Clubcard, ControlKeys.PERSONALDETAILS_PROVINCE, Services.Enums.CustPersonalDetail.ProvinceLocale))
                            customLogs.LogInformation("Province drop down verified");
                        else
                            Assert.Fail("Province drop down not verified");
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Locale is off for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country address or province not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_91,92")]
        [Owner("Infosys")]
        [TestCategory("P0Set1")]
        [TestCategory("P0")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]
        public void PersonalDetails_SuccessfulProvinceSelection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isProvincedenabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROVINCEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (groupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE5) && isProvincedenabled.Equals("1"))
                {
                    string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (isLocaleEnabled.Equals("TRUE"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                          objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                          objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                          objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                           objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                          //Select any value from drop down
                          objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.AnyOption);
                          objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                          objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                          objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                          objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                          //Select -Select Province- value from drop down
                          objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.SelectOption);
                          objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                          objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Locale is off for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country address or province not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_93")]
        [Owner("Infosys")]
        [TestCategory("P0Set1")]
        [TestCategory("P0")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]
        public void PersonalDetails_VerifyProvinceEnglishDropDown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
             if (isPresent)
             {
                 string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                 string isProvincedenabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROVINCEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                 if (groupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE5) && isProvincedenabled.Equals("1"))
                 {
                     string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                     if (isLocaleEnabled.Equals("FALSE"))
                     {
                         objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                         objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                         objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                         objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                         objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                         if (objPersonalDetails.CheckProvice(testData.MainAccount.Clubcard, ControlKeys.PERSONALDETAILS_PROVINCE, Services.Enums.CustPersonalDetail.ProvinceEnglish))
                             customLogs.LogInformation("Province drop down verified");
                         else
                             Assert.Fail("Province drop down not verified");
                     }
                     else
                     {
                         Assert.Inconclusive(string.Format("Locale is on for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                     }
                 }
                 else
                 {
                     Assert.Inconclusive(string.Format("Either group country address or province not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                 }
             }
             else
             {
                 Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_94,95")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]

        public void PersonalDetails_SuccessfulProvinceSelectionLocaleOff()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isProvincedenabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROVINCEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (groupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE5) && isProvincedenabled.Equals("1"))
                {
                    string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (isLocaleEnabled.Equals("FALSE"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);

                        //Select any value from drop down
                        objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.AnyOption);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                        objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        //Select -Select Province- value from drop down
                        objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.SelectOption);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                        objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Locale is on for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country address or province not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_96,97,98,124")]
        [Owner("Infosys")]
        [TestCategory("3435-TH")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]

        [TestCategory("ProvinceOffGroupEnabled")]
        public void PersonalDetails_VerifyAddressLine5AsTextBox()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isProvincedenabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROVINCEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                string isAddressLine5Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.MAILINGADDRESSLINE5, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (groupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE5) && isProvincedenabled.Equals("0"))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //Testcase96---To check if AddressLine5 as textbox exists or not
                    objPersonalDetails.checkIfElementPresent(ControlKeys.PERSONALDETAILS_ADDRESSLINE5);
                    //TestCase 97---Enter details and save
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE5, "AddressLine5");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //TestCase 98---Delete details and save
                    driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE5).Id)).Clear();
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //To enter length less than min length in dbconfig
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    int stringLength = objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE5, ControlKeys.PERSONALDETAILS_ADDRESSLINE5, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine5Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE5, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE5, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //TestCase-124---Enter Invalid data and check error message
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine5))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE5, testData_Personal.InvalidMailingAddressLine5);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE5, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE5, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);

                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country address or addressline5 as textbox not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_99,100,101,102")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]

        public void PersonalDetails_VerifyRaceLocaleDropDown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDERACE))
                {
                    string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (isLocaleEnabled.Equals("TRUE"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);

                        //Test Case 99--To check if Race drown down is visible
                        objPersonalDetails.checkIfElementPresent(ControlKeys.PERSONALDETAILS_RACE);
                        //Test case-100
                        if (objPersonalDetails.CheckProvice(testData.MainAccount.Clubcard, ControlKeys.PERSONALDETAILS_RACE, Services.Enums.CustPersonalDetail.RaceLocale))
                            customLogs.LogInformation("Race drop down verified");
                        else
                            Assert.Fail("Race drop down not verified");
                        //Test case-101-Select any value from drop down
                        objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_RACE, Enums.DropDownValue.AnyOption);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                        objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        //Select -Select Race- value from drop down
                        objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_RACE, Enums.DropDownValue.SelectOption);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                        objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Locale is off for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Race not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_101")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]

        public void PersonalDetails_VerifyRaceEnglishDropDown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDERACE))
                {
                    string isLocaleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.LOCALEENABLED, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (isLocaleEnabled.Equals("FALSE"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        if (objPersonalDetails.CheckProvice(testData.MainAccount.Clubcard, ControlKeys.PERSONALDETAILS_RACE, Services.Enums.CustPersonalDetail.RaceEnglish))
                            customLogs.LogInformation("Race drop down verified");
                        else
                            Assert.Fail("Race drop down not verified");
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Locale is on for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Race not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_22")]
        [Description("To fill all mandatory details and click confirm")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set5")]

        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_FillMandatoryDetails()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPersonalDetails.SetPersonalDetailFields(FieldType.Mandatory);
                objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_23,29,53,57")]
        [Description("To fill all mandatory and optional details and click confirm")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0Set5")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_FillAllDetails()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPersonalDetails.SetPersonalDetailFields(FieldType.All);
                objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_24")]
        [Description("Update surname and check updation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_SurnameReplacementValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isReplacementTextEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.REPLACEMENTTEXT, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isOrderAReplacementPageEnabled = objGeneric.IsPageEnabled(DBConfigKeys.HIDEORDERAREPLACEMENTPAGE);
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME3) && isReplacementTextEnabled.Equals("1") && isOrderAReplacementPageEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    int length = objGeneric.returnConfigLength(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.NAME3LENGTH, Enums.FieldType.Valid);
                    string SurnameText = objPersonalDetails.RandomString(length);
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_SURNAME, SurnameText);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objPersonalDetails.VerifyReplacementText();
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either Name3 or replacement text not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_25")]
        [Description("Enter FirstName in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_VerifyFirstNameValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            string regEx = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.NAME1, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            bool isRegexCheckEnabled = !string.IsNullOrEmpty(regEx);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME1))
                {
                    if (isRegexCheckEnabled)
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME1LENGTH, testData_Personal.InvalidName1))
                        {
                            objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_FIRSTNAME, testData_Personal.InvalidName1);
                        }
                        else
                        {
                            Assert.Fail(string.Format("'{0}' is Valid . Kindly use input that doesn't matches the regular expression '{1}'", testData_Personal.InvalidName1, regEx));
                        }
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORNAME1, ControlKeys.PERSONALDETAILS_ERRORNAME1, "PD Name1 error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Regex Check not present for Name1 in country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Name1 field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_26")]
        [Description("Enter MiddleName in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_VerifyMiddleNameValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            string regEx = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.NAME2, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            bool isRegexCheckEnabled = !string.IsNullOrEmpty(regEx);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME2))
                {
                    if (isRegexCheckEnabled)
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        //  driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME2LENGTH, testData_Personal.InvalidName2))
                        {
                            objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_MIDDLENAME, testData_Personal.InvalidName2);
                        }
                        else
                        {
                            Assert.Fail(string.Format("'{0}' is Valid . Kindly use input that doesn't matches the regular expression '{1}", testData_Personal.InvalidName2, regEx));
                        }
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORNAME2, ControlKeys.PERSONALDETAILS_ERRORNAME2, "PD Name2 error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("Regex Check not present for Name2 in country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }

                }
                else
                {
                    Assert.Inconclusive(string.Format("Name2 field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_27")]
        [Description("Enter Surname in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_VerifySurNameValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            string isRegexCheckEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.NAME3, SanityConfiguration.DbConfigurationFile).IsDeleted;
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME3))
                {
                    if (isRegexCheckEnabled == "N")
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME3LENGTH, testData_Personal.InvalidName3))
                            objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_SURNAME, testData_Personal.InvalidName3);
                        else
                            Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORNAME3, ControlKeys.PERSONALDETAILS_ERRORNAME3, "PD Name3 error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }

                    else
                    {
                        Assert.Inconclusive(string.Format("Regex Check not present for Name3 in country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Name1 field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_28")]
        [Description("Enter DOB in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("DOB")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set4")]
        public void PersonalDetails_DOBErrorValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEDOB))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    // driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_YEAR, Enums.DropDownValue.SelectOption);
                    objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_DAY, Enums.DropDownValue.AnyOption);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDOB, ControlKeys.PERSONALDETAILS_ERRORDOB, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("DOB field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_51,52")]
        [Description("Enter Mobile Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_ConfLengthCheckForMobileNumber()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEMOBILENUMBER))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    int length  = objPersonalDetails.EnterInvalidLength(DBConfigKeys.MOBILENUMBERPREFIX, ControlKeys.PERSONALDETAILS_MOBILENUMBER, Enums.FieldType.InvalidLength1, Enums.JoinElements.MobilePhoneNumber.ToString());
                    if (length == 0)
                    {
                        Assert.Inconclusive(string.Format("Mobile number field not have min length for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORMOBILENUMBER, ControlKeys.PERSONALDETAILS_ERRORMOBILENUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Mobile number field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_55,56")]
        [Description("Enter DayTime Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_ConfLengthCheckForDayTimeNumber()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEDAYTIMENUMBER))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    int length  = objPersonalDetails.EnterInvalidLength(DBConfigKeys.DAYNUMBERPREFIX, ControlKeys.PERSONALDETAILS_DAYTIMENUMBER, Enums.FieldType.InvalidLength1, Enums.JoinElements.DayTimePhoneNumber.ToString());
                    if (length == 0)
                    {
                        Assert.Inconclusive(string.Format("Day number field not have min length for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDAYTIMENUMBER, ControlKeys.PERSONALDETAILS_ERRORDAYNUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Day number field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_58,59")]
        [Description("Enter Evening Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set3")]
        public void PersonalDetails_ConfLengthCheckForEveningNumber()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEEVENINGNUMBER))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    int length = objPersonalDetails.EnterInvalidLength(DBConfigKeys.EVENINGNUMBERPREFIX, ControlKeys.PERSONALDETAILS_EVENINGNUMBER, Enums.FieldType.InvalidLength1, Enums.JoinElements.EveningPhoneNumber.ToString());
                    if (length == 0)
                    {
                        Assert.Inconclusive(string.Format("Evening number field not have min length for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                    }
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERROREVENINGNUMBER, ControlKeys.PERSONALDETAILS_ERROREVNGNUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Evening number field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_68")]
        [Description("Enter email in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set4")]
        public void PersonalDetails_RegexCheckForEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);

                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFOREMAIL, testData_Personal.InvalidEmailAddress))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_EMAIL, testData_Personal.InvalidEmailAddress);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERROREMAIL, ControlKeys.PERSONALDETAILS_ERROREMAIL, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Email field not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_69")]
        [Description("Enter title and gender that does not match and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set4")]
        public void PersonalDetails_TitleGenderMismatchError()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEGENDER) && objGeneric.IscontrolVisible(DBConfigKeys.HIDETITLE))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.selectOptionFromDropDown(ControlKeys.PERSONALDETAILS_TITLE, DropDownValue.SelectOption);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE, "PD");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORGENDERMISMATCH, ControlKeys.PERSONALDETAILS_ERRORGENDERMISMATCH, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either Gender or Title not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_70,71")]
        [Description("Duplicate check for Email")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_DuplicateCheck()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isDuplicacyEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DUPLICATECHECKREQUIRED);
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL) && isDuplicacyEnabled.Equals("TRUE"))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.SetPersonalDetailFields(FieldType.All);
                    objPersonalDetails.EnterDuplicateData();
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDUPLICACY, ControlKeys.JOIN_ERRORPROFANE, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Duplicacy not for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_72")]
        [Description("Enter profane word in field  and validate error message")]
        [Owner("Infosys")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set4")]
        public void PersonalDetails_ProfanityCheck()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isProfanityEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PROFANITYREQUIRED);
                string isProfanityOnName1 = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME1);
                if (isProfanityEnabled.Equals("TRUE"))
                {
                    if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME1) && isProfanityOnName1.Equals("1"))
                    {
                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        //  driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_FIRSTNAME, testData_Personal.ProfaneName1);
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORPROFANITY, ControlKeys.JOIN_ERRORPROFANE, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    else
                        customLogs.LogInformation("Name1 not enabled or profanity for Name1 not enabled for" + CountrySetting.country);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_119")]
        [Description("Enter invalid data in addressLine1 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_InvalidAddressLine1ErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isAddressLine1Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.MAILINGADDRESSLINE1, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isAddressLineRegEx = !string.IsNullOrEmpty(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                if (isGroupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE1) && isAddressLineRegEx)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //To enter length less than min length in dbconfig
                    int stringLength = objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE1, ControlKeys.PERSONALDETAILS_ADDRESSLINE1, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine1Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE1, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE1, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //To enter data that has invalid regex
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine1))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE1, testData_Personal.InvalidMailingAddressLine1);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE1, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE1, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country or addressLine1 or regex not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_120")]
        [Description("Enter invalid data in addressLine2 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_InvalidAddressLine2ErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isAddressLine2Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.MAILINGADDRESSLINE2, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isAddressLineRegEx = !string.IsNullOrEmpty(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                if (isGroupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE2) && isAddressLineRegEx)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //To enter length less than min length in dbconfig
                    int stringLength = objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE2, ControlKeys.PERSONALDETAILS_ADDRESSLINE2, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine2Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE2, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE2, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //To enter data that has invalid regex
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine2))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE2, testData_Personal.InvalidMailingAddressLine2);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE2, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE2, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country or addressLine2 or regex not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_121")]
        [Description("Enter invalid data in addressLine3 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_InvalidAddressLine3ErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isAddressLine3Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.MAILINGADDRESSLINE3, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isAddressLineRegEx = !string.IsNullOrEmpty(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                if (isGroupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE3) && isAddressLineRegEx)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //To enter length less than min length in dbconfig
                    int stringLength = objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE3, ControlKeys.PERSONALDETAILS_ADDRESSLINE3, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine3Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE3, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE3, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //To enter data that has invalid regex
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine3))
                    {
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE3, testData_Personal.InvalidMailingAddressLine3);
                    }
                    else
                    {
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    }
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, FindBy.CSS_SELECTOR_ID);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE3, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE3, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country or addressLine3 or Regex not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_122")]
        [Description("Enter invalid data in addressLine4 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_InvalidAddressLine4ErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isAddressLine4Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.MAILINGADDRESSLINE4, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isAddressLineRegEx = !string.IsNullOrEmpty(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                if (isGroupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE4) && isAddressLineRegEx)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //To enter length less than min length in dbconfig
                    int stringLength=objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE4, ControlKeys.PERSONALDETAILS_ADDRESSLINE4, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine4Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE4, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE4, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //To enter data that has invalid regex
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine4))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE4, testData_Personal.InvalidMailingAddressLine4);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE4, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE4, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country or addressLine4 or regex not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_124")]
        [Description("Enter invalid data in addressLine6 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]

        public void PersonalDetails_InvalidAddressLine6ErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isAddressLine6Mandatory = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, DBConfigKeys.LN_ADDRESSLINE6, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                bool isAddressLineRegEx = !string.IsNullOrEmpty(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                if (isGroupCountryEnabled.Equals("1") && objGeneric.IscontrolVisible(DBConfigKeys.HIDEADDRESSLINE6) && isAddressLineRegEx)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");

                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    //To enter length less than min length in dbconfig
                    int stringLength = objPersonalDetails.EnterInvalidLength(DBConfigKeys.LN_ADDRESSLINE6, ControlKeys.PERSONALDETAILS_ADDRESSLINE6, Enums.FieldType.InvalidLength1, Enums.JoinElements.MailingAddressLine1.ToString());
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    if (!stringLength.Equals(0) && isAddressLine6Mandatory.Equals("0"))
                    {
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_LNERRORADDRESSLINE6, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE6, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    }
                    //To enter data that has invalid regex
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine6))
                        objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE6, testData_Personal.InvalidMailingAddressLine6);
                    else
                        Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE6, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE6, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Either group country or addressLine6 or regex not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_112,113,114,115")]
        [Description("Enter invalid phone number and validate error message")]
        [Owner("Infosys")]
        [TestCategory("P1")]
        [TestCategory("3435-TH")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set2")]
        public void PersonalDetails_InvalidPhoneNumberErrorVerification()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEMOBILENUMBER))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_MOBILENUMBER, testData_Personal.InvalidMobilePhoneNumber);
                    objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORMOBILENUMBER, ControlKeys.PERSONALDETAILS_ERRORMOBILENUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("MobileNumber not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To verify Titles in your household details section")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]

        public void PersonalDetails_HouseholdAgeTitles()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {

                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objJoin.YourHouseholdAge_Titles();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "TRUE", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify Age in You Field in yous household section")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P1Set4")]
        public void PersonalDetails_HouseholdAge_You()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {

                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.HouseholdAgeValue_You();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "TRUE", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the values in the dropdown in your household section")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set1")]
        [TestCategory("PersonalDetails")]

        public void PersonalDetails_HouseholdAge_DropDown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objJoin.HouseholdAgeDropdownValues();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Household members ages displayed in your household section")]
        [TestCategory("P0")]

        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0Set1")]
        public void PersonalDetails_HouseholdAge_ages()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {

                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.HouseholdAges();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isAvailable, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_36")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0Set3")]

        public void PersonalDetails_OptinDietaryNeedsDiabetic()
        {

            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 1))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDDIABETIC, Enums.OptStatus.OptIn.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 1, Enums.OptStatus.OptIn.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted in for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_37")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeedsDiabetic()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);

            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (!objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 1))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDDIABETIC, Enums.OptStatus.OptOut.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 1, Enums.OptStatus.OptOut.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted out for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));

            }
            driver.Quit();
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_38")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptinDietaryNeedsKosher()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 2))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDKOSHER, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDKOSHER, Enums.OptStatus.OptIn.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 2, Enums.OptStatus.OptIn.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted in for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_39")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeedsKosher()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (!objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 2))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDKOSHER, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDKOSHER, Enums.OptStatus.OptOut.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 2, Enums.OptStatus.OptOut.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted out for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_40")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptinDietaryNeedsHalal()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 3))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDHALAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDHALAL, Enums.OptStatus.OptIn.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 3, Enums.OptStatus.OptIn.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted in for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_41")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeedsHalal()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (!objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 3))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDHALAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDHALAL, Enums.OptStatus.OptOut.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 3, Enums.OptStatus.OptOut.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted out for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_42")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptinDietaryNeedsVegeterian()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 4))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDVEGETERIAN, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDVEGETERIAN, Enums.OptStatus.OptIn.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 4, Enums.OptStatus.OptIn.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted in for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_43")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeedsVegeterian()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (!objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 4))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDVEGETERIAN, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDVEGETERIAN, Enums.OptStatus.OptOut.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 4, Enums.OptStatus.OptOut.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted out for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_44")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptinDietaryNeedsTeeTotal()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 5))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDTEATOTAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDTEATOTAL, Enums.OptStatus.OptIn.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 5, Enums.OptStatus.OptIn.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted in for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_45")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeedsTeeTotal()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    if (!objPrefService.CheckDietaryPreference_optin(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 5))
                    {
                        objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                        objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDTEATOTAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDTEATOTAL, Enums.OptStatus.OptOut.ToString());
                        objGeneric.ClickElementJavaElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                        objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                        objPrefService.GetPreference_dietary(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture, 5, Enums.OptStatus.OptOut.ToString());
                    }
                    else
                        Assert.Inconclusive(string.Format("Dieatary Preference already opted out for country: {0}, Clubcard : {1}", CountrySetting.country, testData.DietaryPreferenceAccount));
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_122")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("Personal_dietary")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        public void PersonalDetails_OptoutDietaryNeeds()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {
                    List<string> valuesFromServices = objPrefService.CheckDietaryPreferencesinDB(testData.DietaryPreferenceAccount.Clubcard, CountrySetting.culture);
                    objLogin.Login_Verification(testData.DietaryPreferenceAccount.Clubcard, testData.DietaryPreferenceAccount.Password, testData.DietaryPreferenceAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.DietaryPreferenceAccount.Clubcard);
                    if (objPersonalDetails.PersonalDetails_DietaryNeedUICheck(valuesFromServices))
                        customLogs.LogInformation(" Dietary needs matching withDB");
                    else
                        Assert.Fail("Dietary needs not matching with DB");
                }
                else
                {
                    Assert.Inconclusive(string.Format("Dieatary Preference not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_122")]
        [Owner("Infosys")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_PersonalDetails")]
        [TestCategory("P0Set3")]
        [TestCategory("P0_Personaldetails_Sync50")]
        public void PersonalDetails_Sync50_Validateaddressfromconatctservice()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            //  DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.IsEnterpriceServiceCallsEnabled, SanityConfiguration.DbConfigurationFile);

            string config1 = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.IsEnterpriceServiceCallsEnabled);

            if ((isPresent == true) && (config1 == "1"))
            {
                string Ismerged = objLogin.IsmergedCustomer("IsMerged");
                if (Ismerged.Contains("true"))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    string OauthtokenID = objLogin.LoginBrowserCookie("OAuth.AccessToken");
                    string[] splitCookie = OauthtokenID.Split('=');
                    AddressBook address = objPersonalDetails.contactservice("topic", splitCookie[1]);
                    objPersonalDetails.ValidateMethod(ControlKeys.PERSONALDETAILS_FLD_ADDRESS, address.addressLines.lines.text.ToUpper());
                    objPersonalDetails.ValidateMethod(ControlKeys.PERSONALDETAILS_FLD_ADDRESS, address.addressLines.town.ToUpper());
                }
                else
                {

                    Assert.Inconclusive(string.Format("The customer is not part of the segment", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_124")]
        [TestCategory("Personal")]
        [TestCategory("PersonalDetails_AddressAPI")]
        public void PersonalDetails_SearchPartialPC()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
                string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
                if (isGroupCountryEnabled == "0" && isPostCodeEnabled != "1")
                {
                    bool bIsValidPostCode = false;

                    if (this._AddressAPIEnabled)
                    {
                        bIsValidPostCode = this._objAddressService.IsValidPostCode(testData_Personal.PartialPostcode);

                        objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                        objPersonalDetails.EnterPostcode("");
                        objPersonalDetails.EnterPostcode(testData_Personal.PartialPostcode);

                        List<IWebElement> address = driver.FindElements(By.ClassName("ui-menu-item")).ToList();
                                                                        
                        if (address == null)
                        {
                            Assert.Fail("Partial Post Code Search failed");
                        }
                        else
                        {
                            List<string> receivedAddresses = address.Select(a => a.Text).ToList<string>();
                            List<string> expectedAddresses = this._objAddressService.SearchPostcodes(testData_Personal.PartialPostcode);

                            NUnit.Framework.CollectionAssert.AreEqual(receivedAddresses.AsEnumerable(), expectedAddresses.Take(receivedAddresses.Count));
                        }
                    }
                    else
                    {
                        Assert.Inconclusive("Address API is not configured.");
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("Find Address not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Text present in Household section")]
        [TestCategory("P2")]
        [TestCategory("P2_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_VerifyHouseholdSectionText()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);

            if (isPresent)
            {
                string isAvailable = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLEDIEATARYPREFERENCE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isAvailable.Equals("FALSE"))
                {

                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    string error = objJoin.ValidateHouseHoldTest(driver);
                    objJoin.YourHouseholdAge_Titles();
                    if (!string.IsNullOrEmpty(error))
                    {
                        Assert.Fail(error);
                    }
                }
                else
                {
                    Assert.AreEqual(isAvailable, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify all titles are available in title dropdown")]
        [TestCategory("P2")]
        [TestCategory("P2_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_VerifyTitleDropdown()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent)
            {
                string isTitleEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (isTitleEnabled.Equals("0"))
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objPersonalDetails.verifyTitle(ControlKeys.PERSONALDETAILS_TITLE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Title is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Personal Details link not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
