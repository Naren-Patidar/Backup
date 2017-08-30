using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using CCODundeeApplication.ClubcardService;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.SmartVoucherServices;
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace CCODundeeApplication
{
    /// <summary>
    /// Description: To display the voucher saved so far and amount topped up details
    /// Author: Sadanand Vama
    /// Date: 20 Sept 2010
    /// </summary>
    public partial class ChristmasSavers : System.Web.UI.Page
    {
        //Local variables.
        DateTime strXmasCurrStartDate;
        DateTime strXmasCurrEndDate;
        DateTime strXmasNextStartDate;
        DateTime strXmasNextEndDate;
        ClubcardServiceClient serviceClient = null;
        string culture = ConfigurationManager.AppSettings["Culture"];
        decimal sumVoucherSavedSoFar = 0;
        decimal sumTtlToppedUpMoney = 0;
        DateTime startDate;
        DateTime endDate;
        StringBuilder strBldHTML = new StringBuilder();
        SmartVoucherServicesClient svServiceCall = null;
        CustomerServiceClient custClient = null;
        bool boolResult;
        string resultXml, errorXml;
        XmlDocument resulDoc = null;
        DataSet dsMyAccountDetails = null;
        string cardNumber = string.Empty;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;
        long customerID = 0;
        //CCMCA: 407: Modify by Laxmi
        string TopupRange = ConfigurationManager.AppSettings["TopupRange"];
        string BonusVooucher = ConfigurationManager.AppSettings["BonusVoucher"];
        string Topuptoreceivemaxbonus = ConfigurationManager.AppSettings["Topuptoreceivemaxbonus"];
        string MaxBonusVoucher = ConfigurationManager.AppSettings["MaxBonusVoucher"];
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack && (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID"))))
                {
                    customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                    #region RoleCapabilityImplementation

                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                        Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");

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
                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
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
                            if (dsCapability.Tables[0].Columns.Contains("CreateNewCustomer") != false)
                            {
                                Join.Disabled = false;
                            }
                            else
                            {
                                Join.Disabled = true;
                                Join.HRef = "";
                            }
                            //if (dsCapability.Tables[0].Columns.Contains("ViewDataConfiguaration") != false)
                            //{
                            //    link.Visible = true;
                            //}
                            //else
                            //{
                            //    link.Visible = false;
                            //}
                           
                        }
                    }
                    #endregion
                 

                    //******* Release 1.5 changes start *********//
                    SetHouseHoldStatus();
                    //******* Release 1.5 changes end *********//

                    custClient = new CustomerServiceClient();
                    int rowCount = 0;
                    string com = "7";
                    if (custClient.GetConfigDetails(out errorXml, out resultXml, out rowCount,com, culture))
                    {
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        DataSet dsConfigDetails = new DataSet();
                        dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));

                        if (dsConfigDetails.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                            {
                                if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverCurrDates")
                                {
                                    strXmasCurrStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim()).ToShortDateString());
                                    strXmasCurrEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim()).ToShortDateString());

                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "7" && dr["ConfigurationName"].ToString().Trim() == "XmasSaverNextDates")
                                {
                                    strXmasNextStartDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue1"].ToString().Trim()).ToShortDateString());
                                    strXmasNextEndDate = Convert.ToDateTime(Convert.ToDateTime(dr["ConfigurationValue2"].ToString().Trim()).ToShortDateString());

                                }
                            }
                        }
                    }
                    resulDoc = null;
                    resultXml = string.Empty;
                    errorXml = string.Empty;
                    serviceClient = new ClubcardServiceClient();

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.Page_Load() CustomerID-" + customerID);
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.Page_Load() CustomerID-" + customerID);
                    #endregion

                    //To call the WCF service.
                    boolResult = serviceClient.GetMyAccountDetails(out errorXml, out resultXml, customerID, culture);

                    if (boolResult)
                    {
                        if (resultXml != "" && resultXml != "<NewDataSet />")
                        {
                            dsMyAccountDetails = new DataSet();
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            dsMyAccountDetails.ReadXml(new XmlNodeReader(resulDoc));
                        }
                    }

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

                    //To check the start date and end date for Xmas saver period.
                    if (DateTime.Now.Date < strXmasNextStartDate)
                    {
                        startDate = strXmasCurrStartDate;
                        endDate = strXmasCurrEndDate;

                        spnYear1.InnerHtml = (DateTime.Now.Year).ToString();
                        spnYear2.InnerHtml = (DateTime.Now.Year).ToString();
                    }
                    else if (DateTime.Now.Date >= strXmasNextStartDate)
                    {
                        startDate = strXmasNextStartDate;
                        endDate = strXmasNextEndDate;

                        spnYear1.InnerHtml = (DateTime.Now.Year + 1).ToString();
                        spnYear2.InnerHtml = (DateTime.Now.Year + 1).ToString();
                    }

                    LoadToppedUpMoney();

                    //Display the voucher detail for collection period.
                    DisplayCollectionPeriodDataFromSV();

                    CalculateBonus();

                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.Page_Load() CustomerID-" + customerID);
                    NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.Page_Load() CustomerID-" + customerID);
                    #endregion
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.Page_Load() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.Page_Load()");
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

        /// <summary>
        /// This method will display all the Xmas transactions for the perticular clubcard.
        /// </summary>
        protected void LoadToppedUpMoney()
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                #endregion

                DataSet dsChristmasSaverSummary = new DataSet();
                string resultXml, errorXml;

                int maxRows = 0;
                int rowCount;
                Hashtable XmasSaverSummary = new Hashtable();

                XmasSaverSummary["CustomerID"] = customerID;
                XmasSaverSummary["StartDate"] = startDate;
                XmasSaverSummary["EndDate"] = endDate;
                string searchXML = Helper.HashTableToXML(XmasSaverSummary, "XmasSaver");
                serviceClient = new ClubcardServiceClient();

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                #endregion

                //To call the service.
                if (serviceClient.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, searchXML, maxRows, culture))
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsChristmasSaverSummary.ReadXml(new XmlNodeReader(resulDoc));
                        if (dsChristmasSaverSummary.Tables[0].Rows.Count > 0)
                        {
                            rptXmasDetails.DataSource = dsChristmasSaverSummary;
                            rptXmasDetails.DataBind();
                        }
                    }
                    else
                    {
                        dvMoneyToppedUp.Visible = false;
                    }
                }
                else
                {
                    throw new Exception("GetChristmasSaverSummary Service failed Search XML:" + searchXML + " Error XML" + errorXml);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.LoadToppedUpMoney() CustomerID-" + customerID);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.GetClubCards() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.GetClubCards() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.GetClubCards()");
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
        /// This method calculates the sum of amount topped up field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptXmasDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.rptXmasDetails_ItemDataBound()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.rptXmasDetails_ItemDataBound()");
                #endregion

                Literal lblTransDate = (Literal)e.Item.FindControl("lblTransDate");
                Label lblAmount = (Label)e.Item.FindControl("lblAmount");

                if (e.Item.ItemType == ListItemType.Header)
                {
                    sumTtlToppedUpMoney = 0;
                }
                else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //Sum of the topped up money
                    sumTtlToppedUpMoney += Convert.ToDecimal(((DataRowView)e.Item.DataItem)["AmountSpent"]);
                    //Converting date into dd/mm/yy format
                    lblTransDate.Text = Convert.ToDateTime(((DataRowView)e.Item.DataItem)["transactionDateTime"]).ToString("dd/MM/yy");
                    //converting amount in currency format(£)
                    lblAmount.Text = String.Format("{0:C}", Convert.ToDecimal(((DataRowView)e.Item.DataItem)["AmountSpent"]));
                }
                else if (e.Item.ItemType == ListItemType.Footer)
                {
                    Literal type = (Literal)e.Item.FindControl("LiteralTotal");
                    type.Text = String.Format("{0:C}", sumTtlToppedUpMoney);
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.rptXmasDetails_ItemDataBound()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.rptXmasDetails_ItemDataBound()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.rptXmasDetails_ItemDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.rptXmasDetails_ItemDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.rptXmasDetails_ItemDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
        }

        /// <summary>
        /// This method will get the reward points from smart voucher service and calculate the vouchers.
        /// </summary>
        protected void DisplayCollectionPeriodDataFromSV()
        {
            GetVoucherValAllCPSRsp response = null;
            Hashtable htVouchersToDisplay;

            try
            {
                decimal sumXmasVoucher = 0;
                string voucherValue;
                string stDate = startDate.ToString("yyyyMMdd");
                string enDate = endDate.ToString("yyyyMMdd");

                svServiceCall = new SmartVoucherServicesClient();
                //Get the Clubcard voucher value from the webservice.
                response = new GetVoucherValAllCPSRsp();
                response = svServiceCall.GetVoucherValCPS(cardNumber, stDate, enDate);

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV()");
                #endregion

                if (response != null && response.dsResponse != null)
                {
                    htVouchersToDisplay = new Hashtable();
                    DataSet voucherDataSet = response.dsResponse;
                    int counter = 0;
                    int remainders = 0;
                    int rewardPointsForCP = 0;
                    int prevCPPnts = 0;

                    if (voucherDataSet.Tables.Count >= 1 && voucherDataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = voucherDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow row = voucherDataSet.Tables[0].Rows[i];

                            if (!string.IsNullOrEmpty(row["Reward_Points"].ToString()))
                            {
                                rewardPointsForCP = (Convert.ToInt32(row["Reward_Points"].ToString()) - prevCPPnts) + remainders;
                                prevCPPnts = Convert.ToInt32(row["Reward_Points"].ToString());
                                voucherValue = Helper.VoucherDisplay(rewardPointsForCP, out remainders);

                                sumXmasVoucher = sumXmasVoucher + Convert.ToDecimal(voucherValue);

                                htVouchersToDisplay.Add("StatementDate" + i, row["Statement_Date"].ToString());
                                htVouchersToDisplay.Add("VoucherValue" + i, voucherValue);
                            }
                        }
                    }

                    //Read the hash table
                    for (int j = 0; j < (htVouchersToDisplay.Count / 2); j++)
                    {
                        if ((counter % 2) == 0 || counter == 0)
                        {
                            strBldHTML.Append("<tr>");
                        }
                        else
                        {
                            strBldHTML.Append("<tr class='alternate'>");
                        }

                        //Split - To get the Month name only. (e.g May from May 2010)
                        strBldHTML.Append("<td>" + htVouchersToDisplay["StatementDate" + j].ToString().Split(' ')[0] + "</td>");
                        strBldHTML.Append("<td class='right last'>" + String.Format("{0:C}", Convert.ToDecimal(htVouchersToDisplay["VoucherValue" + j])) + "</td>");

                        counter = counter + 1;
                    }

                    if (Convert.ToDecimal(sumXmasVoucher) <= 0)
                    {
                        dvVouchersSaved.Visible = false;
                    }

                    spnVoucherSaved.InnerHtml = strBldHTML.ToString();
                    spnSumOfVouchersSaved.InnerText = String.Format("{0:C}", sumXmasVoucher);
                    sumVoucherSavedSoFar = sumXmasVoucher;

                    //Clear all the item from Hash table.
                    htVouchersToDisplay.Clear();
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID") + ": Clubcard Number: " + cardNumber);
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.DisplayCollectionPeriodDataFromSV()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

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

                response = null;
                htVouchersToDisplay = null;
            }
        }

        /// <summary>
        /// To assign voucher value and calculate the bonus.
        /// </summary>
        protected void CalculateBonus()
        {
            try
            {
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.CalculateBonus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.CalculateBonus()");
                #endregion
                string[] topupRange = TopupRange.Split(',');
                string[] bonusVoucher = BonusVooucher.Split(',');
                trBonus.Visible = false;
                ttlToppedUoMoney.InnerHtml = String.Format("{0:C}", sumTtlToppedUpMoney);

                //Assign the value to Clubcard Vouchers saved so far
                spnCCVouchersSaved.InnerHtml = String.Format("{0:C}", sumVoucherSavedSoFar);

                ////If sum of vouchers saved so far and total topped up money greater than or equal to 100
                //if (sumTtlToppedUpMoney >= BusinessConstants.VOUCHERVALUE_TOBECOMPARED)
                //{
                //    trBonus.Visible = true;
                //    spnBonusVoucher.InnerHtml = String.Format("{0:C}", BusinessConstants.BONUSVOUCHERVALUE_FOR100);
                //    sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney + BusinessConstants.BONUSVOUCHERVALUE_FOR100;
                //}
                ////If sum of vouchers saved so far and total topped up money greater than or equal to 50 and less than 100
                //else if ((sumTtlToppedUpMoney >= BusinessConstants.MONEY_TOBESAVED_FORBONUS)
                //         && (sumTtlToppedUpMoney < BusinessConstants.VOUCHERVALUE_TOBECOMPARED))
                //{
                //    trBonus.Visible = true;
                //    spnBonusVoucher.InnerHtml = String.Format("{0:C}", BusinessConstants.BONUSVOUCHERVALUE_FOR50);
                //    sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney + BusinessConstants.BONUSVOUCHERVALUE_FOR50;
                //}
                ////If sum of vouchers saved so far and total topped up money less than 50, then show the message to customer.
                //else if ((sumTtlToppedUpMoney < BusinessConstants.MONEY_TOBESAVED_FORBONUS))
                //{
                //    sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney;
                //}
                int countoftopuprange = (topupRange.Length) - 1;
                for (int i = 0; i < countoftopuprange; i++)
                {
                    if ((sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i]))
                           && (sumTtlToppedUpMoney < Convert.ToDecimal(topupRange[i + 1])))
                    {
                        trBonus.Visible = true;
                        spnBonusVoucher.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(bonusVoucher[i]));
                        sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney + Convert.ToDecimal(bonusVoucher[i]);
                        // spnBonusValueFor100.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(bonusVoucher[i]));


                    }
                    else if (sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i + 1]) && sumTtlToppedUpMoney >= Convert.ToDecimal(Topuptoreceivemaxbonus))
                    {
                        if (i == 0)
                        {
                            trBonus.Visible = true;
                            spnBonusVoucher.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(MaxBonusVoucher));
                            sumVoucherSavedSoFar = sumVoucherSavedSoFar + Convert.ToDecimal(MaxBonusVoucher) + sumTtlToppedUpMoney;
                            // spnBonusValueFor100.InnerHtml = String.Format("{0:C}", Convert.ToDecimal(MaxBonusVoucher));
                            //congratesMsg.Visible = true;
                        }
                    }


                }

                spnTtlVouchersSoFar.InnerHtml = String.Format("{0:C}", sumVoucherSavedSoFar);
                spnTtlPnts.InnerHtml = String.Format("{0:C}", sumVoucherSavedSoFar);

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.CalculateBonus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.CalculateBonus()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.CalculateBonus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.CalculateBonus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.CalculateBonus()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
        }

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
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("Start: CSC ChristmasSavers.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC ChristmasSavers.SetHouseHoldStatus()");
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
                NGCTrace.NGCTrace.TraceInfo("End: CSC ChristmasSavers.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC ChristmasSavers.SetHouseHoldStatus()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC ChristmasSavers.SetHouseHoldStatus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC ChristmasSavers.SetHouseHoldStatus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC ChristmasSavers.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {

            }
        }
    }
}