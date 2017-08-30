using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.IO;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Services;
using OpenQA.Selenium.Support.UI;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using Tesco.NGC.RestClient;

namespace Tesco.Framework.UITesting.Test.Common
{
    class PersonalDetails : Base
    {
        CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
        public static Int64 CustomerID = 0;
        string PostcodeDB = string.Empty;
        bool a;
        TestData_PersonalDetails testData = null;
        Generic objGeneric = null;
        private bool _AddressAPIEnabled = false;

        #region Constructor

        public PersonalDetails(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }
        public PersonalDetails(AutomationHelper objHelper, AppConfiguration configuration, TestData_PersonalDetails testData)
        {
            this.testData = testData;
            ObjAutomationHelper = objHelper;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);

            bool bAddressAPIEnabled = false;

            var addressAPICfg = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.IS_ADDRESSAPI_ENABLED, SanityConfiguration.DbConfigurationFile);

            this._AddressAPIEnabled = bool.TryParse(addressAPICfg.ConfigurationValue1, out bAddressAPIEnabled) && bAddressAPIEnabled && !String.IsNullOrWhiteSpace(addressAPICfg.ConfigurationValue2);

        }
        #endregion

        #region Methods
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void EnterPostcode(string Postcode)
        {
            try
            {
                CustomLogs.LogMessage("EnterPostcode started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                IWebElement txtPostCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id));
                txtPostCode.Clear();
                txtPostCode.SendKeys(Postcode);
                CustomLogs.LogMessage("EnterPostcode completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public string SelectPostCodeOutOfRange(string validPostCode)
        {
            List<string> postcode = new List<string>();
            string PostCodeOutOfRange = string.Empty;
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Length_of_the_input_fields, DBConfigKeys.OUTOFRANGEPOSTCODE, SanityConfiguration.DbConfigurationFile);
            string minLength = config.ConfigurationValue1;
            int minlength = int.Parse(minLength);
            string maxLength = config.ConfigurationValue2;
            int maxlength = int.Parse(maxLength);
            int actuallength = validPostCode.Length;
            if (Enumerable.Range(minlength, maxlength).Contains(actuallength))
            {
                int count = validPostCode.Length - 1;
                for (int i = 0; i < count - 1; i++)
                {
                    postcode.Add(validPostCode[i].ToString());
                }
                PostCodeOutOfRange = string.Concat(postcode);

            }
            else
                PostCodeOutOfRange = validPostCode;
            return PostCodeOutOfRange;
        }
        public void ValidateErrorMessage()
        {
            try
            {
                CustomLogs.LogMessage("ValidateErrorMessage started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                var enteredPostCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)).GetAttribute("value");
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ERRORINVALIDPOSTCODE).Id)).Text;
                var expectedErrorMsg1 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg2 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg3 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg4 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE4, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg5 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE5, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg = expectedErrorMsg1 + " " + "\"" + enteredPostCode + "\""  + "\r\n" + expectedErrorMsg2 + "\r\n" + expectedErrorMsg3 + "\r\n" + expectedErrorMsg4 + "\r\n" + expectedErrorMsg5;
                Assert.AreEqual(actualErrorMsg, expectedErrorMsg, " Error Messages doesn't match");
                CustomLogs.LogMessage("ValidateErrorMessage completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void VerifyPostcodeInDB(string clubcard)
        {
            try
            {
                CustomLogs.LogMessage("VerifyPostcodeInDB started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                //Postcode from UI
                var enteredPostCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)).GetAttribute("value");

                //Postcode from DB 
                CustomerID = customerAdaptor.GetCustomerID(clubcard, CountrySetting.culture);
                PostcodeDB = customerAdaptor.GetPostCode(CustomerID, CountrySetting.culture);
                Assert.AreEqual(enteredPostCode, PostcodeDB, "Postcode on UI doesn't match with DB");
                CustomLogs.LogMessage("VerifyPostcodeInDB completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void verifySpaceInPostcode()
        {
            CustomLogs.LogMessage("verifySpaceInPostcode started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            var enteredPostCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)).GetAttribute("value");
            int postCodeLenght = enteredPostCode.Split(' ')[1].Count();
            Assert.AreEqual(3, postCodeLenght, "Space not before last 3 characters");
            CustomLogs.LogMessage("verifySpaceInPostcode completed", TraceEventType.Stop);
        }
        public void Checkamend()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_CheckAmendText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_CheckText).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void YCDetailsTXT()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_YCDETAILSText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YCDETAILSTXT).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void REQUIREDWARNINGText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_REQUIREDWARNINGText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_REQUIREDWARNINGTXT).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void YouHouseholdDetailsTitleText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_YOURHOUSEHOLDDETAILSTITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YOURHOUSEHOLDDETAILSTitleTXT).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void YouHouseholdDetailsText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText1 = AutomationHelper.GetResourceMessage(ValidationKey.PD_YOURHOUSEHOLDDETAILS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedText = expectedText1.Replace("\n", "");
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YOURHOUSEHOLDDETAILSTXT).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void ContactUSText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText1 = AutomationHelper.GetResourceMessage(ValidationKey.PD_CONTACTUSTEXT1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedText2 = AutomationHelper.GetResourceMessage(ValidationKey.PD_CONTACTUSTEXT2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_CONTACTUSTXT).Id)).Text;
            var actualText = actualText1.Replace("\r\n", " ");
            var expectedText = string.Concat(expectedText1, " ", expectedText2);
            Assert.AreEqual(expectedText, actualText);
        }
        public void PageDecriptionText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedstatementOne = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCONEText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var ActualStatementOne = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTONE).Id)).Text;
            Assert.AreEqual(expectedstatementOne, ActualStatementOne);
            var expectedTextTwoStart = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextstart, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextTwoLink = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextlink, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextTwoEnd = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextend, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var ActualStatementTWo = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTTWO).Id)).Text;
            var ExpectedStatementTwo = string.Concat(expectedTextTwoStart, " ", expectedTextTwoLink, " ", expectedTextTwoEnd);
            Assert.AreEqual(ActualStatementTWo, ExpectedStatementTwo);
            var expectedTextThreeStart = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextstart, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextThreeLink = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextlink, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var ActualStatementThree = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTTHREE).Id)).Text;
            var ExpectedStatementThree = string.Concat(expectedTextThreeStart, " ", expectedTextThreeLink);
            Assert.AreEqual(ExpectedStatementThree, ActualStatementThree);
            var expectedTextStatementFour = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextend, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualTextfour = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTFOUR).Id)).Text;
            Assert.AreEqual(actualTextfour, actualTextfour);

        }
        public void ReturnAllFieldValues()
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, string> fields = FieldValuesFromUI(Enums.FieldType.Valid);
            CustomerServiceAdaptor cusDetails = new CustomerServiceAdaptor();
            Dictionary<string, string> CustomerDetail = cusDetails.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);
            if (CustomerDetail.ContainsKey("family_member_1_dob"))
            {
                DateTime dob = new DateTime();
                dob = Convert.ToDateTime(CustomerDetail["family_member_1_dob"]);
                CustomerDetail.Add("Day", dob.Day.ToString());
                CustomerDetail.Add("Month", dob.Month.ToString());
                CustomerDetail.Add("Year", dob.Year.ToString());
            }
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            CustomerDetail.Add("FName", textInfo.ToTitleCase(CustomerDetail["Name1"]));
            CustomerDetail.Add("LName", textInfo.ToTitleCase(CustomerDetail["Name3"]));

            int iCount = 0;
            StringBuilder msg = new StringBuilder();
            foreach (KeyValuePair<string, string> entry in fields)
            {
                if (!CompareCustomerDetails(entry, CustomerDetail))
                {
                    msg.Append(entry.Key);
                    msg.Append(",");
                    iCount++;
                }
            }
            if (!string.IsNullOrEmpty(msg.ToString()))
            {
                msg.Remove(msg.Length - 1, 1);
            }
            Assert.IsTrue(iCount == 0);
        }
        public bool CompareCustomerDetails(KeyValuePair<string, string> entry, Dictionary<string, string> customerDetails)
        {

            bool isvalid = false;
            string expectedDataKey = string.Empty;
            switch (entry.Key)
            {
                case "Name1":
                    expectedDataKey = "FName";
                    break;
                case "Name2":
                    expectedDataKey = "Name2";
                    break;
                case "Name3":
                    expectedDataKey = "LName";
                    break;
                case "EmailAddress":
                    expectedDataKey = "email_address";
                    break;
                case "MailingAddressPostCode":
                    expectedDataKey = "MailingAddressPostCode";
                    break;
                case "Date":
                    expectedDataKey = "Day";
                    break;
                case "Month":
                    expectedDataKey = "Month";
                    break;
                case "Year":
                    expectedDataKey = "Year";
                    break;
                case "Sex":
                    expectedDataKey = "Sex";
                    break;
                case "MobilePhoneNumber":
                    expectedDataKey = "mobile_phone_number";
                    break;

            }
            if (expectedDataKey != string.Empty && entry.Value.Equals(customerDetails[expectedDataKey]))
            {
                return true;
            }
            return isvalid;


        }
        public Dictionary<string, string> FieldValuesFromUI(Enums.FieldType type)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, string> FieldValues = new Dictionary<string, string>();

            switch (type)
            {
                case Enums.FieldType.Valid:
                    FieldValues.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EMAIL).Id)).GetAttribute("value"));
                    FieldValues.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MIDDLENAME).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_FIRSTNAME).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_SURNAME).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EVENINGNUMBER).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MOBILENUMBER).Id)).GetAttribute("value"));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        FieldValues.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PHONENUMBER).Id)).GetAttribute("value"));
                    break;
            }
            if (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOMALE).Id)).Selected == true)

            { FieldValues.Add("Sex", "M"); }
            else
            { FieldValues.Add("Sex", "F"); }

            FieldValues.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DAY).Id)).GetAttribute("value"));
            FieldValues.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MONTH).Id)).GetAttribute("value"));
            FieldValues.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YEAR).Id)).GetAttribute("value"));

            return FieldValues;
        }
        public bool CheckProvice(string clubcard, string key, Services.Enums.CustPersonalDetail personaldetail)
        {
            Driver = ObjAutomationHelper.WebDriver;
            List<string> ValueFromService;
            List<string> ValueFromUI = new List<string>();
            bool isListEqual = false;
            var result = String.Empty;
            IWebElement elem = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
            ReadOnlyCollection<IWebElement> option = (elem.FindElements(By.TagName("option")));
            for (int i = 1; i < option.Count; i++)
            {
                ValueFromUI.Add(option[i].Text.Trim());
            }
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            ValueFromService = customerAdaptor.GetProvince(clubcard, CountrySetting.culture, personaldetail);
            if (ValueFromUI.Count.Equals(ValueFromService.Count))
                isListEqual = new HashSet<String>(ValueFromUI).SetEquals(ValueFromService);
            else
                Assert.Fail("Count different");
            return isListEqual;
        }
        public void checkIfElementPresent(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("checkIfElementPresent started", TraceEventType.Start);
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(key).Id), Driver))
                    CustomLogs.LogInformation("Element exists");
                else
                    Assert.Fail("Element does not exists on the page");
                CustomLogs.LogMessage("checkIfElementPresent stopped", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        
        public void VerifyReplacementText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                var expectedmsg = ValidationKey.PERSONALDETAILS_REPLACEMENTTEXT;
                var actualmsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_REPLACEMENTTEXT).Id)).Text;
                Assert.AreEqual(expectedmsg, actualmsg, expectedmsg + " not equal to " + actualmsg);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public int EnterInvalidLength(string configKey, string controlKey, Enums.FieldType type, string fieldType)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var prefixFromDBConfig = String.Empty;
                var prefix = String.Empty;
                var finalString = String.Empty;
                int lengthFromConfig = objGeneric.returnConfigLength(ConfugurationTypeEnum.Length_of_the_input_fields, configKey, type);
                var rng = new Random(Environment.TickCount);
                if (fieldType.Contains("Number"))
                {
                    prefix = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Prefix, configKey, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                    if (prefix.Contains(","))
                        prefixFromDBConfig = prefix.Split(',')[0];
                    else
                        prefixFromDBConfig = prefix;
                    int prefixlength = prefixFromDBConfig.Length;
                    int randomNumberLength = lengthFromConfig - prefixlength;
                    string phoneNumber = string.Concat(Enumerable.Range(0, randomNumberLength).Select((index) => rng.Next(10).ToString()));
                    finalString = prefixFromDBConfig + phoneNumber;
                }
                else
                    finalString = RandomString(lengthFromConfig);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).Clear();
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys(finalString);
                return finalString.Length;
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return -99;
            }

        }
        public void ValidateMaxLength(ConfugurationTypeEnum configtype, string configKey, string controlKey)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                int configValue2 = Int32.Parse(AutomationHelper.GetDBConfiguration(configtype, configKey, SanityConfiguration.DbConfigurationFile).ConfigurationValue2);
                int MobileNumberLengthFromUI = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).GetAttribute("value").Length;
                Assert.AreEqual(configValue2, MobileNumberLengthFromUI, "");
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }
        public string getPhoneNumberPrefix(string dbconfigKeys)
        {
            var prefix = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Prefix, dbconfigKeys, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            if (prefix.Contains(","))
                return prefix.Split(',')[0];
            else
                return prefix;
        }
        public string getValidPhoneNumber(string configKeys)
        {
            var prefixFromDBConfig = String.Empty;
            var rng = new Random(Environment.TickCount);
            prefixFromDBConfig = getPhoneNumberPrefix(configKeys);
            int prefixLength = prefixFromDBConfig.Length;
            int minLength = Int32.Parse(AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Length_of_the_input_fields, configKeys, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
            string phoneNumber = prefixFromDBConfig + string.Concat(Enumerable.Range(0, minLength - prefixLength).Select((index) => rng.Next(10).ToString()));
            return phoneNumber;
        }
        public void SetPersonalDetailFields(FieldType type)
        {
            switch (type)
            {
                case FieldType.All:
                    SetValuesAllFields(ConfugurationTypeEnum.ChinaHiddenFunctionality, "MyPersonalDetails", type);
                    SetSpecificFields(ConfugurationTypeEnum.ChinaHiddenFunctionality, "MyPersonalDetails", type);
                    
                    break;
                case FieldType.Mandatory:
                    SetValuesAllFields(ConfugurationTypeEnum.Mandatory_fields, "MyPersonalDetails",type);
                    SetSpecificFields(ConfugurationTypeEnum.Mandatory_fields, "MyPersonalDetails",type);
                    break;
            }
        }
        private void SetSpecificFields(ConfugurationTypeEnum configType, string controlCatagory,FieldType type)
        {
            try
            {
                List<DBConfiguration> fields = new List<DBConfiguration>();
                fields = AutomationHelper.GetDBConfigurations(configType, SanityConfiguration.DbConfigurationFile);
                //fields = fields.FindAll(f => f.IsDeleted.ToUpper().Equals("N") && !f.ConfigurationValue1.Equals("1"));
                if (type.Equals(FieldType.All))
                {
                    fields = fields.FindAll(f => !f.ConfigurationValue1.Equals("1"));
                }
                else if (type.Equals(FieldType.Mandatory))
                {
                    fields = fields.FindAll(f => !f.ConfigurationValue1.Equals("0") && f.IsDeleted.Equals("N"));
                }

                List<Control> controls = ObjAutomationHelper.GetControls(controlCatagory);
                if (configType == ConfugurationTypeEnum.ChinaHiddenFunctionality)
                {
                    controls = (from t in controls
                                where fields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                select t).ToList();
                }
                else
                {
                    List<DBConfiguration> availableFields = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.ChinaHiddenFunctionality, SanityConfiguration.DbConfigurationFile);
                    controls = (from t in controls
                                where fields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                & availableFields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                select t).ToList();
                }

                // set post code control data
                Control pcode = controls.Find(c => c.Key == "txtpostcode");
                if (pcode != null)
                {
                    IWebElement uiPcode = objGeneric.GetWebControl(pcode, FindBy.CSS_SELECTOR_ID);
                    if (uiPcode != null)
                    {
                        uiPcode.Clear();
                        uiPcode.SendKeys(testData.ValidPostode);
                    }
                }
                //Set Title
                Control title = controls.Find(c => c.Key == "title");
                if (title != null)
                {
                    IWebElement uiTitle = objGeneric.GetWebControl(title, FindBy.CSS_SELECTOR_ID);
                    var mySelect = new SelectElement(uiTitle);
                    mySelect.SelectByValue("Mr");
                }
                //Set Gender
                Control gender = controls.Find(c => c.Key == "btnmaleradio");
                if (gender != null)
                {
                    IWebElement uiGender = objGeneric.GetWebControl(gender, FindBy.CSS_SELECTOR_ID);
                    if (uiGender.Selected.ToString() == "True")
                    {

                    }
                    else
                    {
                        Driver = ObjAutomationHelper.WebDriver;
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                        jse.ExecuteScript("arguments[0].click();", uiGender);
                        // uiGender.Click();
                    }
                }
                //Set emailId
                Control email = controls.Find(c => c.Key == "txtEmail");
                if (email != null)
                {
                    IWebElement uiEmail = objGeneric.GetWebControl(email, FindBy.CSS_SELECTOR_ID);
                    uiEmail.Clear();
                    uiEmail.SendKeys(testData.EmailAddress);
                }
                //Set Phone Number
                Control mobileNo = controls.Find(c => c.Key == "mobilenumber");
                if (mobileNo != null)
                {
                    IWebElement uiMobNo = objGeneric.GetWebControl(mobileNo, FindBy.CSS_SELECTOR_ID);
                    string mobilePhoneNumber = getValidPhoneNumber(DBConfigKeys.MOBILENUMBERPREFIX);
                    if (uiMobNo != null)
                    {
                        uiMobNo.Clear();
                        uiMobNo.SendKeys(mobilePhoneNumber);
                    }
                }
                Control EvngNo = controls.Find(c => c.Key == "txteveningphone");
                if (EvngNo != null)
                {
                    IWebElement uiEvngNo = objGeneric.GetWebControl(EvngNo, FindBy.CSS_SELECTOR_ID);
                    string mobilePhoneNumber = getValidPhoneNumber(DBConfigKeys.EVENINGNUMBERPREFIX);
                    if (uiEvngNo != null)
                    {
                        uiEvngNo.Clear();
                        uiEvngNo.SendKeys(mobilePhoneNumber);
                    }
                }
                Control DayNo = controls.Find(c => c.Key == "txtdayphone");
                if (DayNo != null)
                {
                    IWebElement uiDayNo = objGeneric.GetWebControl(DayNo, FindBy.CSS_SELECTOR_ID);
                    string mobilePhoneNumber = getValidPhoneNumber(DBConfigKeys.DAYNUMBERPREFIX);
                    if (uiDayNo != null)
                    {
                        uiDayNo.Clear();
                        uiDayNo.SendKeys(mobilePhoneNumber);
                    }
                }
                //Find Address
                string grpEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (grpEnabled.Equals("0"))
                {
                    Control btnFindAdd = ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE);
                    objGeneric.ClickElement(btnFindAdd.ControlId, FindBy.ID, null);
                    Control ddlHouse = ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN);
                    objGeneric.GetWebControl(ddlHouse, FindBy.CSS_SELECTOR_ID).SendKeys(Keys.Down);

                    if (!this._AddressAPIEnabled)
                    {
                        Control txtHouseNo = ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSENUMBER);
                        objGeneric.GetWebControl(txtHouseNo, FindBy.CSS_SELECTOR_ID).Clear();
                        objGeneric.GetWebControl(txtHouseNo, FindBy.CSS_SELECTOR_ID).SendKeys(testData.MailingAddressLine1);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        private void SetValuesAllFields(ConfugurationTypeEnum configType, string controlCatagory, FieldType type)
        {
            try
            {
                IWebElement webControl = null;
                List<DBConfiguration> fields = new List<DBConfiguration>();
                List<DBConfiguration> mandatoryFields = new List<DBConfiguration>();
                List<Control> textCtrls = new List<Control>();
                List<Control> ddlCtrls = new List<Control>();
                List<Control> radioCtrls = new List<Control>();
                fields = AutomationHelper.GetDBConfigurations(configType, SanityConfiguration.DbConfigurationFile);
                if (type.Equals(FieldType.All))
                {
                    fields = fields.FindAll(f => !f.ConfigurationValue1.Equals("1"));
                }
                else if (type.Equals(FieldType.Mandatory))
                {
                    fields = fields.FindAll(f => !f.ConfigurationValue1.Equals("0") && f.IsDeleted.Equals("N"));
                }
                List<Control> controls = ObjAutomationHelper.GetControls(controlCatagory);
                if (configType == ConfugurationTypeEnum.ChinaHiddenFunctionality)
                {
                    controls = (from t in controls
                                where fields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                select t).ToList();
                }
                else
                {
                    List<DBConfiguration> availableFields = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.ChinaHiddenFunctionality, SanityConfiguration.DbConfigurationFile);
                    controls = (from t in controls
                                where fields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                & availableFields.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                                select t).ToList();
                }
                ddlCtrls = controls.FindAll(c => c.UIControlType == UIControlTypes.DropDown);
                foreach (Control c in ddlCtrls)
                {
                    webControl = objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID);
                    if (webControl != null)
                    {

                        webControl.SendKeys(Keys.Down);
                    }
                }
                textCtrls = controls.FindAll(c => c.UIControlType == UIControlTypes.TextBox);
                foreach (Control c in textCtrls)
                {
                    webControl = objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID);
                    c.Text = RandomString(10);
                    if (webControl != null)
                    {
                        webControl.Clear();
                        webControl.SendKeys(c.Text);
                    }
                }
                radioCtrls = controls.FindAll(c => c.UIControlType == UIControlTypes.RadioButton);
                foreach (Control c in radioCtrls)
                {
                    webControl = objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID);
                    if (webControl != null)
                    {
                        if (webControl.Selected.ToString() == "False")
                        {
                            Driver = ObjAutomationHelper.WebDriver;
                            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                            jse.ExecuteScript("arguments[0].click();", webControl);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void validatePostCodeSection()
        {
            try
            {
                CustomLogs.LogMessage("validatePostCodeSection started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_POSTCODEINSTRUCTION).Id)).Text;
                var expectedErrorMsg1 = AutomationHelper.GetResourceMessage(ValidationKey.PD_POSTCODESEC1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg2 = AutomationHelper.GetResourceMessage(ValidationKey.PD_POSTCODESEC2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                
                Regex htmls = new Regex("<[^>]*>");
                List<string> matches = htmls.Split(expectedErrorMsg2).ToList();
                List<string> lstMatches = new List<string>();
                matches.ForEach(m => lstMatches.Add(m));
                expectedErrorMsg2 = string.Join(" ", lstMatches);
                var expectedErrorMsg = expectedErrorMsg1 +" "+ expectedErrorMsg2;
                Assert.AreEqual(actualErrorMsg, expectedErrorMsg, " Error Messages doesn't match");
                CustomLogs.LogMessage("validatePostCodeSection completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void PersonalDetails_CheckDietaryNeeds(string key1, string keys, string status)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking started", TraceEventType.Start);
                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key1).Id)).Text;
                IWebElement DietayNeed = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                switch (status)
                {
                    case "OptIn":
                        if (!DietayNeed.Selected)
                        {
                            objGeneric.ClickElementJavaElement(keys, "");
                            CustomLogs.LogInformation("Check box for " + actualMessage + " is selected");
                        }
                        else
                            Assert.Fail("Check box already selected");
                        break;
                    case "OptOut":
                        if (DietayNeed.Selected)
                        {
                            objGeneric.ClickElementJavaElement(keys, "");
                            CustomLogs.LogInformation("Check box for " + actualMessage + " is unselected");
                        }
                        else
                            Assert.Fail("Check box is already not selected");
                        break;
                }

                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void PersonalDetails_DietaryNeedsFromUI()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking started", TraceEventType.Start);
                var actualMessage1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC).Id)).Text;
                var actualMessage2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDHALAL).Id)).Text;
                var actualMessage3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC).Id)).Text;
                var actualMessage4 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC).Id)).Text;
                var actualMessage5 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_LBLDIETARYNEEDDIABETIC).Id)).Text;

                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public bool PersonalDetails_DietaryNeedUICheck(List<string> valueFromService)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> ValueFromUI = new List<string>();

                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking started", TraceEventType.Start);
                ReadOnlyCollection<IWebElement> ss = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_CHKDIETARYNEED).XPath));

                for (int i = 0; i < ss.Count; i++)
                    ValueFromUI.Add(ss[i].Text.Trim());
                if (ValueFromUI.Count.Equals(valueFromService.Count))
                    a = new HashSet<String>(ValueFromUI).SetEquals(valueFromService);
                CustomLogs.LogMessage("My Personal Details Dietary needs check box checking completed", TraceEventType.Stop);
                return a;
            }
            catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return a;
                //  Driver.Quit();
            }
        }

        /// <summary>
        /// Method to validate the Household 1 age on PD page
        /// </summary>
        public void HouseholdAgeValue_You()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            CustomerServiceAdaptor Member1Age = new CustomerServiceAdaptor();
            var member1Age = Member1Age.GetFamilyMember1Age(Login.CustomerID.ToString(), CountrySetting.culture);
            if (member1Age == "AgeNotSelected")
            {
                member1Age = string.Empty;
            }
            var expectedYouValue = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YEAR).Id)).GetAttribute("value");
            Debug.WriteLine(string.Format("{0} | Household 1 age Actual {1} Expected {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, expectedYouValue, member1Age));
            Assert.AreEqual(member1Age, expectedYouValue, "Age selected in dob and age reflected in household is not same");
        }

        public void HouseholdAges()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            CustomerServiceAdaptor familyDetails = new CustomerServiceAdaptor();
            List<string> expected_famDetail = familyDetails.GetFamilyDetails(Login.CustomerID.ToString(), CountrySetting.culture);
            List<string> actual_famdetails = PersonAgeFromUI();
            Assert.AreEqual(string.Join(",", actual_famdetails), string.Join(",", expected_famDetail), "Family member's ages are not displayed correctly");
        }
        public List<string> PersonAgeFromUI()
        {
            Driver = ObjAutomationHelper.WebDriver;
            List<string> HHDPAge = new List<string>();
            ReadOnlyCollection<IWebElement> lstAgeDropDown = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_DDLPERSON2AGE).Id)));
            foreach (IWebElement ele in lstAgeDropDown)
            {
                HHDPAge.Add(ele.GetAttribute("value"));

            }
            string year = AutomationHelper.GetResourceMessage(ValidationKey.PERSONALDETAILS_HOUSEHOLDTEXTYEAR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            HHDPAge.RemoveAll(a => a.Equals(year));
            return HHDPAge;
        }
        public void EnterDuplicateData()
        {
            string groupCountryEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;

            try
            {
                 if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME1))
                        {
                            objGeneric.EnterDataInField(ControlKeys.JOIN_FIRSTNAME, testData.DuplicateName1);
                        }
                 if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME2))
                 {
                     objGeneric.EnterDataInField(ControlKeys.JOIN_MIDDLENAME, testData.DuplicateName2);
                 }
                 if (objGeneric.IscontrolVisible(DBConfigKeys.HIDENAME3))
                 {
                     objGeneric.EnterDataInField(ControlKeys.JOIN_SURNAME, testData.DuplicateName3);
                 }
                 if (!groupCountryEnabled.Equals("0"))
                 {
                     objGeneric.EnterDataInField(ControlKeys.PERSONALDETAILS_ADDRESSLINE1, testData.DuplicateMailingAddressLine1);
                 }
                 if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEEMAIL))
                 {
                     objGeneric.EnterDataInField(ControlKeys.JOIN_EMAIL, testData.DuplicateEmailAddress);
                 }
                if (objGeneric.IscontrolVisible(DBConfigKeys.HIDEPOSTCODE))
                 {
                     objGeneric.EnterDataInField(ControlKeys.JOIN_TXTPOSTCODE, testData.DuplicatePostCode);
                 }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public AddressBook contactservice(string topicid, string OAuthtokenID)
        {
            RestProxies _restClientManager = new RestProxies();
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.CONTACT_API_URL, SanityConfiguration.DbConfigurationFile);
            string ContactApiUrl = config.ConfigurationValue1;

           // string ContactApiUrl="http://172.28.152.12/mcaapi/v1/AddressBook/PreferredPhysicalAddresses";
          //  topicid = "urn:contact:topic:edad5719-d33f-5fc6-9bc4-4748800f2045";

            DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.TopicId, SanityConfiguration.DbConfigurationFile);
             topicid = config1.ConfigurationValue1;
          //   topicid = "trn:tesco:cid:190d5cba-3581-4270-9b44-676d6efbebbb:3e94a2a1-bcc1-4ad8-8e9f-e076142b59db";
            OAuthtokenID = "Bearer" + " " + OAuthtokenID;
            AddressBook authRes = new AddressBook();
            ContactApiUrl = Helper.AppendUrlQueryString(ContactApiUrl, new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("topic",topicid) });
            authRes = _restClientManager.RestGet<AddressBook>(ContactApiUrl, OAuthtokenID);

              return authRes;
        }

        public bool ValidateMethod(string keys,string Contactaddress)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page 'Current Points Details' started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                var addressfield = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Text;

               // var addressfield = Driver.FindElement(By.Id("fld_address")).Text;
                
                if (!addressfield.Contains(Contactaddress))
                {
                    Assert.Fail(Contactaddress + " " + "is not  same as " + " " + addressfield);
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                return true;
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }
           
        }

        public void verifyTitle(string key)
        {
            Driver = ObjAutomationHelper.WebDriver;
            List<string> ValueFromResx = new List<string>();
            List<string> ValueFromUI = new List<string>();
            var result = String.Empty;
            IWebElement elem = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
            ReadOnlyCollection<IWebElement> option = (elem.FindElements(By.TagName("option")));
            for (int i = 0; i < option.Count; i++)
            {
                ValueFromUI.Add(option[i].Text.Trim());
            }
            var ValueFromResource = AutomationHelper.GetResourceMessage(LabelKey.DDL_TITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            ValueFromResx=ValueFromResource.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            CollectionAssert.AreEqual(ValueFromResx,ValueFromUI);
        }

    }
        #endregion
}

