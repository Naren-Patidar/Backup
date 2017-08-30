using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
//using InstoreClubcardReward.Data;

namespace PrintVouchersAtKiosk
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (ConfigurationManager.AppSettings["ServiceAvailable"] == "False")
            //    Response.Redirect("Disabled.aspx");

            // basic url - web config setting
            //string EntryPageURL = ConfigurationManager.AppSettings["EntryPageURL"];
            string EntryPageURL = "Clubcard.aspx";

            // client IP address
            string clientIP = Request.UserHostAddress;

            // string for constructing parameters
            string parameters="";

            // collection for output from stored procedure
            try
            {
                //parameters = InstoreClubcardReward.Business.Kiosk.GetKioskEntryPage(clientIP);
                //parameters = InstoreClubcardReward.Business.KioskMaster.KioskEntryPageWCF(clientIP);
                parameters = Helper.GetParameters(clientIP);

                // if there is a record for the ip address
                if (parameters.Length <= 0)               
                {
                    parameters = string.Format("?KioskID={0}&StoreID={1}&KioskNo={2}", 1, 999, 999);
                }
            }
            catch(Exception ex)
            {
                // standard parameters
                parameters = string.Format("?KioskID={0}&StoreID={1}&KioskNo={2}", 1, 999, 999);

                Logger.Write(ex.Message + " Default.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

                finally
            {
                Response.Redirect(EntryPageURL + parameters, false);
                //Response.Redirect(parameters);
       
            }


        }
    }
}
