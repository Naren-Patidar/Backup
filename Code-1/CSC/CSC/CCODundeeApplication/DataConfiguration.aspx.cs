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
using CCODundeeApplication.CustomerService;
using System.Globalization;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using NGCTrace;

namespace CCODundeeApplication
{
    public partial class DataConfiguration : System.Web.UI.Page
    {
       

            //Used in .aspx page for hiding/unhiding the controls
            protected string spanStylePtSummStartDate = "display:none";
            protected string spanStylePtSummEnddate = "display:none";
            protected string spanStyleExchangesStartDate = "display:none";
            protected string spanStyleExchangesEndDate = "display:none";
            protected string spanStyleCurXmasStartDate = "display:none";
            protected string spanStyleCurXmasEndDate = "display:none";
            protected string spanStyleNextXmasStartDate = "display:none";
            protected string spanStyleNextXmasEndDate = "display:none";
            protected string spanStyleVoucherStartDate = "display:none";
            protected string spanStyleVoucherEndDate = "display:none";
            protected string spanStyleVoucherCutoffStartDate = "display:none";
            protected string spanStyleVoucherCutoffEndDate = "display:none";
            protected string spanStyleLatestStatementStartDate = "display:none";
            protected string spanStyleLatestStatementEndDate = "display:none";
            protected string spanStyleFlag = "display:none";

            protected string errMsgPtSummStartDate = string.Empty;
            protected string errMsgPtSummEnddate = string.Empty;
            protected string errMsgExchangesStartDate = string.Empty;
            protected string errMsgExchangesEndDate = string.Empty;
            protected string errMsgCurXmasStartDate = string.Empty;
            protected string errMsgCurXmasEndDate = string.Empty;
            protected string errMsgNextXmasStartDate = string.Empty;
            protected string errMsgNextXmasEndDate = string.Empty;
            protected string errMsgVoucherStartDate = string.Empty;
            protected string errMsgVoucherEndDate = string.Empty;
            protected string errMsgVoucherCutoffStartDate = string.Empty;
            protected string errMsgVoucherCutoffEndDate = string.Empty;
            protected string errMsglateststatementStartDate = string.Empty;
            protected string errMsglateststatementEndDate = string.Empty;
            protected string errMsgFlag = string.Empty;

