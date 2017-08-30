using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Enums;
using OpenQA.Selenium.Support.UI;
using Tesco.Framework.UITesting.Test.Common;
using System.Threading;
using Tesco.Framework.Common.Utilities;
using System.Collections.ObjectModel;
using Tesco.Framework.UITesting.Entities;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml.Linq;
using System.Globalization;
using Newtonsoft.Json;


namespace Tesco.Framework.UITesting.Helpers
{
    /// <summary>
    /// Extension class to wait
    /// </summary>
    public static class Extension
    {
        public static bool WaitForLabel(this IWebElement control, IWebDriver driver, string textToVerify)
        {
            bool chk = false;            
            WebDriverWait wait = Base.GetWaitInterval(driver,WaitInterval.MediumSpan);            
            chk = wait.Until(ExpectedConditions.TextToBePresentInElement(control,textToVerify));
            return chk;
        }


        public static bool WaitForTitle(this IWebElement control, IWebDriver driver, string textToVerify)
        {
            bool chk = false;
            WebDriverWait wait = Base.GetWaitInterval(driver, WaitInterval.MediumSpan);
            chk = wait.Until(ExpectedConditions.TitleIs(textToVerify));
            return chk;
        }

        /// <summary>
        /// Extension method for Control class to wait untill control (find by CSS) is loaded in DOM
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="driver"></param>
        public static void WaitForControlByCss(this Control ctrl, IWebDriver driver)
        {
            WebDriverWait wait = Base.GetWaitInterval(driver, WaitInterval.MediumSpan);
            Int64 ticks = DateTime.Now.Ticks;
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(ctrl.Id)));            
            while (driver.FindElement(By.CssSelector(ctrl.Id)) == null)
            {
                ;
            }
            Debug.WriteLine(string.Format("Waited for {0} control by {1} mili sec.", ctrl.Id, TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).Milliseconds));
        }

        /// <summary>
        /// Extension method for Control class to wait untill control (find by XPath) is loaded in DOM
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="driver"></param>
        public static void WaitForControlByxPath(this Control ctrl, IWebDriver driver)
        {
            WebDriverWait wait = Base.GetWaitInterval(driver, WaitInterval.MediumSpan);
            Int64 ticks = DateTime.Now.Ticks;
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(ctrl.XPath))); 
            while (driver.FindElement(By.XPath(ctrl.XPath)) == null)
            {
                ;
            }
            Debug.WriteLine(string.Format("Waited for {0} control by {1} mili sec.", ctrl.XPath, TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).Milliseconds));
        }
        
        //Extension method for executing the java script
        public static T ExecuteJs<T>(this IWebDriver driver, string script, params object[] parameters)
        {
            if (parameters == null)
                return (T)((IJavaScriptExecutor)driver).ExecuteScript(script);
            else
                return (T)((IJavaScriptExecutor)driver).ExecuteScript(script, parameters);
        }

        //Extension method for finding the web element by giving an implicit wait of "x" sec
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds, bool isSleepNeeded = false)
        {
            if (isSleepNeeded)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Convert.ToInt32(Utilities.GetConfigValue("WaitTime"))));
            }

            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement element = null;

                wait.Until<bool>((d) =>
                {
                    try
                    {
                        element = d.FindElement(by);

                        if (element == null)
                        {
                            return false;
                        }

                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });

                return element;
            }

            return driver.FindElement(by);
        }

        //Extension method for finding the web elements(in a collection) by giving an implicit wait of "x" sec
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds, bool isSleepNeeded = false)
        {
            if (isSleepNeeded)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Convert.ToInt32(Utilities.GetConfigValue("WaitTime"))));
            }

            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                ReadOnlyCollection<IWebElement> elements = null;

                wait.Until<bool>((d) =>
                {
                    try
                    {
                        elements = d.FindElements(by);

                        if (elements.Count == 0)
                        {
                            return false;
                        }

                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });

                return elements;
            }

            return driver.FindElements(by);
        }

        public static T GetValue<T>(this XElement element)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (element != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    value = (T)converter.ConvertFrom(element.Value);
                }
                catch { }
            }
            else if (element == null && typeof(T).Name.Equals(typeof(string).Name))
            {
                value = (T)converter.ConvertFrom(string.Empty);
            }
            return value;
        }

        public static bool TryParseDate(this string objValue, out DateTime dtResult)
        {
            bool bReturn = false;
            dtResult = DateTime.Now;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    if (DateTime.TryParseExact(objValue, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtResult))
                    {
                        bReturn = true;
                    }
                }
                catch { }
            }
            return bReturn;
        }

        public static string[] DateFormats = 
            {
                "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
                "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
                "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
                "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
                "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                "yyyy/MM/dd hh:mm", "yyyy/MM/dd hh:mm:ss",
                "yyyy-MM-ddThh:mm:ss:fff", "yyyy-MM-ddThh:mm:ss",
                "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd hh:mm",
                "M-dd-yyyy h:mm:ss", "M-dd-yyyy h:mm",
                "M-dd-yyyy hh:mm:ss", "M-dd-yyyy hh:mm",
                "yyyy/M/dd hh:mm", "yyyy/M/dd hh:mm:ss",
                "M/d/yyyy H:mm:ss tt", "M/d/yyyy H:mm tt", 
                "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss", 
                "M/d/yyyy HH:mm tt", "M/d/yyyy HH tt", 
                "M/d/yyyy H:mm", "M/d/yyyy H:mm", 
                "MM/dd/yyyy HH:mm", "M/dd/yyyy HH:mm",
                "yyyy/MM/dd HH:mm", "yyyy/MM/dd HH:mm:ss",
                "yyyy-MM-ddTHH:mm:ss:fff", "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm",
                "M-dd-yyyy H:mm:ss", "M-dd-yyyy H:mm",
                "M-dd-yyyy HH:mm:ss", "M-dd-yyyy HH:mm",
                "yyyy/M/dd HH:mm", "yyyy/M/dd HH:mm:ss",
                "dd/MM/yy", "MMM dd yyyy hh:mmtt", "MMM d yyyy hh:mmtt",
                "yyyy-MM-ddTHH:mm:sszzz", "yyyy-MM-ddTHH:mm:ssTZD",
                "yyyy-MM-ddTHH:mm:ssTZ", "dd MMM yyyy",
                "dd/MM/yyyy HH:mm:ss"
            };

        public static string MasknFormatClubcard(string clubcardNumber, bool isMaskReq, char maskChar)
        {
            #region Local variable declaration
            string middleGroup = string.Empty;
            string maskedClubCard = string.Empty;
            #endregion
            try
            {

                if (clubcardNumber.Trim().Length > 15)
                {
                    middleGroup = clubcardNumber.Substring(6, 8);
                    StringBuilder MG = new StringBuilder(clubcardNumber);
                    MG.Replace(middleGroup, " XXXX XXXX ");

                    //return the formatted card number
                    maskedClubCard = MG.ToString();
                }
                //if clubcard number found lesser than 16 digits the unformatted clubcard number is returned
                else
                {
                    maskedClubCard = clubcardNumber;
                }

                return maskedClubCard;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public static void AcceptAlert(this IWebDriver driver)
        {
            IAlert alert = driver.GetAlert();
            if (alert != null)
            {
                alert.Accept();
            }
        }

        public static void DismissAlert(this IWebDriver driver)
        {
            IAlert alert = driver.GetAlert();
            if (alert != null)
            {
                alert.Dismiss();
            }
        }

        public static IAlert GetAlert(this IWebDriver driver)
        {
            IAlert alert = null;
            try
            {
                alert = driver.SwitchTo().Alert();                
            }
            catch (NoAlertPresentException)
            {
                //DO NOTHING
            }
            return alert;
        }
        public static T JsonToObject<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public static bool EqualsTo(this string value, string source, bool ignoreSpace)
        {
            bool check = false;
            if (!ignoreSpace)
            {
                check = value.Equals(source);
            }
            else
            {
                if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(value))
                {
                    check = true;
                }
                else if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value))
                {
                    List<char> array = value.ToCharArray().ToList();
                    array.RemoveAll(c => c.Equals(' '));
                    List<char> sourceArray = source.ToCharArray().ToList();
                    sourceArray.RemoveAll(c => c.Equals(' '));
                    check = new string(array.ToArray()).Equals(new string(sourceArray.ToArray()));
                }
            }
            return check;
        }

        /// <summary>
        /// Extension method to object to convert it to given type
        /// </summary>
        /// <typeparam name="T">given Type </typeparam>
        /// <param name="objValue">object value</param>
        /// <returns>converted value in given type</returns>
        public static T TryParse<T>(this object objValue)
        {
            T value = default(T);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (!Convert.IsDBNull(objValue) && objValue != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    value = (T)converter.ConvertFrom(objValue.ToString());
                }
                catch { }
            }
            return value;
        }
        
    }
}
