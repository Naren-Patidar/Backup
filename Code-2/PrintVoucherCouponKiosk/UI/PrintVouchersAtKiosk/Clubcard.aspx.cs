using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Purpose: This section is used to Scan the Clubcard to print vouchers
    /// Updated By : Praveen Yadav to show the messages and button text in local language and make images configurable.
    /// Updated on : 20th july 2012
    /// </summary>
    public partial class Clubcard : Base
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
                    Session["BookingPrintVoucher"] = Helper.CreateBooking("O");
                }

                return (BigExchange.BookingPrintVoucher)Session["BookingPrintVoucher"];

            }
            set
            {
                Session["BookingPrintVoucher"] = value;
            }
        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the Click event of the lnkCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CancelAndRestart.aspx");
            //Response.Redirect(ConfigurationManager.AppSettings["ExitURL"].ToString());
        }

        /// <summary>
        /// Handles the Click event of the lnkTerms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkTerms_Click(object sender, EventArgs e)
        {
            Response.Redirect("TermsAndCondition.aspx?page=Clubcard.aspx");
        }

        /// <summary>
        /// Handles the Click event of the lnkTypeInClubcard control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkTypeInClubcard_Click(object sender, EventArgs e)
        {

            //string flag;
            //if (Request.QueryString["Existing"] != null)
            //    flag = "true";
            //else
            //    flag = "false";
            //Response.Redirect("ClubcardEntry.aspx?Existing=" + flag);
            Response.Redirect("ClubcardEntry.aspx");
        }  
       

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(hdnCountryCode.Value))
                {
                    hdnCountryCode.Value = ApplicationConstants.CurrentCulture.ToUpper();
                }
                else
                {
                    hdnCountryCode.Value = "EN-GB";
                }

                CheckVisibilityOfConfiguredImages();
                if (!IsPostBack)
                {
                    //if (ConfigurationManager.AppSettings["ServiceAvailable"] == "False")
                    //    Response.Redirect("Disabled.aspx");
                    //if (Request.QueryString["Existing"] == null)
                    //{
                    //    BookingPrintVoucher = null;
                    //    SetBookingProperties();
                    //}
                    BookingPrintVoucher = null;
                    SetBookingProperties();
                }
                if (Session["BookingPrintVoucher"] == null)
                {
                    SetBookingProperties();
                }

                if (!String.IsNullOrEmpty(hdnClubcard.Value))
                {
                    string clubcard = hdnClubcard.Value.Replace(Environment.NewLine, "");
                    AddClubcard(clubcard);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Clubcard.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
        /// <summary>
        /// Sets the booking properties.
        /// </summary>
        private void SetBookingProperties()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["KioskID"]))
                {
                    BookingPrintVoucher.KioskID = int.Parse(Request.QueryString["KioskID"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
                {
                    BookingPrintVoucher.StoreID = int.Parse(Request.QueryString["StoreID"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["KioskNo"]))
                {
                    BookingPrintVoucher.KioskNo = int.Parse(Request.QueryString["KioskNo"]);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Clubcard.aspx SetBookingProperties()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }


        private void AddClubcard(string clubcard)
        {
            bool hasValidClubcard = false;
            try
            {
                if (clubcard.Length == 16 || clubcard.Length == 18)
                {
                    hasValidClubcard = Helper.HasValidClubcard(clubcard);
                }
                else if (clubcard.Length == 13)
                {
                    hasValidClubcard = Helper.HasValid13DigitClubdcard(clubcard);
                }
                if (hasValidClubcard)
                {
                    //string flag;
                    //if (Request.QueryString["Existing"] != null)
                    //    flag = "true";
                    //else
                    //    flag = "false";
                    BookingPrintVoucher.Clubcard = clubcard;
                    BookingPrintVoucher.Status = 1;
                    BookingPrintVoucher.TranStartTime = Convert.ToDateTime(DateTime.Now);
                    BookingPrintVoucher.CouponStatusID = 1;
                    if (ProcessBooking())
                        Response.Redirect("Verification.aspx?Existing=false",false);
                    //Response.Redirect("Verification.aspx?Existing=" + flag);

                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Clubcard.aspx AddClubcard()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        private Boolean ProcessBooking()
        {

            BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
            try
            {
                BookingPrintVoucher = clubcardRewardClient.ProcessVoucherBooking(BookingPrintVoucher);
                if (BookingPrintVoucher.TransactionID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //BookingPrintVoucher.Status = 5; //error
                //clubcardRewardClient.Logging(BookingPrintVoucher, ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);
                Logger.Write(ex.Message + " Clubcard.aspx ProcessBooking()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                return false;
            }
            

        }
        #endregion

        /// <summary>
        /// Checks the visibility of some of the images based on configuration entry
        /// </summary>
        public void CheckVisibilityOfConfiguredImages()
        {
            try
            {
                string ShowTypeInYourClubcardNumberPanel = ConfigurationManager.AppSettings["ShowTypeInYourClubcardNumberPanel"];
                string ShowHaveTescoClubcardCreditCardOrPrevilegeCardPanel = ConfigurationManager.AppSettings["ShowHaveTescoClubcardCreditCardOrPrevilegeCardPanel"];
                string ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];

                // Check whether to show "Type In Your Clubcard Number Panel"
                if (ShowTypeInYourClubcardNumberPanel.ToLower() == "true")
                {
                    divTypeInYourClubcardNumber.Visible = true;
                }
                else
                {
                    divTypeInYourClubcardNumber.Visible = false;
                }

                // Check whether to show "Have Tesco Clubcard Credit Card or Previleges Card Panel"
                //if (ShowHaveTescoClubcardCreditCardOrPrevilegeCardPanel.ToLower() == "true")
                //{
                //    //pnlHaveTescoClubcardCreditCardOrPrevilegeCard.Visible = true;
                //}
                //else
                //{
                //    //pnlHaveTescoClubcardCreditCardOrPrevilegeCard.Visible = false;
                //}

                // Check whether to highlight the current screen name
                if (ShowHighlightCurrentScreenName.ToLower() == "true")
                {
                    breadcrumbs.Visible = true;
                }
                else
                {
                    breadcrumbs.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Clubcard.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}