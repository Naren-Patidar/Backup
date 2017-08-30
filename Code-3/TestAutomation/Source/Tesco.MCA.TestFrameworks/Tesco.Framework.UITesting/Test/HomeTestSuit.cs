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
using System.Threading;
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
using OpenQA.Selenium.Interactions;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class HomeTestSuit
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;
        MyContactPreference objMyContactPreference = null;
        Home objHome = null;

        private static string beginMessage = "********************* Home ****************************";
        private static string suiteName = "Home";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> homeTestData = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public HomeTestSuit()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.HOMETESTSUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);

            homeTestData.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = homeTestData.TestData;

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        /// <summary>
        /// Test initialization method
        /// </summary>
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
                customLogs.LogInformation(suiteName + " Suite is currently running for country " + culture + " for domain" + SanityConfiguration.Domain);
                objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.MCAUrl);
                driver = objAutomationHelper.WebDriver;
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
            }

            //initialize helper objects
            objLogin = new Login(objAutomationHelper, SanityConfiguration);
            objGeneric = new Generic(objAutomationHelper);
            objHome = new Home(objAutomationHelper);
            objMyContactPreference = new MyContactPreference(objAutomationHelper);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        [TestMethod]
        [Description("To verify Print functionality of Temporary Clubcard on Home Page")]
        [TestCategory("P0_Regression")]
        [TestCategory("OrderAReplacement")]
        [TestCategory("HomePage")]
        public void Home_Verify_Print_TempClubcard()
        {
            string isPresent = objGeneric.VerifyAppSettings(DBConfigKeys.PRINT_TEMPCLUBCARD);
            if (isPresent == "N")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);

                bool isPrintCLubcardClicked = objHome.Homepage_VerfiyPrintClubcard();
                if (isPrintCLubcardClicked)
                {
                    customLogs.LogInformation("Print Temporary CLubcard button clicked");
                    //objLogin.LogOut_Verification();
                }
            }
            else
            {
                Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
            customLogs.LogInformation(endMessage);
            
        }

        

        [TestMethod]
        [Description("Verify Message on home page when customer mailinststus is 3")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_MessageSection1()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objHome.VerifyMessagSectionCMS3();
        }
        [TestMethod]
        [Description("Verify Message on home page when current date is in between cutoff date and sign off date")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_MessageSection2()
        {
            objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objHome.VerifyMessagSectionCDlessthanSignoff();
        }
        [TestMethod]
        [Description("Verify Message on home page when current date is not in range of cutoff date and sign off date")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_MessageSection3()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objHome.VerifyMessagSectionMailingAE();
        }

        [TestMethod]
        [Description("Verify Points summary home page")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_PointsSummarySection()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objHome.VerifyPointsSection();
        }

        [TestMethod]
        [Description("Verify Vouchers summary on home page")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_VoucherSummarySection()
        {
            objLogin.Login_Verification(testData.Clubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.Clubcard);
            objHome.VerifyVoucherSection(testData.Clubcard);
        }
        [TestMethod]
        [Description("Verify Total vouchers spend on home page for christmas saver type customer")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_ChristamasSaverSummarySection()
        {
            objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
           // objHome.verifyChristmasSaverSummary();
            objHome.verifySecondBoxValue(testData.ChristmasSaver, (int)OptionPreference.Xmas_Saver);
        }

        [TestMethod]
        [Description("VerifY  home page for customer opted for Avois standerd")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_AviosSTDSummarySection()
        {
            objLogin.Login_Verification(testData.ClubcardAviosStd, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ClubcardAviosStd);
            objHome.verifySecondBoxValue(testData.ClubcardAviosStd, (int)OptionPreference.Airmiles_Standard);
        }
        [TestMethod]
        [Description("VerifY home page for customer opted for Avois premimum")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_AviosPreSection()
        {
            objLogin.Login_Verification(testData.ClubcardAviosPre, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ClubcardAviosPre);
            objHome.verifySecondBoxValue(testData.ClubcardAviosPre, (int)OptionPreference.Airmiles_Premium);
        }


        [TestMethod]
        [Description("VerifY  home page for customer opted for BA Avois standerd")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_BAAviosSTDSummarySection()
        {
            objLogin.Login_Verification(testData.ClubcardBAAviosStd, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosStd);
            objHome.verifySecondBoxValue(testData.ClubcardBAAviosStd, (int)OptionPreference.BA_Miles_Standard);
        }

        [TestMethod]
        [Description("VerifY home page for customer opted for BA Avois premimum")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_BAAviosPreSection()
        {
            objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
            objHome.verifySecondBoxValue(testData.ClubcardBAAviosPre, (int)OptionPreference.BA_Miles_Premium);
        }
        [TestMethod]
        [Description("VerifY home page for customer opted for  Virgin Atlantic ")]
        [TestCategory("P0")]
        [TestCategory("HomePage")]
        public void Home_VirginAtlanticSection()
        {
            objLogin.Login_Verification(testData.VirginClubcard, testData.Password, "");
            objLogin.SecurityLayer_Verification(testData.VirginClubcard);
            objHome.verifySecondBoxValue(testData.VirginClubcard, (int)OptionPreference.Virgin_Atlantic);
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }
}
