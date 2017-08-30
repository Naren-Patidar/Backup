using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.NGCUtilityService;
using System.Xml;
using System.Data;
using Resources;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    /// <summary>
    /// this page captures Contact details from the customer
    /// Author Nagaraju T
    /// </summary>
    public partial class WhatIsYourContactDetails : BaseUIPage
    {
        UtilityServiceClient utilityClient = null;
        DataSet dsAddressList = null;
        private string sErrormsg = string.Empty;
        private string showContactControls = string.Empty;
        string resultXml = string.Empty;
        string errorXml = string.Empty;
        XmlDocument resulDoc = null;
        string resultxml = string.Empty;
        int rowCount = 0;

        private string ctrlID = string.Empty;
        private string resID = string.Empty;
        private string imgID = string.Empty;

        public string resourceStr = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Write("Start of WhatIsYourContactDetails Page_Load()", "General");
            //showContactControls = ConfigurationManager.AppSettings["ShowContactDetailsControls"].ToString();
            showContactControls = ConfigurationReader.GetStringConfigKeyToUpper("ShowContactDetailsControls");
            lblCurrentField.Value = showContactControls;
            try
            {
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
                    SetMaxLenth();
                    SetCookieValues();
                }
                SetResourceStr();
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourContactDetails.aspx.cs:Page_Load():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        private void SetCookieValues()
        {
            Logger.Write("Start of WhatIsYourContactDetails SetCookieValues()", "General");
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAddress")))
            {
                if (txtEmailId != null)
                {
                    txtEmailId.Text = Helper.GetTripleDESEncryptedCookieValue("EmailAddress").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EveningNumber")))
            {
                if (txtEveningNo != null)
                {
                    txtEveningNo.Text = Helper.GetTripleDESEncryptedCookieValue("EveningNumber").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MobileNumber")))
            {
                if (txtMobileNo != null)
                {
                    txtMobileNo.Text = Helper.GetTripleDESEncryptedCookieValue("MobileNumber").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber")))
            {
                if (txtDayTimeNo != null)
                {
                    txtDayTimeNo.Text = Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber").ToString().Trim();
                }
            }

            if (Request.QueryString["ctrlID"] != null)
            {
                hdnErrorCtrl.Value = Request.QueryString["ctrlID"].ToString();
            }
            if (Request.QueryString["Page"] != null && Request.QueryString["Page"] == "Confirm")
            {
                Helper.SetTripleDESEncryptedCookie("EmailAndPhone", "EmailAndPhone");
                hdnErrorCtrl.Value = Request.QueryString["Page"].ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")))
            {
                hdnErrorCtrl.Value = "Confirm";
            }
        }
        /// <summary>
        /// set max length for controls
        /// </summary>
        private void SetMaxLenth()
        {
            Logger.Write("Start of WhatIsYourContactDetails SetMaxLenth()", "General");
            if (txtEmailId != null)
            {
                if (CommonClassForJoin.EmailMaxLength > 0)
                {
                    txtEmailId.MaxLength = CommonClassForJoin.EmailMaxLength;
                }
                else
                {
                    txtEmailId.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("EmailIdMaxLength")); 
                }
            }
            if (txtMobileNo != null)
            {
                if (CommonClassForJoin.MobileNumberMaxLength > 0)
                {
                    txtMobileNo.MaxLength = CommonClassForJoin.MobileNumberMaxLength;
                }
                else
                {
                    txtMobileNo.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("MobileNoMaxLength")); 
                }
            }
            if (txtEveningNo != null)
            {
                if (CommonClassForJoin.EveningNumberMaxLength > 0)
                {
                    txtEveningNo.MaxLength = CommonClassForJoin.EveningNumberMaxLength;
                }
                else
                {
                    txtEveningNo.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("EveningNoMaxLength")); 
                }
            }
            if (txtDayTimeNo != null)
            {
                if (CommonClassForJoin.DaytimeNumberMaxLength > 0)
                {
                    txtDayTimeNo.MaxLength = CommonClassForJoin.DaytimeNumberMaxLength;
                }
                else
                {
                    txtDayTimeNo.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("DayTimeNoMaxLength")); 
                }
            }
            if (CommonClassForJoin.EmailRequired == true)
            {
                lblEmailOption.Visible = false;
            }
            else
            {
                lblEmailOption.Visible = true;
            }
            if (CommonClassForJoin.MobileNumberRequired == true)
            {
                lblMobileOption.Visible = false;
            }
            else
            {
                lblMobileOption.Visible = true;
            }
            if (CommonClassForJoin.EveningNumberRequired == true)
            {
                lblEvNOption.Visible = false;
            }
            else
            {
                lblEvNOption.Visible = true;
            }
            if (CommonClassForJoin.DaytimeNumberRequired == true)
            {
                lblDNOption.Visible = false;
            }
            else
            {
                lblDNOption.Visible = true;
            }
        }

        private void SetCookies()
        {
            Logger.Write("Start of WhatIsYourContactDetails SetCookies()", "General");
            if (txtEmailId != null)
            {
                Helper.SetTripleDESEncryptedCookie("EmailAddress", txtEmailId.Text.ToString().Trim());
            }
            
            if (txtEveningNo != null)
            {
                Helper.SetTripleDESEncryptedCookie("EveningNumber", txtEveningNo.Text.Trim());
            }
            if (txtMobileNo != null)
            {
                Helper.SetTripleDESEncryptedCookie("MobileNumber", txtMobileNo.Text.Trim());
            }
            if (txtDayTimeNo != null)
            {
                Helper.SetTripleDESEncryptedCookie("DaytimeNumber", txtDayTimeNo.Text.Trim());
            }
            
        }

        private bool ValiateControlsData()
        {
            Logger.Write("Start of WhatIsYourContactDetails ValiateControlsData()", "General");
            SetCookies();
            string[] cList = showContactControls.Split(',');
            //string phoneNumberValidation = ConfigurationManager.AppSettings["PhoneNumberValidation"].ToString();
            string phoneNumberValidation = ConfigurationReader.GetStringConfigKeyToUpper("PhoneNumberValidation");
            for (int i = 0; i < cList.Length; i++)
            {
                if (cList[i] == "EMAIL")
                {
                    if (txtEmailId != null)
                    {
                        if (txtEmailId.Text.Trim() == string.Empty && CommonClassForJoin.EmailRequired ==true)
                        {
                            ctrlID = "Email";
                            resID = "EmailErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;

                        }
                        if (txtEmailId.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.EmailRegExp))
                        {
                            string regAddressLine1 = CommonClassForJoin.EmailRegExp;
                            if (!Helper.IsRegexMatch(txtEmailId.Text.Trim(), regAddressLine1, false, false))
                            {
                                ctrlID = "Email";
                                resID = "EmailErroMsg";
                                imgID = "EmailBreadCrumb";
                                return false;
                            }
                        }

                    }
                }
                if (cList[i] == "MOBILENO")
                {
                    if (txtMobileNo != null)
                    {
                        if (txtMobileNo.Text.Trim() == string.Empty && CommonClassForJoin.MobileNumberRequired ==true)
                        {
                            ctrlID = "MobileNumber";
                            resID = "MPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;

                        }
                        if (txtMobileNo.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.PhoneRegExp))
                        {
                            string regAddressLine1 = CommonClassForJoin.PhoneRegExp;
                            if (!Helper.IsRegexMatch(txtMobileNo.Text.Trim(), regAddressLine1, false, false))
                            {
                                ctrlID = "MobileNumber";
                                resID = "MPhoneErroMsg";
                                imgID = "EmailBreadCrumb";
                                return false;
                            }

                        }
                        if (txtMobileNo.Text.Trim() != string.Empty && (CommonClassForJoin.MobileNumberMinLength > 0 && txtMobileNo.Text.Trim().Length < CommonClassForJoin.MobileNumberMinLength))
                        {
                            ctrlID = "MobileNumber";
                            resID = "MPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;
                        }
                        if (phoneNumberValidation == "TRUE")
                        {
                            if (txtMobileNo.Text.Trim() != string.Empty)
                            {
                                string mobilePhoneFormat = CommonClassForJoin.MobilePhoneFormat;
                                if (!string.IsNullOrEmpty(CommonClassForJoin.MobilePhoneFormat))
                                {
                                    if (CommonClassForJoin.MobilePhoneFormat.ToString().Contains(','))
                                    {
                                        string[] mobPrefixes = CommonClassForJoin.MobilePhoneFormat.ToString().Split(',');
                                        bool flgMobPrefix = false;
                                        for (int j = 0; j < mobPrefixes.Length; j++)
                                        {
                                            if (txtMobileNo.Text.Trim().Substring(0, mobPrefixes[j].Trim().Length) == mobPrefixes[j].ToString())
                                            {
                                                flgMobPrefix = true;
                                                break;
                                            }
                                            if (!flgMobPrefix)
                                            {
                                                ctrlID = "MobileNumber";
                                                resID = "MPhoneErroMsg";
                                                imgID = "EmailBreadCrumb";
                                                return false;
                                            }
                                        }
                                    }
                                    else if (txtMobileNo.Text.Trim().Substring(0, mobilePhoneFormat.Trim().Length) != mobilePhoneFormat)
                                    {
                                        ctrlID = "MobileNumber";
                                        resID = "MPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                    else if ((!string.IsNullOrEmpty(mobilePhoneFormat))
                                            && txtMobileNo.Text.Trim().Length < Convert.ToInt16(mobilePhoneFormat.Trim()))
                                    {
                                        ctrlID = "MobileNumber";
                                        resID = "MPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                }
                            }

                        }
                    }
                }
                if (cList[i] == "EVENINGNO")
                {
                    if (txtEveningNo != null)
                    {
                        if (txtEveningNo.Text.Trim() == string.Empty && CommonClassForJoin.EveningNumberRequired ==true)
                        {
                            ctrlID = "EveningNumber";
                            resID = "EvnPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;
                        }
                        if (txtEveningNo.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.PhoneRegExp))
                        {
                            string regAddressLine1 = CommonClassForJoin.PhoneRegExp;
                            if (!Helper.IsRegexMatch(txtEveningNo.Text.Trim(), regAddressLine1, false, false))
                            {
                                ctrlID = "EveningNumber";
                                resID = "EvnPhoneErroMsg";
                                imgID = "EmailBreadCrumb";
                                return false;
                            }
                        }
                        if (txtEveningNo.Text.Trim() != string.Empty && (CommonClassForJoin.EveningNumberMinLength > 0 && txtEveningNo.Text.Trim().Length < CommonClassForJoin.EveningNumberMinLength))
                        {
                            ctrlID = "EveningNumber";
                            resID = "EvnPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;
                        }
                        if (phoneNumberValidation == "TRUE")
                        {
                            if (txtEveningNo.Text.Trim() != string.Empty)
                            {
                                string mobilePhoneFormat = CommonClassForJoin.DaytimePhoneFormat;
                                if (!string.IsNullOrEmpty(mobilePhoneFormat))
                                {
                                    if (mobilePhoneFormat.ToString().Contains(','))
                                    {
                                        string[] mobPrefixes = mobilePhoneFormat.ToString().Split(',');
                                        bool flgMobPrefix = false;
                                        for (int j = 0; j < mobPrefixes.Length; j++)
                                        {
                                            if (txtEveningNo.Text.Trim().Substring(0, mobPrefixes[j].Trim().Length) == mobPrefixes[j].ToString())
                                            {
                                                flgMobPrefix = true;
                                                break;
                                            }
                                            if (!flgMobPrefix)
                                            {
                                                ctrlID = "EveningNumber";
                                                resID = "EvnPhoneErroMsg";
                                                imgID = "EmailBreadCrumb";
                                                return false;
                                            }

                                        }

                                    }
                                    else if (txtEveningNo.Text.Trim().Substring(0, mobilePhoneFormat.Trim().Length) != mobilePhoneFormat)
                                    {
                                        ctrlID = "EveningNumber";
                                        resID = "EvnPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                    else if ((!string.IsNullOrEmpty(mobilePhoneFormat))
                                            && txtEveningNo.Text.Trim().Length < Convert.ToInt16(mobilePhoneFormat.Trim()))
                                    {
                                        ctrlID = "EveningNumber";
                                        resID = "EvnPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (cList[i] == "DAYTIMENO")
                {
                    if (txtDayTimeNo != null)
                    {
                        if (txtDayTimeNo.Text.Trim() == string.Empty && CommonClassForJoin.DaytimeNumberRequired==true)
                        {
                            ctrlID = "DayTimeNumber";
                            resID = "DayPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;

                        }
                        if (txtDayTimeNo.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.PhoneRegExp))
                        {
                            string regAddressLine1 = CommonClassForJoin.PhoneRegExp;
                            if (!Helper.IsRegexMatch(txtDayTimeNo.Text.Trim(), regAddressLine1, false, false))
                            {
                                ctrlID = "DayTimeNumber";
                                resID = "DayPhoneErroMsg";
                                imgID = "EmailBreadCrumb";
                                return false;
                            }
                        }
                        if (txtDayTimeNo.Text.Trim() != string.Empty && (CommonClassForJoin.DaytimeNumberMinLength > 0 && txtDayTimeNo.Text.Trim().Length < CommonClassForJoin.DaytimeNumberMinLength))
                        {
                            ctrlID = "DayTimeNumber";
                            resID = "DayPhoneErroMsg";
                            imgID = "EmailBreadCrumb";
                            return false;
                        }
                        if (phoneNumberValidation == "TRUE")
                        {
                            if (txtDayTimeNo.Text.Trim() != string.Empty)
                            {
                                string mobilePhoneFormat = CommonClassForJoin.DaytimePhoneFormat;
                                if (!string.IsNullOrEmpty(mobilePhoneFormat))
                                {
                                    if (mobilePhoneFormat.ToString().Contains(','))
                                    {
                                        string[] mobPrefixes = mobilePhoneFormat.ToString().Split(',');
                                        bool flgMobPrefix = false;
                                        for (int j = 0; j < mobPrefixes.Length; j++)
                                        {
                                            if (txtDayTimeNo.Text.Trim().Substring(0, mobPrefixes[j].Trim().Length) == mobPrefixes[j].ToString())
                                            {
                                                flgMobPrefix = true;
                                                break;
                                            }
                                            if (!flgMobPrefix)
                                            {
                                                ctrlID = "DayTimeNumber";
                                                resID = "DayPhoneErroMsg";
                                                imgID = "EmailBreadCrumb";
                                                return false;
                                            }
                                        }
                                    }
                                    else if (txtDayTimeNo.Text.Trim().Substring(0, mobilePhoneFormat.Trim().Length) != mobilePhoneFormat)
                                    {
                                        ctrlID = "DayTimeNumber";
                                        resID = "DayPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                    else if ((!string.IsNullOrEmpty(mobilePhoneFormat))
                                            && txtDayTimeNo.Text.Trim().Length < Convert.ToInt16(mobilePhoneFormat.Trim()))
                                    {
                                        ctrlID = "DayTimeNumber";
                                        resID = "DayPhoneErroMsg";
                                        imgID = "EmailBreadCrumb";
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void SetResourceStr()
        {
            resourceStr = Resources.GlobalResources.EmailTitle + "," + Resources.GlobalResources.PhoneNoTit + "," + Resources.GlobalResources.EvNoTit + "," + Resources.GlobalResources.DayTimeNoTit;
        }

        protected void Confirm_ContactClick(object sender, EventArgs e)
        {
            Logger.Write("Start of WhatIsYourContactDetails Confirm_ContactClick()", "General");
            try
            {
                Helper.Net35BasicAuthentication();
                string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKey("WhatIsYourContactDetails"));
                if (!ValiateControlsData())
                {
                    SetCookies();
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourContactDetails&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                    Response.Redirect(qstring, false);
                }
                else
                {
                    SetCookies();
                    bool isProfinityCheckNeeded = CommonClassForJoin.IsProfinityCheckNeeded;
                   
                    if (isProfinityCheckNeeded == true)
                    {
                        string sProfaneValues = string.Empty;
                        if (txtEmailId != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("EMAILADDRESS"))
                        {
                            sProfaneValues = txtEmailId.Text.Trim();
                        }

                        utilityClient = new UtilityServiceClient();
                        if (utilityClient.ProfanityCheck(out errorXml, out resultxml, out rowCount, sProfaneValues))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultxml);
                            DataSet dsProfanity = new DataSet();
                            dsProfanity.ReadXml(new XmlNodeReader(resulDoc));

                            if (dsProfanity.Tables["ProfanityCheck"].Rows[0].ItemArray[0].ToString().Trim() == "0")
                            {
                                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")))
                                {
                                    Helper.DeleteTripleDESEncryptedCookie("EmailAndPhone");
                                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                                }
                                else
                                {
                                    Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                                }
                            }
                             else
                            {
                                ctrlID = "Email";
                                resID = "ProfanityMsg";
                                imgID = "EmailBreadCrumb";
                                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourContactDetails&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                                Response.Redirect(qstring, false);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")))
                        {
                            Helper.DeleteTripleDESEncryptedCookie("EmailAndPhone");
                            Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourContactDetails.aspx.cs:Confirm_ContactClick():ProfanityCheck Server down:" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
            finally
            {
                if (utilityClient != null)
                {
                    if (utilityClient.State == CommunicationState.Faulted)
                    {
                        utilityClient.Abort();
                    }
                    else if (utilityClient.State != CommunicationState.Closed)
                    {
                        utilityClient.Close();
                    }
                }
            }
        }

        protected void btnBack_click(object sender, EventArgs e)
        {
            SetCookies();
            string strNextPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKey("WhatIsYourContactDetails"));
            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx?Page=ContactDetails", false);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }
    }
}