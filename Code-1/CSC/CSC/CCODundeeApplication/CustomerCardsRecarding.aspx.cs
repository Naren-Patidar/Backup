using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using CCODundeeApplication.CustomerService;
using CCODundeeApplication.ClubcardService;
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using System.Net;
using CCODundeeApplication.App_Code;
using System.Web.Caching;


namespace CCODundeeApplication
{
    public partial class CustomerCardsRecarding : System.Web.UI.Page
    {
        #region Local varibales

        
        Hashtable htCustomerId = null;

        Hashtable htCustomer = null;
        CustomerServiceClient customerClient = null;
        ////Used in .aspx page for for hiding/unhiding the controls
        protected string spanCardNumber = "display:none";
        //protected string spanLastName = "display:none";
        //protected string spanFirstName = "display:none";
        //protected string spanEmailAddress = "display:none";
        string showMoreInfoLink = null;
        int countCard = 0;
        protected string errMsgCardNumber = string.Empty;

        ClubcardServiceClient serviceClient = null;
        string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
        bool boolStandardRadioMain = false;
        bool boolStandardRadioAsso = false;
        bool isMainConfirmError = false;
        bool isValidRequest = true;
        bool isAssoConfirmError = false;
        DataSet dsCapability = null;
        bool disablerdbMain = false;
        bool disablerdbAssociate = false;
        XmlDocument xmlCapability = null;
        static long mainCount;
        static long assCount;
        static long AssCusId;
        static long MainCusID;
        Boolean idbit = false;

        bool boolcontainsStandardCard = false;//To check standard card exists or not.
        int MaxClubcards = Convert.ToInt32(ConfigurationManager.AppSettings["MaxCardsCount"].ToString());
        bool boolReplaceCard = true;
        int funCount = 0;
        int funGetConfigDetails = 0;
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            string strEnableRecarding = ConfigurationManager.AppSettings["enableRecarding"].ToString();
            if (!(strEnableRecarding.ToUpper() == "TRUE"))
            {
                Response.Redirect("CustomerCards.aspx");
            }
        }

