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
using CCODundeeApplication.ClubcardService;
using System.Xml;
using System.Globalization;
namespace CCODundeeApplication
{
    public partial class TescoStore : System.Web.UI.Page
    {
        protected ClubcardServiceClient clubcardClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        Label lblLastUpdatedBy, lblLastUpdatedDate;
        protected string errMsgStoreName = string.Empty;
        protected string errMsgWelcomePoints = string.Empty;
        protected string errMsgTescoStore = string.Empty;
        protected string spanStoreName = "display:none";
        protected string spanStoreWelPoints = "display:none";
        protected string spanStoreNumber = "display:none";

        protected string errEditMsgStoreName = string.Empty;
        protected string errEditMsgWelcomePoints = string.Empty;
        protected string errEditMsgTescoStore = string.Empty;
        protected string spanEditStoreName = "display:none";
        protected string spanEditStoreWelPoints = "display:none";
        protected string spanEditStoreNumber = "display:none";

        protected string errMsgvalid = string.Empty;
        DataSet dsClubcardType = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewStores(0);

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
                Label lblLastUpdatedBy = (Label)Master.FindControl("lblLastUpdatedBy");
                Label lblLastUpdatedDate = (Label)Master.FindControl("lblLastUpdatedDate");
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                //Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                //lblDataConfiguration.Visible = false;

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
            filter = ViewState["FilterArgs"] == null ? new Hashtable() : (Hashtable)ViewState["FilterArgs"];
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
                Response.Redirect("Default.aspx", false);
        }
        #endregion

        public void ViewStores(int PageNumber)
        {
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                rowCount = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                maxRows = 200;
                conditionXml = "FROMMCA";

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.ViewStores()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.ViewStores() input Xml-" + conditionXml);
                #endregion


                if (clubcardClient.ViewStores(out errorXml, out resultXml, conditionXml, rowCount, "en-GB", maxRows))
                {

                    dvSearchResults.Visible = true;
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                    {
                        grdStores.DataSource = dsUsers.Tables[0].DefaultView;
                        grdStores.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdStores.DataBind();
                        ViewState["dsStores"] = dsUsers;
                    }
                    else
                    {
                        this.DataBind();
                    }

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.ViewStores()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.ViewStores() input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.ViewStores() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.ViewStores() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.ViewStores()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (clubcardClient != null)
                {
                    if (clubcardClient.State == CommunicationState.Faulted)
                    {
                        clubcardClient.Abort();
                    }
                    else if (clubcardClient.State != CommunicationState.Closed)
                    {
                        clubcardClient.Close();
                    }
                }
            }
        }


