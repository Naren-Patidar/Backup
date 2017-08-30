using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Purpose: This section is used to show that there is some problem with the System
    /// Updated By : Praveen Yadav to show the error messages and text of controls in local language and make image configurable for highlighting the current screen.
    /// Updated on : 20th july 2012
    /// </summary>
    public partial class Error : Base
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CheckVisibilityOfConfiguredImages();
                Session["BookingPrintVoucher"] = string.Empty;

                if (Request.QueryString["ActiveVouchers"] != null)
                {
                    if (Request.QueryString["ActiveVouchers"].ToString().Trim() == "False")
                    {
                        spanmsg.InnerText = GetLocalResourceObject("lblErrorSorryYouDontHaveActiveVouchers").ToString();
                    }
                }
                else
                {
                    spanmsg.InnerText = GetLocalResourceObject("lblErrorSorryTheresBeenAProblem").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Error.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

       
        public void CheckVisibilityOfConfiguredImages()
        {
            string ShowHighlightCurrentScreenName = string.Empty;
            try
            {
                ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];
                // Check whether to highlight the current screen name
                if (ShowHighlightCurrentScreenName.ToLower() == "true")
                {
                    breadcrumbs.Visible = true;
                }
                else
                {
                    breadcrumbs.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Error.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void lnkStartAgain_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(ConfigurationManager.AppSettings["ExitURL"].ToString(), false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Error.aspx lnkStartAgain_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}