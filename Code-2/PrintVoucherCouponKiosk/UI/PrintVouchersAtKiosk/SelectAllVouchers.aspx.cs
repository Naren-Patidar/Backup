using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Purpose: This section is allows the user to choose or print coupons and vouchers
    /// Updated By : Praveen Yadav to for localization and adding new functionality of choosing and printing coupons
    /// Updated on : 19th july 2012
    /// </summary>
    public partial class SelectAllVouchers : Base
    {
        #region Properties

        /// <summary>
        /// Gets or sets the session booking.
        /// </summary>
        /// <value>The session booking.</value>
        public BigExchange.BookingPrintVoucher BookingPrintVoucher
        {
            get
            {
                if (Session["BookingPrintVoucher"] == null)
                {
                    Server.Transfer("Error.aspx");
                    return null;
                }
                else
                {
                    return (BigExchange.BookingPrintVoucher)Session["BookingPrintVoucher"];
                }
            }
            set
            {
                Session["BookingPrintVoucher"] = value;
            }
        }

        public BigExchange.UnusedVoucherCollection VoucherCollAll
        {
            get
            {
                if (Session["VoucherCollAll"] == null)
                {
                    BigExchange.UnusedVoucherCollection vc = new BigExchange.UnusedVoucherCollection();
                    Session["VoucherCollAll"] = vc;
                }
                return (BigExchange.UnusedVoucherCollection)Session["VoucherCollAll"];

            }
            set
            {
                Session["VoucherCollAll"] = value;
            }
        }

        //Added to put coupon collection in session
        public BigExchange.UnusedCouponCollection CouponCollAll
        {
            get
            {
                if (Session["CouponCollAll"] == null)
                {
                    BigExchange.UnusedCouponCollection CC = new BigExchange.UnusedCouponCollection();
                    Session["CouponCollAll"] = CC;
                }
                return (BigExchange.UnusedCouponCollection)Session["CouponCollAll"];
            }
            set
            {
                Session["CouponCollAll"] = value;
            }
        }
        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Write("Start of SelectAllVouchers Page_Load", "Information");
            int errorStatusCode = 0;

            CheckVisibilityOfConfiguredImages();   
            if (!IsPostBack)
            {
                try
                { 
                    Int32 totVoucherCount=-1;
                    Int32 totCouponCount = -1;                    
                    decimal totVoucherVal=0;

                    try
                    {
                        VoucherCollAll = Helper.GetVoucherDetails(BookingPrintVoucher.Clubcard);
                        totVoucherCount = VoucherCollAll.Count;
                        if (totVoucherCount == 0)
                        {
                            lnkChooseVouchers.CssClass = "greybtn inactive";
                            lnkPrint.CssClass = "confirmVoucherSummary inactive";
                            lnkChooseVouchers.Enabled = false;
                            lnkPrint.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //If smartvoucher service returns any exception then update voucherstatus to 5 and let couponstatus to be as it is.
                        BookingPrintVoucher.Status = 5; //error
                        BookingPrintVoucher.CouponStatusID = BookingPrintVoucher.CouponStatusID; //error
                        BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                        clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                        Logger.Write(ex.Message + " SelectAllVouchers.aspx Page_Load() called GetVoucherDetails()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                        //voucher fetching error handled
                        lityouhave.Visible = false;
                        litVoucherCount.Visible = false;
                        litClubcardVouchersWorth.Visible = false;
                        litVoucherValue.Visible = false;
                        litVoucherErrorMessage.Visible = true;

                        lnkChooseVouchers.CssClass = "greybtn inactive";
                        lnkPrint.CssClass = "confirmVoucherSummary inactive";
                        lnkChooseVouchers.Enabled = false;
                        lnkPrint.Enabled = false;
                    }

                    try
                    {
                        //To get the Total Coupon Count
                        CouponCollAll = Helper.GetCouponDetails(BookingPrintVoucher.Clubcard, out errorStatusCode);
                        totCouponCount = CouponCollAll.Count();
                        if (totCouponCount == 0)
                        {
                            lnkChooseCoupons.CssClass = "greybtn inactive";
                            lnkPrintAllCoupons.CssClass = "confirmVoucherSummary inactive";
                            lnkChooseCoupons.Enabled = false;
                            lnkPrintAllCoupons.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {                        
                        //If coupon enquiry service returns any exception then update couponstatus to 5 and let voucherstatus to be as it is.
                        BookingPrintVoucher.CouponStatusID = 5; //error
                        BookingPrintVoucher.Status = BookingPrintVoucher.Status; //error
                        BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                        clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                        Logger.Write(ex.Message + " SelectAllVouchers.aspx Page_Load() called GetCouponDetails()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);

                        //Coupon fetching error handled
                        litCouponErrorMessage.Visible = true;
                        litYouhavecoupons.Visible = false;
                        litCouponCount.Visible = false;
                        litClubcardCoupons.Visible = false;

                        lnkChooseCoupons.CssClass = "greybtn inactive";
                        lnkPrintAllCoupons.CssClass = "confirmVoucherSummary inactive";
                        lnkChooseCoupons.Enabled = false;
                        lnkPrintAllCoupons.Enabled = false;

                    }
                    if (totVoucherCount == 0 && totCouponCount == 0)
                    {
                        //0 active vouchers so redirect to Error page
                        Response.Redirect("Error.aspx?ActiveVouchers=False",false);               
                    }                    
                    litVoucherCount.Text = totVoucherCount.ToString();
                    totVoucherVal = Helper.GetVoucherTotal(VoucherCollAll);
                    //Get Coupon Total Count   
                    if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                    {
                        litVoucherValue.Text = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + totVoucherVal;
                    }
                    else
                    {
                        litVoucherValue.Text = totVoucherVal+ " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                    }
                    litCouponCount.Text = totCouponCount.ToString();
                    if (errorStatusCode!=0)
                    {
                        litCouponErrorMessage.Visible = true;
                        litYouhavecoupons.Visible = false;
                        litCouponCount.Visible = false;
                        litClubcardCoupons.Visible = false;
                    }

                    //PV : Update Transaction table set totalactiveVoucher and totalactivecoupons 
                    BookingPrintVoucher.totalActiveVouchers = totVoucherCount;
                    BookingPrintVoucher.totalActiveCoupons = totCouponCount;
                    Helper.UpdateTranDetailsActiveVoucher(BookingPrintVoucher);
                    Logger.Write("End of SelectAllVouchers Page_Load", "Information");
                }
                catch (Exception ex)
                {
                    BookingPrintVoucher.Status = 5; //error
                    BookingPrintVoucher.CouponStatusID = 5; //error
                    BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                    clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                    //clubcardRewardClient.Logging(BookingPrintVoucher, ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);
                    Logger.Write(ex.Message + " SelectAllVouchers.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                    Response.Redirect("Error.aspx",false);  
                }
            }

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of SelectAllVouchers lnkCancel_Click", "Information");
                //Update transactiondetails table set status to CP (cancelled from Print)
                BookingPrintVoucher.Status = 3; //cancelled from Print
                BookingPrintVoucher.CouponStatusID = 3;
                Helper.UpdateTranDetailsStatus(BookingPrintVoucher);
                Logger.Write("End of SelectAllVouchers lnkCancel_Click", "Information");
                Response.Redirect("CancelAndRestart.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkCancel_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);  
            }
            
        }

       
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Verification.aspx?Existing=true", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkBack_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        public void CheckVisibilityOfConfiguredImages()
        {
            string ShowHighlightCurrentScreenName = string.Empty;
            string ShowCouponandVoucherImage = string.Empty;
            try
            {
                Logger.Write("Start of SelectAllVouchers CheckVisibilityOfConfiguredImages()", "Information");
                 ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];
                 ShowCouponandVoucherImage = ConfigurationManager.AppSettings["ShowCouponandVoucherImage"];

                // Check whether to highlight the current screen name
                if (ShowHighlightCurrentScreenName.ToLower() == "true")
                {
                    breadcrumbs.Visible = true;
                }
                else
                {
                    breadcrumbs.Visible = false;
                }
                if (ShowCouponandVoucherImage.ToLower() == "true")
                {
                    imgVoucher.Visible = true;
                    imgCoupon.Visible = true;
                }
                else
                {
                    imgVoucher.Visible = false;
                    imgCoupon.Visible = false;
                }
                Logger.Write("End of SelectAllVouchers CheckVisibilityOfConfiguredImages()", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }


        }

        protected void lnkChooseVouchers_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("VoucherDetails.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkChooseVouchers_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void lnkChooseCoupons_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CouponDetails.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkChooseCoupons_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }


        //Print All Vouchers button Click
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of SelectAllVouchers btnPrint_Click()", "Information");
                BookingPrintVoucher.UnusedVouchers = VoucherCollAll;               
                //VoucherCollAll.Save(BookingPrintVoucher.TransactionID);
                Helper.SaveVouchers(BookingPrintVoucher);
                Logger.Write("End of SelectAllVouchers btnPrint_Click()", "Information");
                Response.Redirect("PrintingVouchers.aspx", false);
            }
            catch (Exception ex)
            {
                BookingPrintVoucher.Status = 5; //error 
                BookingPrintVoucher.CouponStatusID = BookingPrintVoucher.CouponStatusID;
                BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                //clubcardRewardClient.SaveToTransError(BookingPrintVoucher, ex.ToString());
                Logger.Write(ex.Message + " SelectAllVouchers.aspx btnPrint_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void lnkPrintCoupons_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of SelectAllVouchers lnkPrintCoupons_Click()", "Information");
                BookingPrintVoucher.UnusedCoupons = CouponCollAll;
                Helper.SaveCoupons(BookingPrintVoucher);
                Logger.Write("End of SelectAllVouchers lnkPrintCoupons_Click()", "Information");
                Response.Redirect("PrintingCoupons.aspx", false);
            }
            catch (Exception ex)
            {
                BookingPrintVoucher.CouponStatusID = 5;
                BookingPrintVoucher.Status = BookingPrintVoucher.Status;
                BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsStatus);
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkPrintCoupons_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
        
        #endregion

       
        protected void lnkTerms_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TermsAndCondition.aspx?page=SelectAllVouchers.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " SelectAllVouchers.aspx lnkTerms_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }


    }
}