        protected void grdStores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //int index = Convert.ToInt32(e.CommandArgument);
            //GridViewRow selectedRow = grdStores.Rows[index];
            //ImageButton button = (ImageButton)e.CommandSource;
            //button.ImageUrl = "I/GotoSearchCustomer.gif";
            if (e.CommandName == "Select")
            {
                FillDropDown(GetStoreFormat(), ddlEditStoreFormat, "StoreFormatDescEnglish", "StoreFormatID");
                FillDropDown(GetStoreRegion(), ddlEditRegion, "StoreRegionNameEnglish", "StoreRegionID");
                lblHeader.Text = GetLocalResourceObject("EditHeader.Text").ToString();// "Edit - Stores";
                dvFindUser.Visible = false;
                dvEditUser.Visible = true;
                btnAddTescoStore.Visible = false;
                string[] EditCardRange = e.CommandArgument.ToString().Split(';');
                txtEditStoreName.Text = EditCardRange[1].ToString();
                txtEditStoreNumber.Text = EditCardRange[0].ToString();
                txtEditStoreWelPoints.Text = EditCardRange[4].ToString();
                ddlEditRegion.SelectedValue = EditCardRange[3].ToString();
                ddlEditStoreFormat.SelectedValue = EditCardRange[2].ToString();
                long StoreID = long.Parse(EditCardRange[0].ToString());
                ViewState["StoreID"] = StoreID;

            }
            else
            {
                //for Other actions 
            }

        }

        protected void grdStores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //ImageButton button = (ImageButton)e.Row.FindControl("imgSelect");
                    //button.CommandArgument = e.Row.RowIndex.ToString();


                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.grdStores_RowDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.grdStores_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.grdStores_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        protected void grdStores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewStores(e.NewPageIndex);
        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {

            lblSuccessMessage.Text = string.Empty;
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                htCustomer = new Hashtable();
                htCustomer["TescoStoreID"] = ViewState["StoreID"].ToString();
                htCustomer["TescoStoreName"] = txtEditStoreName.Text.Trim();
                htCustomer["StoreFormatID"] = ddlEditStoreFormat.SelectedValue.ToString();
                htCustomer["store_welcome_points"] = txtEditStoreWelPoints.Text.Trim();
                htCustomer["StoreRegionID"] = ddlEditRegion.SelectedValue.ToString();
                conditionXml = Helper.HashTableToXML(htCustomer, "TescoStore");
                maxRows = 200;



                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.btnEditSave_Click() input Xml-" + conditionXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (clubcardClient.UpdateStores(out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0))
                    {
                        if (resultXml.Contains("UniqueConstraint StoreName"))
                        {
                            //case 1
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg.Text").ToString();//"Store Name Already Exists";
                        }
                        else
                        {
                            ViewStores(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
                            btnAddTescoStore.Visible = true;
                            lblSuccessMessage.Text = string.Empty;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.btnEditSave_Click() input Xml-" + conditionXml);
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.btnEditSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.btnEditSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.btnEditSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (clubcardClient != null)
                {
                    if (clubcardClient.State == CommunicationState.Faulted)
                    {
                        clubcardClient.Abort();
                    }
                    else if (clubcardClient.State != CommunicationState.Closed)
                    {
                        clubcardClient.Close();
                    }
                }
            }


        }

        public DataSet GetStoreFormat()
        {
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                rowCount = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.GetStoreFormat()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.GetStoreFormat()");
                #endregion
                if (clubcardClient.ViewStoreFormat(out errorXml, out resultXml, conditionXml, rowCount, "en-GB", maxRows))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.GetStoreFormat()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.GetStoreFormat()");
                #endregion
                return dsUsers;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.GetStoreFormat() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.GetStoreFormat() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.GetStoreFormat()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (clubcardClient != null)
                {
                    if (clubcardClient.State == CommunicationState.Faulted)
                    {
                        clubcardClient.Abort();
                    }
                    else if (clubcardClient.State != CommunicationState.Closed)
                    {
                        clubcardClient.Close();
                    }
                }
            }
        }
        public DataSet GetStoreRegion()
        {
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                rowCount = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.GetStoreRegion()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.GetStoreRegion()");
                #endregion

                if (clubcardClient.ViewStoreRegion(out errorXml, out resultXml, conditionXml, rowCount, "en-GB", maxRows))
                {

                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.GetStoreRegion()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.GetStoreRegion()");
                #endregion

                return dsUsers;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.GetStoreRegion() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.GetStoreRegion() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.GetStoreRegion()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (clubcardClient != null)
                {
                    if (clubcardClient.State == CommunicationState.Faulted)
                    {
                        clubcardClient.Abort();
                    }
                    else if (clubcardClient.State != CommunicationState.Closed)
                    {
                        clubcardClient.Close();
                    }
                }
            }
        }
        protected void btnEditCancel_Click(object sender, EventArgs e)
        {
            ViewStores(0);
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            btnAddTescoStore.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
            lblSuccessMessage.Text = string.Empty;
        }

        protected void btnAddTescoStore_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = false;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = true;
            btnAddTescoStore.Visible = false;
            lblHeader.Text = GetLocalResourceObject("AddHeader.Text").ToString(); // "Add - Stores";
            FillDropDown(GetStoreFormat(), ddlStoreFormat, "StoreFormatDescEnglish", "StoreFormatID");
            FillDropDown(GetStoreRegion(), ddlRegion, "StoreRegionNameEnglish", "StoreRegionID");
        }

        #region Fill Drop Down

        public void FillDropDown(DataSet ds, DropDownList ddl, string textField, string valueField)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl.DataMember = textField;
                ddl.DataTextField = textField;
                ddl.DataValueField = valueField;
                ddl.DataSource = ds.Tables[0];
                ddl.DataBind();
            }
        }
        #endregion

        #region Page fields;
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";
        Hashtable filter;
        private SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }
        #endregion Page fields

        /// <summary>
        /// It is used to sort transactions based on the expression and sort order
        /// </summary>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="sortOrder">EventArgs</param>
        private void SortGridView(string sortExpression, string sortOrder)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.SortGridView()");
                #endregion

                // generate the data view and set the sort order to it
                DataTable dt = (ViewState["dsStores"] as DataSet).Tables[0];
                DataView dv = new DataView(dt);
                dv.Sort = sortExpression + sortOrder;

                //again set the filter keys
                string args = " ";
                int i = 0;
                #region apply the filter again
                foreach (object key in filter.Keys)
                {
                    if (i == 0)
                    {
                        args = key.ToString() + filter[key].ToString();
                    }
                    else
                    {
                        args += " AND " + key.ToString() + filter[key].ToString();
                    }
                    i++;
                }
                dv.RowFilter = args; //this time to dataview
                #endregion
                //data bind the gridview the data view
                grdStores.DataSource = dv;
                grdStores.DataBind();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.SortGridView()");
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.btnAddSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.btnAddSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.btnAddSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        protected void btnAddSave_Click(object sender, EventArgs e)
        {

            lblSuccessMessage.Text = string.Empty;
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["TescoStoreID"] = txtStoreNumber.Text.Trim();
                htCustomer["TescoStoreName"] = txtStoreName.Text.Trim();
                htCustomer["StoreFormatID"] = ddlStoreFormat.SelectedValue.ToString();
                htCustomer["store_welcome_points"] = txtStoreWelPoints.Text.Trim();
                htCustomer["StoreRegionID"] = ddlRegion.SelectedValue.ToString();
                conditionXml = Helper.HashTableToXML(htCustomer, "TescoStore");
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.btnAddSave_Click() Input Xml-" + conditionXml);
                #endregion

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (clubcardClient.AddStores(out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0))
                    //if (clubcardClient.AddStores(conditionXml, 88, 0, resultXml))
                    {
                        if (resultXml.Contains("UniqueConstraint StoreName"))
                        {
                            lblSuccessMessage.Text = "Store Name Already Exist.";
                        }
                        else if (resultXml.Contains("UniqueConstraint StoreNumber"))
                        {
                            lblSuccessMessage.Text = "Store Number Already Exist.";
                        }
                        else
                        {
                            ViewStores(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            dvAddUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
                            btnAddTescoStore.Visible = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.btnAddSave_Click() Input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.btnAddSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.btnAddSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.btnAddSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (clubcardClient != null)
                {
                    if (clubcardClient.State == CommunicationState.Faulted)
                    {
                        clubcardClient.Abort();
                    }
                    else if (clubcardClient.State != CommunicationState.Closed)
                    {
                        clubcardClient.Close();
                    }
                }
            }


        }

        protected void btnAddCancel_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = false;
            btnAddTescoStore.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
            lblSuccessMessage.Text = string.Empty;
        }

        protected void grdStores_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoStore.grdStores_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoStore.grdStores_Sorting()");
                #endregion
                string sortExpression = e.SortExpression; // take the sort expression from event args
                ViewState["SortExpression"] = sortExpression; //save it to viewstate

                //set the gridview sort direction and sort the gridview by calling SortGridView()
                //Ascending or Decending
                if (GridViewSortDirection == SortDirection.Ascending)
                {
                    GridViewSortDirection = SortDirection.Descending;
                    SortGridView(sortExpression, DESCENDING);
                }
                else
                {
                    GridViewSortDirection = SortDirection.Ascending;
                    SortGridView(sortExpression, ASCENDING);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoStore.grdStores_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoStore.grdStores_Sorting()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoStore.grdStores_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoStore.grdStores_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoStore.grdStores_Sorting()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }








    }
}
