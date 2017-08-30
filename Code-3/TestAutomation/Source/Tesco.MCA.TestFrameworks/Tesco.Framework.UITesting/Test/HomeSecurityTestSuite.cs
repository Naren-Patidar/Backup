using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using System.IO;

using System.Xml.Serialization;
using System.Configuration;
using System.Collections.ObjectModel;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.CustomException;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;
using System.Threading.Tasks;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.UITesting.Helpers;
using System.Diagnostics;



namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class HomeSecurityTestSuite
    {
        public IWebDriver driver;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        protected List<string> lst = new List<string>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        ILogger customLogs = null;
        static private string isSecurityPageEnabled = String.Empty;
        string clubcard = string.Empty;

        // declare helpers
        Home objHome = null;
        Login objLogin = null;        
        HomeSecurity objHomeSecurity = null;
        Generic objGeneric = null;

        static string culture;

        public static List<string> Cultures = new List<string>();
   
        static TestData_AccountDetails testData = null;
        static TestData_HomeSecurity testData_Home = null;             
       
        static TestDataHelper<TestData_AccountDetails> ADTestData = new TestDataHelper<TestData_AccountDetails>();
        static TestDataHelper<TestData_HomeSecurity> ADTestData_home = new TestDataHelper<TestData_HomeSecurity>();

        private static string beginMessage = "********************* Home Security ****************************";
        private static string suiteName = "Home Security";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        public HomeSecurityTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
        }

        /// Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            ADTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = ADTestData.TestData;

            ADTestData_home.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_HomeSecurity).Name, SanityConfiguration.Domain);
            testData_Home = ADTestData_home.TestData;

            //isSecurityPageEnabled = (AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile)).Value;
        }

        // Test initialization method
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            if (SanityConfiguration.RunAllBrowsers)
            {
                List<string> browsers = Enum.GetNames(typeof(Browser)).ToList();
                foreach (string browser in browsers)
                {
                    objAutomationHelper = new AutomationHelper();
                    objAutomationHelper.InitializeWebDriver(browser, SanityConfiguration.MCAUrl);
                    lstAutomationHelper.Add(objAutomationHelper);
                }
            }
            else
            {
                customLogs.LogInformation(beginMessage);
                customLogs.LogInformation(suiteName + "Suite is currently running for country " + culture);
                objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                driver = objAutomationHelper.WebDriver;
            }

            switch (SanityConfiguration.DefaultBrowser)
            {
                case Browser.IE:
                    if (driver.Title == "Certificate Error: Navigation Blocked")
                        objAutomationHelper.WebDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
                    break;
                case Browser.GC:
                    break;
                case Browser.MF:
                    break;
            }
            //initialize helper objects
            objLogin = new Login(objAutomationHelper, SanityConfiguration);
            objHomeSecurity = new HomeSecurity(objAutomationHelper, SanityConfiguration, testData_Home);
            objHome = new Home(objAutomationHelper);
            objGeneric = new Generic(objAutomationHelper);

            //populating the list with clubcard numbers
            lst.Add(testData.Clubcard);
            lst.Add(testData.Clubcard1);
            lst.Add(testData.Clubcard2);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_01")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_VerifyPageIsLoadedWithProperErrorMsg()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objHomeSecurity.VerifyDefaultMsg(ControlKeys.SECURITY_DEFAULTMESSAGE);
                objHomeSecurity.VerifySecurityDigitPosition();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_02")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_ErrorMessageVerification_Space()
        {
            try
            {
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                for (int i = 0; i <= lst.Count; i++)
                {
                    objLogin.Login_Verification(lst[i], testData.Password, testData.EmailID);
                    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
                    //    break;
                    //else
                    //    i++;
                    break;
                }              
                    objHomeSecurity.CaptureSecurityDigitsPosition();
                    objHomeSecurity.InsertSecurityDigitsPosition_Space();
                    objHomeSecurity.CaptureInvalidSpaceErrorMessage();   
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_03")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_ErrorMessageVerification_InvalidCharacters()
        {
            try
            {
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                for (int i = 0; i <= lst.Count; i++)
                {
                    objLogin.Login_Verification(lst[i], testData.Password, testData.EmailID);
                    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
                    //    break;
                    //else
                    //    i++;
                    break;
                }
                objHomeSecurity.CaptureSecurityDigitsPosition();
                objHomeSecurity.InsertSecurityDigitsPosition_InvalidCharacters();
                objHomeSecurity.CaptureInvalidErrorMessage();              
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());   
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_04")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void ErrorMessageVerification_randomNo()
        {
            try 
            {
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                for (int i = 0; i <= lst.Count; i++)
                {
                    objLogin.Login_Verification(lst[i], testData.Password, testData.EmailID);
                    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
                    //    break;
                    //else
                    //    i++;
                    break;
                }
                    objHomeSecurity.CaptureSecurityDigitsPosition();
                    objHomeSecurity.InsertSecurityDigitsPosition_random();
                    objHomeSecurity.CaptureInvalidNoErrorMessage();
                    objHomeSecurity.InsertSecurityDigitsPosition_InvalidCharacters();
                    objHomeSecurity.CaptureInvalidCharacterErrorMessage();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_06")]
        [TestCategory("P0")]
        [Owner("Infy")]
        public void HomeSecurity_SuccessfullSecurityVerification()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objHomeSecurity.CaptureSecurityDigitsPosition();
                    objHomeSecurity.InsertSecurityDigitsPosition(clubcard);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_07")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_VerifyErrorMsgForClubcardNoNotInRange()
        {
            try
            {
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                objHomeSecurity.CheckValueFromConfigurationType();
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                //for (int i = 0; i <= lst.Count; i++)
                //{
                //    objLogin.Login_Verification(lst[i], testData.Password, "");
                //    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
                //    //    break;
                //    //else
                //    //    i++;
                //    break;
                //}
                objHomeSecurity.VerifyIsSecurityPageEnabled();
             
                    objHomeSecurity.CaptureSecurityDigitsPosition();
                    objHomeSecurity.InsertWrongSecurityDigitsPosition(clubcard);
                    objHomeSecurity.CaptureInvalidNoErrorMessage();  
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());               
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_013_TC_11")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_VerifySecurityPageSwitchOff()
        {
            try
            {
                objHomeSecurity.VerifyIsSecurityPageEnabled();
                for (int i = 0; i <= lst.Count; i++)
                {
                    objLogin.Login_Verification(lst[i], testData.Password, testData.EmailID);
                    //if (!objHomeSecurity.Verify_MaxAttemptsBlockedClubcard())
                    //    break;
                    //else
                    //    i++;
                    break;
                }
                objHome.Homepage_Verification();                                  
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                customLogs.LogError(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString()); 
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Infy")]
        public void HomeSecurity_ExceedingMaxAttempts()
        {

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            try
            {
                objLogin.Login_Verification(testData.BlockedClubcard, testData.Password, testData.EmailID);
                objHomeSecurity.VerifyIsSecurityPageEnabled();

                objHomeSecurity.Verify_SecurityMaxAttempts();
                objLogin.Security_LogOut();
                objLogin.Login_Verification(testData.BlockedClubcard, testData.Password, testData.EmailID);

                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.ENABLESECURITYLAYERONHOME, SanityConfiguration.WebConfigurationFile);
                string isSecurityEnabled = webConfig.Value;
                if (isSecurityEnabled == "false")
                {
                    objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                    objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                    objHomeSecurity.Verify_MaxAttemptsBlockedClubcard();
                    //objLogin.Security_LogOut();

                }
                else
                    objHomeSecurity.Verify_MaxAttemptsBlockedClubcard();
                objLogin.Security_LogOut();
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);

           
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }


        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_01")]
        [TestCategory("P0")]
        [Owner("Tesco")]
        public void VoucherSecurity_VerifySecurityForUnusedVouchers()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYVOUCHER, "vouchers", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_005_TC_06, MCA_SCN_UK_005_TC_07")]
        [TestCategory("P0")]
        [Owner("Tesco")]
        public void VoucherSecurity_VerifySecurityForUsedorNoVouchers()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");

                objGeneric.verifyPageName(LabelKey.MYVOUCHER, "vouchers", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_15")]
        [TestCategory("P0")]
        [Owner("Tesco")]
        public void VoucherSecurity_NavigateFromVoucherToPersonalDetailsAndVerifySecurity()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_13")]
        [TestCategory("P0")]
        [Owner("Tesco")]
        public void VoucherSecurity_SubmitSecurityOnVoucherAndNavigateToPersonalDetails()
        {
            try
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objLogin.SecurityLayer_Verification(testData.Clubcard);

                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                
                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("MCA_SCN_UK_002_TC_14")]
        [TestCategory("P1")]
        [TestCategory("P1_Regression")]
        [Owner("Tesco")]
        public void VoucherSecurity_SubmitWrongSecurityOnVoucherAndNavigateToPersonalDetails()
        {
            try
            {
                objHomeSecurity.CheckValueFromConfigurationType();
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYVOUCHER, ControlKeys.MYVOUCHER_CLICK, "vouchers");
                objHomeSecurity.CaptureSecurityDigitsPosition();
                objHomeSecurity.InsertWrongSecurityDigitsPosition(testData.Clubcard);
                
                objGeneric.linkNavigate(LabelKey.MYACCOUNT, ControlKeys.ACCOUNT_CLICK, "My Account");
                objGeneric.linkNavigate(LabelKey.MYPERSONALDETAILS, ControlKeys.LINK_CLICK, "personaldetails");
                objLogin.SecurityLayer_Verification(testData.Clubcard);

                objGeneric.verifyPageName(LabelKey.MYPERSONALDETAILS, "personaldetails", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(driver, ex);
                driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            customLogs.LogInformation(endMessage);
        }


        //[TestMethod]
        //[Description("")]
        //[TestCategory("Regression_home")]
        //[Owner("Infy")]
        //public void HomeSecurity_CheckIfReloginFailsWithin1hr()
        //{
        //    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
        //    objLogin.Login_Verification(testData.BlockedClubcard, testData.Password, "");
        //    objHomeSecurity.Verify_MaxAttemptsBlockedClubcard();
        //    objLogin.Security_LogOut();
        //    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        //}



        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }
    }
}
