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

namespace Tesco.Framework.UITesting.Test.Common
{
    class PersonalDetails : Base
    {

        IAlert alert = null;
        CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
        public static Int64 CustomerID = 0;
        string PostcodeDB = string.Empty;
        Random rnd = null;
        bool a;
        TestData_PersonalDetails testData = null;
        Generic objGeneric = null;

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
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ERRORPOSTCODE).Id)).Text;
                var expectedErrorMsg1 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg2 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg3 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg4 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE4, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg5 = AutomationHelper.GetResourceMessage(ValidationKey.PD_ERRORPOSTCODE5, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
                var expectedErrorMsg = expectedErrorMsg1 + " " + "\"" + enteredPostCode + "\"" + "." + "\r\n" + expectedErrorMsg2 + "\r\n" + expectedErrorMsg3 + "\r\n" + expectedErrorMsg4 + "\r\n" + expectedErrorMsg5;
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
        public void VerifySurNameText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = "Do you need a replacement card with your new name?";
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_SURNAMEUPDATETEXT).Id)).Text;
            var actText = actualText.Replace(" Click here", "");
            Assert.AreEqual(expectedText, actText);
        }
        public void fillSurnameField(Enums.FieldType type)
        {
            try
            {
                DBConfiguration config = null;
                CustomLogs.LogMessage("Filling surname Fields started", TraceEventType.Start);
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    var value = properties[i].Name;
                    config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        if (fields.Keys.Contains(properties[i].Name))
                        {
                            if (properties[i].Name.Contains("Name3"))
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                            }
                        }
                    }
                }
                CustomLogs.LogMessage("Filling Surname Fields completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void fillMandatoryFields(Enums.FieldType type)
        {
            try
            {
                DBConfiguration config = null;
                CustomLogs.LogMessage("Filling Mandatory Fields started", TraceEventType.Start);
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    var value = properties[i].Name;
                    if (properties[i].Name == "Date" || properties[i].Name == "Month" || properties[i].Name == "Year")
                        value = "DateOfBirth";
                    else if (properties[i].Name == "AddressDropDown" || properties[i].Name == "PostCodeBtn")
                        value = "MailingAddressPostCode";
                    //if (value.Contains("EmailAddress"))
                    //    config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.JoinEmailMandatory, value, SanityConfiguration.DbConfigurationFile);
                    //else
                    config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        if (fields.Keys.Contains(properties[i].Name))
                        {
                            if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name.Contains("Date") || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                            {
                                if (properties[i].Name.Contains("TitleEnglish"))
                                {
                                    IWebElement ddl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TITLE).Id));
                                    OpenQA.Selenium.Support.UI.SelectElement clickthis = new OpenQA.Selenium.Support.UI.SelectElement(ddl);
                                    clickthis.SelectByText("Mrs");
                                }
                                else
                                {
                                    fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                                }
                            }
                            else if (properties[i].Name == "MobilePhoneNumber" || properties[i].Name == "EveningPhoneNumber" || properties[i].Name == "DayTimePhoneNumber")
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                            }
                            else if (properties[i].Name == "EmailAddress" || properties[i].Name.Contains("Name1") || properties[i].Name.Contains("Name3"))
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                            }
                            else if (properties[i].Name.Contains("Sex"))
                                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE).Id)).Click();
                            // fields[properties[i].Name].Click();
                            else if (properties[i].Name.Contains("PostCodeBtn"))
                            {
                                fields[properties[i].Name].Click();
                                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
                                fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)));
                            }
                            else if (properties[i].Name == "MailingAddressPostCode" && CountrySetting.country == "UK")
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                                fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)));

                            }
                            else
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            }
                        }
                    }
                }
                CustomLogs.LogMessage("Filling Mandatory Fields completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void fillAllFieldsMethodOne(Enums.FieldType type)
        {
            try
            {
                rnd = new Random();
                //testData = new TestData_PersonalDetails();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();

                for (int i = 0; i < properties.Count; i++)
                {
                    //String titlevalue = string.Empty; ;
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        if (properties[i].Name == "DuplicateEmailAddress")
                        {
                            CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                            string emailforjoin = customer.GetEmailIdForJoin(CountrySetting.culture);
                            properties[i].SetValue(testData, emailforjoin, null);
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name == "Date" || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                        {
                            if (properties[i].Name.Contains("TitleEnglish"))
                            {
                                IWebElement ddl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TITLE).Id));
                                OpenQA.Selenium.Support.UI.SelectElement clickthis = new OpenQA.Selenium.Support.UI.SelectElement(ddl);
                                clickthis.SelectByText("Mrs");
                            }
                            else
                                fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);

                        }
                        else if (properties[i].Name == "MobilePhoneNumber" || properties[i].Name == "EveningPhoneNumber" || properties[i].Name == "DayTimePhoneNumber")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                        }
                        else if (properties[i].Name == "EmailAddress" || properties[i].Name == "Name1" || properties[i].Name == "Name3")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("Sex"))
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE).Id)).Click();

                        else if (properties[i].Name.Contains("PostCodeBtn"))
                        {
                            fields[properties[i].Name].Click();
                            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                            fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)));
                        }
                        else if (properties[i].Name == "MailingAddressPostCode" && CountrySetting.country == "UK")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)));

                        }
                        else
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void fillAllFieldsMethodTwo(Enums.FieldType type)
        {
            try
            {
                rnd = new Random();
                //testData = new TestData_PersonalDetails();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();

                for (int i = 0; i < properties.Count; i++)
                {
                    //String titlevalue = string.Empty; ;
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        if (properties[i].Name == "DuplicateEmailAddress")
                        {
                            CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                            string emailforjoin = customer.GetEmailIdForJoin(CountrySetting.culture);
                            properties[i].SetValue(testData, emailforjoin, null);
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name == "Date" || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                        {
                            if (properties[i].Name.Contains("TitleEnglish"))
                            {
                                IWebElement ddl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TITLE).Id));
                                OpenQA.Selenium.Support.UI.SelectElement clickthis = new OpenQA.Selenium.Support.UI.SelectElement(ddl);
                                clickthis.SelectByText("Mr");
                            }
                            else
                                fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);

                        }
                        else if (properties[i].Name == "MobilePhoneNumber" || properties[i].Name == "EveningPhoneNumber" || properties[i].Name == "DayTimePhoneNumber")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                        }
                        else if (properties[i].Name == "EmailAddress" || properties[i].Name == "Name1" || properties[i].Name == "Name3" || properties[i].Name == "HouseNoOrName")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("Sex"))
                            // Driver.FindElement(By.Id("ctl00_ctl00_PageContainer_MyAccountContainer_radioMale")).Click();
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOMALE).Id)).Click();
                        else if (properties[i].Name.Contains("PostCodeBtn"))
                        {
                            fields[properties[i].Name].Click();
                            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                            fields.Add("HouseNoOrName", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSENUMBER).Id)));
                        }
                        else if (properties[i].Name == "MailingAddressPostCode" && CountrySetting.country == "UK")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)));

                        }
                        else
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public Dictionary<string, IWebElement> returnAllFieldsOnUI(Enums.FieldType type)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, IWebElement> PersonalDetails = new Dictionary<string, IWebElement>();

            switch (type)
            {
                case Enums.FieldType.Valid:
                    PersonalDetails.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PHONENUMBER).Id)));
                    break;
                case Enums.FieldType.Invalid:
                    PersonalDetails.Add("InvalidEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    PersonalDetails.Add("InvalidMailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidEveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidMobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidDayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("InvalidName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    break;
                case Enums.FieldType.ProfaneName1:
                    PersonalDetails.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("ProfaneName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateEmail:
                    PersonalDetails.Add("DuplicateEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateNameANDAddress:
                    PersonalDetails.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DuplicateName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.ProfaneName2:
                    PersonalDetails.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("ProfaneName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
                case Enums.FieldType.ProfaneName3:
                    PersonalDetails.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    PersonalDetails.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("ProfaneName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        PersonalDetails.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
            }
            PersonalDetails.Add("Sex", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOMALE).Id)));
            PersonalDetails.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DAY).Id)));
            PersonalDetails.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MONTH).Id)));
            PersonalDetails.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YEAR).Id)));
            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                PersonalDetails.Add("TitleEnglish", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TITLE).Id)));


            switch (CountrySetting.country)
            {
                case "CZ":
                case "PL":
                case "SK":
                    if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine1))
                    {
                        PersonalDetails.Add("ProfaneMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine2))
                    {
                        PersonalDetails.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("ProfaneMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine4))
                    {
                        PersonalDetails.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("ProfaneMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine5))
                    {
                        PersonalDetails.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("ProfaneMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.Invalid))
                    {
                        PersonalDetails.Add("InvalidMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("InvalidMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("InvalidMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("InvalidMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else
                    {
                        PersonalDetails.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        PersonalDetails.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        PersonalDetails.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        PersonalDetails.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }

                    break;
            }
            return PersonalDetails;
        }
        public void Checkamend()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_CheckAmendText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_CheckText).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        //public void Checkamend(string Clubcard)
        //{
        //    Driver = ObjAutomationHelper.WebDriver;
        //    var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_CheckAmendText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
        //    var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_CheckText).XPath)).Text;
        //    Assert.AreEqual(expectedText, actualText);
        //}
        public void YCDetailsTXT()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_YCDETAILSText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YCDETAILSTXT).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void REQUIREDWARNINGText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.PD_REQUIREDWARNINGText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_REQUIREDWARNINGTXT).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);

        }
        public void PageDecriptionText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedTextOne = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCONEText, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualTextOne = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTONE).XPath)).Text;

            var expectedTextTwoStart = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextstart, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextTwoLink = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextlink, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextTwoEnd = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTWOTextend, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualTextTwo = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTTWO).XPath)).Text;
            var expectedTexttwo = string.Concat(expectedTextTwoStart, " ", expectedTextTwoLink, " ", expectedTextTwoEnd);

            var expectedTextThreeStart = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextstart, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextThreeLink = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextlink, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var expectedTextThreeEnd = AutomationHelper.GetResourceMessage(ValidationKey.PD_PAGEDESCTHREETextend, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualTextThree = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PAGEDESCTXTTHREE).XPath)).Text;

            var expectedTextthree = string.Concat(expectedTextThreeStart, " ", expectedTextThreeLink, " ", expectedTextThreeEnd);


            var expectedText = string.Concat(expectedTextOne, "", expectedTexttwo, "", expectedTextthree);
            var actualTextA = string.Concat(actualTextOne, "", actualTextTwo, "", actualTextThree);
            var actualText = actualTextA.Replace("\r\n", " ");
            Assert.AreEqual(expectedText, actualText);
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
        public void PersonalDetails_CheckDietaryNeeds(string key1 ,string keys ,string status)
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
                            DietayNeed.Click();
                            CustomLogs.LogInformation("Check box for " + actualMessage + " is selected");
                        }
                        else
                            Assert.Fail("Check box already selected");
                        break;
                    case "OptOut":
                        if (DietayNeed.Selected)
                        {
                            DietayNeed.Click();
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
        public void YourHouseholdAge_Titles()
        {
            Driver = ObjAutomationHelper.WebDriver;
            string error = string.Empty;
            var expectedText_You = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEYOU, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_You = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgeTextYou).Id)).Text;

            error += expectedText_You.Equals(actualText_You) ? string.Empty : "The text You is not matching, ";
            var expectedText_Person2 = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEPERSON2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_Person2 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgePerson2).XPath)).Text;

            error += expectedText_Person2.Equals(actualText_Person2) ? string.Empty : "The text You is not matching, ";
            var expectedText_Person3 = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEPERSON3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_Person3 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgePerson3).XPath)).Text;

            error += expectedText_Person3.Equals(actualText_Person3) ? string.Empty : "The text You is not matching, ";
            var expectedText_Person4 = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEPERSON4, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_Person4 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgePerson4).XPath)).Text;

            error += expectedText_Person4.Equals(actualText_Person4) ? string.Empty : "The text You is not matching, ";
            var expectedText_Person5 = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEPERSON5, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_Person5 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgePerson5).XPath)).Text;

            error += expectedText_Person5.Equals(actualText_Person5) ? string.Empty : "The text You is not matching, ";
            var expectedText_Person6 = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDAGEPERSON6, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actualText_Person6 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HouseholdAgePerson6).XPath)).Text;

            error += expectedText_Person6.Equals(actualText_Person6) ? string.Empty : "The text You is not matching, ";
            Assert.AreEqual(error, string.Empty, error);
        }
        public void HouseholdAgeValue_You()
        {
            Driver = ObjAutomationHelper.WebDriver;
           
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            CustomerServiceAdaptor Member1Age = new CustomerServiceAdaptor();
            var member1Age = Member1Age.GetFamilyMember1Age(Login.CustomerID.ToString(), CountrySetting.culture);
            if (member1Age == "AgeNotSelected")
            {
                member1Age = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDMEMBER1YEAR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            }
            var expectedYouValue = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YEAR).Id)).GetAttribute("value");
            Assert.AreEqual(member1Age, expectedYouValue, "Age selected in dob and age reflected in household is not same");
        }
        public void HouseholdAgeDropdownValues()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            List<string> Personkeys = new List<string> { ControlKeys.PERSONALDETAILS_DDYearsPerson4, ControlKeys.PERSONALDETAILS_DDYearsPerson5, ControlKeys.PERSONALDETAILS_DDYearsPerson6, ControlKeys.PERSONALDETAILS_DDYearsPerson2, ControlKeys.PERSONALDETAILS_DDYearsPerson3 };
            foreach (string person in Personkeys)
            {

                var ddlSelectYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(person).Id));
                var mySelect = new SelectElement(ddlSelectYear);
                //to count no of years in the list
                int selectOptions = mySelect.Options.Count;
                CustomLogs.LogInformation("Total selected option in the drop down list is " + selectOptions);
                //To select the first index element in the drop down
                mySelect.SelectByIndex(1);
                var startYearInDDl = (mySelect.SelectedOption).Text;
                CustomLogs.LogInformation("Start year in the drop down list " + startYearInDDl);
                mySelect.SelectByIndex(selectOptions - 1);
                var endYearInDDl = (mySelect.SelectedOption).Text;
                CustomLogs.LogInformation("End year in the drop down list " + endYearInDDl);
                int currentYear = DateTime.Now.Year;
                int expectedEndYear = currentYear - 1;
                int expectedStartYear = currentYear - 99;
                Assert.AreEqual(expectedStartYear.ToString(), startYearInDDl.ToString(), "Expected start Year " + expectedStartYear + "not equal to start year in DDL" + startYearInDDl);
                Assert.AreEqual(expectedEndYear.ToString(), endYearInDDl.ToString(), "Expected end Year " + expectedEndYear + "not equal to end year in DDL" + endYearInDDl);

            }
            //string actualvalue = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson4).Id)).GetAttribute("Value");
            //int currentYear = DateTime.Now.Year;
            //   int expectedEndYear = currentYear - 1;
            //    int expectedStartYear = currentYear - 99;
            //    for (int i = 0; i <= expectedEndYear; i++) 
            //    {
                    
            //        string expectedyear = (currentYear - i).ToString();
            //    }
           

            CustomLogs.LogMessage("VerifyYearInDOB completed", TraceEventType.Stop);
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
            HHDPAge.Add(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson2).Id)).GetAttribute("value"));
            HHDPAge.Add(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson3).Id)).GetAttribute("value"));
            HHDPAge.Add(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson4).Id)).GetAttribute("value"));
            HHDPAge.Add(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson5).Id)).GetAttribute("value"));
            HHDPAge.Add(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DDYearsPerson6).Id)).GetAttribute("value"));
            string year = AutomationHelper.GetResourceMessage(ValidationKey.PD_HOUSEHOLDTEXTYEAR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            HHDPAge.RemoveAll(a => a.Equals(year));
            return HHDPAge;
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
        public void selectOtionFromDropDown(string key, Enums.DropDownValue field)
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                var mySelectElm = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                var mySelect = new SelectElement(mySelectElm);
                switch (field)
                {
                    case Enums.DropDownValue.AnyOption:
                        mySelect.SelectByIndex(3);
                        break;
                    case Enums.DropDownValue.SelectOption:
                        mySelect.SelectByIndex(0);
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

        }
        public void ConfirmSelection(string key, Services.Enums.CustPersonalDetail personaldetail)
        {
            var mySelectElm = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
            var mySelect = new SelectElement(mySelectElm);
            var abc = (mySelect.SelectedOption).Text;
        }
        public void fillAllFieldsGeneric(Enums.FieldType type)
        {
            try
            {
                rnd = new Random();
                var prefixFromDBConfig = String.Empty;
                Dictionary<string, IWebElement> fields = returnAllFieldsGeneric(type);
                Type myType = testData.GetType();
                List<string> Dropdown = new List<string>();
                Dropdown.Add(Enums.JoinElements.TitleEnglish.ToString());
                Dropdown.Add(Enums.JoinElements.Date.ToString());
                Dropdown.Add(Enums.JoinElements.Month.ToString());
                Dropdown.Add(Enums.JoinElements.Year.ToString());
                Dropdown.Add(Enums.JoinElements.Race.ToString());
                Dropdown.Add(Enums.JoinElements.Province.ToString());
                var prefix = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Prefix, DBConfigKeys.MOBILENUMBERPREFIX, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (prefix.Contains(","))
                    prefixFromDBConfig = prefix.Split(',')[0];
                else
                    prefixFromDBConfig = prefix;
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        if (Dropdown.Contains(properties[i].Name))
                            fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                        else if (properties[i].Name.Contains("PhoneNumber"))
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(prefixFromDBConfig + rnd.Next(0, 999999999).ToString());
                        }
                        else if (properties[i].Name == Enums.JoinElements.EmailAddress.ToString() || properties[i].Name == Enums.JoinElements.Name1.ToString() || properties[i].Name == Enums.JoinElements.Name3.ToString())
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name == Enums.JoinElements.Sex.ToString())
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE).Id)).Click();
                        else if (properties[i].Name == Enums.JoinElements.MailingAddressPostCode.ToString() && CountrySetting.country == "UK")
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)).Click();
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(Keys.Down);
                        }
                        else
                        {
                            fields[properties[i].Name].Clear();
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public Dictionary<string, IWebElement> returnAllFieldsGeneric(Enums.FieldType type)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, IWebElement> PersonalDetails = new Dictionary<string, IWebElement>();

            switch (type)
            {
                case Enums.FieldType.Valid:
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEMAIL))
                        PersonalDetails.Add(Enums.JoinElements.EmailAddress.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EMAIL).Id)));
                    PersonalDetails.Add(Enums.JoinElements.MailingAddressPostCode.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)));
                    if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS))
                    {
                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE1))
                            PersonalDetails.Add(Enums.JoinElements.MailingAddressLine1.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE1).Id)));
                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE2))
                            PersonalDetails.Add(Enums.JoinElements.MailingAddressLine2.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE2).Id)));
                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE3))
                            PersonalDetails.Add(Enums.JoinElements.MailingAddressLine3.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE3).Id)));
                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE4))
                            PersonalDetails.Add(Enums.JoinElements.MailingAddressLine4.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE4).Id)));
                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE5))
                        {
                            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.AppSettings, DBConfigKeys.PROVINCEENABLED))
                                PersonalDetails.Add(Enums.JoinElements.MailingAddressLine5.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE5).Id)));
                            else
                                PersonalDetails.Add(Enums.JoinElements.Province.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PROVINCE).Id)));
                        }

                        if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEADDRESSLINE6))
                            PersonalDetails.Add(Enums.JoinElements.MailingAddressLine6.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_ADDRESSLINE6).Id)));
                    }
                    else
                        PersonalDetails.Add(Enums.JoinElements.Findaddress.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2))
                        PersonalDetails.Add(Enums.JoinElements.Name2.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MIDDLENAME).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1))
                        PersonalDetails.Add(Enums.JoinElements.Name1.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_FIRSTNAME).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3))
                        PersonalDetails.Add(Enums.JoinElements.Name3.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_SURNAME).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER))
                        PersonalDetails.Add(Enums.JoinElements.EveningPhoneNumber.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_EVENINGNUMBER).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER))
                        PersonalDetails.Add(Enums.JoinElements.MobilePhoneNumber.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MOBILENUMBER).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER))
                        PersonalDetails.Add(Enums.JoinElements.DayTimePhoneNumber.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PHONENUMBER).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDERACE))
                        PersonalDetails.Add(Enums.JoinElements.Race.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_RACE).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPREFERREDLANGUAGE))
                        PersonalDetails.Add(Enums.JoinElements.PreferredLanguage.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_RACE).Id)));
                    if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPRIMARYID))
                        PersonalDetails.Add(Enums.JoinElements.PreferredLanguage.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_PRIMARYID).Id)));

                    break;
            }
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDOB))
            {
                PersonalDetails.Add(Enums.JoinElements.Date.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_DAY).Id)));
                PersonalDetails.Add(Enums.JoinElements.Month.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_MONTH).Id)));
                PersonalDetails.Add(Enums.JoinElements.Year.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_YEAR).Id)));
            }
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEGENDER))
                PersonalDetails.Add(Enums.JoinElements.Sex.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOMALE).Id)));
            if (objGeneric.returnConfigValue1Negative(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE))
                PersonalDetails.Add(Enums.JoinElements.TitleEnglish.ToString(), Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TITLE).Id)));
            return PersonalDetails;
        }
        public void fillMandatoryFieldsGeneric(Enums.FieldType type)
        {
            try
            {
                var prefixFromDBConfig = String.Empty;
                CustomLogs.LogMessage("Filling Mandatory Fields started", TraceEventType.Start);
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsGeneric(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                List<string> Dropdown = new List<string>();
                Dropdown.Add(Enums.JoinElements.TitleEnglish.ToString());
                Dropdown.Add(Enums.JoinElements.Date.ToString());
                Dropdown.Add(Enums.JoinElements.Month.ToString());
                Dropdown.Add(Enums.JoinElements.Year.ToString());
                Dropdown.Add(Enums.JoinElements.Race.ToString());
                Dropdown.Add(Enums.JoinElements.Province.ToString());
                var prefix = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Prefix, DBConfigKeys.MOBILENUMBERPREFIX, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (prefix.Contains(","))
                    prefixFromDBConfig = prefix.Split(',')[0];
                else
                    prefixFromDBConfig = prefix;
                for (int i = 0; i < properties.Count; i++)
                {
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        var value = properties[i].Name;
                        if (properties[i].Name == Enums.JoinElements.Date.ToString() || properties[i].Name == Enums.JoinElements.Month.ToString() || properties[i].Name == Enums.JoinElements.Year.ToString())
                            value = DBConfigKeys.DOB;
                        else if (properties[i].Name == Enums.JoinElements.Findaddress.ToString())
                            value = "MailingAddressPostCode";
                        else if (properties[i].Name == Enums.JoinElements.Province.ToString())
                            value = "MailingAddressLine5";
                        else if (properties[i].Name == Enums.JoinElements.PreferredLanguage.ToString())
                            value = "Language";
                        if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Mandatory_fields, value))
                        {
                            if (Dropdown.Contains(properties[i].Name))
                                fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                            else if (properties[i].Name.Contains("PhoneNumber"))
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(prefixFromDBConfig + rnd.Next(0, 999999999).ToString());
                            }
                            else if (properties[i].Name == Enums.JoinElements.EmailAddress.ToString() || properties[i].Name == Enums.JoinElements.Name1.ToString() || properties[i].Name == Enums.JoinElements.Name3.ToString())
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                            }
                            else if (properties[i].Name == Enums.JoinElements.Sex.ToString())
                                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNRADIOFEMALE).Id)).Click();
                            else if (properties[i].Name == Enums.JoinElements.MailingAddressPostCode.ToString() && CountrySetting.country == "UK")
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_BTNPOSTCODE).Id)).Click();
                                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_HOUSEDROPDOWN).Id)).SendKeys(Keys.Down);
                            }
                            else
                            {
                                fields[properties[i].Name].Clear();
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            }

                        }
                    }
                }
                CustomLogs.LogMessage("Filling Mandatory Fields completed", TraceEventType.Stop);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
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
        public void EnterInvalidLength(string configKey, string controlKey, Enums.FieldType type)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var prefixFromDBConfig = String.Empty;
                var rng = new Random(Environment.TickCount);
                int lengthFromConfig = objGeneric.returnConfigLength(ConfugurationTypeEnum.Length_of_the_input_fields, configKey, type);
                var prefix = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Prefix, configKey, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (prefix.Contains(","))
                    prefixFromDBConfig = prefix.Split(',')[0];
                else
                    prefixFromDBConfig = prefix;
                int prefixlength = prefixFromDBConfig.Length;
                int randomNumberLength = lengthFromConfig - prefixlength;
                string phoneNumber = string.Concat(Enumerable.Range(0, randomNumberLength).Select((index) => rng.Next(10).ToString()));
                var a = prefixFromDBConfig + phoneNumber;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).Clear();
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys(a);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
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
        public Dictionary<String, IWebElement> returnMandatoryFields(Enums.FieldType type)
        {
            Dictionary<string, IWebElement> fields = returnAllFieldsGeneric(type);
            Dictionary<string, IWebElement> mandatoryFields = new Dictionary<string, IWebElement>();
            Type myType = testData.GetType();
            List<PropertyInfo> properties = myType.GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                if (fields.Keys.Contains(properties[i].Name))
                {
                    var value = properties[i].Name;
                    if (properties[i].Name == Enums.JoinElements.Date.ToString() || properties[i].Name == Enums.JoinElements.Month.ToString() || properties[i].Name == Enums.JoinElements.Year.ToString())
                        value = DBConfigKeys.DOB;
                    if (objGeneric.returnConfigValue1Positive(ConfugurationTypeEnum.Mandatory_fields, value))
                    {
                        mandatoryFields.Add(properties[i].Name, fields[properties[i].Name]);

                    }
                }
            }
            //for (int i = 0; i < mandatoryFields.Count; i++)
            //{
            //    if (mandatoryFields.Keys.Contains(properties[i].Name))
            //    mandatoryFields[properties[i].Name].Clear();
            //}
            return mandatoryFields;
        }
    }

        #endregion
}


