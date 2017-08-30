using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Xml;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.ClubcardService;
using System.Configuration;
using Tesco.Com.Framework.Services.Locator.Entities;
using System.Threading;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using CCODundeeApplication.JoinLoyaltyService;
using CCODundeeApplication.PreferenceServices;
using System.Text;
using System.Net.Mail;
using System.IO;
using CCODundeeApplication.App_Code;
using System.Web.Caching;


namespace CCODundeeApplication
{
    /// <summary>
    /// Description: To display Customer details and household details.
    /// It will display associate customer details also if any.
    /// User can update customer details in this page.
    /// Author: Sadanand Vama
    /// Date: 03/July/2010
    /// </summary>
    public partial class CustomerDetail : System.Web.UI.Page
    {
        #region Page level variables
        //Used in .aspx page for hiding/unhiding the controls
        protected string spanStyleFirstName0 = "display:none";
        protected string spanStyleMiddleName0 = "display:none";
        protected string spanStyleSurname0 = "display:none";
        protected string spanStyleDOB0 = "display:none";
        protected string spanStyleGender0 = "display:none";
        protected string spanStyleFirstName1 = "display:none";
        protected string spanStyleMiddleName1 = "display:none";
        protected string spanStyleSurname1 = "display:none";
        protected string spanStyleDOB1 = "display:none";
        protected string spanStyleGender1 = "display:none";
        protected string spanStyleAddress = "display:none";
        protected string spanStylePostCode = "display:none";
        protected string spanStylePhoneNumber = "display:none";
        //protected string spanStyleNoHHPersons = "display:none";
        //protected string spanStyleAge1 = "display:none";
        //protected string spanStyleAge2 = "display:none";
        //protected string spanStyleAge3 = "display:none";
        //protected string spanStyleAge4 = "display:none";
        //protected string spanStyleAge5 = "display:none";
        protected string errMsgFirstName = string.Empty;
        protected string errMsgMiddleName = string.Empty;
        string name3MaxValue = string.Empty;
        string name3MinValue = string.Empty;
        string name2MaxValue = string.Empty;
        string name2MinValue = string.Empty;
        protected string errMsgSurname = string.Empty;
        protected string errMsgDOB = string.Empty;
        protected string errMsgGender = string.Empty;
        protected string errMsgMobileNumber = string.Empty;
        protected string spanStyleMoblieNumber = "display:none";
        protected string errMsgEmailAddress = string.Empty;
        protected string spanStyleEmailAddress = "display:none";
        protected string errMsgPrimaryId = string.Empty;
        protected string spanStylePrimaryId = "display:none";
        protected string errMsgSecondaryId = string.Empty;
        protected string spanStyleSecondaryId = "display:none";
        protected string errMsgLanguage = string.Empty;
        protected string spanStyleLanguage = "display:none";
        protected string errMsgRace = string.Empty;
        protected string spanStyleRace = "display:none";
        protected string errMsgAssoSecondaryId = string.Empty;
        protected string spanStyleAssoSecondaryId = "display:none";
        protected string errMsgAssoPrimaryId = string.Empty;
        protected string spanStyleAssoPrimaryId = "display:none";
        protected string errMsgAssoLanguage = string.Empty;
        protected string spanStyleAssoLanguage = "display:none";
        protected string errMsgAssoRace = string.Empty;
        protected string spanStyleAssoRace = "display:none";
        protected string spanStyleDOBBT1 = "display:none";
        protected string spanStyleDOBBT2 = "display:none";
        protected string spanStyleDOBBT3 = "display:none";
        protected string spanStyleDOBBT4 = "display:none";
        protected string spanStyleDOBBT5 = "display:none";
        protected string spanStyleDOBBT6 = "display:none";
        protected string spanStyleDOBBT7 = "display:none";
        protected string spanStyleDOBBT8 = "display:none";
        protected string spanStyleDOBBT9 = "display:none";
        protected string spanStyleDOBBT10 = "display:none";
        //System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
        //NGC Changes
        protected string errMsgEmail = string.Empty;
        protected string spanEmail = "display:none";

        protected string errMsgAddress = string.Empty;
        protected string errMsgPostCode = string.Empty;
        protected string errMsgPhoneNumber = string.Empty;
        protected string errMsgEveningPhoneNumber = string.Empty;
        protected string spanStyleEveningPhoneNumber = "display:none";
        //protected string errMsgNoHHPersons = string.Empty;
        //protected string errMsgAge1 = string.Empty;
        //protected string errMsgAge2 = string.Empty;
        //protected string errMsgAge3 = string.Empty;
        //protected string errMsgAge4 = string.Empty;
        //protected string errMsgAge5 = string.Empty;
        protected string spanClassDOBDropDown0 = "dtFld";
        protected string spanClassDOBDropDown1 = "dtFld";
        protected string spanClassDOBDropDown2 = "dtFld";
        protected string spanClassDOBDropDown3 = "dtFld";
        protected string spanClassDOBDropDown4 = "dtFld";
        protected string spanClassDOBDropDown5 = "dtFld";
        protected string spanClassDOBDropDown6 = "dtFld";
        protected string spanClassDOBDropDown7 = "dtFld";
        protected string spanClassDOBDropDown8 = "dtFld";
        protected string spanClassDOBDropDown9 = "dtFld";
        protected string spanClassDOBDropDown10 = "dtFld";
        protected string spanClassDOBDropDown11 = "dtFld";
        protected string spanClassGender = "gender";
        protected string spanClassGender1 = "gender";
        protected string spanClassAddress = "dtAddress";
        protected string spanStyleAddressLine1 = "display:none";
        protected string spanStyleAddressLine2 = "display:none";
        protected string spanStyleAddressLine3 = "display:none";
        protected string spanStyleAddressLine4 = "display:none";
        protected string spanStyleAddressLine5 = "display:none";
        protected string errMsgAddressLine1 = string.Empty;
        protected string errMsgAddressLine2 = string.Empty;
        protected string errMsgAddressLine3 = string.Empty;
        protected string errMsgAddressLine4 = string.Empty;
        protected string errMsgAddressLine5 = string.Empty;
        bool MiddleName = false;
        bool Title = false;
        bool EveningPhonenumber = false;
        bool firstName = false;
        bool surname = false;
        bool gender = false;
        #region Main
        protected string errMsgMainEmailAddress = string.Empty;
        protected string spanStyleMainEmailAddress = "display:none";

        protected string errMsgMainMobileNumber = string.Empty;
        protected string spanStyleMainMoblieNumber = "display:none";

        protected string errMsgMainEveningPhoneNumber = string.Empty;
        protected string spanStyleMainEveningPhoneNumber = "display:none";

        protected string errMsgMainDaytimePhoneNumber = string.Empty;
        protected string spanStyleMainDaytimePhoneNumber = "display:none";
        #endregion

        #region Assoc
        protected string errMsgAssocEmailAddress = string.Empty;
        protected string spanStyleAssocEmailAddress = "display:none";

        protected string errMsgAssocMobileNumber = string.Empty;
        protected string spanStyleAssocMoblieNumber = "display:none";

        protected string errMsgAssocEveningPhoneNumber = string.Empty;
        protected string spanStyleAssocEveningPhoneNumber = "display:none";

        protected string errMsgAssocDaytimePhoneNumber = string.Empty;
        protected string spanStyleAssocDaytimePhoneNumber = "display:none";
        #endregion

        string culture = string.Empty;//ConfigurationManager.AppSettings["Culture"].ToString();
        bool enableProvince = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableProvince"].ToString());
        protected CustomerServiceClient customerObj = null;
        protected ClubcardServiceClient clubcardObj = null;
        protected JoinLoyaltyServiceClient serviceClient = null;
        Hashtable searchData = null;
        DataSet dsCustomer = null;
        XmlDocument resulDoc = null;
        string conditionXML = string.Empty;
        string resultXml = string.Empty;
        string errorXml = string.Empty;
        long customerID = 0;
        DataSet dsCustomerHouseholdStatus = null;
        int rowCount = 0;
        short iConfigValue = 0;

        DataSet dsAddressList = null;
        DataSet dsAddressDetails = null;
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        protected ArrayList buildingNoStreetListWithoutStreet = null;
        PreferenceServiceClient preferenceserviceClient = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
                    culture = Helper.GetTripleDESEncryptedCookieValue("Culture").ToString();
                }
                else
                    Response.Redirect("~/Default.aspx", false);               

