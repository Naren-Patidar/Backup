using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    class Activation : Base
    {
        #region Public Members
        public static string firstName = String.Empty;
        public static string surName = String.Empty;
        public static string postCode = String.Empty;
        string returnvalue = String.Empty;
        string email2 = String.Empty;
        string Email = String.Empty;
        Random rnd = null;
        TestData_Activation testData = null;
        TestData_AccountDetails AccttestData = null;
        IAlert alert = null;
        Generic objGeneric = null;

        #endregion

        #region Constructor


        public Activation(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        public Activation(AutomationHelper objHelper, AppConfiguration configuration, TestData_AccountDetails testData)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            this.AccttestData = testData;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion

        #region Methods
        public bool isAlertPresent()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                alert = Driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

        public void verifyPageTitle()
        {

            Control header = ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_TITLE);
            IWebElement title = objGeneric.GetWebControl(header, FindBy.CSS_SELECTOR_ID);
            Resource res = AutomationHelper.GetResourceMessage(LabelKey.ACTIVATION_TITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
            string actual_title = title.Text;
            string expected_title = res.Value;
            Assert.AreEqual(expected_title, actual_title);
        }


        public void verifyConfirmationPageTitle()
        {
            Control header = ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONF_TITLE);
            IWebElement title = objGeneric.GetWebControl(header, FindBy.XPATH_SELECTOR);
            Resource res = AutomationHelper.GetResourceMessage(LabelKey.ACTIVATION_Confirm_TITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
            if (title == null)
            {
                Assert.Fail("PAGE TITLE WEB CONTROL Not Found.");
            }            
            string actual_title = title.Text;
            string expected_title = res.Value;
            Assert.AreEqual(expected_title, actual_title);
        }
        public void VerifyActivationPage(IWebDriver Driver)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_TEXT).Id));
                if (Generic.IsElementPresent((By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)), Driver))
                {
                    CustomLogs.LogInformation("Activation Page verified for Country " + CountrySetting.country);
                    Driver.Manage().Cookies.DeleteAllCookies();

                }
                else
                {
                    Assert.Fail("Activation Page not verified for country " + CountrySetting.country);
                    Driver.Manage().Cookies.DeleteAllCookies();
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Manage().Cookies.DeleteAllCookies();
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void Activation_CheckActivationErrorMessage(String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Activation Error Message Stating", TraceEventType.Start);
                IWebElement ele = null;
                Driver = ObjAutomationHelper.WebDriver;
                ele.WaitForTitle(Driver, pageName);
                Activation_CheckErrorMsgAfterConfirmation();
                CustomLogs.LogMessage("Activation Error Message Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Activation_CheckSuccessMessage(String pageName)
        {
            try
            {
                IWebElement ele = null;
                CustomLogs.LogMessage("Checking success message for Activation ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ele.WaitForTitle(Driver, pageName);
                Activation_CheckMsgAfterConfirmation(ValidationKey.SUCCESSFULMESSAGEFORACTIVATION);
                CustomLogs.LogMessage("Success Message for Activation Checked", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public bool Activation_CheckErrorMsgAfterConfirmation()
        {
            try
            {
                IAlert alert = null;
                CustomLogs.LogMessage("Checking for message to confirm the details again starting", TraceEventType.Stop);
                string expectedActivationErrorMessage1 = AutomationHelper.GetResourceMessage(ValidationKey.MESSAGEFORRACTIVATIONERROR1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
                string expectedActivationErrorMessage2 = AutomationHelper.GetResourceMessage(ValidationKey.MESSAGEFORRACTIVATIONERROR2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;

                string expectedActivationErrorMessage = expectedActivationErrorMessage1 + expectedActivationErrorMessage2;
                Driver = ObjAutomationHelper.WebDriver;
                Thread.Sleep(40);

                var surNameAmended = surName + "k";
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_FIRSTNAME).Id)).SendKeys(firstName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SURNAME).Id)).SendKeys(surNameAmended);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_POSTCODE).Id)).SendKeys(postCode);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();

                string actualActivationErrorMessage1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_ERRORMSG1).XPath)).Text;
                string actualActivationErrorMessage2 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_ERRORMSG2).XPath)).Text;

                string actualActivationErrorMessage = actualActivationErrorMessage1 + actualActivationErrorMessage2;

                if (expectedActivationErrorMessage == actualActivationErrorMessage)
                {
                    CustomLogs.LogInformation("Expected Success Message : " + expectedActivationErrorMessage + "is equal to actual success message" + actualActivationErrorMessage + ". Hence,Success Msg Verified");
                    CustomLogs.LogInformation("Success Msg Verified");
                    Driver.Navigate().GoToUrl(Driver.Url);
                    if (isAlertPresent())
                        alert.Accept();
                }
                else
                {
                    CustomLogs.LogInformation("Success Message NOT Verified. Scenario Failed");
                    Assert.Fail(expectedActivationErrorMessage + " is not equal to " + actualActivationErrorMessage + " ,So" + " Success msg not verified");
                }
                CustomLogs.LogMessage("Message to confirm the details again completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        public bool Activation_CheckMsgAfterConfirmation(string msgID)
        {
            try
            {
                IAlert alert = null;
                CustomLogs.LogMessage("Check Message after confirmaion started", TraceEventType.Start);
                string expectedSuccessMessage = AutomationHelper.GetResourceMessage(msgID, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
                Driver = ObjAutomationHelper.WebDriver;
                Thread.Sleep(40);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_FIRSTNAME).Id)).SendKeys(firstName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SURNAME).Id)).SendKeys(surName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_POSTCODE).Id)).SendKeys(postCode);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();

                string actualSuccessMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SUCCESSCONFIRMMSG).XPath)).Text;
                if (expectedSuccessMessage == actualSuccessMessage)
                {
                    CustomLogs.LogInformation("Expected Success Message : " + expectedSuccessMessage + "is equal to actual success message" + actualSuccessMessage + ". Hence,Success Msg Verified");
                    Driver.Navigate().GoToUrl(Driver.Url);
                    if (isAlertPresent())
                        alert.Accept();
                }
                else
                {
                    CustomLogs.LogInformation("Success Message NOT Verified. Scenario Failed");
                    Assert.Fail(expectedSuccessMessage + " is not equal to " + actualSuccessMessage + " ,So" + " Success msg not verified");
                }
                CustomLogs.LogMessage("Check message after confirmation Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                // CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            return true;

        }

        public bool Activation_CheckMsgAfterConfirmation()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Thread.Sleep(40);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_FIRSTNAME).Id)).SendKeys(firstName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SURNAME).Id)).SendKeys(surName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_POSTCODE).Id)).SendKeys(postCode);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            return true;
        }

        public void Activation_CheckErrorMsg(String value, string msgID, String textBoxKey, String errorMsgKey, String fieldName)
        {
            IAlert alert = null;
            CustomLogs.LogMessage("Checking Error Message for by puttinng the values:" + value + "for field: " + fieldName, TraceEventType.Start);
            try
            {
                Resource res = AutomationHelper.GetResourceMessage(msgID, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
                var expectedErrorMessage = res.Value;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(textBoxKey).Id)).SendKeys(value);
                Thread.Sleep(20);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();
                var actualErrorMsg = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDERRORMSG).XPath)).Text;
                if (expectedErrorMessage == actualErrorMsg)
                {
                    CustomLogs.LogInformation("Error Msg Verified for field " + fieldName);
                    Driver.Navigate().GoToUrl(Driver.Url);
                    if (isAlertPresent())
                        alert.Accept();
                }
                else
                {
                    CustomLogs.LogInformation("Error Message NOT Verified. Scenario Failed");
                    Assert.Fail(expectedErrorMessage + " is not equal to " + actualErrorMsg + " ,So" + fieldName + " Error msg not verified");
                }
                CustomLogs.LogMessage("Checked Error Message for the values:" + value + "for field: " + fieldName, TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                //CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Activation_CheckErrorMsgForInvalidClubCard()
        {
            try
            {
                CustomLogs.LogMessage("Checking Error Message for Invalid Clucard Starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Activation_CheckErrorMsg(testData.SpaceChar, ValidationKey.ERRORFORMANDATORYCLUBCARD, ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_CLUBCARDERRORMSG, "ClubCard");
                Activation_CheckErrorMsg(testData.SpecialChars, ValidationKey.ERRORFORMANDATORYCLUBCARD, ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_CLUBCARDERRORMSG, "ClubCard");
                Activation_CheckErrorMsg(testData.AlphabetChar, ValidationKey.ERRORFORMANDATORYCLUBCARD, ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_CLUBCARDERRORMSG, "ClubCard");
                CustomLogs.LogMessage("Error Message for Invalid Clucard Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void Activation_CheckErrorMsgForInvalidDOB()
        {
            try
            {
                CustomLogs.LogMessage("Checking Error Message for Invalid Clucard Starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Activation_CheckErrorMsgforDOB(ValidationKey.DayOfBirth, ValidationKey.MonthOfBirth, ValidationKey.YearofBirth, ControlKeys.ACTIVATION_CLUBCARDNUMBER, ControlKeys.ACTIVATION_CLUBCARDERRORMSG, "ClubCard");
                CustomLogs.LogMessage("Error Message for Invalid Clucard Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        //        Activatiom error message for DOB
        public void Activation_CheckErrorMsgforDOB(string msgID1, string msgID2, string msgID3, String textBoxKey, String errorMsgKey, String fieldName)
        {
            IAlert alert = null;
            CustomLogs.LogMessage("Checking Error Message for Date fo birth field", TraceEventType.Start);
            try
            {
                Resource resDay = AutomationHelper.GetResourceMessage(msgID1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
                var expectedErrorMessageDay = resDay.Value;
                Resource resMonth = AutomationHelper.GetResourceMessage(msgID2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
                var expectedErrorMessageMonth = resMonth.Value;
                Resource resYear = AutomationHelper.GetResourceMessage(msgID3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE));
                var expectedErrorMessageYear = resYear.Value;
                //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(textBoxKey).Id)).SendKeys(value);
                //Thread.Sleep(20);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();
                var actualErrorMsgDay = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_ErrorMessageDOB).Id)).Text;
                var actualErrorMsgMonth = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_ErrorMessageMOB).Id)).Text;
                var actualErrorMsgYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_ErrorMessageYOB).Id)).Text;
                if (expectedErrorMessageDay == actualErrorMsgDay && expectedErrorMessageMonth == actualErrorMsgMonth && expectedErrorMessageYear == actualErrorMsgYear)
                {
                    CustomLogs.LogInformation("Error Msg Verified for field " + fieldName);
                    Driver.Navigate().GoToUrl(Driver.Url);
                    if (isAlertPresent())
                        alert.Accept();
                }
                else
                {
                    CustomLogs.LogInformation("Error Message NOT Verified. Scenario Failed");
                    Assert.Fail(expectedErrorMessageDay + " is not equal to " + actualErrorMsgDay + " ,So" + fieldName + " Error msg not verified");
                }
                CustomLogs.LogMessage("Checked Error Message for the values:" + fieldName, TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                //CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public bool CSC_SearchCustomerAndCheckAccount()
        {
            try
            {
                CustomLogs.LogMessage("Search for the customer ane check for delink account button", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                //Search Customer with ClubCard with needs to be activated
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERSEARCH_CLUBCARD).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERSEARCH_SEARCHCUSTOMER).Id)).Click();
                //Capture the first Name, Surname and PostCode for the clubcard
                firstName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_FIRSTNAME).Id)).GetAttribute("Value");
                surName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_SURNAME).Id)).GetAttribute("Value");
                postCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_POSTCODE).Id)).GetAttribute("Value");


                //Delink the Account so that customer can activate again
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCDELINK_CLICK).Id)).Click();

                //If Delink button is NOT present , account is Activated one
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_DELINKACC).Id), Driver))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                CustomLogs.LogError(ex);
                Driver.Quit();
                return false;
            }
        }

        public void Activation_CheckConnectedMessage(String pageName)
        {
            try
            {
                IWebElement ele = null;
                CustomLogs.LogMessage("Checking message for already connected message starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ele.WaitForTitle(Driver, pageName);
                Activation_CheckConnectedMsgConfirmation(ValidationKey.MESSAGEFORACTIVATINGCLUBCARD);
                CustomLogs.LogMessage("Message for already connected message completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Activation_CheckConnectedMsgConfirmation(string msgID)
        {
            try
            {
                IAlert alert = null;
                CustomLogs.LogMessage("Checking the message for already connected message starting", TraceEventType.Start);
                string expectedSuccessMessage = AutomationHelper.GetResourceMessage(msgID, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
                Driver = ObjAutomationHelper.WebDriver;
                Thread.Sleep(40);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_FIRSTNAME).Id)).SendKeys(firstName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SURNAME).Id)).SendKeys(surName);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_POSTCODE).Id)).SendKeys(postCode);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CONFIRMBUTTON).Id)).Click();

                string actualSuccessMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_SUCCESSMSG).XPath)).Text;
                if (expectedSuccessMessage == actualSuccessMessage)
                {
                    CustomLogs.LogInformation("Expected Success Message : " + expectedSuccessMessage + "is equal to actual success message" + actualSuccessMessage + ". Hence,Success Msg Verified");
                    CustomLogs.LogInformation("Success Msg Verified");
                    Driver.Navigate().GoToUrl(Driver.Url);
                    if (isAlertPresent())
                        alert.Accept();
                }
                else
                {
                    CustomLogs.LogInformation("Success message NOT verified. Actual Success message coming is " + actualSuccessMessage);
                    Assert.Fail(expectedSuccessMessage + " is not equal to " + actualSuccessMessage + " ,So Success msg not verified");
                }
            }
            catch (Exception ex)
            {
                //CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Activation_CheckErrorMsgForInvalidFirstName()
        {
            try
            {
                CustomLogs.LogMessage("Checking Error Message for Invalid Firstname Starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Activation_CheckErrorMsg(testData.SpaceChar, ValidationKey.ERRORFORMANDATORYFIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAMEERRORMSG, "FirstName");
                Activation_CheckErrorMsg(testData.SpecialChars, ValidationKey.ERRORFORMANDATORYFIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAMEERRORMSG, "FirstName");
                Activation_CheckErrorMsg(testData.Numbers, ValidationKey.ERRORFORMANDATORYFIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAME, ControlKeys.ACTIVATION_FIRSTNAMEERRORMSG, "FirstName");
                CustomLogs.LogMessage("Error Message for Invalid Firstname Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Activation_CheckErrorMsgForInvalidSurName()
        {
            try
            {
                CustomLogs.LogMessage("Checking Error Message for Invalid Surname Starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Activation_CheckErrorMsg(testData.SpaceChar, ValidationKey.ERRORFORMANDATORYSURNAME, ControlKeys.ACTIVATION_SURNAME, ControlKeys.ACTIVATION_SURNAMEERRORMSG, "Surname");
                Activation_CheckErrorMsg(testData.SpecialChars, ValidationKey.ERRORFORMANDATORYSURNAME, ControlKeys.ACTIVATION_SURNAME, ControlKeys.ACTIVATION_SURNAMEERRORMSG, "Surname");
                Activation_CheckErrorMsg(testData.Numbers, ValidationKey.ERRORFORMANDATORYSURNAME, ControlKeys.ACTIVATION_SURNAME, ControlKeys.ACTIVATION_SURNAMEERRORMSG, "Surname");
                CustomLogs.LogMessage("Error Message for Invalid Surname Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void Activation_CheckErrorMsgForInvalidPostCode()
        {
            try
            {
                CustomLogs.LogMessage("Checking Error Message for Invalid Postcode Starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Activation_CheckErrorMsg(testData.SpaceChar, ValidationKey.ERRORFORMANDATORYPOSTCODE, ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, "PostCode");
                Activation_CheckErrorMsg(testData.SpecialChars, ValidationKey.ERRORFORMANDATORYPOSTCODE, ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, "PostCode");
                Activation_CheckErrorMsg(testData.Numbers, ValidationKey.ERRORFORMANDATORYPOSTCODE, ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, "PostCode");
                CustomLogs.LogMessage("Error Message for Invalid Postcode Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void CSC_Login()
        {
            try
            {
                CustomLogs.LogMessage("CSC login started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCLOGIN_USERID).Id)).SendKeys(testData.CSCUsername);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCLOGIN_PASSWORD).Id)).SendKeys(testData.CSCPassword);

                //var ddlSelectDomain = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCLOGIN_DOMAIN).Id));
                //var mySelect = new SelectElement(ddlSelectDomain);
                //mySelect.SelectByValue("LDAP://global.tesco.org");

                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCLOGIN_SUBMIT).Id)).Click();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                CustomLogs.LogInformation("You are Logged in as user " + testData.CSCUsername);
                CustomLogs.LogMessage("CSC Login completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public bool CSC_SearchCustomerAndDelinkAccount()
        {
            try
            {
                CustomLogs.LogMessage("Search for the customer and de link the account in CSC", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                //Search Customer with ClubCard with needs to be activated
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERSEARCH_CLUBCARD).Id)).SendKeys(testData.Clubcard);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERSEARCH_SEARCHCUSTOMER).Id)).Click();

                //Capture the first Name, Surname and PostCode for the clubcard
                firstName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_FIRSTNAME).Id)).GetAttribute("Value");
                surName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_SURNAME).Id)).GetAttribute("Value");
                postCode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_CUSTOMERDETAILS_POSTCODE).Id)).GetAttribute("Value");

                //Delink the Account so that customer can activate again
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCDELINK_CLICK).Id)).Click();

                //If Delink button is present , account is Activated one
                if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_DELINKACC).Id)))
                //if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_DELINKACC).Id), Driver))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSC_DELINKACC).Id)).Click();
                    if (isAlertPresent())
                    {
                        alert.Accept();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }
        }

        public void CSC_Logout()
        {
            try
            {
                CustomLogs.LogMessage("CSC logout started ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                //Logout of CSC Application
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CSCLOGOUT).Id)).Click();
                CustomLogs.LogMessage("CSC logout compeleted ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void CheckConfigurationForActivation()
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            CustomLogs.LogMessage("Checking configurations from UI based on country ", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                //string dob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLDOB).Id)).GetAttribute("style");
                switch (CountrySetting.country)
                {
                    case "UK":
                        //To Check which input fields are displaying on UI
                        string firstName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLFIRSTNAME).Id)).GetAttribute("style");
                        string surname = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLSURNAME).Id)).GetAttribute("style");
                        // string dob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLDOB).Id)).GetAttribute("style");
                        //string mob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLMOB).Id)).GetAttribute("style");
                        //string yob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLYOB).Id)).GetAttribute("style");
                        string addressLine1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLADDRESSLINE1).Id)).GetAttribute("style");
                        string postcode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLPOSTCODE).Id)).GetAttribute("style");
                        string mobileNumer = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLMOBILENUMBER).Id)).GetAttribute("style");
                        string ssn = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLSSN).Id)).GetAttribute("style");
                        string email = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLEMAIL).Id)).GetAttribute("style");



                        if (new[] { firstName, surname, postcode }.All(x => x == "DISPLAY: block"))
                        {
                            CheckFromDBConfiguration(CountrySetting.country);
                            if (returnvalue.Equals("Y"))
                                break;
                            else
                                Assert.Inconclusive("Test case can't be run");
                        }
                        else
                            Assert.Inconclusive("Test case can't be run");
                        break;

                    //case "CK":
                    //    //To Check which input fields are displaying on UI


                    //    if (new[] { firstName, surname, postcode }.All(x => x == "DISPLAY: block"))
                    //    {
                    //        CheckFromDBConfiguration(CountrySetting.country);
                    //        if (returnvalue.Equals("Y"))
                    //            break;
                    //        else
                    //            Assert.Inconclusive("Test case can't be run");
                    //    }
                    //    break;

                    case "MY":

                        string clubcard = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER).Id)).GetAttribute("style");

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

        //To Check the value from DB Configuration
        public void CheckFromDBConfiguration(string country)
        {
            CustomLogs.LogMessage("Checking configurations from DB Configuration based on country", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                DBConfiguration config_Firstname = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "Name1", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_surname = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "Name3", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_postcode = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "MailingAddressPostCode", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_dob = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "DayofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_mob = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "MonthofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_yob = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "YearofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_addressline1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "YearofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_mobileNo = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "YearofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_ssn = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "YearofBirth", SanityConfiguration.DbConfigurationFile);
                DBConfiguration config_email = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Activation, "YearofBirth", SanityConfiguration.DbConfigurationFile);


                string configName = config_Firstname.IsDeleted;
                string configSurname = config_surname.IsDeleted;
                string configPostcode = config_postcode.IsDeleted;
                string configDob = config_dob.IsDeleted;
                string configMob = config_mob.IsDeleted;
                string configYob = config_yob.IsDeleted;
                string configaddress = config_addressline1.IsDeleted;
                string configMobile = config_email.IsDeleted;
                string configSsn = config_ssn.IsDeleted;
                string configEmail = config_email.IsDeleted;

                switch (country)
                {
                    case "UK":
                        if (new[] { configName, configSurname, configPostcode }.All(x => x == "N"))
                        {
                            returnvalue = "Y";
                            break;
                        }
                        else
                            returnvalue = "N";
                        break;
                    case "CK":
                        if (new[] { configName, configSurname, configPostcode }.All(x => x == "N"))
                        {
                            returnvalue = "Y";
                            break;
                        }
                        else
                            returnvalue = "N";
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

        //To register for new account activation
        public void RegisterNewId()
        {
            rnd = new Random();
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            CustomLogs.LogMessage("Registering new account ", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                email2 = "@tesco.com";
                Email = testData.EmailID + rnd.Next(1, 999) + email2;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_EMAIL).Id)).SendKeys(Email);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_PASSWORD).Id)).SendKeys(testData.Password);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_REPASSWORD).Id)).SendKeys(testData.Repassword);

                Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_REGISTER).XPath)).Click();

            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                // Driver.Quit();
            }
        }


        #endregion

        #region Methods V2

        public string IsControlVisible(string key)
        {
            string error = string.Empty;
            try
            {
                Control Ctrl = ObjAutomationHelper.GetControl(key);
                IWebElement wCtrl = objGeneric.GetWebControl(Ctrl, FindBy.CSS_SELECTOR_ID);
                if (wCtrl == null)
                {
                    error = string.Format("Unable to locate the UI control for '{0}'", key);
                }
            }
            catch (Exception ex)
            {
                error = string.Format("{0}, {1}", ex.Message, ex.StackTrace);
            }
            return error;
        }

        public bool IsControlEnabled(string key)
        {
            bool chk = false;
            List<Control> controls = GetAllControls(FieldType.All);
            Control dob = controls.Find(c => c.DBConfigurations.Contains(key));
            if (dob == null)
            {
                Assert.Inconclusive(string.Format("{0} is not enabled for country : {1}, culture : {2}", key, CountrySetting.country, CountrySetting.culture));
            }
            else
            {
                chk = true;
            }
            return chk;
        }

        public List<IWebElement> GetAllUIControls(FieldType type)
        {
            List<IWebElement> webControls = new List<IWebElement>();
            try
            {
                List<Control> allControls = GetAllControls(type);
                allControls.ForEach(c => webControls.Add(objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID)));

                Control clubcard = ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_CLUBCARDNUMBER);
                IWebElement wClubcard = objGeneric.GetWebControl(clubcard, FindBy.CSS_SELECTOR_ID);

                webControls.Add(wClubcard);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return webControls;
        }

        public List<Control> GetAllControls(FieldType type)
        {
            List<DBConfiguration> fields = new List<DBConfiguration>();
            List<Control> allControls = new List<Control>();
            fields = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.Activation, SanityConfiguration.DbConfigurationFile);
            if (type.Equals(FieldType.All))
            {
                fields = fields.FindAll(f => f.ConfigurationValue1.Equals("1"));
            }
            else if (type.Equals(FieldType.Mandatory))
            {
                fields = fields.FindAll(f => !f.ConfigurationValue1.Equals("0") && f.IsDeleted.Equals("N"));
            }

            allControls = ObjAutomationHelper.GetControls("ActivationControls");
            allControls = (from t in allControls
                           where fields.FindAll(f => t.DBConfigurations != null && t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                           select t).ToList();
            return allControls;
        }

        public void ValidateErrorMessage_InvalidValues(string controlKey, string controlKey1, string resourceKey)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys("1Ab34#$%");
            objGeneric.ScrollToBottom();
            objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            objGeneric.ScrollToBottom();
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            var actualErrorMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey1).Id)).Text;
            var ExpectedErrorMessage = AutomationHelper.GetResourceMessage(resourceKey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
            Assert.AreEqual(ExpectedErrorMessage, actualErrorMessage, "Error message displayed for invalid" + controlKey + " is not correct");
        }


        public void ValidateMandatoryField(string controlKey, string resourceKey)
        {
            Driver = ObjAutomationHelper.WebDriver;
            objGeneric.ScrollToBottom();
            objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            objGeneric.ScrollToBottom();
            var actualErrorMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).Text;
            var ExpectedErrorMessage = AutomationHelper.GetResourceMessage(resourceKey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
            Assert.AreEqual(ExpectedErrorMessage, actualErrorMessage, "Error message displayed for invalid" + controlKey + " is not correct");
        }

        public string validateMandatory_ReturnError(string controlKey, string resourceKey)
        {
            string Error = string.Empty;
            Driver = ObjAutomationHelper.WebDriver;
            objGeneric.ScrollToBottom();
            objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            var actualErrorMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).Text;
            var ExpectedErrorMessage = AutomationHelper.GetResourceMessage(resourceKey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
            if (!(actualErrorMessage == ExpectedErrorMessage))
            {
                Error = string.Format("Error message for {0} is not correct. Actual : '{1}', Expected : '{2}'. ", controlKey, actualErrorMessage, ExpectedErrorMessage);
                return Error;
            }
            else
            {
                return Error;
            }
        }

        public void Validate_ReConfirmPageTitle(string controlKey, string reconfirmKey, string resourceKey)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys("1234567891231234");
            List<Control> controls = GetAllControls(FieldType.All);
            List<IWebElement> wControls = GetWebElements(controls);
            int index = 0;
            foreach (Control ctrl in controls)
            {
                IWebElement wControl = wControls[index++];
                if (wControl != null)
                {
                    string defaultValue = TestDataHelper.GetTestData(SanityConfiguration.TestDataFile, "TestData_Activation", ctrl.DataNode, SanityConfiguration.Domain);
                    switch (wControl.TagName.ToUpper())
                    {
                        case "INPUT":
                            string type = wControl.GetAttribute("type");
                            switch (type.ToUpper())
                            {
                                case "TEXT":
                                    if (string.IsNullOrEmpty(defaultValue))
                                    {
                                        defaultValue = "ABCD";
                                    }
                                    wControl.SendKeys(defaultValue);
                                    break;
                            }
                            break;
                        case "SELECT":
                            wControl.SendKeys(Keys.Down);
                            break;
                    }
                }
            }
            objGeneric.ScrollToBottom();
            objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(reconfirmKey).Id)))
            {
                var actualPageTitle = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(reconfirmKey).Id)).Text;
                var ExpectedPageTitle = AutomationHelper.GetResourceMessage(resourceKey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
                Assert.AreEqual(ExpectedPageTitle, actualPageTitle, "Re confirm Page title displayed is not correct for " + CountrySetting.country + CountrySetting.culture);
            }
            if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_RECONFIRMTITLE1).Id)))
            {
                var actualPageTitle = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_RECONFIRMTITLE1).Id)).Text;
                var ExpectedPageTitle = AutomationHelper.GetResourceMessage(ValidationKey.MESSAGEFORRACTIVATIONERROR2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
                Assert.AreEqual(ExpectedPageTitle, actualPageTitle, "Re confirm Page title displayed is not correct for " + CountrySetting.country + CountrySetting.culture);
            }
            
        }

        List<IWebElement> GetWebElements(List<Control> cControls)
        {
            List<IWebElement> wControls = new List<IWebElement>();
            foreach (Control c in cControls)
            {
                if (!string.IsNullOrEmpty(c.Id))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID));
                }
                else if (!string.IsNullOrEmpty(c.ClassName))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_CSS));
                }
                else if (!string.IsNullOrEmpty(c.XPath))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.XPATH_SELECTOR));
                }
            }
            return wControls;
        }

        public void DelinkClubcard(string clubcardNumber, string controlKey, string strCulture, string DotcomID)
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomerServiceAdaptor Cust = new CustomerServiceAdaptor();
            ClubcardServiceAdapter Status = new ClubcardServiceAdapter();
            CustomerServiceAdaptor Linked = new CustomerServiceAdaptor();
            long custId = Cust.GetCustomerID(clubcardNumber, strCulture);
            List<Control> controls = GetAllControls(FieldType.All);
            List<IWebElement> wControls = GetWebElements(controls);
            int index = 0;
            Dictionary<string, string> CustomerDetails = Cust.GetCustomerDetails(custId.ToString(), CountrySetting.culture);
            if (CustomerDetails.ContainsKey("family_member_1_dob"))
             {
                DateTime dob = new DateTime();
                dob = Convert.ToDateTime(CustomerDetails["family_member_1_dob"]);
                CustomerDetails.Add("Day", dob.Day.ToString());
                CustomerDetails.Add("Month", dob.Month.ToString());
                CustomerDetails.Add("Year", dob.Year.ToString());
            }
            //string alternateId = Cust.GetAlternateIDs(custId, CountrySetting.culture);
            bool ActivationStatus = Status.GetActivationstatus(DotcomID, strCulture);
            if (ActivationStatus)
            {
                CustomLogs.LogMessage("Activation |Year or birth", TraceEventType.Start);
                bool LinkedStatus = Linked.delinkAccount(clubcardNumber);
                CustomLogs.LogMessage("Verifying Home Page Navigation ", TraceEventType.Start);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys(clubcardNumber);
                foreach (Control ctrl in controls)
                {
                    IWebElement wControl = wControls[index++];
                    if (wControl != null)
                    {

                        string defaultValue = CustomerDetails[ctrl.PropertyName];
                        switch (wControl.TagName.ToUpper())
                        {
                            case "INPUT":
                                string type = wControl.GetAttribute("type");
                                if (string.IsNullOrEmpty(defaultValue))
                                {
                                    defaultValue = "ABCD";
                                }
                                wControl.SendKeys(defaultValue);
                                break;

                            case "SELECT":

                                SelectElement ddlValue = new SelectElement(wControl);
                                ddlValue.SelectByText(defaultValue);

                                break;
                        }
                    }

                    else
                    {
                        Assert.Inconclusive("Activation Home page is not displayed. Test cases cannot be executed");
                    }
                }
                CustomLogs.LogMessage("Year of birth on Activation", TraceEventType.Stop);
                objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            }
            else
            {


                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys(clubcardNumber);
                foreach (Control ctrl in controls)
                {
                    IWebElement wControl = wControls[index++];
                    if (wControl != null)
                    {
                        string defaultValue = CustomerDetails[ctrl.PropertyName];
                        switch (wControl.TagName.ToUpper())
                        {
                            case "INPUT":
                                string type = wControl.GetAttribute("type");
                                if (string.IsNullOrEmpty(defaultValue))
                                {
                                    defaultValue = "ABCD";
                                }
                                wControl.SendKeys(defaultValue);
                                break;
                            case "SELECT":
                                SelectElement ddlValue = new SelectElement(wControl);
                                 ddlValue.SelectByText(defaultValue);
                                break;
                        }
                    }
                }
                objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            }
        }

        public void LinkedClubcard(string ClubcardNumber, string controlKey, string strCulture)
        {

            Driver = ObjAutomationHelper.WebDriver;
            CustomerServiceAdaptor Cust = new CustomerServiceAdaptor();
            long custId = Cust.GetCustomerID(ClubcardNumber, strCulture);
            List<Control> controls = GetAllControls(FieldType.All);
            List<IWebElement> wControls = GetWebElements(controls);
            int index = 0;
            Dictionary<string, string> CustomerDetails = Cust.GetCustomerDetails(custId.ToString(), CountrySetting.culture);
            if (CustomerDetails.ContainsKey("family_member_1_dob"))
            {
                DateTime dob = new DateTime();
                dob = Convert.ToDateTime(CustomerDetails["family_member_1_dob"]);
                CustomerDetails.Add("Day", dob.Day.ToString());
                CustomerDetails.Add("Month", dob.Month.ToString());
                CustomerDetails.Add("Year", dob.Year.ToString());
            }
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id)).SendKeys(ClubcardNumber);
            foreach (Control ctrl in controls)
            {
                IWebElement wControl = wControls[index++];
                if (wControl != null)
                {
                    string defaultValue = CustomerDetails[ctrl.PropertyName];
                    switch (wControl.TagName.ToUpper())
                    {
                        case "INPUT":
                            string type = wControl.GetAttribute("type");
                            if (string.IsNullOrEmpty(defaultValue))
                            {
                                defaultValue = "ABCD";
                            }
                            wControl.SendKeys(defaultValue);
                            break;
                        case "SELECT":
                           SelectElement ddlValue = new SelectElement(wControl);
                                 ddlValue.SelectByText(defaultValue);
                            break;
                    }
                }
            }
            objGeneric.ClickElement(ControlKeys.ACTIVATION_CONFIRMBUTTON, FindBy.CSS_SELECTOR_ID);
            string ActualActivationLinkedMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LinkedCard).Id)).Text;
            string ExpectedActivationLinkedMessage = AutomationHelper.GetResourceMessage(ValidationKey.MESSAGEFORRACTIVECARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ACTIVATION_RESOURCE)).Value;
        }

        public void VerifyHomePage()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("Verifying Home Page Navigation ", TraceEventType.Start);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
            string Home = (AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE))).Value;
            Assert.AreEqual(Home, ObjAutomationHelper.WebDriver.Title);
        }

        #endregion

    }
}

