
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Entities;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using Tesco.Framework.UITesting.Enums;

namespace Tesco.Framework.UITesting.Test.Common
{

    public class Home : Base
    {
        public IAlert alert = null;
        Generic objGeneric = null;
        CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
        static TestData_Voucher testData = null;
        static TestDataHelper<TestData_Voucher> TestDataHelper = new TestDataHelper<TestData_Voucher>();

        #region Constructor


        public Home(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion



        /// <summary>
        /// To verify the home page 
        /// </summary>
        /// <returns></returns>
        public bool Homepage_Verification()
        {
            try
            {
                CustomLogs.LogMessage("Verifying if Home Page is Reached or not started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                String actualPageTitle = null;
                //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Home);
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.HOMETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                string expectedPageTitle = res.Value;
                IWebElement ele = null;

                if (ele.WaitForTitle(Driver, "Home"))
                {
                    var elTitle = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_TITLE).XPath)).Text;
                    actualPageTitle = elTitle.Split(' ')[0].ToString();
                    if (actualPageTitle == expectedPageTitle)
                        CustomLogs.LogInformation("Title same for the home page");
                    else
                    {
                        CustomLogs.LogInformation("Title not same for the home page");
                        Assert.Fail("Title not same for the home page");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Driver.Quit();
            }
            CustomLogs.LogMessage("Home Page is Reached verified", TraceEventType.Stop);
            return true;
        }

        public bool Homepage_VerfiyPrintClubcard()
        {
            try
            {
                CustomLogs.LogMessage("Verifying the print functionality for Temporary Clubcard", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //  Find and Click print Temporary Clubcard Button
                Driver = ObjAutomationHelper.WebDriver;
                if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)))
                {
                    //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id)).Click();
                    IWebElement print = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_PRINT_CLUBCARD).Id));
                    Driver.ExecuteJs<bool>("arguments[0].click();return true;", print);

                }
                else
                {
                    CustomLogs.LogInformation("Button for Printing Temporary Clubcard does not exist on this page");
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                return true;

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Driver.Quit();
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return false;
            }

        }


        public void VerifyMessagSectionCDlessthanSignoff()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var ActualMessagelable = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_LBL).Id)).Text;
            var ExpectedMessageLable = AutomationHelper.GetResourceMessage(ValidationKey.HOME_MESSAGELABLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;
            Assert.AreEqual(ActualMessagelable, ExpectedMessageLable);
            DateTime Cutoffdate = new DateTime();
            DateTime signoffdate = new DateTime();
            string ActualMessage = string.Empty;
            string expecetedmessagenew = string.Empty;
            CustomerServiceAdaptor dates = new CustomerServiceAdaptor();
            Cutoffdate = dates.GetCutoffDate(Login.CustomerID.ToString(), CountrySetting.culture);
            signoffdate = dates.GetSignoffDate(Login.CustomerID.ToString(), CountrySetting.culture);
            if ((DateTime.Now >= Cutoffdate) && (DateTime.Now <= signoffdate))
            {

                ActualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_Text).Id)).Text;
                string expectedMessage = AutomationHelper.GetResourceMessage(ValidationKey.HOME_MESSAGETEXT2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;
                expecetedmessagenew = expectedMessage.Replace("&quot;", "\"");
                Assert.AreEqual(ActualMessage, expecetedmessagenew);
            }
            else
            {
                Assert.Inconclusive("cutoffdate current date do not fall between cutoff date and signoff date");
            }

        }
        public void VerifyMessagSectionMailingAE()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var ActualMessagelable = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_LBL).Id)).Text;
            var ExpectedMessageLable = AutomationHelper.GetResourceMessage(ValidationKey.HOME_MESSAGELABLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;
            Assert.AreEqual(ActualMessagelable, ExpectedMessageLable);
            DateTime Cutoffdate = new DateTime();
            DateTime signoffdate = new DateTime();
            string ActualMessage = string.Empty;
            string expecetedmessagenew = string.Empty;
            string customermailingstataus = string.Empty;
            CustomerServiceAdaptor dates = new CustomerServiceAdaptor();
            Cutoffdate = dates.GetCutoffDate(Login.CustomerID.ToString(), CountrySetting.culture);
            signoffdate = dates.GetSignoffDate(Login.CustomerID.ToString(), CountrySetting.culture);
            CustomerServiceAdaptor cms = new CustomerServiceAdaptor();
            customermailingstataus = cms.GetCustomerMailStatus(Login.CustomerID.ToString(), CountrySetting.culture);
            if (!((DateTime.Now >= Cutoffdate) && (DateTime.Now <= signoffdate)) && (customermailingstataus != "3"))
            {

                ActualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_Text).Id)).Text;
                var expectedMessage = AutomationHelper.GetResourceMessage(ValidationKey.HOME_MESSAGETEXT3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;
                expecetedmessagenew = expectedMessage.Replace("&quot;", "\"");
                Assert.AreEqual(ActualMessage, expecetedmessagenew);
            }
            else
            {
                Assert.Inconclusive("cutoffdate is not less then current date or sign off date is not greater then current date or Customer mailing status is AddressIN Error");
            }

        }

        public void VerifyMessagSectionCMS3()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var ActualMessagelable = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_LBL).Id)).Text;
            var ExpectedMessageLable = AutomationHelper.GetResourceMessage(ValidationKey.HOME_MESSAGELABLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;
            Assert.AreEqual(ActualMessagelable, ExpectedMessageLable);
            string ActualMessage = string.Empty;
            string expecetedmessagenew = string.Empty;
            string customermailingstataus = string.Empty;
            CustomerServiceAdaptor cms = new CustomerServiceAdaptor();
             customermailingstataus = cms.GetCustomerMailStatus(Login.CustomerID.ToString(), CountrySetting.culture);
            DBConfiguration ISDotcomEnabled = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.ISDOTCOMENABLED, SanityConfiguration.DbConfigurationFile);
            if ((ISDotcomEnabled.ConfigurationValue1== 1.ToString()) && customermailingstataus == 3.ToString())
            {
                ActualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_MESSAGE_Text).Id)).Text;
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(WebConfigKeys.MESSAGEONE, SanityConfiguration.WebConfigurationFile);
                string Message = webConfig.Value;

                Assert.AreEqual(ActualMessage, Message);
            }
            else
            {
                Assert.Inconclusive("CustomeMailingStstus is not 3 or IsDotcom environment is 0");
            }

        }
        public void VerifyPointsSection()
        {
            Driver = ObjAutomationHelper.WebDriver;
            ClubcardServiceAdapter points = new ClubcardServiceAdapter();
            var ActualCurrentPointslable = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_PTSSMRY_LBL).XPath)).Text;
            var ExpectedCurrentPointsLable = AutomationHelper.GetResourceMessage(ValidationKey.HOME_CURRENTPOINTSLBL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE)).Value;

            Assert.AreEqual(ActualCurrentPointslable, ExpectedCurrentPointsLable);
            string acutalCurrentPointsValue = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_PTSSMRY_VALUE).XPath)).Text;
            string expectedcurrentPointsvalue = points.GetPointsTotal(Login.CustomerID, CountrySetting.culture);
            Assert.AreEqual(acutalCurrentPointsValue, expectedcurrentPointsvalue);
        }
        public void VerifyVoucherSection(string Clubcard)
        {
            Driver = ObjAutomationHelper.WebDriver;
            SmartVoucherAdapter Vouchers = new SmartVoucherAdapter();
            var ActualVouchersPointslable = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_LBL).XPath)).Text;
            var ExpectedVouchersPointsLable = AutomationHelper.GetResourceMessage(ValidationKey.HOME_STDVOUCHERSLBL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            Assert.AreEqual(ActualVouchersPointslable, ExpectedVouchersPointsLable);
            string acutalCurrentPointsValue = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_VALUE).XPath)).Text;
            string Voucher = Vouchers.GetAvailableVouchersCount(Clubcard, CountrySetting.culture);
            string ExpectedCurrencySymbol = AutomationHelper.GetResourceMessage(ValidationKey.HOME_CURRENCYSYMBOL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            string ExpectedVouchers = string.Concat(ExpectedCurrencySymbol, " ", Voucher);
            Assert.AreEqual(acutalCurrentPointsValue, ExpectedVouchers);
        }
        public void verifyChristmasSaverSummary()
        {
            Driver = ObjAutomationHelper.WebDriver;
            ClubcardServiceAdapter XmasVouchers = new ClubcardServiceAdapter();
            var ActualChristmasSaverlable = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_LBL).XPath)).Text;
            var ExpectedChristmasSaverLable = AutomationHelper.GetResourceMessage(ValidationKey.Xmas_Saver, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            Assert.AreEqual(ActualChristmasSaverlable, ExpectedChristmasSaverLable);
            string acutualXmasVoucherValue = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_VALUE).XPath)).Text;
            string expectedVouchersvalue = XmasVouchers.GetTotalVouchers(Login.CustomerID, CountrySetting.culture);

            string ExpectedCurrencySymbol = AutomationHelper.GetResourceMessage(ValidationKey.HOME_CURRENCYSYMBOL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            string expectedXmasVouchersvalue = string.Concat(ExpectedCurrencySymbol, " ", expectedVouchersvalue);

            Assert.AreEqual(acutualXmasVoucherValue, expectedXmasVouchersvalue);
        }

        public void verifySecondBoxValue(string Clubcard, int preferenceId)
        {
            string expectedVouchers = string.Empty;
            string expectedFVouchers = string.Empty;
            string resourceId = string.Empty;
            decimal expectedVoucher = 0;
            bool isCurrencyPrefix = false;
            Driver = ObjAutomationHelper.WebDriver;

            var ActuallableForSecondBox = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_LBL).XPath)).Text;
            string acutalCurrentPointsValue = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDPOINTSBOX_VALUE).XPath)).Text;

            ClubcardServiceAdapter clubcardClient = new ClubcardServiceAdapter();
            expectedFVouchers = expectedVouchers = clubcardClient.GetVouchers(Login.CustomerID, CountrySetting.culture, 0);

            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            //string preferenceID =  prefClient.IsContactPreferenceOpted(Login.CustomerID, preferenceId);
            bool isBASTD_Opted = prefClient.IsContactPreferenceOpted(Login.CustomerID, preferenceId);
            if (isBASTD_Opted)
            {
                decimal.TryParse(expectedFVouchers, out expectedVoucher);
                switch ((OptionPreference)preferenceId)
                {
                    case OptionPreference.BA_Miles_Standard:
                        expectedVoucher = BusinessConstants.STANDARD_BAMILES * expectedVoucher / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE);
                        resourceId = ValidationKey.BA_Miles_Standard;
                        break;
                    case OptionPreference.BA_Miles_Premium:
                        expectedVoucher = BusinessConstants.PRIMIUM_BAMILES * expectedVoucher / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE);
                        resourceId = ValidationKey.BA_Miles_Standard;
                        break;
                    case OptionPreference.Xmas_Saver:
                        isCurrencyPrefix = true;
                        resourceId = ValidationKey.Xmas_Saver;
                        break;
                    case OptionPreference.Airmiles_Premium:
                        expectedVoucher = BusinessConstants.PRIMIUM_BAMILES * expectedVoucher / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE);
                        resourceId = ValidationKey.Avios_standerd;
                        break;
                    case OptionPreference.Airmiles_Standard:
                        expectedVoucher = BusinessConstants.STANDARD_AMILES * expectedVoucher / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE);
                        resourceId = ValidationKey.Avios_standerd;
                        break;
                    case OptionPreference.Virgin_Atlantic:
                        expectedVoucher = BusinessConstants.VIRGIN_ATLANTIC * expectedVoucher / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE);
                        resourceId = ValidationKey.Virgin_Atlantic;
                        break;
                }
                string ExpectedCurrencySymbol = AutomationHelper.GetResourceMessage(ValidationKey.HOME_CURRENCYSYMBOL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

                var ExpectedLBl = AutomationHelper.GetResourceMessage(resourceId, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedVouchers = expectedFVouchers = expectedVoucher.ToString();
                DBConfiguration DisableDecimalConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_CURRENCY_DECIMAL, SanityConfiguration.DbConfigurationFile);
                if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    expectedFVouchers = (expectedVouchers.Contains(',') ? expectedVouchers.TrimEnd('0').TrimEnd(',') : expectedVouchers.Contains('.') ? expectedVouchers.TrimEnd('0').TrimEnd('.') : expectedFVouchers);
                    expectedFVouchers = expectedFVouchers.Contains('.') ? expectedVouchers : expectedFVouchers.Contains(',') ? expectedVouchers : expectedFVouchers;
                }
                Assert.AreEqual(acutalCurrentPointsValue, isCurrencyPrefix ? string.Concat(ExpectedCurrencySymbol, " ", expectedFVouchers) : expectedFVouchers);
                Assert.AreEqual(ActuallableForSecondBox, ExpectedLBl);
            }
            else
            {
                Assert.Inconclusive(string.Format("Customer ID: {0} is not opted for {1}", Login.CustomerID, OptionPreference.BA_Miles_Standard.ToString()));
            }
        }
        
    }
}
