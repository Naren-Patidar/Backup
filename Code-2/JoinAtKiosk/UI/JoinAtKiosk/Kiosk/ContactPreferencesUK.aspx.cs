using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using Resources;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class ContactPreferencesUK : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of ContactPreferencesUK Page_Load()", "General");
                if (string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserFirstName")))
                {
                    Response.Redirect("~/Kiosk/TimeOut.aspx", false);
                }
                else
                {
                    Helper.CheckAndResetCookie();
                }
                if (!IsPostBack)
                {
                    Helper.GetAndLoadConfigurationDetails();

                    hdnResources.Value = Resources.GlobalResources.ContactPrefTitle + "," + Resources.GlobalResources.PromotionalCodeTitle;

                    string showControl;
                    showControl = ConfigurationReader.GetStringConfigKeyToUpper("ShowContactPrefControls");
                    if (showControl.Contains("CLUBCARDCHARTERBTN"))
                    {
                        pnlClubcardCharter.Visible = true;
                        divspacer.Attributes.Add("class", "buttonspacer5");
                    }
                    else
                    {
                        pnlClubcardCharter.Visible = false;
                        divspacer.Attributes.Add("class", "buttonspacer6");
                    }

                    if (showControl.Contains("PROMOTIONALCODE"))
                    {
                        hdnShowPromoCode.Value = "Y";
                        //set maxlength for Promotional Code
                        if (CommonClassForJoin.PromotionalCodeMaxLength > 0)
                        {
                            txtPromotionalCode.MaxLength = CommonClassForJoin.PromotionalCodeMaxLength;
                        }
                        else
                        {
                            txtPromotionalCode.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("PromotionalCodeMaxLength"));
                        }
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
                        {
                            txtPromotionalCode.Text = Helper.GetTripleDESEncryptedCookieValue("PromotionCode").ToString();
                        }
                    }
                    else
                    {
                        divUKPostCode1.Attributes.Add("style", "display:none");
                        divNext.Attributes.Add("style", "display:none");
                        divConfirm.Attributes.Add("style", "display:block;margin-top: 0px;margin-left: -20px;");
                    }

                    hdnSelectedChk.Value = "";
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")) && Helper.GetTripleDESEncryptedCookieValue("TescoProduct").ToString().Trim() == "N")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoProduct";
                        lnkTescoProduct.Attributes.Add("class", "input92 inputbluesquaretick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")) && Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo").ToString().Trim() == "N")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoPartnerInfo";
                        lnkTescoPartnerInfo.Attributes.Add("class", "input92 inputbluesquaretick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")) && Helper.GetTripleDESEncryptedCookieValue("CustomerResearch").ToString().Trim() == "N")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "CustomerResearch";
                        lnkCustomerResearch.Attributes.Add("class", "input92 inputbluesquaretick");
                    }
                    if (CommonClassForJoin.PromotionalCodeReq == true)
                    {
                        lblPromOpt.Visible = false;
                    }
                    else
                    {
                        lblPromOpt.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Write("ContactPreferencesUK.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ContactPrefBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }

        }

        protected void NextPage_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of ContactPreferencesUK NextPage_Click()", "General");
                Boolean bFlag = true;
                //DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                if (txtPromotionalCode.Text.Trim() == string.Empty && CommonClassForJoin.PromotionalCodeReq == true)
                {
                    bFlag = false;
                }

                if (txtPromotionalCode.Text != string.Empty && CommonClassForJoin.PromotionalCodeMinLength > 0 && txtPromotionalCode.Text.Trim().Length < CommonClassForJoin.PromotionalCodeMinLength)
                {
                    bFlag = false;
                }
                
                if (bFlag)
                {
                    try
                    {
                        string updateXml = string.Empty;
                        string errorXml = string.Empty;
                        bool result = Helper.AccountDuplicationAndPromotionalCodeCheck(out updateXml, out errorXml, txtPromotionalCode.Text.Trim());
                        if (errorXml != "1")
                        {
                            string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("ContactPreferencesUK"));
                            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                        }
                        if (errorXml == "1")
                        {
                            string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferencesUK&ctrlID=PromoCode&resID=ContactPrefErr&imgID=ContactPrefBreadCrumb";
                            Response.Redirect(qstring, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        string ctrlID = "Final";
                        string imgID = "ContactPrefBreadCrumb";
                        string cResID = "NGCError";
                        Logger.Write("ContactPreferencesUK.aspx.cs:NextPage_Click():This code is not valid, please check and try again.", "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                        string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferencesUK&ctrlID=" + ctrlID + "&resID=" + cResID + "&imgID=" + imgID;
                        Response.Redirect(qstring, false);
                    }
                }
                else
                {
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferencesUK&ctrlID=PromoCode&resID=ContactPrefErr&imgID=ContactPrefBreadCrumb";
                    Response.Redirect(qstring, false);
                }
            }
            catch (Exception exp)
            {
                Logger.Write("ContactPreferencesUK.aspx.cs:NextPage_Click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ContactPrefBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void btnBack_click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of ContactPreferencesUK btnBack_click()", "General");
                //DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("ContactPreferencesUK"));
                Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx", false);
            }
            catch (Exception exp)
            {
                Logger.Write("ContactPreferencesUK.aspx.cs:btnBack_click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ContactPrefBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }

        private void SetCookiesOfCurrentPage()
        {
            try
            {
                Logger.Write("Start of ContactPreferencesUK SetCookiesOfCurrentPage()", "General");
                Helper.SetTripleDESEncryptedCookie("PromotionCode", txtPromotionalCode.Text.ToString().Trim());
                if (hdnSelectedChk.Value.ToString().Contains("TescoProduct"))
                    Helper.SetTripleDESEncryptedCookie("TescoProduct", "N");
                else
                    Helper.SetTripleDESEncryptedCookie("TescoProduct", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("TescoPartnerInfo"))
                    Helper.SetTripleDESEncryptedCookie("TescoPartnerInfo", "N");
                else
                    Helper.SetTripleDESEncryptedCookie("TescoPartnerInfo", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("CustomerResearch"))
                    Helper.SetTripleDESEncryptedCookie("CustomerResearch", "N");
                else
                    Helper.SetTripleDESEncryptedCookie("CustomerResearch", "Y");

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void DeleteCookiesOfCurrentPage()
        {
            try
            {
                Logger.Write("Start of ContactPreferencesUK DeleteCookiesOfCurrentPage()", "General");

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PromotionCode");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoProduct");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoPartnerInfo");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("CustomerResearch");
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        protected void CC_Click(object sender, EventArgs e)
        {
            SetCookiesOfCurrentPage();
            Response.Redirect("~/Kiosk/ClubcardCharter1.aspx?page=ContactPreferencesUK", false);
        }
    }

}