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
    /// Purpose: This section is used to show error messages to users specific to each field in verification screen.
    /// Updated By : Praveen Yadav to show the error message and button text in local language
    /// Updated on : 20th july 2012
    /// </summary>
    public partial class MessageForAddress : Base
    {
        protected string sPageName = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["FinalPage"] != null)
                {
                    if (Request.QueryString["FinalPage"].ToString().Trim() == "InValid")
                    {
                        spanmsg.InnerText = GetLocalResourceObject("lblErrorClubcardNumber&PersonalDetailsDoesnotMatch").ToString(); 
                        sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["FinalPage"].ToString().Trim();
                    }

                }
                if (Request.QueryString["FinalPage"] == null)
                {
                    if (Request.QueryString["ErrorMsg"] != null)
                    {
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "ZeroFields")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnableFields").ToString();
                            sPageName = "Default.aspx";
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "FirstName")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnterValidFirstName").ToString(); 
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "Surname")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnterValidSurname").ToString();
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "PostCode")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnterValidPostCode").ToString();
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "HouseNo")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnterValidHouseName").ToString(); 
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "ClubcardService")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorClubcardService").ToString();
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "ActivationService")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorActivationService").ToString();
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }
                        if (Request.QueryString["ErrorMsg"].ToString().Trim() == "InvalidDOB")
                        {
                            spanmsg.InnerText = GetLocalResourceObject("lblErrorEnterValidDOB").ToString();
                            sPageName = "Verification.aspx?Existing=true&ErrorMsg=" + Request.QueryString["ErrorMsg"].ToString().Trim();
                        }

                    }
                    
                }

            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " MessageForAddress.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
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
                Logger.Write(ex.Message + " MessageForAddress.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
