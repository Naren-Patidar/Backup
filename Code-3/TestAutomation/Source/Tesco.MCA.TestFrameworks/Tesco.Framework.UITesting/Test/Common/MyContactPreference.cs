using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Tesco.Framework.UITesting.Services;

namespace Tesco.Framework.UITesting.Test.Common
{

    class MyContactPreference : Base
    {        
        Generic objGeneric = null;
        List<IWebElement> el = new List<IWebElement>();
        List<int> checkBoxNumber = new List<int>();
        List<int> totalCheckBox = new List<int>();
        string value = string.Empty;
        IWebElement chkBoxElement = null;
        #region Constructor


        public MyContactPreference(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        public MyContactPreference(AutomationHelper objHelper, AppConfiguration configuration, TestData_Activation testData)
        {
            ObjAutomationHelper = objHelper;
            //Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.Login);
            // this.testData = testData;
            SanityConfiguration = configuration;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion

        
        public void ContactPreferences_Email()
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Email opted verification", TraceEventType.Start);
                IWebElement radioButtonEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_EMAIL).Id));
                Assert.AreEqual(radioButtonEmail.Enabled, true, "ContactPreferences is Not selected as email");
            }

            catch (Exception ex)
            {
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                CustomLogs.LogException(ex);
                Driver.Quit();
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
            }



        }

        public void ContactPreferences_EmailText(string Clubcard)
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Email Address verification", TraceEventType.Start);
                IWebElement radioButtonEmail = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_EMAIL).Id));
                if (radioButtonEmail.Selected == true)
                {

                    CustomerServiceAdaptor customer = new CustomerServiceAdaptor();

                    string customerId = customer.GetCustomerID(Clubcard, CountrySetting.culture).ToString();
                    string emailid = customer.GetEmailId(Clubcard, CountrySetting.culture);
                    var EmailAddress = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.EMAILTEXT_VARIFICATION).Id)).GetAttribute("value");
                    Assert.AreEqual(emailid, EmailAddress, "Email address is not verified");

                }
                else
                {
                    CustomLogs.LogMessage("Email is not selected as preference", TraceEventType.Start);
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

        public void ContactPreferences_Phonenumber(string Clubcard)
        {
            try
            {

                string culture = CountrySetting.country;
                if (culture == "SK" || culture == "CZ")
                {
                    CustomLogs.LogMessage("Phone Number is not Applicable for this country preference", TraceEventType.Start);
                }

                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Email Address verification", TraceEventType.Start);
                    IWebElement radioButtonPhoneNumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.RADIOBUTTON_SMS).Id));
                    if (radioButtonPhoneNumber.Selected == true)
                    {

                        CustomerServiceAdaptor customer = new CustomerServiceAdaptor();
                        string customerId = customer.GetCustomerID(Clubcard, CountrySetting.culture).ToString();
                        string phoneNumber = customer.GetPhoneNumber(customerId, CountrySetting.culture);
                        string nameExpectedPhoneNumber = (phoneNumber.Remove(3, 1));
                        var Phonenumber = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.PHONENUMBER_VARIFICATION).Id)).GetAttribute("value");
                        Assert.AreEqual(nameExpectedPhoneNumber, Phonenumber, "Phone Number is not verified");
                    }

                    else
                    {
                        CustomLogs.LogMessage("Phone Number is not selected as preference", TraceEventType.Start);
                    }
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

        public void ContactPreferenceLabels(string msgId1, string msgId2, String pageName, string resourceFileName)
        {
            try
            {

                string culture = CountrySetting.country;
                if (culture == "MY")
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                    //  Fetch Details From resource.XML
                    Resource res1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage1 = res1.Value;
                    var actualMessage1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLEONE_VARIFICATION).XPath)).Text;
                    Assert.AreEqual(expectedMessage1, actualMessage1, pageName + "label1 is not present");
                }
                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("Verifying the page name for the page " + pageName + " started", TraceEventType.Start);
                    Debug.WriteLine(string.Format("{0} - {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Starting"));
                    //  Fetch Details From resource.XML
                    Resource res1 = AutomationHelper.GetResourceMessage(msgId1, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage1 = res1.Value;
                    Debug.WriteLine(string.Format("{0} - {1}", expectedMessage1, "expected message"));
                    Resource res2 = AutomationHelper.GetResourceMessage(msgId2, Path.Combine(SanityConfiguration.MessageDataDirectory, resourceFileName));
                    var expectedMessage2 = res2.Value;
                    Debug.WriteLine(string.Format("{0} - {1}", expectedMessage2, "expected message"));
                    var actualMessage1 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLEONE_VARIFICATION).XPath)).Text;
                    var actualMessage2 = Driver.FindElement(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LABLETWO_VARIFICATION).XPath)).Text;
                    Assert.AreEqual(expectedMessage1, actualMessage1, pageName + "label1 is not present");
                    Assert.AreEqual(expectedMessage2, actualMessage2, pageName + "lable2 is not present");

                }

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

        public void ContactPreferences_BAAviosPremium()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("BA Avios premium is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonBAAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonBAAvios.Enabled, true, "BA Avios is Not selected as email");
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


        public void ContactPreferences_BAAviosStanderd()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("BA Avios Standerd is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_BAAVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonAvios.Enabled, true, "BA Avios is Not selected as email");
                    
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

        public void ContactPreferences_AviosPremium()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("Avios Premium is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {

                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonAvios.Enabled, true, "BA Avios is Not selected as email");
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

        public void ContactPreferences_AviosStanderd()
        {
            try
            {
                if (CountrySetting.culture != "UK")
                {
                    CustomLogs.LogMessage("Avios Standerd is not implemented for countries other then UK", TraceEventType.Start);
                }
                else
                {
                    Driver = ObjAutomationHelper.WebDriver;
                    CustomLogs.LogMessage("BA Avios verification", TraceEventType.Start);
                    IWebElement radioButtonAvios = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.OPTIONSBENEFIT_AVIOSRADIOBTN).Id));
                    Assert.AreEqual(radioButtonAvios.Enabled, true, "BA Avios is Not selected as email");
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
        public void ContactPreference_checkIfGridExist()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("Checking if grid exist on the page started", TraceEventType.Start);
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.GRIDTABLE).Id), Driver))
                    CustomLogs.LogInformation("Grid Exists on the page");
                else
                    Assert.Fail("Grid doesn't exists");
                CustomLogs.LogMessage("Grid is present on the page", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_checkIfGridDoesnotExist()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExiststarted", TraceEventType.Start);
                if (Generic.IsElementPresent(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.GRIDTABLE).Id), Driver))
                    Assert.Fail("Grid Exists on the Page");
                else
                    CustomLogs.LogInformation("Grid doesn't exist on page");
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExist completed", TraceEventType.Stop);

                if (Generic.IsElementPresent(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.NOGRID).XPath), Driver))
                    CustomLogs.LogInformation("Check box Exists on the Page");
                else
                    Assert.Fail("Check box doesn't Exists on Contact Preference");
                CustomLogs.LogMessage("ContactPreference_checkIfGridDoesnotExist completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_SelectProductServiceGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectProductServiceGridFalse started", TraceEventType.Start);
                IWebElement productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPTESCOPRODUCT).Id));
                if (!productElement.Selected)
                {
                    productElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }

                CustomLogs.LogMessage("ContactPreference_SelectProductServiceGridFalse completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }
        
        public void SelectMailingOptionChkBox(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("SelectMailingOptionChkBox started", TraceEventType.Start);
                chkBoxElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                if (chkBoxElement.Selected)
                {
                    chkBoxElement.Click();
                    value = "Unchecked";
                    CustomLogs.LogInformation("Check box for Product services unchecked");
                }
                else
                {
                    chkBoxElement.Click();
                    value = "Checked";
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("SelectMailingOptionChkBox completed", TraceEventType.Stop);
        }
        public void VerifyMailingOptionCheckBox(string key)
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("VerifyMailingOptionCheckBox started", TraceEventType.Start);
                chkBoxElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(key).Id));
                switch (value)
                {
                    case "Checked":
                        if (chkBoxElement.Selected)
                            CustomLogs.LogInformation("Checkbox is selected/Checked");
                        else
                            Assert.Fail("Checkbox is not selected/Checked");
                        break;
                    case "Unchecked":
                        if (!chkBoxElement.Selected)
                            CustomLogs.LogInformation("Element is unchecked");
                        else
                            Assert.Fail("Checkbox is selected/Checked");
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
            CustomLogs.LogMessage("VerifyMailingOptionCheckBox Completed", TraceEventType.Stop);
        }
        public void ContactPreference_SelectContactPermissionGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectContactPermissionGridFalse started", TraceEventType.Start);
                IWebElement ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                if (!ContactElement.Selected)
                {
                    ContactElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is selected");
                }
                CustomLogs.LogMessage("ContactPreference_SelectContactPermissionGridFalse completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_UnSelectContactPermissionGridFalse()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_UnSelectContactPermissionGridFalse started", TraceEventType.Start);
                IWebElement ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                if (!ContactElement.Selected)
                {
                    ContactElement.Click();
                    CustomLogs.LogInformation("Check box for Product services is Unselected");
                }
               
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
            CustomLogs.LogMessage("ContactPreference_UnSelectContactPermissionGridFalse completed", TraceEventType.Stop);
        }

        public void ContactPreference_CheckAllCheckBoxesOpt(string value, string option)
        {
            try
            {
                IWebElement productElement, PartnerElement, ContactElement = null; 

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_CheckAllCheckBoxesOptIn started", TraceEventType.Start);
                
                if (option.Equals("UK"))
                {
                     productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKTESCOPRODUCT).Id));
                     PartnerElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKPARTNEROFFER).Id));
                     ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKRESEARCH).Id));
                }
                else
                {
                     productElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPTESCOPRODUCT).Id));
                     PartnerElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPPARTNEROFFER).Id));
                     ContactElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKGROUPRESEARCH).Id));
                }
                List<IWebElement> CheckOptIn = new List<IWebElement>();
                CheckOptIn.Add(productElement);
                CheckOptIn.Add(PartnerElement);
                CheckOptIn.Add(ContactElement);

                switch(value)
                {
                    case "Out":
                for (int i = 0; i < CheckOptIn.Count(); i++)
                {
                    if (CheckOptIn[i].Selected)
                    {
                        CustomLogs.LogInformation("Check Box is selected/Checked");
                        CheckOptIn[i].Click();
                        CustomLogs.LogInformation("Check box "+ i+" Unchecked");
                    }
                    //else
                       // Assert.Fail("Check box " + i + " not Checked");
                }
                break;
                    case "In":
                for (int i = 0; i < CheckOptIn.Count(); i++)
                {
                    if (!CheckOptIn[i].Selected)
                    {
                        CustomLogs.LogInformation("Check Box is not selected");
                        CheckOptIn[i].Click();
                        CustomLogs.LogInformation("Check box " + i + " checked");
                    }
                   // else
                       // Assert.Fail("Check box " + i + " Checked");
                }
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
            CustomLogs.LogMessage("ContactPreference_CheckAllCheckBoxesOptIn completed", TraceEventType.Stop);
        }


        public void ContactPreference_verifySelectedCheckBox()
        {
            try
            {

                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_verifySelectedCheckBox started", TraceEventType.Start);
                ReadOnlyCollection<IWebElement> ele = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LIST_CHECKBOXES_PR).XPath));
                el = ele.ToList();
                switch (value)
                {
                    case "Unchecked":
                        for (int i = 0; i < checkBoxNumber.Count(); i++)
                        {                    
                            if (!el[checkBoxNumber[i]].Selected)
                                CustomLogs.LogInformation("Checkbox " + checkBoxNumber[i] + " unchecked, hence verified");
                            else
                                Assert.Fail("Checkbox" + checkBoxNumber[i] + " is still checked");
                        }
                        break;
                    case "Checked":
                        for (int i = 0; i < checkBoxNumber.Count(); i++)
                        {
                            if (el[checkBoxNumber[i]].Selected)
                                CustomLogs.LogInformation("Checkbox " + checkBoxNumber[i] + " checked, hence verified");
                            else
                                Assert.Fail("Checkbox" + checkBoxNumber[i] + " is still unchecked");
                        }
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
            CustomLogs.LogMessage("ContactPreference_verifySelectedCheckBox Completed", TraceEventType.Stop);
        }

        public void ContactPreference_SelectAllOrParticularCheckBox()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectAllCheckBox started", TraceEventType.Start);
                ReadOnlyCollection<IWebElement> ele = Driver.FindElements(By.XPath(ObjAutomationHelper.GetControl(ControlKeys.LIST_CHECKBOXES_PR).XPath));
                el = ele.ToList();
                for (int i = 0; i < el.Count; i++)
                {
                    if (!el[i].Selected)
                        totalCheckBox.Add(i);
                }
                if (totalCheckBox.Count.Equals(0))
                {
                    CustomLogs.LogInformation("All checkBoxes are checked, so unchecking all.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        el[i].Click();
                        checkBoxNumber.Add(i);
                        value = "Unchecked";
                        CustomLogs.LogInformation("unchecked checkbox " + i);
                    }
                }
                else if (totalCheckBox.Count.Equals(12))
                {
                    CustomLogs.LogInformation("All checkBoxes are unchecked, so checking all.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        el[i].Click();
                        checkBoxNumber.Add(i);
                        value = "Checked";
                        CustomLogs.LogInformation("checked checkbox " + i);
                    }

                }
                else
                {
                    CustomLogs.LogInformation("Few checkBoxes are unchecked, so checking rest.....");
                    for (int i = 0; i < el.Count; i++)
                    {
                        if (!el[i].Selected)
                        {
                            el[i].Click();
                            checkBoxNumber.Add(i);
                            value = "Checked";
                            CustomLogs.LogInformation("checked checkbox " + i);
                        }

                    }
                }

                CustomLogs.LogMessage("ContactPreference_SelectAllOrOneCheckBox Completed", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }

        public void ContactPreference_SelectParticularCheckBox()
        {
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                CustomLogs.LogMessage("ContactPreference_SelectParticularCheckBox started", TraceEventType.Start);               
                IWebElement chkbox1 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKBOXEMAIL).Id));
                IWebElement chkbox2 = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.CHKBOXSMS).Id));

                List<IWebElement> ChkBox = new List<IWebElement>();
                ChkBox.Add(chkbox1);
                ChkBox.Add(chkbox2);

                for (int i = 0; i < ChkBox.Count(); i++)
                {
                    if (!ChkBox[i].Selected)
                    {
                        ChkBox[i].Click();
                        CustomLogs.LogInformation("Check box clicked");
                    }
                    else
                    {
                        ChkBox[i].Click();
                        CustomLogs.LogInformation("CheckBox already clicked");
                    }
                }
               
                CustomLogs.LogMessage("ContactPreference_SelectParticularCheckBox Completed", TraceEventType.Stop);
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