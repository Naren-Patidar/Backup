#region Copyright
/*
*************************************************************************
Copyright (c) 2013 by Tesco

All	rights reserved. No	portion	of this	software or	its	content	may	be
reproduced in any form or by any means,	without	the	express	written
permission of the copyright	owner.
*************************************************************************
*/
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
#endregion


namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    #region	Header
    //	$Author: Praveen Yadav
    //	$Version: 1.1
    //  Description : This page will display the terms and conditions to be adhered to, as a Tesco Clubcard Customer.
    #endregion Header

    public partial class TermsAndCondition : BaseUIPage
    {
        #region Private Members
        public string sPage = string.Empty;
        private string pageName = string.Empty;
        #endregion

        #region PageLoad
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
            if (Request.QueryString["page"] != null && Request.QueryString["page"] != string.Empty)
            {
                sPage = Request.QueryString["page"].ToString().Trim();
            }
        }
        #endregion       

        private void SetConfiguredTCPages()
        {
            try
            {
                bool showPageTC2 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons2"]);
                bool showPageTC3 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons3"]);
                bool showPageTC4 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons4"]);
                bool showPageTC5 = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowTermsAndConditons5"]);

                if (!showPageTC2)
                {
                    pg1TC2.Visible = false;
                }
                if (!showPageTC3)
                {
                    pg1TC3.Visible = false;
                }
                if (!showPageTC4)
                {
                    pg1TC4.Visible = false;
                }
                if (!showPageTC5)
                {
                    pg1TC5.Visible = false;
                }
                if ((!showPageTC2) && (!showPageTC3) && (!showPageTC4) && (!showPageTC5))
                {
                    TC1Buttons.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Function:SetConfiguredTCPages():" + ex.Message, "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while setting configured TermsAndConditions2 Page");
            }
        }


        #region ButtonClick Events
        protected void Back_Click(object sender, EventArgs e)
        {
            pageName = "~/Kiosk/" + sPage.ToString().Trim() + ".aspx";
            Response.Redirect(pageName);
        }
        #endregion
    }
}