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
using Tesco.Framework.UITesting.Constants;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using System.Globalization;
using OpenQA.Selenium.Support.UI;
using System.Xml.Linq;
using System.Configuration;

namespace Tesco.Framework.UITesting.Test.Common
{
    class MyPoints : Base
    {
        #region Constructor


        public MyPoints(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }
        public MyPoints(AutomationHelper objHelper, AppConfiguration configuration)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            TestData = ObjAutomationHelper.GetTestDataByID(Enums.Messages.Login);
            SanityConfiguration = configuration;
        }
        #endregion

        #region Public Methods

        public bool myCurrentPoint_click()
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.CURRENTDETAILSLINK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                var expectedLinkName = res.Value;
                var actualLinkName = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_CLICK).XPath)).Text).ToString();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if (expectedLinkName.Equals(actualLinkName, StringComparison.OrdinalIgnoreCase))
                {
                    Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_CLICK).XPath)).Click();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected"));
                    CustomLogs.LogInformation(expectedLinkName + "is equal to" + actualLinkName + "So My Current  Points clicked");
                }
                else
                {
                    CustomLogs.LogInformation("My Current  Points Link Not Present");
                    Assert.Fail("My Current Points Link Not Present");
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
            }
            return true;
        }

        public bool myPoint_DetailsClick(int periodIndex)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_COLLECTION_PERIOD_TABLE).ClassName));
                IWebElement ctrl = table.FindElements(By.TagName("tr"))[periodIndex].FindElements(By.TagName("a")).LastOrDefault();
                if (ctrl != null)
                {
                    ctrl.Click();
                }
                else
                {
                    Assert.Inconclusive(string.Format("Transaction details for {0} not exist for this card", periodIndex));
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
            }
            return true;
        }


        /// <summary>
        /// Method to validate collection periods in grid on my points page
        /// </summary>
        public void Validate_CollectionPeriodGrid(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                DBConfiguration dateConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DATERANGE_FOR_COLLECTION_PERIOD, SanityConfiguration.DbConfigurationFile);
                bool isDateRangeEnabled = dateConfig.IsDeleted.ToUpper().Equals("N") && dateConfig.ConfigurationValue1.ToUpper().Equals("TRUE");
                DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
                IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_COLLECTION_PERIOD_TABLE).ClassName));
                List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> expected_periods = client.GetCollectionPeriods(customerID, culture, isDateRangeEnabled, dateFormatConfig.ConfigurationValue1);
                int index = 1;
                foreach (string p in expected_periods)
                {
                    if (!rows[index].Text.Contains(p))
                    {
                        error += string.Format(" Collection Period: {0} not found in grid row no: {1} ", p, index);
                    }
                    index++;
                }
                Assert.AreEqual(error, string.Empty);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Transaction Details Grid");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that clubcard is masked in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateClubcardMasked(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        for (int i = 1; i < rows.Count - 1; i++)
                        {
                            IWebElement td = rows[i].FindElements(By.TagName("td")).FirstOrDefault();
                            if (td != null && !td.Text.Contains(" XXXX XXXX "))
                            {
                                error += string.Format(" Clubcard at row Index: {0} is not masked, Actual : {1}", i, td.Text);
                            }
                        }
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that transaction dates are as expected  in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateTransactionDate(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        for (int i = 1; i < rows.Count - 1; i++)
                        {
                            IWebElement td = rows[i].FindElements(By.TagName("td"))[1];
                            if (transactions[i - 1] == null)
                            {
                                error += string.Format(" Transaction date at row Index: {0} is not matched ", i);
                            }
                            else if (td != null && td.Text != transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1))
                            {
                                error += string.Format(" Transaction date at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1));
                            }
                        }
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that transaction details (store) are as expected in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateTransactionDetails(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        for (int i = 1; i < rows.Count - 1; i++)
                        {
                            IWebElement td = rows[i].FindElements(By.TagName("td"))[2];
                            if (transactions[i - 1] == null)
                            {
                                error += string.Format(" Transaction Description (Store) at row Index: {0} is not matched ", i);
                            }
                            else if (td != null && td.Text != transactions[i - 1].TransactionDescription)
                            {
                                error += string.Format(" Transaction Description  (Store) at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].TransactionDescription);
                            }
                        }
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that bonus points are as expected in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateBonusPoints(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
                if (disableBonusPointsConfig.IsDeleted.Equals("N") && disableBonusPointsConfig.ConfigurationValue1.Equals("1"))
                {
                    ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                    string offerID = client.GetOfferID(customerID, culture, 0);
                    string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                    List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                    if (transactions.Count > 0)
                    {
                        IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        if (table != null)
                        {
                            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                            for (int i = 1; i < rows.Count - 1; i++)
                            {
                                IWebElement td = rows[i].FindElements(By.TagName("td"))[4];
                                if (transactions[i - 1] == null)
                                {
                                    error += string.Format(" Transaction Bonus Points at row Index: {0} is not matched ", i);
                                }
                                else if (td != null && td.Text != transactions[i - 1].BonusPoints)
                                {
                                    error += string.Format("Transaction Bonus Points at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].BonusPoints);
                                }
                            }
                            Assert.AreEqual(error, string.Empty);
                        }
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                    }
                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that total points are as expected in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateTotalPoints(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
                bool isBonusVisible = (disableBonusPointsConfig.IsDeleted.Equals("N") && disableBonusPointsConfig.ConfigurationValue1.Equals("1"));
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        for (int i = 1; i < rows.Count - 1; i++)
                        {
                            IWebElement td = rows[i].FindElements(By.TagName("td"))[isBonusVisible ? 5 : 4];
                            if (transactions[i - 1] == null)
                            {
                                error += string.Format(" Transaction Total Points at row Index: {0} is not matched ", i);
                            }
                            else if (td != null && td.Text != transactions[i - 1].NormalPoints)
                            {
                                error += string.Format(" Transaction Total Points at row Index: {0} is not matched , Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].NormalPoints);
                            }
                        }
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate that total spend amount are as expected in transaction grid on My Current Points Details page
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_ValidateTotalSpend(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                Resource currencySymbol = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYSYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                Resource currencySymbolAlpha = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYALPHASYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                DBConfiguration DisableDecimalConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_CURRENCY_DECIMAL, SanityConfiguration.DbConfigurationFile);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        for (int i = 1; i < rows.Count - 1; i++)
                        {
                            List<IWebElement> tds = rows[i].FindElements(By.TagName("td")).ToList();
                            if (tds.Count > 3)
                            {
                                IWebElement td = tds[3];
                                if (td != null)
                                {
                                    string actualValue = transactions[i - 1].AmountSpent;
                                    string formattedVal = actualValue;
                                    if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                                    {
                                        formattedVal = (actualValue.Contains(',') ? actualValue.TrimEnd('0').TrimEnd(',') : actualValue.Contains('.') ? actualValue.TrimEnd('0').TrimEnd('.') : formattedVal);
                                        formattedVal = formattedVal.Contains('.') ? actualValue : formattedVal.Contains(',') ? actualValue : formattedVal;
                                    }
                                    if (td != null && td.Text != string.Format("{0}{1}{2}", currencySymbol.Value, formattedVal, currencySymbolAlpha.Value))
                                    {
                                        error += string.Format("Transaction Amount spent at row Index: {0} is not matched. Actual : {1}, Expected {2}", i, td.Text, formattedVal);
                                    }
                                }
                            }
                        }
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Masked clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        public void Validate_PreviousTransactions(long customerID, string culture, int index)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, index);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in Validate_PreviousTransactions");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate the search first clubcard on current transaction details
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_SearchCustomer(long customerID, string culture, int index)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement select = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_SELECT_CLUBCARD).Id));
                    SelectElement dropdown = new SelectElement(select);
                    if (dropdown.Options.Count > index)
                    {
                        string clubcard = dropdown.Options[index].Text;
                        dropdown.SelectByIndex(index);
                        int expected_Rows = transactions.FindAll(t => AutomationHelper.GetMaskedClubcard(t.ClubcardId) == clubcard).Count;
                        int actual_Rows = 0;
                        IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        if (table != null)
                        {
                            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                            actual_Rows = rows.Count - 2;
                        }
                        Assert.AreEqual(expected_Rows, actual_Rows, string.Format("Expected transaction count : {0} , actual transaction count : {1}", expected_Rows, actual_Rows));
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("There are no clubcard in search clubcard dropdown at index: {1}", index));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate the search store on current transaction details
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <param name="index"></param>
        public void Validate_SearchStore(long customerID, string culture, int index)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement select = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_SELECT_STORE).Id));
                    SelectElement dropdown = new SelectElement(select);
                    if (dropdown.Options.Count > index)
                    {
                        string store = dropdown.Options[index].Text;
                        dropdown.SelectByIndex(index);
                        int expected_Rows = transactions.FindAll(t => t.TransactionDescription == store).Count;
                        int actual_Rows = 0;
                        IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        if (table != null)
                        {
                            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                            actual_Rows = rows.Count - 2;
                        }
                        Assert.AreEqual(expected_Rows, actual_Rows, string.Format("Expected transaction count : {0} , actual transaction count : {1}", expected_Rows, actual_Rows));
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("There are no clubcard in search clubcard dropdown at index: {1}", index));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate the search clubcard and store on current transaction details
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <param name="index"></param>
        public void Validate_SearchStoreAndClubcard(long customerID, string culture, int index)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    IWebElement select_store = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_SELECT_STORE).Id));
                    SelectElement dropdown_store = new SelectElement(select_store);
                    if (dropdown_store.Options.Count > index)
                    {
                        string store = dropdown_store.Options[index].Text;
                        dropdown_store.SelectByIndex(index);
                        IWebElement select_clubcard = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_SELECT_CLUBCARD).Id));
                        SelectElement dropdown_clubcard = new SelectElement(select_clubcard);
                        if (dropdown_clubcard.Options.Count > index)
                        {
                            string clubcard = dropdown_clubcard.Options[index].Text;
                            dropdown_clubcard.SelectByIndex(index);
                            int expected_Rows = transactions.FindAll(t => t.TransactionDescription == store && AutomationHelper.GetMaskedClubcard(t.ClubcardId) == clubcard).Count;
                            int actual_Rows = 0;
                            IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                            if (table != null)
                            {
                                List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                                actual_Rows = rows.Count - 2;
                            }
                            Assert.AreEqual(expected_Rows, actual_Rows, string.Format("Expected transaction count : {0} , actual transaction count : {1}", expected_Rows, actual_Rows));
                        }
                        else
                        {
                            Assert.Inconclusive(string.Format("There are no clubcard in search clubcard dropdown at index: {1}", index));
                        }
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("There are no store in search store dropdown at index: {1}", index));
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate sorting by clubcard
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_SortByClubcard(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    transactions.ForEach(t => t.ClubcardId = AutomationHelper.GetMaskedClubcard(t.ClubcardId));
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[0];
                        sort_anchor.Click();
                        transactions = transactions.OrderBy(t => t.ClubcardId).ToList();
                        table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        rows = table.FindElements(By.TagName("tr")).ToList();
                        anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        sort_anchor = anchors[0];
                        Assert.AreEqual(error, string.Empty, error);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// Method to validate sorting by store
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_SortByStore(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    transactions.ForEach(t => t.ClubcardId = AutomationHelper.GetMaskedClubcard(t.ClubcardId));
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[2];
                        sort_anchor.Click();
                        transactions = transactions.OrderBy(t => t.TransactionDescription).ToList();
                        table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        rows = table.FindElements(By.TagName("tr")).ToList();
                        anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        sort_anchor = anchors[0];
                        Assert.AreEqual(error, string.Empty, error);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate sorting by transaction Date
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        public void Validate_SortByDate(long customerID, string culture)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    transactions.ForEach(t => t.ClubcardId = AutomationHelper.GetMaskedClubcard(t.ClubcardId));
                    IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[1];
                        sort_anchor.Click();
                        transactions = transactions.OrderBy(t => t.TransactionDateTime).ToList();
                        table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        rows = table.FindElements(By.TagName("tr")).ToList();
                        anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        sort_anchor = anchors[0];
                        Assert.AreEqual(error, string.Empty, error);
                    }
                }
                else
                {
                    Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                }

                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in search clubcard clubcard in transaction page");
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to validate click on Current Minus One View Link for Points Summary Page
        /// </summary>
        public bool PointSummary_CurrentMinusOne_click()
        {
            try
            {
                CustomLogs.LogMessage("Current Minus One Points Summary Click started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.CURRENTMINUSONEPOINTSSUMMARYLINK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var expectedLinkName = res.Value;
                var actualLinkName = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK).Id)).Text).ToString();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                //  if (expectedLinkName == actualLinkName)
                if (expectedLinkName.Equals(actualLinkName, StringComparison.OrdinalIgnoreCase))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK).Id)).Click();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected"));
                    CustomLogs.LogInformation(expectedLinkName + "is equal to" + actualLinkName + "So My Current  Points clicked");
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
                }
                else
                {
                    CustomLogs.LogInformation("My Current  Points Link Not Present");
                    Assert.Fail("My Current Points Link Not Present");
                }
                CustomLogs.LogMessage("Current Minus One Points Summary Click ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate click on Current Minus Two View Link for Points Summary Page
        /// </summary>
        public bool PointSummary_CurrentMinusTwo_click()
        {
            try
            {
                CustomLogs.LogMessage("Current Minus One Points Summary Click started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.CURRENTMINUSONEPOINTSSUMMARYLINK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var expectedLinkName = res.Value;
                var actualLinkName = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous2Summary_CLICK).Id)).Text).ToString();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                //  if (expectedLinkName == actualLinkName)
                if (expectedLinkName.Equals(actualLinkName, StringComparison.OrdinalIgnoreCase))
                {
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous2Summary_CLICK).Id)).Click();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected"));
                    CustomLogs.LogInformation(expectedLinkName + "is equal to" + actualLinkName + "So My Current  Points clicked");
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
                }
                else
                {
                    CustomLogs.LogInformation("My Current  Points Link Not Present");
                    Assert.Fail("My Current Points Link Not Present");
                }
                CustomLogs.LogMessage("Current Minus One Points Summary Click ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Standard Customer Tesco Points Section for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusOne_VerifyData_StandardCustomer_TescoPointsSection(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Tesco Points Section started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOPOINTSSECTION).XPath)));

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);

                XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offers[1]));
                IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints");
                IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                    .Descendants("BoxLogoFileName");
                IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                    .Descendants("DataColumnName");
                if (ActualTescoPointsValues.Count != Boxes.Count())
                {
                    Assert.Fail("Count of Tesco Points are not same");
                }
                else
                {
                    List<XElement> listBoxFiles = BoxLogoFileNames.ToList();
                    List<XElement> listDataFiles = DataColumnNames.ToList();
                    for (int i = 0; i < ActualTescoPointsValues.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(listDataFiles[i].Value))
                        {
                            string points = ActualTescoPointsValues[i].FindElements(By.TagName("td"))[2].FindElement(By.TagName("span")).Text;
                            if (pointsSummaryInfo.ContainsKey(listDataFiles[i].Value))
                            {
                                Assert.AreEqual(pointsSummaryInfo[listDataFiles[i].Value], points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                            else
                            {
                                Assert.AreEqual("0", points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                        }
                        if (!string.IsNullOrEmpty(listBoxFiles[i].Value) && listBoxFiles[i].Value != "icentre.jpg" && listBoxFiles[i].Value != "other.jpg")
                        {
                            string imageNameWithPath = ActualTescoPointsValues[i].FindElements(By.TagName("td"))[0].FindElement(By.TagName("img")).GetAttribute("src");
                            string imageName = imageNameWithPath.Substring(imageNameWithPath.LastIndexOf("/") + 1);
                            Assert.AreEqual(listBoxFiles[i].Value, imageName, string.Format("Image name for {0} box are not same", listBoxFiles[i].Value));
                        }
                    }
                }
                CustomLogs.LogMessage("Verify Standard Customer Tesco Points Section ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Standard Customer Tesco Bank Points Section for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusOne_VerifyData_StandardCustomer_TescoBankPointsSection(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Tesco Bank Points Section started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> ActualTescoBankPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOBANKPOINTSSECTION).XPath)));

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);

                XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offers[1]));
                IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints");
                IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                    .Descendants("BoxLogoFileName");
                IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                    .Descendants("DataColumnName");
                if (ActualTescoBankPointsValues.Count != Boxes.Count())
                {
                    Assert.Fail("Count of Tesco Bank Points are not same");
                }
                else
                {
                    List<XElement> listBoxFiles = BoxLogoFileNames.ToList();
                    List<XElement> listDataFiles = DataColumnNames.ToList();
                    for (int i = 0; i < ActualTescoBankPointsValues.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(listDataFiles[i].Value))
                        {
                            string points = ActualTescoBankPointsValues[i].FindElements(By.TagName("td"))[2].FindElement(By.TagName("span")).Text;
                            if (pointsSummaryInfo.ContainsKey(listDataFiles[i].Value))
                            {
                                Assert.AreEqual(pointsSummaryInfo[listDataFiles[i].Value], points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                            else
                            {
                                Assert.AreEqual("0", points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                        }
                        if (!string.IsNullOrEmpty(listBoxFiles[i].Value) && listBoxFiles[i].Value != "icentre.jpg" && listBoxFiles[i].Value != "other.jpg")
                        {
                            string imageNameWithPath = ActualTescoBankPointsValues[i].FindElements(By.TagName("td"))[0].FindElement(By.TagName("img")).GetAttribute("src");
                            string imageName = imageNameWithPath.Substring(imageNameWithPath.LastIndexOf("/") + 1);
                            Assert.AreEqual(listBoxFiles[i].Value, imageName, string.Format("Image name for {0} box are not same", listBoxFiles[i].Value));
                        }
                    }
                }
                CustomLogs.LogMessage("Verify Standard Customer Tesco Bank Points Section ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Statement Type for Upper Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusTwo_VerifyData_UpperPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Avios Customer Upper Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[2], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_UpperPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_UpperPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_UpperPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_UpperPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Avios Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Statement Type for Lower Left Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusTwo_VerifyData_LowerLeftPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Customer Lower Left Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[2], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_LowerLeftPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Statement Type for Lower Right Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusTwo_VerifyData_LowerRightPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Customer Lower Right Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[2], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_LowerRightPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

                /// <summary>
        /// Method to validate Current Minus One Statement Type for Upper Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusOne_VerifyData_UpperPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Avios Customer Upper Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_UpperPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_UpperPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_UpperPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_UpperPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Avios Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Statement Type for Lower Left Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusOne_VerifyData_LowerLeftPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Customer Lower Left Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_LowerLeftPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_LowerLeftPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Statement Type for Lower Right Part and call corresponding method
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusOne_VerifyData_LowerRightPart(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Customer Lower Right Part started", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                        PointSummary_VerifyData_StandardCustomer_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        PointSummary_VerifyData_Avios_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "XmasSavers":
                        PointSummary_VerifyData_ChristmasSaver_LowerRightPart(pointsSummaryInfo);
                        break;
                    case "VirginMilesReward":
                        PointSummary_VerifyData_Virgin_LowerRightPart(pointsSummaryInfo);
                        break;
                }

                CustomLogs.LogMessage("Verify Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Standard Customer Tesco Points Section for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusTwo_VerifyData_StandardCustomer_TescoPointsSection(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Tesco Points Section started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOPOINTSSECTION).XPath)));

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[2], CountrySetting.culture);

                XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offers[2]));
                IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints");
                IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                    .Descendants("BoxLogoFileName");
                IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                    .Descendants("DataColumnName");
                if (ActualTescoPointsValues.Count != Boxes.Count())
                {
                    Assert.Fail("Count of Tesco Points are not same");
                }
                else
                {
                    List<XElement> listBoxFiles = BoxLogoFileNames.ToList();
                    List<XElement> listDataFiles = DataColumnNames.ToList();
                    for (int i = 0; i < ActualTescoPointsValues.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(listDataFiles[i].Value))
                        {
                            string points = ActualTescoPointsValues[i].FindElements(By.TagName("td"))[2].FindElement(By.TagName("span")).Text;
                            if (pointsSummaryInfo.ContainsKey(listDataFiles[i].Value))
                            {
                                Assert.AreEqual(pointsSummaryInfo[listDataFiles[i].Value], points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                            else
                            {
                                Assert.AreEqual("0", points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                        }
                        if (!string.IsNullOrEmpty(listBoxFiles[i].Value) && listBoxFiles[i].Value != "icentre.jpg" && listBoxFiles[i].Value != "other.jpg")
                        {
                            string imageNameWithPath = ActualTescoPointsValues[i].FindElements(By.TagName("td"))[0].FindElement(By.TagName("img")).GetAttribute("src");
                            string imageName = imageNameWithPath.Substring(imageNameWithPath.LastIndexOf("/") + 1);
                            Assert.AreEqual(listBoxFiles[i].Value, imageName, string.Format("Image name for {0} box are not same", listBoxFiles[i].Value));
                        }
                    }
                }
                CustomLogs.LogMessage("Verify Standard Customer Tesco Points Section ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Standard Customer Tesco Bank Points Section for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        public bool PointSummary_CurrentMinusTwo_VerifyData_StandardCustomer_TescoBankPointsSection(string clubcardNumber)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Tesco Bank Points Section started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> ActualTescoBankPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOBANKPOINTSSECTION).XPath)));

                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[2], CountrySetting.culture);

                XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offers[2]));
                IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints");
                IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                    .Descendants("BoxLogoFileName");
                IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                    .Descendants("DataColumnName");
                if (ActualTescoBankPointsValues.Count != Boxes.Count())
                {
                    Assert.Fail("Count of Tesco Bank Points are not same");
                }
                else
                {
                    List<XElement> listBoxFiles = BoxLogoFileNames.ToList();
                    List<XElement> listDataFiles = DataColumnNames.ToList();
                    for (int i = 0; i < ActualTescoBankPointsValues.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(listDataFiles[i].Value))
                        {
                            string points = ActualTescoBankPointsValues[i].FindElements(By.TagName("td"))[2].FindElement(By.TagName("span")).Text;
                            if (pointsSummaryInfo.ContainsKey(listDataFiles[i].Value))
                            {
                                Assert.AreEqual(pointsSummaryInfo[listDataFiles[i].Value], points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                            else
                            {
                                Assert.AreEqual("0", points, string.Format("Points for {0} box are not same", listDataFiles[i].Value));
                            }
                        }
                        if (!string.IsNullOrEmpty(listBoxFiles[i].Value) && listBoxFiles[i].Value != "icentre.jpg" && listBoxFiles[i].Value != "other.jpg")
                        {
                            string imageNameWithPath = ActualTescoBankPointsValues[i].FindElements(By.TagName("td"))[0].FindElement(By.TagName("img")).GetAttribute("src");
                            string imageName = imageNameWithPath.Substring(imageNameWithPath.LastIndexOf("/") + 1);
                            Assert.AreEqual(listBoxFiles[i].Value, imageName, string.Format("Image name for {0} box are not same", listBoxFiles[i].Value));
                        }
                    }
                }
                CustomLogs.LogMessage("Verify Standard Customer Tesco Bank Points Section ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }
        #endregion

        #region Private Method

        string Validate_TransactionData(List<IWebElement> rows, List<TransactionDetails> transactions)
        {
            Resource currencySymbol = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYSYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
            Resource currencySymbolAlpha = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYALPHASYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
            DBConfiguration DisableDecimalConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_CURRENCY_DECIMAL, SanityConfiguration.DbConfigurationFile);
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
            bool isBonusVisible = (disableBonusPointsConfig.IsDeleted.Equals("N") && disableBonusPointsConfig.ConfigurationValue1.Equals("1"));
            string msg = string.Empty;
            for (int i = 1; i < rows.Count - 1; i++)
            {
                string actualValue = transactions[i - 1].AmountSpent;
                string formattedVal = actualValue;
                if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    formattedVal = (actualValue.Contains(',') ? actualValue.TrimEnd('0').TrimEnd(',') : actualValue.Contains('.') ? actualValue.TrimEnd('0').TrimEnd('.') : formattedVal);
                    formattedVal = formattedVal.Contains('.') ? actualValue : formattedVal.Contains(',') ? actualValue : formattedVal;
                }
                StringBuilder expected_Value = new StringBuilder();
                expected_Value.Append(AutomationHelper.GetMaskedClubcard(transactions[i - 1].ClubcardId) + " ");
                expected_Value.Append(transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1) + " ");
                expected_Value.Append(transactions[i - 1].TransactionDescription + " ");
                expected_Value.Append(string.Format("{0}{1}{2}", currencySymbol.Value, formattedVal, currencySymbolAlpha.Value) + " ");
                if (isBonusVisible)
                {
                    expected_Value.Append(transactions[i - 1].BonusPoints + " ");
                }
                expected_Value.Append(transactions[i - 1].NormalPoints);
                // remove extra spaces
                string expected = string.Join(" ", expected_Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                msg += expected.Trim().Equals(rows[i].Text.Trim()) ? string.Empty : string.Format("data not matched at row {0}. Actual : {1} , Expected: {2}", i, rows[i].Text, expected_Value.ToString());
            }
            return msg;
        }

        /// <summary>
        /// Method to validate Current Minus Two Standard Customer Upper Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_StandardCustomer_UpperPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Upper Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.VOUCHERSTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedVouchersText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCarriedForwardTest = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCOBANK, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoBankText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOTOTALPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;
                var ActualVouchersText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALLABEL).XPath)).Text).ToString();
                var ActualCarriedForwardText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDLABEL).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOLABELS).XPath)));

                var ActualVouchersValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALTEXT).XPath)).Text).ToString();
                var ActualCarriedForwardValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDTEXT).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOTEXTS).XPath)));

                Assert.AreEqual(ExpectedVouchersText, ActualVouchersText, "Vouchers total text are not same");
                Assert.AreEqual(ExpectedCarriedForwardTest, ActualCarriedForwardText, "Carried Forward points text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoText, ActualTescoPointsTexts[0].Text, "Tesco Points text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoBankText, ActualTescoPointsTexts[1].Text, "Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedPointsTotalText, ActualTescoPointsTexts[2].Text, "Total Points text are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, pointsSummaryInfo["TotalReward"], CurrencyAlphaSymbol).Trim(), ActualVouchersValue, "Vouchers total value are not same");
                Assert.AreEqual(pointsSummaryInfo["TotalCarriedForwardPoints"], ActualCarriedForwardValue, "Carried forward points are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoPoints"], ActualTescoPointsValues[0].Text, "Tesco Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoBankPoints"], ActualTescoPointsValues[1].Text, "Tesco Bank Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TotalPoints"], ActualTescoPointsValues[2].Text, "Total Points are not same");
                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOSECTION).XPath), Driver))
                {
                    if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOBANKSECTION).XPath), Driver))
                    {
                        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                    }
                    else
                    {
                        Assert.Fail("Clubcard points from Tesco Bank section is not present");
                    }
                }
                else
                {
                    Assert.Fail("Clubcard points from Tesco section is not present");
                }
                CustomLogs.LogMessage("Verify Standard Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Chirtsmas Saver Customer Upper Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_ChristmasSaver_UpperPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Christmas Saver Customer Upper Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.CHRISTMASSAVERSTART, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var XMasSaverStartText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CHRISTMASSAVEREND, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var XMasSaverEndText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCOBANK, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoBankText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOTOTALPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CLUBCARDVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedClubcardVoucherTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOPUPVOUCHERS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTopUpVouchersText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.BONUSVOUCHERS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedBonusVouchers = res.Value;

                var ActualVouchersText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMCHRISTMASSAVER).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualTescoVouchersTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSFROMCHRISTMASSAVER).XPath)));

                var ActualVouchersValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERVALUEFROMCHRISTMASSAVER).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOTEXTS).XPath)));
                ReadOnlyCollection<IWebElement> ActualTescoVouchersValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_ALLVOUCHERSVALUEFROMCHRISTMASSAVER).XPath)));

                DateTime endDateTime;
                DateTime.TryParse(pointsSummaryInfo["EndDateTime"], out endDateTime);

                decimal totalRewards, topUpVouchers, bonusVouchers;
                if (pointsSummaryInfo["TotalReward"].Equals(string.Empty))
                    totalRewards = 0;
                else
                    Decimal.TryParse(pointsSummaryInfo["TotalReward"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalRewards);

                if (pointsSummaryInfo["TopUpVouchers"].Equals(string.Empty))
                    topUpVouchers = 0;
                else
                    Decimal.TryParse(pointsSummaryInfo["TopUpVouchers"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out topUpVouchers);

                if (pointsSummaryInfo["BonusVouchers"].Equals(string.Empty))
                    bonusVouchers = 0;
                else
                    Decimal.TryParse(pointsSummaryInfo["BonusVouchers"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonusVouchers);

                Assert.AreEqual(string.Format("{0} {1} {2}", XMasSaverStartText, endDateTime.Year.ToString(), XMasSaverEndText), ActualVouchersText, "Vouchers total text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoText, ActualTescoPointsTexts[0].Text, "Tesco Points text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoBankText, ActualTescoPointsTexts[1].Text, "Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedPointsTotalText, ActualTescoPointsTexts[2].Text, "Total Points text are not same");
                Assert.AreEqual(ExpectedClubcardVoucherTotalText, ActualTescoVouchersTexts[0].Text, "Clubcard Vouchers text are not same");
                Assert.AreEqual(ExpectedTopUpVouchersText, ActualTescoVouchersTexts[1].Text, "Top Up Vouchers text are not same");
                Assert.AreEqual(ExpectedBonusVouchers, ActualTescoVouchersTexts[2].Text, "Bonus Vouchers text are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, (totalRewards + topUpVouchers + bonusVouchers).ToString(), CurrencyAlphaSymbol).Trim(), ActualVouchersValue, "Vouchers total value are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoPoints"], ActualTescoPointsValues[0].Text, "Tesco Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoBankPoints"], ActualTescoPointsValues[1].Text, "Tesco Bank Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TotalPoints"], ActualTescoPointsValues[2].Text, "Total Points are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, totalRewards.ToString(), CurrencyAlphaSymbol).Trim(), ActualTescoVouchersValues[0].Text, "Clubcard Vouchers text are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, topUpVouchers.ToString(), CurrencyAlphaSymbol).Trim(), ActualTescoVouchersValues[1].Text, "Top Up Vouchers text are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, bonusVouchers.ToString(), CurrencyAlphaSymbol).Trim(), ActualTescoVouchersValues[2].Text, "Bonus Vouchers text are not same");

                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOSECTION).XPath), Driver))
                {
                    if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOBANKSECTION).XPath), Driver))
                    {
                        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                    }
                    else
                    {
                        Assert.Fail("Clubcard points from Tesco Bank section is not present");
                    }
                }
                else
                {
                    Assert.Fail("Clubcard points from Tesco section is not present");
                }
                CustomLogs.LogMessage("Verify Christmas Saver Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Avios Customer Upper Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Avios_UpperPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Avios Customer Upper Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedAviosTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCONVERTEDANDCARRIEDFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCarriedForwardText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCOBANK, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoBankText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOTOTALPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSTOAVIOS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsConvertedToAviosText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedAviosAwardedText = res.Value;

                var ActualVouchersText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALLABEL).XPath)).Text).ToString();
                var ActualCarriedForwardText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDLABEL).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualMilesTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_MILESBOXLABEL).XPath)));

                var ActualVouchersValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALTEXT).XPath)).Text).ToString();
                var ActualCarriedForwardValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDTEXT).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOTEXTS).XPath)));
                ReadOnlyCollection<IWebElement> ActualMilesValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_MILESBOXTEXT).XPath)));

                var ExpectedVouchersValue = pointsSummaryInfo["TotalRewardMiles"];
                var ExpectedCarriedForwardValue = pointsSummaryInfo["TotalCarriedForwardPoints"];
                int iPtsMiles;
                Int32.TryParse((Convert.ToDecimal(pointsSummaryInfo["TotalReward"]) * 100).ToString(), NumberStyles.Number,
        CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                var ExpectedAwards = pointsSummaryInfo["TotalRewardMiles"];

                Assert.AreEqual(ExpectedAviosTotalText, ActualVouchersText, "Avios total text are not same");
                Assert.AreEqual(ExpectedCarriedForwardText, ActualCarriedForwardText, "Carried Forward text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoText, ActualTescoPointsTexts[0].Text, "Tesco Points text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoBankText, ActualTescoPointsTexts[1].Text, "Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedPointsTotalText, ActualTescoPointsTexts[2].Text, "Total Points text are not same");
                Assert.AreEqual(ExpectedPointsConvertedToAviosText, ActualMilesTexts[0].Text, "Points Converted text are not same");
                Assert.AreEqual(ExpectedAviosAwardedText, ActualMilesTexts[1].Text, "Avios awarded text are not same");
                Assert.AreEqual(ExpectedVouchersValue, ActualVouchersValue, "Avios total value are not same");
                Assert.AreEqual(ExpectedCarriedForwardValue, ActualCarriedForwardValue, "Carried Forward total value are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoPoints"], ActualTescoPointsValues[0].Text, "Tesco Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoBankPoints"], ActualTescoPointsValues[1].Text, "Tesco Bank Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TotalPoints"], ActualTescoPointsValues[2].Text, "Total Points are not same");
                Assert.AreEqual(iPtsMiles.ToString(), ActualMilesValues[0].Text, "Points Converted are not same");
                Assert.AreEqual(ExpectedAwards, ActualMilesValues[1].Text, "Avios awarded are not same");

                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOSECTION).XPath), Driver))
                {
                    if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOBANKSECTION).XPath), Driver))
                    {
                        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                    }
                    else
                    {
                        Assert.Fail("Clubcard points from Tesco Bank section is not present");
                    }
                }
                else
                {
                    Assert.Fail("Clubcard points from Tesco section is not present");
                }
                CustomLogs.LogMessage("Verify Avios Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Virgin Customer Upper Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Virgin_UpperPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Virgin Customer Upper Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDEDTEXT, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedFlyingMilesTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCONVERTEDANDCARRIEDFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCarriedForwardText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCOBANK, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsFromTescoBankText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOTOTALPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsTotalText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSTOFLYINGMILES, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsConvertedToFlyingMilesText = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedFlyingAwardedText = res.Value;

                var ActualFlyingMilesTotalText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALLABEL).XPath)).Text).ToString();
                var ActualCarriedForwardText = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDLABEL).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualMilesTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_MILESBOXLABEL).XPath)));

                var ActualFlyingMilesValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_VOUCHERSTOTALTEXT).XPath)).Text).ToString();
                var ActualCarriedForwardValue = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_CARRIEDFORWARDTEXT).XPath)).Text).ToString();
                ReadOnlyCollection<IWebElement> ActualTescoPointsValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOTEXTS).XPath)));
                ReadOnlyCollection<IWebElement> ActualMilesValues = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_MILESBOXTEXT).XPath)));

                var ExpectedFlyingMilesValue = pointsSummaryInfo["TotalRewardMiles"];
                var ExpectedCarriedForwardValue = pointsSummaryInfo["TotalCarriedForwardPoints"];
                int iPtsMiles;
                Int32.TryParse((Convert.ToDecimal(pointsSummaryInfo["TotalReward"]) * 100).ToString(), NumberStyles.Number,
        CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                var ExpectedAwards = pointsSummaryInfo["TotalRewardMiles"];

                Assert.AreEqual(ExpectedFlyingMilesTotalText, ActualFlyingMilesTotalText, "Flying Miles total text are not same");
                Assert.AreEqual(ExpectedCarriedForwardText, ActualCarriedForwardText, "Carried Forward text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoText, ActualTescoPointsTexts[0].Text, "Tesco Points text are not same");
                Assert.AreEqual(ExpectedPointsFromTescoBankText, ActualTescoPointsTexts[1].Text, "Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedPointsTotalText, ActualTescoPointsTexts[2].Text, "Total Points text are not same");
                Assert.AreEqual(ExpectedPointsConvertedToFlyingMilesText, ActualMilesTexts[0].Text, "Points Converted text are not same");
                Assert.AreEqual(ExpectedFlyingAwardedText, ActualMilesTexts[1].Text, "Flying awarded text are not same");
                Assert.AreEqual(ExpectedFlyingMilesValue, ActualFlyingMilesValue, "Flying Miles total value are not same");
                Assert.AreEqual(ExpectedCarriedForwardValue, ActualCarriedForwardValue, "Carried Forward total value are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoPoints"], ActualTescoPointsValues[0].Text, "Tesco Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TescoBankPoints"], ActualTescoPointsValues[1].Text, "Tesco Bank Points are not same");
                Assert.AreEqual(pointsSummaryInfo["TotalPoints"], ActualTescoPointsValues[2].Text, "Total Points are not same");
                Assert.AreEqual(iPtsMiles.ToString(), ActualMilesValues[0].Text, "Points Converted are not same");
                Assert.AreEqual(ExpectedAwards, ActualMilesValues[1].Text, "Flying Miles awarded are not same");

                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOSECTION).XPath), Driver))
                {
                    if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_POINTSFROMTESCOBANKSECTION).XPath), Driver))
                    {
                        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                    }
                    else
                    {
                        Assert.Fail("Clubcard points from Tesco Bank section is not present");
                    }
                }
                else
                {
                    Assert.Fail("Clubcard points from Tesco section is not present");
                }
                CustomLogs.LogMessage("Verify Virgin Customer Upper Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Virgin Customer Lower Left Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Virgin_LowerLeftPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Virgin Customer Lower Left Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCHANGEFROMCLUBCARDBOOST, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsChangeFromClubcardBoost = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedFlyingClubMilesAwarded = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsCarriedForward = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));
                var ActualCarriedForwardPoints = ActualLowerLeftValuesWithoutBold[2].Text.Substring(ExpectedTescoPointsCarriedForward.Length).Trim();

                var ExpectedCarriedForwardFromPrevious = pointsSummaryInfo["TescoBroughtForwardPoints"];
                var ExpectedChangeFromClubcardBoost = pointsSummaryInfo["TescoPointsChangeFromRewards"];
                var ExpectedTescoPoints = pointsSummaryInfo["TescoPoints"];
                var ExpectedTotalTescoPointsValue = Convert.ToInt64(ExpectedCarriedForwardFromPrevious) + Convert.ToInt64(ExpectedChangeFromClubcardBoost) + Convert.ToInt64(ExpectedTescoPoints);
                var ExpectedFlyingMilesAwarded = pointsSummaryInfo["TescoRewardMiles"];
                var ExpectedTescoCarriedForwardPoints = pointsSummaryInfo["TescoCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardFromPrevious, ActualLowerLeftTexts[0].Text, "Points Carried Forward From Previous text are not same");
                Assert.AreEqual(ExpectedPointsChangeFromClubcardBoost, ActualLowerLeftTexts[1].Text, "Points Changes From Clubcard Boost text are not same");
                Assert.AreEqual(ExpectedTotalTescoPoints, ActualLowerLeftTextsWithTextBox[0].Text, "Total Tesco Points text are not same");
                Assert.AreEqual(ExpectedFlyingClubMilesAwarded, ActualLowerLeftTextsWithTextBox[1].Text, "Flying Club Miles Awarded text are not same");
                Assert.AreEqual(ExpectedTescoPointsCarriedForward, ActualLowerLeftTexts[2].Text, "Tesco Points Carried Forward text are not same");
                Assert.AreEqual(ExpectedCarriedForwardFromPrevious, ActualLowerLeftValuesWithBold[0].Text, "Points Carried Forward From Previous are not same");
                Assert.AreEqual(ExpectedChangeFromClubcardBoost, ActualLowerLeftValuesWithBold[1].Text, "Change From Clubcard Boost value are not same");
                Assert.AreEqual(ExpectedTotalTescoPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[0].Text, "Total Tesco Points value are not same");
                Assert.AreEqual(ExpectedFlyingMilesAwarded, ActualLowerLeftValuesWithTextBox[1].Text, "Flying Miles Awarded value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardPoints, ActualCarriedForwardPoints, "Carried Forward Points are not same");

                CustomLogs.LogMessage("Verify Virgin Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Virgin Customer Lower Right Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Virgin_LowerRightPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Virgin Customer Lower Right Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardBankFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOBANKPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedFlyingClubMilesBankAwarded = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsBankCarriedForward = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));

                var ExpectedCarriedForwardBankFromPrevious = pointsSummaryInfo["TescoBankBroughtForwardPoints"];
                var ExpectedTescoBankPoints = pointsSummaryInfo["TescoBankPoints"];
                var ExpectedTotalTescoBankPointsValue = Convert.ToInt32(ExpectedCarriedForwardBankFromPrevious) + Convert.ToInt32(ExpectedTescoBankPoints);
                var ExpectedFlyingMilesBankAwarded = pointsSummaryInfo["TescoBankRewardMiles"];
                var ExpectedTescoCarriedForwardBankPoints = pointsSummaryInfo["TescoBankCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardBankFromPrevious, ActualLowerLeftTexts[3].Text, "Points Carried Forward From Previous Bank text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPoints, ActualLowerLeftTextsWithTextBox[2].Text, "Total Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedFlyingClubMilesBankAwarded, ActualLowerLeftTextsWithTextBox[3].Text, "Flying Club Miles Bank Awarded text are not same");
                Assert.AreEqual(ExpectedTescoPointsBankCarriedForward, ActualLowerLeftTexts[4].Text, "Tesco Points Carried Forward Bank text are not same");
                Assert.AreEqual(ExpectedCarriedForwardBankFromPrevious, ActualLowerLeftValuesWithBold[2].Text, "Points Carried Forward From Previous Bank are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[2].Text, "Total Tesco Bank Points value are not same");
                Assert.AreEqual(ExpectedFlyingMilesBankAwarded, ActualLowerLeftValuesWithTextBox[3].Text, "Flying Miles Bank Awarded value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardBankPoints, ActualLowerLeftValuesWithBold[3].Text, "Carried Forward Bank Points are not same");

                CustomLogs.LogMessage("Verify Virgin Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Avios Customer Lower Left Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Avios_LowerLeftPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Avios Customer Lower Left Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCHANGEFROMCLUBCARDBOOST, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsChangeFromClubcardBoost = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedAviosAwarded = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsCarriedForward = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));
                var ActualCarriedForwardPoints = ActualLowerLeftValuesWithoutBold[2].Text.Substring(ExpectedTescoPointsCarriedForward.Length).Trim();

                var ExpectedCarriedForwardFromPrevious = pointsSummaryInfo["TescoBroughtForwardPoints"];
                var ExpectedChangeFromClubcardBoost = pointsSummaryInfo["TescoPointsChangeFromRewards"];
                var ExpectedTescoPoints = pointsSummaryInfo["TescoPoints"];
                var ExpectedTotalTescoPointsValue = Convert.ToInt64(ExpectedCarriedForwardFromPrevious) + Convert.ToInt64(ExpectedChangeFromClubcardBoost) + Convert.ToInt64(ExpectedTescoPoints);
                var ExpectedAviosAwardedValue = pointsSummaryInfo["TescoRewardMiles"];
                var ExpectedTescoCarriedForwardPoints = pointsSummaryInfo["TescoCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardFromPrevious, ActualLowerLeftTexts[0].Text, "Points Carried Forward From Previous text are not same");
                Assert.AreEqual(ExpectedPointsChangeFromClubcardBoost, ActualLowerLeftTexts[1].Text, "Points Changes From Clubcard Boost text are not same");
                Assert.AreEqual(ExpectedTotalTescoPoints, ActualLowerLeftTextsWithTextBox[0].Text, "Total Tesco Points text are not same");
                Assert.AreEqual(ExpectedAviosAwarded, ActualLowerLeftTextsWithTextBox[1].Text, "Avios Awarded text are not same");
                Assert.AreEqual(ExpectedTescoPointsCarriedForward, ActualLowerLeftTexts[2].Text, "Tesco Points Carried Forward text are not same");
                Assert.AreEqual(ExpectedCarriedForwardFromPrevious, ActualLowerLeftValuesWithBold[0].Text, "Points Carried Forward From Previous are not same");
                Assert.AreEqual(ExpectedChangeFromClubcardBoost, ActualLowerLeftValuesWithBold[1].Text, "Change From Clubcard Boost value are not same");
                Assert.AreEqual(ExpectedTotalTescoPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[0].Text, "Total Tesco Points value are not same");
                Assert.AreEqual(ExpectedAviosAwardedValue, ActualLowerLeftValuesWithTextBox[1].Text, "Avios Awarded value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardPoints, ActualCarriedForwardPoints, "Carried Forward Points are not same");

                CustomLogs.LogMessage("Verify Avios Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Avios Customer Lower Right Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_Avios_LowerRightPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Avios Customer Lower Right Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardBankFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOBANKPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedAviosBankAwarded = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsBankCarriedForward = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));

                var ExpectedCarriedForwardBankFromPrevious = pointsSummaryInfo["TescoBankBroughtForwardPoints"];
                var ExpectedTescoBankPoints = pointsSummaryInfo["TescoBankPoints"];
                var ExpectedTotalTescoBankPointsValue = Convert.ToInt32(ExpectedCarriedForwardBankFromPrevious) + Convert.ToInt32(ExpectedTescoBankPoints);
                var ExpectedAviosBankAwardedValue = pointsSummaryInfo["TescoBankRewardMiles"];
                var ExpectedTescoCarriedForwardBankPoints = pointsSummaryInfo["TescoBankCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardBankFromPrevious, ActualLowerLeftTexts[3].Text, "Points Carried Forward From Previous Bank text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPoints, ActualLowerLeftTextsWithTextBox[2].Text, "Total Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedAviosBankAwarded, ActualLowerLeftTextsWithTextBox[3].Text, "Avios Bank Awarded text are not same");
                Assert.AreEqual(ExpectedTescoPointsBankCarriedForward, ActualLowerLeftTexts[4].Text, "Tesco Points Carried Forward Bank text are not same");
                Assert.AreEqual(ExpectedCarriedForwardBankFromPrevious, ActualLowerLeftValuesWithBold[2].Text, "Points Carried Forward From Previous Bank are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[2].Text, "Total Tesco Bank Points value are not same");
                Assert.AreEqual(ExpectedAviosBankAwardedValue, ActualLowerLeftValuesWithTextBox[3].Text, "Avios Bank Awarded value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardBankPoints, ActualLowerLeftValuesWithBold[3].Text, "Carried Forward Bank Points are not same");

                CustomLogs.LogMessage("Verify Avios Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus Two Chirstmas Saver Customer Lower Left Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_ChristmasSaver_LowerLeftPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Chirstmas Saver Customer Lower Left Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCHANGEFROMCLUBCARDBOOST, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsChangeFromClubcardBoost = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoVouchers = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsCarriedForward = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));
                var ActualCarriedForwardPoints = ActualLowerLeftValuesWithoutBold[2].Text.Substring(ExpectedTescoPointsCarriedForward.Length).Trim();

                var ExpectedCarriedForwardFromPrevious = pointsSummaryInfo["TescoBroughtForwardPoints"];
                var ExpectedChangeFromClubcardBoost = pointsSummaryInfo["TescoPointsChangeFromRewards"];
                var ExpectedTescoPoints = pointsSummaryInfo["TescoPoints"];
                var ExpectedTotalTescoPointsValue = Convert.ToInt64(ExpectedCarriedForwardFromPrevious) + Convert.ToInt64(ExpectedChangeFromClubcardBoost) + Convert.ToInt64(ExpectedTescoPoints);
                var ExpectedTotalTescoVochersValue = pointsSummaryInfo["TescoTotalReward"];
                var ExpectedTescoCarriedForwardPoints = pointsSummaryInfo["TescoCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardFromPrevious, ActualLowerLeftTexts[0].Text, "Points Carried Forward From Previous text are not same");
                Assert.AreEqual(ExpectedPointsChangeFromClubcardBoost, ActualLowerLeftTexts[1].Text, "Points Changes From Clubcard Boost text are not same");
                Assert.AreEqual(ExpectedTotalTescoPoints, ActualLowerLeftTextsWithTextBox[0].Text, "Total Tesco Points text are not same");
                Assert.AreEqual(ExpectedTotalTescoVouchers, ActualLowerLeftTextsWithTextBox[1].Text, "Total Tesco Vouchers text are not same");
                Assert.AreEqual(ExpectedTescoPointsCarriedForward, ActualLowerLeftTexts[2].Text, "Tesco Points Carried Forward text are not same");
                Assert.AreEqual(ExpectedCarriedForwardFromPrevious, ActualLowerLeftValuesWithBold[0].Text, "Points Carried Forward From Previous are not same");
                Assert.AreEqual(ExpectedChangeFromClubcardBoost, ActualLowerLeftValuesWithBold[1].Text, "Change From Clubcard Boost value are not same");
                Assert.AreEqual(ExpectedTotalTescoPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[0].Text, "Total Tesco Points value are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, ExpectedTotalTescoVochersValue, CurrencyAlphaSymbol), ActualLowerLeftValuesWithTextBox[1].Text, "Total Tesco Vouchers value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardPoints, ActualCarriedForwardPoints, "Carried Forward Points are not same");

                CustomLogs.LogMessage("Verify Chirstmas Saver Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Chirstmas Saver Customer Lower Right Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_ChristmasSaver_LowerRightPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Chirstmas Saver Customer Lower Right Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardBankFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOBANKPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankVouchers = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsBankCarriedForward = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));

                var ExpectedCarriedForwardBankFromPrevious = pointsSummaryInfo["TescoBankBroughtForwardPoints"];
                var ExpectedTescoBankPoints = pointsSummaryInfo["TescoBankPoints"];
                var ExpectedTotalTescoBankPointsValue = Convert.ToInt32(ExpectedCarriedForwardBankFromPrevious) + Convert.ToInt32(ExpectedTescoBankPoints);
                var ExpectedTotalTescoBankAwardedVouchers = pointsSummaryInfo["TescoBankTotalReward"];
                var ExpectedTescoCarriedForwardBankPoints = pointsSummaryInfo["TescoBankCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardBankFromPrevious, ActualLowerLeftTexts[3].Text, "Points Carried Forward From Previous Bank text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPoints, ActualLowerLeftTextsWithTextBox[2].Text, "Total Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankVouchers, ActualLowerLeftTextsWithTextBox[3].Text, "Total Tesco Bank Vouchers text are not same");
                Assert.AreEqual(ExpectedTescoPointsBankCarriedForward, ActualLowerLeftTexts[4].Text, "Tesco Points Carried Forward Bank text are not same");
                Assert.AreEqual(ExpectedCarriedForwardBankFromPrevious, ActualLowerLeftValuesWithBold[2].Text, "Points Carried Forward From Previous Bank are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[2].Text, "Total Tesco Bank Points value are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, ExpectedTotalTescoBankAwardedVouchers, CurrencyAlphaSymbol), ActualLowerLeftValuesWithTextBox[3].Text, "Total Tesco Bank Vouchers value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardBankPoints, ActualLowerLeftValuesWithBold[3].Text, "Carried Forward Bank Points are not same");

                CustomLogs.LogMessage("Verify Chirstmas Saver Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Standard Customer Lower Left Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_StandardCustomer_LowerLeftPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Lower Left Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCHANGEFROMCLUBCARDBOOST, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsChangeFromClubcardBoost = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoVouchers = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsCarriedForward = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));
                var ActualCarriedForwardPoints = ActualLowerLeftValuesWithoutBold[2].Text.Substring(ExpectedTescoPointsCarriedForward.Length).Trim();

                var ExpectedCarriedForwardFromPrevious = pointsSummaryInfo["TescoBroughtForwardPoints"];
                var ExpectedChangeFromClubcardBoost = pointsSummaryInfo["TescoPointsChangeFromRewards"];
                var ExpectedTescoPoints = pointsSummaryInfo["TescoPoints"];
                var ExpectedTotalTescoPointsValue = Convert.ToInt64(ExpectedCarriedForwardFromPrevious) + Convert.ToInt64(ExpectedChangeFromClubcardBoost) + Convert.ToInt64(ExpectedTescoPoints);
                var ExpectedTotalTescoVochersValue = pointsSummaryInfo["TescoTotalReward"];
                var ExpectedTescoCarriedForwardPoints = pointsSummaryInfo["TescoCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardFromPrevious, ActualLowerLeftTexts[0].Text, "Points Carried Forward From Previous text are not same");
                Assert.AreEqual(ExpectedPointsChangeFromClubcardBoost, ActualLowerLeftTexts[1].Text, "Points Changes From Clubcard Boost text are not same");
                Assert.AreEqual(ExpectedTotalTescoPoints, ActualLowerLeftTextsWithTextBox[0].Text, "Total Tesco Points text are not same");
                Assert.AreEqual(ExpectedTotalTescoVouchers, ActualLowerLeftTextsWithTextBox[1].Text, "Total Tesco Vouchers text are not same");
                Assert.AreEqual(ExpectedTescoPointsCarriedForward, ActualLowerLeftTexts[2].Text, "Tesco Points Carried Forward text are not same");
                Assert.AreEqual(ExpectedCarriedForwardFromPrevious, ActualLowerLeftValuesWithBold[0].Text, "Points Carried Forward From Previous are not same");
                Assert.AreEqual(ExpectedChangeFromClubcardBoost, ActualLowerLeftValuesWithBold[1].Text, "Change From Clubcard Boost value are not same");
                Assert.AreEqual(ExpectedTotalTescoPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[0].Text, "Total Tesco Points value are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, ExpectedTotalTescoVochersValue, CurrencyAlphaSymbol), ActualLowerLeftValuesWithTextBox[1].Text, "Total Tesco Vouchers value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardPoints, ActualCarriedForwardPoints, "Carried Forward Points are not same");

                CustomLogs.LogMessage("Verify Standard Customer Lower Left Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }

        /// <summary>
        /// Method to validate Current Minus One Standard Customer Lower Right Part for Points Summary Page
        /// </summary>
        /// <param name="clubcardNumber"></param>
        private bool PointSummary_VerifyData_StandardCustomer_LowerRightPart(Dictionary<string, string> pointsSummaryInfo)
        {
            try
            {
                CustomLogs.LogMessage("Verify Standard Customer Lower Right Part started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPointsCarriedForwardBankFromPrevious = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOBANKPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankPoints = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTotalTescoBankVouchers = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedTescoPointsBankCarriedForward = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencySymbol = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var CurrencyAlphaSymbol = res.Value;

                ReadOnlyCollection<IWebElement> ActualLowerLeftTexts = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELS).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftTextsWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTLABELSWITHTEXTBOX).XPath)));

                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithoutBold = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHOUTBOLD).XPath)));
                ReadOnlyCollection<IWebElement> ActualLowerLeftValuesWithTextBox = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_LOWERLEFTTEXTWITHTEXTBOX).XPath)));

                var ExpectedCarriedForwardBankFromPrevious = pointsSummaryInfo["TescoBankBroughtForwardPoints"];
                var ExpectedTescoBankPoints = pointsSummaryInfo["TescoBankPoints"];
                var ExpectedTotalTescoBankPointsValue = Convert.ToInt32(ExpectedCarriedForwardBankFromPrevious) + Convert.ToInt32(ExpectedTescoBankPoints);
                var ExpectedTotalTescoBankAwardedVouchers = pointsSummaryInfo["TescoBankTotalReward"];
                var ExpectedTescoCarriedForwardBankPoints = pointsSummaryInfo["TescoBankCarriedForwardPoints"];

                Assert.AreEqual(ExpectedPointsCarriedForwardBankFromPrevious, ActualLowerLeftTexts[3].Text, "Points Carried Forward From Previous Bank text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPoints, ActualLowerLeftTextsWithTextBox[2].Text, "Total Tesco Bank Points text are not same");
                Assert.AreEqual(ExpectedTotalTescoBankVouchers, ActualLowerLeftTextsWithTextBox[3].Text, "Total Tesco Bank Vouchers text are not same");
                Assert.AreEqual(ExpectedTescoPointsBankCarriedForward, ActualLowerLeftTexts[4].Text, "Tesco Points Carried Forward Bank text are not same");
                Assert.AreEqual(ExpectedCarriedForwardBankFromPrevious, ActualLowerLeftValuesWithBold[2].Text, "Points Carried Forward From Previous Bank are not same");
                Assert.AreEqual(ExpectedTotalTescoBankPointsValue.ToString(), ActualLowerLeftValuesWithTextBox[2].Text, "Total Tesco Bank Points value are not same");
                Assert.AreEqual(string.Format("{0}{1}{2}", CurrencySymbol, ExpectedTotalTescoBankAwardedVouchers, CurrencyAlphaSymbol), ActualLowerLeftValuesWithTextBox[3].Text, "Total Tesco Bank Vouchers value are not same");
                Assert.AreEqual(ExpectedTescoCarriedForwardBankPoints, ActualLowerLeftValuesWithBold[3].Text, "Carried Forward Bank Points are not same");

                CustomLogs.LogMessage("Verify Standard Customer Lower Right Part ended", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return true;
        }
        #endregion

    }
}
