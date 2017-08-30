using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCODundeeApplication.MarketingService;
using System.Collections;
using System.Data;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.ServiceModel;
using System.Configuration;

namespace CCODundeeApplication
{
    public partial class TescoPartner : System.Web.UI.Page
    {
        protected MarketingServiceClient marketingServiceClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        Label lblLastUpdatedBy, lblLastUpdatedDate;
        protected string errMsgPartnerName = string.Empty;
        protected string errMsgAddLimit = string.Empty;
        protected string errMsgSubtractLimit = string.Empty;
        protected string errMsgPartnerNumber = string.Empty;
        protected string spanPartnerName = "display:none";
        protected string spanPartnerAddLimit = "display:none";
        protected string spanPartnerSubtractLimit = "display:none";
        protected string spanPartnerNumber = "display:none";
        string partnerIdMaxLength = ConfigurationManager.AppSettings["PartnerIDMaxLength"].ToString();
        protected string errEditMsgPartnerName = string.Empty;
        protected string errEditMsgSubtractLimit = string.Empty;
        protected string errEditMsgAddLimit = string.Empty;
        protected string errEditMsgTescoPartner = string.Empty;
        protected string spanEditPartnerName = "display:none";
        protected string spanEditPartnerSubtractLimit = "display:none";
        protected string spanEditPartnerAddLimit = "display:none";
        protected string spanEditPartnerNumber = "display:none";

        protected string errMsgvalid = string.Empty;
        DataSet dsClubcardType = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtEditPartnerNumber.Attributes.Add("maxLength", partnerIdMaxLength);
            txtPartnerNumber.Attributes.Add("maxLength", partnerIdMaxLength);
            
            if (!IsPostBack)
            {
                ViewPartners(0);
                
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
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;

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

        public void ViewPartners(int PageNumber)
        {
            try
            {
                marketingServiceClient = new MarketingServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                rowCount = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                XmlDocument resulDoc = null;
                maxRows = 200;
                string DefaultCulture =ConfigurationSettings.AppSettings["CultureDefault"];
                //conditionXml = "FROMMCA";

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.ViewPartners()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.ViewPartners() input Xml-" + conditionXml);
                #endregion


                if (marketingServiceClient.ViewPartners(out errorXml, out resultXml, conditionXml, rowCount, DefaultCulture, maxRows))
                {

                    dvSearchResults.Visible = true;
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                    {
                        grdPartners.DataSource = dsUsers.Tables[0].DefaultView;
                        grdPartners.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdPartners.DataBind();
                        ViewState["dsPartners"] = dsUsers;
                    }
                    else
                    {
                        this.DataBind();
                    }

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.ViewPartners()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.ViewPartners() input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.ViewPartners() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.ViewPartners() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.ViewPartners()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (marketingServiceClient != null)
                {
                    if (marketingServiceClient.State == CommunicationState.Faulted)
                    {
                        marketingServiceClient.Abort();
                    }
                    else if (marketingServiceClient.State != CommunicationState.Closed)
                    {
                        marketingServiceClient.Close();
                    }
                }
            }
        }


        protected void grdPartners_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                lblSuccessMessage.Text = "";
                FillDropDown(GetPartnerType(), ddlEditPartnerType, "PartnerTypeDescEnglish", "PartnerType");
                lblHeader.Text = GetLocalResourceObject("EditHeader.Text").ToString();// "Edit - Partners";
                dvFindUser.Visible = false;
                dvEditUser.Visible = true;
                btnAddTescoPartner.Visible = false;
                string[] EditCardRange = e.CommandArgument.ToString().Split(';');
                txtEditPartnerName.Text = EditCardRange[1].ToString();
                txtEditPartnerNumber.Text = EditCardRange[0].ToString();
                ddlEditPartnerType.SelectedValue = EditCardRange[2].ToString();
                txtEditPartnerAddLimit.Text = EditCardRange[3].ToString();
                txtEditPartnerSubtractLimit.Text = EditCardRange[4].ToString().Replace("-","");
                long PartnerID = long.Parse(EditCardRange[0].ToString());
                ViewState["PartnerID"] = PartnerID;

            }
            else
            {
                //for Other actions 
            }

        }

