using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using Tesco.Framework.UITesting.Services;
using System.Threading;


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class ChristmasSaverTestSuite
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
        ChristmasSaver objChristmasSaver = null;

        private static string beginMessage = "********************* Christmas saver suite ****************************";
        private static string suiteName = "Christmas saver";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_XmusSaver testData = null;
        static TestDataHelper<TestData_XmusSaver> TestDataHelper = new TestDataHelper<TestData_XmusSaver>();
        static string culture;

        DateTime strXmasCurrStartDate;
        DateTime strXmasCurrEndDate;
        DateTime strXmasNextStartDate;
        DateTime strXmasNextEndDate;

        CustomerServiceAdaptor objCustomerService = null;
        ClubcardServiceAdapter objClubcardService = null;
        SmartVoucherAdapter objSmartVoucherService = null;
        public ChristmasSaverTestSuite()
        {
            objAutomationHelper = new AutomationHelper();
            Utilities.InitializeLogger(ref customLogs, AppenderType.CHRISTMASSAVERSUITE);
            
        }

        // Selects the country and load the control and message xml 
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            culture = CountrySetting.country;
            AutomationHelper.GetCategoryControls(ConfigurationManager.AppSettings["ControlsXML"]);
            TestDataHelper.LoadData(SanityConfiguration.TestDataFile, typeof(TestData_XmusSaver).Name, SanityConfiguration.Domain);
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
            objChristmasSaver = new ChristmasSaver(objAutomationHelper);
            objCustomerService = new CustomerServiceAdaptor();
            objClubcardService = new ClubcardServiceAdapter();
            objSmartVoucherService = new SmartVoucherAdapter();
            SetXmusSaverDatesToGlobalVariables();
        }
        
        [TestMethod]
        [Description("To validate the stamp functionality for Christmas Saver page")]
        public void StampHomepage_ChristmasSaver()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                SetXmusSaverDatesToGlobalVariables();
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);
                        // objGeneric.ElementMouseOver(Control.Keys.STAMP5);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);


                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);

                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);
                        //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                        //   objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);
                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        objAutomationHelper.WebDriver.Quit();

                    }
                    else
                    {
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }

        }


        [TestMethod]
        [Description("To Click on Christmas Saver Stamp And Verify the Title")]        
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0Set2")]

        public void ChristmasSaverStamp_ClickAndVerifyTitle()
        {            
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();
                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;
                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        Thread.Sleep(500);
                        driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        // objGeneric.ClickStamp(LabelKey.CHRISTMASSAVER, ControlKeys.STAMP9, "Christmas Saver");
                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        objAutomationHelper.WebDriver.Close();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }                
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To Verify page title in the header")]
        [TestCategory("P2")]
        [TestCategory("XmusSaver")]
        [TestCategory("P2Set1")]
        
        public void ChristmasSaver_Verify_Header_Title()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        Thread.Sleep(500);
                        driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        //objAutomationHelper.InitializeWebDriver(SanityConfiguration.DefaultBrowser.ToString(), SanityConfiguration.ChristmasSaverUrl);
                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        objChristmasSaver.Verify_Title_Header(driver);
                        // objLogin.LogOut_Verification();
                        objAutomationHelper.WebDriver.Close();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To Verify welcome message")]
        [TestCategory("P2")]
        [TestCategory("P2Set1")]
        [TestCategory("XmusSaver")]
        
        public void ChristmasSaver_Verify_Welcome_Message()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        Thread.Sleep(500);
                        driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        objChristmasSaver.Verify_WelcomeMessage(driver);
                        // objLogin.LogOut_Verification();
                        objAutomationHelper.WebDriver.Close();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To Verify thank you message")]
        [TestCategory("P2")]
        [TestCategory("XmusSaver")]
        [TestCategory("P2Set1")]
      
        public void ChristmasSaver_Verify_ThankYou_Message()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        Thread.Sleep(500);
                        driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);

                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        objChristmasSaver.Verify_ThankYouMessage(driver);
                        // objLogin.LogOut_Verification();
                        objAutomationHelper.WebDriver.Close();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("To Verify options and benefits text")]
        [TestCategory("P2")]
        [TestCategory("XmusSaver")]
        [TestCategory("P2Set1")]
       
        public void ChristmasSaver_Verify_OptionsAndBenefits_Title()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        Thread.Sleep(500);
                        driver.Navigate().GoToUrl(SanityConfiguration.ChristmasSaverUrl);

                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        bool isOptAndBenAvailable = objGeneric.IsPageEnabled(DBConfigKeys.HIDEOPTIONSANDBENEFITS);
                        if (isOptAndBenAvailable)
                        {
                            objChristmasSaver.Verify_OptionsAndBenefitsText(driver);
                        }
                        else
                        {
                            Assert.AreEqual(isOptAndBenAvailable, false, "Configuration Value not matched with DBConfig");
                        }
                        // objLogin.LogOut_Verification();
                        objAutomationHelper.WebDriver.Close();
                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Customer not opted into christmas saver preference")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        
        public void XmusSaver_CustomerNotOptedToXmusSaver()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.NoNXmusSaverAccount.Clubcard, testData.NoNXmusSaverAccount.Password, testData.NoNXmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.NoNXmusSaverAccount.Clubcard);
                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.NoNXmusSaverAccount.Clubcard);
                        objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "Option&Benefit", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                        // objLogin.LogOut_Verification();
                    }
                    else
                    {
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                    }
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Customer opted into christmas saver preference")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("Perk_Elixir_S1")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0Set2")]
        public void XmusSaver_CustomerOptedToXmusSaver()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);
                        objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();

                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify that You have saved xx so far for your November xxxx statement is displayed")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0Set2")]
        public void XmusSaver_VerifyTextYouHaveSaved()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);
                        // objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        // objLogin.LogOut_Verification();
                        if (!(objGeneric.VerifyText_Contains(LabelKey.XMUS_YOUHAVESAVED, ControlKeys.XMUSSAVER_YOUHAVESAVEDMSG, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE)))
                        {
                            customLogs.LogInformation("You have saved section is not present under Xmus Saver");
                            Assert.Fail("You have saved section is not present under Xmus Saver");
                        }
                        if (!(objGeneric.VerifyText_Contains(LabelKey.XMUS_NOVEMBER, ControlKeys.XMUSSAVER_YOUHAVESAVEDMSG, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE)))
                        {
                            customLogs.LogInformation("SO FAR FOR YOUR NOVEMBER section is not present under Xmus Saver");
                            Assert.Fail("SO FAR FOR YOUR NOVEMBER section is not present under Xmus Saver");
                        }
                        string strTotalVoucher = objGeneric.GetDataFromPage(ControlKeys.XMUSSAVER_SPNTTLVOUCHERSSOFAR);
                        string strYouHaveSavedMSG = objGeneric.GetTextFromResourceFile(LabelKey.XMUS_YOUHAVESAVED, "Xmus Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                        string strXMUS_NOVEMBER = objGeneric.GetTextFromResourceFile(LabelKey.XMUS_NOVEMBER, "Xmus Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);

                        string strXmusYear = string.Empty;

                        if (DateTime.Now.Date < strXmasNextStartDate)
                        {
                            strXmusYear = (DateTime.Now.Year).ToString();
                        }
                        else if (DateTime.Now.Date >= strXmasNextStartDate)
                        {
                            strXmusYear = (DateTime.Now.Year + 1).ToString();
                        }


                        if (!(objGeneric.VerifyDataOnPage_Contains_ByXpath(strTotalVoucher, ControlKeys.XMUSSAVER_SPNTTLPNTS)))
                        {
                            customLogs.LogInformation("You have saved section is not present under Xmus Saver");
                            Assert.Fail("You have saved section is not present under Xmus Saver");
                        }
                        if (!(objGeneric.VerifyDataOnPage_Contains(strXmusYear, ControlKeys.XMUSSAVER_YOUHAVESAVEDMSG)))
                        {
                            customLogs.LogInformation("You have saved section is not present under Xmus Saver");
                            Assert.Fail("You have saved section is not present under Xmus Saver");
                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }


        [TestMethod]
        [Description("Verify Xmus Saver Summary")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0Set2")]
        
        public void XmusSaver_VerifyXmusSaverSummary()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        DateTime startDate = DateTime.Now.Date;
                        DateTime endDate = DateTime.Now.Date;

                        if (DateTime.Now.Date < strXmasNextStartDate)
                        {
                            startDate = strXmasCurrStartDate;
                            endDate = strXmasCurrEndDate;
                        }
                        else if (DateTime.Now.Date >= strXmasNextStartDate)
                        {
                            startDate = strXmasNextStartDate;
                            endDate = strXmasNextEndDate;
                        }

                        Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverAccount.Clubcard, "en-GB");

                        DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                        decimal sumTtlToppedUpMoney = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            sumTtlToppedUpMoney += Convert.ToDecimal(dr["AmountSpent"]);
                        }
                        if (!(objGeneric.VerifyDataOnPage_Contains(sumTtlToppedUpMoney.ToString(), ControlKeys.XMUSSAVER_SPNTTLTOPPEDUPMONEY)))
                        {
                            customLogs.LogInformation("TOP UP VOUCHER SAVED SO section is not matched under Xmus Saver");
                            Assert.Fail("TOP UP VOUCHER SAVED SO FAR section is not present under Xmus Saver");
                        }


                        string stDate = startDate.ToString("yyyyMMdd");
                        string enDate = endDate.ToString("yyyyMMdd");

                        DataSet ds1 = objSmartVoucherService.GetCustomerVoucherValCPSDataset(testData.XmusSaverAccount.Clubcard, stDate, enDate);


                        int remainders = 0;
                        int rewardPointsForCP = 0;
                        int prevCPPnts = 0;
                        string voucherValue;
                        decimal sumXmasVoucher = 0;



                        if (ds1 != null && ds1.Tables[0] != null)
                        {
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                if (!string.IsNullOrEmpty(dr["Reward_Points"].ToString()))
                                {
                                    rewardPointsForCP = (Convert.ToInt32(dr["Reward_Points"].ToString()) - prevCPPnts) + remainders; // (Convert.ToInt32(row["Reward_Points"].ToString()) - prevCPPnts) + remainders;
                                    prevCPPnts = Convert.ToInt32(dr["Reward_Points"].ToString());// Convert.ToInt32(row["Reward_Points"].ToString());
                                    voucherValue = VoucherDisplay(rewardPointsForCP, out remainders);

                                    sumXmasVoucher = sumXmasVoucher + Convert.ToDecimal(voucherValue, System.Globalization.CultureInfo.InvariantCulture);

                                }
                            }

                        }

                        string strClubcardVoucherSaved = String.Format("{0:C}", sumXmasVoucher);

                        if (!(objGeneric.VerifyDataOnPage_Contains(sumXmasVoucher.ToString(), ControlKeys.XMUSSAVER_SPNCCVOUCHERSSAVED)))
                        {
                            customLogs.LogInformation("CLUBCARD VOUCHER SAVED SO FAR section is not matched under Xmus Saver");
                            Assert.Fail("CLUBCARD VOUCHER SAVED SO FAR section is not present under Xmus Saver");
                        }




                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Xmus Saver customer has topped up")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyXmusSaverToppedUpValues()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard, testData.XmusSaverCardWithTopUpLessThen100.Password, testData.XmusSaverCardWithTopUpLessThen100.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard);

                        DateTime startDate = DateTime.Now.Date;
                        DateTime endDate = DateTime.Now.Date;

                        if (DateTime.Now.Date < strXmasNextStartDate)
                        {
                            startDate = strXmasCurrStartDate;
                            endDate = strXmasCurrEndDate;
                        }
                        else if (DateTime.Now.Date >= strXmasNextStartDate)
                        {
                            startDate = strXmasNextStartDate;
                            endDate = strXmasNextEndDate;
                        }

                        DBConfiguration config3 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, "DisplayDateFormat", SanityConfiguration.DbConfigurationFile);
                        string dateFormat = config3.ConfigurationValue1.ToString();

                        Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverCardWithTopUpLessThen100.Clubcard, "en-GB");

                        DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                        decimal sumTtlToppedUpMoney = 0;
                        string strDate = string.Empty;
                        if (ds != null && ds.Tables != null)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                sumTtlToppedUpMoney = sumTtlToppedUpMoney + Convert.ToDecimal(dr["AmountSpent"]);

                            }
                            if (!(objGeneric.VerifyDataOnPage_Contains(sumTtlToppedUpMoney.ToString(), ControlKeys.XMUSSAVER_DVMONNEYTOPPEDUP)))
                            {
                                customLogs.LogInformation("XMUS SAVER TOPPED UP VALUE is not matched under Xmus Saver");
                                Assert.Fail("XMUS SAVER TOPPED UP VALUE is not present under Xmus Saver");

                            }
                        }

                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Bonus Amount for a top up")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("P0Set2")]
        [TestCategory("XmusSaver")]
        public void XmusSaver_VerifyBonusAmount()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverAccount.Clubcard, testData.XmusSaverAccount.Password, testData.XmusSaverAccount.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverAccount.Clubcard);

                        DateTime startDate = DateTime.Now.Date;
                        DateTime endDate = DateTime.Now.Date;

                        if (DateTime.Now.Date < strXmasNextStartDate)
                        {
                            startDate = strXmasCurrStartDate;
                            endDate = strXmasCurrEndDate;
                        }
                        else if (DateTime.Now.Date >= strXmasNextStartDate)
                        {
                            startDate = strXmasNextStartDate;
                            endDate = strXmasNextEndDate;
                        }

                        Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverAccount.Clubcard, "en-GB");

                        DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                        decimal sumTtlToppedUpMoney = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            sumTtlToppedUpMoney += Convert.ToDecimal(dr["AmountSpent"]);
                        }


                        DBConfiguration config3 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, "TopupRange", SanityConfiguration.DbConfigurationFile);
                        DBConfiguration config4 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, "BonusVoucher", SanityConfiguration.DbConfigurationFile);
                        DBConfiguration config5 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, "Topuptoreceivemaxbonus", SanityConfiguration.DbConfigurationFile);
                        DBConfiguration config6 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, "MaxBonusVoucher", SanityConfiguration.DbConfigurationFile);

                        string TopupRange = config3.ConfigurationValue1;
                        string BonusVooucher = config4.ConfigurationValue1;
                        string Topuptoreceivemaxbonus = config5.ConfigurationValue1;
                        string MaxBonusVoucher = config6.ConfigurationValue1;

                        string[] topupRange = TopupRange.Split(',');
                        string[] bonusVoucher = BonusVooucher.Split(',');

                        int countoftopuprange = (topupRange.Length) - 1;

                        for (int i = 0; i < countoftopuprange; i++)
                        {
                            if ((sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i]))
                                   && (sumTtlToppedUpMoney < Convert.ToDecimal(topupRange[i + 1])))
                            {

                                //spnBonusVoucher.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(bonusVoucher[i]));
                                // sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney + Convert.ToDecimal(bonusVoucher[i]);   
                                if (!(objGeneric.VerifyDataOnPage_Contains(bonusVoucher[i].ToString(), ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                                {
                                    customLogs.LogInformation("Bonus voucher value = " + bonusVoucher[i].ToString() + " is not matched for topped up amount = " + sumTtlToppedUpMoney.ToString());
                                    Assert.Fail("TEST CASE FAILED : Bonus voucher value = " + bonusVoucher[i].ToString() + " is not matched for topped up amount = " + sumTtlToppedUpMoney.ToString());

                                }


                            }
                            else if (sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i + 1]) && sumTtlToppedUpMoney >= Convert.ToDecimal(Topuptoreceivemaxbonus))
                            {
                                if (i == 0)
                                {

                                    // spnBonusVoucher.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(MaxBonusVoucher));
                                    // sumVoucherSavedSoFar = sumVoucherSavedSoFar + Convert.ToDecimal(MaxBonusVoucher) + sumTtlToppedUpMoney;
                                    if (!(objGeneric.VerifyDataOnPage_Contains(MaxBonusVoucher.ToString(), ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                                    {
                                        customLogs.LogInformation("Bonus voucher value = " + MaxBonusVoucher.ToString() + " is not matched for topped up amount = " + sumTtlToppedUpMoney.ToString());
                                        Assert.Fail("TEST CASE FAILED : Bonus voucher value = " + MaxBonusVoucher.ToString() + " is not matched for topped up amount = " + sumTtlToppedUpMoney.ToString());
                                        break;
                                    }


                                }
                            }

                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount <25£ Bonus Amount should be £0")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyBonusAmountForTopUpLessThen25()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen25.Clubcard, testData.XmusSaverCardWithTopUpLessThen25.Password, testData.XmusSaverCardWithTopUpLessThen25.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen25.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen25.Clubcard);
                        string strBonusAmount = "0";
                        strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                        if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                        {
                            customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");
                            Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");

                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  =>25£> and <50£ Bonus Amount should be £1.50")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT25AndLessThen50()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen50.Clubcard, testData.XmusSaverCardWithTopUpLessThen50.Password, testData.XmusSaverCardWithTopUpLessThen50.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen50.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen50.Clubcard);
                        string strBonusAmount = "1.5";
                        strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);



                        if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                        {
                            customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");
                            Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");

                        }
                        //--Compare message in the right div dvMoneyTobeSavedForBonus
                        string spnBonusValueFor50 = "1.50";
                        string spnMoneyTobeSavedForBonus6 = (50 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen50.Clubcard)).ToString(); ;
                        string spnBonusValueFor100_1 = "3";

                        StringBuilder sb = GetInfomationString(spnBonusValueFor50, spnMoneyTobeSavedForBonus6, spnBonusValueFor100_1);

                        if (!(objGeneric.VerifyDataOnPage_Contains(sb.ToString(), ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                        {
                            customLogs.LogInformation("Right side message is not matched for bonus information");
                            Assert.Fail("Right side message is not matched for bonus information");

                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        private StringBuilder GetInfomationString(string spnBonusValueFor50, string spnMoneyTobeSavedForBonus6, string spnBonusValueFor100_1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(objGeneric.GetTextFromResourceFile("Congratulation", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("YouHaveSavedEnough", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("Currency", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(spnBonusValueFor50);
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("BonusVoucherInStatmnt", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("YouOnlyNeedToSaveExtra", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("Currency", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(spnMoneyTobeSavedForBonus6);
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("ToMakeIt", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("Currency", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
            sb.Append(spnBonusValueFor100_1);
            sb.Append(" ");
            sb.Append(objGeneric.GetTextFromResourceFile("BonusVoucher", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE) + ".");

            return sb;
        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount =>50£ and <100 Bonus Amount should be £3")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT50AndLessThen100()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard, testData.XmusSaverCardWithTopUpLessThen100.Password, testData.XmusSaverCardWithTopUpLessThen100.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen100.Clubcard);
                        string strBonusAmount = "3";
                        strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                        if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                        {
                            customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £100");
                            Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £100");

                        }

                        //--Compare message in the right div dvMoneyTobeSavedForBonus
                        string spnBonusValueFor50 = "3";
                        string spnMoneyTobeSavedForBonus6 = (100 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen100.Clubcard)).ToString();
                        string spnBonusValueFor100_1 = "6";

                        StringBuilder sb = GetInfomationString(spnBonusValueFor50, spnMoneyTobeSavedForBonus6, spnBonusValueFor100_1);

                        if (!(objGeneric.VerifyDataOnPage_Contains(sb.ToString(), ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                        {
                            customLogs.LogInformation("Right side message is not matched for bonus information");
                            Assert.Fail("Right side message is not matched for bonus information");

                        }



                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  =>100£ and <200£ Bonus Amount should be £6")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT100AndLessThen200()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen200.Clubcard, testData.XmusSaverCardWithTopUpLessThen200.Password, testData.XmusSaverCardWithTopUpLessThen200.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen200.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen200.Clubcard);
                        string strBonusAmount = "6";
                        strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                        if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                        {
                            customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £200");
                            Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £200");

                        }

                        //--Compare message in the right div dvMoneyTobeSavedForBonus
                        string spnBonusValueFor50 = "6";
                        string spnMoneyTobeSavedForBonus6 = (200 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen200.Clubcard)).ToString();
                        string spnBonusValueFor100_1 = "12";

                        StringBuilder sb = GetInfomationString(spnBonusValueFor50, spnMoneyTobeSavedForBonus6, spnBonusValueFor100_1);

                        if (!(objGeneric.VerifyDataOnPage_Contains(sb.ToString(), ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                        {
                            customLogs.LogInformation("Right side message is not matched for bonus information");
                            Assert.Fail("Right side message is not matched for bonus information");

                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  £200>= Bonus Amount should be £12")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        [TestCategory("XmusSaver")]
        [TestCategory("P0_Sequential")]
        public void XmusSaver_VerifyBonusAmountForTopUpGreaterThen200()
        {
            bool isKeyPresent = objGeneric.IsPageEnabled(DBConfigKeys.HIDECHRISTMASSAVERPAGE);
            if (!isKeyPresent && culture.Equals("UK"))
            {
                expectedStampName = objGeneric.isStampPresentbyKey();

                if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
                {
                    var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                    DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                    string isPresent = config.IsDeleted;
                    if (isPresent == "N")
                    {
                        objLogin.Login_Verification(testData.XmusSaverCardWithTopUpGreaterThen200.Clubcard, testData.XmusSaverCardWithTopUpGreaterThen200.Password, testData.XmusSaverCardWithTopUpGreaterThen200.EmailID);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpGreaterThen200.Clubcard);

                        objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                        objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                        objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpGreaterThen200.Clubcard);
                        string strBonusAmount = "12";
                        strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                        if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                        {
                            customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount greater then £200");
                            Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount greater then £200");

                        }
                        //--Compare message in the right div dvMoneyTobeSavedForBonus
                        string spnBonusValueFor100 = "12";

                        StringBuilder sb = new StringBuilder();
                        sb.Append(objGeneric.GetTextFromResourceFile("Congratulation", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
                        sb.Append(" ");
                        sb.Append(objGeneric.GetTextFromResourceFile("YouHaveSavedEnough", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
                        sb.Append(" ");
                        sb.Append(objGeneric.GetTextFromResourceFile("Currency", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
                        sb.Append(spnBonusValueFor100);
                        sb.Append(" ");
                        sb.Append(objGeneric.GetTextFromResourceFile("BonusVoucherInStatmnt", "CHRISTMASSAVER", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));

                        if (!(objGeneric.VerifyDataOnPage_Contains(sb.ToString(), ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR100)))
                        {
                            customLogs.LogInformation("Right side message is not matched for bonus information");
                            Assert.Fail("Right side message is not matched for bonus information");

                        }


                    }
                    else
                        Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
                }
            }
            else
            {
                Assert.Inconclusive(string.Format("Christmas Saver not present for country : {0}, culture : {1}", CountrySetting.country, CountrySetting.culture));
            }
        }



        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();

        }

        public string VoucherDisplay(int totalPoints, out int residual)
        {

            try
            {
                if (totalPoints < LabelKey.REWARDEE_LIMIT)
                {
                    residual = totalPoints;
                    return "0.0";
                }
                int remd = totalPoints % 50;
                residual = remd;
                int correctedPoints = totalPoints - remd;
                float dispVal = ((float)(correctedPoints)) / 100;
                string strDispVal = dispVal.ToString();
                if (strDispVal.Contains("."))
                {
                    string temp = strDispVal.Substring(strDispVal.Length - 2, 1);
                    if (temp != "0")
                        strDispVal += "0";
                }
                else
                {
                    strDispVal += ".00";
                }
                return strDispVal;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
        }
        public decimal GetTotalToppedUpValue(string clubcard)
        {
            
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date;
           
            if (DateTime.Now.Date < strXmasNextStartDate)
            {
                startDate = strXmasCurrStartDate;
                endDate = strXmasCurrEndDate;
            }
            else if (DateTime.Now.Date >= strXmasNextStartDate)
            {
                startDate = strXmasNextStartDate;
                endDate = strXmasNextEndDate;
            }

            Int64 customerId = objCustomerService.GetCustomerID(clubcard, "en-GB");

            DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
            decimal sumTtlToppedUpMoney = 0;
            if (ds != null && ds.Tables != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sumTtlToppedUpMoney = sumTtlToppedUpMoney + Convert.ToDecimal(dr["AmountSpent"]);

                }
            }
            return sumTtlToppedUpMoney;
        }

        public void SetXmusSaverDatesToGlobalVariables()
        {
           
            List<ConfugurationTypeEnum> configurationTypes = new List<ConfugurationTypeEnum>(){
                             ConfugurationTypeEnum.Holding_dates                            
                            };
            
           DataSet ds = objCustomerService.GetConfigurationItems(objGeneric.GetConfigurationTypesCsv(configurationTypes), "en-GB");

           if (ds != null && ds.Tables[0].Rows.Count > 0)
           {
               foreach(DataRow dr in ds.Tables[0].Rows)
               {
                   if (dr["ConfigurationName"].ToString() == "XmasSaverCurrDates")
                   {
                       strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim()).ToShortDateString());
                       strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim()).ToShortDateString());
                   }
                   if (dr["ConfigurationName"].ToString() == "XmasSaverNextDates")
                   {
                       strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim()).ToShortDateString());
                       strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim()).ToShortDateString());
                   }

               }             
              
           }
        }

    }
}
