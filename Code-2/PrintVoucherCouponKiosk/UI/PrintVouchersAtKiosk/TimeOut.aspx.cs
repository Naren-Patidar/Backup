using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    public partial class TimeOut : System.Web.UI.Page
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["BookingPrintVoucher"] = null;
        }
        protected void Cancel_Restart(object sender, EventArgs e)
        {
            //Response.Redirect("Default.aspx");
            Response.Redirect(ConfigurationManager.AppSettings["ExitURL"].ToString());
        }
    }
}
