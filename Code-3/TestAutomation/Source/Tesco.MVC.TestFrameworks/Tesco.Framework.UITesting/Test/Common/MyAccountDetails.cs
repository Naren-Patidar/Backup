using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Services;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Data;

namespace Tesco.Framework.UITesting.Test.Common
{
    class MyAccountDetails : Base
    {
        #region PROPERTIES
        string isPresent = string.Empty;
        #endregion

        #region Constructor
        public MyAccountDetails(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }

        #endregion
        #region Methods

        //public bool personalDetails_verify()
        //{

        //       //Verify Personal Details
        //        //var actualPageHeader= Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PAGE_TITLE).XPath)).Text;
        //        //if (actualPageHeader == expectedLinkName.Trim())
        //        //{
        //        //    CustomLogs.LogInformation(actualPageHeader + "is equal to" + expectedLinkName+"So Personal Details Verified");

        //        //}
        //        //else
        //        //{
        //        //    CustomLogs.LogInformation("Personal Details not Verified");
        //        //    Assert.Fail("Personal Details not Verified");

        //        //}
        //    return true;

        //}
        public bool myContactPref_verify()
        {
            //Verify Personal Details
            //var actualPageHeader= Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PAGE_TITLE).XPath)).Text;
            //if (actualPageHeader == expectedLinkName.Trim())
            //{
            //    CustomLogs.LogInformation(actualPageHeader + "is equal to" + expectedLinkName + "So Contact Preference Verified");

            //}
            //else
            //{
            //    CustomLogs.LogInformation("Contact Preference not Verified");
            //    Assert.Fail("Contact Preference not Verified");

            //}
            return true;
        }

        //public string verifyPageEnabled(string keys)
        //{
        //    try
        //    {
        //        StackTrace stackTrace = new StackTrace();
        //        CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
        //        CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);
        //        DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HideJoinFunctionality, keys, SanityConfiguration.DbConfigurationFile);
        //        string isPresent = config.IsDeleted;
        //        return isPresent;
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomLogs.LogError(ex);
        //        return isPresent;
        //    }
        //}
        #endregion

        #region View My Card Test Scenarios

        public void ViewMyCard_Common_Salutation_verify(string msgIDNameofAccount, string msgIDAndSeprator, string ClubcardID, string resourceFileName)
        {
            string fullname = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string MainSalutation = string.Empty; long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                DataSet cusData = new DataSet();
                cusData = clubcard.GetCustomers(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(msgIDNameofAccount, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                string actualCommonMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_COMMONMSG).XPath)).Text;
                Resource res2 = AutomationHelper.GetResourceMessage(msgIDAndSeprator, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));

