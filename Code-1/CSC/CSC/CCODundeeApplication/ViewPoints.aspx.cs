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
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using CCODundeeApplication.ClubcardService;
using CCODundeeApplication.CustomerService;

namespace CCODundeeApplication
{
    public partial class ViewPoints : System.Web.UI.Page
    {

        protected CustomerServiceClient customerClient = null;
        Hashtable htCustomer = null;
        string culture = string.Empty;
        string domain = string.Empty;
        DataSet dsReasons = null;
        DataSet dsResult = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            XmlDocument xmlCapability = new XmlDocument();
            DataSet dsCapability = new DataSet();
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
            {
                #region RoleCapabilityImplementation

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
                    HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                    HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                    HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                    HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                    HtmlAnchor DeLinkAccount = (HtmlAnchor)Master.FindControl("DelinkAccounts");
                    Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                    HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");

                    if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                    {
                        customerCoupon.Disabled = false;
                    }
                    else
                    {
                        customerCoupon.Disabled = true;
                        customerCoupon.HRef = "";
                    }
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
                    if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                    {
                        DeLinkAccount.Visible = true;
                    }
                    else
                    {
                        DeLinkAccount.Visible = false;
                        DeLinkAccount.HRef = "";
                    }

                    if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                    {
                        cutomerDetails.Disabled = false;
                    }
                    else
                    {
                        cutomerDetails.Disabled = true;
                        cutomerDetails.HRef = "";
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
                string CustUseStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerUseStatus").ToString();
                if (CustUseStatus == "13" || CustUseStatus == "12")
                {
                    divVirtualCardMsg.Style.Value = "display: block";
                    btnAddPoints.Enabled = false;
                }
                #endregion
            }
            if (!IsPostBack)
            {

                LoadDataFromService_PrdPointsSummary();
                GetPointBalance();
                MergeGrid();

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


        public void GetPointBalance()
        {
            string addresses = string.Empty;
            string addressDetails = string.Empty;
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = null;
            try
            {
                customerClient = new CustomerServiceClient();
                int rowCount = 0;
                int maxCount = 0;
                htCustomer = new Hashtable();
                //htCustomer["CustomerID"] = 725093;
                htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString();
                string objectXml = Helper.HashTableToXML(htCustomer, "customer");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ViewPoints.GetPointBalance()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ViewPoints.GetPointBalance() input Xml-" + objectXml);
                #endregion
                if (customerClient.GetPointsBalance(out errorXml, out resultXml, out rowCount, objectXml, maxCount, "en-GB"))
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

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ViewPoints.GetPointBalance()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ViewPoints.GetPointBalance() input Xml-" + objectXml);
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ViewPoints.ViewStores() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ViewPoints.ViewStores() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ViewPoints.ViewStores()");
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

        public void MergeGrid()
        {
            for (int i = 0; i < grdPointsSummary.Rows.Count; i++)
            {

                if (i == 1 || i == 3 || i == 4 || i == 5 || i == 6)
                {
                    grdPointsSummary.Rows[i].Cells[1].ColumnSpan = 8;
                    grdPointsSummary.Rows[i].Cells[1].BackColor = System.Drawing.Color.Gray;
                    grdPointsSummary.Rows[i].Cells[1].Text = null;
                    grdPointsSummary.Rows[i].Cells[2].Visible = false;
                    grdPointsSummary.Rows[i].Cells[3].Visible = false;
                    grdPointsSummary.Rows[i].Cells[4].Visible = false;
                    grdPointsSummary.Rows[i].Cells[5].Visible = false;
                    grdPointsSummary.Rows[i].Cells[6].Visible = false;
                    grdPointsSummary.Rows[i].Cells[7].Visible = false;
                    grdPointsSummary.Rows[i].Cells[8].Visible = false;
                    grdPointsSummary.Rows[i].BorderColor = System.Drawing.Color.Gray;

                }
            }
        }

        private DataSet LoadDataFromService_PrdPointsSummary()
        {
            #region Local variables
            string conditionalXml, resultXml, viewXml = string.Empty, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsPrvPrdPointsSummary = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            CustomerService.CustomerServiceClient serviceClient = null;
            #endregion

            try
            {
                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call

                //Initialize the service reference
                serviceClient = new CustomerService.CustomerServiceClient();

                inputParams["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString(); //498559; 

                inputParams["ExpiryBatchDate"] = ConfigurationManager.AppSettings["ExpiryBatchDate"];
                culture = ConfigurationManager.AppSettings["Culture"];

                //Convert all input variables to xml
                conditionalXml = Helper.HashTableToXML(inputParams, "PointSummary");
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ViewPoints.LoadDataFromService_PrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ViewPoints.LoadDataFromService_PrdPointsSummary() input Xml-" + conditionalXml);
                #endregion
                //call the service function GetPointsSummaryInfo() to get Points summary record
                isSuccessful = serviceClient.GetPointsSummary(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

                //If service is successful load the xml into the dsPointsSummaryRec dataset
                if (isSuccessful && string.IsNullOrEmpty(errorXml))
                {
                    if (!string.IsNullOrEmpty(resultXml))
                    {
                        //Load the result xml containing parameters into a data set
                        resulDoc.LoadXml(resultXml);
                        dsPrvPrdPointsSummary.ReadXml(new XmlNodeReader(resulDoc));
                    }
                    if (dsPrvPrdPointsSummary != null &&
                        dsPrvPrdPointsSummary.Tables.Count > 0)
                    {
                        //Save the dataset to view state for postback cycles
                        grdPointsSummary.DataSource = dsPrvPrdPointsSummary.Tables[0].DefaultView;
                        grdPointsSummary.DataBind();
                        //Code Modified for Localization.
                        ltrColPrd.Text = ltrColPrd.Text = GetLocalResourceObject("UniqueConsMsg1.Text").ToString() + dsPrvPrdPointsSummary.Tables[0].Rows[2][0].ToString().Split(':')[1].ToString();
                        //"Collection Period : " + dsPrvPrdPointsSummary.Tables[0].Rows[2][0].ToString().Split(':')[1].ToString();


                        if (dsPrvPrdPointsSummary.Tables.Count > 1)
                        {
                            GridView1.DataSource = dsPrvPrdPointsSummary.Tables[1].DefaultView;
                            GridView1.DataBind();
                        }


                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ViewPoints.LoadDataFromService_PrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ViewPoints.LoadDataFromService_PrdPointsSummary() input Xml-" + conditionalXml);
                #endregion
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ViewPoints.ViewStores() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ViewPoints.ViewStores() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ViewPoints.ViewStores()");
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
            return dsPrvPrdPointsSummary;
        }

        #region SetHouseHoldStatus
        /// <summary>
        /// To set household status on LHN
        /// </summary>
        /// <param name="pCustomerID">Primary customer ID</param>
        public void SetHouseHoldStatus()
        {
            string customerUserStatus = string.Empty;
            string customerMailStatus = string.Empty;
            string CustomerEmailStatus = string.Empty;
            string CustomerMobilePhoneStatus = string.Empty;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Preferences.SetHouseHoldStatus() Clubcard CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                #endregion

                customerUserStatus = Helper.GetTripleDESEncryptedCookieValue("customerUserStatus");
                customerMailStatus = Helper.GetTripleDESEncryptedCookieValue("customerMailStatus");
                CustomerEmailStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerEmailStatus");
                CustomerMobilePhoneStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerMobilePhoneStatus");

                ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");

                HtmlControl spnBannedError = (HtmlControl)leftNav.FindControl("spnBannedError");
                HtmlControl spnLeftError = (HtmlControl)leftNav.FindControl("spnLeftError");
                HtmlControl spnDuplicateError = (HtmlControl)leftNav.FindControl("spnDuplicateError");
                HtmlControl spnAddressError = (HtmlControl)leftNav.FindControl("spnAddressError");
                HtmlControl spnEmailError = (HtmlControl)leftNav.FindControl("spnEmailError");
                HtmlControl spnMobileNoError = (HtmlControl)leftNav.FindControl("spnMobileNoError");

                if (customerUserStatus != "1" || customerMailStatus != "1")
                {
                    // for banned house hold
                    if (customerUserStatus == "2")
                    {
                        spnBannedError.Visible = true;
                    }
                    // for Left Scheme
                    else if (customerUserStatus == "3")
                    {
                        spnLeftError.Visible = true;
                    }
                    //for duplicate
                    else if (customerUserStatus == "5")
                    {
                        spnDuplicateError.Visible = true;
                    }
                    else
                    {
                        //for address in error
                        if (customerMailStatus == "3")
                        {
                            spnAddressError.Visible = true;
                        }
                        if (CustomerEmailStatus == "8")
                        {
                            spnEmailError.Visible = true;
                        }
                        if (CustomerMobilePhoneStatus == "8")
                        {
                            spnMobileNoError.Visible = true;
                        }
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Preferences.SetHouseHoldStatus() Clubcard CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Preferences.SetHouseHoldStatus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Preferences.SetHouseHoldStatus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Preferences.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {

            }
        }
        #endregion

        protected void btnAddPoints_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AddPoints.aspx", false);
        }
    }
}
