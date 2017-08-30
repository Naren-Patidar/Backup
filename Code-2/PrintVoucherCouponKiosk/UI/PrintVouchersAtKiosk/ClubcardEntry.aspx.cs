using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// Purpose: This section is allows the user to Type In their clubcard number to print vouchers
    /// Updated By : Praveen Yadav to show the messages and button text in local language and make images configurable.
    /// Updated on : 19th july 2012
    /// </summary>
    public partial class ClubcardEntry : Base
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
        /// Handles the Click event of the lnkBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            //string url = "default.aspx";

            //if (Request.QueryString["Existing"] != null)
            //{
            //    if (Request.QueryString["Existing"].ToString().Trim() == "true")
            //        url = "Clubcard.aspx?Existing=true";
            //}

            //Response.Redirect(url);
            Response.Redirect("Default.aspx");
        }

        /// <summary>
        /// Handles the Click event of the lnkConfirm control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                pnlError.Visible = false;
                string clubcard = txtClubcard.Text.Replace(Environment.NewLine, "");
                //Validation for numeric 
                clubcard = Helper.ValidateClubcard(clubcard);
                if (clubcard != "")
                    AddClubcard(clubcard);
                else
                {
                    pnlError.Visible = true;
                    lblError.Text = GetLocalResourceObject("lblErrorInvalidClubcardNumber").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " ClubcardEntry.aspx lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
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
                CheckVisibilityOfConfiguredImages();
                if (!IsPostBack)
                {
                    lnkConfirm.Attributes["disabled"] = "disabled";

                    //BookingPrintVoucher.Clubcard = "";
                    //BookingPrintVoucher.Status = 1;
                    BookingPrintVoucher.TranStartTime = Convert.ToDateTime(DateTime.Now);
                    //ProcessBooking();
                    //if (ConfigurationManager.AppSettings["ServiceAvailable"] == "False")
                    //    Response.Redirect("Disabled.aspx");
                    //Booking = null;
                    //SetBookingProperties();
                }
                txtClubcard.Focus();
                txtClubcard.Text = txtClubcard.Text;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " ClubcardEntry.aspx Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        public void CheckVisibilityOfConfiguredImages()
        {
            try
            {
                string ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];

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
                Logger.Write(ex.Message + " ClubcardEntry.aspx CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
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
                    //string flag="false";
                    //if (Request.QueryString["Existing"] != null)
                    //{
                    //    if (Request.QueryString["Existing"].ToString().Trim() == "true")
                    //    {
                    //        flag = "true";
                    //    }
                    //}
                    //else
                    //    flag = "false";
                    bool TransactionDone = false;
                    BookingPrintVoucher.Clubcard = clubcard;
                    BookingPrintVoucher.Status = 1;
                    BookingPrintVoucher.CouponStatusID = 1;
                    //BookingPrintVoucher.TranStartTime = Convert.ToDateTime(DateTime.Now);
                    TransactionDone = ProcessBooking();
                    if (TransactionDone)
                        // Response.Redirect("SelectAllVouchers.aspx");
                        Response.Redirect("Verification.aspx?Existing=false",false);
                    else
                        Response.Redirect("Error.aspx",false);

                    //Response.Redirect("Verification.aspx?Existing=" + flag);
                }
                else
                {
                    //Display Error
                    pnlError.Visible = true;
                    lblError.Text = GetLocalResourceObject("lblErrorInvalidClubcardNumber").ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " ClubcardEntry.aspx AddClubcard()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
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
                //clubcardRewardClient.Logging(BookingPrintVoucher, ex.ToString(),BigExchange.LoggingOperations.SaveToTransError);
                Logger.Write(ex.Message + " ClubcardEntry.aspx ProcessBooking()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                return false;
            }

        }
        #endregion

            
    }

}