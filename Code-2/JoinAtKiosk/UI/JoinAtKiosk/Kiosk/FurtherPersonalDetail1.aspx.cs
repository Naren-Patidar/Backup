using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.NGCUtilityService;
using System.Xml;
using System.Data;
using Resources;
using System.ServiceModel;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.CustomerService;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class FurtherPersonalDetail1 : BaseUIPage
    {
        UtilityServiceClient utilityClient = null;
        private string sRedirectLink = string.Empty;
        private string sPage = string.Empty;
        private bool sTimeFlag = false;
        private bool sCookieFlag = false;

        string resultXml = string.Empty;
        string errorXml = string.Empty;
        XmlDocument resulDoc = null;
        string resultxml = string.Empty;
        int rowCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of FurtherPersonalDetail1 Page_Load()", "General");
                if (string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserFirstName")))
                {
                    Response.Redirect("~/Kiosk/TimeOut.aspx", false);
                }
                else
                {
                    Helper.CheckAndResetCookie();
                }

                ControlMovement();
                if (!IsPostBack)
                {
                    Helper.GetAndLoadConfigurationDetails();
                    ListControlCollections();
                    SetMaxLenth();
                    SetBackCookieValue();
                }
            }
            catch (Exception exp)
            {
                Logger.Write("FurtherPersonalDetail1.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());

                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "LPSCrumb";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }


        /// <summary>
        /// delete cookie values if querry string is null 
        /// </summary>
        private void DeleteCookie()
        {
            Logger.Write("Start of FurtherPersonalDetail1 DeleteCookie()", "General");
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage")) && Request.QueryString["Page"] == null && sCookieFlag)
            {
                Helper.DeleteTripleDESEncryptedCookie("PreferredLanguage");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")) && Request.QueryString["Page"] == null && sCookieFlag)
            {
                Helper.DeleteTripleDESEncryptedCookie("PassportNumber");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")) && Request.QueryString["Page"] == null && sCookieFlag)
            {
                Helper.DeleteTripleDESEncryptedCookie("SSN");
            }
        }

        /// <summary>
        /// set cookie values if querry string is not null 
        /// </summary>
        private void SetBackCookieValue()
        {
            Logger.Write("Start of FurtherPersonalDetail1 SetBackCookieValue()", "General");
            hdnBack.Value = "";
            if (Request.QueryString["Page"] != null && Request.QueryString["Page"] == "Confirm")
            {
                Helper.SetTripleDESEncryptedCookie("FurtherPersonalDetails", "FurtherPersonalDetails");
                hdnConfirm.Value = "Confirm";
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage")))
            {
                hdnBack.Value = "Back";
                //Helper.SetTripleDESEncryptedCookie("PreferredLanguage", Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage").ToString());
                if (Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage").ToString() == "1")
                {

                    //divnext.Attributes.Add("style", "display:none");
                    divmr.Attributes.Add("class", "title_lang1 focus");
                    divmiss.Attributes.Add("class", "title_lang2");
                    divms.Attributes.Add("class", "title_lang4");
                    divmrs.Attributes.Add("class", "title_lang3");
                }
                if (Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage").ToString() == "2")
                {
                    divmiss.Attributes.Add("class", "title_lang2 focus");
                    divmr.Attributes.Add("class", "title_lang1");
                    divms.Attributes.Add("class", "title_lang4");
                    divmrs.Attributes.Add("class", "title_lang3");
                }
                if (Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage").ToString() == "3")
                {
                    divmrs.Attributes.Add("class", "title_lang3 focus");
                    divmiss.Attributes.Add("class", "title_lang2");
                    divmr.Attributes.Add("class", "title_lang1");
                    divms.Attributes.Add("class", "title_lang4");
                }
                if (Helper.GetTripleDESEncryptedCookieValue("PreferredLanguage").ToString() == "4")
                {
                    divms.Attributes.Add("class", "title_lang4 focus");
                    divmr.Attributes.Add("class", "title_lang1");
                    divmiss.Attributes.Add("class", "title_lang2");
                    divmrs.Attributes.Add("class", "title_lang3");
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")))
            {
                //Helper.SetTripleDESEncryptedCookie("PassportNumber", Helper.GetTripleDESEncryptedCookieValue("PassportNumber").ToString());
                txtPassport.Text = Helper.GetTripleDESEncryptedCookieValue("PassportNumber").ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")))
            {
                //Helper.SetTripleDESEncryptedCookie("SSN", Helper.GetTripleDESEncryptedCookieValue("SSN").ToString().Trim());
                txtSSN.Text = Helper.GetTripleDESEncryptedCookieValue("SSN").ToString();
            }

            if (Request.QueryString["ctrlID"] != null)
            {
                hdnErrorCtrl.Value = Request.QueryString["ctrlID"].ToString();
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Text_Check", "Text_Check()", true);
        }

        /// <summary>
        /// used to app reg exp, required to validate at javascript
        /// </summary>
        private void ListControlCollections()
        {
            Logger.Write("Start of FurtherPersonalDetail1 ListControlCollections()", "General");
            SetLanguageToLoad();

            ArrayList controlList = new ArrayList();
            AddControls(Page.Controls, controlList);
            //string cshowList = ConfigurationManager.AppSettings["ShowPassportSSNControls"].ToString();
            string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowPassportSSNControls");
            string[] cList = cshowList.Split(',');
            hdnShowControls.Value = cshowList;
            string final = string.Empty;
            for (int i = 0; i < cList.Length; i++)
            {
                string namecont = string.Empty;

                if (cList[i] == "PASSPORT")
                {
                    string Name1RegExp = string.Empty;
                    string Name1Required = string.Empty;
                    if (!string.IsNullOrEmpty(CommonClassForJoin.PassportRegExp))
                    {
                        Name1RegExp = CommonClassForJoin.PassportRegExp.ToString();
                    }
                    if (!string.IsNullOrEmpty(CommonClassForJoin.PassportRequired.ToString()))
                    {
                        Name1Required = CommonClassForJoin.PassportRequired.ToString();
                    }
                    namecont = cList[i].ToString() + ":" + Name1RegExp + "," + Name1Required;
                }

                if (cList[i] == "SSN")
                {
                    string Name3RegExp = string.Empty;
                    string Name3Required = string.Empty;
                    if (!string.IsNullOrEmpty(CommonClassForJoin.SSNRegExp))
                    {
                        Name3RegExp = CommonClassForJoin.SSNRegExp.ToString();
                    }
                    if (CommonClassForJoin.SSNRequired)
                    {
                        Name3Required = CommonClassForJoin.SSNRequired.ToString();
                    }
                    namecont = cList[i].ToString() + ":" + Name3RegExp + "," + Name3Required;
                }
                if (namecont != string.Empty)
                {

                    final = final + "|" + namecont;
                }

            }
            hdnControlList.Value = final;
        }

        private void SetLanguageToLoad()
        {
            //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ShowPassportSSNLanguage"].ToString()))
            if (!string.IsNullOrEmpty(ConfigurationReader.GetStringConfigKey("ShowPassportSSNLanguage")))
            {
                divmr.Visible = divmiss.Visible = divmrs.Visible = divms.Visible = false;
                string[] strLanguage = ConfigurationReader.GetStringConfigKey("ShowPassportSSNLanguage").Split(',');
                for (int i = 0; i < strLanguage.Count(); i++)
                {
                    switch (strLanguage[i])
                    {
                        case "1": divmr.Visible = true; break;
                        case "2": divmiss.Visible = true; break;
                        case "3": divmrs.Visible = true; break;
                        case "4": divms.Visible = true; break;
                    }
                }
            }
        }

        private void AddControls(ControlCollection page, ArrayList controlList)
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

        /// <summary>
        /// to set page level values in cookie
        /// </summary>
        private void SetPageControlValuesInCookie()
        {
            Logger.Write("Start of FurtherPersonalDetail1 SetPageControlValuesInCookie()", "General");
            if (txtSSN != null)
            {
                Helper.SetTripleDESEncryptedCookie("SSN", txtSSN.Text.Trim());
            }
            if (txtPassport != null)
            {
                Helper.SetTripleDESEncryptedCookie("PassportNumber", txtPassport.Text.Trim());
            }
        }

        /// <summary>
        /// set max length for controls
        /// </summary>
        private void SetMaxLenth()
        {
            Logger.Write("Start of FurtherPersonalDetail1 SetMaxLenth()", "General");
            if (txtPassport != null)
            {
                if (CommonClassForJoin.PassportMaxLength > 0)
                {
                    txtPassport.MaxLength = CommonClassForJoin.PassportMaxLength;
                }
                else
                {
                    txtPassport.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("PassportMaxLength"));
                }
                txtPassport.Enabled = false;
            }

            if (txtSSN != null)
            {
                if (CommonClassForJoin.SSNMaxLength > 0)
                {
                    txtSSN.MaxLength = CommonClassForJoin.SSNMaxLength;
                }
                else
                {
                    txtSSN.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("SSNMaxLength"));
                }
                txtSSN.Enabled = false;
            }

            divError.Attributes.Add("style", "display:none");
            lblError.Text = string.Empty;

            if (CommonClassForJoin.LanguageReq == true)
            {
                lblPLOpt.Visible = false;
            }
            else
            {
                lblPLOpt.Visible = true;
            }
            if (CommonClassForJoin.PassportRequired == true)
            {
                lblPassOpt.Visible = false;
            }
            else
            {
                lblPassOpt.Visible = true;
            }
            if (CommonClassForJoin.SSNRequired == true)
            {
                lblSSNOpt.Visible = false;
            }
            else
            {
                lblSSNOpt.Visible = true;
            }
        }

        private void SelectLanguage(string p)
        {
            Logger.Write("Start of FurtherPersonalDetail1 SelectLanguage()", "General");
            //divnextbutton.Attributes.Add("class", "nextbutton nextsurname");
            divmr.Attributes.Add("class", "title_lang1 unselect");
            divmiss.Attributes.Add("class", "title_lang2 unselect");
            divms.Attributes.Add("class", "title_lang4 unselect");
            divmrs.Attributes.Add("class", "title_lang3 unselect");
            switch (p)
            {
                case "divmr": divmr.Attributes.Add("class", "title_lang1 select"); break;
                case "divmiss": divmiss.Attributes.Add("class", "title_lang2 select"); break;
                case "divmrs": divmrs.Attributes.Add("class", "title_lang3 select"); break;
                case "divms": divms.Attributes.Add("class", "title_lang4 select"); break;
            }
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");
        }

        protected void title_lang1Click(object sender, EventArgs e)
        {
            SelectLanguage(((LinkButton)sender).ID);
            GetLanguage("1");

        }
        protected void title_lang2Click(object sender, EventArgs e)
        {
            SelectLanguage(((LinkButton)sender).ID);
            GetLanguage("2");
        }
        protected void title_lang3Click(object sender, EventArgs e)
        {
            SelectLanguage(((LinkButton)sender).ID);
            GetLanguage("3");
        }
        protected void title_lang4Click(object sender, EventArgs e)
        {
            SelectLanguage(((LinkButton)sender).ID);
            GetLanguage("4");
        }

        private void GetLanguage(string p)
        {
            Logger.Write("Start of FurtherPersonalDetail1 GetLanguage()", "General");
            string strLang = string.Empty;
            Helper.SetTripleDESEncryptedCookie("PreferredLanguage", p);

            strLang = ConfigurationReader.GetStringConfigKey("Language" + p);
            if (strLang == null) strLang = "";

            string[] strLangValues = strLang.Split('|');
            switch (strLangValues.Count())
            {
                case 1: Helper.SetTripleDESEncryptedCookie("PreferredLanguageKey", strLangValues[0]); break;
                case 2: Helper.SetTripleDESEncryptedCookie("PreferredLanguageKey", strLangValues[0]);
                    Helper.SetTripleDESEncryptedCookie("PreferredLanguageValue", strLangValues[1]); break;
            }
            Helper.CheckAndResetCookieExpiration("FurtherPersonalDetails");
        }


        private void ControlMovement()
        {
            Logger.Write("Start of FurtherPersonalDetail1 ControlMovement()", "General");
            string[] strControls = ConfigurationReader.GetStringConfigKey("ShowPassportSSNControls").Split(',');
            string[] strPages = ConfigurationReader.GetStringConfigKey("FurtherPersonalDetail1").Split('|');
            if (strPages.Count() == 1)
            {
                hdnPrevPage.Value = strPages[0] + ".aspx";
            }
            else if (strPages.Count() == 2)
            {
                hdnPrevPage.Value = strPages[0] + ".aspx";
                hdnNextPage.Value = strPages[1] + ".aspx";
            }

            for (int i = 0; i < strControls.Count(); i++)
            {
                switch (strControls[i])
                {
                    case "Language": if (!string.IsNullOrEmpty(CommonClassForJoin.LanguageReq.ToString()))
                            hdnLanguage.Value = CommonClassForJoin.LanguageReq.ToString(); break;
                    case "Passport": if (!string.IsNullOrEmpty(CommonClassForJoin.PassportRequired.ToString()))
                            hdnPassport.Value = CommonClassForJoin.PassportRequired.ToString(); break;
                    case "SSN": if (!string.IsNullOrEmpty(CommonClassForJoin.SSNRequired.ToString()))
                            hdnSSN.Value = CommonClassForJoin.SSNRequired.ToString(); break;
                }
            }
            //hdnLanguage.Value = hdnPassport.Value = hdnSSN.Value = "true";
        }
        protected void Confirm_Details(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of FurtherPersonalDetail1 Confirm_Details()", "General");
                Helper.Net35BasicAuthentication();
                //set Cookies
                SetPageControlValuesInCookie();
                if (ValiateControlsData())
                {
                    //Send  to error page
                    Response.Redirect(sRedirectLink, false);
                }
                else
                {
                    if (CommonClassForJoin.IsProfinityCheckNeeded == true)
                    {
                        string sProfaneValues = string.Empty;
                        if (txtPassport != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("SECONDARYID"))
                        {
                            sProfaneValues = txtPassport.Text.Trim();
                        }

                        if (txtSSN != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("PRIMARYID"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtSSN.Text.Trim();
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
                                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
                                {
                                    Helper.DeleteTripleDESEncryptedCookie("FurtherPersonalDetails");
                                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                                }
                                else
                                {
                                    //Send Succesfully to next page
                                    Response.Redirect("~/Kiosk/" + hdnNextPage.Value, false);
                                }
                            }
                            else
                            {
                                sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=LPSPage&resID=ProfanityMsg&imgID=LPSCrumb";
                                Response.Redirect(sRedirectLink, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
                        {
                            Helper.DeleteTripleDESEncryptedCookie("FurtherPersonalDetails");
                            SetPageControlValuesInCookie();
                            Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                        }
                        else
                        {
                            //Send Succesfully to next page
                            Response.Redirect("~/Kiosk/" + hdnNextPage.Value, false);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Write("FurtherPersonalDetail1.aspx.cs:Confirm_Details():ProfanityCheck Server down:" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                //throw exp;
                sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=final&resID=SorryErr&imgID=LPSCrumb";
                Response.Redirect(sRedirectLink, false);
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

        private bool ValiateControlsData()
        {
            bool value = false;

            Logger.Write("Start of FurtherPersonalDetail1 Confirm_Details()", "General");
            string cshowList = ConfigurationReader.GetStringConfigKey("ShowPassportSSNControls");
            string[] cList = cshowList.Split(',');
            //hdnShowControls.Value = cshowList;
            string final = string.Empty;

            for (int i = 0; i < cList.Length; i++)
            {
                if (cList[i].ToString().ToUpperInvariant() == "PASSPORT")
                {
                    if (txtPassport != null)
                    {
                        if (hdnPassport.Value == "True" && txtPassport.Text.Length == 0)
                        {
                            value = true;
                            sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtPassport&resID=LPSPassportError&imgID=LPSCrumb";
                        }
                        if (CommonClassForJoin.PassportMinLength > 0 && txtPassport.Text.Trim().Length < CommonClassForJoin.PassportMinLength)
                        {
                            value = true;
                            sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtPassport&resID=LPSPassportError&imgID=LPSCrumb";
                        }
                        if (txtPassport.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.PassportRegExp))
                        {
                            string passportRegExp = CommonClassForJoin.PassportRegExp;
                            if (!Helper.IsRegexMatch(txtPassport.Text.Trim(), passportRegExp, false, false))
                            {
                                value = true;
                                sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtPassport&resID=LPSPassportError&imgID=LPSCrumb";
                            }
                        }

                    }
                }
                if (cList[i].ToString().ToUpperInvariant() == "SSN")
                {
                    if (txtSSN != null)
                    {
                        if (hdnSSN.Value == "True" && txtSSN.Text.Length == 0)
                        {
                            value = true;
                            sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtSSN&resID=LPSSSNError&imgID=LPSCrumb";
                        }
                        if (CommonClassForJoin.SSNMinLength > 0 && txtSSN.Text.Trim().Length < CommonClassForJoin.SSNMinLength)
                        {
                            value = true;
                            sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtSSN&resID=LPSSSNError&imgID=LPSCrumb";
                        }
                        if (txtSSN.Text.Trim() != string.Empty && !string.IsNullOrEmpty(CommonClassForJoin.SSNRegExp))
                        {
                            string regAddressLine1 = CommonClassForJoin.SSNRegExp;
                            if (!Helper.IsRegexMatch(txtSSN.Text.Trim(), regAddressLine1, false, false))
                            {
                                value = true;
                                sRedirectLink = "~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetail1&ctrlID=txtSSN&resID=LPSSSNError&imgID=LPSCrumb";
                            }
                        }

                    }
                }

            }
            return value;
        }

        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }

        protected void btnBack_click(object sender, EventArgs e)
        {
            SetPageControlValuesInCookie();
            string strNextPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("FurtherPersonalDetail1"));
            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
        }

    }
}