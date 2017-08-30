using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.CustomerService;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using System.ServiceModel;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Delete all the cookies at first
                Helper.DeleteAllCookies();
                
                Response.Redirect("~/Kiosk/WhatIsYourTitleandName.aspx", false);
            }
            catch (Exception exception)
            {
                Logger.Write(exception, "General", 1, 3500, TraceEventType.Error, "Join at Kiosk");
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }
    }
}