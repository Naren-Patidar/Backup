using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Logging.Logger;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using Tesco.Framework.Common.Utilities;
using OpenQA.Selenium.Support.UI;
using Tesco.Framework.UITesting.Enums;

namespace Tesco.Framework.UITesting.Test.Common
{
    public class Base
    {
        #region Fields
        Message message = null;
        AutomationHelper objAutomationHelper = null;
        IWebDriver driver = null;
        ILogger customLogs = null;
        static AppConfiguration sanityConfiguration = new AppConfiguration();
                
        static string culture;
        #endregion

        #region Properties

        /// <summary>
        /// Property to get or set the message of Login module
        /// </summary>
        public Message Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Property to get or set the automation helper object 
        /// </summary>
        public AutomationHelper ObjAutomationHelper
        {
            get { return objAutomationHelper; }
            set { objAutomationHelper = value; }
        }

        /// <summary>
        /// Property to get or set the driver
        /// </summary>
        public IWebDriver Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        /// <summary>
        /// Property to get or set the custom Logs
        /// </summary>
        public ILogger CustomLogs
        {
            get { return customLogs; }
            set { customLogs = value; }
        }
       
        public static AppConfiguration SanityConfiguration
        {
            get { return Base.sanityConfiguration; }
            set { Base.sanityConfiguration = value; }
        }

        #endregion

        #region Constructor
        public Base()
        {
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
            
        }
        #endregion

        /// <summary>
        /// To wait for the element based on wait type
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="waitType"></param>
        /// <returns></returns>
        public static WebDriverWait GetWaitInterval(IWebDriver driver, WaitInterval waitType)
        {
            int interval = 0;
            switch (waitType)
            {
                case WaitInterval.UltraShortSpan:
                    interval = 10;
                    break;
                case WaitInterval.ShortSpan:
                    interval = 20;
                    break;
                case WaitInterval.MediumSpan:
                    interval = 60;
                    break;
                case WaitInterval.LargeSpan:
                    interval = 90;
                    break;
            }
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(interval));
            return wait;
        }          
    }
}
