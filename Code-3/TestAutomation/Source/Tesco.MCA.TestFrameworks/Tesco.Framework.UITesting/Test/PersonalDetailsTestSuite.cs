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
    public class PersonalDetailsTestSuite
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        LocatorServiceAdaptor objLocator = null;
        PreferenceServiceAdaptor objPrefService = null;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        PersonalDetails objPersonalDetails = null;
        private static string beginMessage = "********************* My Personal Details ****************************";
        private static string suiteName = "Personal Details";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();

        static TestData_PersonalDetails testData_Personal = null;
        static TestDataHelper<TestData_PersonalDetails> ADTestData_Personal = new TestDataHelper<TestData_PersonalDetails>();
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
            //objPersonalDetails = new PersonalDetails(objAutomationHelper);
            objPrefService = new PreferenceServiceAdaptor();
            objPersonalDetails = new PersonalDetails(objAutomationHelper, SanityConfiguration, testData_Personal);
            objLocator = new LocatorServiceAdaptor();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_60")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_EnterInvalidPostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                string isPresent = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORPOSTCODE);
                if (isPresent == "N")
                {
                    if (!objLocator.ValidatePostcode(testData_Personal.InvalidPostcode))
                    {
                        objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                        objLogin.SecurityLayer_Verification(testData.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.Clubcard);
                        objPersonalDetails.EnterPostcode(testData_Personal.InvalidPostcode);
                        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                        objPersonalDetails.ValidateErrorMessage();
                    }
                    else
                        Assert.Fail("Postcode Valid");
                }
                else
                    Assert.Fail("Check db configuration file");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_61")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_EnterValidPostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                string isPresent = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORPOSTCODE);
                if (isPresent == "N")
                {
                    if (objLocator.ValidatePostcode(testData_Personal.ValidPostode))
                    {
                        objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                        objLogin.SecurityLayer_Verification(testData.Clubcard);
                        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                        objLogin.SecurityLayer_Verification(testData.Clubcard);
                        objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                        if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                            driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                        else
                            Assert.IsNotNull("Drop Down for House Address not Present");
                    }
                    else
                        Assert.Fail("Postcode Not valid");
                }
                else
                    Assert.Fail("Check db configuration file");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_62")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_EnterOutOfRangePostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                string isPresent = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.REGEXFORPOSTCODE);
                if (isPresent == "N")
                {
                    string OutOfRangePostcode = objPersonalDetails.SelectPostCodeOutOfRange(testData_Personal.ValidPostode);
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objPersonalDetails.EnterPostcode(OutOfRangePostcode);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                    objPersonalDetails.ValidateErrorMessage();
                }
                else
                    Assert.Fail("Check db configuration file");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_63")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidatePostCodeSection()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.PD_POSTCODESEC1, ControlKeys.PERSONALDETAILS_POSTCODESEC1, "PersonalDetails details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.PD_POSTCODESEC2, ControlKeys.PERSONALDETAILS_POSTCODESEC2, "PersonalDetails details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_64")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSpaceInPostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                objPersonalDetails.verifySpaceInPostcode();
                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                    driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                else
                    Assert.IsNotNull("Drop Down for House Address not Present");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_65")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSpaceComesAutomaticallyInPostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterPostcode(testData_Personal.SpacePostcode);
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");
                objPersonalDetails.verifySpaceInPostcode();
                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                    driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                else
                    Assert.IsNotNull("Drop Down for House Address not Present");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_66")]
        [Owner("Infosys")]
        [TestCategory("Personal")]
        public void PersonalDetails_ValidateSucessfullPostCode()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                if (objLocator.ValidatePostcode(testData_Personal.ValidPostode))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objPersonalDetails.EnterPostcode(testData_Personal.ValidPostode);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNPOSTCODE, "Find Address");

                    if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id), driver))
                        driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(OpenQA.Selenium.Keys.Down);
                    else
                        Assert.IsNotNull("Drop Down for House Address not Present");
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNSAVECHANGES, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPersonalDetails.VerifyPostcodeInDB(testData.Clubcard);
                }
                else
                    Assert.Fail("Postcode InValid");
            }
            else
                customLogs.LogInformation("Test case not applicable for " + CountrySetting.country);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Fill the details as Female and drop down address for UK")]
        [Owner("Bhim")]
        [TestCategory("Personal")]
        public void PersonalDetails_FillAllFemaleDetailsAndConfirm()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                //objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.fillAllFieldsMethodOne(Enums.FieldType.Valid);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "true")
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                Assert.AreEqual("Your details have been saved", ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE);
            }

        }

        [TestMethod]
        [Description("Fill the details as Male and Fill house details in address for UK")]
        [Owner("Bhim")]
        [TestCategory("Personal")]
        public void PersonalDetails_FillAllMaleDetailsAndConfirm()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                //objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.fillAllFieldsMethodTwo(Enums.FieldType.Valid);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "true")
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //Assert.AreEqual("Your details have been saved", ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE);
            }

        }

        [TestMethod]
        [Description("Fill all mandatory fields")]
        [Owner("Bhim")]
        [TestCategory("Personal")]
        public void PersonalDetails_FillMandatoryDetailsAndConfirm()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                //objLogin.SecurityLayer_Verification(testData.Clubcard);

                objPersonalDetails.fillMandatoryFields(Enums.FieldType.Valid);

                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "true")
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //Assert.AreEqual("Your details have been saved", ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE);
            }
        }

        [TestMethod]
        [Description("Verify the Tesco.com account Navigation link on Personal details text")]
        [TestCategory("")]
        [Owner("Bhim")]
        public void VerifyTheDotcomLinkOnPersonalDetailsText()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.PERSONALDETAILSTESCODOTCOMTEXT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE));
                var expectedLinkName = res.Value;
                objGeneric.ClickAndVerifyLinkNavigationBrowserURL(ControlKeys.PERSONALDETAILS_TESCODOTCOMLINKTEXT, "Personal Details Link in the text", expectedLinkName);

            }
        }

        [TestMethod]
        [Description("Verify the change password Navigation link on Personal details text")]
        [TestCategory("")]
        [Owner("Bhim")]
        public void VerifyChangePasswordLinkOnPersonalDetailsText()
        {
            string isGroupCountryEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS);
            string isPostCodeEnabled = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOSTCODE);
            if (isGroupCountryEnabled == "Y" && isPostCodeEnabled == "N")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.PERSONALDETAILSCHANGEPASSWORD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE));
                var expectedLinkName = res.Value;
                objGeneric.ClickAndVerifyLinkNavigationBrowserURL(ControlKeys.PERSONALDETAILS_CHANGEPASSWORDLINK, "Personal Details Link in the text", expectedLinkName);

            }
        }


        [TestMethod]
        [Description("to verify Check/amend message")]
        [TestCategory("Personal")]
        public void PersonalDetails_VerifyCheckamendyourpersonaldetails()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.Checkamend();
        }


        [TestMethod]
        [Description("to verify page name message")]
        [TestCategory("Personal")]
        public void PersonalDetails_VerifyPageName()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objGeneric.verifyPageName(LabelKey.PERSONALDETAILS, "My Personal Details", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
        }

        [TestMethod]
        [Description("to verify Required Warining message")]
        [TestCategory("Personal")]
        public void PersonalDetails_VerifyRequiredwarningtxt()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.REQUIREDWARNINGText();
        }

        [TestMethod]
        [Description("to verify Your Contact detail Text")]
        [TestCategory("Personal")]
        public void PersonalDetails_Verifyyourcontactdetailstxt()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.YCDetailsTXT();
        }

        [TestMethod]
        [Description("to verify page Descrption message")]
        [TestCategory("Personal")]
        public void PersonalDetails_VerifyPageDescriptiontxt()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.PageDecriptionText();
        }


        [TestMethod]
        [Description("to verify customer Details")]
        [TestCategory("Personal")]
        public void PersonalDetails_VerifyCustomerDetails()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objPersonalDetails.ReturnAllFieldValues();
            // objLogin.LogOut_Verification();
        }


        [TestMethod]
        [Description("To verify Titles in your household details section")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_HouseholdAgeTitles()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objPersonalDetails.YourHouseholdAge_Titles();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify Age in You Field in yous household section")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_HouseholdAge_You()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objPersonalDetails.HouseholdAgeValue_You();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }

            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify the values in the dropdown in your household section")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_HouseholdAge_DropDown()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objPersonalDetails.HouseholdAgeDropdownValues();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Household members ages displayed in your household section")]
        [TestCategory("P0_Regression")]
        [TestCategory("PersonalDetails")]
        public void PersonalDetails_HouseholdAge_ages()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPERSONALDETAILS);
            if (isPresent == "Y")
            {
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISABLEDIAETORYPREFERENCE, SanityConfiguration.WebConfigurationFile);
                string isDiateryDisabled = webConfig.Value;
                if (isDiateryDisabled == "false")
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objPersonalDetails.HouseholdAges();
                    // objLogin.LogOut_Verification();
                }
                else
                {
                    Assert.AreEqual(isDiateryDisabled, "true", "Configuration Value not matched with WebConfig");
                    customLogs.LogInformation("Test case not applicable as household section is disabled ");
                }
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_36")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptinDietaryNeedsDiabetic()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 1))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDDIABETIC, Enums.OptStatus.OptIn.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 1, Enums.OptStatus.OptIn.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted In");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check DB config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_37")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeedsDiabetic()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (!objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 1))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                   // objLogin.SecurityLayer_Verification(testData.Clubcard2);

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDDIABETIC, Enums.OptStatus.OptOut.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 1, Enums.OptStatus.OptOut.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted Out");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_38")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptinDietaryNeedsKosher()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 2))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDKOSHER, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDKOSHER, Enums.OptStatus.OptIn.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 2, Enums.OptStatus.OptIn.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted In");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_39")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeedsKosher()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (!objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 2))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDKOSHER, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDKOSHER, Enums.OptStatus.OptOut.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 2, Enums.OptStatus.OptOut.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted Out");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_40")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptinDietaryNeedsHalal()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 3))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDHALAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDHALAL, Enums.OptStatus.OptIn.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 3, Enums.OptStatus.OptIn.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted In");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_41")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeedsHalal()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (!objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 3))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDHALAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDHALAL, Enums.OptStatus.OptOut.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 3, Enums.OptStatus.OptOut.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted Out");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_42")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptinDietaryNeedsVegeterian()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 4))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDVEGETERIAN, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDVEGETERIAN, Enums.OptStatus.OptIn.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 4, Enums.OptStatus.OptIn.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted In");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_43")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeedsVegeterian()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (!objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 4))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDVEGETERIAN, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDVEGETERIAN, Enums.OptStatus.OptOut.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 4, Enums.OptStatus.OptOut.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted Out");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_44")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptinDietaryNeedsTeeTotal()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 5))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDTEATOTAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDTEATOTAL, Enums.OptStatus.OptIn.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 5, Enums.OptStatus.OptIn.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted In");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_45")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeedsTeeTotal()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                if (!objPrefService.CheckDietaryPreference_optin(testData.Clubcard2, CountrySetting.culture, 5))
                {
                    objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard2);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");

                    objPersonalDetails.PersonalDetails_CheckDietaryNeeds(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDTEATOTAL, ControlKeys.PERSONALDETAILS_CHKDIETARYNEEDTEATOTAL, Enums.OptStatus.OptOut.ToString());
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objPrefService.GetPreference_dietary(testData.Clubcard2, CountrySetting.culture, 5, Enums.OptStatus.OptOut.ToString());
                }
                else
                    customLogs.LogInformation("Dietary Preference already Opted Out");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_122")]
        [Owner("Infosys")]
        [TestCategory("P0_Regression")]
        [TestCategory("Personal_dietary")]
        public void PersonalDetails_OptoutDietaryNeeds()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
            {
                List<string> valuesFromServices = objPrefService.CheckDietaryPreferencesinDB(testData.Clubcard2, CountrySetting.culture);
                objLogin.Login_Verification(testData.Clubcard2, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard2);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                if (objPersonalDetails.PersonalDetails_DietaryNeedUICheck(valuesFromServices))
                    customLogs.LogInformation(" Dietary needs matching withDB");
                else
                    Assert.Fail("Dietary needs not matching with DB");
            }
            else
                customLogs.LogInformation("Test cases not applicable. Please check Web config");
        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_90")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]
        public void PersonalDetails_VerifyProvinceLocaleDropDown()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
            {
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    if (objPersonalDetails.CheckProvice(testData.Clubcard, ControlKeys.PERSONALDETAILS_PROVINCE, Services.Enums.CustPersonalDetail.ProvinceLocale))
                        customLogs.LogInformation("Province drop down verified");
                    else
                        Assert.Fail("Province drop down not verified");

                }
            }
        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_91,92")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]
        public void PersonalDetails_SuccessfulProvinceSelection()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
            {
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    //Select any value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.AnyOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    //Select -Select Province- value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.SelectOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
            }
        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_93")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]
        public void PersonalDetails_VerifyProvinceEnglishDropDown()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
            {
                if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {

                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    if (objPersonalDetails.CheckProvice(testData.Clubcard, ControlKeys.PERSONALDETAILS_PROVINCE, Services.Enums.CustPersonalDetail.ProvinceEnglish))
                        customLogs.LogInformation("Province drop down verified");
                    else
                        Assert.Fail("Province drop down not verified");
                }
            }
        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_94,95")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]
        public void PersonalDetails_SuccessfulProvinceSelectionLocaleOff()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
            {
                if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    //Select any value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.AnyOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //Select -Select Province- value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_PROVINCE, DropDownValue.SelectOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
            }
        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_96,97,98,124")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("ProvinceOffGroupEnabled")]
        public void PersonalDetails_VerifyAddressLine5AsTextBox()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                //Testcase96---To check if AddressLine5 as textbox exists or not
                objPersonalDetails.checkIfElementPresent(ControlKeys.PERSONALDETAILS_ADDRESSLINE5);
                //TestCase 97---Enter details and save
                objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE5, "AddressLine5");
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //TestCase 98---Delete details and save
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE5).Id)).Clear();
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                //TestCase-124---Enter Invalid data and check error message
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine5))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE5, testData_Personal.InvalidMailingAddressLine5);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE5, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE5, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }

        }
        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_99,100,101,102")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOn")]
        public void PersonalDetails_VerifyRaceLocaleDropDown()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDERACE))
            {
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    //Test Case 99--To check if Race drown down is visible
                    objPersonalDetails.checkIfElementPresent(ControlKeys.PERSONALDETAILS_RACE);
                    //Test case-100
                    if (objPersonalDetails.CheckProvice(testData.Clubcard, ControlKeys.PERSONALDETAILS_RACE, Services.Enums.CustPersonalDetail.RaceLocale))
                        customLogs.LogInformation("Race drop down verified");
                    else
                        Assert.Fail("Race drop down not verified");
                    //Test case-101-Select any value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_RACE, Enums.DropDownValue.AnyOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                    //Select -Select Race- value from drop down
                    objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_RACE, Enums.DropDownValue.SelectOption);
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "Save Changes");
                    objGeneric.verifyValidationMessage(ValidationKey.PD_SAVESUCCESSFULLMSG, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "PERSONAL DETAILS", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
            }

        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_101")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        [TestCategory("LocaleOff")]
        public void PersonalDetails_VerifyRaceEnglishDropDown()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDERACE))
            {
                if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.LOCALEENABLED))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    if (objPersonalDetails.CheckProvice(testData.Clubcard, ControlKeys.PERSONALDETAILS_RACE, Services.Enums.CustPersonalDetail.RaceEnglish))
                        customLogs.LogInformation("Race drop down verified");
                    else
                        Assert.Fail("Race drop down not verified");
                }
            }
        }

        //[TestMethod]
        //[TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_21")]
        //[Description("Verify error messages for mandatory field")]
        //[Owner("Infosys")]
        //[TestCategory("PersonalDetails")]
        //[TestCategory("P0_Regression")]
        //public void PersonalDetails_VerifyErrorMessages()
        //{
        //    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
        //    objLogin.SecurityLayer_Verification(testData.Clubcard);
        //    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
        //    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
        //    objLogin.SecurityLayer_Verification(testData.Clubcard);
        //    objPersonalDetails.returnMandatoryFields(Enums.FieldType.Valid);
        //    if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
        //        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
        //    else
        //        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
        ////    objJoin.verifyValidationCheck(Enums.FieldType.Mandatory);

        //}

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_22")]
        [Description("To fill all mandatory details and click confirm")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_FillMandatoryDetails()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objPersonalDetails.fillMandatoryFieldsGeneric(Enums.FieldType.Valid);
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
            else
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
            objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_23,29,53,57")]
        [Description("To fill all mandatory and optional details and click confirm")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_FillAllDetails()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objPersonalDetails.fillAllFieldsGeneric(Enums.FieldType.Valid);
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
            else
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
            objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.PERSONALDETAILS_Message, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_24")]
        [Description("Update surname and check updation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_SurnameReplacementValidation()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.REPLACEMENTTEXT) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.HideJoinFunctionality, DBConfigKeys.HIDEORDERAREPLACEMENTPAGE))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                int length = objGeneric.returnConfigLength(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.NAME3LENGTH, Enums.FieldType.Valid);
                string SurnameText = objPersonalDetails.RandomString(length);
                objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_SURNAME, SurnameText);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objPersonalDetails.VerifyReplacementText();
            }

        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_25")]
        [Description("Enter FirstName in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_VerifyFirstNameValidation()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME1LENGTH, testData_Personal.InvalidName1))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_FIRSTNAME, testData_Personal.InvalidName1);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORNAME1, ControlKeys.PERSONALDETAILS_ERRORNAME1, "PD Name1 error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);

            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_26")]
        [Description("Enter MiddleName in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_VerifyMiddleNameValidation()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME2LENGTH, testData_Personal.InvalidName2))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_MIDDLENAME, testData_Personal.InvalidName2);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME2, ControlKeys.PERSONALDETAILS_ERRORNAME2, "PD Name2 error", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);

            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_27")]
        [Description("Enter Surname in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_VerifySurNameValidation()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.NAME3LENGTH, testData_Personal.InvalidName3))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_SURNAME, testData_Personal.InvalidName3);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_GENERICERROR, ControlKeys.PERSONALDETAILS_GENERICERROR, "PD genric error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORNAME3, ControlKeys.PERSONALDETAILS_ERRORNAME3, "PD Name3 error", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_28")]
        [Description("Enter DOB in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_DOBErrorValidation()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDOB))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);

                objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_YEAR, Enums.DropDownValue.SelectOption);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");

                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDOB, ControlKeys.PERSONALDETAILS_ERRORDOB, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_51,52")]
        [Description("Enter Mobile Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_ConfLengthCheckForMobileNumber()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterInvalidLength(DBConfigKeys.MOBILENUMBERPREFIX, ControlKeys.PERSONALDETAILS_MOBILENUMBER, Enums.FieldType.InvalidLength1);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORMOBILENUMBER, ControlKeys.PERSONALDETAILS_ERRORMOBILENUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterInvalidLength(DBConfigKeys.MOBILENUMBERPREFIX, ControlKeys.PERSONALDETAILS_MOBILENUMBER, Enums.FieldType.InvalidLength2);
                objPersonalDetails.ValidateMaxLength(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.MOBILENUMBER, ControlKeys.PERSONALDETAILS_MOBILENUMBER);
            }

        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_55,56")]
        [Description("Enter DayTime Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_ConfLengthCheckForDayTimeNumber()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterInvalidLength(DBConfigKeys.DAYNUMBERPREFIX, ControlKeys.PERSONALDETAILS_DAYTIMENUMBER, Enums.FieldType.InvalidLength1);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDAYTIMENUMBER, ControlKeys.PERSONALDETAILS_DAYTIMENUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterInvalidLength(DBConfigKeys.MOBILENUMBERPREFIX, ControlKeys.PERSONALDETAILS_DAYTIMENUMBER, Enums.FieldType.InvalidLength2);
                objPersonalDetails.ValidateMaxLength(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.DAYTIMENUMBER, ControlKeys.PERSONALDETAILS_DAYTIMENUMBER);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_58,59")]
        [Description("Enter Evening Phone Number in wrong format(digits less than and greater than configuration) and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_ConfLengthCheckForEveningNumber()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                //objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                //objLogin.SecurityLayer_Verification(testData.Clubcard);
                //objPersonalDetails.EnterInvalidLength(DBConfigKeys.EVENINGNUMBER, ControlKeys.PERSONALDETAILS_EVENINGNUMBER, Enums.FieldType.InvalidLength1);
                //if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                //    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                //else
                //    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                //objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERROREVENINGNUMBER, ControlKeys.PERSONALDETAILS_EVENINGNUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.EnterInvalidLength(DBConfigKeys.EVENINGNUMBERPREFIX, ControlKeys.PERSONALDETAILS_EVENINGNUMBER, Enums.FieldType.InvalidLength2);
                objPersonalDetails.ValidateMaxLength(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.EVENINGNUMBER, ControlKeys.PERSONALDETAILS_EVENINGNUMBER);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_68")]
        [Description("Enter email in wrong format and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_RegexCheckForEmail()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEMAIL))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFOREMAIL, testData_Personal.InvalidEmailAddress))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_EMAIL, testData_Personal.InvalidEmailAddress);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERROREMAIL, ControlKeys.PERSONALDETAILS_ERROREMAIL, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);

            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_69")]
        [Description("Enter title and gender that does not match and verify validation message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_TitleGenderMismatchError()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEGENDER) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objPersonalDetails.selectOtionFromDropDown(ControlKeys.PERSONALDETAILS_TITLE, DropDownValue.SelectOption);
                objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE, "PD");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORGENDERMISMATCH, ControlKeys.PERSONALDETAILS_ERRORGENDERMISMATCH, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
                customLogs.LogInformation("Title or gender not present for " + CountrySetting.country);
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_70,71")]
        [Description("Duplicate check for Email And Mobile Phone Number")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_DuplicateCheck()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEMAIL) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DUPLICATECHECKREQUIRED))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string emailforjoin = customer.GetEmailIdForJoin(CountrySetting.culture);
                objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_EMAIL, emailforjoin);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORDUPLICACY, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
                customLogs.LogInformation("Duplicacy not enabled or email field not present for " + CountrySetting.country);
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_72")]
        [Description("Enter profane word in field which are specified in configuration table for profanity check and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_ProfanityCheck()
        {
            //CustomerServiceAdaptor cusDetails = new CustomerServiceAdaptor();
            //CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            //long custId = csa.GetCustomerID(testData.Clubcard, CountrySetting.culture);
            //Dictionary<string, string> CustomerDetail = cusDetails.GetCustomerDetails1(custId, CountrySetting.culture);
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROFANITYREQUIRED))
            {
                if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1) && objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Profanity_check_fields, DBConfigKeys.NAME1))
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    UtilityServiceAdaptor csa = new UtilityServiceAdaptor();
                    // if(csa.CheckProfanity(testData_Personal.ProfaneName1))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_FIRSTNAME, testData_Personal.ProfaneName1);
                    if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                    else
                        objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                    objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORPROFANITY, ControlKeys.PERSONALDETAILS_LBLSUCESSFULMSG, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
                }
                else
                    customLogs.LogInformation("Name1 not enabled or profanity for Name1 not enabled for" + CountrySetting.country);
            }
            else
                customLogs.LogInformation("Profanity Not enabled");
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_119")]
        [Description("Enter invalid data in addressLine1 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidAddressLine1ErrorVerification()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE1))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine1))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE1, testData_Personal.InvalidMailingAddressLine1);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE1, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE1, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_120")]
        [Description("Enter invalid data in addressLine2 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidAddressLine2ErrorVerification()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE2))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine2))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE2, testData_Personal.InvalidMailingAddressLine2);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE2, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE2, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_121")]
        [Description("Enter invalid data in addressLine3 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidAddressLine3ErrorVerification()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE1))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine3))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE3, testData_Personal.InvalidMailingAddressLine3);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE3, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE3, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_122")]
        [Description("Enter invalid data in addressLine4 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidAddressLine4ErrorVerification()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE4))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine4))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE4, testData_Personal.InvalidMailingAddressLine4);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE4, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE4, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        //[TestMethod]
        //[TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_123")]
        //[Description("Enter invalid data in addressLine5 and validate error message")]
        //[Owner("Infosys")]
        //[TestCategory("PersonalDetails")]
        //[TestCategory("P0_Regression")]
        //public void PersonalDetails_InvalidAddressLine5ErrorVerification()
        //{
        //    //if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5))
        //    //{
        //       objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
        //        objLogin.SecurityLayer_Verification(testData.Clubcard);
        //        objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
        //        objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
        //        objLogin.SecurityLayer_Verification(testData.Clubcard);
        //        if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine5))
        //            objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE5, testData_Personal.InvalidMailingAddressLine5);
        //        else
        //            Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
        //        if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
        //            objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
        //        else
        //            objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
        //        objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE5, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE5, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
        //   // }
        //}

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_124")]
        [Description("Enter invalid data in addressLine6 and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidAddressLine6ErrorVerification()
        {
            if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS) && objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE6))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                if (!objGeneric.VerifyRegex(ConfugurationTypeEnum.Format, DBConfigKeys.REGEXFORADDRESSLINE, testData_Personal.InvalidMailingAddressLine6))
                    objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE6, testData_Personal.InvalidMailingAddressLine6);
                else
                    Assert.Fail("Input is Valid . Kindly use input that doesn't matches the regular expression");
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORADDRESSLINE6, ControlKeys.PERSONALDETAILS_ERRORADDRESSLINE6, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestMethod]
        [TestProperty("TestCaseID", "MCA_SCN_UK_002_TC_112,113,114,115")]
        [Description("Enter invalid phone number and validate error message")]
        [Owner("Infosys")]
        [TestCategory("PersonalDetails")]
        [TestCategory("P0_Regression")]
        public void PersonalDetails_InvalidPhoneNumberErrorVerification()
        {
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER))
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_MOBILENUMBER, testData_Personal.InvalidMobilePhoneNumber);
                if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLEDIEATARYPREFERENCE))
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButtonDietaryDisabled, "personaldetails");
                else
                    objGeneric.ClickElement(ControlKeys.PERSONALDETAILS_ConfirmButton, "personaldetails");
                objGeneric.verifyValidationMessage(ValidationKey.PERSONALDETAILS_ERRORMOBILENUMBER, ControlKeys.PERSONALDETAILS_ERRORMOBILENUMBER, "personaldetails", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }

}
