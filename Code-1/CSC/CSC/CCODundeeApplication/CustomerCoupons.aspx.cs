using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCODundeeApplication.CustomerService;
using System.ServiceModel;
using System.Data;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Web.UI.HtmlControls;
using CCODundeeApplication.ClubcardCouponServices;
using System.Globalization;

namespace CCODundeeApplication
{
    public partial class CustomerCoupons : System.Web.UI.Page
    {
        ClubcardCouponServiceClient couponSVCclient = null;
        CustomerServiceClient customerSvcClient = null;
        long houseHoldID = 0;
        long customerID = 0;
        XmlDocument resulDoc = null;
        string resultXml = string.Empty;
        string errorXml = string.Empty;
        string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
        //CCMCA-4855 Fix date format issue
        string cultureDefault = ConfigurationSettings.AppSettings["CultureDefault"].ToString();
        string dateFormat = ConfigurationManager.AppSettings["DateDisplayFormat"];
        DataSet dsCapability = null;
        //For Mutiple Redemption - To access in Item Bound
        DataSet dsRedeemedCoupons = null;
        CCODundeeApplication.ClubcardCouponServices.CouponInformation[] availableCoupons;
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
                Response.Redirect("Default.aspx", false);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
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
                            HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                            HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                            HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
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
                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                            {
                                customerCoupon.Disabled = false;
                            }
                            else
                            {
                                customerCoupon.Disabled = true;
                                customerCoupon.HRef = "";
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


                        }
                    }
                    #endregion
                }
                if ((!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID"))))
                {
                    customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));

                    //Get household ID from NGC, to be used to get customer coupons
                    houseHoldID = getHouseHoldID(customerID);

                    //To display available coupon detail
                    showAvailableCoupons(houseHoldID);

                    //To display redeemed coupon detail
                    showRedeemedCoupons(houseHoldID);
                    ////******** New coupon service call (3.6 release) END ********

                }
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:eCoupon.Home.Page_Load - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:eCoupon.Home.Page_Load - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:eCoupon.Home.Page_Load");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;

            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        protected long getHouseHoldID(long customerID)
        {
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows;
            DataSet dsCustomerInfo = null;
            maxRows = 0;
            Hashtable searchData = new Hashtable();
            searchData["CustomerID"] = customerID;
            //Preparing parameters for service call
            conditionXml = Helper.HashTableToXML(searchData, "customer");
            maxRows = 1;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CustomerCoupon.getHouseHoldID");
                NGCTrace.NGCTrace.TraceDebug("Start:CustomerCoupon.getHouseHoldID  - CustomerID :" + customerID);

                using (customerSvcClient = new CustomerServiceClient())
                {
                    if (customerSvcClient.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, culture))
                    {
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsCustomerInfo = new DataSet();
                        dsCustomerInfo.ReadXml(new XmlNodeReader(resulDoc));

                        if (dsCustomerInfo.Tables.Count > 0)
                        {
                            if (dsCustomerInfo.Tables["Customer"].Columns.Contains("HouseHoldID") != false
                                && !string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString()))
                            {
                                houseHoldID = Convert.ToInt64(dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString().Trim());
                            }
                        }
                    }
                }

                NGCTrace.NGCTrace.TraceInfo("End:CustomerCoupon.getHouseHoldID");
                NGCTrace.NGCTrace.TraceDebug("End:CustomerCoupon.getHouseHoldID  - CustomerID :" + customerID);
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CustomerCoupon.getHouseHoldID - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CustomerCoupon.getHouseHoldID - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CustomerCoupon.getHouseHoldID");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }

            return houseHoldID;
        }

        /// <summary>
        /// Display the list of all available coupons
        /// </summary>
        /// <param name="houseHoldID"></param>
        protected void showAvailableCoupons(long houseHoldID)
        {
            #region Variables
            string couponDetail = string.Empty;
            string errorXml = string.Empty;
            int totalCoupons = 0;
            #endregion

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CustomerCoupon.showAvailableCoupons");
                NGCTrace.NGCTrace.TraceDebug("Start:CustomerCoupon.showAvailableCoupons  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                couponSVCclient = new ClubcardCouponServiceClient();
                List<CCODundeeApplication.ClubcardCouponServices.CouponInformation> couponList = new List<CCODundeeApplication.ClubcardCouponServices.CouponInformation>();


                couponSVCclient.GetAvailableCoupons(out errorXml, out availableCoupons, out totalCoupons, houseHoldID);

                if (availableCoupons != null)
                {
                    rptCouponDetails.DataSource = availableCoupons;
                    rptCouponDetails.DataBind();
                }
                else
                {
                    rptCouponDetails.Visible = false;
                    dvMsgActiveCoupons.Visible = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:CustomerCoupon.showAvailableCoupons");
                NGCTrace.NGCTrace.TraceDebug("End:CustomerCoupon.showAvailableCoupons  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CustomerCoupon.showAvailableCoupons - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CustomerCoupon.showAvailableCoupons - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CustomerCoupon.showAvailableCoupons");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
            finally
            {
                if (couponSVCclient != null)
                {
                    if (couponSVCclient.State == CommunicationState.Faulted)
                    {
                        couponSVCclient.Abort();
                    }
                    else if (couponSVCclient.State != CommunicationState.Closed)
                    {
                        couponSVCclient.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Display the list of all redeemed coupons
        /// </summary>
        /// <param name="houseHoldID"></param>
        protected void showRedeemedCoupons(long houseHoldID)
        {
            #region Variables
            string couponDetail = string.Empty;
            string errorXml = string.Empty;

            #endregion

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CustomerCoupon.showRedeemedCoupons");
                NGCTrace.NGCTrace.TraceDebug("Start:CustomerCoupon.showRedeemedCoupons  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                couponSVCclient = new ClubcardCouponServiceClient();

                couponSVCclient.GetRedeemedCoupons(out errorXml, out couponDetail, houseHoldID, culture);

                if (couponDetail != "" && couponDetail != "<NewDataSet />")
                {
                    resulDoc = new XmlDocument();
                    dsRedeemedCoupons = new DataSet();
                    resulDoc.LoadXml(couponDetail);
                    dsRedeemedCoupons.ReadXml(new XmlNodeReader(resulDoc));

                    rptUsedCouponDetails.DataSource = dsRedeemedCoupons;
                    rptUsedCouponDetails.DataBind();
                }
                else
                {
                    rptUsedCouponDetails.Visible = false;
                    dvMsgRedeemedCoupons.Visible = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:CustomerCoupon.showRedeemedCoupons");
                NGCTrace.NGCTrace.TraceDebug("End:CustomerCoupon.showRedeemedCoupons  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CustomerCoupon.showRedeemedCoupons - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CustomerCoupon.showRedeemedCoupons - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CustomerCoupon.showRedeemedCoupons");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
            finally
            {
                if (couponSVCclient != null)
                {
                    if (couponSVCclient.State == CommunicationState.Faulted)
                    {
                        couponSVCclient.Abort();
                    }
                    else if (couponSVCclient.State != CommunicationState.Closed)
                    {
                        couponSVCclient.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptCouponDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CustomerCoupon.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:CustomerCoupon.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Literal ltrRedemptionStartDate = (Literal)e.Item.FindControl("ltrRedemptionStartDate");
                    Literal ltrRedemptionEndDate = (Literal)e.Item.FindControl("ltrRedemptionEndDate");
                    Literal ltrRedemptionRemain = (Literal)e.Item.FindControl("ltrRedemptionRemain");
                    Literal ltrBarcode = (Literal)e.Item.FindControl("ltrBarcode");
                    Literal ltrOnlineCode = (Literal)e.Item.FindControl("ltrOnlineCode");

                    //For Hiding TD's
                    HtmlTableCell tdRedemptionremain = (HtmlTableCell)e.Item.FindControl("tdRedemptionremain");
                    HtmlTableCell tdTotRedemptions = (HtmlTableCell)e.Item.FindControl("tdTotRedemptions");
                    //For Apply the CSS to last cell
                    HtmlTableCell tdBarCode = (HtmlTableCell)e.Item.FindControl("tdBarCode");

                    //CCMCA-4855 Fix date format issue
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(cultureDefault);

                    #region Hiding and Showing wrto Capabilities

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {

                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            //IF exists
                            if (dsCapability.Tables[0].Columns.Contains("MultipleRedemption") != false)
                            {

                                ltrRedemptionStartDate.Visible = true;
                                ltrRedemptionEndDate.Visible = true;
                                ltrRedemptionRemain.Visible = true;

                                DateTime date;
                                if (ltrRedemptionStartDate != null)
                                {
                                    if (DateTime.TryParse(ltrRedemptionStartDate.Text.Trim(), out date))
                                    {
                                        //ltrRedemptionStartDate.Text = date.ToString("dd/MM/yy");
                                        //CCMCA-4855 Fix date format issue 
                                        ltrRedemptionStartDate.Text = date.ToString(dateFormat, cultures);
                                    }
                                }
                                if (ltrRedemptionEndDate != null)
                                {
                                    if (DateTime.TryParse(ltrRedemptionEndDate.Text.Trim(), out date))
                                    {
                                        //ltrRedemptionEndDate.Text = date.ToString("dd/MM/yy");
                                        //CCMCA-4855 Fix date format issue
                                        ltrRedemptionEndDate.Text = date.ToString(dateFormat, cultures);
                                    }
                                }
                                //For Multiple Redemption
                                if (ltrRedemptionRemain != null)
                                {
                                    ltrRedemptionRemain.Text = (availableCoupons[e.Item.ItemIndex].RedemptionUtilized != null && availableCoupons[e.Item.ItemIndex].MaxRedemptionLimit != null) ? Convert.ToString(availableCoupons[e.Item.ItemIndex].MaxRedemptionLimit - availableCoupons[e.Item.ItemIndex].RedemptionUtilized) : "";
                                }
                                #region For issue: MKTG00007132
                                //Purpose of change: To mask the AplhaCode and Barcode (All not last 4 digit)
                                //Coupon Detail section
                                if (ltrBarcode != null)
                                {
                                    ltrBarcode.Text = Helper.MaskString(ltrBarcode.Text.Trim(), 0, 4, 'X');
                                }

                                if (ltrOnlineCode != null)
                                {
                                    ltrOnlineCode.Text = Helper.MaskString(ltrOnlineCode.Text.Trim(), 0, 4, 'X');
                                }
                                #endregion

                                //Show Table Items
                                tdRedemptionremain.Visible = true;
                                tdTotRedemptions.Visible = true;

                            }
                            //IF Not exists
                            else
                            {
                                //Commented to Fix Defect ID : MKTG00007670
                               // ltrRedemptionStartDate.Visible = false;
                                //ltrRedemptionEndDate.Visible = false;
                                ltrRedemptionRemain.Visible = false;

                                DateTime date;
                                if (ltrRedemptionStartDate != null)
                                {
                                    if (DateTime.TryParse(ltrRedemptionStartDate.Text.Trim(), out date))
                                    {
                                        //ltrRedemptionStartDate.Text = date.ToString("dd/MM/yy");
                                        //CCMCA-4855 Fix date format issue
                                        ltrRedemptionStartDate.Text = date.ToString(dateFormat, cultures);
                                    }
                                }
                                if (ltrRedemptionEndDate != null)
                                {
                                    if (DateTime.TryParse(ltrRedemptionEndDate.Text.Trim(), out date))
                                    {
                                        //ltrRedemptionEndDate.Text = date.ToString("dd/MM/yy");
                                        //CCMCA-4855 Fix date format issue
                                        ltrRedemptionEndDate.Text = date.ToString(dateFormat, cultures);
                                    }
                                }

                                #region For issue: MKTG00007132
                                //Purpose of change: To mask the AplhaCode and Barcode (All not last 4 digit)
                                //Coupon Detail section
                                if (ltrBarcode != null)
                                {
                                    ltrBarcode.Text = Helper.MaskString(ltrBarcode.Text.Trim(), 0, 4, 'X');
                                }

                                if (ltrOnlineCode != null)
                                {
                                    ltrOnlineCode.Text = Helper.MaskString(ltrOnlineCode.Text.Trim(), 0, 4, 'X');
                                }
                                #endregion

                                //For Table row TD's
                                tdRedemptionremain.Visible = false;
                                tdTotRedemptions.Visible = false;
                                //If hiding the last two added cell, then apply "center last" CSS to last visible Cell
                                tdBarCode.Attributes.Add("class", "center last");

                            }
                        }
                    }
                    #endregion
                }


                if (e.Item.ItemType == ListItemType.Header)
                {
                    Literal lclRedemptionRemain = (Literal)e.Item.FindControl("lclRedemptionRemain");
                    Literal lclTotRedemptions = (Literal)e.Item.FindControl("lclTotRedemptions");

                    HtmlTableCell thRedemptionremain = (HtmlTableCell)e.Item.FindControl("thRedemptionremain");
                    HtmlTableCell thTotRedemptions = (HtmlTableCell)e.Item.FindControl("thTotRedemptions");
                    //For Apply the CSS to last cell
                    HtmlTableCell thBarCode = (HtmlTableCell)e.Item.FindControl("thBarCode");

                    #region For Header Hiding and Showing wrto Capabilities

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {

                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            if (dsCapability.Tables[0].Columns.Contains("MultipleRedemption") != false)
                            {
                                lclRedemptionRemain.Visible = true;
                                lclTotRedemptions.Visible = true;

                                //Show Table Header 
                                thRedemptionremain.Visible = true;
                                thTotRedemptions.Visible = true;


                            }
                            else
                            {
                                lclRedemptionRemain.Visible = false;
                                lclTotRedemptions.Visible = false;
                                //For Header row
                                thRedemptionremain.Visible = false;
                                thTotRedemptions.Visible = false;

                                //If hiding the last two added headers cell, then apply "rounded-q4" CSS to last visible Cell
                                thBarCode.Attributes.Add("class", "rounded-q4");

                            }
                        }
                    }
                    #endregion
                }

                NGCTrace.NGCTrace.TraceInfo("End:CustomerCoupon.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:CustomerCoupon.rptUsedCouponDetails_ItemDataBound - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CustomerCoupon.rptUsedCouponDetails_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CustomerCoupon.rptUsedCouponDetails_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CustomerCoupon.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptUsedCouponDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    #region Hiding and Showing wrto Capabilities

                    Literal ltrIssuanceDate = (Literal)e.Item.FindControl("ltrIssuanceDate");
                    Literal ltrRedemptionDate = (Literal)e.Item.FindControl("ltrRedemptionDate");
                    Literal ltrClubCardNo = (Literal)e.Item.FindControl("ltrClubCardNo");
                    Literal ltrBarCode = (Literal)e.Item.FindControl("ltrBarCode");
                    Literal ltrCouponStatus = (Literal)e.Item.FindControl("ltrCouponStatus");
                    Literal ltrRedemptionTime = (Literal)e.Item.FindControl("ltrRedemptionTime");
                    //Table Cell for hiding and showing
                    HtmlTableCell tdClubCardNo = (HtmlTableCell)e.Item.FindControl("tdClubCardNo");
                    HtmlTableCell tdBarCode = (HtmlTableCell)e.Item.FindControl("tdBarCode");
                    HtmlTableCell tdCouponStatus = (HtmlTableCell)e.Item.FindControl("tdCouponStatus");
                    HtmlTableCell tdRedemptionNo = (HtmlTableCell)e.Item.FindControl("tdRedemptionNo");
                    HtmlTableCell tdTotalRedemption = (HtmlTableCell)e.Item.FindControl("tdTotalRedemption");
                    HtmlTableCell tdRedemptionStore = (HtmlTableCell)e.Item.FindControl("tdRedemptionStore");

                    //Input date string from WCF Coupon Service (By Default : MM/dd/YYYY HH:MM:ss tt), because server date format as in US
                    //To make it to UK format(any format), use the DateTime.ParseExtact method to overcome this issue.
                    //If you want change wrto culture/Country basis, change NULL to culture which is needed in ParseExact Method
                    String strServerSpecificDateFormat = ConfigurationManager.AppSettings["ServerSpecificDateFormat"];
                    String[] oldFormat = strServerSpecificDateFormat.Split(',');
                    //CCMCA-4855 Fix date format issue
                    System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(cultureDefault);

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {

                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            //IF exists
                            if (dsCapability.Tables[0].Columns.Contains("MultipleRedemption") != false)
                            {
                                //Enable all the controls
                                ltrIssuanceDate.Visible = true;
                                ltrRedemptionDate.Visible = true;
                                ltrClubCardNo.Visible = true;
                                ltrBarCode.Visible = true;
                                ltrCouponStatus.Visible = true;
                                //Table Cell
                                tdClubCardNo.Visible = true;
                                tdBarCode.Visible = true;
                                tdCouponStatus.Visible = true;
                                tdRedemptionNo.Visible = true;
                                tdTotalRedemption.Visible = true;

                                //DateTime date;

                                if (ltrIssuanceDate != null)
                                {
                                    NGCTrace.NGCTrace.TraceInfo("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Issuance Date");
                                    NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + " Issuance Date -" + ltrIssuanceDate.Text.Trim());

                                    DateTime ConvertedDateIssuance = DateTime.ParseExact(ltrIssuanceDate.Text.Trim(), oldFormat, new CultureInfo("en-GB"), DateTimeStyles.NoCurrentDateDefault);
                                    //ltrIssuanceDate.Text = ConvertedDateIssuance.ToString("dd/MM/yy");
                                    //CCMCA-4855 Fix date format issue
                                    ltrIssuanceDate.Text = ConvertedDateIssuance.ToString(dateFormat, cultures);

                                    //old Code
                                    //if (DateTime.TryParse(ltrIssuanceDate.Text.Trim(), out date))
                                    //{
                                    //    ltrIssuanceDate.Text = date.ToString("dd/MM/yy");
                                    //}


                                }
                                if (ltrRedemptionDate != null)
                                {
                                    NGCTrace.NGCTrace.TraceInfo("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Redemption Date");
                                    NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + " Redemption Date -" + ltrRedemptionDate.Text.Trim());

                                    DateTime ConvertedDateRedemption = DateTime.ParseExact(ltrRedemptionDate.Text.Trim(), oldFormat, new CultureInfo("en-GB"), DateTimeStyles.NoCurrentDateDefault);

                                    //ltrRedemptionDate.Text = ConvertedDateRedemption.ToString("dd/MM/yy");
                                    //CCMCA-4855 Fix date format issue
                                    ltrRedemptionDate.Text = ConvertedDateRedemption.ToString(dateFormat, cultures);
                                    ltrRedemptionTime.Text = ConvertedDateRedemption.ToString("hh:mm");

                                    //old code
                                    //if (DateTime.TryParse(ltrRedemptionDate.Text.Trim(), out date))
                                    //{
                                    //    ltrRedemptionDate.Text = date.ToString("dd/MM/yy");
                                    //    ltrRedemptionTime.Text = date.ToString("HH:MM");
                                    //}

                                }
                                //For Mutliple Redemption
                                if (ltrClubCardNo != null)
                                {
                                    ltrClubCardNo.Text = Helper.MaskString(dsRedeemedCoupons.Tables["UsedCouponDetail"].Rows[e.Item.ItemIndex]["ClubCardNo"].ToString(), 0, 4, 'X');
                                }
                                if (ltrBarCode != null)
                                {
                                    ltrBarCode.Text = Helper.MaskString(dsRedeemedCoupons.Tables["UsedCouponDetail"].Rows[e.Item.ItemIndex]["BarCodeNo"].ToString(), 0, 4, 'X');
                                }
                                if (ltrCouponStatus != null)
                                {
                                    ltrCouponStatus.Text = dsRedeemedCoupons.Tables["UsedCouponDetail"].Rows[e.Item.ItemIndex]["CouponStatus"].ToString().Equals("Redeem") ? "Coupon Used" : dsRedeemedCoupons.Tables["UsedCouponDetail"].Rows[e.Item.ItemIndex]["CouponStatus"].ToString().Equals("UnRedeem") ? "Coupon Voided" : "";

                                }
                            }
                            //If Not exists
                            else
                            {

                                
                                if (ltrIssuanceDate != null)
                                {
                                    NGCTrace.NGCTrace.TraceInfo("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Issuance Date");
                                    NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + " Issuance Date -" + ltrIssuanceDate.Text.Trim());

                                    DateTime ConvertedDateIssuance = DateTime.ParseExact(ltrIssuanceDate.Text.Trim(), oldFormat, new CultureInfo("en-GB"), DateTimeStyles.NoCurrentDateDefault);
                                    //ltrIssuanceDate.Text = ConvertedDateIssuance.ToString("dd/MM/yy");
                                    //CCMCA-4855 Fix date format issue
                                    ltrIssuanceDate.Text = ConvertedDateIssuance.ToString(dateFormat, cultures);

                                    //old Code
                                    //if (DateTime.TryParse(ltrIssuanceDate.Text.Trim(), out date))
                                    //{
                                    //    ltrIssuanceDate.Text = date.ToString("dd/MM/yy");
                                    //}


                                }
                                if (ltrRedemptionDate != null)
                                {
                                    NGCTrace.NGCTrace.TraceInfo("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Redemption Date");
                                    NGCTrace.NGCTrace.TraceDebug("Start:eCoupon.Home.rptUsedCouponDetails_ItemDataBound  - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString() + " Redemption Date -" + ltrRedemptionDate.Text.Trim());

                                    DateTime ConvertedDateRedemption = DateTime.ParseExact(ltrRedemptionDate.Text.Trim(), oldFormat, new CultureInfo("en-GB"), DateTimeStyles.NoCurrentDateDefault);

                                    //ltrRedemptionDate.Text = ConvertedDateRedemption.ToString("dd/MM/yy");
                                    //CCMCA-4855 Fix date format issue
                                    ltrRedemptionDate.Text = ConvertedDateRedemption.ToString(dateFormat, cultures);
                                    ltrRedemptionTime.Text = ConvertedDateRedemption.ToString("hh:mm");

                                    //old code
                                    //if (DateTime.TryParse(ltrRedemptionDate.Text.Trim(), out date))
                                    //{
                                    //    ltrRedemptionDate.Text = date.ToString("dd/MM/yy");
                                    //    ltrRedemptionTime.Text = date.ToString("HH:MM");
                                    //}

                                }

                                //Enable all the controls
                                //ltrIssuanceDate.Visible = false;
                                ltrRedemptionDate.Visible = false;
                                ltrClubCardNo.Visible = false;
                                ltrBarCode.Visible = false;
                                ltrCouponStatus.Visible = false;
                                //Table Cell
                                tdClubCardNo.Visible = false;
                                tdBarCode.Visible = false;
                                tdCouponStatus.Visible = false;
                                tdRedemptionNo.Visible = false;
                                tdTotalRedemption.Visible = false;

                                //If hiding the first and last four added cell, then apply "center last" CSS to last visible Cell
                                tdRedemptionStore.Attributes.Add("class", "center last");



                            }
                        }
                    }




                    #endregion
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    #region For Header Hiding and Showing wrto Capabilities

                    HtmlTableCell thClubcardNo = (HtmlTableCell)e.Item.FindControl("thClubcardNo");
                    HtmlTableCell thBarcocde = (HtmlTableCell)e.Item.FindControl("thBarcocde");
                    HtmlTableCell thCouponStatus = (HtmlTableCell)e.Item.FindControl("thCouponStatus");
                    HtmlTableCell thRedemptionNo = (HtmlTableCell)e.Item.FindControl("thRedemptionNo");
                    HtmlTableCell thTotRedemption = (HtmlTableCell)e.Item.FindControl("thTotRedemption");
                    //For Apply the CSS to second and last cell if hide the redemption section
                    HtmlTableCell thBarnum = (HtmlTableCell)e.Item.FindControl("thBarnum");
                    HtmlTableCell thplace = (HtmlTableCell)e.Item.FindControl("thplace");

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {

                        xmlCapability = new XmlDocument();
                        dsCapability = new DataSet();
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            //IF exists
                            if (dsCapability.Tables[0].Columns.Contains("MultipleRedemption") != false)
                            {
                                //Make it visible
                                thClubcardNo.Visible = true;
                                thBarcocde.Visible = true;
                                thCouponStatus.Visible = true;
                                thRedemptionNo.Visible = true;
                                thTotRedemption.Visible = true;

                            }
                            //If Not exists
                            else
                            {
                                ////For Table row TD's
                                thClubcardNo.Visible = false;
                                thBarcocde.Visible = false;
                                thCouponStatus.Visible = false;
                                thRedemptionNo.Visible = false;
                                thTotRedemption.Visible = false;

                                //If hiding the first and last four added cell, then apply "rounded-q4" CSS to last and first visible Cell
                                thplace.Attributes.Add("class", "rounded-q4");
                                thBarnum.Attributes.Add("class", "rounded-company first");
                            }
                        }
                    }
                    #endregion

                }




                NGCTrace.NGCTrace.TraceInfo("End:eCoupon.Home.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.TraceDebug("End:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:eCoupon.Home.rptUsedCouponDetails_ItemDataBound - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:eCoupon.Home.rptUsedCouponDetails_ItemDataBound");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
        }
    }
}
