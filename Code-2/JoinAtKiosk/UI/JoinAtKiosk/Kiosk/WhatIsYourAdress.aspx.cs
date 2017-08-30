using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Resources;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.NGCUtilityService;
using System.Xml;
using System.ServiceModel;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    /// <summary>
    /// this page captures Address details from the customer
    /// Author Nagaraju T
    /// </summary>
    public partial class WhatIsYourAdress : BaseUIPage
    {
        UtilityServiceClient utilityClient = null;
        DataSet dsAddressList = null;
        private string sErrormsg = string.Empty;
        private string showAddressControls = string.Empty;
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
            Logger.Write("Start of WhatIsYourAdress Page_Load()", "General");
            //showAddressControls = ConfigurationManager.AppSettings["ShowAddressControls"].ToString();
            showAddressControls = ConfigurationReader.GetStringConfigKeyToUpper("ShowAddressControls");
            lblCurrentField.Value = showAddressControls;
            hdnCountryCode.Value = ConfigurationReader.GetStringConfigKeyToUpper("CountryCode");
            int cookieCount = Request.Cookies.Count;
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
                    //ListControlCollections();
                    SetMaxLenth();
                    SetCookieValues();
                }
                SetResourceStr();
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourAddress.aspx.cs:Page_Load():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }

        }
        #region Private methods

        #region Martini PAF service is used

        private bool GetAddressesByPostcode(string postCode)
        {
            bool boolResult = true;
            LocatorSvcSDAClient client = null;
            string addresses = string.Empty;

            try
            {
                Logger.Write("Start of WhatIsYourAdress GetAddressesByPostcode()", "General");
                client = new LocatorSvcSDAClient();
                dsAddressList = new DataSet();

                addresses = client.FindAddressLite(postCode, null, null);

                if (addresses != string.Empty)
                {
                    XmlDocument xmlAddressList = new XmlDocument();
                    xmlAddressList.LoadXml(addresses);
                    dsAddressList.ReadXml(new XmlNodeReader(xmlAddressList));
                }
                else
                {
                    boolResult = false;
                }

                return boolResult;
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourAddress.aspx.cs:GetAddressesByPostcode():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
            finally
            {
                if (client != null)
                {
                    if (client.State == CommunicationState.Faulted)
                    {
                        client.Abort();
                    }
                    else if (client.State != CommunicationState.Closed)
                    {
                        client.Close();
                    }
                }
            }
            return boolResult;
        }

        #endregion

        private bool ValiateControlsData()
        {
            Logger.Write("Start of WhatIsYourAdress ValiateControlsData()", "General");
            SetCookies();
            //string cshowList = ConfigurationManager.AppSettings["ShowAddressControls"].ToString();
            string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowAddressControls");
            imgID = "AddressBreadCrumb";
            string country = ConfigurationReader.GetStringConfigKeyToUpper("Country");
            if (country == "UK")
            {
                return ValidateDataUK(cshowList);
            }
            else if (country == "GROUP")
            {
                return ValidateDataGroup(cshowList);
            }
            return true;
        }

        private bool ValidateDataUK(string cshowList)
        {
            string[] cList = cshowList.Split(',');
            for (int i = 0; i < cList.Length; i++)
            {
                if (cList[i] == "UKPOSTCODE")
                {
                    if (txtUKPostCode != null)
                    {
                        string regPostCode = string.Empty;
                        string regPostCode1 = string.Empty;
                        Helper.SetTripleDESEncryptedCookie("PostCode", txtUKPostCode.Text);
                        hdnPostCode.Value = txtUKPostCode.Text;
                        string sPOstCode = txtUKPostCode.Text.Trim();
                        if (txtUKPostCode.Text.Trim() == string.Empty && CommonClassForJoin.PostcodeReq == true)
                        {
                            ctrlID = "Postcode";
                            resID = "InvalidPostcode";
                            return false;
                        }
                        if ((CommonClassForJoin.PostcodeRegExp1 != null || !string.IsNullOrEmpty(CommonClassForJoin.PostcodeRegExp1)) || (CommonClassForJoin.PostcodeRegExp2 != null || !string.IsNullOrEmpty(CommonClassForJoin.PostcodeRegExp2)))
                        {
                            regPostCode = CommonClassForJoin.PostcodeRegExp1;//Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp1");// @"^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
                            regPostCode1 = CommonClassForJoin.PostcodeRegExp2; //Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp2");//@"^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}$";
                        }
                        if (CommonClassForJoin.PostcodeMinLength > 0 && txtUKPostCode.Text.Trim().Length < CommonClassForJoin.PostcodeMinLength)
                        {
                            ctrlID = "Postcode";
                            resID = "InvalidPostcode";
                            return false;
                        }

                        //PostCode
                        if (!Helper.IsRegexMatch(txtUKPostCode.Text.Trim(), regPostCode, false, false)
                        && !Helper.IsRegexMatch(txtUKPostCode.Text.Trim(), regPostCode1, false, false))
                        {
                            ctrlID = "Postcode";
                            resID = "InvalidPostcode";
                            imgID = "AddressBreadCrumb";
                            return false;
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE1")
                {
                    if (txtAddressLine1 != null)
                    {
                        if (txtAddressLine1.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine1Req == true)
                        {
                            ctrlID = "AddressLine1";
                            resID = "AddressLine1Error";
                            return false;
                        }
                        if (txtAddressLine1.Text.Trim() != string.Empty)
                        {
                            if (txtAddressLine1.Text.Substring(0, 1) == "<")
                            {
                                ctrlID = "AddressLine1";
                                resID = "AddressLine1Error";
                                return false;
                            }
                            if (CommonClassForJoin.AddressLine1MinLength > 0 && txtAddressLine1.Text.Trim().Length < CommonClassForJoin.AddressLine1MinLength)
                            {
                                ctrlID = "AddressLine1";
                                resID = "AddressLine1Error";
                                return false;
                            }
                        }

                    }
                }
                if (cList[i] == "ADDRESSLINE2")
                {
                    if (txtAddressLine2 != null)
                    {
                        if (txtAddressLine2.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine2Req == true) // || ((txtAddressLine2.Text.Trim() != string.Empty && txtAddressLine2.Text.Substring(0, 1) == "<")))
                        {
                            ctrlID = "AddressLine2";
                            resID = "AddressLine2Error";
                            return false;
                        }
                        if (txtAddressLine2.Text.Trim() != string.Empty)
                        {
                            if (txtAddressLine2.Text.Substring(0, 1) == "<")
                            {
                                ctrlID = "AddressLine2";
                                resID = "AddressLine2Error";
                                return false;
                            }
                            if (CommonClassForJoin.AddressLine2MinLength > 0 && txtAddressLine2.Text.Trim().Length < CommonClassForJoin.AddressLine2MinLength)
                            {
                                ctrlID = "AddressLine2";
                                resID = "AddressLine2Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE3")
                {
                    if (txtAddressLine3 != null)
                    {
                        if (CommonClassForJoin.AddressLine3Req == true)
                        {
                            if (txtAddressLine3.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine3Req == true)// || ((txtAddressLine3.Text.Trim() != string.Empty && txtAddressLine3.Text.Substring(0, 1) == "<")))
                            {
                                ctrlID = "AddressLine3";
                                resID = "AddressLine3Error";
                                return false;
                            }
                            if (txtAddressLine3.Text.Trim() != string.Empty)
                            {
                                if (txtAddressLine3.Text.Substring(0, 1) == "<")
                                {
                                    ctrlID = "AddressLine3";
                                    resID = "AddressLine3Error";
                                    return false;
                                }
                                if (CommonClassForJoin.AddressLine3MinLength>0 && txtAddressLine3.Text.Trim().Length < CommonClassForJoin.AddressLine3MinLength)
                                {
                                    ctrlID = "AddressLine3";
                                    resID = "AddressLine3Error";
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE4")
                {
                    if (txtAddressLine4 != null)
                    {
                        if (txtAddressLine4.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine4Req == true)//  || ((txtAddressLine4.Text.Trim() != string.Empty && txtAddressLine4.Text.Substring(0, 1) == "<")))
                        {
                            ctrlID = "AddressLine4";
                            resID = "AddressLine4Error";
                            return false;
                        }
                        if (txtAddressLine4.Text.Trim() != string.Empty)
                        {
                            if(txtAddressLine4.Text.Substring(0, 1) == "<")
                            {
                                ctrlID = "AddressLine4";
                                resID = "AddressLine4Error";
                                return false;
                            }
                            if (CommonClassForJoin.AddressLine4MinLength > 0 && txtAddressLine4.Text.Trim().Length < CommonClassForJoin.AddressLine4MinLength)
                            {
                                ctrlID = "AddressLine4";
                                resID = "AddressLine4Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE5")
                {
                    if (txtAddressLine5 != null)
                    {
                        if (txtAddressLine5.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine4Req == true)//  || ((txtAddressLine5.Text.Trim() != string.Empty && txtAddressLine5.Text.Substring(0, 1) == "<")))
                        {
                            ctrlID = "AddressLine5";
                            resID = "AddressLine4Error";
                            return false;
                        }
                        if (txtAddressLine5.Text.Trim() != string.Empty)
                        {
                            if (txtAddressLine5.Text.Substring(0, 1) == "<")
                            {
                                ctrlID = "AddressLine5";
                                resID = "AddressLine4Error";
                                return false;
                            }
                            if (CommonClassForJoin.AddressLine5MinLength > 0 && txtAddressLine5.Text.Trim().Length < CommonClassForJoin.AddressLine5MinLength)
                            {
                                ctrlID = "AddressLine5";
                                resID = "AddressLine4Error";
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool ValidateDataGroup(string cshowList)
        {
            string[] cList = cshowList.Split(',');
            string regAddressLine = CommonClassForJoin.MailingAddressRegExp;
            for (int i = 0; i < cList.Length; i++)
            {
                if (cList[i] == "ADDRESSLINE1")
                {
                    if (txtAddressLine1 != null)
                    {
                        if (txtAddressLine1.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine1Req == true)
                        {
                            ctrlID = "AddressLine1";
                            resID = "AddressLine1Error";
                            return false;
                        }
                        if (txtAddressLine1.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.AddressLine1MinLength>0 && txtAddressLine1.Text.Trim().Length < CommonClassForJoin.AddressLine1MinLength)
                            {
                                ctrlID = "AddressLine1";
                                resID = "AddressLine1Error";
                                return false;
                            }
                        }
                        if (txtAddressLine1.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.MailingAddressRegExp))
                        {
                            
                            if (!Helper.IsRegexMatch(txtAddressLine1.Text.Trim(), regAddressLine, false, false))
                            {
                                ctrlID = "AddressLine1";
                                resID = "AddressLine1Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE2")
                {
                    if (txtAddressLine2 != null)
                    {
                        if (txtAddressLine2.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine2Req == true)
                        {
                            ctrlID = "AddressLine2";
                            resID = "AddressLine2Error";
                            return false;
                        }
                        if (txtAddressLine2.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.AddressLine2MinLength>0 && txtAddressLine2.Text.Trim().Length < CommonClassForJoin.AddressLine2MinLength)
                            {
                                ctrlID = "AddressLine2";
                                resID = "AddressLine2Error";
                                return false;
                            }
                        }
                        if (txtAddressLine2.Text.Trim() != string.Empty && !string.IsNullOrEmpty(regAddressLine))
                        {
                            if (!Helper.IsRegexMatch(txtAddressLine2.Text.Trim(), regAddressLine, false, false))
                            {
                                ctrlID = "AddressLine2";
                                resID = "AddressLine2Error";
                                return false;
                            }
                        }

                    }
                }
                if (cList[i] == "ADDRESSLINE3")
                {
                    if (txtAddressLine3 != null)
                    {
                        if (txtAddressLine3.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine3Req == true)
                        {
                            ctrlID = "AddressLine3";
                            resID = "AddressLine3Error";
                            return false;
                        }
                        if (txtAddressLine3.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.AddressLine3MinLength>0 && txtAddressLine3.Text.Trim().Length < CommonClassForJoin.AddressLine3MinLength)
                            {
                                ctrlID = "AddressLine3";
                                resID = "AddressLine3Error";
                                return false;
                            }
                        }
                        if (txtAddressLine3.Text.Trim() != string.Empty && !string.IsNullOrEmpty(regAddressLine))
                        {
                            if (!Helper.IsRegexMatch(txtAddressLine3.Text.Trim(), regAddressLine, false, false))
                            {
                                ctrlID = "AddressLine3";
                                resID = "AddressLine3Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE4")
                {
                    if (txtAddressLine4 != null)
                    {
                        if (txtAddressLine4.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine4Req == true)
                        {
                            ctrlID = "AddressLine4";
                            resID = "AddressLine4Error";
                            return false;
                        }
                        if (txtAddressLine4.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.AddressLine4MinLength > 0 && txtAddressLine4.Text.Trim().Length < CommonClassForJoin.AddressLine4MinLength)
                            {
                                ctrlID = "AddressLine4";
                                resID = "AddressLine4Error";
                                return false;
                            }
                        }
                        if (txtAddressLine4.Text.Trim() != string.Empty && !string.IsNullOrEmpty(regAddressLine))
                        {
                            if (!Helper.IsRegexMatch(txtAddressLine4.Text.Trim(), regAddressLine, false, false))
                            {
                                ctrlID = "AddressLine4";
                                resID = "AddressLine4Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "ADDRESSLINE5")
                {
                    if (txtAddressLine5 != null)
                    {
                        if (txtAddressLine5.Text.Trim() == string.Empty && CommonClassForJoin.AddressLine5Req == true)
                        {
                            ctrlID = "AddressLine5";
                            resID = "AddressLine5Error";
                            return false;
                        }
                        if (txtAddressLine5.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.AddressLine5MinLength>0 && txtAddressLine5.Text.Trim().Length < CommonClassForJoin.AddressLine5MinLength)
                            {
                                ctrlID = "AddressLine5";
                                resID = "AddressLine5Error";
                                return false;
                            }
                        }
                        if (txtAddressLine5.Text.Trim() != string.Empty && !string.IsNullOrEmpty(regAddressLine))
                        {
                            if (!Helper.IsRegexMatch(txtAddressLine5.Text.Trim(), regAddressLine, false, false))
                            {
                                ctrlID = "AddressLine5";
                                resID = "AddressLine5Error";
                                return false;
                            }
                        }
                    }
                }
                if (cList[i] == "GRPOSTCODE")
                {
                    if (txtGrPostcode != null)
                    {
                        if (txtGrPostcode.Text.Trim() == string.Empty && CommonClassForJoin.PostcodeReq == true)
                        {
                            ctrlID = "GrPostcode";
                            resID = "InvalidPostcode";
                            return false;
                        }
                        if (txtGrPostcode.Text.Trim() != string.Empty)
                        {
                            if (CommonClassForJoin.PostcodeMinLength>0 && txtGrPostcode.Text.Trim().Length < CommonClassForJoin.PostcodeMinLength)
                            {
                                ctrlID = "GrPostcode";
                                resID = "InvalidPostcode";
                                return false;
                            }
                        }
                        if (txtGrPostcode.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.PostcodeRegExp1))
                        {
                            if (!Helper.IsRegexMatch(txtGrPostcode.Text.Trim(), CommonClassForJoin.PostcodeRegExp1, false, false))
                            {
                                ctrlID = "GrPostcode";
                                resID = "InvalidPostcode";
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void SetCookies()
        {
            Logger.Write("Start of WhatIsYourAdress SetCookies()", "General");
            if (txtUKPostCode != null)
            {
                Helper.SetTripleDESEncryptedCookie("PostCode", txtUKPostCode.Text.ToString().Trim());
            }
            if (txtGrPostcode != null && showAddressControls.Contains("GRPOSTCODE"))
            {
                Helper.SetTripleDESEncryptedCookie("PostCode", txtGrPostcode.Text.ToString().Trim());
            }
            if (txtAddressLine1 != null)
            {
                Helper.SetTripleDESEncryptedCookie("MailingAddress1", txtAddressLine1.Text.Trim());
            }
            if (txtAddressLine2 != null)
            {
                Helper.SetTripleDESEncryptedCookie("MailingAddress2", txtAddressLine2.Text.Trim());
            }
            if (txtAddressLine3 != null)
            {
                Helper.SetTripleDESEncryptedCookie("MailingAddress3", txtAddressLine3.Text.Trim());
            }
            if (txtAddressLine4 != null)
            {
                Helper.SetTripleDESEncryptedCookie("MailingAddress4", txtAddressLine4.Text.Trim());
            }
            if (txtAddressLine5 != null)
            {
                Helper.SetTripleDESEncryptedCookie("MailingAddress5", txtAddressLine5.Text.Trim());
            }
        }

        private void SetCookieValues()
        {
            Logger.Write("Start of WhatIsYourAdress SetCookieValues()", "General");
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")))
            {
                if (txtUKPostCode != null)
                {
                    txtUKPostCode.Text = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString().Trim();
                }
                if (txtGrPostcode != null)
                {
                    txtGrPostcode.Text = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")))
            {
                if (txtAddressLine1 != null)
                {
                    txtAddressLine1.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress1").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")))
            {
                if (txtAddressLine2 != null)
                {
                    txtAddressLine2.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress2").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")) )
            {
                if (txtAddressLine3 != null)
                {
                    txtAddressLine3.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress3").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress4")) )
            {
                if (txtAddressLine4 != null)
                {
                    txtAddressLine4.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress4").ToString().Trim();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress5")))
            {
                if (txtAddressLine5 != null)
                {
                    txtAddressLine5.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress5").ToString().Trim();
                }
            }
            if (Request.QueryString["ErrorMsg"] != null)
            {
                hdnErrorMsg.Value = Request.QueryString["ErrorMsg"].ToString();
            }
            if (Request.QueryString["PostCode"] != null)
            {
                txtUKPostCode.Text = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString();
                hdnErrorMsg.Value = "AddressLine1";
            }
            if (Request.QueryString["InValidPostCode"] != null)
            {
                txtUKPostCode.Text = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString();
                hdnErrorMsg.Value = Request.QueryString["ctrlID"].ToString();
            }
            if (Request.QueryString["ctrlID"] != null)
            {
                hdnErrorMsg.Value = Request.QueryString["ctrlID"].ToString();
            }
            if (Request.QueryString["Page"] != null && Request.QueryString["Page"] == "Confirm")
            {
                Helper.SetTripleDESEncryptedCookie("PostCodeAndAddress", "PostCodeAndAddress");
                hdnErrorCtrl.Value = Request.QueryString["Page"].ToString();

            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")))
            {
                hdnErrorCtrl.Value = "Confirm";
            }
        }
        /// <summary>
        /// set max length for controls
        /// </summary>
        private void SetMaxLenth()
        {
            Logger.Write("Start of WhatIsYourAdress SetMaxLenth()", "General");
            if (txtUKPostCode != null)
            {
                if (CommonClassForJoin.PostcodeMaxLength > 0)
                {
                    txtUKPostCode.MaxLength = CommonClassForJoin.PostcodeMaxLength;
                }
                else
                {
                    txtUKPostCode.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("PostCodeMaxLength")); 
                }
                txtUKPostCode.Enabled = false;
            }
            if (txtAddressLine1 != null)
            {
                if (CommonClassForJoin.AddressLine1MaxLength > 0)
                {
                    txtAddressLine1.MaxLength = CommonClassForJoin.AddressLine1MaxLength;
                }
                else
                {
                    txtAddressLine1.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("AddressLine1MaxLength")); 
                }
                txtAddressLine1.Enabled = false;
            }
            if (txtAddressLine2 != null)
            {
                if (CommonClassForJoin.AddressLine2MaxLength > 0)
                {
                    txtAddressLine2.MaxLength = CommonClassForJoin.AddressLine2MaxLength;
                }
                else
                {
                    txtAddressLine2.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("AddressLine2MaxLength")); 
                }
                txtAddressLine2.Enabled = false;
            }
           
            if (txtAddressLine3 != null)
            {
                if (CommonClassForJoin.AddressLine3MaxLength > 0)
                {
                    txtAddressLine3.MaxLength = CommonClassForJoin.AddressLine3MaxLength;
                }
                else
                {
                    txtAddressLine3.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("AddressLine3MaxLength")); 
                }
                txtAddressLine3.Enabled = false;
            }
            if (txtAddressLine4 != null)
            {
                if (CommonClassForJoin.AddressLine4MaxLength > 0)
                {
                    txtAddressLine4.MaxLength = CommonClassForJoin.AddressLine4MaxLength;
                }
                else
                {
                    txtAddressLine4.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("AddressLine4MaxLength"));
                }
                txtAddressLine4.Enabled = false;
            }
            if (txtAddressLine5 != null)
            {
                if (CommonClassForJoin.AddressLine5MaxLength > 0)
                {
                    txtAddressLine5.MaxLength = CommonClassForJoin.AddressLine5MaxLength;
                }
                else
                {
                    txtAddressLine5.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("AddressLine5MaxLength"));
                }
                txtAddressLine5.Enabled = false;
            }
            if (txtGrPostcode != null)
            {
                if (CommonClassForJoin.PostcodeMaxLength > 0)
                {
                    txtGrPostcode.MaxLength = CommonClassForJoin.PostcodeMaxLength;
                }
                else
                {
                    txtGrPostcode.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("PostCodeMaxLength")); 
                }
                txtGrPostcode.Enabled = false;
            }
            if (CommonClassForJoin.PostcodeReq == true)
            {
                lblUKPOpt.Visible = false;
            }
            else
            {
                lblUKPOpt.Visible = true;
            }
            if (CommonClassForJoin.AddressLine1Req == true)
            {
                lblAd1Opt.Visible = false;
            }
            else
            {
                lblAd1Opt.Visible = true;
            }
            if (CommonClassForJoin.AddressLine2Req == true)
            {
                lblAd2Opt.Visible = false;
            }
            else
            {
                lblAd2Opt.Visible = true;
            }
            if (CommonClassForJoin.AddressLine3Req == true)
            {
                lblAd3Opt.Visible = false;
            }
            else
            {
                lblAd3Opt.Visible = true;
            }
            if (CommonClassForJoin.AddressLine4Req == true)
            {
                lblAd4Opt.Visible = false;
            }
            else
            {
                lblAd4Opt.Visible = true;
            }
            if (CommonClassForJoin.AddressLine5Req == true)
            {
                lblAd5Opt.Visible = false;
            }
            else
            {
                lblAd5Opt.Visible = true;
            }
            if (CommonClassForJoin.PostcodeReq == true)
            {
                lblAd6Opt.Visible = false;
            }
            else
            {
                lblAd6Opt.Visible = true;
            }
        }

        private void SetResourceStr()
        {
            resourceStr = Resources.GlobalResources.AddressLine1Tit + "," + Resources.GlobalResources.AddressLine2Tit + "," + Resources.GlobalResources.AddressLine3Tit + ","
                          + Resources.GlobalResources.AddressLine4Tit + "," + Resources.GlobalResources.AddressLine5Tit + "," + Resources.GlobalResources.AddressLine6Tit;
        }
      
        #region Configuration

        /// <summary>
        /// used to app reg exp, required to validate at javascript
        /// </summary>
        private void ListControlCollections()
        {
            ArrayList controlList = new ArrayList();
            AddControls(Page.Controls, controlList);
            //string cshowList = ConfigurationManager.AppSettings["ShowAddressControls"].ToString();
            string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowAddressControls");
            string[] cList = cshowList.Split(',');
            //hdnShowControls.Value = cshowList;
            string final = string.Empty;
            for (int i = 0; i < cList.Length; i++)
            {
                string namecont = string.Empty;

                if (cList[i] == "UKPOSTCODE")
                {
                    string postcodeRegExp1 = string.Empty;
                    string postcodeRegExp2 = string.Empty;
                    string postcodeRequired = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp1")))
                    {
                        postcodeRegExp1 = Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp1").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp2")))
                    {
                        postcodeRegExp2 = Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp2").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressPostCodeReq")))
                    {
                        postcodeRequired = Helper.GetTripleDESEncryptedCookieValue("AddressPostCodeReq").ToString();
                    }
                    hdnPostRegExpList.Value = cList[i].ToString() + ":" + postcodeRegExp1 + "," + postcodeRegExp2 + "," + postcodeRequired;
                    //namecont = cList[i].ToString() + ":" + Name1RegExp + "," + Name1Required;
                }
                if (cList[i] == "ADDRESSLINE1")
                {
                    string addressLine1Reg = string.Empty;
                    string addressLine1Req = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine1Req")))
                    {
                        addressLine1Req = Helper.GetTripleDESEncryptedCookieValue("AddressLine1Req").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine1RegExp")))
                    {
                        addressLine1Reg = Helper.GetTripleDESEncryptedCookieValue("AddressLine1RegExp").ToString();
                    }
                    namecont = cList[i].ToString() + ":" + addressLine1Reg + "," + addressLine1Req;
                }
                if (cList[i] == "ADDRESSLINE2")
                {
                    string addressLine2Reg = string.Empty;
                    string addressLine2Req = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine2Req")))
                    {
                        addressLine2Req = Helper.GetTripleDESEncryptedCookieValue("AddressLine2Req").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine2RegExp")))
                    {
                        addressLine2Reg = Helper.GetTripleDESEncryptedCookieValue("AddressLine2RegExp").ToString();
                    }
                    namecont = cList[i].ToString() + ":" + addressLine2Reg + "," + addressLine2Req;
                }
                if (cList[i] == "ADDRESSLINE3")
                {
                    string addressLine3Reg = string.Empty;
                    string addressLine3Req = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine3Req")))
                    {
                        addressLine3Req = Helper.GetTripleDESEncryptedCookieValue("AddressLine3Req").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine3RegExp")))
                    {
                        addressLine3Reg = Helper.GetTripleDESEncryptedCookieValue("AddressLine3RegExp").ToString();
                    }
                    namecont = cList[i].ToString() + ":" + addressLine3Reg + "," + addressLine3Req;
                }
                if (cList[i] == "ADDRESSLINE4")
                {
                    string addressLine4Reg = string.Empty;
                    string addressLine4Req = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine4Req")))
                    {
                        addressLine4Req = Helper.GetTripleDESEncryptedCookieValue("AddressLine4Req").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine4RegExp")))
                    {
                        addressLine4Reg = Helper.GetTripleDESEncryptedCookieValue("AddressLine4RegExp").ToString();
                    }
                    namecont = cList[i].ToString() + ":" + addressLine4Reg + "," + addressLine4Req;
                }
                if (cList[i] == "ADDRESSLINE5")
                {
                    string addressLine5Reg = string.Empty;
                    string addressLine5Req = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine5Req")))
                    {
                        addressLine5Req = Helper.GetTripleDESEncryptedCookieValue("AddressLine5Req").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("AddressLine5RegExp")))
                    {
                        addressLine5Reg = Helper.GetTripleDESEncryptedCookieValue("AddressLine5RegExp").ToString();
                    }
                    namecont = cList[i].ToString() + ":" + addressLine5Reg + "," + addressLine5Req;
                }
                if (namecont != string.Empty)
                {

                    final = final + "|" + namecont;
                }

            }
            hdnValidateString.Value = final;
        }

        public void AddControls(ControlCollection page, ArrayList controlList)
        {
            foreach (Control c in page)
            {
                if (c.ID != null)
                {
                    if (c.GetType().Name == "TextBox")
                    {
                        controlList.Add(c.ID);
                    }
                }

                if (c.HasControls())
                {
                    AddControls(c.Controls, controlList);
                }
            }
        }
        #endregion

        #endregion

        protected void Next_FindAddress(object sender, ImageClickEventArgs e)
        {
            Logger.Write("Start of WhatIsYourAdress Next_FindAddress()", "General");
            string qstring = string.Empty;
            try
            {
                if (txtUKPostCode != null)
                {
                    string regPostCode = string.Empty;
                    string regPostCode1 = string.Empty;
                    Helper.SetTripleDESEncryptedCookie("PostCode", txtUKPostCode.Text);
                    hdnPostCode.Value = txtUKPostCode.Text;
                    string sPOstCode = txtUKPostCode.Text.Trim();
                    if (txtUKPostCode.Text.Trim() == string.Empty && CommonClassForJoin.PostcodeReq == true)
                    {
                        ctrlID = "Postcode";
                        resID = "InvalidPostcode";
                        imgID = "AddressBreadCrumb";
                        qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID + "&InValidPostCode=" + sPOstCode;
                        Response.Redirect(qstring, false);
                    }
                    if ((CommonClassForJoin.PostcodeRegExp1 != null || !string.IsNullOrEmpty(CommonClassForJoin.PostcodeRegExp1)) || (CommonClassForJoin.PostcodeRegExp2 != null || !string.IsNullOrEmpty(CommonClassForJoin.PostcodeRegExp2)))
                    {
                        regPostCode = CommonClassForJoin.PostcodeRegExp1;//Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp1");// @"^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
                        regPostCode1 = CommonClassForJoin.PostcodeRegExp2; //Helper.GetTripleDESEncryptedCookieValue("PostcodeRegExp2");//@"^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}$";
                    }
                    if (CommonClassForJoin.PostcodeMinLength > 0 && txtUKPostCode.Text.Trim().Length < CommonClassForJoin.PostcodeMinLength)
                    {
                        ctrlID = "Postcode";
                        resID = "InvalidPostcode";
                        imgID = "AddressBreadCrumb";
                        qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID + "&InValidPostCode=" + sPOstCode;
                        Response.Redirect(qstring, false);
                    }

                    //PostCode
                    if (!Helper.IsRegexMatch(txtUKPostCode.Text.Trim(), regPostCode, false, false)
                    && !Helper.IsRegexMatch(txtUKPostCode.Text.Trim(), regPostCode1, false, false))
                    {
                        ctrlID = "Postcode";
                        resID = "InvalidPostcode";
                        imgID = "AddressBreadCrumb";
                        qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID + "&InValidPostCode=" + sPOstCode;
                        Response.Redirect(qstring, false);
                    }
                    else
                    {
                        if (GetAddressesByPostcode(txtUKPostCode.Text))
                        {
                            if (dsAddressList.Tables.Count > 0)
                            {
                                if (dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim() != string.Empty)
                                {
                                    if (dsAddressList.Tables[0].Rows[0]["SubBuilding"].ToString().Trim() != string.Empty)
                                    {
                                        if (dsAddressList.Tables[0].Rows[0]["BuildingNumber"].ToString().Trim() != string.Empty)
                                        {
                                            txtAddressLine2.Text = dsAddressList.Tables[0].Rows[0]["BuildingNumber"].ToString().Trim() + " "
                                                                   + dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                                        }
                                        else
                                        {
                                            txtAddressLine2.Text = dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        txtAddressLine2.Text = dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                                    }
                                    Helper.SetTripleDESEncryptedCookie("MailingAddress2", txtAddressLine2.Text);
                                }
                                if (dsAddressList.Tables[0].Rows[0]["Town"].ToString().Trim() != string.Empty)
                                {
                                    txtAddressLine3.Text = dsAddressList.Tables[0].Rows[0]["Town"].ToString().Trim();
                                    Helper.SetTripleDESEncryptedCookie("MailingAddress3", txtAddressLine3.Text);
                                }
                                hdnErrorMsg.Value = "AddressLine1";
                            }
                            else
                            {
                                ctrlID = "Postcode";
                                resID = "AddressNotFound";
                                imgID = "AddressBreadCrumb";
                                qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID + "&PostCode=" + txtUKPostCode.Text;
                                Response.Redirect(qstring, false);
                            }
                        }
                        else
                        {
                            Helper.SetTripleDESEncryptedCookie("PostCode", txtUKPostCode.Text);
                            hdnUkPostcode.Value = "true";
                            Helper.DeleteTripleDESEncryptedCookie("MailingAddress1");
                            Helper.DeleteTripleDESEncryptedCookie("MailingAddress2");
                            Helper.DeleteTripleDESEncryptedCookie("MailingAddress2");

                            ctrlID = "Postcode";
                            resID = "AddressNotFound";
                            imgID = "AddressBreadCrumb";
                            qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID + "&PostCode=" + txtUKPostCode.Text;
                            Response.Redirect(qstring, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("WhatIsYourAddress.aspx.cs:Next_FindAddress():" + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void Confirm_AddressClick(object sender, EventArgs e)
        {
            Logger.Write("Start of WhatIsYourAdress Confirm_AddressClick()", "General");
            Helper.Net35BasicAuthentication();
            string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKey("WhatIsYourAdress"));
            try
            {
                if (!ValiateControlsData())
                {
                    SetCookies();
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                    Response.Redirect(qstring, false);
                }
                else
                {
                    SetCookies();
                    bool isProfinityCheckNeeded = CommonClassForJoin.IsProfinityCheckNeeded;
                    if (isProfinityCheckNeeded == true)
                    {
                        string sProfaneValues = string.Empty;
                        if (txtUKPostCode != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSPOSTCODE"))
                        {
                            sProfaneValues = txtUKPostCode.Text.Trim();
                        }
                        if (txtAddressLine1 != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSLINE1"))
                        {
                            sProfaneValues = txtAddressLine1.Text.Trim();
                        }
                        if (txtAddressLine2 != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSLINE2"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtAddressLine2.Text.Trim();
                        }
                        if (txtAddressLine3 != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSLINE3"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtAddressLine3.Text.Trim();
                        }
                        if (txtAddressLine4 != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSLINE4"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtAddressLine4.Text.Trim();
                        }
                        if (txtAddressLine5 != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSLINE5"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtAddressLine5.Text.Trim();
                        }
                        if (txtGrPostcode != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("MAILINGADDRESSPOSTCODE"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtGrPostcode.Text.Trim();
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
                                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")))
                                {
                                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                                }
                                else
                                {
                                    string qstring = "~/Kiosk/" + strNextPageName + ".aspx";
                                    if (Request.QueryString["Page"] != null)
                                    {
                                        qstring = qstring + "?Page=" + Request.QueryString["Page"].ToString();
                                    }
                                    Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                                }
                            }
                            else
                            {
                                ctrlID = "AddressLine1";
                                resID = "ProfanityMsg";
                                imgID = "AddressBreadCrumb";
                                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourAdress&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                                Response.Redirect(qstring, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")))
                        {
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
                Logger.Write("WhatIsYourAddress.aspx.cs:Confirm_AddressClick():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
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

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }
        protected void btnBack_click(object sender, EventArgs e)
        {
            SetCookies();
            string strNextPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKey("WhatIsYourAdress"));
            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx?Page=Address", false);
        }
    }
}