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
using Tesco.Framework.UITesting.Constants;
using System.IO;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class ChristmasSaver : Base
    {
        #region Constructor


        public ChristmasSaver(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }

        #endregion
        #region Methods

        public void Verify_Title_Header(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Header Title on Chirstmas Saver Page started ", TraceEventType.Start);
                string headerTextElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.XMUSSAVER_HEADERTITLE).Id)).Text;

                Resource resHeaderText = AutomationHelper.GetResourceMessage(LabelKey.CHRISTMASSAVER, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));

                if (headerTextElement.Equals(resHeaderText.Value.Trim()))
                {
                    CustomLogs.LogInformation("Christmas Saver Header Title verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Christmas Saver Header Title not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Header Title on Chirstmas Saver Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void Verify_WelcomeMessage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Welcome Message on Chirstmas Saver Page started ", TraceEventType.Start);
                string welcomeTextElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.XMUSSAVER_WELCOMEMESSAGE).Id)).Text;

                Resource resWelcomeTextOne = AutomationHelper.GetResourceMessage(LabelKey.CHRITSMASSAVER_WELCOMEMESSAGEONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
                Resource resWelcomeTextTwo = AutomationHelper.GetResourceMessage(LabelKey.CHRITSMASSAVER_WELCOMEMESSAGETWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));

                if (welcomeTextElement.Contains(resWelcomeTextOne.Value.Trim()) && welcomeTextElement.Contains(resWelcomeTextTwo.Value.Trim()))
                {
                    CustomLogs.LogInformation("Christmas Saver Welcome Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Christmas Saver Welcome Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Welcome Message on Chirstmas Saver Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void Verify_ThankYouMessage(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Thank You Message on Chirstmas Saver Page started ", TraceEventType.Start);
                string ThankYouTextElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.XMUSSAVER_THANKYOUMESSAGE).Id)).Text;

                Resource resThankYouText = AutomationHelper.GetResourceMessage(LabelKey.CHRISTMASSAVER_THANKYOUMESSAGE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));

                if (ThankYouTextElement.Equals(resThankYouText.Value.Trim()))
                {
                    CustomLogs.LogInformation("Christmas Saver Thank You Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Christmas Saver Thank You Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Thank You Message on Chirstmas Saver Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }

        public void Verify_OptionsAndBenefitsText(IWebDriver Driver)
        {
            try
            {
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);
                string OptionsAndBenefitsTextElement = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.XMUSSAVER_OPTIONSANDBENEFITSMESSAGE).Id)).Text;

                Resource resOptionsAndBeenefitsTextOne = AutomationHelper.GetResourceMessage(LabelKey.CHRISTMASSAVER_OPTIONSANDBENEFITSMESSAGEONE, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));
                Resource resOptionsAndBeenefitsTextTwo = AutomationHelper.GetResourceMessage(LabelKey.CHRISTMASSAVER_OPTIONSANDBENEFITSMESSAGETWO, Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.CHRISTMASSAVER_RESOURCE));

                if (OptionsAndBenefitsTextElement.Equals(string.Format("{0} {1}.", resOptionsAndBeenefitsTextOne.Value.Trim(), resOptionsAndBeenefitsTextTwo.Value.Trim()).Trim()))
                {
                    CustomLogs.LogInformation("Christmas Saver Options And Benefits Text verified for country " + CountrySetting.country);
                }
                else
                {
                    Assert.Fail("Christmas Saver Options And Benefits Text not verified for country " + CountrySetting.country);
                }
                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page completed ", TraceEventType.Stop);
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
        #endregion
    }

}