        protected void grdPartners_RowDataBound(object sender, GridViewRowEventArgs e)
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
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartners.grdPartners_RowDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartners.grdPartners_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartners.grdPartners_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        protected void grdPartners_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblSuccessMessage.Text = string.Empty;
            ViewPartners(e.NewPageIndex);
        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {

            lblSuccessMessage.Text = string.Empty;
            try
            {
                marketingServiceClient = new MarketingServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                long sessionCrmId;
                XmlDocument resulDoc = null;
                htCustomer = new Hashtable();
                htCustomer["PartnerID"] = ViewState["PartnerID"].ToString();
                htCustomer["PartnerName"] = txtEditPartnerName.Text.Trim();
                htCustomer["PartnerType"] = ddlEditPartnerType.SelectedValue.ToString();
                htCustomer["PartnerAddPointsLimit"] = txtEditPartnerAddLimit.Text.Trim();
                htCustomer["PartnerSubtractPointsLimit"] = txtEditPartnerSubtractLimit.Text.Trim().Replace("-", "");
                htCustomer["OutletsToBeMaintained"] = "No";
                conditionXml = Helper.HashTableToXML(htCustomer, "Partner");
                maxRows = 200;



                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.btnEditSave_Click() input Xml-" + conditionXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (marketingServiceClient.UpdatePartner(out sessionCrmId, out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")).ToString()))
                    {
                        if (resultXml.Contains("PARTNERNAMEALREADYEXISTS"))
                        {
                            //case 1
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg.Text").ToString();//"Store Name Already Exists";
                        }
                        else
                        {
                            ViewPartners(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
                            btnAddTescoPartner.Visible = true;
                            lblSuccessMessage.Text = GetLocalResourceObject("lblAmmendSuccessMsg.Text").ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.btnEditSave_Click() input Xml-" + conditionXml);
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.btnEditSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.btnEditSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.btnEditSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (marketingServiceClient != null)
                {
                    if (marketingServiceClient.State == CommunicationState.Faulted)
                    {
                        marketingServiceClient.Abort();
                    }
                    else if (marketingServiceClient.State != CommunicationState.Closed)
                    {
                        marketingServiceClient.Close();
                    }
                }
            }


        }

        public DataSet GetPartnerType()
        {
            try
            {
                marketingServiceClient = new MarketingServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                rowCount = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                string DefaultCulture = ConfigurationSettings.AppSettings["CultureDefault"];
                XmlDocument resulDoc = null;
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.GetPartnerType()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.GetPartnerType()");
                #endregion
                if (marketingServiceClient.ViewPartnerType(out errorXml, out resultXml, conditionXml, rowCount, DefaultCulture, maxRows))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.GetPartnerType()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.GetPartnerType()");
                #endregion
                return dsUsers;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.GetPartnerType() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.GetPartnerType() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.GetPartnerType()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (marketingServiceClient != null)
                {
                    if (marketingServiceClient.State == CommunicationState.Faulted)
                    {
                        marketingServiceClient.Abort();
                    }
                    else if (marketingServiceClient.State != CommunicationState.Closed)
                    {
                        marketingServiceClient.Close();
                    }
                }
            }
        }
        protected void btnEditCancel_Click(object sender, EventArgs e)
        {
            ViewPartners(0);
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            btnAddTescoPartner.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Partners";
            lblSuccessMessage.Text = string.Empty;
        }

        protected void btnAddTescoPartner_Click(object sender, EventArgs e)
        {
            lblSuccessMessage.Text = "";
            dvSearchResults.Visible = false;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = true;
            btnAddTescoPartner.Visible = false;
            lblHeader.Text = GetLocalResourceObject("AddHeader.Text").ToString(); // "Add - Partners";
            FillDropDown(GetPartnerType(), ddlPartnerType, "PartnerTypeDescEnglish", "PartnerType");
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.SortGridView()");
                #endregion

                // generate the data view and set the sort order to it
                DataTable dt = (ViewState["dsPartners"] as DataSet).Tables[0];
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
                grdPartners.DataSource = dv;
                grdPartners.DataBind();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.SortGridView()");
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.SortGridView() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.SortGridView() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.SortGridView()");
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
                marketingServiceClient = new MarketingServiceClient();
                int rowCount, maxRows;
                maxRows = 0;
                string conditionXml = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                long sessionCrmId;
                htCustomer = new Hashtable();
                htCustomer["PartnerID"] = txtPartnerNumber.Text.Trim();
                htCustomer["PartnerName"] = txtPartnerName.Text.Trim();
                htCustomer["PartnerType"] = ddlPartnerType.SelectedValue.ToString();
                htCustomer["PartnerAddPointsLimit"] = txtPartnerAddLimit.Text.Trim();
                htCustomer["PartnerSubtractPointsLimit"] = txtPartnerSubtractLimit.Text.Trim().Replace("-", "");
                conditionXml = Helper.HashTableToXML(htCustomer, "Partner");
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.btnAddSave_Click() Input Xml-" + conditionXml);
                #endregion

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (marketingServiceClient.AddPartner(out sessionCrmId,out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")).ToString()))
                    {
                        if (resultXml.Contains("PARTNERNAMEALREADYEXISTS"))
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg.Text").ToString(); //Partner Name Already Exist.
                        }
                        else if (resultXml.Contains("PARTNERNUMBERALREADYEXISTS"))
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsgPartnerNo.Text").ToString(); //Partner Number Already Exist."; 
                        }
                        else
                        {
                            ViewPartners(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            dvAddUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
                            btnAddTescoPartner.Visible = true;
                            lblSuccessMessage.Text = GetLocalResourceObject("lblAddSuccessMsg.Text").ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.btnAddSave_Click() Input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.btnAddSave_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.btnAddSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.btnAddSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (marketingServiceClient != null)
                {
                    if (marketingServiceClient.State == CommunicationState.Faulted)
                    {
                        marketingServiceClient.Abort();
                    }
                    else if (marketingServiceClient.State != CommunicationState.Closed)
                    {
                        marketingServiceClient.Close();
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
            btnAddTescoPartner.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString(); //"Stores";
            lblSuccessMessage.Text = string.Empty;
        }

        protected void grdPartners_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC TescoPartner.grdPartners_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC TescoPartner.grdPartners_Sorting()");
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC TescoPartner.grdPartners_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC TescoPartner.grdPartners_Sorting()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC TescoPartner.grdPartners_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC TescoPartner.grdPartners_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC TescoPartner.grdPartners_Sorting()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }









    }
}
