using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class WhatIsYourDOBAndHHAges : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDOBAndHHAges Page_Load()", "General");
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
                    ListControlCollections();
                    hdnConfirmPg.Value = "N";

                    string strDtTxt, strMonthTxt, strYearText;
                    strDtTxt = Resources.GlobalResources.DateText;
                    strMonthTxt = Resources.GlobalResources.MonthText;
                    strYearText = Resources.GlobalResources.YearText;
                    hdnResources.Value = Resources.GlobalResources.DOBTitle + "," + Resources.GlobalResources.HHAgesTitle + "," + strDtTxt + "," +strMonthTxt +"," +strYearText;
                    hdnDateFormat.Value = ConfigurationReader.GetStringConfigKeyToUpper("DateFormat");
                    hdnDOBLimitInDays.Value = ConfigurationReader.GetStringConfigKey("DOBLimitInDays");
                    hdnDateRegExp.Value = ConfigurationReader.GetStringConfigKey("DOBRegExp");
                    hdnAgeRegExp.Value = ConfigurationReader.GetStringConfigKey("AgeRegExp");

                    if (hdnShowControls.Value.ToUpperInvariant().Contains("DATEOFBIRTH"))
                    {
                        string dateFormat = hdnDateFormat.Value;
                        if (dateFormat == "DMY")
                        {
                            txtDt1.Text = strDtTxt;
                            txtDt2.Text = strMonthTxt;
                            txtDt3.Text = strYearText;
                            txtDt1.MaxLength = 2;
                            txtDt2.MaxLength = 2;
                            txtDt3.MaxLength = 4;
                            pnldt3.Attributes.Add("class", "input92 paddingtop-10 input116white");

                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                        }
                        else if (dateFormat == "MDY")
                        {
                            txtDt1.Text = strMonthTxt;
                            txtDt2.Text = strDtTxt;
                            txtDt3.Text = strYearText;
                            txtDt1.MaxLength = 2;
                            txtDt2.MaxLength = 2;
                            txtDt3.MaxLength = 4;
                            pnldt3.Attributes.Add("class", "input92 paddingtop-10 input116white");
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                        }
                        else if (dateFormat == "YMD")
                        {
                            txtDt1.Text = strYearText;
                            txtDt2.Text = strMonthTxt;
                            txtDt3.Text = strDtTxt;
                            txtDt1.MaxLength = 4;
                            txtDt2.MaxLength = 2;
                            txtDt3.MaxLength = 2;
                            pnldt1.Attributes.Add("class", "input92 paddingtop-10 input116white");

                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }

                        }
                        else if (dateFormat == "DYM")
                        {
                            txtDt1.Text = strDtTxt;
                            txtDt2.Text = strYearText;
                            txtDt3.Text = strMonthTxt;
                            txtDt1.MaxLength = 2;
                            txtDt2.MaxLength = 4;
                            txtDt3.MaxLength = 2;
                            pnldt2.Attributes.Add("class", "input92 paddingtop-10 input116white");
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                        }
                        else if (dateFormat == "MYD")
                        {
                            txtDt1.Text = strMonthTxt;
                            txtDt2.Text = strYearText;
                            txtDt3.Text = strDtTxt;
                            txtDt1.MaxLength = 2;
                            txtDt2.MaxLength = 4;
                            txtDt3.MaxLength = 2;
                            pnldt2.Attributes.Add("class", "input92 paddingtop-10 input116white");

                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }
                        }
                        else if (dateFormat == "YDM")
                        {
                            txtDt1.Text = strYearText;
                            txtDt2.Text = strDtTxt;
                            txtDt3.Text = strMonthTxt;
                            txtDt1.MaxLength = 4;
                            txtDt2.MaxLength = 2;
                            txtDt3.MaxLength = 2;
                            pnldt1.Attributes.Add("class", "input92 paddingtop-10 input116white");
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                            {
                                txtDt1.Text = Helper.GetTripleDESEncryptedCookieValue("Year").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                            {
                                txtDt2.Text = Helper.GetTripleDESEncryptedCookieValue("Date").ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                            {
                                txtDt3.Text = Helper.GetTripleDESEncryptedCookieValue("Month").ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        pnlHHAges.CssClass = "focuspanelage curved";
                        pnlHHAges1.CssClass = "inputtext whitetext";
                        lblAge1.Attributes.Add("class", "smalltext1 whitetext");
                        lblAge2.Attributes.Add("class", "smalltext1 whitetext");
                        lblAge3.Attributes.Add("class", "smalltext1 whitetext");
                        lblAge4.Attributes.Add("class", "smalltext1 whitetext");
                        lblAge5.Attributes.Add("class", "smalltext1 whitetext");
                        divAge6.Attributes.Add("class", "inputtext whitetext");
                        txtAge1.Focus();
                    }

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")) && Request.QueryString["Age1"] == null)
                    {
                        txtAge1.Text = Helper.GetTripleDESEncryptedCookieValue("Age1").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")) && Request.QueryString["Age2"] == null)
                    {
                        txtAge2.Text = Helper.GetTripleDESEncryptedCookieValue("Age2").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")) && Request.QueryString["Age3"] == null)
                    {
                        txtAge3.Text = Helper.GetTripleDESEncryptedCookieValue("Age3").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")) && Request.QueryString["Age4"] == null)
                    {
                        txtAge4.Text = Helper.GetTripleDESEncryptedCookieValue("Age4").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")) && Request.QueryString["Age5"] == null)
                    {
                        txtAge5.Text = Helper.GetTripleDESEncryptedCookieValue("Age5").ToString().Trim();
                    }

                    //if (hdnShowControls.Value.ToUpper().Contains("DATEOFBIRTH") && hdnShowControls.Value.ToUpper().Contains("HHAGES"))
                    //{
                    //    pnlnexthouseages.Attributes.Add("class", "nexthouseages nexthouseagesfocus");
                    //}
                    //else
                    //{
                    //    pnlnexthouseages.Attributes.Add("class", "nexthouseages nextdietary");
                    //}

                    if (Request.QueryString["Page"] != null && Request.QueryString["Page"].ToString().ToUpperInvariant().Trim() == "CONFIRM")
                    {
                        Helper.SetTripleDESEncryptedCookie("DOBDietry", "DOBDietry");
                        //hdnConfirmPg.Value = "Y";
                    }
                    string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourDOBAndHHAges"));
                    if (strNextPageName != "WhatIsYourDietryPreferences" && !string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
                    {
                        hdnConfirmPg.Value = "Y";
                        divspacer.Attributes.Add("class", "buttonspacer3");
                        pnlnexthouseages.Attributes.Add("style", "display:none");
                        divsummary.Attributes.Add("style", "display:block");
                    }
                    ShowOptionalLabel();
                }
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourDOBAndHHAges.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DOBAndHHAgesBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void NextPage_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDOBAndHHAges NextPage_Click()", "General");
                DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                if (hdnErr.Value.Length > 0)
                {
                    if (hdnErr.Value.Contains("Dt"))
                        Response.Redirect("~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourDOBAndHHAges&ctrlID=Dt&resID=" + hdnErr.Value + "&imgID=DOBAndHHAgesBreadCrumb", false);
                    else
                    {
                        string ctrlID = hdnErr.Value.Substring(0, hdnErr.Value.IndexOf("|"));
                        string resID = hdnErr.Value.Substring(hdnErr.Value.IndexOf("|") + 1);
                        Response.Redirect("~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourDOBAndHHAges&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=DOBAndHHAgesBreadCrumb", false);
                    }
                }
                else
                {
                    string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourDOBAndHHAges"));
                    if (strNextPageName == "WhatIsYourDietryPreferences")
                    {
                        Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
                        {
                            Helper.DeleteTripleDESEncryptedCookie("DOBDietry");
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
                Logger.Write("WhatIsYourDOBAndHHAges.aspx.cs:NextPage_Click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DOBAndHHAgesBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }

        }

        protected void btnBack_click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDOBAndHHAges btnBack_click()", "General");
                DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourDOBAndHHAges"));
                Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx", false);
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourDOBAndHHAges.aspx.cs:btnBack_click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DOBAndHHAgesBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }

        /// <summary>
        /// used to app reg exp, required to validate at javascript
        /// </summary>
        private void ListControlCollections()
        {
            try
            {
                Logger.Write("Start of WhatIsYourDOBAndHHAges ListControlCollections()", "General");
                string controlString = string.Empty;
                string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowDOBandHHAgesControls");
                string[] cList = cshowList.ToString().Split(',');
                hdnShowControls.Value = cshowList;
                string final = string.Empty;
                for (int i = 0; i < cList.Length; i++)
                {
                    string ctrl = string.Empty;

                    if (cList[i].ToString().ToUpperInvariant() == "DATEOFBIRTH")
                    {
                        string DOBReq = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.DOBReq.ToString()))
                        {
                            DOBReq = CommonClassForJoin.DOBReq.ToString();
                        }
                        ctrl = cList[i].ToString() + ":," + DOBReq;
                    }
                    if (cList[i].ToString().ToUpperInvariant() == "HHAGES")
                    {
                        string HHAge1Req = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.HHAge1Req.ToString()))
                        {
                            HHAge1Req = CommonClassForJoin.HHAge1Req.ToString();
                        }
                        ctrl = "HHAge1:," + HHAge1Req + "|";

                        string HHAge2Req = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.HHAge2Req.ToString()))
                        {
                            HHAge2Req = CommonClassForJoin.HHAge2Req.ToString();
                        }
                        ctrl = ctrl + "HHAge2:," + HHAge2Req + "|";

                        string HHAge3Req = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.HHAge3Req.ToString()))
                        {
                            HHAge3Req = CommonClassForJoin.HHAge3Req.ToString();
                        }
                        ctrl = ctrl + "HHAge3:," + HHAge3Req + "|";

                        string HHAge4Req = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.HHAge4Req.ToString()))
                        {
                            HHAge4Req = CommonClassForJoin.HHAge4Req.ToString();
                        }
                        ctrl = ctrl + "HHAge4:," + HHAge4Req + "|";

                        string HHAge5Req = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.HHAge5Req.ToString()))
                        {
                            HHAge5Req = CommonClassForJoin.HHAge5Req.ToString();
                        }
                        ctrl = ctrl + "HHAge5:," + HHAge5Req + "|";

                    }

                    if (ctrl != string.Empty)
                    {
                        final = final + "|" + ctrl;
                    }
                }
                hdnControlList.Value = final;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void SetCookiesOfCurrentPage()
        {
            try
            {
                Logger.Write("Start of WhatIsYourDOBAndHHAges SetCookiesOfCurrentPage()", "General");
                if (hdnShowControls.Value.ToUpperInvariant().Contains("DATEOFBIRTH"))
                {
                    if (hdnDt.Value != "DDMMYYY")
                    {
                        string dateFormat = hdnDateFormat.Value;
                        if (dateFormat == "DMY")
                        {
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt3.Text.Trim());

                        }
                        else if (dateFormat == "MDY")
                        {
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt3.Text.Trim());
                        }
                        else if (dateFormat == "YMD")
                        {
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt3.Text.Trim());
                        }
                        else if (dateFormat == "DYM")
                        {
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt3.Text.Trim());
                        }
                        else if (dateFormat == "MYD")
                        {
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt3.Text.Trim());
                        }
                        else if (dateFormat == "YDM")
                        {
                            Helper.SetTripleDESEncryptedCookie("Year", txtDt1.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Date", txtDt2.Text.Trim());
                            Helper.SetTripleDESEncryptedCookie("Month", txtDt3.Text.Trim());
                        }

                        string dateSeparator = ConfigurationReader.GetStringConfigKey("DateSeparator");
                        Helper.SetTripleDESEncryptedCookie("DateOfBirth", txtDt1.Text.Trim() + dateSeparator + txtDt2.Text.Trim() + dateSeparator + txtDt3.Text.Trim());
                    }
                }

                if (hdnShowControls.Value.ToUpperInvariant().Contains("HHAGES"))
                {
                    Helper.SetTripleDESEncryptedCookie("Age1", txtAge1.Text.Trim());
                    Helper.SetTripleDESEncryptedCookie("Age2", txtAge2.Text.Trim());
                    Helper.SetTripleDESEncryptedCookie("Age3", txtAge3.Text.Trim());
                    Helper.SetTripleDESEncryptedCookie("Age4", txtAge4.Text.Trim());
                    Helper.SetTripleDESEncryptedCookie("Age5", txtAge5.Text.Trim());
                }
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
                Logger.Write("Start of WhatIsYourDOBAndHHAges DeleteCookiesOfCurrentPage()", "General");
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Date");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Month");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Year");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DateOfBirth")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DateOfBirth");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age1");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age2");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age3");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age4");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age5");
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void ShowOptionalLabel()
        {
            if (CommonClassForJoin.DOBReq == true)
            {
                lblDOBOpt.Visible = false;
            }
            else
            {
                lblDOBOpt.Visible = true;
            }
            if (CommonClassForJoin.HHAge1Req == true || CommonClassForJoin.HHAge2Req == true || CommonClassForJoin.HHAge3Req == true
                || CommonClassForJoin.HHAge4Req == true || CommonClassForJoin.HHAge5Req == true)
            {
                lblHHAgeOpt.Visible = false;
            }
            else
            {
                lblHHAgeOpt.Visible = true;
            }
        }
    }
}