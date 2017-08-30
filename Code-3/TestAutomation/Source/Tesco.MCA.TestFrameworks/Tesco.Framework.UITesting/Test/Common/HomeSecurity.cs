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
        public void CaptureSecurityDigitsPosition()
        {
            CustomLogs.LogMessage("Capturing Security Digits Position from UI started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
            firstPosition = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).Text);
            secondPosition = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).Text);
            thirdPosition = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).Text);
            CustomLogs.LogMessage("Capturing Security Digits Position from UI completed", TraceEventType.Stop);
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
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[secondPosition - 1]);
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[thirdPosition - 1]);
            Thread.Sleep(500);
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
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + testData_Home.AlphabetChar);
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + testData_Home.SpecialChars);
            Thread.Sleep(500);
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
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[expected2ndPoition - 6]);
            Thread.Sleep(500);
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[expected3rdPoition + 3]);
            Thread.Sleep(500);
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
            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
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

                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
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
        public void CaptureInvalidErrorMessage()
        {
            CustomLogs.LogMessage("Capturing Error Message for invalid character started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.INVALIDCHARMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;

                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                String ExpectedColor = "rgba(255, 0, 0, 1)";
                var actualErrorMsg1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG1).Id)).Text;
                String color1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).GetCssValue("border-bottom-color");

                var actualErrorMsg2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG2).Id)).Text;
                String color2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).GetCssValue("border-bottom-color");

                var actualErrorMsg3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDCHARMSG3).Id)).Text;
                String color3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).GetCssValue("border-bottom-color");

                if (color1.Equals(ExpectedColor) && color2.Equals(ExpectedColor) && color3.Equals(ExpectedColor))
                    CustomLogs.LogInformation("Error border color is red");
                else
                    Assert.Fail("Error border color is not red");

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
            CustomLogs.LogMessage("Capturing Error Message for invalid character completed", TraceEventType.Stop);
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

                var actualErrorMsg1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDSPACEERRORMSG1).Id)).Text;

                var actualErrorMsg2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDSPACEERRORMSG2).Id)).Text;

                var actualErrorMsg3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_INVALIDSPACEERRORMSG3).Id)).Text;

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
            //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

            CustomLogs.LogMessage("Verifying default ErrorMsg started", TraceEventType.Start);
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(ValidationKey.DEFAULTSECURITYMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE));
                var expectedErrorMessage = res.Value;

                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);

                var actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(errorMsgKey).Id)).Text;
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
            //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            CustomLogs.LogMessage("Verifying if security max attempts has been reached or not started", TraceEventType.Start);
            try
            {
                string errorMSgForWrongInput = String.Empty, errorMsgForBlockedCard = String.Empty;
                Driver = ObjAutomationHelper.WebDriver;
                ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                string expectedErrorMsgForBlockedCard = AutomationHelper.GetResourceMessage(ValidationKey.ERRORMAXATTEMPTTEXT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE)).Value;
                string expectedErrorMsgForWrongInput = AutomationHelper.GetResourceMessage(ValidationKey.INVALIDSECURITYNUMBERMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE)).Value;

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
                        errorMSgForWrongInput = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_MAXATTEMPTMSG).Id)).Text;
                        if (!expectedErrorMsgForWrongInput.Equals(errorMSgForWrongInput))
                        {
                            break;
                        }
                    }
                    errorMsgForBlockedCard = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_MAXATTEMPTMSG).Id)).Text;
                    Assert.AreEqual(expectedErrorMsgForBlockedCard, errorMsgForBlockedCard, "Card not blocked after 5 max attempts");
                }
               // Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
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
            //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            CustomLogs.LogMessage("Verifying maximum attempts for Blocked Clubcard started", TraceEventType.Start);
            bool chk = false;
            try
            {
                string actualErrorMsg = String.Empty;
                Driver = ObjAutomationHelper.WebDriver;
                //if (ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver))
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).Id), Driver))
                {
                    string expectedErrorMessage = AutomationHelper.GetResourceMessage(ValidationKey.ERRORMAXATTEMPTTEXT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOMESECURITY_RESOURCE)).Value;

                    if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_MAXATTEMPTMSG).Id), Driver))
                    {
                        actualErrorMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_MAXATTEMPTMSG).Id)).Text;
                    }
                    chk = expectedErrorMessage.Equals(actualErrorMsg);
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

        //To check if the configuration for security page from webconfig
        public void VerifyIsSecurityPageEnabled()
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            CustomLogs.LogMessage("Checking Security Page Enabled based on Web config", TraceEventType.Start);
            isSecurityPageEnabled = (AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile)).Value;
            try
            {
                switch (CountrySetting.country)
                {
                    case "UK":
                        if (isSecurityPageEnabled.Equals("true"))
                            break;
                        else
                            Assert.Fail("Configuration changed in web Config");
                        break;
                    case "ROI":
                    case "SK":
                    case "PL":
                    case "HU":
                    case "CZ":
                        if (isSecurityPageEnabled.Equals("false"))
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
    }
}
