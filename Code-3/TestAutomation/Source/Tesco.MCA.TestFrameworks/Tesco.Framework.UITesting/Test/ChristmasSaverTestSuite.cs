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


namespace Tesco.Framework.UITesting.Test
{
    [TestClass]
    public class ChristmasSaver
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

        private static string beginMessage = "********************* Christmas saver suite ****************************";
        private static string suiteName = "Christmas saver";
        private static string endMessage = string.Format("**************************************************************{0}", Environment.NewLine);

        static TestData_XmusSaver testData = null;
        static TestDataHelper<TestData_XmusSaver> TestDataHelper = new TestDataHelper<TestData_XmusSaver>();
        static string culture;
        CustomerServiceAdaptor objCustomerService = null;
        ClubcardServiceAdapter objClubcardService = null;
        SmartVoucherAdapter objSmartVoucherService = null;
        public ChristmasSaver()
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
            objCustomerService = new CustomerServiceAdaptor();
            objClubcardService = new ClubcardServiceAdapter();
            objSmartVoucherService = new SmartVoucherAdapter(); 
        }



        [TestMethod]
        [Description("To validate the stamp functionality for Christmas Saver page")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void StampHomepage_ChristmasSaver()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);
                    // objGeneric.ElementMouseOver(Control.Keys.STAMP5);


                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);

                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    //  objGeneric.VerifyTextonthePageByXpath(LabelKey.STAMPPERSONALDETAILS, "My Personal Details", StampName.PERSONALDETAILS, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE, driver);
                   //   objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);
                    objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }



        [TestMethod]
        [Description("To Click on Fuel Save Tab And Verify the Title")]
        [TestCategory("Sanity")]
        public void ChristmasSaverStamp_ClickAndVerifyTitle()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();            
            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampNumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, stampNumber, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, "");
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);
                    objGeneric.stampClick(ControlKeys.STAMP5, "Christmas Saver", StampName.CHRISTMASSAVER);
                    // objGeneric.ClickStamp(LabelKey.CHRISTMASSAVER, ControlKeys.STAMP9, "Christmas Saver");
                    objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                    objLogin.LogOut_Verification();
                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }
        }

        [TestMethod]
        [Description("Customer not opted into christmas saver preference")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_CustomerNotOptedToXmusSaver()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.ClubcardNotXmusSaver, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.ClubcardNotXmusSaver);
                    
                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);                   
                   // objLogin.SecurityLayer_Verification(testData.ClubcardAviosPre);
                    objGeneric.verifyPageName(LabelKey.MYOPTIONANDBENEFIT, "Option&Benefit", SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE);
                   // objLogin.LogOut_Verification();

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Customer opted into christmas saver preference")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_CustomerOptedToXmusSaver()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    objGeneric.verifyPageName(LabelKey.CHRISTMASSAVER, "Christmas Saver", SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE);
                    // objLogin.LogOut_Verification();

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify that You have saved xx so far for your November xxxx statement is displayed")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyTextYouHaveSaved()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
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

                    DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverCurrDates", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config2 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverNextDates", SanityConfiguration.DbConfigurationFile);
                   
                    string strXmusYear = string.Empty;                    
                    DateTime strXmasCurrStartDate;
                    DateTime strXmasCurrEndDate;
                    DateTime strXmasNextStartDate;
                    DateTime strXmasNextEndDate;

                    strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue2.ToString().Trim()).ToShortDateString());
                    strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue2.ToString().Trim()).ToShortDateString());

                    if (DateTime.Now.Date < strXmasNextStartDate)
                    {
                        strXmusYear = (DateTime.Now.Year).ToString();
                    }
                    else if (DateTime.Now.Date >= strXmasNextStartDate)
                    {
                        strXmusYear = (DateTime.Now.Year + 1).ToString();  
                    }

                  
                    if (!(objGeneric.VerifyDataOnPage_Contains(strTotalVoucher, ControlKeys.XMUSSAVER_SPNTTLPNTS)))
                    {
                        customLogs.LogInformation("You have saved section is not present under Xmus Saver");
                        Assert.Fail("You have saved section is not present under Xmus Saver");
                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(strXmusYear, ControlKeys.XMUSSAVER_SPNYEAR1)))
                    {
                        customLogs.LogInformation("You have saved section is not present under Xmus Saver");
                        Assert.Fail("You have saved section is not present under Xmus Saver");
                    }
  

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }


        [TestMethod]
        [Description("Verify Xmus Saver Summary")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyXmusSaverSummary()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);

                    DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverCurrDates", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config2 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverNextDates", SanityConfiguration.DbConfigurationFile);
                    DateTime startDate = DateTime.Now.Date;
                    DateTime endDate = DateTime.Now.Date;
                    DateTime strXmasCurrStartDate;
                    DateTime strXmasCurrEndDate;
                    DateTime strXmasNextStartDate;
                    DateTime strXmasNextEndDate;

                    strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue2.ToString().Trim()).ToShortDateString());
                    strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue2.ToString().Trim()).ToShortDateString());

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

                    Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverClubcard, "en-GB");

                    DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                    decimal sumTtlToppedUpMoney = 0;
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        sumTtlToppedUpMoney += Convert.ToDecimal(dr["AmountSpent"]);
                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(sumTtlToppedUpMoney.ToString(), ControlKeys.XMUSSAVER_SPNTTLTOPPEDUPMONEY)))
                    {
                        customLogs.LogInformation("TOP UP VOUCHER SAVED SO section is not matched under Xmus Saver");
                        Assert.Fail("TOP UP VOUCHER SAVED SO FAR section is not present under Xmus Saver");
                    }

                    //DBConfiguration config3 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ConfigKeys, "TopupRange", SanityConfiguration.DbConfigurationFile);
                    //DBConfiguration config4 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ConfigKeys, "BonusVoucher", SanityConfiguration.DbConfigurationFile);
                    //string TopupRange = config3.ConfigurationValue1;
                    //string BonusVooucher = config4.ConfigurationValue1;

                    //string[] topupRange = TopupRange.Split(',');
                    //string[] bonusVoucher = BonusVooucher.Split(',');

                    string stDate = startDate.ToString("yyyyMMdd");
                    string enDate = endDate.ToString("yyyyMMdd");

                    DataSet ds1 = objSmartVoucherService.GetCustomerVoucherValCPSDataset(testData.XmusSaverClubcard, stDate, enDate);

                   
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

                    if (!(objGeneric.VerifyDataOnPage_Contains(sumXmasVoucher.ToString() , ControlKeys.XMUSSAVER_SPNCCVOUCHERSSAVED)))
                    {
                        customLogs.LogInformation("CLUBCARD VOUCHER SAVED SO FAR section is not matched under Xmus Saver");
                        Assert.Fail("CLUBCARD VOUCHER SAVED SO FAR section is not present under Xmus Saver");
                    }




                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify Xmus Saver customer has topped up")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyXmusSaverToppedUpValues()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);

                    DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverCurrDates", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config2 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverNextDates", SanityConfiguration.DbConfigurationFile);
                    DateTime startDate = DateTime.Now.Date;
                    DateTime endDate = DateTime.Now.Date;
                    DateTime strXmasCurrStartDate;
                    DateTime strXmasCurrEndDate;
                    DateTime strXmasNextStartDate;
                    DateTime strXmasNextEndDate;

                    strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue2.ToString().Trim()).ToShortDateString());
                    strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue2.ToString().Trim()).ToShortDateString());

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

                    DBConfiguration config3 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, "DisplayDateFormat", SanityConfiguration.DbConfigurationFile);
                    string dateFormat = config3.ConfigurationValue1.ToString();

                    Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverClubcard, "en-GB");

                    DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                    decimal sumTtlToppedUpMoney = 0;
                    string strDate = string.Empty;
                    if (ds != null && ds.Tables != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            sumTtlToppedUpMoney = Convert.ToDecimal(dr["AmountSpent"]);
                            strDate = Convert.ToDateTime(dr["TransactionDateTime"]).ToString(dateFormat);// dr["TransactionDateTime"].ToString();

                            if (!(objGeneric.VerifyDataOnPage_Contains(sumTtlToppedUpMoney.ToString(), ControlKeys.XMUSSAVER_DVMONNEYTOPPEDUP)))
                            {
                                customLogs.LogInformation("XMUS SAVER TOPPED UP VALUE is not matched under Xmus Saver");
                                Assert.Fail("XMUS SAVER TOPPED UP VALUE is not present under Xmus Saver");
                                break;
                            }


                            if (!(objGeneric.VerifyDataOnPage_Contains(strDate.ToString(), ControlKeys.XMUSSAVER_DVMONNEYTOPPEDUP)))
                            {
                                customLogs.LogInformation("XMUS SAVER TOPPED UP DATE is not matched under Xmus Saver");
                                Assert.Fail("XMUS SAVER TOPPED UP DATE is not present under Xmus Saver");
                                break;
                            }
                        }
                    }

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify Bonus Amount for a top up")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmount()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverClubcard, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverClubcard);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);

                    DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverCurrDates", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config2 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverNextDates", SanityConfiguration.DbConfigurationFile);
                    DateTime startDate = DateTime.Now.Date;
                    DateTime endDate = DateTime.Now.Date;
                    DateTime strXmasCurrStartDate;
                    DateTime strXmasCurrEndDate;
                    DateTime strXmasNextStartDate;
                    DateTime strXmasNextEndDate;

                    strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue2.ToString().Trim()).ToShortDateString());
                    strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue1.ToString().Trim()).ToShortDateString());
                    strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue2.ToString().Trim()).ToShortDateString());

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

                    Int64 customerId = objCustomerService.GetCustomerID(testData.XmusSaverClubcard, "en-GB");

                    DataSet ds = objClubcardService.GetChristmasSaverSummaryDataset(customerId, startDate, endDate, "en-GB");
                    decimal sumTtlToppedUpMoney = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sumTtlToppedUpMoney += Convert.ToDecimal(dr["AmountSpent"]);
                    }


                    DBConfiguration config3 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, "TopupRange", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config4 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, "BonusVoucher", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config5 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, "Topuptoreceivemaxbonus", SanityConfiguration.DbConfigurationFile);
                    DBConfiguration config6 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, "MaxBonusVoucher", SanityConfiguration.DbConfigurationFile);
                   
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

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount <25£ Bonus Amount should be £0")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmountForTopUpLessThen25()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen25, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen25);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    string strBonusAmount = "0";
                    strBonusAmount=string.Format(System.Globalization.CultureInfo.InvariantCulture,"{0:C}", strBonusAmount);  

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

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  =>25£> and <50£ Bonus Amount should be £1.50")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT25AndLessThen50()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen50, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen50);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    string strBonusAmount = "1.5";
                    strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                    if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                    {
                        customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");
                        Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then $25");

                    }
                    //--Compare message in the right div dvMoneyTobeSavedForBonus
                    string spnBonusValueFor50 = "1.5";
                    string spnMoneyTobeSavedForBonus6 = (50 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen50)).ToString(); ;
                    string spnBonusValueFor100_1 = "3";

                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor50, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnMoneyTobeSavedForBonus6, ControlKeys.XMUSSAVER_SPNMONEYTOBESAVEDFORBONUS6)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor100_1, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR100_1)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }

                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount =>50£ and <100 Bonus Amount should be £3")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT50AndLessThen100()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen100, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen100);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    string strBonusAmount = "3";
                    strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                    if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                    {
                        customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £100");
                        Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £100");

                    }

                    //--Compare message in the right div dvMoneyTobeSavedForBonus
                    string spnBonusValueFor50 = "3";
                    string spnMoneyTobeSavedForBonus6 = (100 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen100)).ToString();
                    string spnBonusValueFor100_1 = "6";

                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor50, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnMoneyTobeSavedForBonus6, ControlKeys.XMUSSAVER_SPNMONEYTOBESAVEDFORBONUS6)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor100_1, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR100_1)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }


                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  =>100£ and <200£ Bonus Amount should be £6")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmountForTopUpGT100AndLessThen200()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverCardWithTopUpLessThen200, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpLessThen200);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    string strBonusAmount = "6.0";
                    strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                    if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                    {
                        customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £200");
                        Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount less then £200");

                    }

                    //--Compare message in the right div dvMoneyTobeSavedForBonus
                    string spnBonusValueFor50 = "6";
                    string spnMoneyTobeSavedForBonus6 = (200 - GetTotalToppedUpValue(testData.XmusSaverCardWithTopUpLessThen200)).ToString();
                    string spnBonusValueFor100_1 = "12";

                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor50, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR50)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnMoneyTobeSavedForBonus6, ControlKeys.XMUSSAVER_SPNMONEYTOBESAVEDFORBONUS6)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor100_1, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR100_1)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }


                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
            }

        }

        [TestMethod]
        [Description("Verify Bonus Amount for Customer has topped up amount  £200>= Bonus Amount should be £12")]
        [TestCategory("P0")]
        [TestCategory("P0_Regression")]
        public void XmusSaver_VerifyBonusAmountForTopUpGreaterThen200()
        {
            expectedStampName = objGeneric.isStampPresentbyKey();

            if (expectedStampName.ContainsValue(StampName.CHRISTMASSAVER))
            {
                var stampnumber = expectedStampName.First(kvp => kvp.Value.Contains(StampName.CHRISTMASSAVER)).Key;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, DBConfigKeys.STAMP_CHRISTMASSAVER, SanityConfiguration.DbConfigurationFile);
                string isPresent = config.IsDeleted;
                if (isPresent == "N")
                {
                    objLogin.Login_Verification(testData.XmusSaverCardWithTopUpGreaterThen200, testData.Password, testData.EmailID);
                    objLogin.SecurityLayer_Verification(testData.XmusSaverCardWithTopUpGreaterThen200);

                    objGeneric.StampsTextValidation(ControlKeys.STAMP5, stampnumber, StampName.CHRISTMASSAVER);
                    objGeneric.stampClick(ControlKeys.STAMP5, "CHRISTMASSAVER", StampName.CHRISTMASSAVER);
                    string strBonusAmount = "12.0";
                    strBonusAmount = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:C}", strBonusAmount);

                    if (!(objGeneric.VerifyDataOnPage_Contains(strBonusAmount, ControlKeys.XMUSSAVER_SPNBONUSVOUCHER)))
                    {
                        customLogs.LogInformation("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount greater then £200");
                        Assert.Fail("Bonus voucher value = " + strBonusAmount + " is not matched for topped up amount greater then £200");

                    }
                    //--Compare message in the right div dvMoneyTobeSavedForBonus
                    string spnBonusValueFor100 = "12";
                                       
                    if (!(objGeneric.VerifyDataOnPage_Contains(spnBonusValueFor100, ControlKeys.XMUSSAVER_SPNBONUSVALUEFOR100)))
                    {
                        customLogs.LogInformation("Right side message is not matched for bonus information");
                        Assert.Fail("Right side message is not matched for bonus information");

                    }


                }
                else
                    Assert.AreEqual(isPresent, "Y", "Configuration Value not matched with DBConfig");
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
            DBConfiguration config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverCurrDates", SanityConfiguration.DbConfigurationFile);
            DBConfiguration config2 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Holding_dates, "XmasSaverNextDates", SanityConfiguration.DbConfigurationFile);
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date;
            DateTime strXmasCurrStartDate;
            DateTime strXmasCurrEndDate;
            DateTime strXmasNextStartDate;
            DateTime strXmasNextEndDate;

            strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue1.ToString().Trim()).ToShortDateString());
            strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(config1.ConfigurationValue2.ToString().Trim()).ToShortDateString());
            strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue1.ToString().Trim()).ToShortDateString());
            strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(config2.ConfigurationValue2.ToString().Trim()).ToShortDateString());

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

    }
}
