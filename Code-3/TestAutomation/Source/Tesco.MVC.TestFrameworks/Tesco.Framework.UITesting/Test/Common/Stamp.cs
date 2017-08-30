using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Constants;
using System.IO;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class Stamp : Base
    {
        Generic objGeneric = null;
        Login objLogin = null;
        TestData_AccountDetails testData = null;
        #region Constructor

        public Stamp(AutomationHelper objHelper, AppConfiguration configuration, TestData_AccountDetails testdata)
        {
            ObjAutomationHelper = objHelper;
            testData = testdata;
            SanityConfiguration = configuration;
            objGeneric = new Generic(objHelper);
            objLogin = new Login(objHelper, SanityConfiguration);
        }

        #endregion

        public IWebElement GetStamp(int index)
        {
            IWebElement required_stamp = null;
            Control stamps = ObjAutomationHelper.GetControl(ControlKeys.STAMPS);
            IWebElement wStamps = objGeneric.GetWebControl(stamps, FindBy.CSS_SELECTOR_ID);
            List<IWebElement> lstStamps = wStamps.FindElements(By.TagName("a")).ToList();
            if (lstStamps.Count > index)
            {
                required_stamp = lstStamps[index];
            }
            return required_stamp;
        }

        public string GetStampUrl(int index)
        {
            string expectedUrl = string.Empty;
            Control stamps = ObjAutomationHelper.GetControl(ControlKeys.STAMPS);
            IWebElement wStamps = objGeneric.GetWebControl(stamps, FindBy.CSS_SELECTOR_ID);
            List<IWebElement> lstStamps = wStamps.FindElements(By.TagName("a")).ToList();
            if (lstStamps.Count > index)
            {
                IWebElement required_stamp = lstStamps[index];
                if (required_stamp != null)
                {
                    expectedUrl = required_stamp.GetAttribute("href");
                }
            }
            return expectedUrl;
        }

        public string ValidateStampUrl(int index)
        {
            string error = string.Empty;
            string actualUrl = GetStampUrl(index);
            Resource resStamp = AutomationHelper.GetResourceMessage(string.Format("{0}{1}", ValidationKey.STAMP_URL, index + 1), Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE));            
            error = !actualUrl.ToUpper().Contains(resStamp.Value.ToUpper()) ? string.Format("Stamp Url does not match with resource. Actual : {0}, Expected : {1}", actualUrl, resStamp.Value) : string.Empty;
            return error;
        }

        public string ValidateStampAction(int index)
        {
            string error = string.Empty;
            IWebElement expectedStamp = GetStamp(index);
            string expectedlUrl = expectedStamp.GetAttribute("href");
            string target = expectedStamp.GetAttribute("target");
            bool isExternal = target == "_blank";
            if (!isExternal)
            {
                if (index > 2)
                {
                    objGeneric.ScrollToBottom();
                }
                switch (SanityConfiguration.DefaultBrowser)
                {
                    case Browser.S:
                        ((IWebElement)expectedStamp).Click();
                        break;
                    case Browser.GC:
                    case Browser.IE:
                    case Browser.MF:
                    default:
                        ((IJavaScriptExecutor)ObjAutomationHelper.WebDriver).ExecuteScript("arguments[0].click();", expectedStamp);
                        break;
                }
                ObjAutomationHelper.WebDriver.DismissAlert();
                ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                string actualUrl = ObjAutomationHelper.WebDriver.Url;

                // if navigated to security varification then pass the validation
                if (actualUrl.ToUpper().Contains("SECURITYHOME"))
                {
                    objLogin.SecurityLayer_Verification(testData.MainAccount.Clubcard);
                    ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                    actualUrl = ObjAutomationHelper.WebDriver.Url;
                }

                if (!IsChristmasSaverStamp(index))
                {
                    error = !actualUrl.ToUpper().Equals(expectedlUrl.ToUpper()) ? string.Format("Stamp action didnot redirected to correct page. Actual : {0}, Expected : {1}", actualUrl, expectedlUrl) : string.Empty;
                }
            }
            return error;
        }

        public bool IsChristmasSaverStamp(int index)
        {
            bool chk = false;
            Resource res1 = AutomationHelper.GetResourceMessage(string.Format("{0}{1}", ValidationKey.STAMP_URL, index + 1), Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE));
            chk = res1.Value.ToUpper().Contains("CHRISTMASSAVER");            
            return chk;
        }

        public string ValidateStamp(int index)
        {
            string error = string.Empty;
            error = ValidateStampUrl(index);
            error += ValidateStampAction(index);
            return error;
        }

    }
}
