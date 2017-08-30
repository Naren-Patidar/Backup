using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Data;

namespace CCODundeeApplication.Reports
{
    public partial class AccountReportHomePage : System.Web.UI.Page
    {
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                lblCustomerDtl.Visible = false;
                Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                lblCustomePref.Visible = false;
                Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                lblCustomerPts.Visible = false;
                Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                lblCustomerCards.Visible = false;
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                Label lblViewPoints = (Label)Master.FindControl("lblViewPoints");
                lblViewPoints.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                Label lblLastUpdatedBy = (Label)Master.FindControl("lblLastUpdatedBy");
                Label lblLastUpdatedDate = (Label)Master.FindControl("lblLastUpdatedDate");

                #region RoleCapabilityImplementation
                xmlCapability = new XmlDocument();
                dsCapability = new DataSet();

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    if (dsCapability.Tables.Count > 0)
                    {
                        HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                        HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                        HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                        HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                        HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                        HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                        HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                        HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                        HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                        HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                        HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                        Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");

                        if (dsCapability.Tables[0].Columns.Contains("AccountOperationReports") != false)
                        {
                            AccountOperationReports.Disabled = false;
                        }
                        else
                        {
                            AccountOperationReports.Disabled = true;
                            AccountOperationReports.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("PromotionalCodeReport") != false)
                        {
                            PromotionalCodeReport.Disabled = false;
                        }
                        else
                        {
                            PromotionalCodeReport.Disabled = true;
                            PromotionalCodeReport.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("CustomerLoadReport") != false)
                        {
                            lnkCustomerLoadReport.Disabled = false;
                        }
                        else
                        {
                            lnkCustomerLoadReport.Disabled = true;
                            lnkCustomerLoadReport.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("AccountOperationReports") != false)
                        {
                            lnkClubcardRegistrationReport.Disabled = false;
                        }
                        else
                        {
                            lnkClubcardRegistrationReport.Disabled = true;
                            lnkClubcardRegistrationReport.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("CreateNewCustomer") != false)
                        {
                            Join.Disabled = false;
                        }
                        else
                        {
                            Join.Disabled = true;
                            Join.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("AddUser") != false)
                        {
                            AddUser.Disabled = false;
                        }
                        else
                        {
                            AddUser.Disabled = true;
                            AddUser.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("AddGroup") != false)
                        {
                            AddGroup.Disabled = false;
                        }
                        else
                        {
                            AddGroup.Disabled = true;
                            AddGroup.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("FindGroup") != false)
                        {
                            FindGroup.Disabled = false;
                        }
                        else
                        {
                            FindGroup.Disabled = true;
                            FindGroup.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("finduser") != false)
                        {
                            FindUser.Disabled = false;
                        }
                        else
                        {
                            FindUser.Disabled = true;
                            FindUser.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("editcardranges") != false)
                        {
                            CardRange.Disabled = false;
                        }
                        else
                        {
                            CardRange.Disabled = true;
                            CardRange.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("editcardtypes") != false)
                        {
                            CardTypes.Disabled = false;
                        }
                        else
                        {
                            CardTypes.Disabled = true;
                            CardTypes.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("editstores") != false)
                        {
                            Stores.Disabled = false;
                        }
                        else
                        {
                            Stores.Disabled = true;
                            Stores.HRef = "";
                        }
                    }

                    ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                    leftNav.Visible = false;
                }
                #endregion
            }
        }
        #region Initialize the culture


        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture"));
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion
    }
}
