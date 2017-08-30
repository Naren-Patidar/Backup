using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading;
using System.Globalization;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk
{
    public class BaseUIPage : System.Web.UI.Page
    {
        #region Culture setting
        /// <SUMMARY>
        /// Overriding the InitializeCulture method to set the user selected
        /// option in the current thread. Note that this method is called much
        /// earlier in the Page lifecycle and we don't have access to any controls
        /// in this stage, so have to use Form collection.
        /// </SUMMARY>
        protected override void InitializeCulture()
        {
            string culture = ConfigurationReader.GetStringConfigKey("CountryCode");

            if (Session["MyUICulture"] == null && Session["MyCulture"] == null)
            {
                SetCulture(culture, culture);
            }

            if (Session["MyUICulture"] != null && Session["MyCulture"] != null)
            {
                Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["MyUICulture"];
                Thread.CurrentThread.CurrentCulture = (CultureInfo)Session["MyCulture"];
            }
            base.InitializeCulture();
        }


        /// <Summary>
        /// Sets the current UICulture and CurrentCulture based on
        /// the arguments
        /// </Summary>
        /// <PARAM name="name"></PARAM>
        /// <PARAM name="locale"></PARAM>
        protected void SetCulture(string name, string locale)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            ///<remarks>
            ///Saving the current thread's culture set by the User in the Session
            ///so that it can be used across the pages in the current application.
            ///</remarks>
            Session["MyUICulture"] = Thread.CurrentThread.CurrentUICulture;
            Session["MyCulture"] = Thread.CurrentThread.CurrentCulture;
        }
        #endregion

    }
}