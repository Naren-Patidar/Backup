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


        [TestMethod]
        [Description("To Click on My Point Tab And Verify the Title")]
        [TestCategory("Sanity")]

        public void MyPoints_ClickAndVerifyTitle()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS,SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE , ControlKeys.MYPOINTS_CLICK, "points");
                objGeneric.verifyPageNameNew(LabelKey.MYPOINTS,SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE ,"points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
               // objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }


        [TestMethod]
        [Description("To validate the stamp functionality for Points page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void StampHomepage_MyPoints()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.MYPOINTS))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.MYPOINTS)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_MYPOINTS, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.MYPOINTS);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Mypoints", StampName.MYPOINTS);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                    objLogin.SecurityLayer_Verification(testData.Clubcard);
                    objGeneric.verifyPageName(LabelKey.MYPOINTS, "points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("To Click onMyPoints_Stamp_ClickAndVerifyTitle	Tesco.Framework.UITesting	 My Point Tab And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyPoints_Stamp_ClickAndVerifyTitle()
        {
             expectedStampName = objGeneric.isStampPresentbyKey();
             if (expectedStampName.ContainsValue(StampName.MYPOINTS))
             {
                 var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.MYPOINTS)).Key;
                 DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                 string isPresent = config.IsDeleted;
                 if (isPresent == "N")
                 {
                     objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                     objLogin.SecurityLayer_Verification(testData.Clubcard);
                     //objGeneric.ClickStamp(LabelKey.MYPOINTS, ControlKeys.STAMP1, "My Points");
                     objGeneric.stampClick(ControlKeys.STAMP5, "My Points", StampName.MYPOINTS);
                     objGeneric.verifyPageName(LabelKey.MYPOINTS, "points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                     //objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_CurrentDetails_CLICK, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYCURRENTPOINTHEADER, "My Points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                     //driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("HomePage_lnkHomepage").Id)).Click();
                     //objGeneric.stampClick(ControlKeys.STAMP5, "My Points", StampName.MYPOINTS);
                     ////objGeneric.ClickStamp(LabelKey.MYPOINTS, ControlKeys.STAMP1, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYPOINTS, "points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                    // objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK, "My Points");
                     //objGeneric.verifyPageName_contains(Enums.Messages.MyCurrentPointsSummary, "My Points");
                     //driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("HomePage_lnkHomepage").Id)).Click();
                     //objGeneric.ClickStamp(LabelKey.MYPOINTS, ControlKeys.STAMP1, "My Points");
                     //objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_Previous1Details_CLICK, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYCURRENTPOINTHEADER, "My Points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                     //driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("HomePage_lnkHomepage").Id)).Click();
                     //objGeneric.ClickStamp(LabelKey.MYPOINTS, ControlKeys.STAMP1, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYPOINTS, "points", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                     //objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_Previous2Summary_CLICK, "My Points");
                     //objGeneric.verifyPageName_contains(Enums.Messages.MyCurrentPointsSummary, "My Points");

                     //driver.FindElement(By.CssSelector(objAutomationHelper.GetControl("HomePage_lnkHomepage").Id)).Click();
                     //objGeneric.ClickStamp(LabelKey.MYPOINTS, ControlKeys.STAMP1, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYPOINTS, "points", SanityConfiguration.ResourceFiles.POINTS_RESOURCE);
                     //objGeneric.ClickElement(ControlKeys.MYCURRENTPOINTS_Previous2Details_CLICK, "My Points");
                     //objGeneric.verifyPageName(LabelKey.MYCURRENTPOINTHEADER, "My Points", SanityConfiguration.ResourceFiles.POINTS_RESOURCE);
                     //objLogin.LogOut_Verification();
                 }
                 else
                     Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
             }

        }
        
        [TestMethod]
        [Description("To Click on My Current Point Tab And Verify the Title")]
        [TestCategory("Sanity")]
        public void MyCurrentPoints_ClickAndVerifyTitle()
        {
             string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password,testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                //objGeneric.linkNavigate(LabelKey.MYPOINTS, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myCurrentPoint_click();
                objGeneric.verifyPageNameNew(LabelKey.MYCURRENTPOINTHEADER, SanityConfiguration.ResourceFiles.POINTSDETAILSPOINTS_NODE, "points", SanityConfiguration.ResourceFiles.POINTS_RESOURCE);
                //objGeneric.verifyPageName(LabelKey.MYCURRENTPOINTHEADER, "points", SanityConfiguration.ResourceFiles.POINTS_RESOURCE);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_01")]
        [TestCategory("P0_Regression")]
        public void MyPoints_ValidateCollectionPeriodDetails()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);            
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.Validate_CollectionPeriodGrid(Login.CustomerID, CountrySetting.culture);                
                
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the clubcard number is masked in the clubcard transaction page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_02")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateClubcardMasked()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);                
                objPoint.Validate_ValidateClubcardMasked(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        //Validate the Transaction date and Time
        [TestMethod]
        [Description("Validate the Transaction date and Time")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_03")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateTransactionDate()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTransactionDate(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the Transaction details section in clubcard transaction page")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_04")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateTransactionDetails()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTransactionDetails(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate Bonus Points")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_05")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateBonusPoints()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateBonusPoints(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate Total Points")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_06")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateTotalPoints()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTotalPoints(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate Total Spend")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_07")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateAmount()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_ValidateTotalSpend(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page for current-1 collection period")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_08")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateCurrent_1Transactions()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(2);
                objPoint.Validate_PreviousTransactions(Login.CustomerID, CountrySetting.culture, 1);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the Transaction details section in Customer Points page for current-2 collection period")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_09")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateCurrent_2Transactions()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(3);
                objPoint.Validate_PreviousTransactions(Login.CustomerID, CountrySetting.culture, 2);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Search with Main customer clubcard")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_10")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByMainClubcard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchCustomer(Login.CustomerID, CountrySetting.culture, 1);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Search with clubcard id from the clubcard number dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_11")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByAssociateClubcard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchCustomer(Login.CustomerID, CountrySetting.culture, 2);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Search with Store Transaction from the Transaction dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_12")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByStore()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchStore(Login.CustomerID, CountrySetting.culture, 1);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Select the transaction field(store) and Clubcard number from the transaction and card number dropdown")]
        [TestProperty("TestCaseID", "MCA_SCN_UK_010_TC_20")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSearch_ByClubcardAndStore()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SearchStoreAndClubcard(Login.CustomerID, CountrySetting.culture, 1);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the sorting by clubcard at points details page")]        
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSortByCard()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByClubcard(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the sorting by clubcard at points details page")]
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSortByStore()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByStore(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("Validate the sorting by clubcard at points details page")]        
        [TestCategory("P0_Regression")]
        [TestCategory("MyCurrentPoints")]
        public void MyCurrentPoints_ValidateSortByDate()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.Clubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.Clubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.myPoint_DetailsClick(1);
                objPoint.Validate_SortByDate(Login.CustomerID, CountrySetting.culture);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_01,MCA_SCN_UK_004_TC_05)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_UpperPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_02)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_UpperPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_03,MCA_SCN_UK_004_TC_04)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Avios_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_UpperPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_06)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_UpperPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_07)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerLeftPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_08)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Virgin_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerRightPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_13,MCA_SCN_UK_004_TC_15)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Avios_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerLeftPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_14,MCA_SCN_UK_004_TC_16)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_Avios_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerRightPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_17)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerLeftPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_18)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_ChristmasSaver_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerRightPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_09,MCA_SCN_UK_004_TC_11)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerLeftPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_10,MCA_SCN_UK_004_TC_12)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_LowerRightPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_10,MCA_SCN_UK_004_TC_12)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerRightPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Tesco Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_19)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_TescoPointsSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_StandardCustomer_TescoPointsSection(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Tesco Bank Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_20)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusOne_StandardCustomer_TescoBankPointsSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusOne_click();
                objPoint.PointSummary_CurrentMinusOne_VerifyData_StandardCustomer_TescoBankPointsSection(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_01,MCA_SCN_UK_004_TC_05)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_UpperPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_02)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_UpperPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_03,MCA_SCN_UK_004_TC_04)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_UpperPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify upper part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_06)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_UpperPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_UpperPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_07)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerLeftPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Virgin Customer(MCA_SCN_UK_004_TC_08)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Virgin_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.VirginClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.VirginClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerRightPart(testData.VirginClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_13,MCA_SCN_UK_004_TC_15)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerLeftPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Airmiles/BAMiles Customer(MCA_SCN_UK_004_TC_14,MCA_SCN_UK_004_TC_16)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_Avios_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ClubcardBAAviosPre, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ClubcardBAAviosPre);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerRightPart(testData.ClubcardBAAviosPre);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_17)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerLeftPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower right part of Points summary Page for Christmas Saver Customer(MCA_SCN_UK_004_TC_18)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_ChristmasSaver_LowerRightPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.ChristmasSaver, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.ChristmasSaver);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerRightPart(testData.ChristmasSaver);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify lower left part of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_09,MCA_SCN_UK_004_TC_11)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_LowerLeftPart()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_LowerLeftPart(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Tesco Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_19)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_TescoPointsSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_StandardCustomer_TescoPointsSection(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }

        [TestMethod]
        [Description("To verify Tesco Bank Points Section of Points summary Page for Reward/Non Reward Customer(MCA_SCN_UK_004_TC_20)")]
        [TestCategory("P0_Regression")]
        [TestCategory("PointsSummary")]
        public void PointsSummaryView_CurrentMinusTwo_StandardCustomer_TescoBankPointsSection()
        {
            string isPresent = objGeneric.verifyPageEnabled(DBConfigKeys.HIDEPOINTSSUMMARYPAGE);
            if (isPresent == "Y")
            {
                objLogin.Login_Verification(testData.StandardClubcard, testData.Password, testData.EmailID);
                objLogin.SecurityLayer_Verification(testData.StandardClubcard);
                objGeneric.linkNavigateNew(LabelKey.MYPOINTS, SanityConfiguration.ResourceFiles.MYACCOUNTlOCAL_NODE, ControlKeys.MYPOINTS_CLICK, "points");
                objPoint.PointSummary_CurrentMinusTwo_click();
                objPoint.PointSummary_CurrentMinusTwo_VerifyData_StandardCustomer_TescoBankPointsSection(testData.StandardClubcard);
                //objLogin.LogOut_Verification();
            }
            else
                Assert.AreEqual(isPresent, "N", "Configuration Value not matched with DBConfig");
            customLogs.LogInformation(endMessage);
        }
        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }
}
