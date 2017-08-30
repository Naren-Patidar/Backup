using System;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Threading;
using System.Xml;
using System.ServiceModel;
using System.Web.UI.WebControls;
using CCODundeeApplication.AdminService;
using CCODundeeApplication.CustomerService;

namespace CCODundeeApplication
{
    public partial class PromotionCode : System.Web.UI.Page
    {
        DataSet dsPromotionalcode = null;
        AdminServiceClient objAdmin = null;
        Hashtable htCustomer = null;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;
        string culture = string.Empty;
        CustomerServiceClient serviceClient = null;
        protected string errMsgStoreNamepro = string.Empty;
        protected string errMsgWelcomePoints = string.Empty;
        protected string errMsgTescoStore = string.Empty;
        protected string errMsgStartdate = string.Empty;
        protected string spanStoreName = "display:none";
        protected string spanStoreWelPoints = "display:none";
        protected string SpanStartdate = "display:none";
        protected string spanStoreNumber = "display:none";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPromotioncodepro.Enabled = true;
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
                culture = Helper.GetTripleDESEncryptedCookieValue("Culture").ToString();
            }
            else
                Response.Redirect("~/Default.aspx", false);
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
                ContentPlaceHolder custDetailsLeftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                custDetailsLeftNav.Visible = false;
                GetConfigDetails();
                BindPromotionalCodeGrid(0);
                String DeleteMessage = "Are you sure you would like to delete selected PromotionCode ?";
                btnDeletepro.Attributes.Add("onclick", "javascript:return " + "confirm('" + DeleteMessage + "')");
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    #region RoleCapabilityImplementation
                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    if (dsCapability.Tables.Count > 0)
                    {
                        if (dsCapability.Tables[0].Columns.Contains("UpdatePromotionCode") != false)
                        {
                            dvAddUser1.Visible = true;
                        }
                        else
                        {
                            dvAddUser1.Visible = false;

                        }
                    
                    }
                }
                #endregion
            }
            btnDeletepro.Visible = false;
            btnUpdatepro.Visible = false;
            btnAddSavepro.Visible = true;


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

        public void BindPromotionalCodeGrid(int PageNumber)
        {
            
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try 
            {
                 NGCTrace.NGCTrace.TraceInfo("Start: CSC AddPromotionCode.aspx.BindPromotionalCodeGrid()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FiAddPromotionCode.aspx.BindPromotionalCodeGrid()");
                objAdmin = new AdminServiceClient();
                if (objAdmin.GetPromotionCode(out errorXml,out resultXml))
                {
                    resulDoc = new XmlDocument();
                    dsPromotionalcode = new DataSet();
                    resulDoc.LoadXml(resultXml);
                    dsPromotionalcode.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsPromotionalcode.Tables.Count > 0 && dsPromotionalcode.Tables[0].Rows.Count > 0)
                    {
                        grdPromotionCodepro.DataSource = dsPromotionalcode.Tables[0].DefaultView;
                        grdPromotionCodepro.PageIndex = PageNumber < 0 ? 0 : PageNumber;
                        grdPromotionCodepro.DataBind();
                    }

                }


            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSCAddPromotionCode.aspx.BindPromotionalCodeGrid() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC AddPromotionCode.aspx.BindPromotionalCodeGrid() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: AddPromotionCode.aspx.BindPromotionalCodeGrid()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            if (objAdmin != null)
            {
                if (objAdmin.State == CommunicationState.Faulted)
                {
                    objAdmin.Abort();
                }
                else if (objAdmin.State != CommunicationState.Closed)
                {
                    objAdmin.Close();
                }
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                objAdmin = new AdminServiceClient();
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                htCustomer = new Hashtable();
                htCustomer["UserID"] = ViewState["UserID"].ToString();
                htCustomer["PromotionCode"]=txtPromotioncodepro.Text.ToString();
                htCustomer["PromotionCodeDescEnglish"]=txtDescriptionpro.Text.ToString();
                htCustomer["StartDate"]=txtStartDatepro.Text.ToString();
                htCustomer["EndDate"]=txtEndDatepro.Text.ToString();

                string objectXml = Helper.HashTableToXML(htCustomer, "PromotionCode");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.btnAdd_Click() Input Xml-" + objectXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    //if (adminClient.AddRole(out objectid, out resultXml, addGroupXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))

                    if (objAdmin.AddPromotionCode(out resultXml, objectXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        ClearSelection();
                        lblSuccessMessage.Text = GetLocalResourceObject("InsertPromotionCode").ToString();//"Promotion code added successfully";
                        BindPromotionalCodeGrid(0);
                    }

                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC FindUser.btnAdd_Click() Input Xml-" + objectXml);
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
                if (objAdmin != null)
                {
                    if (objAdmin.State == CommunicationState.Faulted)
                    {
                        objAdmin.Abort();
                    }
                    else if (objAdmin.State != CommunicationState.Closed)
                    {
                        objAdmin.Close();
                    }
                }
            }
        }

        protected void grdPromotionCodepro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                Literal ltrStartDate = (Literal)e.Row.FindControl("ltrStartDate");
                Literal ltrEndDate = (Literal)e.Row.FindControl("ltrEndDate");
                LinkButton lnkSelectCustomer = (LinkButton)e.Row.FindControl("lnkSelectCustomer"); 
                if (ltrStartDate.Text != null)
                {
                    DateTime LastUpdateDate = Convert.ToDateTime(ltrStartDate.Text.ToString());
                    ltrStartDate.Text = LastUpdateDate.ToString("dd/MM/yy", new CultureInfo("en-GB"));

                }

                if (ltrEndDate.Text != null)
                {
                    DateTime LastUpdateDate = Convert.ToDateTime(ltrEndDate.Text.ToString());
                    ltrEndDate.Text = LastUpdateDate.ToString("dd/MM/yy", new CultureInfo("en-GB"));

                }
            }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {

                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    if (dsCapability.Tables.Count > 0)
                    {
                        if (dsCapability.Tables[0].Columns.Contains("UpdatePromotionCode") != false)
                        {
                            e.Row.Cells[0].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[0].Visible = false;
                        }

                    }
                }

            

        }

        protected void grdPromotionCodepro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindPromotionalCodeGrid(e.NewPageIndex);
        }
        protected void grdPromotionCodepro_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            lblSuccessMessage.Text = "";
            string objectXml = string.Empty, resultXml;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC DeLinking.GridForAssociative_RowCommand()");
                #endregion
                if (e.CommandName == "Delete")
                {
                    //serviceClient = new CustomerService.CustomerServiceClient();
                    objAdmin = new AdminServiceClient();
                    String PromotionCode = e.CommandArgument.ToString();
                    htCustomer = new Hashtable();
                    htCustomer["PromotionCode"] = PromotionCode;
                    objectXml = Helper.HashTableToXML(htCustomer, "TblPromotionCode");
                    if (objAdmin.UpdatePromotionCode(out resultXml,objectXml,Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        BindPromotionalCodeGrid(0);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                }

                if (e.CommandName == "Select")
                {
                    string[] PromotionCodearr = e.CommandArgument.ToString().Split(';');
                    txtPromotioncodepro.Text = PromotionCodearr[0].ToString();
                    txtDescriptionpro.Text = PromotionCodearr[1].ToString();
                    txtPromotioncodepro.Enabled = false;
                    string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                    //txtStartDate.Text = PromotionCodearr[2].ToString();
                    if (PromotionCodearr[2].ToString() != "")
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(PromotionCodearr[2].ToString());
                        txtStartDatepro.Text = LastUpdateDate.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture(culture));

                    }

                    if (PromotionCodearr[3].ToString() != "")
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(PromotionCodearr[3].ToString());
                        txtEndDatepro.Text = LastUpdateDate.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture(culture));

                    }
                    btnAddSavepro.Visible = false;
                    btnDeletepro.Visible = true;
                    btnUpdatepro.Visible = true;


                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC DeLinking.GridForAssociative_RowCommand()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (objAdmin != null)
                {
                    if (objAdmin.State == CommunicationState.Faulted)
                    {
                        objAdmin.Abort();
                    }
                    else if (objAdmin.State != CommunicationState.Closed)
                    {
                        objAdmin.Close();
                    }
                }
            }

        }

        protected void grdPromotionCodepro_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected bool validate()
        {
            bool bErrorFlag = true;
            //DateTime fromdate;
            //DateTime todate;
            txtPromotioncodepro.CssClass = "";
            txtDescriptionpro.CssClass = "";
            txtStartDatepro.CssClass = "";
            txtEndDatepro.CssClass = "";
            //string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
            string regDate = hdndatereg.Value;

            try
           {
                string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                if (txtPromotioncodepro.Text.ToString() == string.Empty && txtDescriptionpro.Text.ToString() == string.Empty && txtStartDatepro.Text.ToString() == string.Empty && txtEndDatepro.Text.ToString() == string.Empty)
                {
                    lblSuccessMessage.Text = GetLocalResourceObject("ValidDetails").ToString();//"Please enter valid details";
                    bErrorFlag = false;
                }

                if (txtPromotioncodepro.Text.ToString() == string.Empty)
                {
                    this.errMsgStoreNamepro = GetLocalResourceObject("ValidPromotionCode").ToString();//"Please enter Valid promotion code";//"Please enter valid Primary Id";
                    this.spanStoreName = "";
                    this.txtPromotioncodepro.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (txtDescriptionpro.Text.ToString() == string.Empty)
                {
                    this.errMsgTescoStore = GetLocalResourceObject("ValidDesc").ToString();//"Please enter valid description";
                    this.spanStoreNumber = "";
                    this.txtDescriptionpro.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (txtStartDatepro.Text.ToString() == String.Empty)
                {
                    this.errMsgStartdate = GetLocalResourceObject("ValidStartDate").ToString();//"Please enter valid Start Date";
                    this.SpanStartdate = "";
                    this.txtStartDatepro.CssClass = "errorFld";
                    bErrorFlag = false;

                }
                else if (!Helper.IsRegexMatch(txtStartDatepro.Text.Trim(), regDate, false, false))
                    {
                        this.SpanStartdate = "";
                        errMsgStartdate = GetLocalResourceObject("ValidStartFormat").ToString();//"Please enter a valid Start date in DD/MM/YYYY format.";
                        txtStartDatepro.CssClass = "errorFld";
                        bErrorFlag = false;
                        txtStartDatepro.Focus();
                    }
                
                if (txtEndDatepro.Text.ToString() == string.Empty)
                {
                    this.errMsgWelcomePoints = GetLocalResourceObject("ValidEndDate").ToString();//"Please enter valid End Date";
                    this.spanStoreWelPoints = "";
                    this.txtEndDatepro.CssClass = "errorFld";
                    bErrorFlag = false;

                }
                else if (!Helper.IsRegexMatch(txtEndDatepro.Text.Trim(), regDate, false, false))
                    {
                        this.spanStoreWelPoints = "";
                        errMsgWelcomePoints = GetLocalResourceObject("ValidEndFormat").ToString();//"Please enter a valid End date in DD/MM/YYYY format.";
                        txtEndDatepro.CssClass = "errorFld";
                        bErrorFlag = false;
                        txtEndDatepro.Focus();
                    }
                
               
                return bErrorFlag;

            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
        }

        public void ClearSelection()
        {
            txtPromotioncodepro.Text = "";
            txtDescriptionpro.Text = "";
            txtStartDatepro.Text = "";
            txtEndDatepro.Text = "";
        }

     

        protected void btnUpdatepro_Click(object sender, EventArgs e)
        {
            objAdmin = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();
            lblSuccessMessage.Text = "";
            if (validate())
            {
                htCustomer["PromotionCode"] = txtPromotioncodepro.Text.ToString();
                htCustomer["PromotionCodeDescEnglish"] = txtDescriptionpro.Text.ToString();
                //DateTime DOB = DateTime.Parse(ddlDay0.SelectedValue + "/" + ddlMonth0.SelectedValue + "/" + ddlYear0.SelectedValue, new CultureInfo("en-GB"));
                //htCustomer["DateOfBirth"] = DOB.ToLongDateString();
                DateTime StartDateFormat = DateTime.Parse(txtStartDatepro.Text.ToString(), new CultureInfo("en-GB"));
                htCustomer["StartDate"] = StartDateFormat.ToLongDateString();
                DateTime EndDateFormat = DateTime.Parse(txtEndDatepro.Text.ToString(), new CultureInfo("en-GB"));
                htCustomer["EndDate"] = EndDateFormat.ToLongDateString();
                // htCustomer["UserID"] = ViewState["UserID"].ToString();
                string objectXml = Helper.HashTableToXML(htCustomer, "TblPromotionCode");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.btnAdd_Click() Input Xml-" + objectXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    //if (adminClient.AddRole(out objectid, out resultXml, addGroupXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))

                    if (objAdmin.UpdatePromotionCodePC(out resultXml, objectXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        ClearSelection();
                        lblSuccessMessage.Text = GetLocalResourceObject("UpdatePromotionCode").ToString();//"Promotion code Updated successfully";
                        BindPromotionalCodeGrid(0);
                    }
                }
            }
        }

        protected void btnAddSavepro_Click(object sender, EventArgs e)
        {
            objAdmin = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();
            lblSuccessMessage.Text = "";
            if (validate())
            {
                htCustomer["PromotionCode"] = txtPromotioncodepro.Text.ToString();
                htCustomer["PromotionCodeDescEnglish"] = txtDescriptionpro.Text.ToString();
                //DateTime DOB = DateTime.Parse(ddlDay0.SelectedValue + "/" + ddlMonth0.SelectedValue + "/" + ddlYear0.SelectedValue, new CultureInfo("en-GB"));
                //htCustomer["DateOfBirth"] = DOB.ToLongDateString();
                DateTime StartDateFormat = DateTime.Parse(txtStartDatepro.Text.ToString(), new CultureInfo("en-GB"));
                htCustomer["StartDate"] = StartDateFormat.ToLongDateString();
                DateTime EndDateFormat = DateTime.Parse(txtEndDatepro.Text.ToString(), new CultureInfo("en-GB"));
                htCustomer["EndDate"] = EndDateFormat.ToLongDateString();
                // htCustomer["UserID"] = ViewState["UserID"].ToString();
                string objectXml = Helper.HashTableToXML(htCustomer, "TblPromotionCode");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC FindUser.btnAdd_Click()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC FindUser.btnAdd_Click() Input Xml-" + objectXml);
                #endregion
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                {
                    //if (adminClient.AddRole(out objectid, out resultXml, addGroupXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))

                    if (objAdmin.AddPromotionCode(out resultXml, objectXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                    {
                        if (resultXml.Contains("UniqueConstraint PromotionCode"))
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("AlreadyExistsPC").ToString();//"Promotion code already exists";
                            ClearSelection();
                        }
                        else
                        {
                            ClearSelection();
                            BindPromotionalCodeGrid(0);
                        }
                    }
                }
            }
        }

        protected void btnDeletepro_Click(object sender, EventArgs e)
        {
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            string objectXml = string.Empty;
            lblSuccessMessage.Text = "";
            try
            {
                objAdmin = new AdminServiceClient();
                String PromotionCode = txtPromotioncodepro.Text.ToString();
                htCustomer = new Hashtable();
                htCustomer["PromotionCode"] = PromotionCode;
                objectXml = Helper.HashTableToXML(htCustomer, "TblPromotionCode");
                if (objAdmin.UpdatePromotionCode(out resultXml, objectXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                {
                    ClearSelection();
                    lblSuccessMessage.Text = GetLocalResourceObject("DeletePromotionCode").ToString();//"Promotion code deleted successfully";
                    BindPromotionalCodeGrid(0);
                }
                else
                {
                    Response.Redirect("Default.aspx", false);
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC DeLinking.GridForAssociative_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC DeLinking.GridForAssociative_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (objAdmin != null)
                {
                    if (objAdmin.State == CommunicationState.Faulted)
                    {
                        objAdmin.Abort();
                    }
                    else if (objAdmin.State != CommunicationState.Closed)
                    {
                        objAdmin.Close();
                    }
                }
            }
        }

        protected void btnEditCancepro_Click(object sender, EventArgs e)
        {
            ClearSelection();
            btnAddSavepro.Visible = true;
            btnUpdatepro.Visible = false;
            btnDeletepro.Visible=false;
            btnEditCancepro.Visible = true;
            txtPromotioncodepro.Enabled = true;
            txtPromotioncodepro.CssClass = "";
            txtDescriptionpro.CssClass = "";
            txtStartDatepro.CssClass = "";
            txtEndDatepro.CssClass = "";
            lblSuccessMessage.Text = string.Empty;
        }

        private void GetConfigDetails()
        {
            serviceClient=new CustomerServiceClient();
            string culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string conditionConfigXML = "10";
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount = 0;
            XmlDocument resulDoc = null;
            DataSet dsConfigDetails = new DataSet();
            if (serviceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionConfigXML, Culture))
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
                        if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Date")
                        {
                            hdndatereg.Value = dr["ConfigurationValue1"].ToString();
                        }
                        
                    }
                }
            }
        }

        
        }
}
