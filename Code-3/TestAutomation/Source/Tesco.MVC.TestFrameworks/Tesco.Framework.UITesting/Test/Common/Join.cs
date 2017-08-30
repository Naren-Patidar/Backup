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
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.IO;
using Tesco.Framework.UITesting.Constants;
using System.Reflection;
using Tesco.Framework.UITesting.Services;
using System.Text.RegularExpressions;
using System.Web;

namespace Tesco.Framework.UITesting.Test.Common
{
    class Join : Base
    {

        Generic objGeneric = null;
        TestData_JoinDetails testData = null;
        
        Random rnd=null;
        #region Constructor
        public Join(AutomationHelper objhelper)
        {
           
            this.ObjAutomationHelper = objhelper;
        }
        public Join(AutomationHelper objHelper, AppConfiguration configuration, TestData_JoinDetails testData)
        {
            this.testData = testData;
            ObjAutomationHelper = objHelper;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }
        #endregion

        #region Methods        

        //Join Page banner has been removed in reskin
        //public void VerifyHeaderImageTextOnJoinPage(IWebDriver Driver)
        //{
        //    try
        //    {
        //        CustomLogs.LogMessage("Verifying Header Image Text on Join Page started ", TraceEventType.Start);
        //        string headerText = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HEADERIMAGETEXT).XPath)).Text;

        //        Resource resHeaderImageTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_IMAGE_TEXT_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
        //        Resource resHeaderImageTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_IMAGE_TEXT_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

        //        if (headerText.Equals(string.Format("{0} {1}", resHeaderImageTextOne.Value, resHeaderImageTextTwo.Value).Trim()))
        //        {
        //            CustomLogs.LogInformation("Join Header Image Text verified for country " + CountrySetting.country);
        //        }
        //        else
        //        {
        //            Assert.Fail("Join Header Image Text not verified for country " + CountrySetting.country);
        //        }
        //        CustomLogs.LogMessage("Verifying Header Image Text on Join Page completed ", TraceEventType.Stop);
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomLogs.LogException(ex);
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //        Driver.Quit();
        //    }
        //}

