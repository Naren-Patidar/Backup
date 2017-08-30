using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    /// CouponDetails class
    /// Purpose: This section allows selecting coupons by Clubcard customer
    /// <para>Author</para>
    /// <para>Date Created</para>
    /// </summary>
    public partial class CouponDetails :Base
    {
        #region Properties

        public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";
        string ExcludeChars = string.Empty;
        string[] ExcludeCharsArr = null;

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
                    Logger.Write("Session BookingPrintVoucher is null" + " CouponDetails BookingPrintVoucher", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
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

        /// <summary>
        /// Gets the selected list of unused coupons by customer
        /// </summary>
        /// <value>The session value of list of selected unused coupons</value>
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

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int couponCount = 0;
            int errorStatusCode = 0;
           
            try
            {
                Logger.Write("Start of CouponDetails Page_Load()", "Information");

                ExcludeChars = Convert.ToString(ConfigurationManager.AppSettings["CouponDescExcludeChars"]);
                if (ExcludeChars != null && ExcludeChars != string.Empty)
                {
                    ExcludeCharsArr = ExcludeChars.Split(',');
                }

                CheckVisibilityOfConfiguredImages();
                if (!IsPostBack)
                {
                    couponCount = CouponCollAll.Count;
                    if (couponCount <= 0)
                    {
                        CouponCollAll = Helper.GetCouponDetails(BookingPrintVoucher.Clubcard, out errorStatusCode);
                        couponCount = CouponCollAll.Count;
                        if (couponCount <= 0)
                        {
                            //0 active Coupons so redirect to Error page
                            Logger.Write("There are no unused coupons for this clubcard - CouponCollAll count is 0" + " CouponDetails Page_Load", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                            Response.Redirect("Error.aspx?ActiveVouchers=False", false);
                        }
                        else
                        {
                            foreach (BigExchange.UnusedCoupon coupon in CouponCollAll)
                            {
                                if (ExcludeCharsArr != null)
                                {
                                    for (int i = 0; i < ExcludeCharsArr.Length; i++)
                                    {
                                        coupon.CouponDescription = coupon.CouponDescription.Replace(ExcludeCharsArr[i], " ");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (BigExchange.UnusedCoupon coupon in CouponCollAll)
                        {
                            if (ExcludeCharsArr != null)
                            {
                                for (int i = 0; i < ExcludeCharsArr.Length; i++)
                                {
                                    coupon.CouponDescription = coupon.CouponDescription.Replace(ExcludeCharsArr[i], " ");
                                }
                            }
                        }
                    }

                    //Store the total coupon couunt in viewstate
                    ViewState["TotCouponCount"] = couponCount;
                    gvCoupons.DataSource = CouponCollAll;
                    gvCoupons.DataBind();

                    lblTotalPage.Text = GetLocalResourceObject("lblPage1Of").ToString() + " " + gvCoupons.PageCount.ToString();
                    lnkPageUp.Enabled = false;
                    lnkPrint.Enabled = false;
                    if (gvCoupons.PageCount == 1)
                    {
                        lnkPageDown.Enabled = false;
                        pnlPageDown.CssClass = "greybtnsmall inactive";
                    }

                    if (VoucherCollAll.Count<=0)
                    {
                        lnkChooseVoucher.CssClass = "greybtn inactive";
                        lnkChooseVoucher.Enabled = false;
                    }
                }
                Logger.Write("End of CouponDetails Page_Load()", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails Page_Load", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }

        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of CouponDetails lnkCancel_Click", "Information");
                //Update transactiondetails table set status to 3 (cancelled from Print)
                BookingPrintVoucher.Status = 3;
                BookingPrintVoucher.CouponStatusID = 3;
                Helper.UpdateTranDetailsStatus(BookingPrintVoucher);
                Logger.Write("End of CouponDetails lnkCancel_Click", "Information");
                Response.Redirect("CancelAndRestart.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkCancel_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Back button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("SelectAllVouchers.aspx",false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkBack_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Grid view Page Up click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkPageUp_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of CouponDetails lnkPageUp_Click", "Information");
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    var chkBox = gvRow.FindControl("chkSelect") as CheckBox;

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

                gvCoupons.PageIndex = gvCoupons.PageIndex - 1;
                lblTotalPage.Text = GetLocalResourceObject("lblPage").ToString() + " " + (gvCoupons.PageIndex + 1).ToString() + " " + 
                    GetLocalResourceObject("lblOf").ToString() + " " + gvCoupons.PageCount.ToString();
                gvCoupons.DataSource = CouponCollAll;
                gvCoupons.DataBind();

                RePopulateCheckBoxes();

                if (gvCoupons.PageIndex == gvCoupons.PageCount - 1)
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = false;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall inactive";
                }
                else if (gvCoupons.PageIndex == 0)
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
                Logger.Write("End of CouponDetails lnkPageUp_Click", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkPageUp_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Grid view Page Down click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkPageDown_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of CouponDetails lnkPageDown_Click", "Information");
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    var chkBox = gvRow.FindControl("chkSelect") as CheckBox;

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

                gvCoupons.PageIndex = gvCoupons.PageIndex + 1;
                lblTotalPage.Text = GetLocalResourceObject("lblPageCount").ToString() + " " + (gvCoupons.PageIndex + 1).ToString() + " " + 
                    GetLocalResourceObject("lblPageCountOf").ToString() + " " + gvCoupons.PageCount.ToString();
                gvCoupons.DataSource = CouponCollAll;
                gvCoupons.DataBind();

                RePopulateCheckBoxes();

                if (gvCoupons.PageIndex == gvCoupons.PageCount - 1)
                {
                    lnkPageUp.Enabled = true;
                    lnkPageDown.Enabled = false;
                    pnlPageUp.CssClass = "greybtnsmall";
                    pnlPageDown.CssClass = "greybtnsmall inactive";
                }
                else if (gvCoupons.PageIndex == 0)
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
                Logger.Write("End of CouponDetails lnkPageDown_Click", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkPageDown_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Print button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            BigExchange.UnusedCouponCollection unsedCoupons = null;

            try
            {
                Logger.Write("Start of CouponDetails btnPrint_Click", "Information");
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    var chkBox = gvRow.FindControl("chkSelect") as CheckBox;

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

                unsedCoupons = new BigExchange.UnusedCouponCollection();
                foreach (BigExchange.UnusedCoupon v in CouponCollAll)
                {
                    if (SelectedCustomersIndex != null)
                    {
                        if (SelectedCustomersIndex.Exists(i => i == CouponCollAll.IndexOf(v)))
                        {
                            unsedCoupons.Add(v);
                        }
                    }
                }

                if (unsedCoupons.Count > 0)
                {
                    BookingPrintVoucher.UnusedCoupons = unsedCoupons;
                    Helper.SaveCoupons(BookingPrintVoucher);
                    Response.Redirect("PrintingCoupons.aspx",false);
                }
                Logger.Write("End of CouponDetails btnPrint_Click", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails btnPrint_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
            finally
            {
                unsedCoupons = null;
            }
        }

        /// <summary>
        /// Grid view row bound event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewRowEventArgs</param>
        protected void gvCoupons_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkSelectRow = null;
            try
            {
                Logger.Write("Start of CouponDetails gvCoupons_RowDataBound", "Information");
                switch (e.Row.RowType)
                {
                    case DataControlRowType.Header:
                        e.Row.Cells[3].Text = GetLocalResourceObject("lblGridHeaderStatement").ToString();
                        e.Row.Cells[4].Text = GetLocalResourceObject("lblGridExpiryDate").ToString();
                        e.Row.Cells[0].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;

                    case DataControlRowType.DataRow:
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text;
                        e.Row.Cells[4].Text = Helper.GetCultureSpecificDate(e.Row.Cells[4].Text);
                        lnkSelectRow = e.Row.FindControl("lnkSelect") as LinkButton;
                        lnkSelectRow.CommandName = "cmdlnkSelect";
                        lnkSelectRow.CommandArgument = e.Row.RowIndex.ToString();
                        lnkSelectRow.CssClass = "inputbluesquare";
                        e.Row.Cells[0].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;

                    case DataControlRowType.Footer:
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;
                }
                Logger.Write("End of CouponDetails gvCoupons_RowDataBound", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails gvCoupons_RowDataBound", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Grid view Row command event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewCommandEventArgs</param>
        protected void gvCoupons_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvRow = null;
            LinkButton lnkSelectRow = null;
            CheckBox chkSelectRow = null;
            int index = 0;

            try
            {
                Logger.Write("Start of CouponDetails gvCoupons_RowCommand", "Information");
                if (e.CommandName == "cmdlnkSelect")
                {
                    index = Convert.ToInt32(e.CommandArgument);
                    gvRow = gvCoupons.Rows[index];
                    lnkSelectRow = gvRow.FindControl("lnkSelect") as LinkButton;
                    chkSelectRow = (CheckBox)gvRow.FindControl("chkSelect");
                    if (chkSelectRow != null)
                    {
                        if (lnkSelectRow.CssClass == "inputbluesquare")
                        {
                            chkSelectRow.Checked = true;
                            lnkSelectRow.CssClass = "inputbluesquaretick";
                        }
                        else
                        {
                            chkSelectRow.Checked = false;
                            lnkSelectRow.CssClass = "inputbluesquare";
                        }
                    }
                    chkAnySelectedCoupon();

                }
                Logger.Write("End of CouponDetails gvCoupons_RowCommand", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails gvCoupons_RowCommand", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Grid view page index changing event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GridViewPageEventArgs</param>
        protected void gvCoupons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Logger.Write("Start of CouponDetails gvCoupons_PageIndexChanging", "Information");
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    var chkSelectRow = gvRow.FindControl("chkSelect") as CheckBox;

                    IDataItemContainer container = (IDataItemContainer)chkSelectRow.NamingContainer;

                    if (chkSelectRow.Checked)
                    {
                        PersistRowIndex(container.DataItemIndex);
                    }
                    else
                    {
                        RemoveRowIndex(container.DataItemIndex);
                    }
                }

                gvCoupons.PageIndex = e.NewPageIndex;
                gvCoupons.DataSource = CouponCollAll;
                gvCoupons.DataBind();
                RePopulateCheckBoxes();
                Logger.Write("End of CouponDetails gvCoupons_PageIndexChanging", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails gvCoupons_PageIndexChanging", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Grid view select any checkbox event
        /// </summary>
        /// <param></param>
        private void chkAnySelectedCoupon()
        {
            CheckBox chkHeaderSelect = null;
            LinkButton lnkHeaderSelect = null;

            try
            {
                Logger.Write("Start of CouponDetails chkAnySelectedCoupon", "Information");
                chkHeaderSelect = gvCoupons.HeaderRow.FindControl("chkHSelect") as CheckBox;
                lnkHeaderSelect = gvCoupons.HeaderRow.FindControl("lnkHSelect") as LinkButton;
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    var chkBox = gvRow.FindControl("chkSelect") as CheckBox;
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

                if (ViewState["TotCouponCount"] != null && SelectedCustomersIndex != null && Convert.ToInt32(ViewState["TotCouponCount"]) == SelectedCustomersIndex.Count)
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
                    }
                    else
                    {
                        lnkPrint.Enabled = false;
                        pnlPrint.CssClass = "confirm inactive";
                    }
                }
                else
                {
                    lnkPrint.Enabled = false;
                    pnlPrint.CssClass = "confirm inactive";
                }
                Logger.Write("End of CouponDetails chkAnySelectedCoupon", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails chkAnySelectedCoupon", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Grid view re-load event
        /// </summary>
        /// <param></param>
        private void RePopulateCheckBoxes()
        {
            CheckBox chkSelectRow = null;
            LinkButton lnkSelectRow = null;
            LinkButton lnkHeaderSelect = null;
            CheckBox chkHeaderSelect = null;
            try
            {
                Logger.Write("Start of CouponDetails RePopulateCheckBoxes", "Information");
                lnkHeaderSelect = (LinkButton)gvCoupons.HeaderRow.FindControl("lnkHSelect");
                chkHeaderSelect = (CheckBox)gvCoupons.HeaderRow.FindControl("chkHSelect");

                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    chkSelectRow = gvRow.FindControl("chkSelect") as CheckBox;
                    lnkSelectRow = gvRow.FindControl("lnkSelect") as LinkButton;

                    IDataItemContainer container = (IDataItemContainer)chkSelectRow.NamingContainer;

                    if (SelectedCustomersIndex != null)
                    {
                        if (SelectedCustomersIndex.Exists(i => i == container.DataItemIndex))
                        {
                            chkSelectRow.Checked = true;
                            lnkSelectRow.CssClass = "inputbluesquaretick";
                        }
                    }
                }

                if (ViewState["TotCouponCount"]!=null && SelectedCustomersIndex!=null && (Convert.ToInt32(ViewState["TotCouponCount"]) == SelectedCustomersIndex.Count ))
                {
                    lnkHeaderSelect.CssClass = "inputbluesquaretick";
                    chkHeaderSelect.Checked = true;
                }
                else
                {
                    lnkHeaderSelect.CssClass = "inputbluesquare";
                    chkHeaderSelect.Checked = false;
                }
                Logger.Write("End of CouponDetails RePopulateCheckBoxes", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails RePopulateCheckBoxes", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Remove unselected grid row from selected list
        /// </summary>
        /// <param name="index">int</param>
        private void RemoveRowIndex(int index)
        {
            try
            {
                SelectedCustomersIndex.Remove(index);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails RemoveRowIndex", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Add selected grid row to selected list
        /// </summary>
        /// <param name="index">int</param>
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
                Logger.Write(ex.Message + " CouponDetails PersistRowIndex", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        #endregion

        /// <summary>
        /// Set image in page based on configuration
        /// </summary>
        /// <param></param>
        public void CheckVisibilityOfConfiguredImages()
        {
            string showHighlightCurrentScreenName = string.Empty;
            try
            {
                Logger.Write("Start of CouponDetails CheckVisibilityOfConfiguredImages", "Information");
                showHighlightCurrentScreenName = ConfigurationManager.AppSettings["ShowHighlightCurrentScreenName"];

                if (showHighlightCurrentScreenName.ToLower() == "true")
                {
                    breadcrumbs.Visible = true;
                }
                else
                {
                    breadcrumbs.Visible = false;
                }
                Logger.Write("End of CouponDetails CheckVisibilityOfConfiguredImages", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails CheckVisibilityOfConfiguredImages", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Choose Voucher button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkChooseVoucher_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("VoucherDetails.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkChooseVoucher_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Terms and Conditions button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkTerms_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TermsAndCondition.aspx?page=CouponDetails.aspx",false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkTerms_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }

        /// <summary>
        /// Select All button click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void lnkSelAllCoupons_Click(object sender, EventArgs e)
        {
            bool lbSelAllChecked = false;
            CheckBox chkHeaderSelect = null;
            CheckBox chkSelectRow = null;
            LinkButton lnkHeaderSelect = null;
            LinkButton lnkSelectRow = null;

            try
            {
                Logger.Write("Start of CouponDetails lnkSelAllVouchers_Click", "Information");
                lnkHeaderSelect = gvCoupons.HeaderRow.FindControl("lnkHSelect") as LinkButton;
                chkHeaderSelect = gvCoupons.HeaderRow.FindControl("chkHSelect") as CheckBox;
                foreach (GridViewRow gvRow in gvCoupons.Rows)
                {
                    chkSelectRow = (CheckBox)gvRow.FindControl("chkSelect");
                    lnkSelectRow = gvRow.FindControl("lnkSelect") as LinkButton;

                    if (chkSelectRow != null)
                    {
                        if (!chkHeaderSelect.Checked)
                        {
                            chkSelectRow.Checked = true;
                            lbSelAllChecked = true;
                            lnkSelectRow.CssClass = "inputbluesquaretick";
                            lnkHeaderSelect.CssClass = "inputbluesquaretick";
                        }
                        else
                        {
                            chkSelectRow.Checked = false;
                            lbSelAllChecked = false;
                            lnkSelectRow.CssClass = "inputbluesquare";
                            lnkHeaderSelect.CssClass = "inputbluesquare";
                        }
                    }
                }

                if (!lbSelAllChecked)
                {
                    foreach (BigExchange.UnusedCoupon unusedCoupon in CouponCollAll)
                    {
                        RemoveRowIndex(CouponCollAll.IndexOf(unusedCoupon));
                    }
                    chkHeaderSelect.Checked = false;
                    pnlPrint.CssClass = "confirm inactive";
                }
                else
                {
                    foreach (BigExchange.UnusedCoupon unusedCoupon in CouponCollAll)
                    {
                        PersistRowIndex(CouponCollAll.IndexOf(unusedCoupon));
                    }
                    lnkPrint.Enabled = true;
                    chkHeaderSelect.Checked = true;
                    pnlPrint.CssClass = "confirm";
                }
                Logger.Write("End of CouponDetails lnkSelAllVouchers_Click", "Information");
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " CouponDetails lnkSelAllVouchers_Click", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx",false);
            }
        }
    }
}