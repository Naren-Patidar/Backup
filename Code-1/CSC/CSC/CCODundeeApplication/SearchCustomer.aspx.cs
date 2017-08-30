using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI.HtmlControls;
using CCODundeeApplication.CustomerService;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using CCODundeeApplication.PreferenceServices;

namespace CCODundeeApplication
{
    public partial class SearchCustomer : System.Web.UI.Page
    {

        #region Local varibales
        //Used in .aspx page for for hiding/unhiding the controls
        protected string spanCardNumber = "display:none";
        protected string spanSurname = "display:none";
        protected string spanFirstName = "display:none";
        protected string spanPostcode = "display:none";
        protected string spanDOB = "display:none";

        protected string errMsgCardNumber = string.Empty;
        protected string errMsgFirstName = string.Empty;
        protected string errMsgSurname = string.Empty;
        protected string errMsgPostCode = string.Empty;
        protected string errMsgDOB = string.Empty;

        //NGC Changes
        protected string spanEmail = "display:none";
        protected string spanPhoneNo = "display:none";
        protected string errMsgEmail = string.Empty;
        protected string errMsgPhoneNo = string.Empty;

        int MaxClubcardlen = Convert.ToInt32(ConfigurationManager.AppSettings["ClubCardMaxLength"].ToString());
        int MinClubcardlen = Convert.ToInt32(ConfigurationManager.AppSettings["ClubCardMinLength"].ToString());
        CustomerServiceClient customerObj = null;
        //commented by lakshmi for localization.
        //public const string CUSTOMER_ACTIVE = "Active";
        //public const string CUSTOMER_BANNED = "Banned";
        //public const string CUSTOMER_LEFTSCHEME = "Left Scheme";
        //public const string CUSTOMER_DATAREMOVED = "Data Removed";
        //public const string CUSTOMER_DUPLICATE = "Duplicate";
        //public const string CUSTOMER_CARDLESS = "Cardless";
        //public const string CUSTOMER_MANUAL = "Manual";
        //public const string CUSTOMER_PENDING = "Pending Activation";

