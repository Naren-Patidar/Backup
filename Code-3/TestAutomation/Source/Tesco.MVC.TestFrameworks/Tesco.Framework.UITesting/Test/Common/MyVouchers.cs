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
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium.Interactions;
using Tesco.Framework.UITesting.Constants;
using System.IO;
using System.Globalization;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    class MyVouchers : Base
    {
        IAlert alert = null;
        Generic objGeneric = null;
        List<Decimal> val = new List<Decimal>();
        Decimal text = 0;
        string totalRedeemedValue = string.Empty;
        Decimal RedeemedValue = 0;
        Decimal expectedtotal = 0;
        string total = string.Empty;

        #region Constructor

        public MyVouchers(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric=new Generic(ObjAutomationHelper);
        }


        #endregion
        #region Methods

         public void ClickElement_Print(string keys, String pageName)
        {
            try
            {
                CustomLogs.LogMessage("Clicking on Element " + keys + " starting", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                IWebElement print = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                Driver.ExecuteJs<bool>("arguments[0].click();return true;", print);
                Thread.Sleep(2000);
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

        public void CheckImagePresent(Enums.VoucherSection SocialSite)
        {
            try
            {
                CustomLogs.LogMessage("CheckImage Present started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                switch (SocialSite)
                {
                    case Enums.VoucherSection.BothEnabled:
                        if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TWITTER).XPath)), Driver))
                        {
                            CustomLogs.LogInformation("Images present for Twitter");
                            if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_FACEBOOK).XPath)), Driver))
                                CustomLogs.LogInformation("Images present for Facebbok");
                            else
                                Assert.Fail("Image not Present for Facebook");
                        }
                        else
                            Assert.Fail("Image not Present for Twitter");
                        break;

                    case Enums.VoucherSection.BothDisabled:
                        if (!Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TWITTER).XPath)), Driver))
                        {
                            CustomLogs.LogInformation("Images Not present for Twitter");
                            if (!Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_FACEBOOK).XPath)), Driver))
                                CustomLogs.LogInformation("Images not present for Facebbok");
                            else
                                Assert.Fail("Image Present for Facebook");
                        }
                        else
                            Assert.Fail("Image Present for Twitter");
                        break;

                    case Enums.VoucherSection.TwitterEnabled:
                        if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TWITTER).XPath)), Driver))
                        {
                            CustomLogs.LogInformation("Images present for Twitter");
                            if (!Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_FACEBOOK).XPath)), Driver))
                                CustomLogs.LogInformation("Images not present for Facebbok");
                            else
                                Assert.Fail("Image Present for Facebook");
                        }
                        else
                            Assert.Fail("Image not Present for Twitter");
                        break;

                    case Enums.VoucherSection.FacebookEnabled:
                        if (!Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TWITTER).XPath)), Driver))
                        {
                            CustomLogs.LogInformation("Images not present for Twitter");
                            if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_FACEBOOK).XPath)), Driver))
                                CustomLogs.LogInformation("Images present for Facebbok");
                            else
                                Assert.Fail("Image not Present for Facebook");
                        }
                        else
                            Assert.Fail("Image Present for Twitter");
                        break;
                }
                       

                CustomLogs.LogMessage("CheckImage Present completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void VerifySection(Enums.VoucherSection Visibility,string BoxNumber, string TextFromConfig, string TextFromUI)
        {
            try
            {
                CustomLogs.LogMessage("VerifySection Present started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                switch(Visibility)
                {
                    case Enums.VoucherSection.Displayed:
                        if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(BoxNumber).Id), Driver))
                            objGeneric.verifyValidationMessage(TextFromConfig, TextFromUI, "", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                        else
                            Assert.Fail(BoxNumber + " Field not Present");
                    break;
                    case Enums.VoucherSection.NotDisplayed:
                    if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(BoxNumber).Id), Driver))
                        Assert.Fail(BoxNumber + " Field not Present");
                    else
                        CustomLogs.LogInformation(" section not present");
                    break;
                }
             
                CustomLogs.LogMessage("VerifySection Present completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void VerifyTotalCount(Enums.VoucherSection TypeOfVoucher , string msgId , string resourceFileName)
        {
            try
            {
                CustomLogs.LogMessage("VerifyTotalCount Present started", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;

              //  string JoinTitle = (AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE))).Value;
                 Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                char[] CurrencySymbol = res.Value.ToCharArray();
                var totalOnTop1 = String.Empty;
                switch(TypeOfVoucher)
                {
                    case Enums.VoucherSection.Used:
                        if (CountrySetting.country.Equals("PL") )
                        {
                           totalOnTop1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TOTALONTOP1).Id)).Text.Split(CurrencySymbol[0])[0].TrimStart();
                        }
                        else if (CountrySetting.country.Equals("SK")|| CountrySetting.country.Equals("CZ"))
                        {                            
                             totalOnTop1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TOTALONTOP1).Id)).Text.Split(" "[0])[1];                             
                        }
                        else
                        {
                            totalOnTop1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TOTALONTOP1).Id)).Text.Split(CurrencySymbol[0])[1].TrimStart();
                        }
                        Decimal.TryParse(totalOnTop1, out expectedtotal);
                        total = TotalRedeemedCount(LabelKey.CURRENCYSYMBOL ,SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                        Assert.AreEqual(expectedtotal.ToString(), total, "Redeemed vouchers on top doesn't matches with actual Redeemed voucher's count");                       
                    break;

                    case Enums.VoucherSection.UnUsed:
                    var totalOnTop = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TOTALONTOP).XPath)).Text;
                    List<string> price = new List<string>();
                    var totalCountOnBottom = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TOTALINACTIVEGRID).Id)).Text;
                        //if (totalCountOnBottom.Contains(totalOnTop))
                        //{
                        //    var s = totalCountOnBottom.Split(',');
                        //    for (int i = 0; i < s.Length ; i++)
                        //    {                               
                        //        price.Add(s[i]);
                        //    }

                        //    totalCountOnBottom = string.Concat(price); 
                        //}
                   Assert.AreEqual(totalOnTop, totalCountOnBottom, "Total Count on top section" + totalOnTop + " not equal to total count on bottom section" + totalCountOnBottom);
                        break;
                }                             
                CustomLogs.LogMessage("VerifyTotalCount Present completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public string TotalRedeemedCount(string msgId ,string resourceFileName)
         {
          try
                {
                    Driver = ObjAutomationHelper.WebDriver;                                 
                CustomLogs.LogMessage("VerifyTotalCount Present started", TraceEventType.Start);

                Resource res = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                char[] CurrencySymbol = res.Value.ToCharArray();

                ReadOnlyCollection<IWebElement> Value = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_REDEEMEDVALUE).Id)));
              //   ReadOnlyCollection<IWebElement> Value = Driver.FindElements(By.XPath("//*[@id='lblVoucherValue']"));
              //   Driver.FindElements(By.XPath("//*[@id='div_VoucherUsed']"));
                if (Value.Count > 0)
                {
                    switch (CountrySetting.country)
                    {
                        case "UK":
                            for (int i = 0; i < Value.Count; i++)
                            {
                                text = Decimal.Parse((Value[i].Text.Split(CurrencySymbol[0]))[1]);
                                val.Add(text);
                                RedeemedValue = val[i] + RedeemedValue;
                            }
                            totalRedeemedValue = RedeemedValue.ToString("0.00");
                            break;

                        case "SK":                     
                        case "TH":
                        case "CZ":                       
                        case "PL":
                        case "HU":
                            decimal no = 0;
                            for (int i = 0; i < Value.Count; i++)
                            {
                                int length = Value[i].Text.Split(new Char[] { CurrencySymbol[0], ' ' }).Length;
                                if (length > 2)
                                {
                                    no = int.Parse(Value[i].Text.Split(new Char[] { CurrencySymbol[0], ' ', ',' })[3]);
                                }
                                else if (length == 2)
                                {
                                    Decimal.TryParse(Value[i].Text.Split(CurrencySymbol[0])[1], out no);
                                }
                                else
                                {
                                    no = Decimal.Parse(Value[i].Text.Split(CurrencySymbol[0])[1]);
                                }
                                val.Add(no);
                                RedeemedValue = val[i] + RedeemedValue;
                            }
                            totalRedeemedValue = RedeemedValue.ToString();
                            break;
                    }
                }
                else
                    CustomLogs.LogInformation("No Redeemed vouchers available. Kindly Check again.");
                return totalRedeemedValue;
                }
             
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
          CustomLogs.LogMessage("VerifyTotalCount Present completed", TraceEventType.Stop);
          return totalRedeemedValue;
         
        }
         public void VerifyOptedPreferenceSection(Enums.Preferences Preference)
         {
             try
             {
                 CustomLogs.LogMessage("CheckImage Present started", TraceEventType.Start);
                 Driver = ObjAutomationHelper.WebDriver;

                 Resource res = AutomationHelper.GetResourceMessage(LabelKey.POINTSCOLLECTEDINLAST2YEAR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE));
                 var expectedMessage = res.Value;
                 var actualMessage = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TXTPOINTCOLLECTED).XPath)).Text;
                 //var actualMessage = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TXTPOINTCOLLECTED).Id)).Text;
                 Assert.AreEqual(expectedMessage, actualMessage.Split('\r')[0] , " Points Collected text not verified");

                 //if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_IMGARROW).XPath)), Driver))
                 //{
                     //CustomLogs.LogInformation("Image Present for Arrow");
                     //switch (Preference)
                     //{
                     //    case Enums.Preferences.Avios:
                     //        if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_IMGAVIOS).XPath)), Driver))
                     //            CustomLogs.LogInformation("Images present for Avios");
                     //        else
                     //            Assert.Fail("Image not Present for Avios");
                     //        break;
                     //    case Enums.Preferences.BAAvios:
                     //        if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_IMGAVIOS).XPath)), Driver))
                     //            CustomLogs.LogInformation("Images present for BA Avios");
                     //        else
                     //            Assert.Fail("Image not Present for BA Avios");
                     //        break;
                     //    case Enums.Preferences.VirginAtlantic:
                     //        if (Generic.IsElementPresent((By.XPath(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_IMGVIRGINATLANTIC).XPath)), Driver))
                     //            CustomLogs.LogInformation("Images present for Virgin Atlantic");
                     //        else
                     //            Assert.Fail("Image not Present for Virgin Atlantic");
                     //        break;
                     //}
                    
                 //}
                 //else
                     //Assert.Fail("Image Not Present For Arrow");

             }
             catch (Exception ex)
             {
                 CustomLogs.LogException(ex);
                 ScreenShotDetails.TakeScreenShot(Driver, ex);
                 Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                 Driver.Quit();
             }

         }

         public void verifyPageName(string type)
         {
             CustomLogs.LogMessage("CheckImage Present started", TraceEventType.Start);
             Driver = ObjAutomationHelper.WebDriver;
             switch (type)
             {
                 case "Avios":
                     objGeneric.verifyValidationMessageByValue(LabelKey.AVIOS_TITLE, ControlKeys.PAGE_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                     break;
                 case "XmasSaver":
                     objGeneric.verifyValidationMessageByValue(LabelKey.VOUCHER_TITLE, ControlKeys.PAGE_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                     break;
                 case "BAAvios":
                     objGeneric.verifyValidationMessageByValue(LabelKey.BAAVIOS_TITLE, ControlKeys.PAGE_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                     break;
                 case "VirginAtlantic":
                     objGeneric.verifyValidationMessageByValue(LabelKey.VIRGIN_TITLE, ControlKeys.PAGE_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                     break;
                 case "NoPreference":
                     objGeneric.verifyValidationMessageByValue(LabelKey.VOUCHER_TITLE, ControlKeys.PAGE_TITLE, "vouchers", SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                     break;
             }
         }
        #region Text Validation 
         public void TextValidation(string pageName)
         {
             string errorMessage = string.Empty;
             try
             {
                 Driver = ObjAutomationHelper.WebDriver;
                 string resxFile=Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.VOUCHER_RESOURCE);
                 errorMessage = objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLVIEWSUMMARY,ControlKeys.MYVOUCHER_TEXTLCLVIEWSUMMARY,resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains_year(ValidationKey.VOUCHER_LCLVOUCHERSUMHEADER,ControlKeys.MYVOUCHER_LCLVOUCHERSUMHEADER,resxFile, pageName , CountrySetting.culture);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLFBTWTHEADER,ControlKeys.MYVOUCHER_LCLFBTWTHEADER, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLTELLFRIENDS,ControlKeys.MYVOUCHER_LCLTELLFRIENDS,resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLFBTWTSHAREMESSAGE,ControlKeys.MYVOUCHER_LCLFBTWTSHAREMESSAGE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLUNUSEDVOUCHERSHEADER,ControlKeys.MYVOUCHER_LCLUNUSEDVOUCHERSHEADER,resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLVOUCHERSUSEDONCE,ControlKeys.MYVOUCHER_LCLVOUCHERSUSEDONCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLCURRENTUNSPENDVOUCHERS, ControlKeys.MYVOUCHER_LCLCURRENTUNSPENDVOUCHERS, resxFile, pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLFOUNDBELOW,ControlKeys.MYVOUCHER_LCLFOUNDBELOW, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLASPENTRESOURCE,ControlKeys.MYVOUCHER_LCLASPENTRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLDOTCOMRESOURCE,ControlKeys.MYVOUCHER_LCLDOTCOMRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLCPYVOUCHERCODERESOURCE,ControlKeys.MYVOUCHER_LCLCPYVOUCHERCODERESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLCHECKOUTRESOURCE,ControlKeys.MYVOUCHER_LCLCHECKOUTRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLPRINTVOUCHERRESOURCE,ControlKeys.MYVOUCHER_LCLPRINTVOUCHERRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLSELECTVOUCHERRESOURCE,ControlKeys.MYVOUCHER_LCLSELECTVOUCHERRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLCLICKPRINTRESOURCE,ControlKeys.MYVOUCHER_LCLCLICKPRINTRESOURCE,resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLPRINTRESOURCE,ControlKeys.MYVOUCHER_LCLPRINTRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLTILLRESOURCE,ControlKeys.MYVOUCHER_LCLTILLRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLREDEMVOUCHERRESOURCE,ControlKeys.MYVOUCHER_LCLREDEMVOUCHERRESOURCE, resxFile,pageName);
                 errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.VOUCHER_LCLEXPDATERESOURCE,ControlKeys.MYVOUCHER_LCLEXPDATERESOURCE, resxFile,pageName);
              
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
        #endregion 
         
        #endregion
    }

}
