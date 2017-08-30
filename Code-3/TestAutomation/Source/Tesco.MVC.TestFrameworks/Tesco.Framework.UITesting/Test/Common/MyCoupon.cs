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
using System.IO;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using Tesco.Framework.UITesting.Constants;
using System.Globalization;

namespace Tesco.Framework.UITesting.Test.Common
{
    class MyCoupon : Base
    {

        #region PROPERTIES
        string isPresent = string.Empty;
        #endregion
        #region Constructor

        Generic objGeneric = null;
        public MyCoupon(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(objhelper);
        }
       
        #endregion

        

        #region method
        public void NoActiveCouponPresent(string msgId, String pageName, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                var expectedMessage = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedMessage, "expected message"));
                var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_NOACTIVECOUPON).Id)).Text;
                Assert.AreEqual(expectedMessage, actualMessage, pageName + "Active coupons are present");
                //Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_THANKYOUMSG).ClassName)).Text
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

        public void NoRedeemedCouponsIn4Weeks(string msgId, String pageName, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                //  Fetch Details From resource.XML
                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                var expectedMessage = res.Value;
                Debug.WriteLine(string.Format("{0} - {1}", expectedMessage, "expected message"));
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_NOCOUPONREDEEMED).XPath)).Text;
                Assert.AreEqual(expectedMessage, actualMessage, pageName + "Active coupons are present");

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

        public void DateRedeemed(string clubcard)
        {
            try
            {
                //string dateformat = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.WebConfigurationFile).Value;
                
                Driver = ObjAutomationHelper.WebDriver;
                List<string> dates = new List<string>();
                List<string> expectedDates = new List<string>();
                ReadOnlyCollection<IWebElement> actualDate = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_DATE).XPath)));
                for (int i = 0; i < actualDate.Count; i++)
                {
                    dates.Add(Convert.ToDateTime(actualDate[i].Text.Substring(0, 10)).ToShortDateString());
                }

                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                expectedDates = service.GetUsedCouponUsedDates(Login.CustomerID.ToString(), CountrySetting.culture);

                CollectionAssert.AreEqual(expectedDates, dates);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }

        public void DateFormat(string clubcard)
        {
            try
            {
                // date format has been moved to dbconfig.xml 
              //  string dateformat = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.WebConfigurationFile).Value;
                string dateformat = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.DISPLAYDATEFORMAT, SanityConfiguration.DbConfigurationFile).ConfigurationValue1;
                int formatlenght = dateformat.Length;
                Driver = ObjAutomationHelper.WebDriver;
                List<string> expectedDates = new List<string>();

                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                List<bool> res = new List<bool>();
                expectedDates = service.GetUsedCouponUsedDates(Login.CustomerID.ToString(), CountrySetting.culture);

                if (expectedDates.ToString() == "0")
                {
                    CustomLogs.LogMessage("No coupons redeemed", TraceEventType.Start);
                }
                else
                {
                    ReadOnlyCollection<IWebElement> actualDate = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_DATE).XPath)));
                    int iCheck = 0;
                    for (int i = 0; i < actualDate.Count; i++)
                    {
                        DateTime date = DateTime.Now;
                        string UIDate = actualDate[i].Text.Substring(0, formatlenght);

                        if (DateTime.TryParseExact(UIDate, dateformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            iCheck++;
                        }
                        else
                        {
                            CustomLogs.LogMessage("Date Format is not same", TraceEventType.Start);
                        }

                    }
                    Assert.AreEqual(iCheck, actualDate.Count);
                }
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);
        }
        public void AvailableCoupon(string clubcard)
        {
            try
            {
                //string dateformat = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.WebConfigurationFile).Value;
                Driver = ObjAutomationHelper.WebDriver;
                List<string> dates = new List<string>();
                List<string> expectedDates = new List<string>();
                int CouponCount = 0;

                Int32 couponC = 0;
                ReadOnlyCollection<IWebElement> Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath));

                // bool x = Coupon[0].Text.Equals("8"); 
                // var Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath)).;
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                CouponCount = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out couponC);
                //  bool x = Coupon[0].Text.Equals(CouponCount.ToString()); 


                if (Coupon[0].Text.Equals(CouponCount.ToString()))
                {
                    string msg = string.Format("Available coupon count is", CouponCount, clubcard);
                    CustomLogs.LogInformation(msg);
                }

                else
                {
                    Assert.Fail(CouponCount + "count is incorrect");
                }

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }

        public void UsedCoupon(string clubcard)
        {
            try
            {
                //string dateformat = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.WebConfigurationFile).Value;
                Driver = ObjAutomationHelper.WebDriver;
                List<string> dates = new List<string>();
                List<string> expectedDates = new List<string>();
                int CouponCount = 0;

                // Int32 couponC = 0;
                ReadOnlyCollection<IWebElement> Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_USEDSUMMARY).XPath));

                // bool x = Coupon[0].Text.Equals("8"); 
                // var Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath)).;
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                CouponCount = service.GetUsedCouponCount(Login.CustomerID.ToString(), CountrySetting.culture);
                //  bool x = Coupon[0].Text.Equals(CouponCount.ToString()); 


                if (Coupon[0].Text.Equals(CouponCount.ToString()))
                {
                    string msg = string.Format("Used coupon count in Summary is", CouponCount, clubcard);
                    CustomLogs.LogInformation(msg);
                }

                else
                {
                    Assert.Fail(CouponCount + "count is incorrect");
                }

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }

        public void TotalCoupon(string clubcard)
        {
            try
            {
                //string dateformat = AutomationHelper.GetWebConfiguration(WebConfigKeys.DISPLAY_DATE_FORMAT, SanityConfiguration.WebConfigurationFile).Value;
                Driver = ObjAutomationHelper.WebDriver;
                List<string> dates = new List<string>();
                List<string> expectedDates = new List<string>();
                int CouponCount = 0;

                Int32 couponC = 0;
                ReadOnlyCollection<IWebElement> Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_SENTSUMMARY).XPath));

                // bool x = Coupon[0].Text.Equals("8"); 
                // var Coupon = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath)).;
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                CouponCount = service.GetTotalCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out couponC);
                //  bool x = Coupon[0].Text.Equals(CouponCount.ToString()); 


                if (Coupon[0].Text.Equals(CouponCount.ToString()))
                {
                    string msg = string.Format("Sent coupon count in Summary is", CouponCount, clubcard);
                    CustomLogs.LogInformation(msg);
                }

                else
                {
                    Assert.Fail(CouponCount + "count is incorrect");
                }

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }


        public void ValidateTextCoupon(string msgId, string keys, String pageName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying validation message for page " + pageName + " started", TraceEventType.Start);
                //Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName));
                //var expectedMessage = res.Value;
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath)).Text.Trim();
                //   Assert.AreEqual(actualMessage.Trim(), msgId, pageName + " not verified");
                if (actualMessage.Contains(msgId))
                {
                    CustomLogs.LogMessage("Validation of coupon text " + pageName + " completed", TraceEventType.Stop);
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


        public void StoreName(string clubcard)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> StoreName = new List<string>();
                List<string> expectedStoreName = new List<string>();
                ReadOnlyCollection<IWebElement> ActualStoreName = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_STORENAME).XPath)));
                for (int i = 0; i < ActualStoreName.Count; i++)
                {
                    StoreName.Add(ActualStoreName[i].Text);
                }
                int usedccount = ActualStoreName.Count;
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                expectedStoreName = service.GetUsedCouponUsedStoreName(Login.CustomerID.ToString(), CountrySetting.culture);
                int usedcount = service.GetUsedCouponCount(Login.CustomerID.ToString(), CountrySetting.culture);
                CollectionAssert.AreEqual(expectedStoreName, StoreName);

            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }

        //USED AND UNUSED COUPON COUNT
        public void couponCount(string clubcard)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                List<string> usedcoupon = new List<string>();
                int availableCount = 0;
                Int32 couponC = 0;
                List<string> expectedStoreName = new List<string>();
                ReadOnlyCollection<IWebElement> actualCount = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_STORENAME).XPath)));
                var usedcouponcount = actualCount.Count;
                ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
                int usedcount = service.GetUsedCouponCount(Login.CustomerID.ToString(), CountrySetting.culture);
                availableCount = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out couponC);
                string count = availableCount.ToString();
                var actualAvailableCount = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath)).Text;
                //var actualAvailableCount = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_AVAILABLECOUNT).XPath)).Text;
                //int availablecouponCount = actualAvailableCount.Count;
                Assert.AreEqual(actualAvailableCount, count);
                Assert.AreEqual(usedcouponcount, usedcount);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        }


        public void SelectCouponMessage(string msgId, String pageName, string resourceFileName)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));

                //  Fetch Details From resource.XML
                Resource res1 = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                var expectedMessage = res1.Value;
                var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_SelectCouponMessage).XPath)).Text;
                Assert.AreEqual(expectedMessage, actualMessage, pageName + "Select coupon message is not present is not present");
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
        # region Online
        //public void CouponsRedeemedOnline()
        //{
        //    try
        //    {
        //        Driver = ObjAutomationHelper.WebDriver;
        //        //CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
        //        //Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
        //        //Fetch Details From resource.XML
        //        //Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
        //        //var expectedMessage = res.Value;
        //        //Debug.WriteLine(string.Format("{0} - {1}", expectedMessage, "expected message"));

        //        ReadOnlyCollection<IWebElement> CouponsRedeemedOnline = (Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_STORENAME).XPath)));
        //        for (int i = 0; i < CouponsRedeemedOnline.Count; i++)
        //        {
        //            var OnlineRedeemedCoupons = CouponsRedeemedOnline[i].Text;
        //        }
        //        //if (OnlineRedeemedCoupons == "%Online%") ;
        //        //var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_NOCOUPONREDEEMED).XPath)).Text;
        //        //Assert.AreEqual(expectedMessage, actualMessage, pageName + "Active coupons are present");

        //    }
        //    catch (Exception ex)
        //    {
        //        ScreenShotDetails.TakeScreenShot(Driver, ex);
        //        CustomLogs.LogException(ex);
        //        Driver.Quit();
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //    }
        //    // CustomLogs.LogMessage("Verifying the pagename for the page " + pageName + " Completed", TraceEventType.Stop);

        //}
        #endregion

        public void printbutton()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("Verifying the page name for the page " + " started", TraceEventType.Start);
            IWebElement PrintButtonCoupon = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_PrintButton).Id));

        }

        #region Text Validation
        public void TextValidation(string pageName)
        {
            string errorMessage = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                DBConfiguration config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ONLINESECTION, SanityConfiguration.DbConfigurationFile);
                string isOnlineSectionEnabled = config.ConfigurationValue1;
                string resxFile = Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE);
                errorMessage = objGeneric.VerifyTextWithHtml_Contains(ValidationKey.COUPONDESCRIPTION, ControlKeys.MYCOUPON_TEXTDESCRIPTION, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.SUMMARYHEADING,ControlKeys.MYCOUPON_TEXTCOUPONSUMMARY ,resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.INSTOREHEADING ,ControlKeys.MYCOUPON_TEXTINSTOREHEADING ,resxFile, pageName);
                if (isOnlineSectionEnabled.Equals("TRUE"))
                {
                    errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.ONLINEHEADING, ControlKeys.MYCOUPON_TEXTONLINEHEADING, resxFile, pageName);
                }
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.HOWTOUSEHEADING  ,ControlKeys.MYCOUPON_TEXTHOWTOUSEHEADING ,resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyTextWithHtml_Contains(ValidationKey.INSTOREHTML, ControlKeys.MYCOUPON_TEXTINSTORE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyTextWithHtml_Contains(ValidationKey.ONLINEHTML, ControlKeys.MYCOUPON_TEXTONLINE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.ACTIVEHEADING  ,ControlKeys.MYCOUPON_TEXTACTIVEHEADING ,resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.ACTIVESUBHEADING, ControlKeys.MYCOUPON_TEXTACTIVESUBHEADING, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.FOOTERLABEL ,ControlKeys.MYCOUPON_TEXTFOOTERLABLE ,resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.USEDCOUPONSECTIONHEADING  ,ControlKeys.MYCOUPON_TEXTUSEDCOUPONSECTIONHEADING ,resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.NOREDEEMPTIONMESSAGE, ControlKeys.MYCOUPON_TEXTNOREDEEMPTIONMESSAGE, resxFile, pageName);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail(errorMessage);
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

        public void ValidateCouponHeaderLabel()
        {
            string errorMessage = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var expectedHeaderText = AutomationHelper.GetResourceMessage(LabelKey.LBLCOUPONHEADER, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.COUPON_RESOURCE)).Value;
                IWebElement actualHeader = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_LBLCOUPONHEADER).ClassName));
                Assert.AreEqual(expectedHeaderText, actualHeader.Text, "Select coupon message is not present is not present");
                objGeneric.ClickElement(ControlKeys.MYCOUPON_CHKCOUPONHEADER, FindBy.XPATH_SELECTOR);
                IWebElement chkCouponCheckBox = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYCOUPON_SelectCoupon).Id));
                if (chkCouponCheckBox.Selected.Equals(true))
                {
                    CustomLogs.LogInformation("Checkbox selected on click of CouponHeader");
                }
                else
                {
                    Assert.Fail("On click of CouponHeader, checkbox is not selected");
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
        #endregion

        #endregion

        #region Method V1

        public int GetAvailableCouponCount()
        {
            int availableCoupons = 0;
            ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
            int totalCoupons = 0;
            availableCoupons = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out totalCoupons);
            return availableCoupons;
        }

        #endregion

    }
}
