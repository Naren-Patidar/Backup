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
    public class OptionAndBenefitTestSuite
    {
        #region Fields

        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        OptionsAndBenefits objOpt = null;
        static string error = string.Empty;

        private static string beginMessage = "********************* Option And Benefit ****************************";
        private static string suiteName = "Option And Benefit";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        #endregion        

        public OptionAndBenefitTestSuite()
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
            objOpt = new OptionsAndBenefits(objAutomationHelper, SanityConfiguration);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        #region Sanity


        [TestMethod]
        [Description("To Click on Option And Benefit and verify the title")]
        [TestCategory("Sanity")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_OnB()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region BasicFunctionality

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("MVC")]
        [Priority(0)]
        public void OptionsAndBenefit_Savechanges()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
               objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(ValidationKey.VALIDATIONMESSAGEFORSAVECONTACTPREFERENCE, ControlKeys.OPTIONSBENEFIT_Message, "options and benefits", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Regression of Option and Benifits Select voucher test case")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set1")]
        [TestCategory("OptionAndBenefit")]
        public void OptionsAndBenefit_ValidateVoucher()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);               
                objOpt.SelectOption(OptionPreference.None);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");               
                objOpt.ValidateSelectedOption(OptionPreference.None);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion

        #region P0

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message ChistmasSavechanges")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("P0Set8")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateChistmasSave()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);                
                objOpt.SelectOption(OptionPreference.Xmas_Saver);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.ValidateSelectedOption(OptionPreference.Xmas_Saver);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message AiosSchemeValidateMessage")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("P0Set9")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateAiosValidateMessage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            string isMembershipForAviosEnable = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMEMBERSHIPFORAVIOSENABLE);

            if (isPresent && isMembershipForAviosEnable.ToUpper() == "TRUE")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);                
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_AAVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message AviosSavechanges")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("P0Set7")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateAviosSave()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            string isMembershipForAviosEnable = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMEMBERSHIPFORAVIOSENABLE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                if (isMembershipForAviosEnable.ToUpper() == "TRUE")
                {
                    objOpt.SetMembershipId(OptionPreference.Airmiles_Standard);
                }
                objOpt.SetEmail(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.ValidateSelectedOption(OptionPreference.Airmiles_Standard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link/Membershipid txtbox is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message VirginAtlanticFlyingClubValidateMessage")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("P0Set6")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateVirginValidateMessage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_VIRGINVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message VirginAtlanticFlyingClubValidatetextbox")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set5")]
        public void OptionsAndBenefit_ValidateVirginSave()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
                objOpt.SetMembershipId(OptionPreference.Virgin_Atlantic);
                objOpt.SetEmail(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.ValidateSelectedOption(OptionPreference.Virgin_Atlantic);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate message BA ValidateMessage")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression ")]
        [TestCategory("P0Set4")]       
        [TestCategory("OptionAndBenefit")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateBritishAirwaysValidateMessage()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.BA_Miles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_BAAVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To click on confirm button in Opt and benefit page and validate  BA Validatetextbox message")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("OptionAndBenefit")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateBritishAirwaysSave()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                //objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                objOpt.SelectOption(OptionPreference.BA_Miles_Standard);
                objOpt.SetMembershipId(OptionPreference.BA_Miles_Standard);
                objOpt.SetEmail(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.ValidateSelectedOption(OptionPreference.BA_Miles_Standard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To validate the membershipID value for Avios Option")]
        [TestCategory("P1")]
        [TestCategory("P1_OptionAndBenefit")]
        [TestCategory("P1_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("O&B_MembershipID_Validation")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateAviosMembershipIDValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objOpt.SetIncorrectMembershipId(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the email value")]
        [TestCategory("P1")]
        [TestCategory("P1_OptionAndBenefit")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set9")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objOpt.SetMembershipId(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CHANGEEMAIL).ControlId, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITS_EMAILVALIDATE, ControlKeys.OPTIONSBENEFIT_EMAILERROR, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the confirm email value")]
        [TestCategory("P1")]
        [TestCategory("P1_OptionAndBenefit")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set9")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateConfirmEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objOpt.SetMembershipId(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CHANGEEMAIL).ControlId, FindBy.ID);
                objOpt.SetEmail(OptionPreference.Airmiles_Standard);
                objGeneric.ClearInputField(ControlKeys.OPTIONSBENEFIT_TXTCONFIRMEMAIL, FindBy.CSS_SELECTOR_ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITS_EMAILVALIDATE, ControlKeys.OPTIONSBENEFIT_CONFIRMEMAILERROR, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the confirm email value does not match")]
        [TestCategory("P1")]
        [TestCategory("P1_OptionAndBenefit")]
        [TestCategory("P1_Regression")]
        [TestCategory("P1Set9")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateMismatchEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objOpt.SetMembershipId(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CHANGEEMAIL).ControlId, FindBy.ID);
                objOpt.SetEmail(OptionPreference.Airmiles_Standard);
                objGeneric.ClearInputField(ControlKeys.OPTIONSBENEFIT_TXTCONFIRMEMAIL, FindBy.CSS_SELECTOR_ID);
                objGeneric.EnterDataInField(ControlKeys.OPTIONSBENEFIT_TXTCONFIRMEMAIL, "a@b.com");
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITS_MISMATCHVALIDATE, ControlKeys.OPTIONSBENEFIT_MISMATCHEMAILERROR, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the customer is able to see email field")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateEmailFieldDisplayed()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objOpt.checkEmail(OptionPreference.Airmiles_Standard);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the membershipID value for BAAvios Option")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("O&B_MembershipID_Validation")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateBAAviosMembershipIDValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.BA_Miles_Standard);
                objOpt.SetIncorrectMembershipId(OptionPreference.BA_Miles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the membershipID value for Virgin Option")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("O&B_MembershipID_Validation")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateRegexForVirginMembershipIDValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
                objOpt.SetIncorrectMembershipId(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.OPTIONANDBENIFITSVALIDATEMESSAGE, ControlKeys.OPTIONSBENEFIT_AVIOSVALIDATEMESSAGE, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the membershipID value for Virgin Option")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("O&B_MembershipID_Validation")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateVirginAlgorithmForTenDigitMembershipIDValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
               
              string membershipID =  objOpt.GenerateTenDigitVirginMembershipId();
              driver = objAutomationHelper.WebDriver;
              driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID).Id)).SendKeys(membershipID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.CONTACTPREFERENCESAVE, ControlKeys.LBLVALIDATION_MSG, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To validate the membershipID value for Virgin Option")]
        [TestCategory("P0")]
        [TestCategory("P0_OptionAndBenefit")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set9")]
        [TestCategory("O&B_MembershipID_Validation")]
        [Priority(0)]
        public void OptionsAndBenefit_ValidateVirginAlgorithmForElevenDigitMembershipIDValue()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);

                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);

                string membershipID = objOpt.GenerateElevenDigitVirginMembershipId();
                driver = objAutomationHelper.WebDriver;
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID).Id)).SendKeys(membershipID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.CONTACTPREFERENCESAVE, ControlKeys.LBLVALIDATION_MSG, "options and benefits", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }
        #endregion

        #region P2

        [TestMethod]
        [Description("Check the text validation for heading and labels in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        public void OptionsAndBenefit_TextValidationHeaderandLabels()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_LBLHEADER, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_PAGEHEADER }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_LBLPAGEDESC, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_PAGEDESCRIPTION }, FindBy.CSS_SELECTOR_ID);
             //   error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_LBLHEADER1, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_CLUBCARDHEADER }, FindBy.CSS_SELECTOR_ID);
             //   error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_LBLHEADER2, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_STATEMENTHEADER }, FindBy.CSS_SELECTOR_ID);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        
        [TestMethod]
        [Description("Check the text validation for Christmas saver in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        public void OptionsAndBenefit_ChristmasSaver()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectOption(OptionPreference.Xmas_Saver);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTCSSAVER, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_CHRISTAMASSAVERS }, FindBy.CSS_SELECTOR_CSS);
                error=error+objOpt.verifyChristamsSaverText();
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for Avios in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_Avios()
        {

            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectOption(OptionPreference.Xmas_Saver);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTAVIOS, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_AVIOS }, FindBy.CSS_SELECTOR_ID);
                error=error+objOpt.verifyAviosText();
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for VirginAtlantic in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_Virginatlantic()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectOption(OptionPreference.Xmas_Saver);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTVIRGINATLANTIC, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_VIRGINATLANTIC }, FindBy.CSS_SELECTOR_ID);
                objOpt.verifyVirginAtlanticText();
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for BAAvios in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_BAAvios()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectOption(OptionPreference.Xmas_Saver);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objOpt.SelectOption(OptionPreference.BA_Miles_Standard);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOS, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_BAMILES }, FindBy.CSS_SELECTOR_ID);
                objOpt.verifyBAAviosText();
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for Clubcard Emails in options and benefit page")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        public void OptionsAndBenefit_ClubcardEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_LBLOTHEROPTIONHEADER, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_OPTIONSHEADER }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTCLUBCARDEMAIL, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_OPTIONEMAIL }, FindBy.CSS_SELECTOR_ID);
                error = objOpt.VerifyText(LabelKey.OPTIONANDBENIFITS_MESSAGEEMAIL, LabelKey.OPTIONANDBENIFITS_MOREINFORMATION, ControlKeys.OPTIONSBENEFIT_TXTEMAILDESC, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, FindBy.CSS_SELECTOR_ID, "", 1);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Check if the memberShipID block gets hide/unhide on radio button selection for Avios Tab")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateAviosRadioButtonBehaviour()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectTab(OptionPreference.Airmiles_Standard);
                bool isValid = objOpt.VerfiyRadioButtonBehaviour(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN, ControlKeys.OPTIONSBENEFIT_AVIOS_MEMBERSHIPID);
                if (isValid)
                    customLogs.LogInformation("Radio button behaviour check is completed");
                else
                    Assert.Fail("Radiobutton selection is not working fine");
              
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check if the memberShipID block gets hide/unhide on radio button selection for Virgin Tab")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateVirginRadioButtonBehaviour()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectTab(OptionPreference.Virgin_Atlantic);
                bool isValid = objOpt.VerfiyRadioButtonBehaviour(ControlKeys.OPTIONSBENEFIT_VIRGIN, ControlKeys.OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID);
                if (isValid)
                    customLogs.LogInformation("Radio button behaviour check is completed");
                else
                    Assert.Fail("Radiobutton selection is not working fine");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Check if the memberShipID block gets hide/unhide on radio button selection for BAAvios Tab")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateBAAviosRadioButtonBehaviour()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objOpt.SelectTab(OptionPreference.BA_Miles_Standard);
                bool isValid = objOpt.VerfiyRadioButtonBehaviour(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN, ControlKeys.OPTIONSBENEFIT_BA_MEMBERSHIPID);
                if (isValid)
                    customLogs.LogInformation("Radio button behaviour check is completed");
                else
                    Assert.Fail("Radiobutton selection is not working fine");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the active Avios tab when 'Clear Selection' button is clicked")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateAviosTabAfterClearSelection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            string isMembershipForAviosEnable = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMEMBERSHIPFORAVIOSENABLE);

            if (isPresent && isMembershipForAviosEnable.ToUpper() == "TRUE")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Airmiles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTAVIOS, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_AVIOS }, FindBy.CSS_SELECTOR_ID);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("User is not on the same tab where the clear selection butotn is clicked");
                else
                    customLogs.LogInformation("Validation ailed to retain user on smae tab from where clear selection button is clicked");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the active Virgin tab when 'Clear Selection' button is clicked")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateVirginTabAfterClearSelection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            string isMembershipForAviosEnable = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMEMBERSHIPFORAVIOSENABLE);

            if (isPresent && isMembershipForAviosEnable.ToUpper() == "TRUE")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.Virgin_Atlantic);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTVIRGINATLANTIC, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_VIRGINATLANTIC }, FindBy.CSS_SELECTOR_ID);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("User is not on the same tab where the clear selection butotn is clicked");
                else
                    customLogs.LogInformation("Validation ailed to retain user on smae tab from where clear selection button is clicked");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the active BAAvios tab when 'Clear Selection' button is clicked")]
        [TestCategory("P2")]
        [TestCategory("P2_OptionAndBenefit")]
        [TestCategory("OptionAndBenefit")]
        [TestCategory("O&B_MembershipIDBlock")]
        public void OptionsAndBenefit_ValidateBAAviosTabAfterClearSelection()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
            string isMembershipForAviosEnable = objGeneric.verifyKeyEnabled(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISMEMBERSHIPFORAVIOSENABLE);

            if (isPresent && isMembershipForAviosEnable.ToUpper() == "TRUE")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_ConfirmButton).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.OPTIONBENEFIT, ControlKeys.LINK_CLICK, "options and benefits");
                objOpt.SelectOption(OptionPreference.BA_Miles_Standard);
                objGeneric.ClickElement(objAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CLEAR).Id, FindBy.ID);
                error = objGeneric.ValidateText(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOS, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE, new List<string> { LabelKey.OPTIONANDBENIFITS_BAMILES }, FindBy.CSS_SELECTOR_ID);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("User is not on the same tab where the clear selection butotn is clicked");
                else
                    customLogs.LogInformation("Validation ailed to retain user on smae tab from where clear selection button is clicked");
            }
            else
            {
                Assert.Inconclusive(string.Format("Options & Benefits Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
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
