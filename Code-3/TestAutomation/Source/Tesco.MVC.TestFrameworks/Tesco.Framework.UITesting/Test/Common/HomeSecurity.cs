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
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using Tesco.Framework.UITesting.Constants;
using System.Diagnostics;
using System.IO;
using Tesco.Framework.UITesting.Services;
using System.Xml.Linq;


namespace Tesco.Framework.UITesting.Test.Common
{
    class HomeSecurity : Base
    {
        #region Public Members
        public static int firstPosition = 0;
        public static int secondPosition = 0;
        public static int thirdPosition = 0;
        public static int expected1stPoition = 0;
        public static int expected2ndPoition = 0;
        public static int expected3rdPoition = 0;
        public static int expected4thPoition = 0;
        static private string isSecurityPageEnabled = String.Empty;
        public string returnvalue = string.Empty;
        Random rnd = null;

        TestData_HomeSecurity testData_Home = null;
        Generic objGeneric = null;
        #endregion

        #region Constructor

        public HomeSecurity(AutomationHelper objhelper)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }
        public HomeSecurity(AutomationHelper objHelper, AppConfiguration configuration, TestData_HomeSecurity testData_Home)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            this.testData_Home = testData_Home;
            objGeneric = new Generic(ObjAutomationHelper);
            SanityConfiguration = configuration;
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }
        #endregion

        #region Methods
        public void Verify2LAPage()
        {
            CustomLogs.LogMessage("Verifying 2LA Page started ", TraceEventType.Start);
            string resourceFile = Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE);
            string spTitle = (AutomationHelper.GetResourceMessage(LabelKey.SECURITYPAGETITLE, resourceFile)).Value;
            if (spTitle == null)
            {
                Assert.Fail(string.Format("Expected Page title not found in : {1}, key: {0}", LabelKey.SECURITYPAGETITLE, resourceFile));
            }
            if (!spTitle.Equals(ObjAutomationHelper.WebDriver.Title))
            {
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails ");
                Assert.AreEqual(spTitle, ObjAutomationHelper.WebDriver.Title);
            }
            CustomLogs.LogMessage("verifying 2LA Page completed ", TraceEventType.Stop);
        }
        //Click on Submit button and Verify Home Page.
        public void clickSubmitButton()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            CustomLogs.LogMessage("HomeSecurity_clickSubmitButton", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
            ObjAutomationHelper.GetControl(ControlKeys.HOMEPAGE_LINK).WaitForControlByCss(Driver);
            CustomLogs.LogMessage("HomeSecurity_clickSubmitButton", TraceEventType.Stop);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        //To Capture the Security digits position.
        public List<Int32> CaptureSecurityDigitsPosition()
        {
            List<Int32> actualPositions = new List<int>();
            CustomLogs.LogMessage("Capturing Security Digits Position from UI started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            // ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            string encryVal1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).GetAttribute("value");
            firstPosition = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal1));

            string encryVal2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).GetAttribute("value");
            secondPosition = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal2));

            string encryVal3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).GetAttribute("value");
            thirdPosition = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal3));

            actualPositions.AddRange(new Int32[] { firstPosition, secondPosition, thirdPosition });
            CustomLogs.LogMessage("Capturing Security Digits Position from UI completed", TraceEventType.Stop);
            return actualPositions;
        }

        //To insert the random numbers in the security verification text box and submit.
        public void InsertSecurityDigitsPosition_random()
        {
            CustomLogs.LogMessage("Inserting Security digit position with random numbers started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            rnd = new Random();

            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + rnd.Next(1, 9));
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + rnd.Next(1, 9));
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + rnd.Next(1, 9));
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            CustomLogs.LogMessage("Inserting Security digit position with random numbers completed", TraceEventType.Stop);
        }

        //To insert the correct security digit position in security verification text box ,submit and verify the home page.
        public void InsertSecurityDigitsPosition(String ClubcardId)
        {
            CustomLogs.LogMessage("Inserting Security Digit Position with correct value started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[firstPosition - 1]);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[secondPosition - 1]);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[thirdPosition - 1]);
            Thread.Sleep(50);
            clickSubmitButton();
            CustomLogs.LogMessage("Inserting Security Digit Position with correct value Completed", TraceEventType.Stop);
        }

        //To insert the Invalid characters in the security verification text box and submit.
        public void InsertSecurityDigitsPosition_InvalidCharacters()
        {
            CustomLogs.LogMessage("Inserting security digit position with invalid characters started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + testData_Home.SpecialChars);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + testData_Home.AlphabetChar);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + testData_Home.SpecialChars);
            Thread.Sleep(50);
            objGeneric.ScrollToBottom();
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
            CustomLogs.LogMessage("Inserting security digit position with invalid characters completed", TraceEventType.Stop);

        }

        //To insert the Security verification which is not in the Configuration range
        public void InsertWrongSecurityDigitsPosition(String ClubcardId)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            CustomLogs.LogMessage("Inserting Security Digit Position with Wrong Value started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[expected1stPoition - 3]);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[expected2ndPoition - 6]);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[expected3rdPoition + 2]);
            Thread.Sleep(50);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
            CustomLogs.LogMessage("Inserting Security Digit Position with Wrong Value completed", TraceEventType.Stop);
        }

        //To Check the Configuration type for Security Check
        public void CheckValueFromConfigurationType()
        {
            CustomLogs.LogMessage("Checking value of security positions from DB configuration started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.SecurityCheck, "ClubcardSecureDigits", SanityConfiguration.DbConfigurationFile);
            var numbers = config.ConfigurationValue1.Split(',').Select(Int32.Parse).ToList();
            expected1stPoition = numbers[0];
            expected2ndPoition = numbers[1];
            expected3rdPoition = numbers[2];
            expected4thPoition = numbers[3];
            CustomLogs.LogMessage("Checking value of security positions from DB configuration completed", TraceEventType.Stop);
        }

        //To insert the Space in the security verification text box and submit.
        public void InsertSecurityDigitsPosition_Space()
        {
            CustomLogs.LogMessage("Inserting of security digit position with space started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;

            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + testData_Home.SpaceChar);
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("");
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + testData_Home.SpaceChar);
            Thread.Sleep(500);
            clickSubmitButton();
            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            CustomLogs.LogMessage("Inserting of security digit position with space completed", TraceEventType.Stop);
        }

        //To Capture Error message
        public void CaptureInvalidNoErrorMessage()
        {
            CustomLogs.LogMessage("Capturing Error Message for invalid number started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.INVALIDSECURITYNUMBERMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDNOMESSAGE).Id)).Text;
                Assert.AreEqual(expectedErrorMessage, actualErrorMsg, expectedErrorMessage + " is not equal to " + actualErrorMsg + " ");
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Capturing Error Message for invalid number completed", TraceEventType.Stop);
        }

        //To capture the error message for invalid characters
        public void CaptureInvalidCharacterErrorMessage()
        {
            CustomLogs.LogMessage("Capturing Error Message for Invalid characters started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.INVALIDCHARMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;

                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);

                var actualErrorMsg1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG1).Id)).Text;

                var actualErrorMsg2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG2).Id)).Text;

                var actualErrorMsg3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG3).Id)).Text;

                IWebElement HiddenErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDNOMESSAGE).Id));
                if (!HiddenErrorMsg.Displayed)
                    CustomLogs.LogInformation("Previous error message is not getting displayed now.");
                else
                    CustomLogs.LogInformation("Previous error message is getting displayed");

                Assert.AreEqual(expectedErrorMessage, actualErrorMsg1, expectedErrorMessage + " is not equal to " + actualErrorMsg1);
                Assert.AreEqual(expectedErrorMessage, actualErrorMsg2, expectedErrorMessage + " is not equal to " + actualErrorMsg2);
                Assert.AreEqual(expectedErrorMessage, actualErrorMsg3, expectedErrorMessage + " is not equal to " + actualErrorMsg3);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Capturing Error Message for Invalid characters started", TraceEventType.Stop);
        }

        //To capture the error message for security diigit to be a number
        public string CaptureInvalidErrorMessage(string controlkey, string validationkey)
        {
            string error = string.Empty;
            CustomLogs.LogMessage("Capturing Error Message for invalid character started", TraceEventType.Start);
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(validationkey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;
                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                String ExpectedColor = "rgba(204, 204, 204, 1)";
                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlkey).Id)).Text;
                String color = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).GetCssValue("border-bottom-color");
                if (color.Equals(ExpectedColor))
                {
                    CustomLogs.LogInformation("Error border color is red");
                }
                else
                    Assert.Fail("Error border color is not red");
                if (!(actualErrorMsg == expectedErrorMessage))
                {
                    error = string.Format("Error message for {0} is not correct. Actual : '{1}', Expected : '{2}'. ", controlkey, actualErrorMsg, expectedErrorMessage);
                    return error;
                }
                else
                    return error;
            }


            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Capturing Error Message for invalid character completed", TraceEventType.Stop);
            return error;
        }

        //To capture Error message for space
        public void CaptureInvalidSpaceErrorMessage()
        {
            CustomLogs.LogMessage("Capturing invalid error message for space started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                Resource resMsg1 = AutomationHelper.GetResourceMessage(ValidationKey.FIRSTDIGIT_REQUIRED, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage1 = resMsg1.Value;
                Resource resMsg2 = AutomationHelper.GetResourceMessage(ValidationKey.SECONDDIGIT_REQUIRED, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage2 = resMsg2.Value;
                Resource resMsg3 = AutomationHelper.GetResourceMessage(ValidationKey.THIRDDIGIT_REQUIRED, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage3 = resMsg3.Value;
                var actualErrorMsg1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARACTERERRORMSG1).Id)).Text;
                var actualErrorMsg2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARACTERERRORMSG2).Id)).Text;
                var actualErrorMsg3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARACTERERRORMSG3).Id)).Text;
                Assert.AreEqual(expectedErrorMessage1, actualErrorMsg1, expectedErrorMessage1 + " is not equal to " + actualErrorMsg1);
                Assert.AreEqual(expectedErrorMessage2, actualErrorMsg2, expectedErrorMessage2 + " is not equal to " + actualErrorMsg2);
                Assert.AreEqual(expectedErrorMessage3, actualErrorMsg3, expectedErrorMessage3 + " is not equal to " + actualErrorMsg3);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Capturing invalid error message for space completed", TraceEventType.Stop);
        }

        //To capture error message for default.
        public void VerifyDefaultMsg(string errorMsgKey)
        {
            CustomLogs.LogMessage("Verifying default ErrorMsg started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.DEFAULTSECURITYMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;

                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_HEADERMSG).Id)).Text;

                Assert.AreEqual(expectedErrorMessage, actualErrorMsg, expectedErrorMessage + " is not equal to " + actualErrorMsg + " ,So Security Page  msg not verified");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Verifying default ErrorMsg completed", TraceEventType.Stop);
        }

        //To verify the security Digit Position 
        public void VerifySecurityDigitPosition()
        {
            //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            CustomLogs.LogMessage("Verifying Security Digit Position started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;

            CheckValueFromConfigurationType(); //Capture Configurationvalues from XML

            CaptureSecurityDigitsPosition(); //Capture Configurationvalues from UI
            int[] UIPosition = new int[] { firstPosition, secondPosition, thirdPosition };

            for (int i = 0; i < UIPosition.Length; i++)
            {
                if (Enumerable.Range(expected1stPoition, expected4thPoition).Contains(UIPosition[i]))
                {
                    CustomLogs.LogInformation("Position at " + UIPosition[i] + "verified");
                }
                else
                {
                    Assert.Fail("Position at " + UIPosition[i] + " not verified");
                }
            }
            // Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            CustomLogs.LogMessage("Verify security digit position completed", TraceEventType.Stop);
        }

        public void Verify_SecurityMaxAttempts()
        {
            CustomLogs.LogMessage("Verifying if security max attempts has been reached or not started", TraceEventType.Start);
            try
            {
                string errorMSgForWrongInput = String.Empty, errorMsgForBlockedCard = String.Empty;
                Driver = ObjAutomationHelper.WebDriver;
                if (Verify_MaxAttemptsBlockedClubcard())
                {
                    Assert.Fail("Card is already  blocked");
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + thirdPosition);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + firstPosition);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + secondPosition);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
                        if(!(objGeneric.IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECDETAILSNOTMATCHMSG).XPath))))
                        {
                            break;
                        }
                    }
                    if (Verify_MaxAttemptsBlockedClubcard())
                    {
                        Assert.Fail("Card is already  blocked");
                    }
                }
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

            CustomLogs.LogMessage("Verifying if security max attempts has been reached or not completed", TraceEventType.Stop);
        }

        public bool Verify_MaxAttemptsBlockedClubcard()
        {
            CustomLogs.LogMessage("Verifying maximum attempts for Blocked Clubcard started", TraceEventType.Start);
            bool chk = false;
            try
            {
                string actualErrorMsg = String.Empty;
                Driver = ObjAutomationHelper.WebDriver;
                if (!Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id), Driver))
                {
                    string errorMessage = objGeneric.VerifyTextWithHtml_Contains(ValidationKey.ERRORMAXATTEMPT1, ControlKeys.SECURITY_MAXATTEMPTMSG1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE), "Home Security");
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                     errorMessage = objGeneric.VerifyTextWithHtml_Contains(ValidationKey.ERRORMAXATTEMPT2, ControlKeys.SECURITY_MAXATTEMPTMSG2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE), "Home Security");
                    }
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = objGeneric.VerifyTextWithHtml_Contains(ValidationKey.ERRORMAXATTEMPT3, ControlKeys.SECURITY_MAXATTEMPTMSG3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE), "Home Security");
                    }
                    chk = string.IsNullOrEmpty(errorMessage) ? true : false;
                    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Verifying maximum attempts for Blocked Clubcard Completed", TraceEventType.Stop);
            return chk;
        }

        public bool IsOnlineDetailsMatched(string DotcomID)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                MyAccountServiceAdapter accountAdapter = new MyAccountServiceAdapter();
                string response = accountAdapter.GetPersonalDetails(Convert.ToInt32(DotcomID));
                XDocument xmlDocument = XDocument.Parse(response);

                CustomerServiceAdaptor cusDetails = new CustomerServiceAdaptor();
                Dictionary<string, string> CustomerDetail = cusDetails.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);

                if ((xmlDocument.Descendants("PersonalDetailsEntity").Elements("Forename").First().Value.Trim().ToUpper() == CustomerDetail["Name1"].Trim().ToUpper()) &&
                   (xmlDocument.Descendants("PersonalDetailsEntity").Elements("Surname").First().Value.Trim().ToUpper() == CustomerDetail["Name3"].Trim().ToUpper()) &&
                   (xmlDocument.Descendants("PersonalDetailsEntity").Elements("ClubcardPostcode").First().Value.Trim().ToUpper() == CustomerDetail["MailingAddressPostCode"].Trim().ToUpper()))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return false;
            }

        }
        public void Verify_BannerEnabled(string DotcomID)
        {
            try
            {
                if (IsOnlineDetailsMatched(DotcomID))
                {
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_YES).Id))))
                    {
                        CustomLogs.LogInformation("Verification passed. online Details Matched: Banner is not enabled ");
                    }
                    else
                    {
                        Assert.Fail("Test case Failed. online Details Matched: Banner is enabled");
                    }
                }
                else
                {
                    if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_YES).Id)))
                    {
                        CustomLogs.LogInformation("Verification passed. Online Details not Matched:Banner is enabled ");
                    }
                    else
                    {
                        Assert.Fail("Test case Failed. Online Details not Matched :Banner is not enabled");
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
        public void Verify_BannerPostcode()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedErrorMessage = AutomationHelper.GetResourceMessage(ValidationKey.BANNERPOSTCODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE)).Value;
                var actualMsg = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_POSTCODE).Id)).Text).ToString();

                CustomerServiceAdaptor cusDetails = new CustomerServiceAdaptor();
                Dictionary<string, string> CustomerDetail = cusDetails.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);
                string strPostCode = CustomerDetail["MailingAddressPostCode"];
                if (strPostCode.Length >= 3)
                {
                    StringBuilder PC = new StringBuilder(strPostCode);
                    string strM = strPostCode.Substring(0, 3);
                    PC.Replace(strM, "XXX");
                    strPostCode = PC.ToString();

                }
                string expectedMsg = strPostCode;
                Assert.AreEqual(expectedMsg, actualMsg);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void Verify_BannerSurname()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedResxMessage = AutomationHelper.GetResourceMessage(ValidationKey.BANNERSURNAME, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE)).Value;
                var actualMsg = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_SURNAME).Id)).Text).ToString();
                CustomerServiceAdaptor cusDetails = new CustomerServiceAdaptor();
                Dictionary<string, string> CustomerDetail = cusDetails.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);
                string expectedMsg = CustomerDetail["Name3"];
                Assert.AreEqual(expectedMsg, actualMsg);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Verify_BannerYes()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_YES).Id)).Click();
                if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER).Id))))
                {

                    CustomLogs.LogInformation("Verification passed. Banner is not enabled");
                }
                else
                    Assert.Fail("Banner is still visible on Home Page");

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void Verify_TescoDotCom_BannerYes()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_YES).Id)).Click();
                if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER).Id))))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_TescoDotcomBannerYes).Id)).Click();

                    if (!(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_TescoDotcomBannerYes).Id)).Displayed))
                    {
                        CustomLogs.LogInformation("Verification passed. TescoDotcomBannerYes Banner is not enabled");
                    }
                    
                }
                else
                    Assert.Fail("Banner is still visible on Home Page");

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void Verify_TescoDotCom_BannerNo()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_YES).Id)).Click();
                if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER).Id))))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_TescoDotcomBannerDetailsNotCorrect).Id)).Click();

                    if ((Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_TescoDotcomBannerDetailsNotCorrect).Id)).Displayed))
                    {
                        CustomLogs.LogInformation("Verification passed.");
                    }

                }
                else
                    Assert.Fail("Banner is still visible on Home Page");

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void Verify_Banner_No()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                if ((objGeneric.IsElementPresentOnPage(By.Id(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER_ON_PD).Id))))
                {
                    Driver.Navigate().GoToUrl(SanityConfiguration.PersonalDetailsUrl);
                    if (!objGeneric.IsElementPresentOnPage(By.Id(ObjAutomationHelper.GetControl(ControlKeys.HOME_BANNER).Id)))
                    {
                        CustomLogs.LogInformation("Verification passed. Banner is not present on Home page");
                    }
                    else
                    {
                        Assert.Fail("Banner is still present on Home page");
                    }

                }
                else
                    Assert.Fail("OnClick it did not navigated to Personal Details");

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        public void Verify_Banner_Details(string surnameBanner, string postcodeBanner)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var actualPostcodePD = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_TXTPOSTCODE).Id)).GetAttribute("value");
                var actualSurnamePD = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PERSONALDETAILS_SURNAME).Id)).GetAttribute("value");

                string resxPostcode = AutomationHelper.GetResourceMessage(ValidationKey.BANNERPOSTCODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE)).Value;
                string resxSurname = AutomationHelper.GetResourceMessage(ValidationKey.BANNERSURNAME, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE)).Value;
                Assert.AreEqual(surnameBanner, actualSurnamePD, true);
                if (actualPostcodePD.Length >= 3)
                {
                    StringBuilder PC = new StringBuilder(actualPostcodePD);
                    string strM = actualPostcodePD.Substring(0, 3);
                    PC.Replace(strM, "XXX");
                    actualPostcodePD = PC.ToString();

                }
                string expectedPostcode =  actualPostcodePD;
                Assert.AreEqual(postcodeBanner, expectedPostcode, true);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }
       // To check if the configuration for security page from DB config
        public void VerifyIsSecurityPageEnabled(string keys)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            CustomLogs.LogMessage("Checking Security Page Enabled based on Web config", TraceEventType.Start);
            // isSecurityPageEnabled = (AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile)).Value;

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, keys, SanityConfiguration.DbConfigurationFile);

            // string configuration1 = config.ConfigurationValue1;
            isSecurityPageEnabled = config.ConfigurationValue1;
            //  return isSecurityPageEnabled;

            try
             {
                switch (CountrySetting.country)
                {
                    case "UK":
                        if (isSecurityPageEnabled == "0")
                            break;
                        else
                            Assert.Fail("web Configuration is changed in web Config");
                        break;
                    case "ROI":
                    case "MY":
                        if (isSecurityPageEnabled == "TRUE")
                            break;
                        else
                            Assert.Fail("web Configuration is changed in web Config");
                        break;
                    case "TH":
                        if (isSecurityPageEnabled == "TRUE")
                            break;
                        else
                            Assert.Fail("web Configuration is changed in web Config");
                        break;
                    case "SK":
                        if (isSecurityPageEnabled == "TRUE")
                            break;
                        else
                            Assert.Fail("web Configuration is changed in web Config");
                        break;
                    case "PL":
                        if (isSecurityPageEnabled == "FALSE")
                        {
                            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails ");
                            break;
                        }
                        else
                            Assert.Fail("Configuration changed in web Config");
                        break;
                    case "HU":
                        if (isSecurityPageEnabled == "FALSE")
                        {
                            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails ");
                            break;
                        }
                        else
                            Assert.Fail("Configuration changed in web Config");
                        break;
                    case "CZ":
                        if (isSecurityPageEnabled == "FALSE")
                        {
                            objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                            objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails ");
                            break;
                        }
                        else
                            Assert.Fail("Configuration changed in web Config");
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

        #endregion
        }

        //To verify that customer does not land on home/security after logout
        public void VerifyUrlAfterLogOut(String UrlAfterLogIn)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                Driver.Navigate().GoToUrl(UrlAfterLogIn);
                var currentPageTitle = Driver.Title;
                string securityTitle = AutomationHelper.GetResourceMessage(LabelKey.SECURITYPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE)).Value;
                string homeTitle = AutomationHelper.GetResourceMessage(LabelKey.HOMEPAGETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE)).Value;
                if (currentPageTitle != securityTitle && currentPageTitle != homeTitle)
                {
                    CustomLogs.LogInformation("After logout user is not redirected to home or security page");
                }
                else
                {
                    Assert.Fail("User is redirected to " + currentPageTitle + " after logout");
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

    }
}
