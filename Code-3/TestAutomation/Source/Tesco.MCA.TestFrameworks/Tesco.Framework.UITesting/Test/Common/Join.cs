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
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.IO;
using Tesco.Framework.UITesting.Constants;
using System.Reflection;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    class Join : Base
    {

        Generic objGeneric = null;
        TestData_JoinDetails testData = null;
        Random rnd=null;
        #region Constructor
        public Join(AutomationHelper objhelper)
        {
           
            this.ObjAutomationHelper = objhelper;
        }
        public Join(AutomationHelper objHelper, AppConfiguration configuration, TestData_JoinDetails testData)
        {
            this.testData = testData;
            ObjAutomationHelper = objHelper;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }
        #endregion

        #region Methods

        public void VerifyJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Join Page started ", TraceEventType.Start);
                //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_IMAGE_TEXT).Id));
                if (Generic.IsElementPresent((By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNCONFIRM).Id)), Driver))
                {
                    CustomLogs.LogInformation("Join Page verified for country "+CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Page not verified for country "+CountrySetting.country);
                }
                CustomLogs.LogMessage("verifying Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void FillMandatoryDetails(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Mandatory fields are started filling. ", TraceEventType.Start);
                Driver.FindElement(By.Id("ctl00_PageJoinContainer_ddlTitle")).SendKeys(Keys.Down + Keys.Tab);                
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)).SendKeys(RandomString(7));
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));                             
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)).SendKeys(RandomString(7));
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));               
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)).Click();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));               
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)).SendKeys("RH19 3se");
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)).SendKeys(Keys.Tab);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)).Click();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSENUMBER).Id)).SendKeys(RandomString(7));
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)).SendKeys(RandomString(6)+"@gmail.com");  
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("Mandatory fields are filled completely. ", TraceEventType.Stop);
        }

        public Dictionary<string, IWebElement> returnAllFieldsOnUI(Enums.FieldType type)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, IWebElement> join = new Dictionary<string, IWebElement>();
           
            switch(type)
            {
                case Enums.FieldType.Valid:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                     break;
                case Enums.FieldType.Invalid:
                    join.Add("InvalidEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("InvalidMailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidEveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidMobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidDayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    break;
                case Enums.FieldType.ProfaneName1:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                      join.Add("ProfaneName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));  
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateEmail:
                    join.Add("DuplicateEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateNameANDAddress:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DuplicateName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.ProfaneName2:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("ProfaneName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
                case Enums.FieldType.ProfaneName3:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("ProfaneName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
            }
            join.Add("Sex", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)));
            join.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_DAY).Id)));
            join.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MONTH).Id)));
            join.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id)));
            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                join.Add("TitleEnglish", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TITLE).Id)));
            
           
            switch (CountrySetting.country)
            {
                case "CZ":
                case "PL":
                case "SK":
                    if(type.Equals(Enums.FieldType.ProfaneMailingAddressLine1))
                    {
                    join.Add("ProfaneMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                    join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                    join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                    join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine2))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("ProfaneMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine4))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("ProfaneMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine5))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("ProfaneMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.Invalid))
                    {
                        join.Add("InvalidMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("InvalidMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("InvalidMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("InvalidMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }

                    break;
            }
            return join;
        }
        //public Dictionary<string,IWebElement> commonFields()
        //{
        //    Dictionary<string, IWebElement> join = new Dictionary<string, IWebElement>();
        //   // Dictionary<string, IWebElement> join = returnAllFieldsOnUI(type);
        //    try
        //    {
        //        CustomLogs.LogMessage("commonFields started ", TraceEventType.Start);
        //        Driver = ObjAutomationHelper.WebDriver;
        //            join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
        //            join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
        //            join.Add("Sex", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)));
        //            join.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_Date).Id)));
        //            join.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MONTH).Id)));
        //            join.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id))); 
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("TitleEnglish", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TITLE).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDateTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("DateTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
           
        //        CustomLogs.LogMessage("coomonFields Completed ", TraceEventType.Stop);
        //        return join;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        ScreenShotDetails.TakeScreenShot(Driver, ex);
        //        CustomLogs.LogException(ex);
        //        Driver.Quit();
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //        return join;

        //    }
            
        //}
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
                    if (value.Contains("EmailAddress"))
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.JoinEmailMandatory, value, SanityConfiguration.DbConfigurationFile);
                    else
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        if (fields.Keys.Contains(properties[i].Name))
                        {
                            if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name.Contains("Date") || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                                fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                            else if (properties[i].Name == "MobilePhoneNumber" || properties[i].Name == "EveningPhoneNumber" || properties[i].Name == "DayTimePhoneNumber")
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                            else if (properties[i].Name=="EmailAddress" || properties[i].Name.Contains("Name1"))
                                fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                            else if (properties[i].Name.Contains("Sex"))
                                fields[properties[i].Name].Click();
                            else if (properties[i].Name.Contains("PostCodeBtn"))
                            {
                                fields[properties[i].Name].Click();
                                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                                fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSEDROPDOWN).Id)));
                            }
                            else if (properties[i].Name=="MailingAddressPostCode" && CountrySetting.country == "UK")
                            {
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                                fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)));

                            }
                            else
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
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
        public void enterPromotionalCode(string data)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPROMOCODE).Id)).SendKeys(data);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void fillAllFields(Enums.FieldType type)
        {
            try
            {
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();

                for (int i = 0; i < properties.Count; i++)
                {
               
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        if (properties[i].Name == "DuplicateEmailAddress")
                        {
                            CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                            string emailforjoin = customer.GetEmailIdForJoin(CountrySetting.culture);
                            properties[i].SetValue(testData, emailforjoin, null);
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name=="Date" || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                            fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                        else if (properties[i].Name=="MobilePhoneNumber"||properties[i].Name=="EveningPhoneNumber"||properties[i].Name=="DayTimePhoneNumber")
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                        else if (properties[i].Name=="EmailAddress" || properties[i].Name=="Name1")
                            fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                        else if (properties[i].Name.Contains("Sex"))
                            fields[properties[i].Name].Click();
                        else if (properties[i].Name.Contains("PostCodeBtn"))
                        {
                            fields[properties[i].Name].Click();
                            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                            fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSEDROPDOWN).Id)));
                        }
                        else if (properties[i].Name=="MailingAddressPostCode" && CountrySetting.country == "UK")
                        {
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)));

                        }
                        else
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
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
        public void verifyConfirmMessage()
        {
            try
            {
                CustomLogs.LogMessage("VerifyConfirmMessage Started ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res2 = AutomationHelper.GetResourceMessage(LabelKey.JOIN_CLICKTODOWNLOADPDF, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                var expectedLinkToDownloadPDF = res2.Value;
                var expectedthankYouMsg = (AutomationHelper.GetResourceMessage(LabelKey.JOIN_THANKYOUMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE))).Value;
                var actualthankYouMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_THANKYOUMSG).ClassName)).Text;
                var actualLinkToDownloadPDF = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PDFDOWNLOAD).Id)).Text;
                Assert.AreEqual(expectedthankYouMsg, actualthankYouMsg, expectedthankYouMsg + " not equal to " + actualthankYouMsg);
                Assert.AreEqual(expectedLinkToDownloadPDF, actualLinkToDownloadPDF, expectedLinkToDownloadPDF + " not equal to " + actualLinkToDownloadPDF);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("VerifyConfirmMessage Completed ", TraceEventType.Stop);
        }
        public void acceptLegalPolicy()
        {
            try
            {
                CustomLogs.LogMessage("acceptLegalPolicy started ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDELEGALPOLICY, SanityConfiguration.DbConfigurationFile);
                if (config.IsDeleted == "N")
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TERMSANDCONDITION).Id)).Click();
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PRIVACYPOLICY).Id)).Click();
                    CustomLogs.LogMessage("acceptLegalPolicy completed ", TraceEventType.Stop);
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
        public void verifyValidationCheck(Enums.FieldType type)
        {
            try
            {
                Dictionary<string, IWebElement> fields = null;
                CustomLogs.LogMessage("verifyValidationCheck started ", TraceEventType.Start);
               if(type.Equals(Enums.FieldType.All))
                 fields = returnAllFieldsOnUI(Enums.FieldType.Invalid);
               else if (type.Equals(Enums.FieldType.Mandatory))
                   fields = returnMandatoryFields(Enums.FieldType.Valid);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    if(fields.Keys.Contains(properties[i].Name))
                    {
                        string value=properties[i].Name;
                        if ((value.Contains(Enums.JoinElements.TitleEnglish.ToString()))&&type==Enums.FieldType.Mandatory)
                            objGeneric.VerifyTextonthePageByXpath(ValidationKey.ERRORTITLE, ControlKeys.JOIN_ERRORTITLE, "Join error for Title", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if ((value.Contains(Enums.JoinElements.Date.ToString()) || value.Contains(Enums.JoinElements.Month.ToString()) || value.Contains(Enums.JoinElements.Year.ToString()))&&type==Enums.FieldType.Mandatory)
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORDOB, ControlKeys.JOIN_ERRORDOB, "Join error for DOB", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if ((value.Contains(Enums.JoinElements.Sex.ToString())) && type == Enums.FieldType.Mandatory)
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORGENDER, ControlKeys.JOIN_ERRORGENDER, "Join error for gender", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName1.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME1, ControlKeys.JOIN_ERRORNAME1, "Join Error For Name1", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName2.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME2, ControlKeys.JOIN_ERRORNAME2, "Join Error For Name2", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName3.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME3, ControlKeys.JOIN_ERRORNAME3, "Join error for name3", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidEmailAddress.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERROREMAIL, ControlKeys.JOIN_ERROREMAIL, "Join error for email", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressPostCode.ToString().Contains(value))
                         {
                             string key = String.Empty;
                             if (CountrySetting.country.Equals("UK"))
                                 key = ValidationKey.ERRORPOSTCODE_UK;
                             else
                                 key = ValidationKey.ERRORPOSTCODE_GC;
                             objGeneric.verifyValidationMessage(key, ControlKeys.JOIN_ERRORPOSTCODE, "Join error for postcode", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                         }
                        else if (Enums.JoinElements.InvalidMailingAddressLine1.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE1, ControlKeys.JOIN_ERRORADDRESSLINE1, "Join error for address line 1", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine2.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE2, ControlKeys.JOIN_ERRORADDRESSLINE2, "Join error for address line 2", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine4.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE4, ControlKeys.JOIN_ERRORADDRESSLINE4, "Join error for address line 4", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine5.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE5, ControlKeys.JOIN_ERRORADDRESSLINE5, "Join error for address line 5", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidEveningPhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERROREVENINGNUMBER, ControlKeys.JOIN_ERROREVNGNUMBR, "Join error for evening number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidDayTimePhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORDAYTIMENUMBER, ControlKeys.JOIN_ERRORDAYNUMBR, "Join error for day time number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMobilePhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORMOBILENUMBER, ControlKeys.JOIN_ERRORMOBILENUMBR, "Join error for mobile number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    }
                }
                CustomLogs.LogMessage("verifyValidationCheck completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public Dictionary<String,IWebElement> returnMandatoryFields(Enums.FieldType type)
        {
            DBConfiguration config = null;
            rnd = new Random();
            Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
            Dictionary<string, IWebElement> mandatoryFields = new Dictionary<string,IWebElement>();
            Type myType = testData.GetType();
            List<PropertyInfo> properties = myType.GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                if (fields.Keys.Contains(properties[i].Name))
                {
                    var value = properties[i].Name;
                    if (properties[i].Name == Enums.JoinElements.Date.ToString() || properties[i].Name == Enums.JoinElements.Month.ToString() || properties[i].Name == Enums.JoinElements.Year.ToString())
                        value = DBConfigKeys.DOB;
                    if (value.Contains(Enums.JoinElements.EmailAddress.ToString()))
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.JoinEmailMandatory, value, SanityConfiguration.DbConfigurationFile);
                    else
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        mandatoryFields.Add(properties[i].Name, fields[properties[i].Name]);
                    }
                }
            }
            return mandatoryFields;
        }
        public void SelectYearInDOB()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("SelectYearInDOB started", TraceEventType.Start);
            var ddlSelectYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id));
            var mySelect = new SelectElement(ddlSelectYear);
            mySelect.SelectByIndex(2);
            var yearInDDl = (mySelect.SelectedOption).Text;
            var yearInTextBox=(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTYEAROPTIONALINFO).Id))).GetAttribute("Value");
            Assert.AreEqual(yearInDDl, yearInTextBox, "Year in selected DDL" + yearInDDl + "not equal to year in text Box" + yearInTextBox);
            CustomLogs.LogMessage("SelectYearInDOB completed", TraceEventType.Stop);

        }

        public void VerifyYearInDOB()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            var ddlSelectYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_DDLPERSON2AGE).Id));            
            var mySelect = new SelectElement(ddlSelectYear);
            //to count no of years in the list
            int selectOptions = mySelect.Options.Count;
            CustomLogs.LogInformation("Total selected option in the drop down list is " + selectOptions);
            //To select the first index element in the drop down
            mySelect.SelectByIndex(1);

            var startYearInDDl = (mySelect.SelectedOption).Text;
            CustomLogs.LogInformation("Start year in the drop down list " + startYearInDDl);
            mySelect.SelectByIndex(selectOptions-1);
            var endYearInDDl = (mySelect.SelectedOption).Text;
            CustomLogs.LogInformation("End year in the drop down list " + endYearInDDl);
            int currentYear = DateTime.Now.Year;
            int expectedEndYear = currentYear - 1;
            int expectedStartYear = currentYear - 99;
            Assert.AreEqual(expectedStartYear.ToString(), startYearInDDl.ToString(), "Expected start Year " + expectedStartYear + "not equal to start year in DDL" + startYearInDDl);
            Assert.AreEqual(expectedEndYear.ToString(), endYearInDDl.ToString(), "Expected end Year " + expectedEndYear + "not equal to end year in DDL" + endYearInDDl);
            CustomLogs.LogMessage("VerifyYearInDOB completed", TraceEventType.Stop);

        }
        public void verifyCheckBoxInDataProtection(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("verifyCheckBoxInDataProtection started", TraceEventType.Start);
                if (Generic.IsElementPresent((By.CssSelector(ObjAutomationHelper.GetControl(key).Id)), Driver))
                    CustomLogs.LogInformation("checkBox for" + key + " present");
                else
                    Assert.Fail("checkBox for" + key + " not present");
                CustomLogs.LogMessage("verifyCheckBoxInDataProtection completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        #endregion
    }
}
