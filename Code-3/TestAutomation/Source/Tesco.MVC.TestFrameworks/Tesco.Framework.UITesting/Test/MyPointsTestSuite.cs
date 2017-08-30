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
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Logging.Logger;
using System.Threading.Tasks;
using Tesco.Framework.UITesting.Test.Common;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.UITesting.Enums;
using System.Threading;
using System.Diagnostics;



namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class MyPointsTestSuite
    {
        public IWebDriver driver;
        private ILogger customLogs = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        static string error = string.Empty;
        // declare helpers
        Login objLogin = null;
        MyPoints objPoint = null;
        Generic objGeneric = null;

        private static string beginMessage = "********************* MyPoints Test Suite ****************************";
        private static string suiteName = "My Points";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_AccountDetails testData = null;
        static TestDataHelper<TestData_AccountDetails> TestDataHelper = new TestDataHelper<TestData_AccountDetails>();
        static string culture;

        public MyPointsTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.MYPOINTTESTSUITE);
        }

        // Selects the country and load the control and message xml
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_AccountDetails).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
        }

        // Test initialization method
        [TestInitialize]
        public void TestInitialize()
        {
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
            objPoint = new MyPoints(objAutomationHelper, SanityConfiguration);
        }

        
        #region Sanity

        [TestMethod]
        [Description("To Click on My Point link And Verify the Title")]
        [TestCategory("Sanity")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_Points()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYPOINTS_LINK, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_verifyPageName();
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        #endregion

        #region Points Home Page Test Cases
        [TestMethod]
        [Description("To verify if vouchers box is hidden")]
        [TestCategory("P0_Regression")]
        [TestCategory("Points")]
        public void Points_Home_ViewVouchersBox()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            //DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ENABLEHOMEPAGE, SanityConfiguration.DbConfigurationFile);
            //string isSecurityEnable = config.ConfigurationValue1;
            //if (isSecurityEnable == "FALSE")
            if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
            {
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                if (Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYPOINTS_ViewVouchers).Id), driver))
                {
                    objGeneric.ClickElement(ControlKeys.MYPOINTS_ViewVouchers, "Points Page");
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
                    {
                        Assert.Fail("Home security page is not displayed");
                    }
                    else
                    {
                        objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.POINTS);

                    }
                }
                else
                {
                    Assert.Fail("Security Page is enabled on home");
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_01")]
        [TestCategory("P0")]
        [TestCategory("P0Set1")]
        public void MyPoints_ValidateCollectionPeriodDetails()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.Validate_CollectionPeriodGrid(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify if vouchers box is hidden")]
        [TestCategory("P0_Regression")]
        [TestCategory("Points")]
        public void Points_Summary_ViewVouchersBox()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);

            if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
            {
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                if (!(Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYPOINTS_ViewVouchers).ClassName), driver)))
                {
                    Assert.Fail("ViewVouchers Links not is present");
                }
                else
                {
                    // objGeneric.ClickElement(ControlKeys.MYPOINTS_ViewVouchers, "Points");
                    objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK, "Points");
                    if (!(Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYPOINTS_ViewVouchersPointsSummary).ClassName), driver)))
                    {
                        Assert.Fail("View Balance link on view summary page is not present on Points summary page");
                    }

                    else
                    {
                        objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.POINTS);

                    }

                }
            }

            else
            {
                Assert.Fail("Security Page is enabled on home");
            }
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));

        }

        [TestMethod]
        [Description("To verify if vouchers box is hidden")]
        [TestCategory("P0_Regression")]
        [TestCategory("Points")]
        public void Points_ViewVouchersBox2()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
            objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
            if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
            {
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                if (!(Generic.IsElementPresent(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.MYPOINTS_ViewVouchers).ClassName), driver)))
                {
                    Assert.Fail("ViewVouchers Links not is present");
                }
                else
                {
                    // objGeneric.ClickElement(ControlKeys.MYPOINTS_ViewVouchers, "Points");
                    objGeneric.ClickElement(ControlKeys.MYPOINTS_ViewVouchers, FindBy.CSS_SELECTOR_CSS);
                    if (!(objGeneric.IsElementPresentOnPage(By.CssSelector(objAutomationHelper.GetControl(ControlKeys.SECURITY_BUTTON).Id))))
                    {
                        Assert.Fail("Security page is not displayed");
                    }
                    else
                    {
                        objLogin.SecurityLayer_Verification_Pagewise(testData.MainAccount.Clubcard, DBConfigKeys.POINTS);
                        objPoint.Validate_ValidateTotalPoints(Login.CustomerID, CountrySetting.culture);
                    }

                }
            }

            else
            {
                Assert.Fail("Security Page is enabled on home");
            }
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));

        }

        [TestMethod]
        [Description("Validates the text for Points Home Page ")]
        [TestCategory("P0_Regression")]
        [TestCategory("P2")]
        [TestCategory("Points")]
        public void PointsHome_ValidateText()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS_LINK, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                Tuple<string, string> DatesData = objPoint.GetCurrentCollectionPeriodData(Login.CustomerID, CountrySetting.culture);
                if (objPoint.ValidatepointshomeText(DatesData))
                    customLogs.LogInformation("Text Validation is completed");
                else
                    Assert.Fail("Text validation is not correct");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("Validates the Points Value on Points Home Page ")]
        [TestCategory("P0_Regression")]
        [TestCategory("Points")]
        public void PointsHome_ValidateCurrentPoints()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS_LINK, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                bool errorinPoints = objPoint.Validatepoints(Login.CustomerID, CountrySetting.culture);
                if (!errorinPoints)
                    customLogs.LogInformation("Points Value Validation is completed");
                else
                    Assert.Fail("Points Value validation failed");
            }
            else
                Assert.AreEqual(isPresent, false, "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
        #endregion

        #region Points Detail Test Cases
        [TestMethod]
        [Description("Validate the clubcard number is masked in the clubcard transaction page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_02")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateClubcardMasked()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS,  ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateClubcardMasked(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        //Validate the Transaction date and Time
        [TestMethod]
        [Description("Validate the Transaction date and Time")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_03")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateTransactionDate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTransactionDate(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate the Transaction details section in clubcard transaction page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_04")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateTransactionDetails()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTransactionDetails(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate Bonus Points")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_05")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateBonusPoints()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateBonusPoints(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate Total Points")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_06")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateTotalPoints()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTotalPoints(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate Total Spend")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_07")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateAmount()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTotalSpend(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page for current-1 collection period")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_08")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateCurrent_1Transactions()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS,ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(2);
                objPoint.Validate_PreviousTransactions(Login.CustomerID, CountrySetting.culture, 1);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page for current-2 collection period")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_09")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateCurrent_2Transactions()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(3);
                objPoint.Validate_PreviousTransactions(Login.CustomerID, CountrySetting.culture, 2);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Search with Main customer clubcard")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_10")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateSearch_ByMainClubcard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchCustomer(Login.CustomerID, CountrySetting.culture, 1);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Search with clubcard id from the clubcard number dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_11")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByAssociateClubcard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchCustomer(Login.CustomerID, CountrySetting.culture, 1);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Search with Store Transaction from the Transaction dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_12")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateSearch_ByStore()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchStore(Login.CustomerID, CountrySetting.culture, 1);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("Select the transaction field(store) and Clubcard number from the transaction and card number dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_20")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByClubcardAndStore()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchStoreAndClubcard(Login.CustomerID, CountrySetting.culture, 1);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        // [TestMethod] Remove sorting functionality from points detail column-CCMCA-5693
        [Description("Validate the sorting by clubcard at points details page")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateSortByCard()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByClubcard(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        // [TestMethod] Remove sorting functionality from points detail column-CCMCA-5693
        [Description("Validate the sorting by clubcard at points details page")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateSortByStore()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByStore(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        //[TestMethod] Remove sorting functionality from points detail column-CCMCA-5693
        [Description("Validate the sorting by clubcard at points details page")]
        [TestCategory("P0")]
        [TestCategory("MyCurrentPoints")]
        [TestCategory("P0Set1")]
        public void MyCurrentPoints_ValidateSortByDate()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.MainAccount.Clubcard, testData.MainAccount.Password, testData.MainAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByDate(Login.CustomerID, CountrySetting.culture);
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        #endregion

        #region Points Summary Test Cases
        [TestMethod]
        [Description("To verify upper part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_01,MCA_SCN_UK_004_TC_05) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]        
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_FirstTable()
        {

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.StandardAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_02)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.ChristmasSaverAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_03,MCA_SCN_UK_004_TC_04)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusOne_Avios_FirstTable()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);                
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.BAAviosPreAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_06) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("P0Set6")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.VirginAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_07) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.VirginAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_08) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.VirginAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_13,MCA_SCN_UK_004_TC_15)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusOne_Avios_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.BAAviosPreAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_14,MCA_SCN_UK_004_TC_16)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusOne_Avios_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.BAAviosPreAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_17)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.ChristmasSaverAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_18)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.ChristmasSaverAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_09,MCA_SCN_UK_004_TC_11) usin StandardClubcard")]
        [TestCategory("P0")]        
        [TestCategory("P0Regression")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.StandardAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_10,MCA_SCN_UK_004_TC_12) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.StandardAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_10,MCA_SCN_UK_004_TC_12) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.StandardAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify Tesco Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_19) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("P0_MyPoints")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0_PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_TescoPointsSection()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.ValidateTescoPointsTable(testData.StandardAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify Tesco Bank Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_20) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_TescoBankPointsSection()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusOne_click();
                string error = objPoint.ValidateTescoBankPointsTable(testData.StandardAccount.Clubcard, 1);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_01,MCA_SCN_UK_004_TC_05) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("P0_MyPoints")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");                
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.StandardAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_02)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.ChristmasSaverAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_03,MCA_SCN_UK_004_TC_04)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.BAAviosPreAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_06) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set7")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_FirstTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable1(testData.VirginAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_07) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.VirginAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_08) using VirginClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.VirginAccount.Clubcard, testData.VirginAccount.Password, testData.VirginAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.VirginAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.VirginAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_13,MCA_SCN_UK_004_TC_15)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.BAAviosPreAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_14,MCA_SCN_UK_004_TC_16)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set5")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.BAAviosPreAccount.Clubcard, testData.BAAviosPreAccount.Password, testData.BAAviosPreAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.BAAviosPreAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.BAAviosPreAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_17)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.ChristmasSaverAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_18)")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set4")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_ThirdTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.ChristmasSaverAccount.Clubcard, testData.ChristmasSaverAccount.Password, testData.ChristmasSaverAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.ChristmasSaverAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable3(testData.ChristmasSaverAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_09,MCA_SCN_UK_004_TC_11) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_SecondTable()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.VerifyPointsSummaryTable2(testData.StandardAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify Tesco Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_19) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_TescoPointsSection()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.ValidateTescoPointsTable(testData.StandardAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To verify Tesco Bank Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_20) using StandardClubcard")]
        [TestCategory("P0")]
        [TestCategory("PointsSummary")]
        [TestCategory("P0Set6")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_TescoBankPointsSection()
        {
            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
            bool isPresent = config.ConfigurationValue1.Equals("0");
            if (isPresent)
            {
                objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                objPoint.PointSummary_CurrentMinusTwo_click();
                string error = objPoint.ValidateTescoBankPointsTable(testData.StandardAccount.Clubcard, 2);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }  
            }
            else
            {
                Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
         [Description("To verify Point Summary Text")]
         [TestCategory("P2")]
         [TestCategory("PointsSummary")]
         public void PointsSummaryView_Text()
         {
             DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSUMMARYPAGE, SanityConfiguration.DbConfigurationFile);
             bool isPresent = config.ConfigurationValue1.Equals("0");
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.StandardAccount.Clubcard, testData.StandardAccount.Password, testData.StandardAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                 objLogin.SecurityLayer_Verification(testData.StandardAccount.Clubcard);
                 objPoint.PointSummary_CurrentMinusOne_click();
                 string error = objPoint.ValidatePointSummaryText(testData.StandardAccount.Clubcard, 1);
                 if (!string.IsNullOrEmpty(error))
                 {
                     Assert.Fail(error);
                 }
             }
             else
             {
                 Assert.Inconclusive(string.Format("Points Summary is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             }
             customLogs.LogInformation(endMessage);
             driver.Quit();
         }
        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }
}
