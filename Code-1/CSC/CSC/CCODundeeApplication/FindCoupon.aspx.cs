using System;
using System.Collections.Generic;
using System.ServiceModel;
using CCODundeeApplication.ClubcardCouponServices;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace CCODundeeApplication
{
    public partial class FindCoupon : System.Web.UI.Page
    {
        #region Local varibales
        //Used in .aspx page for for hiding/unhiding the controls
        protected string spanCouponBarcode = "display:none";
        protected string spanOnlineCode = "display:none";
        ClubcardCouponServiceClient couponSVCclient = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Hide the links
                Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                lblCustomerDtl.Visible = false;
                Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                lblCustomePref.Visible = false;
                Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                lblCustomerPts.Visible = false;
                Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                lblCustomerCards.Visible = false;
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblviewpoints = (Label)Master.FindControl("lblviewpoints");
                lblviewpoints.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblMergerCards = (Label)Master.FindControl("lblMergeCards");
                lblMergerCards.Visible = false;
                Label lblAddPoints = (Label)Master.FindControl("lblAddPoints");
                lblAddPoints.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                lblDataConfiguration.Visible = false;
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
            }

            ContentPlaceHolder custDetailsLeftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
            custDetailsLeftNav.Visible = false;
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
                Response.Redirect("Default.aspx", false);
        }
        #endregion

        protected void btnFindCoupon_Click(object sender, EventArgs e)
        {
            #region Variables
            string couponDetail = string.Empty;
            string errorXml = string.Empty;
            string smartBarcode = string.Empty;
            string smartAlphaCode = string.Empty;
            string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
            DateTime RedmtnEndDate;
            DateTime IssuanceDate;
            XmlDocument resultDoc;
            DateTime IssueEndDate;
            DateTime RedemDate;
            DataSet dsRedeemedCoupons;
            string couponStatusID = string.Empty;
            #endregion
            spnErrorMsg.Visible = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:FindCoupon.btnFindCoupon_Click");
                NGCTrace.NGCTrace.TraceDebug("Start:FindCoupon.btnFindCoupon_Click - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                couponSVCclient = new ClubcardCouponServiceClient();
                //CouponInformation couponInformation;
                string couponInformation = string.Empty;

                smartBarcode = txtCouponBarcode.Text.Trim().ToString();
                smartAlphaCode = txtOnlineCode.Text.Trim().ToString();

                if (string.IsNullOrEmpty(smartBarcode))
                {
                    if (string.IsNullOrEmpty(smartAlphaCode))
                    {
                        spnErrorMsg.Visible = true;
                        dvCouponInfo.Visible = false;
                    }
                    else
                    {
                        spnErrorMsg.Visible = false;

                        couponSVCclient.GetCouponInformation(out errorXml, out couponInformation, smartBarcode, smartAlphaCode, culture);

                        if (string.IsNullOrEmpty(errorXml))
                        {
                            //Display coupon information section
                            dvCouponInfo.Visible = true;
                            //dvCouponMsg.Visible = false;

                            resultDoc = new XmlDocument();
                            dsRedeemedCoupons = new DataSet();
                            resultDoc.LoadXml(couponInformation);
                            dsRedeemedCoupons.ReadXml(new XmlNodeReader(resultDoc));

                            if (dsRedeemedCoupons.Tables.Count > 0 && dsRedeemedCoupons.Tables["CouponInformation"].Rows.Count > 0)
                            {
                                couponStatusID = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["CouponStatusId"].ToString();



                                #region For issue: MKTG00007132
                                //Purpose of change: To mask the AplhaCode and Barcode (All not last 4 digit)
                                //Coupon Detail section
                                //lblOnlineCode.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartAlphaNumeric"].ToString();
                                lblOnlineCode.Text = Helper.MaskString(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartAlphaNumeric"].ToString().Trim(), 0, 4, 'X');

                                //lblBarcodeNo.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartBarcode"].ToString();
                                lblBarcodeNo.Text = Helper.MaskString(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartBarcode"].ToString().Trim(), 0, 4, 'X');
                                #endregion
                                lblCouponDescr.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["CouponDescription"].ToString();

                                if (couponStatusID == "0")
                                {
                                    lblCouponStatus.Text = "Active";
                                }
                                else if (couponStatusID == "15")
                                {
                                    lblCouponStatus.Text = "Redeemed";
                                }

                                if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionEndDate"].ToString(), out RedmtnEndDate))
                                {
                                    lblExpDate.Text = RedmtnEndDate.ToString("dd/MM/yy");
                                }

                                int maxRedmtn = Convert.ToInt32(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["MaxRedemptionLimit"]);
                                int redmtnUtlzd = Convert.ToInt32(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionUtilized"]);

                                lblTotalRedmtns.Text = redmtnUtlzd.ToString();
                                if ((maxRedmtn - redmtnUtlzd) < 0)
                                {
                                    lblRedmtnsRemain.Text = "0";
                                }
                                else
                                {
                                    lblRedmtnsRemain.Text = (maxRedmtn - redmtnUtlzd).ToString();
                                }

                                //Coupon Issuance section
                                if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceDate"].ToString(), out IssueEndDate))
                                {
                                    lblIssueDateTime.Text = IssueEndDate.ToString("dd/MM/yy HH:mm:ss");
                                }
                                //lblIssueDateTime.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceDate"].ToString();
                                lblIssuanceChannel.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceChannel"].ToString();
                                lblIssueStore.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceStoreName"].ToString();

                                //Coupon Redemption section
                                lblClubcardNo.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["ClubcardNumber"].ToString();
                                if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionDate"].ToString(), out RedemDate))
                                {
                                    lblRedemtnDate.Text = RedemDate.ToString("dd/MM/yy HH:mm:ss");
                                }
                                //lblRedemtnDate.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionDate"].ToString();
                                lblRedemdPlace.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionStoreName"].ToString();
                                lblTransType.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionType"].ToString();
                            }
                            else
                            {
                                spnErrorMsg.Visible = true;
                                dvCouponInfo.Visible = false;
                            }
                        }
                        else
                        {
                            spnErrorMsg.Visible = true;
                            dvCouponInfo.Visible = false;
                        }
                    }
                }
                else
                {
                    spnErrorMsg.Visible = false;

                    couponSVCclient.GetCouponInformation(out errorXml, out couponInformation, smartBarcode, smartAlphaCode, culture);

                    if (string.IsNullOrEmpty(errorXml))
                    {
                        //Display coupon information section
                        dvCouponInfo.Visible = true;
                        //dvCouponMsg.Visible = false;

                        resultDoc = new XmlDocument();
                        dsRedeemedCoupons = new DataSet();
                        resultDoc.LoadXml(couponInformation);
                        dsRedeemedCoupons.ReadXml(new XmlNodeReader(resultDoc));
                        if (dsRedeemedCoupons.Tables.Count > 0 && dsRedeemedCoupons.Tables["CouponInformation"].Rows.Count > 0)
                        {
                            couponStatusID = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["CouponStatusId"].ToString();

                            #region For issue: MKTG00007132
                            //Purpose of change: To mask the AplhaCode and Barcode (All not last 4 digit)
                            //Coupon Detail section
                            //lblOnlineCode.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartAlphaNumeric"].ToString();
                            lblOnlineCode.Text = Helper.MaskString(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartAlphaNumeric"].ToString().Trim(), 0, 4, 'X');
                            //lblBarcodeNo.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartBarcode"].ToString();
                            lblBarcodeNo.Text = Helper.MaskString(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["SmartBarcode"].ToString().Trim(), 0, 4, 'X');
                            #endregion

                            lblCouponDescr.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["CouponDescription"].ToString();


                            if (couponStatusID == "0")
                            {
                                lblCouponStatus.Text = "Active";
                            }
                            else if (couponStatusID == "15")
                            {
                                lblCouponStatus.Text = "Redeemed";
                            }

                            if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionEndDate"].ToString(), out RedmtnEndDate))
                            {
                                lblExpDate.Text = RedmtnEndDate.ToString("dd/MM/yy");
                            }

                            int maxRedmtn = Convert.ToInt32(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["MaxRedemptionLimit"]);
                            int redmtnUtlzd = Convert.ToInt32(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionUtilized"]);

                            lblTotalRedmtns.Text = redmtnUtlzd.ToString();
                            if ((maxRedmtn - redmtnUtlzd) < 0)
                            {
                                lblRedmtnsRemain.Text = "0";
                            }
                            else
                            {
                                lblRedmtnsRemain.Text = (maxRedmtn - redmtnUtlzd).ToString();
                            }


                            //Coupon Issuance section
                            //lblIssueDateTime.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceDate"].ToString();
                            if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceDate"].ToString(), out IssueEndDate))
                            {
                                lblIssueDateTime.Text = IssueEndDate.ToString("dd/MM/yy HH:mm:ss");
                            }
                            lblIssuanceChannel.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceChannel"].ToString();
                            lblIssueStore.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["IssuanceStoreName"].ToString();

                            //Coupon Redemption section
                            lblClubcardNo.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["ClubcardNumber"].ToString();
                            //lblRedemtnDate.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionDate"].ToString();
                            if (DateTime.TryParse(dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionDate"].ToString(), out RedemDate))
                            {
                                lblRedemtnDate.Text = RedemDate.ToString("dd/MM/yy HH:mm:ss");
                            }
                            lblRedemdPlace.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionStoreName"].ToString();
                            lblTransType.Text = dsRedeemedCoupons.Tables["CouponInformation"].Rows[0]["RedemptionType"].ToString();
                        }
                        else
                        {
                            spnErrorMsg.Visible = true;
                            dvCouponInfo.Visible = false;
                        }
                    }
                    else
                    {
                        spnErrorMsg.Visible = true;
                        dvCouponInfo.Visible = false;
                    }
                }

                NGCTrace.NGCTrace.TraceInfo("End:FindCoupon.btnFindCoupon_Click");
                NGCTrace.NGCTrace.TraceDebug("End:FindCoupon.btnFindCoupon_Click - CustomerID :" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:FindCoupon.btnFindCoupon_Click - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:FindCoupon.btnFindCoupon_Click - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:FindCoupon.btnFindCoupon_Click");
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
    }
}
