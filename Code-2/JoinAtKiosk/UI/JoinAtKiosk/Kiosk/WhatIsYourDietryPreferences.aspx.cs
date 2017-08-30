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
    public partial class WhatIsYourDietryPreferences : BaseUIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDietryPreferences Page_Load()", "General");
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
                    hdnNoOfDietryPreferences.Value = ConfigurationReader.GetStringConfigKey("NoOfDietryPreferences");
                    hdnNoOfDietCheckBox.Value = ConfigurationReader.GetStringConfigKey("NoOfDietCheckBox");
                    hdnSelectedDietChk.Value = "";
                    hdnSelectedDietry.Value = "";

                    if (hdnNoOfDietryPreferences.Value == "0")
                    {
                        divDietryPreferencesLbl.Attributes.Add("style", "display:none");
                    }

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox1")))
                    {
                        lnkChk1.Attributes.Add("class", "input92 inputbluesquaretick");
                        hdnSelectedDietChk.Value = "lnkChk1";
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox2")))
                    {
                        lnkChk2.Attributes.Add("class", "input92 inputbluesquaretick");
                        hdnSelectedDietChk.Value = hdnSelectedDietChk.Value + "lnkChk2";
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SelectedDietryPref")))
                    {
                        string SelectedDietryPref = Helper.GetTripleDESEncryptedCookieValue("SelectedDietryPref").ToString();
                        hdnSelectedDietry.Value = SelectedDietryPref;
                        switch (SelectedDietryPref)
                        {
                            case "ID_DietryPrefOption1":
                                lnkDietary1.Attributes.Add("class", "dietary dietary1 selected");
                                break;
                            case "ID_DietryPrefOption2":
                                lnkDietary2.Attributes.Add("class", "dietary dietary2 selected");
                                break;
                            case "ID_DietryPrefOption3":
                                lnkDietary3.Attributes.Add("class", "dietary dietary3 selected");
                                break;
                            case "ID_DietryPrefOption4":
                                lnkDietary4.Attributes.Add("class", "dietary dietary4 selected");
                                break;
                            case "ID_DietryPrefOption5":
                                lnkDietary5.Attributes.Add("class", "dietary dietary5 selected");
                                break;
                        }
                    }

                    //if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPrefReq")) && Helper.GetTripleDESEncryptedCookieValue("DietryPrefReq").ToString().ToUpper() == "TRUE" && hdnSelectedDietry.Value == "" && hdnSelectedDietChk.Value == "")
                    //{
                    //    lnkNext.Attributes.Add("style", "display:none");
                    //    pnlNext.CssClass = "nextfromdietary";
                    //    lnkNext.CssClass = "nextfromdietary";
                    //}

                    if (Request.QueryString["Page"] != null && Request.QueryString["Page"].ToString().ToUpperInvariant().Trim() == "CONFIRM")
                    {
                        Helper.SetTripleDESEncryptedCookie("DOBDietry", "DOBDietry");
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
                    {
                        divspacer.Attributes.Add("class", "buttonspacer3");
                        divconfirm.Attributes.Add("style", "display:none");
                        divsummary.Attributes.Add("style", "display:block");
                    }
                    ShowOptionalLabel();
                }
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourDietryPreferences.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DietryPrefBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }

        }

        protected void NextPage_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDietryPreferences NextPage_Click()", "General");
                DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DOBDietry");
                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                }
                else
                {
                    string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourDietryPreferences"));
                    Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                }
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourDietryPreferences.aspx.cs:NextPage_Click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DietryPrefBreadCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        protected void btnBack_click(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourDietryPreferences btnBack_click()", "General");
                DeleteCookiesOfCurrentPage();
                SetCookiesOfCurrentPage();
                string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourDietryPreferences"));
                Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx", false);
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourDietryPreferences.aspx.cs:btnBack_click():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "DietryPrefBreadCrumb";
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
                Logger.Write("Start of WhatIsYourDietryPreferences ListControlCollections()", "General");
                string controlString = string.Empty;
                string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowDietryPrefControls");
                string[] cList = cshowList.ToString().Split(',');
                hdnShowControls.Value = cshowList;
                string final = string.Empty;
                for (int i = 0; i < cList.Length; i++)
                {
                    string ctrl = string.Empty;

                    if (cList[i].ToString().ToUpperInvariant() == "DIETRYPREFERENCES")
                    {
                        string DietryPrefReq = string.Empty;
                        if (!string.IsNullOrEmpty(CommonClassForJoin.DietryPrefReq.ToString()))
                        {
                            DietryPrefReq = CommonClassForJoin.DietryPrefReq.ToString();
                        }
                        ctrl = cList[i].ToString() + ":," + DietryPrefReq;
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
                Logger.Write("Start of WhatIsYourDietryPreferences SetCookiesOfCurrentPage()", "General");
                string prefName = "";
                string prefID = "";
                string prefIDNametmp = "";
                string[] prefIDName = null;
                if (hdnSelectedDietChk.Value.Contains("lnkChk1"))
                {
                    Helper.SetTripleDESEncryptedCookie("DietCheckBox1", "Y");
                    prefIDNametmp = ConfigurationReader.GetStringConfigKey("ID_DietCheckBox1");
                    if (prefIDNametmp != "0")
                    {
                        prefIDName = prefIDNametmp.ToString().Trim().Split('|').ToArray();
                        prefID = prefIDName[0];
                        prefName = prefIDName[1];
                    }
                }
                if (hdnSelectedDietChk.Value.Contains("lnkChk2"))
                {
                    Helper.SetTripleDESEncryptedCookie("DietCheckBox2", "Y");
                    prefIDNametmp = ConfigurationReader.GetStringConfigKey("ID_DietCheckBox2");
                    if (prefIDNametmp != "0")
                    {
                        if (prefID.Length > 0)
                        {
                            prefID = prefID + ",";
                            prefName = prefName + ",";
                        }
                        prefIDName = prefIDNametmp.ToString().Trim().Split('|').ToArray();
                        prefID = prefID + prefIDName[0];
                        prefName = prefName + prefIDName[1];
                    }
                }
                if (!string.IsNullOrEmpty(hdnSelectedDietry.Value))
                {
                    Helper.SetTripleDESEncryptedCookie("SelectedDietryPref", hdnSelectedDietry.Value);
                    prefIDNametmp = ConfigurationReader.GetStringConfigKey(hdnSelectedDietry.Value);
                    if (prefIDNametmp != "0")
                    {
                        if (prefID.Length > 0)
                        {
                            prefID = prefID + ",";
                            prefName = prefName + ",";
                        }
                        prefIDName = prefIDNametmp.ToString().Trim().Split('|').ToArray();
                        prefID = prefID + prefIDName[0];
                        prefName = prefName + prefIDName[1];
                    }
                    //else
                    //{
                    //    if (prefID.Length > 0)
                    //    {
                    //        prefID = prefID + ",NONE";
                    //    }
                    //    else
                    //    {
                    //        prefID = "NONE";
                    //    }
                    //}
                }
                if (prefID.Length > 0)
                {
                    Helper.SetTripleDESEncryptedCookie("DietryPreferencesID", prefID);
                    Helper.SetTripleDESEncryptedCookie("DietryPreferencesName", prefName);
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
                Logger.Write("Start of WhatIsYourDietryPreferences DeleteCookiesOfCurrentPage()", "General");
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox1")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DietCheckBox1");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietCheckBox2")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DietCheckBox2");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SelectedDietryPref")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("SelectedDietryPref");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesID")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DietryPreferencesID");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("DietryPreferencesName");
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void ShowOptionalLabel()
        {
            if (CommonClassForJoin.DietryPrefReq == true)
            {
                lblDietOpt.Visible = false;
            }
            else
            {
                lblDietOpt.Visible = true;
            }
           
        }
    }
}