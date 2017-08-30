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
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;


namespace Tesco.Framework.UITesting
{
    /// <summary>
    /// Sanity cases for UK
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        private ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private Tesco.Framework.UITesting.Entities.Message message = null;
        private string mcaURL = null;
        static string country;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            country = CountrySetting.country;

            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
           // AutomationHelper.GetMessages(ConfigurationManager.AppSettings["MessageXml"]);
            AutomationHelper.GetMessages(ConfigurationManager.AppSettings["DataXML_UK"]);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
            objAutomationHelper = new AutomationHelper();
            //objAutomationHelper.InitializeWebDriver(Consts.INTERNET_EXPLORER.ToString());
            //objCommonMethod = new CommonMethod(objAutomationHelper);


            //objAutomationHelper.InitializeWebDriver(Consts.MOZILLA_FIREFOX.ToString());
          //  objAutomationHelper.InitializeWebDriver(Consts.INTERNET_EXPLORER.ToString());
            driver = objAutomationHelper.WebDriver;

            //System.Environment.SetEnvironmentVariable("webdriver.ie.driver", @"C:\Users\qx25adm\AppData\Local\Microsoft\Windows\Temporary Internet Files\Content.IE5\KBSHOT59\IEDriverServer_Win32_2.37.0[1].zip\IEDriverServer.exe");
           // driver = new InternetExplorerDriver(@"E:\MCA_TESTFRAMEWORK\Tesco.MCA.TestFrameworks\Configuration");
            // driver.Navigate().GoToUrl("http://172.21.179.41/Clubcard/3Ipublish/default.aspx");
        }

       

        //[TestMethod, TestCategory("A2"), TestCategory("Hotel")]
        public void OneWay()
        {
            message = objAutomationHelper.GetMessageByID(Enums.Messages.Login);
         //objCommonMethod.MCALogin(message.Clubcard, message.Password, "");

         //objCommonMethod.SecurityLayerVerification(message.Clubcard, objAutomationHelper);

                   
            // String tripId = "tripTypeText";
            // IWebElement tripIdElement = driver.FindElement(By.ClassName(tripId));
            // tripIdElement.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            AutomationHelper ah = new AutomationHelper();
            string element = ah.GetControl("CommonControls_btnOneWay").ClassName;

            IWebElement ownerElement = driver.FindElement(By.CssSelector(element));
            ownerElement.Click();


            driver.FindElement(By.Id("to")).SendKeys("Delhi");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            Logger("After Wait in one way");
            customLogs.LogInformation("After sendkeys Delhi");
            driver.FindElement(By.Id("to")).SendKeys(Keys.Enter);

            try{
            driver.FindElement(By.Id("searchButton")).Click();
            }
            catch(Exception ex)
            {
                customLogs.LogError(ex);
                Assert.Fail(ex.Message);
            }
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));

            String value = driver.FindElement(By.Id("availabilityLink")).Text;
            Logger(value + "two way");

            //IWebElement searchLink = driver.FindElement(By.CssSelector(ah.GetControl("CommonControls_btnSearchLink").Id));
            //String sl = searchLink.Text;

            //customLogs.LogInformation("Clicked checkInMonth");
            //Assert.AreEqual("sl", "Search", "Search Text not found.");
            //Assert.AreEqual(firstName.GetAttribute("value"), "1");


        }

       
 

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }

        public void Logger(string message)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"d:\WriteLines.txt", true))
            {

                file.WriteLine(message);

            }
        }
    }
}
