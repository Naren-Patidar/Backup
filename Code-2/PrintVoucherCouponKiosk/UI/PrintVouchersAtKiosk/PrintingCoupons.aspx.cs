using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// PrintingCoupons class
    /// Purpose: This section allows printing coupons selected by Clubcard customer
    /// <para>Author : Jennifer John</para>
    /// <para>Date Created : 21/11/2012</para>
    /// </summary>
    public partial class PrintingCoupons : Base
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
                    Response.Redirect("Error.aspx",false);
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

        #endregion

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
                Logger.Write("Start of PrintingCoupons Page_Load()", "Information");
                BookingPrintVoucher.CouponStatusID = 4; //Success
                BookingPrintVoucher.PrintDate = Convert.ToDateTime(DateTime.Now);
                //Update transactiondetails table set status to 4 (Success) and PrintDate
                Helper.UpdateTranDetailsPrintDate(BookingPrintVoucher);
                AddPrintJavascriptToPage();
                Logger.Write("End of PrintingCoupons Page_Load()", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " PrintingCoupons Page_Load", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Adds the print javascript to page.
        /// </summary>
        /// <remarks></remarks>
        /// 
    
        private void AddPrintJavascriptToPage()
        {
            string clubCardNo = string.Empty;
            string maskedClubcardNo = string.Empty;
            string customerName = string.Empty;
            StringBuilder tokenPrintScript = null;
            int tokenCount = 0;
            string ExcludeChars = Convert.ToString(ConfigurationManager.AppSettings["CouponDescExcludeChars"]);
            string[] ExcludeCharsArr = null;

            try
            {
                Logger.Write("Start of PrintingCoupons AddPrintJavascriptToPage()", "Information");
                if (ExcludeChars != null && ExcludeChars != string.Empty)
                {
                    ExcludeCharsArr = ExcludeChars.Split(',');
                }
                tokenPrintScript = new StringBuilder();
                if (BookingPrintVoucher.UnusedCoupons.Count <= 0)
                {
                    // if we have no product lines to print something went wrong
                    Response.Redirect("Error.aspx",false);
                }
                else
                {
                    tokenCount = BookingPrintVoucher.UnusedCoupons.Count;
                    lblExchangeTokens.Text = tokenCount.ToString();
                }

                clubCardNo = BookingPrintVoucher.Clubcard.ToString();
                maskedClubcardNo = clubCardNo.PadLeft((clubCardNo.Length * 2) - 4, 'X');
                maskedClubcardNo = maskedClubcardNo.Replace(clubCardNo, "");
                maskedClubcardNo = maskedClubcardNo + clubCardNo.Substring(clubCardNo.Length - 4);

                customerName = BookingPrintVoucher.FirstName.Trim() + " " + BookingPrintVoucher.Surname.Trim();
                tokenPrintScript.Append("<script language=\"javascript\" type=\"text/javascript\">\n");
                tokenPrintScript.AppendLine("\tfunction PrintTokens() {");
                tokenPrintScript.AppendLine("\t\twindow.external.setTimeout(60);");
                foreach (BigExchange.UnusedCoupon coupon in BookingPrintVoucher.UnusedCoupons)
                {
                    if (ExcludeCharsArr != null)
                    {
                        for (int i = 0; i < ExcludeCharsArr.Length; i++)
                        {
                            coupon.CouponDescription = coupon.CouponDescription.Replace(ExcludeCharsArr[i], " ");
                        }
                    }
                    coupon.CouponDescription = coupon.CouponDescription.Replace("\n", " ");
                    tokenPrintScript.AppendFormat("\t\tPrintCoupon(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\");\n",
                                    coupon.CouponDescription,
                                    customerName,
                                    maskedClubcardNo,
                                    coupon.SmartAlphaCode,
                                    coupon.SmartBarcode,
                                    Helper.GetCultureSpecificDate(coupon.CouponExpiryDate.ToString()),
                                    BookingPrintVoucher.StoreID,
                                    BookingPrintVoucher.KioskNo
                                    );
                    tokenPrintScript.AppendLine("\t\twindow.external.deviceExecutive('Printer', 'Print', '');");
                }

                tokenPrintScript.Append("\t}\n\n");
                tokenPrintScript.AppendLine("\tfunction RedirectPage() {");
                tokenPrintScript.AppendLine("\t\tlocation.replace('Completed.aspx?PrintCode=2');");
                tokenPrintScript.Append("\t}\n\n");
                tokenPrintScript.AppendLine("\tfunction Print() {");
                tokenPrintScript.AppendLine("\t\tPrintTokens();");
                tokenPrintScript.AppendLine("\t\tsetTimeout(\"RedirectPage()\", 10000);");
                tokenPrintScript.AppendLine("\t}");
                tokenPrintScript.Append("</script>\n\n");

                ClientScript.RegisterStartupScript(typeof(String), "ClientManagerBottom", tokenPrintScript.ToString());
                Logger.Write("End of PrintingCoupons AddPrintJavascriptToPage()", "Information");   
            }
            catch(Exception ex)
            {
                Logger.Write(ex.Message + " PrintingCoupons AddPrintJavascriptToPage", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

    }
}