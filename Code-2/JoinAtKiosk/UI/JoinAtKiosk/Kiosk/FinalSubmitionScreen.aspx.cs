using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using System.Globalization;
using System.Collections;
using System.Xml;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService;
using Tesco.Com.Marketing.Kiosk.JoinAtKiosk.KioskReportingService;
using System.Data;
using Resources;
using System.ServiceModel;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class FinalSubmitionScreen : BaseUIPage
    {
        private string errqstring = string.Empty;
        private string ctrlID = "Final";
        private string resID = "DuplicateAccountErr";
        private string imgID = "ImgCrumbPrint";
        private string cResID = "NGCError";
        private string culture = ConfigurationReader.GetStringConfigKey("CountryCode");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of FinalSubmitionScreen Page_Load()", "General");
                if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserFirstName")))
                {
                    Response.Redirect("~/Kiosk/TimeOut.aspx", false);
                }
                else
                {
                    //Resetting expiration for all cookies.
                    Helper.CheckAndResetCookie();
                }
                //Show only configured sections based on Configuration Entry.
                GetConfiguredSections();
                ShowConfiguredLangSSNLabels();
                ShowConfiguredDOBHHAges();
                if (!IsPostBack)
                {
                    //imgPrint.Attributes.Add("onclick", "this.disabled=true;document.getElementById('imgTC').disabled=true;document.getElementById('imgback').disabled=true;document.getElementById('divcancel').style.display='none';document.getElementById('editDOB').style.display='none';document.getElementById('editEmail').style.display='none';document.getElementById('editpostcode').style.display='none';document.getElementById('editname').style.display='none';" + GetPostBackEventReference(imgPrint).ToString());

                    //Setting Name Field.
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Tilte")))
                    {
                        if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "1")
                        {
                            lblName.Text = Resources.GlobalResources.FSTitle1.ToString(); //Mr
                        }
                        else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "2")
                        {
                            lblName.Text = Resources.GlobalResources.FSTitle2.ToString(); //Miss
                        }
                        else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "3")
                        {
                            lblName.Text = Resources.GlobalResources.FSTitle3.ToString(); //Mrs
                        }
                        else if (Helper.GetTripleDESEncryptedCookieValue("Tilte").ToString().Trim() == "4")
                        {
                            lblName.Text = Resources.GlobalResources.FSTitle4.ToString(); //Ms
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FirstName")))
                    {
                        if (lblName.Text != string.Empty)
                        {
                            lblName.Text = lblName.Text + " " + Helper.GetTripleDESEncryptedCookieValue("FirstName").ToString().Trim();
                        }
                        else
                        {
                            lblName.Text = Helper.GetTripleDESEncryptedCookieValue("FirstName").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Initial")))
                    {
                        if (lblName.Text != string.Empty)
                        {
                            lblName.Text = lblName.Text + " " + Helper.GetTripleDESEncryptedCookieValue("Initial").ToString().Trim();
                        }
                        else
                        {
                            lblName.Text = Helper.GetTripleDESEncryptedCookieValue("Initial").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("LastName")))
                    {
                        if (lblName.Text != string.Empty)
                        {
                            lblName.Text = lblName.Text + " " + Helper.GetTripleDESEncryptedCookieValue("LastName").ToString().Trim();
                        }
                        else
                        {
                            lblName.Text = Helper.GetTripleDESEncryptedCookieValue("LastName").ToString().Trim();
                        }
                    }

                    //Setting Mailing Address and Postcode.
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress1")))
                    {
                        lblAddress.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress1").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress2")))
                    {
                        if (lblAddress.Text != string.Empty)
                        {
                            lblAddress.Text = lblAddress.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("MailingAddress2").ToString().Trim();
                        }
                        else
                        {
                            lblAddress.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress2").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress3")))
                    {
                        if (lblAddress.Text != string.Empty)
                        {
                            lblAddress.Text = lblAddress.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("MailingAddress3").ToString().Trim();
                        }
                        else
                        {
                            lblAddress.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress3").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress4")))
                    {
                        if (lblAddress.Text != string.Empty)
                        {
                            lblAddress.Text = lblAddress.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("MailingAddress4").ToString().Trim();
                        }
                        else
                        {
                            lblAddress.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress4").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MailingAddress5")))
                    {
                        if (lblAddress.Text != string.Empty)
                        {
                            lblAddress.Text = lblAddress.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("MailingAddress5").ToString().Trim();
                        }
                        else
                        {
                            lblAddress.Text = Helper.GetTripleDESEncryptedCookieValue("MailingAddress5").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PostCode")))
                    {
                        lblPostCode.Text = Helper.GetTripleDESEncryptedCookieValue("PostCode").ToString().Trim();
                    }

                    //Setting Email & Phone Numbers.
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EmailAddress")))
                    {
                        lblEmail.Text = Helper.GetTripleDESEncryptedCookieValue("EmailAddress").ToString().Trim();
                        if (lblEmail.Text.Length > 27)
                        {
                            string email = lblEmail.Text.Substring(0, 27);
                            string email1 =lblEmail.Text.Substring(27);
                            email = email + "<br/>" + email1;
                            lblEmail.Text = email;
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("EveningNumber")))
                    {
                        lblPhone.Text = Helper.GetTripleDESEncryptedCookieValue("EveningNumber").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("MobileNumber")))
                    {
                        if (lblPhone.Text != string.Empty)
                        {
                            lblPhone.Text = lblPhone.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("MobileNumber").ToString().Trim();
                        }
                        else
                        {
                            lblPhone.Text = Helper.GetTripleDESEncryptedCookieValue("MobileNumber").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber")))
                    {
                        if (lblPhone.Text != string.Empty)
                        {
                            lblPhone.Text = lblPhone.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber").ToString().Trim();
                        }
                        else
                        {
                            lblPhone.Text = Helper.GetTripleDESEncryptedCookieValue("DaytimeNumber").ToString().Trim();
                        }
                    }

                    //Setting Language, PassportNumber, SSN Numbers and Race.
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PreferredLanguageValue")))
                    {
                        lblPreferredLanguage.Text = Helper.CheckAndResetCookieExpiration("PreferredLanguageValue").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PassportNumber")))
                    {
                        lblPassportNumber.Text = Helper.GetTripleDESEncryptedCookieValue("PassportNumber").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("SSN")))
                    {
                        lblSocialSecurityNumber.Text = Helper.GetTripleDESEncryptedCookieValue("SSN").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Race")))
                    {
                        lblRace.Text = Helper.GetTripleDESEncryptedCookieValue("Race").ToString().Trim();
                    }

                    //Setting DOB, OtherhouseholdAges DOB cookie value will be local datetime format.
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DateOfBirth")))
                    {
                        lblDOB.Text = Helper.GetTripleDESEncryptedCookieValue("DateOfBirth").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age1")))
                    {
                        lblAges.Text = Helper.GetTripleDESEncryptedCookieValue("Age1").ToString().Trim();
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age2")))
                    {
                        if (lblAges.Text != string.Empty)
                        {
                            lblAges.Text = lblAges.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("Age2").ToString().Trim();
                        }
                        else
                        {
                            lblAges.Text = Helper.GetTripleDESEncryptedCookieValue("Age2").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age3")))
                    {
                        if (lblAges.Text != string.Empty)
                        {
                            lblAges.Text = lblAges.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("Age3").ToString().Trim();
                        }
                        else
                        {
                            lblAges.Text = Helper.GetTripleDESEncryptedCookieValue("Age3").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age4")))
                    {
                        if (lblAges.Text != string.Empty)
                        {
                            lblAges.Text = lblAges.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("Age4").ToString().Trim();
                        }
                        else
                        {
                            lblAges.Text = Helper.GetTripleDESEncryptedCookieValue("Age4").ToString().Trim();
                        }
                    }
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Age5")))
                    {
                        if (lblAges.Text != string.Empty)
                        {
                            lblAges.Text = lblAges.Text + ", " + Helper.GetTripleDESEncryptedCookieValue("Age5").ToString().Trim();
                        }
                        else
                        {
                            lblAges.Text = Helper.GetTripleDESEncryptedCookieValue("Age5").ToString().Trim();
                        }
                    }

                    //Dietry preference, for UK, it should be "IsHalal,IsKosher, IsDiabetic, IsTeeTotal, IsVegeterian"
                    //Dietry preference, for Group, it should be "1,2,3,4"
                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName"))) 
                    {
                        string strpref = Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName").ToString().ToUpperInvariant(); //Eg:IsHalal, IsKoshar, IsDiabetic, IsTeeTotal for UK.
                        if (strpref.Contains("ISHALAL") || strpref.Contains("ISKOSHER") || strpref.Contains("ISDIABETIC")
                                || strpref.Contains("ISTEETOTAL") || strpref.Contains("ISVEGETERIAN"))
                        {
                            string[] arrDietaryPref = Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName").ToString().ToUpperInvariant().Split(',').ToArray();
                            if (arrDietaryPref.Count() > 0)
                            {
                                for (int i = 0; i <= arrDietaryPref.Length - 1; i++)
                                {
                                    if (arrDietaryPref[i].ToString() == "ISHALAL")
                                    {
                                        lblDietry.Text = lblDietry.Text + " Halal";
                                    }
                                    if (arrDietaryPref[i].ToString() == "ISKOSHER")
                                    {
                                        lblDietry.Text = lblDietry.Text + " Kosher";
                                    }
                                    if (arrDietaryPref[i].ToString() == "ISDIABETIC")
                                    {
                                        lblDietry.Text = lblDietry.Text + " Diabetic";
                                    }
                                    if (arrDietaryPref[i].ToString() == "ISTEETOTAL")
                                    {
                                        lblDietry.Text = lblDietry.Text + " TeeTotal";
                                    }
                                    if (arrDietaryPref[i].ToString() == "ISVEGETERIAN")
                                    {
                                        lblDietry.Text = lblDietry.Text + " Vegeterian";
                                    }
                                    lblDietry.Text = lblDietry.Text + ",";
                                }
                                string str = lblDietry.Text;
                                str = str.Substring(0, str.Length - 1); // Removing last comma from text.
                                lblDietry.Text = str;
                            }
                        }
                        else
                        {
                            string[] arrDietaryPref = Helper.GetTripleDESEncryptedCookieValue("DietryPreferencesName").ToString().Split(',').ToArray();
                            if (arrDietaryPref.Count() > 0)
                            {
                                for (int i = 0; i <= arrDietaryPref.Length - 1; i++)
                                {
                                    lblDietry.Text = lblDietry.Text + arrDietaryPref[i].ToString();
                                    if (lblDietry.Text != string.Empty)
                                    {
                                        lblDietry.Text = lblDietry.Text + ", ";
                                    }
                                }
                                string str = lblDietry.Text;
                                str = str.Substring(0, str.Length - 1); // Removing last comma from text.
                                lblDietry.Text = str;
                            }
                        }
                    }
                    else
                    {
                        lblDietry.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.DeleteAllCookies();
                Logger.Write("FinalSubmitionScreen.aspx.cs:PageLoad():Exception occured while assigning cookie values to labels" + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                errqstring = "~/Kiosk/ErrorMessage.aspx?PgName=FinalSubmitionScreen&ctrlID=" + ctrlID + "&resID=" + cResID + "&imgID=" + imgID;
                Response.Redirect(errqstring, false);
            }
        }

        public void ShowConfiguredDOBHHAges()
        {
            Logger.Write("Start of FinalSubmitionScreen ShowConfiguredDOBHHAges()", "General");
            //string DOBHHAgesLabels = ConfigurationManager.AppSettings["ShowDOBandHHAgesControls"].ToString().ToUpper();
            //string DietryLables = ConfigurationManager.AppSettings["ShowDietryPrefControls"].ToString().ToUpper();
            string DOBHHAgesLabels = ConfigurationReader.GetStringConfigKeyToUpper("ShowDOBandHHAgesControls");
            string DietryLables = ConfigurationReader.GetStringConfigKeyToUpper("ShowDietryPrefControls");

            divDOB.Visible = divHousehold.Visible = divDietaryNeeds.Visible = false;
            try
            {
                string[] strDOBControls = null;
                if (DOBHHAgesLabels != string.Empty)
                {
                    strDOBControls = DOBHHAgesLabels.Split(',');
                    for (int i = 0; i < strDOBControls.Count(); i++)
                    {
                        switch (strDOBControls[i])
                        {
                            case "DATEOFBIRTH": divDOB.Visible = true; break;
                            case "HHAGES": divHousehold.Visible = true; break;
                        }
                    }
                }
                if (DOBHHAgesLabels != string.Empty)
                {
                    if (DOBHHAgesLabels.Contains("DATEOFBIRTH") && DOBHHAgesLabels.Contains("HHAGES"))
                    {
                        divDOB.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (DOBHHAgesLabels.Contains("DATEOFBIRTH") && (!DOBHHAgesLabels.Contains("HHAGES")))
                    {
                        divDOB.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (DOBHHAgesLabels.Contains("HHAGES") && (!DOBHHAgesLabels.Contains("DATEOFBIRTH")))
                    {
                        divHousehold.Attributes.Add("class", "leftcontent2 first");
                    }
                }
                if (DietryLables.ToString() != string.Empty)
                {
                    if ((!DOBHHAgesLabels.Contains("HHAGES")) && (!DOBHHAgesLabels.Contains("DATEOFBIRTH")))
                    {
                        divDietaryNeeds.Visible = true;
                        divDietaryNeeds.Attributes.Add("class", "leftcontent2 first");
                    }
                    else
                    {
                        divDietaryNeeds.Visible = true;
                    }
                }
                divEditDOB.Attributes.Add("class", "editLangPassport");
            }
            catch (Exception ex)
            {
                Logger.Write("Function: ShowConfiguredDOBHHAges()" + ex.Message.ToString(), "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while enabling configured labels for DOBHHAges Section.");
            }
        }

        public void ShowConfiguredLangSSNLabels()
        {
            Logger.Write("Start of FinalSubmitionScreen ShowConfiguredLangSSNLabels()", "General");
            //string LangPassportSSNLables = ConfigurationManager.AppSettings["ShowPassportSSNControls"].ToString().ToUpper();
            //string RaceLabels = ConfigurationManager.AppSettings["ShowRaces"].ToString().ToUpper();
            string LangPassportSSNLables = ConfigurationReader.GetStringConfigKeyToUpper("ShowPassportSSNControls");
            string RaceLabels = ConfigurationReader.GetStringConfigKeyToUpper("ShowRaces");

            divPreferredLanguage.Visible = divPassportNumber.Visible = divSSN.Visible = divRace.Visible = false;
            try
            {
                string[] strControls = null;
                if (LangPassportSSNLables.ToString() != string.Empty)
                {
                    strControls = LangPassportSSNLables.ToString().Split(',');
                    for (int i = 0; i < strControls.Count(); i++)
                    {
                        switch (strControls[i])
                        {
                            case "LANGUAGE": divPreferredLanguage.Visible = true; break;
                            case "PASSPORT": divPassportNumber.Visible = true; break;
                            case "SSN": divSSN.Visible = true; break;
                        }
                    }
                }
                if (LangPassportSSNLables != string.Empty)
                {
                    if (strControls[0] == "LANGUAGE")
                    {
                        divPreferredLanguage.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "PASSPORT" && (!LangPassportSSNLables.Contains("LANGUAGE")))
                    {
                        divPassportNumber.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "PASSPORT" && LangPassportSSNLables.Contains("LANGUAGE"))
                    {
                        divPreferredLanguage.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "PASSSPORT" && (LangPassportSSNLables.Contains("SSN")) || (!LangPassportSSNLables.Contains("SSN")))
                    {
                        divPassportNumber.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "SSN" && ((!LangPassportSSNLables.Contains("LANGUAGE")) && (!LangPassportSSNLables.Contains("PASSPORT"))))
                    {
                        divSSN.Attributes.Add("class", "leftcontent2 first");                 
                    }
                    else if (strControls[0] == "SSN" && ((LangPassportSSNLables.Contains("LANGUAGE")) && (LangPassportSSNLables.Contains("PASSPORT"))))
                    {
                        divPreferredLanguage.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "SSN" && LangPassportSSNLables.Contains("LANGUAGE"))
                    {
                        divPreferredLanguage.Attributes.Add("class", "leftcontent2 first");
                    }
                    else if (strControls[0] == "SSN" && LangPassportSSNLables.Contains("PASSPORT"))
                    {
                        divPassportNumber.Attributes.Add("class", "leftcontent2 first");
                    }
                }
                if (RaceLabels.ToString() != string.Empty)
                {
                    if ((!(LangPassportSSNLables.Contains("LANGUAGE"))) && (!(LangPassportSSNLables.Contains("PASSPORT"))))
                    {
                        divRace.Visible = true;
                        divRace.Attributes.Add("class", "leftcontent2 first");
                    }
                    else
                    {
                        divRace.Visible = true;
                    }
                }
                divEdit.Attributes.Add("class", "editLangPassport");
            }
            catch (Exception ex)
            {
                Logger.Write("Function: ShowConfiguredLangSSNLabels()" + ex.Message.ToString(), "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while enabling configured labels for PreferredLanguage Section.");
            }
        }

        private void GetConfiguredSections()
        {
            try
            {
                Logger.Write("Start of FinalSubmitionScreen GetConfiguredSections()", "General");
                string NameSection = ConfigurationReader.GetStringConfigKeyToUpper("ShowNameControls");
                string AddressSection = ConfigurationReader.GetStringConfigKeyToUpper("ShowAddressControls");
                string EmailSection = ConfigurationReader.GetStringConfigKeyToUpper("ShowContactDetailsControls");
                string PreferredLanguageSection = ConfigurationReader.GetStringConfigKeyToUpper("ShowPassportSSNControls");
                string RaceSection = ConfigurationReader.GetStringConfigKeyToUpper("ShowRaces");
                string DOBHHAges = ConfigurationReader.GetStringConfigKeyToUpper("ShowDOBandHHAgesControls");
                string DietryPreferencesID = ConfigurationReader.GetStringConfigKeyToUpper("ShowDietryPrefControls");

                lblNameSection.Value = NameSection;
                lblAddressSection.Value = AddressSection;
                lblEmailSection.Value = EmailSection.ToUpperInvariant();
                lblPreferredLanguageSection.Value = PreferredLanguageSection;
                lblRaceSection.Value = RaceSection;
                lblDOBHHAges.Value = DOBHHAges;
                lblDietaryPreferences.Value = DietryPreferencesID;
            }
            catch (Exception ex)
            {
                Logger.Write("Function: GetConfiguredSections()" + ex.Message.ToString(), "General", 1, 1, System.Diagnostics.TraceEventType.Error, "Exception occured while reading Configuration values in FinalSubmissionPage");
            }
        }

        protected void Cancel_Restart(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
        }
        protected void TC_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/TermsAndCondition.aspx?page=FinalSubmitionScreen", false);
        }
        protected void imgBack_Click(object sender, EventArgs e)
        {
            string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKey("FinalSubmitionScreen"));
            Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx", false);
        }
        protected void edit_DOB(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/WhatIsYourDOBAndHHAges.aspx?Page=Confirm", false);
        }
        protected void edit_Email(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/WhatIsYourContactDetails.aspx?Page=Confirm", false);
        }
        protected void edit_Address(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/WhatIsYourAdress.aspx?Page=Confirm", false);
        }
        protected void edit_Name(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/WhatIsYourTitleandName.aspx?Page=Confirm", false);
        }
        protected void edit_Lang(object sender, EventArgs e)
        {
            Response.Redirect("~/Kiosk/FurtherPersonalDetail1.aspx?Page=Confirm", false);
        }


        protected void imgPrint_Click(object sender, ImageClickEventArgs e)
        {
            Hashtable htCustomer = new Hashtable();
            string resultXml = string.Empty;
            XmlDocument resultDoc = null;
            Int64 dotcomID = 0; //This will be always zero for kiosk users. 
            int iCount = 0;
            Helper.Net35BasicAuthentication();
            Logger.Write("Start of FinalSubmitionScreen imgPrint_Click()", "General");
            JoinLoyaltyServiceClient loyaltyServiceClient = new JoinLoyaltyServiceClient();
            try
            {
                string strjoinroutecode = string.Empty;
                //Calling NGC Service to insert Customer Information into NGC Database.
                string updateXml = string.Empty;// Helper.HashTableToXML(htCustomer, "customer");
                string errorXml = string.Empty;

                if (hdnJoinRouteCode.Value != null && hdnJoinRouteCode.Value != string.Empty)
                {
                    strjoinroutecode = hdnJoinRouteCode.Value.ToString();
                }
                else
                {
                    strjoinroutecode = ConfigurationReader.GetStringConfigKey("JoinRouteCode");
                }

                string proCode = Helper.GetTripleDESEncryptedCookieValue("PromotionCode").ToString().Trim();
                //Check for Duplicate Account
                if (Helper.AccountDuplicationAndPromotionalCodeCheck(out updateXml, out errorXml, proCode))
                {
                    resultDoc = new XmlDocument();
                    resultXml = loyaltyServiceClient.AccountCreate(dotcomID, updateXml, strjoinroutecode, culture);
                    resultDoc.LoadXml(resultXml);
                    string sClubcardID = string.Empty;
                    string sClientID = string.Empty;
                    DataSet dsClubcard = new DataSet();
                    dsClubcard.ReadXml(new XmlNodeReader(resultDoc));
                    sClubcardID = dsClubcard.Tables["Clubcard"].Rows[0].ItemArray[0].ToString().Trim();
                    KioskReportingServiceClient KioskserviceClient = new KioskReportingServiceClient();
                    sClientID = Helper.GetTripleDESEncryptedCookieValue("ClientID").ToString().Trim();
                    
                    if (KioskserviceClient.SetKioskReport(sClubcardID, sClientID))
                    {
                    }
                    string qstring = "~/Kiosk/PrintClubcard.aspx?clubcardID=" + sClubcardID;
                    Response.Redirect(qstring, false);
                    dsClubcard = null;
                }
                else
                {
                    Helper.DeleteAllCookies(); //Delete cookies, since error page will restart the join process
                    Logger.Write("FinalSubmitionScreen.aspx.cs:PageLoad():NGC System shows that you are already a Clubcard member", "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                    errqstring = "~/Kiosk/ErrorMessage.aspx?PgName=FinalSubmitionScreen&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                    Response.Redirect(errqstring, false);
                }
            }
            catch (Exception ex)
            {
                Helper.DeleteAllCookies(); //Delete cookies, since error page will restart the join process
                Logger.Write("FinalSubmitionScreen.aspx.cs:PageLoad():" + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error, GlobalResources.ClubcardJionAtKioskMsg.ToString());
                errqstring = "~/Kiosk/ErrorMessage.aspx?PgName=FinalSubmitionScreen&ctrlID=" + ctrlID + "&resID=" + cResID + "&imgID=" + imgID;
                Response.Redirect(errqstring, false);
            }
            finally
            {
                if (loyaltyServiceClient != null)
                {
                    if (loyaltyServiceClient.State == CommunicationState.Faulted)
                    {
                        loyaltyServiceClient.Abort();
                    }
                    else if (loyaltyServiceClient.State != CommunicationState.Closed)
                    {
                        loyaltyServiceClient.Close();
                    }
                }
            }

        }
    }

}


