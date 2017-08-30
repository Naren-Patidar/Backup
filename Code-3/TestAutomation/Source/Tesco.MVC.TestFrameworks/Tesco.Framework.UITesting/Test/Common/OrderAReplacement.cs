using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    class OrderAReplacement : Base
    {
        #region Constructor

        Generic objGeneric = null;
        public OrderAReplacement(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }
        public OrderAReplacement(AutomationHelper objHelper, AppConfiguration configuration)
        {
            ObjAutomationHelper = objHelper;
            objGeneric = new Generic(objHelper);
            SanityConfiguration = configuration;
        }
        #endregion
        public void GetClubcardMaskedDigits(string key)
        {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Clubcard Masked Digits", TraceEventType.Start);
                if ((objGeneric.IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(key).XPath))))
                {
                    var clubcardNumber = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(key).XPath)).Text).ToString();
                    string clubcardMaskedElements = clubcardNumber.Substring(7, 9);
                    Assert.AreEqual("XXXX XXXX", clubcardMaskedElements);
                }
                else
                {
                    Assert.Inconclusive("Clubacrd Number is not present in Order Replacement page for Country-"+ CountrySetting.country);
                }
           }

        public void OrderReplacementMaxCountReached(long customerID, string culture)
        {
            try
            {
                int orderReplacementCount = 0;
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Order Replacement requests count", TraceEventType.Start);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                orderReplacementCount = client.GetOrderReplacementCount(customerID, culture);
                if (orderReplacementCount >= 3)
                {
                    var sorryText = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_SORRY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
                    var customerservice = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_CUSTOMERCARE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
                    var commMsg1 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_COMMMSG1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
                    var commMsg2 = AutomationHelper.GetResourceMessage(ValidationKey.ORDERREPLACEMENT_COMMMSG2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE)).Value;
                    var expectedMsg = string.Concat(sorryText, customerservice, commMsg1, commMsg2);
                    var actualMsg = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_MAXORDERSREACHED).XPath)).Text).ToString(); //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_MAXORDERSREACHED).Id)).Text;
                    objGeneric.ValidateResourceValueWithHTMLContent(actualMsg,expectedMsg);
                }
                else
                {
                    Assert.Fail("Number of Placed orders is not less than 3");
                    CustomLogs.LogInformation("OrdeReplacement within a year less than 3 for this customer");
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

        public bool VerifySuccessfulConfirm()
        {
            bool bReasonSelected = false;
            try
            {
                CustomLogs.LogMessage("Verifying the Confirm functionality on Order Replacement Page", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                IWebElement rdbtnStolen = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_RADIOSTOLEN).Id));
                if (rdbtnStolen.Selected == true)
                {
                    bReasonSelected = true;
                }
                if (bReasonSelected)
                {
                    objGeneric.ClickElementJavaElement(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM, "OrderAReplacement");
                    Thread.Sleep(5000);
                    if((objGeneric.IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_ORDERINPROCESSMSG).XPath))))
                    {
                        return true;
                    }
                    else
                    {
                        Assert.Fail("Test case Failed. Reason is Selected yet success message is not displayed to the user");
                        return false;
                    }
                }
                else
                {
                   //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM).Id)).Click();
                    objGeneric.ClickElementJavaElement(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM, "OrderAReplacement");
                    Thread.Sleep(5000);
                    if ((objGeneric.IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_REASONERRORMESSAGE).XPath))))
                    {
                        CustomLogs.LogInformation("Error message is displayed to the user.Please select a Reason");
                        return true;
                    }
                    else 
                    {
                        Assert.Fail("Test case Failed. Reason is not Selected yet error message is not displayed to the user");
                        return false;
                    }

                }
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
                return true;
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return false;
            }
        }

        public bool ResetOrderReplacementStubData(string ClubcardNumber, string culture)
        {
            string errorXml = string.Empty;
            bool bSuccess = false;
            long CustomerID = 0;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Order Replacement requests count", TraceEventType.Start);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
                CustomerID = customerAdaptor.GetCustomerID(ClubcardNumber, culture);
                bSuccess = client.ResetOrderReplacementData(CustomerID, ClubcardNumber, culture);
                if (!string.IsNullOrEmpty(errorXml))
                {
                    Assert.Fail("Failed while resetting OrderReplacement Data");
                    CustomLogs.LogInformation(errorXml);
                }
                return bSuccess;
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return false;
            }
        }

        public string VerifyPageTitle()
        {
            string error = string.Empty;
            try
            {
                CustomLogs.LogMessage("Verifying the page name for the page 'Current Points Details' started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessageNew(LabelKey.ORDERREPLACEMENTRS, SanityConfiguration.ResourceFiles._POINTS_NODE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.ORDERREPLACEMENT_RESOURCE));
                var expectedPageHeader = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedPageHeader, "expected link name"));
                var actualPageHeader = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYPOINTS_HEADER).Id)).Text;
                error =  !expectedPageHeader.Equals(actualPageHeader) ? string.Format("'Order Replacement' not Verified. Actual : {0}, Expected : {1}", actualPageHeader, expectedPageHeader) : string.Empty;
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
            }            
            return error;
        }

    }
}


