using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using Resources;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class ErrorMessage : BaseUIPage
    {
        protected string pageName = string.Empty;
        protected string controlID = string.Empty;
        protected string resourceID = string.Empty;
        protected string errorMsg = string.Empty;
        protected string sPageName = string.Empty;
        protected string imageUrl = string.Empty;

        private string postcode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            pageName = Request.QueryString["PgName"].ToString();
            controlID = Request.QueryString["ctrlID"].ToString();
            resourceID = Request.QueryString["resID"].ToString();
            imageUrl = Request.QueryString["imgID"].ToString();

            if (controlID.ToString().ToUpperInvariant().Trim() == "FINAL")
            {
               Back.Visible = false;
               Helper.DeleteAllCookies();
            }
            else
            {
                CancelAndStartAgain.Visible = false;
            }
            hdnImageUrl.Value = GetResourceValue(imageUrl);
            errorMsg = GetResourceValue(resourceID);
            spanmsg.InnerHtml = errorMsg;
            sPageName = pageName + ".aspx?ctrlID=" + controlID;

            if (Request.QueryString["PostCode"] != null)
            {
                postcode = Request.QueryString["PostCode"].ToString();

                spanmsg.InnerHtml = GlobalResources.AddressNotFound.ToString() + " '" + Request.QueryString["PostCode"].ToString().Trim() + "'" + GlobalResources.EnterManually.ToString();
                sPageName = pageName + ".aspx?ctrlID=" + controlID + "&PostCode=" + postcode;
            }
            if (Request.QueryString["InValidPostCode"] != null)
            {
                postcode = Request.QueryString["InValidPostCode"].ToString();

                spanmsg.InnerHtml = GlobalResources.InvalidPostcode + " '" + Request.QueryString["InValidPostCode"].ToString().Trim() + "'" + GlobalResources.InvalidPostcode1;
                sPageName = pageName + ".aspx?ctrlID=" + controlID + "&InValidPostCode=" + postcode;
            }
        }
        private string GetResourceValue(string resourceid)
        {
            string resourceValue = GetGlobalResourceObject("GlobalResources", resourceid).ToString();
            return resourceValue;
        }
    }
}