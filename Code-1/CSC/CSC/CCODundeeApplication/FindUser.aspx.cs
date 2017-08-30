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
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.AdminService;
using System.Xml;
using System.Globalization;
namespace CCODundeeApplication
{
    public partial class FindUser : System.Web.UI.Page
    {
        protected CustomerServiceClient customerClient = null;
        protected AdminServiceClient adminClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        Label lblLastUpdatedBy, lblLastUpdatedDate;

        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Hide the links
                Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                lblCustomerDtl.Visible = false;
                Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                lblCustomePref.Visible = false;
                Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                lblCustomerPts.Visible = false;
                Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                lblCustomerCards.Visible = false;
                Label lblMergerCards = (Label)Master.FindControl("lblMergeCards");
                lblMergerCards.Visible = false;
                Label lblAddPoints = (Label)Master.FindControl("lblAddPoints");
                lblAddPoints.Visible = false;
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                Label lblViewPoints = (Label)Master.FindControl("lblViewPoints");
                lblViewPoints.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                Label lblLastUpdatedBy = (Label)Master.FindControl("lblLastUpdatedBy");
                Label lblLastUpdatedDate = (Label)Master.FindControl("lblLastUpdatedDate");
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                
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
                        HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = false;
            lblSuccessMessage.Text = string.Empty;
            if (!(string.IsNullOrEmpty(txtDescription.Text)) || !(string.IsNullOrEmpty(txtUserName.Text)) || !(string.IsNullOrEmpty(txtGroup.Text)))
            {
                SearchUser(0);
            }
            else
            {
                string AlertMsg = GetLocalResourceObject("AlertMsg.Text").ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + AlertMsg + "');", true);
            }


        }
        public void SearchUser(int PageNumber)
        {
            try
            {
                customerClient = new CustomerServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                htCustomer = new Hashtable();
                htCustomer["UserName"] = txtUserName.Text.Trim();
                htCustomer["UserDescription"] = txtDescription.Text.Trim();
                htCustomer["RoleNameEnglish"] = txtGroup.Text.Trim();
                conditionXml = Helper.HashTableToXML(htCustomer, "ApplicationUser");
                maxRows = 200;
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.SearchUser()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.SearchUser() Input Xml-" + conditionXml);
                #endregion
                if (customerClient.SearchUser(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, "en-GB"))
                {
                    if (resultXml.Contains("UniqueConstraint UserName"))
                        lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg1.Text").ToString() + txtUserName.Text + GetLocalResourceObject("UniqueConMsg2.Text").ToString();
                    //lblSuccessMessage.Text = "User " + txtUserName.Text + " Already Exists.";
                    else
                    {
                        dvSearchResults.Visible = true;
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsUsers = new DataSet();
                        dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                        if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                        {
                            ContentPlaceHolder cntplace = (ContentPlaceHolder)Master.FindControl("LastUpdatedLeftNav");
                            cntplace.Visible = true;
                            Label lblLastUpdatedBy = (Label)cntplace.FindControl("lblLastUpdatedBy");
                            Label lblLastUpdatedDate = (Label)cntplace.FindControl("lblLastUpdatedDate");

                            lblLastUpdatedBy.Text = dsUsers.Tables[0].Rows[0][5].ToString();
                            DateTime LastUpdateDate = Convert.ToDateTime(dsUsers.Tables[0].Rows[0][4].ToString());
                            lblLastUpdatedDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));

                            grdCustomerDetail.DataSource = dsUsers.Tables[0].DefaultView;
                            grdCustomerDetail.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                            grdCustomerDetail.DataBind();
                        }
                        else
                        {
                            this.DataBind();
                        }

                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.SearchUser()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.SearchUser() Input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.SearchUser() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.SearchUser() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.SearchUser()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (customerClient != null)
                {
                    if (customerClient.State == CommunicationState.Faulted)
                    {
                        customerClient.Abort();
                    }
                    else if (customerClient.State != CommunicationState.Closed)
                    {
                        customerClient.Close();
                    }
                }
            }
        }
        protected void GrdCustomerDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.GrdCustomerDetail_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.GrdCustomerDetail_RowCommand()");
                #endregion
                if (e.CommandName == "Select")
                {
                    dvFindUser.Visible = false;
                    dvEditUser.Visible = true;
                    string[] EditUserDetails = e.CommandArgument.ToString().Split(';');
                    txtEditUserName.Text = EditUserDetails[1].ToString();
                    txtEditDescription.Text = EditUserDetails[2].ToString();
                    string lastUpdateBy = EditUserDetails[4].ToString();
                    string lastUpdatedDate = EditUserDetails[5].ToString();

                    Helper.SetTripleDESEncryptedCookie("lblLastUpdatedBy", lastUpdateBy);
                    Helper.SetTripleDESEncryptedCookie("lblLastUpdatedDate", lastUpdatedDate);

                    //lblLastUpdatedBy = (Label)Page.Master.FindControl("lblLastUpdatedBy");
                    //lblLastUpdatedBy.Text = lastUpdateBy;

                    long UserID = long.Parse(EditUserDetails[0].ToString());
                    ViewState["UserID"] = UserID;
                    if (EditUserDetails[3].ToString() == "1")
                    {
                        rbtnEnabled.Checked = true;
                        rbtnDisabled.Checked = false;
                    }
                    else
                    {
                        rbtnEnabled.Checked = false;
                        rbtnDisabled.Checked = true;
                    }
                    ViewRoleMembership(UserID, "en-GB", 0);
                }
                else
                {
                    //for Other actions 
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.GrdCustomerDetail_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.GrdCustomerDetail_RowCommand()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.GrdCustomerDetail_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (customerClient != null)
                {
                    if (customerClient.State == CommunicationState.Faulted)
                    {
                        customerClient.Abort();
                    }
                    else if (customerClient.State != CommunicationState.Closed)
                    {
                        customerClient.Close();
                    }
                }
            }

        }

        protected void grdCustomerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.grdCustomerDetail_RowDataBound()");
                #endregion

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Literal ltrUpdatedDate = (Literal)e.Row.FindControl("ltrUpdatedDate");
                    Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                    if (ltrUpdatedDate.Text != null)
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(ltrUpdatedDate.Text.ToString());
                        ltrUpdatedDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));
                        //ltrUpdatedDate.Text = Convert.ToDateTime(ltrUpdatedDate.Text).ToString("dd/MM/yy");

                    }
                    if (ltrStatus.Text == "1")
                    {
                        ltrStatus.Text = "Enabled";
                    }
                    else
                        ltrStatus.Text = "Disabled";

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.grdCustomerDetail_RowDataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdCustomerDetail_RowDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdCustomerDetail_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        /// <summary>
        /// Fetch All Groups From DataBase
        /// </summary>
        public void GetGroupDetails(long userID)
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.GetGroupDetails()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.GetGroupDetails() User ID-" + userID);
            #endregion
            try
            {
                customerClient = new CustomerServiceClient();
                dsGroups = new DataSet();

                htCustomer = new Hashtable();
                htCustomer["UserID"] = userID;
                string insertxml = Helper.HashTableToXML(htCustomer, "Capability");
                if (customerClient.GetGroupDetails(out errorXml, out resultXml, insertxml, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsGroups.ReadXml(new XmlNodeReader(resulDoc));
                    FillGroupDropDown(dsGroups);
                }
                else
                {
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.GetGroupDetails() User ID-" + userID);
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.GetGroupDetails() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.GetGroupDetails() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.GetGroupDetails()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

            }
            finally
            {
                if (customerClient != null)
                {
                    if (customerClient.State == CommunicationState.Faulted)
                    {
                        customerClient.Abort();
                    }
                    else if (customerClient.State != CommunicationState.Closed)
                    {
                        customerClient.Close();
                    }
                }
            }

        }

        #region Fill Groups Drop Down

        public void FillGroupDropDown(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlGroups.DataMember = "RoleNameEnglish";
                ddlGroups.DataTextField = "RoleNameEnglish";
                ddlGroups.DataValueField = "RoleID";
                ddlGroups.DataSource = ds.Tables[0];
                ddlGroups.DataBind();
            }
        }
        #endregion
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdRoleMembership_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            adminClient = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            long objectid = 0;

            string objectXml = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.grdRoleMembership_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.grdRoleMembership_RowCommand()");
                #endregion

                if (e.CommandName == "Delete")
                {
                    string[] DeleteDetails = e.CommandArgument.ToString().Split(';');
                    htCustomer = new Hashtable();
                    htCustomer["UserID"] = DeleteDetails[1].ToString();
                    htCustomer["RoleID"] = DeleteDetails[0].ToString();
                    objectXml = Helper.HashTableToXML(htCustomer, "ApplicationUser");
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {

                        if (adminClient.DeleteRoleMembership(out objectid, out resultXml, objectXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {

                            ViewRoleMembership(Convert.ToInt64(DeleteDetails[1].ToString()), "en-GB", 0);
                            GetGroupDetails(Convert.ToInt64(DeleteDetails[1].ToString()));
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }


                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.grdRoleMembership_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.grdRoleMembership_RowCommand()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdRoleMembership_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdRoleMembership_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdRoleMembership_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (adminClient != null)
                {
                    if (adminClient.State == CommunicationState.Faulted)
                    {
                        adminClient.Abort();
                    }
                    else if (adminClient.State != CommunicationState.Closed)
                    {
                        adminClient.Close();
                    }
                }
            }
        }


        protected void grdRoleMembership_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnkDeleteUserRole");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('" + GetLocalResourceObject("AlertRoleMsg.Text") +
                DataBinder.Eval(e.Row.DataItem, "RoleName") + "')");

            }
        }

        protected void grdRoleMembership_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        /// <summary>
        /// Function to View Role Membership
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Culture"></param>
        public void ViewRoleMembership(long UserID, string Culture, int PageNumber)
        {

            adminClient = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.ViewRoleMembership()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.ViewRoleMembership()");
                #endregion
                if (adminClient.ViewRoleMembership(out errorXml, out resultXml, UserID, Culture))
                {
                    GetGroupDetails(UserID);
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables["ApplicationUser"] != null && dsUsers.Tables["ApplicationUser"].Rows.Count > 0)
                    {
                        dvSearchResults.Visible = true;
                        grdRoleMembership.DataSource = dsUsers.Tables[0].DefaultView;
                        grdRoleMembership.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdRoleMembership.DataBind();

                    }
                    else
                    {
                        this.DataBind();
                    }
                }
                else
                    this.DataBind();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.ViewRoleMembership()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.ViewRoleMembership()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ViewRoleMembership() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ViewRoleMembership() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ViewRoleMembership()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (adminClient != null)
                {
                    if (adminClient.State == CommunicationState.Faulted)
                    {
                        adminClient.Abort();
                    }
                    else if (adminClient.State != CommunicationState.Closed)
                    {
                        adminClient.Close();
                    }
                }
            }

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                adminClient = new AdminServiceClient();
                long objectid = 0;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["UserID"] = ViewState["UserID"].ToString();
                htCustomer["RoleID"] = ddlGroups.SelectedValue.ToString();
                string addUserXml = Helper.HashTableToXML(htCustomer, "ApplicationUser");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.btnAdd_Click() Input Xml-" + addUserXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {

                    if (adminClient.AddRoleMembership(out objectid, out resultXml, out errorXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        if (resultXml.Contains("UniqueConstraint RoleID"))
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Confirmation", "alert('" + GetLocalResourceObject("ExistMsg.Text") + "');", true);
                        else
                        {
                            ViewRoleMembership(Convert.ToInt64(ViewState["UserID"].ToString()), "en-GB", 0);
                            GetGroupDetails(Convert.ToInt64(ViewState["UserID"].ToString()));
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.btnAdd_Click() Input Xml-" + addUserXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.btnAdd_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.btnAdd_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.btnAdd_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (adminClient != null)
                {
                    if (adminClient.State == CommunicationState.Faulted)
                    {
                        adminClient.Abort();
                    }
                    else if (adminClient.State != CommunicationState.Closed)
                    {
                        adminClient.Close();
                    }
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                customerClient = new CustomerServiceClient();
                long objectid = 0;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["UserID"] = ViewState["UserID"].ToString();
                htCustomer["UserName"] = txtEditUserName.Text;
                htCustomer["UserDescription"] = txtEditDescription.Text;
                if (rbtnEnabled.Checked) { htCustomer["UserStatusCode"] = "1"; } else if (rbtnDisabled.Checked) { htCustomer["UserStatusCode"] = "0"; }
                htCustomer["EmailAddress"] = string.Empty;
                string addUserXml = Helper.HashTableToXML(htCustomer, "ApplicationUser");

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.ImageButton1_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.ImageButton1_Click() Input Xml-" + addUserXml);
                #endregion

                if (customerClient.Update(out objectid, out resultXml, out errorXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                {
                    if (resultXml.Contains("UniqueConstraint UserName"))
                        lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg1.Text").ToString() + txtUserName.Text + GetLocalResourceObject("UniqueConMsg2.Text").ToString();
                    else
                    {
                        dvSearchResults.Visible = false;
                        dvEditUser.Visible = false;
                        dvFindUser.Visible = true;
                        lblSuccessMessage.Text = GetLocalResourceObject("SuccessMsg.Text").ToString();
                        //lblSuccessMessage.Text = "Your changes have been saved successfuly.";


                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.ImageButton1_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.ImageButton1_Click() Input Xml-" + addUserXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ImageButton1_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ImageButton1_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ImageButton1_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (customerClient != null)
                {
                    if (customerClient.State == CommunicationState.Faulted)
                    {
                        customerClient.Abort();
                    }
                    else if (customerClient.State != CommunicationState.Closed)
                    {
                        customerClient.Close();
                    }
                }
            }


        }

        protected void grdCustomerDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SearchUser(e.NewPageIndex);
        }

        protected void grdRoleMembership_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewRoleMembership(Convert.ToInt64(ViewState["UserID"]), "en-GB", e.NewPageIndex);
        }
    }
}