            protected CustomerServiceClient customerObj = null;
            int rowCount = 0;
            string culture = ConfigurationManager.AppSettings["Culture"];
            XmlDocument resulDoc = null;
            string conditionXML = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            DataSet dsCapability = null;
            XmlDocument xmlCapability = null;
      

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
 

       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Page_Load");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Page_Load");
                #endregion
                if (!IsPostBack)
                {
                    //Hide the links
                    //Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                    //lblCustomerDtl.Visible = false;
                    //Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                    //lblCustomePref.Visible = false;
                    //Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                    //lblCustomerPts.Visible = false;
                    //Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                    //lblCustomerCards.Visible = false;
                    //Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                    //lblXmasSaver.Visible = false;

                    ContentPlaceHolder custDetailsLeftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                    custDetailsLeftNav.Visible = false;
                 

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
                            HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                            HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                            HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                            Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                            if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                            {
                                PointsEarnedReports.Disabled = false;
                            }
                            else
                            {
                                PointsEarnedReports.Disabled = true;
                                PointsEarnedReports.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                            {
                                customerCoupon.Disabled = false;
                            }
                            else
                            {
                                customerCoupon.Disabled = true;
                                customerCoupon.HRef = "";
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


                            if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                            {
                                DeLinkAccount.Visible = true;
                            }
                            else
                            {
                                DeLinkAccount.Visible = false;
                                
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

                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                            {
                                cutomerDetails.Disabled = false;
                            }
                            else
                            {
                                cutomerDetails.Disabled = true;
                                cutomerDetails.HRef = "";
                            }

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

                            if (dsCapability.Tables[0].Columns.Contains("UpdateDataConfiguaration") != false)
                            {
                            }
                            else
                            {
                                dvBody.Disabled = true;
                                btnConfirmConfigDtls.Attributes.Add("disabled", "true");
                                txtCurXmasEndDate.Enabled = false;
                                txtCurXmasStartDate.Enabled = false;
                                txtExchangesEndDate.Enabled = false;
                                txtExchangesStartDate.Enabled = false;
                                txtNextXmasEndDate.Enabled = false;
                                txtNextXmasStartDate.Enabled = false;
                                txtPtSummEnddate.Enabled = false;
                                txtPtSummStartDate.Enabled = false;
                                txtVoucherEndDate.Enabled = false;
                                txtVoucherStartDate.Enabled = false;
                                txtFlag.Enabled = false;
                                txtLatestStatementStartDate.Enabled = false;
                                txtLatestStatementEndDate.Enabled = false;
                            }
                        }
                    }
                    #endregion

                    string sxmasdates = ConfigurationManager.AppSettings["xmasdatesconfigreq"].ToString();
                    string sbigexchangedates = ConfigurationManager.AppSettings["bigexchangedatesconfigreq"].ToString();
                    if (sxmasdates == "yes")
                    {
                        xmasdates.Visible = true;
                    }
                    else if (sxmasdates == "no")
                    {
                        xmasdates.Visible = false;
                    }

                    if (sbigexchangedates == "yes")
                    {
                        bigexchangedates.Visible = true;
                    }
                    else if (sbigexchangedates == "no")
                    {
                        bigexchangedates.Visible = false;
                    }
                    customerObj = new CustomerServiceClient();
                    string CardType = "7,10";
                    if (customerObj.GetConfigDetails(out errorXml, out resultXml, out rowCount,CardType, culture))
                    {
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        DataSet dsConfigDetails = new DataSet();
                        dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                        System.Globalization.CultureInfo cultEnGb = new System.Globalization.CultureInfo("en-GB");
                    
                       if (dsConfigDetails.Tables.Count > 0)
                        {


                            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                            {

                                if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "PtsSummaryDates")
                                {
                                    DateTime PtSummStartDate = Convert.ToDateTime(dr["ConfigurationValue1"].ToString());
                                    txtPtSummStartDate.Text = PtSummStartDate.ToString("dd/MM/yyyy");
                                                                       
                                    DateTime PtSummEnddate = Convert.ToDateTime(dr["ConfigurationValue2"].ToString());
                                    txtPtSummEnddate.Text = PtSummEnddate.ToString("dd/MM/yyyy");

                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "YourExchangesDates")
                                {
                                    DateTime ExchangesStartDate =Convert.ToDateTime(dr["ConfigurationValue1"].ToString());
                                    txtExchangesStartDate.Text = ExchangesStartDate.ToString("dd/MM/yyyy");

                                    DateTime ExchangesEndDate =Convert.ToDateTime(dr["ConfigurationValue2"].ToString());
                                    txtExchangesEndDate.Text = ExchangesEndDate.ToString("dd/MM/yyyy");

                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverCurrDates")
                                {
                                    DateTime CurXmasStartDate = Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim());
                                    txtCurXmasStartDate.Text = CurXmasStartDate.ToString("dd/MM/yyyy");

                                    DateTime CurXmasEndDate = Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim());
                                    txtCurXmasEndDate.Text = CurXmasEndDate.ToString("dd/MM/yyyy");



                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverNextDates")
                                {
                                    DateTime NextXmasStartDate = Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim());
                                    txtNextXmasStartDate.Text = NextXmasStartDate.ToString("dd/MM/yyyy");

                                    DateTime NextXmasEndDate = Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim());
                                    txtNextXmasEndDate.Text = NextXmasEndDate.ToString("dd/MM/yyyy");



                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "SmartVoucherDates")
                                {
                                    DateTime VoucherStartDate =Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim());
                                    txtVoucherStartDate.Text = VoucherStartDate.ToString("dd/MM/yyyy");

                                    DateTime VoucherEndDate =Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim());
                                    txtVoucherEndDate.Text = VoucherEndDate.ToString("dd/MM/yyyy");



                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "MyLatestStatementDates")
                                {
                                    DateTime LatestStatementStartDate = Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim());
                                    txtLatestStatementStartDate.Text = LatestStatementStartDate.ToString("dd/MM/yyyy");

