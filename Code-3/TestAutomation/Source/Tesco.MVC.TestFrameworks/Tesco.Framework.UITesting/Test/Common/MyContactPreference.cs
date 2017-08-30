using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Tesco.Framework.UITesting.Services;
using OpenQA.Selenium.Interactions;

namespace Tesco.Framework.UITesting.Test.Common
{

    class MyContactPreference : Base
    {
        Generic objGeneric = null;
        List<IWebElement> el = new List<IWebElement>();
        List<int> checkBoxNumber = new List<int>();
        List<int> totalCheckBox = new List<int>();
        string value = string.Empty;
        IWebElement chkBoxElement = null;
        PreferenceServiceAdaptor preferenceAdaptor = new PreferenceServiceAdaptor();

        #region Constructor


        public MyContactPreference(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        public MyContactPreference(AutomationHelper objHelper, AppConfiguration configuration, TestData_Activation testData)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            // this.testData = testData;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion

        #region Public Methods

        public void Validate_Title()
        {
            Control header = ObjAutomationHelper.GetControl(ControlKeys.PREFERENCE_PageTitle);
            IWebElement eHeader = objGeneric.GetWebControl(header, FindBy.CSS_SELECTOR_ID);
            Resource hResource = AutomationHelper.GetResourceMessage("PageHeading", Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
            string actualText = eHeader.Text;
            string expectedText = hResource.Value;
            Assert.AreEqual(expectedText, actualText);
        }

        /// <summary>
        /// UI : Method will select the radio button for the given preference
        /// eg : For E_Mail_Contact it will select check box for Via Email radio button on UI
        /// </summary>
        /// <param name="preference"></param>
        public void SelectPreference(ContactPreference preference)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Generic objGeneric = new Generic(ObjAutomationHelper);
            string optionRadioId = string.Empty;
            string tabHeader = string.Empty;
            //switch (preference)
            //{
            //    case ContactPreference.E_Mail_Contact:
            //        tabHeader = "0";
            //        optionRadioId = ControlKeys.RADIOBUTTON_EMAIL;
            //        break;
            //    case ContactPreference.Mobile_SMS:
            //        tabHeader = "1";
            //        optionRadioId = ControlKeys.RADIOBUTTON_SMS;
            //        break;
            //    case ContactPreference.Post_Contact:
            //        tabHeader = "2";
            //        optionRadioId = ControlKeys.RADIOBUTTON_POST;
            //        break;
            //}
            switch (preference)
            {
                case ContactPreference.E_Mail_Contact:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLEMAIL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_EMAIL;
                    break;
                case ContactPreference.Mobile_SMS:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLTEXT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_SMS;
                    break;
                case ContactPreference.Post_Contact:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLPOSTHEADING, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_POST;
                    break;
            }
            //Control tab = ObjAutomationHelper.GetControl(ControlKeys.PREFERENCE_TAB);
            //By queryTab = By.CssSelector(tab.Id);
            //IWebElement eTab = ObjAutomationHelper.WebDriver.FindElement(queryTab);

            //Control tabHeaders = ObjAutomationHelper.GetControl(ControlKeys.PREFERENCE_TABHEADERS);
            //List<IWebElement> etabHeaders = eTab.FindElements(By.TagName(tabHeaders.Id)).ToList();
            ////IWebElement eTabHeader = etabHeaders.Find(e => e.GetAttribute(tabHeaders.ClassName) == tabHeader);
            //IWebElement eTabHeader = etabHeaders.Find(e => e.Text == tabHeader);

            if (tabHeader != null)
            {
                Control radio = ObjAutomationHelper.GetControl(optionRadioId);
                By queryRadio = By.CssSelector(radio.Id);
                IWebElement eRadio = ObjAutomationHelper.WebDriver.FindElement(queryRadio);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                jse.ExecuteScript("arguments[0].click();", eRadio);
                //eRadio.Click();
            }
            else
            {
                Assert.Inconclusive(preference + " not present for " + CountrySetting.country);
            }
        }

        /// <summary>
        /// UI : Method to enter valid data to input controls on UI (eg. email or phone number) for the given contact preference.
        /// eg : For E_Mail_Contact it will enter customer's valid email address in email and confirm email fields
        /// </summary>
        /// <param name="preference">Preference</param>
        public void SetInputData(ContactPreference preference)
        {
            Generic objGeneric = new Generic(ObjAutomationHelper);
            string inputText = string.Empty;
            string confirmInputText = string.Empty;
            switch (preference)
            {
                case ContactPreference.E_Mail_Contact:
                    inputText = ControlKeys.PREFERENCE_HDNEMAIL;
                    confirmInputText = ControlKeys.CONFIRMEMAILTEXT_VARIFICATION;
                    break;
                case ContactPreference.Mobile_SMS:
                    inputText = ControlKeys.PREFERENCE_HDNMOBILE;
                    confirmInputText = ControlKeys.CONFIRMPHONENUMBER_VARIFICATION;
                    break;
                case ContactPreference.Post_Contact:
                    inputText = string.Empty;
                    confirmInputText = string.Empty;
                    break;
            }
            if (!string.IsNullOrEmpty(inputText) && !string.IsNullOrEmpty(confirmInputText))
            {
                Control input = ObjAutomationHelper.GetControl(inputText);
                By queryInput = By.CssSelector(input.Id);
                IWebElement eInput = ObjAutomationHelper.WebDriver.FindElement(queryInput);

                Control confirmInput = ObjAutomationHelper.GetControl(confirmInputText);
                By queryConfirmInput = By.CssSelector(confirmInput.Id);
                IWebElement eConfirmInput = ObjAutomationHelper.WebDriver.FindElement(queryConfirmInput);
                string txt = eInput.GetAttribute("value");
                eConfirmInput.Clear();
                if (!string.IsNullOrEmpty(txt))
                {
                    eConfirmInput.SendKeys(txt);
                }
            }
        }

        /// <summary>
        /// OPR : Method will validate if given preference is selected on UI. if not it will fail the test case
        /// It will check that the hidden field in ui has the correct preference id also the correct radio button is selected
        /// </summary>
        /// <param name="contactPreference">Preference</param>
        public void ValidateSelectedPreference(ContactPreference contactPreference)
        {
            string optionRadioId = string.Empty;
            switch (contactPreference)
            {
                case ContactPreference.E_Mail_Contact:
                    optionRadioId = ControlKeys.RADIOBUTTON_EMAIL;
                    break;
                case ContactPreference.Mobile_SMS:
                    optionRadioId = ControlKeys.RADIOBUTTON_SMS;
                    break;
                case ContactPreference.Post_Contact:
                    optionRadioId = ControlKeys.RADIOBUTTON_POST;
                    break;
            }
            Control radio = ObjAutomationHelper.GetControl(optionRadioId);
            By queryRadio = By.CssSelector(radio.Id);
            IWebElement eRadio = ObjAutomationHelper.WebDriver.FindElement(queryRadio);
            Assert.AreEqual(eRadio.Selected, true);
        }

        /// <summary>
        /// OPR : Method will validate the input fields on UI has the correct data. if not it will fail the test case
        /// eg: For E_Mail_Contact it will validate that email field has the correct email (provided by the service)
        /// </summary>
        /// <param name="contactPreference"></param>
        /// <param name="Clubcard"></param>
        public void ValidateData(ContactPreference contactPreference, string Clubcard)
        {
            string hdnID = string.Empty;
            string expectedData = string.Empty;
            CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
            switch (contactPreference)
            {
                case ContactPreference.E_Mail_Contact:
                    hdnID = ControlKeys.PREFERENCE_HDNEMAIL;
                    expectedData = customer.GetEmailId(Clubcard, CountrySetting.culture);
                    break;
                case ContactPreference.Mobile_SMS:
                    hdnID = ControlKeys.PREFERENCE_HDNMOBILE;
                    expectedData = customer.GetPhoneNumber(Login.CustomerID.ToString(), CountrySetting.culture);
                    break;
            }
            Control hdnInput = ObjAutomationHelper.GetControl(hdnID);
            By queryHdnInput = By.CssSelector(hdnInput.Id);
            IWebElement eHdnInput = ObjAutomationHelper.WebDriver.FindElement(queryHdnInput);
            string acutualData = eHdnInput.GetAttribute("value");
            Assert.AreEqual(expectedData, acutualData);
        }

        /// <summary>
        /// UI : Method to enter invalid data to input controls on UI (eg. email or phone number) for the given contact preference.
        /// eg : For E_Mail_Contact it will enter customer's invalid email address in email and confirm email fields
        /// </summary>
        /// <param name="contactPreference"></param>
        public void SetInvalidData(ContactPreference contactPreference)
        {
            Generic objGeneric = new Generic(ObjAutomationHelper);
            string inputText = string.Empty;
            string confirmInputText = string.Empty;
            switch (contactPreference)
            {
                case ContactPreference.E_Mail_Contact:
                    inputText = ControlKeys.PREFERENCE_HDNEMAIL;
                    confirmInputText = ControlKeys.CONFIRMEMAILTEXT_VARIFICATION;
                    break;
                case ContactPreference.Mobile_SMS:
                    inputText = ControlKeys.PREFERENCE_HDNMOBILE;
                    confirmInputText = ControlKeys.CONFIRMPHONENUMBER_VARIFICATION;
                    break;
            }
            if (!string.IsNullOrEmpty(inputText) && !string.IsNullOrEmpty(confirmInputText))
            {
                Control input = ObjAutomationHelper.GetControl(inputText);
                By queryInput = By.CssSelector(input.Id);
                IWebElement eInput = ObjAutomationHelper.WebDriver.FindElement(queryInput);

                Control confirmInput = ObjAutomationHelper.GetControl(confirmInputText);
                By queryConfirmInput = By.CssSelector(confirmInput.Id);
                IWebElement eConfirmInput = ObjAutomationHelper.WebDriver.FindElement(queryConfirmInput);
                //string txt = eInput.GetAttribute("value");
                eConfirmInput.Clear();
                eConfirmInput.SendKeys("123456");
                //if (!string.IsNullOrEmpty(txt))
                //{
                //    eConfirmInput.SendKeys(txt + "123");
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactPreference"></param>
        public void ContactPreferenceEnabled(ContactPreference contactPreference)
        {
            string tabHeader = string.Empty;
            string optionRadioId = string.Empty;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            bool expectedIsVisible = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)contactPreference);
            bool actualIsVisisble = false;
            switch (contactPreference)
            {
                case ContactPreference.E_Mail_Contact:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLEMAIL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_EMAIL;
                    break;
                case ContactPreference.Mobile_SMS:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLTEXT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_SMS;
                    break;
                case ContactPreference.Post_Contact:
                    tabHeader = AutomationHelper.GetResourceMessage(LabelKey.CONTACT_PREFERENCE_LBLPOSTHEADING, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE)).Value;
                    optionRadioId = ControlKeys.RADIOBUTTON_POST;
                    break;
            }
            //Control tab = ObjAutomationHelper.GetControl(ControlKeys.PREFERENCE_TAB);
            //By queryTab = By.CssSelector(tab.Id);
            //IWebElement eTab = ObjAutomationHelper.WebDriver.FindElement(queryTab);

            //Control tabHeaders = ObjAutomationHelper.GetControl(ControlKeys.PREFERENCE_TABHEADERS);
            //List<IWebElement> etabHeaders = eTab.FindElements(By.TagName(tabHeaders.Id)).ToList();
            //IWebElement eTabHeader = etabHeaders.Find(e => e.Text == tabHeader);
            //actualIsVisisble = eTabHeader != null;

            Control radio = ObjAutomationHelper.GetControl(optionRadioId);
            By queryRadio = By.CssSelector(radio.Id);
            IWebElement eRadio = ObjAutomationHelper.WebDriver.FindElement(queryRadio);
            actualIsVisisble = eRadio.Enabled;
            Assert.AreEqual(expectedIsVisible, actualIsVisisble);
        }

