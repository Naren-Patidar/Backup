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
using Tesco.Framework.UITesting.Constants;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class OptionsAndBenefits : Base
    {
        Generic objGeneric = null;
        #region Constructor

        public OptionsAndBenefits(AutomationHelper objHelper, AppConfiguration configuration)
        {
            ObjAutomationHelper = objHelper;            
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get the selected option preference from the redio button control on UI
        /// it will check for all radio buttons and will return the selected option
        /// </summary>
        /// <returns></returns>
        private OptionPreference getActualOption()
        {
            OptionPreference option = OptionPreference.None;
            string[] options = new string[] { ControlKeys.OPTIONSBENEFIT_XMASSAVER, ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN, ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN, ControlKeys.OPTIONSBENEFIT_VIRGIN };
            foreach (string o in options)
            {
                Control radioBtn = ObjAutomationHelper.GetControl(o);
                By byRadioBtn = By.CssSelector(radioBtn.Id);
                IWebElement eRadioBtn = ObjAutomationHelper.WebDriver.FindElement(byRadioBtn);
                if (eRadioBtn.Selected)
                {
                    int optionId = 0;
                    string strOptionId = eRadioBtn.GetAttribute("value");
                    if (Int32.TryParse(strOptionId, out optionId))
                    {
                        option = (OptionPreference)optionId;
                    }
                }
            }
            return option;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to validate that the given preference is selected on UI
        /// from the hidden control (there is hidden control for selected preference id)
        /// also from the radio buttons
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public bool ValidateSelectedOption(OptionPreference option)
        {
            bool chk = true;
            OptionPreference actualOption = OptionPreference.None;
            try
            {
                // check from hidden value
                Control hdnSelectedOption = ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_SELECTED_OPTION);
                By byHdnSelectedOption = By.CssSelector(hdnSelectedOption.Id);
                string selectedOption = ObjAutomationHelper.WebDriver.FindElement(byHdnSelectedOption).GetAttribute("value");
                int intSelectedOption = 0;
                if (Int32.TryParse(selectedOption, out intSelectedOption))
                {
                    actualOption = (OptionPreference)intSelectedOption;
                    Assert.AreEqual(option, actualOption, "Failed in matching hidden value");
                }
                // check from radio button is selected
                actualOption = getActualOption();
                Assert.AreEqual(option, actualOption, "Failed in matching radio button value");
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            return chk;
        }        

        /// <summary>
        /// method to select the radio button control based on the given option preference
        /// </summary>
        /// <param name="optionPreference"></param>
        public void SelectOption(OptionPreference optionPreference)
        {
            Control ControlTab = ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_OPTIONS_TAB);
            By byTab = By.CssSelector(ControlTab.Id);
            IWebElement eTab = ObjAutomationHelper.WebDriver.FindElement(byTab);
            IWebElement eTab_Header = null;
            Generic objGeneric = new Generic(ObjAutomationHelper);
            switch (optionPreference)
            {
                case OptionPreference.Xmas_Saver:
                    //eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("id").Equals("0"));
                    ////eTab_Header = eTab.FindElement(By.XPath("//input[@tabIndex='0']"));
                    //eTab_Header.Click();
                    //ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_XMASSAVER).Id)).Click();
                    objGeneric.ClickElement(ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_XMASSAVER).Id)).GetAttribute("id"), FindBy.ID);               
                    break;
                case OptionPreference.Airmiles_Standard:
                    //eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("id").Equals("1"));
                    //eTab_Header.Click();
                    //ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN).Id)).Click();
                    objGeneric.ClickElement(ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN).Id)).GetAttribute("id"), FindBy.ID);               
                    break;
                case OptionPreference.BA_Miles_Standard:
                    //eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("id").Equals("3"));
                    //eTab_Header.Click();
                    //ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id)).Click();
                    objGeneric.ClickElement(ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id)).GetAttribute("id"), FindBy.ID);               

                    break;
                case OptionPreference.Virgin_Atlantic:
                    //eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("id").Equals("2"));
                    //eTab_Header.Click();
                    //ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VIRGIN).Id)).Click();
                    objGeneric.ClickElement(ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VIRGIN).Id)).GetAttribute("id"), FindBy.ID);                 
                    break;
                case OptionPreference.None:                   
                   // ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VOUCHERS).Id)).GetAttribute("id");
                    objGeneric.ClickElement(ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_VOUCHERS).Id)).GetAttribute("id"), FindBy.ID);                 
                    break;   
            }
        }

        /// <summary>
        /// method to enter any random values in membership id field
        /// </summary>
        /// <param name="optionPreference">preference type</param>
        public void SetMembershipId(OptionPreference optionPreference)
        {
            string txtID = string.Empty;
            string val = string.Empty;
            switch (optionPreference)
            {
                case OptionPreference.BA_Miles_Standard:
                    txtID = ControlKeys.OPTIONSBENEFIT_BA_MEMBERSHIPID;
                    val = "01234567";
                    break;
                case OptionPreference.Virgin_Atlantic:
                    txtID = ControlKeys.OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID;
                    val = "00121212120";
                    break;
                case OptionPreference.Airmiles_Standard:
                    txtID = ControlKeys.OPTIONSBENEFIT_AVIOS_MEMBERSHIPID;
                    val = "3081472229120214";
                    break;
            }
            Control txtMembershipId = ObjAutomationHelper.GetControl(txtID);
            By byTxtMembershipId = By.CssSelector(txtMembershipId.Id);
            IWebElement etxtMembershipId = ObjAutomationHelper.WebDriver.FindElement(byTxtMembershipId);
          // etxtMembershipId.Click();
            etxtMembershipId.SendKeys(val);
        }

        /// <summary>
        /// Method to check if given customer is Xmas Saver customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsXmasSaverOpted(string customerId)
        {
            bool status = false;
            PreferenceServiceAdaptor prefClient = new PreferenceServiceAdaptor();
            status = prefClient.IsContactPreferenceOpted(Login.CustomerID, (int)OptionPreference.Xmas_Saver);
            return status;
        }

        public void VerifyText2(string msgId, string msgId1, string keys, string LocalResourceFileName, int no ,FindBy query)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedMessage = string.Empty;
                string expectedMessage1 = string.Empty;
                IWebElement ctrl = null;

                CustomLogs.LogMessage("Verifying validation message for post section started", TraceEventType.Start);

                switch (no)
                {
                    case 1:
                        //expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        //expectedMessage = string.Concat(expectedMessage, " ", address1);
                        break;
                    case 2:
                        expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                        expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;

                        expectedMessage = string.Concat(expectedMessage, " ", expectedMessage1);
                        break;
                }

                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                        break;                                             
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName));
                        break;
                }
                var actualMessage = ctrl.Text;
                Assert.AreEqual(expectedMessage, actualMessage, " not verified");
                CustomLogs.LogMessage("Verifying validation message for post section completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public string VerifyText(string msgId, string msgId1, string keys, string LocalResourceFileName, FindBy query ,string rate , int no)
        {
            string error = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string expectedMessage = string.Empty;
                string expectedMessage1 = string.Empty;
                IWebElement ctrl = null;               

                CustomLogs.LogMessage("Verifying validation message started", TraceEventType.Start);
                
                expectedMessage = AutomationHelper.GetResourceMessage(msgId, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;
                expectedMessage1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, LocalResourceFileName)).Value;

                switch (no)
                {
                    case 1:
                        expectedMessage = string.Concat(expectedMessage, " ", expectedMessage1);
                        break;
                    case 2:
                        expectedMessage = string.Concat(expectedMessage, expectedMessage1);
                        break;
                }
                string expectedMe = expectedMessage.Replace("{0}",rate );
                expectedMessage = expectedMe.Replace("{1}", BusinessCons.PRICE);
                switch (query)
                {
                    case FindBy.XPATH_SELECTOR:
                        ctrl = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(keys).XPath));
                        break;
                    case FindBy.CSS_SELECTOR_ID:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).Id));
                        break;
                    case FindBy.CSS_SELECTOR_CSS:
                        ctrl = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(keys).ClassName));
                        break;
                }
                var actualMessage = ctrl.Text;
                Assert.AreEqual(expectedMessage, actualMessage, "not matching");
                CustomLogs.LogMessage("Verifying validation message completed", TraceEventType.Stop);
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

        /// <summary>
        /// method to enter any random values in membership id field
        /// </summary>
        /// <param name="optionPreference">preference type</param>
        public void SetIncorrectMembershipId(OptionPreference optionPreference)
        {
            string txtID = string.Empty;
            string val = string.Empty;
            switch (optionPreference)
            {
                case OptionPreference.BA_Miles_Standard:
                    txtID = ControlKeys.OPTIONSBENEFIT_BA_MEMBERSHIPID;
                    val = "sw_ .,/&";
                    break;
                case OptionPreference.Virgin_Atlantic:
                    txtID = ControlKeys.OPTIONSBENEFIT_VIRGIN_MEMBERSHIPID;
                    val = ",./76*  21w";
                    break;
                case OptionPreference.Airmiles_Standard:
                    txtID = ControlKeys.OPTIONSBENEFIT_AVIOS_MEMBERSHIPID;
                    val = "sdss  %$#@.,  df";
                    break;
            }
            Control txtMembershipId = ObjAutomationHelper.GetControl(txtID);
            By byTxtMembershipId = By.CssSelector(txtMembershipId.Id);
            IWebElement etxtMembershipId = ObjAutomationHelper.WebDriver.FindElement(byTxtMembershipId);
            etxtMembershipId.Click();
            etxtMembershipId.SendKeys(val);
        }

        /// <summary>
        /// method to validate membership ID field for Virgin as per alogorithm
        /// </summary>
        /// <param name="optionPreference">preference type</param>
        public string GenerateTenDigitVirginMembershipId()
        {
            Random rnd = new Random();
            int oddnumber = rnd.Next(1, 4);
            int evennumber = rnd.Next(1, 4);
            while (oddnumber == evennumber)
            {
                evennumber = rnd.Next(1, 9);
                continue;
            }
             string newNumber=string.Empty;
             int index = default(int);
             double result = default(double);
             double roundedTotal = default(double);
            for (int i = 0; i <= 4; i++)
            {
                newNumber = newNumber + oddnumber.ToString() + evennumber.ToString();
            }
            char[] detailsChars = newNumber.ToString().Substring(0, 9).ToCharArray();
             index = 0;
           while (index <= detailsChars.Length - 1)
            {
                result = result + double.Parse(detailsChars[index].ToString());
                index += 2;
            }
           result = result*2;
           index = 1;
           while (index <= detailsChars.Length - 1)
           {
               result = result + double.Parse(detailsChars[index].ToString()); 
               index+=2;
           }
           roundedTotal = Math.Ceiling(result / 10) * 10;
           //Subtract the calculated total from the rounded total
           double checksum = roundedTotal - result;
           //Output after subtraction should be same as check digit i.e. 10th index
           if (double.Parse(newNumber[9].ToString()) != checksum)
           {
              
               StringBuilder sb = new StringBuilder(newNumber);
               sb[9] = Convert.ToChar(checksum.ToString());
               newNumber = sb.ToString();
               
           }
           return newNumber;
        }

        /// <summary>
        /// method to validate membership ID field for Virgin as per alogorithm
        /// </summary>
        /// <param name="optionPreference">preference type</param>
        public string GenerateElevenDigitVirginMembershipId()
        {
            Random rnd = new Random();
            int oddnumber = rnd.Next(1, 4);
            int evennumber = rnd.Next(1, 4);
            while (oddnumber == evennumber)
            {
                evennumber = rnd.Next(1, 9);
                continue;
            }
            string newNumber = "00";
            int index = default(int);
            double result = default(double);
            double roundedTotal = default(double);
            for (int i = 0; i <= 3; i++)
            {
                newNumber = newNumber + oddnumber.ToString() + evennumber.ToString();
            }
            newNumber = newNumber + oddnumber.ToString();
            char[] detailsChars = newNumber.ToString().Substring(0, 10).ToCharArray();
            index = 1;
            while (index <= detailsChars.Length - 1)
            {
                result = result + double.Parse(detailsChars[index].ToString());
                index += 2;
            }
            result = result * 2;
            index = 0;
            while (index <= detailsChars.Length - 1)
            {
                result = result + double.Parse(detailsChars[index].ToString());
                index += 2;
            }
            roundedTotal = Math.Ceiling(result / 10) * 10;
            //Subtract the calculated total from the rounded total
            double checksum = roundedTotal - result;
            
            //Output after subtraction should be same as check digit i.e. 10th index
            if (double.Parse(newNumber[10].ToString()) != checksum)
            {
                StringBuilder sb = new StringBuilder(newNumber);
                sb[10] = Convert.ToChar(checksum.ToString());
                newNumber = sb.ToString();
               
            }
            return newNumber;
        }

        public bool VerfiyRadioButtonBehaviour(string radiobtncontrolkey,string memberIDControlKey)
        {
            try
            {
                    Control radioBtn = ObjAutomationHelper.GetControl(radiobtncontrolkey);
                    By byRadioBtn = By.CssSelector(radioBtn.Id);
                    IWebElement eRadioBtn = ObjAutomationHelper.WebDriver.FindElement(byRadioBtn);
                    Generic objGeneric = new Generic(ObjAutomationHelper);
                    Driver = ObjAutomationHelper.WebDriver;
                    string divMemershipIDBlockStyle = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(memberIDControlKey).Id)).GetAttribute("style");

                    if(!eRadioBtn.Selected)
                    {
                        if (divMemershipIDBlockStyle.Equals("display: none;"))
                        {
                            ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(radiobtncontrolkey).Id)).Click();
                            divMemershipIDBlockStyle = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(memberIDControlKey).Id)).GetAttribute("style");
                            if (string.IsNullOrEmpty(divMemershipIDBlockStyle) || divMemershipIDBlockStyle.Equals("display: block;"))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to select the radio button control based on the given option preference
        /// </summary>
        /// <param name="optionPreference"></param>
        public void SelectTab(OptionPreference optionPreference)
        {
            Control ControlTab = ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_OPTIONS_TAB);
            By byTab = By.CssSelector(ControlTab.Id);
            IWebElement eTab = ObjAutomationHelper.WebDriver.FindElement(byTab);
            IWebElement eTab_Header = null;
            switch (optionPreference)
            {
                case OptionPreference.Airmiles_Standard:
                    eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("tabIndex").Equals("1"));
                    eTab_Header.Click();
                    break;
                case OptionPreference.BA_Miles_Standard:
                    eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("tabIndex").Equals("3"));
                    eTab_Header.Click();
                    break;
                case OptionPreference.Virgin_Atlantic:
                    eTab_Header = eTab.FindElements(By.TagName("input")).ToList().Find(c => c.GetAttribute("tabIndex").Equals("2"));
                    eTab_Header.Click();
                    break;
            }
        }

        public string verifyChristamsSaverText()
        {
                Driver = ObjAutomationHelper.WebDriver;
                string error = string.Empty;
                var expectedLine1 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_CHRISTMASDESCRIPTION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedLine2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_XMASDESC, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedMsg1 = string.Concat(expectedLine1, " ", expectedLine2);
                var expectedLImpLabel = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_IMPORTANT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedLImpText = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedImpMsg = string.Concat(expectedLImpLabel, " ", expectedLImpText);

                var actualMsg1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTCSMSG).Id)).Text;
                var actualImpMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTIMPMSG).Id)).Text;

                error=objGeneric.ValidateResourceValueWithHTMLContent(actualMsg1, expectedMsg1);
                error = error + objGeneric.ValidateResourceValueWithHTMLContent(actualImpMsg, expectedImpMsg);
                return error;
        }

        public string verifyAviosText()
        {
            string error = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var expectedAviosDesc1 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedAviosDesc2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSOPTEDOUT1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedAviosOptedout1 = string.Concat(expectedAviosDesc1, " ", expectedAviosDesc2);
                string expectedMe = expectedAviosOptedout1.Replace("{0}", BusinessCons.AVIOSRATE);
                expectedAviosOptedout1 = expectedMe.Replace("{1}", BusinessCons.PRICE);

                string expectedMoreInfo = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_MOREINFORMATION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string schemeName= AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOS, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                expectedMoreInfo = expectedMoreInfo.Replace("{0}", schemeName);

                string expectedAviosOptedout2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string expectedAviosClubMembership = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESC1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                expectedAviosOptedout2 = string.Concat(expectedAviosOptedout2, " ", expectedAviosClubMembership);

                string expectedAviosEnsureMsg = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_ENSUREMSG, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;

                var expectedLImpLabel = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_IMPORTANT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedLImpText = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedImpMsg = string.Concat(expectedLImpLabel, " ", expectedLImpText);

                var actualAviosOptedout1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTAVIOSDESC1).Id)).Text;
                var actualMoreInfo = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTMOREINFO).XPath)).Text;
                var actualAviosOptedout2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTAVIOSOPTEDOUT2).Id)).Text;

                var actualAviosEnsureMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTENSUREMSG).Id)).Text;
                var actualImpMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTIMPMSG).Id)).Text;

                Assert.AreEqual(expectedAviosOptedout1, actualAviosOptedout1);
                Assert.AreEqual(expectedMoreInfo, actualMoreInfo);
                error=objGeneric.ValidateResourceValueWithHTMLContent(actualAviosOptedout2,expectedAviosOptedout2);
                Assert.AreEqual(expectedAviosEnsureMsg, actualAviosEnsureMsg);
                Assert.AreEqual(expectedImpMsg, actualImpMsg);



                
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

        public void verifyVirginAtlanticText()
        {
           try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var expectedVADesc1 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINATLANTICDESC1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedVADesc2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINOPTEDOUT1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedVAOptedout1 = string.Concat(expectedVADesc1, " ", expectedVADesc2);
                string expectedMe = expectedVAOptedout1.Replace("{0}", BusinessCons.VIRGINRATE);
                expectedVAOptedout1 = expectedMe.Replace("{1}", BusinessCons.PRICE);

                string expectedMoreInfo = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_MOREINFORMATION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string schemeName = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINATLANTIC, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                expectedMoreInfo = expectedMoreInfo.Replace("{0}", schemeName);

                string expectedVAOptedout2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINATLANTICDESC3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string expectedVAClubMembership = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINATLANTICDESC2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;

                string expectedVAEnsureMsg = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_VIRGINOPTEDOUT3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;

                var expectedLImpLabel = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_IMPORTANT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedLImpText = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedImpMsg = string.Concat(expectedLImpLabel, " ", expectedLImpText);

                var actualVAOptedout1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTVADESC1).Id)).Text;
                var actualMoreInfo = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTVAMOREINFO).XPath)).Text;
                var actualVAOptedout2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTVADESC3).Id)).Text;
                var actualVAClubMembership = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTVACLUBMEMBER).XPath)).Text;
                var actualVAEnsureMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTVAENSUREMSG).Id)).Text;
                var actualImpMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTIMPMSG).Id)).Text;

                Assert.AreEqual(expectedVAOptedout1, actualVAOptedout1);
                Assert.AreEqual(expectedMoreInfo, actualMoreInfo);
                Assert.AreEqual(expectedVAOptedout2, actualVAOptedout2);
                Assert.AreEqual(expectedVAClubMembership, actualVAClubMembership);
                Assert.AreEqual(expectedVAEnsureMsg, actualVAEnsureMsg);
                Assert.AreEqual(expectedImpMsg, actualImpMsg);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void verifyBAAviosText()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                var expectedBAAviosDesc1 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAMILESDESC1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedBAAviosDesc2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAOPTEDOUT1, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedBAAviosOptedout1 = string.Concat(expectedBAAviosDesc1, " ", expectedBAAviosDesc2);
                string expectedMe = expectedBAAviosOptedout1.Replace("{0}", BusinessCons.AVIOSRATE);
                expectedBAAviosOptedout1 = expectedMe.Replace("{1}", BusinessCons.PRICE);

                string expectedMoreInfo = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_MOREINFORMATION, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string schemeName = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAMILES, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                expectedMoreInfo = expectedMoreInfo.Replace("{0}", schemeName);

                string expectedBAAviosOptedout2 = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAMILESDESC3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                string expectedBAAviosClubMembership = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAMILESDESC2, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;

                string expectedBAAviosEnsureMsg = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_BAOPTEDOUT3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;

                var expectedLImpLabel = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_IMPORTANT, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedLImpText = AutomationHelper.GetResourceMessage(LabelKey.OPTIONANDBENIFITS_AVIOSDESCRIPTION3, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.OPTIONANDBENEFIT_RESOURCE)).Value;
                var expectedImpMsg = string.Concat(expectedLImpLabel, " ", expectedLImpText);

                var actualBAAviosOptedout1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOSDESC1).Id)).Text;
                var actualMoreInfo = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOSMOREINFO).XPath)).Text;
                var actualBAAviosOptedout2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOSDESC3).Id)).Text;
                var actualBAAviosClubMembership = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOSCLUBMEMBER).XPath)).Text;
                var actualBAAviosEnsureMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTBAAVIOSENSUREMSG).Id)).Text;
                var actualImpMsg = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTIMPMSG).Id)).Text;

                Assert.AreEqual(expectedBAAviosOptedout1, actualBAAviosOptedout1);
                Assert.AreEqual(expectedMoreInfo, actualMoreInfo);
                Assert.AreEqual(expectedBAAviosOptedout2, actualBAAviosOptedout2);
                Assert.AreEqual(expectedBAAviosClubMembership, actualBAAviosClubMembership);
                Assert.AreEqual(expectedBAAviosEnsureMsg, actualBAAviosEnsureMsg);
                Assert.AreEqual(expectedImpMsg, actualImpMsg);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void SetEmail(OptionPreference optionPreference)
        {
            string email = string.Format("{0}@test.com", new Guid());
            string val = string.Empty;
            switch (optionPreference)
            {
                case OptionPreference.BA_Miles_Standard:
                case OptionPreference.Virgin_Atlantic:
                case OptionPreference.Airmiles_Standard:
                    if (objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTEMAIL).Id)))
                    {
                        Control txtemail = ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTEMAIL);
                        IWebElement etxtemail = ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(txtemail.Id));
                        Control txtconfirmemail = ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTCONFIRMEMAIL);
                        IWebElement etxtconfirmemail = ObjAutomationHelper.WebDriver.FindElement(By.CssSelector(txtconfirmemail.Id));
                        etxtemail.SendKeys(email);
                        etxtconfirmemail.SendKeys(email);
                    }
                    break;
            }            
        }

        public void checkEmail(OptionPreference optionPreference)
        {
            string email = string.Format("{0}@test.com", new Guid());
            string val = string.Empty;
            switch (optionPreference)
            {
                case OptionPreference.BA_Miles_Standard:
                case OptionPreference.Virgin_Atlantic:
                case OptionPreference.Airmiles_Standard:
                    CustomerServiceAdaptor objCustService = new CustomerServiceAdaptor();
                    Dictionary<string, string> CustomerDetail = objCustService.GetCustomerDetails(Login.CustomerID.ToString(), CountrySetting.culture);
                    if (CustomerDetail.ContainsKey("email_address") && string.IsNullOrEmpty(CustomerDetail["email_address"]))
                    {
                        if (!objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_TXTEMAIL).Id)))
                        {
                            Assert.Fail("Email field is not visible on selecting Miles preference.");
                        }
                    }
                    else
                    {
                        if (!objGeneric.IsElementPresentOnPage(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_CHANGEEMAIL).Id)))
                        {
                            Assert.Fail("Email field is not visible on selecting Miles preference.");
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
