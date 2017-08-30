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
    public partial class CardRanges : System.Web.UI.Page
    {
        protected ClubcardServiceClient clubcardClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        DataSet dsUsers, dsGroups = null;
        Label lblLastUpdatedBy, lblLastUpdatedDate;
        protected string errAddFromCardRange = string.Empty;
        protected string errAddToCardRange = string.Empty;
        protected string errMsgGroupName = string.Empty;
        protected string spanAddFromCardRange = "display:none";
        protected string spanAddToCardRange = "display:none";
        protected string errMsgvalid = string.Empty;
        protected string errEditFromCardNumber = string.Empty;
        protected string errEditToCardNumber = string.Empty;
        protected string spanEditFromCardNumber = "display:none";
        protected string spanEditToCardNumber = "display:none";
        protected string errMsgvalidEdit = string.Empty;

        DataSet dsClubcardType = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewCardRange(0);
                String DeleteMessage = GetLocalResourceObject("DeleteMsg.Text").ToString();
                btnEditDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + DeleteMessage + "')");
                // btnEditDelete.Attributes.Add("onclick", "javascript:return " + "confirm('Are you sure you would like to delete selected card range?')");
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
        public void ViewCardRange(int PageNumber)
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
                culture = ConfigurationManager.AppSettings["Culture"].ToString();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.ViewCardRange()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.ViewCardRange()");
                #endregion


                if (clubcardClient.ViewCardRange(out errorXml, out resultXml, conditionXml, maxRows, Culture, rowCount))
                {

                    dvSearchResults.Visible = true;
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsUsers = new DataSet();
                    dsUsers.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                    {
                        grdCardRanges.DataSource = dsUsers.Tables[0].DefaultView;
                        grdCardRanges.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdCardRanges.DataBind();
                        ViewState["dsCardRanges"] = dsUsers;
                    }
                    else
                    {
                        this.DataBind();
                    }

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.ViewCardRange()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.ViewCardRange()");
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.ViewCardRange() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.ViewCardRange() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.ViewCardRange()");
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

        protected void grdCustomerDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {


                string sortExpression = e.SortExpression; // take the sort expression from event args
                ViewState["SortExpression"] = sortExpression; //save it to viewstate

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.grdCustomerDetail_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.grdCustomerDetail_Sorting()");
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.grdCustomerDetail_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.grdCustomerDetail_Sorting()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.grdCustomerDetail_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.grdCustomerDetail_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.grdCustomerDetail_Sorting()");
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
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder);
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder);
            #endregion

            try
            {
                // generate the data view and set the sort order to it
                DataTable dt = (ViewState["dsCardRanges"] as DataSet).Tables[0];
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
                grdCardRanges.DataSource = dv;
                grdCardRanges.DataBind();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder);
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.SortGridView() Sort Expression -" + sortExpression + " Sort Order-" + sortOrder + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.SortGridView()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
        }

        protected void GrdCustomerDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {
                culture = ConfigurationManager.AppSettings["Culture"].ToString();
                lblHeader.Text = GetLocalResourceObject("EditHeader.Text").ToString();
                //lblHeader.Text = "Edit - Card Ranges";
                dvFindUser.Visible = false;
                dvEditUser.Visible = true;
                btnAddCardRange.Visible = false;
                string[] EditCardRange = e.CommandArgument.ToString().Split(';');
                txtDateAdded.Text = Convert.ToDateTime(EditCardRange[3]).ToString("d", CultureInfo.CreateSpecificCulture(culture));
                txtFromCardNumber.Text = EditCardRange[1].ToString();
                txtToCardNumber.Text = EditCardRange[2].ToString();
                long CardRangeID = long.Parse(EditCardRange[0].ToString());
                ViewState["CardRangeID"] = CardRangeID;
                ViewState["CardLength"] = int.Parse(EditCardRange[5].ToString());
            }
            else
            {
                //for Other actions 
            }

        }

        protected void grdCustomerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.grdCustomerDetail_RowDataBound()");
                #endregion

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    culture = ConfigurationManager.AppSettings["Culture"].ToString();
                    Literal ltrDateAdded = (Literal)e.Row.FindControl("ltrDateAdded");
                    if (ltrDateAdded.Text != null)
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(ltrDateAdded.Text.ToString());
                        ltrDateAdded.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));
                    }


                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.grdCustomerDetail_RowDataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.grdCustomerDetail_RowDataBound()" + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.grdCustomerDetail_RowDataBound()" + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.grdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
        }


        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdRoleMembership_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }



        protected void grdCustomerDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewCardRange(e.NewPageIndex);
        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {
            int CardLength = Convert.ToInt32(ViewState["CardLength"].ToString());
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            if (txtFromCardNumber.Text.ToString().Length == CardLength && txtToCardNumber.Text.ToString().Length == CardLength)
            {

                try
                {

                    clubcardClient = new ClubcardServiceClient();
                    htCustomer = new Hashtable();
                    htCustomer["MinCardNumber"] = txtFromCardNumber.Text.Trim();
                    htCustomer["MaxCardNumber"] = txtToCardNumber.Text.Trim();
                    htCustomer["ClubcardRangeID"] = ViewState["CardRangeID"].ToString();
                    conditionXml = Helper.HashTableToXML(htCustomer, "ClubcardRange");

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.btnEditSave_Click() Condition Xml- " + conditionXml);
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.btnEditSave_Click()Condition Xml- " + conditionXml);
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (clubcardClient.UpdateCardRange(conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0, resultXml))
                        {
                            ViewCardRange(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            btnAddCardRange.Visible = true;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
                            // lblHeader.Text = "Card Ranges";
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.btnEditSave_Click()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.btnEditSave_Click()");
                    #endregion
                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.btnEditSave_Click() Condition Xml- " + conditionXml + " - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.btnEditSave_Click() Condition Xml- " + conditionXml + " - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.btnEditSave_Click()");
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
            else
            {
                //string message = "From and to card numbers should contain " + CardLength + " digits for this type of card";
                string message = GetLocalResourceObject("CardLengthMsg1.Text").ToString() + CardLength + GetLocalResourceObject("CardLengthMsg2.Text").ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + message + "');", true);
            }

        }

        public DataSet GetClubcardType()
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
                culture = ConfigurationManager.AppSettings["Culture"].ToString();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.GetClubcardType()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.GetClubcardType()");
                #endregion

                if (clubcardClient.ViewCardType(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, culture))
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.GetClubcardType() Condition Xml- " + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.GetClubcardType()Condition Xml- " + conditionXml);
                #endregion
                return dsUsers;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.GetClubcardType() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.GetClubcardType()  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.GetClubcardType()");
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
            ViewCardRange(0);
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            btnAddCardRange.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
        }

        protected void btnAddCardRange_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = false;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = true;
            btnAddCardRange.Visible = false;
            lblHeader.Text = GetLocalResourceObject("AddHeader.Text").ToString();
            FillGroupDropDown(GetClubcardType());
        }

        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            dsClubcardType = new DataSet();
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();

            dsClubcardType = (DataSet)ViewState["dsClubcardType"];
            DataView dvFilter = new DataView();
            dvFilter = dsClubcardType.Tables[0].DefaultView;
            dvFilter.RowFilter = "ClubcardTypeDesc='" + ddlCardType.SelectedItem.Text + "'";
            int CardLength = Convert.ToInt32(dvFilter[0].Row["CardNumberLength"].ToString());

            if (txtAddFromCardRange.Text.ToString().Length == CardLength && txtAddToCardRange.Text.ToString().Length == CardLength)
            {
                try
                {
                    clubcardClient = new ClubcardServiceClient();

                    htCustomer["MinCardNumber"] = txtAddFromCardRange.Text.Trim();
                    htCustomer["MaxCardNumber"] = txtAddToCardRange.Text.Trim();
                    htCustomer["ClubCardType"] = ddlCardType.SelectedValue.ToString();
                    conditionXml = Helper.HashTableToXML(htCustomer, "ClubcardRange");


                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.btnAddSave_Click() conditionXml-" + conditionXml);
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.btnAddSave_Click() conditionXml-" + conditionXml);
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (clubcardClient.AddCardRange(conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0, resultXml))
                        {
                            ViewCardRange(0);
                            dvSearchResults.Visible = true;
                            dvFindUser.Visible = true;
                            dvEditUser.Visible = false;
                            dvAddUser.Visible = false;
                            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
                            btnAddCardRange.Visible = true;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.btnAddSave_Click() conditionXml-" + conditionXml);
                    NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.btnAddSave_Click() conditionXml-" + conditionXml);
                    #endregion
                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.btnAddSave_Click() conditionXml-" + conditionXml + " - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.btnAddSave_Click()  - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.btnAddSave_Click()");
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
            else
            {
                //string message = "From and to card numbers should contain " + CardLength + " digits for this type of card";
                string message = GetLocalResourceObject("CardLengthMsg1.Text").ToString() + CardLength + GetLocalResourceObject("CardLengthMsg2.Text").ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + message + "');", true);
            }


        }

        protected void btnAddCancel_Click(object sender, EventArgs e)
        {
            dvSearchResults.Visible = true;
            dvFindUser.Visible = true;
            dvEditUser.Visible = false;
            dvAddUser.Visible = false;
            btnAddCardRange.Visible = true;
            lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
        }

        #region Fill Groups Drop Down

        public void FillGroupDropDown(DataSet ds)
        {

            ddlCardType.DataMember = "ClubcardTypeDesc";
            ddlCardType.DataTextField = "ClubcardTypeDesc";
            ddlCardType.DataValueField = "ClubcardTypeID";
            ddlCardType.DataSource = ds.Tables[0];
            ddlCardType.DataBind();
        }
        #endregion

        protected void btnEditDelete_Click(object sender, EventArgs e)
        {
            clubcardClient = new ClubcardServiceClient();
            int rowCount, maxRows;
            maxRows = 0;
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            htCustomer = new Hashtable();
            try
            {


                htCustomer["ClubcardRangeID"] = ViewState["CardRangeID"].ToString();
                conditionXml = Helper.HashTableToXML(htCustomer, "ClubcardRange");
                maxRows = 200;

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CardRanges.btnEditDelete_Click() conditionXml-" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.btnEditDelete_Click() conditionXml-" + conditionXml);
                #endregion

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    if (clubcardClient.DeleteCardRange(conditionXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID")), 0, resultXml))
                    {
                        ViewCardRange(0);
                        dvSearchResults.Visible = true;
                        dvFindUser.Visible = true;
                        dvEditUser.Visible = false;
                        dvAddUser.Visible = false;
                        lblHeader.Text = GetLocalResourceObject("lblHeaderResource1.Text").ToString();
                        btnAddCardRange.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CardRanges.btnAddSave_Click() btnEditDelete_Click-" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End: CSC CardRanges.btnAddSave_Click() btnEditDelete_Click-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CardRanges.btnEditDelete_Click() conditionXml-" + conditionXml + " - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC CardRanges.btnEditDelete_Click()  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CardRanges.btnEditDelete_Click()");
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


    }
}
