using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Web.UI.HtmlControls;

namespace CCODundeeApplication
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  DataSet dsCapability = null;
      
            XmlDocument xmlCapability = null;
              #region RoleCapabilityImplementation
                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();

                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                        {
                            xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                            dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                            if (dsCapability.Tables.Count > 0)
                            {
                                HtmlAnchor findCustomer = (HtmlAnchor)Master.FindControl("findCustomer");
                                HtmlAnchor cutomerDetails = (HtmlAnchor)Master.FindControl("cutomerDetails");
                                HtmlAnchor customerPreferences = (HtmlAnchor)Master.FindControl("customerPreferences");
                                HtmlAnchor customerPoints = (HtmlAnchor)Master.FindControl("customerPoints");
                                HtmlAnchor customerCards = (HtmlAnchor)Master.FindControl("customerCards");
                                HtmlAnchor christmasSaver = (HtmlAnchor)Master.FindControl("christmasSaver");
                                HtmlAnchor aAdmin = (HtmlAnchor)Master.FindControl("aAdmin");
                                HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                                HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                                HtmlAnchor agroups = (HtmlAnchor)Master.FindControl("agroups");
                                HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                                HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                                PlaceHolder plAdmin = (PlaceHolder)Master.FindControl("plAdmin");
                                HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                                HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                                HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                                HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                                HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                                HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                                //Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
                                HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                                HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                                HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                                HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                                HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                                HtmlAnchor DeLinkAccount = (HtmlAnchor)Master.FindControl("DelinkAccounts");
                                Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");

                                if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                                {
                                    PointsEarnedReports.Disabled = false;
                                }
                                else
                                {
                                    PointsEarnedReports.Disabled = true;
                                    PointsEarnedReports.HRef = "";
                                }
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

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                                {
                                    customerCoupon.Disabled = false;
                                }
                                else
                                {
                                    customerCoupon.Disabled = true;
                                    customerCoupon.HRef = "";
                                }


                                if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                                {
                                    DeLinkAccount.Visible = true;
                                }
                                else
                                {
                                    DeLinkAccount.Visible = false;
                                    DeLinkAccount.HRef = "";
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

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerPreferences") != false)
                                {
                                    customerPreferences.Disabled = false;
                                }
                                else
                                {
                                    customerPreferences.Disabled = true;

                                }

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerPoints") != false)
                                {
                                    customerPoints.Disabled = false;
                                }
                                else
                                {
                                    customerPoints.Disabled = true;
                                    customerPoints.HRef = "";
                                }

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCards") != false)
                                {
                                    customerCards.Disabled = false;
                                }
                                else
                                {
                                    customerCards.Disabled = true;
                                    customerCards.HRef = "";
                                }

                                if (dsCapability.Tables[0].Columns.Contains("ViewChristmasSaver") != false)
                                {
                                    christmasSaver.Disabled = false;
                                }
                                else
                                {
                                    christmasSaver.Disabled = true;
                                    christmasSaver.HRef = "";
                                }


                                //NGC COde

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                                {
                                    cutomerDetails.Disabled = false;
                                }
                                else
                                {
                                    cutomerDetails.Disabled = true;
                                    cutomerDetails.HRef = "";
                                }

                                if (dsCapability.Tables[0].Columns.Contains("resetpassword") != false)
                                {
                                    ResetPass.Disabled = false;
                                }
                                else
                                {
                                    ResetPass.Disabled = true;
                                    ResetPass.HRef = "";
                                }

                                if (dsCapability.Tables[0].Columns.Contains("viewpoints") != false)
                                {
                                    viewpoints.Disabled = false;
                                }
                                else
                                {
                                    viewpoints.Disabled = true;
                                    viewpoints.HRef = "";
                                }


                            }
                        }
                        #endregion
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
