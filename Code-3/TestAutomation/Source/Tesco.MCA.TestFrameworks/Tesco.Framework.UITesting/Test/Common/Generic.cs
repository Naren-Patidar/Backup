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

namespace Tesco.Framework.UITesting.Test.Common
{
    class Generic : Base
    {
        string isPresent = string.Empty;
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
                var expectedLinkName = res.Value.ToUpper();
                ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));
                var expectedLink = actualLinkName.ToList().Find(lnk => lnk.Text.ToUpper() == expectedLinkName);

                if (expectedLink != null)
                {
                    ((IWebElement)expectedLink).Click();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                    string msg = string.Format("Left Navigation link {0} under {1} is present", expectedLinkName, key);
                    CustomLogs.LogInformation(msg);
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
                isPresent = config.IsDeleted;
                return isPresent;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isPresent;
            }
        }

        public string verifyPageEnabledByWebConfig(string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for page enabled or not from Web configuration ", TraceEventType.Start);
                WebConfiguration webConfig = AutomationHelper.GetWebConfiguration(keys, SanityConfiguration.WebConfigurationFile);
                isPresent = webConfig.Value;
                return isPresent;
            }
            catch (Exception ex)
            {
                CustomLogs.LogError(ex);
                return isPresent;
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
                isPresent = config.IsDeleted;
                return isPresent;
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
        public void ElementEnabled(string keys, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Enabled on Element " + keys + " starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;

                bool keysenabled = true;
                
                if (keysenabled == Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Enabled)

                    CustomLogs.LogMessage(keys + " Element is Enabled", TraceEventType.Stop);
                else
                {
                    CustomLogs.LogMessage(keys + " Element is not Enabled", TraceEventType.Stop);
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

        public void ClickElementByXpath(string keys, String pageName)
        {
            try
            {
                //Driver = ObjAutomationHelper.WebDriver;
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, " Starting"));
                Driver = ObjAutomationHelper.WebDriver;
                ObjAutomationHelper.GetControl(keys).WaitForControlByxPath(Driver);
                Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Click();
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
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id)).Text;
                Assert.AreEqual(expectedMessage,actualMessage.TrimStart(), pageName + " not verified");
                CustomLogs.LogInformation(actualMessage+ " Verified");
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
                Assert.AreEqual(expectedMessage ,actualMessage, pageName + " not verified");
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

        public void VerifyTextonthePageByXpath_Contains(string msgId, string keys, String pageName, string LocalResourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                var expectedMessage = res.Value;
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Text;
                if (actualMessage.Contains(expectedMessage)) 
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

                string actualURL=Driver.Url;
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
                alert=Driver.SwitchTo().Alert();
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
                    ((IWebElement)expectedLink).Click();
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
                Resource res = AutomationHelper.GetResourceMessage(stampKey[i], Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.GLOBAL_RESOURCE));
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
        
        //  public void stampClick(string key, String pageName, string expectedLinkName)
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
        //  public void StampsTextValidation(string key, string msgId, string expectedLinkName)
        //  {

             
        //      try
        //      {
        //          Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
        //          Driver = ObjAutomationHelper.WebDriver;
        //          ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));

        //          // var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(key).XPath)).Text;
        //          var ActualLink = actualLinkName.ToList().Find(lnk => lnk.GetAttribute("href").Contains(expectedLinkName));

        //        if (ActualLink.Text.Contains(expectedtext))
        //        {
        //            string msg = string.Format("Stamp text  {0} under {1} is present", expectedLinkName, key);
        //            CustomLogs.LogInformation(msg);
        //            ((IWebElement)ActualLink).Click();


        //        }
        //        else
        //        {
        //            string msg = string.Format("Stamp text {0} under {1} is not present", expectedLinkName, key);
        //            CustomLogs.LogInformation(msg);
        //            Assert.Fail(msg);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        CustomLogs.LogException(ex);
        //        ScreenShotDetails.TakeScreenShot(Driver, ex);
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //        Driver.Quit();
        //    }
        //    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        //}
                       
        public void linkNavigateNew(string msgId, string node, string key, String pageName)
          {
              try
              {
                  CustomLogs.LogMessage("Navigating to the link on page " + pageName + " started", TraceEventType.Start);
                  Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                  Driver = ObjAutomationHelper.WebDriver;
                  Resource res = AutomationHelper.GetResourceMessageNew(msgId, node, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                  var expectedLinkName = res.Value;

                  //ObjAutomationHelper.GetControl(key).WaitForControlByxPath(Driver);
                  ReadOnlyCollection<IWebElement> actualLinkName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(key).XPath)));

                  var expectedLink = actualLinkName.ToList().Find(lnk => lnk.Text == expectedLinkName);

                  if (expectedLink != null)
                  {
                      ((IWebElement)expectedLink).Click();
                      Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
                      string msg = string.Format("Left Navigation link {0} under {1} is present", expectedLinkName, key);
                      CustomLogs.LogInformation(msg);
                      //ele.WaitForTitle(Driver, expectedLink.ToString());
                      // ele.WaitForTitle(Driver, pageName);
                  }

                  else
                  {
                      string msg = string.Format("Left Navigation link {0} under {1} is not present", expectedLinkName, key);
                      CustomLogs.LogInformation(msg);
                  }               
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("Navigated to the page " + pageName + " completed", TraceEventType.Stop);
            Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Ending"));
        }

        public void VerifyTextExistOnPage(string keys)
        {
            try
            {
                CustomLogs.LogMessage("VerifyTextExistOnPage started", TraceEventType.Start);
                var textExist = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Text;
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

        public string VerifyAppSettings(string keys)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
                CustomLogs.LogMessage("Checking configurations for AppSettings from DB configuration ", TraceEventType.Start);
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.AppSettings, keys, SanityConfiguration.DbConfigurationFile);
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
            try
            {
                CustomLogs.LogMessage("EnterDataInField started", TraceEventType.Start);
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id)).Clear();
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id)).SendKeys(value);
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

        public bool returnConfigValue1Negative(ConfugurationTypeEnum configtype, string keyname)
        {

            try
            {
                CustomLogs.LogMessage("returnConfigValue1Negative started", TraceEventType.Start);
                string configValue1 = AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                if (configValue1.Equals("0")||configValue1.Equals("FALSE"))
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
                if (configValue1.Equals("1")||configValue1.Equals("TRUE"))
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
        public int returnConfigLength(ConfugurationTypeEnum configtype, string keyname, Enums.FieldType type)
        {
            
            try
            {
                CustomLogs.LogMessage("returnConfigLength started", TraceEventType.Start);
                int length=0;
                int configValue1 = Int32.Parse(AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1);
                int configValue2 = Int32.Parse(AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue2);
                switch (type)
                {
                    case Enums.FieldType.Valid:
                        length = configValue2 - configValue1;
                        break;
                    case Enums.FieldType.InvalidLength1:
                        length =  configValue1-1;
                        break;
                    case Enums.FieldType.InvalidLength2:
                        length = configValue2+1;
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
        public bool VerifyRegex(ConfugurationTypeEnum configtype, string keyname, string value)
        {
            
            try
            {
                CustomLogs.LogMessage("VerifyRegex started", TraceEventType.Start);           
                string configValue1 = AutomationHelper.GetDBConfiguration(configtype, keyname, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                return Regex.IsMatch(value, configValue1);

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

        public bool VerifyDataOnPage_Contains(string DataToVerify, string keys)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Inside method VerifyDataOnPage_Contains,returns true or false ", TraceEventType.Start);

                var expectedMessage = DataToVerify;
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

        public string GetDataFromPage( string keys)
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

       }
}
