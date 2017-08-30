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
        public static Int64 CustomerID = 0;
        #region Constructor


        public OrderAReplacement(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }
        public OrderAReplacement(AutomationHelper objHelper, AppConfiguration configuration)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            TestData = ObjAutomationHelper.GetTestDataByID(Enums.Messages.Login);
            SanityConfiguration = configuration;
        }
        #endregion
        public void GetClubcardMaskedDigits(string key)
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Clubcard Masked Digits", TraceEventType.Start);
                var clubcardNumber = (Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(key).XPath)).Text).ToString();
                string clubcardMaskedElements = clubcardNumber.Substring(7, 9);
                Assert.AreEqual("XXXX XXXX", clubcardMaskedElements);
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
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
                    var tempActualMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_MAXORDERSREACHED).Id)).Text;
                    var actualMsg = tempActualMsg.Replace("\r\n", "");

                    Assert.AreEqual(expectedMsg, actualMsg);
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
                    Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM).Id)).Click();
                    string style = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_ORDERINPROCESSMSG).Id)).GetAttribute("style");
                    if (style == "DISPLAY: block")
                    {
                        return true;
                    }
                    else if (style == "DISPLAY: none")
                    {
                        Assert.Fail("Test case Failed. Reason is Selected yet success message is not displayed to the user");
                        return false;
                    }
                }
                else
                {
                    // Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_BTNCONFIRM).Id)).Click();
                    string style = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ORDERREPLACEMENT_REASONERRORMESSAGE).Id)).GetAttribute("style");
                    if (style == "DISPLAY: block")
                    {
                        CustomLogs.LogInformation("Error message is displayed to the user.Please select a Reason");
                        return true;
                    }
                    else if (style == "DISPLAY: none")
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
          string errorXml=string.Empty;
          bool bSuccess = false;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Order Replacement requests count", TraceEventType.Start);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
                CustomerID = customerAdaptor.GetCustomerID(ClubcardNumber, culture);
                bSuccess=  client.ResetOrderReplacementData(CustomerID,ClubcardNumber, culture);
                if(!string.IsNullOrEmpty(errorXml))
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

    }
}