        public void VerifyHeaderTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Header Text on Join Page started ", TraceEventType.Start);
                string headerTextElements = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HEADERTEXT).Id)).Text;

                Resource resHeaderTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextThree = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_THREE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextFour = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_FOUR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextFive = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_FIVE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextSix = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_SIX, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resHeaderTextSeven = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_SEVEN, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                //Resource resHeaderTextEight = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_EIGHT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                if (headerTextElements.Equals(string.Format("{0} {1} {2} {3} {4} {5} {6}", resHeaderTextOne.Value, resHeaderTextTwo.Value,
                    resHeaderTextThree.Value, resHeaderTextFour.Value, resHeaderTextFive.Value, resHeaderTextSix.Value, resHeaderTextSeven.Value).Trim()))
                {
                    CustomLogs.LogInformation("Join Header Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Header Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Header Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        //Join doesnt have this scenario. Its present in Personal Details page only.
        //public void VerifyHeaderParaOneTextOnJoinPage(IWebDriver Driver)
        //{
        //    try
        //    {
        //        CustomLogs.LogMessage("Verifying Header Para One Text on Join Page started ", TraceEventType.Start);
        //        string headerTextElements = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HEADERTEXT).Id)).Text;
        //        Resource resHeaderTextParaOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_PARA_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
        //        string expectedText = string.IsNullOrEmpty(resHeaderTextParaOne.Value) ? string.Empty : resHeaderTextParaOne.Value;
        //        string actualText = headerTextElements;
        //        if (!string.IsNullOrWhiteSpace(headerTextElements))
        //        {
        //            Assert.AreEqual(expectedText, actualText);
        //        }                
        //        else
        //        {
        //            Assert.Fail(string.Format("Unable to find text '{0}' on Join Page", expectedText));
        //        }
        //        CustomLogs.LogMessage("Verifying Header Text on Join Page completed ", TraceEventType.Stop);
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomLogs.LogException(ex);
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //        Driver.Quit();
        //    }
        //}

        public void VerifyHeaderInContactDetailsOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Header Text in contact details section on Join Page started ", TraceEventType.Start);
                if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HEADERTEXTCONTACTDETAILS).Id)))
                {
                    string headerTextElements = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HEADERTEXTCONTACTDETAILS).Id)).Text;

                    Resource resHeaderTextContactDetails = AutomationHelper.GetResourceMessage(LabelKey.JOIN_HEADER_TEXT_CONTACTDETAILS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                    if (headerTextElements.Equals(string.Concat(resHeaderTextContactDetails.Value.Trim(), ".")))
                    {
                        CustomLogs.LogInformation("Join Header Text in contact section verified for country " + CountrySetting.country);
                    }
                    else
                    {
                        Assert.Fail("Join Header Text in contact section not verified for country " + CountrySetting.country);
                    }
                    CustomLogs.LogMessage("Verifying Header Text in contact details section on Join Page completed ", TraceEventType.Stop);
                }
                else
                {
                    CustomLogs.LogMessage("control is not present on the page",TraceEventType.Stop);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyHeaderInTermsAndConditionsOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Header For Terms And Conditions Section on Join Page started ", TraceEventType.Start);
                string headerTermsAndConditionsSection = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_LABELTERMSANDCONDITIONS).Id)).Text;

                Resource resHeaderTermsAndConditions = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_TERMS_AND_CONDITIONS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                if (headerTermsAndConditionsSection.Equals(resHeaderTermsAndConditions.Value))
                {
                    CustomLogs.LogInformation("Join Header Text for Terms And Conditions verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Header Text for Terms And Conditions not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying For Terms And Conditions Section on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyAgreeTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Agree Text on Join Page started ", TraceEventType.Start);
                string agreeText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_LABELAGREE).Id)).Text;

                Resource resAgreeTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_AGREE_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resAgreeTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_TERMS_AND_CONDITIONS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resAgreeTextThree = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_AGREE_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                if (agreeText.Equals(string.Format("{0} {1} {2}", resAgreeTextOne.Value, resAgreeTextTwo.Value, resAgreeTextThree.Value).Trim()))
                {
                    CustomLogs.LogInformation("Join Agree Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Agree Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Agree Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyConfirmClickTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Confirm Click Text on Join Page started ", TraceEventType.Start);
                string confirmClickText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_LABELCONFIRMCLICK).Id)).Text;

                Resource resConfirmClickTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CONFIRM_CLICK_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resConfirmClickTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CONFIRM_CLICK_FOUR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                string error = objGeneric.ValidateResourceValueWithHTMLContent(confirmClickText, HttpUtility.HtmlDecode(string.Format("{0} {1}", resConfirmClickTextOne.Value, resConfirmClickTextTwo.Value).Trim()));

                if (string.IsNullOrEmpty(error))
                {
                    CustomLogs.LogInformation("Join Confirm Click Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Confirm Click Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Confirm Click Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyCaptchaTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Captcha Text on Join Page started ", TraceEventType.Start);
                string captchaText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_CAPTCHATEXT).Id)).Text;

                Resource resCaptchaTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CAPTCHA_TEXT_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resCaptchaTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CAPTCHA_TEXT_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));

                if (captchaText.Equals(string.Format("{0} {1}", resCaptchaTextOne.Value, resCaptchaTextTwo.Value).Trim()))
                {
                    CustomLogs.LogInformation("Join Captcha Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Captcha Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Captcha Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyOptInsHeaderTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Opts Ins Header Text on Join Page started ", TraceEventType.Start);
                string optInsTextOne = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTINSTEXTONE).Id)).Text;
                string optInsTextTwo = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTINSTEXTTWO).Id)).Text;
                string optInsTextThree = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTINSTEXTTHREE).Id)).Text;
                string optInsTextFour = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTINSTEXTFOUR).Id)).Text;
                string optInsTextFive = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTINSTEXTFIVE).Id)).Text;

                Resource resOptInsTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_INS_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptInsTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_INS_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptInsTextThree = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_INS_THREE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptInsTextFour = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_INS_FOUR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptInsTextFive = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_INS_FIVE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));

                if (optInsTextOne.Equals(resOptInsTextOne.Value) && optInsTextTwo.Equals(resOptInsTextTwo.Value) && optInsTextThree.Equals(resOptInsTextThree.Value)
                    && optInsTextFour.Equals(resOptInsTextFour.Value) && optInsTextFive.Equals(resOptInsTextFive.Value))
                {
                    CustomLogs.LogInformation("Join Opts Ins Header Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Opts Ins Header Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Opts Ins Header Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        public void VerifyOptInsFooterTextOnJoinPage(IWebDriver Driver, string resID, string ctr)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Opts Ins Header Text on Join Page started ", TraceEventType.Start);
                string actualFooterNote = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ctr).Id)).Text;
                Resource expectedFooterNote = AutomationHelper.GetResourceMessage(resID, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Regex htmls = new Regex("<[^>]*>");
                List<string> matches = htmls.Split(expectedFooterNote.Value).ToList();
                List<string> lstMatches = new List<string>();
                matches.ForEach(m => lstMatches.Add(m));
                String expectedFooterNoteValue = string.Join(" ", lstMatches);
                string[] str = expectedFooterNoteValue.Split(new[] {',', ' ', '.'},StringSplitOptions.RemoveEmptyEntries);
                expectedFooterNoteValue = string.Join("", str);
                string[] str1 = actualFooterNote.Split(new[] { ',', ' ','.' },StringSplitOptions.RemoveEmptyEntries);
                String actualFooterNoteValue = string.Join("", str1);
                actualFooterNoteValue = actualFooterNoteValue.Replace("\r\n", string.Empty);
                if (expectedFooterNoteValue.Equals(actualFooterNoteValue))
                {
                    CustomLogs.LogInformation("Join Opts Ins Footer Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Opts Ins Header Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Opts Ins Header Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyOptOutsHeaderTextOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Opt Outs Header Text on Join Page started ", TraceEventType.Start);
                string optOutsTextOne = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTOUTSTEXTONE).Id)).Text;
                string optOutsTextTwo = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTOUTSTEXTTWO).Id)).Text;
                string optOutsTextThree = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTOUTSTEXTTHREE).Id)).Text;
                optOutsTextThree = optOutsTextThree.Split(new[] {optOutsTextTwo}, StringSplitOptions.RemoveEmptyEntries)[0];
                string optOutsTextFour = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTOUTSTEXTFOUR).Id)).Text;

                Resource resOptOutsTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_OUTS_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptOutsTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_OUTS_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptOutsTextThree = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_OUTS_THREE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptOutsTextFour = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_OUTS_FOUR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));

                if (optOutsTextOne.Equals(resOptOutsTextOne.Value) && optOutsTextTwo.Equals(resOptOutsTextTwo.Value)
                    && optOutsTextThree.Trim().Equals(resOptOutsTextThree.Value) && optOutsTextFour.Equals(resOptOutsTextFour.Value))
                {
                    CustomLogs.LogInformation("Join Opt Outs Header Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Opt Outs Header Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Opt Outs Header Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyOptFooterLinkOnJoinPage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Opt Footer Text on Join Page started ", TraceEventType.Start);
                string optFooterTextOne = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTFOOTERONE).Id)).Text;
                string optFooterTextTwo = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_OPTFOOTERTWO).Id)).Text;

                Resource resOptFooterTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_FOOTER_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));
                Resource resOptFooterTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_OPT_FOOTER_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));

                if (optFooterTextOne.Equals(resOptFooterTextOne.Value) && optFooterTextTwo.Equals(resOptFooterTextTwo.Value))
                {
                    CustomLogs.LogInformation("Join Opt Footer Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Opt Footer Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Opt Footer Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void VerifyCallRateTextOnJoinPage(IWebDriver Driver)
        {
            try
            {

                CustomLogs.LogMessage("Verifying Call Rate Text on Join Page started ", TraceEventType.Start);
                string callRateText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_LABELCALLRATE).Id)).Text;
               callRateText= objGeneric.SimplifyResourceText(callRateText);
                Resource resCallRateTextOne = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CALL_RATE_ONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resCallRateTextTwo = AutomationHelper.GetResourceMessage(LabelKey.JOIN_LABEL_CALL_RATE_TWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
                Resource resCallRateTextThree = AutomationHelper.GetResourceMessage(LabelKey.JOIN_FOOTERNOTE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PREFERENCES_RESOURCE));

                resCallRateTextOne.Value = objGeneric.SimplifyResourceText(resCallRateTextOne.Value);
                resCallRateTextTwo.Value = objGeneric.SimplifyResourceText(resCallRateTextTwo.Value);
                resCallRateTextThree.Value = objGeneric.SimplifyResourceText(resCallRateTextThree.Value);

                if (callRateText.Contains(resCallRateTextOne.Value) && callRateText.Contains(resCallRateTextTwo.Value) && callRateText.Contains(resCallRateTextOne.Value))
                {
                    CustomLogs.LogInformation("Join Call Rate Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Join Call Rate Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Call Rate Text on Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }


        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void FillMandatoryDetails(IWebDriver Driver)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver;
                CustomLogs.LogMessage("Mandatory fields are started filling. ", TraceEventType.Start);
                Driver.FindElement(By.Id("fld_title")).SendKeys(Keys.Down + Keys.Tab);                
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)).SendKeys(RandomString(7));
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));                             
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)).SendKeys(RandomString(7));
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));               
               // Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)).Click();
             //   Driver.FindElement(By.Name("CustomerFamilyMasterData.CustomerData[0].Sex")).Click();
                jse.ExecuteScript("document.getElementById('Male').click();");
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));               
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)).SendKeys("RH19 3se");
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)).SendKeys(Keys.Tab);
                jse.ExecuteScript("document.getElementById('btnfindAddress').click();");
              //  Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)).Click();
                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSENUMBER).Id)).SendKeys(RandomString(7));
                Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)).SendKeys(RandomString(6)+"@gmail.com");  
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("Mandatory fields are filled completely. ", TraceEventType.Stop);
        }

        public Dictionary<string, IWebElement> returnAllFieldsOnUI(Enums.FieldType type)
        {
            Driver = ObjAutomationHelper.WebDriver;
            Dictionary<string, IWebElement> join = new Dictionary<string, IWebElement>();
           
            switch(type)
            {
                case Enums.FieldType.Valid:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                     break;
                case Enums.FieldType.Invalid:
                    join.Add("InvalidEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("InvalidMailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidEveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidMobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidDayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("InvalidName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    break;
                case Enums.FieldType.ProfaneName1:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                      join.Add("ProfaneName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));  
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateEmail:
                    join.Add("DuplicateEmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.DuplicateNameANDAddress:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DuplicateName1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));

                    break;
                case Enums.FieldType.ProfaneName2:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("ProfaneName2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
                case Enums.FieldType.ProfaneName3:
                    join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
                    join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME2, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MIDDLENAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME1, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("Name1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_FIRSTNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDENAME3, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("ProfaneName3", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_SURNAME).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                        join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
                    if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDAYTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                       join.Add("DayTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
                    break;
            }
            join.Add("Sex", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)));
            join.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_DAY).Id)));
            join.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MONTH).Id)));
            join.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id)));
            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
                join.Add("TitleEnglish", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TITLE).Id)));
            
           
            switch (CountrySetting.country)
            {
                case "CZ":
                case "PL":
                case "SK":
                    if(type.Equals(Enums.FieldType.ProfaneMailingAddressLine1))
                    {
                    join.Add("ProfaneMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                    join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                    join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                    join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine2))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("ProfaneMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine4))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("ProfaneMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.ProfaneMailingAddressLine5))
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("ProfaneMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else if (type.Equals(Enums.FieldType.Invalid))
                    {
                        join.Add("InvalidMailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("InvalidMailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("InvalidMailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("InvalidMailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }
                    else
                    {
                        join.Add("MailingAddressLine1", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE1).Id)));
                        join.Add("MailingAddressLine2", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE2).Id)));
                        join.Add("MailingAddressLine4", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE4).Id)));
                        join.Add("MailingAddressLine5", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_ADDRESSLINE5).Id)));
                    }

                    break;
            }
            return join;
        }
        //public Dictionary<string,IWebElement> commonFields()
        //{
        //    Dictionary<string, IWebElement> join = new Dictionary<string, IWebElement>();
        //   // Dictionary<string, IWebElement> join = returnAllFieldsOnUI(type);
        //    try
        //    {
        //        CustomLogs.LogMessage("commonFields started ", TraceEventType.Start);
        //        Driver = ObjAutomationHelper.WebDriver;
        //            join.Add("EmailAddress", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EMAIL).Id)));
        //            join.Add("MailingAddressPostCode", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTPOSTCODE).Id)));
        //            join.Add("Sex", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNRADIOMALE).Id)));
        //            join.Add("Date", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_Date).Id)));
        //            join.Add("Month", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MONTH).Id)));
        //            join.Add("Year", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id))); 
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDETITLE, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("TitleEnglish", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TITLE).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEEVENINGNUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("EveningPhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_EVENINGNUMBER).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEMOBILENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("MobilePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_MOBILENUMBER).Id)));
        //            if (AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.ChinaHiddenFunctionality, DBConfigKeys.HIDEDateTIMENUMBER, SanityConfiguration.DbConfigurationFile).IsDeleted == "N")
        //                join.Add("DateTimePhoneNumber", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_PHONENUMBER).Id)));
           
        //        CustomLogs.LogMessage("coomonFields Completed ", TraceEventType.Stop);
        //        return join;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        ScreenShotDetails.TakeScreenShot(Driver, ex);
        //        CustomLogs.LogException(ex);
        //        Driver.Quit();
        //        Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
        //        return join;

        //    }
            
        //}
        public void fillMandatoryFields(Enums.FieldType type)
        {
            try
            {
                DBConfiguration config = null;
                CustomLogs.LogMessage("Filling Mandatory Fields started", TraceEventType.Start);
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();              
                for (int i = 0; i < properties.Count; i++)
                {
                    var value = properties[i].Name;
                    if (properties[i].Name == "Date" || properties[i].Name == "Month" || properties[i].Name == "Year")
                        value = "DateOfBirth";
                    else if (properties[i].Name == "AddressDropDown" || properties[i].Name == "PostCodeBtn")
                        value = "MailingAddressPostCode";
                    if (value.Contains("EmailAddress"))
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.JoinEmailMandatory, value, SanityConfiguration.DbConfigurationFile);
                    else
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        if (fields.Keys.Contains(properties[i].Name))
                        {
                            if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name.Contains("Date") || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                                fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                            else if (properties[i].Name == "MobilePhoneNumber" || properties[i].Name == "EveningPhoneNumber" || properties[i].Name == "DayTimePhoneNumber")
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                            else if (properties[i].Name=="EmailAddress" || properties[i].Name.Contains("Name1"))
                                fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                            else if (properties[i].Name.Contains("Sex"))
                                fields[properties[i].Name].Click();
                            else if (properties[i].Name.Contains("PostCodeBtn"))
                            {
                                fields[properties[i].Name].Click();
                                Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                                fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSEDROPDOWN).Id)));
                            }
                            else if (properties[i].Name=="MailingAddressPostCode" && CountrySetting.country == "UK")
                            {
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                                fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)));

                            }
                            else
                                fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                    }
                }
                CustomLogs.LogMessage("Filling Mandatory Fields completed", TraceEventType.Stop);
            }            
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
       
        public void fillAllFields(Enums.FieldType type)
        {
            try
            {
                rnd = new Random();
                Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();

                for (int i = 0; i < properties.Count; i++)
                {
               
                    if (fields.Keys.Contains(properties[i].Name))
                    {
                        if (properties[i].Name == "DuplicateEmailAddress")
                        {
                            CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                            string emailforjoin = customer.GetEmailIdForJoin(CountrySetting.culture);
                            properties[i].SetValue(testData, emailforjoin, null);
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                        }
                        else if (properties[i].Name.Contains("TitleEnglish") || properties[i].Name=="Date" || properties[i].Name.Contains("Month") || properties[i].Name.Contains("Year") || properties[i].Name.Contains("AddressDropDown"))
                            fields[properties[i].Name].SendKeys(OpenQA.Selenium.Keys.Down);
                        else if (properties[i].Name=="MobilePhoneNumber"||properties[i].Name=="EveningPhoneNumber"||properties[i].Name=="DayTimePhoneNumber")
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string + rnd.Next(0, 999999999).ToString());
                        else if (properties[i].Name=="EmailAddress" || properties[i].Name=="Name1")
                            fields[properties[i].Name].SendKeys(RandomString(6) + properties[i].GetValue(testData, null) as string);
                        else if (properties[i].Name.Contains("Sex"))
                            fields[properties[i].Name].Click();
                        else if (properties[i].Name.Contains("PostCodeBtn"))
                        {
                            fields[properties[i].Name].Click();
                            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                            fields.Add("AddressDropDown", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSEDROPDOWN).Id)));
                        }
                        else if (properties[i].Name=="MailingAddressPostCode" && CountrySetting.country == "UK")
                        {
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                            fields.Add("PostCodeBtn", Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_BTNPOSTCODE).Id)));

                        }
                        else
                            fields[properties[i].Name].SendKeys(properties[i].GetValue(testData, null) as string);
                    }
                }
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        
                
        public void verifyValidationCheck(Enums.FieldType type)
        {
            try
            {
                CustomLogs.LogMessage("verifyValidationCheck started ", TraceEventType.Start);
                Dictionary<string, IWebElement> fields = null;
                
               if(type.Equals(Enums.FieldType.All))
                 fields = returnAllFieldsOnUI(Enums.FieldType.Invalid);
               else if (type.Equals(Enums.FieldType.Mandatory))
                   fields = returnMandatoryFields(Enums.FieldType.Valid);
                Type myType = testData.GetType();
                List<PropertyInfo> properties = myType.GetProperties().ToList();
                for (int i = 0; i < properties.Count; i++)
                {
                    if(fields.Keys.Contains(properties[i].Name))
                    {
                        string value=properties[i].Name;
                        if ((value.Contains(Enums.JoinElements.TitleEnglish.ToString()))&&type==Enums.FieldType.Mandatory)
                            objGeneric.VerifyTextonthePageByXpath(ValidationKey.ERRORTITLE, ControlKeys.JOIN_ERRORTITLE, "Join error for Title", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if ((value.Contains(Enums.JoinElements.Date.ToString()) || value.Contains(Enums.JoinElements.Month.ToString()) || value.Contains(Enums.JoinElements.Year.ToString()))&&type==Enums.FieldType.Mandatory)
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORDOB, ControlKeys.JOIN_ERRORDOB, "Join error for DOB", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if ((value.Contains(Enums.JoinElements.Sex.ToString())) && type == Enums.FieldType.Mandatory)
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORGENDER, ControlKeys.JOIN_ERRORGENDER, "Join error for gender", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName1.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME1, ControlKeys.JOIN_ERRORNAME1, "Join Error For Name1", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName2.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME2, ControlKeys.JOIN_ERRORNAME2, "Join Error For Name2", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidName3.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORNAME3, ControlKeys.JOIN_ERRORNAME3, "Join error for name3", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidEmailAddress.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERROREMAIL, ControlKeys.JOIN_ERROREMAIL, "Join error for email", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressPostCode.ToString().Contains(value))
                         {
                             string key = String.Empty;
                             if (CountrySetting.country.Equals("UK"))
                                 key = ValidationKey.ERRORPOSTCODE_UK;
                             else
                                 key = ValidationKey.ERRORPOSTCODE_GC;
                             objGeneric.verifyValidationMessage(key, ControlKeys.JOIN_ERRORPOSTCODE, "Join error for postcode", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                         }
                        else if (Enums.JoinElements.InvalidMailingAddressLine1.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE1, ControlKeys.JOIN_ERRORADDRESSLINE1, "Join error for address line 1", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine2.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE2, ControlKeys.JOIN_ERRORADDRESSLINE2, "Join error for address line 2", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine4.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE4, ControlKeys.JOIN_ERRORADDRESSLINE4, "Join error for address line 4", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMailingAddressLine5.ToString().Contains(value))
                             objGeneric.verifyValidationMessage(ValidationKey.ERRORADDRESSLINE5, ControlKeys.JOIN_ERRORADDRESSLINE5, "Join error for address line 5", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidEveningPhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERROREVENINGNUMBER, ControlKeys.JOIN_ERROREVNGNUMBR, "Join error for evening number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidDayTimePhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORDAYTIMENUMBER, ControlKeys.JOIN_ERRORDAYNUMBR, "Join error for day time number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                        else if (Enums.JoinElements.InvalidMobilePhoneNumber.ToString().Contains(value))
                            objGeneric.verifyValidationMessage(ValidationKey.ERRORMOBILENUMBER, ControlKeys.JOIN_ERRORMOBILENUMBR, "Join error for mobile number", SanityConfiguration.ResourceFiles.JOIN_RESOURCE);
                    }
                }
                CustomLogs.LogMessage("verifyValidationCheck completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }

        public Dictionary<String,IWebElement> returnMandatoryFields(Enums.FieldType type)
        {
            DBConfiguration config = null;
            rnd = new Random();
            Dictionary<string, IWebElement> fields = returnAllFieldsOnUI(type);
            Dictionary<string, IWebElement> mandatoryFields = new Dictionary<string,IWebElement>();
            Type myType = testData.GetType();
            List<PropertyInfo> properties = myType.GetProperties().ToList();
            for (int i = 0; i < properties.Count; i++)
            {
                if (fields.Keys.Contains(properties[i].Name))
                {
                    var value = properties[i].Name;
                    if (properties[i].Name == Enums.JoinElements.Date.ToString() || properties[i].Name == Enums.JoinElements.Month.ToString() || properties[i].Name == Enums.JoinElements.Year.ToString())
                        value = DBConfigKeys.DOB;
                    if (value.Contains(Enums.JoinElements.EmailAddress.ToString()))
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.JoinEmailMandatory, value, SanityConfiguration.DbConfigurationFile);
                    else
                        config = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Mandatory_fields, value, SanityConfiguration.DbConfigurationFile);
                    if (config.IsDeleted == "N")
                    {
                        mandatoryFields.Add(properties[i].Name, fields[properties[i].Name]);
                    }
                }
            }
            return mandatoryFields;
        }

        public void SelectYearInDOB()
        {
            Driver = ObjAutomationHelper.WebDriver;
            IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("SelectYearInDOB started", TraceEventType.Start);

            var ddlSelectYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_YEAR).Id));
            var mySelect = new SelectElement(ddlSelectYear);
            mySelect.SelectByIndex(2);
            var yearInDDl = (mySelect.SelectedOption).Text;

            var yearInTextBox = (Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_TXTYEAROPTIONALINFO).Id))).GetAttribute("value");
            Assert.AreEqual(yearInDDl, yearInTextBox, "Year in selected DDL" + yearInDDl + "not equal to year in text Box" + yearInTextBox);
            CustomLogs.LogMessage("SelectYearInDOB completed", TraceEventType.Stop);
        }

        public void HouseholdAgeDropdownValues()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("VerifyYearInDOB started", TraceEventType.Start);
            var ddlSelectYear = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_DDLPERSON2AGE).Id));            
            var mySelect = new SelectElement(ddlSelectYear);
            //to count no of years in the list
            int selectOptions = mySelect.Options.Count;
            CustomLogs.LogInformation("Total selected option in the drop down list is " + selectOptions);
            //To select the first index element in the drop down
            mySelect.SelectByIndex(1);

            var startYearInDDl = (mySelect.SelectedOption).Text;
            CustomLogs.LogInformation("Start year in the drop down list " + startYearInDDl);
            mySelect.SelectByIndex(selectOptions-1);
            var endYearInDDl = (mySelect.SelectedOption).Text;
            CustomLogs.LogInformation("End year in the drop down list " + endYearInDDl);
            int currentYear = DateTime.Now.Year;
            int expectedEndYear = currentYear - 1;
            int expectedStartYear = currentYear - 99;
            Assert.AreEqual(expectedStartYear.ToString(), startYearInDDl.ToString(), "Expected start Year " + expectedStartYear + "not equal to start year in DDL" + startYearInDDl);
            Assert.AreEqual(expectedEndYear.ToString(), endYearInDDl.ToString(), "Expected end Year " + expectedEndYear + "not equal to end year in DDL" + endYearInDDl);
            CustomLogs.LogMessage("VerifyYearInDOB completed", TraceEventType.Stop);

        }

        public void verifyCheckBoxInDataProtection(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("verifyCheckBoxInDataProtection started", TraceEventType.Start);
                if (Generic.IsElementPresent((By.CssSelector(ObjAutomationHelper.GetControl(key).Id)), Driver))
                    CustomLogs.LogInformation("checkBox for" + key + " present");
                else
                    Assert.Fail("checkBox for" + key + " not present");
                CustomLogs.LogMessage("verifyCheckBoxInDataProtection completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
        }
        
        public void YourHouseholdAge_Titles()
        {
             Driver = ObjAutomationHelper.WebDriver;
            objGeneric.verifyValidationMessage(LabelKey.JOIN_AGE, ControlKeys.JOIN_LBLYOU, "", SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE);
            ReadOnlyCollection<IWebElement> lstPerson = (Driver.FindElements(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_LBLPERSON2).Id)));
            var expectedlblPerson2= AutomationHelper.GetResourceMessage(LabelKey.JOIN_PERSON2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actuallblPerson2 = lstPerson[0].Text;
            Assert.AreEqual(expectedlblPerson2, actuallblPerson2);
            var expectedlblPerson3 = AutomationHelper.GetResourceMessage(LabelKey.JOIN_PERSON3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actuallblPerson3 = lstPerson[1].Text;
            Assert.AreEqual(expectedlblPerson3, actuallblPerson3);
            var expectedlblPerson4 = AutomationHelper.GetResourceMessage(LabelKey.JOIN_PERSON4, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actuallblPerson4 = lstPerson[2].Text;
            Assert.AreEqual(expectedlblPerson4, actuallblPerson4);
            var expectedlblPerson5 = AutomationHelper.GetResourceMessage(LabelKey.JOIN_PERSON5, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actuallblPerson5 = lstPerson[3].Text;
            Assert.AreEqual(expectedlblPerson5, actuallblPerson5);
            var expectedlblPerson6 = AutomationHelper.GetResourceMessage(LabelKey.JOIN_PERSON6, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE)).Value;
            var actuallblPerson6 = lstPerson[4].Text;
            Assert.AreEqual(expectedlblPerson6, actuallblPerson6);
        }
        #endregion

        #region Method V2

        public void VerifyJoinPage()
        {
            try
            {
                CustomLogs.LogMessage("Verifying Join Page started ", TraceEventType.Start);                
                string JoinTitle =  (AutomationHelper.GetResourceMessage(LabelKey.JOIN_PAGE_TITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE))).Value;
                Assert.AreEqual(JoinTitle, ObjAutomationHelper.WebDriver.Title);
                CustomLogs.LogMessage("Varification of Join Page Title is Passes.", TraceEventType.Information);                
                CustomLogs.LogMessage("verifying Join Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                ObjAutomationHelper.WebDriver.Quit();
            }
        }

        public void verifyConfirmMessage()
        {
            try
            {
                CustomLogs.LogMessage("VerifyConfirmMessage Started ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                var expectedCongratsMsg = AutomationHelper.GetResourceMessage(LabelKey.JOIN_CONGRATSMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE)).Value;
                var expectedWelcomeMsg = (AutomationHelper.GetResourceMessage(LabelKey.JOIN_WELCOMEMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE))).Value;
                string actualWelcomeMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_WELCOMEMSG).Id)).Text;
                var actualCongratsMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.JOIN_CONGRATSMSG).Id)).Text;
                // Assert.AreEqual(expectedCongratsMsg, actualCongratsMsg, expectedCongratsMsg + " not equal to " + actualCongratsMsg);
                Assert.AreEqual(expectedWelcomeMsg, actualWelcomeMsg, expectedWelcomeMsg + " not equal to " + actualWelcomeMsg);
            }
            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }
            CustomLogs.LogMessage("VerifyConfirmMessage Completed ", TraceEventType.Stop);
        }

        public string EnterData(FieldType type)
        {
            string errorMsg = string.Empty;
            switch (type)
            {
                case FieldType.All:
                    errorMsg += Fill_All_Fields();
                    break;
                case FieldType.Mandatory:
                    errorMsg += Fill_Mandatory_Fields();
                    break;
            }            
            if (string.IsNullOrEmpty(errorMsg))
            {
                errorMsg += FindAddress();
                if (string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg += SelectAddress();
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        acceptLegalPolicy();                        
                    }
                }
            }
            return errorMsg;
        }

        public string Fill_Mandatory_Fields()
        {
            string error = string.Empty;
            List<Control> mandatoryControls = GetMandatoryControls();
            List<IWebElement> mandatoryWControls = GetWebElements(mandatoryControls);
            int index = 0;
            foreach (Control cControl in mandatoryControls)
            {                
                IWebElement wControl = mandatoryWControls[index++];
                if (wControl != null)
                {
                    string defaultValue = TestDataHelper.GetTestData(SanityConfiguration.TestDataFile,"TestData_JoinDetails", cControl.DataNode, SanityConfiguration.Domain);
                    switch (wControl.TagName.ToUpper())
                    {
                        case "INPUT":
                            string type = wControl.GetAttribute("type");
                            switch (type.ToUpper())
                            {
                                case "TEXT":
                                    if (string.IsNullOrEmpty(defaultValue))
                                    {
                                        defaultValue = "ABCD";
                                    }
                                    wControl.SendKeys(defaultValue);                                    
                                    break;
                                case "RADIO":
                                    wControl.SendKeys(Keys.Space);
                                    break;
                            }
                            break;
                        case "SELECT":
                            wControl.SendKeys(Keys.Down);                            
                            break;
                        case "DIV":
                            IWebElement ctrl = wControl.FindElements(By.TagName("input")).FirstOrDefault();
                            if (ctrl != null)
                            {
                                ctrl.Click();
                            }
                            else
                            {
                                error += string.Format(" Unable to find radio button for '{0}'. ", cControl.Key);
                            }
                            break;
                    }
                }
                else
                {
                    error += string.Format(" Unable to find control for '{0}'. ", cControl.Key);
                }
            }
            return error;
        }

        public string Fill_All_Fields()
        {
            CustomLogs.LogMessage("Fill_All_Fields started ", TraceEventType.Start);
            string error = string.Empty;
            List<Control> mandatoryControls = GetAllControls();
            CustomLogs.LogMessage(string.Format("allControls : {0}", mandatoryControls.Count), TraceEventType.Information);
            CustomLogs.LogMessage(string.Format("allControls not Null : {0}", mandatoryControls.FindAll(c => c != null).Count), TraceEventType.Information);
            List<IWebElement> mandatoryWControls = GetWebElements(mandatoryControls);
            CustomLogs.LogMessage(string.Format("allWebControls : {0}", mandatoryWControls.Count), TraceEventType.Information);
            CustomLogs.LogMessage(string.Format("allWebControls not Null : {0}", mandatoryWControls.FindAll(c => c != null).Count), TraceEventType.Information);
           
            int index = 0;
            foreach (Control cControl in mandatoryControls)
            {
                IWebElement wControl = mandatoryWControls[index++];
                if (wControl != null)
                {
                    string defaultValue = TestDataHelper.GetTestData(SanityConfiguration.TestDataFile, "TestData_JoinDetails", cControl.DataNode, SanityConfiguration.Domain);
                    switch (wControl.TagName.ToUpper())
                    {
                        case "INPUT":
                            string type = wControl.GetAttribute("type");
                            switch (type.ToUpper())
                            {
                                case "TEXT":
                                    if (string.IsNullOrEmpty(defaultValue))
                                    {
                                        defaultValue = "ABCD";
                                    }
                                    wControl.SendKeys(defaultValue);
                                    break;
                                case "RADIO":
                                    IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
                                    jse.ExecuteScript("arguments[0].click();", wControl);
                                    break;
                            }
                            break;
                        case "SELECT":
                            var mySelect = new SelectElement(wControl);
                            mySelect.SelectByIndex(1);
                             break;
                        case "DIV":
                            IWebElement ctrl = wControl.FindElements(By.TagName("input")).FirstOrDefault();
                            if (ctrl != null)
                            {
                                IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
                                jse.ExecuteScript("arguments[0].click();", ctrl);
                            }
                            else
                            {
                                error += string.Format(" Unable to find radio button for '{0}'. ", cControl.Key);
                            }
                            break;
                    }
                }
                else
                {
                    error += string.Format(" Unable to find control for '{0}'. ", cControl.Key);
                }
            }
            CustomLogs.LogMessage("Fill_All_Fields completed ", TraceEventType.Stop);
            return error;
        }

        public string Confirm()
        {
            string errorMsg = string.Empty;
            objGeneric.ClickElement(ControlKeys.JOIN_BTNCONFIRM, FindBy.CSS_SELECTOR_ID);
            ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            string JoinWelcome = (AutomationHelper.GetResourceMessage(LabelKey.JOIN_WELCOMEMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE))).Value;
            if (HasErrorDisplayed())
            {
                string error = GetErrors();
                string captchaError = GetCaptchaError();
                if (error.Trim().ToUpper().Equals(captchaError.Trim().ToUpper()))
                {
                    Assert.Inconclusive("Unable to Join as CAPTCHA is enabled, that cannot be automated.");
                }
                else
                {
                    errorMsg = error;
                }
            }
            else if (objGeneric.IsErrorPageDispalyed(JoinWelcome))
            {
                errorMsg = "There is some Server Error. User landed on Error page.";
            }
            return errorMsg;
        }

        public bool HasErrorDisplayed()
        {
            bool chk = false;
            Control cErrSummary = ObjAutomationHelper.GetControl(ControlKeys.VALIDATION_ERRORS);
            List<IWebElement> errs = objGeneric.GetWebControls(cErrSummary, FindBy.CSS_SELECTOR_ID);
            if (errs.Count > 0)
            {
                chk = true;
            }
            return chk;            
        }

        public string GetErrors()
        {
            StringBuilder error = new StringBuilder();
            Control cErrSummary = ObjAutomationHelper.GetControl(ControlKeys.VALIDATION_ERRORS);
            List<IWebElement> errs = objGeneric.GetWebControls(cErrSummary, FindBy.CSS_SELECTOR_ID);
            foreach (IWebElement ctrl in errs)
            {
                if (ctrl != null)
                {
                    error.Append(string.Format(" {0} ", ctrl.Text));
                }
            }
            return error.ToString();
        }

        public string FindAddress()
        {
            string errorMsg = string.Empty;
            DBConfiguration grpConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile);
            if (grpConfig.ConfigurationValue1.Equals("0"))
            {
                objGeneric.ClickElement(ControlKeys.JOIN_BTNPOSTCODE, FindBy.CSS_SELECTOR_ID);
                ObjAutomationHelper.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
                // check if error
                Control error = ObjAutomationHelper.GetControl(ControlKeys.JOIN_ERRORINVALIDPOSTCODE);
                IWebElement errorCtrl = objGeneric.GetWebControl(error, FindBy.CSS_SELECTOR_ID);
                if (errorCtrl != null)
                {
                    errorMsg += "Invalid Postcode.";
                }
            }
            return errorMsg;
        }

        public string SelectAddress()
        {
            string error = string.Empty;
            DBConfiguration grpConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Group_Config_Values, DBConfigKeys.GROUPCOUNTRYADDRESS, SanityConfiguration.DbConfigurationFile);
            if (grpConfig.ConfigurationValue1.Equals("0"))
            {
                Control chooseAddress = ObjAutomationHelper.GetControl(ControlKeys.JOIN_DDLADDRESS);
                IWebElement wChooseAddress = objGeneric.GetWebControl(chooseAddress, FindBy.CSS_SELECTOR_ID);
                if (chooseAddress != null)
                {
                    wChooseAddress.SendKeys(Keys.Down);
                }
                else
                {
                    error += string.Format(" Unable to find control for '{0}' ", chooseAddress.Key);
                }

                Control houseNo = ObjAutomationHelper.GetControl(ControlKeys.JOIN_HOUSENUMBER);
                IWebElement wHouseNo = objGeneric.GetWebControl(houseNo, FindBy.CSS_SELECTOR_ID);
                if (wHouseNo != null)
                {
                    wHouseNo.SendKeys("ABCD");
                }
                else
                {
                    error += string.Format(" Unable to find control for '{0}' ", houseNo.Key);
                }
            }
            return error;
        }

        public void acceptLegalPolicy()
        {
            try
            {
                CustomLogs.LogMessage("acceptLegalPolicy started ", TraceEventType.Start);
                Driver = ObjAutomationHelper.WebDriver;
                bool isLegalPolicyVisible = objGeneric.IscontrolVisible(DBConfigKeys.HIDELEGALPOLICY);
                if (isLegalPolicyVisible)
                {
                    objGeneric.ClickElementJavaElement(ControlKeys.JOIN_TERMSANDCONDITION,"Legal Policy");
                    objGeneric.ClickElementJavaElement(ControlKeys.JOIN_PRIVACYPOLICY, "Legal Policy");
                    CustomLogs.LogMessage("acceptLegalPolicy completed ", TraceEventType.Stop);
                }
                else
                {
                    Assert.AreEqual(isLegalPolicyVisible.ToString().ToUpper(), "FALSE", "Value doesn't match with DBconfig");
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

        public string GetProfaneError()
        {
            string error = string.Empty;
            Resource profaneError = AutomationHelper.GetResourceMessage(ValidationKey.ERRORPROFANE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
            error = profaneError != null ? profaneError.Value : string.Empty;
            return error;
        }

        public string GetDuplicateError()
        {
            string error = string.Empty;
            Resource dupError = AutomationHelper.GetResourceMessage(ValidationKey.ERRORDUPLICACY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
            error = dupError != null ? dupError.Value : string.Empty;
            return error;
        }

        public string EnterDataInField(string key, string value)
        {
            string isDataEntered = "Control not found";
            try
            {
                CustomLogs.LogMessage("EnterDataInField started", TraceEventType.Start);
                Control ctrl = ObjAutomationHelper.GetControl(key);
                IWebElement wCtrl = objGeneric.GetWebControl(ctrl, FindBy.CSS_SELECTOR_ID);
                if (wCtrl != null)
                {
                    wCtrl.Clear();
                    wCtrl.SendKeys(value);
                    isDataEntered = string.Empty;
                }
                CustomLogs.LogMessage("VerifyTextExistOnPage completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return isDataEntered;
        }

        public string ValidateHouseHoldTest(IWebDriver Driver)
        {
            string errorMessage = string.Empty;
            string actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.FIELDSET_HOUSEHOLD_DETAILS).Id)).Text;

            Resource res = new Resource();
            res = AutomationHelper.GetResourceMessage(ValidationKey.PD_YOURHOUSEHOLDDETAILSTITLE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE));            
            string expectedText = res != null ? res.Value : string.Empty;
            if (!actualText.Trim().Equals(expectedText.Trim()))
            {
                errorMessage += string.Format("Legend did not match. Actual: '{0}', Expected: '{1}' ", actualText, expectedText);
            }

            actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.HOUSEHOLDINFO).Id)).Text;

            // paragarph 1
            res = AutomationHelper.GetResourceMessage(LabelKey.LBL_HOUSEHOLD_DETAILS_P1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE));            
            expectedText = res != null ? res.Value : string.Empty;

            if (!string.IsNullOrWhiteSpace(actualText))
            {             
                if (!actualText.Trim().Equals(expectedText.Trim()))
                {
                    errorMessage += string.Format("Legend did not match. Actual: '{0}', Expected: '{1}' ", actualText, expectedText);
                }
            }
            else 
            {
                errorMessage += string.Format("Control for label '{0}' not found.", expectedText);
            }

            // paragarph 2

            actualText = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.ENTERAGE).Id)).Text;

            res = AutomationHelper.GetResourceMessage(LabelKey.LBL_HOUSEHOLD_DETAILS_P2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.PERSONALDETAILS_RESOURCE));            
            expectedText = res != null ? res.Value : string.Empty;

            if (!string.IsNullOrWhiteSpace(actualText))
            {
                if (!actualText.Trim().Equals(expectedText.Trim()))
                {
                    errorMessage += string.Format("Legend did not match. Actual: '{0}', Expected: '{1}' ", actualText, expectedText);
                }
            }
            else
            {
                errorMessage += string.Format("Control for label '{0}' not found.", expectedText);
            }
            return errorMessage;
        }

        public string SelectMarketingPreferences(SecurityPreference contactPreference)
        {
            string error = string.Empty;
            bool isVisible= false;
            string chkBoxID = string.Empty;
            List<PreferencesUIConfig> prefUI = new List<PreferencesUIConfig>();
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            bool isEnabled = prefClient.IsContactPreferenceEnabled(Login.CustomerID, (int)contactPreference);
            DBConfiguration preferenceUIConfig = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.PREFERENCEUICONFIGURATION, SanityConfiguration.DbConfigurationFile);
            prefUI.AddRange(preferenceUIConfig.ConfigurationValue1.JsonToObject<List<PreferencesUIConfig>>());
            PreferencesUIConfig cConf = prefUI.Find(p => p.preferenceid ==(int)contactPreference);
            isVisible = (cConf != null && cConf.isvisible);
            if (isEnabled && isVisible)
            {
                DBConfiguration optInBehavior = AutomationHelper.GetDBConfiguration(ConfugurationTypeEnum.Webconfiguration, DBConfigKeys.ISOPTINBEHAVIOUR, SanityConfiguration.DbConfigurationFile);
                bool isOptinBehavior = optInBehavior.ConfigurationValue1.ToUpper().Equals("FALSE");
                switch (contactPreference)
                {
                    case SecurityPreference.Tesco_Products:
                        chkBoxID = isOptinBehavior ? ControlKeys.CHKTESCOPRODUCT : ControlKeys.OPTOUT_CHKTESCOPRODUCT;
                        break;
                    case SecurityPreference.Tesco_Partners:
                        chkBoxID = isOptinBehavior ? ControlKeys.CHKPARTNEROFFER : ControlKeys.OPTOUT_CHKPARTNEROFFER;
                        break;
                    case SecurityPreference.Customer_Research:
                        chkBoxID = isOptinBehavior ? ControlKeys.CHKRESEARCH : ControlKeys.OPTOUT_CHKRESEARCH;
                        break;
                }
                Control chk = ObjAutomationHelper.GetControl(chkBoxID);
                By queryChk = By.CssSelector(chk.Id);
                IWebElement eChk = ObjAutomationHelper.WebDriver.FindElement(queryChk);
                if (eChk != null)
                {
                    IJavaScriptExecutor jse = (IJavaScriptExecutor)ObjAutomationHelper.WebDriver;
                    jse.ExecuteScript("arguments[0].click();", eChk);
                    //eChk.SendKeys(Keys.Space);
                }
                else
                {
                    error += string.Format("Check box for preference {0} not found", contactPreference.ToString());
                }
            }
            return error;
        }

        /// <summary>
        /// Method to select the House Hold Details dropdown controls
        /// </summary>
        public void selectddl()
        {
            Driver = ObjAutomationHelper.WebDriver;
            CustomLogs.LogMessage("selectddl started", TraceEventType.Start);
            Control ddlAges = ObjAutomationHelper.GetControl(ControlKeys.JOIN_DDLPERSON2AGE);
            List<IWebElement> lstAgeDropDown = objGeneric.GetWebControls(ddlAges, FindBy.CSS_SELECTOR_ID);
            foreach (IWebElement ele in lstAgeDropDown)
            {
                ele.SendKeys(Keys.Down);
            }
            CustomLogs.LogMessage("selectddl completed", TraceEventType.Stop);
        }

        #region Private

        List<Control> GetAllControls()
        {
            List<Control> cControls = new List<Control>();
            List<DBConfiguration> configEnable = new List<DBConfiguration>();
            cControls = ObjAutomationHelper.GetControls(ControlKeys.JOIN);
            configEnable = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.ChinaHiddenFunctionality, SanityConfiguration.DbConfigurationFile);
            configEnable = configEnable.FindAll(f => !f.ConfigurationValue1.Equals("1"));
            cControls = (from t in cControls
                         where configEnable.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                         select t).ToList();
            return cControls;
        }

        List<Control> GetMandatoryControls()
        {
            List<Control> cControls = new List<Control>();
            List<Control> cControls1 = new List<Control>();
            List<DBConfiguration> configMandatory = new List<DBConfiguration>();
            List<DBConfiguration> joinConfigMandatory = new List<DBConfiguration>();
            cControls = ObjAutomationHelper.GetControls(ControlKeys.JOIN);
            configMandatory = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.Mandatory_fields, SanityConfiguration.DbConfigurationFile);
            joinConfigMandatory = AutomationHelper.GetDBConfigurations(ConfugurationTypeEnum.JoinEmailMandatory, SanityConfiguration.DbConfigurationFile);          
            configMandatory = configMandatory.FindAll(f => f.ConfigurationValue1.Equals("1"));
            joinConfigMandatory = joinConfigMandatory.FindAll(f => f.ConfigurationValue1.Equals("1"));
            cControls = (from t in cControls
                         where configMandatory.FindAll(f => t.DBConfigurations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Contains(f.ConfigurationName)).Count > 0
                         select t  ).ToList();
            List<Control> enableControls = GetAllControls();
            cControls = cControls.Intersect(enableControls).ToList();
            return cControls;
        }

        List<IWebElement> GetWebElements(List<Control> cControls)
        {
            List<IWebElement> wControls = new List<IWebElement>();
            foreach (Control c in cControls)
            {
                if (!string.IsNullOrEmpty(c.Id))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_ID));
                }
                else if (!string.IsNullOrEmpty(c.ClassName))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.CSS_SELECTOR_CSS));
                }
                else if (!string.IsNullOrEmpty(c.XPath))
                {
                    wControls.Add(objGeneric.GetWebControl(c, FindBy.XPATH_SELECTOR));
                }
            }
            return wControls;
        }

        internal string GetCaptchaError()
        {
            string error = string.Empty;
            Resource captchaError = AutomationHelper.GetResourceMessage(ValidationKey.CAPTCHA_ERROR, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
            Resource errorSummary = AutomationHelper.GetResourceMessage(ValidationKey.ERROR_SUMMARY, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.JOIN_RESOURCE));
            error = string.Format("{0}  {1}", errorSummary != null ? errorSummary.Value : string.Empty, captchaError != null ? captchaError.Value : string.Empty);
            return error;
        }

        #endregion

        #endregion        
    }
}
