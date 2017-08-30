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

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    /// <summary>
    /// this page captures Title and Name details from the customer
    /// Author Nagaraju T
    /// </summary>
    public partial class WhatIsYourTitleandName : BaseUIPage
    {
        UtilityServiceClient utilityClient = null;
        
        private string sPage = string.Empty;
        private bool sTimeFlag = false;
        private bool sCookieFlag = false;
        public string resourceStr = string.Empty;
        private string showNameControls = string.Empty;

        private string ctrlID = string.Empty;
        private string resID = string.Empty;
        private string imgID = string.Empty;

        string resultXml = string.Empty;
        string errorXml = string.Empty;
        XmlDocument resulDoc = null;
        string resultxml = string.Empty;
        int rowCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourTitleandName Page_Load()", "General");
                
                Helper.CheckAndResetCookie();
                //showNameControls = ConfigurationManager.AppSettings["ShowNameControls"].ToString().ToUpper(System.Globalization.CultureInfo.InvariantCulture);
                showNameControls = ConfigurationReader.GetStringConfigKeyToUpper("ShowNameControls");
                hdnShowControls.Value = showNameControls;
                if (!IsPostBack)
                {
                    Helper.GetAndLoadConfigurationDetails();
                    SetMaxLenth();
                    SetCookieValue();
                }
                SetResourceStr();
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourTitleandName.aspx.cs:PageLoad():" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                string ctrlID = "final";
                string resID = "SorryErr";
                string imgID = "ImgCrumbPrint";
                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                Response.Redirect(qstring, false);
            }
        }

        /// <summary>
        /// delete cookie values if querry string is null 
        /// </summary>
        private void DeleteCookie()
        {
            Logger.Write("Start of WhatIsYourTitleandName DeleteCookie()", "General");
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
            {
                if (Request.QueryString["Ttl"] == null && !string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")) && Request.QueryString["Page"] == null)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Tilte");
                    sCookieFlag = true;
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")) && Request.QueryString["Page"] == null  && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("FirstName");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Initial");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("LastName");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("PostCode");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Email")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Email");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PhoneNo")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("PhoneNo");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Date");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Date")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Date");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Month")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Month");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Year")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Year");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Dietry")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Dietry");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age1");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age2");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age3");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age4");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Age5");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("MailingAddress1");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("MailingAddress2");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("MailingAddress3");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Vegeterian")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Vegeterian");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Halal")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Halal");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Koshar")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Koshar");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("None")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("None");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoProduct")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoProduct");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TescoPartnerInfo")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("TescoPartnerInfo");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerResearch")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("CustomerResearch");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ClientID")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("ClientID");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Teetotal")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Teetotal");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Diabetic")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("Diabetic");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("PostCodeAndAddress");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("TitleAndName");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("DOBDietry");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("EmailAndPhone");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("TitleAndName");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCodeAndAddress")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("PostCodeAndAddress");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAndPhone")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("EmailAndPhone");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DOBDietry")) && Request.QueryString["Page"] == null && sCookieFlag)
                {
                    Helper.DeleteTripleDESEncryptedCookie("DOBDietry");
                }
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
                {
                    divspacer.Attributes.Add("style", "display:none");
                    divs.Attributes.Add("style", "display:none");
                    divsummary.Attributes.Add("style", "display:block");
                }

            }
        }

        /// <summary>
        /// set cookie values if querry string is not null 
        /// </summary>
        private void SetCookieValue()
        {
            Logger.Write("Start of WhatIsYourTitleandName SetCookieValue()", "General");
            if (Request.QueryString["Page"] != null && Request.QueryString["Page"] == "Confirm")
            {
                Helper.SetTripleDESEncryptedCookie("TitleAndName", "TitleAndName");
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")) )
            {
                if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString() == "1")
                {
                    
                    divnext.Attributes.Add("style", "display:none");
                    divmr.Attributes.Add("class", "title_mr select");
                    divmiss.Attributes.Add("class", "title_miss");
                    divms.Attributes.Add("class", "title_ms");
                    divmrs.Attributes.Add("class", "title_mrs");
                } 
                if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString() == "2")
                {
                    divnext.Attributes.Add("style", "display:none");
                    divmiss.Attributes.Add("class", "title_miss select");
                    divmr.Attributes.Add("class", "title_mr");
                    divms.Attributes.Add("class", "title_ms");
                    divmrs.Attributes.Add("class", "title_mrs");
                }
                if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString() == "3")
                {
                    divnext.Attributes.Add("style", "display:none");
                    divmrs.Attributes.Add("class", "title_mrs select");
                    divmiss.Attributes.Add("class", "title_miss");
                    divmr.Attributes.Add("class", "title_mr");
                    divms.Attributes.Add("class", "title_ms");
                }
                if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString() == "4")
                {
                    divnext.Attributes.Add("style", "display:none");
                    divms.Attributes.Add("class", "title_ms select");
                    divmr.Attributes.Add("class", "title_mr");
                    divmiss.Attributes.Add("class", "title_miss");
                    divmrs.Attributes.Add("class", "title_mrs");
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")))
            {
                txtFirstName.Text = Helper.GetTripleDESEncryptedCookieValue("FirstName").ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")) )
            {
                txtMiddleName.Text = Helper.GetTripleDESEncryptedCookieValue("Initial").ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")))
            {
                txtSurname.Text = Helper.GetTripleDESEncryptedCookieValue("LastName").ToString();
            }
            if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ClientID")))
            {
                Helper.SetTripleDESEncryptedCookie("ClientID", Request.UserHostAddress);
            }
            if (Request.QueryString["ctrlID"] != null)
            {
                hdnErrorCtrl.Value = Request.QueryString["ctrlID"].ToString();
            }
            if (Request.QueryString["Page"] != null)
            {
                hdnErrorCtrl.Value = Request.QueryString["Page"].ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
            {
                hdnErrorCtrl.Value = "Confirm";
            }
            hdnControlList.Value = CommonClassForJoin.Name3Required.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// used to app reg exp, required to validate at javascript
        /// </summary>
        private void ListControlCollections()
        {
            ArrayList controlList = new ArrayList();
            AddControls(Page.Controls, controlList);
            //string cshowList = ConfigurationManager.AppSettings["ShowNameControls"].ToString();
            string cshowList = ConfigurationReader.GetStringConfigKeyToUpper("ShowNameControls");
            string[] cList = cshowList.Split(',');
            hdnShowControls.Value = cshowList;
            string final = string.Empty;
            for (int i = 0; i < cList.Length; i++)
            {
                string namecont = string.Empty;

                if (cList[i] == "FIRSTNAME")
                {
                    string Name1RegExp = string.Empty;
                    string Name1Required = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name1RegExp")))
                    {
                        Name1RegExp = Helper.GetTripleDESEncryptedCookieValue("Name1RegExp").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name1Required")))
                    {
                        Name1Required = Helper.GetTripleDESEncryptedCookieValue("Name1Required").ToString();
                    }
                    namecont = cList[i] + ":" + Name1RegExp + "," + Name1Required;
                }
                if (cList[i] == "MIDDLENAME")
                {
                    string Name2RegExp = string.Empty;
                    string Name2Required = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name2RegExp")))
                    {
                        Name2RegExp = Helper.GetTripleDESEncryptedCookieValue("Name2RegExp").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name2Required")))
                    {
                        Name2Required = Helper.GetTripleDESEncryptedCookieValue("Name2Required").ToString();
                    }
                    namecont = cList[i] + ":" + Name2RegExp + "," + Name2RegExp;
                }
                if (cList[i] == "SURNAME")
                {
                    string Name3RegExp = string.Empty;
                    string Name3Required = string.Empty;
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name3RegExp")))
                    {
                        Name3RegExp = Helper.GetTripleDESEncryptedCookieValue("Name3RegExp").ToString();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Name3Required")))
                    {
                        Name3Required = Helper.GetTripleDESEncryptedCookieValue("Name3Required").ToString();
                    }
                    namecont = cList[i] + ":" + Name3RegExp + "," + Name3Required;
                }
                if (namecont != string.Empty)
                {

                    final = final + "|" + namecont;
                }
               
            }
            hdnControlList.Value = final;
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
            Logger.Write("Start of WhatIsYourTitleandName SetPageControlValuesInCookie()", "General");
            if (txtSurname != null)
            {
                Helper.SetTripleDESEncryptedCookie("LastName", txtSurname.Text.Trim());
            }
            if (txtFirstName != null)
            {
                Helper.SetTripleDESEncryptedCookie("FirstName", txtFirstName.Text.Trim());
            }
            if (txtMiddleName != null)
            {
                Helper.SetTripleDESEncryptedCookie("Initial", txtMiddleName.Text.Trim());
            }
            Helper.SetTripleDESEncryptedCookie("UserFirstName", "FirstName");
        }

        /// <summary>
        /// set max length for controls
        /// </summary>
        private void SetMaxLenth()
        {
            Logger.Write("Start of WhatIsYourTitleandName SetMaxLenth()", "General");
            if (txtFirstName != null)
            {
                if (CommonClassForJoin.Name1MaxLength > 0)
                {
                    txtFirstName.MaxLength = CommonClassForJoin.Name1MaxLength;
                }
                else
                {
                    txtFirstName.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("FirstNameMaxLength"));
                }
               
                txtFirstName.Enabled = false;
            }
            if (txtMiddleName != null)
            {
                if (CommonClassForJoin.Name2MaxLength > 0)
                {
                    txtMiddleName.MaxLength = CommonClassForJoin.Name2MaxLength;
                }
                else
                {
                    txtMiddleName.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("MiddleNameMaxLength"));
                }
                txtSurname.Enabled = false;
            }
            if (txtSurname != null)
            {
                if (CommonClassForJoin.Name3MaxLength > 0)
                {
                    txtSurname.MaxLength = CommonClassForJoin.Name3MaxLength;
                }
                else
                {
                    txtSurname.MaxLength = Convert.ToInt32(ConfigurationReader.GetStringConfigKey("SurnameMaxLength"));
                }
                txtMiddleName.Enabled = false;
            }
            if (CommonClassForJoin.Name1Required == true)
            {
                lblFNOpt.Visible = false;
            }
            else
            {
                lblFNOpt.Visible = true;
            }
            if (CommonClassForJoin.Name2Required ==true)
            {
                lblMNOpt.Visible = false;
            }
            else
            {
                lblMNOpt.Visible = true;
            }
            if (CommonClassForJoin.Name3Required == true)
            {
                lblSNOpt.Visible = false;
            }
            else
            {
                lblSNOpt.Visible = true;
            }
        }

        private void SetResourceStr()
        {
            resourceStr = Resources.GlobalResources.Name1Title + "," + Resources.GlobalResources.Name2Title + "," + Resources.GlobalResources.Name3Title + "," + Resources.GlobalResources.BackBtn + "," + Resources.GlobalResources.BackBtnGrey;
        }

        private bool ValiateControlsData()
        {
            Logger.Write("Start of WhatIsYourTitleandName ValiateControlsData()", "General");
            bool value = false;

            SetPageControlValuesInCookie();
            string[] cList = showNameControls.Split(',');

            for (int i = 0; i < cList.Length; i++)
            {
                if (cList[i] == "TITLE")
                {
                    if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
                    {
                        ctrlID = "Title";
                        resID = "TitleError";
                        imgID = "NameBreadCrumb";
                        return false;
                    }

                }
                if (cList[i] == "FIRSTNAME")
                {
                    if (txtFirstName.Text == "" && CommonClassForJoin.Name1Required ==true)
                    {
                        ctrlID = "FirstName";
                        resID = "Name1Error";
                        imgID = "NameBreadCrumb";
                        return false;
                    }
                    if (txtFirstName.Text != string.Empty)
                    {
                        if (CommonClassForJoin.Name1MinLength >0 && txtFirstName.Text.Trim().Length < CommonClassForJoin.Name1MinLength)
                        {
                            ctrlID = "FirstName";
                            resID = "Name1Error";
                            imgID = "NameBreadCrumb";
                            return false;
                        }
                        if (CommonClassForJoin.Name1RegExp != null || !string.IsNullOrEmpty(CommonClassForJoin.Name1RegExp))
                        {
                            string regExp = CommonClassForJoin.Name1RegExp.ToString();
                            if (!Helper.IsRegexMatch(txtFirstName.Text.Trim(), regExp, false, false))
                            {
                                ctrlID = "FirstName";
                                resID = "Name1Error";
                                imgID = "NameBreadCrumb";
                                return false;

                            }
                        }
                    }
                }
                if (cList[i] == "MIDDLENAME")
                {
                    if (txtMiddleName.Text == "" && CommonClassForJoin.Name2Required ==true)
                    {
                        ctrlID = "MiddleName";
                        resID = "Name2Error";
                        imgID = "NameBreadCrumb";
                        return false;
                    }
                    if (txtMiddleName.Text != string.Empty)
                    {
                        if (CommonClassForJoin.Name2MinLength > 0 && txtMiddleName.Text.Trim().Length < CommonClassForJoin.Name2MinLength)
                        {
                            ctrlID = "MiddleName";
                            resID = "Name2Error";
                            imgID = "NameBreadCrumb";
                            return false;
                        }
                        if (CommonClassForJoin.Name2RegExp != null || !string.IsNullOrEmpty(CommonClassForJoin.Name2RegExp))
                        {
                            string regExp = CommonClassForJoin.Name2RegExp.ToString();

                            if (!Helper.IsRegexMatch(txtMiddleName.Text.Trim(), regExp, false, false))
                            {
                                ctrlID = "MiddleName";
                                resID = "Name2Error";
                                imgID = "NameBreadCrumb";
                                return false;

                            }
                        }
                    }
                }
                if (cList[i] == "SURNAME")
                {
                    if (txtSurname.Text == "" && CommonClassForJoin.Name3Required ==true)
                    {
                        ctrlID = "Surname";
                        resID = "Name3Error";
                        imgID = "NameBreadCrumb";
                        return false;
                    }
                    if (txtSurname.Text != string.Empty)
                    {
                        if (CommonClassForJoin.Name3MinLength >0 && txtSurname.Text.Trim().Length < CommonClassForJoin.Name3MinLength)
                        {
                            ctrlID = "Surname";
                            resID = "Name3Error";
                            imgID = "NameBreadCrumb";
                            return false;
                        }
                        if (CommonClassForJoin.Name3RegExp != null || !string.IsNullOrEmpty(CommonClassForJoin.Name3RegExp))
                        {
                            string regExp = CommonClassForJoin.Name3RegExp.ToString();

                            if (!Helper.IsRegexMatch(txtSurname.Text.Trim(), regExp, false, false))
                            {
                                ctrlID = "Surname";
                                resID = "Name3Error";
                                imgID = "NameBreadCrumb";
                                return false;

                            }
                        }
                    }
                }
            }
            return true;
        }

        protected void title_mrClick(object sender, EventArgs e)
        {
            divmr.Attributes.Add("class", "title_mr select");
            divmiss.Attributes.Add("class", "title_miss unselect");
            divms.Attributes.Add("class", "title_ms unselect");
            divmrs.Attributes.Add("class", "title_mrs unselect");
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            txtFirstName.Enabled = true;
            divFirstName1.Attributes.Add("class", "focuspanel curved paddingtop-10");
            divFirstName2.Attributes.Add("class", "inputtext whitetext paddingtop10");
            divFirstName3.Attributes.Add("class", "input322 paddingtop10");
            Helper.SetTripleDESEncryptedCookie("Tilte", "1");
        }
        protected void title_missClick(object sender, EventArgs e)
        {
            divmiss.Attributes.Add("class", "title_miss select");
            divmr.Attributes.Add("class", "title_mr unselect");
            divms.Attributes.Add("class", "title_ms unselect");
            divmrs.Attributes.Add("class", "title_mrs unselect");
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            txtFirstName.Enabled = true;
            divFirstName1.Attributes.Add("class", "focuspanel curved paddingtop-10");
            divFirstName2.Attributes.Add("class", "inputtext whitetext paddingtop10");
            divFirstName3.Attributes.Add("class", "input322 paddingtop10");
            Helper.SetTripleDESEncryptedCookie("Tilte", "2");
        }
        protected void title_mrsClick(object sender, EventArgs e)
        {
            divmrs.Attributes.Add("class", "title_mrs select");
            divmiss.Attributes.Add("class", "title_miss unselect");
            divmr.Attributes.Add("class", "title_mr unselect");
            divms.Attributes.Add("class", "title_ms unselect");
            divTitle.Attributes.Add("class", "");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            Span1.Attributes.Add("class", "titletext grey");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            txtFirstName.Enabled = true;
            divFirstName1.Attributes.Add("class", "focuspanel curved paddingtop-10");
            divFirstName2.Attributes.Add("class", "inputtext whitetext paddingtop10");
            divFirstName3.Attributes.Add("class", "input322 paddingtop10");
            Helper.SetTripleDESEncryptedCookie("Tilte", "3");
        }
        protected void title_msClick(object sender, EventArgs e)
        {
            divms.Attributes.Add("class", "title_ms select");
            divmr.Attributes.Add("class", "title_mr unselect");
            divmiss.Attributes.Add("class", "title_miss unselect");
            divmrs.Attributes.Add("class", "title_mrs unselect");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            Span1.Attributes.Add("class", "titletext grey");
            divTitle.Attributes.Add("class", "");
            divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            txtFirstName.Enabled = true;
            divFirstName1.Attributes.Add("class", "focuspanel curved paddingtop-10");
            divFirstName2.Attributes.Add("class", "inputtext whitetext paddingtop10");
            divFirstName3.Attributes.Add("class", "input322 paddingtop10");
            Helper.SetTripleDESEncryptedCookie("Tilte", "4");
        }
        protected void Confirm_Details(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of WhatIsYourTitleandName Confirm_Details()", "General");
                Helper.Net35BasicAuthentication();
                string strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("WhatIsYourTitleandName"));
                if (!ValiateControlsData())
                {
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourTitleandName&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                    Response.Redirect(qstring, false);
                }
                else
                {
                    bool isProfinityCheckNeeded = CommonClassForJoin.IsProfinityCheckNeeded;
                    if (isProfinityCheckNeeded == true)
                    {
                        string sProfaneValues = string.Empty;
                        if (txtFirstName != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("NAME1"))
                        {
                            sProfaneValues = txtFirstName.Text.Trim();
                        }
                        if (txtMiddleName != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("NAME2"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtMiddleName.Text.Trim();
                        }
                        if (txtSurname != null && CommonClassForJoin.ProfinityCheckFields.ToUpperInvariant().Contains("NAME3"))
                        {
                            sProfaneValues = sProfaneValues + "," + txtSurname.Text.Trim();
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
                                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
                                {
                                    Helper.DeleteTripleDESEncryptedCookie("TitleAndName");
                                    SetPageControlValuesInCookie();
                                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                                }
                                else
                                {
                                    string qstring = "~/Kiosk/" + strNextPageName + ".aspx";
                                    if (Request.QueryString["Page"] != null)
                                    {
                                        qstring = qstring + "?Page=" + Request.QueryString["Page"].ToString();
                                    }
                                    SetPageControlValuesInCookie();
                                    Response.Redirect(qstring, false);
                                }
                            }
                            else
                            {
                                ctrlID = "FirstName";
                                resID = "ProfanityMsg";
                                imgID = "NameBreadCrumb";
                                string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=WhatIsYourTitleandName&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                                Response.Redirect(qstring, false);
                            }
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("TitleAndName")))
                        {
                            Helper.DeleteTripleDESEncryptedCookie("TitleAndName");
                            SetPageControlValuesInCookie();
                            Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);
                        }
                        else
                        {
                            SetPageControlValuesInCookie();
                            Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                Logger.Write("WhatIsYourTitleandName.aspx.cs:Confirm_Details(),ProfanityCheck Server down::" + exp.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
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

        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }
        protected void TandC_Click(object sender, EventArgs e)
        {
            SetPageControlValuesInCookie();
            Response.Redirect("~/Kiosk/TermsAndCondition.aspx?page=WhatIsYourTitleandName", false);
        }
    }
}