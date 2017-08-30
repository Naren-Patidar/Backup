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
    public partial class TermsAndCondition5 : BaseUIPage
    {
        protected string sPage = string.Empty;
        protected string pageName = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserFirstName")) && Request.QueryString["page"].ToString().Trim() == string.Empty)
            {
                Response.Redirect("~/Kiosk/TimeOut.aspx", false);
            }
            else
            {
                Helper.CheckAndResetCookieExpiration("UserFirstName");
            }
            SetConfiguredTCPages();
            if ((Request.QueryString["page"].ToString().Trim() != null) && (Request.QueryString["page"].ToString().Trim() != string.Empty))
            {
                sPage = Request.QueryString["page"].ToString().Trim();
            }
        }

        private void SetConfiguredTCPages()
        {
            try
            {
                bool showPageTC1 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons1"]);
                bool showPageTC2 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons2"]);
                bool showPageTC3 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons3"]);
                bool showPageTC4 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons4"]);

                if (!showPageTC1)
                {
                    Pg5TC1.Visible = false;
                }
                if (!showPageTC2)
                {
                    Pg5TC2.Visible = false;
                }
                if (!showPageTC3)
                {
                    Pg5TC3.Visible = false;
                }
                if (!showPageTC4)
                {
                    Pg5TC4.Visible = false;
                }
                if ((!showPageTC1) && (!showPageTC2) && (!showPageTC3) && (!showPageTC4))
                {
                    TC5Buttons.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Function:SetConfiguredTCPages():" + ex.Message, "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while setting configured TermsAndConditions5 Page");
            }
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            pageName = "~/Kiosk/" + sPage.ToString().Trim() + ".aspx";
            Response.Redirect(pageName);
        }
    }
}