        #region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {
                if ((!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID"))))
                {
                    //******* Release 1.5 changes start *********//
                    SetHouseHoldStatus();
                    //******* Release 1.5 changes end *********//

                    if (!IsPostBack)
                    {
                        AssCusId = 0;

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
                                HtmlAnchor christmasSaver = (HtmlAnchor)Master.FindControl("christmasSaver");
                                HtmlAnchor aAdmin = (HtmlAnchor)Master.FindControl("aAdmin");
                                HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                                HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                                HtmlAnchor agroups = (HtmlAnchor)Master.FindControl("agroups");
                                HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                                HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                                PlaceHolder plAdmin = (PlaceHolder)Master.FindControl("plAdmin");
                                HtmlAnchor viewpoints = (HtmlAnchor)Master.FindControl("viewpoints");
                                HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                                HtmlAnchor ResetPass = (HtmlAnchor)Master.FindControl("resetpass");
                                HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                                HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                                HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                                //Label DeLinkAccount = (Label)Master.FindControl("lblDelinking");
                                HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                                HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                                HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                                HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                                HtmlAnchor customerCoupon = (HtmlAnchor)Master.FindControl("customerCoupon");
                                HtmlAnchor DeLinkAccount = (HtmlAnchor)Master.FindControl("DelinkAccounts");
                                Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");

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

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerCoupons") != false)
                                {
                                    customerCoupon.Disabled = false;
                                }
                                else
                                {
                                    customerCoupon.Disabled = true;
                                    customerCoupon.HRef = "";
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
                                if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerCards") != false)
                                {
                                    //orderConfirm.Enabled = true;
                                }
                                else
                                {
                                    disablerdbMain = true;
                                    disablerdbAssociate = true;
                                    orderConfirm.Attributes.Add("disabled", "true");
                                }


                                if (dsCapability.Tables[0].Columns.Contains("AddCardToAccount") != false)
                                {
                                    AddCardToAccount.Visible = true;
                                    divAddCard.Visible = true;
                                    dvMainAssCus.Visible = true;
                                    hdnIsAddCard.Value = "true";
                                }
                                else
                                {
                                    AddCardToAccount.Visible = false;
                                    divAddCard.Visible = false;
                                    dvMainAssCus.Visible = false;
                                    hdnIsAddCard.Value = "false";
                                }
                                if (dsCapability.Tables[0].Columns.Contains("ChangePrimary") != false)
                                {
                                    imgbtnChangePrimary.Visible = true;
                                }
                                else
                                {
                                    imgbtnChangePrimary.Visible = false;
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

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerPreferences") != false)
                                {
                                    customerPreferences.Disabled = false;
                                }
                                else
                                {
                                    customerPreferences.Disabled = true;

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

                                if (dsCapability.Tables[0].Columns.Contains("ViewCustomerDetails") != false)
                                {
                                    cutomerDetails.Disabled = false;
                                }
                                else
                                {
                                    cutomerDetails.Disabled = true;
                                    cutomerDetails.HRef = "";
                                }

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


                            }
                        #endregion

                            string resultXml, errorXml;
                            serviceClient = new ClubcardServiceClient();
                            txtReplaceCardNo.Attributes.Add("readonly", "true");

                            long customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

                            #region Trace Start
                            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.Page_Load() CustomerID-" + customerID);
                            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.Page_Load() CustomerID-" + customerID);
                            #endregion

                            if (serviceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
                            {
                                XmlDocument resulDoc = new XmlDocument();
                                resulDoc.LoadXml(resultXml);
                                DataSet dsHHCustomers = new DataSet();
                                dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                                if (dsHHCustomers.Tables.Count > 0)
                                {
                                    MainCusID = Convert.ToInt64(dsHHCustomers.Tables[0].Rows[0]["CustomerID"].ToString());

                                    if (dsHHCustomers.Tables[0].Rows.Count > 1)
                                    {
                                        if (dsCapability.Tables[0].Columns.Contains("AddCardToAccount") != false)
                                        {
                                            dvMainAssCus.Visible = true;
                                        }
                                        else
                                        {
                                            dvMainAssCus.Visible = false;
                                        }
                                        idbit = true;
                                        AssCusId = Convert.ToInt64(dsHHCustomers.Tables[0].Rows[1]["CustomerID"].ToString());
                                    }

                                    //--Recarding - Populate UUID for customers in hashtable

                                    htCustomerId = new Hashtable(); 
                                    //--Code for recarding
                                    if (MainCusID > 0)
                                        htCustomerId.Add("Primary" ,MainCusID);
                                    if (AssCusId > 0)
                                        htCustomerId.Add("Associate", AssCusId);
                                   
                                    populateUUIDforCutomers(htCustomerId);

                                    //---Recarding Populate UUID for customers in hashtable End

                                    RenderCardsSectionByCustomers(dsHHCustomers);
                                    orderConfirm.Attributes.Add("disabled", "true");
                                    CheckNewOrderValid();
                                }
                            }
                        }
                        //To show the text message only for non standard cards(Not shown even if one standard card exists)
                        if (!boolcontainsStandardCard)
                        {
                            divNonStandardCardMsg.Style.Value = "display: block";

                        }
                    }
                    else
                    {

                        CheckNewOrderValid();
                        //Response.Write(mainCount);
                        if (dvMainAssCus.Visible == true)
                        {
                            if (RBMain.Checked == true)
                            {
                                //lblSuccessMessage.Visible = false;
                                // txtCardNumber.Text = "";
                                if (mainCount >= MaxClubcards)
                                {


                                    AddCardToAccount.Visible = false;
                                    if (idbit == false)
                                    {
                                        divAddCard.Visible = false;
                                    }
                                }
                                else
                                    AddCardToAccount.Visible = true;
                                divAddCard.Visible = true;

                            }
                            else
                            {
                                //lblSuccessMessage.Visible = false;
                                //txtCardNumber.Text = "";
                                if (assCount >= MaxClubcards)
                                {
                                    AddCardToAccount.Visible = false;
                                }
                                else
                                    AddCardToAccount.Visible = true;
                                divAddCard.Visible = true;
                            }
                        }


                    }

                   
                }
                else
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.Page_Load() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.Page_Load()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }
        }

        #endregion

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

        #region RenderCards

        protected void RenderCardsSectionByCustomers(DataSet houseHoldCustomers)
        {
            //string allUserNames = string.Empty;
            long customerID = 0;
            mainCount = 0;
            assCount = 0;
            int asstotcardcount = 0;
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.Page_Load() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            #endregion
            try
            {
                foreach (DataRow customer in houseHoldCustomers.Tables[0].Rows)
                {
                    //concatenate all user names for the welcome message
                    /*allUserNames += ((customer.Table.Columns.Contains("TitleEnglish")) ? Helper.ToTitleCase(customer["TitleEnglish"].ToString()) : "") +
                        " " + ((customer.Table.Columns.Contains("Name1")) ? customer["Name1"].ToString().Substring(0, 1) : "") +
                        " " + ((customer.Table.Columns.Contains("Name3")) ? Helper.ToTitleCase(customer["Name3"].ToString()) : "") + " and ";*/


                    string cardTitle = string.Empty;
                    string customerType = string.Empty;
                    string LocalisedcustomerType = string.Empty;

                    //get appropriate label as per Customer type
                    if (customer["CustomerID"].ToString() == customer["PrimaryCustomerID"].ToString())
                    {
                        customerType = BusinessConstants.CUSTOMER_TYPECODE_MAIN;
                        LocalisedcustomerType = GetLocalResourceObject("MainCardholder").ToString();
                        hdnMainCusId.Value = customer["CustomerID"].ToString();
                        //MKTG00007324 07-08-2012  Kumar P
                        hdnPassMainCusId.Value = CryptoUtil.EncryptTripleDES(customer["CustomerID"].ToString());
                    }
                    else
                    {
                        customerType = BusinessConstants.CUSTOMER_TYPECODE_ASSOC;
                        LocalisedcustomerType = GetLocalResourceObject("AssoicateCardholder").ToString();
                        hdnAssCusID.Value = customer["CustomerID"].ToString();
                        //MKTG00007324 07-08-2012  Kumar P
                        hdnPassAssCusID.Value = CryptoUtil.EncryptTripleDES(customer["CustomerID"].ToString());
                    }
                    //print appropriate welcome message for each Customer on different sections as per Customer type


                    cardTitle = LocalisedcustomerType + ((customer.Table.Columns.Contains("TitleEnglish") && !string.IsNullOrEmpty(customer["TitleEnglish"].ToString())) ? Helper.ToTitleCase(customer["TitleEnglish"].ToString().Trim()) : "") +
                        " " + ((customer.Table.Columns.Contains("Name1") && !string.IsNullOrEmpty(customer["Name1"].ToString().Trim())) ? Helper.ToTitleCase(customer["Name1"].ToString().Trim().Substring(0, 1)) : "") +
                        " " + ((customer.Table.Columns.Contains("Name3") && !string.IsNullOrEmpty(customer["Name3"].ToString().Trim())) ? Helper.ToTitleCase(customer["Name3"].ToString().Trim()) : "");

                    if (customer.Table.Columns.Contains("TitleEnglish"))
                    {
                        if ((string.IsNullOrEmpty(customer["TitleEnglish"].ToString())) || (customer["TitleEnglish"].ToString().ToUpper() == "UNKNOWN"))
                        {

                            cardTitle = LocalisedcustomerType +
                                " " + ((customer.Table.Columns.Contains("Name1") && !string.IsNullOrEmpty(customer["Name1"].ToString().Trim())) ? Helper.ToTitleCase(customer["Name1"].ToString().Trim().Substring(0, 1)) : "") +
                                " " + ((customer.Table.Columns.Contains("Name3") && !string.IsNullOrEmpty(customer["Name3"].ToString().Trim())) ? Helper.ToTitleCase(customer["Name3"].ToString().Trim()) : "");

                        }
                    }
                    //Get all cards for a perticular customer
                    //Service call to fetch all clubcards for a customer
                    string resultXml, errorXml;
                    serviceClient = new ClubcardServiceClient();

                    customerID = Convert.ToInt64(customer["CustomerID"]);

                    //Commented by Noushad
                    if (serviceClient.GetClubcards(out errorXml, out resultXml, customerID, culture))
                    //if (serviceClient.GetClubcardsCustomer(out errorXml, out resultXml, customerID, culture))
                    {
                        if (resultXml != "" && resultXml != "<NewDataSet />")
                        {
                            XmlDocument resulDoc = new XmlDocument();
                            resulDoc.LoadXml(resultXml);
                            DataSet dsClubcard = new DataSet();
                            dsClubcard.ReadXml(new XmlNodeReader(resulDoc));

                            ////Bind it to the repeater on the Cards control
                            if (dsClubcard.Tables["ClubcardDetails"] != null)
                            {

                                //Modal popup call generation
                                //get all values and generate javascript function parameters
                                //showMoreInfoLink = "modalMoreInfoShow('ChangePrimaryCard.aspx?custID=" + CryptoUtil.EncryptTripleDES(customerID.ToString()) + "');return false;";
                                //MKTG00007324 07-08-2012  Kumar P
                                showMoreInfoLink = "modalMoreInfoShowReplacePrimAsscoCard('ChangePrimaryCard.aspx?custID=" + CryptoUtil.EncryptTripleDES(customerID.ToString()) + "');return false;";
                                imgbtnChangePrimary.OnClientClick = showMoreInfoLink;

                                if (customerType == BusinessConstants.CUSTOMER_TYPECODE_MAIN)
                                {
                                    for (int i = 0; i < dsClubcard.Tables["ClubcardDetails"].Rows.Count; i++)
                                    {

                                        string CardStatus = dsClubcard.Tables["ClubcardDetails"].Rows[i]["ClubcardStatusDescEnglish"].ToString();
                                        if (CardStatus == "Normal")
                                        {
                                            mainCount++;
                                        }
                                    }
                                    if (mainCount >= MaxClubcards)
                                    {
                                        AddCardToAccount.Visible = false;
                                        divAddCard.Visible = false;
                                        dvMainAssCus.Visible = false;

                                    }
                                    else
                                    {
                                        AddCardToAccount.Visible = true;
                                        divAddCard.Visible = true;
                                        dvMainAssCus.Visible = true;

                                    }

                                    //if (dsClubcard.Tables["ClubcardDetails"].Rows.Count >= MaxClubcards)
                                    //{
                                    //    AddCardToAccount.Visible = false;
                                    //    if (idbit == false)
                                    //    {
                                    //        divAddCard.Visible = false;
                                    //    }
                                    //}
                                    //mainCount = Convert.ToInt16(dsClubcard.Tables["ClubcardDetails"].Rows.Count);
                                    string Sort_Column_Key = "CardIssuedDate";
                                    string Sort_Direction = " DESC";
                                    string Sort_Format = "{0} {1}";
                                    dsClubcard.Tables[0].DefaultView.Sort = string.Format(Sort_Format, Sort_Column_Key, Sort_Direction);
                                    //DataView dv = dsClubcard.Tables[0].DefaultView;
                                    //dv.Sort = "CardIssueDate";
                                    rptCardDetails.DataSource = dsClubcard;
                                    rptCardDetails.DataBind();
                                    ltrCardHeader.Text = cardTitle;
                                }
                                if (customerType == BusinessConstants.CUSTOMER_TYPECODE_ASSOC)
                                {
                                    for (int i = 0; i < dsClubcard.Tables["ClubcardDetails"].Rows.Count; i++)
                                    {
                                        asstotcardcount++;
                                        string CardStatus = dsClubcard.Tables["ClubcardDetails"].Rows[i]["ClubcardStatusDescEnglish"].ToString();
                                        if (CardStatus == "Normal")
                                        {
                                            assCount++;
                                        }
                                    }
                                    if (assCount >= MaxClubcards)
                                    {
                                        AddCardToAccount.Visible = false;
                                        divAddCard.Visible = false;
                                        dvMainAssCus.Visible = false;

                                    }
                                    else
                                    {
                                        AddCardToAccount.Visible = true;
                                        divAddCard.Visible = true;
                                        dvMainAssCus.Visible = true;
                                        //RBAssociative.Checked = true;
                                    }

                                    //if (dsClubcard.Tables["ClubcardDetails"].Rows.Count >= MaxClubcards)
                                    //{
                                    //    AddCardToAccount.Visible = false;
                                    //}
                                    //assCount = Convert.ToInt16(dsClubcard.Tables["ClubcardDetails"].Rows.Count);

                                    string Sort_Column_Key = "CardIssuedDate";
                                    string Sort_Direction = " DESC";
                                    string Sort_Format = "{0} {1}";
                                    dsClubcard.Tables[0].DefaultView.Sort = string.Format(Sort_Format, Sort_Column_Key, Sort_Direction);
                                    divAssociate.Visible = true;
                                    rptCardDetailsAssociate.DataSource = dsClubcard;
                                    rptCardDetailsAssociate.DataBind();
                                    ltrCardHeaderAssociate.Text = cardTitle;
                                    if (assCount >= MaxClubcards && mainCount >= MaxClubcards)
                                    {
                                        //AddCardToAccount.Visible = false;
                                        divAddCard.Visible = false;
                                        dvMainAssCus.Visible = false;
                                    }
                                    else
                                    {
                                        //AddCardToAccount.Visible = true;
                                        divAddCard.Visible = true;
                                        dvMainAssCus.Visible = true;
                                    }
                                    if (mainCount >= MaxClubcards)
                                    {
                                        AddCardToAccount.Visible = false;
                                    }
                                    else
                                    {
                                        AddCardToAccount.Visible = true;
                                    }


                                }
                                if (asstotcardcount == 0)
                                {
                                    dvMainAssCus.Visible = false;
                                }


                                if (!Convert.ToBoolean(hdnIsAddCard.Value))
                                {
                                    AddCardToAccount.Visible = false;
                                    divAddCard.Visible = false;
                                    dvMainAssCus.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.RenderCardsSectionByCustomers() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.RenderCardsSectionByCustomers() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.RenderCardsSectionByCustomers()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();

                    }
                }
            }
        }

        #endregion

        #region RepeaterDataBind

        protected void rptCardDetailsMain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string showMoreInfoLink = string.Empty;
            Localize lclReplace = new Localize();
            try
            {
                //CountofCardsforMain = 0;
                Literal ltrCardNo = (Literal)e.Item.FindControl("ltrClubcardNumber");
                Literal ltrIssueDate = (Literal)e.Item.FindControl("ltrIssueDate");
                LinkButton ltrCardStatus = (LinkButton)e.Item.FindControl("ltrCardStatus");
                Literal lblCardStatus = (Literal)e.Item.FindControl("lblCardStatus");
                Literal ltrTypeofCard = (Literal)e.Item.FindControl("ltrTypeofCard");
                RadioButton rdbReplaceMain = (RadioButton)e.Item.FindControl("rdbReplaceMain");
                //RadioButton rdbReplaceAssociate = (RadioButton)e.Item.FindControl("rdbReplaceAssociate");
                HiddenField hdnCardNumber = (HiddenField)e.Item.FindControl("hdnCardNumber");

                #region Find Control By Header

                if (funCount == 0)
                {
                    boolReplaceCard = GetConfigDetails();
                    funCount++;
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell thTypeofcard = (HtmlTableCell)e.Item.FindControl("thTypeofcard");
                        e.Item.FindControl("thReplace").Visible = false;
                        lclReplace = (Localize)e.Item.FindControl("lclReplace");
                        lclReplace.Visible = false;
                    }
                }
                #endregion

                #region Find Control By Item
                if (e.Item.ItemType == ListItemType.Item)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell tdTypeofCard = (HtmlTableCell)e.Item.FindControl("tdTypeofCard");
                        HtmlTableCell tdrdbReplaceMain = (HtmlTableCell)e.Item.FindControl("tdrdbReplaceMain");
                        tdrdbReplaceMain.Visible = false;
                    }
                }
                #endregion

                #region Find Control By AlternatingItem
                if (e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell tdTypeofCard = (HtmlTableCell)e.Item.FindControl("tdTypeofCard");
                        HtmlTableCell tdrdbReplaceMain = (HtmlTableCell)e.Item.FindControl("tdrdbReplaceMain");
                        tdrdbReplaceMain.Visible = false;
                    }
                }
                #endregion


                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion

                //Culture based Date Display 
                // NGC Change
                if (ltrIssueDate != null)
                {
                    ltrIssueDate.Text = Helper.ToTitleCase(ltrIssueDate.Text);
                    //ltrIssueDate.Text = ltrIssueDate.Text.ToString("MM/dd/yyyy");
                    if (!string.IsNullOrEmpty(ltrIssueDate.Text))
                    {
                        System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo("en-GB");
                        DateTime dates = Convert.ToDateTime(ltrIssueDate.Text, cultures);
                        ltrIssueDate.Text = dates.ToString("dd/MM/yy", cultures);
                    }
                }


                //Alter Clubcard Type Description to make it Title Case
                if (ltrTypeofCard != null)
                {
                    #region RoleCapabilityImplementation
                    XmlDocument xmlCapability = new XmlDocument();
                    DataSet dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables[0].Columns.Contains("ChangeCardStatus"))
                        {
                            ltrCardStatus.Visible = true;
                            lblCardStatus.Visible = false;

                            showMoreInfoLink = "modalMoreInfoShow('EditCardStatus.aspx?" + CryptoUtil.EncryptTripleDES(ltrCardNo.Text) + "," + hdnMainCusId.Value + "');return false;";
                            if (ltrCardStatus != null && ltrCardStatus.Text != "LostStolenDamaged")
                            {
                                ltrCardStatus.OnClientClick = showMoreInfoLink;
                                //if (culture == "en-US")
                                //    ltrCardStatus.Visible = true;

                            }
                        }
                        else
                        {
                            ltrCardStatus.Visible = false;
                            lblCardStatus.Visible = true;
                        }
                    }
                    #endregion

                    //Modal popup call generation
                    //get all values and generate javascript function parameters


                    ltrTypeofCard.Text = Helper.ToTitleCase(ltrTypeofCard.Text);
                    if (ltrTypeofCard.Text.ToUpper() == "STANDARD" || ltrTypeofCard.Text.ToUpper() == "ONLINE STANDARD")
                    {
                        if (boolReplaceCard)
                        {

                            if (!boolStandardRadioMain)
                            {
                                //if (!Convert.ToBoolean(hdnmaxreplacementsts.Value))
                                //{
                                lclReplace.Visible = true;
                                rdbReplaceMain.Visible = true;
                                rdbReplaceMain.Style.Add("background", "transparent");
                                rdbReplaceMain.Style.Add("margin-left", "12px");
                                rdbReplaceMain.Style.Add(" margin-bottom", "10px");
                                //}
                                //else
                                //{
                                //rdbReplaceMain.Enabled = false;
                                //}

                                if (hdnCardNumber != null)
                                {
                                    rdbReplaceMain.Attributes.Add("onclick", "rdbCheckedMain('" + hdnCardNumber.Value + "');");
                                    boolStandardRadioMain = true;
                                    if (disablerdbMain)
                                    {
                                        rdbReplaceMain.Attributes.Add("disabled", "true");
                                        rdbReplaceMain.Style.Add("background", "transparent");
                                    }

                                    //--Recarding change - Start
                                    if (htCustomerId.Count > 0)
                                    {
                                        string strPrimaryCustomerUUID = string.Empty; 
                                        //--Uncomment this below line for real data
                                        strPrimaryCustomerUUID = htCustomerId["Primary"].ToString();

                                        //-- Uncomment the below one with stubed data
                                        strPrimaryCustomerUUID = GetLocalResourceObject("UUIDForStub").ToString();

                                        //--Make call for customer card replacement eligibility
                                        NewCardEntitled card = new NewCardEntitled();

                                        card = isCustomerEligibleForCardReplacement(strPrimaryCustomerUUID);
                                        if (!card.isEntitledToNewCard)
                                        {
                                            string str = string.Empty;
                                            //--Customer already placed the order replacement request or reached to max attempt
                                            if (card.reason != null)
                                                str = GetLocalResourceObject(card.reason).ToString();
                                            else
                                                str = GetLocalResourceObject("cardOrderedWithin10Days").ToString();
                                            lblPrimary.Text = GetLocalResourceObject("MainCardholder").ToString() + str;
                                            divReplaceCardMessage.Style.Value = "display:block;";
                                        }
                                    
                                    }                                  

                                    //-- Recarding changes - End
                                }
                            }
                        }
                        boolcontainsStandardCard = true; //To check standard card exists or not.
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.rptCardDetailsMain_ItemDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.rptCardDetailsMain_ItemDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        protected void rptCardDetailsAssociate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            string showMoreInfoLink = string.Empty;
            Localize lclReplace = new Localize();
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
            #endregion
            try
            {
                Literal ltrCardNo = (Literal)e.Item.FindControl("ltrClubcardNumber");
                Literal ltrIssueDate = (Literal)e.Item.FindControl("ltrIssueDate");
                LinkButton ltrCardStatus = (LinkButton)e.Item.FindControl("ltrCardStatusAssoc");
                Literal lblCardStatus = (Literal)e.Item.FindControl("lblCardStatusAssoc");
                Literal ltrTypeofCard = (Literal)e.Item.FindControl("ltrTypeofCard");
                //RadioButton rdbReplaceMain = (RadioButton)e.Item.FindControl("rdbReplaceMain");
                RadioButton rdbReplaceAssociate = (RadioButton)e.Item.FindControl("rdbReplaceAssociate");
                HiddenField hdnCardNumber = (HiddenField)e.Item.FindControl("hdnCardNumber");

                #region Find Control By Header
                if (funGetConfigDetails == 0)
                {
                    boolReplaceCard = GetConfigDetails();
                    funGetConfigDetails++;
                }
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell thTypeofcard = (HtmlTableCell)e.Item.FindControl("thTypeofcard");
                        e.Item.FindControl("thReplace").Visible = false;
                        lclReplace = (Localize)e.Item.FindControl("lclReplace");
                        lclReplace.Visible = false;
                    }
                }
                #endregion

                #region Find Control By Item
                if (e.Item.ItemType == ListItemType.Item)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell tdTypeofCard = (HtmlTableCell)e.Item.FindControl("tdTypeofCard");
                        HtmlTableCell tdrdbReplaceMain = (HtmlTableCell)e.Item.FindControl("tdrdbReplaceMain");
                        tdrdbReplaceMain.Visible = false;
                    }
                }
                #endregion

                #region Find Control By AlternatingItem
                if (e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (!boolReplaceCard)
                    {
                        HtmlTableCell tdTypeofCard = (HtmlTableCell)e.Item.FindControl("tdTypeofCard");
                        HtmlTableCell tdrdbReplaceMain = (HtmlTableCell)e.Item.FindControl("tdrdbReplaceMain");
                        tdrdbReplaceMain.Visible = false;
                    }
                }
                #endregion


                //Culture based Date Display 
                // NGC Change
                if (ltrIssueDate != null)
                {
                    ltrIssueDate.Text = Helper.ToTitleCase(ltrIssueDate.Text);
                    if (!string.IsNullOrEmpty(ltrIssueDate.Text))
                    {

                        System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo("en-GB");
                        DateTime dates = Convert.ToDateTime(ltrIssueDate.Text, cultures);
                        ltrIssueDate.Text = dates.ToString("dd/MM/yy", cultures);
                    }
                }
                if (ltrCardStatus != null)
                {
                    if (ltrCardStatus.Text != "")
                    {
                        // lblCardStatus.Visible = false;
                        ltrCardStatus.Visible = true;
                    }
                    else
                        ltrCardStatus.Visible = false;
                }

                //Alter Clubcard Type Description to make it Title Case
                //if (ltrTypeofCard != null)
                //{
                //    //Modal popup call generation
                //    //get all values and generate javascript function parameters
                //    showMoreInfoLink = "modalMoreInfoShow('" + CryptoUtil.EncryptTripleDES(ltrCardNo.Text) + "');return false;";
                //    if (ltrCardStatus != null)
                //    {
                //        ltrCardStatus.OnClientClick = showMoreInfoLink;
                //    }

                if (ltrTypeofCard != null)
                {
                    #region RoleCapabilityImplementation
                    XmlDocument xmlCapability = new XmlDocument();
                    DataSet dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables[0].Columns.Contains("ChangeCardStatus"))
                        {
                            ltrCardStatus.Visible = true;
                            lblCardStatus.Visible = false;

                            showMoreInfoLink = "modalMoreInfoShow('EditCardStatus.aspx?" + CryptoUtil.EncryptTripleDES(ltrCardNo.Text) + "," + hdnAssCusID.Value + "');return false;";
                            if (ltrCardStatus != null && ltrCardStatus.Text != "LostStolenDamaged")
                            {
                                ltrCardStatus.OnClientClick = showMoreInfoLink;
                                //if (culture == "en-US")
                                //    ltrCardStatus.Visible = true;

                            }
                        }
                        else
                        {
                            ltrCardStatus.Visible = false;
                            lblCardStatus.Visible = true;
                        }
                    }
                    #endregion
                    ltrTypeofCard.Text = Helper.ToTitleCase(ltrTypeofCard.Text);
                    if (ltrTypeofCard.Text.ToUpper() == "STANDARD" || ltrTypeofCard.Text.ToUpper() == "ONLINE STANDARD")
                    {
                        if (boolReplaceCard)
                        {
                            if (!boolStandardRadioAsso)
                            {

                                lclReplace.Visible = true;
                                rdbReplaceAssociate.Visible = true;
                                rdbReplaceAssociate.Style.Add("background", "transparent");
                                rdbReplaceAssociate.Style.Add("margin-left", "12px");
                                rdbReplaceAssociate.Style.Add(" margin-bottom", "10px");

                                if (hdnCardNumber != null)
                                {
                                    rdbReplaceAssociate.Attributes.Add("onclick", "rdbCheckedAssociate('" + hdnCardNumber.Value + "');");
                                    boolStandardRadioAsso = true;
                                    if (disablerdbAssociate)
                                    {
                                        rdbReplaceAssociate.Attributes.Add("disabled", "true");
                                        rdbReplaceAssociate.Style.Add("background", "transparent");
                                    }
                                    //--Recarding change - Start
                                    if (htCustomerId.Count > 0)
                                    {
                                        string strAssociateCustomerUUID = string.Empty;

                                        strAssociateCustomerUUID = htCustomerId["Associate"].ToString();

                                        //-- Uncomment the below one with stubed data
                                        strAssociateCustomerUUID = GetLocalResourceObject("UUIDForStub").ToString();

                                        //--Make call for customer card replacement eligibility
                                        NewCardEntitled card = new NewCardEntitled();
                                        card = isCustomerEligibleForCardReplacement(strAssociateCustomerUUID);
                                        if (!card.isEntitledToNewCard)
                                        {
                                            string str=string.Empty;
                                            //--Customer already placed the order replacement request or reached to max attempt
                                            if (card.reason != null)
                                                str = GetLocalResourceObject(card.reason).ToString();
                                            else
                                                str = GetLocalResourceObject("cardOrderedWithin10Days").ToString();

                                            lblAsscociate.Text = GetLocalResourceObject("AssoicateCardholder").ToString() + str;
                                            divReplaceCardMessage.Style.Value = "display:block;";
                                        }

                                    }

                                    //-- Recarding changes - End
                                }
                            }
                        }
                        boolcontainsStandardCard = true; //To check standard card exists or not.
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        #endregion

        #region ConfirmOrder

        /// <summary>
        /// On Confirm click event, submits the new replacement order if the form is valid
        /// <para>Calls a entity method to save the Customer.Add_New_OrderReplacement through web service</para>
        /// </summary>
        /// <param name="sender">button object</param>
        /// <param name="e">click event</param>
        protected void orderConfirm_Click(object sender, ImageClickEventArgs e)
        {
            string consumer = string.Empty;
            xmlCapability = new XmlDocument();
            dsCapability = new DataSet();

            try
            {

                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                {
                    xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                    dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                    //Check if user has update role.
                    if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerCards") != false)
                    {
                        string strStateOfCard = string.Empty;
                        string strCustomerUUID = string.Empty;

                        if ((RadioLostMain.Checked || RadioDamagedMain.Checked || RadioStolenMain.Checked) && hdnPrimaryCustomerUUID.Value != "")
                        {
                            strCustomerUUID = hdnPrimaryCustomerUUID.Value.ToString();
                        }
                        else if ((RadioLost.Checked || RadioDamaged.Checked || RadioStolen.Checked) && hdnAssociateCustomerUUID.Value != "")
                        {
                            strCustomerUUID = hdnAssociateCustomerUUID.Value.ToString();
                        }


                        //--Primary Customer
                        if (RadioLostMain.Checked)
                            strStateOfCard = StateOfOldCard.compromised.ToString();
                        if (RadioDamagedMain.Checked)
                            strStateOfCard = StateOfOldCard.notCompromised.ToString();
                        if (RadioStolenMain.Checked)
                            strStateOfCard = StateOfOldCard.none.ToString();

                        //--Associate customer

                        if (RadioLost.Checked)
                            strStateOfCard = StateOfOldCard.compromised.ToString();
                        if (RadioDamaged.Checked)
                            strStateOfCard = StateOfOldCard.notCompromised.ToString();
                        if (RadioStolen.Checked)
                            strStateOfCard = StateOfOldCard.none.ToString();

                        CreateCardsetResponse res = new CreateCardsetResponse();

                        //--This code only for stub. with web services you need to uncomment.
                        strCustomerUUID = GetLocalResourceObject("UUIDForStubSave").ToString();

                        if (strCustomerUUID == "" || strStateOfCard == "")
                        {
                            return;
                        }

                        res = createCardReplacementRequest(strCustomerUUID, strStateOfCard);

                        //if (res.cardSetRequestId != null && res.cardSetRequestId.Length > 0)
                        //{
                        if (res == null)
                        {
                            divErrorMessage.Style.Value = "display: block";
                            divErrorMessage.InnerHtml = GetLocalResourceObject("processReqResource1.Text").ToString();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.orderConfirm_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.orderConfirm_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.orderConfirm_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        protected void orderConfirm_Old_Click(object sender, ImageClickEventArgs e)
        {
            string consumer = string.Empty;
            xmlCapability = new XmlDocument();
            dsCapability = new DataSet();

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
            {
                xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                //Check if user has update role.
                if (dsCapability.Tables[0].Columns.Contains("UpdateCustomerCards") != false)
                {
                    if (isValidRequest)
                    {
                        if (IsPageValidMain())
                        {
                            //If card replacement request is valid,
                            //Capture the inputs in CardReqType and CardReqRsn
                            #region Local variable declaration

                            Hashtable inputParams = new Hashtable();
                            string cardReqType = string.Empty;
                            string cardReqRsn = string.Empty;
                            string objXml = string.Empty;
                            string errorXml = string.Empty;
                            string resultXml = string.Empty;
                            XmlDocument resulDoc = new XmlDocument();
                            DataSet dsResult = new DataSet();
                            bool isServiceSuccessful = false;
                            short orderProcessWindow = Convert.ToInt16(ConfigurationSettings.AppSettings["ORDRPL_PROCESSWINDOW"]);
                            long customerID;
                            #endregion

                            //Get selected Request type code
                            //if (RadioNewCardMain.Checked)
                            //    cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_CARD;
                            //else if (RadioNewFobMain.Checked)
                            //    cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_KEYFOB;
                            //else if (RadioNewCardnFobMain.Checked)
                            cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_CARD_KEYFOB;

                            //Get selected Request type Reason
                            if (RadioLostMain.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_LOST;
                            else if (RadioDamagedMain.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_DAMAGED;
                            else if (RadioStolenMain.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_STOLEN;
                            else if (RadioMoreFobsMain.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_MOREFOBS;
                            else if (RadioOtherMain.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_OTHER;

                            //set  up the hashtable with customer id, card number and request type and reason
                            if (hdnSelCustID.Value == "")
                            {
                                customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                            }
                            else
                            {
                                customerID = Convert.ToInt64(hdnSelCustID.Value);
                            }
                            //customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                            inputParams["CustomerID"] = customerID;
                            inputParams["ClubcardID"] = Convert.ToInt64(txtReplaceCardNo.Text.ToString());
                            inputParams["RequestCode"] = cardReqType;
                            inputParams["RequestReasonCode"] = cardReqRsn;

                            //load the xml string with hashtable values
                            objXml = Helper.HashTableToXML(inputParams, "OrderReplacement");
                            #region Trace Start
                            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.orderConfirm_Click() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.orderConfirm_Click() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                            #endregion
                            try
                            {
                                //Initialize the service reference
                                serviceClient = new ClubcardServiceClient();
                                consumer = Helper.GetTripleDESEncryptedCookieValue("UserName").ToString();

                                //Calls the loyalty entity layer function Customer.Add_New_OrderReplacement() through service function AddNewOrderReplacement()
                                isServiceSuccessful = serviceClient.AddNewOrderReplacement(out errorXml, out customerID, objXml, consumer);

                                if (isServiceSuccessful && string.IsNullOrEmpty(errorXml))
                                {
                                    ShowInConfirmMsg();
                                }
                            }
                            catch (Exception exp)
                            {
                                #region Trace Error
                                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.orderConfirm_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.orderConfirm_Click() - Error Message :" + exp.ToString());
                                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.orderConfirm_Click()");
                                NGCTrace.NGCTrace.ExeptionHandling(exp);
                                #endregion Trace Error
                                throw exp;
                            }
                            finally
                            {
                                if (serviceClient != null)
                                {
                                    if (serviceClient.State == CommunicationState.Faulted)
                                    {
                                        serviceClient.Abort();
                                    }
                                    else if (serviceClient.State != CommunicationState.Closed)
                                    {
                                        serviceClient.Close();
                                    }
                                }
                            }
                        }
                        else if (isMainConfirmError)
                        {

                            #region Local variable declaration

                            Hashtable inputParams = new Hashtable();
                            string cardReqType = string.Empty;
                            string cardReqRsn = string.Empty;
                            string objXml = string.Empty;
                            string errorXml = string.Empty;
                            string resultXml = string.Empty;
                            XmlDocument resulDoc = new XmlDocument();
                            DataSet dsResult = new DataSet();
                            bool isServiceSuccessful = false;
                            short orderProcessWindow = Convert.ToInt16(ConfigurationSettings.AppSettings["ORDRPL_PROCESSWINDOW"]);
                            long customerID;
                            #endregion

                            //Get selected Request type code
                            //if (RadioNewCard.Checked)
                            //    cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_CARD;
                            //else if (RadioNewFob.Checked)
                            //    cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_KEYFOB;
                            //else if (RadioNewCardnFob.Checked)
                            cardReqType = BusinessConstants.ORDRPL_TYPE_NEW_CARD_KEYFOB;

                            //Get selected Request type Reason
                            if (RadioLost.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_LOST;
                            else if (RadioDamaged.Checked)
                                cardReqRsn = BusinessConstants.ORDRPL_RSN_DAMAGED;
                            //else if (RadioStolen.Checked)
                            //    cardReqRsn = BusinessConstants.ORDRPL_RSN_STOLEN;
                            //else if (RadioMoreFobs.Checked)
                            //    cardReqRsn = BusinessConstants.ORDRPL_RSN_MOREFOBS;
                            //else if (RadioOther.Checked)
                            //    cardReqRsn = BusinessConstants.ORDRPL_RSN_OTHER;

                            //set  up the hashtable with customer id, card number and request type and reason

                            if (hdnSelCustID.Value == "")
                            {
                                customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                            }
                            else
                            {
                                customerID = Convert.ToInt64(hdnSelCustID.Value);
                            }

                            inputParams["CustomerID"] = customerID;
                            inputParams["ClubcardID"] = Convert.ToInt64(txtReplaceCardNo.Text.ToString());
                            inputParams["RequestCode"] = cardReqType;
                            inputParams["RequestReasonCode"] = cardReqRsn;


                            //load the xml string with hashtable values
                            objXml = Helper.HashTableToXML(inputParams, "OrderReplacement");

                            try
                            {
                                //Initialize the service reference
                                serviceClient = new ClubcardServiceClient();
                                //to get the dundee user id.
                                consumer = Helper.GetTripleDESEncryptedCookieValue("UserName").ToString();

                                //Calls the loyalty entity layer function Customer.Add_New_OrderReplacement() through service function AddNewOrderReplacement()
                                isServiceSuccessful = serviceClient.AddNewOrderReplacement(out errorXml, out customerID, objXml, consumer);

                                if (isServiceSuccessful && string.IsNullOrEmpty(errorXml))
                                {
                                    ShowInConfirmMsg();
                                }
                            }
                            catch (Exception exp)
                            {
                                #region Trace Error
                                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound() - Error Message :" + exp.ToString());
                                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.rptCardDetailsAssociate_ItemDataBound()");
                                NGCTrace.NGCTrace.ExeptionHandling(exp);
                                #endregion Trace Error
                                throw exp;
                            }
                            finally
                            {
                                if (serviceClient != null)
                                {
                                    if (serviceClient.State == CommunicationState.Faulted)
                                    {
                                        serviceClient.Abort();
                                    }
                                    else if (serviceClient.State != CommunicationState.Closed)
                                    {
                                        serviceClient.Close();
                                    }
                                }
                            }
                        }
                        #region Trace End
                        NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.orderConfirm_Click() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.orderConfirm_Click() CustomerID-" + Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());
                        #endregion
                    }
                }
            }
        }


        #endregion

        #region PageValidity

        /// <summary>
        /// Validate if both request type and reason radio buttons are selected, if not returns false also 
        /// <para>show/hide the error message</para>
        /// </summary>
        /// <returns>true if validation is successful</returns>
        private bool IsPageValidMain()
        {

            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.IsPageValidMain()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.IsPageValidMain()");
            #endregion
            try
            {


                bool isTypeSected = false, isReasonSelected = false;
                //Atleast a card reason and card request type code is required to go further.
                //if ((RadioNewCardMain.Checked || RadioNewFobMain.Checked || RadioNewCardnFobMain.Checked))
                //{
                //    //Request type is not selected
                //    isTypeSected = true;
                //}

                isTypeSected = true;// Always true bcoz new card and 2 key fobs is only one option and should always true

                if ((RadioLostMain.Checked || RadioDamagedMain.Checked || RadioStolenMain.Checked || RadioMoreFobsMain.Checked || RadioOtherMain.Checked))
                {
                    //Request reason is not selected
                    isReasonSelected = true;
                }

                //if both type and reason not selected then show appropriate UI validation messages
                if (!isTypeSected && !isReasonSelected)
                {
                    divErrorMessage.Style.Value = "display: block";
                    isMainConfirmError = true;
                    return false;
                }
                //if only type not selected then show appropriate UI validation message
                else if (!isTypeSected)
                {
                    divErrorMessage.Style.Value = "display: block";
                    isMainConfirmError = true;
                    return false;
                }
                //if only request reason not selected then show appropriate UI validation message
                else if (!isReasonSelected)
                {
                    divErrorMessage.Style.Value = "display: block";
                    isMainConfirmError = true;
                    return false;
                }
                //if both type and reason are selected hide the UI validation messages
                else
                {
                    return true;
                }

            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.IsPageValidMain() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.IsPageValidMain() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.IsPageValidMain()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }

        }

        private bool IsPageValidAssociate()
        {
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.IsPageValidAssociate()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.IsPageValidAssociate()");
            #endregion
            try
            {
                bool bResult = false;
                if (RadioNewCard.Checked)
                {
                    bool isTypeSected = false, isReasonSelected = false;
                    //Atleast a card reason and card request type code is required to go further.
                    if ((RadioNewCard.Checked || RadioNewFob.Checked || RadioNewCardnFob.Checked))
                    {
                        //Request type is not selected
                        bResult = true;
                    }
                    if ((RadioLost.Checked || RadioDamaged.Checked))
                    {
                        //Request reason is not selected
                        bResult = true;
                    }

                    //if both type and reason not selected then show appropriate UI validation messages
                    if (!isTypeSected && !isReasonSelected)
                    {
                        divErrorMessage.Style.Value = "display: block";
                        isAssoConfirmError = true;
                        bResult = false;
                    }
                    //if only type not selected then show appropriate UI validation message
                    else if (!isTypeSected)
                    {
                        divErrorMessage.Style.Value = "display: block";
                        isAssoConfirmError = true;
                        bResult = false;
                    }
                    //if only request reason not selected then show appropriate UI validation message
                    else if (!isReasonSelected)
                    {
                        divErrorMessage.Style.Value = "display: block";
                        isAssoConfirmError = true;
                        bResult = false;
                    }
                    //if both type and reason are selected hide the UI validation messages
                    else
                    {
                        bResult = true;
                    }
                }
                return bResult;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.IsPageValidAssociate() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.IsPageValidAssociate() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.IsPageValidAssociate()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error
                throw exp;
            }
        }

        #endregion

        #region CheckValidityOfOrder

        protected bool CheckNewOrderValid()
        {
            /* Internally Checks 
             * card validity, only standard cards to be replaced 
             * previous order condition
             * max 3 order in a year condition
             */

            #region Local variable declaration

            //Local veriable declarations
            //string objName = string.Empty, methodName = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            Hashtable conditionParams = new Hashtable();
            int rowCount = 0, maxRowCount = 1;
            DataSet dsIsNewOrderValid = new DataSet();
            int noOfDays = 0;
            short orderProcessWindow = Convert.ToInt16(ConfigurationSettings.AppSettings["ORDRPL_PROCESSWINDOW"]);
            XmlDocument resulDoc = new XmlDocument();
            bool isServiceSuccessful = false;
            string cardNumber = string.Empty;

            #endregion

            //initialize values in hashtable
            if (hdnSelCustID.Value == "")
            {
                conditionParams["CustomerID"] = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
            }
            else
            {
                conditionParams["CustomerID"] = hdnSelCustID.Value;
            }
            conditionParams["OrderProcessWindow"] = orderProcessWindow;

            //load the xml string with hashtable values
            conditionXml = Helper.HashTableToXML(conditionParams, "CheckOrderReplacement");
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.CheckNewOrderValid()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.CheckNewOrderValid() conditionXml-" + conditionXml);
            #endregion
            try
            {
                //Initialize the service reference
                serviceClient = new ClubcardServiceClient();

                //This function returns the oldOrderExists,noOfDaysLeftToProcess,countOrdersPlacedInYear,standardCardNumber parameters
                isServiceSuccessful = serviceClient.IsNewOrderReplacementValid(out errorXml, out resultXml,
                                                         out rowCount, conditionXml, maxRowCount,
                                                         Thread.CurrentThread.CurrentCulture.IetfLanguageTag);

                if (string.IsNullOrEmpty(errorXml) && isServiceSuccessful)
                {
                    if (!string.IsNullOrEmpty(resultXml) && rowCount > 0)
                    {
                        //Load the result xml containing parameters into a data set
                        resulDoc.LoadXml(resultXml);
                        dsIsNewOrderValid.ReadXml(new XmlNodeReader(resulDoc));
                        //Check for the existing order.. checks oldOrderExists,noOfDaysLeftToProcess values interanally
                        if (!IsOrderExist(dsIsNewOrderValid, ref noOfDays))
                        {
                            //Checks for the Maximum number of orders reached (maximum 3 in a year)
                            //internally checks for countOrdersPlacedInYear value
                            if (!IsMaxOrdersReached(dsIsNewOrderValid))
                            {
                                isValidRequest = true;
                            }
                            else
                            {
                                isValidRequest = false;
                                ShowMaxOrdersReached();
                            }
                        }
                        else
                        {
                            isValidRequest = false;
                            //If yes show the appropriate message
                            ShowInProcessMsg();
                        }
                    }
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.CheckNewOrderValid()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.CheckNewOrderValid() conditionXml-" + conditionXml);
                #endregion

                return isValidRequest;
            }
            catch (Exception exp)
            {

                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.CheckNewOrderValid() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.CheckNewOrderValid() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.CheckNewOrderValid()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }
        }

        #endregion

        #region Check Different Validity

        /// <summary>
        /// Checks for the existing order
        /// <para>internally check values of oldOrderExists, noOfDaysLeftToProcess in dataset passed</para>
        /// <para>and returns true/false accordingly</para>
        /// </summary>
        /// <param name="dsOrderReplacement">dataset containing oldOrderExists, noOfDaysLeftToProcess values</param>
        /// <param name="noOfDays">reference variable to be set</param>
        /// <returns>true if order existing else false</returns>
        private bool IsOrderExist(DataSet dsOrderReplacement, ref int noOfDays)
        {
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.IsOrderExist()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.IsOrderExist() noOfDays-" + noOfDays);
            #endregion
            try
            {
                string existingOrder = dsOrderReplacement.Tables[0].Rows[0]["oldOrderExists"].ToString();
                if (!string.IsNullOrEmpty(existingOrder))
                {
                    if (existingOrder.Equals("0"))
                    {
                        #region Trace End
                        NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.CheckNewOrderValid()");
                        NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.CheckNewOrderValid() noOfDays-" + noOfDays);
                        #endregion
                        return false; // if 0 order exists then return false

                    }
                    else
                    {
                        //set number of days variable with the value in the dataset and return true
                        noOfDays = Convert.ToInt32(dsOrderReplacement.Tables[0].Rows[0]["noOfDaysLeftToProcess"].ToString());
                        #region Trace End
                        NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.CheckNewOrderValid()");
                        NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.CheckNewOrderValid() noOfDays-" + noOfDays);
                        #endregion
                        return true;
                    }
                }
                else //throws exception there is any expected value
                    throw new Exception("Could not fetch number of replacement orders");
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.IsOrderExist() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.IsOrderExist() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.IsOrderExist()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
        }

        /// <summary>
        /// Checks for the Maximum number of orders reached (maximum 3 in a year)
        /// <para>internally checks for standardCardNumber value</para>
        /// <para>returns true if maximum 3 orders limit reached (processed and unprocessed)</para>
        /// </summary>
        /// <param name="dsOrderReplacement">dataset containing standardCardNumber value</param>
        /// <returns>returns true if maximum 3 orders limit reached (processed and unprocessed)</returns>
        private bool IsMaxOrdersReached(DataSet dsOrderReplacement)
        {

            string countOrdersInAYear = dsOrderReplacement.Tables[0].Rows[0]["countOrdersPlacedInYear"].ToString();
            int countOrders = 0;
            if (!string.IsNullOrEmpty(countOrdersInAYear))
            {
                countOrders = Convert.ToInt32(countOrdersInAYear);
            }
            //check for count of orders value > 3
            if (countOrders >= 3) return true; //if yes return true
            else return false; //else return false
        }

        #endregion

        #region HTML elelments show/hide functions

        protected void ShowInProcessMsg()
        {
            pInProcessMsg.Visible = true;
            pInProcessMsg.Style.Clear();
            pInProcessMsg.Style.Value = "display: block";
            pMaxOrdersReached.Visible = false;

            if (rptCardDetails != null)
            {
                if (rptCardDetails.Items.Count > 0)
                {
                    for (int i = 0; i < rptCardDetails.Items.Count; i++)
                    {
                        if (rptCardDetails.Items[i].FindControl("ltrTypeofCard") != null)
                        {
                            Literal ltrCardType = ((Literal)rptCardDetails.Items[i].FindControl("ltrTypeofCard"));
                            if (ltrCardType.Text.ToUpper() == "STANDARD" || ltrCardType.Text.ToUpper() == "ONLINE STANDARD")
                            {
                                if (rptCardDetails.Items[i].FindControl("rdbReplaceMain") != null)
                                {
                                    RadioButton rdbMaindisable = ((RadioButton)rptCardDetails.Items[i].FindControl("rdbReplaceMain"));
                                    rdbMaindisable.Style.Add("background", "transparent");
                                    rdbMaindisable.Style.Add("margin-left", "12px");
                                    rdbMaindisable.Style.Add(" margin-bottom", "10px");
                                    rdbMaindisable.Attributes.Add("disabled", "true");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (rptCardDetailsAssociate != null)
            {
                if (rptCardDetailsAssociate.Items.Count > 0)
                {
                    for (int i = 0; i < rptCardDetailsAssociate.Items.Count; i++)
                    {
                        if (rptCardDetailsAssociate.Items[i].FindControl("ltrTypeofCard") != null)
                        {
                            Literal ltrCardType = ((Literal)rptCardDetailsAssociate.Items[i].FindControl("ltrTypeofCard"));
                            if (ltrCardType.Text.ToUpper() == "STANDARD" || ltrCardType.Text.ToUpper() == "ONLINE STANDARD")
                            {
                                if (rptCardDetailsAssociate.Items[0].FindControl("rdbReplaceAssociate") != null)
                                {
                                    RadioButton rdbAssodisable = ((RadioButton)rptCardDetailsAssociate.Items[0].FindControl("rdbReplaceAssociate"));
                                    rdbAssodisable.Style.Add("background", "transparent");
                                    rdbAssodisable.Style.Add("margin-left", "12px");
                                    rdbAssodisable.Style.Add(" margin-bottom", "10px");
                                    rdbAssodisable.Attributes.Add("disabled", "true");
                                    break;
                                }
                            }
                        }
                    }

                }
            }
        }

        protected void ShowInConfirmMsg()
        {
            pInConfirmMsg.Visible = true;
            pInConfirmMsg.Style.Clear();
            pInConfirmMsg.Style.Value = "display: block";
            pMaxOrdersReached.Visible = false;

            if (rptCardDetails != null)
            {
                if (rptCardDetails.Items.Count > 0)
                {
                    for (int i = 0; i < rptCardDetails.Items.Count; i++)
                    {
                        if (rptCardDetails.Items[i].FindControl("ltrTypeofCard") != null)
                        {
                            Literal ltrCardType = ((Literal)rptCardDetails.Items[i].FindControl("ltrTypeofCard"));
                            if (ltrCardType.Text.ToUpper() == "STANDARD" || ltrCardType.Text.ToUpper() == "ONLINE STANDARD")
                            {
                                if (rptCardDetails.Items[i].FindControl("rdbReplaceMain") != null)
                                {
                                    RadioButton rdbMaindisable = ((RadioButton)rptCardDetails.Items[i].FindControl("rdbReplaceMain"));
                                    rdbMaindisable.Style.Add("background", "transparent");
                                    rdbMaindisable.Style.Add("margin-left", "12px");
                                    rdbMaindisable.Style.Add(" margin-bottom", "10px");
                                    rdbMaindisable.Attributes.Add("disabled", "true");
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (rptCardDetailsAssociate != null)
            {
                if (rptCardDetailsAssociate.Items.Count > 0)
                {
                    for (int i = 0; i < rptCardDetailsAssociate.Items.Count; i++)
                    {
                        if (rptCardDetailsAssociate.Items[i].FindControl("ltrTypeofCard") != null)
                        {
                            Literal ltrCardType = ((Literal)rptCardDetailsAssociate.Items[i].FindControl("ltrTypeofCard"));
                            if (ltrCardType.Text.ToUpper() == "STANDARD" || ltrCardType.Text.ToUpper() == "ONLINE STANDARD")
                            {
                                if (rptCardDetailsAssociate.Items[0].FindControl("rdbReplaceAssociate") != null)
                                {
                                    RadioButton rdbAssodisable = ((RadioButton)rptCardDetailsAssociate.Items[0].FindControl("rdbReplaceAssociate"));
                                    rdbAssodisable.Style.Add("background", "transparent");
                                    rdbAssodisable.Style.Add("margin-left", "12px");
                                    rdbAssodisable.Style.Add(" margin-bottom", "10px");
                                    rdbAssodisable.Attributes.Add("disabled", "true");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void ShowMaxOrdersReached()
        {
            pInProcessMsg.Visible = false;
            pMaxOrdersReached.Visible = true;
            pMaxOrdersReached.Style.Clear();
            pMaxOrdersReached.Style.Value = "display: block";

        }
        #endregion

        #region Change Primary Card

        protected void imgbtnChangePrimary_Click(object sender, ImageClickEventArgs e)
        {

        }

        #endregion Change Primary Card

        #region Add Card

        protected void imgbtnAddCard_Click(object sender, ImageClickEventArgs e)
        {
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.imgbtnAddCard_Click()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.imgbtnAddCard_Click()");
            #endregion
            if (ValidatePage())
            {
                if (CalculateCheckDigit(txtCardNumber.Text.Trim().ToString()))
                {
                    try
                    {
                        string resultXml = string.Empty;
                        string errorXml = string.Empty;
                        lblSuccessMessage.Text = string.Empty;
                        long objectId = 0;
                        int rowCount = 0;
                        customerClient = new CustomerServiceClient();
                        htCustomer = new Hashtable();
                        if (dvMainAssCus.Visible == true)
                        {
                            if (RBMain.Checked == true)
                            {
                                htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID");
                            }
                            else
                            {
                                htCustomer["CustomerID"] = AssCusId;
                            }
                        }
                        else
                            htCustomer["CustomerID"] = Helper.GetTripleDESEncryptedCookieValue("CustomerID");
                        htCustomer["ClubcardID"] = txtCardNumber.Text.Trim();
                        // customerClient.GetCustomerDetails(errorXml,resultXml,rowCount,c
                        string addPrimaryCardXml = Helper.HashTableToXML(htCustomer, "Clubcard");
                        if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                        {

                            if (rdbReplacePrimaryYes.Checked)
                            {
                                serviceClient = new ClubcardServiceClient();

                                if (serviceClient.AddPrimaryCard(out objectId, out resultXml, out errorXml, addPrimaryCardXml, Convert.ToInt32(Helper.GetTripleDESEncryptedCookieValue("UserID"))))
                                {
                                    //code modified for localization.
                                    //lblSuccessMessage.Text = "Primary card have been added successfuly.";//SuccPrimaryCardAck
                                    lblSuccessMessage.Text = GetLocalResourceObject("SuccPrimaryCardAck").ToString();
                                    txtCardNumber.Text = "";
                                    rdbReplacePrimaryNo.Checked = true;
                                    ReloadClubCards();
                                }
                                else
                                {
                                    if (resultXml.Contains("Primary card unknown"))
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "Primary card unknown.";//ErrAckForCard
                                        lblSuccessMessage.Text = GetLocalResourceObject("ErrAckForCard").ToString();
                                        txtCardNumber.Text = "";
                                        rdbReplacePrimaryNo.Checked = true;

                                    }
                                    else if (resultXml.Contains("card account already exists"))
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "Card account already exists.";//AckForAccountExistance
                                        lblSuccessMessage.Text = GetLocalResourceObject("AckForAccountExistance").ToString();
                                        txtCardNumber.Text = "";
                                        rdbReplacePrimaryNo.Checked = true;

                                    }
                                    else if (resultXml.Contains("Invalid card range"))
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "Invalid card range.";//InvalidRangeCard
                                        lblSuccessMessage.Text = GetLocalResourceObject("InvalidRangeCard").ToString();
                                        txtCardNumber.Text = "";
                                        rdbReplacePrimaryNo.Checked = true;

                                    }
                                    else
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "Primary card have been added successfuly.";//SuccPrimaryCardAck
                                        lblSuccessMessage.Text = GetLocalResourceObject("SuccPrimaryCardAck").ToString();
                                        ReloadClubCards();
                                        txtCardNumber.Text = "";
                                        rdbReplacePrimaryNo.Checked = true;

                                    }
                                }
                            }
                            else
                            {

                                string addSupplementaryCardXml = Helper.HashTableToXML(htCustomer, "cardno");
                                if (customerClient.AddSupplementaryCard(out objectId, out errorXml, out resultXml, addSupplementaryCardXml))
                                {
                                    //code modified for localization.
                                    //lblSuccessMessage.Text = "Supplementary card have been added successfuly.";//SuccSuppCatdAck
                                    lblSuccessMessage.Text = GetLocalResourceObject("SuccSuppCatdAck").ToString();
                                    //MKTG00007296  01-08-2012  Kumar P
                                    txtCardNumber.Text = "";
                                    ReloadClubCards();
                                }
                                else
                                {
                                    if (resultXml.Contains("No Primary clubcard exists"))
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "No Primary clubcard exists.";//AckForNoPrimaryCard
                                        lblSuccessMessage.Text = GetLocalResourceObject("AckForNoPrimaryCard").ToString();
                                        //MKTG00007296  01-08-2012  Kumar P
                                        txtCardNumber.Text = "";
                                    }
                                    if (resultXml.Contains("card account already exists"))
                                    {
                                        //code modified for localization.
                                        //lblSuccessMessage.Text = "Card account already exists.";//DupCardAck
                                        lblSuccessMessage.Text = GetLocalResourceObject("DupCardAck").ToString();
                                        //MKTG00007296  01-08-2012  Kumar P
                                        txtCardNumber.Text = "";
                                    }
                                    if (resultXml.Contains("No clubcardtype present"))
                                    {
                                        //code modified for localization.
                                        // lblSuccessMessage.Text = "No clubcardtype present.";//AckForClubCardPresentance
                                        lblSuccessMessage.Text = GetLocalResourceObject("AckForClubCardPresentance").ToString();
                                        //MKTG00007296  07-08-2012  Kumar P
                                        txtCardNumber.Text = "";
                                    }
                                }
                            }
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }

                        #region Trace End
                        NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.imgbtnAddCard_Click()");
                        NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.imgbtnAddCard_Click()");
                        #endregion
                    }


                    catch (Exception exp)
                    {
                        #region Trace Error
                        NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.imgbtnAddCard_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                        NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.imgbtnAddCard_Click() - Error Message :" + exp.ToString());
                        NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.imgbtnAddCard_Click()");
                        NGCTrace.NGCTrace.ExeptionHandling(exp);
                        #endregion Trace Error
                        throw exp;
                    }
                    finally
                    {
                        if (customerClient != null)
                        {
                            if (customerClient.State == CommunicationState.Faulted)
                            {
                                customerClient.Abort();
                            }
                            else if (customerClient.State != CommunicationState.Closed)
                            {
                                customerClient.Close();
                            }
                        }
                    }
                }
                else
                {
                    //errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                    errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                    spanCardNumber = "";
                    txtCardNumber.CssClass = "errorFld";
                }
            }
        }      

        /// <summary>
        /// Method to reload clubcards.
        /// </summary>
        private void ReloadClubCards()
        {
            mainCount = 0;
            assCount = 0;
            string resultXml, errorXml;
            serviceClient = new ClubcardServiceClient();

            long customerID = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID").ToString());

            if (serviceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
            {
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                DataSet dsHHCustomers = new DataSet();
                dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                if (dsHHCustomers.Tables.Count > 0)
                {
                    RenderCardsSectionByCustomers(dsHHCustomers);

                    orderConfirm.Attributes.Add("disabled", "true");
                    CheckNewOrderValid();
                }
            }
        }

        #endregion Add Card

        #region Check the Card Number Validity

        /// <summary>
        /// CalculateCheckDigit -- It is used to do check digit validation of a clubcard
        /// </summary>
        public bool CalculateCheckDigit(string cardNumber)
        {

            try
            {
                int cardNumberBound = cardNumber.Length - 1;
                // Card Number must contain at least 2 digits, including the check digit
                if (cardNumberBound < 1)
                {
                    return false;
                }
                int sum = 0;
                // ignore the last digit, as this is the check digit
                for (int i = 0; i < cardNumberBound; i++)
                {
                    int weight = 2 - (i % 2);
                    int digitTimesWeight = int.Parse(cardNumber[i].ToString()) * weight;
                    sum += (digitTimesWeight % 10) + (digitTimesWeight / 10);
                }
                int correctCheckDigit = 10 - (sum % 10);
                // If the check digit is 10, the check digit should be zero
                if (correctCheckDigit == 10)
                {
                    correctCheckDigit = 0;
                }
                int cardCheckDigit = int.Parse(cardNumber[cardNumberBound].ToString());
                return (correctCheckDigit == cardCheckDigit);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// To validate customer details
        /// </summary>
        /// <returns>boolean</returns>
        protected bool ValidatePage()
        {
            #region Trace Start
            NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.ValidatePage()");
            NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.ValidatePage()");
            #endregion
            try
            {
                //string regNumeric = @"^[0-9]*$";
                string regNumeric = hdnClubcardnumberreg.Value;

                bool bErrorFlag = true;
                txtCardNumber.CssClass = "";
                string cardNumber = txtCardNumber.Text.ToString().Trim();

                //Server side validations
                if (string.IsNullOrEmpty(cardNumber))
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
                            //code modified for localization.
                            //errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                            errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        //Card number should be more between 16  and 18 digits
                        else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && (txtCardNumber.Text.Trim().Length < 16 || txtCardNumber.Text.Trim().Length > 18))
                        {
                            //code modified for localization.
                            //errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                            errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                        //Card number should not be all zeros.
                        else if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()) && Convert.ToInt64(txtCardNumber.Text) == 0)
                        {
                            //code modified for localization.
                            // errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                            errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                            spanCardNumber = "";
                            txtCardNumber.CssClass = "errorFld";
                            bErrorFlag = false;
                        }
                    }
                    catch (FormatException)
                    {
                        //code modified for localization.
                        //errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                        errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                        spanCardNumber = "";
                        txtCardNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                    //Card Number
                    if (!Helper.IsRegexMatch(txtCardNumber.Text.Trim(), regNumeric, true, false))
                    {
                        //code modified for localization.
                        //errMsgCardNumber = "Please enter a valid Card Number";//ValidateCardNumField
                        errMsgCardNumber = GetLocalResourceObject("ValidateCardNumField").ToString();
                        spanCardNumber = "";
                        txtCardNumber.CssClass = "errorFld";
                        bErrorFlag = false;
                    }

                } return bErrorFlag;
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.ValidatePage() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.ValidatePage() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.ValidatePage()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
        }

        #region SetHouseHoldStatus
        /// <summary>
        /// To set household status on LHN
        /// </summary>
        /// <param name="pCustomerID">Primary customer ID</param>
        public void SetHouseHoldStatus()
        {
            string customerUserStatus = string.Empty;
            string customerMailStatus = string.Empty;
            string CustomerEmailStatus = string.Empty;
            string CustomerMobilePhoneStatus = string.Empty;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC CustomerCards.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC CustomerCards.SetHouseHoldStatus()");
                #endregion

                customerUserStatus = Helper.GetTripleDESEncryptedCookieValue("customerUserStatus");
                customerMailStatus = Helper.GetTripleDESEncryptedCookieValue("customerMailStatus");
                CustomerEmailStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerEmailStatus");
                CustomerMobilePhoneStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerMobilePhoneStatus");

                ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");

                HtmlControl spnBannedError = (HtmlControl)leftNav.FindControl("spnBannedError");
                HtmlControl spnLeftError = (HtmlControl)leftNav.FindControl("spnLeftError");
                HtmlControl spnDuplicateError = (HtmlControl)leftNav.FindControl("spnDuplicateError");
                HtmlControl spnAddressError = (HtmlControl)leftNav.FindControl("spnAddressError");
                HtmlControl spnEmailError = (HtmlControl)leftNav.FindControl("spnEmailError");
                HtmlControl spnMobileNoError = (HtmlControl)leftNav.FindControl("spnMobileNoError");
                if (customerUserStatus != "1" || customerMailStatus != "1")
                {
                    // for banned house hold
                    if (customerUserStatus == "2")
                    {
                        spnBannedError.Visible = true;
                    }
                    // for Left Scheme
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
                        //for address in error
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

                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("End: CSC CustomerCards.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC CustomerCards.SetHouseHoldStatus()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC CustomerCards.imgbtnAddCard_Click() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC CustomerCards.imgbtnAddCard_Click() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC CustomerCards.imgbtnAddCard_Click()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {

            }
        }
        #endregion

        protected void RBMain_CheckedChanged(object sender, EventArgs e)
        {
            lblSuccessMessage.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
        }

        protected void RBAssociative_CheckedChanged(object sender, EventArgs e)
        {
            lblSuccessMessage.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
        }


        #region GetConfigDetails
        /// <summary>
        /// Gets the config details to decide should wheather we enable/disable tdrdbReplaceMain
        /// </summary>
        /// <returns></returns>
        public bool GetConfigDetails()
        {

            string conditionXML = string.Empty;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            XmlDocument resulDoc = new XmlDocument();
            string resultxml = string.Empty;
            int rowCount = 0;
            conditionXML = "19,10";
            string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
            customerClient = new CustomerServiceClient();
            DataSet dsConfigDetails = new DataSet();
            bool bolVisbaleValue = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:CSC CustomerCards.GetConfigDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:CSC CustomerCards.GetConfigDetails");
                if (customerClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
                {
                    resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                    if (dsConfigDetails.Tables.Count > 0)
                    {
                        DataView dvConfigDetails = new DataView();
                        dvConfigDetails.Table = dsConfigDetails.Tables["ActiveDateRangeConfig"];
                        foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                        {
                            if (dr["ConfigurationType"].ToString().Trim() == "10" && dr["ConfigurationName"].ToString().Trim() == "ClubcardNumber")
                            {
                                hdnClubcardnumberreg.Value = dr["ConfigurationValue1"].ToString();
                            }
                        }
                        dvConfigDetails.RowFilter = "ConfigurationType = '19' and ConfigurationName='HideJoinFunctionality'";
                        if (dvConfigDetails.Count > 0)
                        {
                            bolVisbaleValue = true;
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("Start:CSC CustomerCards.GetConfigDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:CSC CustomerCards.GetConfigDetails");
                return bolVisbaleValue;
            }
            catch (Exception exception)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:CSC CustomerCards.GetConfigDetails - Error Message :" + exception.ToString());
                NGCTrace.NGCTrace.TraceError("Error:CSC CustomerCards.GetConfigDetails - Error Message :" + exception.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:CSC CustomerCards.GetConfigDetails");
                NGCTrace.NGCTrace.ExeptionHandling(exception);
                throw exception;
            }
            finally
            {
                if (this.customerClient != null)
                {
                    if (this.customerClient.State == CommunicationState.Faulted)
                    {
                        this.customerClient.Abort();
                    }
                    else if (this.customerClient.State != CommunicationState.Closed)
                    {
                        this.customerClient.Close();
                    }
                }
            }

        }
        #endregion

        #region Methods for Recading

        public Hashtable populateUUIDforCutomers(Hashtable ht)
        { 
         

            Int64 intPrimaryCust = Convert.ToInt64(ht["Primary"]);
            Int64 intAssociateCust = Convert.ToInt64(ht["Associate"]);
            string strUUID=string.Empty;
            RestClient restClientObj = new RestClient();

            try
            {

                if (intPrimaryCust > 0)
                {
                    //DataSet ds = getCustomerDetails(intPrimaryCust.ToString());
                    //strUUID = getUUIDFromCustomerDataset(ds);
                    string strMethodName = string.Format("{0}{1}{2}", HttpContext.GetGlobalResourceObject("ApiMethodNames", "getCustomerUUID").ToString(), "/", intPrimaryCust.ToString());
                    CustomerUUID customerUUIDobj = restClientObj.getCustomerUUID(strMethodName);
                    if (customerUUIDobj != null && customerUUIDobj.UUID != null)
                    {
                        ht["Primary"] = customerUUIDobj.UUID.ToString();
                        hdnPrimaryCustomerUUID.Value = customerUUIDobj.UUID.ToString(); ;
                    }
                    else
                    {
                        //--put the error msg here to show that customer don't have the UUID in the system
                    }



                }
                if (intAssociateCust > 0)
                {
                    //DataSet ds1 = getCustomerDetails(intAssociateCust.ToString());
                    //strUUID = getUUIDFromCustomerDataset(ds1);
                    string strMethodName = string.Format("{0}{1}{2}", HttpContext.GetGlobalResourceObject("ApiMethodNames", "getCustomerUUID").ToString(), "/", intAssociateCust.ToString());
                    CustomerUUID customerUUIDobj = restClientObj.getCustomerUUID(strMethodName);
                    if (customerUUIDobj != null && customerUUIDobj.UUID != null)
                    {
                        ht["Associate"] = customerUUIDobj.UUID.ToString();
                        hdnAssociateCustomerUUID.Value = customerUUIDobj.UUID.ToString(); ;
                    }
                    else
                    {
                        //--put the error msg here to show that customer don't have the UUID in the system
                    }

                }

                return ht;
            }
            catch
            {
                throw;
            }
        }
      
        public DataSet getCustomerDetails(string customerId)
        {
            Hashtable searchData = new Hashtable();
            string conditionXML = string.Empty;
            int maxRows, rowCount;
            string errorXml = string.Empty;
            string resultXml = string.Empty; 
            CustomerServiceClient customerObj=null;
            XmlDocument resulDoc = null;
            DataSet dsCustomer = null;

            try
            {
                searchData["CustomerID"] = customerId;
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
                }
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
            }

            return dsCustomer;


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
        public NewCardEntitled isCustomerEligibleForCardReplacement(string strUUID)
        {
            RestClient obj = new RestClient();
            try
            {

                string strMethod = HttpContext.GetGlobalResourceObject("ApiMethodNames", "entitledToNewCard").ToString();
                string strAuthorizationHeader = string.Format("{0}{1}{2}", HttpContext.GetGlobalResourceObject("ApiMethodNames", "bearer").ToString(), " ", getAccessToken());
                strUUID = string.Format("{0}{1}", HttpContext.GetGlobalResourceObject("ApiMethodNames", "uuidPrefix").ToString(), strUUID);
                strMethod = strMethod.Replace("{0}", strUUID);
                NewCardEntitled card = new NewCardEntitled();
                card = obj.getCardEntitlementStatus(strMethod, strAuthorizationHeader);
                return card;
            }
            catch
            {
                throw;
            }

        }
        public CreateCardsetResponse createCardReplacementRequest(string strUUID, string stateOfOldCaerd)
        {
            RestClient obj = new RestClient();
            CreateCardsetRequest req = new CreateCardsetRequest();
            CreateCardsetResponse res = new CreateCardsetResponse(); 
            Guid g = Guid.NewGuid();
            try
            {
                string strUUIDPrefix = HttpContext.GetGlobalResourceObject("ApiMethodNames", "uuidPrefix").ToString().Replace("%3A", ":");
                strUUID = string.Format("{0}{1}", strUUIDPrefix, strUUID);
                string strAuthorizationHeader = string.Format("{0}{1}{2}", HttpContext.GetGlobalResourceObject("ApiMethodNames", "bearer").ToString(), " ", getAccessToken());
                req.cardSetRequestId = g.ToString();
                req.customerUuid = strUUID;
                req.stateOfOldCard = stateOfOldCaerd;
                res = obj.CreateCardset(HttpContext.GetGlobalResourceObject("ApiMethodNames", "createCardSet").ToString(), strAuthorizationHeader, req);
                return res;
            }
            catch
            {
                throw;
            }
        }        

        #endregion

        protected void rptCardDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void rptCardDetailsAssociate_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }


    }
}