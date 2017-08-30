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
    public partial class CardType : System.Web.UI.Page
    {
        protected ClubcardServiceClient clubcardClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        Label lblLastUpdatedBy, lblLastUpdatedDate;
        protected string errName = string.Empty;
        protected string errLength = string.Empty;
        protected string errSequenceTable = string.Empty;
        protected string spanAddFromCardRange = "display:none";
        protected string spanAddToCardRange = "display:none";
        protected string spanSequenceTable = "display:none";
        protected string errEditName = string.Empty;
        protected string spanEditName = "display:none";

        protected string errMsgvalid = string.Empty;
        DataSet dsClubcardType = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewCardTypes(0);

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
                Response.Redirect("~/Default.aspx", false);
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
        protected void grdCardType_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            try
            {
                // take the sort expression from event args
                ViewState["SortExpression"] = sortExpression; //save it to viewstate

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.grdCardType_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.grdCardType_Sorting() SortExp-" + sortExpression);
                #endregion


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
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.grdCardType_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.grdCardType_Sorting() SortExp-" + sortExpression);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.grdCardType_Sorting() SortExp-" + sortExpression + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.grdCardType_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.grdCardType_Sorting()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
        }

        /// <summary>
        /// It is used to sort transactions based on the expression and sort order
        /// </summary>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="sortOrder">EventArgs</param>
        private void SortGridView(string sortExpression, string sortOrder)
        {
            try
            {
                // generate the data view and set the sort order to it
                DataTable dt = (ViewState["dsCardTypes"] as DataSet).Tables[0];
                DataView dv = new DataView(dt);
                dv.Sort = sortExpression + sortOrder;
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.SortGridView() SortExp-" + sortExpression + " sortOrder-" + sortOrder);
                #endregion
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
                grdCardType.DataSource = dv;
                grdCardType.DataBind();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.SortGridView() SortExp-" + sortExpression + " sortOrder-" + sortOrder);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.SortGridView() SortExp-" + sortExpression + " sortOrder-" + sortOrder + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.SortGridView() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.SortGridView()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
        }
        public void ViewCardTypes(int PageNumber)
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
                maxRows = 400;
                conditionXml = "FROMMCA";
                culture = ConfigurationManager.AppSettings["Culture"].ToString();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.ViewCardTypes()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.ViewCardTypes()");
                #endregion

                if (clubcardClient.ViewCardType(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, culture))
                {

                    dvSearchResults.Visible = true;
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                    {
                        grdCardType.DataSource = dsUsers.Tables[0].DefaultView;
                        grdCardType.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdCardType.DataBind();
                        ViewState["dsCardTypes"] = dsUsers;
                    }
                    else
                    {
                        this.DataBind();
                    }

                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.ViewCardTypes()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.ViewCardTypes()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.ViewCardTypes() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.ViewCardTypes() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.ViewCardTypes()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
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


        protected void grdCardType_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "Select")
            {
                lblHeader.Text = GetLocalResourceObject("lblEditHeader.Text").ToString();
                //lblHeader.Text = "Edit - Card Type";
                dvFindUser.Visible = false;
                dvEditUser.Visible = true;
                btnAddCardType.Visible = false;
                string[] EditCardRange = e.CommandArgument.ToString().Split(';');
                txtEditName.Text = EditCardRange[1].ToString();
                txtEditLength.Text = EditCardRange[2].ToString();
                txtEditSequenceTable.Text = EditCardRange[3].ToString();
                long CardTypeID = long.Parse(EditCardRange[0].ToString());
                ViewState["CardTypeID"] = CardTypeID;

            }
            else
            {
                //for Other actions 
            }

        }

        protected void grdCardType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.grdCardType_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.grdCardType_RowDataBound()");
                #endregion

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Literal ltrDateAdded = (Literal)e.Row.FindControl("ltrFromCardNo");
                    if (ltrDateAdded.Text != null)
                    {

                    }


                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.grdCardType_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.grdCardType_RowDataBound()");
                #endregion

            }
            catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.grdCardType_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.grdCardType_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.grdCardType_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
        }

        protected void grdCardType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewCardTypes(e.NewPageIndex);
        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            htCustomer = new Hashtable();
            lblSuccessMessage.Text = string.Empty;
            try
            {
                clubcardClient = new ClubcardServiceClient();
                int rowCount, maxRows;
                maxRows = 0;

                htCustomer["ClubcardType"] = ViewState["CardTypeID"].ToString();
                htCustomer["ClubcardTypeDesc"] = txtEditName.Text.Trim();
                htCustomer["CardNumberLength"] = txtEditLength.Text.Trim();
                htCustomer["ClubcardSeqTable"] = txtEditSequenceTable.Text.Trim();
                htCustomer["ConditionalFlag"] = "1"; //Flag - From MCA
                conditionXml = Helper.HashTableToXML(htCustomer, "ClubcardType");
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.btnEditSave_Click() Condition Xml- " + conditionXml);
                #endregion

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (clubcardClient.UpdateCardType(out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0))
                    {
                        if (resultXml.Contains("UniqueConstraint Cardtype"))
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg.Text").ToString();
                        }
                        else
                        {
                            ViewCardTypes(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            btnAddCardType.Visible = true;
                            //lblHeader.Text = "Card Types";
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
                        }

                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.btnEditSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.btnEditSave_Click() Condition Xml- " + conditionXml);
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.btnEditSave_Click()  Condition Xml- " + conditionXml + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.btnEditSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.btnEditSave_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
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

        public DataSet GetClubcardType()
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

            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.GetClubcardType()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.GetClubcardType()");
                #endregion

                if (clubcardClient.ViewCardType(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, "en-GB"))
                {

                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                    {

                        ViewState["dsClubcardType"] = dsUsers;
                        dsUsers = dsUsers.Copy();
                    }
                    else
                    {
                        dsUsers = null;
                    }

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.GetClubcardType()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.GetClubcardType()");
                #endregion

                return dsUsers;


            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.GetClubcardType() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.GetClubcardType() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.GetClubcardType()");
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
            ViewCardTypes(0);
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            btnAddCardType.Visible = true;
            //lblHeader.Text = "Card Types";
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
        }

        protected void btnAddCardType_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = false;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = true;
            btnAddCardType.Visible = false;
            //lblHeader.Text = "Add - Card Types";
            lblHeader.Text = GetLocalResourceObject("lblAddHeader.Text").ToString();
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {

            clubcardClient = new ClubcardServiceClient();
            int rowCount, maxRows;
            maxRows = 0;
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();
            htCustomer["ClubcardTypeDescEnglish"] = txtAddName.Text.Trim();
            htCustomer["CardNumberLength"] = txtAddLength.Text.Trim();
            htCustomer["ClubcardSeqTable"] = txtAddSequenceTable.Text.Trim();
            htCustomer["ConditionalFlag"] = "1"; //Flag - From MCA
            conditionXml = Helper.HashTableToXML(htCustomer, "ClubcardType");
            maxRows = 200;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardType.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardType.btnAddSave_Click()  Condition Xml- " + conditionXml);
                #endregion

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (clubcardClient.AddCardType(out resultXml, conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0))
                    {
                        if (resultXml.Contains("UniqueConstraint Cardtype"))
                        {
                            //lblSuccessMessage.Text ="Card Name Already Exist"; 
                            lblSuccessMessage.Text = GetLocalResourceObject("UniqueConMsg.Text").ToString();
                        }
                        else
                        {
                            ViewCardTypes(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            dvAddUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
                            btnAddCardType.Visible = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardType.btnAddSave_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardType.btnAddSave_Click() Condition Xml- " + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardType.btnAddSave_Click()  Condition Xml- " + conditionXml + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardType.btnAddSave_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardType.btnAddSave_Click()");
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
            btnAddCardType.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
        }

    }
}
