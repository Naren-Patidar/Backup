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
    public class ContactPreferenceTestSuite : Base
    {
        ILogger customLogs = null;
        
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        PreferenceServiceAdaptor objPrefService = null;
        CustomerServiceAdaptor objCustService = null;
        string error = string.Empty;

        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyContactPreference objMyContactPreference = null;

        private string beginMessage = "********************* My Contact Preference ****************************";
        private string suiteName = "Contact Preference";
        private string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        TestData_AccountDetails testData = null;
        TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();        

        public ContactPreferenceTestSuite()
        {
            ObjAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.CONTACTPREFERNECESUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));            
            
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }


        /// <summary>
        /// Test initialization method
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;
            if (SanityConfiguration.RunAllBrowsers)
            {
                List<string> browsers = Enum.GetNames(typeof(Browser)).ToList();
                foreach (string browser in browsers)
                {
                    ObjAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl);
                    lstAutomationHelper.Add(ObjAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + CountrySetting.culture + " for domain" + SanityConfiguration.Domain);
                ObjAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                switch (SanityConfiguration.DefaultBrowser)
                {
                    case Browser.IE:
                        if (ObjAutomationHelper.WebDriver.Title == "Certificate Error: Navigation Blocked")
                            ObjAutomationHelper.WebDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                        break;
                    case Browser.GC:
                        break;
                    case Browser.MF:
                        break;
                }
            }

            //initialize helper objects
            objLogin = new Login(ObjAutomationHelper, SanityConfiguration);
            objGeneric = new Generic(ObjAutomationHelper);
            objMyContactPreference = new MyContactPreference(ObjAutomationHelper);
            objPrefService = new PreferenceServiceAdaptor();
            objCustService = new CustomerServiceAdaptor();
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("To Click on Contact Preference Link and verify the title")]
        [TestCategory("ContactPreferences")]
        [TestCategory("Sanity")]
        [TestCategory("3435-TH")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_Preference()
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
        }

        [TestMethod]
        [Description("To Save Contact Preferences and verify save message")]
        [TestCategory("BasicFunctionality")]
        [TestCategory("MVC")]
        [TestCategory("3435-TH")]
        [TestCategory("ContactPreferences")]
        [Priority(0)]
        public void ContactPreferences_SaveChanges()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                objGeneric.verifyValidationMessage(LabelKey.CONTACTPREFERENCESAVE, ControlKeys.LBLVALIDATION_MSG, "contact preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #region P0

        [TestMethod]
        [Description("To verify Selected preference as Email")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set9")]
        
        public void ContactPreferences_Email()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.E_Mail_Contact, testData.MainAccount.Clubcard);
            if (isPresent)
            {
                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.E_Mail_Contact);
                    objMyContactPreference.SetInputData(ContactPreference.E_Mail_Contact);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objMyContactPreference.ValidateSelectedPreference(ContactPreference.E_Mail_Contact);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Email Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Selected preference as SMS")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]        
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set8")]
        
        public void ContactPreferences_SMS()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.Mobile_SMS, testData.MainAccount.Clubcard);
            if (isPresent)
            {
                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.SelectPreference(ContactPreference.Mobile_SMS);
                objMyContactPreference.SetInputData(ContactPreference.Mobile_SMS);
                objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objMyContactPreference.ValidateSelectedPreference(ContactPreference.Mobile_SMS);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Mobile/SMS Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Selected preference as POST")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]        
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set7")]
        
        public void ContactPreferences_POST()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.Post_Contact, testData.MainAccount.Clubcard);
            if (isPresent)
            {

                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.Post_Contact);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objMyContactPreference.ValidateSelectedPreference(ContactPreference.Post_Contact);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Post Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To verify Email id is valid")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set6")]
        
        
        public void ContactPreferences_EmailValidate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.E_Mail_Contact, testData.MainAccount.Clubcard);
            if (isPresent)
            {
                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ValidateData(ContactPreference.E_Mail_Contact, testData.MainAccount.Clubcard);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Email Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Mobile is valid")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set4")]
        

        public void ContactPreferences_MobileValidate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.Mobile_SMS, testData.MainAccount.Clubcard);
            if (isPresent)
            {

                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.ValidateData(ContactPreference.Mobile_SMS, testData.MainAccount.Clubcard);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Mobile/SMS Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Email mandatory validation message")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set5")]
        public void ContactPreferences_EmailMandatoryValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.E_Mail_Contact, testData.MainAccount.Clubcard);
            if (isPresent)
            {

                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.E_Mail_Contact);
                    objGeneric.ClearInputField(ControlKeys.EMAILTEXT_VARIFICATION, FindBy.CSS_SELECTOR_ID);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.verifyValidationMessage(LabelKey.CONTACT_PREFERENCE_MANDATORY_EMAIL, ControlKeys.VALIDATION_MESSAGE, "Contact Preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Email Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Email and Confirm email must be same validation message")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set2")]
        public void ContactPreferences_EmailConfirmValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.E_Mail_Contact, testData.MainAccount.Clubcard);
            if (isPresent)
            {

                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.E_Mail_Contact);
                    objMyContactPreference.SetInvalidData(ContactPreference.E_Mail_Contact);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.verifyValidationMessage(LabelKey.CONTACT_PREFERENCE_MANDATORY_EMAIL, ControlKeys.VALIDATION_MESSAGE, "Contact Preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Email Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Mobile mandatody validation message")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0Set5")]
        [TestCategory("P0_ContactPreferences")]
        public void ContactPreferences_MobileMandatoryValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.Mobile_SMS, testData.MainAccount.Clubcard);
            if (isPresent)
            {

                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.Mobile_SMS);
                    objGeneric.ClearInputField(ControlKeys.CONFIRMPHONENUMBER_VARIFICATION, FindBy.CSS_SELECTOR_ID);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.verifyValidationMessage(LabelKey.CONTACT_PREFERENCE_MANDATORY_MOBILE, ControlKeys.VALIDATION_MESSAGE, "Contact Preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Mobile/SMS Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Mobile and Confirm Mobile shoud be same validation message")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set5")]
        public void ContactPreferences_MobileConfirmValidation()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            bool isPreferenceEnabled = objMyContactPreference.IsPreferenceEnabled(ContactPreference.Mobile_SMS, testData.MainAccount.Clubcard);
            if (isPresent)
            {
                if (isPreferenceEnabled)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objMyContactPreference.SelectPreference(ContactPreference.Mobile_SMS);
                    objMyContactPreference.SetInvalidData(ContactPreference.Mobile_SMS);
                    objGeneric.ClickElement(ObjAutomationHelper.GetControl(ControlKeys.BUTTON_CLICK).Id, FindBy.ID);
                    objGeneric.verifyValidationMessage(LabelKey.CONTACT_PREFERENCE_CONFIRM_MOBILE, ControlKeys.VALIDATION_MESSAGE, "Contact Preferences", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Mobile/SMS Preference is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
                }

            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify that SMS contact preference is visible only when it is enabled.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set1")]
        public void ContactPreferences_SMS_Enabled()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ContactPreferenceEnabled(ContactPreference.Mobile_SMS);
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify that Email contact preference is visible only when it is enabled.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set1")]
        public void ContactPreferences_Email_Enabled()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ContactPreferenceEnabled(ContactPreference.E_Mail_Contact);
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify that Post contact preference is visible only when it is enabled.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("3435-TH")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0")]
        [TestCategory("P0_ContactPreferences")]
        [TestCategory("P0Set1")]
        public void ContactPreferences_Post_Enabled()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.ContactPreferenceEnabled(ContactPreference.Post_Contact);
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify all enabled preference have corresponding checkbox control on UI.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        public void OptIns_Validate_Enable()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                string error = objMyContactPreference.ValidateSecurityPreferences();
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify select all security preferences. And successfully saved message.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        public void OptIns_SelectAll_Save()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.SelectSecurityPreferences();
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, FindBy.CSS_SELECTOR_ID);
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MESSAGEDATASAVED, ControlKeys.OPTINS_SUCCESSMSG, "Optins Success Message", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objMyContactPreference.ValidateSelectedSecurityPreferences();
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify un-select all security preferences. And successfully saved message.")]
        [TestCategory("ContactPreferences")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        public void OptIns_UnSelectAll_Save()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.UnSelectSecurityPreferences();
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, FindBy.CSS_SELECTOR_ID);
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MESSAGEDATASAVED, ControlKeys.OPTINS_SUCCESSMSG, "Optins Success Message", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objMyContactPreference.ValidateSelectedSecurityPreferences();
            }
            else
            {
                Assert.Inconclusive(string.Format("Contact Preference page is not enabled for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        #endregion        

        #region P2

        [TestMethod]
        [Description("Grid format true for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_CheckGridForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "true")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_checkIfGridExist();
            }
            else if (string.IsNullOrEmpty(isGridFormat) || isGridFormat=="false")
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Select all checkboxes in grid for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_SelectAllOrParticularCheckBoxes()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "true")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_checkIfGridExist();
                objMyContactPreference.ContactPreference_SelectAllOrParticularCheckBox();
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Success Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_verifySelectedCheckBox();

            }
            else if (string.IsNullOrEmpty(isGridFormat) || isGridFormat == "false")
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
             customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Grid format false for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_CheckboxesFormat()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                objMyContactPreference.ContactPreference_checkIfGridDoesnotExist();
            }
            else if (string.IsNullOrEmpty(isGridFormat) || isGridFormat == "false")
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox product servcie for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_SelectProductServiceCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {

                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string value = objPrefService.GetPreference_contact(testData.MainAccount.Clubcard, CountrySetting.culture);
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
            else if (string.IsNullOrEmpty(isGridFormat))
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox partner servcie for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_SelectPartnerServiceCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                string value = objPrefService.GetPreference_contact(testData.MainAccount.Clubcard, CountrySetting.culture);
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
            else if (string.IsNullOrEmpty(isGridFormat) )
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false,select checkbox partner servcie for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_SelectContactPermissionCheckBox()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;
            if (isGridFormat == "false")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                string value = objPrefService.GetPreference_contact(testData.MainAccount.Clubcard, CountrySetting.culture);
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
            else if (string.IsNullOrEmpty(isGridFormat) )
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Grid format false and opt in preference false for contact preference")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_OptOutForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
            string isOptInBehaviour = webConfig1.Value;

            if (isGridFormat == "false" && isOptInBehaviour == "false")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string option = objPrefService.GetPreference_contact(testData.MainAccount.Clubcard, CountrySetting.culture);
                objMyContactPreference.ContactPreference_CheckAllCheckBoxesOpt("Out", option);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else if (string.IsNullOrEmpty(isGridFormat) )
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
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
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        public void ContactPreference_OptInForContactPreference()
        {
            WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.IS_GRID_FORMAT, SanityConfiguration.WebConfigurationFile);
            string isGridFormat = webConfig.Value;

            WebConfiguration webConfig1 = AutomationHelper.GetWebConfiguration(WebConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.WebConfigurationFile);
            string isOptInBehaviour = webConfig1.Value;

            if (isGridFormat == "false" && isOptInBehaviour == "true")
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.VerifyTextonthePageByXpath(ValidationKey.CONTACTPREFERENCE_VALIDATIONMESSAGE, ControlKeys.LBLVALIDATION_MSG, "Validation message checked", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                string option = objPrefService.GetPreference_contact(testData.MainAccount.Clubcard, CountrySetting.culture);
                objMyContactPreference.ContactPreference_CheckAllCheckBoxesOpt("In", option);
                objGeneric.ClickElement(ControlKeys.BTNPREFSUBMIT, "contact preferences");
                objGeneric.verifyValidationMessage(ValidationKey.CONTACTPREFERENCE_MAILINGOPTIONSUCCESSMSG, ControlKeys.PREFSUCCESSMSG, "Succes Msg For Preference Save", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
            }
            else if (string.IsNullOrEmpty(isGridFormat))
            {
                Assert.Inconclusive(string.Format("Is Grid format doesn't exist for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            else
            {
                Assert.AreEqual(isGridFormat, "true", "Configuration Value not matched with WebConfig");
                Assert.AreEqual(isOptInBehaviour, "false", "Configuration Value not matched with WebConfig");
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for heading and labels in contact Preference ")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        [TestCategory("3435-TH")]
        public void ContactPreference_TextValidationHeaderandLabels()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLHEADER, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_PAGEHEADING }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_TEXTPARA1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_PAGEDESCRIPTION }, FindBy.XPATH_SELECTOR);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLHEADING, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_HEADERTEXT }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLDESCP, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_DESCRIPTIONTEXT }, FindBy.CSS_SELECTOR_ID);
                objMyContactPreference.VerifyTextForPost(LabelKey.CONTACT_PREFERENCE_LABELDESC, LabelKey.CONTACT_PREFERENCE_lNKTC, ControlKeys.PREFERENCE_TXTTERMS, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, "", 3, FindBy.XPATH_SELECTOR);
                objMyContactPreference.VerifyTextCallPhn(LabelKey.CONTACT_PREFERENCE_LABELDESC1, LabelKey.CONTACT_PREFERENCE_LABELDESC2, ControlKeys.PREFERENCE_TXTTERMS1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, FindBy.XPATH_SELECTOR);
                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for Email Section in contact Preference ")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        [TestCategory("3435-TH")]
        public void ContactPreference_TextValidationEmail()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.SelectPreference(ContactPreference.E_Mail_Contact);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLEMAILHEADING, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLEMAIL }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLENTEREMAIL, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLENTERMAIL }, FindBy.XPATH_SELECTOR);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLCNFRMEMAIL, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLCNFRMEMAIL }, FindBy.XPATH_SELECTOR);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLEMAILDESC, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLEMAILDESC }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLEMAILDESC1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLEMAILDESC1 }, FindBy.CSS_SELECTOR_ID);

                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for Text Section in contact Preference ")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        [TestCategory("3435-TH")]
        public void ContactPreference_TextValidationText()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.SelectPreference(ContactPreference.Mobile_SMS);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLTEXTHEADING, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLTEXT }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLENTERTEXT, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLENTERTEXT }, FindBy.XPATH_SELECTOR);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLCNFRMETEXT, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLCNFRMTEXT }, FindBy.XPATH_SELECTOR);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLTEXTDESC, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLTEXTDESC }, FindBy.CSS_SELECTOR_ID);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLTEXTDESC1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLTEXTDESC1 }, FindBy.CSS_SELECTOR_ID);

                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for Post Section in contact Preference ")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("ContactPreference")]
        [TestCategory("3435-TH")]
        public void ContactPreference_TextValidationPost()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objMyContactPreference.SelectPreference(ContactPreference.Post_Contact);
                error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLPOSTHEADING, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLPOSTHEADING }, FindBy.CSS_SELECTOR_ID);
                
                Dictionary<string, string> CustomerDetail = objCustService.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);
                string custAddress= CustomerDetail.ContainsKey("MailingAddressLine1")?CustomerDetail["MailingAddressLine1"]:string.Empty;
                objMyContactPreference.VerifyTextForPost(LabelKey.CONTACT_PREFERENCE_LBLCHECKADDRESS, "", ControlKeys.PREFERENCE_LBLADDRESSLINE1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, custAddress, 1, FindBy.XPATH_SELECTOR);
                objMyContactPreference.VerifyTextForPost(LabelKey.CONTACT_PREFERENCE_LBLPOSTDESC, LabelKey.CONTACT_PREFERENCE_TXTPERSONALDETAILS, ControlKeys.PREFERENCE_LBLWRONGADDRESS, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, custAddress, 3, FindBy.XPATH_SELECTOR);

                //string res = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LABELDESC3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                //if (res != string.Empty && res != null)
                //{
                //    error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLLATESTSTATEMENT, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LABELDESC3 }, FindBy.XPATH_SELECTOR);
                //}
                //else
                //{
                //    error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLLATESTSTATEMENT, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LABELDESC }, FindBy.XPATH_SELECTOR);
                //}
                error = objMyContactPreference.verifyLargeBrailleStatement(objPrefService);
                //error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLCUSTOMERCARE, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLBRAILLEDESC }, FindBy.XPATH_SELECTOR);

                if (!string.IsNullOrEmpty(error))
                    Assert.Fail("Text validation is not correct");
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Check the text validation for OptinPreferences in contact Preference ")]
        [TestCategory("P2")]
        [TestCategory("P2_ContactPreference")]
        [TestCategory("CCMCA-4990")]
        public void ContactPreference_TextValidationOptin()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPREFERENCESPAGE);
            if (isPresent)
            {
                List<string> Id = objPrefService.ViewPreference(testData.MainAccount.Clubcard, CountrySetting.culture);
                if (Id != null)
                {
                    objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYCONTACTPREF, ControlKeys.LINK_CLICK, "contact preferences");
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLHEADER1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINSUBHEADER }, FindBy.XPATH_SELECTOR);
                    DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
                    bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("TRUE");
                    if (!isOptinBehavior)
                    {
                        error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLHEADING1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINSUBHEADING }, FindBy.CSS_SELECTOR_ID);
                        objMyContactPreference.VerifyTextOptin(LabelKey.CONTACT_PREFERENCE_OPTINDESCRIPTION, LabelKey.CONTACT_PREFERENCE_OPTINCLICKHERE, ControlKeys.PREFERENCE_LBLDESCRIPTION, ControlKeys.PREFERENCE_LNKCLICKHERE, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, "", 2, FindBy.CSS_SELECTOR_ID);
                        error = error+objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLHEADING2, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINSUBHEADING2 }, FindBy.CSS_SELECTOR_ID);
                        error = error+objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLDESCRIPTION2, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINDESCRIPTION2 }, FindBy.CSS_SELECTOR_ID);

                        error = error+objGeneric.ValidatePreferenceText(SecurityPreference.Tesco_Products, ControlKeys.PREFERENCE_TXTCHKOPT1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, LabelKey.CONTACT_PREFERENCE_OPTINCHKOPT1, LabelKey.CONTACT_LBLTESCOSTORE);
                        error = error+objGeneric.ValidatePreferenceText(SecurityPreference.Tesco_Partners, ControlKeys.PREFERENCE_TXTCHKOPT2, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, LabelKey.CONTACT_PREFERENCE_OPTINCHKOPT2, LabelKey.CONTACT_LBLTESCOSTORE);
                        error = error+objGeneric.ValidatePreferenceText(SecurityPreference.Customer_Research, ControlKeys.PREFERENCE_TXTCHKOPT3, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, LabelKey.CONTACT_PREFERENCE_OPTINCHKOPT3, LabelKey.CONTACT_LBLTESCOSTORE);
                        error = error+objGeneric.ValidateText(ControlKeys.PREFERENCE_TXTVIEWCND, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINLNKTNC }, FindBy.CSS_SELECTOR_ID);
                        error = error+objGeneric.ValidateText(ControlKeys.PREFERENCE_TXTCUSTOMERCHARTER, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTINCUSTOMERCHARTER }, FindBy.CSS_SELECTOR_ID);
                    }
                    else
                    {
                        error = objGeneric.ValidateText(ControlKeys.PREFERENCE_OPSOUTDESCRIPTION, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTOUTDESCRIPTION }, FindBy.CSS_SELECTOR_ID);
                        if (Id.Contains("7"))
                            error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLOUTS1, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTOUTS1 }, FindBy.CSS_SELECTOR_ID);
                        if (Id.Contains("8"))
                            error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLOUTS2, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTOUTS2 }, FindBy.CSS_SELECTOR_ID);
                        if (Id.Contains("9"))
                            error = objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLOUTS3, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_OPTOUTS3 }, FindBy.CSS_SELECTOR_ID);

                    }
                }

                if (!string.IsNullOrEmpty(error))
                    Assert.Fail(error);
                else
                    customLogs.LogInformation("Text Validation is completed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            if (ObjAutomationHelper != null && ObjAutomationHelper.WebDriver != null)
            {
                ObjAutomationHelper.WebDriver.Quit();
            }
        }
    }

}
