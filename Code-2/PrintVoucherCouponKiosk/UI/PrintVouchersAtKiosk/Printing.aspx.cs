using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Threading;
using System.Globalization;



namespace PrintVouchersAtKiosk
{
    public partial class Printing : System.Web.UI.Page
    {

        #region Properties
        int PrintCoupon = 0;

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

        #endregion

        string culture = Helper.GetCulture();
        protected override void InitializeCulture()
        {

            string culture = Helper.GetCulture();
            UICulture = culture;
            Culture = culture;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(culture);
            base.InitializeCulture();
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
                PrintCoupon = Convert.ToInt32(Request.QueryString["PrintCode"]);
                if (PrintCoupon == 1) //Voucher
                {
                    BookingPrintVoucher.Status = 4; //Success

                }
                else if (PrintCoupon == 2) //Coupon
                {
                    BookingPrintVoucher.CouponStatusID = 4; //Success
                }
                //BookingPrintVoucher.PrintDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                BookingPrintVoucher.PrintDate = Convert.ToDateTime(DateTime.Now);
                //Update transactiondetails table set status to 4 (Success) and PrintDate
                Helper.UpdateTranDetailsPrintDate(BookingPrintVoucher);
                AddPrintJavascriptToPage();

            }
            catch (Exception ex)
            {
                Response.Redirect("Error.aspx", false);
            }
        }



        /// <summary>
        /// Adds the print javascript to page.
        /// </summary>
        /// <remarks></remarks>
        private void AddPrintJavascriptToPage()
        {
            StringBuilder tokenPrintScript = new StringBuilder();
            int tokenCount = 0;

            if (PrintCoupon == 1)
            {
                if (BookingPrintVoucher.UnusedVouchers.Count <= 0)
                {
                    // if we have no product lines to print something went wrong
                    Response.Redirect("Error.aspx");
                }
                else
                {
                    tokenCount = BookingPrintVoucher.UnusedVouchers.Count;
                    lblExchangeTokens.Text = tokenCount.ToString();
                }
            }
            else if (PrintCoupon == 2)
            {
                if (BookingPrintVoucher.UnusedCoupons.Count <= 0)
                {
                    // if we have no product lines to print something went wrong
                    Response.Redirect("Error.aspx");
                }
                else
                {
                    tokenCount = BookingPrintVoucher.UnusedCoupons.Count;
                    lblExchangeTokens.Text = tokenCount.ToString();
                }
            }

            //int tokenCount = Business.Booking.GetTokensCount(Booking) + 1;



            string cc = BookingPrintVoucher.Clubcard.ToString();
            string newcc = "";
            newcc = cc.PadLeft((cc.Length * 2) - 4, 'X');
            newcc = newcc.Replace(cc, "");
            newcc = newcc + cc.Substring(cc.Length - 4);


            string CustName = "Darren White";//BookingPrintVoucher.FirstName.Trim() + " " + BookingPrintVoucher.Surname.Trim();

            tokenPrintScript.Append("<script language=\"javascript\" type=\"text/javascript\">\n");

            tokenPrintScript.AppendLine("\tfunction PrintTokens() {");

            tokenPrintScript.AppendLine("\t\twindow.external.setTimeout(60);");
            if (PrintCoupon == 1)
            {
                foreach (BigExchange.UnusedVoucher voucher in BookingPrintVoucher.UnusedVouchers)
                {
                    tokenPrintScript.AppendFormat("\t\tPrintVoucher(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\");\n",
                                    GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + voucher.VoucherValue.ToString(),
                                    CustName,
                                    newcc,
                                    voucher.OnlineCode,
                                    voucher.Ean,
                                    voucher.ExpiryDate.ToString("dd-MM-yyyy"),
                                    BookingPrintVoucher.StoreID,
                                    BookingPrintVoucher.KioskNo
                                    );
                    tokenPrintScript.AppendLine("\t\twindow.external.deviceExecutive('Printer', 'Print', '');");

                }
            }
            else if (PrintCoupon == 2)
            {
                foreach (BigExchange.UnusedCoupon coupon in BookingPrintVoucher.UnusedCoupons)
                {
                    tokenPrintScript.AppendFormat("\t\tPrintCoupon(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\");\n",
                                    coupon.CouponDescription.ToString(),
                                    CustName,
                                    newcc,
                                    coupon.SmartAlphaCode,
                                    coupon.SmartBarcode,
                                    coupon.CouponExpiryDate.ToString(), //ToString("dd-MM-yyyy")
                                    BookingPrintVoucher.StoreID,
                                    BookingPrintVoucher.KioskNo
                                    );
                    tokenPrintScript.AppendLine("\t\twindow.external.deviceExecutive('Printer', 'Print', '');");

                }
            }

            tokenPrintScript.Append("\t}\n\n");

            tokenPrintScript.AppendLine("\tfunction RedirectPage() {");

            tokenPrintScript.AppendLine("\t\tlocation.replace('Completed.aspx');");

            tokenPrintScript.Append("\t}\n\n");

            tokenPrintScript.AppendLine("\tfunction Print() {");

            tokenPrintScript.AppendLine("\t\tPrintTokens();");

            tokenPrintScript.AppendLine("\t\tsetTimeout(\"RedirectPage()\", 10000);");

            tokenPrintScript.AppendLine("\t}");

            tokenPrintScript.Append("</script>\n\n");

            ClientScript.RegisterStartupScript(typeof(String), "ClientManagerBottom", tokenPrintScript.ToString());

        }


        /// <summary>
        /// Handles the Click event of the lnkCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Error.aspx");
        }
    }
}