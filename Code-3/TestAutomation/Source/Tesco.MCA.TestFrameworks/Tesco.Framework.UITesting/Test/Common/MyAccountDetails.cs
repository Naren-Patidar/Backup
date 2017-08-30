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
using Tesco.Framework.UITesting.Helpers;
using System.IO;

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


        #region View My Card Test Scenarios

        public void ViewMyCard_Common_Salutation_verify(string msgIDNameofAccount, string msgIDAndSeprator, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string MainSalutation = string.Empty;  long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                int count = clubcard.GetHouseholdCustomersCount(customerId, CountrySetting.culture);
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(msgIDNameofAccount, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                Resource res2 = AutomationHelper.GetResourceMessage(msgIDAndSeprator, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                MainSalutation = GetSalutation(string.Empty, MaincustomerDetails);
                string actualMainMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_MAINMSG).XPath)).Text;
                string actualCommonMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_COMMONMSG).XPath)).Text;
                string expectedCommonMessage = string.Empty;
                if (count > 1)
                {
                    string AssoSalutation = string.Empty;
                    Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                    AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                    AssoSalutation = GetSalutation(string.Empty, AssocustomerDetails);
                    string actualAssoMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOMSG).XPath)).Text;
                    expectedCommonMessage = res1.Value + MainSalutation + " " + res2.Value.Trim() +  AssoSalutation;
                    Assert.AreEqual(actualCommonMessage, expectedCommonMessage, true);
                }
                else
                {
                    expectedCommonMessage = res1.Value + " " + MainSalutation;
                    Assert.AreEqual(actualMainMessage, expectedCommonMessage, true);
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

        public void ViewMyCard_Main_Salutation_verify(string msgIDMain, string msgIDAsso, string msgCardHolder, string ClubcardID, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string MainSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(msgIDMain, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                Resource res3 = AutomationHelper.GetResourceMessage(msgCardHolder, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                string msgMain = res1.Value + " " + res3.Value;
                MainSalutation = GetSalutation(msgMain, MaincustomerDetails);
                string actualMainMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_MAINMSG).XPath)).Text;
                Assert.AreEqual(actualMainMessage, MainSalutation,true);

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
                    Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                    AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                    Resource res2 = AutomationHelper.GetResourceMessage(msgIDAsso, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    Resource res3 = AutomationHelper.GetResourceMessage(msgCardHolder, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    string msgAsso = res2.Value + " " + res3.Value;
                    AssoSalutation = GetSalutation(msgAsso, AssocustomerDetails);
                    string actualAssoMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOMSG).XPath)).Text;
                    Assert.AreEqual(actualAssoMessage, AssoSalutation,true);
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

        public void ViewMyCard_Main_WhereUsed_verify(string WhereUsed, string DateTimeFormat, string ClubcardID, string resourceFileName)
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
                ReadOnlyCollection<IWebElement> detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_WHEREUSEDDETAILS).XPath)));
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

        public void ViewMyCard_Asso_WhereUsed_verify(string WhereUsed, string DateTimeFormat, string ClubcardID, string resourceFileName)
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
                ReadOnlyCollection<IWebElement> detailsWhereUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSO_WHEREUSEDDETAILS).XPath)));
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
                List<string> expectedLastUsed = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                string AssoSalutation = string.Empty;
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
                MaincustomerDetails = clubcard.GetMaincustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.VALIDATIONMORESIXMONTHS, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                ReadOnlyCollection<IWebElement> detailsLastUsed;
                if (ShowTypeofCard=="TRUE")
                {
                    detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_LASTUSEDWITHTYPEOFCARD).XPath)));
                }
                else
                    detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_LASTUSED).XPath)));

                expectedLastUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(MaincustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionDateTime");
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
                            expectedLastUsed[i] = res1.Value;
                        else
                            expectedLastUsed[i] = lastdate.ToString(DateTimeFormat);
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
                List<string> expectedLastUsed = new List<string>();
                CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                long customerId = Convert.ToInt64(customer.GetCustomerID(ClubcardID, CountrySetting.culture));
                ClubcardServiceAdapter clubcard = new ClubcardServiceAdapter();
                Dictionary<string, string> AssocustomerDetails = new Dictionary<string, string>();
                AssocustomerDetails = clubcard.GetAssociatecustomerDetails(customerId, CountrySetting.culture);
                Resource res1 = AutomationHelper.GetResourceMessage(ValidationKey.VALIDATIONMORESIXMONTHS, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                ReadOnlyCollection<IWebElement> detailsLastUsed;
                if (ShowTypeofCard == "TRUE")
                {
                    detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOLASTUSEDWITHTYPEOFCARD).XPath)));
                }
                else
                    detailsLastUsed = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ACCOUNT_MC_ASSOLASTUSED).XPath)));

                expectedLastUsed = clubcard.GetClubcardsCustomer(Convert.ToInt64(AssocustomerDetails["CustomerID"].ToString()), CountrySetting.culture, "TransactionDateTime");
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
                            expectedLastUsed[i] = res1.Value;
                        else
                            expectedLastUsed[i] = lastdate.ToString(DateTimeFormat);
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

        private string GetSalutation(string msgID, Dictionary<string, string> customerDetails)
        {
            string config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            string config1 = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEFIRSTNAMEINSALUTATION, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
            bool IsTitleHide = config == "1" ? true : false;
            bool IsFirstNameHideinSalutation = config1 == "1" ? true : false;
            string Salutation = msgID + " " + FullyQualifiedName(customerDetails, IsTitleHide, IsFirstNameHideinSalutation);
            //string Salutation = msgID + " " + customerDetails["TitleEnglish"].ToString() + " " + customerDetails["Name1"].Substring(0, 1) + " " + (customerDetails["Name3"].Substring(0, 1).ToUpper() + customerDetails["Name3"].Substring(1).ToLower().Trim());
            return Salutation;
        }

        private string FullyQualifiedName(Dictionary<string, string> customerDetails, bool IsTitleHide, bool IsFirstNameHideinSalutation)
        {
            string name1 = string.Empty;
            string title = string.Empty;
            if (!IsTitleHide)
            {
                title = string.IsNullOrEmpty(customerDetails["TitleEnglish"]) ? string.Empty : customerDetails["TitleEnglish"];
            }
            if (IsFirstNameHideinSalutation == true)
            {
                name1 = string.IsNullOrEmpty(customerDetails["Name1"]) ? string.Empty : customerDetails["Name1"].Substring(0, 1);
            }
            else
            {
                name1 = string.IsNullOrEmpty(customerDetails["Name1"]) ? string.Empty : customerDetails["Name1"];
            }
            string name3 = string.IsNullOrEmpty(customerDetails["Name3"]) ? string.Empty : customerDetails["Name3"];
            string fullName = title + " " + name1 + " " + name3;
            return fullName.Trim();
        }

        #endregion

    }

        #endregion
}


