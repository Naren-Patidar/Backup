﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class ClubcardCharter4 : BaseUIPage
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
                bool showPageCC5 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowClubcardCharter5"]);

                if (!showPageCC1)
                {
                    pg4CC1.Visible = false;
                }
                if (!showPageCC2)
                {
                    pg4CC2.Visible = false;
                }
                if (!showPageCC3)
                {
                    pg4CC3.Visible = false;
                }
                if (!showPageCC5)
                {
                    pg4CC5.Visible = false;
                }
                if ((!showPageCC1) && (!showPageCC2) && (!showPageCC3) && (!showPageCC5))
                {
                    CC4Buttons.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Function:SetConfiguredCCPages():" + ex.Message, "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while setting configured ClubcardCharacters Page4");
            }
        }
        protected void Back_Click(object sender, EventArgs e)
        {
            pageName = "~/Kiosk/" + sPage.ToString().Trim() + ".aspx";
            Response.Redirect(pageName);
        }
    }
}