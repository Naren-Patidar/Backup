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
using System.Xml.Linq;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System.Globalization;
using Tesco.Framework.UITesting.Services.ClubcardService;
using System.Web;
using System.Net;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class MyLatestStatementTestSuite : Base
    {
        public IWebDriver driver;
        ILogger customLogs = null;
        MyLatestStatement ObjMLS = null;
        private AutomationHelper objAutomationHelper = null;
        private List<AutomationHelper> lstAutomationHelper = new List<AutomationHelper>();
        static AppConfiguration SanityConfiguration = new AppConfiguration();
        private Dictionary<string, string> expectedStampName;
        // declare helpers
        Login objLogin = null;
        Generic objGeneric = null;

        private static string beginMessage = "********************* My Latest Statement Suite ****************************";
        private static string suiteName = "My Latest Statement";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_MLS testData = null;
        static TestDataHelper<TestData_MLS> TestDataHelper = new TestDataHelper<TestData_MLS>();
        static string culture;

        public MyLatestStatementTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.MYLATESTSTATEMENTSUITE);
        }

        // Selects the country and load the control and message xml 
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_MLS).Name, SanityConfiguration.Domain);
            testData = TestDataHelper.TestData;
        }

        // Test initialization method
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
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

            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }


        [TestMethod]
        [Description("To Click on My Latest Statement Tab And Verify the Title")]        
        [TestCategory("Sanity")]
        [TestCategory("MVC")]
        [TestCategory("LeftNavigation")]
        public void LeftNavigation_ValidatePageTitle_MLS()
        {            
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.TypeAAccount.Clubcard, testData.TypeAAccount.Password, testData.TypeAAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);
                string homeUrl = objAutomationHelper.WebDriver.Url;
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);                
                string mlsUrl = objAutomationHelper.WebDriver.Url;
                Assert.AreNotEqual(homeUrl, mlsUrl);
                //  objLogin.LogOut_Verification();
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }                
            customLogs.LogInformation(endMessage);
            driver.Quit();
        }

        [TestMethod]
        [Description("To validate the stamp functionality for MLS page")]
        public void StampHomepage_MLS()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isKeyPresent)
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.LATESTSTATEMENT))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.LATESTSTATEMENT)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_LATESTSTATEMENT, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.TypeAAccount.Clubcard, testData.TypeAAccount.Password, testData.TypeAAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.LATESTSTATEMENT);

                        objGeneric.stampClick(ControlKeys.STAMP5, "MLS", StampName.LATESTSTATEMENT);                        
                        objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);
                        objAutomationHelper.WebDriver.Quit();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            } 
        }


        [TestMethod]
        [Description("To validate the details of type A customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeA()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.TypeAAccount.Clubcard, testData.TypeAAccount.Password, testData.TypeAAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.TypeAAccount.Clubcard);
                Thread.Sleep(5000);
                objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                VerifyPoints(testData.TypeAAccount.Clubcard);
                VerifyVoucher(testData.TypeAAccount.Clubcard);
                VerifyCoupon(testData.TypeAAccount.Clubcard);

                string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "A-footer.html");
                string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "A-letter.html");

                VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                //(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                objAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            } 
        }
        [TestMethod]
        [Description("To validate the details of type B customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeB()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.TypeBAccount.Clubcard, testData.TypeBAccount.Password, testData.TypeBAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.TypeBAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.TypeBAccount.Clubcard);
                Thread.Sleep(5000);
                objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                VerifyPoints(testData.TypeBAccount.Clubcard);
                //  VerifyVoucher(testData.ClubcardB);
                VerifyCoupon(testData.TypeBAccount.Clubcard);
                VerifyAvios(testData.TypeBAccount.Clubcard);

                string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "B-footer.html");
                string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "B-letter.html");

                VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                objAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            } 
        }
        [TestMethod]
        [Description("To validate the details of type C customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeC()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.TypeCAccount.Clubcard, testData.TypeCAccount.Password, testData.TypeCAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.TypeCAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.TypeCAccount.Clubcard);
                Thread.Sleep(5000);
                objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                VerifyPoints(testData.TypeCAccount.Clubcard);
                //  VerifyVoucher(testData.ClubcardB);
                VerifyCoupon(testData.TypeCAccount.Clubcard);
                VerifyAvios(testData.TypeCAccount.Clubcard);

                string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "C-footer.html");
                string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "C-letter.html");

                VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);

                objAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            } 
        }
        [TestMethod]
        [Description("To validate the details of type D customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeD()
        {
            bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
            if (isPresent)
            {
                objLogin.Login_Verification(testData.TypeDAccount.Clubcard, testData.TypeDAccount.Password, testData.TypeDAccount.EmailID);
                objLogin.SecurityLayer_Verification(testData.TypeDAccount.Clubcard);
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                objLogin.SecurityLayer_Verification(testData.TypeDAccount.Clubcard);
                Thread.Sleep(5000);
                objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                VerifyPoints(testData.TypeDAccount.Clubcard);
                //  VerifyVoucher(testData.ClubcardD);
                VerifyCoupon(testData.TypeDAccount.Clubcard);
                VerifyAvios(testData.TypeDAccount.Clubcard);

                string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "D-footer.html");
                string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "D-letter.html");

                VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);


                objAutomationHelper.WebDriver.Close();
            }
            else
            {
                Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            } 
        }
        [TestMethod]
        [Description("To validate the details of type E customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeE()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeEAccount.Clubcard, testData.TypeEAccount.Password, testData.TypeEAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeEAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeEAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeEAccount.Clubcard);
                 //  VerifyVoucher(testData.ClubcardA);
                 VerifyCoupon(testData.TypeEAccount.Clubcard);
                 VerifyAvios(testData.TypeEAccount.Clubcard);

                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "E-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "E-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type F customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeF()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeFAccount.Clubcard, testData.TypeFAccount.Password, testData.TypeFAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeFAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeFAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeFAccount.Clubcard);
                 VerifyVoucher(testData.TypeFAccount.Clubcard);
                 VerifyCoupon(testData.TypeFAccount.Clubcard);
                 VerifyTopUP(testData.TypeFAccount.Clubcard);
                 VerifyBONUS(testData.TypeFAccount.Clubcard);

                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "F-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "F-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);

                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type G customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeG()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.ClubcardG.Clubcard, testData.ClubcardG.Password, testData.ClubcardG.EmailID);
                 objLogin.SecurityLayer_Verification(testData.ClubcardG.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.ClubcardG.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.ClubcardG.Clubcard);
                 VerifyVoucher(testData.ClubcardG.Clubcard);
                 VerifyCoupon(testData.ClubcardG.Clubcard);
                 VerifyTopUP(testData.ClubcardG.Clubcard);
                 VerifyBONUS(testData.ClubcardG.Clubcard);

                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "G-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "G-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);

                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type H customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeH()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeHAccount.Clubcard, testData.TypeHAccount.Password, testData.TypeHAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeHAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeHAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeHAccount.Clubcard);
                 VerifyVoucher(testData.TypeHAccount.Clubcard);
                 VerifyCoupon(testData.TypeHAccount.Clubcard);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "H-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "H-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type I customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeI()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeIAccount.Clubcard, testData.TypeIAccount.Password, testData.TypeIAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeIAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeIAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeIAccount.Clubcard);
                 // VerifyVoucher(testData.ClubcardI);
                 VerifyCoupon(testData.TypeIAccount.Clubcard);
                 VerifyVirginAtlantic(testData.TypeIAccount.Clubcard);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "I-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "I-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type J customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeJ()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeJAccount.Clubcard, testData.TypeJAccount.Password, testData.TypeJAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeJAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeJAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 VerifyPoints(testData.TypeJAccount.Clubcard);
                 // VerifyVoucher(testData.ClubcardI);
                 VerifyCoupon(testData.TypeJAccount.Clubcard);
                 VerifyVirginAtlantic(testData.TypeJAccount.Clubcard);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "J-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "J-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type K customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeK()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeKAccount.Clubcard, testData.TypeKAccount.Password, testData.TypeKAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeKAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeKAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeKAccount.Clubcard);
                 VerifyVoucher(testData.TypeKAccount.Clubcard);
                 VerifyCoupon(testData.TypeKAccount.Clubcard);
                 //   VerifyVirginAtlantic(testData.ClubcardI);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "K-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "K-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type L customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeL()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeLAccount.Clubcard, testData.TypeLAccount.Password, testData.TypeLAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeLAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeLAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                 VerifyPoints(testData.TypeLAccount.Clubcard);
                 VerifyVoucher(testData.TypeLAccount.Clubcard);
                 VerifyCoupon(testData.TypeLAccount.Clubcard);
                 //   VerifyVirginAtlantic(testData.ClubcardI);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "L-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "L-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);

                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }
        [TestMethod]
        [Description("To validate the details of type M customer")]
        [TestCategory("MLS")]
        [TestCategory("P2_Regression")]
        public void MyLatestStatement_CustomerTypeM()
        {
             bool isPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDELATESTSTATEMENTPAGE);
             if (isPresent)
             {
                 objLogin.Login_Verification(testData.TypeMAccount.Clubcard, testData.TypeMAccount.Password, testData.TypeMAccount.EmailID);
                 objLogin.SecurityLayer_Verification(testData.TypeMAccount.Clubcard);
                 objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
                 objLogin.SecurityLayer_Verification(testData.TypeMAccount.Clubcard);
                 Thread.Sleep(5000);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTHANKYOUMESSAGE, ControlKeys.MLS_SPNTHANKYOUMESSAGE, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSMYACCOUNTSUMMARY, ControlKeys.MLS_MYACCOUNTSUMMARY, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 objGeneric.verifyValidationMessage(LabelKey.MLSTESCOBANK, ControlKeys.MLS_LBLTESCOBANK, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                 VerifyPoints(testData.TypeMAccount.Clubcard);
                 VerifyVoucher(testData.TypeMAccount.Clubcard);
                 VerifyCoupon(testData.TypeMAccount.Clubcard);
                 //   VerifyVirginAtlantic(testData.ClubcardI);


                 string htmlFilefooter = Path.Combine(SanityConfiguration.HtmlDataDirectory, "M-footer.html");
                 string htmlFileHeader = Path.Combine(SanityConfiguration.HtmlDataDirectory, "M-letter.html");

                 VerifyFooter(htmlFilefooter, ControlKeys.MLS_DIV_FOOTER);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_1);
                 //VerifyHeader(htmlFileHeader, ControlKeys.MLS_DIV_header_2);
                 objAutomationHelper.WebDriver.Close();
             }
             else
             {
                 Assert.Inconclusive(string.Format("MLS Link is not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
             } 
        }

        public void VerifyPoints(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);

                objGeneric.verifyValidationMessage(LabelKey.MLSMYPOINTS, ControlKeys.MLS_POINTSTEXT, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["TotalPoints"];
                var MLSPointsvalue = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYPOINTSVALUE).XPath));
                string MLSPoints = MLSPointsvalue.Text;
                Assert.AreEqual(MLSPoints, value, "My points value is correct");
                objGeneric.ClickElementJavaElement(ControlKeys.MLS_BTNMYPOINTS, "Points");
                var PointsValue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSVALUE).XPath));
                string Points = PointsValue[0].Text;

                Assert.AreEqual(MLSPoints, Points, "My points value is same as MLS points");
                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");


            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyVoucher(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);

                objGeneric.verifyValidationMessage(LabelKey.MLSMYPOINTS, ControlKeys.MLS_POINTSTEXT, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["TotalReward"];


                //  var Vouchervalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYVOUCHERVALUE).XPath));
                var MLSVouchervalue = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYVOUCHERVALUE).XPath));
                string voucher = MLSVouchervalue.Text;
                Assert.AreEqual(voucher, value, "My Voucher value is correct");

                if (value == "0.00")
                {
                    objGeneric.ClickElementJavaElement(ControlKeys.MLS_BTNMYVOUCHERS, "Voucher");
                    objGeneric.verifyPageName(LabelKey.MYVOUCHER, "mycVoucher", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);

                    // Assert.AreEqual(Voucher, voucher, "My Voucher value is same as MLS voucher");
                }
                else
                {

                    objGeneric.ClickElementJavaElement(ControlKeys.MLS_BTNMYVOUCHERS, "Voucher");
                    objGeneric.verifyPageName(LabelKey.MYVOUCHER, "mycVoucher", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    string Voucher = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MYVOUCHER_LBLCURSYMS).XPath)).Text;
                }


                objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");
            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyCoupon(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["NoCoupons"];

                if (value != "0")
                {
                    objGeneric.verifyValidationMessage(LabelKey.MLSMYCOUPONS, ControlKeys.MLS_MYCOUPONLABEL, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    // var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    var MLSCoupon = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    string Coupon = MLSCoupon.Text;
                    Assert.AreEqual(Coupon, value, "My Coupon value is correct");



                    objGeneric.ClickElementJavaElement(ControlKeys.MLS_BTNMYCOUPONS, "Coupon");
                    objGeneric.verifyPageName(LabelKey.MYCOUPONS, "mycoupons", SanityConfiguration.ResourceFiles.LOCAL_RESOURCE);
                    var Couponvalue = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath));
                    string couponvalue = Couponvalue.Text;
                    Assert.AreEqual(couponvalue, Coupon, "My Coupon value is same as MLS");
                    objGeneric.linkNavigate(LabelKey.MYLATESTSTATEMENT, ControlKeys.MYVOUCHER_CLICK, "latest statement");

                }
                else
                {
                    customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
                }
            }



            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyAvios(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["TotalRewardMiles"];

                if (value != "0")
                {
                    objGeneric.verifyValidationMessage(LabelKey.MLSMYAVIOS, ControlKeys.MLS_MYAVIOS, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSPOINTSTOAVIOS, ControlKeys.MLS_LCLPOINTSTOAVIOS, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    // var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    var Avios = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_LTRMYAVIOS).XPath));
                    string Aviospoints = Avios.Text;
                    Assert.AreEqual(Aviospoints, value, "My Avios value is correct");

                }
                else
                {
                    customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
                }
            }



            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyVirginAtlantic(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["TotalRewardMiles"];

                if (value != "0")
                {
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSMYVIRGINATLANTIC, ControlKeys.MLS_MYVIRGINATLANTIC, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSPOINTSTOVIRGINATLANTIC, ControlKeys.MLS_LCLPOINTSTOVIRGINATLANTIC, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    // var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    var Avios = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_LTRMYVIRGINMILES).XPath));
                    string Aviospoints = Avios.Text;
                    Assert.AreEqual(Aviospoints, value, "My VirginAltantic value is correct");

                }
                else
                {
                    customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
                }
            }



            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyBONUS(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["BonusVouchers"];

                if (value != "0")
                {
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSMYBONUS, ControlKeys.MLS_MYBONUS, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSYOURBONUSVOUCHERS, ControlKeys.MLS_LBLBONUSVOUCHERS, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    // var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    var Avios = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_LTRMYBONUS).XPath));
                    string Aviospoints = Avios.Text;
                    Assert.AreEqual(Aviospoints, value, "My Avios value is correct");

                }
                else
                {
                    customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
                }
            }



            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyTopUP(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["TopUpVouchers"];

                if (value != "0")
                {
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSMYTOPUP, ControlKeys.MLS_MYTOPUP, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    objGeneric.VerifyTextonthePageByXpath(LabelKey.MLSYOURTOPUPVOUCHERS, ControlKeys.MLS_LBLYOURTOPUPVOUCHERS, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);
                    // var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                    var TopUp = driver.FindElement(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_LTRMYTOPUP).XPath));
                    string TopUpVoucher = TopUp.Text;
                    Assert.AreEqual(TopUpVoucher, value, "My Top up value is correct");

                }
                else
                {
                    customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
                }
            }



            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyConversion(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);

                objGeneric.verifyValidationMessage(LabelKey.MLSMYPOINTS, ControlKeys.MLS_MYPOINTSLABEL, "latest statement", SanityConfiguration.ResourceFiles.LATESTSTATEMENT_RESOURCE);

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                List<string> expectedTypeofCard = new List<string>();
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                string value = pointsSummaryInfo["RewardMilesRate"];

                if (value == "L")
                {

                    var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));

                    Assert.AreEqual(Couponvalue, value, "My Coupon value is correct");
                }


                else
                {
                    var Couponvalue = driver.FindElements(By.XPath(objAutomationHelper.GetControl(ControlKeys.MLS_MYCOUPONVALUE).XPath));
                }


                customLogs.LogMessage("There is no coupon for this" + " " + clubcardNumber, TraceEventType.Information);
            }




            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyFooter(string htmlFile, string controlKey)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                string content = Encoding.ASCII.GetString(File.ReadAllBytes(htmlFile));
                try
                {
                    if (File.Exists(htmlFile))
                    {
                        string expectedContent = Encoding.UTF8.GetString(File.ReadAllBytes(htmlFile));
                        expectedContent = WebUtility.HtmlDecode(expectedContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Trim();
                        string errorMessage = string.Empty;
                        //  Driver = ObjAutomationHelper.WebDriver;
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                        string actualContent = string.Empty;
                        var element = driver.FindElement(By.XPath(objAutomationHelper.GetControl(controlKey).XPath));
                        // var element= driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Trim();
                        if (expectedContent.Contains(actualContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");

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







            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void VerifyHeader(string htmlFile, string controlKey)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);



                string content = Encoding.ASCII.GetString(File.ReadAllBytes(htmlFile));
                try
                {
                    if (File.Exists(htmlFile))
                    {
                        string expectedContent = Encoding.UTF8.GetString(File.ReadAllBytes(htmlFile));
                        expectedContent = WebUtility.HtmlDecode(expectedContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Trim();
                        string errorMessage = string.Empty;
                        //  Driver = ObjAutomationHelper.WebDriver;
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                        string actualContent = string.Empty;
                        var element = driver.FindElement(By.XPath(objAutomationHelper.GetControl(controlKey).XPath));
                        // var element= driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Trim();
                        if (expectedContent.Contains(actualContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");

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







            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }

    }

}
