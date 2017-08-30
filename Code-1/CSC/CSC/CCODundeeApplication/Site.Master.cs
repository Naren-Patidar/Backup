using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using CCODundeeApplication.AdminService;
using CCODundeeApplication.CustomerService;

namespace CCODundeeApplication
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            ShowCustomerDetails();
        }

        /// <summary>
        /// Method to show customer details.
        /// </summary>
        private void ShowCustomerDetails()
        {
            if (string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserCapability"))
                || string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserName"))
                || string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserID"))
                || string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("Culture")))
            {
                Response.Redirect("Default.aspx", false);
            }
            else //if (!string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("lblCardNo")))
            {
                Helper.CheckAndResetCookieExpiration("UserName");
                Helper.CheckAndResetCookieExpiration("UserCapability");
                Helper.CheckAndResetCookieExpiration("CustomerID");
                Helper.CheckAndResetCookieExpiration("UserID");
                Helper.CheckAndResetCookieExpiration("Culture");

                lblName.Text = Helper.CheckAndResetCookieExpiration("lblName").ToString();
                lblCardNo.Text = Helper.CheckAndResetCookieExpiration("lblCardNo").ToString();
                lblHouseholdID.Text = Helper.CheckAndResetCookieExpiration("lblHouseholdID").ToString();
                lblCurrPoints.Text = Helper.CheckAndResetCookieExpiration("lblCurrPoints").ToString();
                lblJoinedDate.Text = Helper.CheckAndResetCookieExpiration("JoinedDate").ToString();
                lblRouteCode.Text = Helper.CheckAndResetCookieExpiration("lblJoinRouteID").ToString();
                lblPromotionalCode1.Text = Helper.CheckAndResetCookieExpiration("lblPromotionalCode").ToString();
                lblCustType.Text = Helper.CheckAndResetCookieExpiration("lblCustType").ToString();
                lblLastUpdatedBy.Text = Helper.CheckAndResetCookieExpiration("lblLastUpdatedBy").ToString();
                lblLastUpdatedDate.Text = Helper.CheckAndResetCookieExpiration("lblLastUpdatedDate").ToString();
                //Added as a part of Group CR phase CR12
                lblCustomerLastUpdatedBy.Text = Helper.CheckAndResetCookieExpiration("lblCustomerLastAmendedBy").ToString() + " @ " + Helper.CheckAndResetCookieExpiration("lblCustomerLastAmendDate").ToString();
                //******** Group CR phase1 CR12 ********


                //this.lclFindUser.Text = GetGlobalResourceObject("CSCGlobal", "lnkFindUser").ToString();
                if ((Request.Url.AbsoluteUri.ToUpper().Contains("FINDUSER.ASPX")) || (Request.Url.AbsoluteUri.ToUpper().Contains("ADDUSER.ASPX"))
                        || (Request.Url.AbsoluteUri.ToUpper().Contains("ADDGROUP.ASPX")) || (Request.Url.AbsoluteUri.ToUpper().Contains("FINDGROUP.ASPX"))
                        || (Request.Url.AbsoluteUri.ToUpper().Contains("JOIN.ASPX")) || (Request.Url.AbsoluteUri.ToUpper().Contains("CARDRANGES.ASPX"))
                        || (Request.Url.AbsoluteUri.ToUpper().Contains("CARDTYPE.ASPX")) || (Request.Url.AbsoluteUri.ToUpper().Contains("TESCOSTORE.ASPX")))
                {
                    divCustDetailsLeftNav.Visible = false;

                }
                else
                {
                    divCustDetailsLeftNav.Visible = true;

                }
                HtmlAnchor christmasSaver = (HtmlAnchor)FindControl("christmasSaver");
                if (lblCustType.Text != "Christmas saver")
                {
                    lblXmasSaver.Visible = false;
                }

                #region RoleCapabilityImplementation
                //DataSet dsCapability = null;
                //XmlDocument xmlCapability = null;
                //xmlCapability = new XmlDocument();
                //dsCapability = new DataSet();

                //if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                //{
                //    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                //    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                //    if (dsCapability.Tables.Count > 0)
                //    {
                //        HtmlAnchor DataConfig = (HtmlAnchor)FindControl("dataconfig");
                //        Control link = (HtmlGenericControl)FindControl("liDataconfig");

                //        if (dsCapability.Tables[0].Columns.Contains("ViewDataConfiguaration") != false)
                //        {
                //            link.Visible = true;

                //        }
                //        else
                //        {
                //            link.Visible = false;
                //        }
                //    }
                //}
                #endregion

                #region RoleCapabilityImplementation
                DataSet dsCapability = null;
                XmlDocument xmlCapability = null;
                xmlCapability = new XmlDocument();
                dsCapability = new DataSet();

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    HtmlAnchor DataConfig = (HtmlAnchor)FindControl("dataconfig");
                    Control link = (HtmlGenericControl)FindControl("liDataconfig");

                    if (dsCapability.Tables.Count > 0)
                    {
                        HtmlAnchor findCustomer = (HtmlAnchor)FindControl("findCustomer");
                        HtmlAnchor cutomerDetails = (HtmlAnchor)FindControl("cutomerDetails");
                        HtmlAnchor customerPreferences = (HtmlAnchor)FindControl("customerPreferences");
                        HtmlAnchor customerPoints = (HtmlAnchor)FindControl("customerPoints");
                        HtmlAnchor customerCards = (HtmlAnchor)FindControl("customerCards");
                        christmasSaver = (HtmlAnchor)FindControl("christmasSaver");
                        HtmlAnchor customerCoupon = (HtmlAnchor)FindControl("customerCoupon");
                        HtmlAnchor aAdmin = (HtmlAnchor)FindControl("aAdmin");
                        HtmlAnchor FindUser = (HtmlAnchor)FindControl("FindUser");
                        HtmlAnchor AddUser = (HtmlAnchor)FindControl("AddUser");
                        HtmlAnchor agroups = (HtmlAnchor)FindControl("agroups");
                        HtmlAnchor FindGroup = (HtmlAnchor)FindControl("FindGroup");
                        HtmlAnchor AddGroup = (HtmlAnchor)FindControl("AddGroup");
                        PlaceHolder plAdmin = (PlaceHolder)FindControl("plAdmin");
                        HtmlAnchor viewpoints = (HtmlAnchor)FindControl("viewpoints");
                        HtmlAnchor AddPoints = (HtmlAnchor)FindControl("AddPoints");
                        HtmlAnchor Join = (HtmlAnchor)FindControl("Join");
                        HtmlAnchor ResetPass = (HtmlAnchor)FindControl("resetpass");
                        HtmlAnchor CardRange = (HtmlAnchor)FindControl("CardRange");
                        HtmlAnchor CardTypes = (HtmlAnchor)FindControl("CardType");
                        HtmlAnchor Stores = (HtmlAnchor)FindControl("Stores");
                        Label DeLinkAccount = (Label)FindControl("lblDelinking");
                        HtmlAnchor AccountOperationReports = (HtmlAnchor)FindControl("AccReports");
                        HtmlAnchor PromotionalCodeReport = (HtmlAnchor)FindControl("PromotionalCode");
                        HtmlAnchor PointsEarnedReports = (HtmlAnchor)FindControl("PointsEarnedReport");
                        HtmlAnchor findCoupon = (HtmlAnchor)FindControl("findCoupon");
                        HtmlAnchor Partners = (HtmlAnchor)FindControl("Partners");
						HtmlAnchor PromotionCode = (HtmlAnchor)FindControl("Promotioncode");
                        HtmlAnchor MergeCard = (HtmlAnchor)FindControl("MergeCard");
                        //Group CR12
                        HtmlAnchor UserNotes = (HtmlAnchor)FindControl("UserNotes");
                        //Group CR12


                        if (dsCapability.Tables[0].Columns.Contains("MergeCards") != false)
                        {
                            MergeCard.Disabled = false;
                        }
                        else
                        {
                            MergeCard.Disabled = true;
                            MergeCard.HRef = "";
                        }

                        if (dsCapability.Tables[0].Columns.Contains("editpartner") != false)
                        {
                            Partners.Disabled = false;
                        }
                        else
                        {
                            Partners.Disabled = true;
                            Partners.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("SearchCoupons") != false)
                        {
                            findCoupon.Disabled = false;
                        }
                        else
                        {
                            findCoupon.Disabled = true;
                            findCoupon.HRef = "";
                        }
						 if (dsCapability.Tables[0].Columns.Contains("Promotioncode") != false)
                        {
                            PromotionCode.Disabled = false;
                        }
                        else
                        {
                            PromotionCode.Disabled = true;
                            PromotionCode.HRef = "";
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

                        if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                        {
                            DeLinkAccount.Visible = true;
                        }
                        else
                        {
                            DeLinkAccount.Visible = false;
                           
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
                        //AddPoints CR14 modified BY Lakshmi

                        if (dsCapability.Tables[0].Columns.Contains("AddPoints") != false)
                        {
                            AddPoints.Disabled = false;
                        }
                        else
                        {
                            AddPoints.Disabled = true;
                            AddPoints.HRef = "";
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
                        if (dsCapability.Tables[0].Columns.Contains("ViewDataConfiguaration") != false)
                        {
                            link.Visible = true;
                        }
                        else
                        {
                            link.Visible = false;
                        }
                        //Group CR12
                        if (dsCapability.Tables[0].Columns.Contains("UserNotes") != false)
                        {
                            UserNotes.Disabled = false;
                        }
                        else
                        {
                            UserNotes.Disabled = true;
                            UserNotes.HRef = "";
                        }
                        //Group CR12
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// Logout button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //Delete all the cookies.
            if (Helper.GetTripleDESEncryptedCookieValue("UserCapability") != null)
            {
                Helper.DeleteTripleDESEncryptedCookie("UserCapability");
            }
            if (Helper.GetTripleDESEncryptedCookieValue("CustomerID") != null)
            {
                Helper.DeleteTripleDESEncryptedCookie("CustomerID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblCardNo")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblCardNo");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblHouseholdID")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblHouseholdID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblCurrPoints")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblCurrPoints");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("JoinedDate")))
            {
                Helper.DeleteTripleDESEncryptedCookie("JoinedDate");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblJoinRouteID")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblJoinRouteID");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblPromotionalCode")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblPromotionalCode");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblCustType")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblCustType");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserName")))
            {
                Helper.DeleteTripleDESEncryptedCookie("UserName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                Helper.DeleteTripleDESEncryptedCookie("Culture");
            }
            //Added as a part of Group CR phase CR12
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblCustomerLastAmendedBy")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblCustomerLastAmendedBy");
            }

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("lblCustomerLastAmendDate")))
            {
                Helper.DeleteTripleDESEncryptedCookie("lblCustomerLastAmendDate");
            }
            //******** Group CR phase1 CR12 ********
           
            Response.Redirect("~/Default.aspx", false);
        }

        /// <summary>
        /// Handle page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!String.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString()))
            {
                string Culture = Helper.GetTripleDESEncryptedCookieValue("Culture");
                if (Culture != "en-GB")
                {
                    FlagLogo.ImageUrl = "~/I/" + ConfigurationSettings.AppSettings["LoginFlagImageURL"].ToString();
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx", false);
            }
        }

    }
}
