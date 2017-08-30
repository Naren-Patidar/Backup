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
    public partial class ClubcardCharter5 : BaseUIPage
    {
        protected string sPage = string.Empty;
        protected string pageName = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")) && (Request.QueryString["page"].ToString().Trim() == string.Empty))
            {
                Response.Redirect("~/Kiosk/TimeOut.aspx", false);
            }
            else
            {
                Helper.CheckAndResetCookieExpiration("FirstName");
            }
            SetConfiguredCCPages();
            if ((Request.QueryString["page"].ToString().Trim() != null) && (Request.QueryString["page"].ToString().Trim() != string.Empty))
            {
                sPage = Request.QueryString["page"].ToString().Trim();
            }
        }
        private void SetConfiguredCCPages()
        {
            try
            {
                bool showPageCC1 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowClubcardCharter1"]);
                bool showPageCC2 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowClubcardCharter2"]);
                bool showPageCC3 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowClubcardCharter3"]);
                bool showPageCC4 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowClubcardCharter4"]);

                if (!showPageCC1)
                {
                    pg5CC1.Visible = false;
                }
                if (!showPageCC2)
                {
                    pg5CC2.Visible = false;
                }
                if (!showPageCC3)
                {
                    pg5CC3.Visible = false;
                }
                if (!showPageCC4)
                {
                    pg5CC4.Visible = false;
                }
                if ((!showPageCC1) && (!showPageCC2) && (!showPageCC3) && (!showPageCC4))
                {
                    CC5Buttons.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Function:SetConfiguredCCPages():" + ex.Message, "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while setting configured ClubcardCharacters Page5");
            }
        }
        protected void Back_Click(object sender, EventArgs e)
        {
            pageName = "~/Kiosk/" + sPage.ToString().Trim() + ".aspx";
            Response.Redirect(pageName);
        }
    }
}