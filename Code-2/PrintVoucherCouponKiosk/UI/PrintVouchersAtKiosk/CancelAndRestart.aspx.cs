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
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Purpose: This section is used to show error messages to users that the transaction is cancelled and all the data entered by user is cleared
    /// Updated By : Praveen Yadav to show the error message and button text in local language
    /// Updated on : 26th july 2012
    /// </summary>
    public partial class CancelAndRestart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CheckVisibilityOfConfiguredImages();
                Session["BookingPrintVoucher"] = null;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CancelAndRestart.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
        protected void Cancel_Restart(object sender, EventArgs e)
        {
            //Response.Redirect("Default.aspx", false);
            Response.Redirect(ConfigurationManager.AppSettings["ExitURL"].ToString());
        }

        public void CheckVisibilityOfConfiguredImages()
        {
            try
            {
                string ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];

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
                Logger.Write(ex.Message + " CancelAndRestart.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        string culture = Helper.GetCulture();
        protected override void InitializeCulture()
        {
            try
            {
                string culture = Helper.GetCulture();
                UICulture = culture;
                Culture = culture;

                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(culture);
                Thread.CurrentThread.CurrentUICulture = new
                    CultureInfo(culture);
                base.InitializeCulture();
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CancelAndRestart.aspx InitializeCulture()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

    }
}
