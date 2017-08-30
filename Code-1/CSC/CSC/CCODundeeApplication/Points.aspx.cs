using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI.HtmlControls;
using CCODundeeApplication.ClubcardService;
using ClubcardOnline.PointsSummarySequencing;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.SmartVoucherServices;
using CCODundeeApplication.PreferenceServices; 
//using ClubcardOnline.Web.SmartVoucherServices;
//using ClubcardOnline.Web.PreferenceServices;


namespace CCODundeeApplication
{
    /// <summary>
    /// Points Section in Dundee screens, having following functionality:
    ///<para>1. Current Collection Period Points Section</para>
    ///<para>2. Earlier 2 Collection Period Points Summary / Details View Link</para>
    ///<para>3. Current collection period Summary and Transactions</para>
    ///<para>4. Earlier 2 Collection periods Summary and Transactions</para>
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>DateCreated: 29/07/2010</para>
    /// </para>
    /// </summary>
    /// </summary>
    public partial class Points : System.Web.UI.Page
    {
        private int offerID = 0;
        private bool isCurrentOffer = true;
        private long customerID;
        ClubcardServiceClient client = null;
        CustomerServiceClient custClient = null;
        DateTime strXmasCurrStartDate;
        DateTime strXmasCurrEndDate;
        DateTime strXmasNextStartDate;
        DateTime strXmasNextEndDate;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;
        string culture = ConfigurationManager.AppSettings["Culture"];
        Decimal VoucherCost = 0;
        Decimal miles = 0;
        PreferenceServiceClient preferenceserviceClient = null;
        SmartVoucherServicesClient svServiceCall = null;
        bool boolResult = false;
        string cardNumber = null;
        string dateFormat = ConfigurationManager.AppSettings["DateDisplayFormat"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
            }
            xmlCapability = new XmlDocument();
            dsCapability = new DataSet();

            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.Page_Load()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.Page_Load()");
                #endregion

