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
        IAlert alert = null;
        Generic objGeneric = null;

        #endregion

        #region Constructor


        public Activation(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        public Activation(AutomationHelper objHelper, AppConfiguration configuration, TestData_Activation testData)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            this.testData = testData;
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
                alert=Driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

        public void VerifyActivationPage(IWebDriver Driver)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_TEXT).Id));
                if (Generic.IsElementPresent((By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_TEXT).Id)), Driver))
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

        public bool Activation_CheckMsgAfterConfirmation(string  msgID)
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
                    CustomLogs.LogInformation("Expected Success Message : "+ expectedSuccessMessage + "is equal to actual success message" + actualSuccessMessage +". Hence,Success Msg Verified");
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
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(errorMsgKey).Id)).Text;
                if (expectedErrorMessage == actualErrorMsg)
                {
                    CustomLogs.LogInformation("Error Msg Verified for field "+fieldName);
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
                Activation_CheckErrorMsg(testData.SpaceChar,ValidationKey.ERRORFORMANDATORYPOSTCODE, ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, "PostCode");
                Activation_CheckErrorMsg(testData.SpecialChars,ValidationKey.ERRORFORMANDATORYPOSTCODE, ControlKeys.ACTIVATION_POSTCODE, ControlKeys.ACTIVATION_POSTCODEERRORMSG, "PostCode");
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
                //To Check which input fields are displaying on UI
                string firstName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLFIRSTNAME).Id)).GetAttribute("style");
                string surname = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLSURNAME).Id)).GetAttribute("style");
                string dob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLDOB).Id)).GetAttribute("style");
                string mob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLMOB).Id)).GetAttribute("style");
                string yob = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLYOB).Id)).GetAttribute("style");
                string addressLine1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLADDRESSLINE1).Id)).GetAttribute("style");
                string postcode = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLPOSTCODE).Id)).GetAttribute("style");
                string mobileNumer = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLMOBILENUMBER).Id)).GetAttribute("style");
                string ssn = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLSSN).Id)).GetAttribute("style");
                string email = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_LBLEMAIL).Id)).GetAttribute("style");

                switch (CountrySetting.country)
                {
                    case "UK":
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

                    case "CK":
                        if (new[] { firstName, surname, postcode }.All(x => x == "DISPLAY: block"))
                        {
                            CheckFromDBConfiguration(CountrySetting.country);
                            if (returnvalue.Equals("Y"))
                                break;
                            else
                                Assert.Inconclusive("Test case can't be run");
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
                Email = testData.Email + rnd.Next(1, 999) + email2;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_EMAIL).Id)).SendKeys(Email);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_PASSWORD).Id)).SendKeys(testData.Password);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_REPASSWORD).Id)).SendKeys(testData.Repassword);

                Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACTIVATION_REGISTER).XPath)).Click();
               
            }
            catch(Exception ex)
            {
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
               // Driver.Quit();
            }
        }
        #endregion
    }
}
