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
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class Login : Base
    {
        public static Int64 CustomerID = 0;
        public static Int64 HouseHoldID = 0;
        Generic objGeneric = null;
        #region Constructor

        public Login(AutomationHelper objhelper)
        {   
            this.ObjAutomationHelper = objhelper;
        }
        public Login(AutomationHelper objHelper, AppConfiguration configuration)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            //TestData = ObjAutomationHelper.GetTestDataByID(Enums.Messages.Login);
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }
        #endregion

        #region Methods

        // To Login into application based on domain        
        public bool Login_Verification(string UserName, string Password, string Emailaddress)
        {
            bool bStatus = false;
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Login verification started", TraceEventType.Start);
               
                CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
                CustomerID = customerAdaptor.GetCustomerID(UserName, CountrySetting.culture);
                HouseHoldID = customerAdaptor.GetHouseholdID(CustomerID.ToString(), CountrySetting.culture);

                if (SanityConfiguration.Domain == Domain.DBT.ToString() || SanityConfiguration.Domain == Domain.GD.ToString())
                {
                   ObjAutomationHelper.GetControl(ControlKeys.LOGIN_TEXTVERIFICATION).WaitForControlByxPath(Driver);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_CLUBCARD).Id)).SendKeys(UserName);                    
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_PASSWORD).Id)).SendKeys(Password);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_SUBMIT).Id)).Click();
                    CustomLogs.LogInformation("You are currently login with customer Id :" + UserName);
                }
                else if (SanityConfiguration.Domain == Domain.PPE.ToString())
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_EMAIL).Id)).SendKeys(Emailaddress);                    
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_PASSWORD).Id)).SendKeys(Password);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_SIGNIN).Id)).Click();
                    CustomLogs.LogInformation("You are currently login with email " + Emailaddress);
                }
                switch (CountrySetting.country)
                {
                    case "UK":
                    case "SK":
                        ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                        break;
                    case "CZ":
                        //case "TH":
                        //    ObjAutomationHelper.GetControl(ControlKeys.HOME_TITLE).WaitForControlByxPath(Driver);
                        break;
                }
                CustomLogs.LogMessage("Login verification completed", TraceEventType.Stop);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);  
                throw ex;
                              
            }
            return bStatus;
        }

        /// <summary>
        /// To enter the text into the secutiy page
        /// </summary>
        /// <param name="ClubcardId"></param>
        /// <returns></returns>
        public bool SecurityLayer_Verification(String ClubcardId)
        {
            try
            {
                CustomLogs.LogMessage("Verification of security Layer digits started", TraceEventType.Start);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile);
                string isSecurityEnabled=webConfig.Value;
                if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                {
                    //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                    Driver = ObjAutomationHelper.WebDriver;
                    ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).WaitForControlByxPath(Driver);
                    //if (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.TEXT_VERIFICATION).XPath)).WaitForLabel(Driver, "Clubcard Security Verification"))
                    //{
                    int _1stNumber = 0;
                    int _2ndNumber = 0;
                    int _3rdNumber = 0;
                    //Find Position of the security text
                    _1stNumber = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).Text) - 1;
                    _2ndNumber = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).Text) - 1;
                    _3rdNumber = Convert.ToInt32(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).Text) - 1;
                    //Insert Security code 
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[_1stNumber]);
                    Thread.Sleep(500);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[_2ndNumber]);
                    Thread.Sleep(500);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[_3rdNumber]);
                    Thread.Sleep(500);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(60));
                    switch (CountrySetting.country)
                    {
                        case "UK":
                            ObjAutomationHelper.GetControl(ControlKeys.HOME_TITLE).WaitForControlByxPath(Driver);

                            break;
                        //case "CZ":
                        //case "TH":
                        //    ObjAutomationHelper.GetControl(ControlKeys.HOME_TITLE).WaitForControlByxPath(Driver);
                        //    break;
                    }

                    //}
                    CustomLogs.LogMessage("Verification of security Layer digits Completed", TraceEventType.Stop);
                    // Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                }
                else
                {
                    CustomLogs.LogMessage("Security is not applicable", TraceEventType.Stop);
                }
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

        /// <summary>
        /// To Logout of the application
        /// </summary>
        /// <returns></returns>
        public bool LogOut_Verification()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGOUT).Id)).Click();
                //CustomLogs.LogInformation("Test case Passed");
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        public bool Security_LogOut()
        {
            try
            {
                CustomLogs.LogMessage("Logging Out of security Digit Position Page", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_LOGOUT).Id)).Click();
                //CustomLogs.LogInformation("Test case Passed");
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Logging Out of security Digit Position Page", TraceEventType.Stop);
            return true;
        }

        /// <summary>
        /// To verify the join page
        /// </summary>
        /// <returns></returns>
        public bool Join_Verification()
        {
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                IWebElement join = Driver.FindElement(By.Id("ctl00_PageJoinContainer_btnConfirmJoin"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to verify the activation page
        /// </summary>
        /// <returns></returns>
        public bool Activation_Verification()
        {
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                IWebElement Activation = Driver.FindElement(By.Id("ctl00_PageContainer_hdrConfirmClubcardDetails"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

            return true;
        }
         
        #endregion
    }
}
