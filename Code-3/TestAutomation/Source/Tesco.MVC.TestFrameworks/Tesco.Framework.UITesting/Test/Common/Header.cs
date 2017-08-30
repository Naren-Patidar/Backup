using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.UITesting.Entities;
using System.IO;
using System.Diagnostics;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using System.Web;
using System.Net;
using Tesco.Framework.UITesting.Constants;
using System.Collections.ObjectModel;

namespace Tesco.Framework.UITesting.Test.Common
{
    /// <summary>
    /// Summary description for Header
    /// </summary>
    [TestClass]
    public class Header :Base 
    {
        Generic objGeneric = null;
        public Header()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Header(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric=new Generic(ObjAutomationHelper);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }

        public void ValidateUtilityBar()
        {
            try{
            Driver = ObjAutomationHelper.WebDriver;
            IWebElement expectedTescoSite = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_TESCOCOM).Id));
            IWebElement expectedSignOut = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_SIGNOUTKEY).Id));
            IWebElement expectedStoreLocator = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_STORELOCATOR).XPath));
            IWebElement expectedWebsiteFeedback = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_WEBSITEFEEDBACK).Id));

             var actualTescoSite= AutomationHelper.GetResourceMessage(ValidationKey.HEADER_TESCOCOM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
             var actualSignout = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_SIGNOUTKEY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
             var actualStoreLocator = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_STORELOCATOR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
             var actualHrefStoreLocator = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HREFSTORELOCATOR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
             var actualFeedBack = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_FEEDBACK, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;

             Assert.AreEqual(expectedTescoSite.Text, actualTescoSite);
             Assert.AreEqual(expectedSignOut.Text, actualSignout);
             Assert.AreEqual(expectedStoreLocator.Text, actualStoreLocator);
             Assert.AreEqual(expectedStoreLocator.GetAttribute("href"), actualHrefStoreLocator);
             Assert.AreEqual(expectedWebsiteFeedback.Text, actualFeedBack);
            }
            catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void ValidateSearchBar()
        {
            try
            {
            Driver = ObjAutomationHelper.WebDriver;
            bool isSearchTextBoxPresent=Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_SEARCHTEXTBOX).Id), Driver);
            bool isSearchButtonPresent = Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_SEARCHBUTTON).Id), Driver);
            bool isBasketPresent = Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_BASKET).Id), Driver);
            if (!(isSearchTextBoxPresent && isSearchButtonPresent && isBasketPresent))
            {
                Assert.Fail("Basket link not verified");
            }
            IWebElement expectedBasketLink = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HEADER_BASKET).Id));
            var actualBasketLink = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HREFBASKET, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            Assert.AreEqual(expectedBasketLink.GetAttribute("href"), actualBasketLink, "Basket Link not verified");
            }
            catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            
        }
        public void ValidatePrimaryNav()
        {
            try
            {
            Driver = ObjAutomationHelper.WebDriver;
            //Primary Navigations
            List<string> expectedPrimaryNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_PRIMARYNAV).XPath)).Select(iw => iw.Text));
            List<string> actualPrimaryNav = new List<string>();
            var actualAboutClubcard = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_ABOUTCLUBCARD, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var actualCollectPoints = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_COLLECTPOINTS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var actualSpendVouchers = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_SPENDVOUCHERS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var actualMCA = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_MCA, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualPrimaryNav.InsertRange(actualPrimaryNav.Count, new List<string> { actualAboutClubcard, actualCollectPoints, actualSpendVouchers, actualMCA });
            CollectionAssert.AreEqual(expectedPrimaryNav, actualPrimaryNav);

            //Spend Voucher Child 
            List<string> expectedSpendVoucherNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_SPENDVOUCHERNAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualSpendVoucherNav = new List<string>();
            var howToSpend = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HOWTOSPEND, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var boost = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_CBOOST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var fun = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_FUN, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var eatingOut = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_EATINGOUT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var travel = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_TRAVEL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var home = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HOME, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var lifestyle = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_LIFESTYLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var value = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VALUE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualSpendVoucherNav.InsertRange(actualSpendVoucherNav.Count, new List<string> { howToSpend, boost, fun, eatingOut, travel, home, lifestyle, value });
            CollectionAssert.AreEqual(expectedSpendVoucherNav, actualSpendVoucherNav);
            }
            catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void validateFunNavigation()
        {
            //Fun Navigation
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
            List<string> expectedFunNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_FUNNAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualFunNav = new List<string>();
            var findADay = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_FINDADAY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var daysOut = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_DAYSOUT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var exp = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_EXP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var entertainmnet = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_ENTERTAINMENT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualFunNav.InsertRange(actualFunNav.Count, new List<string> { findADay, daysOut, exp, entertainmnet, viewAll});
            CollectionAssert.AreEqual(expectedFunNav, actualFunNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void validateEatingOutNavigation()
        {
            //Eating Out Navigation
            try
            {
            List<string> expectedEatingOutNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_EATINGOUTNAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualEatingOutNav = new List<string>();
            var findRest = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_FINDREST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var pub = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_PUB, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var rest = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_REST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualEatingOutNav.InsertRange(actualEatingOutNav.Count, new List<string> { findRest, pub, rest, viewAll});
            CollectionAssert.AreEqual(expectedEatingOutNav, actualEatingOutNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void validateTravelNavigation()
        {
            //Travel Navigation
            try
            {
            List<string> expectedTravelNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_TRAVELNAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualTravelNav = new List<string>();
            var airline = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_AIRLINE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var holiday = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HOLIDAY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var hotel = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HOTEL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var transport = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_TRANSPORT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualTravelNav.InsertRange(actualTravelNav.Count, new List<string> { airline, holiday, hotel,transport, viewAll});
            CollectionAssert.AreEqual(expectedTravelNav, actualTravelNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void validateHomeNavigation()
        {
            //Home and Essential Navigation
            try
            {
            List<string> expectedhomeNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_HOMENAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualhomeNav = new List<string>();
            var garden = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_GARDEN, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var software = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_SOFTWARE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var motoring = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_MOTORING, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualhomeNav.InsertRange(actualhomeNav.Count, new List<string> { garden, software, motoring, viewAll});
            CollectionAssert.AreEqual(expectedhomeNav, actualhomeNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void validateLifestyleNavigation()
        {
            //Lifestyle Navigation
            try{
            List<string> expectedLifestyleNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_LIFESTYLENAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualLifestyleNav = new List<string>();
            var health = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_HEALTH, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var shopping = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_SHOPPING, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var magazine = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_MAGAZINE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var membership = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_MEMBERSHIP, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualLifestyleNav.InsertRange(actualLifestyleNav.Count, new List<string> { health, shopping, magazine,membership, viewAll});
            CollectionAssert.AreEqual(expectedLifestyleNav, actualLifestyleNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        public void validate4XValueNavigation()
        {
            //$x Value Navigation
            try
            {
            List<string> expectedValueNav = new List<string>(Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HEADER_4XNAV).XPath)).Select(iw => HttpUtility.HtmlDecode(iw.GetAttribute("innerHTML"))));
            List<string> actualValueNav = new List<string>();
            var valuefindADay = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VALUEFINDADAY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var valueAttraction = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VALUEATTRACTION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var valueRest = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VALUEREST, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            var viewAll = AutomationHelper.GetResourceMessage(ValidationKey.HEADER_VIEWALL, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HEADER_RESOURCE)).Value;
            actualValueNav.InsertRange(actualValueNav.Count, new List<string> { valuefindADay, valueAttraction, valueRest, viewAll});
            CollectionAssert.AreEqual(expectedValueNav, actualValueNav);
            }
             catch (Exception ex)
            {

                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        }
    
}
