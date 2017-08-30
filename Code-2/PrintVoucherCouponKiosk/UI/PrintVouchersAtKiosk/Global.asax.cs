using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;

namespace PrintVouchersAtKiosk
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ApplicationConstants.EventId=Convert.ToInt32(ConfigurationManager.AppSettings["EventId"]);
            ApplicationConstants.CurrentCulture = ConfigurationManager.AppSettings["CountryCode"];
            ApplicationConstants.ShowCurrencyAsPrefix = ConfigurationManager.AppSettings["ShowCurrencyAsPrefix"].ToString().ToUpper();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}