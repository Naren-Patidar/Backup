using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Resources;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class MessageForAddress : System.Web.UI.Page
    {
        protected string sAddress = string.Empty;
        protected string sPageName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("FirstName")))
                {
                    Response.Redirect("~/Kiosk/TimeOut.aspx", false);
                }
                else
                {
                    Helper.CheckAndResetCookieExpiration("Tilte");
                }
                if (Request.QueryString["PostCode"] != null)
                {
                    spanmsg.InnerText = GlobalResources.AddressNotFound.ToString() + Request.QueryString["PostCode"].ToString().Trim() + GlobalResources.EnterManually.ToString(); ;
                    string postcode = Request.QueryString["PostCode"].ToString().Trim();
                    Helper.CheckAndResetCookieExpiration("PostCode");
                    sPageName = "WhatIsYourAdress.aspx?PostCode=' "+postcode + "'";
                }
                if (Request.QueryString["ErrorMsg"] != null)
                {
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "AddressLine3")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine3Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("MailingAddress3");
                    }
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "AddressLine2")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine2Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("MailingAddress2");
                    }
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "AddressLine1")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine1Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("MailingAddress1");
                    }
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "GrPostcode")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine1Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("PostCode");
                    }
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "AddressLine4")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine1Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("MailingAddress4");
                    }
                    if (Request.QueryString["ErrorMsg"].ToString().Trim() == "AddressLine5")
                    {
                        spanmsg.InnerText = GlobalResources.AddressLine1Error;
                        sPageName = "WhatIsYourAdress.aspx?ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        Helper.CheckAndResetCookieExpiration("MailingAddress5");
                    }
                }


                if (Request.QueryString["InValidPostCode"] != null)
                {
                    spanmsg.InnerText = GlobalResources.InvalidPostcode + Request.QueryString["InValidPostCode"].ToString().Trim() + GlobalResources.InvalidPostcode1;
                    sPageName = "WhatIsYourAdress.aspx?PostCode=' " + Request.QueryString["InValidPostCode"].ToString().Trim() + "'";
                    Helper.CheckAndResetCookieExpiration("PostCode");
                }
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 3500, System.Diagnostics.TraceEventType.Error, "Clubcard Jion At Kiosk");
                throw exp;
            }
        }
    }
}