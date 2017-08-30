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
using System.Xml;
using System.ServiceModel;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.ClubcardService;
using CCODundeeApplication.MarketingService;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CCODundeeApplication
{
    public partial class AddPoints : System.Web.UI.Page
    {

        #region Private Variables

        protected CustomerServiceClient customerClient = null;
        protected ClubcardServiceClient clubcardClient = null;
        protected MarketingServiceClient marketingClient = null;

        protected string errMsgPoints = string.Empty;
        protected string spanPoints = "display:none";

        protected string errPartner = string.Empty;
        protected string spanPartner = "display:none";

        protected string errReasonCode = string.Empty;
        protected string spanReasonCode = "display:none";

        Hashtable htCustomer = null;

        string culture = string.Empty;

        string domain = string.Empty;

        DataSet dsReasons = null;

        DataSet dsResult = null;
        DataSet dsStore = null;
        DataSet dsPartner = null;
        #endregion Private Variables

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            AttachClientScripts();
            HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
            XmlDocument xmlCapability = new XmlDocument();
            DataSet dsCapability = new DataSet();
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
            {
                #region RoleCapabilityImplementation

                xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                if (dsCapability.Tables.Count > 0)
                {

                    //Added by neeta for Add card to Account Req.
                    if (dsCapability.Tables[0].Columns.Contains("resetpassword") != false)
                    {
                        ResetPass.Disabled = false;
                    }
                    else
                    {
                        ResetPass.Disabled = true;
                        ResetPass.HRef = "";
                    }

                }
                #endregion
            }
            if (!IsPostBack)
            {
                Prepview();
            }
        }

        #endregion Page Load
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

        #region Event Handlers

        protected void btnAddPoints_Click(object sender, ImageClickEventArgs e)
        {

            DataSet dsCapability = Validate();
            string addUserXml = string.Empty;
            try
            {

                if (validatePage(dsCapability))
                {
                    customerClient = new CustomerServiceClient();
                    long objectid = 0;
                    string resultXml = string.Empty;
                    string errorXml = string.Empty;
                    string ReasonID = ddlReasonCode.SelectedValue.ToString().Split(',')[0];
                    htCustomer = new Hashtable();
                    htCustomer["total_points"] = txtPoints.Text;
                    htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                    htCustomer["TxnReasonID"] = ReasonID;
                    htCustomer["TescoStoreID"] = ddlStore.SelectedValue.ToString();
                    htCustomer["PartnerID"] = ddlPartner.SelectedValue.ToString();
                    htCustomer["MaxPoints"] = int.Parse(dsCapability.Tables[0].Rows[0]["MaxPoints"].ToString());
                    htCustomer["MinPoints"] = int.Parse(dsCapability.Tables[0].Rows[0]["MinPoints"].ToString());
                    addUserXml = Helper.HashTableToXML(htCustomer, "Clubcard");

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD Points.btnAddPoints_Click() Input XML :" + addUserXml);
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD Points.btnAddPoints_Click() Input XML :" + addUserXml);
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        if (customerClient.AddPoints(out objectid, out errorXml, out resultXml, addUserXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                        {
                            if (resultXml.Contains("Add Points Limit"))
                            {
                                errMsgPoints = resultXml.Split('~')[1].ToString();
                                spanPoints = "";
                                txtPoints.CssClass = "errorFld";
                            }
                            else
                            {
                                txtPoints.Text = string.Empty;
                                ddlPartner.SelectedIndex = -1;
                                ddlStore.SelectedIndex = -1;
                                ddlReasonCode.SelectedIndex = -1;
                                lblSuccessMessage.Text = GetLocalResourceObject("succMsg").ToString();
                                    //"Points succesfully added to your account.";
                                //Response.Redirect("ViewPoints.aspx", false);GetLocalResourceObject("txtEnterPoints").ToString();
                                Prepview();
                            }
                        }
                    }
                    else
                    {
                        //Response.Redirect("Default.aspx", false);
                    }
                }
                else
                {
                    lblSuccessMessage.Text = GetLocalResourceObject("correctMsg").ToString();
                    //"Please correct the following information";
                    //correctMsg//GetLocalResourceObject("correctMsg").ToString();
                }
                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Points.btnAddPoints_Click Input XML :" + addUserXml);
                NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Points.btnAddPoints_Click Input XML :" + addUserXml);
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Points.btnAddPoints_Click() Input XML :" + addUserXml + "Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Points.btnAddPoints_Click()Input XML :" + addUserXml + "Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Points.btnAddPoints_Click()");
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


        #endregion Event Handlers

        #region Private Methods

        /// <summary>
        /// Method to register the ClientID of controls.
        /// </summary>
        private void AttachClientScripts()
        {
            //Getting and Setting CouponsCanBeSelectedFromFirstRow to variable.
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RegisterDdlStatusClientID", "ddlReasonCodeClientId = '" + ddlReasonCode.ClientID + "';", true);
        }

        /// <summary>
        /// Method to get point balance.
        /// </summary>
        private void Prepview()
        {
            GetGroupDetails();
            GetStoreDetails();
            GetPartnerDetails();
            GetPointBalance();

        }

        /// <summary>
        /// Fetch All Groups From DataBase
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
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD Points.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD Points.GetGroupDetails()");
                #endregion

                customerClient = new CustomerServiceClient();
                dsReasons = new DataSet();

                if (customerClient.GetTransactionReasonCode(out errorXml, out resultXml, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsReasons.ReadXml(new XmlNodeReader(resulDoc));
                    FillGroupDropDown(dsReasons);
                }
                else
                {
                }

                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Points.GetGroupDetails()");
                NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Points.GetGroupDetails()");
                #endregion
            }
            ///
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Points.GetGroupDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Points.GetGroupDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Points.GetGroupDetails()");
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

        /// <summary>
        /// Fetch All StoreDetails From DataBase
        /// </summary>
        private void GetStoreDetails()
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            int rowCount, maxRows;
            string conditionXml = string.Empty;
            //DataSet dsStore=null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD Points.GetStoreDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD Points.GetStoreDetails()");
                #endregion

                clubcardClient = new ClubcardServiceClient();
                dsStore = new DataSet();
                maxRows = 0;
                rowCount = 0;
                maxRows = 200;
                //conditionXml = "FROMMCA";

                if (clubcardClient.ViewStores(out errorXml, out resultXml, conditionXml, rowCount, "en-GB", maxRows))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsStore.ReadXml(new XmlNodeReader(resulDoc));
                    FillStoreDropDown(dsStore);
                }
                else
                {
                }

                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Points.GetStoreDetails()");
                NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Points.GetStoreDetails()");
                #endregion
            }
            ///
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Points.GetGroupDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Points.GetGroupDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Points.GetGroupDetails()");
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

        /// <summary>
        /// Fetch All PartnerDetails From DataBase
        /// </summary>
        private void GetPartnerDetails()
        {
            int rowCount, maxRows;
            maxRows = 0;
            rowCount = 0;
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {

                marketingClient = new MarketingServiceClient();
                maxRows = 200;
                string DefaultCulture = ConfigurationSettings.AppSettings["CultureDefault"];
                //conditionXml = "FROMMCA";

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC AddPoints.GetPartnerDetails()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC AddPoints.GetPartnerDetails() input Xml-" + conditionXml);
                #endregion


                if (marketingClient.ViewPartners(out errorXml, out resultXml, conditionXml, rowCount, DefaultCulture, maxRows))
                {


                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsResult = new DataSet();
                    dsResult.ReadXml(new XmlNodeReader(resulDoc));
                    FillPartnerDropDown(dsResult);
                }

                else
                {
                }

                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Points.GetPartnerDetails()");
                NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Points.GetPartnerDetails()");
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Points.GetPartnerDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Points.GetPartnerDetails() Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Points.GetPartnerDetails()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

            }
            finally
            {
                if (marketingClient != null)
                {
                    if (marketingClient.State == CommunicationState.Faulted)
                    {
                        marketingClient.Abort();
                    }
                    else if (marketingClient.State != CommunicationState.Closed)
                    {
                        marketingClient.Close();
                    }
                }
            }
        }
        private void GetPointBalance()
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
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ADD Points.GetPointBalance()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ADD Points.GetPointBalance() CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion


                customerClient = new CustomerServiceClient();
                int rowCount = 0;
                int maxCount = 0;
                htCustomer = new Hashtable();
                htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                string objectXml = Helper.HashTableToXML(htCustomer, "customer");

                if (customerClient.GetPointsBalance(out errorXml, out resultXml, out rowCount, objectXml, maxCount, culture))
                {

                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsResult = new DataSet();
                    dsResult.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsResult.Tables.Count > 0)
                    {
                        string totalPoints = String.Format("{0:#,###}", Convert.ToInt32(dsResult.Tables[0].Rows[0][0].ToString()));
                        if (totalPoints == string.Empty)
                        {
                            ltrTotalPoints.Text = "0";
                        }
                        else
                            ltrTotalPoints.Text = totalPoints;

                    }
                    else
                        ltrTotalPoints.Text = "0";
                }

                #region Trace END
                NGCTrace.NGCTrace.TraceInfo("END: CSC ADD Points.GetPointBalance()");
                NGCTrace.NGCTrace.TraceDebug("END: CSC ADD Points.GetPointBalance() CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ADD Points.GetPointBalance() CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + " Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC ADD Points.GetPointBalance() CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + "Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ADD Points.GetPointBalance()");
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

        private void FillGroupDropDown(DataSet ds)
        {

            if (ds.Tables.Count > 0)
            {
                ddlReasonCode.Items.Clear();
                int i = 1;
                ddlReasonCode.Items.Insert(0, new ListItem("Choose Reason Code", "-11"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddlReasonCode.Items.Insert(i, new ListItem(dr["TransactionReasonDescEnglish"].ToString(), dr["TransactionReasonID"].ToString() + "," + (dr["AllowNegativePointsInd"].ToString())));
                    i++;
                }
            }
        }


        private void FillStoreDropDown(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                ddlStore.DataMember = "TescoStoreName";
                ddlStore.DataTextField = "TescoStoreName";
                ddlStore.DataValueField = "TescoStoreID";
                ddlStore.DataSource = ds.Tables[0];
                ddlStore.DataBind();
                ddlStore.Items.Insert(0, new ListItem("Choose Store", "-11"));
            }
        }

        private void FillPartnerDropDown(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                ddlPartner.DataMember = "PartnerName";
                ddlPartner.DataTextField = "PartnerName";
                ddlPartner.DataValueField = "PartnerID";
                ddlPartner.DataSource = ds.Tables[0];
                ddlPartner.DataBind();
                ddlPartner.Items.Insert(0, new ListItem("Choose Partner", "-1"));
            }
        }
        #endregion Private Methods




        public DataSet Validate()
        {
            XmlDocument xmlCapability = new XmlDocument();
            DataSet dsCapability = new DataSet();
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
            {
                xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                dsCapability.ReadXml(new XmlNodeReader(xmlCapability));
            }
            return dsCapability;
        }

        public bool validatePage(DataSet dsCapability)
        {
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC AddPoints.ValidatePage()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC AddPoints.ValidatePage()");
            #endregion
            ddlReasonCode.CssClass = "";
            ddlStore.CssClass = "";
            ddlPartner.CssClass = "";
            txtPoints.CssClass = "";
            string IsAllowNegativeInd = "0";
            string regNumeric = "^[0-9]*$";
            try
            {
                bool bErrorFlag = true;

                if (!Helper.IsRegexMatch(txtPoints.Text.Trim(), regNumeric, false, false))
                {
                    this.errMsgPoints = GetGlobalResourceObject("CSCGlobal", "ValidateNumValue").ToString();
                    this.spanPoints = "";
                    txtPoints.CssClass = "errorFld";
                    bErrorFlag = false;

                }
                else if (Convert.ToInt32(txtPoints.Text.Trim()) == 0)
                {
                    this.errMsgPoints = GetLocalResourceObject("txtEnterPoints").ToString();
                    this.spanPoints = "";
                    txtPoints.CssClass = "errorFld";
                    bErrorFlag = false;
                }

                if (ddlStore.SelectedValue.ToString() == "-11" && ddlStore.Enabled == true)
                {
                    this.errPartner = GetLocalResourceObject("lblPartnerStoreResource1").ToString();
                    this.spanPartner = "";

                    ddlStore.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (ddlPartner.SelectedValue.ToString() == "-1" && ddlPartner.Enabled == true)
                {
                    this.errPartner = GetLocalResourceObject("lblPartnerStoreResource1").ToString();
                    this.spanPartner = "";
                    ddlPartner.CssClass = "errorFld";

                    bErrorFlag = false;
                }

                if (ddlReasonCode.SelectedValue.ToString() == "-11")
                {
                    this.errReasonCode = GetLocalResourceObject("lclReasonCodeResource1").ToString();
                    this.spanReasonCode = "";
                    ddlReasonCode.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                else
                {

                    IsAllowNegativeInd = ddlReasonCode.SelectedValue.ToString().Split(',')[1];
                }
            
                if (IsAllowNegativeInd == "1")
                {
                    if (!String.IsNullOrEmpty(txtPoints.Text.Trim()))
                    {
                        if ((Convert.ToInt32(txtPoints.Text) * -1) < Convert.ToInt32(dsCapability.Tables[0].Rows[0]["MinPoints"].ToString()))
                        {
                            this.errMsgPoints = GetLocalResourceObject("txtEnterMinPoints").ToString();
                            this.spanPoints = "";
                            txtPoints.CssClass = "errorFld";
                            bErrorFlag = false;

                        }
                    }
                }


                if (!String.IsNullOrEmpty(txtPoints.Text.Trim()))
                {
                    if ((Convert.ToInt32(txtPoints.Text)) > Convert.ToInt32(dsCapability.Tables[0].Rows[0]["MaxPoints"].ToString()))
                    {
                        this.errMsgPoints = GetLocalResourceObject("txtEnterMaxPoints").ToString();
                        this.spanPoints = "";
                        txtPoints.CssClass = "errorFld";
                        bErrorFlag = false;

                    }
                }
                return bErrorFlag;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC AddPoints.ValidatePage() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC AddPoints.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC AddPoints.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
        }

        protected void rBtnStore_OnCheckedChanged(object sender, EventArgs e)
        {
            ddlPartner.SelectedIndex = 0;
            ddlPartner.Enabled = false;
            ddlStore.Enabled = true;
            ddlStore.CssClass = "";
            ddlPartner.CssClass = "";
            ddlReasonCode.CssClass = "";
            txtPoints.CssClass = "";
            lblSuccessMessage.Text = "";

        }

        protected void rBtnPartner_OnCheckedChanged(object sender, EventArgs e)
        {
            ddlStore.SelectedIndex = 0;
            ddlStore.Enabled = false;
            ddlPartner.Enabled = true;
            ddlStore.CssClass = "";
            ddlPartner.CssClass = "";
            ddlReasonCode.CssClass = "";
            txtPoints.CssClass = "";
            lblSuccessMessage.Text  = "";
            
            
        }


    }
}
