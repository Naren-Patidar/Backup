using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
//using InstoreClubcardReward.Business;

namespace PrintVouchersAtKiosk
{
    // <summary>
    /// VoucherDetails class
    /// Purpose: This section allows selecting Vouchers by Clubcard customer
    ///Author Dimple 
    ///Modified by seema to make the screen multi lingual and to support country
    /// </summary>
    public partial class VoucherDetails : Base
    {

        #region Properties

        public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";
      
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

         private List<Int32> SelectedCustomersIndex
        {
            get
            {
                if (ViewState[SELECTED_CUSTOMERS_INDEX] == null)
                {
                    ViewState[SELECTED_CUSTOMERS_INDEX] = new List<Int32>();
                }

                return (List<Int32>)ViewState[SELECTED_CUSTOMERS_INDEX];
            }
        }
        #endregion
   
        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of VoucherDetails Page_Load()", "Information");
                CheckVisibilityOfConfiguredImages();

                if (!IsPostBack)
                {

                    int voucherCount;
                    voucherCount = VoucherCollAll.Count;

                    ViewState["TotVoucherCount"] = voucherCount;
                    if (voucherCount <= 0)
                    {
                        VoucherCollAll = Helper.GetVoucherDetails(BookingPrintVoucher.Clubcard);
                        voucherCount = VoucherCollAll.Count;
                        if (voucherCount <= 0)
                        {
                            //0 active vouchers so redirect to Error page
                            Response.Redirect("Error.aspx?ActiveVouchers=False", false);
                        }
                    }

                    gvVouchers.DataSource = VoucherCollAll;
                    gvVouchers.DataBind();

                    lblSelVoucher.Text = "0";
                    lblTotalPage.Text = GetLocalResourceObject("lblPage1Of").ToString() + gvVouchers.PageCount.ToString();
                    lnkPageUp.Enabled = false;
                    lnkPrint.Enabled = false;
                    if (gvVouchers.PageCount == 1)
                    {
                        lnkPageDown.Enabled = false;
                        pnlPageDown.CssClass = "greybtnsmall inactive";
                    }

                    if (CouponCollAll.Count <= 0)
                    {
                        lnkChooseCoupon.CssClass = "greybtn inactive";
                        lnkChooseCoupon.Enabled = false;
                    }

                }
                Logger.Write("End of VoucherDetails Page_Load()", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //Update transactiondetails table set status to 3 (cancelled from Print)
                BookingPrintVoucher.Status = 3;
                BookingPrintVoucher.CouponStatusID = 3;
                Helper.UpdateTranDetailsStatus(BookingPrintVoucher);
                Response.Redirect("CancelAndRestart.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkCancel_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SelectAllVouchers.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkBack_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }

        }

        protected void lnkSelAllVouchers_Click(object sender, EventArgs e)
        {
            
            bool lbSelAllChecked = false;
            LinkButton lnkHSelect = null;
            CheckBox chkHSelect = null;
            try
            {
                lnkHSelect = gvVouchers.HeaderRow.FindControl("lnkHSelect") as LinkButton;
                chkHSelect = gvVouchers.HeaderRow.FindControl("chkHSelect") as CheckBox;
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("chkSelect");
                    LinkButton lnkSelect = row.FindControl("lnkSelect") as LinkButton;

                    if (cb != null)
                    {
                        if (!chkHSelect.Checked)
                        {
                            cb.Checked = true;
                            lbSelAllChecked = true;
                            lnkSelect.CssClass = "inputbluesquaretick";
                            lnkHSelect.CssClass = "inputbluesquaretick";
                        }
                        else
                        {
                            cb.Checked = false;
                            lbSelAllChecked = false;
                            lnkSelect.CssClass = "inputbluesquare";
                            lnkHSelect.CssClass = "inputbluesquare";
                        }
                    }
                }

                if (!lbSelAllChecked)
                {
                    foreach (BigExchange.UnusedVoucher v in VoucherCollAll)
                    {
                        RemoveRowIndex(VoucherCollAll.IndexOf(v));
                    }
                    chkHSelect.Checked = false;

                    lblSelVoucher.Text = "0";
                    pnlPrint.CssClass = "confirm inactive";

                }
                else
                {
                    Decimal val = 0;
                    foreach (BigExchange.UnusedVoucher v in VoucherCollAll)
                    {
                        PersistRowIndex(VoucherCollAll.IndexOf(v));
                        val = val + v.VoucherValue;
                    }

                    lnkPrint.Enabled = true;
                    chkHSelect.Checked = true;
                    pnlPrint.CssClass = "confirm";
                    if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                    {
                        lblSelVoucher.Text = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + val;
                    }
                    else
                    {
                        lblSelVoucher.Text =val + " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkSelAllVouchers_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }

        }


        protected void lnkPageUp_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    var chkBox = row.FindControl("chkSelect") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (chkBox.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                gvVouchers.PageIndex = gvVouchers.PageIndex - 1;
                lblTotalPage.Text = GetLocalResourceObject("lblPage").ToString() +" "+ (gvVouchers.PageIndex + 1).ToString() +" "+ GetLocalResourceObject("lblOf").ToString() + " "+ gvVouchers.PageCount.ToString();
                gvVouchers.DataSource = VoucherCollAll;
                gvVouchers.DataBind();

                RePopulateCheckBoxes();

                if (gvVouchers.PageIndex == gvVouchers.PageCount - 1)
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = false;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall inactive";
                }
                else if (gvVouchers.PageIndex == 0)
                {
                    lnkPageUp.Enabled = false;
                    lnkPageDown.Enabled = true;
                    pnlPageUp.CssClass = "greybtnsmall inactive";
                    pnlPageDown.CssClass = "greybtnsmall";
                }
                else
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = true;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall";
                }

            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkPageUp_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void lnkPageDown_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    var chkBox = row.FindControl("chkSelect") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (chkBox.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                gvVouchers.PageIndex = gvVouchers.PageIndex + 1;
                lblTotalPage.Text = GetLocalResourceObject("lblPageCount").ToString() + " " +(gvVouchers.PageIndex + 1).ToString() +" "+ GetLocalResourceObject("lblPageCountOf").ToString() +" "+ gvVouchers.PageCount.ToString();
                gvVouchers.DataSource = VoucherCollAll;
                gvVouchers.DataBind();

                RePopulateCheckBoxes();


                if (gvVouchers.PageIndex == gvVouchers.PageCount - 1)
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = false;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall inactive";
                }
                else if (gvVouchers.PageIndex == 0)
                {
                    lnkPageUp.Enabled = false;
                    lnkPageDown.Enabled = true;
                    pnlPageUp.CssClass = "greybtnsmall inactive";
                    pnlPageDown.CssClass = "greybtnsmall";
                }
                else
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = true;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall";
                }

            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkPageDown_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    var chkBox = row.FindControl("chkSelect") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (chkBox.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                BigExchange.UnusedVoucherCollection usedVC = new BigExchange.UnusedVoucherCollection();
                foreach (BigExchange.UnusedVoucher v in VoucherCollAll)
                {
                    if (SelectedCustomersIndex != null)
                    {
                        if (SelectedCustomersIndex.Exists(i => i == VoucherCollAll.IndexOf(v)))
                        {
                            usedVC.Add(v);
                        }
                    }
                }

                if (usedVC.Count > 0)
                {
                    try
                    {
                        BookingPrintVoucher.UnusedVouchers = usedVC;
                        Helper.SaveVouchers(BookingPrintVoucher);
                        Response.Redirect("PrintingVouchers.aspx", false);
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(ex.Message + " VoucherDetails btnPrint_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                        Response.Redirect("Error.aspx",false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails btnPrint_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void gvVouchers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                switch (e.Row.RowType)
                {
                    case DataControlRowType.Header:
                        e.Row.Cells[2].Text = GetLocalResourceObject("lblGridHeaderValue").ToString() ;
                        e.Row.Cells[3].Text = GetLocalResourceObject("lblGridHeaderStatement").ToString();
                        e.Row.Cells[4].Text = GetLocalResourceObject("lblGridExpiryDate").ToString();

                        e.Row.Cells[0].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        break;

                    case DataControlRowType.DataRow:
                        if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                        {
                            e.Row.Cells[2].Text = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + e.Row.Cells[9].Text;
                        }
                        else
                        {
                            e.Row.Cells[2].Text = e.Row.Cells[9].Text + " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                        }
                        e.Row.Cells[2].CssClass = "valuelbl";
                        e.Row.Cells[4].Text =Helper.GetCultureSpecificDate(e.Row.Cells[3].Text); //string.Format(Thread.CurrentThread.CurrentCulture, "{0:d}", DateTime.ParseExact(Convert.ToDateTime(e.Row.Cells[3].Text).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToShortDateString());
                        //e.Row.Cells[3].Text = e.Row.Cells[6].Text;
                        if (e.Row.Cells[6].Text != string.Empty)
                        {
                            string issued = "01 " + e.Row.Cells[6].Text;
                            issued = issued.Replace(' ', '/');
                            DateTime issuedDate = Convert.ToDateTime(issued);
                            e.Row.Cells[3].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(issuedDate.Month) + " " + issuedDate.Year;
                        }


                        LinkButton lnkSelect = e.Row.FindControl("lnkSelect") as LinkButton;
                        lnkSelect.CommandName = "cmdlnkSelect";
                        lnkSelect.CommandArgument = e.Row.RowIndex.ToString();
                        lnkSelect.CssClass = "inputbluesquare";
                        e.Row.Cells[0].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        break;

                    case DataControlRowType.Footer:
                        e.Row.Cells[0].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        break;

                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails gvVouchers_RowDataBound()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }
        
        protected void gvVouchers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "cmdlnkSelect")
                {
                    int index = Convert.ToInt32(e.CommandArgument);

                    GridViewRow row = gvVouchers.Rows[index];
                    LinkButton lnkSelect = row.FindControl("lnkSelect") as LinkButton;
                    CheckBox cb = (CheckBox)row.FindControl("chkSelect");
                    if (cb != null)
                    {
                        if (lnkSelect.CssClass == "inputbluesquare")
                        {
                            cb.Checked = true;
                            lnkSelect.CssClass = "inputbluesquaretick";
                        }
                        else
                        {
                            cb.Checked = false;
                            lnkSelect.CssClass = "inputbluesquare";
                        }
                    }
                    chkAnySelectedvoucher();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails gvVouchers_RowCommand()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void gvVouchers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    var chkBox = row.FindControl("chkSelect") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (chkBox.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                gvVouchers.PageIndex = e.NewPageIndex;
                gvVouchers.DataSource = VoucherCollAll;
                gvVouchers.DataBind();

                RePopulateCheckBoxes();
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails gvVouchers_PageIndexChanging()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }
        
        #endregion

        #region Private Methods

        private void chkAnySelectedvoucher()
        {
            //int selectedRowCount = 0;
            Decimal val = 0;
            CheckBox chkHeaderSelect = null;
            LinkButton lnkHeaderSelect = null;
            try
            {
                chkHeaderSelect = gvVouchers.HeaderRow.FindControl("chkHSelect") as CheckBox;
                lnkHeaderSelect = gvVouchers.HeaderRow.FindControl("lnkHSelect") as LinkButton;
                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    var chkBox = row.FindControl("chkSelect") as CheckBox;
                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;
                    if (chkBox.Checked)
                    {
                        //selectedRowCount++;
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                if (ViewState["TotVoucherCount"] != null && SelectedCustomersIndex != null && Convert.ToInt32(ViewState["TotVoucherCount"]) == SelectedCustomersIndex.Count)
                {
                    chkHeaderSelect.Checked = true;
                    lnkHeaderSelect.CssClass = "inputbluesquaretick";
                }
                else
                {
                    lnkHeaderSelect.CssClass = "inputbluesquare";
                    chkHeaderSelect.Checked = false;
                }

                if (SelectedCustomersIndex != null)
                {
                    if (SelectedCustomersIndex.Count > 0)
                    {
                        lnkPrint.Enabled = true;
                        pnlPrint.CssClass = "confirm";
                        foreach (int itm in SelectedCustomersIndex)
                        {
                            val = val + VoucherCollAll[itm].VoucherValue;
                        }
                    }
                    else
                    {
                        lnkPrint.Enabled = false;
                        pnlPrint.CssClass = "confirm inactive";
                    }
                    if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                    {
                        lblSelVoucher.Text = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + val;
                    }
                    else
                    {
                        lblSelVoucher.Text = val+ " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                    }
                }
                else
                {
                    lnkPrint.Enabled = false;
                    pnlPrint.CssClass = "confirm inactive";
                    if (ApplicationConstants.ShowCurrencyAsPrefix == "TRUE")
                    {
                        lblSelVoucher.Text = GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() + val;
                    }
                    else
                    {
                        lblSelVoucher.Text =val + " " + GetGlobalResourceObject("GlobalResource", "CountryCurrencySymbol").ToString() ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails chkAnySelectedvoucher()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx");
            }
        }

        private void RePopulateCheckBoxes()
        {
            //int selVoucherCount = 0;
            LinkButton lnkHeaderSelect = null;
            CheckBox chkHeaderSelect = null;
            try
            {
                lnkHeaderSelect = (LinkButton)gvVouchers.HeaderRow.FindControl("lnkHSelect");
                chkHeaderSelect = (CheckBox)gvVouchers.HeaderRow.FindControl("chkHSelect");

                foreach (GridViewRow row in gvVouchers.Rows)
                {
                    CheckBox chkBox = row.FindControl("chkSelect") as CheckBox;
                    LinkButton lnkSelect = row.FindControl("lnkSelect") as LinkButton;

                    IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

                    if (SelectedCustomersIndex != null)
                    {
                        if (SelectedCustomersIndex.Exists(i => i == container.DataItemIndex))
                        {
                            //selVoucherCount = selVoucherCount + 1;
                            chkBox.Checked = true;
                            lnkSelect.CssClass = "inputbluesquaretick";
                        }
                    }
                }

                if (ViewState["TotVoucherCount"] != null && SelectedCustomersIndex != null && Convert.ToInt32(ViewState["TotVoucherCount"]) == SelectedCustomersIndex.Count)
                {
                    chkHeaderSelect.Checked = true;
                    lnkHeaderSelect.CssClass = "inputbluesquaretick";
                }
                else
                {
                    lnkHeaderSelect.CssClass = "inputbluesquare";
                    chkHeaderSelect.Checked = false;
                }

                //if (selVoucherCount == gvVouchers.Rows.Count)
                //{
                //    LinkButton lnkHeaderSelect = (LinkButton)gvVouchers.HeaderRow.FindControl("lnkHSelect");
                //    CheckBox chkHeaderSelect = (CheckBox)gvVouchers.HeaderRow.FindControl("chkHSelect");
                //    if (lnkHeaderSelect != null)
                //    {
                //        lnkHeaderSelect.CssClass = "inputbluesquaretick";
                //    }
                //    if (chkHeaderSelect != null)
                //    {
                //        chkHeaderSelect.Checked = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails RePopulateCheckBoxes()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx");
            }
        }
        
        private void RemoveRowIndex(int index)
        {
            try
            {
                SelectedCustomersIndex.Remove(index);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails RemoveRowIndex()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx");
            }
        }

        private void PersistRowIndex(int index)
        {
            try
            {
                if (!SelectedCustomersIndex.Exists(i => i == index))
                {
                    SelectedCustomersIndex.Add(index);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails PersistRowIndex()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx");
            }
        }

        #endregion

        public void CheckVisibilityOfConfiguredImages()
        {
            string ShowHighlightCurrentScreenName = null;
            try
            {
                ShowHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];
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
                Logger.Write(ex.Message + " VoucherDetails CheckVisibilityOfConfiguredImages()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }

        }

        protected void lnkTerms_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TermsAndCondition.aspx?page=VoucherDetails.aspx",false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkTerms_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        protected void lnkChooseCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CouponDetails.aspx",false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " VoucherDetails lnkChooseCoupon_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void gvVouchers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    
    }

}