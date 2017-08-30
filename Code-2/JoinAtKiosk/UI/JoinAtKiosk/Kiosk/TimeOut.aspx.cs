using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class TimeOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Write("Start of TimeOut Page_Load()", "General");
        }
        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Helper.DeleteAllCookies();
            string sUrl = ConfigurationReader.GetStringConfigKey("MainPage");
            Response.Redirect(sUrl, false);
        }
    }
}