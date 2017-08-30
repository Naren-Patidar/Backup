using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Resources;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class ContactPreferences : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of ContactPreferences Page_Load()", "General");
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

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail")) && Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoGroupMail";
                        lnkTescoGroupMail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail")) && Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoGroupEmail";
                        lnkTescoGroupEmail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone")) && Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoGroupPhone";
                        lnkTescoGroupPhone.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS")) && Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "TescoGroupSMS";
                        lnkTescoGroupSMS.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerMail")) && Helper.GetTripleDESEncryptedCookieValue("PartnerMail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "PartnerMail";
                        lnkPartnerMail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerEmail")) && Helper.GetTripleDESEncryptedCookieValue("PartnerEmail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "PartnerEmail";
                        lnkPartnerEmail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerPhone")) && Helper.GetTripleDESEncryptedCookieValue("PartnerPhone").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "PartnerPhone";
                        lnkPartnerPhone.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerSMS")) && Helper.GetTripleDESEncryptedCookieValue("PartnerSMS").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "PartnerSMS";
                        lnkPartnerSMS.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchMail")) && Helper.GetTripleDESEncryptedCookieValue("ResearchMail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "ResearchMail";
                        lnkResearchMail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchEmail")) && Helper.GetTripleDESEncryptedCookieValue("ResearchEmail").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "ResearchEmail";
                        lnkResearchEmail.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchPhone")) && Helper.GetTripleDESEncryptedCookieValue("ResearchPhone").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "ResearchPhone";
                        lnkResearchPhone.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchSMS")) && Helper.GetTripleDESEncryptedCookieValue("ResearchSMS").ToString().Trim() == "Y")
                    {
                        hdnSelectedChk.Value = hdnSelectedChk.Value + "ResearchSMS";
                        lnkResearchSMS.Attributes.Add("class", "inputbluesquareCPtick");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
                    {
                        txtPromotionalCode.Text = Helper.GetTripleDESEncryptedCookieValue("PromotionCode").ToString().Trim();
                    }
                    if(CommonClassForJoin.PromotionalCodeReq==true)
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
                Logger.Write("ContactPreferences.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
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
                Logger.Write("Start of ContactPreferences NextPage_Click()", "General");
                Boolean bFlag = true;
                DeleteCookiesOfCurrentPage();
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
                            string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("ContactPreferences"));
                            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                        }
                        if (errorXml == "1")
                        {
                            string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferences&ctrlID=PromoCode&resID=ContactPrefErr&imgID=ContactPrefBreadCrumb";
                            Response.Redirect(qstring, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        string ctrlID = "Final";
                        string imgID = "ContactPrefBreadCrumb";
                        string cResID = "NGCError";
                        Logger.Write("ContactPreferences.aspx.cs:NextPage_Click():This code is not valid, please check and try again.", "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                        string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferences&ctrlID=" + ctrlID + "&resID=" + cResID + "&imgID=" + imgID;
                        Response.Redirect(qstring, false);
                    }
                }
                else
                {
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=ContactPreferences&ctrlID=PromoCode&resID=ContactPrefErr&imgID=ContactPrefBreadCrumb";
                    Response.Redirect(qstring, false);
                }
            }
            catch (Exception exp)
            {
                Logger.Write("ContactPreferences.aspx.cs:NextPage_Click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
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
                Logger.Write("Start of ContactPreferences btnBack_click()", "General");
                DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("ContactPreferences"));
                Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx", false);
            }
            catch (Exception exp)
            {
                Logger.Write("ContactPreferences.aspx.cs:btnBack_click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
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
                Logger.Write("Start of ContactPreferences SetCookiesOfCurrentPage()", "General");
                Helper.SetTripleDESEncryptedCookie("PromotionCode", txtPromotionalCode.Text.ToString().Trim());

                if (hdnSelectedChk.Value.ToString().Contains("TescoGroupMail"))
                    Helper.SetTripleDESEncryptedCookie("TescoGroupMail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("TescoGroupEmail"))
                    Helper.SetTripleDESEncryptedCookie("TescoGroupEmail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("TescoGroupPhone"))
                    Helper.SetTripleDESEncryptedCookie("TescoGroupPhone", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("TescoGroupSMS"))
                    Helper.SetTripleDESEncryptedCookie("TescoGroupSMS", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("PartnerMail"))
                    Helper.SetTripleDESEncryptedCookie("PartnerMail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("PartnerEmail"))
                    Helper.SetTripleDESEncryptedCookie("PartnerEmail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("PartnerPhone"))
                    Helper.SetTripleDESEncryptedCookie("PartnerPhone", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("PartnerSMS"))
                    Helper.SetTripleDESEncryptedCookie("PartnerSMS", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("ResearchMail"))
                    Helper.SetTripleDESEncryptedCookie("ResearchMail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("ResearchEmail"))
                    Helper.SetTripleDESEncryptedCookie("ResearchEmail", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("ResearchPhone"))
                    Helper.SetTripleDESEncryptedCookie("ResearchPhone", "Y");

                if (hdnSelectedChk.Value.ToString().Contains("ResearchSMS"))
                    Helper.SetTripleDESEncryptedCookie("ResearchSMS", "Y");
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
                Logger.Write("Start of ContactPreferencesU DeleteCookiesOfCurrentPage()", "General");

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PromotionCode")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PromotionCode");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupMail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoGroupMail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupEmail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoGroupEmail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupPhone")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoGroupPhone");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoGroupSMS")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoGroupSMS");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerMail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PartnerMail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerEmail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PartnerEmail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerPhone")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PartnerPhone");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PartnerSMS")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("PartnerSMS");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchMail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("ResearchMail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchEmail")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("ResearchEmail");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchPhone")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("ResearchPhone");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ResearchSMS")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("ResearchSMS");
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
            Response.Redirect("~/Kiosk/ClubcardCharter1.aspx?page=ContactPreferences", false);
        }
    }
}