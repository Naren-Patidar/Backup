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
using System.Xml;
using System.ServiceModel;
using System.Web.UI.WebControls;
using CCODundeeApplication.AdminService;

namespace CCODundeeApplication
{
    public partial class AddPromotionCode : System.Web.UI.Page
    {
        DataSet dsPromotionalcode = null;
        AdminServiceClient objAdmin = null;
        Hashtable htCustomer = null;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;
        protected string errMsgStoreName = string.Empty;
        protected string errMsgWelcomePoints = string.Empty;
        protected string errMsgTescoStore = string.Empty;
        protected string errMsgStartdate = string.Empty;
        protected string spanStoreName = "display:none";
        protected string spanStoreWelPoints = "display:none";
        protected string SpanStartdate = "display:none";
        protected string spanStoreNumber = "display:none";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPromotionalCodeGrid();
                String DeleteMessage = "Are you sure you would like to delete selected PromotionCode ?";
                btnDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + DeleteMessage + "')");
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
                            dvAddUser.Visible = true;
                        }
                        else
                        {
                            dvAddUser.Visible = false;

                        }
                    
                    }
                }
                #endregion
            }
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
            btnAddSave.Visible = true;

        }

        public void BindPromotionalCodeGrid()
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
                        grdPromotionCode.DataSource = dsPromotionalcode.Tables[0].DefaultView;
                        grdPromotionCode.DataBind();
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
                htCustomer["PromotionCode"]=txtPromotioncode.Text.ToString();
                htCustomer["PromotionCodeDescEnglish"]=txtDescription.Text.ToString();
                htCustomer["StartDate"]=txtStartDate.Text.ToString();
                htCustomer["EndDate"]=txtEndDate.Text.ToString();

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
                        lblSuccessMessage.Text = "Promotion code added successfully";
                        BindPromotionalCodeGrid();
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

        protected void grdPromotionCode_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    ltrStartDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));

                }

                if (ltrEndDate.Text != null)
                {
                    DateTime LastUpdateDate = Convert.ToDateTime(ltrEndDate.Text.ToString());
                    ltrEndDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));

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
                            lnkSelectCustomer.Visible = true;
                        }
                        else
                        {
                            lnkSelectCustomer.Visible = false;

                        }

                    }
                }

            }

        }


        protected void grdPromotionCode_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        BindPromotionalCodeGrid();
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                }

                if (e.CommandName == "Select")
                {
                    string[] PromotionCodearr = e.CommandArgument.ToString().Split(';');
                    txtPromotioncode.Text = PromotionCodearr[0].ToString();
                    txtDescription.Text = PromotionCodearr[1].ToString();
                    string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                    //txtStartDate.Text = PromotionCodearr[2].ToString();
                    if (PromotionCodearr[2].ToString() != "")
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(PromotionCodearr[2].ToString());
                        txtStartDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));

                    }

                    if (PromotionCodearr[3].ToString() != "")
                    {
                        DateTime LastUpdateDate = Convert.ToDateTime(PromotionCodearr[3].ToString());
                        txtEndDate.Text = LastUpdateDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));

                    }
                    btnAddSave.Visible = false;
                    btnDelete.Visible = true;
                    btnUpdate.Visible = true;

                
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

        protected void grdPromotionCode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected bool validate()
        {
            bool bErrorFlag = true;
            //DateTime fromdate;
            //DateTime todate;
            txtPromotioncode.CssClass = "";
            txtDescription.CssClass = "";
            txtStartDate.CssClass = "";
            txtEndDate.CssClass = "";
            try
            {
                string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                if (txtPromotioncode.Text.ToString() == string.Empty && txtDescription.Text.ToString() == string.Empty && txtStartDate.Text.ToString() == string.Empty && txtEndDate.Text.ToString() == string.Empty)
                {
                    lblSuccessMessage.Text = "Please enter valid details";
                    bErrorFlag = false;
                }

                if (txtPromotioncode.Text.ToString() == string.Empty)
                {
                    this.errMsgStoreName = "Please enter Valid promotion code";//"Please enter valid Primary Id";
                    this.spanStoreName = "";
                    this.txtPromotioncode.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (txtDescription.Text.ToString() == string.Empty)
                {
                    this.errMsgTescoStore = "Please enter valid description";
                    this.spanStoreNumber = "";
                    this.txtDescription.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if(txtStartDate.Text.ToString()==String.Empty)
                {
                    this.errMsgStartdate = "Please enter valid Start Date";
                    this.SpanStartdate = "";
                    this.txtStartDate.CssClass = "errorFld";
                    bErrorFlag = false;

                }
                if(txtEndDate.Text.ToString()==string.Empty)
                {
                    this.errMsgWelcomePoints = "Please enter valid End Date";
                    this.spanStoreWelPoints = "";
                    this.txtEndDate.CssClass = "errorFld";
                    bErrorFlag = false;

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


        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            objAdmin = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();
            lblSuccessMessage.Text = "";
            if (validate())
            {
                htCustomer["PromotionCode"] = txtPromotioncode.Text.ToString();
                htCustomer["PromotionCodeDescEnglish"] = txtDescription.Text.ToString();
                //DateTime DOB = DateTime.Parse(ddlDay0.SelectedValue + "/" + ddlMonth0.SelectedValue + "/" + ddlYear0.SelectedValue, new CultureInfo("en-GB"));
                //htCustomer["DateOfBirth"] = DOB.ToLongDateString();
                DateTime StartDateFormat = DateTime.Parse(txtStartDate.Text.ToString(), new CultureInfo("en-GB"));
                htCustomer["StartDate"] = StartDateFormat.ToLongDateString();
                DateTime EndDateFormat = DateTime.Parse(txtEndDate.Text.ToString(), new CultureInfo("en-GB"));
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
                        ClearSelection();
                        BindPromotionalCodeGrid();
                    }
                }
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            string objectXml = string.Empty;
            lblSuccessMessage.Text = "";
            try
            {
                objAdmin = new AdminServiceClient();
                String PromotionCode = txtPromotioncode.Text.ToString();
                htCustomer = new Hashtable();
                htCustomer["PromotionCode"] = PromotionCode;
                objectXml = Helper.HashTableToXML(htCustomer, "TblPromotionCode");
                if (objAdmin.UpdatePromotionCode(out resultXml, objectXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                {
                    ClearSelection();
                    lblSuccessMessage.Text = "Promotion code deleted successfully";
                    BindPromotionalCodeGrid();
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

        public void ClearSelection()
        {
            txtPromotioncode.Text = "";
            txtDescription.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            objAdmin = new AdminServiceClient();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            htCustomer = new Hashtable();
            lblSuccessMessage.Text = "";
            if(validate())
            {
            htCustomer["PromotionCode"] = txtPromotioncode.Text.ToString();
            htCustomer["PromotionCodeDescEnglish"] = txtDescription.Text.ToString();
            //DateTime DOB = DateTime.Parse(ddlDay0.SelectedValue + "/" + ddlMonth0.SelectedValue + "/" + ddlYear0.SelectedValue, new CultureInfo("en-GB"));
            //htCustomer["DateOfBirth"] = DOB.ToLongDateString();
            DateTime StartDateFormat = DateTime.Parse(txtStartDate.Text.ToString(), new CultureInfo("en-GB"));
            htCustomer["StartDate"] = StartDateFormat.ToLongDateString();
            DateTime EndDateFormat = DateTime.Parse(txtEndDate.Text.ToString(), new CultureInfo("en-GB"));
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
                    lblSuccessMessage.Text = "Promotion code Updated successfully";
                    BindPromotionalCodeGrid();
                }
            }
            }
        }

        
        
      







    }
}
