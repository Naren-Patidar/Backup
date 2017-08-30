using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
//using CCODundeeApplication.CustomerService;
using CCODundeeApplication.ClubcardService;
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;

namespace CCODundeeApplication
{
    public partial class UserNotes : System.Web.UI.Page
    {
        #region Private Variables

        //protected CustomerServiceClient customerClient = null;
        protected ClubcardServiceClient serviceClient = null;
        DataSet dsReasons = null;
        string culture = string.Empty;
        Hashtable htCustomer = null;
        DataSet dsCustomerInfo = null;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;

        #endregion Private Variables

        /// <summary>
        /// Get Data from Database during page load for call history.
        /// get the Call Reason Codes from Database to bind in Reason code DropDown list
        /// </summary>
        /// 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
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
                            //HtmlLink liadmin = (HtmlLink)Master.FindControl("liadmin");
                            HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                            HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                            HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                            //HtmlAnchor userNotes = (HtmlAnchor)Master.FindControl("userNotes");

                            // Added For Call Log

                            Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                            lblUserNotes.Visible = true;


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
                            if (dsCapability.Tables[0].Columns.Contains("ViewFindCustomer") != false)
                            {
                                findCustomer.Disabled = false;
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
                    AttachClientScripts();
                    GetGroupDetails();
                    GetCallDetails();
                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical:UserNotes.Page_Load - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceError("Error:UserNotes.Page_Load - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning:UserNotes.Page_Load");
                    NGCTrace.NGCTrace.ExeptionHandling(exp);
                    #endregion Trace Error
                    throw exp;
                }

            }
        }


        #region Private Methods

        /// <summary>
        /// Fetch All Reason Code From DataBase
        /// </summary>
        private void GetGroupDetails()
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                serviceClient = new ClubcardServiceClient();
                dsReasons = new DataSet();

                if (serviceClient.GetCallReasonCode(out errorXml, out resultXml, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsReasons.ReadXml(new XmlNodeReader(resulDoc));
                    FillGroupDropDown(dsReasons, culture);
                }
            }

            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Call History CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }

        }

        /// <summary>
        /// Bind all the Reason Code to the DropDown List
        /// </summary>

        private void FillGroupDropDown(DataSet ds, string culture)
        {
            if (ds.Tables.Count > 0)
            {
                if(culture=="en-GB")
                {
                    ddlReasonCode.DataTextField = "ReasonCodeDescEnglish";
                }
                else
                {
                    ddlReasonCode.DataTextField = "ReasonCodeDescLocal";
                }
                ddlReasonCode.DataValueField = "ReasonCodeID";
                ddlReasonCode.DataSource = ds.Tables[0];
                ddlReasonCode.DataBind();
                //ddlReasonCode.Items.Insert(0, new ListItem("Choose Reason Code", "-1"));
            }
        }

        private void AttachClientScripts()
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RegisterDdlStatusClientID", "ddlReasonCodeClientId = '" + ddlReasonCode.ClientID + "';", true);
        }

        /// <summary>
        /// Get All the logged Call Details from Database
        /// binding all the call Details to the GridView
        /// </summary>

        private void GetCallDetails()
        {
            try
            {
                string resultXml = string.Empty;
                //For pagination
                int pageSize = 20;
                XmlDocument resulDoc = null;
                long CustomerId = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                string ClutId = ConfigurationManager.AppSettings["Culture"].ToString();
                serviceClient = new ClubcardServiceClient();

                if (serviceClient.ViewCustomerHelplineInformation(out resultXml, CustomerId, ClutId))
                   
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsCustomerInfo = new DataSet();
                    dsCustomerInfo.ReadXml(new XmlNodeReader(resulDoc));

                    //Save the dataset to view state for postback cycles
                    ViewState["dsCallDetails"] = dsCustomerInfo;

                    if (dsCustomerInfo.Tables.Count > 0)
                    {
                        DataTable tblTransactions;
                        tblTransactions = dsCustomerInfo.Tables["HelplineDetails"];
                        ViewState["UserNoteDS"] = tblTransactions;
                        dvSearchResults.Visible = true;
                        grdCallDetails.DataSource = tblTransactions;
                        grdCallDetails.DataBind();

                        lblCallDetails.Text = string.Empty;
                    }
                    else
                    {
                        lblCallDetails.Text = "No Call Detail(s) were found For this Customer.";
                    }

                }
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Customer Call Logged :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }

        }


        #endregion Private Methods

        /// <summary>
        /// Save all the call Details in the database on Save Button Click
        /// </summary>

        protected void btnConfirmCustomerDtls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (txtCallDetails.Text.Length > 0)
                {
                    if (txtCallDetails.Text.Length <= 800)
                    {
                        serviceClient = new ClubcardServiceClient();
                        string resultXml = string.Empty;
                        string errorXml = string.Empty;
                        lblSuccessMessage.Text = string.Empty;
                        long objectId = 0;
                        htCustomer = new Hashtable();

                        htCustomer["ReasonCodeID"] = ddlReasonCode.SelectedValue.ToString();
                        htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID");
                        htCustomer["CallDetails"] = txtCallDetails.Text.ToString();
                        string addCallXml = Helper.HashTableToXML(htCustomer, "Helpline");
                        if (Convert.ToInt16(ddlReasonCode.SelectedValue) != -1)
                        {
                            if (serviceClient.AddHelplineInformation(out objectId, out resultXml, addCallXml, Convert.ToInt16(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                            {
                                txtCallDetails.Text = "";
                                ddlReasonCode.SelectedIndex = 0;
                                lblSuccessMessage.Text = GetLocalResourceObject("lblSuccessMessage").ToString();
                                GetCallDetails();

                            }
                        }
                        else
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("lblReasonCodeError").ToString();
                        }

                    }
                    else
                    {
                        lblSuccessMessage.Text = GetLocalResourceObject("lblMaxLengthError").ToString();
                    }
                }
                else
                {
                    lblSuccessMessage.Text = GetLocalResourceObject("lblWarningAlert").ToString();
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('"+ GetLocalResourceObject("lblWarningAlert").ToString() +"');", true);
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:UserNotes.btnConfirmCustomerDtls_Click - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:UserNotes.btnConfirmCustomerDtls_Click - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:UserNotes.btnConfirmCustomerDtls_Click");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }

            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }

        }

        /// <summary>
        /// Onclick of View Details bind row data
        /// </summary>

        protected void grdTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Local Variables
            string showMoreInfoLink = string.Empty;
            #endregion

            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);

                    #region RoleCapabilityImplementation
                    LinkButton lnkShowInfo = (LinkButton)e.Row.Cells[4].FindControl("lnkSelectCallDetails");
                    if (ViewState["dsCallDetails"] == null)
                    {
                        showMoreInfoLink = "modalMoreInfoShow('" + lnkShowInfo.CommandArgument.ToString() + "');return false;";
                        lnkShowInfo.OnClientClick = showMoreInfoLink;
                    }
                    else if (ViewState["dsCallDetails"].ToString() == string.Empty)
                    {
                        showMoreInfoLink = "modalMoreInfoShow('" + lnkShowInfo.CommandArgument.ToString() + "');return false;";
                        lnkShowInfo.OnClientClick = showMoreInfoLink;
                    }
                    else
                    {
                        showMoreInfoLink = "modalMoreInfoShow('" + lnkShowInfo.CommandArgument.ToString() + "');return false;";
                        lnkShowInfo.OnClientClick = showMoreInfoLink;
                    }
                }
                    #endregion


            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:UserNotes.grdTransactions_RowDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:UserNotes.grdTransactions_RowDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:UserNotes.grdTransactions_RowDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }
        }

        protected void grdCallDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdCallDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable tblTransactions = null;
            try
            {
                grdCallDetails.PageIndex = e.NewPageIndex;
                tblTransactions = (DataTable)ViewState["UserNoteDS"];
                grdCallDetails.DataSource = tblTransactions;
                grdCallDetails.DataBind();
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical:UserNotes.grdCallDetails_PageIndexChanging - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:UserNotes.grdCallDetails_PageIndexChanging - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:UserNotes.grdCallDetails_PageIndexChanging");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
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
                Response.Redirect("Default.aspx", false);
        }
        #endregion

    }
}