                                    DateTime LatestStatementEndDate = Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim());
                                    txtLatestStatementEndDate.Text = LatestStatementEndDate.ToString("dd/MM/yyyy");



                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "YourExchangesFlag")
                                {

                                    txtFlag.Text = dr["ConfigurationValue1"].ToString().Trim();

                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Date")
                                {
                                    hdndatereg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Numeric")
                                {
                                    hdnNumericeg.Value = dr["ConfigurationValue1"].ToString();
                                }

                            }



                            //txtPtSummStartDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[0]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtPtSummEnddate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[1]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtExchangesStartDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[2]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtExchangesEndDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[3]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtCurXmasStartDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[4]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtCurXmasEndDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[5]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtNextXmasStartDate.Text =Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[6]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtNextXmasEndDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[7]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtVoucherStartDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[8]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                            //txtVoucherEndDate.Text = Convert.ToDateTime(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[9]["ConfigurationValue1"].ToString().Trim()).ToShortDateString();
                           // txtFlag.Text = Convert.ToInt16(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows[10]["ConfigurationValue1"].ToString().Trim()).ToString();


                        }
                    }
                   // Add Javascript to the Save Changes button(top button)
                    btnConfirmConfigDtls.Attributes.Add("onclick", "return ValidateDateForConfig('" + txtPtSummStartDate.ClientID + "','"
                    + txtPtSummEnddate.ClientID + "','"
                    + txtExchangesStartDate.ClientID + "','"
                    + txtExchangesEndDate.ClientID + "','"
                    + txtCurXmasStartDate.ClientID + "','"
                    + txtCurXmasEndDate.ClientID + "','"
                    + txtNextXmasStartDate.ClientID + "','"
                    + txtNextXmasEndDate.ClientID + "','"
                    + txtVoucherStartDate.ClientID + "','"
                    + txtVoucherEndDate.ClientID + "','"
                    + txtLatestStatementStartDate.ClientID + "','"
                    + txtLatestStatementEndDate.ClientID + "','"
                    + txtFlag.ClientID + "','"
                    + lblSuccessMessage.ClientID + "')");
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Page_Load");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Page_Load");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Page_Load - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC Page_Load  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Page_Load");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
            finally
            {
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }
            }
        }

        

        protected void btnConfirmConfigDtls_Click(object sender, ImageClickEventArgs e)
        {
            CustomerServiceClient service = null;
            Hashtable htConfig = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC btnConfirmConfigDtls_Click");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC btnConfirmConfigDtls_Click");
                #endregion
                if (ValidateDate())
                {
                    htConfig = new Hashtable();
                    htConfig["PtSummStartDate"] = txtPtSummStartDate.Text.Trim();
                    htConfig["PtSummEndDate"] = txtPtSummEnddate.Text.Trim() + " 23:59:59.000";
                    htConfig["ExchangesStartDate"] = txtExchangesStartDate.Text.Trim();
                    htConfig["ExchangesEndDate"] = txtExchangesEndDate.Text.Trim() + " 23:59:59.000";
                    htConfig["CurXmasStartDate"] = txtCurXmasStartDate.Text.Trim();
                    htConfig["CurXmasEndDate"] = txtCurXmasEndDate.Text.Trim() + " 23:59:59.000";
                    htConfig["NextXmasStartDate"] = txtNextXmasStartDate.Text.Trim();
                    htConfig["NextXmasEndDate"] = txtNextXmasEndDate.Text.Trim() + " 23:59:59.000";
                    htConfig["VoucherStartDate"] = txtVoucherStartDate.Text.Trim();
                    htConfig["VoucherEndDate"] = txtVoucherEndDate.Text.Trim() + " 23:59:59.000";

                    htConfig["LatestStatementStartDate"] = txtLatestStatementStartDate.Text.Trim();
                    htConfig["LatestStatementEndDate"] = txtLatestStatementEndDate.Text.Trim() + " 23:59:59.000";

                    htConfig["Flag"] = txtFlag.Text.Trim();
                    

                    //Prepare the parameters for service call
                    string updateXml = Helper.HashTableToXML(htConfig, "ActiveDateRangeConfig");
                    string errorXml;
                    long CustomerID;
                    string consumer = string.Empty;

                    consumer = Helper.GetTripleDESEncryptedCookieValue("UserName").ToString();
                    
                    service = new CustomerServiceClient();
                    
                    //CCO Service call to update customer details
                    if (service.UpdateConfig(out errorXml, out CustomerID, updateXml, consumer))
                    {
                        lblSuccessMessage.Text = GetLocalResourceObject("lclSuccessMsg.Text").ToString();//"Your changes have been saved";
                    }
                    else
                    {
                        throw new Exception(errorXml);
                    }
                }

                else
                {
                    lblSuccessMessage.Text = GetLocalResourceObject("lclErrorMsg.Text").ToString();// "Please correct following information";
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC btnConfirmConfigDtls_Click");
                NGCTrace.NGCTrace.TraceDebug("End: CSC btnConfirmConfigDtls_Click");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC btnConfirmConfigDtls_Click - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC btnConfirmConfigDtls_Click  - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC btnConfirmConfigDtls_Click");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
            }
            finally
            {
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }
            }
        }
            protected bool ValidateDate()
            {
                bool bErrorFlag = true;
                try
                {
                    //string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
                    //string regNumeric = @"^[0-1]*$";
                     string regDate=hdndatereg.Value;
                     string regNumeric = hdnNumericeg.Value;

                    //Clear the class
                    txtPtSummStartDate.CssClass = "";
                    txtPtSummEnddate.CssClass = "";
                    txtPtSummStartDate.CssClass = "";
                    txtExchangesStartDate.CssClass = "";
                    txtFlag.CssClass = "";
                    txtCurXmasStartDate.CssClass = "";
                    txtCurXmasEndDate.CssClass = "";
                    txtNextXmasStartDate.CssClass = "";
                    txtNextXmasEndDate.CssClass = "";
                    txtVoucherStartDate.CssClass = "";
                    txtVoucherEndDate.CssClass = "";
                    txtLatestStatementStartDate.CssClass = "";
                    txtLatestStatementEndDate.CssClass = "";
                

                    int errorcount = 0;





                   
                    if (!Helper.IsRegexMatch(txtPtSummStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgPtSummStartDate = GetLocalResourceObject("ValidDate.Text").ToString();// "Enter a valid date";
                        spanStylePtSummStartDate = "";
                        txtPtSummStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtPtSummEnddate.Text.Trim(), regDate, false, false) )
                    {
                        errMsgPtSummEnddate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStylePtSummEnddate = "";
                        txtPtSummEnddate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                   
                    
                    if (!Helper.IsRegexMatch(txtExchangesStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgExchangesStartDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleExchangesStartDate = "";
                        txtExchangesStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtExchangesEndDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgExchangesEndDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleExchangesEndDate = "";
                        txtExchangesEndDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    
                    if (!Helper.IsRegexMatch(txtCurXmasStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgCurXmasStartDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleCurXmasStartDate = "";
                        txtCurXmasStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtCurXmasEndDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgCurXmasEndDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleCurXmasEndDate = "";
                        txtCurXmasEndDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    
                    if (!Helper.IsRegexMatch(txtNextXmasStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgNextXmasStartDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleNextXmasStartDate = "";
                        txtNextXmasStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtNextXmasEndDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgNextXmasEndDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleNextXmasEndDate = "";
                        txtNextXmasEndDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    
                    if (!Helper.IsRegexMatch(txtVoucherStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgVoucherStartDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleVoucherStartDate = "";
                        txtVoucherStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtVoucherEndDate.Text.Trim(), regDate, false, false))
                    {
                        errMsgVoucherEndDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleVoucherEndDate = "";
                        txtVoucherEndDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    

                    if (!Helper.IsRegexMatch(txtLatestStatementStartDate.Text.Trim(), regDate, false, false))
                    {
                        errMsglateststatementStartDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleLatestStatementStartDate = "";
                        txtLatestStatementStartDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                    if (!Helper.IsRegexMatch(txtLatestStatementEndDate.Text.Trim(), regDate, false, false))
                    {
                        errMsglateststatementEndDate = GetLocalResourceObject("ValidDate.Text").ToString(); //"Enter a valid date";
                        spanStyleLatestStatementEndDate = "";
                        txtLatestStatementEndDate.CssClass = "errorFld";
                        bErrorFlag = false;
                        errorcount = 1;
                    }
                   


                    if (!Helper.IsRegexMatch(txtFlag.Text.Trim(), regNumeric, false, false))
                    {
                        errMsgFlag = GetLocalResourceObject("Either01Msg.Text").ToString();// "Please enter either 0 and 1";
                        spanStyleFlag = "";
                        txtFlag.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    if (errorcount == 0)
                    {
                        if (ValidateDateRanges())
                        {
                            return bErrorFlag = true;
                        }
                        else
                        {
                            return bErrorFlag = false;
                        }
                      
                    }
                    else
                    {
                        return bErrorFlag;
                    }
                   
                }
                catch (Exception exp)
                {
                    Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                        "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                    throw exp;
                }
            }

            public bool ValidateDateRanges()
            {
                bool bErrorFlag = true;

                if (Convert.ToDateTime(txtPtSummStartDate.Text.Trim()) > (Convert.ToDateTime(txtPtSummEnddate.Text.Trim())))
                {
                    errMsgPtSummStartDate = GetLocalResourceObject("CompareDate.Text").ToString(); //"Start date should be less than end date";
                    spanStylePtSummStartDate = "";
                    txtPtSummStartDate.CssClass = "errorFld";
                    spanStylePtSummEnddate = "";
                    txtPtSummEnddate.CssClass = "errorFld";
                    bErrorFlag = false;
                }

                if (Convert.ToDateTime(txtExchangesStartDate.Text.Trim()) > (Convert.ToDateTime(txtExchangesEndDate.Text.Trim())))
                {
                    errMsgExchangesStartDate = GetLocalResourceObject("CompareDate.Text").ToString(); //"Start date should be less than end date";
                    spanStyleExchangesStartDate = "";
                    txtExchangesStartDate.CssClass = "errorFld";
                    spanStyleExchangesEndDate = "";
                    txtExchangesEndDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (Convert.ToDateTime(txtCurXmasStartDate.Text.Trim()) > (Convert.ToDateTime(txtCurXmasEndDate.Text.Trim())))
                {
                    errMsgCurXmasStartDate = GetLocalResourceObject("CompareDate.Text").ToString(); //"Start date should be less than end date";
                    spanStyleCurXmasStartDate = "";
                    txtCurXmasStartDate.CssClass = "errorFld";
                    spanStyleCurXmasEndDate = "";
                    txtCurXmasEndDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }

                if (Convert.ToDateTime(txtNextXmasStartDate.Text.Trim()) > (Convert.ToDateTime(txtNextXmasEndDate.Text.Trim())))
                {
                    errMsgNextXmasEndDate = GetLocalResourceObject("CompareDate.Text").ToString(); //"Start date should be less than end date";
                    spanStyleNextXmasEndDate = "";
                    txtNextXmasEndDate.CssClass = "errorFld";
                    spanStyleNextXmasStartDate = "";
                    txtNextXmasStartDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (Convert.ToDateTime(txtCurXmasStartDate.Text.Trim()) > (Convert.ToDateTime(txtNextXmasStartDate.Text.Trim())))
                {
                    errMsgNextXmasStartDate = GetLocalResourceObject("CurrCompareDate.Text").ToString();//"Cuttent start date should be less than next start date";
                    spanStyleCurXmasStartDate = "";
                    txtCurXmasStartDate.CssClass = "errorFld";
                    spanStyleNextXmasStartDate = "";
                    txtNextXmasStartDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (Convert.ToDateTime(txtCurXmasEndDate.Text.Trim()) > (Convert.ToDateTime(txtNextXmasEndDate.Text.Trim())))
                {
                    errMsgNextXmasEndDate = GetLocalResourceObject("CurrCompareDate.Text").ToString(); //"Cuttent end date should be less than next end date";
                    spanStyleNextXmasEndDate = "";
                    txtNextXmasEndDate.CssClass = "errorFld";
                    spanStyleNextXmasEndDate = "";
                    txtNextXmasEndDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (Convert.ToDateTime(txtVoucherStartDate.Text.Trim()) > (Convert.ToDateTime(txtVoucherEndDate.Text.Trim())))
                {
                    errMsgVoucherStartDate = GetLocalResourceObject("CompareDate.Text").ToString();// "Start date should be less than end date";
                    spanStyleVoucherStartDate = "";
                    txtVoucherStartDate.CssClass = "errorFld";
                    spanStyleVoucherEndDate = "";
                    txtVoucherEndDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                if (Convert.ToDateTime(txtLatestStatementStartDate.Text.Trim()) > (Convert.ToDateTime(txtLatestStatementEndDate.Text.Trim())))
                {
                    errMsglateststatementStartDate = GetLocalResourceObject("CompareDate.Text").ToString(); //"Start date should be less than end date";
                    spanStyleLatestStatementStartDate = "";
                    txtLatestStatementStartDate.CssClass = "errorFld";
                    spanStyleLatestStatementEndDate = "";
                    txtLatestStatementEndDate.CssClass = "errorFld";
                    bErrorFlag = false;
                }

                return bErrorFlag;
            }
    }
}

