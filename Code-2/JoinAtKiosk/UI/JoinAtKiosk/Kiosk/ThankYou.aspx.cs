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
    public partial class ThankYou : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Write("Start of ThankYou Page_Load()", "General");
            Helper.DeleteAllCookies();
        }
        protected void Cancel_Restart(object sender, EventArgs e)
        {
            string sUrl = ConfigurationReader.GetStringConfigKey("MainPage");
            Response.Redirect(sUrl, false);
        }
    }
}