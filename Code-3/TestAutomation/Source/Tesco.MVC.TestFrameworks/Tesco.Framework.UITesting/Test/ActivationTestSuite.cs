using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;

using System.Configuration;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.UITesting.Helpers;
using System.Threading;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class ActivationTestSuite
    {

        public IWebDriver driver;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        ILogger customLogs = null;

        // declare helpers
        Login objLogin = null;
        Activation objActivation = null;
        Generic objGeneric = null;
        MyAccountDetails objAccountDetails = null;

        //static TestDataHelper<TestData_Activation> ActTestData = new TestDataHelper<TestData_Activation>();
        //static TestData_Activation testData;

        //static string culture;
        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> AdTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;


        public static List<string> Cultures = new List<string>();

        private static string beginMessage = "********************* Activation ****************************";
        private static string suiteName = "Activation";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);


        public ActivationTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.ACTIVATIONSUITE);

        }

        /// <summary>
        /// Selects the country and load the control and message xml
        /// </summary>
        /// <param name="testContext"></param>
        /// 
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            AdTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = AdTestData.TestData;
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
                    objAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl);
                    lstAutomationHelper.Add(objAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + "Suite is currently running for country " + culture + " for domain" + SanityConfiguration.Domain);
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
            objActivation = new Activation(objAutomationHelper, SanityConfiguration, testData);
            objAccountDetails = new MyAccountDetails(objAutomationHelper);
        }


        # region Oldtestcases

        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_25")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_SuccessfulActivationConfirmationMsgVerification()
        //{

        //    //objActivation.CheckConfigurationForActivation();
        //    driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
        //    objActivation.CSC_Login();
        //    objActivation.CSC_SearchCustomerAndDelinkAccount();
        //    // objActivation.CSC_Logout();
        //    driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //    objActivation.Activation_CheckSuccessMessage("Activation");
        //    customLogs.LogInformation(endMessage);
        //}

        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P0")]
        //public void Activation_ActivatedConfirmatioMsgVerification()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
        //    objActivation.CSC_Login();
        //    if (objActivation.CSC_SearchCustomerAndCheckAccount())
        //    {
        //        driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //        objActivation.Activation_CheckConnectedMessage("Activation");
        //    }
        //    else
        //    {
        //        driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //        objActivation.Activation_CheckSuccessMessage("Activation");
        //        driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //        objActivation.Activation_CheckConnectedMessage("Activation");
        //    }

        //    customLogs.LogInformation(endMessage);

        //}


        //[TestMethod]
        //[Description("MCA_SCN_UK_012_TC_26")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ActivationConfirmationErrorMsgVerification()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    driver.Navigate().GoToUrl(SanityConfiguration.CscUrl);
        //    objActivation.CSC_Login();
        //    if (objActivation.CSC_SearchCustomerAndDelinkAccount())
        //    {
        //        // objActivation.CSC_Logout();
        //        driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //        objActivation.Activation_CheckActivationErrorMessage("Activation");
        //    }
        //    else
        //    {
        //        //objActivation.CSC_Logout();
        //        driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
        //        objActivation.Activation_CheckActivationErrorMessage("Activation");
        //    }
        //    customLogs.LogInformation(endMessage);

        //}

        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ErrorMsgVerification_Clubcard()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    objActivation.Activation_CheckErrorMsgForInvalidClubCard();
        //    customLogs.LogInformation(endMessage);
        //}
        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ErrorMsgVerification_Date()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    objActivation.Activation_CheckErrorMsgForInvalidDOB();
        //    customLogs.LogInformation(endMessage);
        //}

        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ErrorMsgVerification_FirstName()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    objActivation.Activation_CheckErrorMsgForInvalidFirstName();
        //    customLogs.LogInformation(endMessage);
        //}

        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ErrorMsgVerification_PostCode()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    objActivation.Activation_CheckErrorMsgForInvalidPostCode();
        //    customLogs.LogInformation(endMessage);
        //}

        //[TestMethod]
        //[Description("")]
        //[Owner("Infosys Team")]
        //[TestCategory("P1")]
        //public void Activation_ErrorMsgVerification_Surname()
        //{
        //    objActivation.CheckConfigurationForActivation();
        //    objActivation.Activation_CheckErrorMsgForInvalidSurName();
        //    customLogs.LogInformation(endMessage);
        //}

        # endregion
        // new test methods

        # region Sanity

        [TestMethod]
        [Description("Browse Activation page")]
        [TestCategory("Sanity")]
        [TestCategory("LeftNavigation")]
        [TestCategory("Activation")]
        public void Activation_ValidatePageTitle()
        {
            objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            objActivation.verifyPageTitle();
            customLogs.LogInformation(endMessage);
        }

        # endregion

        # region P0

        [TestMethod]
        [Description("To validate if clubcard is mandatory")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]

        public void Activation_Mandatory_Clubcard()
        {

           objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            Thread.Sleep(40);
            objActivation.verifyPageTitle();
            objActivation.ValidateMandatoryField(ControlKeys.ACTIVATION_CLUBCARDERRORMSG, ValidationKey.ERRORFORMANDATORYCLUBCARD);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Check if DOB is present on UI")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]


        public void Activation_Validate_DOB_Visible()
        {

            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_DOB))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                Thread.Sleep(40);
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_LBLDateOfBirth);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To validate if DOB is mandatory")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]

        [TestCategory("P0Set1")]
        public void Activation_Validate_Mandatory_DOB()
        {
            StringBuilder error = new StringBuilder();
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_DOB))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                Thread.Sleep(40);
                objActivation.verifyPageTitle();
                string msg = objActivation.IsControlVisible(ControlKeys.ACTIVATION_LBLDateOfBirth);
                if (string.IsNullOrEmpty(msg))
                {
                    error.Append(objActivation.validateMandatory_ReturnError(ControlKeys.ACTIVATION_ErrorMessageDOB, ValidationKey.DayOfBirth));
                    error.Append(objActivation.validateMandatory_ReturnError(ControlKeys.ACTIVATION_ErrorMessageMOB, ValidationKey.MonthOfBirth));
                    error.Append(objActivation.validateMandatory_ReturnError(ControlKeys.ACTIVATION_ErrorMessageYOB, ValidationKey.YearofBirth));
                    if (!string.IsNullOrEmpty(error.ToString()))
                    {
                        Assert.Fail(error.ToString());
                    }
                }
                else
                {
                    Assert.Fail(msg);
                }
                customLogs.LogInformation(endMessage);
            }
            else
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]
        [Description("To validate if DOB is mandatory")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]

        public void Activation_Validate_MOB_Visible()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_MONTH_OF_BIRTH))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                Thread.Sleep(40);
                objActivation.verifyPageTitle();

                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_DDLMOB);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To Check if Year of Birth is present on UI")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_YOB_Visible()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_YEAR_OF_BIRTH))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_DDLYOB);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }


        [TestMethod]
        [Description("To Check if FirstName is present on UI")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_FirstName_Visible()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_FIRSTNAME))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_FIRSTNAME);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To validate if first name is mandatory ")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_Mandatory_FirstName()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_FIRSTNAME))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_FIRSTNAME);
                if (string.IsNullOrEmpty(error))
                {
                    objActivation.ValidateMandatoryField(ControlKeys.ACTIVATION_FIRSTNAMEERRORMSG, ValidationKey.ERRORFORMANDATORYFIRSTNAME);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To Check if SurName is present on UI")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_SurName_Visible()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_SURNAME))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_SURNAME);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To validate if surname is mandatory")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_Mandatory_SurName()
        {
            string error = string.Empty;
            if (!(CountrySetting.culture == "cs-CZ"))
            {
                if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_SURNAME))
                {

                   objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                    driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    objActivation.verifyPageTitle();
                    error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_SURNAME);
                    if (string.IsNullOrEmpty(error))
                    {
                        objActivation.ValidateMandatoryField(ControlKeys.ACTIVATION_SURNAMEERRORMSG, ValidationKey.ERRORFORMANDATORYSURNAME);
                    }
                    else
                    {
                        Assert.Fail(error);
                    }

                    customLogs.LogInformation(endMessage);
                }
            }
        }

        [TestMethod]
        [Description("To Check if PostCode is present on UI")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_PostCode_Visible()
        {

            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_POSTCODE))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_POSTCODE);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("To Check if PostCode mandatory")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_Mandatory_PostCode()
        {

            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_POSTCODE))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_POSTCODE);
                if (string.IsNullOrEmpty(error))
                {
                    objActivation.ValidateMandatoryField(ControlKeys.ACTIVATION_POSTCODEERRORMSG, ValidationKey.ERRORFORMANDATORYPOSTCODE);
                }
                else
                {
                    Assert.Fail(error);
                }

                customLogs.LogInformation(endMessage);

            }
        }

        [TestMethod]
        [Description("To validate activation Re-confirmation page")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_ReConfirm()
        {
           objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            objActivation.verifyPageTitle();
            objActivation.Validate_ReConfirmPageTitle(ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_RECONFIRMTITLE, ValidationKey.RECONFIRMPAGETITLE);
            customLogs.LogInformation(endMessage);

        }

        [TestMethod]
        [Description("To delink clubcard")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_delink_clubcardAndConfirm()
        {
            objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            objActivation.DelinkClubcard(testData.StandardAccount.Clubcard, ControlKeys.ACTIVATION_CLUBCARDNUMBER, CountrySetting.culture, testData.StandardAccount.DotcomId);
            objActivation.verifyConfirmationPageTitle();
        }


        [TestMethod]
        [Description("To validate text for already activated account")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_ActiveClubcard_TextVerification()
        {
            objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            objActivation.DelinkClubcard(testData.StandardAccount.Clubcard, ControlKeys.ACTIVATION_CLUBCARDNUMBER, CountrySetting.culture, testData.StandardAccount.DotcomId);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            objActivation.verifyConfirmationPageTitle();
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            objActivation.LinkedClubcard(testData.StandardAccount.Clubcard, ControlKeys.ACTIVATION_CLUBCARDNUMBER, CountrySetting.culture);

        }

        [TestMethod]
        [Description("To validate if user is redirected to home page from activation page")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P0_Activation")]
        public void Activation_Navigation_HomePage()
        {
            objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            objActivation.DelinkClubcard(testData.StandardAccount.Clubcard, ControlKeys.ACTIVATION_CLUBCARDNUMBER, CountrySetting.culture, testData.StandardAccount.DotcomId);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            objActivation.verifyConfirmationPageTitle();
            objGeneric.ClickElement(ControlKeys.ACTIVATION_HOMELINK, FindBy.CSS_SELECTOR_ID);
            DBConfiguration config = AutomationHelper.GetDBConfigurationForSecurity(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.SECURITYAFTERACTIVATION, SanityConfiguration.DbConfigurationFile);
            string isPresent = config.ConfigurationValue1;
            if (isPresent == "1")
            {
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
            }
            objActivation.VerifyHomePage();
        }
        # endregion


        # region P1


        [TestMethod]
        [Description("To validate error message for invalid clubcard")]
        [TestCategory("P1")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P1Set1")]
        public void Activation_Invalid_Clubcard()
        {

           objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
            driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
            Thread.Sleep(40);
            objActivation.verifyPageTitle();
            objActivation.ValidateErrorMessage_InvalidValues(ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_CLUBCARDERRORMSG, ValidationKey.ERRORFORMANDATORYCLUBCARD);
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To Check Error message for invalid FirstName ")]
        [TestCategory("P1")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P1Set1")]
        public void Activation_Validate_Invalid_FirstName()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_FIRSTNAME))
            {
                //driver.Navigate().GoToUrl(SanityConfiguration.MCAUrl);
               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                Thread.Sleep(40);
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_FIRSTNAME);
                if (string.IsNullOrEmpty(error))
                {
                    objActivation.ValidateErrorMessage_InvalidValues(ControlKeys.ACTIVATION_FIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAMEERRORMSG, ValidationKey.ERRORFORMANDATORYFIRSTNAME);
                }
                customLogs.LogInformation(endMessage);
            }
        }

        [TestMethod]
        [Description("to check error message for invalid surname")]
        [TestCategory("P0")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P0Set1")]
        public void Activation_Validate_Invalid_SurName()
        {
            string error = string.Empty;
            if (!(CountrySetting.culture == "cs-CZ"))
            {
                if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_SURNAME))
                {

                   objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                    driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                    Thread.Sleep(40);
                    objActivation.verifyPageTitle();
                    error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_SURNAME);
                    if (string.IsNullOrEmpty(error))
                    {
                        objActivation.ValidateErrorMessage_InvalidValues(ControlKeys.ACTIVATION_SURNAME, ControlKeys.ACTIVATION_SURNAMEERRORMSG, ValidationKey.ERRORFORMANDATORYSURNAME);
                    }
                    else
                    {
                        Assert.Fail(error);
                    }

                    customLogs.LogInformation(endMessage);
                }
            }
            else
            {
                Assert.Inconclusive("Invalid surname is not implemented for" + CountrySetting.culture);
            }


        }


        [TestMethod]
        [Description("To Check error for invalid PostCode is mandatory")]
        [TestCategory("P1")]
        [TestCategory("Activation")]
        [TestCategory("P1_Activation")]
        [TestCategory("P1Set1")]
        public void Activation_Validate_Invalid_PostCode()
        {
            if (objActivation.IsControlEnabled(DBConfigKeys.ACTIVATION_POSTCODE))
            {

               objLogin.loginDetails_Activation(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID, testData.StandardAccount.DotcomId);
                driver.Navigate().GoToUrl(SanityConfiguration.ActivationUrl);
                Thread.Sleep(40);
                objActivation.verifyPageTitle();
                string error = objActivation.IsControlVisible(ControlKeys.ACTIVATION_POSTCODE);
                if (string.IsNullOrEmpty(error))
                {
                    objActivation.ValidateErrorMessage_InvalidValues(ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, ValidationKey.ERRORFORMANDATORYPOSTCODE);
                }
                else
                {
                    Assert.Fail(error);
                }

                customLogs.LogInformation(endMessage);
            }
        }
        # endregion

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
            customLogs.LogInformation(endMessage);


        }

    }
}
