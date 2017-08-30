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
using Tesco.Framework.UITesting.Entities;
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

namespace Tesco.Framework.UITesting.MCA.CommonMethods
{
    /// <summary>
    /// Summary description for CommonMethod
    /// </summary>
    [TestClass]
    public  class CommonMethod
    {

         IWebDriver driver;
         ILogger customLogs = null;
       AutomationHelper objAutomationHelper = null;
        private Tesco.Framework.UITesting.Entities.Message message = null;
       // private string mcaURL = null;
        static string country;
        public static string txtUserName = string.Empty;
        public static string txtEmailAddress = string.Empty;
        public static string Emailaddress = string.Empty;
        public static string txtDotcomPassword = string.Empty;
        //public static string txtUserID = string.Empty;
        public static string txtPassword = string.Empty;
        public static string btnLogin = string.Empty;
        public static string lnkCoupon = string.Empty;
        public static string imgThumbnail = string.Empty;
        public static string DivActiveCoupons = string.Empty;
        public static string Logout = string.Empty;
        public static string overridelink = string.Empty;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            country = CountrySetting.country;

            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            AutomationHelper.GetMessages(ConfigurationManager.AppSettings["MessageXml"]);
        }

        [TestInitialize]
        public void TestInitialize()
        {
          

            //System.Environment.SetEnvironmentVariable("webdriver.ie.driver", @"C:\Users\qx25adm\AppData\Local\Microsoft\Windows\Temporary Internet Files\Content.IE5\KBSHOT59\IEDriverServer_Win32_2.37.0[1].zip\IEDriverServer.exe");
           // driver = new InternetExplorerDriver(@"E:\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Configuration");
            // driver.Navigate().GoToUrl("http://172.21.179.41/Clubcard/3Ipublish/default.aspx");
        }

        
        public  bool MCALogin( string UserName, string Password,string Emailaddress)
        {
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
            objAutomationHelper = new AutomationHelper();




            objAutomationHelper.InitializeWebDriver(Consts.INTERNET_EXPLORER.ToString());
            driver = objAutomationHelper.WebDriver;
           // customLogs.LogInformation("Used Clubcard for Login is " + UserName);
            IWebElement certi = driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("LoginControls_btnCerti").Id));
            if (certi.Enabled)
                certi.Click();
            
            bool bStatus = false;
            
            string domainFromXml = Utilities.EnvironmentSettings["Domain"];
            if (domainFromXml == Domain.DBT.ToString())
            {
               // driver.FindElement(By.Id("ctl00_PageContainer_txtCardNo")).SendKeys(UserName);
                driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("LoginControls_btnMCAloginDbt").Id)).SendKeys(UserName);
                 driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("LoginControls_btnMCAPassDbt").Id)).SendKeys(Password);
                 driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("LoginControls_btnSubmit").Id)).Click();

               // objAutomationHelper.InitializeWebDriver(Consts.INTERNET_EXPLORER.ToString());
                
              

                
              driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            }


            
            else
            {
            
              /*  CUITe_BrowserWindow MCALogin = new CUITe_BrowserWindow("Clubcard - Tesco.com");
                CUITe_BrowserWindow MCAHome = new CUITe_BrowserWindow("Clubcard - Tesco.com");
                CUITe_BrowserWindow errorPage = new CUITe_BrowserWindow(WindowNameConstants.certificateErrorNavigationBlockedConfigWindow);


                CUITe_BrowserWindow.Launch(sURL);
                if (!MCAHome.Maximized)
                    MCAHome.Maximized = true;
                if (!MCALogin.Maximized)
                    MCALogin.Maximized = true;
                if (!errorPage.Maximized)
                    errorPage.Maximized = true;

                if (Clubcard_UI_Validation.Reusables.IsWindowPresent(WindowNameConstants.certificateErrorNavigationBlockedConfigWindow))
                    errorPage.Get<CUITe_HtmlHyperlink>("Id=overridelink").Click();

                MCALogin.Get<CUITe_HtmlEdit>(txtUserName).SetText(UserName);
                MCALogin.Get<CUITe_HtmlEdit>(txtPassword).SetText(Password);
                MCALogin.Get<CUITe_HtmlInputButton>(btnLogin).Click();

                Thread.Sleep(2000);
                if (MCAHome.Get<CUITe_HtmlHyperlink>(lnkCoupon).Exists)
                {
                    bStatus = true;
                }
                else
                {
                    bStatus = false;
                }}

           */ 
            //if (Reusables.IsWindowPresent(WindowNameConstants.securityVerificationWindow))
               // Utility.SecurityLayerVerification(UserName);

            //Utility.ExpandMyAccountDetails();
            }
            return bStatus;
        }

      

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

       
    }
}
