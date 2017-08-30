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
    public partial class FindGroup : System.Web.UI.Page
    {

        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected CustomerServiceClient customerClient = null;
        protected AdminServiceClient adminClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        protected string errMsgAddLimit = string.Empty;
        protected string errMsgSubLimit = string.Empty;
        protected string errMsgGroupName = string.Empty;
        protected string spanAddLimit = "display:none";
        protected string spanSubLimit = "display:none";
        protected string spanGroupName = "display:none";
        protected string spanValidName = "display:none";
        protected string errMsgvalid = string.Empty;
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
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                GetConfigDetails();
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
                        if (dsCapability.Tables[0].Columns.Contains("changepointslimit") != false)
                        {
                            changepointslimit.Visible = true;
                            dvChangePointsLimit.Visible = true;
                            ImgSave.Visible = true;
                        }
                        else
                        {
                            changepointslimit.Visible = false;
                            dvChangePointsLimit.Visible = false;
                            ImgSave.Visible = false;
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
            if (!(string.IsNullOrEmpty(txtDescription.Text)) || !(string.IsNullOrEmpty(txtGroupName.Text)))
            {

                SearchGroup(txtDescription.Text, txtGroupName.Text, 0);
            }
            else
            {
                string errMSg = GetLocalResourceObject("errMsg.Text").ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + errMSg + "');", true);
            }
        }

        public void SearchGroup(string RoleDesc, string RoleName, int PageNumber)
        {
            try
            {
                adminClient = new AdminServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                htCustomer = new Hashtable();
                htCustomer["RoleName"] = RoleName;
                htCustomer["RoleDesc"] = RoleDesc;

                conditionXml = Helper.HashTableToXML(htCustomer, "Role");
                maxRows = 200;
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.SearchGroup()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.SearchGroup() Input Xml-" + conditionXml);
                #endregion
                if (adminClient.SearchRole(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, "en-GB"))
                {
                    if (resultXml.Contains("UniqueConstraint UserName"))
                        //lblSuccessMessage.Text = "Group " + txtGroupName.Text + " Already Exists.";
                        lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString() + txtEditGroupName.Text + GetLocalResourceObject("UniqueConsMsg2.Text").ToString();
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

                            lblLastUpdatedBy.Text = dsUsers.Tables[0].Rows[0][6].ToString();
                            DateTime LastUpdateDate = Convert.ToDateTime(dsUsers.Tables[0].Rows[0][5].ToString());
                            lblLastUpdatedDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));
                            //lblLastUpdatedDate.Text = Convert.ToDateTime(dsUsers.Tables[0].Rows[0][5].ToString()).ToShortDateString();

                            dvEditUser.Visible = false;
                            grdGroupDetails.DataSource = dsUsers.Tables[0].DefaultView;
                            grdGroupDetails.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                            grdGroupDetails.DataBind();
                        }
                        else
                        {
                            this.DataBind();
                        }

                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.SearchGroup()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.SearchGroup() Input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.SearchGroup() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.SearchGroup() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.SearchGroup()");
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

        protected void grdGroupDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.grdGroupDetails_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.grdGroupDetails_RowCommand()");
                #endregion

                if (e.CommandName == "Select")
                {
                    lblSuccessMessage.Text = string.Empty;
                    dvFindUser.Visible = false;
                    dvEditUser.Visible = true;
                    string[] EditUserDetails = e.CommandArgument.ToString().Split(';');
                    txtEditGroupName.Text = EditUserDetails[1].ToString();
                    txtEditGroupDescription.Text = EditUserDetails[2].ToString();

                    string lastUpdateBy = EditUserDetails[3].ToString();
                    string lastUpdatedDate = EditUserDetails[4].ToString();


                    long RoleID = long.Parse(EditUserDetails[0].ToString());
                    ViewState["RoleID"] = RoleID;
                    ViewRoleCapability(RoleID, "en-GB", 0);
                    GetGroupDetails(RoleID);
                }
                else
                {
                    //for Other actions 
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.grdGroupDetails_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.grdGroupDetails_RowCommand()");
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdGroupDetails_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdGroupDetails_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdGroupDetails_RowCommand()");
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.grdCustomerDetail_RowDataBound()");
                #endregion
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Literal ltrUpdatedDate = (Literal)e.Row.FindControl("ltrUpdatedDate");
                    if (ltrUpdatedDate.Text != null)
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(ltrUpdatedDate.Text.ToString());
                        ltrUpdatedDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));


                    }


                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.grdCustomerDetail_RowDataBound()");
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
        public void GetGroupDetails(long RoleID)
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.GetGroupDetails() Role Id-" + RoleID);
                #endregion


                adminClient = new AdminServiceClient();
                dsGroups = new DataSet();

                htCustomer = new Hashtable();
                htCustomer["RoleID"] = RoleID;
                string insertxml = Helper.HashTableToXML(htCustomer, "Capability");


                if (adminClient.GetCapabilty(out errorXml, out resultXml, insertxml, culture))
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.GetGroupDetails() Role Id-" + RoleID);
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

        #region Fill Groups Drop Down

        public void FillGroupDropDown(DataSet ds)
        {
            if (ds!=null && ds.Tables.Count >0 &&ds.Tables[0]!=null && ds.Tables[0].Rows.Count > 0)
            {
                ddlCapability.DataMember = "CapabilityName";
                ddlCapability.DataTextField = "CapabilityName";
                ddlCapability.DataValueField = "CapabilityID";
                ddlCapability.DataSource = ds.Tables[0];
                ddlCapability.DataBind();
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.grdRoleMembership_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.grdRoleMembership_RowCommand()");
                #endregion
                if (e.CommandName == "Delete")
                {
                    string[] DeleteDetails = e.CommandArgument.ToString().Split(';');
                    htCustomer = new Hashtable();
                    htCustomer["RoleID"] = DeleteDetails[0].ToString();
                    htCustomer["CapabilityID"] = DeleteDetails[1].ToString();
                    objectXml = Helper.HashTableToXML(htCustomer, "RoleCapability");
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (adminClient.RemoveCapability(out objectid, out resultXml, objectXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {
                            ViewRoleCapability(Convert.ToInt64(DeleteDetails[0].ToString()), "en-GB", 0);
                            GetGroupDetails(long.Parse(DeleteDetails[0].ToString()));
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.grdRoleMembership_RowCommand()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.grdRoleMembership_RowCommand()");
                    #endregion
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdRoleMembership_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdRoleMembership_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdRoleMembership_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion
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
            string errDeleteMsg = GetLocalResourceObject("errDeleteMsg.Text").ToString();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnkDeleteUserRole");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('" + errDeleteMsg + DataBinder.Eval(e.Row.DataItem, "RoleName") + "')");

            }
        }

        protected void grdRoleCapability_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        /// <summary>
        /// Function to View Role Membership
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Culture"></param>
        public void ViewRoleCapability(long RoleID, string Culture, int PageNumber)
        {

            adminClient = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.ViewRoleCapability()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.ViewRoleCapability() Role ID-" + RoleID);
                #endregion
                if (adminClient.ViewRoleCapability(out errorXml, out resultXml, RoleID, Culture))
                {

                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables["PointsLimit"] != null)
                    {
                        txtAddLimit.Text = dsUsers.Tables["PointsLimit"].Rows[0][2].ToString();
                        txtSubstract.Text = dsUsers.Tables["PointsLimit"].Rows[0][3].ToString();
                    }
                    if (dsUsers.Tables["RoleCapability"] != null && dsUsers.Tables["RoleCapability"].Rows.Count > 0)
                    {
                        dvSearchResults.Visible = true;
                        grdRoleCapability.DataSource = dsUsers.Tables[0].DefaultView;
                        grdRoleCapability.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdRoleCapability.DataBind();


                    }
                    else
                    {
                        this.DataBind();
                    }
                }
                else
                {
                    this.DataBind();
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.ViewRoleCapability()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.ViewRoleCapability() Role ID-" + RoleID);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ViewRoleCapability() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ViewRoleCapability() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ViewRoleCapability()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion
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
                htCustomer["RoleID"] = ViewState["RoleID"].ToString();
                htCustomer["CapabilityID"] = ddlCapability.SelectedValue.ToString();
                string addUserXml = Helper.HashTableToXML(htCustomer, "RoleCapability");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.btnAdd_Click() Input Xml-" + addUserXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (adminClient.AddRoleCapability(out objectid, out resultXml, addUserXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        if (resultXml.Contains("UniqueConstraint CapibilityID"))
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Confirmation", "alert('" + GetLocalResourceObject("UniqueConsMsgCap.Text").ToString() + "');", true);
                        else
                        {

                            ViewRoleCapability(Convert.ToInt64(ViewState["RoleID"].ToString()), "en-GB", 0);
                            GetGroupDetails(long.Parse(ViewState["RoleID"].ToString()));
                        }
                    }
                }

                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.btnAdd_Click() Input Xml-" + addUserXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.btnAdd_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.btnAdd_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.btnAdd_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion
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

        protected void ImgSave_Click(object sender, ImageClickEventArgs e)
        {
            lblSuccessMessage.Text = string.Empty;
            if (ValidatePage())
            {
                try
                {
                    adminClient = new AdminServiceClient();
                    long objectid = 0;
                    string resultXml = string.Empty;
                    string errorXml = string.Empty;
                    htCustomer = new Hashtable();
                    htCustomer["RoleID"] = ViewState["RoleID"].ToString();
                    htCustomer["RoleName"] = txtEditGroupName.Text;
                    htCustomer["RoleDesc"] = txtEditGroupDescription.Text;
                    htCustomer["RoleAddPointsLimit"] = txtAddLimit.Text;
                    htCustomer["RoleSubtractPointsLimit"] = txtSubstract.Text;
                    string addUserXml = Helper.HashTableToXML(htCustomer, "Role");

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.ImgSave_Click()");
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.ImgSave_Click() Input Xml-" + addUserXml);
                    #endregion
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (adminClient.UpdateRoleDetails(out objectid, out resultXml, out errorXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {
                            if (resultXml.Contains("UniqueConstraint RoleName"))
                                lblSuccessMessage.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString() + txtEditGroupName.Text + GetLocalResourceObject("UniqueConsMsg2.Text").ToString();
                            else
                            {
                                lblSuccessMessage.Text = GetLocalResourceObject("SuccessMSg.Text").ToString();//"Your changes have been saved successfuly.";
                                dvFindUser.Visible = true;
                                dvEditUser.Visible = false;
                                SearchGroup(txtEditGroupDescription.Text, txtEditGroupName.Text, 0);

                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.ImgSave_Click()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.ImgSave_Click() Input Xml-" + addUserXml);
                    #endregion
                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ImgSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                    NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ImgSave_Click() - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ImgSave_Click()");
                    NGCTrace.NGCTrace.ExeptionHandling(exp);
                    #endregion
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
        }



        protected void grdGroupDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }


        /// <summary>
        /// To validate customer details
        /// </summary>
        /// <returns>boolean</returns>
        protected bool ValidatePage()
        {
            try
            {
                //string regNumeric = @"^[0-9]*$";
                string regNumeric = hdnNumericeg.Value;
                bool bErrorFlag = true;

                //Clear the class
                txtAddLimit.CssClass = "";
                txtSubstract.CssClass = "";
                string groupName = txtEditGroupName.Text.ToString().Trim();
                string addLimit = txtAddLimit.Text.ToString().Trim();
                string subLimit = txtSubstract.Text.ToString().Trim();
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindGroup.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindGroup.ValidatePage()");
                #endregion
                if (string.IsNullOrEmpty(groupName))
                {
                    errMsgGroupName = GetLocalResourceObject("errMsgGroupName.Text").ToString();
                    spanGroupName = "";
                    txtEditGroupName.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (!Helper.IsRegexMatch(txtAddLimit.Text.Trim(), regNumeric, false, false))
                {
                    errMsgAddLimit = GetLocalResourceObject("errMsgAddLimit.Text").ToString();// "Please enter a numeric value";
                    spanAddLimit = "";
                    txtAddLimit.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (!Helper.IsRegexMatch(txtSubstract.Text.Trim(), regNumeric, false, false))
                {
                    errMsgSubLimit = GetLocalResourceObject("errMsgSubLimit.Text").ToString(); //"Please enter a numeric value";
                    spanSubLimit = "";
                    txtSubstract.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindGroup.ValidatePage()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindGroup.ValidatePage()");
                #endregion
                return bErrorFlag;

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ValidatePage() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion
                throw exp;
            }
        }

        protected void grdRoleCapability_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string errMsgDelCap = GetLocalResourceObject("errMsgDelCap.Text").ToString();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("lnkDeleteCapabilty");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('" + errMsgDelCap +
                DataBinder.Eval(e.Row.DataItem, "CapabilityName") + "')");

            }
        }

        protected void grdGroupDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SearchGroup(txtEditGroupDescription.Text, txtEditGroupName.Text, e.NewPageIndex);
        }

        protected void grdRoleCapability_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewRoleCapability(Convert.ToInt64(ViewState["RoleID"].ToString()), "en-GB", e.NewPageIndex);
        }

        private void GetConfigDetails()
        {
            customerClient = new CustomerServiceClient();
            string culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string conditionConfigXML = "10";
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount = 0;
            XmlDocument resulDoc = null;
            DataSet dsConfigDetails = new DataSet();
            if (customerClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionConfigXML, Culture))
            {
                resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                if (dsConfigDetails.Tables.Count > 0)
                {
                    DataView dvConfigDetails = new DataView();
                    dvConfigDetails.Table = dsConfigDetails.Tables["ActiveDateRangeConfig"];
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Numeric")
                        {
                            hdnNumericeg.Value = dr["ConfigurationValue1"].ToString();
                        }

                    }
                }
            }
        }

    }
}
