using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using Tesco.Framework.UITesting.Services;
using System.Data;
using Tesco.Framework.UITesting.Constants;

namespace Tesco.Framework.UITesting.Test.Common
{
    class Generic : Base
    {
        string isPresent = string.Empty;
        string configuration1 = string.Empty;
        public IAlert alert = null;
        #region Constructor


        public Generic(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }

        #endregion

        public void linkNavigate(string msgId, string key, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Navigating to the link " + pageName + " Started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                var expectedLinkName = res.Value;
                IWebElement leftNavigation = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.LEFT_NAVIGATION).Id));
                List<IWebElement> allLinks = leftNavigation.FindElements(By.TagName("a")).ToList();
                var expectedLink = allLinks.Find(a => a.GetAttribute("text") == expectedLinkName);
                if (expectedLink != null)
                {
                    switch (SanityConfiguration.DefaultBrowser)
                    {
                        case Browser.S:
                            ((IWebElement)expectedLink).Click();
                            break;
                        case Browser.GC:
                        case Browser.IE:
                        case Browser.MF:
                        default:
                            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", expectedLink);
                            break;
                    }
                    // Driver.DismissAlert();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                    string msg = string.Format("Left Navigation link {0} under {1} is present", expectedLinkName, key);
                    CustomLogs.LogInformation(msg);
                }
                else
                {
                    string msg = string.Format("Left Navigation link with text '{0}' not found", expectedLinkName);
                    CustomLogs.LogInformation(msg);
                    Assert.Fail(msg);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Page is navigated to the page " + pageName + " Completed", TraceEventType.Stop);
        }

        public string verifyPageEnabled(string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HideJoinFunctionality, keys, SanityConfiguration.DbConfigurationFile);

                //string configuration1 = config.ConfigurationValue1;
                isPresent = config.IsDeleted;
                return isPresent;
                // return configuration1;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isPresent;
            }
        }

        public bool IsPageEnabled(string key)
        {
            bool isEnabled = false;
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HideJoinFunctionality, key, SanityConfiguration.DbConfigurationFile);

                if (key.ToUpper().Contains("HIDE"))
                {
                    isEnabled = config.IsDeleted.Equals("N") && config.ConfigurationValue1.Equals("0");
                }
                else
                {
                    isEnabled = config.IsDeleted.Equals("N") && config.ConfigurationValue1.Equals("1");
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                Assert.Fail(string.Format("Unable to load dbConfiguration xml. Error:' {0} '", ex.Message));
            }
            return isEnabled;
        }


        public void verifyPageEnabledbasedonDBconfig(string keys, out string isPresent, out string configuration1)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);

            DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HideJoinFunctionality, keys, SanityConfiguration.DbConfigurationFile);
            configuration1 = config.ConfigurationValue1;
            isPresent = config.IsDeleted;
        }

        public string verifyPageEnabledByWebConfig(string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from Web configuration ", TraceEventType.Start);
                // WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(keys, SanityConfiguration.WebConfigurationFile);
                //isPresent = webConfig.Value;
                //return isPresent;

                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, keys, SanityConfiguration.DbConfigurationFile);
                string configuration1 = config.ConfigurationValue1;
                return configuration1;
                // isPresent = config.IsDeleted;
            }

            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return configuration1;
            }
        }

        public string verifyKeyEnabled(ConfugurationTypeEnum configurationType, string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(configurationType, keys, SanityConfiguration.DbConfigurationFile);
                string configValue1 = config.ConfigurationValue1;

                //if (config.ConfigurationValue1 == "true" || config.ConfigurationValue1 == "1")
                //{
                //    isPresent = "Enabled";
                //}
                //else if (config.ConfigurationValue1 == "false" || config.ConfigurationValue1 == "0")
                //{
                //    isPresent = "Disabled";
                //}
                return configValue1;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isPresent;
            }
        }

        public void linkNavigateWithTwoRersource(string msgexpected, string key, String pageName)
        {
            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, " Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                IWebElement ele = null;


                ObjAutomationHelper.GetControl(key).WaitForControlByxPath(Driver);
                ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));
                var expectedLink = actualLinkName.ToList().Find(lnk => lnk.Text == msgexpected);

                if (expectedLink != null)
                {
                    ((IWebElement)expectedLink).Click();
                    string msg = string.Format("link {0} under {1} is present", msgexpected, key);
                    CustomLogs.LogInformation(msg);
                    // ele.WaitForTitle(Driver, expectedLink.ToString());
                    // ele.WaitForTitle(Driver, pageName);
                }
                else
                {
                    string msg = string.Format("link {0} under {1} is not present", msgexpected, key);
                    CustomLogs.LogInformation(msg);
                    Assert.Fail(msg);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, " Ending"));
        }

        public void ClickElement(string keys, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on Element " + keys + " starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Click();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                if (isAlertPresent())
                    alert.Accept();
                CustomLogs.LogMessage(keys + " Element is clicked", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ClickElement(string key, FindBy by, object[] parameters = null)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on Element " + key + " starting", TraceEventType.Start);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
                Control ctrl = null;
                By byQuery = null;
                IWebElement eCtrl = null;
                if (parameters != null)
                {
                    List<string> pars = new List<string>();
                    foreach (object p in parameters)
                    {
                        Resource res = AutomationHelper.GetResourceMessage(p.ToString(), Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                        pars.Add(res.Value);
                    }
                    key = string.Format(key, pars);
                }
                switch (by)
                {
                    case FindBy.ID:
                        jse.ExecuteScript(string.Format("document.getElementById('{0}').click();", key));
                        break;
                    case FindBy.CSS_CLASS:
                        break;
                    case FindBy.TAG_NAME:
                        break;
                    case FindBy.ATTRIBUTES:
                        jse.ExecuteScript(string.Format("document.querySelectorAll('{0}').click();", key));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = ObjAutomationHelper.GetControl(key);
                        byQuery = By.CssSelector(ctrl.Id);
                        eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = ObjAutomationHelper.GetControl(key);
                        byQuery = By.CssSelector(ctrl.ClassName);
                        eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                        break;
                    case FindBy.XPATH_SELECTOR:
                        ctrl = ObjAutomationHelper.GetControl(key);
                        byQuery = By.XPath(ctrl.XPath);
                        eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                        break;
                }
                if (eCtrl != null)
                {
                    switch (SanityConfiguration.DefaultBrowser)
                    {
                        case Browser.S:
                            ((IWebElement)eCtrl).Click();
                            break;
                        case Browser.GC:
                        case Browser.IE:
                        case Browser.MF:
                        default:
                            ((IJavaScriptExecutor)ObjAutomationHelper.WebDriver).ExecuteScript("arguments[0].click();", eCtrl);
                            break;
                    }
                }
                if (isAlertPresent())
                {
                    alert.Accept();
                }
                Debug.WriteLine(string.Format("{0}|{1}-{2}", new object[] { "ClickElement", "Element clicked", key }));
                CustomLogs.LogMessage(key + " Element is clicked", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ClickElementJavaElement(string keys, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on Element " + keys + " starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                IWebElement print = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                // Driver.ExecuteJs<bool>("document.getElementById", print);
                jse.ExecuteScript("arguments[0].click();", print);
                // jse.ExecuteScript("document.getElementById(Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id))).click();");
                //  Driver.ExecuteJs<bool>("arguments[0].click();return true;", print);
                Thread.Sleep(2000);
                CustomLogs.LogMessage(keys + " Element is clicked on page - " + pageName , TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ClickAnchorTagInsideAContainer(string key, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on Element " + key + " starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                IWebElement anchorContainer = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                anchorContainer.FindElement(By.XPath("..//a")).Click();

                CustomLogs.LogMessage(key + " Element is clicked on page - " + pageName, TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ClickElementByXpath(string keys, String pageName)
        {
            try
            {
                //Driver = ObjAutomationHelper.WebDriver;
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, " Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                ObjAutomationHelper.GetControl(keys).WaitForControlByxPath(Driver);
                IWebElement ele = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                jse.ExecuteScript("arguments[0].click();", ele);
                CustomLogs.LogInformation(keys + "is clicked");
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ClickStamp(string msgId, string keys, String pageName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                var expectedLinkName = res.Value;
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Click();
                CustomLogs.LogInformation(expectedLinkName + " is clicked");
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void NavigateBrowserBack()
        {
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            Driver = ObjAutomationHelper.WebDriver;
            Driver.Navigate().Back();
        }

        public void verifyPageName(string msgId, String pageName, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                var expectedLinkName = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected link name"));
                var actualPageHeader = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PAGE_TITLE).XPath)).Text;
                Assert.AreEqual(expectedLinkName, actualPageHeader, pageName + " not Verified");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);
        }

        public void verifyPageNameNew(string msgId, string node, String pageName, string resourceFileName)
        {
            try
            {
                CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //  Fetch Details From resource.XML

                Resource res = AutomationHelper.GetResourceMessageNew(msgId, node, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                var expectedLinkName = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedLinkName, "expected link name"));
                var actualPageHeader = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.PAGE_TITLE).XPath)).Text;
                Assert.AreEqual(expectedLinkName, actualPageHeader, pageName + " not Verified");
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Driver.Quit();
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);
        }

        public void verifyValidationMessage(string msgId, string keys, String pageName, string LocalResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var actualMessage = String.Empty;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                IWebElement lblError = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                ScrollToElement(lblError);
                actualMessage = lblError.Text;
                if (msgId.Equals(LabelKey.MLSTESCOBANK))
                {
                    string error = ValidateResourceValueWithHTMLContent(actualMessage, expectedMessage);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Assert.Fail("Text not verified on" + pageName);
                    }
                }
                else
                {
                    Assert.AreEqual(expectedMessage, actualMessage.TrimStart(), pageName + " not verified");
                    Debug.WriteLine(string.Format("{0}|{1}-{2}", new object[] { "verifyValidationMessage", "Validation Message checked", msgId }));
                    CustomLogs.LogMessage("Verifying validation message for page " + pageName + " completed", TraceEventType.Stop);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Debug.WriteLine(string.Format("{0}|{1}-{2}", new object[] { "verifyValidationMessage", "Validation Message failed", msgId }));
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void verifyValidationMessageByValue(string msgId, string keys, String pageName, string LocalResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var actualMessage = String.Empty;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Text;
                string error=ValidateResourceValueWithHTMLContent(actualMessage.TrimStart(), expectedMessage);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                 CustomLogs.LogMessage("Verifying validation message for page " + pageName + " completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void verifyValidationMessageByClass(string msgId, string keys, String pageName, string LocalResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var actualMessage = String.Empty;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName)).Text;
                Assert.AreEqual(expectedMessage, actualMessage.TrimStart(), pageName + " not verified");
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void VerifyTextonthePageByXpath(string msgId, string keys, String pageName, string LocalResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Text;

                if (actualMessage.Contains(expectedMessage))
                {
                    CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);
                }
                else
                    Assert.Fail(expectedMessage + " is not same as" + actualMessage + pageName);

                //Assert.AreEqual(expectedMessage, actualMessage, pageName + " not verified");
                //CustomLogs.LogMessage("Verifying validation message for page " + pageName + " completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public string ValidateText(string controlId, string LocalResourceFileName, List<string> resourceIds, FindBy query)
        {
            string error = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying validation text for page  started", TraceEventType.Start);
                IWebElement ctrl = null;
                string expectedMsg = string.Empty;
                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlId).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlId).Id));
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlId).ClassName));
                        break;
                }
                foreach (string resId in resourceIds)
                {
                    string msg = AutomationHelper.GetResourceMessage(resId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                    expectedMsg += string.Format("{0} ", msg);
                }
                expectedMsg = expectedMsg.Trim();

                // format html tags                
                Regex htmls = new Regex("<[^>]*>");
                List<string> matches = htmls.Split(expectedMsg).ToList();
                List<string> lstMatches = new List<string>();
                matches.ForEach(m => lstMatches.Add(m.Trim()));
                expectedMsg = string.Join(" ", lstMatches);


                if (!expectedMsg.ToUpper().Equals(ctrl.Text.ToUpper()))
                {
                    error = string.Format("expected: {0} , actual: {1} ", expectedMsg, ctrl.Text);
                    CustomLogs.LogInformation(error);
                }
                CustomLogs.LogMessage("Verifying validation text for page  completed", TraceEventType.Start);

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return error;
        }

        public bool VerifyText_Contains(string msgId, string keys, string ResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Inside method VerifyText_Contains,returns true or false ", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, ResourceFileName));
                var expectedMessage = res.Value;
                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Text;
                if (actualMessage.Contains(expectedMessage))
                    return true;
                else
                    return false;

            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }

        }
        public string VerifyText_Contains(string msgId, string controlKey, string ResourceFileName, string pageName)
        {
            string errorMessage = string.Empty;
            try
            {
                if ((IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath))))
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Inside method VerifyText_Contains,returns string ", TraceEventType.Start);
                    Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, ResourceFileName));
                    var expectedMessage = res.Value;
                    var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath)).Text.Trim();
                    Regex htmls = new Regex("<[^>]*>");
                    List<string> matches = htmls.Split(expectedMessage).ToList();
                    List<string> lstMatches = new List<string>();
                    matches.ForEach(m => lstMatches.Add(m.Trim()));
                    expectedMessage = string.Join(" ", lstMatches);

                    List<string> matche = htmls.Split(actualMessage).ToList();
                    List<string> lstMatche = new List<string>();
                    matches.ForEach(m => lstMatche.Add(m.Trim()));
                    actualMessage = string.Join(" ", lstMatche);
                    if (actualMessage.Contains(expectedMessage))
                    {
                        CustomLogs.LogMessage("Validation of " + msgId + " on" + pageName + " completed", TraceEventType.Stop);
                    }
                    else
                        errorMessage = "Actual and Expected " + msgId + " text are not equal";

                    return errorMessage;
                }
                return errorMessage;
            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return errorMessage;
            }

        }


        public string VerifyText_Contains_year(string msgId, string controlKey, string ResourceFileName, string pageName, string culture)
        {
            string errorMessage = string.Empty;
            try
            {
                if ((IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath))))
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Inside method VerifyText_Contains,returns string ", TraceEventType.Start);
                    Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, ResourceFileName));
                    var expectedMessage = res.Value;

                    if (culture == "th-TH")
                    {
                        expectedMessage = String.Concat(expectedMessage, " ", DateTime.Now.Year.ToString());
                    }

                    var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath)).Text.Trim();
                    Regex htmls = new Regex("<[^>]*>");
                    List<string> matches = htmls.Split(expectedMessage).ToList();
                    List<string> lstMatches = new List<string>();
                    matches.ForEach(m => lstMatches.Add(m.Trim()));
                    expectedMessage = string.Join(" ", lstMatches);

                    List<string> matche = htmls.Split(actualMessage).ToList();
                    List<string> lstMatche = new List<string>();
                    matches.ForEach(m => lstMatche.Add(m.Trim()));
                    actualMessage = string.Join(" ", lstMatche);
                    if (actualMessage.Contains(expectedMessage))
                    {
                        CustomLogs.LogMessage("Validation of " + msgId + " on" + pageName + " completed", TraceEventType.Stop);
                    }
                    else
                        errorMessage = "Actual and Expected " + msgId + " text are not equal";

                    return errorMessage;
                }
                return errorMessage;
            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return errorMessage;
            }

        }

        public string VerifyTextWithHtml_Contains(string msgId, string controlKey, string ResourceFileName, string pageName)
        {
            string errorMessage = string.Empty;
            try
            {
                if ((IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath))))
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Inside method VerifyText_Contains, string", TraceEventType.Start);
                    Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, ResourceFileName));
                    var expectedMessage = res.Value.Trim();
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    IWebElement element = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath));
                    string actualMessage = jse.ExecuteScript("return      arguments[0].innerHTML;", element).ToString().Replace("\r\n", "").Trim();
                    // Split the values at spaces, removing duplicates.
                    string[] expectedMsg = expectedMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    // Rejoin them.
                    string resultExpected = String.Join(";", expectedMsg);
                    resultExpected = WebUtility.HtmlDecode(resultExpected).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                    resultExpected = Regex.Replace(resultExpected, @"\s+", "");
                    // Split the values at spaces, removing duplicates.
                    string[] actualMsg = actualMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    // Rejoin them.
                    string resultActual = String.Join(";", actualMsg);
                    resultActual = WebUtility.HtmlDecode(resultActual).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                    resultActual = Regex.Replace(resultActual, @"\s+", "");
                    if (resultExpected.Equals(resultActual))
                    {
                        CustomLogs.LogMessage("Validation of " + msgId + " on" + pageName + " completed", TraceEventType.Stop);
                    }
                    else
                        errorMessage = "Actual and Expected " + msgId + " text are not equal";

                    return errorMessage;
                }
                return errorMessage;
            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return errorMessage;
            }

        }
        public void ClickAndVerifyLinkNavigationBrowserURL(string keys, String pageName, string Expectedvalue)
        {
            try
            {
                //Driver = ObjAutomationHelper.WebDriver;
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, " Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                ObjAutomationHelper.GetControl(keys).WaitForControlByxPath(Driver);
                Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Click();
                CustomLogs.LogInformation(keys + "is clicked");

                string actualURL = Driver.Url;
                Driver.Navigate().Back();
                Driver.Close();
                Assert.AreEqual(Expectedvalue, actualURL);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public bool VerifyBrowserAddress(string ExpectedAddress, IWebDriver driver, string linkName, bool res)
        {
            String url = driver.Url;
            string[] SplitParts = url.Split('.');

            string s2 = string.Empty;


            for (int i = 1; i < SplitParts.Length; i++)
            {
                s2 += SplitParts[i];
                if (i < SplitParts.Length - 1)
                {
                    s2 += ".";
                }
            }
            s2 = s2.Replace("/.", "");
            //Message = ObjAutomationHelper.GetMessageByID(address);
            //var expectedLinkName = Message.Title;
            bool res1;
            res1 = res;


            if (ExpectedAddress.Contains(s2))
            {
                CustomLogs.LogInformation(linkName + " link" + " is Verified");
                CustomLogs.LogInformation("\n");
            }

            else
            {
                CustomLogs.LogInformation(linkName + " Navigation address" + " is not correct");
                CustomLogs.LogInformation("\n");
                res1 = false;

            }
            return res1;
        }

        public bool isAlertPresent()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                alert = Driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

        public static bool IsElementPresent(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsElementPresentOnPage(By by)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public bool IsSecurityEnableOnHome(string DBkeys)
        {
            try
            {
                bool securityonHome = false;
                Driver = ObjAutomationHelper.WebDriver;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBkeys, SanityConfiguration.DbConfigurationFile);
                string isSecurityEnable = config.ConfigurationValue1;
                if (isSecurityEnable == "TRUE")
                {
                    securityonHome = true;
                }
                else
                { }
                return securityonHome;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }
        public string[] isStampPresent()
        {
            string[] stampKey = new string[] { "urlStamp1", "urlStamp2", "urlStamp3", "urlStamp4", "urlStamp5", "urlStamp6", "urlStamp7", "urlStamp8", "urlStamp9" };
            string expectedStamp = null;
            List<string> stampName = new List<string>();
            for (int i = 0; i < stampKey.Count(); i++)
            {
                Resource res = AutomationHelper.GetResourceMessage(stampKey[i], Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE));
                expectedStamp = res.Value;
                stampName.Add(expectedStamp.Split('/')[0]);
            }
            string[] array = stampName.ToArray();
            return array;


        }

        public void stampClick(string key, String pageName, string expectedLinkName)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on stamp " + expectedLinkName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));

                var expectedLink = actualLinkName.ToList().Find(lnk => lnk.GetAttribute("href").Contains(expectedLinkName));
                if (expectedLink != null)
                {
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    jse.ExecuteScript("arguments[0].click();", expectedLink);
                }
                else
                {
                    string msg = string.Format("Left Navigation link {0} under {1} is not present", expectedLinkName, key);
                    CustomLogs.LogInformation(msg);
                    Assert.Fail(msg);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Clicked on stamp " + expectedLinkName + " completed", TraceEventType.Stop);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        public string GetStampUrl(int index)
        {
            string expectedUrl = string.Empty;
            Control stamps = ObjAutomationHelper.GetControl(ControlKeys.STAMPS);
            IWebElement wStamps = GetWebControl(stamps, FindBy.CSS_SELECTOR_ID);
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

        public IWebElement GetStamp(int index)
        {
            IWebElement required_stamp = null;
            Control stamps = ObjAutomationHelper.GetControl(ControlKeys.STAMPS);
            IWebElement wStamps = GetWebControl(stamps, FindBy.CSS_SELECTOR_ID);
            List<IWebElement> lstStamps = wStamps.FindElements(By.TagName("a")).ToList();
            if (lstStamps.Count > index)
            {
                required_stamp = lstStamps[index];
            }
            return required_stamp;
        }


        //public void linkNavigateNew(string msgId, string node, string key, String pageName)
        //{
        //    try
        //    {
        //        CustomLogs.LogMessage("Navigating to the link on page " + pageName + " started", TraceEventType.Start);
        //        Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
        //        Driver = ObjAutomationHelper.WebDriver;
        //        Resource res = AutomationHelper.GetResourceMessageNew(msgId, node, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
        //        var expectedLinkName = res.Value;
        //    }


        //public string[] isStampPresent()
        //{
        //    string[] stampKey = new string[] { "urlStamp1", "urlStamp2", "urlStamp3", "urlStamp4", "urlStamp5", "urlStamp6", "urlStamp7", "urlStamp8", "urlStamp9" };
        //    string expectedStamp = null;
        //    List<string> stampName = new List<string>();
        //    for (int i = 0; i < stampKey.Count(); i++)
        //    {
        //        Resource res = AutomationHelper.GetResourceMessage(stampKey[i], Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE));
        //        expectedStamp = res.Value;
        //        stampName.Add(expectedStamp.Split('/')[0]);
        //    }
        //    string[] array = stampName.ToArray();
        //    return array;


        //}

        //public void stampClick(string key, String pageName, string expectedLinkName)
        //  {
        //      try
        //      {
        //          Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
        //          Driver = ObjAutomationHelper.WebDriver;
        //          ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));
        //          var expectedLink = actualLinkName.ToList().Find(lnk => lnk.GetAttribute("href").Contains(expectedLinkName));
        //          if (expectedLink != null)
        //          {
        //              ((IWebElement)expectedLink).Click();
        //          }
        //          else
        //          {
        //              string msg = string.Format("Left Navigation link {0} under {1} is not present", expectedLinkName, key);
        //              CustomLogs.LogInformation(msg);
        //              Assert.Fail(msg);
        //          }
        //      }
        //      catch (Exception ex)
        //      {
        //          CustomLogs.LogException(ex);
        //          ScreenShotDetails.TakeScreenShot(Driver, ex);
        //          Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //          Driver.Quit();
        //      }
        //      Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        //  }

        public Dictionary<string, string> isStampPresentbyKey()
        {
            string[] stampKey = new string[] { "urlStamp1", "urlStamp2", "urlStamp3", "urlStamp4", "urlStamp5", "urlStamp6", "urlStamp7", "urlStamp8", "urlStamp9" };
            string expectedStamp = null;
            //List<string> stampName = new List<string>();
            Dictionary<string, string> stampKeyValue = new Dictionary<string, string>();

            for (int i = 0; i < stampKey.Count(); i++)
            {
                Resource res = AutomationHelper.GetResourceMessage(stampKey[i], Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                expectedStamp = res.Value;
                stampKeyValue.Add(stampKey[i].Split(new string[] { "url" }, StringSplitOptions.None)[1], expectedStamp.Split('/')[0]);
                //stampName.Add(expectedStamp.Split('/')[0]);
            }
            //string[] array = stampName.ToArray();
            //return array;
            return stampKeyValue;


        }

        public void StampsTextValidation(string key, string msgId, string expectedLinkName)
        {


            try
            {
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));

                // var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(key).XPath)).Text;
                var ActualLink = actualLinkName.ToList().Find(lnk => lnk.GetAttribute("href").Contains(expectedLinkName));
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        
        public void VerifyTextExistOnPage(string keys)
        {
            try
            {
                CustomLogs.LogMessage("VerifyTextExistOnPage started", TraceEventType.Start);
                var textExist = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Text;
                if (textExist != null)
                    CustomLogs.LogInformation("Text Exist on the page");
                else
                    Assert.Fail("Text not there");

                CustomLogs.LogMessage("VerifyTextExistOnPage completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public string GetDataFromPage(string keys)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Inside method VerifyDataOnPage_Contains,returns true or false ", TraceEventType.Start);

                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Text;

                return actualMessage.ToString();


            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return "";
            }

        }

        public string GetTextFromResourceFile(string msgId, String pageName, string LocalResourceFileName)
        {
            try
            {
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " completed", TraceEventType.Stop);
                return expectedMessage.ToString();

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                return "";

            }
        }

        public bool VerifyDataOnPage_Contains_ByXpath(string DataToVerify, string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Inside method VerifyDataOnPage_Contains,returns true or false ", TraceEventType.Start);

                var expectedMessage = DataToVerify;
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(key).XPath)).Text;
                if (actualMessage.Contains(expectedMessage))
                    return true;
                else
                    return false;

            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }

        }

        public bool VerifyDataOnPage_Contains(string DataToVerify, string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Inside method VerifyDataOnPage_Contains,returns true or false ", TraceEventType.Start);

                var expectedMessage = DataToVerify;
                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id)).Text;
                if (actualMessage.Contains(expectedMessage))
                    return true;
                else
                    return false;

            }

            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }

        }


        public void ClearInputField(string id, FindBy query)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;

            Control ctrl = null;
            By byQuery = null;
            IWebElement eCtrl = null;
            switch (query)
            {
                case FindBy.ID:
                    jse.ExecuteScript(string.Format("document.getElementById('{0}').value = '';", id));
                    break;
                case FindBy.CSS_SELECTOR_ID:
                    ctrl = ObjAutomationHelper.GetControl(id);
                    byQuery = By.CssSelector(ctrl.Id);
                    eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                    eCtrl.Clear();
                    break;
                case FindBy.CSS_SELECTOR_CSS:
                    ctrl = ObjAutomationHelper.GetControl(id);
                    byQuery = By.CssSelector(ctrl.ClassName);
                    eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                    eCtrl.Clear();
                    break;
                case FindBy.XPATH_SELECTOR:
                    ctrl = ObjAutomationHelper.GetControl(id);
                    byQuery = By.XPath(ctrl.XPath);
                    eCtrl = ObjAutomationHelper.WebDriver.FindElement(byQuery);
                    eCtrl.Clear();
                    break;
            }

        }
        public bool returnConfigValue1Negative(ConfugurationTypeEnum configtype, string keyname)
        {

            try
            {
                CustomLogs.LogMessage("returnConfigValue1Negative started", TraceEventType.Start);
                string configValue1 = AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (configValue1.Equals("0") || configValue1.Equals("FALSE"))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }
        }
        public bool returnConfigValue1Positive(ConfugurationTypeEnum configtype, string keyname)
        {

            try
            {
                CustomLogs.LogMessage("returnConfigValue1Positive started", TraceEventType.Start);
                string configValue1 = AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (configValue1.Equals("1") || configValue1.Equals("TRUE"))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }
        }

        public bool VerifyRegex(ConfugurationTypeEnum configtype, string keyname, string value)
        {
            bool check = true;
            try
            {
                CustomLogs.LogMessage("VerifyRegex started", TraceEventType.Start);
                string configValue1 = AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (!string.IsNullOrEmpty(configValue1))
                {
                    check = Regex.IsMatch(value, configValue1);
                    Debug.WriteLine(string.Format("{0}|{1}-{2}", new object[] { "VerifyRegex", "regex result", check }));
                }
                else
                {
                    Debug.WriteLine(string.Format("{0}|{1}", new object[] { "VerifyRegex", "No regex provided" }));
                }
                return check;
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }


        }

        public string VerifyAppSettings(string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for AppSettings from DB configuration ", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, keys, SanityConfiguration.DbConfigurationFile);
                isPresent = config.IsDeleted;
                return isPresent;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isPresent;
            }
        }
        public void EnterDataInField(string key, string value)
        {
            Driver = ObjAutomationHelper.WebDriver;
            try
            {
                CustomLogs.LogMessage("EnterDataInField started", TraceEventType.Start);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id)).Clear();
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id)).SendKeys(value);
                Debug.WriteLine(string.Format("{0}|{1}-{2}", new object[] { "EnterDataInField", "Entered value", value }));
                CustomLogs.LogMessage("VerifyTextExistOnPage completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public int returnConfigLength(ConfugurationTypeEnum configtype, string keyname, Enums.FieldType type)
        {
            try
            {
                CustomLogs.LogMessage("returnConfigLength started", TraceEventType.Start);
                int length = 0;

                int configValue1 = 0, configValue2 = 0;
                Int32.TryParse(AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1, out configValue1);
                Int32.TryParse(AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue2, out configValue2);
                switch (type)
                {
                    case Enums.FieldType.Valid:
                        length = configValue2 - configValue1;
                        break;
                    case Enums.FieldType.InvalidLength1:
                        if (configValue1.Equals(0))
                        {
                            length = configValue1;
                        }
                        else
                        {
                            length = configValue1-1;
                        }
                        break;
                    case Enums.FieldType.InvalidLength2:
                        length = configValue2 + 1;
                        break;
                    case Enums.FieldType.MinLength:
                        length = configValue1;
                        break;
                }
                return length;
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return 0;
            }


        }

        public string GetConfigurationTypesCsv(List<ConfugurationTypeEnum> configurationTypes)
        {


            string configurationTypesCsv = string.Empty;
            try
            {

                foreach (ConfugurationTypeEnum configurationType in configurationTypes)
                {
                    configurationTypesCsv = configurationTypesCsv + (int)configurationType + ",";
                }

                if (configurationTypes.Count > 0)
                    configurationTypesCsv = configurationTypesCsv.Substring(0, configurationTypesCsv.Length - 1);

                return configurationTypesCsv;
            }
            catch (Exception exp)
            {

                throw;
            }
            finally
            {

            }
        }

        public bool IscontrolVisible(string keyname)
        {
            try
            {
                bool isVisible = false;
                CustomLogs.LogMessage("returnConfigValue1Negative started", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, keyname, SanityConfiguration.DbConfigurationFile);
                if (string.IsNullOrEmpty(config.ConfigurationValue1))
                {
                    config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HideJoinFunctionality, keyname, SanityConfiguration.DbConfigurationFile);
                }
                string configValue1 = config.ConfigurationValue1;

                isVisible = (configValue1.Equals("0"));
                return isVisible;
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
                return false;
            }
        }

        public IWebElement GetWebControl(Control c, FindBy query)
        {
            IWebElement ctrl = null;
            try
            {
                By by = null;
                switch (query)
                {
                    case FindBy.CSS_SELECTOR_CSS:
                        by = By.CssSelector(c.ClassName);
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        by = By.CssSelector(c.Id);
                        break;
                    case FindBy.XPATH_SELECTOR:
                        by = By.XPath(c.XPath);
                        break;
                }
                ctrl = ObjAutomationHelper.WebDriver.FindElement(by);
            }
            catch { }
            return ctrl;
        }

        public List<IWebElement> GetWebControls(Control c, FindBy query)
        {
            List<IWebElement> ctrls = new List<IWebElement>();
            try
            {
                By by = null;
                switch (query)
                {
                    case FindBy.CSS_SELECTOR_CSS:
                        by = By.CssSelector(c.ClassName);
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        by = By.CssSelector(c.Id);
                        break;
                    case FindBy.XPATH_SELECTOR:
                        by = By.XPath(c.XPath);
                        break;
                }
                ctrls = ObjAutomationHelper.WebDriver.FindElements(by).ToList();
            }
            catch { }
            return ctrls;
        }

        public bool IsStampEnabled(string key)
        {
            bool isEnabled = false;
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from DB configuration ", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.HomePageStamps, key, SanityConfiguration.DbConfigurationFile);
                isEnabled = config.IsDeleted.Equals("N");
                return isEnabled;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isEnabled;
            }
        }

        public void ValidateHtml(string htmlFile, string controlKey)
        {
            try
            {
                if (File.Exists(htmlFile))
                {
                    string expectedContent = Encoding.UTF8.GetString(File.ReadAllBytes(htmlFile));
                    expectedContent = WebUtility.HtmlDecode(expectedContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("</p>", "").Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                    expectedContent = Regex.Replace(expectedContent, @"\s+", "");

                    string errorMessage = string.Empty;
                    Driver = ObjAutomationHelper.WebDriver;
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    string actualContent = string.Empty;
                    if ((IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath))))
                    {
                        IWebElement element = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                        actualContent = Regex.Replace(actualContent, @"\s+", "");

                        if (expectedContent.Contains(actualContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else if (actualContent.Contains(expectedContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");
                    }
                    else if ((IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id))))
                    {
                        IWebElement element = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                        actualContent = Regex.Replace(actualContent, @"\s+", "");

                        if (expectedContent.Contains(actualContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else if (actualContent.Contains(expectedContent))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");
                    }
                    else
                    {
                        Assert.Inconclusive("Not Enabled - Validation of HTML File : " + htmlFile);
                    }
                }
                else
                {
                    Assert.Fail("File not found : " + htmlFile);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ValidateYear(string htmlFile, string controlKey)
        {
            try
            {
                if (File.Exists(htmlFile))
                {
                    string expectedContent = Encoding.UTF8.GetString(File.ReadAllBytes(htmlFile));
                    expectedContent = WebUtility.HtmlDecode(expectedContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("</p>", "").Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                    expectedContent = Regex.Replace(expectedContent, @"\s+", "");

                    string errorMessage = string.Empty;
                    Driver = ObjAutomationHelper.WebDriver;
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                    string actualContent = string.Empty;
                    if ((IsElementPresentOnPage(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath))))
                    {
                        IWebElement element = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(controlKey).XPath));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                        actualContent = Regex.Replace(actualContent, @"\s+", "");

                        if (expectedContent.Contains(DateTime.Now.Year.ToString()) && actualContent.Contains(DateTime.Now.Year.ToString()))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");
                    }
                    else if ((IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id))))
                    {
                        IWebElement element = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlKey).Id));
                        actualContent = (String)jse.ExecuteScript("return      arguments[0].innerHTML;", element);
                        actualContent = WebUtility.HtmlDecode(actualContent).Replace('"', '\'').Replace("\r", "").Replace("\n", "").Replace("<br>", "<br />").Replace("</p>", string.Empty).Replace("</u>", string.Empty).Replace("<p>", string.Empty).Replace("<u>", string.Empty).Trim();
                        actualContent = Regex.Replace(actualContent, @"\s+", "");

                        if (expectedContent.Contains(DateTime.Now.Year.ToString()) && actualContent.Contains(DateTime.Now.Year.ToString()))
                        {
                            CustomLogs.LogMessage("Validation of " + htmlFile + " completed", TraceEventType.Stop);
                        }
                        else
                            Assert.Fail("Validation of HTML File : " + htmlFile + " -Failed");
                    }
                    else
                    {
                        Assert.Inconclusive("Not Enabled - Validation of HTML File : " + htmlFile);
                    }
                }
                else
                {
                    Assert.Fail("File not found : " + htmlFile);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ScrollToBottom()
        {
            Control cFooter = ObjAutomationHelper.GetControl(ControlKeys.HTML_FOOTERLINKS);
            IWebElement footer = GetWebControl(cFooter, FindBy.CSS_SELECTOR_ID);
            IWebElement link = (footer != null) ? footer.FindElements(By.TagName("a")).ToList().FirstOrDefault() : null;
            if (link != null)
            {
                link.SendKeys(Keys.PageDown);
            }
        }

        public void ScrollToElement(IWebElement we)
        {
            Driver = ObjAutomationHelper.WebDriver;
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
            jse.ExecuteScript("arguments[0].scrollIntoView()", we);
        }

        public bool IsErrorPageDispalyed(string expectedTitle)
        {
            bool chk = false;
            string errorContent = File.ReadAllText(Path.Combine(SanityConfiguration.HtmlDataDirectory, SanityConfiguration.HTMLFiles.ERROR_HTML));
            chk = ObjAutomationHelper.WebDriver.PageSource.Contains(errorContent);
            if (!chk)
            {
                // check if custom error is off (yellow page)
             //   chk = !expectedTitle.ToUpper().Equals(ObjAutomationHelper.WebDriver.Title.ToUpper());
                IWebElement lblMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_CONFIRMWELMSG).Id));
                chk = !expectedTitle.Equals(lblMsg.Text);
            }
            return chk;
        }


        public bool IsUnSpendBoostExists()
        {
            bool chk=false;
            List<ConfugurationTypeEnum> configurationTypes = new List<ConfugurationTypeEnum>(){
                             ConfugurationTypeEnum.Holding_dates                            
                            };
                CustomerServiceAdaptor objCustomerService = new CustomerServiceAdaptor();
                DataSet ds = objCustomerService.GetConfigurationItems(GetConfigurationTypesCsv(configurationTypes), "en-GB");
                        
                if (IsExchangeEnabled(ds))
                {
                    RewardServiceAdaptor service = new RewardServiceAdaptor();
                    List<Reward> BoostTokenValue = new List<Reward>();
                    BoostTokenValue = service.GetRewardDetails(Login.CustomerID, CountrySetting.culture);
                    chk =BoostTokenValue.Count > 0 ? true : false;
                    
                }

                return chk;
               
        }
        public bool IsExchangeEnabled(DataSet ds)
        {
            bool chk = false;
            DateTime? exchangeStartDate =null;
            DateTime? exchangeEndDate = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ConfigurationName"].ToString().Equals("YourExchangesFlag"))
                    {
                        string strExchnageflag = dr["ConfigurationValue1"].ToString();
                        chk = strExchnageflag != "1" ?true :false;
                        
                    }
                    if (dr["ConfigurationName"].ToString().Equals("YourExchangesDates"))
                    {
                         exchangeStartDate  = Convert.ToDateTime(dr["ConfigurationValue1"].ToString());
                         exchangeEndDate = Convert.ToDateTime(dr["ConfigurationValue2"].ToString());
                         chk = (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) >= exchangeStartDate)
               && (Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()) <= exchangeEndDate);
                        
                    }
                }
            }
            return chk;
           
        }


        public string GetRedirectedUrl(string Decodedurl)
        {
            string requestedPage = string.Empty;
            string requestedAction = string.Empty;
           // Dictionary<string, object> qs = null;
            if (Decodedurl.Contains("from"))
            {
                string strQs = Decodedurl.Substring(Decodedurl.IndexOf("=") + 1);
                //qs = GetQueryString(strQs);
                Decodedurl = strQs.Replace(string.Format("={0}", strQs), string.Empty);
            }
                string[] parts = Decodedurl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    requestedAction = parts[parts.Length - 1];
                    requestedPage = parts[parts.Length - 2];
                }
            
            string Url = requestedPage + "/" + requestedAction;
            return Url;
        }

        Dictionary<string, object> GetQueryString(string qs)
        {


            Dictionary<string, object> lstQs = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(qs))
            {
                lstQs = (from t in qs.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries)
                         where t.Length > 2 & t.Contains('=') & !string.IsNullOrEmpty(t)
                         select t).ToDictionary(e => e.Split('=')[0], e => (object)e.Split('=')[1]);

            }
            return lstQs;
        }

        public string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        public string ValidatePreferenceText(SecurityPreference contactPreference, string controlId, string LocalResourceFileName, string resId, string resourceID)
        {
            bool isVisible = false;
            string error = string.Empty;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            foreach (var p in prefUI)
            {
                if (p.preferenceid == (int)contactPreference)
                {
                    isVisible = p.isvisible;
                }
            }
            if (isVisible)
            {

                string actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlId).Id)).Text;
                string expectedText = string.Empty;
                string msg = AutomationHelper.GetResourceMessage(resId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                string storeMsg = AutomationHelper.GetResourceMessage(resourceID, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                expectedText = (msg + " " + storeMsg).Trim();
                error = ValidateResourceValueWithHTMLContent(actualText, expectedText);
                if (!string.IsNullOrEmpty(error))
                {
                    Assert.Fail(error);
                }
                
            }
         
            return error;
        }

        public void selectOptionFromDropDown(string key, Enums.DropDownValue field)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var mySelectElm = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                var mySelect = new SelectElement(mySelectElm);
                switch (field)
                {
                    case Enums.DropDownValue.AnyOption:
                        mySelect.SelectByIndex(3);
                        break;
                    case Enums.DropDownValue.SelectOption:
                        mySelect.SelectByIndex(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public string ValidateResourceValueWithHTMLContent(string actualText, string expectedText)
        {
            string error = string.Empty;
            Regex htmls = new Regex("<[^>]*>");
            List<string> Expectedmatches = htmls.Split(expectedText).ToList();
            List<string> ExpectedlstMatches = new List<string>();
            Expectedmatches.ForEach(m => ExpectedlstMatches.Add(m.Trim()));
            var expectedMsg = string.Join("", ExpectedlstMatches);

            List<string> expectedLst = new List<string>();
            List<string> exp = expectedMsg.Split(' ').ToList();
            expectedLst.ForEach(m => exp.Add(m.Trim()));
            expectedMsg = string.Join("", exp);

            List<string> Actualmatches = htmls.Split(actualText).ToList();
            List<string> ActuallstMatches = new List<string>();
            Actualmatches.ForEach(m => ActuallstMatches.Add(m.Trim()));
            var actualMsg = string.Join("", ActuallstMatches);

            List<string> actualLst = new List<string>();
            List<string> act = expectedMsg.Split(' ').ToList();
            actualLst.ForEach(m => act.Add(m.Trim()));
            actualMsg = string.Join("", act);

            if (!expectedMsg.Equals(actualMsg))
            {
                error = string.Format("expected: {0} , actual: {1} ", expectedMsg, actualMsg);
                CustomLogs.LogInformation(error);
            }
            return error;
        }

        public string SimplifyResourceText(string Text)
        {
            Regex htmls = new Regex("<[^>]*>");
           List<string> Expectedmatches = htmls.Split(Text).ToList();
           List<string> ExpectedlstMatches = new List<string>();
            Expectedmatches.ForEach(m => ExpectedlstMatches.Add(m.Trim()));
           var expectedMsg = string.Join("", ExpectedlstMatches);
           List<string> expectedLst = new List<string>();
           List<string> exp = expectedMsg.Split(' ').ToList();
           expectedLst.ForEach(m => exp.Add(m.Trim()));
           expectedMsg = string.Join("", exp);
           return expectedMsg;
        }

     }

}
