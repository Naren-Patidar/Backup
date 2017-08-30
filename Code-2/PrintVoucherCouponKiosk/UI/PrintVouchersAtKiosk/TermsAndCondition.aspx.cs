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
    /// Purpose: This section is used to show Terms & Conditions of how to use clubcard Vouchers and where to use. It also includes the tesco contact details for any queries.
    /// Updated By : Praveen Yadav to show the messages and button text in local language and make tab highlighting the current screen configurable.
    /// Updated on : 20th july 2012
    /// </summary>
    public partial class TermsAndCondition : Base
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CheckVisibilityOfConfiguredImages();
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " TermsAndCondition.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
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
                Logger.Write(ex.Message + " TermsAndCondition.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void Back_Click(object sender, EventArgs e)
        {
            string PageName=string.Empty;
            try
            {
                PageName = Request.QueryString["page"].ToString();
                if (PageName == "Clubcard.aspx")
                    PageName = "Default.aspx";

                if (PageName == "Verification.aspx")
                    PageName = "Verification.aspx?Existing=false";


                Response.Redirect(PageName, false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " TermsAndCondition.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
