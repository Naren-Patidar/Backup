using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    ///Prints Selected Vouchers at the printer
    ///Author Dimple 
    ///Modified by seema to make the screen multi lingual and to support country specific printing configuration
    /// </summary>
    public partial class PrintingVouchers : Base
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
                Logger.Write("Start of PrintingVouchers Page_Load()", "Information");
                BookingPrintVoucher.Status = 4; //Success
                BookingPrintVoucher.PrintDate = Convert.ToDateTime(DateTime.Now);
                //Update transactiondetails table set status to 4 (Success) and PrintDate
                Helper.UpdateTranDetailsPrintDate(BookingPrintVoucher);
                AddPrintJavascriptToPage();
                Logger.Write("End of PrintingVouchers Page_Load()", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " PrintingVouchers Page_Load", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        /// <summary>
        /// Adds the print javascript to page.
        /// </summary>
        /// <remarks></remarks>
        private void AddPrintJavascriptToPage()
        {
            string clubCard = String.Empty, newcc = String.Empty, CustName = String.Empty, VoucherVal=String.Empty;
            StringBuilder tokenPrintScript;
            int tokenCount = 0;
            try
            {
                 tokenPrintScript = new StringBuilder();

                if (BookingPrintVoucher.UnusedVouchers.Count <= 0)
                {
                    Logger.Write("Zero Vouchers "+ " PrintingVouchers AddPrintJavascriptToPage()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                    Response.Redirect("Error.aspx", false);
                }
                else
                {
                    tokenCount = BookingPrintVoucher.UnusedVouchers.Count;
                    lblExchangeTokens.Text = tokenCount.ToString();
                }

                clubCard = BookingPrintVoucher.Clubcard.ToString();
                newcc = clubCard.PadLeft((clubCard.Length * 2) - 4, 'X');
                newcc = newcc.Replace(clubCard, "");
                newcc = newcc + clubCard.Substring(clubCard.Length - 4);


                CustName = BookingPrintVoucher.FirstName.Trim() + " " + BookingPrintVoucher.Surname.Trim();

                tokenPrintScript.Append("<script language=\"javascript\" type=\"text/javascript\">\n");

                tokenPrintScript.AppendLine("\tfunction PrintTokens() {");

                tokenPrintScript.AppendLine("\t\twindow.external.setTimeout(60);");

                foreach (BigExchange.UnusedVoucher voucher in BookingPrintVoucher.UnusedVouchers)
                {
                    if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                    {
                        VoucherVal = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + voucher.VoucherValue;
                    }
                    else
                    {
                        VoucherVal =  voucher.VoucherValue + " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                    }
                    tokenPrintScript.AppendFormat("\t\tPrintVoucher(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\");\n",
                                    VoucherVal,
                                    CustName,
                                    newcc,
                                    voucher.OnlineCode,
                                    voucher.Ean,
                                    Helper.GetCultureSpecificDate(voucher.ExpiryDate.ToString()),//string.Format(Thread.CurrentThread.CurrentCulture, "{0:d}", DateTime.ParseExact(voucher.ExpiryDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString()), 
                                    BookingPrintVoucher.StoreID,
                                    BookingPrintVoucher.KioskNo
                                    );
                    tokenPrintScript.AppendLine("\t\twindow.external.deviceExecutive('Printer', 'Print', '');");

                }

                tokenPrintScript.Append("\t}\n\n");

                tokenPrintScript.AppendLine("\tfunction RedirectPage() {");

                tokenPrintScript.AppendLine("\t\tlocation.replace('Completed.aspx?PrintCode=1');");

                tokenPrintScript.Append("\t}\n\n");

                tokenPrintScript.AppendLine("\tfunction Print() {");

                tokenPrintScript.AppendLine("\t\tPrintTokens();");

                tokenPrintScript.AppendLine("\t\tsetTimeout(\"RedirectPage()\", 10000);");

                tokenPrintScript.AppendLine("\t}");

                tokenPrintScript.Append("</script>\n\n");

                ClientScript.RegisterStartupScript(typeof(String), "ClientManagerBottom", tokenPrintScript.ToString());
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " PrintingVouchers AddPrintJavascriptToPage()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {

        }


    }
}