                if (!IsPostBack && !string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        #region RoleCapabilityImplementation
                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
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
                            HtmlAnchor DeLinkAccount = (HtmlAnchor)Master.FindControl("DelinkAccounts");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
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
                            if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                            {
                                DeLinkAccount.Visible = true;
                            }
                            else
                            {
                                DeLinkAccount.Visible = false;
                                DeLinkAccount.HRef = "";
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
                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                            {
                                cutomerDetails.Disabled = false;
                            }
                            else
                            {
                                cutomerDetails.Disabled = true;
                                cutomerDetails.HRef = "";
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
                        #endregion

                        //******* Release 1.5 changes start *********//
                        SetHouseHoldStatus();
                        //******* Release 1.5 changes end *********//
                    }
                    IsPointSummaryVisible();
                      XmlDocument resulDoc = null;
                      string resultXml = string.Empty;
                      string errorXml = string.Empty;
                      DataSet dsMyAccountDetails = new DataSet();
                      ClubcardServiceClient serviceClient = new ClubcardServiceClient();
                      boolResult = serviceClient.GetMyAccountDetails(out errorXml, out resultXml, customerID, culture);
                            if (boolResult)
                            {
                                if (resultXml != "" && resultXml != "<NewDataSet />")
                                {
                                    resulDoc = new XmlDocument();
                                    resulDoc.LoadXml(resultXml);
                                    dsMyAccountDetails.ReadXml(new XmlNodeReader(resulDoc));
                                    
                                    //to assign the retieved values to the related fields.
                                    if (dsMyAccountDetails.Tables.Count != 0)
                                    {
                                        if (dsMyAccountDetails.Tables[0].Columns.Contains("ClubcardID") != false)
                                        {
                                            if (dsMyAccountDetails.Tables[0].Rows[0]["ClubcardID"].ToString() != "")
                                            {
                                                cardNumber = dsMyAccountDetails.Tables[0].Rows[0]["ClubcardID"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                    if (Request.QueryString["o"] != null)
                    {
                        if (Int32.TryParse(Request.QueryString["o"].ToString().Trim(), out offerID))
                            isCurrentOffer = false;
                    }

                    if (Int64.TryParse(Helper.GetTripleDESEncryptedCookieValue("CustomerID"), out customerID))
                    {
                        if (!isCurrentOffer)
                        {
                            if (Request.QueryString["v"] != null)
                            {

                                if (Request.QueryString["v"].ToUpper() == "SMRY")
                                {
                                    //Earlier Period Points Summary
                                    LoadPrevPrdSummaryAndTitle();
                                    LoadPrevPrdPointsSummarySection();
                                    SummarySection.Visible = true;
                                    PointsSummarySection.Visible = true;
                                }

                                else if (Request.QueryString["v"].ToUpper() == "DTL")
                                {
                                    //Earlier Period Transction Detail
                                    LoadTransactions();
                                    LoadTransacionsTitle();
                                    LoadTransactionTypes(ddlTransactionTypes);
                                    LoadCards(ddlCardNumbers);
                                    lnkGoBack.Visible = true;
                                    TransactionSection.Visible = true;
                                }
                                divChristmasSaverSummary.Visible = false;
                            }
                        }
                        else
                        {
                            //Current Offer
                            LoadCurPrdSummaryAndTitle();
                            LoadEarlierColPrdList();
                            LoadTransactions();
                            LoadTransactionTypes(ddlTransactionTypes);
                            LoadCards(ddlCardNumbers);
                            XmasClubMemberSummary();
                            SummarySection.Visible = true;
                            EarlierColPrdSection.Visible = true;
                            TransactionSection.Visible = true;
                        }
                    }
                    //info.AddMergedColumns(new int[] { 2, 3 }, "Transaction");
                    //CCMCA-4853 fixed localization
                    info.AddMergedColumns(new int[] { 2, 3 }, GetLocalResourceObject("lclgrdTransactionHeaderResource1").ToString());
                    #region
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                    {

                        NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.Page_Load - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        //string culture = ConfigurationManager.AppSettings["Culture"];

                        customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                        custClient = new CustomerServiceClient();
                        preferenceserviceClient = new PreferenceServiceClient();
                        CustomerPreference objPreference = new CustomerPreference();
                        objPreference = preferenceserviceClient.ViewCustomerPreference(customerID, PreferenceType.NULL, false);

                        if (objPreference != null && objPreference.Preference != null && objPreference.Preference.Count > 0)
                        {
                            // To load the Opted Preference
                            List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                            objPreferenceFilter = objPreference.Preference;
                            string PrefID = string.Empty;
                            List<string> PreferenceIds = new List<string>();
                            foreach (var pref in objPreferenceFilter)
                            {
                                if (pref.POptStatus == OptStatus.OPTED_IN)
                                {
                                    PrefID = pref.PreferenceID.ToString().Trim();
                                    PreferenceIds.Add(PrefID);
                                }
                            }

                            if (PreferenceIds.Contains(BusinessConstants.XMASSAVER.ToString()))    //Ecoupon
                            {
                               // ltrTotalRewardValueTitle.Text = "Total vouchers saved:";
                                ltrTotalRewardValueTitle.Text = GetLocalResourceObject("Totalvouchersaved").ToString();
                            }
                            else if ((PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString())) || PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString()))
                            {
                                ltrTotalRewardValueTitle.Text = "Equivalent BA Avios :";
                                //(BusinessConstants.PRIMIUM_BAMILES * vachVal)/(BusinessConstants.VOUCHER_PERMILE)
                                if (PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString()))
                                {
                                    miles = (BusinessConstants.PRIMIUM_BAMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                                    ltrTotalRewardValue.Text = Convert.ToString(miles);
                                    //symPound.Visible = false;
                                }
                                else
                                {
                                    miles = (BusinessConstants.STANDARD_BAMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                                    ltrTotalRewardValue.Text = Convert.ToString(miles);
                                    //symPound.Visible = false;
                                }
                            }
                            else if (PreferenceIds.Contains(BusinessConstants.VIRGIN.ToString()))
                            {
                                ltrTotalRewardValueTitle.Text = "Equivalent Virgin Flying Club Miles :";
                                miles = (BusinessConstants.VIRGIN_ATLANTIC * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                                ltrTotalRewardValue.Text = Convert.ToString(miles);
                               // symPound.Visible = false;

                            }
                            else if ((PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())) || PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString()))
                            {
                                ltrTotalRewardValueTitle.Text = "Equivalent Avios :";
                                if ((PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString())))
                                {
                                    miles = (BusinessConstants.PRIMIUM_AMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                                    ltrTotalRewardValue.Text = Convert.ToString(miles);
                                    //symPound.Visible = false;
                                }
                                else
                                {
                                    miles = (BusinessConstants.STANDARD_AMILES * VoucherCost) / Convert.ToDecimal(BusinessConstants.VOUCHER_PERMILE.ToString());
                                    ltrTotalRewardValue.Text = Convert.ToString(miles);
                                    //symPound.Visible = false;
                                }
                            }
                            else
                            {
                                ltrTotalRewardValueTitle.Text = GetLocalResourceObject("Vouchertospendnow").ToString();
                                //ltrTotalRewardValueTitle.Text = "Vouchers to spend now:";
                                SmartVoucherServiceCall();
                            }
                        }
                        else
                        {
                            ltrTotalRewardValueTitle.Text = GetLocalResourceObject("Vouchertospendnow").ToString();
                            //ltrTotalRewardValueTitle.Text = "Vouchers to spend now:";
                            SmartVoucherServiceCall();
                        }
                    }
                    #endregion
                }
                // initialize the filter from viewstate if available
                filter = ViewState["FilterArgs"] == null ? new Hashtable() : (Hashtable)ViewState["FilterArgs"];

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.Page_Load()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.Page_Load()");
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.Page_Load() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.Page_Load()");
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
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion
        #region SmartVoucherServiceCall

        private void SmartVoucherServiceCall()
        {

            svServiceCall = new SmartVoucherServicesClient();
            decimal totalRewardLeftOver = 0;
            GetRewardDtlsRsp response = null;
            DataSet dsVouchers = null;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Vouchers.Home.SmartVoucherServiceCall");
                NGCTrace.NGCTrace.TraceDebug("Start:Vouchers.Home.SmartVoucherServiceCall");

                response = new GetRewardDtlsRsp();
                dsVouchers = new DataSet();
                int totalRecords = 0;
                //getRewardSummary method is called from the SV service
                response = svServiceCall.GetRewardDtls(cardNumber);
                if (response != null)
                {
                    if (response.dsResponse != null)
                    {
                        dsVouchers = response.dsResponse;
                        if (dsVouchers.Tables != null && dsVouchers.Tables.Count != 0)
                        {
                            if (dsVouchers.Tables.Count > 0 && dsVouchers.Tables[0].Rows.Count > 0)
                            {
                                totalRecords = dsVouchers.Tables[0].Rows.Count;
                            }
                            if (totalRecords != 0)
                            {
                                for (int i = 0; i < totalRecords; i++)
                                {
                                    //totalRewardLeftOver = totalRewardLeftOver + Convert.ToDecimal(dsVouchers.Tables[0].Rows[i][6].ToString());
                                    totalRewardLeftOver = totalRewardLeftOver + Convert.ToDecimal((dsVouchers.Tables[0].Rows[i][6].ToString()));
                                }

                                //ltrTotalRewardValue.Text = String.Format("{0:C}", Convert.ToDouble(totalRewardLeftOver.ToString()));
                                //CCMCA-4853 fixed localization
                                ltrTotalRewardValue.Text = String.Format(new CultureInfo(culture), "{0:C}", Convert.ToDouble(totalRewardLeftOver.ToString()));
                            }
                            else
                            {
                                ltrTotalRewardValue.Text = "0.00";
                            }
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:Vouchers.Home.SmartVoucherServiceCall");
                NGCTrace.NGCTrace.TraceDebug("End:Vouchers.Home.SmartVoucherServiceCall");

            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Vouchers.Home.SmartVoucherServiceCall - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Vouchers.Home.SmartVoucherServiceCall - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Vouchers.Home.SmartVoucherServiceCall");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
            finally
            {
                if (svServiceCall != null)
                {
                    if (svServiceCall.State == CommunicationState.Faulted)
                    {
                        svServiceCall.Abort();
                    }
                    else if (svServiceCall.State != CommunicationState.Closed)
                    {
                        svServiceCall.Close();
                    }
                }
            }
        }
        #endregion

        #region Christmas Savers

        DataSet dsChristmasSaverSummary = null;
        DataSet dsXmasClubMember = null;
        DateTime startDate;
        DateTime endDate;

        #region IsXmasClubMember or Not
        /// <summary>
        /// To check the Primary customer is a Xmas club member or not
        /// </summary>
        private void XmasClubMemberSummary()
        {
            bool boolResult = false;

            try
            {
                dsXmasClubMember = new DataSet();
                string resultXml, errorXml;
                string pCulture = ConfigurationManager.AppSettings["Culture"].ToString();
                client = new ClubcardServiceClient();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.XmasClubMemberSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.XmasClubMemberSummary()");
                #endregion


                //To call the WCF service.
                boolResult = client.IsXmasClubMember(out errorXml, out resultXml, customerID, pCulture);
                if (boolResult)
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsXmasClubMember.ReadXml(new XmlNodeReader(resulDoc));
                    }

                    if (dsXmasClubMember.Tables.Count.ToString() == "1")
                    {
                        if (dsXmasClubMember.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            GetChristmasSaverSummary();
                        }
                        else
                        {
                            divChristmasSaverSummary.Visible = false;
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.XmasClubMemberSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.XmasClubMemberSummary()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.XmasClubMemberSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.XmasClubMemberSummary() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.XmasClubMemberSummary()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (client != null)
                {
                    if (client.State == CommunicationState.Faulted)
                    {
                        client.Abort();
                    }
                    else if (client.State != CommunicationState.Closed)
                    {
                        client.Close();
                    }
                }
            }
        }
        public void IsPointSummaryVisible()
        {
            string Culture = ConfigurationManager.AppSettings["Culture"];

            string resultXml1 = string.Empty;
            string errorXml1 = string.Empty;
            XmlDocument resulDoc1 = null;
            custClient = new CustomerServiceClient();
            int rowCount1 = 0;
            String com = "25";
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.IsPointSummaryVisible()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.IsPointSummaryVisible()");
            #endregion
            try
            {
                if (custClient.GetConfigDetails(out errorXml1, out resultXml1, out rowCount1, com, Culture))
                {
                    resulDoc1 = new XmlDocument();
                    resulDoc1.LoadXml(resultXml1);
                    DataSet dsConfigDetails = new DataSet();
                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc1));

                    if (dsConfigDetails.Tables.Count > 0)
                    {
                        if (dsConfigDetails.Tables.Contains("ActiveDateRangeConfig"))
                        {
                            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                            {
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HidePointsSummaryPage")
                                {
                                    hdnIsSummaryEnabled.Value = "false";
                                }

                            }
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.IsPointSummaryVisible()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.IsPointSummaryVisible()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.IsPointSummaryVisible() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.IsPointSummaryVisible() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.IsPointSummaryVisible()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
               if (custClient != null)
                {
                    if (custClient.State == CommunicationState.Faulted)
                    {
                        custClient.Abort();
                    }
                    else if (custClient.State != CommunicationState.Closed)
                    {
                        custClient.Close();
                    }
                }
            }
        }
        /// <summary>
        /// To Get Christmas Saver Summary Records
        /// </summary>
        private void GetChristmasSaverSummary()
        {
            bool boolResult = false;
            string Culture = ConfigurationManager.AppSettings["Culture"];
            IFormatProvider dtFormat = CultureInfo.GetCultureInfo(Culture).DateTimeFormat;

            string resultXml1 = string.Empty;
            string errorXml1 = string.Empty;
            XmlDocument resulDoc1 = null;

            try
            {
                custClient = new CustomerServiceClient();
                int rowCount1 = 0;
                String com = "7";
                if (custClient.GetConfigDetails(out errorXml1, out resultXml1, out rowCount1, com, Culture))
                {
                    resulDoc1 = new XmlDocument();
                    resulDoc1.LoadXml(resultXml1);
                    DataSet dsConfigDetails = new DataSet();
                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc1));

                    if (dsConfigDetails.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                        {
                            if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverCurrDates")
                            {
                                strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim(), new CultureInfo("en-GB")).ToShortDateString());
                                strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim(), new CultureInfo("en-GB")).ToShortDateString());

                            }
                            else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverNextDates")
                            {
                                strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim(), new CultureInfo("en-GB")).ToShortDateString());
                                strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim(), new CultureInfo("en-GB")).ToShortDateString());

                            }
                        }
                    }
                }

                //To check the start date and end date for Xmas saver period.
                if (DateTime.Now.Date < strXmasNextStartDate)
                {
                    startDate = strXmasCurrStartDate;
                    endDate = strXmasCurrEndDate;
                }
                else if (DateTime.Now.Date >= strXmasNextStartDate)
                {
                    startDate = strXmasNextStartDate;
                    endDate = strXmasNextEndDate;
                }

                dsChristmasSaverSummary = new DataSet();
                string resultXml, errorXml;
                Hashtable XmasSaverSummary = new Hashtable();
                int maxRows = 0;
                int rowCount;
                maxRows = 100;

                XmasSaverSummary["CustomerID"] = customerID;
                XmasSaverSummary["StartDate"] = startDate.ToString("yyyy-MM-dd'T'HH:mm:ss", new CultureInfo("en-GB"));
                XmasSaverSummary["EndDate"] = endDate.ToString("yyyy-MM-dd'T'HH:mm:ss", new CultureInfo("en-GB"));
                string searchXML = Helper.HashTableToXML(XmasSaverSummary, "XmasSaver");

                string pCulture = ConfigurationManager.AppSettings["Culture"].ToString();
                client = new ClubcardServiceClient();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.GetChristmasSaverSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.GetChristmasSaverSummary() Input Xml-" + searchXML);
                #endregion

                //To call the WCF service.
                boolResult = client.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, searchXML, maxRows, pCulture);
                if (boolResult)
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsChristmasSaverSummary.ReadXml(new XmlNodeReader(resulDoc));
                    }
                    if (dsChristmasSaverSummary.Tables.Count != 0)
                    {
                        if (dsChristmasSaverSummary.Tables[0].Rows.Count > 0)
                        {
                            rptChristmasSummary.DataSource = dsChristmasSaverSummary;
                            rptChristmasSummary.DataBind();
                        }
                    }
                    else
                    {
                        divChristmasSaverSummary.Visible = false;
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.GetChristmasSaverSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.GetChristmasSaverSummary()Input Xml-" + searchXML);
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.GetChristmasSaverSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.GetChristmasSaverSummary() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.GetChristmasSaverSummary()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (client != null)
                {
                    if (client.State == CommunicationState.Faulted)
                    {
                        client.Abort();
                    }
                    else if (client.State != CommunicationState.Closed)
                    {
                        client.Close();
                    }
                }

                if (custClient != null)
                {
                    if (custClient.State == CommunicationState.Faulted)
                    {
                        custClient.Abort();
                    }
                    else if (custClient.State != CommunicationState.Closed)
                    {
                        custClient.Close();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Current Points Summary
        /// <summary>
        /// Load current and previous two collectin periods, their total points and total voucher values
        /// <para>It also loads total points earned by customer in each of these collection periods.</para>
        /// </summary>
        private void LoadCurPrdSummaryAndTitle()
        {
            #region Local variables

            DataSet dsSmrySection = new DataSet();
            DateTime? colPrdStartDate = null, colPrdEndDate = null;
            //Ngc Change Date Configurable
            string culture = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
            System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);

            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadCurPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadCurPrdSummaryAndTitle()");
                #endregion

                dsSmrySection = LoadDataFromService_CurPrdPointsSummary();
                if (dsSmrySection != null && dsSmrySection.Tables.Count > 0)
                {
                    //scan through all data rows of dataset table
                    foreach (DataRow row in dsSmrySection.Tables[0].Rows)
                    {
                        if ((isCurrentOffer && row["OfferPeriod"].ToString().ToUpper() == "CURRENT") ||
                        (Convert.ToInt32(row["OfferID"].ToString()) == offerID))
                        {
                            //Load the Collection period page heading (start date - end date)
                            colPrdStartDate = DateTime.Parse(row["StartDateTime"].ToString());
                            colPrdEndDate = DateTime.Parse(row["EndDateTime"].ToString());

                            //Load summary section data
                            ltrTotalRewardPoints.Text = String.Format("{0:#,###}", Convert.ToInt32(row["PointsBalanceQty"].ToString()));
                            //ltrTotalRewardValue.Text = "&pound;" + row["Vouchers"].ToString();
                            //ltrTotalRewardValue.Text = "$" + row["Vouchers"].ToString();
                            //ltrTotalRewardValue.Text = String.Format("{0:C}", Convert.ToDouble(row["Vouchers"].ToString()));
                            //CCMCA-4853 fixed localization
                            ltrTotalRewardValue.Text = String.Format(new CultureInfo(culture), "{0:C}", Convert.ToDouble(row["Vouchers"].ToString()));
                            VoucherCost = Convert.ToDecimal(row["Vouchers"].ToString());
                        }
                    }

                    //set the string and datatime values initialized in data row loop, to the UI fields (literals controls)
                    #region Assign other literal values on page

                    if (colPrdStartDate.HasValue && colPrdEndDate.HasValue)
                       // ltrColPrd.Text = GetLocalResourceObject("ltrCollectionPeriod").ToString() + " (" + colPrdStartDate.Value.ToString("dd/MM/yy", new CultureInfo("en-GB")) + " - " +
                                                                //colPrdEndDate.Value.ToString("dd/MM/yy", new CultureInfo("en-GB")) + ")";
                        ltrColPrd.Text = GetLocalResourceObject("ltrCollectionPeriod").ToString() + " (" + colPrdStartDate.Value.ToString(dateFormat) + " - " + colPrdEndDate.Value.ToString(dateFormat) + ")";

                    if (ltrTotalRewardPoints.Text.Equals(""))
                        ltrTotalRewardPoints.Text = "0";

                    //if (ltrTotalRewardValue.Equals("&pound;"))
                    //    ltrTotalRewardValue.Text = "&pound;0";

                    if (ltrTotalRewardValue.Equals("$"))
                        ltrTotalRewardValue.Text = "$0";
                    #endregion

                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadCurPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadCurPrdSummaryAndTitle()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadCurPrdSummaryAndTitle() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadCurPrdSummaryAndTitle() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadCurPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        private void LoadEarlierColPrdList()
        {
            #region Local variables

            DataSet dsSmrySection = new DataSet();
            DataTable SmryEarierPrdsTableMod = null;

            #endregion

            try
            {
                dsSmrySection = LoadDataFromService_CurPrdPointsSummary();
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadEarlierColPrdList()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadEarlierColPrdList()");
                #endregion
                if (dsSmrySection != null && dsSmrySection.Tables.Count > 0)
                {
                    SmryEarierPrdsTableMod = dsSmrySection.Tables[0].Clone();
                    //scan through all data rows of dataset table
                    foreach (DataRow row in dsSmrySection.Tables[0].Rows)
                    {
                        //Added '|| row["OfferPeriod"].ToString().ToUpper() == "MAY 2010"' this condition to remove May 2010 row.
                        if (!(row["OfferPeriod"].ToString().ToUpper() == "CURRENT" || row["OfferPeriod"].ToString().ToUpper() == "MAY 2010"))
                            SmryEarierPrdsTableMod.ImportRow(row);
                    }

                    if (SmryEarierPrdsTableMod.Rows.Count > 1)
                    {
                        if (SmryEarierPrdsTableMod.Rows[1]["OfferID"] != null)
                        {
                            //Cookie implementation
                            Helper.SetTripleDESEncryptedCookie("PrevOfferID", SmryEarierPrdsTableMod.Rows[1]["OfferID"].ToString());
                        }
                    }

                    //Bind filtered data (imported rows) to repeater
                    rptEarlierColPrds.DataSource = SmryEarierPrdsTableMod;
                    rptEarlierColPrds.DataBind();
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadEarlierColPrdList()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadEarlierColPrdList()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadEarlierColPrdList() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadEarlierColPrdList() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadEarlierColPrdList()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        private DataSet LoadDataFromService_CurPrdPointsSummary()
        {

            #region Local variables
            string conditionalXml, resultXml, viewXml, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsSmrySection = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            ClubcardService.ClubcardServiceClient serviceClient = null;
            #endregion

            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadDataFromService_CurPrdPointsSummary()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadDataFromService_CurPrdPointsSummary()");
            #endregion

            //Check ViewState first if the dataset is present in ViewState
            //Initialize it from ViewState and bypass the NGC service call
            if (ViewState["dsSummaryEarierPeriodsSections"] != null)
            {
                dsSmrySection = ViewState["dsSmrySection"] as DataSet;
            }
            else
            {
                try
                {
                    //Initialize the service reference
                    //NGCServer.Service service = new NGCServer.Service(); NGC Service call replaced by ClubcardOnline service call
                    serviceClient = new ClubcardService.ClubcardServiceClient();
                    inputParams["CustomerID"] = customerID; //customer id from session
                    culture = ConfigurationManager.AppSettings["Culture"];

                    //Convert all input variables to xml
                    conditionalXml = Helper.HashTableToXML(inputParams, "PointsInfoCondition");

                    isSuccessful = serviceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

                    if (isSuccessful && string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrEmpty(resultXml))
                        {
                            //Load the result xml containing parameters into a data set
                            resulDoc.LoadXml(resultXml);
                            dsSmrySection.ReadXml(new XmlNodeReader(resulDoc));
                        }
                        if (dsSmrySection != null &&
                            dsSmrySection.Tables.Count > 0)
                        {
                            //Check whether the Data has PointsBalanceQty and Vouchers columns

                            DataTable dtPointsInfo = dsSmrySection.Tables[0];
                            //In case PointsBalanceQty and Vouchers columns not exisiting in the DataTable
                            //Adding these columns and setting the default values to zeros
                            if (!dtPointsInfo.Columns.Contains("PointsBalanceQty"))
                            {
                                DataColumn dcPointsBalanceQty = new DataColumn("PointsBalanceQty", typeof(string));
                                dcPointsBalanceQty.DefaultValue = "0";

                                DataColumn dcVouchers = new DataColumn("Vouchers", typeof(string));
                                dcVouchers.DefaultValue = "0.00";

                                dtPointsInfo.Columns.Add(dcPointsBalanceQty);
                                dtPointsInfo.Columns.Add(dcVouchers);
                            }
                            //In the columns are existing but the records does not have records
                            //then adding the zeros only to those records
                            else
                            {
                                foreach (DataRow row in dtPointsInfo.Rows)
                                {
                                    if (string.IsNullOrEmpty(row["PointsBalanceQty"].ToString()))
                                        row["PointsBalanceQty"] = "0";
                                    if (string.IsNullOrEmpty(row["Vouchers"].ToString()))
                                        row["Vouchers"] = "0.00";
                                    else
                                    {
                                        row["Vouchers"] = string.Format("{0:0.00}",
                                                          double.Parse(row["Vouchers"].ToString(),new CultureInfo("en-GB")));
                                    }
                                }
                            }

                            //If the loyaltyEntityServiceLayer function has return the valid dataset then
                            //Pass it to the FillPageValueswithDataSet() which fills the repeater
                            //and all the required UI variables
                            //FillummaryEarierPeriodsSections(dsSmrySection);
                            ViewState["dsSmrySection"] = dsSmrySection;//Save the dataset to view state for postback cycles
                        }
                    }
                }
                catch (Exception exp)
                {
                    #region Trace Error
                    NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.LoadDataFromService_CurPrdPointsSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                    NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.LoadDataFromService_CurPrdPointsSummary() - Error Message :" + exp.ToString());
                    NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.LoadDataFromService_CurPrdPointsSummary()");
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
            #region Trace End
            NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadDataFromService_CurPrdPointsSummary()");
            NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadDataFromService_CurPrdPointsSummary()");
            #endregion
            return dsSmrySection;
        }

        /// <summary>
        /// Checks each row being data bound
        /// <para>Sets (activates / deactivates) the View links for summary and detail pages for each collection period</para>
        /// <para>Activation and deactivation of links is done on total points=0 condition</para>
        /// </summary>
        /// <param name="sender">Repeater control</param>
        /// <param name="e">Repeater event args</param>
        protected void rptEarlierColPrds_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            #region Local Variables

            DataRowView currentRow = null;
            string offerId = string.Empty;
            string colPrdName = string.Empty;
            int totalPtsBalance = 0;
            HyperLink hplViewSummary, hplViewDetail;
            HtmlTableCell thSummary=new HtmlTableCell();
            HtmlTableCell tdSummary = new HtmlTableCell();
            HtmlTableCell tdASummary = new HtmlTableCell();
            HtmlTableCell tdFoot = new HtmlTableCell();

            #endregion

            if (e.Item.ItemType == ListItemType.Header)
            {
               thSummary = (HtmlTableCell)e.Item.FindControl("thSummary");
               if (!Convert.ToBoolean(hdnIsSummaryEnabled.Value))
               {
                    thSummary.Visible = false;
               }
            }
            if (e.Item.ItemType == ListItemType.Item)
            {
                tdSummary = (HtmlTableCell)e.Item.FindControl("tdSummary");
                
                if (!Convert.ToBoolean(hdnIsSummaryEnabled.Value))
                {
                 tdSummary.Visible = false;
               
                 
                 }
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                tdASummary = (HtmlTableCell)e.Item.FindControl("tdASummary");
                if (!Convert.ToBoolean(hdnIsSummaryEnabled.Value))
                {
                    
                    tdASummary.Visible = false;

                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                tdFoot = (HtmlTableCell)e.Item.FindControl("tdFoot");
                if (!Convert.ToBoolean(hdnIsSummaryEnabled.Value))
                {
                    tdFoot.Visible = false;
                }
            }
               
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.rptEarlierColPrds_ItemDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.rptEarlierColPrds_ItemDataBound()");
                #endregion
                //get current row being bound from repeater
                currentRow = ((DataRowView)e.Item.DataItem);
                if (currentRow != null)
                {
                    //get offerid, collection period name, total balance and initialize link button objects from the row.
                    offerId = currentRow["OfferID"].ToString();
                    colPrdName = currentRow["OfferPeriod"].ToString();
                    totalPtsBalance = Convert.ToInt32(currentRow["PointsBalanceQty"].ToString());
                    hplViewSummary = (HyperLink)e.Item.FindControl("lnkViewSummary");
                    hplViewDetail = (HyperLink)e.Item.FindControl("lnkViewDetail");

                    hplViewSummary.ID = "lnkViewSummary_" + offerId;
                    hplViewDetail.ID = "lnkViewDetail_" + offerId;

                 
                    if (offerId != "211")
                    {
                        hplViewSummary.NavigateUrl = "PointsSummary.aspx?o=" + offerId + "&v=smry";
                        hplViewDetail.NavigateUrl = "Points.aspx?o=" + offerId + "&v=dtl";
                    }
                    else
                    {
                        hplViewSummary.NavigateUrl = "Points.aspx?o=" + offerId + "&v=smry";
                        hplViewDetail.NavigateUrl = "Points.aspx?o=" + offerId + "&v=dtl";
                    }
                }
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.rptEarlierColPrds_ItemDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.rptEarlierColPrds_ItemDataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.rptEarlierColPrds_ItemDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.rptEarlierColPrds_ItemDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.rptEarlierColPrds_ItemDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        #endregion

        #region #Previous Points Summary

        #region Page Variables
        protected int offerId;
        protected DateTime offerStartDate;
        protected DateTime offerEndDate;
        private StatementTypes statementType;
        protected enum StatementTypes
        {
            Reward,
            AirmilesReward,
            XmasSavers,
            BAmilesReward,
            NonReward
        }
        #endregion

        private void LoadPrevPrdSummaryAndTitle()
        {
            #region Local variables
            DataSet dsPrvPrdPointsSummary;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadPrevPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadPrevPrdSummaryAndTitle()");
                #endregion

                dsPrvPrdPointsSummary = LoadDataFromService_PrvPrdPointsSummary();

                btnSeePointsDetail.HRef = "Points.aspx?o=" + offerID + "&v=dtl";
                if (dsPrvPrdPointsSummary.Tables.Count > 0)
                {
                    //set page variables Offer StartDate and EndDate and StatementType
                    DataRow drPtsSmr = dsPrvPrdPointsSummary.Tables[0].Rows[0];
                    DateTime.TryParse(drPtsSmr["StartDateTime"].ToString(), out offerStartDate);
                    DateTime.TryParse(drPtsSmr["EndDateTime"].ToString(), out offerEndDate);
                    statementType = (StatementTypes)Enum.Parse(typeof(StatementTypes), drPtsSmr["PointSummaryDescEnglish"].ToString());

                    //Setting the Page title
                    //Ngc Change Date Configurable
                    string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);
                    //ltrColPrd.Text = "Collection Period (" + offerStartDate.ToString("dd/MM/yy", cultures) + " - " + offerEndDate.ToString("dd/MM/yy", cultures) + ")";
                    ltrColPrd.Text = GetLocalResourceObject("ltrColPrdResource1.Text").ToString() + "(" + offerStartDate.ToString("dd/MM/yy", cultures) + " - " +
                                                                offerEndDate.ToString("dd/MM/yy", cultures) + ")";

                    //Setting the Summary section
                    switch (statementType)
                    {
                        case StatementTypes.Reward:
                        case StatementTypes.XmasSavers:
                        case StatementTypes.NonReward:
                            //Following field is same for Standard, Xmas and NonReward statement types
                            ltrTotalRewardPoints.Text = String.Format("{0:#,###}", Convert.ToInt32(drPtsSmr["TotalPoints"].ToString()));
                            //Comented to Get Country specific Currency symbol 
                            //ltrTotalRewardValue.Text = "$" + drPtsSmr["TotalReward"].ToString();
                            ltrTotalRewardValue.Text = String.Format("{0:C}", Convert.ToDouble(drPtsSmr["TotalReward"].ToString()));
                            VoucherCost = Convert.ToDecimal(drPtsSmr["TotalReward"].ToString());
                            break;
                        case StatementTypes.AirmilesReward:
                        case StatementTypes.BAmilesReward:
                            //Following field are same for AirMiles and BAMiles statement types
                            //Only the wording changes with AirMiles and BAMiles statement types
                            ltrTotalRewardPoints.Text = String.Format("{0:#,###}", Convert.ToInt32(drPtsSmr["TotalPoints"].ToString()));
                            if (statementType.ToString().Replace("Reward", "") == "BAmiles")
                            {
                                //ltrTotalRewardValueTitle.Text = "Number of BA Miles credited to your account:";
                                ltrTotalRewardValueTitle.Text = GetLocalResourceObject("CreditedBAMiles").ToString();
                            }
                            else
                            {
                                //ltrTotalRewardValueTitle.Text = "Number of " + statementType.ToString().Replace("Reward", "") + " credited to your account:";
                                ltrTotalRewardValueTitle.Text = GetLocalResourceObject("NoOf").ToString() + statementType.ToString().Replace("Reward", "") + GetLocalResourceObject("CredetedAcc").ToString();
                            }
                            ltrTotalRewardValue.Text = drPtsSmr["TotalRewardMiles"].ToString();
                            VoucherCost = Convert.ToDecimal(drPtsSmr["TotalReward"].ToString());
                            break;
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadPrevPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadPrevPrdSummaryAndTitle()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadPrevPrdSummaryAndTitle() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadPrevPrdSummaryAndTitle() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadPrevPrdSummaryAndTitle()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        private void LoadPrevPrdPointsSummarySection()
        {
            #region Local variables
            DataSet dsPrvPrdPointsSummary;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadPrevPrdPointsSummarySection()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadPrevPrdPointsSummarySection()");
                #endregion
                dsPrvPrdPointsSummary = LoadDataFromService_PrvPrdPointsSummary();
                if (dsPrvPrdPointsSummary.Tables.Count > 0)
                {
                    //set page variables Offer StartDate and EndDate and StatementType
                    DataRow drPtsSmr = dsPrvPrdPointsSummary.Tables[0].Rows[0];
                    statementType = (StatementTypes)Enum.Parse(typeof(StatementTypes), drPtsSmr["PointSummaryDescEnglish"].ToString());

                    //following functions loads the page elements with values from the dataset
                    LoadOtherPageFields(drPtsSmr);
                    LoadPointsBoxes(drPtsSmr, "TescoPoints", ref pnlTescoPointsTotals);
                    LoadPointsBoxes(drPtsSmr, "TescoBankPoints", ref pnlTescoBankPointsTotals);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadPrevPrdPointsSummarySection()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadPrevPrdPointsSummarySection()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadPrevPrdPointsSummarySection() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadPrevPrdPointsSummarySection() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadPrevPrdPointsSummarySection()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// Connect to the Clubcard service and fetch the dsPointsSummaryRec by calling GetPointsSummaryInfo()
        /// <para>Also check whether the dsPointsSummaryRec is available in ViewState</para>
        /// <para>Then call the FillPageValueswithDataSet() which will then fill out Points boxes</para>
        /// <para>and other fields on the page</para>
        /// </summary>
        private DataSet LoadDataFromService_PrvPrdPointsSummary()
        {
            #region Local variables
            string conditionalXml, resultXml, viewXml = string.Empty, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsPrvPrdPointsSummary = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            ClubcardService.ClubcardServiceClient serviceClient = null;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call
                if (ViewState["dsPrvPrdPointsSummary"] != null)
                {
                    dsPrvPrdPointsSummary = ViewState["dsPrvPrdPointsSummary"] as DataSet;
                }
                else
                {
                    //Initialize the service reference
                    serviceClient = new ClubcardService.ClubcardServiceClient();

                    inputParams["CustomerID"] = customerID; //selected customerID
                    inputParams["OfferID"] = offerID; //offer id for the previous collection period
                    culture = ConfigurationManager.AppSettings["Culture"];

                    //Convert all input variables to xml
                    conditionalXml = Helper.HashTableToXML(inputParams, "PointsSummaryCondition");

                    //call the service function GetPointsSummaryInfo() to get Points summary record
                    isSuccessful = serviceClient.GetPointsSummaryInfo(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

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
                            ViewState["dsPrvPrdPointsSummary"] = dsPrvPrdPointsSummary;
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadDataFromService_PrvPrdPointsSummary()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadDataFromService_PrvPrdPointsSummary()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadDataFromService_PrvPrdPointsSummary() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadDataFromService_PrvPrdPointsSummary()");
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

        /// <summary>
        /// Loads the following fields other than PointsBoxes with values in the dataset:             
        /// <para>-------------------------------------</para>                                        
        /// <para>Fields which other than Tesco and Tesco Bank section:</para>                        
        /// <para>ltrStatementTitle</para>                                                            
        /// <para>ltrStatementRange</para>                                                            
        /// <para>ltrTotalRewardLabel, this field is filled in LoadPrevPrdSummaryAndTitle()</para>    
        /// <para>ltrTotalReward, this field is filled in LoadPrevPrdSummaryAndTitle()</para>         
        /// <para>-------------------------------------</para>                                        
        /// <para>Fields in Tesco section:</para>                                                     
        /// <para>ltrTescoPoints</para>                                                               
        /// <para>ltrTescoBroughtForwardPoints</para>                                                 
        /// <para>ltrTescoPointsChangeFromRewards</para>                                              
        /// <para>ltrOfferEndDate1</para>                                                             
        /// <para>ltrTescoPointsTotal</para>                                                          
        /// <para>ltrTescoTotalRewardLabel</para>                                                     
        /// <para>ltrTescoTotalReward</para>                                                          
        /// <para>ltrTescoCarriedForwardPoints</para>                                                 
        /// <para>-------------------------------------</para>                                        
        /// <para>Fields in TescoBank section:</para>                                                 
        /// <para>ltrTescoBankPoints</para>                                                           
        /// <para>ltrTescoBankBroughtForwardPoints</para>                                             
        /// <para>ltrOfferEndDate2</para>                                                             
        /// <para>ltrTescoBankPointsTotal</para>                                                      
        /// <para>ltrTescoBankTotalRewardLabel</para>                                                 
        /// <para>ltrTescoBankTotalReward</para>                                                      
        /// <para>ltrTescoBankCarriedForwardPoints</para>                                             
        /// <param name="dsPrvPrdPointsSummary">DataSet having Points Summary information</param>     
        /// </summary>                                                                                

        private void LoadOtherPageFields(DataRow dsPrvPrdPointsSummary)
        {
            #region Local Variables
            int tescoPoints = 0, tescoBroughtForwardPoints = 0, tescoPointsChangeFromRewards = 0;
            int tescoBankPoints = 0, tescoBankBroughtForwardPoints = 0;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadOtherPageFields()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadOtherPageFields()");
                #endregion
                //#######################################################################
                //#    Set the fields which are in Tesco Section                        #
                //#######################################################################
                ltrTescoPoints.Text = dsPrvPrdPointsSummary["TescoPoints"].ToString();
                tescoPoints = Convert.ToInt32(dsPrvPrdPointsSummary["TescoPoints"].ToString());

                ltrTescoBroughtForwardPoints.Text = dsPrvPrdPointsSummary["TescoBroughtForwardPoints"].ToString();
                tescoBroughtForwardPoints = Convert.ToInt32(dsPrvPrdPointsSummary["TescoBroughtForwardPoints"].ToString());

                ltrTescoPointsChangeFromRewards.Text = dsPrvPrdPointsSummary["TescoPointsChangeFromRewards"].ToString();
                tescoPointsChangeFromRewards = Convert.ToInt32(dsPrvPrdPointsSummary["TescoPointsChangeFromRewards"].ToString());

                //Ngc Change Date Configurable
                string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
                System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);
                ltrOfferEndDate1.Text = offerEndDate.ToString("dd/MM/yy", cultures);

                ltrTescoPointsTotal.Text = (tescoPoints + tescoBroughtForwardPoints + tescoPointsChangeFromRewards).ToString();
                ltrTescoCarriedForwardPoints.Text = dsPrvPrdPointsSummary["TescoCarriedForwardPoints"].ToString();

                //#######################################################################
                //#    Set the fields which are in Tesco Bank Section                   #
                //#######################################################################
                ltrTescoBankPoints.Text = dsPrvPrdPointsSummary["TescoBankPoints"].ToString();
                tescoBankPoints = Convert.ToInt32(dsPrvPrdPointsSummary["TescoBankPoints"].ToString());

                ltrTescoBankBroughtForwardPoints.Text = dsPrvPrdPointsSummary["TescoBankBroughtForwardPoints"].ToString();
                tescoBankBroughtForwardPoints = Convert.ToInt32(dsPrvPrdPointsSummary["TescoBankBroughtForwardPoints"].ToString());

                ltrOfferEndDate2.Text = offerEndDate.ToString("dd/MM/yy", cultures);
                ltrTescoBankPointsTotal.Text = (tescoBankPoints + tescoBankBroughtForwardPoints).ToString();
                ltrTescoBankCarriedForwardPoints.Text = dsPrvPrdPointsSummary["TescoBankCarriedForwardPoints"].ToString();

                //##########################################################################################
                //#    Fields which changes with statement types (Common, in Tesco and TescoBank sections  #
                //##########################################################################################
                switch (statementType)
                {
                    case StatementTypes.Reward:
                    case StatementTypes.XmasSavers:
                    case StatementTypes.NonReward:
                        //Following fields are same for Standard, Xmas and NonReward statement types

                        //ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = "Clubcard Voucher Total";//TotVoucherCost
                        ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = GetLocalResourceObject("TotVoucherCost").ToString();
                        //ltrTescoTotalReward.Text = "$" + dsPrvPrdPointsSummary["TescoTotalReward"].ToString();
                        //ltrTescoBankTotalReward.Text = "$" + dsPrvPrdPointsSummary["TescoBankTotalReward"].ToString();
                        //NGC 36 Country specific Currency Symbol
                        ltrTescoTotalReward.Text = String.Format("{0:C}", Convert.ToDouble(dsPrvPrdPointsSummary["TescoTotalReward"].ToString()));
                        ltrTescoBankTotalReward.Text = String.Format("{0:C}", Convert.ToDouble(dsPrvPrdPointsSummary["TescoBankTotalReward"].ToString()));
                        break;
                    case StatementTypes.AirmilesReward:
                    case StatementTypes.BAmilesReward:
                        //Following fields are same for AirMiles and BAMiles statement types
                        //Only the wording changes with AirMiles and BAMiles statement types
                        if (statementType.ToString().Replace("Reward", "") == "BAmiles")
                        {
                            //ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = "Number of BA Miles credited to your account";//CreditedBAMiles
                            ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = GetLocalResourceObject("CreditedBAMiles").ToString();
                        }
                        else
                        {
                            //ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = "Number of " + statementType.ToString().Replace("Reward", "") + " credited to your account";//NoOf+credited to your account
                            ltrTescoTotalRewardLabel.Text = ltrTescoBankTotalRewardLabel.Text = GetLocalResourceObject("NoOf").ToString() + statementType.ToString().Replace("Reward", "") + GetLocalResourceObject("CredetedAcc").ToString();
                        }
                        ltrTescoTotalReward.Text = dsPrvPrdPointsSummary["TescoRewardMiles"].ToString();
                        ltrTescoBankTotalReward.Text = dsPrvPrdPointsSummary["TescoBankRewardMiles"].ToString();
                        break;
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadOtherPageFields()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadOtherPageFields()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadOtherPageFields() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadOtherPageFields() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadOtherPageFields()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// Loads the PointsBoxes (page elements) dynamically with the defined sequence and logo information
        /// <para>defied in the XML file for the given collection period</para>
        /// <para>the path for the XML file has </para>
        /// </summary>
        /// <param name="drPtsSmr">DataSet having Points Summary information</param>
        /// <param name="sectionType">Standard, NonReward, Xmas, AirMiles or BAMiles</param>
        /// <param name="pnlTarget">Tesco / TescoBank html panels in which the PointsBoxes will get rendered</param>
        private void LoadPointsBoxes(DataRow drPtsSmr, string sectionType, ref Panel pnlTarget)
        {
            #region Local Variables
            StatementFormat stformat;
            Statement selStatement;
            ArrayList pointsBoxes;
            short boxCounter = 0, totalRows, totalColumns;
            string rowClass = string.Empty, columnClass = string.Empty, spacer = string.Empty;
            double rowsd;
            bool isLastRow = false;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadPointsBoxes()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadPointsBoxes()");
                #endregion

                pnlTarget.AddLiteral("<div class=\"dataTable\">" +
                                                   "<table class=\"pointsTable\">" +
                                                        "<tbody>");
                //Load the appropriate statement format from the xml with respect to offer id
                stformat = XMLSerializer<StatementFormat>.
                        Load(ConfigurationManager.AppSettings["StatementFormatPath"] + offerID + ".xml");
                //Load the appropriate statement from statement format with respect to the statement types :
                //Standard, NonReward, Xmas, AirMiles or BAMiles
                selStatement = stformat[statementType.ToString()];

                //Load the PointsBoxes with respect to the section type provided
                //Tesco or TescoBank
                pointsBoxes = selStatement[sectionType];

                //initialize the local variables with rowcount and column count (html grid)
                rowsd = (double)pointsBoxes.Count / (double)3;
                totalRows = Convert.ToInt16(Math.Ceiling(rowsd));
                totalColumns = 3;

                //Row Loop row 1 to total rows (html grid)
                for (int row = 1; row <= totalRows; row++)
                {
                    if (row == totalRows)
                    {
                        rowClass = " class=\"last\"";
                        isLastRow = true;
                    }
                    //start the row tag
                    pnlTarget.AddLiteral("<tr" + rowClass + ">");

                    //Column Loop column 1 to 3 (html grid)
                    for (int column = 1; column <= totalColumns; column++)
                    {
                        //set the class name for every td
                        //if the row is last then the class name for every td changes
                        if (isLastRow)
                        {
                            if (column == totalColumns)
                                columnClass = " class=\"rowLast colLast\"";
                            else
                                columnClass = " class=\"rowLast\"";
                        }
                        else if (column == totalColumns)
                            columnClass = " class=\"colLast\"";
                        else
                            columnClass = "";

                        //start the td tag
                        pnlTarget.AddLiteral("<td" + columnClass + ">");

                        if (boxCounter < pointsBoxes.Count)
                        {
                            //get the appropriate PointsBox object from the Array
                            PointsBox box = pointsBoxes[boxCounter] as PointsBox;
                            pnlTarget.AddLiteral("<div class=\"pointsList\">");

                            //render the image if the Box has a logo name
                            if (!string.IsNullOrEmpty(box.BoxLogoFileName))
                            {
                                Image boxImage = new Image();
                                boxImage.ImageUrl = "~/I/" + box.BoxLogoFileName;
                                boxImage.AlternateText = box.BoxName;
                                pnlTarget.Controls.Add(boxImage);
                            }
                            pnlTarget.AddLiteral("<span id=\"sp" + sectionType + "_" + (boxCounter) + "\">");

                            //fetch the value from DataSet if PointsBox has DataColumn defined
                            if (!string.IsNullOrEmpty(box.DataColumnName))
                            {
                                //Check if column is not available in the table.
                                if (drPtsSmr.Table.Columns.Contains(box.DataColumnName))
                                {
                                    pnlTarget.AddLiteral(drPtsSmr[box.DataColumnName].ToString());
                                }
                            }

                            pnlTarget.AddLiteral("</span>");
                            pnlTarget.AddLiteral("</div>");
                        }
                        else
                            pnlTarget.AddLiteral("&nbsp;");
                        //end td tag
                        pnlTarget.AddLiteral("</td>");
                        boxCounter++;
                    }
                    //end column loop (html grid)
                    //end tr tag 
                    pnlTarget.AddLiteral("</tr>");
                }
                //end row loop (html grid)
                pnlTarget.AddLiteral("</table>" +
                               "</div>");

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadPointsBoxes()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadPointsBoxes()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.LoadPointsBoxes() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.LoadPointsBoxes() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.LoadPointsBoxes()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        #endregion

        #region Transactions

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

        private void LoadTransacionsTitle()
        {
            #region Local variables
            DataSet dsTransactions = new DataSet();
            DataTable tblOfferDetails;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadTransacionsTitle()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadTransacionsTitle()");
                #endregion
                dsTransactions = LoadDataFromService_Transactions();

                tblOfferDetails = dsTransactions.Tables["OfferDetails"];
                //Set the offer start date and end date on the view state so that these can be used on later time
                if (tblOfferDetails != null && tblOfferDetails.Rows.Count == 1)
                {
                    //Ngc Change Date Configurable
                    string culture = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);
                    //ltrColPrd.Text = "Collection Period (" + Convert.ToDateTime(tblOfferDetails.Rows[0]["StartDateTime"].ToString()).ToString(dateFormat,cultures) + " - " + Convert.ToDateTime(tblOfferDetails.Rows[0]["EndDateTime"].ToString()).ToString(dateFormat,cultures) + ")";
                    //CCMCA-4853 fixed loclization
                    ltrColPrd.Text = GetLocalResourceObject("ltrColPrdResource1.Text").ToString() + " (" + Convert.ToDateTime(tblOfferDetails.Rows[0]["StartDateTime"].ToString()).ToString(dateFormat, cultures) + " - " +
                                                                Convert.ToDateTime(tblOfferDetails.Rows[0]["EndDateTime"].ToString()).ToString(dateFormat, cultures) + ")";
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadTransacionsTitle()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadTransacionsTitle()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadTransacionsTitle() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadTransacionsTitle() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadTransacionsTitle()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        private void LoadTransactions()
        {
            #region Local variables
            DataSet dsTransactions = new DataSet();
            DataTable tblTransactions;
            #endregion


            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadTransactions()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadTransactions()");
                #endregion

                dsTransactions = LoadDataFromService_Transactions();

                if (dsTransactions != null &&
                    dsTransactions.Tables.Count > 0)
                {
                    tblTransactions = dsTransactions.Tables["Transactions"];

                    if (tblTransactions != null)
                    {
                        //if no rows found the set the appropriate UI message with ShowNoTransactionMessage()
                        if (tblTransactions.Rows.Count == 0)
                            ShowNoTransactionMessage();
                        else
                        {
                            grdTransactions.DataSource = tblTransactions;
                            grdTransactions.DataBind();
                        }
                        //data bind the gridview with the dataset
                    }
                    else
                        ShowNoTransactionMessage();
                }
                // if no tables were returned then show the appropriate UI message
                else
                    ShowNoTransactionMessage();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadTransactions()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadTransactions()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadTransactions() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadTransactions() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadTransactions()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        #region Private Functions
        /// <summary>
        /// It fetches the transactions from loyalty entity service layer via ngc web service call
        /// also formats the clubcard number in selected data with FormatCard()
        /// </summary>
        private DataSet LoadDataFromService_Transactions()
        {
            #region Local Variables
            string conditionalXml, resultXml = string.Empty, errorXml = string.Empty;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsTransactions = new DataSet();
            bool isServiceSuccessful = false;
            ClubcardService.ClubcardServiceClient serviceClient = null;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadDataFromService_Transactions()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadDataFromService_Transactions()");
                #endregion
                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call
                if (ViewState["dsTransactions"] != null)
                {
                    dsTransactions = ViewState["dsTransactions"] as DataSet;
                }
                else
                {
                    //Replaced the NGC service call with the new service
                    //NGCServer.Service service = new NGCServer.Service();
                    //objName = "Customer"; methodName = "GetTxnDetailsByCustomerAndOfferID";
                    serviceClient = new ClubcardService.ClubcardServiceClient();


                    //Set the input parameters for GetTxnDetailsByCustomerAndOfferID()
                    //and convert them to xml
                    inputParams["CustomerID"] = customerID;
                    inputParams["OfferID"] = offerID.ToString();
                    inputParams["ShowMerchantFlag"] = 1; //1 - To include the ' - MerchantName' in Transaction Description
                    conditionalXml = Helper.HashTableToXML(inputParams, "TransactionCondition");

                   
                    //Changes as per NGC 36 
                    isServiceSuccessful = serviceClient.GetTxnDetailsByHouseholdCustomerAndOfferID(out errorXml, out resultXml, out rowCount,
                                                                    conditionalXml, maxRowCount,
                                                                    ConfigurationManager.AppSettings["Culture"]);

                    if (isServiceSuccessful && string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrEmpty(resultXml))
                        {
                            //Load the result xml containing parameters into a data set if the xml is not empty
                            resulDoc.LoadXml(resultXml);
                            dsTransactions.ReadXml(new XmlNodeReader(resulDoc));
                        }
                        if (dsTransactions != null)
                        {
                            ViewState["dsTransactions"] = dsTransactions;
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadDataFromService_Transactions()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadDataFromService_Transactions()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadDataFromService_Transactions() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadDataFromService_Transactions() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadDataFromService_Transactions()");
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

            return dsTransactions;
        }

        /// <summary>
        /// show the appropriate UI messsage if there are no row in the transactions fetched 
        /// <para>or there was no table fetched in the result dataset</para>
        /// </summary>
        private void ShowNoTransactionMessage()
        {
            //if current offer is not selected alter the UI message 
            lblShow0PtsMsg.Visible = true;
        }

        /// <summary>
        /// It is used to load transaction types (perticularly stores in which customer has shopped) in ddl
        /// uses the same dataset which was fetched from the NGC service in GetTransaction()
        /// </summary>
        /// <param name="ddlTransactionTypes">ddl to which the transaction types are to be bound</param>
        private void LoadTransactionTypes(DropDownList ddlTransactionTypes)//, DataTable transactionTypes - removed
        {
            DataTable transactions = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadTransactionTypes()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadTransactionTypes()");
                #endregion
                // take the datatable from viewstate
                if (ViewState["dsTransactions"]!= null)
                {
                     transactions = (ViewState["dsTransactions"] as DataSet).Tables["Transactions"];
                }
                if (transactions != null)
                {
                    //get unique transaction types from the datatable fetched from the NGC service

                    DataTable uniqueTransactionTypes = transactions.DefaultView.ToTable(true, "TransactionDescription");

                    DataRow selectRow = uniqueTransactionTypes.NewRow();
                    //selectRow["TransactionDescription"] = "Select"; // add a 'select' row on the top of the table
                    //CCMCA-4853 fixed the localization
                    selectRow["TransactionDescription"] = GetLocalResourceObject("lclddlTransactionTypesResource1").ToString();
                    uniqueTransactionTypes.Rows.InsertAt(selectRow, 0);
                    ddlTransactionTypes.DataSource = uniqueTransactionTypes; // bind it to the dropdown list
                    ddlTransactionTypes.DataTextField = "TransactionDescription";
                    ddlTransactionTypes.DataValueField = "TransactionDescription";
                    ddlTransactionTypes.DataBind(); // bind it to the dropdown list
                }

                else
                    //ddlTransactionTypes.Items.Add(new ListItem("Select", "Select")); // else just keep select option in the ddl
                    //CCMCA-4853 fixed the localization
                    ddlTransactionTypes.Items.Add(new ListItem(GetLocalResourceObject("lclddlTransactionTypesResource1").ToString(), GetLocalResourceObject("lclddlTransactionTypesResource1").ToString()));

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadTransactionTypes()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadTransactionTypes()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadTransactionTypes() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadTransactionTypes() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadTransactionTypes()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }

        }

        /// <summary>
        /// It is used to load Clubcard information on the screen. 
        /// uses the same dataset which was fetched from the NGC service in GetTransaction()
        /// </summary>
        /// <param name="ddlTransactionTypes">Array of Clubcards</param>
        private void LoadCards(DropDownList ddlCardNumbers) //, DataTable clubcards - removed
        {
            DataTable transactions = null;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.LoadCards()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.LoadCards()");
                #endregion
                // take the datatable from viewstate
                if (ViewState["dsTransactions"] != null)
                {
                    transactions = (ViewState["dsTransactions"] as DataSet).Tables["Transactions"];
                }
                if (transactions != null)
                {
                    //get unique cards from the datatable fetched from the NGC service
                    DataTable uniqueCards = transactions.DefaultView.ToTable("UniqueCards", true, new string[] { "ClubcardID" }); ;
                    DataRow selectRow = uniqueCards.NewRow();
                    //selectRow["ClubcardID"] = "Select"; // add a 'select' row on the top of the table
                    //CCMCA-4853 fixed the localization
                    selectRow["ClubcardID"] = GetLocalResourceObject("lclddlCardNumbersResouce1").ToString();
                    uniqueCards.Rows.InsertAt(selectRow, 0);
                    ddlCardNumbers.DataSource = uniqueCards; // bind it to the dropdown list
                    ddlCardNumbers.DataTextField = "ClubcardID";
                    ddlCardNumbers.DataValueField = "ClubcardID";
                    ddlCardNumbers.DataBind(); // bind it to the dropdown list
                }
                else
                    //ddlCardNumbers.Items.Add(new ListItem("Select")); // else just keep select option in the ddl
                    //CCMCA-4853 fixed the localization
                    ddlCardNumbers.Items.Add(new ListItem(GetLocalResourceObject("lclddlCardNumbersResouce1").ToString()));

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.LoadCards()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.LoadCards()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.LoadCards() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.LoadCards() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.LoadCards()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
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
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.SortGridView()");
                #endregion
                // generate the data view and set the sort order to it
                DataTable dt = (ViewState["dsTransactions"] as DataSet).Tables["Transactions"];
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
                grdTransactions.DataSource = dv;
                grdTransactions.DataBind();
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.SortGridView()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.SortGridView()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC Points.SortGridView() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC Points.SortGridView() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC Points.SortGridView()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// Applies the filter criteria selected
        /// //data binds the gridview again with datatable with filter criteria applied
        /// </summary>
        private void ApplyGridFilter()
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.ApplyGridFilter()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.ApplyGridFilter()");
                #endregion
                string args = " ";
                int i = 0;
                //get the selcted filter criteria from filter keys
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
                if (ViewState["dsTransactions"] != null)
                {
                    //get datatable from viewstate
                    DataTable transactions = (ViewState["dsTransactions"] as DataSet).Tables["Transactions"];
                    if (transactions != null)
                    {
                        transactions.DefaultView.RowFilter = args;
                        grdTransactions.DataSource = transactions;
                        grdTransactions.DataBind(); // set the filtered datatable and bind it to gridview again
                    }
                }
                //Filter needs to be saved between postbacks
                ViewState.Add("FilterArgs", filter);

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.ApplyGridFilter()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.ApplyGridFilter()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ApplyGridFilter() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ApplyGridFilter() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ApplyGridFilter()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }
        #endregion

        #region Page Control Events

        /// <summary>
        /// It will be called each time a row is bind to the Grid.
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void grdTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Local Variables
            Literal ltrCardNo, ltrDatePtsAdded, ltrTimePtsAdded,
                    ltrTransactionDetails, ltrActualSpent, ltrTotalPoints,
                    ltrCardStatus, ltrCardCancelled;
            string showMoreInfoLink = string.Empty;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.grdTransactions_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.grdTransactions_RowDataBound()");
                #endregion
                //Format the dates and Clubcard numbers, RowDatabound event handdler
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // initialize the literal controls from the current row
                    //Ngc Change Date Configurable
                    string culture = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);

                    ltrCardStatus = (Literal)e.Row.Cells[0].FindControl("ltrCardStatus");
                    ltrCardCancelled = (Literal)e.Row.Cells[0].FindControl("ltrCardCancelled");

                    //ltrCardNo = (Literal)e.Row.Cells[1].FindControl("ltrCardNo");
                    ltrDatePtsAdded = (Literal)e.Row.Cells[2].FindControl("ltrdatePtsAdded");
                    ltrTimePtsAdded = (Literal)e.Row.Cells[3].FindControl("ltrTimePtsAdded");
                    //ltrTransactionDetails = (Literal)e.Row.Cells[4].FindControl("ltrTransactionDetails");
                    //ltrActualSpent = (Literal)e.Row.Cells[5].FindControl("ltrActualSpent");
                    //ltrTotalPoints = (Literal)e.Row.Cells[6].FindControl("ltrTotalPoints");

                    //set the date in required format (dd/mm/yyyy)
                    ltrDatePtsAdded.Text = (Convert.ToDateTime(ltrDatePtsAdded.Text)).ToString(dateFormat,cultures);
                    ltrTimePtsAdded.Text = (Convert.ToDateTime(ltrTimePtsAdded.Text)).ToString("HH:mm");


                    #region RoleCapabilityImplementation
                    XmlDocument xmlCapability = new XmlDocument();
                    DataSet dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables[0].Columns.Contains("viewtransactiondetails"))
                        {
                            LinkButton lnkShowInfo = (LinkButton)e.Row.Cells[6].FindControl("lnkShowInfo");
                            //Modal popup call generation
                            //get all values and generate javascript function parameters
                            showMoreInfoLink = "modalMoreInfoShow('" + lnkShowInfo.CommandArgument.ToString() + "," + offerID + "');return false;";
                            lnkShowInfo.OnClientClick = showMoreInfoLink;
                        }
                        else
                        {
                            LinkButton lnkShowInfo = (LinkButton)e.Row.Cells[6].FindControl("lnkShowInfo");
                            lnkShowInfo.Visible = false;
                            ltrTotalPoints = (Literal)e.Row.Cells[6].FindControl("ltrTotalPoints");
                            ltrTotalPoints.Visible = true;
                        }
                    }
                    #endregion


                    if (ltrCardCancelled.Text.ToUpper().Equals("CANCELLED/REPLACED"))
                        ltrCardStatus.Text = "C";
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.grdTransactions_RowDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.grdTransactions_RowDataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdTransactions_RowDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdTransactions_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdTransactions_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// Sort Grid on ClubcardNumber,Transaction Date, and Description, with the help of view state
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">EventArgs</param>
        protected void grdTransactions_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.grdTransactions_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.grdTransactions_Sorting()");
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.grdTransactions_Sorting()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.grdTransactions_Sorting()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdTransactions_Sorting() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdTransactions_Sorting() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdTransactions_Sorting()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// It is used to load transactions when the user filters the transactions by Clubcard numbers
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlCardNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.ddlCardNumbers_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.ddlCardNumbers_SelectedIndexChanged()");
                #endregion
                DropDownList ddlCardNumbers = (DropDownList)sender;

                //add the filter criteria if clubcard number is selected,
                //else if 'select' is selected then remove the existing clubcard number filter criteria
                if (ddlCardNumbers.SelectedItem.Text != "Select")
                {
                    if (filter.ContainsKey("ClubcardID"))
                    {
                        filter["ClubcardID"] = "='" + ddlCardNumbers.SelectedItem.Text + "'";
                    }
                    else
                    {
                        filter.Add("ClubcardID", "='" + ddlCardNumbers.SelectedItem.Text + "'");
                    }
                }
                else
                {
                    filter.Remove("ClubcardID");
                }
                //filter the gridview with selected filter criteria
                ApplyGridFilter();

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.ddlCardNumbers_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.ddlCardNumbers_SelectedIndexChanged()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ddlCardNumbers_SelectedIndexChanged() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ddlCardNumbers_SelectedIndexChanged() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ddlCardNumbers_SelectedIndexChanged()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// It is used to load transactions when the user filters the transactions by transaction types
        /// <para>PointIssuePartnerGroupDesc is referred as Transaction Types</para>
        /// <para>PointIssuePartnerGroupDesc is grouping of Tesco Stores and Partners</para>
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">EventArgs</param>
        protected void ddlTransactionTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.ddlTransactionTypes_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.ddlTransactionTypes_SelectedIndexChanged()");
                #endregion
                DropDownList ddlTransactionTypes = (DropDownList)sender;
                //add the filter criteria if transaction type is selected,
                //else if 'select' is selected then remove the existing transaction type filter criteria
                if (ddlTransactionTypes.SelectedItem.Text != "Select")
                {
                    if (filter.ContainsKey("TransactionDescription"))
                    {
                        filter["TransactionDescription"] = "='" + ddlTransactionTypes.SelectedItem.Value + "'";
                    }
                    else
                    {
                        filter.Add("TransactionDescription", "='" + ddlTransactionTypes.SelectedItem.Value + "'");
                    }
                }
                else
                {
                    filter.Remove("TransactionDescription");
                }
                //filter the gridview with selected filter criteria
                ApplyGridFilter();
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.ddlTransactionTypes_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.ddlTransactionTypes_SelectedIndexChanged()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.ddlTransactionTypes_SelectedIndexChanged() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.ddlTransactionTypes_SelectedIndexChanged() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.ddlTransactionTypes_SelectedIndexChanged()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// This event is fired when Clear selection button is clicked
        /// It clears down the dropdown boxes and clears the filters for gridviews
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnClearSelection_Click(object sender, ImageClickEventArgs e)
        {
            //clear the dropdown selecttions
            ddlCardNumbers.SelectedIndex = 0;
            ddlTransactionTypes.SelectedIndex = 0;

            //clear the filters if any
            filter.Clear();

            //databind gridview again with no filters
            ApplyGridFilter();
        }
        protected void grdTransactions_DataBound(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.grdTransactions_DataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.grdTransactions_DataBound()");
                #endregion
                GridView grid = sender as GridView;

                if (grid != null)
                {
                    GridViewRow row = new GridViewRow(0, -1,
                        DataControlRowType.Header, DataControlRowState.Normal);

                    TableCell left = new TableHeaderCell();
                    left.ColumnSpan = 2;
                    left.CssClass = "GridHeaderLeft";
                    row.Cells.Add(left);

                    TableCell middle = new TableHeaderCell();
                    middle.ColumnSpan = 2;
                    middle.Text = "Transaction";
                    middle.CssClass = "GridHeader";
                    row.Cells.Add(middle);

                    TableCell right = new TableHeaderCell();
                    right.ColumnSpan = 3;
                    right.CssClass = "GridHeaderRightLast";
                    row.Cells.Add(right);

                    Table t = grid.Controls[0] as Table;
                    if (t != null)
                    {
                        t.Rows.AddAt(0, row);
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.grdTransactions_DataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.grdTransactions_DataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.grdTransactions_DataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.grdTransactions_DataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.grdTransactions_DataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        [Serializable]
        private class MergedColumnsInfo
        {
            // indexes of merged columns
            public List<int> MergedColumns = new List<int>();
            // key-value pairs: key = first column index, value = number of merged columns
            public Hashtable StartColumns = new Hashtable();
            // key-value pairs: key = first column index, value = common title of merged columns 
            public Hashtable Titles = new Hashtable();

            //parameters: merged columns's indexes, common title of merged columns 
            public void AddMergedColumns(int[] columnsIndexes, string title)
            {
                MergedColumns.AddRange(columnsIndexes);
                StartColumns.Add(columnsIndexes[0], columnsIndexes.Length);
                Titles.Add(columnsIndexes[0], title);
            }
        }

        //property for storing of information about merged columns
        private MergedColumnsInfo info
        {
            get
            {
                if (ViewState["info"] == null)
                    ViewState["info"] = new MergedColumnsInfo();
                return (MergedColumnsInfo)ViewState["info"];

            }
        }

        //method for rendering of columns's headers	
        private void RenderHeader(HtmlTextWriter output, Control container)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC Points.RenderHeader()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC Points.RenderHeader()");
                #endregion
                for (int i = 0; i < container.Controls.Count; i++)
                {
                    TableCell cell = (TableCell)container.Controls[i];
                    //stretch non merged columns for two rows
                    if (!info.MergedColumns.Contains(i))
                    {
                        cell.Attributes["rowspan"] = "2";
                        cell.RenderControl(output);
                    }
                    else //render merged columns's common title
                        if (info.StartColumns.Contains(i))
                        {
                            output.Write(string.Format("<th align='center' class=\"GridHeader\" colspan='{0}'>{1}</th>",
                                     info.StartColumns[i], info.Titles[i]));
                        }
                }

                //close first row	
                output.RenderEndTag();
                //set attributes for second row
                grdTransactions.HeaderStyle.AddAttributesToRender(output);
                //start second row
                output.RenderBeginTag("tr");

                //render second row (only merged columns)
                for (int i = 0; i < info.MergedColumns.Count; i++)
                {
                    TableCell cell = (TableCell)container.Controls[info.MergedColumns[i]];
                    cell.RenderControl(output);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC Points.RenderHeader()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC Points.RenderHeader()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC FindGroup.RenderHeader() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC FindGroup.RenderHeader() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC FindGroup.RenderHeader()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        protected void grdTransaction_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //call the method for custom rendering of columns's headers	
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.SetRenderMethodDelegate(RenderHeader);

        }

        #endregion


        #endregion

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
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
            finally
            {

            }
        }

        #endregion
    }
}