        string title = string.Empty;
        string fName = string.Empty;
        string mName = string.Empty;
        string lName = string.Empty;
        string cardNumber = string.Empty;
        string houseHoldID = string.Empty;
        string currentPoints = string.Empty;
        string joinDate = string.Empty;
         string JoinRouteCode = string.Empty;
         string PromotionalCode = string.Empty;
         //Added as a part of Group CR phase CR12
         string amendBy = string.Empty;
         string amendDateTime = string.Empty;
         //******** Group CR phase1 CR12 ********
        long customerID = 0;
        string culture = ConfigurationManager.AppSettings["CultureDefault"];
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
            string phoneNoMinValue = string.Empty;
                    string phoneNoMaxValue = string.Empty;
                    string mobileNoMinValue = string.Empty;
                    string mobileNoMaxValue = string.Empty;
                    string postCodeMaxVal = string.Empty;
                    string postCodeMinValue = string.Empty;
                    string name3MaxValue = string.Empty;
                    string name3MinValue = string.Empty;
                XmlDocument resulDoc = null;
                string conditionXML = string.Empty;
                string resultXml = string.Empty;
                string errorXml = string.Empty;
                DataSet dsConfigDetails;
                int rowCount = 0;
        PreferenceServiceClient preferenceserviceClient = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            txtCardNumber.Focus(); // to put the Focus on Clubcard Number Text Box
            hdnRegion.Value = culture;
            if (!IsPostBack)
            {
                Helper.GetMonthDdl(ddlMonth); //Load Month dropdown
                Helper.GetYearDdl(ddlYear); //Load Year Dropdown

                //Hide the links
                Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                lblCustomerDtl.Visible = false;
                Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                lblCustomePref.Visible = false;
                Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                lblCustomerPts.Visible = false;
                Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                lblCustomerCards.Visible = false;
                Label lblMergerCards = (Label)Master.FindControl("lblMergeCards");
                lblMergerCards.Visible = false;
                Label lblAddPoints = (Label)Master.FindControl("lblAddPoints");
                lblAddPoints.Visible = false;
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                lblresetpass.Visible = false;
                Label lblviewpoints = (Label)Master.FindControl("lblviewpoints");
                lblviewpoints.Visible = false;
                Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                lblDelinking.Visible = false;
                Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                lblCustomerCoupons.Visible = false;
                Label lblDataConfiguration =(Label)Master.FindControl("lblDataConfiguration");
                //Added as a part of Group CR phase CR12
                Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                lblUserNotes.Visible = false;
                //******** Group CR phase1 CR12 *******
                lblDataConfiguration.Visible = false;
                //Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                //lblCustomerCoupons.Visible = false;
                #region RoleCapabilityImplementation
                xmlCapability = new XmlDocument();
                dsCapability = new DataSet();
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    if (dsCapability.Tables.Count > 0)
                    {
                        HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                        HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                        HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                        HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                        HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                        HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                        HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                        HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                        HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                        HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                        HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                        HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                        Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                        HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                        if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                        {
                            customerCoupon.Disabled = false;
                        }
                        else
                        {
                            customerCoupon.Disabled = true;
                            customerCoupon.HRef = "";
                        }
                        if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                        {
                            PointsEarnedReports.Disabled = false;
                        }
                        else
                        {
                            PointsEarnedReports.Disabled = true;
                            PointsEarnedReports.HRef = "";
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



                    }
                }
                #endregion
                customerObj = new CustomerServiceClient();
                conditionXML = "5,10,25,2";
                if (customerObj.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsConfigDetails = new DataSet();
                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                    ViewState["Config"] = dsConfigDetails;
                    if (dsConfigDetails.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                        {
                            if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                            {
                                postCodeMinValue = dr["ConfigurationValue1"].ToString();
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

                             if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                            {
                                postCodeMinValue = dr["ConfigurationValue1"].ToString();
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

                             if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber")
                             {
                                 phoneNoMinValue = dr["ConfigurationValue1"].ToString();
                                 phoneNoMaxValue = dr["ConfigurationValue2"].ToString();

                                 //If Max value is not configured in the table, then assign DB field length as Max length
                                 if (!string.IsNullOrEmpty(phoneNoMaxValue))
                                 {
                                     
                                     txtMobileNumber.Attributes.Add("MaxLength", phoneNoMaxValue);

                                 }
                                 else
                                 {
                                     
                                     txtMobileNumber.Attributes.Add("MaxLength", "40");
                                     

                                 }

                             }
                             if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "Name3")
                             {
                                 name3MinValue = dr["ConfigurationValue1"].ToString();
                                 name3MaxValue = dr["ConfigurationValue2"].ToString();

                                 //If Max value is not configured in the table, then assign DB field length as Max length
                                 if (!string.IsNullOrEmpty(name3MaxValue))
                                 {

                                     txtSurname.Attributes.Add("MaxLength", name3MaxValue);

                                 }
                                 else
                                 {

                                     txtSurname.Attributes.Add("MaxLength", "30");

                                 }

                             }
                             //if (dr["ConfigurationName"].ToString().Trim() == "Name3" && dr["ConfigurationType"].ToString().Trim() == "2")
                             //{
                             //    imgmandLN.Visible = true;
                             //    hdnName3.Value = "false";
                             //}
                            if (dr["ConfigurationType"].ToString().Trim() == "5" && dr["ConfigurationName"].ToString().Trim() == "DaytimePhoneNumber")
                            {
                                phoneNoMinValue = dr["ConfigurationValue1"].ToString();
                                phoneNoMaxValue = dr["ConfigurationValue2"].ToString();

                                //If Max value is not configured in the table, then assign DB field length as Max length
                                if (!string.IsNullOrEmpty(phoneNoMaxValue))
                                {
                                    txtPhoneNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                    txtEveningNumber.Attributes.Add("MaxLength", phoneNoMaxValue);
                                    //txtMobileNumber.Attributes.Add("MaxLength", phoneNoMaxValue);

                                }
                                else
                                {
                                    txtPhoneNumber.Attributes.Add("MaxLength", "40");
                                    //txtMobileNumber.Attributes.Add("MaxLength", "40");
                                    txtEveningNumber.Attributes.Add("MaxLength", "40");

                                }


                            }
                            else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode")
                            {
                                
                                hdnPostCodeFormat.Value = dr["ConfigurationValue1"].ToString();
                                hdnPostCodeFormat1.Value = dr["ConfigurationValue2"].ToString();
                            }
                            else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name2")
                            {
                                hdnname2validation.Value = dr["ConfigurationValue1"].ToString();
                            }

                            else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "ClubcardNumber")
                            {
                                hdnClubcardnumberreg.Value = dr["ConfigurationValue1"].ToString();
                            }
                            else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "Name1")
                            {
                                hdnname1reg.Value = dr["ConfigurationValue1"].ToString();
                            }
                            else if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "EmailAddress")
                            {
                                hdnemailreg.Value = dr["ConfigurationValue1"].ToString();
                            }



                            else if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HidePostCode")
                            {
                                HidePostCode.Value = "0";
                                divpostcode.Visible = false;
                            }

                            if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideFirstName")
                            {
                                HiddenFName.Value = "false";
                                divFName.Visible = true;
                            }
                            else
                            {
                                HiddenFName.Value = "true";
                                //divFName.Visible = false;
                            }

                            if (dr["ConfigurationType"].ToString().Trim() == "25" && dr["ConfigurationName"].ToString().Trim() == "HideSurName")
                            {
                                HiddenLName.Value = "false";
                                divLname.Visible = true;
                            }
                            else
                            {
                                HiddenLName.Value = "true";
                                //divFName.Visible = false;
                            }
                        }
                    }
                }
                }


            


            if (ConfigurationManager.AppSettings["Culture"].ToString() == "en-US")
            {
                lblSirname.InnerText = "Last Name:";
                ddlYear.Items.Add("1988");
                ddlYear.SelectedValue = "1988";
            }

            if (hdnPageNo.Value != string.Empty)
            {
                PageNumber_Click(sender, e);
            }
            else
            {
                grdCustomerDetail.PageIndex = 0;
            }

            ContentPlaceHolder custDetailsLeftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
            custDetailsLeftNav.Visible = false;
        }
        #region Initialize the culture


        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture"));
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("Default.aspx", false);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void FindCustomer(object sender, EventArgs e)
        {
            dvPaging.Visible = false;
            DataSet dsCustomerInfo = null;
            XmlDocument resulDoc = null;
            Hashtable searchData = null;
            string conditionXml = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            int rowCount, maxRows;
            maxRows = 0;
            bool getFromViewState;
            string strDay = string.Empty;
            string strMonth = string.Empty;
            string strYear = string.Empty;
            DateTime searchDate;
            hdnRegion.Value = culture;
            //For pagination
            int pageSize = 10;
            if ((ddlDay.SelectedValue != "Day") && (ddlMonth.SelectedValue != "- Select Month -"))
            {
                if (ConfigurationManager.AppSettings["Culture"].ToString() == "en-US")
                {

                    ddlYear.Items.Add("1988");
                    ddlYear.SelectedValue = "1988";
                }
            }
            else
                ddlYear.SelectedIndex = 0;

            try
            {
                if (ValidatePage())
                {
                    searchData = new Hashtable();

                    if (txtCardNumber.Text != string.Empty) searchData["cardAccountNumber"] = Convert.ToInt64(txtCardNumber.Text);
                    if (txtFirstname.Text != string.Empty) searchData["Name1"] = txtFirstname.Text.ToString().Replace("'", "''");
                    if (txtSurname.Text != string.Empty) searchData["Name3"] = txtSurname.Text.ToString().Replace("'", "''");
                    if (txtPostCode.Text != string.Empty) searchData["MailingAddressPostCode"] = txtPostCode.Text.ToString();

                    //NGC Changes    
                    if (txtEmail.Text != string.Empty) searchData["Email"] = txtEmail.Text.ToString();
                    if (txtPhoneNumber.Text != string.Empty) searchData["PhoneNumber"] = txtPhoneNumber.Text.ToString();
                    //NGC Changes 
                    if (txtBSName.Text != string.Empty) searchData["BusinessName"] = txtBSName.Text.ToString();
                    if (txtBsRegNumber.Text != string.Empty) searchData["BusinessRegistrationNumber"] = txtBsRegNumber.Text.ToString();
                    if (txtEveningNumber.Text != string.Empty) searchData["evening_phone_number"] = txtEveningNumber.Text.ToString();
                    if (txtMobileNumber.Text != string.Empty) searchData["mobile_phone_number"] = txtMobileNumber.Text.ToString();
                    if (txtSSN.Text != string.Empty) searchData["SSN"] = txtSSN.Text.ToString();
                    if (ddlDay.SelectedValue != "Day") strDay = ddlDay.SelectedValue;
                    if (ddlMonth.SelectedValue != "- Select Month -") strMonth = ddlMonth.SelectedValue;
                    if (strDay != "" && strMonth != "" && ddlYear.SelectedValue != "Year")
                    {
                        strYear = ddlYear.SelectedValue;
                        searchDate = Convert.ToDateTime(strDay + "/" + strMonth + "/" + strYear);
                        searchData["family_member_1_dob"] = searchDate;
                    }
                    searchData["CustomerID"] = 0;
                    //searchData["Culture"] = ConfigurationManager.AppSettings["Culture"].ToString();
                    //Preparing parameters for service call
                    conditionXml = Helper.HashTableToXML(searchData, "customer");

                    maxRows = 200;

                    //If stored dataset in viewstate has the data.
                    if (ViewState["dsCustomerInfo"] != null)
                    {
                        //If search creteria is same..
                        if (string.Compare(ViewState["conditionXml"].ToString(), conditionXml) == 0)
                        {
                            getFromViewState = true;
                        }
                        else
                        {
                            getFromViewState = false;
                        }
                    }
                    else
                    {
                        getFromViewState = false;
                    }

                    if (getFromViewState)
                    {
                        dsCustomerInfo = ViewState["dsCustomerInfo"] as DataSet;
                    }
                    else
                    {
                        //Store search creteria in viewstate to compare with next submit.
                        ViewState["conditionXml"] = conditionXml;
                        customerObj = new CustomerServiceClient();
                        #region Trace Start
                        NGCTrace.NGCTrace.TraceInfo("Start: CSC SearchCustomer.FindCustomer()");
                        NGCTrace.NGCTrace.TraceDebug("Start: CSC SearchCustomer.FindCustomer() input Xml-" + conditionXml);
                        #endregion

                        if (customerObj.SearchCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRows, ConfigurationManager.AppSettings["Culture"].ToString()))
                        {
                            resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            dsCustomerInfo = new DataSet();
                            dsCustomerInfo.ReadXml(new XmlNodeReader(resulDoc));

                            //Save the dataset to view state for postback cycles
                            ViewState["dsCustomerInfo"] = dsCustomerInfo;
                        }
                        else
                        {
                            throw new Exception("Search Customer failed for condition: " + conditionXml + "; errorXml: " + errorXml);
                        }
                    }

                    dvSearchResults.Visible = true;

                    if (dsCustomerInfo.Tables.Count > 0)
                    {
                        ImageButton btnClicked = sender as ImageButton;
                        //To check whether Find customer buttonis clicked, if yes reset pageindex to "0".
                        if (btnClicked != null && btnClicked.ID == "btnFindCustomer")
                        {
                            grdCustomerDetail.PageIndex = 0;
                            hdnPageNo.Value = "0";
                        }

                        dvNoDataFound.Visible = false;
                        grdCustomerDetail.PageSize = pageSize;

                        //If the column value in BD is NULL, then resultXML will not return the column. Hence adding the columns.
                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("TitleEnglish") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("TitleEnglish");
                        }
                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("ClubcardID") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("ClubcardID");
                        }
                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("Name1") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("Name1");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("Name2") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("Name2");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("Name3") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("Name3");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("MailingAddressLine1") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("MailingAddressLine1");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("MailingAddressLine2") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("MailingAddressLine2");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("MailingAddressLine3") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("MailingAddressLine3");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("DateOfBirth") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("DateOfBirth");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("JoinedDate") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("JoinedDate");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("CurrentPointsBalanceQty") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("CurrentPointsBalanceQty");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("PreviousPointsBalanceQty") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("PreviousPointsBalanceQty");
                        }

                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("HouseHoldID") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("HouseHoldID");
                        }
                        //Added as a part of Group CR phase CR12
                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("AmendBy") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("AmendBy");
                        }
                        if (dsCustomerInfo.Tables["Customer"].Columns.Contains("AmendDateTime") == false)
                        {
                            dsCustomerInfo.Tables["Customer"].Columns.Add("AmendDateTime");
                        }
                        //******* Group CR phase1 CR12

                        if (dsCustomerInfo.Tables["Customer"].Rows.Count == 1)
                        {
                            string customerName = string.Empty;
                            DateTime joinedDate;

                            title = dsCustomerInfo.Tables["Customer"].Rows[0]["TitleEnglish"].ToString().Trim();
                            fName = dsCustomerInfo.Tables["Customer"].Rows[0]["Name1"].ToString().Trim();
                            mName = dsCustomerInfo.Tables["Customer"].Rows[0]["Name2"].ToString().Trim();
                            lName = dsCustomerInfo.Tables["Customer"].Rows[0]["Name3"].ToString().Trim();

                           //Added as a part of Group CR phase CR12
                            amendBy = dsCustomerInfo.Tables["Customer"].Rows[0]["AmendBy"].ToString().Trim();
                            amendDateTime = dsCustomerInfo.Tables["Customer"].Rows[0]["AmendDateTime"].ToString().Trim();
                           //******* Group CR phase1 CR12

                            cardNumber = dsCustomerInfo.Tables["Customer"].Rows[0]["ClubcardID"].ToString().Trim();
                            houseHoldID = dsCustomerInfo.Tables["Customer"].Rows[0]["HouseHoldID"].ToString().Trim();

                            if (dsCustomerInfo.Tables["Customer"].Rows[0]["CurrentPointsBalanceQty"].ToString().Trim() == string.Empty)
                            {
                                currentPoints = "0";
                            }
                            else
                            {
                                currentPoints = dsCustomerInfo.Tables["Customer"].Rows[0]["CurrentPointsBalanceQty"].ToString().Trim();
                            }

                            if (DateTime.TryParse(dsCustomerInfo.Tables["Customer"].Rows[0]["JoinedDate"].ToString().Trim(), out joinedDate))
                            {
                                joinDate = joinedDate.ToString("dd/MM/yy");
                            }
                            if (!string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["JoinRouteDesc"].ToString().Trim()))
                            {
                                JoinRouteCode = dsCustomerInfo.Tables["Customer"].Rows[0]["JoinRouteDesc"].ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["PromotionCode"].ToString().Trim()))
                            {
                                PromotionalCode = dsCustomerInfo.Tables["Customer"].Rows[0]["PromotionCode"].ToString().Trim();
                            }
                            
                            if (!string.IsNullOrEmpty(dsCustomerInfo.Tables["Customer"].Rows[0]["CustomerID"].ToString().Trim()))
                            {
                                customerID = Convert.ToInt64(dsCustomerInfo.Tables["Customer"].Rows[0]["CustomerID"].ToString().Trim());
                            }

                            //Call private method to show the customer info on Left Navigation Bar.
                            ShowCustomerInfoOnLeftNav(title, fName, mName, lName, cardNumber, houseHoldID, currentPoints, joinDate, JoinRouteCode,PromotionalCode,customerID,amendBy,amendDateTime);
                        }
                        else
                        {

                            //Display Page numbers
                            int pageCount = 0;
                            int rowCnt = 0;
                            int exactmodule;
                            rowCnt = dsCustomerInfo.Tables["Customer"].Rows.Count;
                            pageCount = rowCnt / pageSize;
                            pnlPageNos.Controls.Clear();
                            exactmodule = rowCnt % pageSize;
                            if (pageCount != 20 && exactmodule !=0)
                            {
                                pageCount = pageCount + 1;
                            }

                            //If rows retreived are more than 10 then display pagination section.
                            if (pageCount > 1)
                            {
                                dvPaging.Visible = true;

                                for (int i = 0; i < pageCount; i++)
                                {
                                    LinkButton lnk = new LinkButton();

                                    lnk.ID = "lnk" + i;
                                    lnk.Text = (i + 1).ToString();
                                    lnk.CommandArgument = i.ToString();

                                    if ((grdCustomerDetail.PageIndex) != i)
                                    {
                                        lnk.ControlStyle.ForeColor = System.Drawing.Color.Blue;
                                        lnk.Click += new EventHandler(this.PageNumber_Click);
                                        lnk.Attributes.Add("onclick", "PageNoClicked(" + i + ")");
                                    }
                                    else
                                    {
                                        lnk.ControlStyle.ForeColor = System.Drawing.Color.Black;
                                        lnk.Attributes.Remove("href");
                                        lnk.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                                        lnk.Enabled = false;
                                    }

                                    pnlPageNos.Controls.Add(lnk);
                                }

                                //To show and hide the image buttons
                                if (grdCustomerDetail.PageIndex == (pageCount - 1))
                                {
                                    ImgNext1.Visible = false;
                                    ImgPrevDisabled1.Visible = false;
                                    ImgNextDisabled1.Visible = true;
                                    ImgPrev1.Visible = true;

                                    ImgNextDisabled1.Attributes.Remove("href");
                                    ImgNextDisabled1.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                                    ImgNextDisabled1.Disabled = true;
                                }
                                else if (grdCustomerDetail.PageIndex == 0)
                                {
                                    ImgPrevDisabled1.Visible = true;
                                    ImgPrev1.Visible = false;
                                    ImgNext1.Visible = true;
                                    ImgNextDisabled1.Visible = false;

                                    ImgPrevDisabled1.Attributes.Remove("href");
                                    ImgPrevDisabled1.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                                    ImgPrevDisabled1.Disabled = true;
                                }
                                else
                                {
                                    ImgPrev1.Visible = true;
                                    ImgNext1.Visible = true;
                                    ImgPrevDisabled1.Visible = false;
                                    ImgNextDisabled1.Visible = false;
                                }
                            }
                            else
                            {
                                dvPaging.Visible = false;
                            }

                            grdCustomerDetail.DataSource = dsCustomerInfo.Tables["Customer"];
                            grdCustomerDetail.DataBind();
                        }
                    }
                    else
                    {
                        //Clear the grid if there are no data found
                        grdCustomerDetail.DataSource = null;
                        grdCustomerDetail.DataBind();

                        dvNoDataFound.Visible = true;
                        dvPaging.Visible = false;
                    }
                }
                else
                {
                    dvSearchResults.Visible = false;
                    dvNoDataFound.Visible = false;
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC SearchCustomer.FindCustomer()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC SearchCustomer.FindCustomer() input Xml-" + conditionXml);
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.FindCustomer() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID") + "; Search Condition: " + conditionXml);
                NGCTrace.NGCTrace.TraceError("Error: CSC SearchCustomer.FindCustomer() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC SearchCustomer.FindCustomer()");
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
            }
        }



        //On click of page numbers displayed.
        void PageNumber_Click(object sender, EventArgs e)
        {
            grdCustomerDetail.PageIndex = Convert.ToInt32(hdnPageNo.Value);
            FindCustomer(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdCustomerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //e.Row.Cells[1].Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
                    //e.Row.Cells[2].Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

                    Literal ltrName = (Literal)e.Row.FindControl("ltrName");
                    Literal ltrAddress = (Literal)e.Row.FindControl("ltrAddress");
                    Literal ltrDOB = (Literal)e.Row.FindControl("ltrDOB");
                    Literal ltrJoinDate = (Literal)e.Row.FindControl("ltrJoinDate");
                    Literal ltrCustStatus = (Literal)e.Row.FindControl("ltrCustStatus");
                    Literal ltrCurrentPts = (Literal)e.Row.FindControl("ltrCurrentPts");
                    Literal ltrPrevpts = (Literal)e.Row.FindControl("ltrPrevpts");

                    if (ltrName.Text != "")
                    {
                        string comma = string.Empty;
                        if (((DataRowView)e.Row.DataItem)["Name1"].ToString().Trim() != "")
                        {
                            comma = ", ";
                        }

                        ltrName.Text = Helper.ToTitleCase(((DataRowView)e.Row.DataItem)["TitleEnglish"].ToString().Trim()) + " "
                            + Helper.ToTitleCase(((DataRowView)e.Row.DataItem)["Name3"].ToString().Trim()) + comma
                            + Helper.ToTitleCase(((DataRowView)e.Row.DataItem)["Name1"].ToString().Trim());
                    }
                    else
                    {
                       
                        ltrName.Text = Helper.ToTitleCase(((DataRowView)e.Row.DataItem)["TitleEnglish"].ToString().Trim()) + " "
                            + Helper.ToTitleCase(((DataRowView)e.Row.DataItem)["Name3"].ToString().Trim());
                    }

                    if (!string.IsNullOrEmpty(ltrDOB.Text))
                    {
                        DateTime DOBDate = Convert.ToDateTime(ltrDOB.Text.ToString());
                        ltrDOB.Text = DOBDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));
                        if (ltrDOB.Text == "01/01/01")
                        {
                            ltrDOB.Text = "";
                        }
                    }

                    if (ltrAddress.Text != "")
                    {
                        ltrAddress.Text = ((DataRowView)e.Row.DataItem)["MailingAddressLine1"].ToString() + " "
                                            + ((DataRowView)e.Row.DataItem)["MailingAddressLine2"].ToString() + " "
                                            + ((DataRowView)e.Row.DataItem)["MailingAddressLine3"].ToString();


                    }

                    if (!string.IsNullOrEmpty(ltrJoinDate.Text))
                    {
                        DateTime JoinDate = Convert.ToDateTime(ltrJoinDate.Text.ToString());
                        ltrJoinDate.Text = JoinDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture));
                    }

                    //Customer status
                    if (ltrCustStatus.Text != "")
                    {
                        if (culture != "en-US")
                        {
                            if (ltrCustStatus.Text == "1")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusActive").ToString();//CUSTOMER_ACTIVE;
                            }
                            else if (ltrCustStatus.Text == "2")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusBanned").ToString();//CUSTOMER_BANNED;
                            }
                            else if (ltrCustStatus.Text == "3")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusLeftSchem").ToString();//CUSTOMER_LEFTSCHEME;
                            }
                            else if (ltrCustStatus.Text == "4")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusDataRemoved").ToString();//CUSTOMER_DATAREMOVED;
                            }
                            else if (ltrCustStatus.Text == "5")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusDuplicate").ToString();//CUSTOMER_DUPLICATE;
                            }
                        }
                        else
                        {
                            if (ltrCustStatus.Text == "0")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusActive").ToString();//CUSTOMER_ACTIVE;
                            }
                            else if (ltrCustStatus.Text == "4")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusBanned").ToString();//CUSTOMER_BANNED;
                            }
                            else if (ltrCustStatus.Text == "2")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusLeftSchem").ToString();//CUSTOMER_LEFTSCHEME;
                            }
                            else if (ltrCustStatus.Text == "13")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusCardless").ToString();//CUSTOMER_CARDLESS;
                            }
                            else if (ltrCustStatus.Text == "13")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusCardless").ToString();//CUSTOMER_CARDLESS;
                            }
                            else if (ltrCustStatus.Text == "10")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusManual").ToString();//CUSTOMER_MANUAL;
                            }
                            else if (ltrCustStatus.Text == "12")
                            {
                                ltrCustStatus.Text = GetLocalResourceObject("CusPending").ToString();//CUSTOMER_PENDING;
                            }
                        }
                    }

                    if (ltrCurrentPts.Text == string.Empty)
                    {
                        ltrCurrentPts.Text = "0";
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.GrdCustomerDetail_RowDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC SearchCustomer.GrdCustomerDetail_RowDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC SearchCustomer.GrdCustomerDetail_RowDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// On click of GO button this method is called.
        /// In this method all the left navigation links are enabled, also selected customer details will be displayed on LNB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdCustomerDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    if (ViewState["dsCustomerInfo"] != null)
                    {
                        DataSet ds = (DataSet)ViewState["dsCustomerInfo"];
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["CustomerID"].ToString() == e.CommandArgument.ToString())
                            {
                                title = row["TitleEnglish"].ToString().Trim();
                                fName = row["Name1"].ToString().Trim();
                                mName = row["Name2"].ToString().Trim();
                                lName = row["Name3"].ToString().Trim();
                                cardNumber = row["ClubcardID"].ToString().Trim();
                                houseHoldID = row["HouseHoldID"].ToString().Trim();
                                currentPoints = row["CurrentPointsBalanceQty"].ToString().Trim();
                                joinDate = row["JoinedDate"].ToString().Trim();
                                JoinRouteCode = row["JoinRouteDesc"].ToString().Trim();
                                PromotionalCode = row["PromotionCode"].ToString().Trim();
                                customerID = Convert.ToInt64(row["CustomerID"].ToString().Trim());
                                //Added as a part of Group CR phase CR12
                                amendBy = row["AmendBy"].ToString().Trim();
                                amendDateTime = row["AmendDateTime"].ToString().Trim();
                                //******** Group CR phase1 CR12 ********

                                break;
                            }
                        }
                    }

                    //Call private method to show the customer info on Left Navigation Bar.
                    ShowCustomerInfoOnLeftNav(title, fName, mName, lName, cardNumber, houseHoldID, currentPoints, joinDate,JoinRouteCode,PromotionalCode,customerID,amendBy,amendDateTime);
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC SearchCustomer.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC SearchCustomer.GrdCustomerDetail_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// To validate customer details
        /// </summary>
        /// <returns>boolean</returns>
        protected bool ValidatePage()
        {
            try
            {
                //string regNumeric = @"^[0-9]*$";
                //string regForeName = @"^\p{L}[\p{L}\p{Pd}\x27]*\p{L}$";//@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";
                string regNumeric=hdnClubcardnumberreg.Value;
                string regForeName = hdnname1reg.Value;
                string regSurName = hdnname2validation.Value; // @"^\p{L}[\p{L}\p{Pd}\x27]*\p{L}$";// @"^[a-zA-Z]+(([\'\.\- ][a-zA-Z ])?[a-zA-Z]*)*$";

                //string regPostCode = @"^/\b[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}\b/g $";
                string regPostCode = hdnPostCodeFormat.Value; ; //@"^ ?(([BEGLMNSWbeglmnsw][0-9][0-9]?)|(([A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9]?)|(([ENWenw][0-9][A-HJKSTUWa-hjkstuw])|([ENWenw][A-HK-Ya-hk-y][0-9][ABEHMNPRVWXYabehmnprvwxy])))) ?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
                string regPostCode1 = hdnPostCodeFormat1.Value;// @"^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y0-9][A-HJKSTUWa-hjkstuw0-9]?[ABEHMNPRVWXYabehmnprvwxy0-9]? {1,2}[0-9][ABD-HJLN-UW-Zabd-hjln-uw-z]{2}$";
                //NGC Change
                //string regMail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                string regMail = hdnemailreg.Value;

                bool bErrorFlag = true;
                bool bErrorAgeFlag = true;

                DateTime dob = DateTime.Now;

                //Clear the class
                txtCardNumber.CssClass = "";
                txtFirstname.CssClass = "";
                txtSurname.CssClass = "";
                spanDOB = "dtFld";
                txtPostCode.CssClass = "";
                
                string cardNumber = txtCardNumber.Text.ToString().Trim();
                string surName = txtSurname.Text.ToString().Trim();
                string firstName = txtFirstname.Text.ToString().Trim();
                string postCode = txtPostCode.Text.ToString().Trim();
                string day = ddlDay.SelectedValue.ToString().Trim();
                string month = ddlMonth.SelectedValue.ToString().Trim();
                string year = ddlYear.SelectedIndex.ToString().Trim();
                string email = txtEmail.Text.ToString().Trim();
                string phone = txtPhoneNumber.Text.ToString().Trim();

                //Server side validations
                

               
                if (string.IsNullOrEmpty(cardNumber) && string.IsNullOrEmpty(surName) && string.IsNullOrEmpty(firstName) &&
                    string.IsNullOrEmpty(postCode) && string.IsNullOrEmpty(day) && month == "- Select Month -" && year == "Year" &&
                    string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phone))
                {
                    spnErrorMsg.Visible = true;
                    spnErrorMsg.Attributes.Add("Style", "display:block");
                    bErrorFlag = false;
                }
                else
                {
                    spnErrorMsg.Visible = true;
                    spnErrorMsg.Attributes.Add("Style", "display:none");

                    try
                    {
                        //Card number
                        if (!Helper.IsRegexMatch(txtCardNumber.Text.Trim(), regNumeric, true, false))
                        {
                            //errMsgCardNumber = "Please enter a valid Card Number";
                            errMsgCardNumber = GetLocalResourceObject("ValidCardNo.Text").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        //Card number should be more between 16  and 18 digits
                        else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && (txtCardNumber.Text.Trim().Length < MinClubcardlen || txtCardNumber.Text.Trim().Length > MaxClubcardlen))
                        {
                            // errMsgCardNumber = "Please enter a valid Card Number";
                            errMsgCardNumber = GetLocalResourceObject("ValidCardNo.Text").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        
                    }
                    catch (FormatException)
                    {
                        //errMsgCardNumber = "Please enter a valid Card Number";
                        errMsgCardNumber = GetLocalResourceObject("ValidCardNo.Text").ToString();
                        spanCardNumber = "";
                        txtCardNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }
                    if (HiddenLName.Value == "false")
                    {
                        if (Culture == "en-GB")
                        {
                            //Surname should be at least 2 characters.
                            if (!Helper.IsRegexMatch(this.txtSurname.Text.Trim(), regSurName, Convert.ToBoolean(hdnName3.Value), false))
                            {
                                // this.errMsgSurname = "Please note Surname is required, or the entered name is not valid.";
                                errMsgSurname = GetLocalResourceObject("ValidFirstName.Text").ToString();//"Please enter a valid Name";
                                spanSurname = "";
                                txtSurname.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                            else if (string.IsNullOrEmpty(txtSurname.Text.Trim()) && txtSurname.Text.Trim().Length < 2)
                            {
                                errMsgSurname = GetLocalResourceObject("ValidSurnameAtleasttwo.Text").ToString();// "Surname must be atleast 2 letters long";
                                spanSurname = "";
                                txtSurname.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        else
                        {
                            if (!Helper.IsRegexMatch(this.txtSurname.Text.Trim(), regSurName, Convert.ToBoolean(hdnName3.Value), false))
                            {
                                // this.errMsgSurname = "Please note Surname is required, or the entered name is not valid.";
                                errMsgSurname = GetLocalResourceObject("ValidFirstName.Text").ToString();//"Please enter a valid Name";
                                spanSurname = "";
                                txtSurname.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                            else if (!Helper.IsRegexMatch(txtSurname.Text.Trim(), regSurName, true, false))
                            {
                                errMsgSurname = GetLocalResourceObject("ValidFirstName.Text").ToString();//"Please enter a valid Name";
                                spanSurname = "";
                                txtSurname.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                    }



                    //if (!(HiddenFName.Value == "false")) //Commented for fixing FirstName validation issue in Search Page
                    if ((HiddenFName.Value == "false"))
                    {
                        //First Name
                        if (!Helper.IsRegexMatch(txtFirstname.Text.Trim(), regForeName, true, false))
                        {
                            errMsgFirstName = GetLocalResourceObject("ValidFirstName.Text").ToString();//"Please enter a valid Name";
                            spanFirstName = "";
                            txtFirstname.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                   
                   
                        if (culture == "en-US")
                        {
                            if (!Helper.IsRegexMatch(txtPostCode.Text.Trim(), regNumeric, true, false))
                            {
                                errMsgPostCode = GetLocalResourceObject("ValidPostCode.Text").ToString();// "Please enter a valid Postcode";
                                spanPostcode = "";
                                txtPostCode.CssClass = "errorFld";
                                bErrorFlag = false;
                            }
                        }
                        else
                        {
                            if (HidePostCode.Value != "0")
                            {
                                if (!Helper.IsRegexMatch(txtPostCode.Text.Trim(), regPostCode, true, false) && !Helper.IsRegexMatch(txtPostCode.Text.Trim(), regPostCode1, true, false))
                                {
                                    errMsgPostCode = GetLocalResourceObject("ValidPostCode.Text").ToString();//"Please enter a valid Postcode";
                                    spanPostcode = "";
                                    txtPostCode.CssClass = "errorFld";
                                    bErrorFlag = false;
                                }
                            }
                        }

                    //Email
                    //NGC Change
                    if (!Helper.IsRegexMatch(txtEmail.Text.Trim(), regMail, true, false))
                    {
                        errMsgEmail = GetLocalResourceObject("ValidEmailAdd.Text").ToString();// "Please enter a valid Email Address.";
                        spanEmail = "";
                        txtEmail.CssClass = "errorFld";
                        bErrorFlag = false;
                    }


                    //Phone Number
                    //NGC Change
                    if (!Helper.IsRegexMatch(txtPhoneNumber.Text.Trim(), regNumeric, true, false))
                    {
                        errMsgPhoneNo = GetLocalResourceObject("ValidPhoneNo.Text").ToString();//"Please enter a valid Phone Number.";
                        spanPhoneNo = "";
                        txtPhoneNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }


                    //DOB
                    if ((ddlDay.SelectedValue == "" && (ddlMonth.SelectedValue == "" || ddlMonth.SelectedValue == "- Select Month -")
                        && (ddlYear.SelectedValue == "" || ddlYear.SelectedValue == "Year")))
                    {
                        //DOB is empty then don't validate as it not required field.
                       
                    }
                    else
                    {
                        if ((ddlDay.SelectedValue == "" || ddlMonth.SelectedValue == "" || ddlYear.SelectedValue == "") ||
                              (!DateTime.TryParse(ddlDay.SelectedValue + "/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue, out dob)))
                        {
                            errMsgDOB = GetLocalResourceObject("ValidDate.Text").ToString();// "Date Of Birth is invalid";
                            spanDOB = "";
                            spanDOB = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                        else if (GetAge(dob) < 18)
                        {
                            errMsgDOB = GetLocalResourceObject("ValidAge.Text").ToString();// "Please note you must be over 18 to be a member of Clubcard";
                            spanDOB = "";
                            spanDOB = "errorFld dtFld";
                            bErrorFlag = false;
                        }
                    }
                }

                if (bErrorAgeFlag)
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
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC SearchCustomer.GrdCustomerDetail_RowCommand()- Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC SearchCustomer.GrdCustomerDetail_RowCommand()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        /// <summary>
        /// It converts the date in to age
        /// </summary>
        /// <param name="dob">DateTime</param>
        private static short GetAge(DateTime dob)
        {
            int dobYear = dob.Year;
            short age = Convert.ToInt16(DateTime.Now.Year - dobYear);
            return age;
        }

        /// <summary>
        /// This method is used to show the Customer info on left navigation bar.
        /// Also to enable the links on LNB.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fName"></param>
        /// <param name="mName"></param>
        /// <param name="lName"></param>
        /// <param name="cardNumber"></param>
        /// <param name="houseHoldID"></param>
        /// <param name="currentPoints"></param>
        /// <param name="joinDate"></param>
        public void ShowCustomerInfoOnLeftNav(string title, string fName, string mName, string lName, string cardNumber,
                                               string houseHoldID, string currentPoints, string joinDate, string JoinRouteCode,string PromotionalCode,long customerID,string amendBy, string amendDateTime)
        {
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            DataSet dsCustomerPreference = null;
            string customerName = string.Empty;
            DateTime joinedDate;
            DateTime amendedDateTime;

            try
            {
                if ((!string.IsNullOrEmpty(title)) && (title.ToUpper() != "UNKNOWN"))
                {
                    customerName = customerName + Helper.ToTitleCase(title) + ". ";
                }
                customerName = customerName + Helper.ToTitleCase(fName) + " ";
                customerName = customerName + Helper.ToTitleCase(mName) + " ";
                customerName = customerName + Helper.ToTitleCase(lName);

                Helper.SetTripleDESEncryptedCookie("lblName", customerName);
                Helper.SetTripleDESEncryptedCookie("lblCardNo", cardNumber);
                Helper.SetTripleDESEncryptedCookie("lblHouseholdID", houseHoldID);
                Helper.SetTripleDESEncryptedCookie("lblJoinRouteID", JoinRouteCode);
                Helper.SetTripleDESEncryptedCookie("lblPromotionalCode", PromotionalCode);
                //Added as a part of Group CR phase CR12
                Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendedBy", amendBy);
                //Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendDate", amendDateTime);
                //******** Group CR phase1 CR12 ********

                if (currentPoints == string.Empty)
                {
                    Helper.SetTripleDESEncryptedCookie("lblCurrPoints", "0");
                }
                else
                {
                    Helper.SetTripleDESEncryptedCookie("lblCurrPoints", currentPoints);
                }

                if (DateTime.TryParse(joinDate, out joinedDate))
                {

                    Helper.SetTripleDESEncryptedCookie("JoinedDate", joinedDate.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture)));
                }
                if (DateTime.TryParse(amendDateTime, out amendedDateTime))
                {

                    Helper.SetTripleDESEncryptedCookie("lblCustomerLastAmendDate", amendedDateTime.ToString("dd/MM/yy", CultureInfo.CreateSpecificCulture(culture)));
                }

                //Enable the links
                Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                lblCustomerDtl.Visible = true;
                Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                lblCustomePref.Visible = true;
                Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                lblCustomerPts.Visible = true;
                Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                lblCustomerCards.Visible = true;
                Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                lblXmasSaver.Visible = false;
                //Added as a part of Group CR phase CR12
                //Label lblUserNotes = (Label)Master.FindControl("lblUserNotes");
                //lblUserNotes.Visible = true;
                //******** Group CR phase1 CR12 ******** 

                //Cookie implementation
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("CustomerID");
                }

                if (customerID.ToString() != null)
                {
                    Helper.SetTripleDESEncryptedCookie("CustomerID", customerID.ToString());
                }

                preferenceserviceClient = new PreferenceServiceClient();
                customerObj = new CustomerServiceClient();
                CustomerPreference objPreference = new CustomerPreference();
                objPreference = preferenceserviceClient.ViewCustomerPreference(customerID, PreferenceType.NULL, true);

                if (objPreference != null && objPreference.Preference != null && objPreference.Preference.Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = objPreference.Preference;
                    string PrefID = string.Empty;
                    List<string> PreferenceIds = new List<string>();
                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.POptStatus == OptStatus.OPTED_IN)
                        {
                            PrefID = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(PrefID);
                        }
                    }

                    if (PreferenceIds.Contains(BusinessConstants.XMASSAVER.ToString()))    //Xmas Saver
                    {
                        Helper.SetTripleDESEncryptedCookie("lblCustType", "Christmas saver");
                        lblXmasSaver.Visible = true;
                    }
                    else if (PreferenceIds.Contains(BusinessConstants.AIRMILES_STD.ToString()) || PreferenceIds.Contains(BusinessConstants.AIRMILES_PREMIUM.ToString()))    //Airmiles
                    {
                        Helper.SetTripleDESEncryptedCookie("lblCustType", "Airmiles");
                    }
                    else if (PreferenceIds.Contains(BusinessConstants.BAMILES_STD.ToString()) || PreferenceIds.Contains(BusinessConstants.BAMILES_PREMIUM.ToString()))    //Airmiles
                    {
                        Helper.SetTripleDESEncryptedCookie("lblCustType", "BA Miles");
                    }
                    else
                    {
                        Helper.SetTripleDESEncryptedCookie("lblCustType", "Standard");
                    }
                }
                else
                {
                    Helper.SetTripleDESEncryptedCookie("lblCustType", "Standard");
                }
                Response.Redirect("CustomerDetail.aspx", false);
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC SearchCustomer.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC SearchCustomer.GrdCustomerDetail_RowCommand() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC SearchCustomer.GrdCustomerDetail_RowCommand()");
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
            }
        }
    }
}