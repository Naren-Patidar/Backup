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
using System.Web;

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

            SanityConfiguration = configuration;
        }
        #endregion

        #region Public Methods

        public void myCurrentPoint_verifyPageName()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page 'Current Points Details' started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.MYCURRENTPOINTS_TITLE, SanityConfiguration.ResourceFiles.POINTSDETAILSPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var expectedLinkName = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected link name"));

                var actualPageHeader = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_HEADER).Id)).Text;
                Assert.AreEqual(expectedLinkName, actualPageHeader, " 'Current Points Details' not Verified");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();

            }

        }

        public bool myPoint_verifyPageName()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page 'Current Points Details' started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSTITLE, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var expectedLinkName = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected link name"));

                var actualPageHeader = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_HEADER).Id)).Text;
                Assert.AreEqual(expectedLinkName, actualPageHeader, " 'Current Points Details' not Verified");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                return true;
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }

        }

        public bool myCurrentPoint_click()
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.MYCURRENTPOINTS_LINK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                var expectedLinkName = res.Value;
                var actualLinkName = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_CLICK).XPath)).Text).ToString();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if (string.IsNullOrEmpty(expectedLinkName) && expectedLinkName.Equals(actualLinkName, StringComparison.OrdinalIgnoreCase))
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
                var ctrl = table.FindElements(By.TagName("tr"))[periodIndex].FindElements(By.TagName("a")).LastOrDefault();
                if (ctrl != null)
                {
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    jse.ExecuteScript("arguments[0].click();", ctrl);
                    //  ((IWebElement)ctrl).Click();
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
                DBConfiguration dateConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DATERANGE_FOR_COLLECTION_PERIOD, SanityConfiguration.DbConfigurationFile);
                bool isDateRangeEnabled = dateConfig.IsDeleted.ToUpper().Equals("N") && dateConfig.ConfigurationValue1.ToUpper().Equals("TRUE");
                DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
                IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_COLLECTION_PERIOD_TABLE).Id));
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
                    ReadOnlyCollection<IWebElement> ltrClubcard = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id)));
                    if (ltrClubcard != null)
                    {
                        foreach (IWebElement wb in ltrClubcard)
                        {
                            if (wb != null && !wb.Text.Contains(" XXXX XXXX "))
                            {
                                error += string.Format(" Clubcard is not masked, Actual : {1}", wb.Text);
                            }
                        }
                        //List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        //for (int i = 1; i < rows.Count - 1; i++)
                        //{
                        //    IWebElement td = rows[i].FindElements(By.TagName("td")).FirstOrDefault();
                        //    if (td != null && !td.Text.Contains(" XXXX XXXX "))
                        //    {
                        //        error += string.Format(" Clubcard at row Index: {0} is not masked, Actual : {1}", i, td.Text);
                        //    }
                        //}
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
                DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> tdTransDate = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_TD_TRAN_DATE).Id)));
                    if (tdTransDate != null)
                    {
                        int iCount = 0;
                        int flag = 1;
                        foreach (IWebElement wb in tdTransDate)
                        {
                            if (wb.Text != transactions[iCount].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1))
                            {
                                error += string.Format(" Transaction date at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", iCount, wb.Text, transactions[flag - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1));
                            }
                            iCount++;
                            flag++;
                        }

                        //List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        //for (int i = 1; i < rows.Count - 1; i++)\
                        //{
                        //    IWebElement td = rows[i].FindElements(By.TagName("td"))[1];
                        //    if (transactions[i - 1] == null)
                        //    {
                        //        error += string.Format(" Transaction date at row Index: {0} is not matched ", i);
                        //    }
                        //    else if (td != null && td.Text != transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1))
                        //    {
                        //        error += string.Format(" Transaction date at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1));
                        //    }
                        //}
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
                    ReadOnlyCollection<IWebElement> webElement = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_TD_TRAN_DETAILS).Id)));
                    if (webElement != null)
                    {
                        int iCount = 0;
                        foreach (IWebElement we in webElement)
                        {
                            if (we != null && we.Text != transactions[iCount].TransactionDescription)
                            {
                                error += string.Format(" Transaction Description  (Store) at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", iCount, we.Text, transactions[iCount].TransactionDescription);
                            }
                            iCount++;
                        }
                        //List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        //for (int i = 1; i < rows.Count - 1; i++)
                        //{
                        //    IWebElement td = rows[i].FindElements(By.TagName("td"))[2];
                        //    if (transactions[i - 1] == null)
                        //    {
                        //        error += string.Format(" Transaction Description (Store) at row Index: {0} is not matched ", i);
                        //    }
                        //    else if (td != null && td.Text != transactions[i - 1].TransactionDescription)
                        //    {
                        //        error += string.Format(" Transaction Description  (Store) at row Index: {0} is not matched, Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].TransactionDescription);
                        //    }
                        //}
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
                DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
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
                DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
                bool isBonusVisible = (disableBonusPointsConfig.IsDeleted.Equals("N") && disableBonusPointsConfig.ConfigurationValue1.Equals("1"));
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> webElement = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_TD_TOTAL_POINTS).Id)));
                    if (webElement != null)
                    {
                        int i = 0;
                        foreach (IWebElement ele in webElement)
                        {
                            if (ele != null && ele.Text != transactions[i].TotalPoints)
                            {
                                error += string.Format(" Transaction Total Points at row Index: {0} is not matched , Actual : {1}, Expected : {2} ", i, ele.Text, transactions[i].NormalPoints);
                            }
                            i++;
                        }
                        //List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        //for (int i = 1; i < rows.Count - 1; i++)
                        //{
                        //    IWebElement td = rows[i].FindElements(By.TagName("td"))[isBonusVisible ? 5 : 4];
                        //    if (transactions[i - 1] == null)
                        //    {
                        //        error += string.Format(" Transaction Total Points at row Index: {0} is not matched ", i);
                        //    }
                        //    else if (td != null && td.Text != transactions[i - 1].NormalPoints)
                        //    {
                        //        error += string.Format(" Transaction Total Points at row Index: {0} is not matched , Actual : {1}, Expected : {2} ", i, td.Text, transactions[i - 1].NormalPoints);
                        //    }
                        //}
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
                DBConfiguration DisableDecimalConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLE_CURRENCY_DECIMAL, SanityConfiguration.DbConfigurationFile);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                string offerID = client.GetOfferID(customerID, culture, 0);
                string transactions_xml = client.GetTransactions(customerID, culture, offerID);
                List<TransactionDetails> transactions = TransactionDetails.ConvertFromXml(transactions_xml);
                if (transactions.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> webElement = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_TD_AMOUNT_SPENT).Id)));
                    if (webElement != null)
                    {
                        int iCount = 0;
                        foreach (IWebElement ele in webElement)
                        {
                            string actualValue = transactions[iCount].AmountSpent;
                            string formattedVal = actualValue;
                            if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                            {
                                formattedVal = (actualValue.Contains(',') ? actualValue.TrimEnd('0').TrimEnd(',') : actualValue.Contains('.') ? actualValue.TrimEnd('0').TrimEnd('.') : formattedVal);
                                formattedVal = formattedVal.Contains('.') ? actualValue : formattedVal.Contains(',') ? actualValue : formattedVal;
                            }
                            if (ele != null && ele.Text != string.Format("{0}{1}{2}", currencySymbol.Value + " ", formattedVal, currencySymbolAlpha.Value))
                            {
                                error += string.Format("Transaction Amount spent at row Index: {0} is not matched. Actual : {1}, Expected {2}", iCount, ele.Text, formattedVal);
                            }


                            iCount++;
                        }

                        Assert.AreEqual(error, string.Empty);
                        //List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        //for (int i = 1; i < rows.Count - 1; i++)
                        //{
                        //    List<IWebElement> tds = rows[i].FindElements(By.TagName("td")).ToList();
                        //    if (tds.Count > 3)
                        //    {
                        //        IWebElement td = tds[3];
                        //        if (td != null)
                        //        {
                        //            string actualValue = transactions[i - 1].AmountSpent;
                        //            string formattedVal = actualValue;
                        //            if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                        //            {
                        //                formattedVal = (actualValue.Contains(',') ? actualValue.TrimEnd('0').TrimEnd(',') : actualValue.Contains('.') ? actualValue.TrimEnd('0').TrimEnd('.') : formattedVal);
                        //                formattedVal = formattedVal.Contains('.') ? actualValue : formattedVal.Contains(',') ? actualValue : formattedVal;
                        //            }
                        //            if (td != null && td.Text != string.Format("{0}{1}{2}", currencySymbol.Value, formattedVal, currencySymbolAlpha.Value))
                        //            {
                        //                error += string.Format("Transaction Amount spent at row Index: {0} is not matched. Actual : {1}, Expected {2}", i, td.Text, formattedVal);
                        //            }
                        //        }
                        //    }
                        //}
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
                    IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                    IWebElement table = divTransaction.FindElement(By.TagName("table"));
                    table = table.FindElement(By.TagName("tbody"));

                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        Assert.AreEqual(error, string.Empty);
                    }
                }
                else
                {
                    // Assert.Inconclusive(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));
                    CustomLogs.LogInformation(string.Format("There are no transactions for the customer {0} and offer Id {1}", customerID, offerID));

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
                        // IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                        IWebElement table = divTransaction.FindElement(By.TagName("table"));
                        table = table.FindElement(By.TagName("tbody"));
                        if (table != null)
                        {
                            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                            actual_Rows = rows.Count;
                        }
                        Assert.AreEqual(expected_Rows, actual_Rows, string.Format("Expected transaction count : {0} , actual transaction count : {1}", expected_Rows, actual_Rows));
                    }
                    else
                    {
                        Assert.Inconclusive(string.Format("There are no clubcard in search clubcard dropdown at index: {0}", index));
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
                        // IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                        IWebElement table = divTransaction.FindElement(By.TagName("table"));
                        table = table.FindElement(By.TagName("tbody"));
                        if (table != null)
                        {
                            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                            actual_Rows = rows.Count;
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
                            //IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                            IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                            IWebElement table = divTransaction.FindElement(By.TagName("table"));
                            table = table.FindElement(By.TagName("tbody"));
                            if (table != null)
                            {
                                List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                                actual_Rows = rows.Count;
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
                    //  IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                    IWebElement table = divTransaction.FindElement(By.TagName("table"));
                    // table = divTransaction.FindElement(By.TagName("tbody")); 
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[0];
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                        jse.ExecuteScript("arguments[0].click();", sort_anchor);
                        // sort_anchor.Click();
                        transactions = transactions.OrderBy(t => t.ClubcardId).ToList();
                        divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                        table = divTransaction.FindElement(By.TagName("table")); ;
                        table = table.FindElement(By.TagName("tbody"));
                        //table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        rows = table.FindElements(By.TagName("tr")).ToList();
                        // anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        // sort_anchor = anchors[0];
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
                    // IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                    IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                    IWebElement table = divTransaction.FindElement(By.TagName("table"));
                    // table = divTransaction.FindElement(By.TagName("tbody")); 
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[2];
                        IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                        jse.ExecuteScript("arguments[0].click();", sort_anchor);
                        // sort_anchor.Click();
                        transactions = transactions.OrderBy(t => t.TransactionDescription).ToList();
                        // table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));
                        divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                        table = divTransaction.FindElement(By.TagName("table"));
                        table = table.FindElement(By.TagName("tbody"));
                        rows = table.FindElements(By.TagName("tr")).ToList();
                        // anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        //  sort_anchor = anchors[0];
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
                    //  IWebElement table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));

                    IWebElement divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                    IWebElement table = divTransaction.FindElement(By.TagName("table"));
                    // table = divTransaction.FindElement(By.TagName("tbody")); 
                    if (table != null)
                    {
                        List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
                        List<IWebElement> anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        IWebElement sort_anchor = anchors[1];

                        IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                        jse.ExecuteScript("arguments[0].click();", sort_anchor);

                        transactions = transactions.OrderBy(t => t.TransactionDateTime).ToList();
                        //table = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_TARNSACTION_TABLE).Id));

                        divTransaction = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_DIV_TRANSACTIONS).ClassName));
                        table = divTransaction.FindElement(By.TagName("table"));
                        table = table.FindElement(By.TagName("tbody"));

                        rows = table.FindElements(By.TagName("tr")).ToList();
                        // anchors = rows[0].FindElements(By.TagName("a")).ToList();
                        error = Validate_TransactionData(rows, transactions);
                        // sort_anchor = anchors[0];
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
                    ReadOnlyCollection<IWebElement> lnkView = Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK).Id));
                    // Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous1Summary_CLICK).Id)).Click();
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView[0]);
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
                    ReadOnlyCollection<IWebElement> lnkView = Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous2Summary_CLICK).Id));
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    jse.ExecuteScript("arguments[0].click();", lnkView[1]);
                    // Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCURRENTPOINTS_Previous2Summary_CLICK).Id)).Click();
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

        public string VerifyPointsSummaryTable1(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();
            Debug.WriteLine("2|VerifyPointsSummaryTable1|Starting");
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|customer id:" + customerID);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|offer id:" + offerId);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|get points");
            string message = VerifyPointsSummaryStandardTable1(pointsSummaryInfo);
            error.Append(message);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|Ending");
            return error.ToString();
        }

        private string VerifyPointsSummaryStandardTable1(Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            CustomLogs.LogMessage("Verify Standard Customer Upper Part started", TraceEventType.Start);
            Generic objgeneric = new Generic(ObjAutomationHelper);
            IWebElement table = objgeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TOP_TABLE1BODY), FindBy.CSS_SELECTOR_ID);
            IWebElement tbody = table.FindElement(By.TagName("tbody"));
            List<IWebElement> trows = tbody.FindElements(By.TagName("tr")).ToList();

            string message = string.Empty;
            switch (pointsSummaryInfo["PointSummaryDescEnglish"])
            {
                case "Reward":
                case "NonReward":
                    message = ValidateTopTableStandard(trows, pointsSummaryInfo);
                    break;
                case "AirmilesReward":
                case "BAmilesReward":
                    message = ValidateTopTableBAAvios(trows, pointsSummaryInfo);
                    break;
                case "XmasSavers":
                    message = ValidateTopTableXmasSaver(trows, pointsSummaryInfo);
                    break;
                case "VirginMilesReward":
                    message = ValidateTopTableVirgin(trows, pointsSummaryInfo);
                    break;
            }
            error.Append(message);
            CustomLogs.LogMessage("Verify Standard Customer Upper Part ended", TraceEventType.Stop);
            return error.ToString();
        }

        public string ValidateTopTableStandard(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            string errorMessage = string.Empty, expectedText = string.Empty, expectedValue = string.Empty;
            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            // only one row
            if (trows.Count == 0)
            {
                error.Append("Top table 1 controle Row not found for Standard Card. ");
            }
            else
            {
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.VOUCHERSTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = expectedValue = string.Format("{0} {1} {2}", currency, pointsSummaryInfo["TotalReward"], currencyAlpha).Trim();
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        public string ValidateTopTableBAAvios(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            int iPtsMiles;
            string expectedText = string.Empty, expectedValue = string.Empty, errorMessage = string.Empty;
            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            // two rows
            if (trows.Count == 0)
            {
                error.Append("Top table 1 controle Row not found for BAAvios Card. ");
            }
            else
            {
                // first row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSTOAVIOS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Int32.TryParse((Convert.ToDecimal(pointsSummaryInfo["TotalReward"]) * 100).ToString(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                expectedValue = iPtsMiles.ToString();
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);

                // Second row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Int32.TryParse(pointsSummaryInfo["TotalRewardMiles"], NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                expectedValue = iPtsMiles.ToString();
                errorMessage = ValidateSummaryRow(trows[1], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        public string ValidateTopTableVirgin(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            int iPtsMiles;
            string expectedText = string.Empty, expectedValue = string.Empty, errorMessage = string.Empty;
            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            // two rows
            if (trows.Count == 0)
            {
                error.Append("Top table 1 controle Row not found for BAAvios Card. ");
            }
            else
            {
                // first row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSTOFLYINGMILES, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Int32.TryParse((Convert.ToDecimal(pointsSummaryInfo["TotalReward"]) * 100).ToString(), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                expectedValue = iPtsMiles.ToString();
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);

                // Second row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Int32.TryParse(pointsSummaryInfo["TotalRewardMiles"], NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out iPtsMiles);
                expectedValue = iPtsMiles.ToString();
                errorMessage = ValidateSummaryRow(trows[1], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        public string ValidateTopTableXmasSaver(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            string errorMessage = string.Empty;
            string expectedValue = string.Empty, expectedText = string.Empty;
            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            DateTime date;
            DateTime.TryParse(pointsSummaryInfo["EndDateTime"], out date);
            string year = date.Year.ToString();
            decimal totalRewards = 0, topUpVouchers = 0, bonusVouchers = 0, total = 0;
            // four rows
            if (trows.Count == 0)
            {
                error.Append("Top table 1 controle Row not found for XmasSaver Card. ");
            }
            else
            {
                // first row
                List<IWebElement> cells = trows[0].FindElements(By.TagName("td")).ToList();
                expectedText =
                    string.Format("{0} {1} {2}",
                    AutomationHelper.GetResourceMessageNew(LabelKey.CHRISTMASSAVERSTART, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value,
                    year,
                    AutomationHelper.GetResourceMessageNew(LabelKey.CHRISTMASSAVEREND, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value);

                Decimal.TryParse(pointsSummaryInfo["TotalReward"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalRewards);
                Decimal.TryParse(pointsSummaryInfo["TopUpVouchers"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out topUpVouchers);
                Decimal.TryParse(pointsSummaryInfo["BonusVouchers"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonusVouchers);
                total = (totalRewards + topUpVouchers + bonusVouchers);
                expectedValue = string.Format("{0} {1} {2}", currency, total.ToString(), currencyAlpha);
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);

                // Second row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.CLUBCARDVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Decimal.TryParse(pointsSummaryInfo["TotalReward"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out totalRewards);
                expectedValue = string.Format("{0} {1} {2}", currency, totalRewards.ToString(), currencyAlpha);
                errorMessage = ValidateSummaryRow(trows[1], expectedText, expectedValue);
                error.Append(errorMessage);

                // third row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TOPUPVOUCHERS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Decimal.TryParse(pointsSummaryInfo["TopUpVouchers"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out topUpVouchers);
                expectedValue = string.Format("{0} {1} {2}", currency, topUpVouchers.ToString(), currencyAlpha);
                errorMessage = ValidateSummaryRow(trows[2], expectedText, expectedValue);
                error.Append(errorMessage);

                // fourth row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.BONUSVOUCHERS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                Decimal.TryParse(pointsSummaryInfo["BonusVouchers"], NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out bonusVouchers);
                expectedValue = string.Format("{0} {1} {2}", currency, bonusVouchers.ToString(), currencyAlpha);
                errorMessage = ValidateSummaryRow(trows[3], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        public bool ValidatepointshomeText(Tuple<string, string> Date)
        {
            try
            {
                CustomLogs.LogMessage("Verify PointsCOllectedStatement Text", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLUHVCOLLECTED, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedPoinstCollectedText = res.Value;
                string error = string.Empty;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLCURRNETCOLLECTEDPERIOD, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCurrentCollectionPeriodText = res.Value;
                DBConfiguration hideStartDateConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSSTARTDATE, SanityConfiguration.DbConfigurationFile);
                DBConfiguration hideEndDateConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEPOINTSENDDATE, SanityConfiguration.DbConfigurationFile);
                bool isHidePointsStartDate = hideStartDateConfig.IsDeleted.ToUpper().Equals("N") && hideStartDateConfig.ConfigurationValue1.ToUpper().Equals("1");
                ExpectedCurrentCollectionPeriodText = !isHidePointsStartDate ? ExpectedCurrentCollectionPeriodText + " " + Date.Item1 : ExpectedCurrentCollectionPeriodText + string.Empty;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLSEPERATOR, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                Resource resFullstop = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_FULLSTOP, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                bool isHidePointsEndDate = hideEndDateConfig.IsDeleted.ToUpper().Equals("N") && hideEndDateConfig.ConfigurationValue1.ToUpper().Equals("1");
                ExpectedCurrentCollectionPeriodText = !isHidePointsEndDate ? ExpectedCurrentCollectionPeriodText + " " + res.Value + " " + Date.Item2 + " "+resFullstop.Value : ExpectedCurrentCollectionPeriodText + " " + res.Value + string.Empty;


                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLFINALDATE, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedFinlDateText = res.Value;
                string month = GetColMonthName(Convert.ToDateTime(Date.Item2), true, CultureInfo.CurrentCulture);
                ExpectedFinlDateText = ExpectedFinlDateText + " " + Date.Item2;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_WILLAPPEAR, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                ExpectedFinlDateText = ExpectedFinlDateText + " " + res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLCLUBCARDSTATEMENT, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                ExpectedFinlDateText = ExpectedFinlDateText + " " +month+" "+ res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_WUDLIKETEXT, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedWudLikeText = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLCURRENTPOINTSTOTAL, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCurrentPointsTotalText = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLWHICHCOLLPERIOD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedWhichColPeriod = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLMISPOINTS, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedMisPoints = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLMISSINGPOINTS, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedMissingPoints = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_LBLCALLCC, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCCText = res.Value;
                ExpectedCCText = HttpUtility.HtmlDecode(ExpectedCCText);

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_COMMSG1, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCommsg1 = res.Value;


                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_COMMSG2, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedCommsg2 = res.Value;

                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_SMRYRESOURCE, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedSmryResource = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_DTLRESOURCE, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedDtlResource = res.Value;
                res = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTS_COLPERIODRESOURCE, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
                var ExpectedColPeriodResource = res.Value;

                var ActualPointsCollectedText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_POINTSCOLLECTED).Id))).Text;
                var ActualCurrentCollectionPeriodText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_OFFERDATE).Id))).Text;
                var ActualFinalDateText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_OFFERENDDATE).Id))).Text;
                var ActualWudLikeText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_LBLWUDLIKE).Id))).Text;
                var ActualCurrentPointsTotalText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_CURPNTSTOTAL).Id))).Text;
                var ActualWhichCollPeriodText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_WHICHCOLPERIOD).Id))).Text;
                var ActualMisPointsText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_MISPOINTS).Id))).Text;
                var ActualMissingPointsText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_MISSINGPOINTS).Id))).Text;
                var ActualCallccText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_CALLCC).Id))).Text;
                var ActualCommsg1Text = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_COMMSG1).Id))).Text;
                var ActualCommsg2Text = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_COMMSG2).Id))).Text;
                var ActualSmryHeaderText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_SUMMARYHEADER).Id))).Text;
                var ActualDtlHeaderText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.DETAILHEADER).Id))).Text;
                var ActualColPeriodHeaderText = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_COLPERIODHEADER).Id))).Text;


                Generic objGeneric = new Generic(ObjAutomationHelper);
                error=objGeneric.ValidateResourceValueWithHTMLContent(ActualCallccText, ExpectedCCText);

                Assert.AreEqual(ExpectedPoinstCollectedText, ActualPointsCollectedText, "Points So Far Collected text doesn't match");
                Assert.AreEqual(ExpectedCurrentCollectionPeriodText, ActualCurrentCollectionPeriodText, "Current Collection Period Text doesn't match");
                Assert.AreEqual(ExpectedFinlDateText, ActualFinalDateText, "Final Date Text doesn't match");

                Assert.AreEqual(ExpectedWudLikeText, ActualWudLikeText, "Would Like to Text doesn't match");
                Assert.AreEqual(ExpectedCurrentPointsTotalText, ActualCurrentPointsTotalText, "Current Points Text doesn't match");
                Assert.AreEqual(ExpectedWhichColPeriod, ActualWhichCollPeriodText, "Which Collection Period Text doesn't match");
                Assert.AreEqual(ExpectedMisPoints, ActualMisPointsText, "Missing Points Header Text doesn't match");
                Assert.AreEqual(ExpectedMissingPoints, ActualMissingPointsText, "Missing Points Text doesn't match");

                error=objGeneric.ValidateResourceValueWithHTMLContent(ActualCommsg1Text, ExpectedCommsg1);
                error=objGeneric.ValidateResourceValueWithHTMLContent(ActualCommsg2Text, ExpectedCommsg2);

                Assert.AreEqual(ExpectedSmryResource, ActualSmryHeaderText, "Summary header Text doesn't match");
                Assert.AreEqual(ExpectedDtlResource, ActualDtlHeaderText, "Detail Header Text doesn't match");
                Assert.AreEqual(ExpectedColPeriodResource, ActualColPeriodHeaderText, "COllection Period Header Text Text doesn't match");
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

        public Tuple<string, string> GetCurrentCollectionPeriodData(long customerID, string culture)
        {
            List<string> StartDate = new List<string>();
            List<string> EndDate = new List<string>();
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                DBConfiguration dateConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DATERANGE_FOR_COLLECTION_PERIOD, SanityConfiguration.DbConfigurationFile);
                bool isDateRangeEnabled = dateConfig.IsDeleted.ToUpper().Equals("N") && dateConfig.ConfigurationValue1.ToUpper().Equals("TRUE");


                DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();

                StartDate = client.GetCurrentCollectionPeriodStartDate(customerID, culture, dateFormatConfig.ConfigurationValue1);
                EndDate = client.GetCurrentCollectionPeriodEndDate(customerID, culture, dateFormatConfig.ConfigurationValue1);

                Assert.AreEqual(error, string.Empty);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));


            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Transaction Details Grid");
                Assert.Fail(ex.Message);
            }
            return new Tuple<string, string>(StartDate[0].ToString(), EndDate[0].ToString());
        }

        public bool Validatepoints(long customerID, string culture)
        {
            bool isError = false;
            List<string> CurrentPoints = new List<string>();
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                CurrentPoints = client.GetCurrentCollectionPeriodPoints(customerID, culture);
                var ActualPoints = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_POINTSTOTAL).Id))).Text;
                Assert.AreEqual(CurrentPoints[0].ToString(), ActualPoints, "Points Value doesn't match");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogInformation("Error in validating Transaction Details Grid");
                Assert.Fail(ex.Message);
            }
            return isError;
        }

        public string ValidatePointSummaryText(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();

            DateTime endDate, startDate;
            Debug.WriteLine("2|ValidatePointSummaryText|Starting");
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|customer id:" + customerID);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|offer id:" + offerId);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable1|get points");

            //check heading
            int numDays = -1;
            DBConfiguration ColMonthConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ColMonthName, DBConfigKeys.COLLECTIONPERIODMONTH, SanityConfiguration.DbConfigurationFile);
            Int32.TryParse(ColMonthConfig.ConfigurationValue1, out numDays);
            if (numDays == -1)
            {
                numDays = 30;
            }
            DateTime.TryParse(pointsSummaryInfo["StartDateTime"], out startDate);
            DateTime.TryParse(pointsSummaryInfo["EndDateTime"], out endDate);

            string endDateString = (endDate.Day <= 12) ? endDate.ToString("MMMM yyyy") : endDate.AddDays(numDays).ToString("MMMM yyyy");
            string lblStatement = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSSTMT, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            string endMonthYear = (endDate.Day <= 12) ? endDate.ToString("MMMM yyyy") : endDate.AddDays(numDays).ToString("MMMM yyyy");

            string expectedText = string.Format("{0} {1}", endMonthYear, lblStatement);
            Generic objGeneric = new Generic(ObjAutomationHelper);

            IWebElement h1 = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_PAGEHEADING), FindBy.CSS_SELECTOR_ID);
            if (h1 == null)
            {
                error.Append("H1 control not found. ");
            }
            else if (!h1.Text.EqualsTo(expectedText, true))
            {
                error.Append(string.Format("Heading not matched Actual: '{0}' Expected: '{1}' . ", h1.Text, expectedText));
            }

            // check summary
            string lblPtsColl = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            string lblPtsCollTo = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDTO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            expectedText = string.Format("{0} {1} {2} {3}", lblPtsColl, startDate.ToShortDateString(), lblPtsCollTo, endDate.ToShortDateString());
            IWebElement pSummary = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_PAGESUMMARY), FindBy.CSS_SELECTOR_ID);

            if (pSummary == null)
            {
                error.Append("Point Summary, summary p not found. ");
            }
            else if (!pSummary.Text.EqualsTo(expectedText, true))
            {
                error.Append(string.Format("Summary not matched Actual: '{0}' Expected: '{1}' . ", pSummary.Text, expectedText));
            }

            // bottom summary
            expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSREVIEW, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            string clickHere = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSCLICKHERE, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            expectedText = string.Format("{0} {1}", expectedText, clickHere);
            IWebElement pBottomSummary = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_BOTTOMSUMMARY), FindBy.CSS_SELECTOR_ID);

            if (pBottomSummary == null)
            {
                error.Append("Point Bottom Summary, summary p not found. ");
            }
            else if (!pBottomSummary.Text.EqualsTo(expectedText, true))
            {
                error.Append(string.Format("Bottom Summary not matched Actual: '{0}' Expected: '{1}' . ", pBottomSummary.Text, expectedText));
            }

            return error.ToString();
        }

        #endregion

        #region Private Method

        string Validate_TransactionData(List<IWebElement> rows, List<TransactionDetails> transactions)
        {
            Debug.WriteLine("Validate_TransactionData|Starting");
            Resource currencySymbol = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYSYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
            Resource currencySymbolAlpha = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYALPHASYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
            DBConfiguration DisableDecimalConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLE_CURRENCY_DECIMAL, SanityConfiguration.DbConfigurationFile);
            DBConfiguration dateFormatConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.DbConfigurationFile);
            DBConfiguration disableBonusPointsConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISABLE_BONUS_POINTS, SanityConfiguration.DbConfigurationFile);
            bool isBonusVisible = (disableBonusPointsConfig.IsDeleted.Equals("N") && disableBonusPointsConfig.ConfigurationValue1.Equals("1"));
            string msg = string.Empty;
            Debug.WriteLine("Validate_TransactionData|tr count" + rows.Count + " service count" + transactions.Count);
            for (int i = 1; i < rows.Count ; i++)
            {
                string actualValue = transactions[i - 1].AmountSpent;
                decimal amount;
                // formatting done in MCA App service adapter layer
                Decimal.TryParse(actualValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount);
                actualValue = amount.ToString();
                string formattedVal = amount.ToString();
                if (DisableDecimalConfig.IsDeleted.Equals("N") && DisableDecimalConfig.ConfigurationValue1.ToUpper().Equals("TRUE"))
                {
                    actualValue = amount.ToString().Replace('.', ',');
                    formattedVal = amount.ToString().Replace('.', ',');
                    formattedVal = (actualValue.Contains(',') ? actualValue.TrimEnd('0').TrimEnd(',') : actualValue.Contains('.') ? actualValue.TrimEnd('0').TrimEnd('.') : formattedVal);
                    formattedVal = formattedVal.Contains('.') ? actualValue : formattedVal.Contains(',') ? actualValue : formattedVal;
                }
                StringBuilder expected_Value = new StringBuilder();
                expected_Value.Append(AutomationHelper.GetMaskedClubcard(transactions[i - 1].ClubcardId) + " ");
                expected_Value.Append(transactions[i - 1].TransactionDateTime.ToString(dateFormatConfig.ConfigurationValue1) + " ");
                expected_Value.Append(transactions[i - 1].TransactionDescription + " ");
                expected_Value.Append(string.Format("{0}{1}{2}", currencySymbol.Value + " ", formattedVal, currencySymbolAlpha.Value) + " ");
                if (isBonusVisible)
                {
                    expected_Value.Append(transactions[i - 1].BonusPoints + " ");
                }
                expected_Value.Append(transactions[i - 1].TotalPoints);
                // remove extra spaces
                string expected = string.Join(" ", expected_Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                msg += expected.Trim().Equals(rows[i - 1].Text.Trim()) ? string.Empty : string.Format("data not matched at row {0}. Actual : {1} , Expected: {2}", i, rows[i - 1].Text, expected_Value.ToString());
            }
            Debug.WriteLine("Validate_TransactionData|Ending");
            return msg;
        }

        internal string VerifyPointsSummaryTable2(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();
            Debug.WriteLine("2|VerifyPointsSummaryTable2|Starting");
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable2|customer id:" + customerID);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Debug.WriteLine("2|VerifyPointsSummaryTable2|offer id:" + offerId);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable2|get points");
            string message = VerifyPointsSummaryTable2(pointsSummaryInfo);
            error.Append(message);
            Debug.WriteLine("2|VerifyPointsSummaryTable2|Ending");
            return error.ToString();
        }

        private string VerifyPointsSummaryTable2(Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            Generic objgeneric = new Generic(ObjAutomationHelper);
            IWebElement table = objgeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TOP_TABLE2BODY), FindBy.CSS_SELECTOR_ID);
            IWebElement tbody = table.FindElement(By.TagName("tbody"));
            List<IWebElement> trows = tbody.FindElements(By.TagName("tr")).ToList();
            string expectedText = string.Empty, expectedValue = string.Empty, errorMessage = string.Empty;
            string message = string.Empty;
            // tthrethreeewo rows
            if (trows.Count < 3)
            {
                error.Append("Top table 2 doesn't have 3 rows. ");
            }
            else
            {
                // first row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCO, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoPoints"];
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);

                // second row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCOLLECTEDFROMTESCOBANK, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoBankPoints"];
                errorMessage = ValidateSummaryRow(trows[1], expectedText, expectedValue);
                error.Append(errorMessage);

                // third row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOTOTALPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TotalPoints"];
                errorMessage = ValidateSummaryRow(trows[2], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        internal string VerifyPointsSummaryTable3(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();
            Debug.WriteLine("2|VerifyPointsSummaryTable3|Starting");
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable3|customer id:" + customerID);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Debug.WriteLine("2|VerifyPointsSummaryTable3|offer id:" + offerId);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);
            Debug.WriteLine("2|VerifyPointsSummaryTable3|get points");
            string message = VerifyPointsSummaryTable3(pointsSummaryInfo);
            error.Append(message);
            Debug.WriteLine("2|VerifyPointsSummaryTable3|Ending");
            return error.ToString();
        }

        private string VerifyPointsSummaryTable3(Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            Generic objgeneric = new Generic(ObjAutomationHelper);
            IWebElement table = objgeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TOP_TABLE3BODY), FindBy.CSS_SELECTOR_ID);
            IWebElement tbody = table.FindElement(By.TagName("tbody"));
            List<IWebElement> trows = tbody.FindElements(By.TagName("tr")).ToList();

            string message = string.Empty;
            switch (pointsSummaryInfo["PointSummaryDescEnglish"])
            {
                case "Reward":
                case "NonReward":
                    message = ValidateThirdTableStandard(trows, pointsSummaryInfo);
                    break;
                case "AirmilesReward":
                case "BAmilesReward":
                case "VirginMilesReward":
                    message = ValidateThirdTableBAAvios(trows, pointsSummaryInfo);
                    break;
            }
            error.Append(message);
            return error.ToString();
        }

        private string ValidateThirdTableBAAvios(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            //PointsConvertedAndCarried
            StringBuilder error = new StringBuilder();
            string expectedText = string.Empty, expectedValue = string.Empty, errorMessage = string.Empty;
            string message = string.Empty;
            // one rows
            if (trows.Count == 0)
            {
                error.Append("Top table 3 doesn't have any row. ");
            }
            else
            {
                // first row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCONVERTEDANDCARRIEDFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TotalCarriedForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        private string ValidateThirdTableStandard(List<IWebElement> trows, Dictionary<string, string> pointsSummaryInfo)
        {
            StringBuilder error = new StringBuilder();
            string expectedText = string.Empty, expectedValue = string.Empty, errorMessage = string.Empty;
            string message = string.Empty;
            // one rows
            if (trows.Count == 0)
            {
                error.Append("Top table 3 doesn't have any row. ");
            }
            else
            {
                // first row
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TotalCarriedForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[0], expectedText, expectedValue);
                error.Append(errorMessage);
            }
            return error.ToString();
        }

        public string ValidateTescoPointsTable(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();
            string errorMessage = string.Empty, expectedText = string.Empty, expectedValue = string.Empty;
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);

            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offerId));
            IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints");
            IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                .Descendants("BoxLogoFileName");
            IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                .Descendants("DataColumnName");

            IEnumerable<XElement> DataBoxNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoPoints")
                .Descendants("BoxName");
            List<XElement> listBoxFiles = BoxLogoFileNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            List<XElement> listDataFiles = DataColumnNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            List<XElement> listDataNames = DataBoxNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            int iCount = listBoxFiles.Count();

            Generic objgeneric = new Generic(ObjAutomationHelper);
            IWebElement table = objgeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOPOINTS_TABLE), FindBy.CSS_SELECTOR_ID);
            IWebElement tbody = table.FindElement(By.TagName("tbody"));
            List<IWebElement> trows = tbody.FindElements(By.TagName("tr")).ToList();

            //check header
            IWebElement thead = table.FindElement(By.TagName("thead"));
            List<IWebElement> hrows = thead.FindElements(By.TagName("tr")).ToList();
            if (hrows.Count == 0)
            {
                error.Append("Header row not present. ");
            }
            else
            {
                List<string> headings = new List<string>();
                headings.Add(AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSWHERE, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value);
                headings.Add(AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSPAGETITLE, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value);

                errorMessage = ValidateSummaryHeader(hrows[0], headings);
                error.Append(errorMessage);
            }
            //check rows
            if (trows.Count - 5 != iCount)
            {
                error.Append("Count of Tesco Points are not same. ");
            }
            else
            {
                for (int i = 0; i < iCount; i++)
                {
                    if (listDataFiles[i].Value.Equals("icentre.jpg"))
                    {
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSNUTRI, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                    }
                    else if (listDataFiles[i].Value.Equals("other.jpg"))
                    {
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSOTHER, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                    }
                    else
                    {
                        expectedText = listDataNames[i].Value;
                    }
                    expectedValue = pointsSummaryInfo.ContainsKey(listDataFiles[i].Value) ? pointsSummaryInfo[listDataFiles[i].Value] : "0";
                    errorMessage = ValidateSummaryRow(trows[i], expectedText, expectedValue);
                    error.Append(errorMessage);
                }
            }
            //check footer rows
            if (trows.Count != iCount + 5)
            {
                error.Append("Footer rows not available. ");
            }
            else
            {
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSTESCOFOOT1, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoBroughtForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[iCount], expectedText, expectedValue);
                error.Append(errorMessage);

                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCHANGEFROMCLUBCARDBOOST, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoPointsChangeFromRewards"];
                errorMessage = ValidateSummaryRow(trows[iCount + 1], expectedText, expectedValue);
                error.Append(errorMessage);

                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = (pointsSummaryInfo["TescoPointsChangeFromRewards"].TryParse<Int32>()
                    + pointsSummaryInfo["TescoBroughtForwardPoints"].TryParse<Int32>()
                    + pointsSummaryInfo["TescoPoints"].TryParse<Int32>()).ToString();
                errorMessage = ValidateSummaryRow(trows[iCount + 2], expectedText, expectedValue);
                error.Append(errorMessage);

                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                    case "XmasSavers":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = string.Format("{0} {1} {2}", currency, pointsSummaryInfo["TescoTotalReward"], currencyAlpha);
                        break;
                    case "AirmilesReward":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = pointsSummaryInfo["TescoRewardMiles"];
                        break;
                    case "BAmilesReward":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = pointsSummaryInfo["TescoRewardMiles"];
                        break;
                    case "VirginMilesReward":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = pointsSummaryInfo["TescoRewardMiles"];
                        break;
                }
                errorMessage = ValidateSummaryRow(trows[iCount + 3], expectedText, expectedValue);
                error.Append(errorMessage);

                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoCarriedForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[iCount + 4], expectedText, expectedValue);
                error.Append(errorMessage);
            }

            return error.ToString();
        }

        public string ValidateTescoBankPointsTable(string clubcard, int index)
        {
            StringBuilder error = new StringBuilder();
            string errorMessage = string.Empty, expectedText = string.Empty, expectedValue = string.Empty;
            CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
            long customerID = customerServiceAdpator.GetCustomerID(clubcard, CountrySetting.culture);
            ClubcardServiceAdapter client = new ClubcardServiceAdapter();
            string offerId = client.GetOfferID(customerID, CountrySetting.culture, index);
            Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offerId, CountrySetting.culture);

            var currency = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYSYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
            var currencyAlpha = AutomationHelper.GetResourceMessageNew(LabelKey.CURRENCYALPHASYM, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;

            XDocument xmlDocument = XDocument.Load(string.Format(ConfigurationManager.AppSettings["StatementFormat"], CountrySetting.country, offerId));
            IEnumerable<XElement> Boxes = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                    .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints");
            IEnumerable<XElement> BoxLogoFileNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                .Descendants("BoxLogoFileName");
            IEnumerable<XElement> DataColumnNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                .Descendants("DataColumnName");
            IEnumerable<XElement> DataBoxNames = xmlDocument.Descendants("Statement").Where(x => x.Attribute("StatementType").Value == "Reward")
                .Descendants("PointsBox").Where(x => x.Element("SectionType").Value == "TescoBankPoints")
                .Descendants("BoxName");

            List<XElement> listBoxFiles = BoxLogoFileNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            List<XElement> listDataFiles = DataColumnNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            List<XElement> listDataNames = DataBoxNames.ToList().FindAll(c => !string.IsNullOrEmpty(c.Value));
            int iCount = listBoxFiles.Count();

            Generic objgeneric = new Generic(ObjAutomationHelper);
            IWebElement table = objgeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.POINTSSUMMARY_TESCOBANKPOINTS_TABLE), FindBy.CSS_SELECTOR_ID);
            IWebElement tbody = table.FindElement(By.TagName("tbody"));
            List<IWebElement> trows = tbody.FindElements(By.TagName("tr")).ToList();

            //check header
            IWebElement thead = table.FindElement(By.TagName("thead"));
            List<IWebElement> hrows = thead.FindElements(By.TagName("tr")).ToList();
            if (hrows.Count == 0)
            {
                error.Append("Header row not present. ");
            }
            else
            {
                List<string> headings = new List<string>();
                headings.Add(AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSWHERE, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value);
                headings.Add(AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSPAGETITLE, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value);

                errorMessage = ValidateSummaryHeader(hrows[0], headings);
                error.Append(errorMessage);
            }
            //check rows
            if (trows.Count - 4 != iCount)
            {
                error.Append("Count of Tesco Points are not same. ");
            }
            else
            {
                for (int i = 0; i < iCount; i++)
                {
                    if (listDataFiles[i].Value.Equals("icentre.jpg"))
                    {
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSNUTRI, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                    }
                    else if (listDataFiles[i].Value.Equals("other.jpg"))
                    {
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.MYPOINTSOTHER, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                    }
                    else
                    {
                        expectedText = listDataNames[i].Value;
                    }
                    expectedValue = pointsSummaryInfo.ContainsKey(listDataFiles[i].Value) ? pointsSummaryInfo[listDataFiles[i].Value] : "0";
                    errorMessage = ValidateSummaryRow(trows[i], expectedText, expectedValue);
                    error.Append(errorMessage);
                }
            }
            //check footer rows
            if (trows.Count != iCount + 4)
            {
                error.Append("Footer rows not available. ");
            }
            else
            {
                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.POINTSCARRIEDFORWARDFROMPREVIOUS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoBankBroughtForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[iCount], expectedText, expectedValue);
                error.Append(errorMessage);

                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TOTALTESCOBANKPOINTS, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = (pointsSummaryInfo["TescoBankPoints"].TryParse<Int32>() + pointsSummaryInfo["TescoBankBroughtForwardPoints"].TryParse<Int32>()).ToString();
                errorMessage = ValidateSummaryRow(trows[iCount + 1], expectedText, expectedValue);
                error.Append(errorMessage);


                switch (pointsSummaryInfo["PointSummaryDescEnglish"])
                {
                    case "Reward":
                    case "NonReward":
                    case "XmasSavers":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKVOUCHERTOTAL, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = string.Format("{0} {1} {2}", currency, pointsSummaryInfo["TescoBankTotalReward"], currencyAlpha);
                        break;
                    case "AirmilesReward":
                    case "BAmilesReward":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.AVIOSAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = pointsSummaryInfo["TescoBankRewardMiles"];
                        break;
                    case "VirginMilesReward":
                        expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.FLYINGAWARDED, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                        expectedValue = pointsSummaryInfo["TescoBankRewardMiles"];
                        break;
                }
                errorMessage = ValidateSummaryRow(trows[iCount + 2], expectedText, expectedValue);
                error.Append(errorMessage);

                expectedText = AutomationHelper.GetResourceMessageNew(LabelKey.TESCOBANKPOINTSCARRIEDCFORWARD, SanityConfiguration.ResourceFiles.POINTSSUMMARYPOINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE)).Value;
                expectedValue = pointsSummaryInfo["TescoBankCarriedForwardPoints"];
                errorMessage = ValidateSummaryRow(trows[iCount + 3], expectedText, expectedValue);
                error.Append(errorMessage);
            }

            return error.ToString();
        }

        private string ValidateSummaryRow(IWebElement row, string expectedText, string expectedValue)
        {
            StringBuilder error = new StringBuilder();
            List<IWebElement> cells = row.FindElements(By.TagName("td")).ToList();
            if (cells.Count > 1)
            {
                var actualText = cells[0].Text.Trim();
                if (!expectedText.EqualsTo(actualText, true))
                {
                    error.Append(string.Format("Actual Text '{0}' Expected Text '{1}'. ", actualText, expectedText));
                }

                var actualValue = cells[1].Text;
                if (!expectedValue.EqualsTo(actualValue, true))
                {
                    error.Append(string.Format("Actual Value '{0}' Expected Value '{1}'. ", actualValue, expectedValue));
                }
            }
            else
            {
                error.Append("Invalid Row");
            }
            return error.ToString();
        }

        private string ValidateSummaryHeader(IWebElement row, List<string> headings)
        {
            StringBuilder error = new StringBuilder();
            List<IWebElement> cells = row.FindElements(By.TagName("th")).ToList();
            if (cells.Count == headings.Count)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    if (!cells[i].Text.EqualsTo(headings[i], true))
                    {
                        error.Append(string.Format("Actual Heading '{0}' Expected Heading '{1}'. ", cells[i].Text, headings[i]));
                    }
                }
            }
            else
            {
                error.Append("Header row does not have all columns. ");
            }
            return error.ToString();
        }

        private string GetColMonthName(DateTime colEndDate, bool onlyMonthFlg, CultureInfo culture)
        {
            string yearFormat = " yyyy";
            if (onlyMonthFlg)
                yearFormat = string.Empty;

            return (colEndDate.Day <= 12) ? colEndDate.ToString("MMMM" + yearFormat, culture) :
                                      colEndDate.AddMonths(1).ToString("MMMM" + yearFormat, culture);

        }

        #endregion


    }
}