        /// <summary>
        /// OPR : method will check that UI control should be 
        /// </summary>
        /// <param name="contactPreference"></param>
        public void SecurityPreferenceEnabled(SecurityPreference contactPreference)
        {
            string chkBoxID = string.Empty;
            bool isVisibleFromConfig = false;
            bool expectedIsVisible = false;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            bool expectedIsVisibleFromService = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)contactPreference);
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            if (string.IsNullOrEmpty(preferenceUIConfig.ConfigurationValue1))
            {
                Assert.Fail(string.Format("Preference Configuration value is null in DBConfig - custID: {0}, Country: {1}", Login.CustomerID, CountrySetting.country));
            }
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            PreferencesUIConfig cConf = prefUI.Find(p => p.preferenceid == (int)contactPreference);
            isVisibleFromConfig = (cConf != null && cConf.isvisible);
            if (isVisibleFromConfig && expectedIsVisibleFromService)
            {
                expectedIsVisible = true;
            }

            bool actualIsVisisble = false;
            chkBoxID = GetCheckboxId(contactPreference);
            Control chk = ObjAutomationHelper.GetControl(chkBoxID);
            By queryChk = By.CssSelector(chk.Id);
            IWebElement eChk = ObjAutomationHelper.WebDriver.FindElement(queryChk);
            actualIsVisisble = eChk.Displayed;
            Assert.AreEqual(expectedIsVisible, actualIsVisisble);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactPreference"></param>
        public void checkDependent(SecurityPreference contactPreference)
        {
            string chkBoxID = string.Empty;
            List<string> dependentPrefStatus = new List<string>();
            bool isVisible = false;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            List<short> dependentprefID = new List<short>();
            string prefStatus = prefClient.ContactPreferenceStatus(Login.CustomerID, (int)contactPreference);
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            if (string.IsNullOrEmpty(preferenceUIConfig.ConfigurationValue1))
            {
                Assert.Fail(string.Format("Preference Configuration value is null in DBConfig - custID: {0}, Country: {1}", Login.CustomerID, CountrySetting.country));
            }
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            PreferencesUIConfig cConf = prefUI.Find(p => p.preferenceid == (int)contactPreference);
            dependentprefID = cConf.dependentprefidsassame;
            isVisible = cConf.isvisible;
            if (isVisible)
            {
                if (dependentprefID != null)
                {
                    int count = dependentprefID.Count;
                    for (int i = 0; i < dependentprefID.Count; i++)
                    {
                        dependentPrefStatus.Add(prefClient.ContactPreferenceStatus(Login.CustomerID, dependentprefID[count - 1]));
                        if (dependentPrefStatus[i].Equals(prefStatus))
                        {
                            CustomLogs.LogMessage("dependent has same prefrence status", TraceEventType.Start);
                        }
                        else
                        {
                            Assert.Fail("Preference status for dependent preference id" + dependentprefID[count - 1] + " is not same as prefrence status for " + (int)contactPreference);
                        }
                        count--;
                    }
                }
                else
                {
                    Assert.Inconclusive("There is no dependent preference for preference id " + (int)contactPreference);
                }
            }
            else
            {
                Assert.Inconclusive("Prefrence id " + (int)contactPreference + "not visible on UI for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture);
            }
        }
        /// <summary>
        /// OPR : method will validate if given preference is opted-in / opted-out based on the service response
        /// it will fail the test case  if there is any mismatch found.
        /// </summary>
        /// <param name="contactPreference"></param>
        public void SecurityPreferenceValidate(SecurityPreference contactPreference)
        {
            string chkBoxID = string.Empty;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            bool expectedIsOpted = prefClient.IsContactPreferenceOpted(Login.CustomerID, (int)contactPreference);

            DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
            bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("FALSE");
            bool actualIsOpted = false;
            chkBoxID = GetCheckboxId(contactPreference);
            Control chk = ObjAutomationHelper.GetControl(chkBoxID);
            By queryChk = By.CssSelector(chk.Id);
            IWebElement eChk = ObjAutomationHelper.WebDriver.FindElement(queryChk);

            actualIsOpted = isOptinBehavior ? !eChk.Selected : eChk.Selected;

            Assert.AreEqual(expectedIsOpted, actualIsOpted);
        }

        private string GetCheckboxId(SecurityPreference contactPreference)
        {
            string chkBoxID = string.Empty;
            DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
            bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("FALSE");
            switch (contactPreference)
            {
                case SecurityPreference.Tesco_Products:
                    chkBoxID = isOptinBehavior ? ControlKeys.OPTOUT_CHKTESCOPRODUCT : ControlKeys.CHKTESCOPRODUCT;
                    break;
                case SecurityPreference.Tesco_Partners:
                    chkBoxID = isOptinBehavior ? ControlKeys.OPTOUT_CHKPARTNEROFFER : ControlKeys.CHKPARTNEROFFER;
                    break;
                case SecurityPreference.Customer_Research:
                    chkBoxID = isOptinBehavior ? ControlKeys.OPTOUT_CHKRESEARCH : ControlKeys.CHKRESEARCH;
                    break;
                case SecurityPreference.Tesco_Group_Mail:
                    chkBoxID = isOptinBehavior ? ControlKeys.CHKTESCOPRODUCT : ControlKeys.OPTOUT_CHKTESCOPRODUCT;
                    break;
                case SecurityPreference.Partner_3rd_Party_Mail:
                    chkBoxID = isOptinBehavior ? ControlKeys.CHKPARTNEROFFER : ControlKeys.OPTOUT_CHKPARTNEROFFER;
                    break;
                case SecurityPreference.Research_Mail:
                    chkBoxID = isOptinBehavior ? ControlKeys.CHKRESEARCH : ControlKeys.OPTOUT_CHKRESEARCH;
                    break;
            }
            return chkBoxID;
        }

        /// <summary>
        /// UI : method to opt in the given preference
        /// it will handle the opt-in and opt-out behaviour internally
        /// </summary>
        /// <param name="securityPreference"></param>
        public void SecurityPreferenceOpt(SecurityPreference securityPreference)
        {
            string chkBoxID = string.Empty;
            DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
            bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("FALSE");

            chkBoxID = GetCheckboxId(securityPreference);
            Control chk = ObjAutomationHelper.GetControl(chkBoxID);
            By queryChk = By.CssSelector(chk.Id);
            IWebElement eChk = ObjAutomationHelper.WebDriver.FindElement(queryChk);
            Generic objGeneric = new Generic(ObjAutomationHelper);
            if (isOptinBehavior)
            {
                if (eChk.Selected)
                {
                    Actions actions = new Actions(ObjAutomationHelper.WebDriver);
                    actions.MoveToElement(eChk);
                    actions.Perform();
                    objGeneric.ClickElement(chk.ControlId, FindBy.ID);
                }
                else
                {
                    Assert.Inconclusive(string.Format("{0} preference is already opted for customer ID : {1}.", securityPreference.ToString(), Login.CustomerID));
                }
            }
            else
            {
                if (eChk.Selected)
                {
                    Assert.Inconclusive(string.Format("{0} preference is already opted for customer ID : {1}.", securityPreference.ToString(), Login.CustomerID));
                }
                else
                {
                    Actions actions = new Actions(ObjAutomationHelper.WebDriver);
                    actions.MoveToElement(eChk);
                    actions.Perform();
                    objGeneric.ClickElement(chk.ControlId, FindBy.ID);
                }
            }
        }

        /// <summary>
        /// UI : method to opt-out the given preference
        /// it will handle the opt-in and opt-out behaviour internally
        /// </summary>
        /// <param name="securityPreference"></param>
        public void SecurityPreferenceOptOut(SecurityPreference securityPreference)
        {
            string chkBoxID = string.Empty;
            DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
            bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("FALSE");
            chkBoxID = GetCheckboxId(securityPreference);
            Control chk = ObjAutomationHelper.GetControl(chkBoxID);
            By queryChk = By.CssSelector(chk.Id);
            IWebElement eChk = ObjAutomationHelper.WebDriver.FindElement(queryChk);
            if (isOptinBehavior)
            {
                if (eChk.Selected)
                {
                    Assert.Inconclusive(string.Format("{0} preference is already opted out for customer ID : {1}.", securityPreference.ToString(), Login.CustomerID));
                }
                else
                {
                    Actions actions = new Actions(ObjAutomationHelper.WebDriver);
                    actions.MoveToElement(eChk);
                    actions.Perform();
                    objGeneric.ClickElement(chk.ControlId, FindBy.ID);
                }
            }
            else
            {
                if (eChk.Selected)
                {
                    Actions actions = new Actions(ObjAutomationHelper.WebDriver);
                    actions.MoveToElement(eChk);
                    actions.Perform();
                    objGeneric.ClickElement(chk.ControlId, FindBy.ID);
                }
                else
                {
                    Assert.Inconclusive(string.Format("{0} preference is already opted out for customer ID : {1}.", securityPreference.ToString(), Login.CustomerID));
                }
            }
        }

        /// <summary>
        /// OPR : Method will check if the 7-9 preference are enabled or (27-38)
        /// it will check if none of 7-9 are enabled than 27-38 must be enabled
        /// </summary>
        /// <returns>bool</returns>
        public bool IsUKMarketingPreference()
        {
            bool isUK = false;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            bool p7Enabled = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)SecurityPreference.Tesco_Products);
            bool p8Enabled = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)SecurityPreference.Tesco_Partners);
            bool p9Enabled = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)SecurityPreference.Customer_Research);
            isUK = p7Enabled && p8Enabled && p9Enabled;
            return isUK;
        }

        /// <summary>
        /// method to check all security preference check boxes
        /// </summary>
        /// <returns></returns>
        public string SelectSecurityPreferences()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "SelectSecurityPreferences", "Starting"));
            StringBuilder error = new StringBuilder();
            List<IWebElement> ctrlPreferences = GetVisibleSecurityPrefCheckBoxes();
            foreach (IWebElement ctrlPreference in ctrlPreferences)
            {
                objGeneric.ScrollToElement(ctrlPreference);
                if (!ctrlPreference.Selected)
                {
                    ctrlPreference.Click();                    
                }
            }
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "SelectSecurityPreferences", "Ending"));
            return error.ToString();
        }

        /// <summary>
        /// method to un check all security preference check boxes
        /// </summary>
        /// <returns></returns>
        public string UnSelectSecurityPreferences()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "SelectSecurityPreferences", "Starting"));
            StringBuilder error = new StringBuilder();
            List<IWebElement> ctrlPreferences = GetVisibleSecurityPrefCheckBoxes();
            foreach (IWebElement ctrlPreference in ctrlPreferences)
            {
                objGeneric.ScrollToElement(ctrlPreference);
                if (ctrlPreference.Selected)
                {
                    ctrlPreference.Click();
                }
            }
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "SelectSecurityPreferences", "Ending"));
            return error.ToString();
        }

        /// <summary>
        /// method to get all opted security preference ids as per UI
        /// </summary>
        /// <returns></returns>
        public List<Int16> GetUIOptedSecurityPrefIds()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetUIOptedSecurityPrefIds", "Starting"));
            List<Int16> prefIds = new List<short>();
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            if (string.IsNullOrEmpty(preferenceUIConfig.ConfigurationValue1))
            {
                Assert.Fail(string.Format("Preference Configuration value is null in DBConfig - custID: {0}, Country: {1}", Login.CustomerID, CountrySetting.country));
            }
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            prefUI.RemoveAll(p => !p.isvisible);
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetUIOptedSecurityPrefIds", "get PreferencesUIConfig"));

            DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
            bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("TRUE");

            List<IWebElement> ctrlPreferences = GetVisibleSecurityPrefCheckBoxes();
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetUIOptedSecurityPrefIds", "get visible checkboxes:" +  ctrlPreferences.Count));
            int index = 0;
            foreach (IWebElement prefCtrl in ctrlPreferences)
            {
                PreferencesUIConfig pref = prefUI[index++];
                if (isOptinBehavior)
                {
                    if (prefCtrl.Selected)
                    {
                        prefIds.Add(pref.preferenceid);
                        if (pref.dependentprefidsassame != null)
                        {
                            prefIds.AddRange(pref.dependentprefidsassame);
                        }
                    }
                }
                else
                {
                    if (!prefCtrl.Selected)
                    {
                        prefIds.Add(pref.preferenceid);
                        if (pref.dependentprefidsassame != null)
                        {
                            prefIds.AddRange(pref.dependentprefidsassame);
                        }
                    }
                }
            }
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetUIOptedSecurityPrefIds", "Ending"));
            return prefIds;
        }

        /// <summary>
        /// method to validate that correct number of security preference check boxes are present 
        /// </summary>
        /// <returns></returns>
        public string ValidateSecurityPreferences()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "ValidateSecurityPreferences", "Starting"));
            StringBuilder error = new StringBuilder();
            Int32 expectedCount = GetExpectedSecurityPrefCheckBoxes();
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "ValidateSecurityPreferences", "enabled pref count:" + expectedCount));
            List<IWebElement> ctrlPreferences = GetVisibleSecurityPrefCheckBoxes();
            if (ctrlPreferences.Count != expectedCount)
            {
                error.Append(string.Format("As per config enabled preference ids '{0}', but checkboxes count {1}. ", expectedCount, ctrlPreferences.Count));
            }
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "ValidateSecurityPreferences", "Ending"));
            return error.ToString();
        }

        private List<IWebElement> GetVisibleSecurityPrefCheckBoxes()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetVisibleSecurityPrefCheckBoxes", "Starting"));
            IWebElement prefContainer = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.OPTINS_CONTAINER), FindBy.CSS_SELECTOR_CSS);
            List<IWebElement> ctrlPreferences = prefContainer.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.CHECKBOX).XPath)).ToList();
            // check if control is visible
            List<string> ctrlInvisible = new List<string>();
            for (int i = 0; i < ctrlPreferences.Count; i++)
            {
                try
                {
                    objGeneric.ScrollToElement(ctrlPreferences[i]);
                    ctrlPreferences[i].SendKeys(Keys.Space);
                }
                catch (ElementNotVisibleException)
                {
                    ctrlInvisible.Add(ctrlPreferences[i].GetAttribute("id"));
                }
            }
            ctrlPreferences.RemoveAll(c => ctrlInvisible.Contains(c.GetAttribute("id")));
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetVisibleSecurityPrefCheckBoxes", "Ending"));
            return ctrlPreferences;
        }

        private Int32 GetExpectedSecurityPrefCheckBoxes()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetExpectedSecurityPrefCheckBoxes", "Starting"));
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            if (string.IsNullOrEmpty(preferenceUIConfig.ConfigurationValue1))
            {
                Assert.Fail(string.Format("Preference Configuration value is null in DBConfig - custID: {0}, Country: {1}", Login.CustomerID, CountrySetting.country));
            }
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            prefUI.RemoveAll(p => !p.isvisible);
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetExpectedSecurityPrefCheckBoxes", "count" + prefUI.Count));
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "GetExpectedSecurityPrefCheckBoxes", "Eding"));
            return prefUI.Count;
        }

        /// <summary>
        /// Method to get the selected security preferences count
        /// </summary>
        /// <returns></returns>
        public void ValidateSelectedSecurityPreferences()
        {
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "ValidateSelectedSecurityPreferences", "Starting"));
            List<Int16> expectedPrefIds = preferenceAdaptor.GetOptedPreferences(Login.CustomerID);
            List<Int16> actualPrefIds = GetUIOptedSecurityPrefIds();
            expectedPrefIds.Sort();
            actualPrefIds.Sort();
            CollectionAssert.AreEqual(expectedPrefIds, actualPrefIds);
            Debug.WriteLine(string.Format("{0}|{1}|{2}", 2, "ValidateSelectedSecurityPreferences", "Ending"));
        }


        #endregion

        public void ContactPreferences_Email()
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Email opted verification", TraceEventType.Start);
                IWebElement radioButtonEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_EMAIL).Id));
                Assert.AreEqual(radioButtonEmail.Enabled, true, "ContactPreferences is Not selected as email");
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }



        }

        public void ContactPreferences_EmailText(string Clubcard)
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Email Address verification", TraceEventType.Start);
                IWebElement radioButtonEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_EMAIL).Id));
                if (radioButtonEmail.Selected == true)
                {

                    CustomerServiceAdaptor customer = new CustomerServiceAdaptor();

                    string customerId = customer.GetCustomerID(Clubcard, CountrySetting.culture).ToString();
                    string emailid = customer.GetEmailId(Clubcard, CountrySetting.culture);
                    var EmailAddress = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.EMAILTEXT_VARIFICATION).Id)).GetAttribute("value");
                    Assert.AreEqual(emailid, EmailAddress, "Email address is not verified");

                }
                else
                {
                    CustomLogs.LogMessage("Email is not selected as preference", TraceEventType.Start);
                }
            }


            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }


        }

        public void ContactPreferences_Phonenumber(string Clubcard)
        {
            try
            {

                string culture = CountrySetting.country;
                if (culture == "SK" || culture == "CZ")
                {
                    CustomLogs.LogMessage("Phone Number is not Applicable for this country preference", TraceEventType.Start);
                }

                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Email Address verification", TraceEventType.Start);
                    IWebElement radioButtonPhoneNumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_SMS).Id));
                    if (radioButtonPhoneNumber.Selected == true)
                    {

                        CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                        string customerId = customer.GetCustomerID(Clubcard, CountrySetting.culture).ToString();
                        string phoneNumber = customer.GetPhoneNumber(customerId, CountrySetting.culture);
                        //string nameExpectedPhoneNumber = (phoneNumber.Remove(3, 1));
                        var Phonenumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PHONENUMBER_VARIFICATION).Id)).GetAttribute("value");
                        Assert.AreEqual(phoneNumber, Phonenumber, "Phone Number is not verified");
                    }

                    else
                    {
                        CustomLogs.LogMessage("Phone Number is not selected as preference", TraceEventType.Start);
                    }
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ContactPreferences_ConfirmPhonenumber(string Clubcard)
        {
            try
            {

                string culture = CountrySetting.country;
                if (culture == "SK" || culture == "CZ")
                {
                    CustomLogs.LogMessage("Phone Number is not Applicable for this country preference", TraceEventType.Start);
                }

                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("confirm Phone verification", TraceEventType.Start);
                    IWebElement radioButtonPhoneNumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_SMS).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    jse.ExecuteScript("arguments[0].click();", radioButtonPhoneNumber);

                    var Phonenumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CONFIRMPHONENUMBER_VARIFICATION).Id)).GetAttribute("value");
                    if (Phonenumber.ToString() == "")
                    {

                    }
                    else
                    {
                        Assert.Fail("confirm phone number value is not cleared");
                    }

                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ContactPreferences_ConfirmEmail(string Clubcard)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("confirm Phone verification", TraceEventType.Start);
                IWebElement radioButtonEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_EMAIL).Id));
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                jse.ExecuteScript("arguments[0].click();", radioButtonEmail);

                var confirmEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CONFIRMEMAILTEXT_VARIFICATION).Id)).GetAttribute("value");
                if (confirmEmail.ToString() == "")
                {

                }
                else
                {
                    Assert.Fail("confirm email value is not cleared");
                }

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ContactPreferenceLabels(string msgId1, string msgId2, String pageName, string resourceFileName)
        {
            try
            {

                string culture = CountrySetting.country;
                if (culture == "MY")
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                    //  Fetch Details From resource.XML
                    Resource res1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage1 = res1.Value;
                    var actualMessage1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLEONE_VARIFICATION).XPath)).Text;
                    Assert.AreEqual(expectedMessage1, actualMessage1, pageName + "label1 is not present");
                }
                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                    //  Fetch Details From resource.XML
                    Resource res1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage1 = res1.Value;
                    Debug.WriteLine(string.Format("{0} - {1}", expectedMessage1, "expected message"));
                    Resource res2 = AutomationHelper.GetResourceMessage(msgId2, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage2 = res2.Value;
                    Debug.WriteLine(string.Format("{0} - {1}", expectedMessage2, "expected message"));
                    var actualMessage1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLEONE_VARIFICATION).XPath)).Text;
                    var actualMessage2 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLETWO_VARIFICATION).XPath)).Text;
                    Assert.AreEqual(expectedMessage1, actualMessage1, pageName + "label1 is not present");
                    Assert.AreEqual(expectedMessage2, actualMessage2, pageName + "lable2 is not present");

                }

            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);


        }

        public void ContactPreferences_BAAviosPremium()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("BA Avios premium is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonBAAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonBAAvios.Enabled, true, "BA Avios is Not selected as email");
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ContactPreferences_BAAviosStanderd()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("BA Avios Standerd is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonAvios.Enabled, true, "BA Avios is Not selected as email");

                }
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ContactPreferences_AviosPremium()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("Avios Premium is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonAvios.Enabled, true, "BA Avios is Not selected as email");
                }
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }


        public void ContactPreference_checkIfGridExist()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Checking if grid exist on the page started", TraceEventType.Start);
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.GRIDTABLE).Id), Driver))
                    CustomLogs.LogInformation("Grid Exists on the page");
                else
                    Assert.Fail("Grid doesn't exists");
                CustomLogs.LogMessage("Grid is present on the page", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_checkIfGridDoesnotExist()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExiststarted", TraceEventType.Start);
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.GRIDTABLE).Id), Driver))
                    Assert.Fail("Grid Exists on the Page");
                else
                    CustomLogs.LogInformation("Grid doesn't exist on page");
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExist completed", TraceEventType.Stop);

                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.NOGRID).XPath), Driver))
                    CustomLogs.LogInformation("Check box Exists on the Page");
                else
                    Assert.Fail("Check box doesn't Exists on Contact Preference");
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExist completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_SelectProductServiceGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectProductServiceGridFalse started", TraceEventType.Start);
                IWebElement productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPTESCOPRODUCT).Id));
                if (!productElement.Selected)
                {
                    productElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }

                CustomLogs.LogMessage("ContactPreference_SelectProductServiceGridFalse completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void SelectMailingOptionChkBox(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("SelectMailingOptionChkBox started", TraceEventType.Start);
                chkBoxElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                if (chkBoxElement.Selected)
                {
                    chkBoxElement.Click();
                    value = "Unchecked";
                    CustomLogs.LogInformation("Check box for Product services unchecked");
                }
                else
                {
                    chkBoxElement.Click();
                    value = "Checked";
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("SelectMailingOptionChkBox completed", TraceEventType.Stop);
        }

        public void VerifyMailingOptionCheckBox(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("VerifyMailingOptionCheckBox started", TraceEventType.Start);
                chkBoxElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                switch (value)
                {
                    case "Checked":
                        if (chkBoxElement.Selected)
                            CustomLogs.LogInformation("Checkbox is selected/Checked");
                        else
                            Assert.Fail("Checkbox is not selected/Checked");
                        break;
                    case "Unchecked":
                        if (!chkBoxElement.Selected)
                            CustomLogs.LogInformation("Element is unchecked");
                        else
                            Assert.Fail("Checkbox is selected/Checked");
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("VerifyMailingOptionCheckBox Completed", TraceEventType.Stop);
        }

        public void ContactPreference_SelectContactPermissionGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectContactPermissionGridFalse started", TraceEventType.Start);
                IWebElement ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                if (!ContactElement.Selected)
                {
                    ContactElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }
                CustomLogs.LogMessage("ContactPreference_SelectContactPermissionGridFalse completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_UnSelectContactPermissionGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_UnSelectContactPermissionGridFalse started", TraceEventType.Start);
                IWebElement ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                if (!ContactElement.Selected)
                {
                    ContactElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is Unselected");
                }

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("ContactPreference_UnSelectContactPermissionGridFalse completed", TraceEventType.Stop);
        }

        public void ContactPreference_CheckAllCheckBoxesOpt(string value, string option)
        {
            try
            {
                IWebElement productElement, PartnerElement, ContactElement = null;

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_CheckAllCheckBoxesOptIn started", TraceEventType.Start);

                if (option.Equals("UK"))
                {
                    productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKTESCOPRODUCT).Id));
                    PartnerElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKPARTNEROFFER).Id));
                    ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKRESEARCH).Id));
                }
                else
                {
                    productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPTESCOPRODUCT).Id));
                    PartnerElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPPARTNEROFFER).Id));
                    ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                }
                List<IWebElement> CheckOptIn = new List<IWebElement>();
                CheckOptIn.Add(productElement);
                CheckOptIn.Add(PartnerElement);
                CheckOptIn.Add(ContactElement);

                switch (value)
                {
                    case "Out":
                        for (int i = 0; i < CheckOptIn.Count(); i++)
                        {
                            if (CheckOptIn[i].Selected)
                            {
                                CustomLogs.LogInformation("Check Box is selected/Checked");
                                CheckOptIn[i].Click();
                                CustomLogs.LogInformation("Check box " + i + " Unchecked");
                            }
                            //else
                            // Assert.Fail("Check box " + i + " not Checked");
                        }
                        break;
                    case "In":
                        for (int i = 0; i < CheckOptIn.Count(); i++)
                        {
                            if (!CheckOptIn[i].Selected)
                            {
                                CustomLogs.LogInformation("Check Box is not selected");
                                CheckOptIn[i].Click();
                                CustomLogs.LogInformation("Check box " + i + " checked");
                            }
                            // else
                            // Assert.Fail("Check box " + i + " Checked");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("ContactPreference_CheckAllCheckBoxesOptIn completed", TraceEventType.Stop);
        }

        public void ContactPreference_verifySelectedCheckBox()
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_verifySelectedCheckBox started", TraceEventType.Start);
                ReadOnlyCollection<IWebElement> ele = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LIST_CHECKBOXES_PR).XPath));
                el = ele.ToList();
                switch (value)
                {
                    case "Unchecked":
                        for (int i = 0; i < checkBoxNumber.Count(); i++)
                        {
                            if (!el[checkBoxNumber[i]].Selected)
                                CustomLogs.LogInformation("Checkbox " + checkBoxNumber[i] + " unchecked, hence verified");
                            else
                                Assert.Fail("Checkbox" + checkBoxNumber[i] + " is still checked");
                        }
                        break;
                    case "Checked":
                        for (int i = 0; i < checkBoxNumber.Count(); i++)
                        {
                            if (el[checkBoxNumber[i]].Selected)
                                CustomLogs.LogInformation("Checkbox " + checkBoxNumber[i] + " checked, hence verified");
                            else
                                Assert.Fail("Checkbox" + checkBoxNumber[i] + " is still unchecked");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("ContactPreference_verifySelectedCheckBox Completed", TraceEventType.Stop);
        }

        public void ContactPreference_SelectAllOrParticularCheckBox()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectAllCheckBox started", TraceEventType.Start);
                ReadOnlyCollection<IWebElement> ele = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LIST_CHECKBOXES_PR).XPath));
                el = ele.ToList();
                for (int i = 0; i < el.Count; i++)
                {
                    if (!el[i].Selected)
                        totalCheckBox.Add(i);
                }
                if (totalCheckBox.Count.Equals(0))
                {
                    CustomLogs.LogInformation("All checkBoxes are checked, so unchecking all.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        el[i].Click();
                        checkBoxNumber.Add(i);
                        value = "Unchecked";
                        CustomLogs.LogInformation("unchecked checkbox " + i);
                    }
                }
                else if (totalCheckBox.Count.Equals(12))
                {
                    CustomLogs.LogInformation("All checkBoxes are unchecked, so checking all.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        el[i].Click();
                        checkBoxNumber.Add(i);
                        value = "Checked";
                        CustomLogs.LogInformation("checked checkbox " + i);
                    }

                }
                else
                {
                    CustomLogs.LogInformation("Few checkBoxes are unchecked, so checking rest.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        if (!el[i].Selected)
                        {
                            el[i].Click();
                            checkBoxNumber.Add(i);
                            value = "Checked";
                            CustomLogs.LogInformation("checked checkbox " + i);
                        }

                    }
                }

                CustomLogs.LogMessage("ContactPreference_SelectAllOrOneCheckBox Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_SelectParticularCheckBox()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectParticularCheckBox started", TraceEventType.Start);
                IWebElement chkbox1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKBOXEMAIL).Id));
                IWebElement chkbox2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKBOXSMS).Id));

                List<IWebElement> ChkBox = new List<IWebElement>();
                ChkBox.Add(chkbox1);
                ChkBox.Add(chkbox2);

                for (int i = 0; i < ChkBox.Count(); i++)
                {
                    if (!ChkBox[i].Selected)
                    {
                        ChkBox[i].Click();
                        CustomLogs.LogInformation("Check box clicked");
                    }
                    else
                    {
                        ChkBox[i].Click();
                        CustomLogs.LogInformation("CheckBox already clicked");
                    }
                }

                CustomLogs.LogMessage("ContactPreference_SelectParticularCheckBox Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void VerifyTextForPost(string msgId, string msgId1, string keys, string LocalResourceFileName, string address1, int no, FindBy query)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedMessage = string.Empty;
                string expectedMessage1 = string.Empty;
                IWebElement ctrl = null;

                CustomLogs.LogMessage("Verifying validation message for post section started", TraceEventType.Start);

                switch (no)
                {
                    case 1:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage = string.Concat(expectedMessage, " ", address1);
                        break;
                    case 2:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage = string.Concat(expectedMessage, expectedMessage1);

                        break;
                    case 3:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;

                        expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage = string.Concat(expectedMessage, " ", expectedMessage1);

                        expectedMessage = expectedMessage.Trim();
                        break;
                    case 4:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage = string.Concat(expectedMessage, expectedMessage1);
                        break;
                }

                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName));
                        break;
                }
                var actualMessage = ctrl.Text;

                objGeneric.ValidateResourceValueWithHTMLContent(actualMessage.Trim(), expectedMessage.Trim());
                //Assert.AreEqual(expectedMessage.Trim(), actualMessage.Trim(), " not verified");
                CustomLogs.LogMessage("Verifying validation message for post section completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyTextCallPhn(string msgId, string msgId1, string keys, string LocalResourceFileName, FindBy query)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedMessage = string.Empty;
                string expectedMessage1 = string.Empty;
                IWebElement ctrl = null;

                CustomLogs.LogMessage("Verifying text for call Phone rate started", TraceEventType.Start);

                expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;

                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName));
                        break;
                }
                var actualMessage = ctrl.Text;
                if (actualMessage.Contains(expectedMessage) && actualMessage.Contains(expectedMessage1))
                {
                    CustomLogs.LogInformation("Call Rate Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Call Rate Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying text for call Phone rate  completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyTextOptin(string msgId, string msgId1, string keys, string keys1, string LocalResourceFileName, string address1, int no, FindBy query)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedMessage = string.Empty;
                string expectedMessage1 = string.Empty;
                IWebElement ctrl = null;
                IWebElement ctrl1 = null;
                CustomLogs.LogMessage("Verifying validation message for post section started", TraceEventType.Start);

                switch (no)
                {
                    case 1:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage = string.Concat(expectedMessage, " ", address1);
                        break;
                    case 2:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;

                        expectedMessage = string.Concat(expectedMessage, expectedMessage1);
                        break;
                }

                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                        ctrl1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys1).Id));
                        //ctrl.ToString() = ctrl.Text + ctrl1.Text;
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName));
                        break;
                }
                var actualMessage = ctrl.Text + ctrl1.Text;
                Assert.AreEqual(expectedMessage, actualMessage, " not verified");
                CustomLogs.LogMessage("Verifying validation message for post section completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public string verifyLargeBrailleStatement(PreferenceServiceAdaptor objPrefService)
        {
            string error = string.Empty;
            try
            {
                CustomLogs.LogMessage(" Verify large braille statement started", TraceEventType.Start);
                if (objPrefService.IsContactPreferenceEnabled(Login.CustomerID, (int)ContactPreference.Post_Large_Print))
                {
                    objGeneric.ValidateText(ControlKeys.PREFERENCE_LBLLARGESTATE, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE, new List<string> { LabelKey.CONTACT_PREFERENCE_LBLBRAILLESTAT }, FindBy.XPATH_SELECTOR);
                    //objGeneric.verifyValidationMessageByClass(LabelKey.CONTACT_PREFERENCE_LBLBRAILLESTAT, ControlKeys.PREFERENCE_LBLLARGESTATE, "Preference", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                    bool LargeStatOpted = objPrefService.IsContactPreferenceOpted(Login.CustomerID, (int)ContactPreference.Post_Large_Print);
                    bool BrailleStatOpted = objPrefService.IsContactPreferenceOpted(Login.CustomerID, (int)ContactPreference.Braille);
                    if ((LargeStatOpted == false && BrailleStatOpted == false))
                        objGeneric.verifyValidationMessageByValue(LabelKey.CONTACT_PREFERENCE_LBLBRAILLESTATEMENTNOTOPTIN, ControlKeys.PREFERENCE_TXTLBSTAT, "Preference", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                    else if ((LargeStatOpted == true && BrailleStatOpted == false))
                        objGeneric.verifyValidationMessageByValue(LabelKey.CONTACT_PREFERENCE_LBLLARGEPRINTOPTED, ControlKeys.PREFERENCE_TXTLBSTAT, "Preference", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                    else if ((LargeStatOpted == false && BrailleStatOpted == true))
                        objGeneric.verifyValidationMessageByValue(LabelKey.CONTACT_PREFERENCE_LBLBRAILLESTAT, ControlKeys.PREFERENCE_TXTLBSTAT, "PReference", SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE);
                    
                }
                CustomLogs.LogMessage(" Verify large braille statement completed", TraceEventType.Stop);

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return error;
        }

        public bool IsPreferenceEnabled(ContactPreference contactPreference, string Username)
        {
            bool expectedIsVisible = false;
            try
            {
                PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
                CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
                long CustomerID = customerAdaptor.GetCustomerID(Username, CountrySetting.culture);
                expectedIsVisible = prefClient.IsContactPreferenceEnabled(CustomerID, (int)contactPreference);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return expectedIsVisible;
        }
    }
}
