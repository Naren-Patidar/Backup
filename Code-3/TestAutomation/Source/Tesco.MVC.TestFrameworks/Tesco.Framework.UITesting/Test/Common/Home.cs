using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Entities;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Tesco.Framework.UITesting.Services;
using Tesco.Framework.UITesting.Enums;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class Home : Base
    {
        Generic objGeneric;

        #region Constructor


        public Home(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            this.objGeneric = new Generic(objhelper);
        }

        #endregion

        /// <summary>
        /// To verify the home page 
        /// </summary>
        /// <returns></returns>
        public bool Homepage_Verification()
        {
            try
            {
                CustomLogs.LogMessage("Verifying if Home Page is Reached or not started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                String actualPageTitle = null;
                //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Home);
                Resource res = AutomationHelper.GetResourceMessage(LabelKey.HOMETITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.LOCAL_RESOURCE));
                string expectedPageTitle = res.Value;
                IWebElement ele =null;

                if (ele.WaitForTitle(Driver, "Home"))
                {
                    var elTitle = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.HOME_TITLE).XPath)).Text;
                    actualPageTitle = elTitle.Split(' ')[0].ToString();
                    if (actualPageTitle == expectedPageTitle)
                        CustomLogs.LogInformation("Title same for the home page");
                    else
                    {
                        CustomLogs.LogInformation("Title not same for the home page");
                        Assert.Fail("Title not same for the home page");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Driver.Quit();
            }
            CustomLogs.LogMessage("Home Page is Reached verified", TraceEventType.Stop);
            return true;
        }

        public void VerifyMyMessageSection(string controlkey, string labelkey)
        {
            CustomLogs.LogMessage("VerifyMyMessageSection Started", TraceEventType.Start);
            Driver = ObjAutomationHelper.WebDriver;
            Resource res = AutomationHelper.GetResourceMessage(labelkey, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.HOME_RESOURCE));
            var expectedText = res.Value;
            var actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(controlkey).Id)).Text;
            Regex htmls = new Regex("<[^>]*>");
            List<string> matches = htmls.Split(expectedText).ToList();
            List<string> lstMatches = new List<string>();
            matches.ForEach(m => lstMatches.Add(m));
            expectedText = (string.Join(" ", lstMatches)).Trim();
            Assert.AreEqual(expectedText, actualText, expectedText + "is not equal to " + actualText); 
        }

        /// <summary>
          /// method to verify the voucher / miles value
          /// </summary>
          /// <param name="clubcard"></param>
          /// <param name="prefId"></param>
          internal void verifySecondBoxValue(string clubcard, OptionPreference preference, bool securityCleared = true)
          {
              Debug.WriteLine("verifySecondBoxValue Start");
              SmartVoucherAdapter adaptor = new SmartVoucherAdapter();
              ClubcardServiceAdapter cAdaptor = new ClubcardServiceAdapter();
              string strVouchers = cAdaptor.GetVouchers(Login.CustomerID, CountrySetting.culture, 0);
              decimal vouchers = 0;
              decimal.TryParse(strVouchers, out vouchers);
              string availableVouchers = string.Empty;
              Resource currencySymbol = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYSYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
              Resource currencySymbolAlpha = AutomationHelper.GetResourceMessage(LabelKey.CURRENCYALPHASYM, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.POINTS_RESOURCE));
              
              switch (preference)
              {
                  case OptionPreference.None:
                      availableVouchers = adaptor.GetAvailableVouchersCount(clubcard, CountrySetting.culture);
                      availableVouchers = string.Format("{0}{1}{2}", currencySymbol.Value, availableVouchers, currencySymbolAlpha.Value);
                      break;
                  case OptionPreference.Xmas_Saver:
                      availableVouchers = string.Format("{0}{1}{2}", currencySymbol.Value, strVouchers, currencySymbolAlpha.Value);
                      break;
                  case OptionPreference.Airmiles_Premium:
                      availableVouchers = (BusinessConstants.PRIMIUM_AMILES * vouchers / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE)).ToString();
                      break;
                  case OptionPreference.Airmiles_Standard:
                      availableVouchers = (BusinessConstants.STANDARD_AMILES * vouchers / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE)).ToString();
                      break;
                  case OptionPreference.BA_Miles_Premium:
                      availableVouchers = (BusinessConstants.PRIMIUM_BAMILES * vouchers / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE)).ToString();
                      break;
                  case OptionPreference.BA_Miles_Standard:
                      availableVouchers = (BusinessConstants.STANDARD_BAMILES * vouchers / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE)).ToString();
                      break;
                  case OptionPreference.Virgin_Atlantic:
                      availableVouchers = (BusinessConstants.VIRGIN_ATLANTIC * vouchers / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE)).ToString();
                      break;
              }
              string actualValue = string.Empty;
              // varify if security cleared
              if (securityCleared)
              {
                  IWebElement sb = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDARYBOX), FindBy.CSS_SELECTOR_CSS);
                  if (sb != null)
                  {
                      if (!sb.Text.Contains(availableVouchers))
                      {
                          Assert.Fail(string.Format("{0} not found in {1}", availableVouchers, sb.Text));
                      }
                  }
                  else 
                  {
                      Debug.WriteLine("verifySecondBoxValue|secondary box ctrl not found");
                      Assert.Fail("secondary box ctrl not found");
                  }
              }
              else
              {
                  switch (preference)
                  {
                      case OptionPreference.None:
                      case OptionPreference.Xmas_Saver:
                          IWebElement sb = objGeneric.GetWebControl(ObjAutomationHelper.GetControl(ControlKeys.HOME_SECONDARYBOX), FindBy.CSS_SELECTOR_CSS);
                          if (sb != null)
                          {
                              if (sb.Text.Contains(availableVouchers))
                              {
                                  Assert.Fail(string.Format("{0} found in {1}", availableVouchers, sb.Text));
                              }
                          }
                          else
                          {
                              Debug.WriteLine("verifySecondBoxValue|secondary box ctrl not found");
                              Assert.Fail("secondary box ctrl not found");
                          }
                          break;
                  }
              }
              Debug.WriteLine("verifySecondBoxValue End");
          }

          public string VerifyCouponBanner()
          {
              string error = string.Empty;
              Control banner = ObjAutomationHelper.GetControl(ControlKeys.HOME_COUPONBANNER);
              IWebElement eBanner = objGeneric.GetWebControl(banner, FindBy.CSS_SELECTOR_ID);
              ClubcardCouponAdaptor service = new ClubcardCouponAdaptor();
              int totalCoupon;
              int CouponCount = service.GetAvailableCouponCount(Login.CustomerID.ToString(), CountrySetting.culture, out totalCoupon);
              if (CouponCount > 0 && !eBanner.Displayed)
              {
                  error = string.Format("Customer has {0} coupons, but banner is not visible", CouponCount);
              }
              //validate coupon count is correct
              Control cc = ObjAutomationHelper.GetControl(ControlKeys.HOME_COUPONBANNERCOUNT);
              IWebElement ecc = objGeneric.GetWebControl(banner, FindBy.CSS_SELECTOR_ID);
              if(!string.IsNullOrEmpty(ecc.Text) && !ecc.Text.Trim().Equals(CouponCount.ToString()))
              {
                  error += string.Format("Customer has {0} coupons, but banner is showing {1} coupons", CouponCount, ecc.Text);
              }
              return error;
          }
    }
}