                string expectedCommonMessage = string.Empty;
                if (cusData.Tables[0].Rows.Count>0)
                {
                    string AssoSalutation = string.Empty;
                    foreach (DataRow cust in cusData.Tables[0].Rows)
                    {
                       fullname = GetSalutation(string.Empty, "VIEWMYCARDS", cust);
                        expectedCommonMessage += string.Format("{0} {1} ", fullname, res2.Value);
                    }
                    expectedCommonMessage = expectedCommonMessage.Substring(0, expectedCommonMessage.Length - (res2.Value.Length + 1));
                    expectedCommonMessage = res1.Value + " " + expectedCommonMessage;
                    Assert.AreEqual(actualCommonMessage.Trim(), expectedCommonMessage.Trim(), true);
                }
                else
                {
                    expectedCommonMessage = res1.Value + " " + MainSalutation;
                    Assert.AreEqual(actualCommonMessage.Trim(), expectedCommonMessage.Trim(), true);
                }

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_Main_Salutation_verify(string msgIDMain,string msgIDAsso, string msgCardHolder, string ClubcardID, string resourceFileName)
        {
            
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string MainSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                DataSet cusData = new DataSet();
                cusData = clubcard.GetCustomers(customerId, CountrySetting.culture);
                //Resource res1 = AutomationHelper.GetResourceMessage(msgIDMain, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                //Resource res3 = AutomationHelper.GetResourceMessage(msgCardHolder, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                string msgMain = string.Empty;
                if (cusData.Tables[0].Rows.Count > 0)
                {
                       MainSalutation = GetSalutation(msgMain, "VIEWMYCARDS", cusData.Tables[0].Rows[0]);
                }
                string actualMainMessage = Driver.FindElement(By.Id(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_MAINMSG).Id)).Text;
                Assert.AreEqual(actualMainMessage, MainSalutation, true);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_Asso_Salutation_verify(string msgIDAsso, string msgCardHolder, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                int count = clubcard.GetHouseholdCustomersCount(customerId, CountrySetting.culture);
                if (count > 1)
                {
                    string AssoSalutation = string.Empty;
                    //Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                    //AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                    ////Resource res2 = AutomationHelper.GetResourceMessage(msgIDAsso, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    ////Resource res3 = AutomationHelper.GetResourceMessage(msgCardHolder, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    string msgAsso = string.Empty;
                    DataSet cusData = new DataSet();
                    cusData = clubcard.GetCustomers(customerId, CountrySetting.culture);
                    if (cusData.Tables[0].Rows.Count > 1)
                    {
                            AssoSalutation = GetSalutation(msgAsso, "VIEWMYCARDS", cusData.Tables[0].Rows[1]);
                    }
                    string actualAssoMessage = Driver.FindElement(By.Id(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOMSG).Id)).Text;
                    Assert.AreEqual(actualAssoMessage, AssoSalutation, true);
                }
                else
                {
                    CustomLogs.LogMessage("There is no Associate customer to this account", TraceEventType.Stop);
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ViewMyCard_Main_Cards_verify(string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedMainClubcards = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                ReadOnlyCollection<IWebElement> actualMainClubcards = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_MAINCLUBCARDS).XPath)));
                //ReadOnlyCollection<IWebElement> actualAssoClubcards = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOCLUBCARDS).XPath)));
                //Assert.AreEqual(actualAssoMessage, AssoSalutation);           
                expectedMainClubcards = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "ClubCardID");
                int iCheck = 0;
                for (int i = 0; i < expectedMainClubcards.Count; i++)
                {

                    if (actualMainClubcards[i].Text.ToString().Equals(Extension.MasknFormatClubcard(expectedMainClubcards[i].ToString(), true, 'X')))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Main Customer's Clubcards are not same", TraceEventType.Start);
                    }

                }
                Assert.AreEqual(iCheck, expectedMainClubcards.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

        }

        public void ViewMyCard_Asso_Cards_verify(string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedAssoClubcards = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                int count = clubcard.GetHouseholdCustomersCount(customerId, CountrySetting.culture);

                if (count > 1)
                {
                    Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                    AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                    ReadOnlyCollection<IWebElement> actualAssoClubcards = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOCLUBCARDS).XPath)));
                    expectedAssoClubcards = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "ClubCardID");
                    int iCheck = 0;
                    for (int i = 0; i < expectedAssoClubcards.Count; i++)
                    {

                        if (actualAssoClubcards[i].Text.ToString().Equals(Extension.MasknFormatClubcard(expectedAssoClubcards[i].ToString(), true, 'X')))
                        {
                            iCheck++;
                        }
                        else
                        {
                            CustomLogs.LogMessage("Associate Customer's Clubcards are not same", TraceEventType.Start);
                        }

                    }
                    Assert.AreEqual(iCheck, expectedAssoClubcards.Count);
                }
                else
                {
                    CustomLogs.LogMessage("There is no Associate customer to this account", TraceEventType.Stop);
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_ShowCardTypeHeader_Verify(string TypeofCard, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> MainClubcards = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                string hdrTypeofCard = Driver.FindElement(By.Id(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_SHOWTYPEOFCARD).Id)).Text;
                Resource res1 = AutomationHelper.GetResourceMessage(TypeofCard, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                Assert.AreEqual(hdrTypeofCard, res1.Value);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_ShowCardType_Verify(string TypeofCard, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> MainClubcards = new List<string>();
                List<string> expectedTypeofCard = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                ReadOnlyCollection<IWebElement> actualTypeofCardUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_SHOWTYPEOFCARDDETAILS).XPath)));
                expectedTypeofCard = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "ClubCardTypeDesc");
                int iCheck = 0;
                for (int i = 0; i < expectedTypeofCard.Count; i++)
                {

                    if (actualTypeofCardUsed[i].Text.ToString().Trim().Equals(expectedTypeofCard[i].ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Main Customer's Where Used details are not same", TraceEventType.Start);
                    }

                }
                Assert.AreEqual(iCheck, expectedTypeofCard.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_Asso_ShowCardType_Verify(string TypeofCard, string ClubcardID, string resourceFileName)
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                List<string> MainClubcards = new List<string>();
                List<string> expectedTypeofCard = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                int count = clubcard.GetHouseholdCustomersCount(customerId, CountrySetting.culture);
                if (count > 1)
                {
                    Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                    AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                    ReadOnlyCollection<IWebElement> actualTypeofCardUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSO_SHOWTYPEOFCARDDETAILS).XPath)));
                    expectedTypeofCard = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "ClubCardTypeDesc");
                    int iCheck = 0;
                    for (int i = 0; i < expectedTypeofCard.Count; i++)
                    {

                        if (actualTypeofCardUsed[i].Text.ToString().Trim().Equals(expectedTypeofCard[i].ToString().Trim()))
                        {
                            iCheck++;
                        }
                        else
                        {
                            CustomLogs.LogMessage("Main Customer's Where Used details are not same", TraceEventType.Start);
                        }

                    }
                    Assert.AreEqual(iCheck, expectedTypeofCard.Count);
                }
                else
                {
                    CustomLogs.LogMessage("There is no Associate customer to this account", TraceEventType.Stop);
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_Main_WhereUsed_verify(string TypeofCard, string WhereUsed, string DateTimeFormat, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedWhereUsed = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                Thread.Sleep(5000);
                ReadOnlyCollection<IWebElement> detailsWhereUsed;
                if (TypeofCard == "TRUE")
                {
                    detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_WHEREUSEDWITHTYPEOFCARD).XPath)));
                }
                else
                    detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_WHEREUSEDDETAILS).XPath)));

                expectedWhereUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "StoreName");
                int iCheck = 0;
                for (int i = 0; i < expectedWhereUsed.Count; i++)
                {
                    if (string.IsNullOrEmpty(expectedWhereUsed[i]))
                    {
                        expectedWhereUsed[i] = string.Empty;
                    }

                    if (detailsWhereUsed[i].Text.ToString().Trim().Equals(expectedWhereUsed[i].ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Main Customer's Where Used details are not same", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, expectedWhereUsed.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_Asso_WhereUsed_verify(string TypeofCard, string WhereUsed, string DateTimeFormat, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedWhereUsed = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                Thread.Sleep(5000);
                ReadOnlyCollection<IWebElement> detailsWhereUsed;
                if (TypeofCard == "TRUE")
                {
                    detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOWHEREUSEDWITHTYPEOFCARD).XPath)));
                }
                else
                    detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSO_WHEREUSEDDETAILS).XPath)));

                expectedWhereUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "StoreName");
                int iCheck = 0;
                for (int i = 0; i < expectedWhereUsed.Count; i++)
                {
                    if (string.IsNullOrEmpty(expectedWhereUsed[i]))
                    {
                        expectedWhereUsed[i] = string.Empty;
                    }

                    if (detailsWhereUsed[i].Text.ToString().Trim().Equals(expectedWhereUsed[i].ToString().Trim()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Main Customer's Where Used details are not same", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, expectedWhereUsed.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_WhereUsedHeader_verify(string WhereUsed, string DateTimeFormat, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedWhereUsed = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                ReadOnlyCollection<IWebElement> hdrWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_WHEREUSED).XPath)));
                string ExpectedHeader = AutomationHelper.GetResourceMessage(WhereUsed, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName)).Value;
                ReadOnlyCollection<IWebElement> detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_WHEREUSEDDETAILS).XPath)));
                Assert.AreEqual(hdrWhereUsed[0].Text, ExpectedHeader);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_LastUsed_Main_verify(string ShowTypeofCard, string DateTimeFormat, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                DBConfiguration transactionTypeConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.TRANSACTION_TYPE, SanityConfiguration.DbConfigurationFile);
                string transactionType = transactionTypeConfig.ConfigurationValue1.ToString();
                List<string> expectedLastUsed = new List<string>(), expectedTransactionType = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.VALIDATIONMORESIXMONTHS, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                ReadOnlyCollection<IWebElement> detailsLastUsed;
                detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_LASTUSED).XPath)));
                expectedLastUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionDateTime");
                expectedTransactionType = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionType");
                int iCheck = 0;
                for (int i = 0; i < expectedLastUsed.Count; i++)
                {
                    DateTime lastdate;
                    if (string.IsNullOrEmpty(expectedLastUsed[i]))
                    {
                        expectedLastUsed[i] = res1.Value;
                    }
                    if (DateTime.TryParse(expectedLastUsed[i].ToString(), out lastdate))
                    {
                        if (lastdate < (DateTime.Now.AddMonths(-6)))
                        {
                            expectedLastUsed[i] = res1.Value;
                        }
                        else if (expectedTransactionType[i].Equals(transactionType))
                        {
                            expectedLastUsed[i] = "-";
                        }
                        else
                        {
                            expectedLastUsed[i] = lastdate.ToString(DateTimeFormat);
                        }
                    }
                    if (detailsLastUsed[i].Text.Equals(expectedLastUsed[i].ToString()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Main Customer's Last used values are not same", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, expectedLastUsed.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public void ViewMyCard_LastUsed_Asso_verify(string ShowTypeofCard, string DateTimeFormat, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                DBConfiguration transactionTypeConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.TRANSACTION_TYPE, SanityConfiguration.DbConfigurationFile);
                string transactionType = transactionTypeConfig.ConfigurationValue1.ToString();
                List<string> expectedLastUsed = new List<string>(), expectedTransactionType = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.VALIDATIONMORESIXMONTHS, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                ReadOnlyCollection<IWebElement> detailsLastUsed;
                detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOLASTUSED).XPath)));
                expectedLastUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionDateTime");
                expectedTransactionType = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionType");
                int iCheck = 0;
                for (int i = 0; i < expectedLastUsed.Count; i++)
                {
                    DateTime lastdate;
                    if (string.IsNullOrEmpty(expectedLastUsed[i]))
                    {
                        expectedLastUsed[i] = res1.Value;
                    }
                    if (DateTime.TryParse(expectedLastUsed[i].ToString(), out lastdate))
                    {
                        if (lastdate < (DateTime.Now.AddMonths(-6)))
                        {
                            expectedLastUsed[i] = res1.Value;
                        }
                        else if (expectedTransactionType[i].Equals(transactionType))
                        {
                            expectedLastUsed[i] = "-";
                        }
                        else
                        {
                            expectedLastUsed[i] = lastdate.ToString(DateTimeFormat);
                        }
                    }
                    if (detailsLastUsed[i].Text.Equals(expectedLastUsed[i].ToString()))
                    {
                        iCheck++;
                    }
                    else
                    {
                        CustomLogs.LogMessage("Associate Customer's Last used values are not same", TraceEventType.Start);
                    }

                }

                Assert.AreEqual(iCheck, expectedLastUsed.Count);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        //private string GetSalutation(string msgID, Dictionary<string, string> customerDetails)
        //{
        //    string config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
        //    string config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEFIRSTNAMEINSALUTATION, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
        //    bool IsTitleHide = config == "1" ? true : false;
        //    bool IsFirstNameHideinSalutation = config1 == "1" ? true : false;
        //    string Salutation = msgID + " " + FullyQualifiedName(customerDetails, IsTitleHide, IsFirstNameHideinSalutation);
        //    //string Salutation = msgID + " " + customerDetails["TitleEnglish"].ToString() + " " + customerDetails["Name1"].Substring(0, 1) + " " + (customerDetails["Name3"].Substring(0, 1).ToUpper() + customerDetails["Name3"].Substring(1).ToLower().Trim());
        //    return Salutation;
        //}

        public string GetSalutation(string msgID, string pageName, DataRow customerDetails)
        {
            string displayName = string.Empty;
            List<bool> isNameDisplay = new List<bool>();
            List<bool> isNameAbbreviated = new List<bool>();
            List<int> isNameCased = new List<int>();
            try
            {
                List<DBConfiguration> nameDisplayList = new List<DBConfiguration>();
                List<DBConfiguration> nameAbbreviationList = new List<DBConfiguration>();
                List<DBConfiguration> textCasingList = new List<DBConfiguration>();

                List<Control> allControls = new List<Control>();
                nameDisplayList = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.DisplayFunctionality, SanityConfiguration.DbConfigurationFile);
                nameAbbreviationList = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.FieldAbbreviation, SanityConfiguration.DbConfigurationFile);
                textCasingList = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.TextCasing, SanityConfiguration.DbConfigurationFile);

                List<string> name1CaseList = textCasingList.Count > 0 ? textCasingList[0].ConfigurationValue1.Split(',').ToList() : new List<string>();
                List<string> name2CaseList = textCasingList.Count > 1 ? textCasingList[1].ConfigurationValue1.Split(',').ToList() : new List<string>();
                List<string> name3CaseList = textCasingList.Count > 2 ? textCasingList[2].ConfigurationValue1.Split(',').ToList() : new List<string>();

                bool isTitleDisplay = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1.Equals("0");

                isNameDisplay = nameDisplayList.Select(id => ((id.ConfigurationValue2.ToUpper().Contains(pageName) && id.ConfigurationValue1.Equals("1")) || (!id.ConfigurationValue2.ToUpper().Contains(pageName) && id.ConfigurationValue1.Equals("0")))).ToList();
                isNameAbbreviated = nameAbbreviationList.Select(id => ((id.ConfigurationValue2.ToUpper().Contains(pageName) && id.ConfigurationValue1.Equals("1")) || (!id.ConfigurationValue2.ToUpper().Contains(pageName) && id.ConfigurationValue1.Equals("0")))).ToList();
                string Name1Case = name1CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));
                string Name2Case = name2CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));
                string Name3Case = name3CaseList.FirstOrDefault(id => id.ToUpper().Contains(pageName));

                string name1 = GetFormattedValue(customerDetails["Name1"].ToString(), isNameDisplay.Count > 0 ? isNameDisplay[0] : true, isNameAbbreviated.Count > 0 ? isNameAbbreviated[0] : false, string.IsNullOrEmpty(Name1Case) ? string.Empty : Name1Case.Substring(Name1Case.Length - 1));
                string name2 = GetFormattedValue(customerDetails["Name2"].ToString(), isNameDisplay.Count > 1 ? isNameDisplay[1] : true, isNameAbbreviated.Count > 1 ? isNameAbbreviated[1] : false, string.IsNullOrEmpty(Name2Case) ? string.Empty : Name2Case.Substring(Name2Case.Length - 1));
                string name3 = GetFormattedValue(customerDetails["Name3"].ToString(), isNameDisplay.Count > 2 ? isNameDisplay[2] : true, isNameAbbreviated.Count > 2 ? isNameAbbreviated[2] : false, string.IsNullOrEmpty(Name3Case) ? string.Empty : Name3Case.Substring(Name3Case.Length - 1));

                string title = isTitleDisplay ? customerDetails["TitleEnglish"].ToString().Trim() : string.Empty;
                string[] data = new string[] { msgID, title, name1, name2, name3 };
                displayName = string.Join(" ", data);
                data = displayName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                displayName = string.Join(" ", data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return displayName;
        }

        public string GetFormattedValue(string nameInput, bool isVisible, bool isInitial, string TextCasing)
        {
            string output = string.Empty;
            NameCasingEnum result;

            output = !string.IsNullOrEmpty(nameInput) ? isVisible ? isInitial ? nameInput.FirstOrDefault().ToString() : nameInput : string.Empty : string.Empty;
            if (!string.IsNullOrEmpty(output))
            {
                if (Enum.TryParse(TextCasing, out result))
                {
                    switch (result)
                    {
                        case NameCasingEnum.Camel:
                            TextInfo textInfo = new CultureInfo(CountrySetting.culture, false).TextInfo;
                            output = textInfo.ToTitleCase(output);
                            break;
                        case NameCasingEnum.Lower:
                            output = output.ToLower();
                            break;
                        case NameCasingEnum.Upper:
                            output = output.ToUpper();
                            break;
                    }
                }
            }

            return output;
        }



        //private string FullyQualifiedName(Dictionary<string, string> customerDetails, bool IsTitleHide, bool IsFirstNameHideinSalutation)
        //{
        //    string name1 = string.Empty;
        //    string title = string.Empty;
        //    if (!IsTitleHide)
        //    {
        //        title = string.IsNullOrEmpty(customerDetails["TitleEnglish"]) ? string.Empty : customerDetails["TitleEnglish"];
        //    }
        //    if (IsFirstNameHideinSalutation == true)
        //    {
        //        name1 = string.IsNullOrEmpty(customerDetails["Name1"]) ? string.Empty : customerDetails["Name1"].Substring(0, 1);
        //    }
        //    else
        //    {
        //        name1 = string.IsNullOrEmpty(customerDetails["Name1"]) ? string.Empty : customerDetails["Name1"];
        //    }
        //    string name3 = string.IsNullOrEmpty(customerDetails["Name3"]) ? string.Empty : customerDetails["Name3"];
        //    string fullName = title + " " + name1 + " " + name3;
        //    return fullName.Trim();
        //}
        public void TextStatement1()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.Statement1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_TextStatement1).XPath)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void TextStatement2()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedTextStart = AutomationHelper.GetResourceMessage(ValidationKey.Statement2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_TextStatement2).XPath)).Text;
            var expectedTextContacts = AutomationHelper.GetResourceMessage(ValidationKey.Statement2Contact1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextContact2 = AutomationHelper.GetResourceMessage(ValidationKey.Statement2Contact2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextstar = AutomationHelper.GetResourceMessage(ValidationKey.Statement2Star1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextstar2 = AutomationHelper.GetResourceMessage(ValidationKey.Statement2Star2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedText = string.Concat(expectedTextStart, " ", expectedTextContacts, " ", expectedTextstar, " ", expectedTextContact2, " ", expectedTextstar2);
            Assert.AreEqual(expectedText, actualText);
        }
        public void FAQText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_TextStatement3).XPath)).Text;
            var expectedTextstart = AutomationHelper.GetResourceMessage(ValidationKey.Statement3start, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextmid = AutomationHelper.GetResourceMessage(ValidationKey.Statement3mid, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextlink = AutomationHelper.GetResourceMessage(ValidationKey.Statement3link, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedTextend = AutomationHelper.GetResourceMessage(ValidationKey.Statement3end, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedText = string.Concat(expectedTextstart, "", expectedTextmid, " ", expectedTextlink, " ", expectedTextend);
            Assert.AreEqual(expectedText, actualText);
        }
        public void Telephoniccommunication()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText1 = AutomationHelper.GetResourceMessage(ValidationKey.PhoneStatement1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var expectedText2 = AutomationHelper.GetResourceMessage(ValidationKey.PhoneStatement2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.MANAGECARDS_RESOURCE)).Value;
            var actualText1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_TextStatement4).XPath)).Text;
            var actualText = actualText1.Replace("\r\n", " ");
            var expectedText = string.Concat(expectedText1, " ", expectedText2);
            Assert.AreEqual(expectedText, actualText);

        }
        public void CardReplacementText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_CardReplacement, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_CardReplacement).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void safelyDystroyText()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_SafelyDystroy, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_SafelyDestroy).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void LostCard()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var actualText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_lostcard).XPath)).Text;
            var expectedTextstart = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_LostCard, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedTextcontact1 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_LostCardContact1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedTextcondition1 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_LostCardcondition1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedTextcontact2 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_LostCardcontact2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedTextcondition2 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_LostCardcondition2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedText = string.Concat(expectedTextstart, " ", expectedTextcontact1, "", expectedTextcondition1, " ", expectedTextcontact2, "", expectedTextcondition2);
            Assert.AreEqual(expectedText, actualText);
        }
        public void OrderAReplacement()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_OrderAReplacement, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_OrderAReplacement).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }
        public void RequestReason()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_RequestReason, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RequestReason).Id)).Text;
            Assert.AreEqual(expectedText, actualText);
        }

        public void communication()
        {
            Driver = ObjAutomationHelper.WebDriver;
            var expectedText1 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_PhoneStatement1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var expectedText2 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_PhoneStatement2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
            var actualText1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_Communication).XPath)).Text;
            var actualText = actualText1.Replace("\r\n", " ");
            var expectedText = string.Concat(expectedText1, " ", expectedText2);
            Assert.AreEqual(expectedText, actualText);

        }

        public bool VerifyWelcomeMessageName(string xmlFile, string clubcardID)
        {
            bool chk = false;
            string expectedMessageName =string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying Welcome Message on Home Page ", TraceEventType.Start);
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string actualMessageName = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOME_WELCOME_MSGNAME).Id)).Text;
                long customerId = Convert.ToInt64(customer.GetCustomerID(clubcardID, CountrySetting.culture));

                Resource res = AutomationHelper.GetResourceMessage(LabelKey.HOMETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                   DataSet cusData = new DataSet();
                    cusData = clubcard.GetCustomers(customerId, CountrySetting.culture);
                    if (cusData.Tables[0].Rows.Count > 0)
                    {
                        expectedMessageName = res.Value + " " + GetSalutation(string.Empty, "HOME", cusData.Tables[0].Rows[0]);
                    }
                Assert.AreEqual(actualMessageName, expectedMessageName, true);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }

            return chk;
        }


        #endregion
    }









}


