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
using OpenQA.Selenium.Interactions;
using Tesco.Framework.UITesting.Services;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class ContactPreferenceTestSuite
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        PreferenceServiceAdaptor objPrefService = null;

        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyContactPreference objMyContactPreference = null;

        private static string beginMessage = "********************* My Contact Preference ****************************";
        private static string suiteName = "Contact Preference";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public ContactPreferenceTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.CONTACTPREFERNECESUITE);
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
                //objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), "http://dvdcccoweb001uk.dev.global.tesco.org/Clubcard_CZ/Ucet/Default.aspx");                
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
            objMyContactPreference = new MyContactPreference(objAutomationHelper);
            objPrefService = new PreferenceServiceAdaptor();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }


        [TestMethod]
        [Description("Grid format true for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_CheckGridForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
                string isGridFormat = webConfig.Value;
                if (isGridFormat == "true")
                {
                     objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                     objLogin.SecurityLayer_Verification(testData.Clubcard);
                     objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                     objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                     objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                     objMyContactPreference.ContactPreference_checkIfGridExist();                     
                }
                else
                    Assert.AreEqual(isGridFormat, "false", "Configuration Value not matched with WebConfig");
                
                customLogs.LogInformation(endMessage);
          }

        [TestMethod]
        [Description("Select all checkboxes in grid for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_SelectAllOrParticularCheckBoxes()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "true")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_checkIfGridExist();                
                objMyContactPreference.ContactPreference_SelectAllOrParticularCheckBox();
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Success Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_verifySelectedCheckBox();

            }
            else
                Assert.AreEqual(isGridFormat, "false", "Configuration Value not matched with WebConfig");

            customLogs.LogInformation(endMessage);
        }

        
        [TestMethod]
        [Description("Grid format false for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_CheckboxesFormat()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_checkIfGridDoesnotExist();
            }
            else
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox product servcie for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_SelectProductServiceCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {

                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string value  = objPrefService.GetPreference_contact(testData.Clubcard, CountrySetting.culture);
                if (value.Equals("UK"))
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKTESCOPRODUCT);
                else
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKGROUPTESCOPRODUCT);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                if (value.Equals("UK"))
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKTESCOPRODUCT);
                else
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKGROUPTESCOPRODUCT);             
            }
            else
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox partner servcie for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_SelectPartnerServiceCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                string value = objPrefService.GetPreference_contact(testData.Clubcard, CountrySetting.culture);
                if (value.Equals("UK"))
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKPARTNEROFFER);
                else
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKGROUPPARTNEROFFER);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                //objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKGROUPPARTNEROFFER);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                //objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKGROUPPARTNEROFFER);
                if (value.Equals("UK"))
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKPARTNEROFFER);
                else
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKGROUPPARTNEROFFER);  
            }
            else
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox partner servcie for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_SelectContactPermissionCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                string value = objPrefService.GetPreference_contact(testData.Clubcard, CountrySetting.culture);
                if (value.Equals("UK"))
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKRESEARCH);
                else
                    objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKGROUPRESEARCH);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                //objMyContactPreference.SelectMailingOptionChkBox(ControlKeys.CHKGROUPRESEARCH);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                //objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKGROUPRESEARCH);
                if (value.Equals("UK"))
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKRESEARCH);
                else
                    objMyContactPreference.VerifyMailingOptionCheckBox(ControlKeys.CHKGROUPRESEARCH);  
            }
            else
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false and opt in preference false for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_OptOutForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
            string isOptInBehaviour = webConfig1.Value;

            if (isGridFormat == "false" && isOptInBehaviour == "false")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string option = objPrefService.GetPreference_contact(testData.Clubcard, CountrySetting.culture);
                objMyContactPreference.ContactPreference_CheckAllCheckBoxesOpt("Out" , option);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else
            {
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isOptInBehaviour, "true", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }
        [TestMethod]
        [Description("Grid format false and opt in preference true for contact preference")]
        [TestCategory("p3")]
        public void ContactPreference_OptInForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
            string isOptInBehaviour = webConfig1.Value;

            if (isGridFormat == "false" && isOptInBehaviour == "true")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string option = objPrefService.GetPreference_contact(testData.Clubcard, CountrySetting.culture);
                objMyContactPreference.ContactPreference_CheckAllCheckBoxesOpt("In" , option);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else
            {
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isOptInBehaviour, "false", "Configuration Value not matched with WebConfig");
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