                if (!IsPostBack)
                {

                    #region RoleCapabilityImplementation
                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            HtmlAnchor findCustomer = (HtmlAnchor)Master.FindControl("findCustomer");
                            HtmlAnchor cutomerDetails = (HtmlAnchor)Master.FindControl("cutomerDetails");
                            HtmlAnchor customerPreferences = (HtmlAnchor)Master.FindControl("customerPreferences");
                            HtmlAnchor customerPoints = (HtmlAnchor)Master.FindControl("customerPoints");
                            HtmlAnchor customerCards = (HtmlAnchor)Master.FindControl("customerCards");
                            HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                            HtmlAnchor christmasSaver = (HtmlAnchor)Master.FindControl("christmasSaver");
                            HtmlAnchor aAdmin = (HtmlAnchor)Master.FindControl("aAdmin");
                            HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                            HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                            HtmlAnchor agroups = (HtmlAnchor)Master.FindControl("agroups");
                            HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                            HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                            PlaceHolder plAdmin = (PlaceHolder)Master.FindControl("plAdmin");
                            HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                            HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                            HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            HtmlAnchor DeLinkAccount = (HtmlAnchor)Master.FindControl("DelinkAccounts");
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            HtmlAnchor findCoupon = (HtmlAnchor)Master.FindControl("findCoupon");
                            Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                            HtmlAnchor PointsearnedReport = (HtmlAnchor)Master.FindControl("PointsEarnedReport");

                            //accountUnlock.Visible = dsCapability.Tables[0].Columns.Contains("AccountUnlocking");
                            accountUnlock.Visible = ConfigurationManager.AppSettings.AllKeys.Contains("AccountUnlocking") && 
                                    ConfigurationManager.AppSettings["AccountUnlocking"].Equals("true");

                            if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                            {
                                PointsearnedReport.Disabled = false;
                            }
                            else
                            {
                                PointsearnedReport.Disabled = true;
                                PointsearnedReport.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("AccountOperationReports") != false)
                            {
                                AccountOperationReports.Disabled = false;
                            }
                            else
                            {
                                AccountOperationReports.Disabled = true;
                                AccountOperationReports.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("PromotionalCodeReport") != false)
                            {
                                PromotionalCodeReport.Disabled = false;
                            }
                            else
                            {
                                PromotionalCodeReport.Disabled = true;
                                PromotionalCodeReport.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("SearchCoupons") != false)
                            {
                                findCoupon.Disabled = false;
                            }
                            else
                            {
                                findCoupon.Disabled = true;
                                findCoupon.HRef = "";
                            }


                            if (dsCapability.Tables[0].Columns.Contains("DeLinkingAccount") != false)
                            {
                                DeLinkAccount.Visible = true;
                            }
                            else
                            {
                                DeLinkAccount.Visible = false;
                                DeLinkAccount.HRef = "";
                            }


                            if (dsCapability.Tables[0].Columns.Contains("editcardranges") != false)
                            {
                                CardRange.Disabled = false;
                            }
                            else
                            {
                                CardRange.Disabled = true;
                                CardRange.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("editcardtypes") != false)
                            {
                                CardTypes.Disabled = false;
                            }
                            else
                            {
                                CardTypes.Disabled = true;
                                CardTypes.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("editstores") != false)
                            {
                                Stores.Disabled = false;
                            }
                            else
                            {
                                Stores.Disabled = true;
                                Stores.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("CreateNewCustomer") != false)
                            {
                                Join.Disabled = false;
                            }
                            else
                            {
                                Join.Disabled = true;
                                Join.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("AddUser") != false)
                            {
                                AddUser.Disabled = false;
                            }
                            else
                            {
                                AddUser.Disabled = true;
                                AddUser.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("AddGroup") != false)
                            {
                                AddGroup.Disabled = false;
                            }
                            else
                            {
                                AddGroup.Disabled = true;
                                AddGroup.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("FindGroup") != false)
                            {
                                FindGroup.Disabled = false;
                            }
                            else
                            {
                                FindGroup.Disabled = true;
                                FindGroup.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("finduser") != false)
                            {
                                FindUser.Disabled = false;
                            }
                            else
                            {
                                FindUser.Disabled = true;
                                FindUser.HRef = "";
                            }


                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                            {
                                cutomerDetails.Disabled = false;
                                btnFindAddress.Enabled = false;
                                btnSaveAddress.Enabled = false;
                                btnConfirmCustomerDtls.Enabled = false;
                                //mainContent.Disabled = true;
                                ddlTitle0.Enabled = false;
                                ddlTitle1.Enabled = false;
                                txtFirstName0.Enabled = false;
                                txtFirstName1.Enabled = false;
                                txtSurname0.Enabled = false;
                                txtSurname1.Enabled = false;
                                txtInitial0.Enabled = false;
                                txtInitial1.Enabled = false;
                                txtAddressLine1.Enabled = false;
                                txtAge1.Enabled = false;
                                //txtAge2.Enabled = false;
                                //txtAge3.Enabled = false;
                                //txtAge4.Enabled = false;
                                //txtAge5.Enabled = false;
                                ddlAge2.Enabled = false;
                                ddlAge3.Enabled = false;
                                ddlAge4.Enabled = false;
                                ddlAge5.Enabled = false;
                                ddlAge6.Enabled = false;
                                txtNoofPeople.Enabled = false;
                                txtPhoneNumber.Enabled = false;
                                txtPostCode.Enabled = false;
                                txtStreet.Enabled = false;
                                txtTown.Enabled = false;
                                ddlAddress.Enabled = false;
                                ddlDay0.Enabled = false;
                                ddlDay1.Enabled = false;
                                ddlMonth0.Enabled = false;
                                ddlMonth1.Enabled = false;
                                ddlYear0.Enabled = false;
                                ddlYear1.Enabled = false;
                                radioFemale0.Enabled = false;
                                radioMale0.Enabled = false;
                                chkDiabetic0.Enabled = false;
                                chkTeeTotal0.Enabled = false;
                                radioVegeterian0.Enabled = false;
                                radioHalal0.Enabled = false;
                                radioKosher0.Enabled = false;
                                chkDiabetic1.Enabled = false;
                                chkTeeTotal1.Enabled = false;
                                radioMale1.Enabled = false;
                                radioFemale1.Enabled = false;
                                radioVegeterian1.Enabled = false;
                                radioHalal1.Enabled = false;
                                radioKosher1.Enabled = false;



                            }
                            else
                            {
                                cutomerDetails.Disabled = true;
                                cutomerDetails.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerPreferences") != false)
                            {
                                customerPreferences.Disabled = false;
                            }
                            else
                            {
                                customerPreferences.Disabled = true;
                                customerPreferences.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerPoints") != false)
                            {
                                customerPoints.Disabled = false;
                            }
                            else
                            {
                                customerPoints.Disabled = true;
                                customerPoints.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCards") != false)
                            {
                                customerCards.Disabled = false;
                            }
                            else
                            {
                                customerCards.Disabled = true;
                                customerCards.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                            {
                                customerCoupon.Disabled = false;
                            }
                            else
                            {
                                customerCoupon.Disabled = true;
                                customerCoupon.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("ViewChristmasSaver") != false)
                            {
                                christmasSaver.Disabled = false;
                            }
                            else
                            {
                                christmasSaver.Disabled = true;
                                christmasSaver.HRef = "";
                            }

                            //NGC COde

                            if (dsCapability.Tables[0].Columns.Contains("resetpassword") != false)
                            {
                                ResetPass.Disabled = false;
                            }
                            else
                            {
                                ResetPass.Disabled = true;
                                ResetPass.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("viewpoints") != false)
                            {
                                viewpoints.Disabled = false;
                            }
                            else
                            {
                                viewpoints.Disabled = true;
                                viewpoints.HRef = "";
                            }

                            //NGC COde

                            if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerDetails") != false)
                            {
                                cutomerDetails.Disabled = false;
                                //clearSel0.Visible = true;
                                //clearSel1.Visible = true;
                                btnFindAddress.Enabled = true;
                                btnSaveAddress.Enabled = true;
                                btnConfirmCustomerDtls.Enabled = true;
                                ddlTitle0.Enabled = true;
                                ddlTitle1.Enabled = true;
                                txtFirstName0.Enabled = true;
                                txtFirstName1.Enabled = true;
                                txtSurname0.Enabled = true;
                                txtSurname1.Enabled = true;
                                txtInitial0.Enabled = true;
                                txtInitial1.Enabled = true;
                                txtAddressLine1.Enabled = true;
                                txtAge1.Enabled = false;
                                //txtAge2.Enabled = true;
                                //txtAge3.Enabled = true;
                                //txtAge4.Enabled = true;
                                //txtAge5.Enabled = true;
                                ddlAge2.Enabled = true;
                                ddlAge3.Enabled = true;
                                ddlAge4.Enabled = true;
                                ddlAge5.Enabled = true;
                                ddlAge6.Enabled = true;
                                txtNoofPeople.Enabled = true;
                                txtPhoneNumber.Enabled = true;
                                txtPostCode.Enabled = true;
                                txtStreet.Enabled = true;
                                txtTown.Enabled = true;
                                ddlAddress.Enabled = true;
                                ddlDay0.Enabled = true;
                                ddlDay1.Enabled = true;
                                ddlMonth0.Enabled = true;
                                ddlMonth1.Enabled = true;
                                ddlYear0.Enabled = true;
                                ddlYear1.Enabled = true;
                                radioFemale0.Enabled = true;
                                radioMale0.Enabled = true;
                                chkDiabetic0.Enabled = true;
                                chkTeeTotal0.Enabled = true;
                                radioVegeterian0.Enabled = true;
                                radioHalal0.Enabled = true;
                                radioKosher0.Enabled = true;
                                chkDiabetic1.Enabled = true;
                                chkTeeTotal1.Enabled = true;
                                radioMale1.Enabled = true;
                                radioFemale1.Enabled = true;
                                radioVegeterian1.Enabled = true;
                                radioHalal1.Enabled = true;
                                radioKosher1.Enabled = true;

                                //CR13 changes
                                hdnUpdateCustomerStatusCapability.Value = "true";

                            }
                        }
                    }
                    #endregion

                    Helper.GetMonthDdl(ddlMonth0); //Load Month dropdown
                    Helper.GetYearDdl(ddlYear0); //Load Year Dropdown
                    Helper.GetMonthDdl(ddlMonth1); //Load Month dropdown
                    Helper.GetYearDdl(ddlYear1); //Load Year Dropdown
                    Helper.GetHouseholdYearsList(ddlAge2);//CCMCA-441
                    Helper.GetHouseholdYearsList(ddlAge3);
                    Helper.GetHouseholdYearsList(ddlAge4);
                    Helper.GetHouseholdYearsList(ddlAge5);
                    Helper.GetHouseholdYearsList(ddlAge6);


                    if (culture == "en-US")
                    {
                        imgTilte.Visible = false;
                        imgAddress.Visible = false;
                        imgGender.Visible = false;
                        imgPostcode.Visible = false;
                        imgHouseName.Visible = false;
                        imgEmail.Visible = true;
                        btnFindAddress.Attributes.Add("style", "display:none");
                        //liEmailUK.Attributes.Add("style", "display:none");
                        liemailUS.Attributes.Add("style", "display:block");
                        //divCustomerStatusForUSL.Attributes.Add("style", "display:block");
                        ddlYear0.Visible = false;
                        ddlYear0.Items.Add("1988");
                        ddlYear0.SelectedValue = "1988";
                        lblSurname.Visible = false;
                        lblLastName.Visible = true;
                        //lblSurname.InnerText = "Last Name:";


                    }

                    //To configure mandatoty Fields

                    //Configuration From DB for Mandotary fields
                    SetMandatoryConfigurations();

                    //To load the titles
                    customerObj = new CustomerServiceClient();
                    if (customerObj.GetTitles(out errorXml, out resultXml, out rowCount, culture))
                    {
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        DataSet dsTitles = new DataSet();
                        dsTitles.ReadXml(new XmlNodeReader(resulDoc));

                        #region Tittles
                        if (dsTitles.Tables.Count > 0)
                        {
                            if (dsTitles.Tables.Contains("Titles"))
                            {
                                //Load titles into the dropdown list of Main customer
                                ddlTitle0.DataSource = dsTitles.Tables["Titles"].DefaultView;
                                ddlTitle0.DataTextField = "TitleEnglish";
                                ddlTitle0.DataValueField = "TitleEnglish";
                                ddlTitle0.DataBind();

                                //Load titles into the dropdown list of Associate customer
                                ddlTitle1.DataSource = dsTitles.Tables["Titles"].DefaultView;
                                ddlTitle1.DataTextField = "TitleEnglish";
                                ddlTitle1.DataValueField = "TitleEnglish";
                                ddlTitle1.DataBind();
                            }
                        }

                        #endregion
                        if (hdnEditCustomerStatusSettings.Value.ToLower() == "true")
                        {
                            //CR13 Changes
                            #region CustomerUseStatus
                            if (dsTitles.Tables.Count > 1)
                            {
                                //Load CustomerStatus into the dropdown list of Main customer
                                ddlCustomerStatus.DataSource = dsTitles.Tables["Table3"].DefaultView;
                                ddlCustomerStatus.DataTextField = "CustomerUseStatusDescLocal";
                                ddlCustomerStatus.DataValueField = "CustomerUseStatusID";
                                ddlCustomerStatus.DataBind();
                                ddlCustomerStatus.SelectedValue = BusinessConstants.CUSTOMERUSESTATUS_ACTIVE.ToString();


                                //Load CustomerStatus into the dropdown list of Associate customer
                                ddlAssocCustStatus.DataSource = dsTitles.Tables["Table3"].DefaultView;
                                ddlAssocCustStatus.DataTextField = "CustomerUseStatusDescLocal";
                                ddlAssocCustStatus.DataValueField = "CustomerUseStatusID";
                                ddlAssocCustStatus.DataBind();
                                ddlAssocCustStatus.SelectedValue = BusinessConstants.CUSTOMERUSESTATUS_ACTIVE.ToString();


                                //Load EmailStatus into the dropdown list of Main customer
                                ddlEmailStatus.DataSource = dsTitles.Tables["Table5"].DefaultView;
                                ddlEmailStatus.DataTextField = "CustomerEMailStatusDescLocal";
                                ddlEmailStatus.DataValueField = "CustomerEMailStatusID";
                                ddlEmailStatus.DataBind();
                                ddlEmailStatus.SelectedValue = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE.ToString();

                                //Load EmailStatus into the dropdown list of Associate customer
                                ddlAssoEmailStatus.DataSource = dsTitles.Tables["Table5"].DefaultView;
                                ddlAssoEmailStatus.DataTextField = "CustomerEMailStatusDescLocal";
                                ddlAssoEmailStatus.DataValueField = "CustomerEMailStatusID";
                                ddlAssoEmailStatus.DataBind();
                                ddlAssoEmailStatus.SelectedValue = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE.ToString();
                                hdnAEmailStatus.Value = ddlAssoEmailStatus.SelectedValue;

                                //Load MobileStatus into the dropdown list of Main customer
                                ddlMobileStatus.DataSource = dsTitles.Tables["Table5"].DefaultView;
                                ddlMobileStatus.DataTextField = "CustomerEMailStatusDescLocal";
                                ddlMobileStatus.DataValueField = "CustomerEMailStatusID";
                                ddlMobileStatus.DataBind();
                                ddlMobileStatus.SelectedValue = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE.ToString();

                                //Load MobileStatus into the dropdown list of Associate customer
                                ddlAssoMobileStatus.DataSource = dsTitles.Tables["Table5"].DefaultView;
                                ddlAssoMobileStatus.DataTextField = "CustomerEMailStatusDescLocal";
                                ddlAssoMobileStatus.DataValueField = "CustomerEMailStatusID";
                                ddlAssoMobileStatus.DataBind();
                                ddlAssoMobileStatus.SelectedValue = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE.ToString();
                                hdnAMobileStatus.Value = ddlAssoMobileStatus.SelectedValue;

                                //Load MailStatus into the dropdown list of Main customer
                                ddlMailStatus.DataSource = dsTitles.Tables["Table4"].DefaultView;
                                ddlMailStatus.DataTextField = "CustomerMailStatusDescLocal";
                                ddlMailStatus.DataValueField = "CustomerMailStatusID";
                                ddlMailStatus.DataBind();
                                ddlMailStatus.SelectedValue = BusinessConstants.CUSTOMERMAILSTATUS_MAILABLE.ToString();

                                //Load titles into the dropdown list of Main customer

                            }
                            #endregion
                        }

                        if (hdnHideBusinessDetails.Value == "false")
                        {
                            if (dsTitles.Tables.Contains("BusniessType"))
                            {
                                ddlBusinessType.DataSource = dsTitles.Tables["BusniessType"].DefaultView;
                                ddlBusinessType.DataTextField = "BusinessTypeDescLocal";
                                ddlBusinessType.DataValueField = "BusinessTypeId";
                                ddlBusinessType.DataBind();
                            }
                        }

                    }

                    #region Trace Start
                    NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerDetail.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                    NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerDetail.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                    #endregion

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                    {
                        customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        clubcardObj = new ClubcardServiceClient();

                        if (clubcardObj.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            DataSet dsHHCustomers = new DataSet();
                            dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                            if (dsHHCustomers.Tables.Count > 0)
                            {
                                //Load customer personal details
                                LoadPersonalDetails(dsHHCustomers, customerID);
                            }
                        }

                        #region

                        if (!Convert.ToBoolean(hdnAddressGroupconfig.Value))
                        {
                            ////Add Javascript to the Save Changes button(top button)
                            btnConfirmCustomerDtls.Attributes.Add("onclick", "return ValidatePage('" + ddlTitle0.ClientID + "','"
                            + txtFirstName0.ClientID + "','"
                            + txtInitial0.ClientID + "','"
                            + txtSurname0.ClientID + "','"
                            + ddlDay0.ClientID + "','"
                            + ddlMonth0.ClientID + "','"
                            + ddlYear0.ClientID + "','"
                            + radioMale0.ClientID + "','"
                            + radioFemale0.ClientID + "','"
                            + ddlTitle1.ClientID + "','"
                            + txtFirstName1.ClientID + "','"
                            + txtInitial1.ClientID + "','"
                            + txtSurname1.ClientID + "','"
                            + ddlDay1.ClientID + "','"
                            + ddlMonth1.ClientID + "','"
                            + ddlYear1.ClientID + "','"
                            + radioMale1.ClientID + "','"
                            + radioFemale1.ClientID + "','"
                            + txtPostCode.ClientID + "','"
                            + txtPhoneNumber.ClientID + "','"
                            + txtNoofPeople.ClientID + "','"
                            + txtAge1.ClientID + "','"
                            + ddlAge2.ClientID + "','"
                            + ddlAge3.ClientID + "','"
                            + ddlAge4.ClientID + "','"
                            + ddlAge5.ClientID + "','"
                            + ddlAge6.ClientID + "','"
                                //+ txtAge2.ClientID + "','"
                                //+ txtAge3.ClientID + "','"
                                //+ txtAge4.ClientID + "','"
                                //+ txtAge5.ClientID + "','"
                            + hdnPostCodeNumber.ClientID + "','"
                            + ddlAddress.ClientID + "','"
                            + txtAddressLine1.ClientID + "','"
                            + txtStreet.ClientID + "','"
                            + txtTown.ClientID + "','"
                            + hdnNumberOfCustomers.ClientID + "','"
                            + lblSuccessMessage.ClientID + "','"
                            + dvAssociateCustomer.ClientID + "','"

                            + culture + "')");

                            //Add Javascript to the Save Changes button(bottom button)
                            btnConfirmCustomerDtls1.Attributes.Add("onclick", "return ValidatePage('" + ddlTitle0.ClientID + "','"
                            + txtFirstName0.ClientID + "','"
                            + txtInitial0.ClientID + "','"
                            + txtSurname0.ClientID + "','"
                            + ddlDay0.ClientID + "','"
                            + ddlMonth0.ClientID + "','"
                            + ddlYear0.ClientID + "','"
                            + radioMale0.ClientID + "','"
                            + radioFemale0.ClientID + "','"
                            + ddlTitle1.ClientID + "','"
                            + txtFirstName1.ClientID + "','"
                            + txtInitial1.ClientID + "','"
                            + txtSurname1.ClientID + "','"
                            + ddlDay1.ClientID + "','"
                            + ddlMonth1.ClientID + "','"
                            + ddlYear1.ClientID + "','"
                            + radioMale1.ClientID + "','"
                            + radioFemale1.ClientID + "','"
                            + txtPostCode.ClientID + "','"
                            + txtPhoneNumber.ClientID + "','"
                            + txtNoofPeople.ClientID + "','"
                            + txtAge1.ClientID + "','"
                                //+ txtAge2.ClientID + "','"
                                //+ txtAge3.ClientID + "','"
                                //+ txtAge4.ClientID + "','"
                                //+ txtAge5.ClientID + "','"
                            + ddlAge2.ClientID + "','" //CCMCA-441
                            + ddlAge3.ClientID + "','"
                            + ddlAge4.ClientID + "','"
                            + ddlAge5.ClientID + "','"
                            + ddlAge6.ClientID + "','"
                            + hdnPostCodeNumber.ClientID + "','"
                            + ddlAddress.ClientID + "','"
                            + txtAddressLine1.ClientID + "','"
                            + txtStreet.ClientID + "','"
                            + txtTown.ClientID + "','"
                            + hdnNumberOfCustomers.ClientID + "','"
                            + lblSuccessMessage.ClientID + "','"
                            + dvAssociateCustomer.ClientID + "','"

                            + culture + "')");
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }

                    btnFindAddress.Attributes.Add("onclick", "return ValidatePostCode('" + txtPostCode.ClientID + "','" + lblSuccessMessage.ClientID + "','" + culture + "');");
                    ddlAddress.Attributes.Add("onchange", "return OnSelectedIndexChange('" + ddlAddress.ClientID + "','" + hdnMailingAddressLine1.ClientID + "','" + hdnMailingAddressLine1Index.ClientID + "');");

                    ////CR13 changes
                    //if (hdnEditCustomerStatusSettings.Value == "True")
                    //{
                    //    String ConfirmMessage = GetLocalResourceObject("ConfirmMsg.Text").ToString();
                    //    btnConfirmCustomerDtls.Attributes.Add("onclick", "javascript:return " + "confirm('" + ConfirmMessage + "')");
                    //    btnConfirmCustomerDtls1.Attributes.Add("onclick", "javascript:return " + "confirm('" + ConfirmMessage + "')");
                    //}
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerDetail.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerDetail.Page_Load() CustomerID" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerDetail.Page_Load()- Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerDetail.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerDetail.Page_Load()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }

                if (clubcardObj != null)
                {
                    if (clubcardObj.State == CommunicationState.Faulted)
                    {
                        clubcardObj.Abort();
                    }
                    else if (clubcardObj.State != CommunicationState.Closed)
                    {
                        clubcardObj.Close();
                    }
                }
            }
        }
        public void LoadPrefernces(DataTable dtable, bool mainassocFlag)
        {
            ////Defect MKTG00007835: Prevent customer to delete emailAddress or mobilenumber when opted in for Email or SMS contact Starts
            //CustomerPreference objPreference = new CustomerPreference();
            //objPreference = preferenceserviceClient.ViewCustomerPreference(customerID, PreferenceType.NULL, true);
            //if (objPreference != null && objPreference.Preference != null && objPreference.Preference.Count > 0)
            //{
            //    // To load the Opted Preference
            //    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
            //    objPreferenceFilter = objPreference.Preference;
            //    string PrefID = string.Empty;
            //    List<string> PreferenceIds = new List<string>();
            //    foreach (var pref in objPreferenceFilter)
            //    {
            //        if (pref.POptStatus == OptStatus.OPTED_IN)
            //        {
            //            PrefID = pref.PreferenceID.ToString().Trim();
            //            PreferenceIds.Add(PrefID);
            //        }
            //    }

            //    // To load Preferences enabled for the country
            //    string sprefType = string.Empty;
            //    List<string> liPreferenceTypes = new List<string>();

            //    for (int i = 0; i < objPreference.Preference.Count; i++)
            //    {
            //        sprefType = objPreference.Preference[i].PreferenceID.ToString();
            //        liPreferenceTypes.Add(sprefType);
            //    }
            //    if (PreferenceIds.Contains(BusinessConstants.MOBILE_CONTACT.ToString())) //Email Pref
            //    {
            //       hdnMobile.Value = "false";
            //    }
            //    if (PreferenceIds.Contains(BusinessConstants.EMAIL_CONTACT.ToString())) //SMS Pref
            //    {
            //        hdnEmail.Value = "false";
            //    }
            //}
            ////Defect MKTG00007835: Prevent customer to delete emailAddress or mobilenumber when opted in for Email or SMS contact Ends

            List<string> strEmailDietaryPref = new List<string>();
            List<string> strEmailAllergyPref = new List<string>();
            List<string> strDietaryPrefList = new List<string>();
            List<string> strAllergyPrefList = new List<string>();
            if (mainassocFlag)
            {
                for (int i = 0; i < dtable.Rows.Count; i++)
                {
                    cblDietaryNeeds.Items.Add(dtable.Rows[i]["PreferenceDescLocal"].ToString());
                    cblDietaryNeeds.Items[i].Value = dtable.Rows[i]["PreferenceID"].ToString();
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN")
                    {
                        cblDietaryNeeds.Items[i].Selected = true;
                    }
                    //R1.6 Changes for Thank customer for opting less email statements
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN" && dtable.Rows[i]["PreferenceType"].ToString() == "1")
                    {
                        strEmailAllergyPref.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN" && dtable.Rows[i]["PreferenceType"].ToString() == "2")
                    {
                        strEmailDietaryPref.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["PreferenceType"].ToString() == "1")
                    {
                        strAllergyPrefList.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["PreferenceType"].ToString() == "2")
                    {
                        strDietaryPrefList.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    //R1.6 Changes for Thank customer for opting less email statements
                }
            }
            else
            {
                for (int i = 0; i < dtable.Rows.Count; i++)
                {
                    cblDietaryNeeds1.Items.Add(dtable.Rows[i]["PreferenceDescLocal"].ToString());
                    cblDietaryNeeds1.Items[i].Value = dtable.Rows[i]["PreferenceID"].ToString();
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN")
                    {
                        cblDietaryNeeds1.Items[i].Selected = true;
                    }
                    //R1.6 Changes for Thank customer for opting less email statements
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN" && dtable.Rows[i]["PreferenceType"].ToString() == "1")
                    {
                        strEmailAllergyPref.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["OptStatus"].ToString().Trim() == "OPTED_IN" && dtable.Rows[i]["PreferenceType"].ToString() == "2")
                    {
                        strEmailDietaryPref.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["PreferenceType"].ToString() == "1")
                    {
                        strAllergyPrefList.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    if (dtable.Rows[i]["PreferenceType"].ToString() == "2")
                    {
                        strDietaryPrefList.Add(dtable.Rows[i]["PreferenceID"].ToString());
                    }
                    //R1.6 Changes for Thank customer for opting less email statements
                }
            }
            //R1.6 Changes for Thank customer for opting less email statements
            hdnAllergyPrefList.Value = string.Join(",", strAllergyPrefList.ToArray());
            hdnDietaryPrefList.Value = string.Join(",", strDietaryPrefList.ToArray());
            hdnDietaryPref.Value = string.Join(",", strEmailDietaryPref.ToArray());
            hdnAllergyPref.Value = string.Join(",", strEmailAllergyPref.ToArray());
            //R1.6 Changes for Thank customer for opting less email statements
        }

        #region Initialize the culture

        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
                System.Globalization.DateTimeFormatInfo dateTimeFormat = new System.Globalization.DateTimeFormatInfo();
                dateTimeFormat.FullDateTimePattern = "dd/MM/yyyy hh:mm s";
                ci.DateTimeFormat = dateTimeFormat;

                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("Default.aspx", false);
        }
        #endregion

        /// <summary>
        /// Dynamic Preferences for Group Countries
        /// Author : Neeta Kewlani
        /// </summary>
        /// <returns></returns>
        public string DyanamicPreferences(CheckBoxList cblDietaryNeeds)
        {
            string dynamicPreferences = string.Empty;
            for (int i = 0; i < cblDietaryNeeds.Items.Count; i++)
            {
                if (cblDietaryNeeds.Items[i].Selected)
                {
                    dynamicPreferences = dynamicPreferences + cblDietaryNeeds.Items[i].Value + ",";
                }
            }
            return dynamicPreferences;
        }

        /// <summary>
        /// It updates the customer details
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnConfirmCustomerDtls_Click(object sender, ImageClickEventArgs e)
        {
            CustomerServiceClient service = null;
            Hashtable htCustomer = null;
            int noOfCustomers = Convert.ToInt32(hdnNumberOfCustomers.Value);
            bool blnAddressLine2 = true;
            bool blnAddressLine3 = true;
            bool blnAddressLine4 = true;
            string amendDateTime = string.Empty;
            string dateFormat = string.Empty;
            ClubDetails objClubDetails = new ClubDetails();

            try
            {
                xmlCapability = new XmlDocument();
                dsCapability = new DataSet();
                dateFormat = ConfigurationManager.AppSettings["DateDisplayFormat"].ToString();

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    //Check if user has update role.
                    if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerDetails") != false)
                    {
                        string noHHPersons = "0";
                        if (ValidatePersonalDetailsPage())
                        {
                            //Boolean ConfirmMsgResult = true;
                            //if (hdnEditCustomerStatusSettings.Value == "True")
                            //{
                            //    String ConfirmMessage = GetLocalResourceObject("ConfirmMsg.Text").ToString();



                            //    var confirmResult = System.Windows.Forms.MessageBox.Show(ConfirmMessage, ConfirmMessage, System.Windows.Forms.MessageBoxButtons.YesNo,System.Windows.Forms.MessageBoxIcon.Information,System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                            //    if (confirmResult==System.Windows.Forms.DialogResult.Yes)
                            //    {
                            //        ConfirmMsgResult =true;
                            //    }
                            //    else
                            //    { 
                            //        ConfirmMsgResult = false;
                            //    }
                            //}
                            //if (ConfirmMsgResult)
                            //{
                            bool istxtStRequired = true;
                            for (int cust = 0; cust < noOfCustomers; cust++)
                            {
                                DataTable dtPreference = new DataTable("Preference");
                                dtPreference.Columns.Add("PreferenceID", typeof(Int16));
                                dtPreference.Columns.Add("OptStatus", typeof(Enum));
                                dtPreference.Columns.Add("updateDateTime", typeof(DateTime), null);
                                dtPreference.Columns.Add("EmailSubject", typeof(string), null);

                                //For main customer.
                                if (cust == 0)
                                {
                                    if (hdnPrimaryCustID.Value != "")
                                    {
                                        htCustomer = new Hashtable();
                                        htCustomer["CustomerID"] = hdnPrimaryCustID.Value;
                                        if (hdnISTitle.Value == "true")
                                        {
                                            htCustomer["TitleEnglish"] = ddlTitle0.SelectedValue;
                                        }
                                        else
                                            htCustomer["TitleEnglish"] = string.Empty;
                                        htCustomer["Name1"] = txtFirstName0.Text.Trim();
                                        htCustomer["Name2"] = txtInitial0.Text.Trim();
                                        htCustomer["Name3"] = txtSurname0.Text.Trim();
                                        //NGC Change
                                        //Changes for CR13
                                        if (hdnEditCustomerStatusSettings.Value.ToLower() == "false")
                                        {
                                            htCustomer["CustomerUseStatusMain"] = hdnUseStatus.Value;
                                        }
                                        else
                                        {
                                            htCustomer["CustomerUseStatusMain"] = ddlCustomerStatus.SelectedValue;
                                        }
                                        //NGC Change 3.6 - Mobile and Email Status 
                                        htCustomer["SSN"] = txtPrimId.Text.ToString().Trim();
                                        htCustomer["PassportNo"] = txtSecId.Text.ToString().Trim();
                                        htCustomer["ISOLanguageCode"] = rdoLanguage.SelectedValue.ToString();
                                        if (!string.IsNullOrEmpty(ddlRace.SelectedValue.ToString()) && ddlRace.SelectedValue.ToString() != "- Select race -")
                                        {
                                            htCustomer["RaceID"] = ddlRace.SelectedValue.ToString();
                                        }
                                        else
                                        {
                                            htCustomer["RaceID"] = 0;

                                        }
                                        //CodeModified for CR13
                                        if (hdnEditCustomerStatusSettings.Value.ToLower() == "false")
                                        {
                                            if (!string.IsNullOrEmpty(txtMobileNumber.Text.Trim()))
                                            {
                                                htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                            }
                                            else
                                            {
                                                htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                            }

                                            if (!string.IsNullOrEmpty(txtEmailAddress.Text.Trim()))
                                            {
                                                htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                            }
                                            else
                                            {
                                                htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                            }


                                        }
                                        else
                                        {
                                            htCustomer["CustomerMobilePhoneStatus"] = ddlMobileStatus.SelectedValue;
                                            htCustomer["CustomerEmailStatus"] = ddlEmailStatus.SelectedValue;

                                            hdnAMobileStatus.Value = ddlAssoMobileStatus.SelectedValue;
                                            hdnAEmailStatus.Value = ddlAssoEmailStatus.SelectedValue;

                                        }
                                        hdnMEmailStatus.Value = htCustomer["CustomerEmailStatus"].ToString();
                                        hdnMMobileStatus.Value = htCustomer["CustomerMobilePhoneStatus"].ToString();
                                        //End of CR13 changes
                                        htCustomer["DynamicPreferences"] = "";
                                        htCustomer["Culture"] = culture;


                                        if ((ddlDay0.SelectedValue == "" && (ddlMonth0.SelectedValue == "" || ddlMonth0.SelectedValue == "- Select Month -")
                                            && (ddlYear0.SelectedValue == "" || ddlYear0.SelectedValue == "Year" || ddlYear0.SelectedValue == "1988")))
                                        {
                                            //DOB is empty then don't update as it not required field. Fix of Defect MKTG00003574
                                        }
                                        else
                                        {
                                            String TargetDateformat = ConfigurationManager.AppSettings["SpecifiedDateFormat"].ToString();
                                            string TargetLinker = ConfigurationManager.AppSettings["Linkerfordate"].ToString();
                                            string dateofbirth = string.Empty;
                                            if (TargetDateformat == "dd/MM/yyyy" || TargetDateformat == "d/MM/yyyy" || TargetDateformat == "dd/M/yyyy" || TargetDateformat == "d/M/yyyy")
                                            {
                                                htCustomer["DateOfBirth"] = ddlDay0.SelectedValue + TargetLinker + ddlMonth0.SelectedValue.ToString() + TargetLinker + ddlYear0.SelectedValue;
                                            }
                                            else if (TargetDateformat == "MM/dd/yyyy" || TargetDateformat == "M/dd/yyyy" || TargetDateformat == "MM/d/yyyy" || TargetDateformat == "M/d/yyyy")
                                            {
                                                htCustomer["DateOfBirth"] = ddlMonth0.SelectedValue + TargetLinker + ddlDay0.SelectedValue + TargetLinker + ddlYear0.SelectedValue;
                                                                                                
                                            }
                                            else if (TargetDateformat == "yyyy/dd/MM" || TargetDateformat == "yyyy/d/MM" || TargetDateformat == "yyyy/dd/M" || TargetDateformat == "yyyy/d/M")
                                            {
                                                htCustomer["DateOfBirth"] = ddlYear0.SelectedValue + TargetLinker + ddlDay0.SelectedValue + TargetLinker + ddlMonth0.SelectedValue;
                                                
                                            }
                                            else if (TargetDateformat == "yyyy/MM/dd" || TargetDateformat == "yyyy/M/dd" || TargetDateformat == "yyyy/MM/d" || TargetDateformat == "yyyy/M/d")
                                            {
                                                htCustomer["DateOfBirth"] = ddlYear0.SelectedValue + TargetLinker + ddlMonth0.SelectedValue + TargetLinker + ddlDay0.SelectedValue;
                                                
                                            }
                                        }

                                        if(MainGender.Visible)
                                        {
                                            htCustomer["Sex"] = radioFemale0.Checked ? "F" : "M"; 
                                        }

                                        //Set the dietary preferences
                                        htCustomer["Diabetic"] = chkDiabetic0.Checked ? "1" : "0";
                                        htCustomer["Teetotal"] = chkTeeTotal0.Checked ? "1" : "0";
                                        htCustomer["Vegetarian"] = radioVegeterian0.Checked ? "1" : "0";
                                        htCustomer["Halal"] = radioHalal0.Checked ? "1" : "0";
                                        htCustomer["Kosher"] = radioKosher0.Checked ? "1" : "0";
                                        //NGC Change
                                        if (culture == "en-US")
                                        {
                                            htCustomer["email_address"] = txtEmail.Text.Trim();
                                        }
                                        else
                                        {
                                            htCustomer["email_address"] = txtEmailAddress.Text.Trim();
                                            htCustomer["EmailAddress"] = txtEmailAddress.Text.Trim();
                                        }

                                        htCustomer["MobilePhoneNumber"] = txtMobileNumber.Text.Trim();
                                        htCustomer["evening_phone_number"] = txtEveningPhoneNumber.Text.Trim();
                                        htCustomer["mobile_phone_number"] = txtMobileNumber.Text.Trim();
                                        htCustomer["daytime_phone_number"] = txtPhoneNumber.Text;

                                        bool dietaryEmailSubjectAdded = false;
                                        bool allergyEmailSubjectAdded = false;
                                        for (int i = 0; i < cblDietaryNeeds.Items.Count; i++)
                                        {
                                            if (cblDietaryNeeds.Items[i].Selected)
                                            {
                                                DataRow newPreferencerow = dtPreference.NewRow();
                                                newPreferencerow["PreferenceID"] = Convert.ToInt16(cblDietaryNeeds.Items[i].Value).ToString();
                                                newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                                newPreferencerow["updateDateTime"] = DateTime.Now;
                                                //R1.6 Changes for Thank customer for opting less email statements
                                                if (hdnDietaryPrefList.Value != "")
                                                {
                                                    if ((!Array.Exists(hdnDietaryPref.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value) && !string.IsNullOrEmpty(hdnSendEmailForDietaryPref.Value)) && !dietaryEmailSubjectAdded)
                                                    {
                                                        newPreferencerow["EmailSubject"] = hdnSendEmailForDietaryPref.Value;
                                                        dietaryEmailSubjectAdded = true;
                                                    }
                                                }
                                                if (hdnAllergyPrefList.Value != "")
                                                {
                                                    if ((!Array.Exists(hdnAllergyPref.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value) && !string.IsNullOrEmpty(hdnSendEmailForAllergyPref.Value)) && !allergyEmailSubjectAdded)
                                                    {
                                                        newPreferencerow["EmailSubject"] = hdnSendEmailForAllergyPref.Value;
                                                        allergyEmailSubjectAdded = true;
                                                    }
                                                }
                                                //R1.6 Changes for Thank customer for opting less email statements
                                                dtPreference.Rows.Add(newPreferencerow);
                                            }
                                            else
                                            {
                                                DataRow newPreferencerow = dtPreference.NewRow();
                                                newPreferencerow["PreferenceID"] = Convert.ToInt16(cblDietaryNeeds.Items[i].Value).ToString();
                                                newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                                newPreferencerow["updateDateTime"] = DateTime.Now;
                                                dtPreference.Rows.Add(newPreferencerow);
                                            }
                                        }
                                        dtPreference.AcceptChanges();

                                        if (hdnHideBusinessDetails.Value == "false")
                                        {
                                            if (hdnHideBusinessType.Value == "false")
                                                htCustomer["BusinessType"] = ddlBusinessType.SelectedIndex;
                                            if (hdnHideBusniessName.Value == "false")
                                                htCustomer["BusinessName"] = TxtBusniessName.Text.Trim();

                                            //htCustomer["BusinessRegistrationNumber"] = lblBusniessRegNoVal.Text.Trim();
                                            if (hdnHideBusinessAddr1.Value == "false")
                                                htCustomer["BusinessAddressLine1"] = txtBusinessAddress1.Text.Trim();
                                            if (hdnHideBusinessAddr2.Value == "false")
                                                htCustomer["BusinessAddressLine2"] = txtBusinessAddress2.Text.Trim();
                                            if (hdnHideBusinessAddr3.Value == "false")
                                                htCustomer["BusinessAddressLine3"] = txtBusinessAddress3.Text.Trim();
                                            if (hdnHideBusinessAddr4.Value == "false")
                                                htCustomer["BusinessAddressLine4"] = txtBusinessAddress4.Text.Trim();
                                            if (hdnHideBusinessAddr5.Value == "false")
                                                htCustomer["BusinessAddressLine5"] = txtBusinessAddress5.Text.Trim();
                                            if (hdnHideBusinessAddr6.Value == "false")
                                                htCustomer["BusinessAddressLine6"] = txtBusinessAddress6.Text.Trim();
                                            if (hdnHideBusinessPostcode.Value == "false")
                                                htCustomer["BusinessAddressPostCode"] = txtBusinessPostcode.Text.Trim();
                                        }
                                    }
                                }
                                //For Associate customer.
                                else if (cust == 1)
                                {
                                    if (hdnAssociateCustID.Value != null)
                                    {
                                        htCustomer = new Hashtable();
                                        DateTime DOB;

                                        //May 2011 release changes
                                        if (hdnAssociateCustomerDiv.Value == "1")
                                        {
                                            htCustomer["CustomerID"] = hdnAssociateCustID.Value;
                                            // To Fix China Defect Title set Mr By Default and Dependent on Gender.
                                            if (hdnISTitle.Value == "true")
                                            {
                                                htCustomer["TitleEnglish"] = hdnddlTitle1.Value;
                                            }
                                            else
                                                htCustomer["TitleEnglish"] = string.Empty;
                                            htCustomer["Name1"] = hdntxtFirstName1.Value.Trim();
                                            htCustomer["Name2"] = hdntxtInitial1.Value.Trim();
                                            htCustomer["Name3"] = hdntxtSurname1.Value.Trim();
                                            if (hdnddlDay1.Value.Trim() != "" && hdnddlMonth1.Value.Trim() != "" && hdnddlYear1.Value.Trim() != "")
                                            {
                                                htCustomer["DateOfBirth"] = DateTime.Parse(hdnddlDay1.Value + "/" + hdnddlMonth1.Value + "/" + hdnddlYear1.Value);
                                            }
                                            ////NGC Change 3.6 - Mobile and Email Status 
                                            //Start: Changes for CR13
                                            if (hdnEditCustomerStatusSettings.Value.ToLower() == "false")
                                            {
                                                htCustomer["CustomerUseStatusMain"] = hdnUseStatus1.Value;

                                                if (!string.IsNullOrEmpty(txtAssocMobileNumber.Text.Trim()))
                                                {
                                                    htCustomer["CustomerMobilePhoneStatus"] =
                                                        BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                                }
                                                else
                                                {
                                                    htCustomer["CustomerMobilePhoneStatus"] =
                                                        BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                                }
                                                if (!string.IsNullOrEmpty(txtAssocEmailAddress.Text.Trim()))
                                                {
                                                    htCustomer["CustomerEmailStatus"] =
                                                        BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                                }
                                                else
                                                {
                                                    htCustomer["CustomerEmailStatus"] =
                                                        BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                                }

                                            }
                                            else
                                            {
                                                htCustomer["CustomerUseStatusMain"] = ddlAssocCustStatus.SelectedValue;
                                                htCustomer["CustomerMobilePhoneStatus"] = ddlAssoMobileStatus.SelectedValue;
                                                htCustomer["CustomerEmailStatus"] = ddlAssoEmailStatus.SelectedValue;
                                            }

                                            hdnAMobileStatus.Value = htCustomer["CustomerMobilePhoneStatus"].ToString();
                                            hdnAEmailStatus.Value = htCustomer["CustomerEmailStatus"].ToString();
                                            //End: CR13 

                                            htCustomer["SSN"] = txtAssoPrimId.Text.ToString().Trim();
                                            htCustomer["PassportNo"] = txtAssoSecId.Text.ToString().Trim();
                                            htCustomer["email_address"] = txtAssocEmailAddress.Text.Trim(); ;
                                            htCustomer["EmailAddress"] = txtAssocEmailAddress.Text.Trim();
                                            htCustomer["MobilePhoneNumber"] = txtAssocMobileNumber.Text.Trim();
                                            htCustomer["evening_phone_number"] = txtAssocEveningPhoneNumber.Text.Trim();
                                            htCustomer["mobile_phone_number"] = txtAssocMobileNumber.Text.Trim();
                                            htCustomer["daytime_phone_number"] = txtAssocDaytimePhoneNumber.Text;
                                            htCustomer["ISOLanguageCode"] = rdoAssoLanguage.SelectedValue.ToString();

                                            if (!string.IsNullOrEmpty(ddlAssoRace.SelectedValue.ToString()) && ddlAssoRace.SelectedValue.ToString() != "- Select race -")
                                            {
                                                htCustomer["RaceID"] = ddlAssoRace.SelectedValue.ToString();
                                            }
                                            else
                                                htCustomer["RaceID"] = 0;

                                            htCustomer["Sex"] = hdnGender.Value;
                                            //Set the dietary preferences
                                            htCustomer["Diabetic"] = hdnchkDiabetic1.Value;
                                            htCustomer["Teetotal"] = hdnchkTeeTotal1.Value;
                                            htCustomer["Vegetarian"] = hdnradioVegeterian1.Value;
                                            htCustomer["Halal"] = hdnradioHalal1.Value;
                                            htCustomer["Kosher"] = hdnradioKosher1.Value;
                                            htCustomer["DynamicPreferences"] = "";
                                            htCustomer["Culture"] = culture;
                                            //Assign the values to disabled fields
                                            if (hdnddlTitle1.Value.Trim() != "Unknown" && !String.IsNullOrEmpty(hdnddlTitle1.Value.Trim()))
                                            {
                                                ddlTitle1.SelectedValue = hdnddlTitle1.Value;
                                            }
                                            else
                                            {
                                                ddlTitle1.SelectedValue = "Unknown";
                                            }
                                            if (hdnddlDay1.Value.Trim() != "" && hdnddlMonth1.Value.Trim() != "" && hdnddlYear1.Value.Trim() != "")
                                            {
                                                ddlDay1.SelectedValue = hdnddlDay1.Value;
                                                ddlMonth1.SelectedValue = hdnddlMonth1.Value;
                                                ddlYear1.SelectedValue = hdnddlYear1.Value;
                                            }
                                            else
                                            {
                                                ddlDay1.SelectedValue = "";
                                                ddlMonth1.SelectedValue = "- Select Month -";
                                                ddlYear1.SelectedValue = "Year";
                                            }

                                            if (AssGender.Visible)
                                            {
                                                radioFemale1.Checked = hdnGender.Value.ToUpper() == "F";
                                                radioMale1.Checked = !radioFemale1.Checked;
                                            }

                                            //Set the dietary preferences
                                            if (hdnchkDiabetic1.Value == "1") chkDiabetic1.Checked = true;
                                            if (hdnchkTeeTotal1.Value == "1") chkTeeTotal1.Checked = true;
                                            if (hdnradioVegeterian1.Value == "1") radioVegeterian1.Checked = true;
                                            if (hdnradioHalal1.Value == "1") radioHalal1.Checked = true;
                                            if (hdnradioKosher1.Value == "1") radioKosher1.Checked = true;

                                        }
                                        else
                                        {
                                            htCustomer["CustomerID"] = hdnAssociateCustID.Value;
                                            // To Fix China Defect Title set Mr By Default and Dependent on Gender.
                                            if (hdnISTitle.Value == "true")
                                            {
                                                htCustomer["TitleEnglish"] = ddlTitle1.SelectedValue;
                                            }
                                            else
                                                htCustomer["TitleEnglish"] = string.Empty;
                                            htCustomer["Name1"] = txtFirstName1.Text.Trim();
                                            htCustomer["Name2"] = txtInitial1.Text.Trim();
                                            htCustomer["Name3"] = txtSurname1.Text.Trim();

                                            //NGC Change 3.6 - Mobile and Email Status 
                                            //Start: CR13 set status values to hash table htCustomer for saving
                                            if (hdnEditCustomerStatusSettings.Value.ToLower() == "false")
                                            {
                                                htCustomer["CustomerUseStatusMain"] = hdnUseStatus1.Value;
                                                if (!string.IsNullOrEmpty(txtAssocMobileNumber.Text.Trim()))
                                                {
                                                    htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                                }
                                                else
                                                {
                                                    htCustomer["CustomerMobilePhoneStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                                }
                                                if (!string.IsNullOrEmpty(txtAssocEmailAddress.Text.Trim()))
                                                {
                                                    htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE;
                                                }
                                                else
                                                {
                                                    htCustomer["CustomerEmailStatus"] = BusinessConstants.CUSTOMERMAILSTATUS_MISSING;
                                                }
                                            }
                                            else
                                            {
                                                htCustomer["CustomerUseStatusMain"] = ddlAssocCustStatus.SelectedValue;
                                                htCustomer["CustomerMobilePhoneStatus"] = ddlAssoMobileStatus.SelectedValue;
                                                htCustomer["CustomerEmailStatus"] = ddlAssoEmailStatus.SelectedValue;
                                            }

                                            hdnAEmailStatus.Value = htCustomer["CustomerEmailStatus"].ToString();
                                            hdnAMobileStatus.Value = htCustomer["CustomerMobilePhoneStatus"].ToString();
                                            //End:CR13 Changes
                                            htCustomer["SSN"] = txtAssoPrimId.Text.ToString().Trim();
                                            htCustomer["PassportNo"] = txtAssoSecId.Text.ToString().Trim();

                                            htCustomer["ISOLanguageCode"] = rdoAssoLanguage.SelectedValue.ToString();
                                            if (!string.IsNullOrEmpty(ddlAssoRace.SelectedValue.ToString()) && ddlAssoRace.SelectedValue.ToString() != "- Select race -")
                                            {
                                                htCustomer["RaceID"] = ddlAssoRace.SelectedValue.ToString();
                                            }
                                            else
                                                htCustomer["RaceID"] = 0;

                                            htCustomer["DynamicPreferences"] = DyanamicPreferences(cblDietaryNeeds1);
                                            htCustomer["Culture"] = culture;
                                            if ((ddlDay1.SelectedValue == "" && (ddlMonth1.SelectedValue == "" || ddlMonth1.SelectedValue == "- Select Month -")
                                                && (ddlYear1.SelectedValue == "" || ddlYear1.SelectedValue == "Year")))
                                            {
                                                //DOB is empty then don't update as it not required field. Fix of Defect MKTG00003574
                                            }
                                            else
                                            {
                                                //DOB = DateTime.Parse(ddlDay1.SelectedValue + "/" + ddlMonth1.SelectedValue + "/" + ddlYear1.SelectedValue);
                                                //htCustomer["DateOfBirth"] = DOB.ToLongDateString();
                                                String TargetDateformat = ConfigurationManager.AppSettings["SpecifiedDateFormat"].ToString();
                                                string TargetLinker = ConfigurationManager.AppSettings["Linkerfordate"].ToString();
                                                if (TargetDateformat == "dd/MM/yyyy" || TargetDateformat == "d/MM/yyyy" || TargetDateformat == "dd/M/yyyy" || TargetDateformat == "d/M/yyyy")
                                                {
                                                    htCustomer["DateOfBirth"] = ddlDay1.SelectedValue + TargetLinker + ddlMonth1.SelectedValue + TargetLinker + ddlYear1.SelectedValue;
                                                }
                                                else if (TargetDateformat == "MM/dd/yyyy" || TargetDateformat == "M/dd/yyyy" || TargetDateformat == "MM/d/yyyy" || TargetDateformat == "M/d/yyyy")
                                                {
                                                    htCustomer["DateOfBirth"] = ddlMonth1.SelectedValue + TargetLinker + ddlDay1.SelectedValue + TargetLinker + ddlYear1.SelectedValue;
                                                }
                                                else if (TargetDateformat == "yyyy/dd/MM" || TargetDateformat == "yyyy/d/MM" || TargetDateformat == "yyyy/dd/M" || TargetDateformat == "yyyy/d/M")
                                                {
                                                    htCustomer["DateOfBirth"] = ddlYear1.SelectedValue + TargetLinker + ddlDay1.SelectedValue + TargetLinker + ddlMonth1.SelectedValue;
                                                }
                                                else if (TargetDateformat == "yyyy/MM/dd" || TargetDateformat == "yyyy/M/dd" || TargetDateformat == "yyyy/MM/d" || TargetDateformat == "yyyy/M/d")
                                                {
                                                    htCustomer["DateOfBirth"] = ddlYear1.SelectedValue + TargetLinker + ddlMonth1.SelectedValue + TargetLinker + ddlDay1.SelectedValue;
                                                }
                                            }

                                            if (radioFemale1.Checked)
                                                htCustomer["Sex"] = "F";
                                            else
                                                htCustomer["Sex"] = "M";

                                            //Set the dietary preferences
                                            htCustomer["Diabetic"] = chkDiabetic1.Checked ? "1" : "0";
                                            htCustomer["Teetotal"] = chkTeeTotal1.Checked ? "1" : "0";
                                            htCustomer["Vegetarian"] = radioVegeterian1.Checked ? "1" : "0";
                                            htCustomer["Halal"] = radioHalal1.Checked ? "1" : "0";
                                            htCustomer["Kosher"] = radioKosher1.Checked ? "1" : "0";
                                            htCustomer["email_address"] = txtAssocEmailAddress.Text.Trim(); ;
                                            htCustomer["EmailAddress"] = txtAssocEmailAddress.Text.Trim();
                                            htCustomer["MobilePhoneNumber"] = txtAssocMobileNumber.Text.Trim();
                                            htCustomer["evening_phone_number"] = txtAssocEveningPhoneNumber.Text.Trim();
                                            htCustomer["mobile_phone_number"] = txtAssocMobileNumber.Text.Trim();
                                            htCustomer["daytime_phone_number"] = txtAssocDaytimePhoneNumber.Text;
                                        }
                                    }
                                    //Opt-in of Clubcard Email.
                                    bool dietaryEmailSubjectAdded = false;
                                    bool allergyEmailSubjectAdded = false;
                                    for (int i = 0; i < cblDietaryNeeds1.Items.Count; i++)
                                    {
                                        if (cblDietaryNeeds1.Items[i].Selected)
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = Convert.ToInt16(cblDietaryNeeds1.Items[i].Value).ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_IN;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            //R1.6 Changes for Thank customer for opting less email statements
                                            if (hdnDietaryPrefList.Value != "")
                                            {
                                                if ((!Array.Exists(hdnDietaryPref.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value) && !string.IsNullOrEmpty(hdnSendEmailForDietaryPref.Value)) && !dietaryEmailSubjectAdded)
                                                {
                                                    newPreferencerow["EmailSubject"] = hdnSendEmailForDietaryPref.Value;
                                                    dietaryEmailSubjectAdded = true;
                                                }
                                            }
                                            if (hdnAllergyPrefList.Value != "")
                                            {
                                                if ((!Array.Exists(hdnAllergyPref.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value) && !string.IsNullOrEmpty(hdnSendEmailForAllergyPref.Value)) && !allergyEmailSubjectAdded)
                                                {
                                                    newPreferencerow["EmailSubject"] = hdnSendEmailForAllergyPref.Value;
                                                    allergyEmailSubjectAdded = true;
                                                }
                                            }
                                            //R1.6 Changes for Thank customer for opting less email statements
                                            dtPreference.Rows.Add(newPreferencerow);
                                        }
                                        else
                                        {
                                            DataRow newPreferencerow = dtPreference.NewRow();
                                            newPreferencerow["PreferenceID"] = Convert.ToInt16(cblDietaryNeeds1.Items[i].Value).ToString();
                                            newPreferencerow["OptStatus"] = OptStatus.OPTED_OUT;
                                            newPreferencerow["updateDateTime"] = DateTime.Now;
                                            dtPreference.Rows.Add(newPreferencerow);
                                        }
                                    }
                                    dtPreference.AcceptChanges();

                                }

                                //Load address and other fields
                                if (ddlAddress.SelectedValue != string.Empty)
                                {
                                    if (hdnMailingAddressLine1.Value != string.Empty)
                                    {
                                        if (hdnMailingAddressLine1.Value.ToString() == ddlAddress.SelectedValue.ToString())
                                        {
                                            if (hdnMailingAddressLine1Index.Value != string.Empty)
                                            {
                                                Int32 index = Convert.ToInt32(hdnMailingAddressLine1Index.Value.ToString());
                                                if (index >= 1)
                                                {
                                                    index = index - 1;
                                                }
                                                string[] strSplitAddress = ddlbuildingNoStreetListWithoutStreet.Items[index].Text.Split(',');
                                                if (strSplitAddress[0].ToString().Trim().Length <= 4)
                                                {
                                                    htCustomer["MailingAddressLine1"] = strSplitAddress[0] + " " + strSplitAddress[1];
                                                    istxtStRequired = false;

                                                }
                                                else
                                                {
                                                    htCustomer["MailingAddressLine1"] = strSplitAddress[0].ToString().Trim().ToUpper();

                                                    if (strSplitAddress[1].ToString().Trim().ToUpper() == txtStreet.Text.ToString().Trim().ToUpper())
                                                    {
                                                        istxtStRequired = true;
                                                    }
                                                    else
                                                    {
                                                        istxtStRequired = true;
                                                        txtStreet.Text = strSplitAddress[1].ToString().Trim().ToUpper();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                htCustomer["MailingAddressLine1"] = ddlAddress.SelectedValue;
                                            }
                                        }
                                        else
                                        {
                                            htCustomer["MailingAddressLine1"] = ddlAddress.SelectedValue;
                                        }
                                    }
                                    else
                                    {
                                        htCustomer["MailingAddressLine1"] = ddlAddress.SelectedValue;
                                    }
                                }
                                else
                                {
                                    if (culture == "en-GB")
                                    {
                                        htCustomer["MailingAddressLine1"] = txtAddressLine1.Text.Trim();
                                        htCustomer["MailingAddressLine1"] = txtAddressLine1.Text.ToString().Trim().ToUpper();
                                        string[] arrstrStreet = null;
                                        string strStreet = string.Empty;

                                        string[] arrStreet = txtAddressLine1.Text.Trim().Split(' ').ToArray();

                                        if (arrStreet.Length > 1)
                                        {
                                            arrstrStreet = new string[arrStreet.Length];
                                            for (int i = 1; i < arrStreet.Length; i++)
                                            {
                                                if (!String.IsNullOrEmpty(txtAddressLine1.Text.Trim().Split(' ')[i]))
                                                {
                                                    arrstrStreet[i] = txtAddressLine1.Text.Trim().Split(' ')[i].ToString().Trim().ToUpper();
                                                }
                                            }
                                            for (int j = 1; j < arrstrStreet.Length; j++)
                                            {
                                                if (arrstrStreet[j] != null)
                                                {
                                                    strStreet = strStreet + " " + arrstrStreet[j].ToString();
                                                }
                                            }

                                            if (strStreet.Trim() == txtStreet.Text.ToString().Trim().ToUpper())
                                            {
                                                istxtStRequired = false;
                                            }
                                        }
                                    }
                                    //Added to Remove upper case from Address for Microsite
                                    else if (culture != "en-GB")
                                    {
                                        htCustomer["MailingAddressLine1"] = txtAddressLine1.Text.Trim();
                                        htCustomer["MailingAddressLine1"] = txtAddressLine1.Text.ToString().Trim();
                                        string[] arrstrStreet = null;
                                        string strStreet = string.Empty;

                                        string[] arrStreet = txtAddressLine1.Text.Trim().Split(' ').ToArray();

                                        if (arrStreet.Length > 1)
                                        {
                                            arrstrStreet = new string[arrStreet.Length];
                                            for (int i = 1; i < arrStreet.Length; i++)
                                            {
                                                if (!String.IsNullOrEmpty(txtAddressLine1.Text.Trim().Split(' ')[i]))
                                                {
                                                    arrstrStreet[i] = txtAddressLine1.Text.Trim().Split(' ')[i].ToString().Trim();
                                                }
                                            }
                                            for (int j = 1; j < arrstrStreet.Length; j++)
                                            {
                                                if (arrstrStreet[j] != null)
                                                {
                                                    strStreet = strStreet + " " + arrstrStreet[j].ToString();
                                                }
                                            }

                                            if (strStreet.Trim() == txtStreet.Text.ToString().Trim())
                                            {
                                                istxtStRequired = false;
                                            }
                                        }
                                    }
                                }

                                if (culture == "en-GB")
                                {
                                    if (istxtStRequired)
                                    {
                                        if (txtStreet.Text.ToString().Trim() != string.Empty)
                                        {
                                            htCustomer["MailingAddressLine2"] = txtStreet.Text.ToString().Trim().ToUpper();
                                        }
                                        else
                                        {
                                            blnAddressLine2 = false;
                                        }
                                    }
                                    else
                                    {
                                        blnAddressLine2 = false;
                                        txtStreet.Text = string.Empty;
                                    }
                                    if (txtLocality.Text.ToString() != string.Empty)
                                    {
                                        htCustomer["MailingAddressLine3"] = txtLocality.Text.ToString().Trim().ToUpper();
                                    }
                                    else
                                    {
                                        blnAddressLine3 = false;
                                    }
                                    if (txtTown.Text.ToString() != string.Empty)
                                    {
                                        htCustomer["MailingAddressLine4"] = txtTown.Text.ToString().Trim().ToUpper();
                                    }
                                    else
                                    {
                                        blnAddressLine4 = false;
                                    }
                                    //if (txtCountyDetails.Text.ToString() != string.Empty)
                                    //{
                                    //    htCustomer["MailingAddressLine5"] = txtCountyDetails.Text.ToString().Trim().ToUpper();
                                    //}
                                    //CSC ThaiLand change: Replaced AddressLine5 text box (MailingAddressLine5) with Dropdown list
                                    if (enableProvince)
                                    {
                                        if (!string.IsNullOrEmpty(ddlProvince.SelectedValue.ToString()) && ddlProvince.SelectedIndex != 0)
                                        {
                                            htCustomer["MailingAddressLine5"] = ddlProvince.SelectedValue.ToString();
                                        }
                                        else
                                        {
                                            htCustomer["MailingAddressLine5"] = "-1";
                                        }
                                    }
                                    else
                                    {
                                        if (txtCountyDetails.Text.ToString() != string.Empty)
                                        {
                                            htCustomer["MailingAddressLine5"] = txtCountyDetails.Text.ToString().Trim().ToUpper();
                                        }
                                    }
                                    if (!blnAddressLine2)
                                    {
                                        if (blnAddressLine3)
                                        {
                                            htCustomer["MailingAddressLine2"] = txtLocality.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine3"] = txtTown.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine4"] = string.Empty;
                                        }
                                        else if (!blnAddressLine3)
                                        {
                                            htCustomer["MailingAddressLine2"] = txtTown.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine4"] = string.Empty;
                                        }
                                    }
                                    else if (!blnAddressLine3)
                                    {
                                        if (blnAddressLine4)
                                        {
                                            htCustomer["MailingAddressLine3"] = txtTown.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine4"] = txtCountyDetails.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine5"] = string.Empty;
                                        }
                                        else if (!blnAddressLine4)
                                        {
                                            htCustomer["MailingAddressLine3"] = txtCountyDetails.Text.ToString().Trim().ToUpper();
                                            htCustomer["MailingAddressLine5"] = string.Empty;
                                        }
                                    }
                                }
                                //else if (Culture != "en-GB")
                                //{
                                //    if (istxtStRequired)
                                //    {
                                //        if (txtStreet.Text.ToString().Trim() != string.Empty)
                                //        {
                                //            htCustomer["MailingAddressLine2"] = txtStreet.Text.ToString().Trim();
                                //        }
                                //        else
                                //        {
                                //            blnAddressLine2 = false;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        blnAddressLine2 = false;
                                //        txtStreet.Text = string.Empty;
                                //    }
                                //    if (txtLocality.Text.ToString() != string.Empty)
                                //    {
                                //        htCustomer["MailingAddressLine3"] = txtLocality.Text.ToString().Trim();
                                //    }
                                //    else
                                //    {
                                //        blnAddressLine3 = false;
                                //    }
                                //    if (txtTown.Text.ToString() != string.Empty)
                                //    {
                                //        htCustomer["MailingAddressLine4"] = txtTown.Text.ToString().Trim();
                                //    }
                                //    else
                                //    {
                                //        blnAddressLine4 = false;
                                //    }
                                //    if (txtCountyDetails.Text.ToString() != string.Empty)
                                //    {
                                //        htCustomer["MailingAddressLine5"] = txtCountyDetails.Text.ToString().Trim();
                                //    }
                                //    if (!blnAddressLine2)
                                //    {
                                //        if (blnAddressLine3)
                                //        {
                                //            htCustomer["MailingAddressLine2"] = txtLocality.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine3"] = txtTown.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine4"] = string.Empty;
                                //        }
                                //        else if (!blnAddressLine3)
                                //        {
                                //            htCustomer["MailingAddressLine2"] = txtTown.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine4"] = string.Empty;
                                //        }
                                //    }
                                //    else if (!blnAddressLine3)
                                //    {
                                //        if (blnAddressLine4)
                                //        {
                                //            htCustomer["MailingAddressLine3"] = txtTown.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine4"] = txtCountyDetails.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine5"] = string.Empty;
                                //        }
                                //        else if (!blnAddressLine4)
                                //        {
                                //            htCustomer["MailingAddressLine3"] = txtCountyDetails.Text.ToString().Trim();
                                //            htCustomer["MailingAddressLine5"] = string.Empty;
                                //        }
                                //    }

                                //}
                                if (hdnAddressGroupconfig.Value == "true") // PAF disabled
                                {

                                    htCustomer["MailingAddressLine2"] = txtStreet.Text.ToString().Trim();
                                    htCustomer["MailingAddressLine3"] = txtLocality.Text.ToString().Trim();
                                    htCustomer["MailingAddressLine4"] = txtTown.Text.ToString().Trim();
                                    if (enableProvince)
                                    {
                                        if (ddlProvince.SelectedIndex != 0)
                                            htCustomer["MailingAddressLine5"] = ddlProvince.SelectedValue.ToString();
                                        else
                                            htCustomer["MailingAddressLine5"] = "-1";
                                    }
                                    else
                                        htCustomer["MailingAddressLine5"] = txtCountyDetails.Text.ToString().Trim();

                                }
                                if (hdnHidepostcodeFields.Value != "0")
                                {
                                    htCustomer["MailingAddressPostCode"] = txtPostCode.Text;
                                }
                                else
                                {
                                    htCustomer["MailingAddressPostCode"] = string.Empty;
                                }

                                //**************************************************************************
                                //To set Customer Mail Status as Deliverable,Missing and INError
                                //**************************************************************************

                                ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                                HtmlControl spnAddressError = (HtmlControl)leftNav.FindControl("spnAddressError");
                                //Changes for CR13
                                //Thailand changes for CCMCA-4504 
                                string countryValue = enableProvince ? txtCountyDetails.Text.ToString() : ddlProvince.SelectedValue.ToString();
                                if (hdnEditCustomerStatusSettings.Value.ToLower() == "false")
                                {
                                    if (!String.IsNullOrEmpty(txtPostCode.Text.ToString()) || !String.IsNullOrEmpty(txtAddressLine1.Text.ToString())
                                        || !String.IsNullOrEmpty(txtStreet.Text.ToString()) || enableProvince ? ddlProvince.SelectedIndex != 0 : !String.IsNullOrEmpty(txtCountyDetails.Text.ToString())
                                            || !String.IsNullOrEmpty(txtTown.Text.ToString()))
                                    {
                                        if (String.Equals(hdnChkPostcode.Value, txtPostCode.Text.Trim()) && String.Equals(hdnChkAddressline1.Value, txtAddressLine1.Text.Trim())
                                        && String.Equals(hdnChkAddressline2.Value, txtStreet.Text.Trim()) && String.Equals(hdnChkAddressline3.Value, txtLocality.Text.Trim())
                                        && String.Equals(hdnChkAddressline4.Value, txtTown.Text.Trim()) && String.Equals(hdnChkAddressline5.Value, countryValue))
                                        {
                                            htCustomer["CustomerMailStatus"] = hdnMailStatus.Value;
                                        }
                                        else
                                        {
                                            htCustomer["CustomerMailStatus"] = BusinessConstants.CUSTOMERMAILADDSTATUS_DELIVERABLE;
                                            spnAddressError.Visible = false;
                                        }

                                    }
                                    else
                                    {
                                        htCustomer["CustomerMailStatus"] = BusinessConstants.CUSTOMERMAILADDSTATUS_MISSING;
                                        spnAddressError.Visible = false;
                                    }
                                }
                                else
                                {
                                    htCustomer["CustomerMailStatus"] = ddlMailStatus.SelectedValue;
                                }
                                hdnMailStatus.Value = htCustomer["CustomerMailStatus"].ToString();
                                //End:CR13 Changes
                                htCustomer["PromotionCode"] = string.Empty;
                                htCustomer["BusinessLineNumber"] = string.Empty;
                                htCustomer["Culture"] = culture;
                                //CCMCA-441
                                if (txtNoofPeople.Text.Trim() != string.Empty)
                                {
                                    noHHPersons = txtNoofPeople.Text.Trim();
                                    htCustomer["number_of_household_members"] = Convert.ToInt16(noHHPersons);

                                    for (int i = 2, j = 1; i <= 6; i++)
                                    {
                                        ContentPlaceHolder _mainMaster = (ContentPlaceHolder)Page.Master.Controls[0].FindControl("PageContainer");
                                        //TextBox txtAge = (TextBox)_mainMaster.FindControl("txtAge" + i);
                                        DropDownList ddlAge = (DropDownList)_mainMaster.FindControl("ddlAge" + i);

                                        //if (i <= Convert.ToInt16(noHHPersons))
                                        //{
                                        //    if (txtAge.Text.Trim() != "")
                                        //    {
                                        //        txtAge.Text = Convert.ToInt16(txtAge.Text).ToString();

                                        //        htCustomer["family_member_" + j + "_dob"] = GetDateOfBirth(Convert.ToInt16(txtAge.Text)).ToString();
                                        //        j++;
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    txtAge.Text = "";
                                        //}
                                        //    }
                                        //}
                                        if (ddlAge.SelectedValue.ToUpper() != "YEAR")
                                        {

                                            htCustomer["family_member_" + j + "_dob"] =
                                                DateTime.Parse((ddlAge.SelectedValue) + "/1/1").ToString();


                                            j++;

                                        }
                                    }
                                }
                                else
                                    htCustomer["number_of_household_members"] = 0;

                                //Prepare the parameters for service call
                                string DuplicateXml = Helper.HashTableToXML(htCustomer, "customer");
                                string updateXml = Helper.HashTableToXML(htCustomer, "customer");
                                long CustomerID; string errorXml;
                                string consumer = string.Empty;


                                consumer = Helper.GetTripleDESEncryptedCookieValue("UserName").ToString();
                                #region Trace Start
                                NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerDetail.btnConfirmCustomerDtls_Click() Input Xml" + consumer);
                                NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerDetail.btnConfirmCustomerDtls_Click() Input Xml" + consumer);
                                #endregion

                                service = new CustomerServiceClient();
                                //NGC 3.6 Duplication Check
                                serviceClient = new JoinLoyaltyServiceClient();
                                if (serviceClient.AccountDuplicateCheck(out resultXml, DuplicateXml))
                                {
                                    resulDoc = new XmlDocument();
                                    resulDoc.LoadXml(resultXml);
                                    DataSet dsJoin = new DataSet();
                                    dsJoin.ReadXml(new XmlNodeReader(resulDoc));
                                    //CCO Service call to update customer details
                                    if (dsJoin.Tables["Duplicate"].Rows[0].ItemArray[0].ToString().Trim() == "0" && dsJoin.Tables["Duplicate"].Rows[0].ItemArray[2].ToString().Trim() == "0")
                                    {
                                        ////CR13 changes
                                        //String ConfirmMessage = GetLocalResourceObject("ConfirmMsg.Text").ToString();
                                        //String alert;
                                        ////btnConfirmCustomerDtls.Attributes.Add("onclick", "javascript:return " + "confirm('" + ConfirmMessage + "')");
                                        //alert = "<script> if (confirm(' " + ConfirmMessage + " ')){window.close()}</script>";
                                        //Page.RegisterStartupScript("key", alert);
                                        ////CR13
                                        //String ConfirmMessage = GetLocalResourceObject("ConfirmMsg.Text").ToString();
                                        //String alertCmd;
                                        ////btnConfirmCustomerDtls.Attributes.Add("onclick", "javascript:return " + "confirm('" + ConfirmMessage + "')");
                                        //alertCmd = "javascript:return " + "confirm('" + ConfirmMessage + "')";//"alert('" + ConfirmMessage + "');";
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", alertCmd, true);


                                        if (service.UpdateCustomerDetails(out errorXml, out CustomerID, updateXml, consumer))
                                        {
                                            amendDateTime = DateTime.UtcNow.ToString(dateFormat);
                                            preferenceserviceClient = new PreferenceServiceClient();
                                            CustomerPreference objcustPref = new CustomerPreference();
                                            if (cust == 0)
                                            {
                                                objcustPref.CustomerID = Convert.ToInt64(hdnPrimaryCustID.Value);
                                            }
                                            else
                                            {
                                                objcustPref.CustomerID = Convert.ToInt64(hdnAssociateCustID.Value);
                                            }
                                            List<CustomerPreference> preferencelist = new List<CustomerPreference>();
                                            if (dtPreference.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dtPreference.Rows.Count; i++)
                                                {
                                                    CustomerPreference custpref = new CustomerPreference
                                                    {
                                                        PreferenceID = Convert.ToInt16(dtPreference.Rows[i][0].ToString().Trim()),
                                                        POptStatus = (OptStatus)Enum.Parse(typeof(OptStatus), dtPreference.Rows[i][1].ToString().Trim() != "" ? dtPreference.Rows[i][1].ToString().Trim() : "2"),
                                                        UpdateDateTime = Convert.ToDateTime(dtPreference.Rows[i][2].ToString().Trim()),
                                                        EmailSubject = dtPreference.Rows[i][3].ToString().Trim()

                                                    };
                                                    preferencelist.Add(custpref);
                                                }
                                            }
                                            objcustPref.Preference = preferencelist;
                                            objcustPref.Culture = culture;
                                            objcustPref.UserID = consumer;

                                            DataSet dsMyAccountDetails = new DataSet();
                                            bool boolResult = false;
                                            htCustomer["TitleEnglish"] = hdnddlTitle1.Value;
                                            htCustomer["Name1"] = hdntxtFirstName1.Value.Trim();
                                            htCustomer["Name2"] = hdntxtInitial1.Value.Trim();
                                            htCustomer["Name3"] = hdntxtSurname1.Value.Trim();
                                            CustomerDetails objCustomerDetails = new CustomerDetails();
                                            objCustomerDetails.EmailId = htCustomer["email_address"].ToString().Trim();
                                            objCustomerDetails.Surname = htCustomer["Name2"].ToString().Trim();
                                            objCustomerDetails.Title = htCustomer["TitleEnglish"].ToString().Trim();
                                            objCustomerDetails.Firstname = htCustomer["Name1"].ToString().Trim();

                                            //To get the clubcard number 
                                            clubcardObj = new ClubcardServiceClient();

                                            boolResult = clubcardObj.GetMyAccountDetails(out errorXml, out resultXml, Convert.ToInt64(objcustPref.CustomerID), culture);
                                            if (boolResult)
                                            {
                                                if (resultXml != "" && resultXml != "<NewDataSet />")
                                                {
                                                    resulDoc = new XmlDocument();
                                                    resulDoc.LoadXml(resultXml);
                                                    dsMyAccountDetails.ReadXml(new XmlNodeReader(resulDoc));

                                                    //to assign the retieved values to the related fields.
                                                    if (dsMyAccountDetails.Tables.Count != 0)
                                                    {
                                                        if (dsMyAccountDetails.Tables[0].Columns.Contains("ClubcardID") != false)
                                                        {
                                                            if (dsMyAccountDetails.Tables[0].Rows[0]["ClubcardID"].ToString() != "")
                                                            {
                                                                objCustomerDetails.CardNumber = dsMyAccountDetails.Tables[0].Rows[0]["ClubcardID"].ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }


                                            preferenceserviceClient.MaintainCustomerPreference(objcustPref.CustomerID, objcustPref, objCustomerDetails);
                                            objClubDetails.UserID = consumer;
                                            objClubDetails.JoinDate = DateTime.Now;
                                            objClubDetails.Culture = culture;
                                            if (cust == 0)
                                            {
                                                customerID = Convert.ToInt64(hdnPrimaryCustID.Value);
                                            }
                                            else
                                            {
                                                customerID = Convert.ToInt64(hdnAssociateCustID.Value);
                                            }


                                            //-------------------------------------------End Baby Todler--------------------------------------------------------

                                            //lblSuccessMessage.Text = "Your changes have been saved";
                                            //CCMCA-4853 fixed localization
                                            lblSuccessMessage.Text = GetLocalResourceObject("AckforSavingDetails").ToString();
                                            hdnConfirmMsg.Value = "false";
                                            LoadStatus(hdnMailStatus.Value.ToString(), hdnMEmailStatus.Value.ToString(), hdnAEmailStatus.Value.ToString(), hdnMMobileStatus.Value.ToString(), hdnAMobileStatus.Value.ToString());
                                            ResetHiddenValues();
                                            //To update the customer name on left navigation bar.
                                            leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                                            Label lblCustName = (Label)leftNav.FindControl("lblName");
                                            Label lblCustomerLastUpdatedBy =
                                                (Label)leftNav.FindControl("lblCustomerLastUpdatedBy");

                                            string customerName = string.Empty;
                                            long selectedCustomerID = 0;

                                            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                                            {
                                                selectedCustomerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                                            }

                                            if (selectedCustomerID == Convert.ToInt64(hdnPrimaryCustID.Value))
                                            {
                                                customerName = customerName + Helper.ToTitleCase(ddlTitle0.SelectedValue) + ". ";
                                                customerName = customerName + Helper.ToTitleCase(txtFirstName0.Text.Trim()) + " ";
                                                customerName = customerName + Helper.ToTitleCase(txtInitial0.Text.Trim()) + " ";
                                                customerName = customerName + Helper.ToTitleCase(txtSurname0.Text.Trim());
                                            }
                                            else if (selectedCustomerID == Convert.ToInt64(hdnAssociateCustID.Value))
                                            {
                                                customerName = customerName + Helper.ToTitleCase(ddlTitle1.SelectedValue) + ". ";
                                                customerName = customerName + Helper.ToTitleCase(txtFirstName1.Text.Trim()) + " ";
                                                customerName = customerName + Helper.ToTitleCase(txtInitial1.Text.Trim()) + " ";
                                                customerName = customerName + Helper.ToTitleCase(txtSurname1.Text.Trim());
                                            }

                                            lblCustName.Text = customerName;
                                            lblCustomerLastUpdatedBy.Text = consumer + " @ " + amendDateTime;
                                            Helper.SetTripleDESEncryptedCookie("lblName", customerName);
                                            Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendedBy", consumer);
                                            Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendDate", amendDateTime);
                                        }
                                        else
                                        {
                                            throw new Exception(errorXml);
                                            if (errorXml.Contains("Phone Number already exists"))
                                            {
                                                lblSuccessMessage.Text = "Please correct following information";
                                                lblSuccessMessage.Text = GetLocalResourceObject("InformationValidation").ToString();
                                                errMsgPhoneNumber = GetLocalResourceObject("dupmobilevalidation").ToString(); //"Phone Number already exists";
                                                spanStylePhoneNumber = "";
                                                txtPhoneNumber.CssClass = "errorFld";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //this.lblSuccessMessage.Text = "Our database shows that you are already a Clubcard member.  If you have lost or damaged your card and need a replacement, please call us on 0800 59 16 88 so we can transfer your points balance to your new card.";
                                        this.lblSuccessMessage.Text = GetLocalResourceObject("LclDBValidate").ToString();
                                        //CR13 break the loop as its a duplicate record
                                        break;
                                    }
                                }

                            }
                            //}
                            //else
                            //{
                            //    if (hdnEditCustomerStatusSettings.Value == "True")
                            //    { 
                            //        this.lblSuccessMessage.Text="";
                            //    }
                            //}
                        }
                        else
                        {
                            lblSuccessMessage.Text = GetLocalResourceObject("InformationValidation").ToString(); //"Please correct following information";
                        }
                    }
                }

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerDetail.btnConfirmCustomerDtls_Click()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerDetail.btnConfirmCustomerDtls_Click()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerDetail.btnConfirmCustomerDtls_Click()- Error Message :" + exp.ToString() + "CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerDetail.btnConfirmCustomerDtls_Click()) - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerDetail.btnConfirmCustomerDtls_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {
                objClubDetails = null;
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }
                if (preferenceserviceClient != null)
                {
                    if (preferenceserviceClient.State == CommunicationState.Faulted)
                    {
                        preferenceserviceClient.Abort();
                    }
                    else if (preferenceserviceClient.State != CommunicationState.Closed)
                    {
                        preferenceserviceClient.Close();
                    }
                }
                if (clubcardObj != null)
                {
                    if (clubcardObj.State == CommunicationState.Faulted)
                    {
                        clubcardObj.Abort();
                    }
                    else if (clubcardObj.State != CommunicationState.Closed)
                    {
                        clubcardObj.Close();
                    }
                }
            }
        }

        #region Load the screen
        /// <summary>
        /// To Load Personal Details
        /// </summary>
        /// <param name="cust">DataSet</param>
        protected void LoadPersonalDetails(DataSet houseHoldCustomers, long loggedInCustomerID)
        {
            try
            {
                int rowCount, maxRows;
                searchData = new Hashtable();
                int rowNumber = 99;
                int numberOfCustomers = houseHoldCustomers.Tables[0].Rows.Count;
                string customerID = string.Empty;
                long pCustomerID = 0;
                string customerUserStatus = string.Empty;
                string customerMailStatus = string.Empty;

                if (numberOfCustomers > 1)
                {
                    dvAssociateCustomer.Visible = true;
                    divAssocCustStatus.Visible = true;
                    middlenameAss.Visible = MiddleName;
                    EveningNumberAss.Visible = EveningPhonenumber;
                    titleAss.Visible = Title;
                    AssGender.Visible = gender;
                    aCusfName.Visible = firstName;
                    liSurnameAss.Visible = surname;
                }

                bool isAssociate = false;

                foreach (DataRow customer in houseHoldCustomers.Tables[0].Rows)
                {
                    bool isThirdCustomer = true;

                    if (customer["PrimaryCustomerID"].ToString() == customer["CustomerID"].ToString())
                    {
                        customerID = customer["CustomerID"].ToString();
                        rowNumber = 0;
                        hdnPrimaryCustID.Value = customerID;
                        //Assign primary customer ID to get household status
                        pCustomerID = Convert.ToInt64(customer["CustomerID"]);
                        Helper.SetTripleDESEncryptedCookie("pCustomerID", pCustomerID.ToString());
                        isThirdCustomer = false;
                    }
                    else if ((Convert.ToInt64(customer["CustomerID"].ToString()) == loggedInCustomerID) && isAssociate == false)
                    {
                        customerID = customer["CustomerID"].ToString();
                        rowNumber = 1;
                        hdnAssociateCustID.Value = customerID;
                        //To break the loop, if there more than 2 customers.
                        isAssociate = true;
                        isThirdCustomer = false;

                        //******* May 2011 release changes *******
                        //If associate customer is Banned or Left Scheme then disable the fields.
                        if (customer["CustomerUseStatusID"].ToString() != "1")
                        {
                            // for banned house hold
                            if (customer["CustomerUseStatusID"].ToString() == "2")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssBannedError.Visible = true;
                                //dvAssociateCustomer.Disabled = false;                                           
                                txtFirstName1.Enabled = false;
                                txtSurname1.Enabled = false;
                                txtInitial1.Enabled = false;
                                txtAssocEmailAddress.Enabled = false;
                                txtAssocMobileNumber.Enabled = false;
                                txtAssocDaytimePhoneNumber.Enabled = false;
                                txtAssocEveningPhoneNumber.Enabled = false;
                                ddlAssoRace.Enabled = false;
                                txtAssoSecId.Enabled = false;
                                txtAssoPrimId.Enabled = false;
                                hdnAssociateCustomerDiv.Value = "1";
                                lblAssocCustStatus.Enabled = false;
                                divAssocCustStatus.Disabled = false;
                            }
                            // for Left Scheme
                            else if (customer["CustomerUseStatusID"].ToString() == "3")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssLeftError.Visible = true;
                                dvAssociateCustomer.Disabled = true;
                                txtFirstName1.Enabled = false;
                                txtSurname1.Enabled = false;
                                txtInitial1.Enabled = false;
                                txtAssocEmailAddress.Enabled = false;
                                txtAssocMobileNumber.Enabled = false;
                                txtAssocDaytimePhoneNumber.Enabled = false;
                                txtAssocEveningPhoneNumber.Enabled = false;
                                ddlAssoRace.Enabled = false;
                                txtAssoSecId.Enabled = false;
                                txtAssoPrimId.Enabled = false;
                                hdnAssociateCustomerDiv.Value = "1";
                                lblAssocCustStatus.Enabled = false;
                                divAssocCustStatus.Disabled = false;
                            }
                            //for duplicate
                            else if (customer["CustomerUseStatusID"].ToString() == "5")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssDuplicateError.Visible = true;
                            }
                        }

                        //for address in error
                        else if (customer["CustomerMailStatus"].ToString() == "3")
                        {
                            Span1.Attributes.Add("style", "display:block");
                            spnAssAddressError.Visible = true;
                        }

                        //******* May 2011 release changes ends*******
                    }
                    else if (((Convert.ToInt64(customer["PrimaryCustomerID"].ToString()) == loggedInCustomerID) && isAssociate == false))
                    {
                        customerID = customer["CustomerID"].ToString();
                        rowNumber = 1;
                        hdnAssociateCustID.Value = customerID;
                        isAssociate = true;
                        isThirdCustomer = false;

                        //******* May 2011 release changes *******
                        //If associate customer is Banned or Left Scheme then disable the fields.
                        if (customer["CustomerUseStatusID"].ToString() != "1")
                        {
                            // for banned house hold
                            if (customer["CustomerUseStatusID"].ToString() == "2")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssBannedError.Visible = true;
                                dvAssociateCustomer.Disabled = true;
                                txtFirstName1.Enabled = false;
                                txtSurname1.Enabled = false;
                                txtInitial1.Enabled = false;
                                txtAssocEmailAddress.Enabled = false;
                                txtAssocMobileNumber.Enabled = false;
                                txtAssocDaytimePhoneNumber.Enabled = false;
                                txtAssocEveningPhoneNumber.Enabled = false;
                                txtAssoSecId.Enabled = false;
                                txtAssoPrimId.Enabled = false;
                                ddlAssoRace.Enabled = false;
                                lblAssocCustStatus.Enabled = false;
                                hdnAssociateCustomerDiv.Value = "1";
                                divAssocCustStatus.Disabled = false;
                            }
                            // for Left Scheme
                            else if (customer["CustomerUseStatusID"].ToString() == "3")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssLeftError.Visible = true;
                                dvAssociateCustomer.Disabled = true;
                                txtFirstName1.Enabled = false;
                                txtSurname1.Enabled = false;
                                txtInitial1.Enabled = false;
                                txtAssocEmailAddress.Enabled = false;
                                txtAssocMobileNumber.Enabled = false;
                                txtAssocDaytimePhoneNumber.Enabled = false;
                                txtAssocEveningPhoneNumber.Enabled = false;
                                txtAssoSecId.Enabled = false;
                                txtAssoPrimId.Enabled = false;
                                ddlAssoRace.Enabled = false;
                                lblAssocCustStatus.Enabled = false;
                                hdnAssociateCustomerDiv.Value = "1";
                                divAssocCustStatus.Disabled = false;
                            }
                            //for duplicate
                            else if (customer["CustomerUseStatusID"].ToString() == "5")
                            {
                                Span1.Attributes.Add("style", "display:block");
                                spnAssDuplicateError.Visible = true;
                            }
                        }
                        //for address in error
                        else if (customer["CustomerMailStatus"].ToString() == "3")
                        {
                            Span1.Attributes.Add("style", "display:block");
                            spnAssAddressError.Visible = true;
                        }
                        //******* May 2011 release changes ends*******
                    }

                    //Check if already Main and Associate customers displayed.
                    if (!isThirdCustomer)
                    {
                        searchData["CustomerID"] = customerID;
                        //Preparing parameters for service call
                        conditionXML = Helper.HashTableToXML(searchData, "customer");
                        maxRows = 100; 
                        customerObj = new CustomerServiceClient();

                        if (customerObj.GetCustomerDetails(out errorXml, out resultXml, out rowCount, conditionXML, maxRows, Culture))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            dsCustomer = new DataSet();
                            dsCustomer.ReadXml(new XmlNodeReader(resulDoc));

                            DateTime dob = DateTime.Now;
                            if(dsCustomer != null && dsCustomer.Tables != null && dsCustomer.Tables.Contains("Customer") && dsCustomer.Tables["Customer"].Rows.Count > 0 )
                            {
                                //For main customer
                                if (rowNumber == 0)
                                {
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("TitleEnglish"))
                                        ddlTitle0.SelectedValue = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim());

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name1")) txtFirstName0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name1"].ToString().Trim());
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name2")) txtInitial0.Text = dsCustomer.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name3")) txtSurname0.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name3"].ToString().Trim());

                                    //Date Of Birth
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("family_member_1_dob"))
                                    {

                                        dob = Convert.ToDateTime(dsCustomer.Tables["Customer"].Rows[0]["family_member_1_dob"]);

                                        ////if date is not valid leave it blank
                                        if (dob.ToString("dd/MM/yyyy") != "01/01/0001" && dob.ToString("dd/MM/yyyy") != "01/01/1901")
                                        {
                                            ddlDay0.SelectedValue = dob.ToString("dd");
                                            ddlMonth0.SelectedValue = Convert.ToString(dob.Month);
                                            ddlYear0.SelectedValue = dob.Year.ToString();
                                        }
                                    }

                                    //Gender
                                    if(MainGender.Visible)
                                    {
                                        if (dsCustomer.Tables["Customer"].Columns.Contains("Sex") && dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString().Trim() != "")
                                        {
                                            if (dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString() == "M")
                                            {
                                                radioMale0.Checked = true;
                                            }
                                            else if (dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString() == "F")
                                            {
                                                radioFemale0.Checked = true;
                                            }
                                        }
                                        else // Default values
                                        {
                                            if (ddlTitle0.SelectedValue.Trim() == "Mr")
                                                radioMale0.Checked = true;
                                            else if (ddlTitle0.SelectedValue.Contains("Mrs") || ddlTitle0.SelectedValue.Contains("Ms") || ddlTitle0.SelectedValue.Contains("Miss"))
                                                radioFemale0.Checked = true;
                                        }
                                  }

                                    //Load address and other fields
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine1"))
                                    {
                                        txtAddressLine1.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine1"].ToString().Trim();
                                        hdnChkAddressline1.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine1"].ToString().Trim();
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine2"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine2"].ToString().Trim() != string.Empty)
                                        {
                                            txtStreet.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine2"].ToString().Trim();
                                        }
                                        else
                                        {
                                            txtStreet.Text = string.Empty;
                                        }
                                        hdnChkAddressline2.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine2"].ToString().Trim();
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine3"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine3"].ToString().Trim() != string.Empty)
                                        {
                                            txtLocality.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine3"].ToString().Trim();
                                        }
                                        else
                                        {
                                            txtLocality.Text = string.Empty;
                                        }
                                        hdnChkAddressline3.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine3"].ToString().Trim();
                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine4"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine4"].ToString().Trim() != string.Empty)
                                        {
                                            txtTown.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine4"].ToString().Trim();
                                        }
                                        else
                                        {
                                            txtTown.Text = string.Empty;
                                        }
                                        hdnChkAddressline4.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine4"].ToString().Trim();
                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressLine5"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine5"].ToString().Trim() != string.Empty)
                                        {
                                            if (enableProvince)
                                                ddlProvince.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine5"].ToString().Trim();
                                            else
                                                txtCountyDetails.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine5"].ToString().Trim();
                                        }
                                        else
                                        {
                                            txtCountyDetails.Text = string.Empty;
                                        }
                                        hdnChkAddressline5.Value = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressLine5"].ToString().Trim();
                                    }

                                    //Email address
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("email_address"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim() != "NULL")
                                        {
                                            if (culture != "en-GB")
                                            {
                                                //NGC Change
                                                txtEmailAddress.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                                txtEmail.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                                hdnEmailAddress.Value = txtEmail.Text.ToString().Trim();
                                            }
                                            else
                                            {
                                                //NGC 3.6 Changes
                                                txtEmailAddress.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                            }
                                        }
                                    }

                                    //NGC Change

                                    //CR13 Changes
                                    //Customer Status
                                    if (dsCustomer.Tables["Customer"].Columns["CustomerUseStatusID"] != null)
                                    {
                                        Helper.SetTripleDESEncryptedCookie("CustomerUseStatus", dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim());
                                        int UseStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim());
                                        hdnUseStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim();


                                        //CR13 Load Primary Customer UseStatusID
                                        ddlCustomerStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim();

                                        if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_SKELETON)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusSkeleton").ToString();//BusinessConstants.CUSTOMER_USESTATUS_SKELETON;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_ACTIVE)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusActive").ToString();//BusinessConstants.CUSTOMER_USESTATUS_ACTIVE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_BANNED)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusBanned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_BANNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_LEFTSCHEME)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusLeft").ToString();//BusinessConstants.CUSTOMER_USESTATUS_LEFTSCHEME;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DATAREMOVED)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStstusDataRMD").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DATAREMOVED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DUPLICATE)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusDUP").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DUPLICATE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNDERAGE)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusUnderage").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNDERAGE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNSIGNED)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusUnsigned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNSIGNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNDERAGEANDUNSIGNED)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusUnAgendSigned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNDERAGEANDUNSIGNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_POSSIBLEFRAUD)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusFraud").ToString();//BusinessConstants.CUSTOMER_USESTATUS_POSSIBLEFRAUD;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_OTHER)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusOther").ToString();//BusinessConstants.CUSTOMER_USESTATUS_OTHER;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_MANUALENTRY)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusManualEntry").ToString();//BusinessConstants.CUSTOMER_USESTATUS_MANUALENTRY;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_PENDINGACTIVATION)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusPending").ToString();//BusinessConstants.CUSTOMER_USESTATUS_PENDINGACTIVATION;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_CARDLESS)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusCardless").ToString();//BusinessConstants.CUSTOMER_USESTATUS_CARDLESS;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_PROFANITYERROR)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusProfanity").ToString();//BusinessConstants.CUSTOMER_USESTATUS_PROFANITYERROR;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_INACTIVE)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusInactive").ToString();//BusinessConstants.CUSTOMER_USESTATUS_INACTIVE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DECEASED)
                                        {
                                            lblCustomerStatus.Text = GetLocalResourceObject("CusUseStatusDeceased").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DECEASED;
                                        }


                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    if (dsCustomer.Tables["Customer"].Columns["CustomerMailStatus"] != null)
                                    {
                                        int MailStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerMailStatus"].ToString().Trim());
                                        hdnMailStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerMailStatus"].ToString().Trim();

                                        //CR13 Load Primary Customer MailStatus
                                        ddlMailStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerMailStatus"].ToString().Trim();

                                        if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_DELIVERABLE)
                                        {
                                            lblMailStatus.Text = GetLocalResourceObject("CusEmailStatusDelivar").ToString();//BusinessConstants.CUST_EMAIL_STATUS_DELIVERABLE;
                                        }
                                        else if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_MISSING)
                                        {
                                            lblMailStatus.Text = GetLocalResourceObject("CusEmailStatusMissing").ToString();
                                        }
                                        else if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_INERROR)
                                        {
                                            lblMailStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                                        }


                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    //NGC Change 3.6- Mobile and Email Status
                                    if (dsCustomer.Tables["Customer"].Columns["CustomerMobilePhoneStatus"] != null)
                                    {
                                        int MobileStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim());
                                        //CR13 Load Primary Customer Mobile Status
                                        ddlMobileStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim();
                                        hdnMMobileStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim();

                                        if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                                        {
                                            lblMobileStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                                        }
                                        else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                                        {
                                            lblMobileStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                                        }
                                        else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                                        {
                                            lblMobileStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                                        }
                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    if (dsCustomer.Tables["Customer"].Columns["CustomerEmailStatus"] != null)
                                    {
                                        int EmailStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim());
                                        //CR13 Load Primary Customer Email Status
                                        ddlEmailStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim();
                                        hdnMEmailStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim();

                                        if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                                        {
                                            lblEmailStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                                        }
                                        else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                                        {
                                            lblEmailStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                                        }
                                        else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                                        {
                                            lblEmailStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                                        }

                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    //Phone number
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("daytime_phone_number"))
                                        txtPhoneNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["daytime_phone_number"].ToString().Trim();

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("SSN"))
                                        txtPrimId.Text = dsCustomer.Tables["Customer"].Rows[0]["SSN"].ToString().Trim();

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("RaceID"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["RaceID"].ToString().Trim() != "0")
                                            ddlRace.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["RaceID"].ToString().Trim();
                                        else
                                            ddlRace.SelectedIndex = 0;
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("PassportNo"))
                                        txtSecId.Text = dsCustomer.Tables["Customer"].Rows[0]["PassportNo"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("ISOLanguageCode"))
                                        rdoLanguage.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["ISOLanguageCode"].ToString().Trim();

                                    //Post code
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("MailingAddressPostCode"))
                                    {
                                        if (hdnHidepostcodeFields.Value == "0")
                                        {

                                            txtPostCode.Text = string.Empty;
                                            hdnPostCodeNumber.Value = "null";
                                            hdnChkPostcode.Value = "null";
                                        }

                                        else
                                        {
                                            txtPostCode.Text = dsCustomer.Tables["Customer"].Rows[0]["MailingAddressPostCode"].ToString().Trim();
                                            hdnPostCodeNumber.Value = txtPostCode.Text;
                                            hdnChkPostcode.Value = txtPostCode.Text;
                                        }
                                    }

                                    /*Saving the hdnEveningPhoneNumber and hdnMobilePhoneNumber in hidden fields so that they can be 
                                    accessed later while updating details*/
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("evening_phone_number"))
                                    {
                                        txtEveningPhoneNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["evening_phone_number"].ToString().Trim();
                                        hdnEveningPhoneNumber.Value = dsCustomer.Tables["Customer"].Rows[0]["evening_phone_number"].ToString().Trim();
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("mobile_phone_number"))
                                    {
                                        txtMobileNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                                        hdnMobilePhoneNumber.Value = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                                    }
                                    //NGC 3.6 Enhancements - Neeta
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("JoinRouteCode"))
                                    {
                                        lblJoinRoute.Text = dsCustomer.Tables["Customer"].Rows[0]["JoinRouteCode"].ToString().Trim();

                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("JoinedStoreID"))
                                    {
                                        lblJoinedStoreID.Text = dsCustomer.Tables["Customer"].Rows[0]["JoinedStoreID"].ToString().Trim();

                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("PromotionalCode"))
                                    {
                                        lblPromotionalCode.Text = dsCustomer.Tables["Customer"].Rows[0]["PromotionalCode"].ToString().Trim();

                                    }

                                    //Load Family details
                                    LoadFamilyDetails(dsCustomer, 0);

                                    //Load Busniess Details
                                    if (hdnHideBusinessDetails.Value == "false")
                                    {
                                        LoadBusinessDetails(customer);
                                    }
                                }
                                //For associate customer customer
                                else if (rowNumber == 1)
                                {
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("TitleEnglish"))
                                    {
                                        ddlTitle1.SelectedValue = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim());
                                        //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                        hdnddlTitle1.Value = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim());
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name1")) txtFirstName1.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name1"].ToString().Trim());
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name2")) txtInitial1.Text = dsCustomer.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("Name3")) txtSurname1.Text = Helper.ToTitleCase(dsCustomer.Tables["Customer"].Rows[0]["Name3"].ToString().Trim());

                                    //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                    hdntxtFirstName1.Value = txtFirstName1.Text;
                                    hdntxtInitial1.Value = txtInitial1.Text;
                                    hdntxtSurname1.Value = txtSurname1.Text;

                                    //Date Of Birth
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("family_member_1_dob"))
                                    {
                                        dob = Convert.ToDateTime(dsCustomer.Tables["Customer"].Rows[0]["family_member_1_dob"]);

                                        ////if date is not valid leave it blank
                                        if (dob.ToString("dd/MM/yyyy") != "01/01/0001" && dob.ToString("dd/MM/yyyy") != "01/01/1901")
                                        {
                                            ddlDay1.SelectedValue = dob.ToString("dd");
                                            ddlMonth1.SelectedValue = Convert.ToString(dob.Month);
                                            ddlYear1.SelectedValue = dob.Year.ToString();

                                            //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                            hdnddlDay1.Value = dob.Day.ToString();
                                            hdnddlMonth1.Value = Convert.ToString(dob.Month);
                                            hdnddlYear1.Value = dob.Year.ToString();
                                        }
                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("evening_phone_number"))
                                    {
                                        txtAssocEveningPhoneNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["evening_phone_number"].ToString().Trim();
                                        hdnEveningPhoneNumber.Value = dsCustomer.Tables["Customer"].Rows[0]["evening_phone_number"].ToString().Trim();
                                    }

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("mobile_phone_number"))
                                    {
                                        txtAssocMobileNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                                        hdnMobilePhoneNumber.Value = dsCustomer.Tables["Customer"].Rows[0]["mobile_phone_number"].ToString().Trim();
                                    }
                                    //Phone number
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("daytime_phone_number"))
                                        txtAssocDaytimePhoneNumber.Text = dsCustomer.Tables["Customer"].Rows[0]["daytime_phone_number"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("SSN"))
                                        txtAssoPrimId.Text = dsCustomer.Tables["Customer"].Rows[0]["SSN"].ToString().Trim();

                                    if (dsCustomer.Tables["Customer"].Columns.Contains("RaceID"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["RaceID"].ToString().Trim() != "0")
                                            ddlAssoRace.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["RaceID"].ToString().Trim();
                                        else
                                            ddlAssoRace.SelectedIndex = 0;
                                    }
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("PassportNo"))
                                        txtAssoSecId.Text = dsCustomer.Tables["Customer"].Rows[0]["PassportNo"].ToString().Trim();
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("ISOLanguageCode"))
                                        rdoAssoLanguage.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["ISOLanguageCode"].ToString().Trim();

                                    //Email address
                                    if (dsCustomer.Tables["Customer"].Columns.Contains("email_address"))
                                    {
                                        if (dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim() != "NULL")
                                        {
                                            if (culture != "en-GB")
                                            {
                                                //NGC Change
                                                txtAssocEmailAddress.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                                hdnEmailAddress.Value = txtEmail.Text.ToString().Trim();
                                            }
                                            else
                                            {
                                                //NGC 3.6 Changes
                                                txtAssocEmailAddress.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                                // lblEmailAddress.Text = dsCustomer.Tables["Customer"].Rows[0]["email_address"].ToString().Trim();
                                            }
                                        }
                                    }
                                    //NGC Change 3.6- Mobile and Email Status

                                    //CR13 Changes

                                    if (dsCustomer.Tables["Customer"].Columns["CustomerMobilePhoneStatus"] != null)
                                    {
                                        int MobileStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim());
                                        //CR13 Load Associate Mobile Status
                                        ddlAssoMobileStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim();
                                        hdnAMobileStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim();

                                        if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                                        {
                                            lblAssoMobileStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                                        }
                                        else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                                        {
                                            lblAssoMobileStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                                        }
                                        else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                                        {
                                            lblAssoMobileStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                                        }
                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    if (dsCustomer.Tables["Customer"].Columns["CustomerEmailStatus"] != null)
                                    {
                                        int EmailStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim());
                                        //CR13 Load Associate Email Status
                                        ddlAssoEmailStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim();
                                        hdnAEmailStatus.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerEmailStatus"].ToString().Trim();

                                        if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                                        {
                                            lblAssoEmailStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                                        }
                                        else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                                        {
                                            lblAssoEmailStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                                        }
                                        else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                                        {
                                            lblAssoEmailStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                                        }
                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }


                                    //Customer Status
                                    if (dsCustomer.Tables["Customer"].Columns["CustomerUseStatusID"] != null)
                                    {
                                        Helper.SetTripleDESEncryptedCookie("CustomerUseStatus", dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim());
                                        int UseStatus = Convert.ToInt32(dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim());
                                        hdnUseStatus1.Value = dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim();

                                        //CR13 Load Associate UseStatusID
                                        ddlAssocCustStatus.SelectedValue = dsCustomer.Tables["Customer"].Rows[0]["CustomerUseStatusID"].ToString().Trim();
                                        if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_SKELETON)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusSkeleton").ToString();//BusinessConstants.CUSTOMER_USESTATUS_SKELETON;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_ACTIVE)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusActive").ToString();//BusinessConstants.CUSTOMER_USESTATUS_ACTIVE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_BANNED)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusBanned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_BANNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_LEFTSCHEME)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusLeft").ToString();//BusinessConstants.CUSTOMER_USESTATUS_LEFTSCHEME;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DATAREMOVED)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStstusDataRMD").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DATAREMOVED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DUPLICATE)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusDUP").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DUPLICATE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNDERAGE)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusUnderage").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNDERAGE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNSIGNED)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusUnsigned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNSIGNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_UNDERAGEANDUNSIGNED)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusUnAgendSigned").ToString();//BusinessConstants.CUSTOMER_USESTATUS_UNDERAGEANDUNSIGNED;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_POSSIBLEFRAUD)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusFraud").ToString();//BusinessConstants.CUSTOMER_USESTATUS_POSSIBLEFRAUD;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_OTHER)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusOther").ToString();//BusinessConstants.CUSTOMER_USESTATUS_OTHER;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_MANUALENTRY)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusManualEntry").ToString();//BusinessConstants.CUSTOMER_USESTATUS_MANUALENTRY;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_PENDINGACTIVATION)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusPending").ToString();//BusinessConstants.CUSTOMER_USESTATUS_PENDINGACTIVATION;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_CARDLESS)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusCardless").ToString();//BusinessConstants.CUSTOMER_USESTATUS_CARDLESS;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_PROFANITYERROR)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusProfanity").ToString();//BusinessConstants.CUSTOMER_USESTATUS_PROFANITYERROR;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_INACTIVE)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusInactive").ToString();//BusinessConstants.CUSTOMER_USESTATUS_INACTIVE;
                                        }
                                        else if (UseStatus == BusinessConstants.CUSTOMERUSESTATUS_DECEASED)
                                        {
                                            lblAssocCustStatus.Text = GetLocalResourceObject("CusUseStatusDeceased").ToString();//BusinessConstants.CUSTOMER_USESTATUS_DECEASED;
                                        }
                                    }
                                    //CR13 change - Confirm msg
                                    else { hdnConfirmMsg.Value = "true"; }

                                    //Gender
                                    if (AssGender.Visible)
                                    {
                                        if (dsCustomer.Tables["Customer"].Columns.Contains("Sex") && dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString().Trim() != "")
                                        {
                                            if (dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString() == "M")
                                            {
                                                radioMale1.Checked = true;
                                                //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                                hdnGender.Value = "M";
                                            }
                                            else if (dsCustomer.Tables["Customer"].Rows[0]["Sex"].ToString() == "F")
                                            {
                                                radioFemale1.Checked = true;
                                                //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                                hdnGender.Value = "F";
                                            }
                                        }
                                        else // Default values
                                        {

                                            if (ddlTitle1.SelectedValue.Contains("Mr"))
                                            {
                                                radioMale1.Checked = true;
                                                //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                                hdnGender.Value = "M";
                                            }
                                            else if (ddlTitle1.SelectedValue.Contains("Mrs") || ddlTitle1.SelectedValue.Contains("Ms") || ddlTitle1.SelectedValue.Contains("Miss"))
                                            {
                                                radioFemale1.Checked = true;
                                                //To store the value in hidden field for updation when the field is disabled(May 2011 release)
                                                hdnGender.Value = "F";
                                            }

                                        }
                                    }

                                    //Load Family details
                                    LoadFamilyDetails(dsCustomer, 1);
                                }
                            }
                        }
                    }
                }

                //To set household status on LHN bar
                SetHouseHoldStatus(pCustomerID);

                //Assign household customer count to hidden field.
                hdnNumberOfCustomers.Value = numberOfCustomers.ToString();
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
            finally
            {
                //Close the sercise connections.
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }

                if (clubcardObj != null)
                {
                    if (clubcardObj.State == CommunicationState.Faulted)
                    {
                        clubcardObj.Abort();
                    }
                    else if (clubcardObj.State != CommunicationState.Closed)
                    {
                        clubcardObj.Close();
                    }
                }
            }
        }

        private void LoadBusinessDetails(DataRow customer)
        {
            try
            {

                if (customer["BusinessName"].ToString().Trim() != null && hdnHideBusniessName.Value == "false")
                {
                    TxtBusniessName.Text = customer["BusinessName"].ToString().Trim();
                }
                if (customer["BusinessType"].ToString() != null && hdnHideBusinessType.Value == "false")
                {
                    ddlBusinessType.SelectedIndex = Convert.ToInt32(customer["BusinessType"].ToString());
                }
                if (customer["BusinessRegistrationNumber"].ToString().Trim() != null && hdnHideBusniessRegNo.Value == "false")
                {
                    lclBusniessRegNoVal.Text = customer["BusinessRegistrationNumber"].ToString().Trim();
                }
                if (customer["BusinessAddressLine1"].ToString().Trim() != null && hdnHideBusinessAddr1.Value == "false")
                {
                    txtBusinessAddress1.Text = customer["BusinessAddressLine1"].ToString().Trim();
                }
                if (customer["BusinessAddressLine2"].ToString().Trim() != null && hdnHideBusinessAddr2.Value == "false")
                {
                    txtBusinessAddress2.Text = customer["BusinessAddressLine2"].ToString().Trim();
                }
                if (customer["BusinessAddressLine3"].ToString().Trim() != null && hdnHideBusinessAddr3.Value == "false")
                {
                    txtBusinessAddress3.Text = customer["BusinessAddressLine3"].ToString().Trim();
                }
                if (customer["BusinessAddressLine4"].ToString().Trim() != null && hdnHideBusinessAddr4.Value == "false")
                {
                    txtBusinessAddress4.Text = customer["BusinessAddressLine4"].ToString().Trim();
                }
                if (customer["BusinessAddressLine5"].ToString().Trim() != null && hdnHideBusinessAddr5.Value == "false")
                {
                    txtBusinessAddress5.Text = customer["BusinessAddressLine5"].ToString().Trim();
                }
                if (customer["BusinessAddressLine6"].ToString().Trim() != null && hdnHideBusinessAddr6.Value == "false")
                {
                    txtBusinessAddress6.Text = customer["BusinessAddressLine6"].ToString().Trim();
                }
                if (customer["BusinessAddressPostCode"].ToString().Trim() != null && hdnHideBusinessPostcode.Value == "false")
                {
                    txtBusinessPostcode.Text = customer["BusinessAddressPostCode"].ToString().Trim();
                }

                //if (dsCustomer.Tables["FamilyDetails"].Columns.Contains("DateOfBirth")) //Set family Age details
                //{
                //    ArrayList ages = new ArrayList();

                //    for (int i = 0; i < dsCustomer.Tables["FamilyDetails"].Rows.Count; i++)
                //    {
                //        ages.Add(GetAge(Convert.ToDateTime(dsCustomer.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])));
                //    }

                //    UpdateAgeBoxes(Convert.ToInt16(dsCustomer.Tables["FamilyDetails"].Rows.Count), ages);
                //}
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
        }


        /// <summary>
        /// To Load Family Details
        /// </summary>
        /// <param name="familyDetails">DataSet</param>
        protected void LoadFamilyDetails(DataSet familyDetails, int rowNumber)
        {
            try
            {
                short noOfFamilyMembers = 0;
                //CCMCA-441
                if (ddlYear0.SelectedValue.ToUpper() != "YEAR")
                    txtAge1.Text = ddlYear0.SelectedValue;
                else
                    txtAge1.Text = "Year";

                if (familyDetails.Tables.Contains("FamilyDetails") && familyDetails.Tables["FamilyDetails"].Rows.Count > 0)
                {
                    if (familyDetails.Tables["FamilyDetails"].Rows[0]["number_of_household_members"].ToString() != "0")
                    {
                        txtNoofPeople.Text = familyDetails.Tables["FamilyDetails"].Rows[0]["number_of_household_members"].ToString();
                        noOfFamilyMembers = Convert.ToInt16(familyDetails.Tables["FamilyDetails"].Rows[0]["number_of_household_members"]);
                    }
                    //CCMCA-441
                    if (familyDetails.Tables["FamilyDetails"].Columns.Contains("DateOfBirth")) //Set family Age details
                    {
                        ArrayList ages = new ArrayList();

                        for (int i = 0; i < familyDetails.Tables["FamilyDetails"].Rows.Count; i++)
                        {
                            //ages.Add(GetAge(Convert.ToDateTime(familyDetails.Tables["FamilyDetails"].Rows[i]["DateOfBirth"])));
                            ages.Add(Convert.ToDateTime(familyDetails.Tables["FamilyDetails"].Rows[i]["DateOfBirth"]));
                        }

                        UpdateAgeBoxes(Convert.ToInt16(familyDetails.Tables["FamilyDetails"].Rows.Count), ages);
                    }
                }
                else if (familyDetails.Tables["NoOFFamilyMembers"] != null)
                {
                    noOfFamilyMembers = Convert.ToInt16(familyDetails.Tables["NoOFFamilyMembers"].Rows[0]["number_of_household_members"]);
                    if (noOfFamilyMembers > 0)
                        txtNoofPeople.Text = noOfFamilyMembers.ToString();
                }

                if (rowNumber == 0)
                {
                    LoadCustomerPreferences(hdnPrimaryCustID.Value, true);
                }
                else if (rowNumber == 1)
                {
                    LoadCustomerPreferences(hdnAssociateCustID.Value, false);
                }
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
        }

        #endregion



        /// <summary>
        /// It converts the date in to age
        /// </summary>
        /// <param name="dob">DateTime</param>
        private short GetAge(DateTime dob)
        {
            int dobYear = dob.Year;
            short age = Convert.ToInt16(DateTime.Now.Year - dobYear);
            return age;
        }

        /// <summary>
        /// It converts the age in to date format. 
        /// </summary>
        /// <param name="age">short</param>
        private DateTime GetDateOfBirth(short age)
        {
            int presentYear = DateTime.Now.Year;

            //Considering employee has born on 1 January
            DateTime dob = DateTime.Parse((presentYear - age) + "/1/1");

            return dob;
        }

        #region Dynamic Age Boxes
        /// <summary>
        /// It updates the age text boxes
        /// </summary>
        /// <param name="noOfPeople">short</param>
        /// <param name="peopleAges">ArrayList</param>
        protected void UpdateAgeBoxes(short noOfPeople, ArrayList peopleAges)
        {
            //CCMCA-441
            for (int i = 1; i <= noOfPeople + 1; i++)//For every family member including Clubcard Holder
            {
                ContentPlaceHolder mainMaster = (ContentPlaceHolder)Page.Master.Controls[0].FindControl("PageContainer");
                //TextBox txtAge = (TextBox)mainMaster.FindControl("txtAge" + i);
                DropDownList ddlAge = (DropDownList)mainMaster.FindControl("ddlAge" + i);

                if (i <= noOfPeople + 1)//For every family member including the clubcard holder
                {
                    //txtAge.Text = Convert.ToInt16(peopleAges[i - 1]).ToString();
                    if (i == 1)
                    {
                        if (hdnDateOfBirth.Value != string.Empty)
                            txtAge1.Text = Convert.ToDateTime(hdnDateOfBirth.Value).Year.ToString(); //Convert.ToDateTime(peopleAges[i - 1]).Year.ToString();
                    }
                    else
                    {
                        ddlAge.SelectedValue = Convert.ToDateTime(peopleAges[i - 2]).Year.ToString();
                    }
                }
                //else
                //    txtAge.Text = "";
            }
        }
        #endregion

        /// <summary>
        /// To validate customer details
        /// MKTG00007304 Fix : Get regular expression values from Configuration for PostCode. 
        /// string regPostCode = hdnPostCodeFormat.Value;
        ///string regPostCode1 = hdnPostCodeFormat1.Value;
        /// </summary>
        /// <returns>boolean</returns>
        protected bool ValidatePersonalDetailsPage()
        {
            try
            {
                #region Removing hard coded regular expressions and make it configurable from DB
                //string regForeName = "";
                //string regInitial = "";
                //string regSurName = "";

                //if (ConfigurationManager.AppSettings["CultureDefault"] == "en-GB" || ConfigurationManager.AppSettings["CultureDefault"] == "en-IE")
                //{

                //    regForeName = @"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";
                //    regInitial =  @"^[a-zA-Z]*$";
                //    regSurName =  @"^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";

                //}

                //else
                //{

                //    regForeName = @"^\p{L}[\p{L}\p{Pd}\x27]*\p{L}$";//@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";
                //    regInitial = hdnname2validation.Value; //@"^\\s*([\\p{L}\\s]*)\\s*$";//@"^[a-zA-Z]*$";
                //    regSurName = @"^\p{L}[\p{L}\p{Pd}\x27]*\p{L}$";// @"^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";

                //}
                #endregion
                string regForeName = hdnname1reg.Value;

                string regInitial = hdnmiddleinitialreg.Value;

                string regSurName = hdnname3reg.Value;


                //string regPostCode = @"^/\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g $";
                string regPostCode = hdnPostCodeFormat.Value;//@"^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
                string regPostCode1 = hdnPostCodeFormat1.Value;// @"^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}$";
                //string regPostCodeForUS = @"^[0-9]*$";
                string regPostCodeForUS = hdnUSPostCodeFormat.Value;

                //string regAddress = @"^(a-z|A-Z|0-9)*[^#$%^&*()\']*$";
                //string regPhoneNumber = @"^[0-9]*$";
                string regPhoneNumber = hdnphonenumberreg.Value;
                string strAlphNum = "";
                //NGC Change
                //string regMail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                //string regMail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$";
                string regMail = hdnemailreg.Value;
                //string regMail = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (hdnPostcoderegexp.Value.ToString() != "")
                {
                    strAlphNum = hdnPostcoderegexp.Value.ToString();
                }
                string strAlphNum2 = "";
                if (hdnAddressGroupconfig.Value == "true")
                {
                    if (hdnAddressLineFormat.Value.ToString() != "")
                    {
                        strAlphNum2 = hdnAddressLineFormat.Value.ToString();
                    }
                }


                bool bErrorFlag = true;
                bool bErrorAgeFlag = true;
                DateTime dob = DateTime.Now;

                //Clear the class
                txtFirstName0.CssClass = "";
                txtInitial0.CssClass = "";
                txtSurname0.CssClass = "";
                txtFirstName1.CssClass = "";
                txtInitial1.CssClass = "";
                txtSurname1.CssClass = "";
                spanClassDOBDropDown0 = "dtFld";
                txtPhoneNumber.CssClass = "";
                txtPostCode.CssClass = "";
                spanClassAddress = "dtAddress";
                txtEmailAddress.CssClass = "";
                txtPhoneNumber.CssClass = "";
                txtMobileNumber.CssClass = "";
                txtEveningPhoneNumber.CssClass = "";
                this.ddlRace.CssClass = "";
                this.ddlAssoRace.CssClass = "";
                txtPrimId.CssClass = "";
                txtSecId.CssClass = "";
                txtAssoPrimId.CssClass = "";
                txtAssoSecId.CssClass = "";
                rdoAssoLanguage.CssClass = "";
                rdoLanguage.CssClass = "";
                spanClassDOBDropDown2 = "dtFld";
                spanClassDOBDropDown3 = "dtFld";
                spanClassDOBDropDown4 = "dtFld";
                spanClassDOBDropDown5 = "dtFld";
                spanClassDOBDropDown6 = "dtFld";
                spanClassDOBDropDown7 = "dtFld";
                spanClassDOBDropDown8 = "dtFld";
                spanClassDOBDropDown9 = "dtFld";
                spanClassDOBDropDown10 = "dtFld";
                txtCountyDetails.CssClass = "";
                ddlProvince.CssClass = "";
                txtAddressLine1.CssClass = "";
                txtStreet.CssClass = "";
                txtLocality.CssClass = "";
                txtTown.CssClass = "";
                txtAssocEmailAddress.CssClass = "";
                txtAssocDaytimePhoneNumber.CssClass = "";
                txtAssocEveningPhoneNumber.CssClass = "";
                txtAssocMobileNumber.CssClass = "";

                //CCMCA-441
                if (ddlYear0.SelectedValue.ToUpper() != "YEAR")
                    txtAge1.Text = ddlYear0.SelectedValue;
                else
                    txtAge1.Text = "Year";
                //Server side validations
                if (Convert.ToBoolean(hdnConfigVisible.Value))
                {
                    //CSC ThaiLand change: Replaced AddressLine5 text box (MailingAddressLine5) with Dropdown list
                    if (this.ddlProvince.SelectedIndex == 0 && (!Convert.ToBoolean(hdnAddressLine5.Value)))
                    {
                        //this.errMsgRace = "Please select valid Race.";
                        this.errMsgAddressLine5 = GetLocalResourceObject("ErrorMsgValidProvince").ToString();
                        this.spanStyleAddressLine5 = "";
                        this.ddlProvince.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    if (this.ddlRace.SelectedValue == "- Select race -" && (!Convert.ToBoolean(hdnRace.Value)))
                    {
                        //this.errMsgRace = "Please select valid Race.";
                        this.errMsgRace = GetLocalResourceObject("ErrorMsgValidRace").ToString();
                        this.spanStyleRace = "";
                        this.ddlRace.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    if (this.rdoLanguage.SelectedValue == "- Select Language -" && (!Convert.ToBoolean(hdnLanguage.Value)))
                    {
                        //this.errMsgRace = "Please select valid Langauge.";
                        this.errMsgLanguage = GetLocalResourceObject("ErrorMsgValidLanguage").ToString();
                        this.spanStyleLanguage = "";
                        this.rdoLanguage.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    if (txtPrimId.Text == "" && txtSecId.Text == "")
                    {
                        if (hdnPrimId.Value == "false")
                        {
                            this.errMsgPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString();//"Please enter valid Primary Id";
                            this.spanStylePrimaryId = "";
                            this.txtPrimId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        if (hdnSecId.Value == "false")
                        {
                            this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();//"Please enter valid Secondary Id";
                            this.spanStyleSecondaryId = "";
                            this.txtSecId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    if (txtPrimId.Text == "" || txtSecId.Text == "")
                    {
                        if (hdnPrimId.Value == "false" || hdnSecId.Value == "false")
                        {
                            this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgValidPrimorSecID").ToString();//"Please enter either Primary Id or Secondary Id";
                            this.spanStylePrimaryId = "";
                            this.spanStyleSecondaryId = "";
                            this.txtPrimId.CssClass = "errorFld";
                            this.txtSecId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }


                    if (txtPrimId.Text != "" && txtSecId.Text == "")
                    {

                        if (!Helper.IsRegexMatch(this.txtPrimId.Text.Trim(), strAlphNum, Convert.ToBoolean(hdnPrimId.Value), false))
                        {
                            this.errMsgPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString();
                            this.spanStylePrimaryId = "";
                            this.txtPrimId.CssClass = "errorFld";
                            bErrorFlag = false;

                        }
                        if (!string.IsNullOrEmpty(hdnPrimIdMinValue.Value))
                        {
                            if (txtPrimId.Text.Length < int.Parse(hdnPrimIdMinValue.Value.ToString()))
                            {
                                this.errMsgPrimaryId = GetLocalResourceObject("ErrorMsgPrimIDLength").ToString();
                                this.spanStylePrimaryId = "";
                                this.txtPrimId.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(hdnPrimIdMaxValue.Value))
                        {
                            if (txtPrimId.Text.Length > int.Parse(hdnPrimIdMaxValue.Value.ToString()))
                            {
                                this.errMsgPrimaryId = GetLocalResourceObject("ErrorMsgPrimIDLength").ToString();
                                this.spanStylePrimaryId = "";
                                this.txtPrimId.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }

                        if (hdnSecId.Value == "false" && txtPrimId.Text.ToString() == "")
                        {
                            this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();
                            this.spanStyleSecondaryId = "";
                            this.txtSecId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    if (txtPrimId.Text == "" && txtSecId.Text != "")
                    {
                        if (!Helper.IsRegexMatch(this.txtSecId.Text.Trim(), strAlphNum, Convert.ToBoolean(hdnSecId.Value), false))
                        {
                            this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();
                            this.spanStyleSecondaryId = "";
                            this.txtSecId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }

                        if (!string.IsNullOrEmpty(hdnSecIdMinValue.Value))
                        {
                            if (txtSecId.Text.Length < int.Parse(hdnSecIdMinValue.Value.ToString()))
                            {
                                this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgSecIDLength").ToString();
                                this.spanStyleSecondaryId = "";
                                this.txtSecId.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(hdnSecIdMaxValue.Value))
                        {
                            if (txtSecId.Text.Length > int.Parse(hdnSecIdMaxValue.Value.ToString()))
                            {
                                this.errMsgSecondaryId = GetLocalResourceObject("ErrorMsgSecIDLength").ToString();
                                this.spanStyleSecondaryId = "";
                                this.txtSecId.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        if (hdnPrimId.Value == "false" && txtSecId.Text.ToString() == "")
                        {
                            this.errMsgPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString();
                            this.spanStylePrimaryId = "";
                            this.txtPrimId.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    //Associate Customer
                    if (Convert.ToInt16(hdnNumberOfCustomers.Value) > 1)
                    {
                        if (!dvAssociateCustomer.Disabled)
                        {
                            if (this.ddlAssoRace.SelectedValue == "- Select race -" && (!Convert.ToBoolean(hdnAssoRace.Value)))
                            {
                                //this.errMsgAssoRace = "Please select valid Race.";
                                this.errMsgAssoRace = GetLocalResourceObject("ErrorMsgValidRace").ToString();
                                this.spanStyleAssoRace = "";
                                this.ddlAssoRace.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                            if (this.rdoAssoLanguage.SelectedValue == "- Select Language -" && (!Convert.ToBoolean(hdnAssoLanguage.Value)))
                            {
                                // this.errMsgRace = "Please select valid Langauge.";
                                this.errMsgAssoLanguage = GetLocalResourceObject("ErrorMsgValidLanguage").ToString();
                                this.spanStyleAssoLanguage = "";
                                this.rdoAssoLanguage.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                            if (txtAssoPrimId.Text == "" && txtAssoSecId.Text == "")
                            {
                                if (hdnAssoPrimId.Value == "false")
                                {
                                    this.errMsgAssoPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString(); //"Please enter valid Primary Id";
                                    this.spanStyleAssoPrimaryId = "";
                                    this.txtAssoPrimId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                                if (hdnAssoSecId.Value == "false")
                                {
                                    this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();
                                    this.spanStyleAssoSecondaryId = "";
                                    this.txtAssoSecId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }
                            if (txtAssoPrimId.Text == "" || txtAssoSecId.Text == "")
                            {
                                if (hdnAssoPrimId.Value == "false" || hdnAssoSecId.Value == "false")
                                {
                                    this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgValidPrimorSecID").ToString();
                                    this.spanStyleAssoPrimaryId = "";
                                    this.spanStyleAssoSecondaryId = "";
                                    this.txtAssoPrimId.CssClass = "errorFld";
                                    this.txtAssoSecId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }


                            if (txtAssoPrimId.Text != "" && txtAssoSecId.Text == "")
                            {

                                if (!Helper.IsRegexMatch(this.txtAssoPrimId.Text.Trim(), strAlphNum, Convert.ToBoolean(hdnAssoPrimId.Value), false))
                                {
                                    this.errMsgAssoPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString();
                                    this.spanStyleAssoPrimaryId = "";
                                    this.txtAssoPrimId.CssClass = "errorFld";
                                    bErrorFlag = false;

                                }
                                if (!string.IsNullOrEmpty(hdnAssoPrimIdMinValue.Value))
                                {
                                    if (txtAssoPrimId.Text.Length < int.Parse(hdnAssoPrimIdMinValue.Value.ToString()))
                                    {
                                        this.errMsgAssoPrimaryId = GetLocalResourceObject("ErrorMsgPrimIDLength").ToString();//"Please enter valid Primary Id of appropriate length";
                                        this.spanStyleAssoPrimaryId = "";
                                        this.txtAssoPrimId.CssClass = "errorFld";
                                        bErrorFlag = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(hdnPrimIdMaxValue.Value))
                                {
                                    if (txtAssoPrimId.Text.Length > int.Parse(hdnAssoPrimIdMaxValue.Value.ToString()))
                                    {
                                        this.errMsgAssoPrimaryId = GetLocalResourceObject("ErrorMsgPrimIDLength").ToString();
                                        this.spanStyleAssoPrimaryId = "";
                                        this.txtAssoPrimId.CssClass = "errorFld";
                                        bErrorFlag = false;
                                    }
                                }

                                if (hdnAssoSecId.Value == "false" && txtAssoPrimId.Text.ToString() == "")
                                {
                                    this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();
                                    this.spanStyleAssoSecondaryId = "";
                                    this.txtAssoSecId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }
                            if (txtAssoPrimId.Text == "" && txtAssoSecId.Text != "")
                            {
                                if (!Helper.IsRegexMatch(this.txtAssoSecId.Text.Trim(), strAlphNum, Convert.ToBoolean(hdnAssoSecId.Value), false))
                                {
                                    this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgValidSecID").ToString();
                                    this.spanStyleAssoSecondaryId = "";
                                    this.txtAssoSecId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }

                                if (!string.IsNullOrEmpty(hdnAssoSecIdMinValue.Value))
                                {
                                    if (txtAssoSecId.Text.Length < int.Parse(hdnAssoSecIdMinValue.Value.ToString()))
                                    {
                                        this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgSecIDLength").ToString();//"Please enter valid Secondary Id of appropriate length";
                                        this.spanStyleAssoSecondaryId = "";
                                        this.txtAssoSecId.CssClass = "errorFld";
                                        bErrorFlag = false;
                                    }
                                }
                                if (!string.IsNullOrEmpty(hdnAssoSecIdMaxValue.Value))
                                {
                                    if (txtAssoSecId.Text.Length > int.Parse(hdnAssoSecIdMaxValue.Value.ToString()))
                                    {
                                        this.errMsgAssoSecondaryId = GetLocalResourceObject("ErrorMsgSecIDLength").ToString();
                                        this.spanStyleAssoSecondaryId = "";
                                        this.txtAssoSecId.CssClass = "errorFld";
                                        bErrorFlag = false;
                                    }
                                }
                                if (hdnAssoPrimId.Value == "false" && txtAssoSecId.Text.ToString() == "")
                                {
                                    this.errMsgAssoPrimaryId = GetLocalResourceObject("ErrorMsgValidPrimID").ToString();
                                    this.spanStyleAssoPrimaryId = "";
                                    this.txtAssoPrimId.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }
                        }
                    }
                }


                if (!Convert.ToBoolean(hdnAddressGroupconfig.Value))
                {
                    if (ddlAddress.SelectedValue == string.Empty && txtAddressLine1.Text.Trim() == string.Empty)
                    {
                        spanStyleAddress = "";
                        errMsgAddress = GetLocalResourceObject("ValidAddForFindAddress").ToString(); //"Please select an Address or enter a house no./name";//ValidAddForFindAddress
                        spanClassAddress = "errorFld dtAddress";
                        bErrorFlag = false;
                    }
                }


                if (hdnFirstName.Value == "true")
                {
                    if (!Helper.IsRegexMatch(this.txtFirstName0.Text.Trim(), regForeName, Convert.ToBoolean(hdnName1.Value), false) || (this.txtFirstName0.Text.Trim().Length == 1))
                    {
                        //this.errMsgFirstName = "Please note First Name is required, or the entered name is not valid.";
                        errMsgFirstName = GetLocalResourceObject("errMsgFirstName").ToString();
                        this.spanStyleFirstName0 = "";
                        this.txtFirstName0.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }
                if (hdnIsMiddleName.Value == "true")
                {
                    if (!Helper.IsRegexMatch(this.txtInitial0.Text.Trim(), regInitial, Convert.ToBoolean(hdnName2.Value), false))
                    {
                        // this.errMsgMiddleName = "Please enter a valid letter";
                        errMsgMiddleName = GetLocalResourceObject("errMsgMiddleName").ToString();
                        this.spanStyleMiddleName0 = "";
                        this.txtInitial0.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }
                //Defect MKTG00007835:Validation for Main Customer Starts
                //if (!Helper.IsRegexMatch(this.txtMobileNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnMobile.Value), false))
                //{
                //    //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                //    this.errMsgMobileNumber = GetLocalResourceObject("dataintegritysms").ToString();
                //    this.spanStyleMoblieNumber = "";
                //    this.txtMobileNumber.CssClass = "errorFld";
                //    bErrorFlag = false;
                //}
                //if (!Helper.IsRegexMatch(this.txtEmail.Text.Trim(), regMail, Convert.ToBoolean(hdnEmail.Value), false))
                //{
                //    //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                //    this.errMsgEmail = GetLocalResourceObject("dataintegrityemail").ToString();
                //    this.spanEmail = "";
                //    this.txtEmail.CssClass = "errorFld";
                //    bErrorFlag = false;
                //}
                //Defect MKTG00007835:Validation for Main Customer Ends

                //Surname
                if (ConfigurationManager.AppSettings["Culture"].ToString() != "en-US")
                {
                    if (hdnSurName.Value == "true")
                    {
                        if (!Helper.IsRegexMatch(this.txtSurname0.Text.Trim(), regSurName, Convert.ToBoolean(hdnName3.Value), false) || (this.txtSurname0.Text.Trim().Length == 1))
                        {
                            this.errMsgSurname = GetLocalResourceObject("errMsgSurname").ToString();
                            this.spanStyleSurname0 = "";
                            this.txtSurname0.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                }
                else // For US 
                {
                    if (!Helper.IsRegexMatch(txtSurname0.Text.Trim(), regSurName, false, false))
                    {
                        // errMsgSurname = "Please note Last Name is required.";
                        errMsgSurname = GetLocalResourceObject("ReqLastNameValidation").ToString();
                        spanStyleSurname0 = "";
                        txtSurname0.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }
                //CCMCA-847:disable error message for txtbox address
                //Added  hdnAddressGroupconfig.Value check condition bcz below validations should happen for Group CCMCA-3644
                if (hdnAddressGroupconfig.Value != "false")
                {
                    if (!Helper.IsRegexMatch(this.txtAddressLine1.Text.Trim(), strAlphNum2, Convert.ToBoolean(hdnAddressLine1.Value), false) || (this.txtAddressLine1.Text.Trim().Length == 1))
                    {
                        this.errMsgAddressLine1 = GetLocalResourceObject("ValidForAdd1FindAdd").ToString(); //"Please note House No is required, or the entered House No is not valid.";//ValidForAdd1FindAdd
                        this.spanStyleAddressLine1 = "";
                        this.txtAddressLine1.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    else if ((!string.IsNullOrEmpty(hdAddressLine1MinValue.Value))
                           && txtAddressLine1.Text.Trim().Length < Convert.ToInt16(hdAddressLine1MinValue.Value.Trim()))
                    {
                        this.errMsgAddressLine1 = GetLocalResourceObject("ValidForAdd1FindAdd").ToString();//"Please note House No is required, or the entered House No is not valid.";//ValidForAdd1FindAdd
                        this.spanStyleAddressLine1 = "";
                        this.txtAddressLine1.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    if (!Helper.IsRegexMatch(this.txtStreet.Text.Trim(), strAlphNum2, Convert.ToBoolean(hdnAddressLine2.Value), false) || (this.txtStreet.Text.Trim().Length.ToString() == ConfigurationManager.AppSettings["AddressLineLen"].ToString()))
                    {
                        this.errMsgAddressLine2 = GetLocalResourceObject("ValidForAdd2FindAdd").ToString();//"Please note Address Line2 is required, or the entered Address is not valid.";//ValidForAdd2FindAdd
                        this.spanStyleAddressLine2 = "";
                        this.txtStreet.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if (hdnAddressLine2.Value == "false")
                    {
                        if ((!string.IsNullOrEmpty(hdAddressLine2MinValue.Value)) && txtStreet.Text.Trim().Length < Convert.ToInt16(hdAddressLine2MinValue.Value.Trim()))
                        {
                            this.errMsgAddressLine2 = GetLocalResourceObject("ValidForAdd2FindAdd").ToString();//"Please note Address Line2 is required, or the entered Address is not valid.";//ValidForAdd2FindAdd
                            this.spanStyleAddressLine2 = "";
                            this.txtStreet.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                }
                //CCMCA-847:disable error message for txtbox address

                if (!Helper.IsRegexMatch(this.txtLocality.Text.Trim(), strAlphNum2, Convert.ToBoolean(hdnAddressLine3.Value), false) || (this.txtLocality.Text.Trim().Length == 1))
                {
                    this.errMsgAddressLine3 = GetLocalResourceObject("ValidForAdd3FindAdd").ToString();//"Please note Address Line3 is required, or the entered Address is not valid.";//ValidForAdd3FindAdd
                    this.spanStyleAddressLine3 = "";
                    this.txtLocality.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                else if (hdnAddressLine3.Value == "false")
                {
                    if ((!string.IsNullOrEmpty(hdAddressLine3MinValue.Value)) && txtLocality.Text.Trim().Length < Convert.ToInt16(hdAddressLine3MinValue.Value.Trim()))
                    {
                        this.errMsgAddressLine3 = GetLocalResourceObject("ValidForAdd3FindAdd").ToString();//"Please note Address Line3 is required, or the entered Address is not valid.";//ValidForAdd3FindAdd
                        this.spanStyleAddressLine3 = "";
                        this.txtLocality.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }
                if (!Helper.IsRegexMatch(this.txtTown.Text.Trim(), strAlphNum2, Convert.ToBoolean(hdnAddressLine4.Value), false) || (this.txtTown.Text.Trim().Length == 1))
                {
                    this.errMsgAddressLine4 = GetLocalResourceObject("ValidForAdd4FindAdd").ToString();//"Please note Address Line4 is required, or the entered Address is not valid.";//ValidForAdd4FindAdd
                    this.spanStyleAddressLine4 = "";
                    this.txtTown.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                else if (hdnAddressLine4.Value == "false")
                {
                    if ((!string.IsNullOrEmpty(hdAddressLine4MinValue.Value)) && txtTown.Text.Trim().Length < Convert.ToInt16(hdAddressLine4MinValue.Value.Trim()))
                    {
                        this.errMsgAddressLine4 = GetLocalResourceObject("ValidForAdd4FindAdd").ToString();//"Please note Address Line4 is required, or the entered Address is not valid.";//ValidForAdd4FindAdd
                        this.spanStyleAddressLine4 = "";
                        this.txtTown.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }

                if (!Helper.IsRegexMatch(this.txtCountyDetails.Text.Trim(), strAlphNum2, Convert.ToBoolean(hdnAddressLine5.Value), false) || (this.txtCountyDetails.Text.Trim().Length == 1))
                {
                    this.errMsgAddressLine5 = GetLocalResourceObject("ValidForAdd5FindAdd").ToString();//"Please note Address Line5 is required, or the entered Address is not valid.";//ValidForAdd5FindAdd
                    this.spanStyleAddressLine5 = "";
                    this.txtCountyDetails.CssClass = "errorFld";
                    bErrorFlag = false;
                }

                else if (hdnAddressLine5.Value == "false")
                {
                    if ((!string.IsNullOrEmpty(hdAddressLine5MinValue.Value)) && txtCountyDetails.Text.Trim().Length < Convert.ToInt16(hdAddressLine5MinValue.Value.Trim()))
                    {
                        //this.errMsgAddressLine5 = "Please note Address Line5 is required, or the entered Address is not valid.";//ValidForAdd5FindAdd
                        this.errMsgAddressLine5 = GetLocalResourceObject("ValidForAdd5FindAdd").ToString();
                        this.spanStyleAddressLine5 = "";
                        this.txtCountyDetails.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }

                //Email
                //NGC Change
                if (culture != "en-US")
                {

                    //NGC 3.6 - Email Mandatory and RE.
                    if (!Helper.IsRegexMatch(this.txtEmailAddress.Text.Trim(), regMail, Convert.ToBoolean(hdnEmail.Value), false))
                    {
                        //this.errMsgEmail = "Please note the Email address entered is required or not valid";
                        errMsgEmailAddress = GetLocalResourceObject("validationOfEmail").ToString();
                        this.spanStyleEmailAddress = "";
                        this.txtEmailAddress.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                }
                else
                {

                    if (!Helper.IsRegexMatch(txtEmail.Text.Trim(), regMail, false, false))
                    {
                        // errMsgEmail = "Please enter a valid Email Address.";
                        errMsgEmail = GetLocalResourceObject("validationOfEmail").ToString();
                        spanEmail = "";
                        txtEmail.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                }



                //DOB Mandatory
                if (!Convert.ToBoolean(hdnDOB.Value))
                {
                    if (((ddlDay0.SelectedValue == "" || ddlDay0.SelectedValue == "Day") && (ddlMonth0.SelectedValue == "" || ddlMonth0.SelectedValue == "- Select Month -")
                   && (ddlYear0.SelectedValue == "" || ddlYear0.SelectedValue == "Year")))
                    {
                        this.errMsgDOB = GetLocalResourceObject("DOBValidation").ToString();
                        this.spanStyleDOB0 = "";
                        this.spanClassDOBDropDown0 = "dtFld errorFld ";
                        bErrorFlag = false;
                    }
                }

                //DOB
                //DOB
                if ((ddlDay0.SelectedValue == "" && (ddlMonth0.SelectedValue == "" || ddlMonth0.SelectedValue == "- Select Month -")
                    && (ddlYear0.SelectedValue == "" || ddlYear0.SelectedValue == "Year")))
                {
                    //DOB is empty then don't validate as it not required field.
                }
                else
                {
                    if (Culture != "en-US")
                    {
                        System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                        if ((ddlDay0.SelectedValue == "" || ddlMonth0.SelectedValue == "" || ddlYear0.SelectedValue == "") ||
                              (!DateTime.TryParse(ddlDay0.SelectedValue + "/" + ddlMonth0.SelectedValue + "/" + ddlYear0.SelectedValue, enGBCulture, DateTimeStyles.None, out dob)))
                        {
                            //errMsgDOB = "Date Of Birth is invalid";
                            errMsgDOB = GetLocalResourceObject("DOBValidation").ToString();
                            spanStyleDOB0 = "";
                            spanClassDOBDropDown0 = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                        else if (GetAge(dob) < 18)
                        {
                            //errMsgDOB = "Please note you must be over 18 to be a member of Clubcard";
                            errMsgDOB = GetLocalResourceObject("AgeValidationForClubCard").ToString();
                            spanStyleDOB0 = "";
                            spanClassDOBDropDown0 = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                    }
                }
                if (culture == "en-US")
                {
                    customerObj = new CustomerServiceClient();
                    if (!customerObj.UpdateEmailAddresss(txtEmail.Text.ToString().Trim(), Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"))))
                    {
                        //errMsgEmail = "Email Id Already Exist";
                        errMsgEmail = GetLocalResourceObject("DupEmailCheck").ToString();
                        spanEmail = "";
                        txtEmail.CssClass = "errorFld";
                        bErrorFlag = false;

                    }
                    else
                    {

                        txtEmail.CssClass = "display:none";
                    }
                }
                //Gender
                //To configure mandatoty Fields
                if (MainGender.Visible)
                {
                    if ((!Convert.ToBoolean(hdnSex.Value)) && (!this.radioMale0.Checked && !this.radioFemale0.Checked))
                    {
                        //errMsgGender = "Please select Gender";
                        errMsgGender = GetLocalResourceObject("ReqGenderMsg").ToString();
                        spanStyleGender0 = "";
                        spanClassGender = "gender errorFld";
                        bErrorFlag = false;
                    }
                    else if ((radioMale0.Checked && hdnISTitle.Value.ToUpper() == "TRUE" && 
                                (ddlTitle0.SelectedValue.ToUpper() == "MRS" || ddlTitle0.SelectedValue.ToUpper() == "MISS" || ddlTitle0.SelectedValue.ToUpper() == "MS"))
                     || ((radioFemale0.Checked && hdnISTitle.Value.ToUpper() == "TRUE" && ddlTitle0.SelectedValue.ToUpper() == "MR")))//If Title doesn't match the gender
                    {
                        //errMsgGender = "Sorry, the Gender selected doesn't match with the Title choosen";
                        errMsgGender = GetLocalResourceObject("errMsgGender").ToString();
                        spanStyleGender0 = "";
                        spanClassGender = "gender errorFld";
                        bErrorFlag = false;
                    }
                }

                if ((Convert.ToInt32(hdnNumberOfCustomers.Value) > 1) && (!dvAssociateCustomer.Disabled))
                {
                    if (hdnFirstName.Value == "true")
                    {
                        if (!Helper.IsRegexMatch(this.txtFirstName1.Text.Trim(), regForeName, Convert.ToBoolean(hdnName1.Value), false) || (this.txtFirstName1.Text.Trim().Length == 1))
                        {
                            //this.errMsgFirstName = "Please note First Name is required, or the entered name is not valid.";
                            errMsgFirstName = GetLocalResourceObject("errMsgFirstName").ToString();
                            this.spanStyleFirstName1 = "";
                            this.txtFirstName1.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    if (!Helper.IsRegexMatch(this.txtAssocEmailAddress.Text.Trim(), regMail, Convert.ToBoolean(hdnEmail.Value), false))
                    {
                        //this.errMsgEmail = "Please note the Email address entered is required or not valid";
                        errMsgAssocEmailAddress = GetLocalResourceObject("validationOfEmail").ToString();
                        this.spanStyleAssocEmailAddress = "";
                        this.txtAssocEmailAddress.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    //Associate Evening Phone Number
                    if (hdnIsEvenNumbr.Value == "true")
                    {
                        if (!Helper.IsRegexMatch(this.txtAssocEveningPhoneNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnEvening.Value), false))
                        {
                            // this.errMsgMobileNumber = "Mobile phone number is required or not a valid number";//mobileVali
                            this.errMsgAssocEveningPhoneNumber = GetLocalResourceObject("EveningValid").ToString();
                            this.spanStyleAssocEveningPhoneNumber = "";
                            this.txtAssocEveningPhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        else if (!string.IsNullOrEmpty(txtAssocEveningPhoneNumber.Text))//If the configured data has more than one value(comma seperated)
                        {
                            if (hdnPhoneNoPrefix.Value.Contains(','))
                            {
                                string[] evePrefixes = hdnPhoneNoPrefix.Value.Split(',');
                                bool flgEvePrefix = false;

                                for (int i = 0; i < evePrefixes.Length; i++)
                                {
                                    if (txtAssocEveningPhoneNumber.Text.Trim().Substring(0, evePrefixes[i].Trim().Length) == evePrefixes[i].ToString())
                                    {
                                        flgEvePrefix = true;
                                        break;
                                    }
                                }

                                if (!flgEvePrefix)
                                {
                                    this.errMsgAssocEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";//ValidateMobile
                                    this.spanStyleAssocEveningPhoneNumber = "";
                                    this.txtAssocEveningPhoneNumber.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }
                            else if (txtAssocEveningPhoneNumber.Text.Trim().Substring(0, hdnPhoneNoPrefix.Value.Trim().Length) != hdnPhoneNoPrefix.Value)
                            {
                                this.errMsgAssocEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";
                                this.spanStyleAssocEveningPhoneNumber = "";
                                this.txtAssocEveningPhoneNumber.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                            else if ((!string.IsNullOrEmpty(hdnPhoneNoMinVal.Value))
                                    && txtAssocEveningPhoneNumber.Text.Trim().Length < Convert.ToInt16(hdnPhoneNoMinVal.Value.Trim()))
                            {
                                this.errMsgAssocEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";
                                this.spanStyleAssocEveningPhoneNumber = "";
                                this.txtAssocEveningPhoneNumber.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                    }
                    if (!Helper.IsRegexMatch(this.txtAssocDaytimePhoneNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnlandline.Value), false))
                    {
                        //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                        this.errMsgAssocDaytimePhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                        this.spanStyleAssocDaytimePhoneNumber = "";
                        this.txtAssocDaytimePhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    //Associate Phone Number
                    if (!Helper.IsRegexMatch(this.txtAssocDaytimePhoneNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnlandline.Value), false))
                    {
                        //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                        this.errMsgAssocDaytimePhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                        this.spanStyleAssocDaytimePhoneNumber = "";
                        this.txtAssocDaytimePhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if (!string.IsNullOrEmpty(txtAssocDaytimePhoneNumber.Text))    //If the configured data has more than one value(comma seperated)
                    {
                        if (hdnPhoneNoPrefix.Value.Contains(','))
                        {
                            string[] phonePrefixes = hdnPhoneNoPrefix.Value.Split(',');
                            bool flgPhonePrefix = false;

                            for (int i = 0; i < phonePrefixes.Length; i++)
                            {
                                if (txtAssocDaytimePhoneNumber.Text.Trim().Substring(0, phonePrefixes[i].Trim().Length) == phonePrefixes[i].ToString())
                                {
                                    flgPhonePrefix = true;
                                    break;
                                }
                            }

                            if (!flgPhonePrefix)
                            {
                                //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                                this.errMsgAssocDaytimePhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                                this.spanStyleAssocDaytimePhoneNumber = "";
                                this.txtAssocDaytimePhoneNumber.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        else if (txtAssocDaytimePhoneNumber.Text.Trim().Substring(0, hdnPhoneNoPrefix.Value.Trim().Length) != hdnPhoneNoPrefix.Value)
                        {
                            // this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                            this.errMsgAssocDaytimePhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                            this.spanStyleAssocDaytimePhoneNumber = "";
                            this.txtAssocDaytimePhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        else if ((!string.IsNullOrEmpty(hdnPhoneNoMinVal.Value))
                                && txtAssocDaytimePhoneNumber.Text.Trim().Length < Convert.ToInt16(hdnPhoneNoMinVal.Value.Trim()))
                        {
                            // this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                            this.errMsgAssocDaytimePhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                            this.spanStyleAssocDaytimePhoneNumber = "";
                            this.txtAssocDaytimePhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    //Associate Mobile Number
                    if (!Helper.IsRegexMatch(this.txtAssocMobileNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnMobile.Value), false))
                    {
                        // this.errMsgMobileNumber = "Mobile phone number is required or not a valid number";//mobileVali
                        this.errMsgAssocMobileNumber = GetLocalResourceObject("mobileVali").ToString();
                        this.spanStyleAssocMoblieNumber = "";
                        this.txtAssocMobileNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if (!string.IsNullOrEmpty(txtAssocMobileNumber.Text))//If the configured data has more than one value(comma seperated)
                    {
                        if (hdnMobileNoPrefix.Value.Contains(','))
                        {
                            string[] mobPrefixes = hdnMobileNoPrefix.Value.Split(',');
                            bool flgMobPrefix = false;

                            for (int i = 0; i < mobPrefixes.Length; i++)
                            {
                                if (txtAssocMobileNumber.Text.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                {
                                    flgMobPrefix = true;
                                    break;
                                }
                            }

                            if (!flgMobPrefix)
                            {
                                this.errMsgAssocMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";//ValidateMobile
                                this.spanStyleAssocMoblieNumber = "";
                                this.txtAssocMobileNumber.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        else if (txtAssocMobileNumber.Text.Trim().Substring(0, hdnMobileNoPrefix.Value.Trim().Length) != hdnMobileNoPrefix.Value)
                        {
                            this.errMsgAssocMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";
                            this.spanStyleAssocMoblieNumber = "";
                            this.txtAssocMobileNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        else if ((!string.IsNullOrEmpty(hdnMobileNoMinVal.Value))
                                && txtAssocMobileNumber.Text.Trim().Length < Convert.ToInt16(hdnMobileNoMinVal.Value.Trim()))
                        {
                            this.errMsgAssocMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";
                            this.spanStyleAssocMoblieNumber = "";
                            this.txtAssocMobileNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    if (hdnIsMiddleName.Value == "true")
                    {
                        if (!Helper.IsRegexMatch(this.txtInitial1.Text.Trim(), regInitial, Convert.ToBoolean(hdnName2.Value), false))
                        {
                            //this.errMsgMiddleName = "Please enter a valid letter";
                            errMsgMiddleName = GetLocalResourceObject("errMsgMiddleName").ToString();
                            this.spanStyleMiddleName1 = "";
                            this.txtInitial1.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }

                    if (hdnSurName.Value == "true")
                    {
                        if (!Helper.IsRegexMatch(this.txtSurname1.Text.Trim(), regSurName, Convert.ToBoolean(hdnName3.Value), false) || (this.txtSurname1.Text.Trim().Length == 1))
                        {
                            // this.errMsgSurname = "Please note Surname is required, or the entered name is not valid.";
                            errMsgSurname = GetLocalResourceObject("errMsgSurname").ToString();
                            this.spanStyleSurname1 = "";
                            this.txtSurname1.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }

                    //Defect MKTG00007835:Validation for Associate Customer Starts
                    //if (!Helper.IsRegexMatch(this.txtAssocMobileNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnMobile.Value), false))
                    //{
                    //    //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                    //    this.errMsgAssocMobileNumber = GetLocalResourceObject("dataintegritysms").ToString();
                    //    this.spanStyleAssocMoblieNumber = "";
                    //    this.txtAssocMobileNumber.CssClass = "errorFld";
                    //    bErrorFlag = false;
                    //}
                    //if (!Helper.IsRegexMatch(this.txtAssocEmailAddress.Text.Trim(), regMail, Convert.ToBoolean(hdnEmail.Value), false))
                    //{
                    //    //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                    //    this.errMsgAssocEmailAddress = GetLocalResourceObject("dataintegrityemail").ToString();
                    //    this.spanStyleAssocEmailAddress = "";
                    //    this.txtAssocEmailAddress.CssClass = "errorFld";
                    //    bErrorFlag = false;
                    //}
                    //Defect MKTG00007835:Validation for Associate Customer Ends
                    //DOB Mandatory
                    if (!Convert.ToBoolean(hdnDOB.Value))
                    {
                        if (((ddlDay0.SelectedValue == "" || ddlDay0.SelectedValue == "Day") && (ddlMonth0.SelectedValue == "" || ddlMonth0.SelectedValue == "- Select Month -")
                       && (ddlYear0.SelectedValue == "" || ddlYear0.SelectedValue == "Year")))
                        {
                            this.errMsgDOB = GetLocalResourceObject("DOBValidation").ToString();
                            this.spanStyleDOB0 = "";
                            this.spanClassDOBDropDown0 = "dtFld errorFld ";
                            bErrorFlag = false;
                        }
                    }
                    if (!Convert.ToBoolean(hdnDOB.Value))
                    {
                        if (((ddlDay1.SelectedValue == "" || ddlDay1.SelectedValue == "Day") && (ddlMonth1.SelectedValue == "" || ddlMonth1.SelectedValue == "- Select Month -")
                       && (ddlYear1.SelectedValue == "" || ddlYear1.SelectedValue == "Year")))
                        {
                            this.errMsgDOB = GetLocalResourceObject("DOBValidation").ToString();
                            this.spanStyleDOB1 = "";
                            this.spanClassDOBDropDown1 = "dtFld errorFld ";
                            bErrorFlag = false;
                        }
                    }
                    //DOB




                    if ((ddlDay1.SelectedValue == "" && (ddlMonth1.SelectedValue == "" || ddlMonth1.SelectedValue == "- Select Month -")
                        && (ddlYear1.SelectedValue == "" || ddlYear1.SelectedValue == "Year")))
                    {
                        //DOB is empty then don't validate as it not required field. Fix of Defect MKTG00003574
                    }
                    else
                    {
                        if ((ddlDay1.SelectedValue == "" || ddlMonth1.SelectedValue == "" || ddlYear1.SelectedValue == "") ||
                              (!DateTime.TryParse(ddlDay1.SelectedValue + "/" + ddlMonth1.SelectedValue + "/" + ddlYear1.SelectedValue, out dob)))
                        {
                            //errMsgDOB = "Date Of Birth is invalid";
                            errMsgDOB = GetLocalResourceObject("DOBValidation").ToString();
                            spanStyleDOB1 = "";
                            spanClassDOBDropDown1 = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                        else if (GetAge(dob) < 18)
                        {
                            //errMsgDOB = "Please note you must be over 18 to be a member of Clubcard";
                            errMsgDOB = GetLocalResourceObject("AgeValidationForClubCard").ToString();
                            spanStyleDOB1 = "";
                            spanClassDOBDropDown1 = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                    }

                    //Gender
                    //To configure mandatoty Fields
                    if (MainGender.Visible)
                    {
                        if ((!Convert.ToBoolean(hdnSex.Value)) && (!this.radioMale1.Checked && !this.radioFemale1.Checked))
                        {
                            //errMsgGender = "Please select Gender";
                            errMsgGender = GetLocalResourceObject("ReqGenderMsg").ToString();
                            spanStyleGender1 = "";
                            spanClassGender = "gender errorFld";
                            bErrorFlag = false;
                        }
                    }

                    else if ((radioMale1.Checked && hdnISTitle.Value.ToUpper() == "TRUE" && (ddlTitle1.SelectedValue.ToUpper() == "MRS" || ddlTitle1.SelectedValue.ToUpper() == "MISS" || ddlTitle1.SelectedValue.ToUpper() == "MS"))
                        || ((radioFemale1.Checked && hdnISTitle.Value.ToUpper() == "TRUE" && ddlTitle1.SelectedValue.ToUpper() == "MR")))//If Title doesn't match the gender
                    {
                        //errMsgGender = "Sorry, the Gender selected doesn't match with the Title choosen";
                        errMsgGender = GetLocalResourceObject("errMsgGender").ToString();
                        spanStyleGender1 = "";
                        spanClassGender1 = "gender errorFld";
                        bErrorFlag = false;
                    }
                }

                //PostCode
                //To configure mandatoty Fields
                if (culture != "en-US")
                {

                    if (!Helper.IsRegexMatch(this.txtPostCode.Text.Trim(), regPostCode, Convert.ToBoolean(hdnPostcode.Value), false) && !Helper.IsRegexMatch(this.txtPostCode.Text.Trim(), regPostCode1, Convert.ToBoolean(hdnPostcode.Value), false))
                    {
                        //if txtPostCode is hided no validation required.
                        if (hdnHidepostcodeFields.Value != "0")
                        {
                            this.spanStylePostCode = "";
                            //this.errMsgPostCode = "Postcode is required or not valid";
                            errMsgPostCode = GetLocalResourceObject("PostCodeValidation").ToString();
                            this.txtPostCode.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }

                }
                else
                {
                    //if txtPostCode is hided no validation required.
                    if (hdnHidepostcodeFields.Value != "0")
                    {
                        if (!Helper.IsRegexMatch(txtPostCode.Text.Trim(), regPostCodeForUS, true, true))
                        {
                            spanStylePostCode = "";
                            //errMsgPostCode = "Postcode is not valid";
                            errMsgPostCode = GetLocalResourceObject("ERRorPostCodeValidation").ToString();
                            txtPostCode.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }


                }
                if (!Convert.ToBoolean(hdnNewpostcode.Value))
                {
                    txtStreet.Text = string.Empty;
                    txtTown.Text = string.Empty;
                    spanStylePostCode = "";
                    errMsgPostCode = "Sorry, unable to find the Postcode ";
                    txtPostCode.CssClass = "errorFld";
                    ddlAddress.Items.Clear();
                    btnSaveAddress.Visible = false;
                }
                //NGC 3.6 - Mandatory Fields - Mobile Number and Telephone number.
                //Evening Phone Number
                if (hdnIsEvenNumbr.Value == "true")
                {
                    if (!Helper.IsRegexMatch(this.txtEveningPhoneNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnEvening.Value), false))
                    {
                        // this.errMsgMobileNumber = "Mobile phone number is required or not a valid number";//mobileVali
                        this.errMsgEveningPhoneNumber = GetLocalResourceObject("EveningValid").ToString();
                        this.spanStyleEveningPhoneNumber = "";
                        this.txtEveningPhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if (!string.IsNullOrEmpty(txtEveningPhoneNumber.Text))//If the configured data has more than one value(comma seperated)
                    {
                        if (hdnPhoneNoPrefix.Value.Contains(','))
                        {
                            string[] evePrefixes = hdnPhoneNoPrefix.Value.Split(',');
                            bool flgEvePrefix = false;

                            for (int i = 0; i < evePrefixes.Length; i++)
                            {
                                if (txtEveningPhoneNumber.Text.Trim().Substring(0, evePrefixes[i].Trim().Length) == evePrefixes[i].ToString())
                                {
                                    flgEvePrefix = true;
                                    break;
                                }
                            }

                            if (!flgEvePrefix)
                            {
                                this.errMsgEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";//ValidateMobile
                                this.spanStyleEveningPhoneNumber = "";
                                this.txtEveningPhoneNumber.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        else if (txtEveningPhoneNumber.Text.Trim().Substring(0, hdnPhoneNoPrefix.Value.Trim().Length) != hdnPhoneNoPrefix.Value)
                        {
                            this.errMsgEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";
                            this.spanStyleEveningPhoneNumber = "";
                            this.txtEveningPhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        else if ((!string.IsNullOrEmpty(hdnPhoneNoMinVal.Value))
                                && txtEveningPhoneNumber.Text.Trim().Length < Convert.ToInt16(hdnPhoneNoMinVal.Value.Trim()))
                        {
                            this.errMsgEveningPhoneNumber = GetLocalResourceObject("ValidateEvening").ToString();//"Please note Mobile phone number entered is not a valid number";
                            this.spanStyleEveningPhoneNumber = "";
                            this.txtEveningPhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                }


                //Mobile Number
                if (!Helper.IsRegexMatch(this.txtMobileNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnMobile.Value), false))
                {
                    // this.errMsgMobileNumber = "Mobile phone number is required or not a valid number";//mobileVali
                    this.errMsgMobileNumber = GetLocalResourceObject("mobileVali").ToString();
                    this.spanStyleMoblieNumber = "";
                    this.txtMobileNumber.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                else if (!string.IsNullOrEmpty(txtMobileNumber.Text))//If the configured data has more than one value(comma seperated)
                {
                    if (hdnMobileNoPrefix.Value.Contains(','))
                    {
                        string[] mobPrefixes = hdnMobileNoPrefix.Value.Split(',');
                        bool flgMobPrefix = false;

                        for (int i = 0; i < mobPrefixes.Length; i++)
                        {
                            if (txtMobileNumber.Text.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                            {
                                flgMobPrefix = true;
                                break;
                            }
                        }

                        if (!flgMobPrefix)
                        {
                            this.errMsgMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";//ValidateMobile
                            this.spanStyleMoblieNumber = "";
                            this.txtMobileNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    else if (txtMobileNumber.Text.Trim().Substring(0, hdnMobileNoPrefix.Value.Trim().Length) != hdnMobileNoPrefix.Value)
                    {
                        this.errMsgMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";
                        this.spanStyleMoblieNumber = "";
                        this.txtMobileNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if ((!string.IsNullOrEmpty(hdnMobileNoMinVal.Value))
                            && txtMobileNumber.Text.Trim().Length < Convert.ToInt16(hdnMobileNoMinVal.Value.Trim()))
                    {
                        this.errMsgMobileNumber = GetLocalResourceObject("ValidateMobile").ToString();//"Please note Mobile phone number entered is not a valid number";
                        this.spanStyleMoblieNumber = "";
                        this.txtMobileNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }

                //Phone Number
                if (!Helper.IsRegexMatch(this.txtPhoneNumber.Text.Trim(), regPhoneNumber, Convert.ToBoolean(hdnlandline.Value), false))
                {
                    //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                    this.errMsgPhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                    this.spanStylePhoneNumber = "";
                    this.txtPhoneNumber.CssClass = "errorFld";
                    bErrorFlag = false;
                }
                else if (!string.IsNullOrEmpty(txtPhoneNumber.Text))    //If the configured data has more than one value(comma seperated)
                {
                    if (hdnPhoneNoPrefix.Value.Contains(','))
                    {
                        string[] phonePrefixes = hdnPhoneNoPrefix.Value.Split(',');
                        bool flgPhonePrefix = false;

                        for (int i = 0; i < phonePrefixes.Length; i++)
                        {
                            if (txtPhoneNumber.Text.Trim().Substring(0, phonePrefixes[i].Trim().Length) == phonePrefixes[i].ToString())
                            {
                                flgPhonePrefix = true;
                                break;
                            }
                        }

                        if (!flgPhonePrefix)
                        {
                            //this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                            this.errMsgPhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                            this.spanStylePhoneNumber = "";
                            this.txtPhoneNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    else if (txtPhoneNumber.Text.Trim().Substring(0, hdnPhoneNoPrefix.Value.Trim().Length) != hdnPhoneNoPrefix.Value)
                    {
                        // this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                        this.errMsgPhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                        this.spanStylePhoneNumber = "";
                        this.txtPhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    else if ((!string.IsNullOrEmpty(hdnPhoneNoMinVal.Value))
                            && txtPhoneNumber.Text.Trim().Length < Convert.ToInt16(hdnPhoneNoMinVal.Value.Trim()))
                    {
                        // this.errMsgPhoneNumber = "Please note Preferred contact number is required or not valid";
                        this.errMsgPhoneNumber = GetLocalResourceObject("MobileNumberValidationMsg").ToString();
                        this.spanStylePhoneNumber = "";
                        this.txtPhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                }


                //Validate Age
                //CCMCA-441
                //if (!ValidateAge(txtNoofPeople, false, ref errMsgNoHHPersons, ref spanStyleNoHHPersons)) bErrorAgeFlag = false;
                //if (!ValidateAge(txtAge1, true, ref errMsgAge1, ref spanStyleAge1)) bErrorAgeFlag = false;
                //if (!ValidateAge(txtAge2, true, ref errMsgAge2, ref spanStyleAge2)) bErrorAgeFlag = false;
                //if (!ValidateAge(txtAge3, true, ref errMsgAge3, ref spanStyleAge3)) bErrorAgeFlag = false;
                //if (!ValidateAge(txtAge4, true, ref errMsgAge4, ref spanStyleAge4)) bErrorAgeFlag = false;
                //if (!ValidateAge(txtAge5, true, ref errMsgAge5, ref spanStyleAge5)) bErrorAgeFlag = false;

                if (bErrorAgeFlag && ValidateAgeCount())//No age error then validate age count
                {
                    //No error
                    //Validate whether ages entered are equal to the family members in the House hold
                }
                else //If any error
                {
                    bErrorFlag = false;
                }
                return bErrorFlag;
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
        }

        /// <summary>
        /// To validate customer age
        /// </summary>
        /// <param name="textObject">TextBox</param>
        /// <param name="bFlag">bool</param>
        /// <param name="errMessage">ref string</param>
        /// <param name="spanStyle">ref string</param>
        /// <returns>boolean</returns>
        protected bool ValidateAge(TextBox textObject, bool bFlag, ref string errMessage, ref string spanStyle)
        {
            string regNumeric = @"^[0-9 ]*$";
            int age = 0;
            bool successFlag = true;
            try
            {
                if (textObject.Text.Trim() != "")
                {
                    if (!Helper.IsRegexMatch(textObject.Text.Trim(), regNumeric, true, false))
                    {
                        errMessage = GetLocalResourceObject("ErrNumberValidationMsg").ToString();
                        spanStyle = "";

                        textObject.CssClass = "errorFld";
                        successFlag = false;
                        return successFlag;

                    }
                    else
                    {
                        age = Convert.ToInt32(textObject.Text);

                        if (!Helper.IsRegexMatch(textObject.Text.Trim(), regNumeric, true, false)
                            || (bFlag == true && age < 0) || age > 99 || (bFlag == false && age < 1))
                        {
                            if (bFlag)
                            {
                                // errMessage = "Please enter an Age between 0 and 99";
                                errMessage = GetLocalResourceObject("AgeValidationMsg").ToString();
                                spanStyle = "";
                            }
                            else
                            {
                                // errMessage = "Please enter a valid number between 1 and 99";
                                errMessage = GetLocalResourceObject("ErrNumberValidationMsg").ToString();
                                spanStyle = "";
                            }

                            textObject.CssClass = "errorFld";
                            successFlag = false;
                            return successFlag;
                        }
                        else
                        {
                            textObject.CssClass = "";
                        }
                    }
                }
                textObject.CssClass = "";
            }
            catch (FormatException exp)
            {
                if (bFlag)
                {
                    //errMessage = "Please enter an Age between 0 and 99";
                    errMessage = GetLocalResourceObject("AgeValidationMsg").ToString();
                    spanStyle = "";
                }
                else
                {
                    //errMessage = "Please enter a valid number between 1 and 99";
                    errMessage = GetLocalResourceObject("ErrNumberValidationMsg").ToString();
                    spanStyle = "";
                }

                successFlag = false;

                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }

            return successFlag;
        }

        /// <summary>
        /// To validate customer family ages count against number of household memebrs
        /// </summary>
        /// <returns>boolean</returns>

        protected bool ValidateAgeCount()
        {
            try
            {
                //string noHHPersons = txtNoofPeople.Text;
                string age1 = txtAge1.Text.Trim();
                //string age2 = txtAge2.Text.Trim();
                //string age3 = txtAge3.Text.Trim();
                //string age4 = txtAge4.Text.Trim();
                //string age5 = txtAge5.Text.Trim();
                //CCMCA-441
                string age2 = ddlAge2.SelectedValue;
                string age3 = ddlAge3.SelectedValue;
                string age4 = ddlAge4.SelectedValue;
                string age5 = ddlAge5.SelectedValue;
                string age6 = ddlAge6.SelectedValue;

                int ageCount = 0;
                //int errBoxNumber = 0;

                //string errMessage = "Sorry, you have entered too many ages for the size of the household. Please check and re-enter.";
                string errMessage = GetLocalResourceObject("errMessageForAge").ToString();
                if (age1 != "")
                {
                    ageCount++;
                    //errBoxNumber = 1;
                }

                if (age2.ToUpper() != "YEAR")
                {
                    ageCount++;
                    // errBoxNumber = 2;
                }

                if (age3.ToUpper() != "YEAR")
                {
                    ageCount++;
                    //errBoxNumber = 3;
                }

                if (age4.ToUpper() != "YEAR")
                {
                    ageCount++;
                    //errBoxNumber = 4;
                }

                if (age5.ToUpper() != "YEAR")
                {
                    ageCount++;
                    //errBoxNumber = 5;
                }
                if (age6.ToUpper() != "YEAR")
                {
                    ageCount++;
                }

                this.txtNoofPeople.Text = ageCount.ToString();

                //if (noHHPersons.Trim() == "")
                //    noHHPersons = "0";

                //if (ageCount >= Convert.ToInt16(noHHPersons) && (ageCount != 0))
                //{
                //    if (errBoxNumber == 1)
                //    {
                //        errMsgAge1 = errMessage;
                //        txtAge1.CssClass = "errorFld";
                //        spanStyleAge1 = "";
                //    }
                //    else if (errBoxNumber == 2)
                //    {
                //        errMsgAge2 = errMessage;
                //        txtAge2.CssClass = "errorFld";
                //        spanStyleAge2 = "";
                //    }
                //    else if (errBoxNumber == 3)
                //    {
                //        errMsgAge3 = errMessage;
                //        txtAge3.CssClass = "errorFld";
                //        spanStyleAge3 = "";
                //    }
                //    else if (errBoxNumber == 4)
                //    {
                //        errMsgAge4 = errMessage;
                //        txtAge4.CssClass = "errorFld";
                //        spanStyleAge4 = "";
                //    }
                //    else if (errBoxNumber == 5)
                //    {
                //        errMsgAge5 = errMessage;
                //        txtAge5.CssClass = "errorFld";
                //        spanStyleAge5 = "";
                //    }

                //    return false;
                //}

                return true;
            }
            catch (Exception exp)
            {
                Logger.Write(exp, "General", 1, 4500, System.Diagnostics.TraceEventType.Error,
                    "Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                throw exp;
            }
        }

        /// <summary>
        /// To set household status on LHN
        /// </summary>
        /// <param name="pCustomerID">Primary customer ID</param>
        public void SetHouseHoldStatus(long pCustomerID)
        {
            string customerUserStatus = string.Empty;
            string customerMailStatus = string.Empty;

            string CustomerEmailStatus = string.Empty;
            string CustomerMobilePhoneStatus = string.Empty;
            try
            {
                // Check house hold status of primary customer
                if (clubcardObj.CheckHouseholdStatusOfCustomer(out errorXml, out resultXml, pCustomerID, culture))
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        dsCustomerHouseholdStatus = new DataSet();
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsCustomerHouseholdStatus.ReadXml(new XmlNodeReader(resulDoc));

                        if (dsCustomerHouseholdStatus.Tables.Count > 0)
                        {
                            if (dsCustomerHouseholdStatus.Tables[0].Columns.Contains("CustomerUseStatus") != false)
                            {
                                if (dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerUseStatus"].ToString().Trim() != string.Empty)
                                {
                                    customerUserStatus = dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerUseStatus"].ToString();
                                    //Store in cookie to check in other pages
                                    Helper.SetTripleDESEncryptedCookie("customerUserStatus", customerUserStatus);
                                }

                            }
                            if (dsCustomerHouseholdStatus.Tables[0].Columns.Contains("CustomerEmailStatus") != false)
                            {
                                if (dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerEmailStatus"].ToString().Trim() != string.Empty)
                                {
                                    CustomerEmailStatus = dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerEmailStatus"].ToString();
                                    //Store in cookie to check in other pages
                                    Helper.SetTripleDESEncryptedCookie("CustomerEmailStatus", CustomerEmailStatus);
                                }
                            }

                            if (dsCustomerHouseholdStatus.Tables[0].Columns.Contains("CustomerMailStatus") != false)
                            {
                                if (dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerMailStatus"].ToString().Trim() != string.Empty)
                                {
                                    customerMailStatus = dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerMailStatus"].ToString();
                                    //Store in cookie to check in other pages
                                    Helper.SetTripleDESEncryptedCookie("customerMailStatus", customerMailStatus);
                                }
                            }

                            if (dsCustomerHouseholdStatus.Tables[0].Columns.Contains("CustomerMobilePhoneStatus") != false)
                            {
                                if (dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerMobilePhoneStatus"].ToString().Trim() != string.Empty)
                                {
                                    CustomerMobilePhoneStatus = dsCustomerHouseholdStatus.Tables[0].Rows[0]["CustomerMobilePhoneStatus"].ToString();
                                    //Store in cookie to check in other pages
                                    Helper.SetTripleDESEncryptedCookie("CustomerMobilePhoneStatus", CustomerMobilePhoneStatus);
                                }
                            }
                        }
                    }

                    ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");

                    HtmlControl spnBannedError = (HtmlControl)leftNav.FindControl("spnBannedError");
                    HtmlControl spnLeftError = (HtmlControl)leftNav.FindControl("spnLeftError");
                    HtmlControl spnDuplicateError = (HtmlControl)leftNav.FindControl("spnDuplicateError");
                    HtmlControl spnAddressError = (HtmlControl)leftNav.FindControl("spnAddressError");
                    HtmlControl spnEmailError = (HtmlControl)leftNav.FindControl("spnEmailError");
                    HtmlControl spnMobileNoError = (HtmlControl)leftNav.FindControl("spnMobileNoError");

                    if (customerUserStatus != "1" || customerMailStatus != "1")
                    {
                        // for banned house hold --2
                        if (customerUserStatus == "2")
                        {
                            spnBannedError.Visible = true;
                        }
                        // for Left Scheme --3
                        else if (customerUserStatus == "3")
                        {
                            spnLeftError.Visible = true;
                        }
                        //for duplicate
                        else if (customerUserStatus == "5")
                        {
                            spnDuplicateError.Visible = true;
                        }
                        else
                        {
                            //for address in error --3
                            if (customerMailStatus == "3")
                            {
                                spnAddressError.Visible = true;
                            }
                            if (CustomerEmailStatus == "8")
                            {
                                spnEmailError.Visible = true;
                            }
                            if (CustomerMobilePhoneStatus == "8")
                            {
                                spnMobileNoError.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("CheckHouseholdStatusOfCustomer Function failed. Customer ID:" + Convert.ToString(pCustomerID) + ": ErrorXML" + errorXml);
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerDetail.Page_Load()- Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerDetail.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerDetail.Page_Load()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                //Close the sercise connections.
                if (customerObj != null)
                {
                    if (customerObj.State == CommunicationState.Faulted)
                    {
                        customerObj.Abort();
                    }
                    else if (customerObj.State != CommunicationState.Closed)
                    {
                        customerObj.Close();
                    }
                }

                if (clubcardObj != null)
                {
                    if (clubcardObj.State == CommunicationState.Faulted)
                    {
                        clubcardObj.Abort();
                    }
                    else if (clubcardObj.State != CommunicationState.Closed)
                    {
                        clubcardObj.Close();
                    }
                }
            }
        }

        #region PAF Martini Code FindAddress
        /// <summary>
        /// It loads the address details corresponding to the post code entered
        /// </summary>
        /// <param name="source">object</param>
        /// <param name="e">ImageClickEventArgs</param>
        protected void btnFindAddress_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string enableAddressLookupAPI = ConfigurationManager.AppSettings["enableAddressLookupAPI"].ToString();

                if (enableAddressLookupAPI.ToUpper() == "TRUE")
                {
                    populateAddressFromAPI();
                    return;
                }



                string culture = ConfigurationManager.AppSettings["Culture"].ToString();
                string postCode = string.Empty;
                postCode = txtPostCode.Text.Trim().ToUpper();
                txtPostCode.CssClass = "";//Clear the CSS
                if (lblSuccessMessage.Text != "")
                {
                    lblSuccessMessage.Text = "";//Clear the success message when the Find button is clicked.
                }
                ArrayList buildingNoStreetList = null;
                ArrayList buildingNameStreetList = null;
                int noOfRows = 0;
                txtCountyDetails.Text = string.Empty;
                txtTown.Text = string.Empty;
                txtStreet.Text = string.Empty;
                txtLocality.Text = string.Empty;

                //PAF Service call from Martini
                if (GetAddressesByPostcode(postCode))
                {
                    ddlAddress.Items.Clear();
                    txtAddressLine1.Text = "";

                    if (dsAddressList.Tables.Count > 0)
                    {
                        buildingNoStreetList = new ArrayList();
                        buildingNameStreetList = new ArrayList();
                        btnSaveAddress.Visible = true;//make the save button visible
                        buildingNoStreetListWithoutStreet = new ArrayList();
                        hdnNewpostcode.Value = "true";
                        noOfRows = dsAddressList.Tables[0].Rows.Count;

                        if (dsAddressList.Tables[0].Rows[0]["BuildingNumber"].ToString() != string.Empty)
                        {
                            for (int i = 0; i < noOfRows; i++)
                            {
                                if (dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim() != string.Empty)
                                {
                                    if (dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() != string.Empty)
                                    {
                                        if (dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNoStreetList.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());

                                        }
                                        else
                                        {
                                            buildingNoStreetList.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                    }
                                    else if (dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() != string.Empty)
                                    {
                                        if (dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNoStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                        else
                                        {
                                            buildingNoStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                    }
                                    else
                                    {
                                        buildingNoStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                    }
                                }
                            }
                        }
                        else if (dsAddressList.Tables[0].Rows[0]["BuildingName"].ToString() != string.Empty)
                        {
                            if (dsAddressList.Tables[0].Rows[0]["BuildingName"].ToString() != string.Empty)
                            {
                                for (int i = 0; i < noOfRows; i++)
                                {
                                    if (dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() != string.Empty)
                                    {
                                        if (dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                        else
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["SubBuilding"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                    }
                                    else if (dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() != string.Empty)
                                    {
                                        if (dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                        else
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingNumber"].ToString().Trim() + " ," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                    }
                                    else
                                    {
                                        if (dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["BuildingName"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                        else if (dsAddressList.Tables[0].Rows[i]["Organisation"].ToString().Trim() != string.Empty)
                                        {
                                            buildingNameStreetList.Add(dsAddressList.Tables[0].Rows[i]["Organisation"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                            buildingNoStreetListWithoutStreet.Add(dsAddressList.Tables[0].Rows[i]["Organisation"].ToString().Trim() + "," + dsAddressList.Tables[0].Rows[i]["Street"].ToString().Trim());
                                        }
                                    }
                                }
                            }
                        }

                        if (buildingNoStreetList.Count > 0)
                        {
                            buildingNoStreetList.Sort();
                            ddlAddress.DataSource = buildingNoStreetList;
                            ddlAddress.DataBind();
                        }
                        else
                        {
                            if (buildingNameStreetList.Count > 0)
                            {
                                buildingNameStreetList.Sort();
                                ddlAddress.DataSource = buildingNameStreetList;
                                ddlAddress.DataBind();
                            }
                        }

                        if (buildingNoStreetListWithoutStreet.Count > 0)
                        {
                            buildingNoStreetListWithoutStreet.Sort();
                            ddlbuildingNoStreetListWithoutStreet.DataSource = buildingNoStreetListWithoutStreet;
                            ddlbuildingNoStreetListWithoutStreet.DataBind();
                        }
                        if (dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim() != string.Empty)
                        {
                            if (dsAddressList.Tables[0].Rows[0]["SubBuilding"].ToString().Trim() != string.Empty)
                            {
                                if (dsAddressList.Tables[0].Rows[0]["BuildingNumber"].ToString().Trim() != string.Empty)
                                {
                                    txtStreet.Text = dsAddressList.Tables[0].Rows[0]["BuildingNumber"].ToString().Trim() + " " + dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                                }
                                else
                                {
                                    txtStreet.Text = dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                                }
                            }
                            else
                            {
                                txtStreet.Text = dsAddressList.Tables[0].Rows[0]["Street"].ToString().Trim();
                            }

                        }

                        else
                        {
                            if (dsAddressList.Tables[0].Rows[0]["Locality"].ToString().Trim() != string.Empty)
                            {
                                txtStreet.Text = string.Empty;
                            }
                        }
                        if (dsAddressList.Tables[0].Rows[0]["Town"].ToString().Trim() != string.Empty)
                        {
                            txtTown.Text = dsAddressList.Tables[0].Rows[0]["Town"].ToString().Trim();
                        }
                        else
                        {
                            txtTown.Text = string.Empty;
                        }

                        if (dsAddressList.Tables[0].Rows[0]["Locality"].ToString().Trim() != string.Empty)
                        {
                            txtLocality.Text = dsAddressList.Tables[0].Rows[0]["Locality"].ToString().Trim();
                        }
                        else
                        {
                            txtLocality.Text = string.Empty;
                        }
                        if (dsAddressList.Tables[0].Rows[0]["County"].ToString().Trim() != string.Empty)
                        {
                            liCounty.Visible = true;
                            txtCountyDetails.Text = dsAddressList.Tables[0].Rows[0]["County"].ToString().Trim();
                        }
                        else
                        {
                            liCounty.Visible = false;
                        }
                    }
                    else
                    {
                        txtStreet.Text = string.Empty;
                        txtTown.Text = string.Empty;
                        spanStylePostCode = "";
                        errMsgPostCode = "Sorry, unable to find the Postcode ";
                        txtPostCode.CssClass = "errorFld";
                        ddlAddress.Items.Clear();
                        btnSaveAddress.Visible = false; //make the save button invisible if post code is not proper
                        hdnNewpostcode.Value = "false";
                    }

                    ddlAddress.Items.Insert(0, new ListItem("Click the arrow to select your home ->", ""));

                    hdnPostCodeNumber.Value = postCode;

                    //Add Javascript to the Save Addess button
                    btnSaveAddress.Attributes.Add("onclick", "return ValidatePage('" + ddlTitle0.ClientID + "','"
                        + txtFirstName0.ClientID + "','"
                        + txtInitial0.ClientID + "','"
                        + txtSurname0.ClientID + "','"
                        + ddlDay0.ClientID + "','"
                        + ddlMonth0.ClientID + "','"
                        + ddlYear0.ClientID + "','"
                        + radioMale0.ClientID + "','"
                        + radioFemale0.ClientID + "','"
                        + ddlTitle1.ClientID + "','"
                        + txtFirstName1.ClientID + "','"
                        + txtInitial1.ClientID + "','"
                        + txtSurname1.ClientID + "','"
                        + ddlDay1.ClientID + "','"
                        + ddlMonth1.ClientID + "','"
                        + ddlYear1.ClientID + "','"
                        + radioMale1.ClientID + "','"
                        + radioFemale1.ClientID + "','"
                        + txtPostCode.ClientID + "','"
                        + txtPhoneNumber.ClientID + "','"
                        + txtNoofPeople.ClientID + "','"
                        + txtAge1.ClientID + "','"
                        //+ txtAge2.ClientID + "','"
                        //+ txtAge3.ClientID + "','"
                        //+ txtAge4.ClientID + "','"
                        //+ txtAge5.ClientID + "','"
                        + ddlAge2.ClientID + "','"
                        + ddlAge3.ClientID + "','"
                        + ddlAge4.ClientID + "','"
                        + ddlAge5.ClientID + "','"
                        + ddlAge6.ClientID + "','"
                        + hdnPostCodeNumber.ClientID + "','"
                        + ddlAddress.ClientID + "','"
                        + txtAddressLine1.ClientID + "','"
                        + txtStreet.ClientID + "','"
                        + txtTown.ClientID + "','"
                        + hdnNumberOfCustomers.ClientID + "','"
                        + lblSuccessMessage.ClientID + "','"
                        + dvAssociateCustomer.ClientID + "','"

                        + culture + "')");

                }
                else
                {
                    ddlAddress.Items.Clear();
                }

                //CCMCA-441
                if (ddlYear0.SelectedValue.ToUpper() != "YEAR")
                    txtAge1.Text = ddlYear0.SelectedValue;
                else
                    txtAge1.Text = "Year";
            }
            catch (Exception exp)
            {
                ddlAddress.Items.Clear();
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerDetail.btnFindAddress_Click()- Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerDetail.btnFindAddress_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerDetail.btnFindAddress_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                dsAddressList = null;
                dsAddressDetails = null;
            }
        }
        private void populateAddressFromAPI()
        {
            string token = getAccessToken();
            RestClient obj = new RestClient();
            string postCode = string.Empty;
            postCode = txtPostCode.Text.Trim().ToUpper();

            postCode = postCode.Replace(" ", "");
            List<addressList> addressList = new List<addressList>();
            addressList = obj.getAddressDetails(postCode, "Bearer " + token);

            ddlAddress.DataSource = addressList;
            ddlAddress.DataTextField = "concatAddressLineValue";
            ddlAddress.DataBind();

            txtTown.Text = addressList[0].postTown;
            txtStreet.Text = addressList[0].streetValue;



        }
        #endregion

        #region Martini PAF service is used

        public bool GetAddressesByPostcode(string postCode)
        {
            bool boolResult = true;
            LocatorSvcSDAClient client = null;
            string addresses = string.Empty;
            string addressDetails = string.Empty;

            try
            {
                client = new LocatorSvcSDAClient();
                dsAddressList = new DataSet();
                dsAddressDetails = new DataSet();

                addresses = client.FindAddressLite(postCode, null, null);

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerDetail.GetAddressesByPostcode() addresses-" + addresses);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.GetAddressesByPostcode() addresses-" + addresses);
                #endregion

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
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerDetail.GetAddressesByPostcode() addresses-" + addresses);
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CardRanges.GetAddressesByPostcode() addresses-" + addresses);
                #endregion
                return boolResult;
            }

            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerDetail.GetAddressesByPostcode()- Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerDetail.GetAddressesByPostcode() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerDetail.GetAddressesByPostcode()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                return false;
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
        }
        #endregion

        /// <summary>
        ///  <Author>
        ///  Neeta 
        ///  </Author>
        ///  To Set Mandatory Configuration from Database.
        /// </summary>

        public void SetMandatoryConfigurations()
        {
            string AddressLine1MinValue = string.Empty;
            string AddressLine1MaxValue = string.Empty;
            string AddressLine2MinValue = string.Empty;
            string AddressLine2MaxValue = string.Empty;
            string AddressLine3MinValue = string.Empty;
            string AddressLine3MaxValue = string.Empty;
            string AddressLine4MinValue = string.Empty;
            string AddressLine4MaxValue = string.Empty;
            string AddressLine5MinValue = string.Empty;
            string AddressLine5MaxValue = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CutomerDetails.SetMandatoryConfigurations");
                NGCTrace.NGCTrace.TraceDebug("Start:CSC CutomerDetails.SetMandatoryConfigurations");

                if (!base.IsPostBack)
                {

                    //********* Release3.6 - Configuration values
                    string phoneNoMinValue = string.Empty;
                    string phoneNoMaxValue = string.Empty;
                    string mobileNoMinValue = string.Empty;
                    string mobileNoMaxValue = string.Empty;
                    string postCodeMaxVal = string.Empty;
                    string postCodeMinValue = string.Empty;
                    string accountTypeMaxVal = string.Empty;
                    DataSet dsConfigDetails = new DataSet();
                    string primaryIdMaxLength = string.Empty;
                    string secondaryIdMaxLength = string.Empty;
                    string primaryIdMinLength = string.Empty;
                    string secondaryIdMinLength = string.Empty;

                    conditionXML = "2,4,5,6,8,9,10,11,21,25,19,27";
                    customerObj = new CustomerServiceClient();
                    txtPostCode.Attributes.Add("MaxLength", "30");
                    txtAddressLine1.Attributes.Add("MaxLength", "30");
                    txtStreet.Attributes.Add("MaxLength", "30");
                    txtLocality.Attributes.Add("MaxLength", "30");
                    txtTown.Attributes.Add("MaxLength", "30");
                    txtCountyDetails.Attributes.Add("MaxLength", "30");


                    //CR13 Changes
                    ddlCustomerStatus.Visible = false;
                    ddlEmailStatus.Visible = false;
                    ddlMobileStatus.Visible = false;
                    ddlMailStatus.Visible = false;
                    ddlAssocCustStatus.Visible = false;
                    ddlAssoEmailStatus.Visible = false;
                    ddlAssoMobileStatus.Visible = false;

                    //End:CR13 Changes

                    if (customerObj.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
                    {
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                        ViewState["Config"] = dsConfigDetails;

                        if (dsConfigDetails.Tables.Count > 0)
                        {
                            //fill race dropdown
                            if (dsConfigDetails.Tables.Contains("Table1") && dsConfigDetails.Tables["Table1"].Rows.Count > 0)
                            {
                                ddlRace.DataSource = dsConfigDetails.Tables["Table1"];
                                ddlRace.DataValueField = dsConfigDetails.Tables["Table1"].Columns[0].ToString();
                                //ddlRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                //ddlRace.DataBind();
                                //ddlRace.Items.Insert(0, "- Select race -");
                                //CCMCA-4852 fix race localization issue
                                if (culture == "en-GB")
                                {
                                    ddlRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                    ddlRace.DataBind();
                                    ddlRace.Items.Insert(0, GetLocalResourceObject("selectRace").ToString());
                                }
                                else
                                {
                                    ddlRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                    ddlRace.DataBind();
                                    ddlRace.Items.Insert(0, GetLocalResourceObject("selectRace").ToString());
                                }

                                ddlAssoRace.DataSource = dsConfigDetails.Tables["Table1"];
                                ddlAssoRace.DataValueField = dsConfigDetails.Tables["Table1"].Columns[0].ToString();
                                //ddlAssoRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                //ddlAssoRace.DataBind();
                                //ddlAssoRace.Items.Insert(0, "- Select race -");
                                //CCMCA-4852 fix race localization issue
                                if (culture == "en-GB")
                                {
                                    ddlAssoRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                    ddlAssoRace.DataBind();
                                    ddlAssoRace.Items.Insert(0, GetLocalResourceObject("selectRace").ToString());
                                }
                                else
                                {
                                    ddlAssoRace.DataTextField = dsConfigDetails.Tables["Table1"].Columns[1].ToString();
                                    ddlAssoRace.DataBind();
                                    ddlAssoRace.Items.Insert(0, GetLocalResourceObject("selectRace").ToString());
                                }
                            }
                            if (dsConfigDetails.Tables.Contains("Table2") && dsConfigDetails.Tables["Table2"].Rows.Count > 0)
                            {
                                rdoLanguage.DataSource = dsConfigDetails.Tables["Table2"];
                                rdoLanguage.DataValueField = dsConfigDetails.Tables["Table2"].Columns[0].ToString();
                                rdoLanguage.DataTextField = dsConfigDetails.Tables["Table2"].Columns[1].ToString();
                                rdoLanguage.DataBind();
                                rdoLanguage.Items.Insert(0, "- Select Language -");

                                rdoAssoLanguage.DataSource = dsConfigDetails.Tables["Table2"];
                                rdoAssoLanguage.DataValueField = dsConfigDetails.Tables["Table2"].Columns[0].ToString();
                                rdoAssoLanguage.DataTextField = dsConfigDetails.Tables["Table2"].Columns[1].ToString();
                                rdoAssoLanguage.DataBind();
                                rdoAssoLanguage.Items.Insert(0, "- Select Language -");
                            }
                            if (enableProvince)
                            {
                                ddlProvince.Visible = true;
                                txtCountyDetails.Visible = false;
                                if (dsConfigDetails.Tables.Contains("Table3") && dsConfigDetails.Tables["Table3"].Rows.Count > 0)
                                {
                                    DataTable tblProvince = dsConfigDetails.Tables["Table3"];
                                    DataView dvProvince = new DataView(tblProvince);

                                    ddlProvince.DataValueField = dsConfigDetails.Tables["Table3"].Columns[0].ToString();
                                    if (culture == "en-GB")
                                    {
                                        dvProvince.Sort = dsConfigDetails.Tables["Table3"].Columns[1].ToString();
                                        ddlProvince.DataTextField = dsConfigDetails.Tables["Table3"].Columns[1].ToString();
                                    }
                                    else
                                    {
                                        ddlProvince.DataTextField = dsConfigDetails.Tables["Table3"].Columns[2].ToString();
                                    }
                                    ddlProvince.DataSource = dvProvince;
                                    ddlProvince.DataBind();
                                    ddlProvince.Items.Insert(0, "- Select province -");
                                }
                            }
                            foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                            {
                                if (dr["ConfigurationType"].ToString().Trim() == "6" && dr["ConfigurationName"].ToString().Trim() == "GroupCountryAddress")
                                {
                                    hdnAddressGroupconfig.Value = "true";
                                    btnFindAddress.Visible = false;
                                    ddlAddress.Visible = false;
                                    lclchoseadd.Visible = false;
                                    imgAddress.Visible = false;
                                    liAddress.Attributes.Add("style", "display:none");
                                }

                                //For Hiding functionality...

                                //Hide PostCode
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HidePostCode")
                                {
                                    hdnHidepostcodeFields.Value = "0";
                                    divlblpostcode.Visible = false;
                                    divhidetxtpostcode.Visible = false;
                                }

                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "ChinaHiddenFunctionalityMiddleName")
                                {
                                    middlenamemain.Visible = true;
                                    MiddleName = true;
                                    hdnIsMiddleName.Value = "true";

                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "ChinaHiddenFunctionalityTitle")
                                {
                                    hdnISTitle.Value = "true";
                                    titlemain.Visible = true;
                                    Title = true;

                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideGender")
                                {
                                    MainGender.Visible = true;
                                    gender = true;
                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "ChinaHiddenFunctionalityEveningPhoneNo")
                                {
                                    EveningNumberMain.Visible = true;
                                    EveningPhonenumber = true;
                                    hdnIsEvenNumbr.Value = "true";
                                }

                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideFirstName")
                                {
                                    fnamelist.Visible = true;
                                    firstName = true;
                                    hdnFirstName.Value = "true";
                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideSurName")
                                {
                                    liSurnameMain.Visible = true;
                                    surname = true;
                                    hdnSurName.Value = "true";
                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HidePrimaryId")
                                {
                                    liPrimaryID.Attributes.Add("style", "display:block");
                                    liAssoPrimaryID.Attributes.Add("style", "display:block");
                                    hdnConfigVisible.Value = "true";
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideSecondaryId")
                                {
                                    liSecondaryID.Attributes.Add("style", "display:block");
                                    liAssoSecondaryID.Attributes.Add("style", "display:block");
                                    hdnConfigVisible.Value = "true";
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HidePreferredLanguage")
                                {
                                    liLanguage.Attributes.Add("style", "display:block");
                                    liAssoLanguage.Attributes.Add("style", "display:block");
                                    hdnConfigVisible.Value = "true";
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideRace")
                                {
                                    liRace.Attributes.Add("style", "display:block");
                                    liAssoRace.Attributes.Add("style", "display:block");
                                    hdnConfigVisible.Value = "true";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "Race" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandRace.Visible = true;
                                    imgAssoRace.Visible = true;
                                    hdnRace.Value = "false";
                                    hdnAssoRace.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "Language" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandLanguage.Visible = true;
                                    imgmandAssoLanguage.Visible = true;
                                    hdnLanguage.Value = "false";
                                    hdnAssoLanguage.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "PrimaryId" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandPrimId.Visible = true;
                                    imgmandAssoPrimId.Visible = true;
                                    hdnPrimId.Value = "false";
                                    hdnAssoPrimId.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "SecondaryId" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandSecId.Visible = true;
                                    imgAssomandSecId.Visible = true;
                                    hdnSecId.Value = "false";
                                    hdnAssoSecId.Value = "false";
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "PrimId")
                                {
                                    primaryIdMinLength = dr["ConfigurationValue1"].ToString();
                                    primaryIdMaxLength = dr["ConfigurationValue2"].ToString();

                                    hdnPrimIdMinValue.Value = primaryIdMinLength;
                                    hdnPrimIdMaxValue.Value = primaryIdMaxLength;

                                    hdnAssoPrimIdMinValue.Value = primaryIdMinLength;
                                    hdnAssoPrimIdMaxValue.Value = primaryIdMaxLength;

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(primaryIdMaxLength))
                                    {
                                        txtPrimId.Attributes.Add("MaxLength", primaryIdMaxLength);
                                        txtAssoPrimId.Attributes.Add("MaxLength", primaryIdMaxLength);
                                    }
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "SecId")
                                {
                                    secondaryIdMinLength = dr["ConfigurationValue1"].ToString();
                                    secondaryIdMaxLength = dr["ConfigurationValue2"].ToString();

                                    hdnSecIdMinValue.Value = secondaryIdMinLength;
                                    hdnSecIdMaxValue.Value = secondaryIdMaxLength;

                                    hdnAssoSecIdMinValue.Value = secondaryIdMinLength;
                                    hdnAssoSecIdMaxValue.Value = secondaryIdMaxLength;

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(secondaryIdMaxLength))
                                    {
                                        txtSecId.Attributes.Add("MaxLength", secondaryIdMaxLength);
                                        txtAssoSecId.Attributes.Add("MaxLength", secondaryIdMaxLength);
                                    }
                                }
                                else

                                    if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                                    {

                                        postCodeMinValue = dr["ConfigurationValue1"].ToString();
                                        postCodeMaxVal = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(postCodeMaxVal))
                                        {
                                            txtPostCode.Attributes.Add("MaxLength", postCodeMaxVal);
                                            txtPostCode.MaxLength = Convert.ToInt32(postCodeMaxVal);
                                        }
                                        hdnpostCodeMinVal2.Value = postCodeMinValue.ToString();

                                    }

                                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1")
                                    {


                                        AddressLine1MinValue = dr["ConfigurationValue1"].ToString();
                                        AddressLine1MaxValue = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(AddressLine1MaxValue))
                                        {
                                            txtAddressLine1.Attributes.Add("MaxLength", AddressLine1MaxValue);
                                            txtAddressLine1.MaxLength = Convert.ToInt32(AddressLine1MaxValue);
                                        }
                                        hdAddressLine1MinValue.Value = AddressLine1MinValue.ToString();

                                    }
                                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                                    {

                                        AddressLine2MinValue = dr["ConfigurationValue1"].ToString();
                                        AddressLine2MaxValue = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(AddressLine2MaxValue))
                                        {
                                            txtStreet.Attributes.Add("MaxLength", AddressLine2MaxValue);
                                            txtStreet.MaxLength = Convert.ToInt32(AddressLine2MaxValue);
                                        }
                                        hdAddressLine2MinValue.Value = AddressLine2MinValue.ToString();

                                    }

                                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                                    {
                                        AddressLine3MinValue = dr["ConfigurationValue1"].ToString();
                                        AddressLine3MaxValue = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(AddressLine3MaxValue))
                                        {
                                            txtLocality.Attributes.Add("MaxLength", AddressLine3MaxValue);
                                            txtLocality.MaxLength = Convert.ToInt32(AddressLine3MaxValue);
                                        }
                                        hdAddressLine3MinValue.Value = AddressLine3MinValue.ToString();

                                    }

                                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine4")
                                    {

                                        AddressLine4MinValue = dr["ConfigurationValue1"].ToString();
                                        AddressLine4MaxValue = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(AddressLine4MaxValue))
                                        {
                                            txtTown.Attributes.Add("MaxLength", AddressLine4MaxValue);
                                            txtTown.MaxLength = Convert.ToInt32(AddressLine4MaxValue);
                                        }
                                        hdAddressLine4MinValue.Value = AddressLine4MinValue.ToString();

                                    }
                                    else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                                    {

                                        AddressLine5MinValue = dr["ConfigurationValue1"].ToString();
                                        AddressLine5MaxValue = dr["ConfigurationValue2"].ToString();

                                        //If Max value is not configured in the table, then assign DB field length as Max length
                                        if (!string.IsNullOrEmpty(AddressLine4MaxValue))
                                        {
                                            txtCountyDetails.Attributes.Add("MaxLength", AddressLine5MaxValue);
                                            txtCountyDetails.MaxLength = Convert.ToInt32(AddressLine5MaxValue);
                                        }
                                        hdAddressLine5MinValue.Value = AddressLine5MinValue.ToString();
                                    }
                                if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                                {
                                    name3MinValue = dr["ConfigurationValue1"].ToString();
                                    name3MaxValue = dr["ConfigurationValue2"].ToString();

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(name3MaxValue))
                                    {

                                        txtSurname0.Attributes.Add("MaxLength", name3MaxValue);
                                        txtSurname1.Attributes.Add("MaxLength", name3MaxValue);

                                    }
                                    else
                                    {

                                        txtSurname0.Attributes.Add("MaxLength", "30");
                                        txtSurname1.Attributes.Add("MaxLength", "30");
                                    }

                                }
                                //  CCMCA-4851 Fix to make length of MiddleInitials configurable
                                if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                                {
                                    name2MinValue = dr["ConfigurationValue1"].ToString();
                                    name2MaxValue = dr["ConfigurationValue2"].ToString();

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(name2MaxValue))
                                    {

                                        txtInitial0.Attributes.Add("MaxLength", name2MaxValue);
                                        txtInitial1.Attributes.Add("MaxLength", name2MaxValue);

                                    }
                                    else
                                    {

                                        txtInitial0.Attributes.Add("MaxLength", "30");
                                        txtInitial1.Attributes.Add("MaxLength", "30");
                                    }

                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "IdFormat")
                                {
                                    hdnIdFormat.Value = dr["ConfigurationValue1"].ToString();
                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                                {
                                    hdnname2validation.Value = dr["ConfigurationValue1"].ToString();
                                }

                                else if (dr["ConfigurationName"].ToString().Trim() == "Name1" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandFN.Visible = true;
                                    imgmandFN1.Visible = true;
                                    hdnName1.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "Name2" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandMN.Visible = true;
                                    imgmandMN1.Visible = true;
                                    hdnName2.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "Name3" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandLN.Visible = true;
                                    imgmandLN1.Visible = true;
                                    hdnName3.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "TitleEnglish" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgTilte.Visible = true;
                                    imgmandTittle1.Visible = true;
                                    hdnTitle.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "DateOfBirth" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandDOB.Visible = true;
                                    imgmandDOB1.Visible = true;
                                    hdnDOB.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "Sex" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgGender.Visible = true;
                                    imgmandGender1.Visible = true;
                                    hdnSex.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgAddress.Visible = true;
                                    hdnAddressLine1.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgPostcode.Visible = true;
                                    hdnPostcode.Value = "false";
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine2")
                                {

                                    imgadd2.Visible = true;
                                    hdnAddressLine2.Value = "false";

                                }

                                else if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine3")
                                {
                                    imgadd3.Visible = true;
                                    hdnAddressLine3.Value = "false";

                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine4")
                                {

                                    imgadd4.Visible = true;
                                    hdnAddressLine4.Value = "false";

                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "2" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine5")
                                {
                                    imgadd5.Visible = true;
                                    hdnAddressLine5.Value = "false";

                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandPhoneNo.Visible = true;
                                    imgAssocDaytimePhoneNumber.Visible = true;
                                    hdnlandline.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgMandMobile.Visible = true;
                                    imgMandAssocMobile.Visible = true;
                                    hdnMobile.Value = "false";
                                }
                                else if (dr["ConfigurationName"].ToString().Trim() == "EveningPhoneNumber" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgEveningPhoneNumber.Visible = true;
                                    imgAssocEveningPhoneNumber.Visible = true;
                                    hdnEvening.Value = "false";
                                }

                                else if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress" && dr["ConfigurationType"].ToString().Trim() == "2")
                                {
                                    imgmandEmail.Visible = true;
                                    imgmandAssocEmail.Visible = true;
                                    hdnEmail.Value = "false";
                                }

                                //Retrieve the values from Dataset and store in local variables.
                                //Min and Max values for phone numbers.
                                else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                                {
                                    phoneNoMinValue = dr["ConfigurationValue1"].ToString();
                                    phoneNoMaxValue = dr["ConfigurationValue2"].ToString();

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(phoneNoMaxValue))
                                    {
                                        txtPhoneNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                        txtEveningPhoneNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                        txtAssocDaytimePhoneNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                        txtAssocEveningPhoneNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                    }
                                    else
                                    {
                                        txtPhoneNumber.Attributes.Add("MaxLength", "40");
                                        txtMobileNumber.Attributes.Add("MaxLength", "40");
                                        txtAssocDaytimePhoneNumber.Attributes.Add("MaxLength", "40");
                                        txtAssocEveningPhoneNumber.Attributes.Add("MaxLength", "40");
                                        txtEveningPhoneNumber.Attributes.Add("MaxLength", "40");
                                        txtAssocMobileNumber.Attributes.Add("MaxLength", "40");
                                    }

                                    hdnPhoneNoMinVal.Value = phoneNoMinValue.ToString();
                                }
                                //Min and Max values for Mobile phone numbers.
                                else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                                {
                                    mobileNoMinValue = dr["ConfigurationValue1"].ToString();
                                    mobileNoMaxValue = dr["ConfigurationValue2"].ToString();

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(mobileNoMaxValue))
                                    {
                                        txtMobileNumber.Attributes.Add("MaxLength", mobileNoMaxValue);
                                        txtAssocMobileNumber.Attributes.Add("MaxLength", mobileNoMaxValue);
                                    }

                                    hdnMobileNoMinVal.Value = mobileNoMinValue.ToString();
                                }

                                //For Landline number prefix
                                else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                                {
                                    hdnPhoneNoPrefix.Value = dr["ConfigurationValue1"].ToString();
                                }

                                //For Mobile phone number prefix
                                else if (dr["ConfigurationType"].ToString().Trim() == "9" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                                {
                                    hdnMobileNoPrefix.Value = dr["ConfigurationValue1"].ToString();
                                }

                                //For PostCode Min and Max values
                                else if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                                {
                                    hdnPostCodeMinVal.Value = dr["ConfigurationValue1"].ToString();
                                    postCodeMaxVal = dr["ConfigurationValue2"].ToString();

                                    //If Max value is not configured in the table, then assign DB field length as Max length
                                    if (!string.IsNullOrEmpty(postCodeMaxVal))
                                    {
                                        txtPostCode.Attributes.Add("MaxLength", postCodeMaxVal);
                                    }
                                    else
                                    {
                                        txtPostCode.Attributes.Add("MaxLength", "40");
                                    }
                                }

                                //For PostCode format
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                                {
                                    hdnPostCodeFormat.Value = dr["ConfigurationValue1"].ToString();
                                    hdnPostCodeFormat1.Value = dr["ConfigurationValue2"].ToString();
                                }
                                //For PostCode format
                                else if (dr["ConfigurationType"].ToString().Trim() == "21")
                                {
                                    hdnConfigDOB.Value = dr["ConfigurationValue1"].ToString();
                                }
                                if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "IdFormat")
                                {
                                    hdnPostcoderegexp.Value = dr["ConfigurationValue1"].ToString();
                                }
                                //For AddressLine1 to AddressLine5 req exp format for group
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine")
                                {
                                    hdnAddressLineFormat.Value = dr["ConfigurationValue1"].ToString();
                                }


                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                                {
                                    hdnname3reg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                                {
                                    hdnname1reg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                                {
                                    hdnmiddleinitialreg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                                {
                                    hdnemailreg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "PhoneNumber")
                                {
                                    hdnphonenumberreg.Value = dr["ConfigurationValue1"].ToString();
                                }
                                else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "USMailingAddressPostCode")
                                {
                                    hdnUSPostCodeFormat.Value = dr["ConfigurationValue1"].ToString();
                                }

                                //R1.6 Changes for Thank customer for opting less email statements
                                if (dr["ConfigurationType"].ToString().Trim() == "27")
                                {
                                    if (dr["ConfigurationName"].ToString().Trim() == "2")
                                    {
                                        hdnSendEmailForDietaryPref.Value = dr["ConfigurationValue1"].ToString().Trim();
                                    }
                                    else if (dr["ConfigurationName"].ToString().Trim() == "1")
                                    {
                                        hdnSendEmailForAllergyPref.Value = dr["ConfigurationValue1"].ToString().Trim();
                                    }
                                }
                                //R1.6 Changes for Thank customer for opting less email statements

                                //CR13 Changes
                                if (dr["ConfigurationType"].ToString().Trim() == "25" &&
                                   dr["ConfigurationName"].ToString().Trim() == "EditCustomerStatusSettings" &&
                                   hdnUpdateCustomerStatusCapability.Value == "true")
                                {
                                    lblCustomerStatus.Visible = false;
                                    lblEmailStatus.Visible = false;
                                    lblMobileStatus.Visible = false;
                                    lblAssocCustStatus.Visible = false;
                                    lblAssoEmailStatus.Visible = false;
                                    lblAssoMobileStatus.Visible = false;
                                    lblMailStatus.Visible = false;

                                    ddlCustomerStatus.Visible = true;
                                    ddlEmailStatus.Visible = true;
                                    ddlMobileStatus.Visible = true;
                                    ddlAssocCustStatus.Visible = true;
                                    ddlAssoEmailStatus.Visible = true;
                                    ddlAssoMobileStatus.Visible = true;
                                    ddlMailStatus.Visible = true;
                                    hdnEditCustomerStatusSettings.Value = true.ToString();

                                }
                                //End:CR13 Changes
                                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessDetails")
                                {
                                    divBusinessDetails.Visible = true;
                                    HideOrShowBusinessDetails(dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows);
                                }
                            }
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End: CSC CutomerDetails.SetMandatoryConfigurations");
                NGCTrace.NGCTrace.TraceDebug("End:CSC CutomerDetails.SetMandatoryConfigurations");
            }//End Try
            catch (Exception exception)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CSC CutomerDetails.SetMandatoryConfigurations - Error Message :" + exception.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CSC CutomerDetails.SetMandatoryConfigurations - Error Message :" + exception.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CSC CutomerDetails.SetMandatoryConfigurations");
                NGCTrace.NGCTrace.ExeptionHandling(exception);
                throw exception;
            }
            finally
            {

                if (this.customerObj != null)
                {
                    if (this.customerObj.State == CommunicationState.Faulted)
                    {
                        this.customerObj.Abort();
                    }
                    else if (this.customerObj.State != CommunicationState.Closed)
                    {
                        this.customerObj.Close();
                    }
                }
            }
        }


        public void HideOrShowBusinessDetails(DataRowCollection ds)
        {
            hdnHideBusinessDetails.Value = "false";
            foreach (DataRow dr in ds)
            {
                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessName")
                {
                    hdnHideBusniessName.Value = "false";
                    liBusniessName.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessType")
                {
                    hdnHideBusinessType.Value = "false";
                    liBusinessType.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessRegistrationNumber")
                {
                    hdnHideBusniessRegNo.Value = "false";
                    liBusniessRegNo.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress1")
                {
                    hdnHideBusinessAddr1.Value = "false";
                    liBusinessAddress1.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress2")
                {
                    hdnHideBusinessAddr2.Value = "false";
                    liBusinessAddress2.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress3")
                {
                    hdnHideBusinessAddr3.Value = "false";
                    liBusinessAddress3.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress4")
                {
                    hdnHideBusinessAddr4.Value = "false";
                    liBusinessAddress4.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress5")
                {
                    hdnHideBusinessAddr5.Value = "false";
                    liBusinessAddress5.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessAddress6")
                {
                    hdnHideBusinessAddr6.Value = "false";
                    liBusinessAddress6.Visible = true;
                }

                if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideBusinessPostCode")
                {
                    hdnHideBusinessPostcode.Value = "false";
                    liBusinessPostcode.Visible = true;
                }
            }
        }

        protected void HiddenField1_ValueChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// LoadBabtTodlerDetails to load the Baby Todler Club Data
        /// </summary>
        //public void LoadBabtTodlerDetails(long customerID, bool mainassocFlag)
        //{
        //    preferenceserviceClient = new PreferenceServiceClient();
        //    ClubDetails objClubs = new ClubDetails();
        //    DataTable dtPreference = new DataTable("BTClub");
        //    dtPreference.Columns.Add("DateOfBirth", typeof(string), null);
        //    dtPreference.Columns.Add("OriginalDateOfBirth", typeof(string), null);
        //    dtPreference.Columns.Add("MediaID", typeof(Int16), null);
        //    dtPreference.Columns.Add("ClubID", typeof(Int16), null);
        //    dtPreference.Columns.Add("MembershipID", typeof(string), null);
        //    dtPreference.Columns.Add("IsDeleted", typeof(string), null);

        //    DataTable dtMedia = new DataTable("Media");
        //    dtMedia.Columns.Add("MediaID", typeof(Int16), null);
        //    dtMedia.Columns.Add("MediaDescription", typeof(string), null);

        //    objClubs = preferenceserviceClient.ViewClubDetails(customerID);
        //    if (objClubs != null && objClubs.ClubInformation.Count > 0)
        //    {
        //        for (int i = 0; i < objClubs.ClubInformation.Count; i++)
        //        {
        //            if (objClubs.ClubInformation[i].ClubID == 1 && objClubs.ClubInformation[i].DateOfBirth != "" && objClubs.ClubInformation[i].IsDeleted == "N")
        //            {
        //                dtPreference.Rows.Add("", objClubs.ClubInformation[i].DateOfBirth, objClubs.ClubInformation[i].Media, objClubs.ClubInformation[i].ClubID,
        //                    objClubs.ClubInformation[i].MembershipID, objClubs.ClubInformation[i].IsDeleted);
        //            }
        //        }
        //        dtPreference.AcceptChanges();
        //    }
        //    //Date Of Birth
        //    for (int i = 0; i < dtPreference.Rows.Count; i++)
        //    {
        //        DateTime dob;
        //        dob = Convert.ToDateTime(dtPreference.Rows[i]["OriginalDateOfBirth"]);

        //        ////if date is not valid leave it blank
        //        if (dob.ToString("dd/MM/yyyy") != "01/01/0001" && dob.ToString("dd/MM/yyyy") != "01/01/1901")
        //        {
        //            if (mainassocFlag)
        //            {
        //                if (i == 0)
        //                {
        //                    ddlBT1.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT1.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT1.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 1)
        //                {
        //                    ddlBT2.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT2.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT2.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 2)
        //                {
        //                    ddlBT3.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT3.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT3.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 3)
        //                {
        //                    ddlBT4.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT4.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT4.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 4)
        //                {
        //                    ddlBT5.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT5.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT5.SelectedValue = dob.Year.ToString();
        //                }
        //                ViewState["BTOriginalDetailsPrimary"] = dtPreference;
        //            }
        //            else
        //            {
        //                if (i == 0)
        //                {
        //                    ddlBT6.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT6.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT6.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 1)
        //                {
        //                    ddlBT7.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT7.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT7.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 2)
        //                {
        //                    ddlBT8.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT8.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT8.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 3)
        //                {
        //                    ddlBT9.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT9.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT9.SelectedValue = dob.Year.ToString();
        //                }
        //                else if (i == 4)
        //                {
        //                    ddlBT10.SelectedValue = dob.ToString("dd");
        //                    ddlMonthBT10.SelectedValue = Convert.ToString(dob.Month);
        //                    ddlYearBT10.SelectedValue = dob.Year.ToString();
        //                }
        //                ViewState["BTOriginalDetailsAssoc"] = dtPreference;
        //            }
        //        }
        //    }


        //}

        #region Load Dynamic Preferences

        public void LoadCustomerPreferences(string lCustID, bool mainassocFlag)
        {
            //Dynamic Dietary Preferences for NGC 36
            customerID = Convert.ToInt64(lCustID);
            preferenceserviceClient = new PreferenceServiceClient();
            CustomerPreference objPreference = new CustomerPreference();
            objPreference = preferenceserviceClient.ViewCustomerPreference(customerID, PreferenceType.NULL, true);
            if (objPreference != null && objPreference.Preference != null && objPreference.Preference.Count > 0)
            {
                iConfigValue = Convert.ToInt16(Convert.ToInt16(hdnConfigDOB.Value) / 365);

                // To load the Opted Preference
                DataTable dtPreference = new DataTable("BTClub");
                dtPreference.Columns.Add("PreferenceID", typeof(Int16), null);
                dtPreference.Columns.Add("PreferenceType", typeof(Int16), null);
                dtPreference.Columns.Add("PreferenceDescLocal", typeof(string), null);
                dtPreference.Columns.Add("PreferenceDescEnglish", typeof(string), null);
                dtPreference.Columns.Add("OptStatus", typeof(string), null);
                List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                objPreferenceFilter = objPreference.Preference;
                foreach (var pref in objPreferenceFilter)
                {
                    if ((pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_ALLERGIC || pref.CustomerPreferenceType == BusinessConstants.PREFERENCETYPE_DIETRY))
                    {
                        dtPreference.Rows.Add(pref.PreferenceID, pref.CustomerPreferenceType, pref.PreferenceDescriptionLocal, pref.PreferenceDescriptionEng, pref.POptStatus);
                    }

                }
                dtPreference.AcceptChanges();
                LoadPrefernces(dtPreference, mainassocFlag);

            }
        }
        #endregion

        public void LoadStatus(string mailStatus, string emailMStatus, string emailAStatus, string mobileMStatus, string mobileAStatus)
        {

            if (!string.IsNullOrEmpty(mailStatus))
            {
                int MailStatus = Convert.ToInt32(mailStatus);
                //CR13 Update primary mailstatus value after customer data has been saved
                ddlMailStatus.SelectedValue = mailStatus;
                if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_DELIVERABLE)
                {
                    lblMailStatus.Text = GetLocalResourceObject("CusEmailStatusDelivar").ToString();//BusinessConstants.CUST_EMAIL_STATUS_DELIVERABLE;
                }
                else if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_MISSING)
                {
                    //lblMailStatus.Text = BusinessConstants.CUST_EMAIL_STATUS_MISSING;
                    lblMailStatus.Text = GetLocalResourceObject("CusEmailStatusMissing").ToString();
                }
                else if (MailStatus == BusinessConstants.CUSTOMERMAILADDSTATUS_INERROR)
                {
                    lblMailStatus.Text = GetLocalResourceObject("CusEmailStatusInError").ToString(); //BusinessConstants.CUST_EMAIL_STATUS_INERROR;
                }
                else
                    lblMailStatus.Text = string.Empty;
            }

            //NGC Change 3.6- Mobile and Email Status
            if (!string.IsNullOrEmpty(mobileMStatus))
            {
                int MobileStatus = Convert.ToInt32(mobileMStatus);
                //CR13 Update primary mobile status value after customer data has been saved
                ddlMobileStatus.SelectedValue = mobileMStatus;

                if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                {
                    lblMobileStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                }
                else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                {
                    lblMobileStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                }
                else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                {
                    lblMobileStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                }
            }
            if (!string.IsNullOrEmpty(mobileAStatus))
            {
                int MobileStatus = Convert.ToInt32(mobileAStatus);
                //CR13 Update associate mobile status value after customer data has been saved
                ddlAssoMobileStatus.SelectedValue = mobileAStatus;

                if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                {
                    lblAssoMobileStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                }
                else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                {
                    lblAssoMobileStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                }
                else if (MobileStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                {
                    lblAssoMobileStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                }
            }
            if (!string.IsNullOrEmpty(emailMStatus))
            {
                int EmailStatus = Convert.ToInt32(emailMStatus);

                //CR13 Update primary email status value after customer data has been saved
                ddlEmailStatus.SelectedValue = emailMStatus;

                if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                {
                    lblEmailStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                }
                else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                {
                    lblEmailStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                }
                else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                {
                    lblEmailStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                }
            }
            if (!string.IsNullOrEmpty(emailAStatus))
            {
                int EmailStatus = Convert.ToInt32(emailAStatus);
                //CR13 Update associate email status value after customer data has been saved
                ddlAssoEmailStatus.SelectedValue = emailAStatus;

                if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_DELIVERABLE)
                {
                    lblAssoEmailStatus.Text = GetLocalResourceObject("CusMstatusDelivar").ToString();//BusinessConstants.CUST_MOBILE_STATUS_DELIVERABLE;
                }
                else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_MISSING)
                {
                    lblAssoEmailStatus.Text = GetLocalResourceObject("CusMtatusMissing").ToString();//BusinessConstants.CUST_MOBILE_STATUS_MISSING;
                }
                else if (EmailStatus == BusinessConstants.CUSTOMERMAILSTATUS_INERROR)
                {
                    lblAssoEmailStatus.Text = GetLocalResourceObject("CusMstatusInerror").ToString();//BusinessConstants.CUST_MOBILE_STATUS_INERROR;
                }
            }
        }

        protected void ResetHiddenValues()
        {
            List<string> strEmailDietaryPref = new List<string>();
            List<string> strEmailAllergyPref = new List<string>();
            for (int i = 0; i < cblDietaryNeeds.Items.Count; i++)
            {
                if (cblDietaryNeeds.Items[i].Selected && Array.Exists(hdnDietaryPrefList.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value))
                {
                    strEmailDietaryPref.Add(cblDietaryNeeds.Items[i].Value);
                }
                if (cblDietaryNeeds.Items[i].Selected && Array.Exists(hdnAllergyPrefList.Value.Trim().Split(',').ToArray(), c => c == cblDietaryNeeds.Items[i].Value))
                {
                    strEmailAllergyPref.Add(cblDietaryNeeds.Items[i].Value);
                }
            }
            hdnDietaryPref.Value = string.Join(",", strEmailDietaryPref.ToArray());
            hdnAllergyPref.Value = string.Join(",", strEmailAllergyPref.ToArray());
        }

        //--API calls
        public string getAccessToken()
        {
            string strAccessToken = string.Empty;
            try
            {

                if (Cache["AccessToken"] != null)
                {
                    strAccessToken = Cache["AccessToken"].ToString();
                }
                else
                {
                    RestClient obj = new RestClient();
                    GetAccessTokenRequest req = new GetAccessTokenRequest();
                    GetAccessTokenResponse res = new GetAccessTokenResponse();
                    double expiryTime;
                    double.TryParse(HttpContext.GetGlobalResourceObject("ApiMethodNames", "tokenExpiryTime").ToString(), out expiryTime);

                    req.client_id = HttpContext.GetGlobalResourceObject("ApiMethodNames", "client_id").ToString();
                    req.grant_type = HttpContext.GetGlobalResourceObject("ApiMethodNames", "grant_type").ToString();
                    req.password = HttpContext.GetGlobalResourceObject("ApiMethodNames", "password").ToString();
                    req.username = HttpContext.GetGlobalResourceObject("ApiMethodNames", "username").ToString();
                    req.scope = HttpContext.GetGlobalResourceObject("ApiMethodNames", "scope").ToString();

                    res = obj.GetAccessToken(HttpContext.GetGlobalResourceObject("ApiMethodNames", "getAccessToken").ToString(), "", req);
                    Cache.Insert("AccessToken", res.access_token, null, DateTime.Now.AddMinutes(expiryTime), Cache.NoSlidingExpiration);
                    strAccessToken = res.access_token;
                }
                return strAccessToken;
            }
            catch
            {
                throw;
            }
        }
    }
}
