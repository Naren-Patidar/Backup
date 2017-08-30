using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Utilities;

namespace Tesco.Framework.UITesting.Helpers
{
    public static class ScreenShotDetails
    {
        #region PRIVATE MEMBERS
        //The path where the screen shot gets saved
        private static string screenShotPath = ConfigurationManager.AppSettings["ScreenShotPath"];
        //The method that threw the exception or failed
        private static string sourceMethod = string.Empty;
        //The class that threw the exception or failed
        private static string sourceClass = string.Empty;
        private static ILogger customLogs = null;
        private static string timeStamp = null;
        #endregion

        #region PROPERTIES
        static string FolderName
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        #endregion

        #region STATIC CONSTRUCTOR

        static ScreenShotDetails()
        {
            LoggingFactory.InitializeLogFactory(Log4NetAdapter.SetInstance(AppenderType.UNITTEST.ToString()));
            customLogs = LoggingFactory.GetLogger();

            timeStamp = DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy_HHmmss");

        }

        #endregion

        #region MEMBER FUNCTION(S)

        // Method to take the screenshot in case an error/exception occurs
        public static void TakeScreenShot(IWebDriver webDriver, Exception ex)
        {
            try
            {
                //if modal dialog comes
                if (IsModalDialogPresent(ref webDriver))
                {
                    webDriver.SwitchTo().Alert().Accept();
                }

                //Extracting Exception Source Class & Method Name
                ExtractExceptionDetails(ex);

                //Creating folder for saving error screenshot
                bool ifFolderExists = System.IO.Directory.Exists(screenShotPath);
                if (!ifFolderExists)
                    System.IO.Directory.CreateDirectory(screenShotPath);

                //Take the screenshot            
                Screenshot screenCapture = ((ITakesScreenshot)webDriver).GetScreenshot();

                // Create test folder
                string path = screenShotPath + @"\" + FolderName;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                //Save the screenshot
                screenCapture.SaveAsFile((string.Format("{0}\\{1}", path, sourceMethod + ".jpeg")), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception exception)
            {
                customLogs.LogException(exception);
            }
        }

        /// <summary>
        /// Private Method to Extract Source Class & Method Name for an Exception
        /// </summary>
        /// <param name="ex"></param>
        private static void ExtractExceptionDetails(Exception ex)
        {
            StackTrace trace = new StackTrace(ex, true);
            sourceClass = trace.GetFrame(trace.FrameCount - 1).GetMethod().DeclaringType.ToString();
            sourceMethod = trace.GetFrame(trace.FrameCount - 1).GetMethod().Name;
        }

        /// <summary>
        /// Check if a modal dialog is present or not
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        private static bool IsModalDialogPresent(ref IWebDriver webDriver)
        {
            try
            {
                webDriver.SwitchTo().Alert();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}
