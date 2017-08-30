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
        SmartVoucherAdapter objVoucherService = null;        
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
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Login verification started", TraceEventType.Start);

                CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
                CustomerID = customerAdaptor.GetCustomerID(UserName, CountrySetting.culture);
                HouseHoldID = customerAdaptor.GetHouseholdID(CustomerID.ToString(), CountrySetting.culture);

                if (SanityConfiguration.Domain == Domain.DBT.ToString() || SanityConfiguration.Domain == Domain.GD.ToString())
                {
                    ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_CLUBCARD).WaitForControlByCss(Driver);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_CLUBCARD).Id)).SendKeys(UserName);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_PASSWORD).Id)).SendKeys(Password);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_SUBMIT).Id)).Click();
                    CustomLogs.LogInformation("You are currently login with customer Id :" + UserName);
                }
                else
                {
                    if (CountrySetting.country.Equals("UK"))
                    {
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_EMAIL).Id)).SendKeys(Emailaddress);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_PASSWORD).Id)).SendKeys(Password);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_SIGNIN).Id)).Click();
                    }
                    else
                    {
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_EMAIL).Id)).SendKeys(Emailaddress);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_PASSWORD).Id)).SendKeys(Password);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_SIGNIN).Id)).Click();
                    }
                    CustomLogs.LogInformation("You are currently login with email " + Emailaddress);
                }
                if (CustomerID == 0)
                {
                    Assert.Fail(string.Format("Not able to login with clubcard : '{0}'", UserName));
                }
                CustomLogs.LogMessage("Login verification completed", TraceEventType.Stop);
                Debug.WriteLine(string.Format("{0}|{1}-{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Login Successfull with ", UserName));
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
                ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                {                    
                    ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).WaitForControlByCss(ObjAutomationHelper.WebDriver);
                    Driver = ObjAutomationHelper.WebDriver;

                    {
                        int _1stNumber = 0;
                        int _2ndNumber = 0;
                        int _3rdNumber = 0;
                        //Find Position of the security text
                        //  string encryVal1 = Driver.FindElement(By.Id("firstSecureDigitEncrypt")).GetAttribute("value");
                        string encryVal1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).GetAttribute("value");
                        _1stNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal1)) - 1;

                        string encryVal2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).GetAttribute("value");
                        _2ndNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal2)) - 1;

                        string encryVal3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).GetAttribute("value");
                        _3rdNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal3)) - 1;
                        ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                        //Insert Security code 
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[_1stNumber]);

                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[_2ndNumber]);

                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[_3rdNumber]);

                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
                        ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                    }
                    CustomLogs.LogMessage("Verification of security Layer digits Completed", TraceEventType.Stop);
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
        /// Security verification for every page
        /// </summary>
        /// <param name="ClubcardId"></param>
        /// <returns></returns>
        public bool SecurityLayer_Verification_Pagewise(String ClubcardId, string DBkey)
        {
            try
            {
                ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                DBConfiguration config = AutomationHelper.GetDBConfigurationForSecurity(ConfugurationTypeEnum.SecurityCheck, DBkey, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.ConfigurationValue1;
                if (isPresent == "0")
                {
                    if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                    {
                        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                        ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).WaitForControlByCss(ObjAutomationHelper.WebDriver);
                        Driver = ObjAutomationHelper.WebDriver;

                        {
                            int _1stNumber = 0;
                            int _2ndNumber = 0;
                            int _3rdNumber = 0;
                            //Find Position of the security text

                            string encryVal1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).GetAttribute("value");
                            _1stNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal1)) - 1;

                            string encryVal2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).GetAttribute("value");
                            _2ndNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal2)) - 1;

                            string encryVal3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).GetAttribute("value");
                            _3rdNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal3)) - 1;
                            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                            //Insert Security code 
                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[_1stNumber]);

                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[_2ndNumber]);

                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[_3rdNumber]);

                            Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
                            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                        }
                        CustomLogs.LogMessage("Verification of security Layer digits Completed", TraceEventType.Stop);
                        Debug.WriteLine(string.Format("{0}|{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Security check done"));
                    }

                    else
                    {
                        Assert.Fail("Security Page is not displayed");
                    }
                }
                //else if (isPresent == "0" && (DBkey == "VOUCHERS"))
                //    {
                //        objVoucherService = new SmartVoucherAdapter();
                //        if (objVoucherService.GetUnUsedVoucher(ClubcardId))
                //        {
                //            if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)))
                //            {
                //                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //                ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).WaitForControlByCss(ObjAutomationHelper.WebDriver);
                //                Driver = ObjAutomationHelper.WebDriver;

                //                {
                //                    int _1stNumber = 0;
                //                    int _2ndNumber = 0;
                //                    int _3rdNumber = 0;
                //                    //Find Position of the security text

                //                    string encryVal1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTDIGIT).Id)).GetAttribute("value");
                //                    _1stNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal1)) - 1;

                //                    string encryVal2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDDIGIT).Id)).GetAttribute("value");
                //                    _2ndNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal2)) - 1;

                //                    string encryVal3 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDDIGIT).Id)).GetAttribute("value");
                //                    _3rdNumber = Convert.ToInt32(EncryptionHelper.DecryptTripleDES(encryVal3)) - 1;
                //                    ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                //                    //Insert Security code 
                //                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_FIRSTANSWER).Id)).SendKeys("" + ClubcardId[_1stNumber]);

                //                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_SECONDANSWER).Id)).SendKeys("" + ClubcardId[_2ndNumber]);

                //                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_THIRDANSWER).Id)).SendKeys("" + ClubcardId[_3rdNumber]);

                //                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id)).Click();
                //                    ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                //                }
                //                CustomLogs.LogMessage("Verification of security Layer digits Completed", TraceEventType.Stop);

                //                // Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                //            }

                //        }
                //        else { }
                //    }
                //
                else
                {

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
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MCALOGOUT).Id)).Click();
                if ((SanityConfiguration.Domain == Domain.PPE.ToString() || SanityConfiguration.Domain == Domain.STG.ToString()) && CountrySetting.country.Equals("UK"))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.SIGNOUTCOMPLETELY).Id)).Click();
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DOTCOMLOGOUT).Id)).Click();
                }

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

        public bool loginDetails_Activation(string UserName, string Password, string Emailaddress, string DotcomId)
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
                    ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_CLUBCARD).WaitForControlByCss(Driver);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_CLUBCARD).Id)).SendKeys(UserName);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_PASSWORD).Id)).SendKeys(Password);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_DOTCOMID).Id)).SendKeys(DotcomId);
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DBTLOGIN_SUBMIT).Id)).Click();
                    CustomLogs.LogInformation("You are currently login with customer Id :" + UserName);
                }
                else
                {
                    if (CountrySetting.country.Equals("UK"))
                    {
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_EMAIL).Id)).SendKeys(Emailaddress);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_PASSWORD).Id)).SendKeys(Password);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGINUK_SIGNIN).Id)).Click();
                    }
                    else
                    {
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_EMAIL).Id)).SendKeys(Emailaddress);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_PASSWORD).Id)).SendKeys(Password);
                        Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PPELOGIN_SIGNIN).Id)).Click();
                    }
                    CustomLogs.LogInformation("You are currently login with email " + Emailaddress);
                }
                if (CustomerID == 0)
                {
                    Assert.Fail(string.Format("Not able to login with clubcard : '{0}'", UserName));
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


        public string LoginBrowserCookie(string CookieName)
        {
            string cookie_ID=null;           
            if (SanityConfiguration.Domain == Domain.DBT.ToString() || SanityConfiguration.Domain == Domain.GD.ToString())
            {
                Cookie name = new Cookie(CookieName, "b739c626-5e8c-4a8d-a1fe-19020c389f0c");
                Driver.Manage().Cookies.AddCookie(name);
                cookie_ID = Driver.Manage().Cookies.GetCookieNamed(CookieName).ToString();
            }
            else if (SanityConfiguration.Domain == Domain.PPE.ToString())
            {
                cookie_ID = Driver.Manage().Cookies.GetCookieNamed(CookieName).ToString();
            }
            string[] splitCookie = cookie_ID.Split(';');
            return splitCookie[0];
        }

        public string IsmergedCustomer(string CookieName)
        {
            Driver = ObjAutomationHelper.WebDriver;
            string cookie_ID = null;

            if (SanityConfiguration.Domain == Domain.DBT.ToString() || SanityConfiguration.Domain == Domain.GD.ToString())
            {
                Cookie name = new Cookie(CookieName, "true");
                Driver.Manage().Cookies.AddCookie(name);
                cookie_ID = Driver.Manage().Cookies.GetCookieNamed(CookieName).ToString();
            }
            else if (SanityConfiguration.Domain == Domain.PPE.ToString())
            {
                cookie_ID = Driver.Manage().Cookies.GetCookieNamed(CookieName).ToString();
            }
            string[] splitCookie = cookie_ID.Split(';');
            return splitCookie[0];
        }

        #endregion



        
    }
}
