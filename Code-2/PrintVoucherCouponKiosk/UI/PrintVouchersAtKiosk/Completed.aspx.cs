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
    /// Purpose: This section is used to show success message once customer is done with printing his clubcard vouchers.
    /// Updated By : Praveen Yadav to show the messages and button text in local language and make tab highlighting the current screen configurable.
    /// Updated on : 20th july 2012
    /// </summary>
    public partial class Printed : Base
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CheckVisibilityOfConfiguredImages();
                    SetPrintCompleteText();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Completed.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
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
                Logger.Write(ex.Message + " Completed.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void SetPrintCompleteText()
        {
            string printCode = string.Empty;
            try
            {
                /* Get the print code from Query string.
                    1 - print voucher 
                    2 - print coupon
                */
                printCode = Request.QueryString["PrintCode"].Trim();
                //If printing vouchers, default text is displayed from resource file.
                //If printing coupons, alter text from resource file.
                if (printCode.Equals("2"))
                {
                    FinishedPrinting.Text = GetLocalResourceObject("FinishedCouponPrinting").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Completed.aspx SetPrintCompleteText()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void lnkNewCustomer_Click(object sender, EventArgs e)
        {
           try
           {
            Session["BookingPrintVoucher"] = null;
            Response.Redirect(ConfigurationManager.AppSettings["ExitURL"].ToString(),false);
           }
           catch (Exception ex)
           {
               Logger.Write(ex.Message + " Completed.aspx lnkNewCustomer_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
               Response.Redirect("Error.aspx", false);
           }
        }


        protected void lnkPrintOthers_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SelectAllVouchers.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Completed.aspx lnkPrintOthers_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}